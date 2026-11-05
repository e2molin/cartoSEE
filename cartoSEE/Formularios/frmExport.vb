Public Class frmExport

    Dim cancelar As Boolean
    Dim procTilde As New Destildator

    Dim CallBack As New System.Drawing.Image.GetThumbnailImageAbort(AddressOf MycallBack)
    Function MycallBack() As Boolean
        Return False
    End Function



    Private Sub procesarProvinciaGEODOCAT4CdD(ByVal cProv As Integer, ByVal folderOUT As String)

        Dim listaTerris As New ArrayList
        Dim tiposdocu2CDD As New ArrayList
        Dim proce As procGenerateHTMLReport

        Me.Cursor = Cursors.WaitCursor

        If CheckedListBox1.CheckedItems.Count = 0 Then
            ModalExclamation("No se han seleccionado los tipos de documento a procesar")
            Me.Cursor = Cursors.Default
            Exit Sub
        End If

        'Seleccionamos los documentos de la provincia
        ToolStripStatusLabel1.Text = "Accediendo a la información de " & DameProvinciaByINE(cProv)

        Dim seqTiposDoc As String = ""
        tiposdocu2CDD.Clear()
        For Each elem As itemData In CheckedListBox1.CheckedItems
            tiposdocu2CDD.Add(CType(elem.Valor, Integer))
            If seqTiposDoc = "" Then seqTiposDoc = CType(elem.Valor, Integer) : Continue For
            seqTiposDoc = seqTiposDoc & "," & CType(elem.Valor, Integer)
        Next


        Dim resultDocumentos As New docCartoSEEquery
        resultDocumentos.flag_ActualizarInfoGeom = True
        resultDocumentos.flag_CargarFicherosGEO = True
        resultDocumentos.flag_CargarFicherosGEOSoloETRS89 = True
        'resultDocumentos.getDocsSIDDAE_ByFiltroSellado("archivo.provincia_id=" & cProv & " AND tipodoc_id IN (" & seqTiposDoc & ")")
        resultDocumentos.getDocsSIDDAE_ByFiltroSellado("tipodoc_id=8 and cdd_fecha is null")

        If resultDocumentos.resultados.Count = 0 Then
            ModalInfo("No se han encontrado documentos para el CdD")
            Exit Sub
        End If

        If ModalQuestion($"Se van a procesar {resultDocumentos.resultados.Count} documentos. ¿Continuar?") <> DialogResult.Yes Then
            Me.Cursor = Cursors.Default
            Exit Sub
        End If

        ToolStripStatusLabel1.Text = "Procesando " & DameProvinciaByINE(cProv) & ". Copiando ficheros para el CdD"
        folderOUT = IIf(folderOUT.EndsWith("\"), folderOUT, folderOUT & "\")
        Try
            If Not IO.Directory.Exists(folderOUT) Then
                IO.Directory.CreateDirectory(folderOUT)
            End If
        Catch ex As Exception
            ModalExclamation(ex.Message)
        End Try
        proce = New procGenerateHTMLReport
        proce.CopyFiles2DirectoryToCDD(resultDocumentos.resultados, folderOUT, tiposdocu2CDD, RadioButton2.Checked)
        proce = Nothing
        ToolStripProgressBar1.Visible = False
        ToolStripStatusLabel1.Text = "Documentos procesados: " & resultDocumentos.resultados.Count
        resultDocumentos.resultados.Clear()
        resultDocumentos = Nothing
        Me.Cursor = Cursors.Default

    End Sub

    Private Sub frmExport_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        procTilde = Nothing
    End Sub

    Private Sub frmExport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim contadorProv As Integer = 0
        cboProvincias.Items.Add(New itemData("(Procesar todas)", 0))

        For Each Provincia As DataRow In ListaProvincias.Select
            contadorProv += 1
            cboProvincias.Items.Add(New itemData(Provincia.ItemArray(1).ToString, contadorProv))
        Next

        For Each tipoDocu As docCartoSEETipoDocu In tiposDocSIDCARTO
            CheckedListBox1.Items.Add(New itemData(tipoDocu.NombreTipo, tipoDocu.idTipodoc), False)
        Next

        txtDirTarget.Text = unidadActualizCDD

        txtDirTarget.Text = "\\10.67.32.101\cartoteca_temp\archivo\"

        DateTimePicker3.Value = CDate(Now.AddDays(-180))
        DateTimePicker4.Value = CDate(Now)
        cancelar = False
        ToolStripStatusLabel1.Text = "Seleccione provincia"
        TextBox1.Text = "iddocsiddae in (Select docsiddae_id from bdsidschema.docsiddaelog where fecha_update between'2019-02-11' and '2019-11-15') or (fecha_alta between '2019-02-11' and '2019-11-15') Or subtipo='1925'"

        CheckBox1.Visible = usuarioMyApp.permisos.usuarioISTARI
        chkHTML.Visible = usuarioMyApp.permisos.usuarioISTARI
        chkThumb.Visible = usuarioMyApp.permisos.usuarioISTARI
        chkMuniIndex.Visible = usuarioMyApp.permisos.usuarioISTARI
        chkCreateNEM.Visible = usuarioMyApp.permisos.usuarioISTARI
        chkLinkDocGeo.Visible = usuarioMyApp.permisos.usuarioISTARI
        chkCreateINDEX.Visible = usuarioMyApp.permisos.usuarioISTARI


        Dim infoLastCdD As String = ""

        ObtenerEscalar("SELECT 'Última extracción para el CdD:  ' || DATE(fechafilecdd::timestamp) || '. Ficheros: ' || count(*)
	                       FROM bdsidschema.archivodocmtn
                            WHERE subtipo='Itinerarios con brújula' 
                            GROUP BY fechafilecdd order by fechafilecdd limit 1", infoLastCdD)
        Label11.Text = infoLastCdD


    End Sub


    Private Sub btnSpecialProc_Click(sender As System.Object, e As System.EventArgs) Handles btnSpecialProc.Click




        Dim filtroSQL As String
        filtroSQL = TextBox1.Text
        If filtroSQL = "" Then
            MessageBox.Show("Escriba un filtro SQL", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        Application.DoEvents()
        procesarListaDocsCdD(txtDirTarget.Text, filtroSQL, "Actas y cuadernos")
        MessageBox.Show("Proceso terminado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)








    End Sub

    Private Sub btnProcess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        If cboProvincias.SelectedIndex = -1 Then
            MessageBox.Show("Selecciona una provincia", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If cboProvincias.Text = "(Procesar todas)" Then
            ToolStripProgressBar1.Minimum = 0
            ToolStripProgressBar1.Maximum = 50
            For ibucle = 1 To 50
                ToolStripProgressBar1.Value = ibucle
                Application.DoEvents()
                procesarListaDocsCdD(txtDirTarget.Text, "docsiddae.provincia=" & ibucle, ibucle)
            Next
            ModalInfo("Proceso terminado")
            Exit Sub
        End If

        If Not IO.Directory.Exists(txtDirTarget.Text.Trim) Then
            ModalExclamation("El directorio no existe")
            Exit Sub
        End If


        ToolStripProgressBar1.Minimum = 0
        ToolStripProgressBar1.Maximum = 2
        ToolStripProgressBar1.Value = 1
        procesarListaDocsCdD(txtDirTarget.Text, "docsiddae.provincia=" & CType(cboProvincias.SelectedItem, itemData).Valor, "Actas y cuadernos")

        ModalInfo("Proceso terminado")

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="folderOUT"></param>
    ''' <param name="filtroSQL"></param>
    ''' <param name="coleccion">Dos opciones: Actas y cuadernos / Cuadernos interiores / SIDCECA_I / SIDCECA_II / SIDCECA_I_y_II</param>
    ''' <param name="cProv"></param>
    Private Sub procesarListaDocsCdD(ByVal folderOUT As String, filtroSQL As String, coleccion As String, Optional cProv As Integer = 0)

        Dim listaTerris As New ArrayList
        Dim folderWork As String
        Dim proce As procGenerateHTMLReport
        Dim cadFechaAhora As String
        Dim listaSQLFinales As New ArrayList
        Dim cadSQLtmp As String
        Dim resultCuadernos As docCuadMTNQuery
        Dim resultCedulas As docSIDCECAQuery

        Me.Cursor = Cursors.WaitCursor
        Dim RutaLOG As String = txtDirTarget.Text & "\logger.log"
        Dim ficheroBaseCSV As String = txtDirTarget.Text & "\data.csv"
        If IO.File.Exists(RutaLOG) Then
            Try
                IO.File.Delete(RutaLOG)
            Catch ex As Exception
                ModalExclamation($"No se puede eliminar {RutaLOG}")
                Exit Sub
            End Try
        End If
        If IO.File.Exists(ficheroBaseCSV) Then
            Try
                IO.File.Delete(ficheroBaseCSV)
            Catch ex As Exception
                ModalExclamation($"No se puede eliminar {ficheroBaseCSV}")
                Exit Sub
            End Try
        End If

        'Seleccionamos los documentos de la provincia
        ToolStripStatusLabel1.Text = "Accediendo a la información" & IIf(cProv > 0, " de " & DameProvinciaByINE(cProv), "...")
        Application.DoEvents()


        If coleccion = "Cuadernos interiores" Then
            resultCuadernos = New docCuadMTNQuery
            resultCuadernos.getByFiltroSQL(filtroSQL)
            If resultCuadernos.resultados.Count = 0 Then
                ModalInfo("No se han encontrado datos")
                Me.Cursor = Cursors.Default
                Exit Sub
            Else
                If ModalQuestion($"La consulta devuelve {resultCuadernos.resultados.Count} resultados.¿Continuar?") = DialogResult.No Then
                    Me.Cursor = Cursors.Default
                    Exit Sub
                End If

            End If
        ElseIf coleccion = "SIDCECA_I" Then
            resultCedulas = New docSIDCECAQuery("parcelasdatos")
            resultCedulas.LimitResponse = 100
            resultCedulas.getByFiltroSQL(filtroSQL)
        ElseIf coleccion = "SIDCECA_II" Then
            resultCedulas = New docSIDCECAQuery("parcelasdata")
            resultCedulas.LimitResponse = 100
            resultCedulas.getByFiltroSQL(filtroSQL)
        ElseIf coleccion = "SIDCECA_I_y_II" Then
            ToolStripStatusLabel1.Text = "Accediendo a la información de SIDCECA I y II"
            Application.DoEvents()
            resultCedulas = New docSIDCECAQuery("bothDatasets")
            'resultCedulas.LimitResponse = 100
            resultCedulas.getByFiltroSQL(filtroSQL)
        End If

        If resultCedulas.resultados.Count = 0 Then
            Me.Cursor = Cursors.Default
            ModalExclamation("No hay datos para exportar")
            Exit Sub
        End If



        Dim provProc As Integer = 0
        Dim resDocParcial As New ArrayList

        'Aplicamos tareas

        ToolStripStatusLabel1.Text = "Procesando datos. Copiando ficheros para el CdD"
        proce = New procGenerateHTMLReport
        proce.overWriteFiles = chkOverWrite.Checked 'Para que no vuelva a copiar los PDFs
        proce.pathIncluding = False
        proce.taxonomyCodeIncluding = False
        If CheckBox1.Checked Then proce.groupPDFbyFolderProv = True
        If CheckBox2.Checked Then proce.groupPDFbyFolderINE = True
        'If ModalQuestion($"Se van a preparar para el CdD {resultCedulas.resultados.Count} documentos. ¿Continuar?") = DialogResult.No Then
        '    resultCedulas.resultados.Clear()
        '    resultCedulas = Nothing
        '    proce = Nothing
        '    Application.DoEvents()
        '    Me.Cursor = Cursors.Default
        '    ToolStripStatusLabel1.Text = "Proceso abortado"
        'End If
        If coleccion = "Cuadernos interiores" Then
            proce.CopyFiles2Directory(resultCuadernos.resultados, folderOUT, coleccion, chkCopyTest.Checked, cProv, False)
            resultCuadernos.resultados.Clear()
            resultCuadernos = Nothing
        ElseIf coleccion = "SIDCECA_I_y_II" Then
            proce.CopyFiles2Directory(resultCedulas.resultados, folderOUT, coleccion, chkCopyTest.Checked, cProv, False)
            resultCedulas.resultados.Clear()
            resultCedulas = Nothing
        End If
        proce = Nothing

        Application.DoEvents()

        ToolStripStatusLabel1.Text = "Eliminando líneas duplicadas en ficheros TXT de salida"
        Dim pathDeleteDuplicates As String = ""
        pathDeleteDuplicates = folderOUT & "\_ficherospdf2codigosINE.txt"
        deleteDuplicateLinesFromFile(pathDeleteDuplicates, True)
        pathDeleteDuplicates = folderOUT & "\_ficherospdf.txt"
        deleteDuplicateLinesFromFile(pathDeleteDuplicates, True)
        pathDeleteDuplicates = folderOUT & "\_fichaDocHTML2codigosINE.txt"
        deleteDuplicateLinesFromFile(pathDeleteDuplicates, True)
        pathDeleteDuplicates = folderOUT & "\_fichaDocHTML.txt"
        deleteDuplicateLinesFromFile(pathDeleteDuplicates, True)
        Me.Cursor = Cursors.Default
        ToolStripStatusLabel1.Text = "Proceso terminado"



    End Sub



    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim DirRepo As String
        FolderBrowserDialog1.Description = "Selecciona el directorio para realizar el volcado"
        FolderBrowserDialog1.ShowNewFolderButton = True
        FolderBrowserDialog1.ShowDialog()
        Application.DoEvents()
        DirRepo = FolderBrowserDialog1.SelectedPath.ToString
        txtDirTarget.Text = DirRepo

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click, Button4.Click

        Dim filtroSQL As String
        filtroSQL = TextBox1.Text

        Dim cadfechaDesde As String
        Dim cadfechaHasta As String
        cadfechaDesde = DateTimePicker3.Value.Year & "-" &
                        String.Format("{0:00}", CInt(DateTimePicker3.Value.Month.ToString)) & "-" &
                        String.Format("{0:00}", CInt(DateTimePicker3.Value.Day.ToString))
        cadfechaHasta = DateTimePicker4.Value.AddDays(1).Year & "-" &
                        String.Format("{0:00}", CInt(DateTimePicker4.Value.AddDays(1).Month.ToString)) & "-" &
                        String.Format("{0:00}", CInt(DateTimePicker4.Value.AddDays(1).Day.ToString))

        'filtroSQL = "iddocsiddae in (Select docsiddae_id from bdsidschema.docsiddaelog where fecha_update between'" & cadfechaDesde & "' and '" & cadfechaHasta & "') " &"or (fecha_alta between '" & cadfechaDesde & "' and '" & cadfechaHasta & "')"

        If sender.name = "Button4" Then
            filtroSQL = "(subtipo='Itinerarios con brújula' and create_at between '" & cadfechaDesde & "' and '" & cadfechaHasta & "') OR
                         (subtipo='Itinerarios con brújula' and  idarchivodocmtn in (Select archivodocmtn_id from bdsidschema.archivodocmtnlog where fecha_update between '" & cadfechaDesde & "' and '" & cadfechaHasta & "'))"
        End If
        If sender.name = "Button6" Then
            filtroSQL = "subtipo='Itinerarios con brújula' and fechafilecdd is null"
        End If


        'filtroSQL = $"subtipo='Itinerarios con brújula' and create_at between '{cadfechaDesde}' and '{cadfechaHasta}'"
        'filtroSQL = $"subtipo='Itinerarios con brújula'"
        'filtroSQL = $"sellado=504231"

        If filtroSQL = "" Then
            ModalExclamation("Escriba un filtro SQL")
            Exit Sub
        End If
        registrarDatabaseLog("Lanzado proceso de exportación CdD", $"filtroSQL={filtroSQL}")
        Application.DoEvents()
        procesarListaDocsCdD(txtDirTarget.Text, filtroSQL, "Cuadernos interiores")
        registrarDatabaseLog("Terminado proceso de exportación CdD")
        ModalInfo("Proceso de extracción para el CdD terminado")

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click


        Dim filtroSQL As String
        filtroSQL = TextBox1.Text
        If filtroSQL = "" Then
            ModalExclamation("Escriba un filtro SQL")
            Exit Sub
        End If
        Application.DoEvents()

        filtroSQL = "idparceladato>0"
        procesarListaDocsCdD(txtDirTarget.Text, filtroSQL, "Cédulas JGE")
        ModalInfo("Proceso terminado")




    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click, Button7.Click

        'Dim filtroSQL As String
        'filtroSQL = TextBox1.Text

        'Dim cadfechaDesde As String
        'Dim cadfechaHasta As String


        'If sender.name = "Button4" Then
        '    filtroSQL = "(subtipo='Itinerarios con brújula' and create_at between '" & cadfechaDesde & "' and '" & cadfechaHasta & "') OR
        '                 (subtipo='Itinerarios con brújula' and  idarchivodocmtn in (Select archivodocmtn_id from bdsidschema.archivodocmtnlog where fecha_update between '" & cadfechaDesde & "' and '" & cadfechaHasta & "'))"
        'End If
        'If sender.name = "Button6" Then
        '    filtroSQL = "subtipo='Itinerarios con brújula' and fechafilecdd is null"
        'End If


        'If filtroSQL = "" Then
        '    ModalExclamation("Escriba un filtro SQL")
        '    Exit Sub
        'End If
        'registrarDatabaseLog("Lanzado proceso de exportación CdD", $"filtroSQL={filtroSQL}")
        Application.DoEvents()


        If sender.name = "Button5" Then
            procesarListaDocsCdD(txtDirTarget.Text, "idparceladato>0$listaprop.territorio_id=5305", "SIDCECA_I_y_II")
            'registrarDatabaseLog("Terminado proceso de exportación CdD")
            ModalInfo("Proceso de extracción para el CdD terminado")
        ElseIf sender.name = "Button7" Then
            procesarListaDocsCdD(txtDirTarget.Text, "$", "SIDCECA_I_y_II")
            ModalInfo("Proceso de extracción para el CdD terminado")
        End If




    End Sub

    Private Sub btnProcessProvin_Click(sender As Object, e As EventArgs) Handles btnProcessProvin.Click

        If cboProvincias.SelectedIndex = -1 Then
            ModalExclamation("Selecciona una provincia")
            Exit Sub
        End If
        If Not System.IO.Directory.Exists(txtDirTarget.Text.Trim) Then
            ModalExclamation("El directorio no existe")
            Exit Sub
        End If

        If cboProvincias.Text = "(Procesar todas)" Then
            ToolStripProgressBar1.Minimum = 0
            ToolStripProgressBar1.Maximum = 50
            For ibucle = 1 To 50
                ToolStripProgressBar1.Value = ibucle
                Application.DoEvents()
                procesarProvinciaGEODOCAT4CdD(ibucle, txtDirTarget.Text)
            Next
        Else
            ToolStripProgressBar1.Minimum = 0
            ToolStripProgressBar1.Maximum = 2
            ToolStripProgressBar1.Value = 1
            procesarProvinciaGEODOCAT4CdD(CType(cboProvincias.SelectedItem, itemData).Valor, txtDirTarget.Text)
        End If

        ModalInfo("Proceso terminado")

    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs)

    End Sub
End Class