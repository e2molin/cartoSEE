Public Class frmExportCdD

    Dim exportDocs As New docCartoSEEquery
    Dim cancelar As Boolean
    Dim procTilde As New Destildator

    Dim CallBack As New System.Drawing.Image.GetThumbnailImageAbort(AddressOf MycallBack)
    Function MycallBack() As Boolean
        Return False
    End Function




    Private Sub frmExport_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        procTilde = Nothing
    End Sub

    Private Sub frmExport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim contadorProv As Integer = 0
        cboProvincias.Items.Add(New itemData("(Procesar todas)", 0))
        For Each Provincia As DataRow In ListaProvincias.Select
            contadorProv = contadorProv + 1
            cboProvincias.Items.Add(New itemData(Provincia.ItemArray(1).ToString, contadorProv))
        Next

        txtDirTarget.Text = My.Computer.FileSystem.SpecialDirectories.Temp
        txtDirTarget.Text = "S:\CdDJE"
        cancelar = False
        ToolStripStatusLabel1.Text = "Seleccione provincia"

        For Each tipoDocu As docCartoSEETipoDocu In tiposDocSIDCARTO
            CheckedListBox1.Items.Add(New itemData(tipoDocu.NombreTipo, tipoDocu.idTipodoc), False)
        Next


    End Sub


    Private Sub btnProcess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcess.Click

        Application.DoEvents()
        'REPETIR LEON OOOOJJJJOOOO
        'procesarProvincia4CdD(47, txtDirTarget.Text)
        'procesarProvincia4CdD(48, txtDirTarget.Text)
        'procesarProvincia4CdD(49, txtDirTarget.Text)
        'procesarProvincia4CdD(50, txtDirTarget.Text)
        'MessageBox.Show("Proceso terminado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
        'Exit Sub




        If cboProvincias.SelectedIndex = -1 Then
            MessageBox.Show("Selecciona una provincia", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        If Not System.IO.Directory.Exists(txtDirTarget.Text.Trim) Then
            MessageBox.Show("El directorio no existe", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Try
            If System.IO.File.Exists(txtDirTarget.Text.Trim & "\logCopy.log") Then System.IO.File.Delete(txtDirTarget.Text.Trim & "\logCopy.log")
            If System.IO.File.Exists(txtDirTarget.Text.Trim & "\alias.txt") Then System.IO.File.Delete(txtDirTarget.Text.Trim & "\alias.txt")
            If System.IO.File.Exists(txtDirTarget.Text.Trim & "\municipios.txt") Then System.IO.File.Delete(txtDirTarget.Text.Trim & "\municipios.txt")
            If System.IO.File.Exists(txtDirTarget.Text.Trim & "\actualizCartoSEE.sql") Then System.IO.File.Delete(txtDirTarget.Text.Trim & "\actualizCartoSEE.sql")
            Using archivoAlias As System.IO.StreamWriter = New System.IO.StreamWriter(txtDirTarget.Text.Trim & "\alias.txt", False, System.Text.Encoding.UTF8)
                Using archivoMunicipios As System.IO.StreamWriter = New System.IO.StreamWriter(txtDirTarget.Text.Trim & "\municipios.txt", False, System.Text.Encoding.UTF8)
                    archivoMunicipios.WriteLine("idProductor;Nombre Fichero JPG;Códigos INE de municipio asociado")
                    archivoAlias.WriteLine("idProductor;Fichero;Temática;Alias;Fecha;TipoFichero")
                End Using
            End Using
        Catch ex As Exception
            GenerarLOG("e2m: " & ex.Message)
            Exit Sub
        End Try

        If cboProvincias.Text = "(Procesar todas)" Then
            ToolStripProgressBar1.Minimum = 0
            ToolStripProgressBar1.Maximum = 50
            For ibucle = 1 To 50
                ToolStripProgressBar1.Value = ibucle
                Application.DoEvents()
                procesarProvincia4CdD(ibucle, txtDirTarget.Text)
            Next
        Else
            ToolStripProgressBar1.Minimum = 0
            ToolStripProgressBar1.Maximum = 2
            ToolStripProgressBar1.Value = 1
            procesarProvincia4CdD(CType(cboProvincias.SelectedItem, itemData).Valor, txtDirTarget.Text)
        End If
        ToolStripStatusLabel1.Text = "Proceso terminado"
        MessageBox.Show("Proceso terminado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub


    Private Sub procesarProvincia4CdD(ByVal cProv As Integer, ByVal folderOUT As String)

        Dim listaTerris As New ArrayList
        Dim tiposdocu2CDD As New ArrayList
        Dim folderWork As String
        Dim proce As procGenerateHTMLReport

        Me.Cursor = Cursors.WaitCursor
        Dim RutaLOG As String = txtDirTarget.Text & "\" & String.Format("{0:00}", cProv) & "\loggerCdD.log"
        Dim ficheroBaseCSV As String = txtDirTarget.Text & "\" & String.Format("{0:00}", cProv) & "\data.csv"
        If System.IO.File.Exists(RutaLOG) Then
            Try
                System.IO.File.Delete(RutaLOG)
            Catch ex As Exception
                MessageBox.Show("No se puede eliminar " & ficheroBaseCSV, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End Try
        End If
        If System.IO.File.Exists(ficheroBaseCSV) Then
            Try
                System.IO.File.Delete(ficheroBaseCSV)
            Catch ex As Exception
                MessageBox.Show("No se puede eliminar " & ficheroBaseCSV, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End Try
        End If

        'Seleccionamos los documentos de la provincia
        ToolStripStatusLabel1.Text = "Accediendo a la información de " & DameProvinciaByINE(cProv)
        Application.DoEvents()

        Dim seqTiposDoc As String = ""
        tiposdocu2CDD.Clear()
        For Each elem As itemData In CheckedListBox1.CheckedItems
            Application.DoEvents()
            tiposdocu2CDD.Add(CType(elem.Valor, Integer))
            If seqTiposDoc = "" Then seqTiposDoc = CType(elem.Valor, Integer) : Continue For
            seqTiposDoc = seqTiposDoc & "," & CType(elem.Valor, Integer)
        Next


        Dim resultDocumentos As New docCartoSEEquery
        resultDocumentos.flag_ActualizarInfoGeom = True
        resultDocumentos.flag_CargarFicherosGEO = True
        resultDocumentos.getDocsSIDDAE_ByFiltroSellado("archivo.provincia_id=" & cProv & " AND tipodoc_id IN (" & seqTiposDoc & ")")

        If resultDocumentos.resultados.Count = 0 Then Exit Sub

        'Aplicamos tareas
        If chkHTML.Checked Then
            ToolStripStatusLabel1.Text = "Procesando " & DameProvinciaByINE(cProv) & ". Generando informes HTML"
            folderWork = folderOUT & "\" & String.Format("{0:00}", cProv) & "\html\"
            Try
                If Not System.IO.Directory.Exists(folderWork) Then
                    System.IO.Directory.CreateDirectory(folderWork)
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
            proce = New procGenerateHTMLReport
            proce.generarHTMLreport(resultDocumentos.resultados, My.Application.Info.DirectoryPath & "\resources\docsidcarto-template.html", folderWork, True, tiposdocu2CDD)
            proce = Nothing
        End If

        If chkCreateINDEX.Checked Then
            ToolStripStatusLabel1.Text = "Procesando " & DameProvinciaByINE(cProv) & ". Generando índice"
            folderWork = folderOUT & "\" & String.Format("{0:00}", cProv) & "\html\"
            Try
                If Not System.IO.Directory.Exists(folderWork) Then
                    System.IO.Directory.CreateDirectory(folderWork)
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try

            proce = New procGenerateHTMLReport
            proce.generarHTMLIndexProvinReport(resultDocumentos.resultados, My.Application.Info.DirectoryPath & "\resources\index-template.html", folderWork, tiposdocu2CDD)
            proce = Nothing
        End If

        If chkMuniIndex.Checked Then
            ToolStripStatusLabel1.Text = "Procesando " & DameProvinciaByINE(cProv) & ". Generando índice para cada municipio histórico"
            folderWork = folderOUT & "\" & String.Format("{0:00}", cProv) & "\html\"
            Try
                If Not System.IO.Directory.Exists(folderWork) Then
                    System.IO.Directory.CreateDirectory(folderWork)
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
            proce = New procGenerateHTMLReport
            proce.generarMuniHistoIndex(cProv, folderWork, tiposdocu2CDD, chkHTML.Checked)
            proce.Dispose()
            proce = Nothing

            proce = New procGenerateHTMLReport
            proce.generarMuniActualIndex(cProv, folderWork, tiposdocu2CDD, chkHTML.Checked)
            proce.Dispose()
            proce = Nothing
        End If

        If chkCreateNEM.Checked Then
            ToolStripStatusLabel1.Text = "Procesando " & DameProvinciaByINE(cProv) & ". Generando metadatos ISO19115"
            folderWork = folderOUT & "\" & String.Format("{0:00}", cProv) & "\xml\"
            Try
                If Not System.IO.Directory.Exists(folderWork) Then
                    System.IO.Directory.CreateDirectory(folderWork)
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
            proce = New procGenerateHTMLReport
            proce.generarXMLISO19115(resultDocumentos.resultados, folderWork, tiposdocu2CDD)
            proce = Nothing
        End If

        If chkCopiaFicherosImagen.Checked Then
            Dim folderDocs4CdD As String
            Dim folderMetadata4CdD As String
            ToolStripStatusLabel1.Text = "Procesando " & DameProvinciaByINE(cProv) & ". Copiando ficheros para el CdD"
            folderMetadata4CdD = folderOUT
            folderDocs4CdD = folderOUT & "\documentos\" & String.Format("{0:00}", cProv)
            Try
                If Not System.IO.Directory.Exists(folderDocs4CdD) Then
                    System.IO.Directory.CreateDirectory(folderDocs4CdD)
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
            proce = New procGenerateHTMLReport
            proce.exportCdDGenerico(resultDocumentos.resultados, folderMetadata4CdD, folderDocs4CdD, tiposdocu2CDD, cboxOverwrite.Checked)
            proce = Nothing
        End If

        If chkThumb.Checked Then
            Dim folderCopyThumb As String
            ToolStripStatusLabel1.Text = "Procesando " & DameProvinciaByINE(cProv) & ". Copiando miniaturas para el CdD"
            folderCopyThumb = folderOUT & "\" & String.Format("{0:00}", cProv) & "\thumb\"
            Try
                If Not System.IO.Directory.Exists(folderCopyThumb) Then
                    System.IO.Directory.CreateDirectory(folderCopyThumb)
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
            proce = New procGenerateHTMLReport
            proce.copyFilesThumb2Directory(resultDocumentos.resultados, folderCopyThumb, tiposdocu2CDD)
            proce = Nothing
        End If

        If chkCopiaFicherosZIP.Checked Then
            Dim folderCopyZIP As String
            ToolStripStatusLabel1.Text = "Procesando " & DameProvinciaByINE(cProv) & ". Copiando ficheros para el CdD"
            folderCopyZIP = folderOUT & "\" & String.Format("{0:00}", cProv) & "\zip\"
            Try
                If Not System.IO.Directory.Exists(folderCopyZIP) Then
                    System.IO.Directory.CreateDirectory(folderCopyZIP)
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
            proce = New procGenerateHTMLReport
            proce.copyFiles2DirectoryZIPPED(resultDocumentos.resultados, folderCopyZIP, tiposdocu2CDD)
            proce = Nothing
        End If

        resultDocumentos.resultados.Clear()
        resultDocumentos = Nothing
        ToolStripProgressBar1.Value = ToolStripProgressBar1.Maximum
        Me.Cursor = Cursors.Default

    End Sub


    Private Sub procesarProvincia(ByVal cProv As Integer)

        Dim listaTerris As New ArrayList
        Dim okProc As Boolean
        Dim contador As Integer

        Dim RutaLOG As String = txtDirTarget.Text & "\" & String.Format("{0:00}", cProv) & "\logger.log"
        Dim ficheroBaseCSV As String = txtDirTarget.Text & "\" & String.Format("{0:00}", cProv) & "\data.csv"
        If System.IO.File.Exists(RutaLOG) Then
            Try
                System.IO.File.Delete(RutaLOG)
            Catch ex As Exception
                MessageBox.Show("No se puede eliminar " & ficheroBaseCSV, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End Try
        End If
        If System.IO.File.Exists(ficheroBaseCSV) Then
            Try
                System.IO.File.Delete(ficheroBaseCSV)
            Catch ex As Exception
                MessageBox.Show("No se puede eliminar " & ficheroBaseCSV, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End Try
        End If

        'Cargamos la lista de territorios involucrados
        Me.Cursor = Cursors.WaitCursor
        listaTerris = dameListaMunicipios(cProv)
        btnProcess.Visible = False
        ToolStripStatusLabel1.Text = "Municipios: " & listaTerris.Count
        ToolStripProgressBar1.Visible = True
        ToolStripProgressBar1.Minimum = 0
        ToolStripProgressBar1.Maximum = listaTerris.Count
        ToolStripStatusLabel1.Text = "Procesando " & DameProvinciaByINE(cProv) & ". Municipios: " & listaTerris.Count

        'Iniciamos el proceso
        contador = 0
        'For Each terri As TerritorioBSID In listaTerris
        '    'Application.DoEvents()
        '    'If terri.Nombre.IndexOf("Harana") = -1 Then Continue For
        '    'Application.DoEvents()
        '    If cancelar = True Then
        '        If MessageBox.Show("¿Desea cancelar el proceso?", AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
        '            Exit For
        '        End If
        '        cancelar = False
        '    End If
        '    'Cargamos la información del municipio
        '    exportDocs.resultados.Clear()
        '    exportDocs.getDocsSIDDAE_ByIdTerritorio(terri.indice)

        '    'DameDocumentacionSIDDAE_ByIdTerritorio(terri.Indice, listaDocs)
        '    'Abrimos un fichero para volcar información
        '    okProc = generarFichMunicipio(terri.nombre, terri.municipioINE)
        '    If okProc = False Then
        '        If MessageBox.Show("No se pudo exportar " & terri.nombre & ".¿Continuar?", AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.No Then Exit For
        '    End If
        '    'okProc = copiarLoteFicheros(terri.Nombre, terri.INE, cProv)
        '    'If okProc = False Then
        '    '    If MessageBox.Show("No se pudo copiar los documentos de " & terri.Nombre & ".¿Continuar?", AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.No Then Exit For
        '    'End If
        '    contador = contador + 1
        '    ToolStripProgressBar1.Value = contador
        '    Application.DoEvents()
        'Next


        'Una vez terminados con los datos de cada municipio, copiamos y generamos las miniaturas
        okProc = copiarLoteFicheros(cProv)
        If okProc = False Then
            MessageBox.Show("No se pudo copiar los documentos", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

        btnProcess.Visible = True
        ToolStripProgressBar1.Visible = False
        ToolStripStatusLabel1.Text = "Municipios: " & listaTerris.Count
        MessageBox.Show("Proceso terminado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Me.Cursor = Cursors.Default



    End Sub

    Function copiarLoteFicheros(ByVal terriNombre As String, ByVal terriINE As Integer, ByVal cProv As Integer) As Boolean

        Dim CarpetaPDF As String
        Dim CarpetaThumb As String
        Dim codINE As String
        Dim RutaLOG As String
        Dim RutaOrigen As String
        Dim RutaDestino As String
        Dim nombreNuevo As String
        Dim ficheroBaseCSV As String

        'Aplicamos reglas de nomenclatura al fichero de salida
        codINE = String.Format("{0:00000}", terriINE)
        CarpetaPDF = txtDirTarget.Text & "\" & String.Format("{0:00}", cProv) & "\PDF\"
        CarpetaThumb = txtDirTarget.Text & "\" & String.Format("{0:00}", cProv) & "\Thumbs\"
        RutaLOG = txtDirTarget.Text & "\" & String.Format("{0:00}", cProv) & "\logger.log"
        ficheroBaseCSV = txtDirTarget.Text & "\" & String.Format("{0:00}", cProv) & "\data.csv"

        Using archivoLog As System.IO.StreamWriter = New System.IO.StreamWriter(RutaLOG, True, System.Text.Encoding.Default)
            Using archivoData As System.IO.StreamWriter = New System.IO.StreamWriter(ficheroBaseCSV, True, System.Text.Encoding.Default)
                'Creamos las carpetas de destino por si n o existen
                If Not System.IO.Directory.Exists(CarpetaPDF) Then
                    Try
                        System.IO.Directory.CreateDirectory(CarpetaPDF)
                    Catch ex As Exception
                        MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return False
                    End Try
                End If
                If Not System.IO.Directory.Exists(CarpetaThumb) Then
                    Try
                        System.IO.Directory.CreateDirectory(CarpetaThumb)
                    Catch ex As Exception
                        MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return False
                    End Try
                End If
                Application.DoEvents()
                For Each doc As docCartoSEE In exportDocs.resultados
                    'RutaOrigen = doc.ficheroPDF
                    ''Al nombre nuevo le aplicamos convención de reglas
                    ''Dim munisSec() As String = doc.MunicipiosSecundario.Split(",")
                    ''Dim muniSec As String = ""
                    ''If munisSec.Length >= 1 Then muniSec = munisSec(0)
                    'Dim firmas() As String = doc.firmas.Split(",")
                    'Dim firma As String = ""
                    'If firmas.Length >= 1 Then firma = firmas(0)
                    'If chkCreateNEM.Checked = True Then
                    '    nombreNuevo = SacarFileDeRuta(doc.ficheroPDF)
                    'Else
                    '    nombreNuevo = doc.Sellado & "_" & doc.Tipo.nombreTipo & "_" & doc.getTerritorioPrincipal & "_" & doc.getTerritorioSecundario & "_" & firma & doc.Tipo.extension
                    '    nombreNuevo = procTilde.destildar(nombreNuevo).Replace(" ", "_")
                    'End If
                    'RutaDestino = CarpetaPDF & nombreNuevo
                    'If Not System.IO.File.Exists(RutaDestino) Then
                    '    Try
                    '        If chkCreateINDEX.Checked = False Then
                    '            System.IO.File.Copy(RutaOrigen, RutaDestino)
                    '            'Generamos la miniatura si nos la piden
                    '            If chkThumb.Checked = True Then
                    '                generaThumbPortada(RutaDestino, CarpetaThumb & "\" & SacarFileDeRuta(RutaDestino).Replace(".pdf", ".jpg"))
                    '            End If
                    '        Else
                    '            'Generamos la miniatura si nos la piden
                    '            If chkThumb.Checked = True Then
                    '                generaThumbPortada(RutaOrigen, CarpetaThumb & "\" & SacarFileDeRuta(RutaDestino).Replace(".pdf", ".jpg"))
                    '            End If
                    '        End If
                    '        archivoLog.WriteLine("#COPIA#" & RutaOrigen & "#" & RutaDestino)

                    '        'Ahora generamos el fichero para la base de datos
                    '        Dim fraseCSV As String
                    '        Dim tmpMunicipios As String
                    '        tmpMunicipios = doc.getTerritorioPrincipal & "|" & doc.getTerritorioSecundario
                    '        Dim tmpMunis() As String = tmpMunicipios.Split("|")
                    '        For Each tmpMuni As String In tmpMunis
                    '            Dim fileCopy As New System.IO.FileInfo(RutaDestino)
                    '            If fileCopy.Exists Then
                    '                fraseCSV = doc.Sellado & ";" & _
                    '                            tmpMuni.Replace("(", ";").Replace(")", "").Trim & ";" & _
                    '                            fileCopy.FullName & ";" & _
                    '                            fileCopy.Length
                    '            Else
                    '                fraseCSV = doc.Sellado & ";" & _
                    '                            tmpMuni.Replace("(", ";").Replace(")", "").Trim & ";" & _
                    '                            fileCopy.FullName & ";0"
                    '            End If
                    '            archivoData.WriteLine(fraseCSV)
                    '            fileCopy = Nothing
                    '        Next
                    '    Catch ex As Exception
                    '        archivoLog.WriteLine("#ERROR COPIA#" & RutaOrigen & "#" & RutaDestino & "#" & ex.Message)
                    '        'MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    '        'Return False
                    '    End Try
                    'End If
                Next
                Return True
            End Using
        End Using


    End Function

    Function copiarLoteFicheros(ByVal cProv As Integer) As Boolean

        Dim CarpetaPDF As String
        Dim CarpetaThumb As String
        Dim contador As Integer
        Dim RutaLOG As String
        Dim RutaOrigen As String
        Dim RutaDestino As String
        Dim nombreNuevo As String
        Dim ficheroBaseCSV As String
        'Dim ListaDocsProv() As docSIDDAE

        'Aplicamos reglas de nomenclatura al fichero de salida
        'codINE = String.Format("{0:00000}", terriINE)
        CarpetaPDF = txtDirTarget.Text & "\" & String.Format("{0:00}", cProv) & "\PDF\"
        CarpetaThumb = txtDirTarget.Text & "\" & String.Format("{0:00}", cProv) & "\Thumbs\"
        RutaLOG = txtDirTarget.Text & "\" & String.Format("{0:00}", cProv) & "\logger.log"
        ficheroBaseCSV = txtDirTarget.Text & "\" & String.Format("{0:00}", cProv) & "\data.csv"


        'exportDocs.getDocsSIDDAE_ByProvincia(cProv)

        'DameDocumentacionSIDDAE_ByProvinciaExclusiva(cProv, ListaDocsProv)
        ToolStripStatusLabel1.Text = "Procesando " & DameProvinciaByINE(cProv) & ". Documentos: " & exportDocs.resultados.Count
        ToolStripProgressBar1.Maximum = exportDocs.resultados.Count
        ToolStripProgressBar1.Minimum = 0
        contador = -1


        Using archivoLog As System.IO.StreamWriter = New System.IO.StreamWriter(RutaLOG, True, System.Text.Encoding.Default)
            Using archivoData As System.IO.StreamWriter = New System.IO.StreamWriter(ficheroBaseCSV, True, System.Text.Encoding.Default)
                'Creamos las carpetas de destino por si n o existen
                If Not System.IO.Directory.Exists(CarpetaPDF) Then
                    Try
                        System.IO.Directory.CreateDirectory(CarpetaPDF)
                    Catch ex As Exception
                        MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return False
                    End Try
                End If
                If Not System.IO.Directory.Exists(CarpetaThumb) Then
                    Try
                        System.IO.Directory.CreateDirectory(CarpetaThumb)
                    Catch ex As Exception
                        MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return False
                    End Try
                End If
                Application.DoEvents()
                For Each doc As docCartoSEE In exportDocs.resultados
                    'Application.DoEvents()
                    'contador = contador + 1
                    'If doc.Tipo.nombreTipo = "Memoria" Then Continue For
                    'If doc.Tipo.nombreTipo = "Disco de datos" Then Continue For
                    'ToolStripProgressBar1.Value = contador
                    'RutaOrigen = doc.ficheroPDF
                    'Dim firmas() As String = doc.firmas.Split(",")
                    'Dim firma As String = ""
                    'If firmas.Length >= 1 Then firma = firmas(0)
                    'If chkCreateNEM.Checked = True Then
                    '    nombreNuevo = SacarFileDeRuta(doc.ficheroPDF)
                    'Else
                    '    nombreNuevo = doc.Sellado & "_" & doc.Tipo.nombreTipo & "_" & doc.getTerritorioPrincipal & "_" & doc.getTerritorioSecundario & "_" & firma & doc.Tipo.extension
                    '    nombreNuevo = procTilde.destildar(nombreNuevo).Replace(" ", "_")
                    'End If
                    'RutaDestino = CarpetaPDF & nombreNuevo
                    'If Not System.IO.File.Exists(RutaDestino) Then
                    '    Try
                    '        If chkCreateINDEX.Checked = False Then
                    '            System.IO.File.Copy(RutaOrigen, RutaDestino)
                    '            'Generamos la miniatura si nos la piden
                    '            If chkThumb.Checked = True Then
                    '                generaThumbPortada(RutaDestino, CarpetaThumb & "\" & SacarFileDeRuta(RutaDestino).Replace(".pdf", ".jpg"))
                    '            End If
                    '        Else
                    '            'Generamos la miniatura si nos la piden
                    '            If chkThumb.Checked = True Then
                    '                generaThumbPortada(RutaOrigen, CarpetaThumb & "\" & SacarFileDeRuta(RutaDestino).Replace(".pdf", ".jpg"))
                    '            End If
                    '        End If
                    '        archivoLog.WriteLine("#COPIA#" & RutaOrigen & "#" & RutaDestino)

                    '        'Ahora generamos el fichero para la base de datos
                    '        Dim fraseCSV As String
                    '        Dim tmpMunicipios As String = doc.getTerritorioPrincipal & "|" & doc.getTerritorioSecundario
                    '        Dim tmpMunis() As String = tmpMunicipios.Split("|")
                    '        For Each tmpMuni As String In tmpMunis
                    '            Dim fileCopy As New System.IO.FileInfo(RutaDestino)
                    '            If fileCopy.Exists Then
                    '                fraseCSV = doc.Sellado & ";" & _
                    '                            tmpMuni.Replace("(", ";").Replace(")", "").Trim & ";" & _
                    '                            fileCopy.FullName & ";" & _
                    '                            fileCopy.Length
                    '            Else
                    '                fraseCSV = doc.Sellado & ";" & _
                    '                            tmpMuni.Replace("(", ";").Replace(")", "").Trim & ";" & _
                    '                            fileCopy.FullName & ";0"
                    '            End If
                    '            archivoData.WriteLine(fraseCSV)
                    '            fileCopy = Nothing
                    '        Next
                    '    Catch ex As Exception
                    '        archivoLog.WriteLine("#ERROR COPIA#" & RutaOrigen & "#" & RutaDestino & "#" & ex.Message)
                    '    End Try
                    'End If
                Next

            End Using
        End Using
        Return True

    End Function

    Function generarFichMunicipio(ByVal terriNombre As String, ByVal terriINE As Integer) As Boolean

        Dim RutaCSV As String
        Dim CarpetaCSV As String
        Dim ArchivoCSV As String
        Dim codINE As String

        'Aplicamos reglas de nomenclatura al fichero de salida
        codINE = String.Format("{0:00000}", terriINE)

        ArchivoCSV = codINE & "-" & terriNombre & ".csv"
        ArchivoCSV = procTilde.destildar(ArchivoCSV).Replace(" ", "_")
        CarpetaCSV = txtDirTarget.Text & "\" & codINE.Substring(0, 2) & "\CSV\"
        RutaCSV = CarpetaCSV & ArchivoCSV

        If Not System.IO.Directory.Exists(CarpetaCSV) Then
            Try
                System.IO.Directory.CreateDirectory(CarpetaCSV)
            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End Try
        End If

        Using archivo As System.IO.StreamWriter = New System.IO.StreamWriter(RutaCSV, False, System.Text.Encoding.Default)
            ' variable para almacenar la línea actual del dataview  
            Dim linea As String = String.Empty
            Me.Cursor = Cursors.WaitCursor
            Application.DoEvents()
            'Primero Generamos las cabeceras
            linea = String.Empty
            linea = "Número de documento" & ";" & _
                    "Tipo" & ";" & _
                    "Firmas" & ";" & _
                    "Provincia" & ";" & _
                    "Deslinde Principal" & ";" & _
                    "Deslindes Secundarios" & ";" & _
                    "Anejos" & ";" & _
                    "Cambios de nombre" & ";" & _
                    "Anulado" & ";" & _
                    "Adicionales" & ";" & _
                    "Comentarios"
            With archivo
                .WriteLine(linea.ToString)
            End With
            For Each doc As docCartoSEE In exportDocs.resultados
                'If doc.Tipo.nombreTipo = "Memoria" Then Continue For
                'If doc.Tipo.nombreTipo = "Disco de datos" Then Continue For
                '' vaciar la línea  
                'linea = String.Empty
                'linea = doc.Sellado.ToString & ";" & _
                '        doc.Tipo.nombreTipo & ";" & _
                '        doc.firmas & ";" & _
                '        doc.provinciaNombre & ";" & _
                '        doc.getTerritorioPrincipal & ";" & _
                '        doc.getTerritorioSecundario & ";" & _
                '        doc.anejos.Replace(";", ":") & ";" & _
                '        doc.renombres.Replace(";", ":") & ";" & _
                '        IIf(doc.estaAnulado = True, "Sí", "No") & ";" & _
                '        doc.Adicionales & ";" & _
                '        doc.comentarios.Replace(";", ":")
                'With archivo
                '    .WriteLine(linea.ToString)
                'End With
            Next
        End Using
        Return True

    End Function


    Function dameListaMunicipios(ByVal codProv As Integer) As ArrayList

        'Dim rcdMunis As DataTable
        'Dim filas() As DataRow
        'Dim listaTerritorios As ArrayList
        'Dim terri As TerritorioBSID


        'rcdMunis = New DataTable
        'listaTerritorios = New ArrayList
        'If CargarRecordset("SELECT idterritorio,nombre,tipo,municipio from territorios where tipo='Municipio' and provincia=" & codProv, rcdMunis) Then
        '    filas = rcdMunis.Select()
        '    For Each fila As DataRow In filas
        '        terri = New TerritorioBSID
        '        terri.Indice = fila.Item("idterritorio")
        '        terri.Nombre = fila.Item("nombre")
        '        terri.municipioINE = fila.Item("municipio")
        '        terri.tipo = fila.Item("tipo")
        '        listaTerritorios.Add(terri)
        '        terri = Nothing
        '    Next
        '    Erase filas
        '    rcdMunis.Dispose()
        '    rcdMunis = Nothing
        'End If
        'Return listaTerritorios

    End Function




    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim DirRepo As String
        FolderBrowserDialog1.Description = "Selecciona el directorio para realizar el volcado"
        FolderBrowserDialog1.ShowNewFolderButton = False
        FolderBrowserDialog1.ShowDialog()
        Application.DoEvents()
        DirRepo = FolderBrowserDialog1.SelectedPath.ToString
        txtDirTarget.Text = DirRepo

    End Sub



End Class