Public Class procGenerateHTMLReport
    Inherits System.Windows.Forms.Form
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox

    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(procGenerateHTMLReport))
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.Label1 = New System.Windows.Forms.Label()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(350, 1)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(69, 69)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBar1.Location = New System.Drawing.Point(12, 96)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(396, 23)
        Me.ProgressBar1.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 68)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Label1"
        '
        'procGenerateHTMLReport
        '
        Me.ClientSize = New System.Drawing.Size(420, 131)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.PictureBox1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "procGenerateHTMLReport"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub runProcWait(fileBatch)

        Dim proceso As Process
        Dim si As New ProcessStartInfo

        Me.Visible = True

        Try
            si.FileName = fileBatch
            si.WindowStyle = ProcessWindowStyle.Hidden
            'si.Arguments=""
            proceso = New Process
            proceso = Process.Start(si)
            Do
                Application.DoEvents()
                'If _CancelarProceso Then
                '    proceso.Kill()
                'End If
            Loop While proceso.HasExited = False
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        proceso.Dispose()
        proceso = Nothing
        si = Nothing
        Me.Visible = False
    End Sub


    Public Function generarHTMLreport(ByVal lista As ArrayList, ByVal template As String, folderOUTHTML As String, generateIndexFile As Boolean, tiposdocu2CDD As ArrayList) As Boolean


        Dim reportHTML As WebCdDTemplate
        Dim indexProc As Integer
        Me.Text = "Generando informes HTML de los documentos SIDDAE"
        Label1.Text = "Generando informes HTML por cada documento"
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = lista.Count
        Me.Show()
        Using archivoData As System.IO.StreamWriter = New System.IO.StreamWriter(folderOUTHTML.Replace("\html\", "\") & "\_HTML2codigosINE.txt", False, System.Text.Encoding.Default)
            For Each documento As docCartoSEE In lista
                If tiposdocu2CDD.IndexOf(documento.tipoDocumento.idTipodoc) = -1 Then Continue For
                indexProc = indexProc + 1
                ProgressBar1.Value = indexProc
                Application.DoEvents()
                reportHTML = New WebCdDTemplate
                Try
                    reportHTML.templateHTML = template
                    reportHTML.folderHTML = folderOUTHTML
                    reportHTML.docuAT = documento
                    reportHTML.prepareTemplate()
                    reportHTML.generateHTMLreport()
                    reportHTML.saveHTMLreport()
                    Application.DoEvents()
                    For Each codActual In documento.listaCodMuniActual
                        archivoData.WriteLine(documento.nameFileHTML & ";" & codActual)
                    Next
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                    Return False
                Finally
                    reportHTML = Nothing
                End Try
            Next
            ProgressBar1.Value = lista.Count
        End Using

        If generateIndexFile Then generarHTMLIndexProvinReport(lista, template.Replace("docsidcarto-template", "index-template"), folderOUTHTML, tiposdocu2CDD)
        Me.Close()
        Return True
    End Function

    Public Function generarHTMLIndexProvinReport(ByVal lista As ArrayList, ByVal template As String, folderOUTHTML As String, tiposdocu2CDD As ArrayList) As Boolean

        Dim indexProc As Integer
        Dim srTemplate As System.IO.StreamReader
        Dim swIndex As System.IO.StreamWriter
        Dim templateContent As String = ""
        Dim contenido As String = ""
        Dim provinciaNombre As String = ""
        Dim provinciaID As String = 0

        Me.Text = "Generando informe índice documentos SIDDAE"
        Label1.Text = "Generando indice HTML"
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = lista.Count
        Me.Show()


        For Each documento As docCartoSEE In lista
            If tiposdocu2CDD.IndexOf(documento.tipoDocumento.idTipodoc) = -1 Then Continue For
            indexProc = indexProc + 1
            If documento.Sellado = "480003" Then
                Application.DoEvents()
            End If
            ProgressBar1.Value = indexProc
            Application.DoEvents()
            If contenido = "" Then
                contenido = "<tr class=""odd gradeX"">" &
                                    "<td>" & documento.tipoDocumento.NombreTipo & "</td>" &
                                    "<td>" & documento.Sellado & "</td>" &
                                    "<td>" & documento.municipiosHistoLiteralHTML & "</td>" &
                                    "<td>" & documento.fechaPrincipal & "</td>" &
                                    "<td>" &
                                    "<a href=""" & documento.nameFileHTML & """ title=""Información del documento""><span class=""glyphicon glyphicon glyphicon-info-sign""></span></a> " &
                                    "<a href=""../zip/" & documento.nameFileHTML.Replace(".html", ".zip") & """ title=""Descarga del documento""><span class=""glyphicon glyphicon glyphicon-link""></span></a> " &
                                    "<a href=""../xml/" & documento.nameFileNEMXML & """ title=""Metadatos ISO19115 del documento""><span class=""glyphicon glyphicon glyphicon-globe""></span></a>" &
                                    "</td></tr>"
            Else
                contenido = contenido & "<tr class=""odd gradeX"">" &
                                    "<td>" & documento.tipoDocumento.NombreTipo & "</td>" &
                                    "<td>" & documento.Sellado & "</td>" &
                                    "<td>" & documento.municipiosHistoLiteralHTML & "</td>" &
                                    "<td>" & documento.fechaPrincipal & "</td>" &
                                    "<td>" &
                                    "<a href=""" & documento.nameFileHTML & """ title=""Información del documento""><span class=""glyphicon glyphicon glyphicon-info-sign""></span></a> " &
                                    "<a href=""../zip/" & documento.nameFileHTML.Replace(".html", ".zip") & """ title=""Descarga del documento""><span class=""glyphicon glyphicon glyphicon-link""></span></a> " &
                                    "<a href=""../xml/" & documento.nameFileNEMXML & """ title=""Metadatos ISO19115 del documento""><span class=""glyphicon glyphicon glyphicon-globe""></span></a>" &
                                    "</td></tr>"
            End If
            provinciaNombre = documento.Provincias
        Next

        srTemplate = New System.IO.StreamReader(template)
        templateContent = srTemplate.ReadToEnd
        srTemplate.Close()
        srTemplate.Dispose()
        srTemplate = Nothing
        templateContent = templateContent.Replace("_#CONTENIDOTABLA#_", contenido)
        templateContent = templateContent.Replace("_#NOMBREPROVINCIA#_", provinciaNombre)

        Try
            swIndex = New System.IO.StreamWriter(folderOUTHTML & "\index.html")
            swIndex.Write(templateContent)
            swIndex.Close()
            swIndex.Dispose()
            swIndex = Nothing
        Catch ex As Exception
            MessageBox.Show("Error:" & ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try


        ProgressBar1.Value = lista.Count
        Me.Close()
        Return True
    End Function

    Public Function generarMuniHistoIndex(codProv As Integer, folderWork As String, tiposdocu2CDD As ArrayList, Optional appendMode As Boolean = False) As Boolean

        Dim filas() As DataRow
        Dim procTilde As New Destildator
        Dim indexProc As Integer

        filas = ListaMunicipiosHisto.Select("provincia_id=" & codProv)

        Me.Text = "Generando informe índice de municipios históricos"
        Label1.Text = "Generando informe índice de municipios históricos de " & DameProvinciaByINE(codProv)
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = filas.Count
        Me.Show()
        Dim resultDocumentosMunicipal As docCartoSEEquery
        Dim nombreFicheroCDD As String

        Using archivoData As System.IO.StreamWriter = New System.IO.StreamWriter(folderWork.Replace("\html\", "\") & "\_HTML2codigosINE.txt", appendMode, System.Text.Encoding.Default)
            For Each dR As DataRow In filas
                indexProc = indexProc + 1
                ProgressBar1.Value = indexProc
                Application.DoEvents()
                nombreFicheroCDD = procTilde.destildar(dR("nombre").ToString)
                nombreFicheroCDD = nombreFicheroCDD.Replace(" ", "_")
                Try
                    resultDocumentosMunicipal = New docCartoSEEquery
                    resultDocumentosMunicipal.getDocsSIDDAE_ByFiltroSellado("idarchivo in (Select archivo_id from archivo2munihisto where munihisto_id=" & dR("idmunihisto").ToString & ")")
                    If generarHTMLIndexMunicipalHistoReport(resultDocumentosMunicipal.resultados,
                                                        My.Application.Info.DirectoryPath & "\resources\index-muni-template.html", folderWork, tiposdocu2CDD,
                                                        String.Format("{0:0000000}", dR("cod_munihisto")), dR("nombre").ToString, nombreFicheroCDD) Then
                        Application.DoEvents()
                        archivoData.WriteLine(String.Format("{0:0000000}", dR("cod_munihisto")) & "_Resumen_documentos_Archivo-" & nombreFicheroCDD & ".html" & ";" & String.Format("{0:00000}", dR("inecortoActual")))
                    End If
                    resultDocumentosMunicipal.resultados.Clear()
                    resultDocumentosMunicipal = Nothing
                Catch ex As Exception
                    GenerarLOG(ex.Message)
                End Try
            Next
        End Using
        Erase filas
        procTilde = Nothing
        Me.Close()
    End Function

    Private Function generarHTMLIndexMunicipalHistoReport(ByVal lista As ArrayList, ByVal template As String, folderOUTHTML As String, tiposdocu2CDD As ArrayList, codMuni As String, nombreMunicipioCompleto As String, nombreMuni4CDD As String) As Boolean

        Dim indexProc As Integer
        Dim srTemplate As System.IO.StreamReader
        Dim swIndex As System.IO.StreamWriter
        Dim templateContent As String = ""
        Dim contenido As String = ""
        Dim provinciaNombre As String = ""
        Dim provinciaID As String = 0

        For Each documento As docCartoSEE In lista
            If tiposdocu2CDD.IndexOf(documento.tipoDocumento.idTipodoc) = -1 Then Continue For
            If contenido = "" Then
                contenido = "<tr class=""odd gradeX"">" &
                                   "<td>" & documento.tipoDocumento.NombreTipo & "</td>" &
                                   "<td>" & documento.Sellado & "</td>" &
                                   "<td>" & documento.municipiosHistoLiteral & "</td>" &
                                   "<td>" & documento.fechaPrincipal & "</td>" &
                                    "<td>" &
                                    "<a href=""" & documento.nameFileHTML & """ title=""Información del documento""><span class=""glyphicon glyphicon glyphicon-info-sign""></span></a> " &
                                    "<a href=""../zip/" & documento.nameFileECWJPG & """ title=""Descarga del documento""><span class=""glyphicon glyphicon glyphicon-link""></span></a> " &
                                    "<a href=""../xml/" & documento.nameFileNEMXML & """ title=""Metadatos ISO19115 del documento""><span class=""glyphicon glyphicon glyphicon-globe""></span></a>" &
                                    "</td></tr>"
            Else
                contenido = contenido & "<tr class=""odd gradeX"">" &
                                     "<td>" & documento.tipoDocumento.NombreTipo & "</td>" &
                                     "<td>" & documento.Sellado & "</td>" &
                                     "<td>" & documento.municipiosHistoLiteral & "</td>" &
                                     "<td>" & documento.fechaPrincipal & "</td>" &
                                     "<td>" &
                                     "<a href=""" & documento.nameFileHTML & """ title=""Información del documento""><span class=""glyphicon glyphicon glyphicon-info-sign""></span></a> " &
                                     "<a href=""../zip/" & documento.nameFileECWJPG & """ title=""Descarga del documento""><span class=""glyphicon glyphicon glyphicon-link""></span></a> " &
                                     "<a href=""../xml/" & documento.nameFileNEMXML & """ title=""Metadatos ISO19115 del documento""><span class=""glyphicon glyphicon glyphicon-globe""></span></a>" &
                                     "</td></tr>"

            End If
            provinciaNombre = documento.Provincias
            provinciaID = documento.ProvinciaRepo
        Next

        srTemplate = New System.IO.StreamReader(template)
        templateContent = srTemplate.ReadToEnd
        srTemplate.Close()
        srTemplate.Dispose()
        srTemplate = Nothing
        templateContent = templateContent.Replace("_#CONTENIDOTABLA#_", contenido)
        templateContent = templateContent.Replace("_#NOMBREMUNICIPIO#_", nombreMunicipioCompleto)

        Try
            If System.IO.File.Exists(folderOUTHTML & "\" & codMuni & "_Resumen_documentos_Archivo-" & nombreMuni4CDD & ".html") Then
                System.IO.File.Delete(folderOUTHTML & "\" & codMuni & "_Resumen_documentos_Archivo-" & nombreMuni4CDD & ".html")
            End If
            swIndex = New System.IO.StreamWriter(folderOUTHTML & "\" & codMuni & "_Resumen_documentos_Archivo-" & nombreMuni4CDD & ".html")
            swIndex.Write(templateContent)
            swIndex.Close()
            swIndex.Dispose()
            swIndex = Nothing
        Catch ex As Exception
            MessageBox.Show("Error:" & ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try



        Return True
    End Function

    Public Function generarMuniActualIndex(codProv As Integer, folderWork As String, tiposdocu2CDD As ArrayList, Optional appendMode As Boolean = False) As Boolean

        Dim filas() As DataRow
        Dim procTilde As New Destildator
        Dim indexProc As Integer
        filas = ListaMunicipiosActual.Select("provincia_id=" & codProv)
        Me.Text = "Generando informe índice de municipios actuales"
        Label1.Text = "Generando informe índice de municipios actuales de " & DameProvinciaByINE(codProv)
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = filas.Count
        Me.Show()

        Dim resultDocumentosMunicipal As docCartoSEEquery
        Dim nombreFicheroCDD As String

        Using archivoData As System.IO.StreamWriter = New System.IO.StreamWriter(folderWork.Replace("\html\", "\") & "\_HTML2codigosINE.txt", appendMode, System.Text.Encoding.Default)
            For Each dR As DataRow In filas
                indexProc = indexProc + 1
                ProgressBar1.Value = indexProc
                Application.DoEvents()
                nombreFicheroCDD = procTilde.destildar(dR("nombre").ToString)
                nombreFicheroCDD = nombreFicheroCDD.Replace(" ", "_")
                Try
                    resultDocumentosMunicipal = New docCartoSEEquery
                    resultDocumentosMunicipal.getDocsSIDDAE_ByFiltroSellado("idarchivo in (Select archivo_id from archivo2munihisto inner join munihisto on munihisto.idmunihisto=archivo2munihisto.munihisto_id " &
                                                                        "where munihisto.entidad_id=" & dR("idmunihisto").ToString & ")")
                    If generarHTMLIndexMunicipalActualReport(resultDocumentosMunicipal.resultados,
                                                        My.Application.Info.DirectoryPath & "\resources\index-muni-template.html", folderWork, tiposdocu2CDD,
                                                        String.Format("{0:00000}", dR("inecortoactual")), dR("nombre").ToString, nombreFicheroCDD) Then
                        Application.DoEvents()
                        archivoData.WriteLine(String.Format("{0:00000}", dR("inecortoactual")) & "_Resumen_documentos_Archivo-" & nombreFicheroCDD & ".html" & ";" & String.Format("{0:00000}", dR("inecortoActual")))
                    End If
                    resultDocumentosMunicipal.resultados.Clear()
                    resultDocumentosMunicipal = Nothing
                Catch ex As Exception
                    GenerarLOG(ex.Message)
                End Try
            Next
        End Using
        Erase filas
        procTilde = Nothing
        Me.Close()

    End Function


    Private Function generarHTMLIndexMunicipalActualReport(ByVal lista As ArrayList, ByVal template As String, folderOUTHTML As String, tiposdocu2CDD As ArrayList, codMuni As String, nombreMunicipioCompleto As String, nombreMuni4CDD As String) As Boolean

        Dim srTemplate As System.IO.StreamReader
        Dim swIndex As System.IO.StreamWriter
        Dim templateContent As String = ""
        Dim contenido As String = ""
        Dim provinciaNombre As String = ""
        Dim provinciaID As String = 0

        For Each documento As docCartoSEE In lista
            If tiposdocu2CDD.IndexOf(documento.tipoDocumento.idTipodoc) = -1 Then Continue For
            If contenido = "" Then
                contenido = "<tr class=""odd gradeX"">" &
                                   "<td>" & documento.tipoDocumento.NombreTipo & "</td>" &
                                   "<td>" & documento.Sellado & "</td>" &
                                   "<td>" & documento.municipiosHistoLiteral & "</td>" &
                                   "<td>" & documento.fechaPrincipal & "</td>" &
                                    "<td>" &
                                    "<a href=""" & documento.nameFileHTML & """ title=""Información del documento""><span class=""glyphicon glyphicon glyphicon-info-sign""></span></a> " &
                                     "<a href=""../zip/" & documento.nameFileECWJPG & """ title=""Descarga del documento""><span class=""glyphicon glyphicon glyphicon-link""></span></a> " &
                                     "<a href=""../xml/" & documento.nameFileNEMXML & """ title=""Metadatos ISO19115 del documento""><span class=""glyphicon glyphicon glyphicon-globe""></span></a>" &
                                     "</td></tr>"
            Else
                contenido = contenido & "<tr class=""odd gradeX"">" &
                                     "<td>" & documento.tipoDocumento.NombreTipo & "</td>" &
                                     "<td>" & documento.Sellado & "</td>" &
                                     "<td>" & documento.municipiosHistoLiteral & "</td>" &
                                     "<td>" & documento.fechaPrincipal & "</td>" &
                                    "<td>" &
                                    "<a href=""" & documento.nameFileHTML & """ title=""Información del documento""><span class=""glyphicon glyphicon glyphicon-info-sign""></span></a> " &
                                     "<a href=""../zip/" & documento.nameFileECWJPG & """ title=""Descarga del documento""><span class=""glyphicon glyphicon glyphicon-link""></span></a> " &
                                     "<a href=""../xml/" & documento.nameFileNEMXML & """ title=""Metadatos ISO19115 del documento""><span class=""glyphicon glyphicon glyphicon-globe""></span></a>" &
                                     "</td></tr>"
            End If
            provinciaNombre = documento.Provincias
            provinciaID = documento.ProvinciaRepo
        Next

        srTemplate = New System.IO.StreamReader(template)
        templateContent = srTemplate.ReadToEnd
        srTemplate.Close()
        srTemplate.Dispose()
        srTemplate = Nothing
        templateContent = templateContent.Replace("_#CONTENIDOTABLA#_", contenido)
        templateContent = templateContent.Replace("_#NOMBREMUNICIPIO#_", nombreMunicipioCompleto)

        Try
            If System.IO.File.Exists(folderOUTHTML & "\" & codMuni & "_Resumen_documentos_Archivo-" & nombreMuni4CDD & ".html") Then
                System.IO.File.Delete(folderOUTHTML & "\" & codMuni & "_Resumen_documentos_Archivo-" & nombreMuni4CDD & ".html")
            End If
            swIndex = New System.IO.StreamWriter(folderOUTHTML & "\" & codMuni & "_Resumen_documentos_Archivo-" & nombreMuni4CDD & ".html")
            swIndex.Write(templateContent)
            swIndex.Close()
            swIndex.Dispose()
            swIndex = Nothing
        Catch ex As Exception
            MessageBox.Show("Error:" & ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
        Return True
    End Function

    Public Function generarMiniaturasFromDocs(ByVal lista As ArrayList, folderOUTThumb As String) As Boolean

        Dim indexProc As Integer
        Me.Text = "Generando miniaturas de los documentos SIDDAE"
        Label1.Text = "Generando miniaturas de los documentos HTML"
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = lista.Count
        Me.Show()
        For Each documento As docCartoSEE In lista
            'If documento.getTipoSubTipo <> "Cuaderno de Campo" And documento.getTipoSubTipo <> "Acta de Deslinde" And documento.getTipoSubTipo <> "Reseñas y Coordenadas" Then Continue For
            'indexProc = indexProc + 1
            'ProgressBar1.Value = indexProc
            'Application.DoEvents()
            'generaThumbPortada(documento.ficheroPDF, folderOUTThumb & documento.nameThumbnail)
        Next
        ProgressBar1.Value = lista.Count

        Me.Close()
        Return True

    End Function

    Public Function generarXMLISO19115(ByVal lista As ArrayList, folderOUTXML As String, tiposdocu2CDD As ArrayList) As Boolean


        Dim docXML As MetadataISO19115
        Dim indexProc As Integer
        Me.Text = "Generando metadatos XML ISO19115 de los documentos SIDDAE"
        Label1.Text = "Generando metadato XML ISO19115 HTML por cada documento"
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = lista.Count
        Me.Show()

        'Borramos todos los XML de la carpeta
        Label1.Text = "Eliminando metadatos presentes en el directorio"
        Try
            For Each foundFile As String In My.Computer.FileSystem.GetFiles(folderOUTXML, Microsoft.VisualBasic.FileIO.SearchOption.SearchAllSubDirectories, "*.xml")
                Application.DoEvents()
                My.Computer.FileSystem.DeleteFile(foundFile, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.DeletePermanently)
            Next
        Catch ex As Exception
            MessageBox.Show("No se pudieron eliminar los ficheros presentes en la carpeta.", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try




        Label1.Text = "Generando metadato XML esquema ISO 19115 por cada documento"
        Using archivoLog As System.IO.StreamWriter = New System.IO.StreamWriter(folderOUTXML & "\_logCopy.log", False, System.Text.Encoding.Default)
            For Each documento As docCartoSEE In lista
                If tiposdocu2CDD.IndexOf(documento.tipoDocumento.idTipodoc) = -1 Then Continue For
                indexProc = indexProc + 1
                ProgressBar1.Value = indexProc
                Application.DoEvents()
                docXML = New MetadataISO19115
                Try
                    docXML.templateXML = documento.rutaPlantillaMetadato
                    If docXML.templateXML = "" Then
                        archivoLog.WriteLine("No se definión una plantilla para " & documento.tipoDocumento.NombreTipo & " para el documento nº" & documento.Sellado)
                        Continue For
                    End If
                    docXML.folderXML = folderOUTXML
                    docXML.docuAT = documento
                    docXML.prepareTemplateMetadato()
                    docXML.fillMetadataTemplate()
                    docXML.saveMetadataFile()
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                    Return False
                Finally
                    docXML = Nothing
                End Try
            Next
        End Using


        ProgressBar1.Value = lista.Count

        Me.Close()
        Return True
    End Function


    Function exportCdDGenerico(ByVal lista As ArrayList, folderMetadata4CdD As String, folderOUTfilesCopy As String, tiposdocu2CDD As ArrayList, copiarSiExisteEnDestino As Boolean) As Boolean


        Dim indexProc As Integer
        Dim pathOrigen As String
        Dim pathDestino As String

        Me.Text = "Copia de los ficheros JPG para el Centro de Descargas"
        Label1.Text = "Copiando los ficheros JPG para el Centro de Descargas"
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = lista.Count
        Me.Show()

        If Not System.IO.Directory.Exists(folderOUTfilesCopy) Then
            MessageBox.Show("La ruta destino de los ficheros no existe", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Me.Close()
            Return False
        End If

        Using archivoLog As System.IO.StreamWriter = New System.IO.StreamWriter(folderMetadata4CdD & "\logCopy.log", True, System.Text.Encoding.UTF8)
            Using archivoAlias As System.IO.StreamWriter = New System.IO.StreamWriter(folderMetadata4CdD & "\alias.txt", True, System.Text.Encoding.UTF8)
                Using archivoMunicipios As System.IO.StreamWriter = New System.IO.StreamWriter(folderMetadata4CdD & "\municipios.txt", True, System.Text.Encoding.UTF8)
                    Using sentenciaSQL As System.IO.StreamWriter = New System.IO.StreamWriter(folderMetadata4CdD & "\actualizCartoSEE.sql", True, System.Text.Encoding.UTF8)
                        'archivoMunicipios.WriteLine("idProductor;Nombre Fichero JPG;Códigos INE de municipio asociado")
                        'archivoAlias.WriteLine("idProductor;Fichero;Temática;Alias;Escala;Autor;Fecha;TipoFichero")
                        For Each documento As docCartoSEE In lista
                            Application.DoEvents()
                            If tiposdocu2CDD.IndexOf(documento.tipoDocumento.idTipodoc) = -1 Then Continue For
                            indexProc = indexProc + 1
                            ProgressBar1.Value = indexProc
                            'Primero copiamos el JPG
                            pathOrigen = documento.rutaFicheroBajaRes
                            pathDestino = folderOUTfilesCopy & "\" & documento.nameFile4CDD
                            Try
                                If System.IO.File.Exists(pathDestino) = True Then
                                    If copiarSiExisteEnDestino Then
                                        System.IO.File.Copy(pathOrigen, pathDestino, True)
                                        archivoLog.WriteLine("*COPIA CORRECTA*" & pathOrigen & "#" & pathDestino)
                                    Else
                                        archivoLog.WriteLine("*COPIA CORRECTA. YA EXISTE*" & pathOrigen & "#" & pathDestino)
                                    End If
                                Else
                                    System.IO.File.Copy(pathOrigen, pathDestino, True)
                                    archivoLog.WriteLine("*COPIA CORRECTA*" & pathOrigen & "#" & pathDestino)
                                End If
                                archivoAlias.WriteLine(documento.getIdProductor4CdD & ";" &
                                                   documento.nameFile4CDD & ";" &
                                                   documento.tipoDocumento.tematicaCdD & ";" &
                                                   documento.getCdDAlias & ";" &
                                                   IIf(documento.Escala = 0, "Sin escala", documento.Escala.ToString) & ";" &
                                                   IIf(documento.autorDocumento = "", "Desconocido", documento.autorDocumento.ToString) & ";" &
                                                   documento.yearFechaPrincipal & ";" & "JPG")


                                sentenciaSQL.WriteLine("UPDATE bdsidschema.archivo SET " &
                                                       "cdd_nomfich = E'" & documento.nameFile4CDD.Replace("'", "\'") & "'," &
                                                       "cdd_titulo = E'" & documento.getCdDAlias.Replace("'", "\'") & ";" & documento.yearFechaPrincipal & "' " &
                                                       "WHERE sellado=" & documento.getIdProductor4CdD & ";")

                                'Por cada INE metemos una línea
                                For Each muni As String In documento.listaCodMuniActual
                                    Application.DoEvents()
                                    'Convertimos el codigo INE a INE completo según BDLL
                                    If muni.Length = 5 Then
                                        muni = "34" & DameAutonomiaByINEProvincia(muni.Substring(0, 2)) & muni.Substring(0, 2) & muni
                                    Else
                                        GenerarLOG("No peude convertirse " & muni & " a código INE largo")
                                    End If
                                    archivoMunicipios.WriteLine(documento.Sellado & ";" & documento.nameFile4CDD & ";" & muni)
                                Next
                            Catch ex As Exception
                                archivoLog.WriteLine("#COPIA ERRONEA#" & pathOrigen & "#" & pathDestino & "#" & ex.Message)
                            End Try
                        Next
                    End Using
                End Using
            End Using
        End Using

        ProgressBar1.Value = lista.Count
        Me.Close()
        Return True


    End Function


    Function copyFilesJPG2Directory(ByVal lista As ArrayList, folderOUTfilesCopy As String, tiposdocu2CDD As ArrayList) As Boolean


        Dim indexProc As Integer
        Dim pathOrigen As String
        Dim pathDestino As String

        Me.Text = "Copia de los ficheros JPG para el Centro de Descargas"
        Label1.Text = "Copiando los ficheros JPG para el Centro de Descargas"
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = lista.Count
        Me.Show()

        If Not System.IO.Directory.Exists(folderOUTfilesCopy) Then
            MessageBox.Show("La ruta destino de los ficheros no existe", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Me.Close()
            Return False
        End If

        Using archivoLog As System.IO.StreamWriter = New System.IO.StreamWriter(folderOUTfilesCopy & "\_logCopy.log", False, System.Text.Encoding.Default)
            Using archivoData As System.IO.StreamWriter = New System.IO.StreamWriter(folderOUTfilesCopy.Replace("\jpg\", "\") & "\_JPG2codigosINE.txt", False, System.Text.Encoding.Default)
                archivoData.WriteLine("Nombre Fichero JPG;Códigos INE de municipio asociado")
                For Each documento As docCartoSEE In lista
                    'Application.DoEvents()
                    If tiposdocu2CDD.IndexOf(documento.tipoDocumento.idTipodoc) = -1 Then Continue For
                    indexProc = indexProc + 1
                    ProgressBar1.Value = indexProc
                    'Primero copiamos el JPG
                    pathOrigen = documento.rutaFicheroBajaRes
                    pathDestino = folderOUTfilesCopy & documento.nameFile4CDD
                    Try
                        System.IO.File.Copy(pathOrigen, pathDestino, True)
                        archivoLog.WriteLine("*COPIA CORRECTA*" & pathOrigen & "#" & pathDestino)
                        'Por cada INE metemos una línea
                        For Each muni As String In documento.listaCodMuniActual
                            Application.DoEvents()
                            archivoData.WriteLine(documento.nameFile4CDD & ";" & muni)
                        Next
                    Catch ex As Exception
                        archivoLog.WriteLine("#COPIA ERRONEA#" & pathOrigen & "#" & pathDestino & "#" & ex.Message)
                    End Try
                Next
            End Using
        End Using

        ProgressBar1.Value = lista.Count
        Me.Close()
        Return True


    End Function

    Function copyFiles2DirectoryToCDD(lista As ArrayList, folderOUT As String, tiposdocu2CDD As ArrayList, procesarGeorref As Boolean) As Boolean


        Dim indexProc As Integer
        Dim pathOrigen As String
        Dim pathDestino As String
        Dim pathDestinoPRJ As String

        Dim folderFileOut As String
        Dim extensionFile As String
        Dim nombreFileOut As String

        If Not folderOUT.EndsWith("\") Then folderOUT &= "\"
        Me.Text = "Copia de los ficheros JPG+ECW en ZIP para el Centro de Descargas"
        Label1.Text = "Copiando los ficheros JPG+ECW para el Centro de Descargas"
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = lista.Count
        Me.Show()

        If Not IO.Directory.Exists(folderOUT) Then
            MessageBox.Show("La ruta destino de los ficheros no existe", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Me.Close()
            Return False
        End If
        If Not IO.Directory.Exists(folderOUT & "archivos\") Then IO.Directory.CreateDirectory(folderOUT & "archivos\")

        Using archivoZIP As New IO.StreamWriter(folderOUT & "archivos\zipProc.bat", False, System.Text.Encoding.UTF8)
            Using archivoLog As New IO.StreamWriter(folderOUT & "archivos\_logCopy.log", False, System.Text.Encoding.UTF8)
                Using archivoFileToMuni As New IO.StreamWriter(folderOUT & "\files2codigosINE.txt", False, System.Text.Encoding.UTF8)
                    Using archivoAlias As New IO.StreamWriter(folderOUT & "\alias.txt", False, System.Text.Encoding.UTF8)
                        Using sentenciaSQL As New IO.StreamWriter(folderOUT & "\database.sql", False, System.Text.Encoding.UTF8)
                            archivoFileToMuni.WriteLine("Nombre Fichero;Códigos INE de municipio asociado")
                            archivoAlias.WriteLine("idProductor;Fichero;Temática;Alias;Escala;Autor;Fecha;TipoFichero")
                            For Each documento As docCartoSEE In lista

                                If tiposdocu2CDD.IndexOf(documento.tipoDocumento.idTipodoc) = -1 Then Continue For
                                indexProc = indexProc + 1
                                ProgressBar1.Value = indexProc
                                extensionFile = "JPG"
                                nombreFileOut = documento.nameFile4CDD
                                If procesarGeorref Then
                                    extensionFile = IIf(documento.listaFicherosGeo.Count = 0, "JPG", "ZIP")
                                    nombreFileOut = IIf(documento.listaFicherosGeo.Count = 0, documento.nameFile4CDD, documento.nameFolder4CDD & ".zip")
                                End If

                                archivoAlias.WriteLine(documento.getIdProductor4CdD & ";" &
                                                   nombreFileOut & ";" &
                                                   documento.tipoDocumento.tematicaCdD & ";" &
                                                   documento.getCdDAlias & ";" &
                                                   IIf(documento.Escala = 0, "Sin escala", documento.Escala.ToString) & ";" &
                                                   IIf(documento.autorDocumento = "", "Desconocido", documento.autorDocumento.ToString) & ";" &
                                                   documento.yearFechaPrincipal & ";" & extensionFile)

                                sentenciaSQL.WriteLine("UPDATE bdsidschema.archivo SET " &
                                                       "cdd_nomfich = E'" & nombreFileOut & "'," &
                                                       "cdd_titulo = E'" & documento.getCdDAlias.Replace("'", "\'") & ";" & documento.yearFechaPrincipal & "' " &
                                                       "WHERE sellado=" & documento.getIdProductor4CdD & ";")

                                'Preparamos la carpeta que contendrá los ficheros de cada documento
                                folderFileOut = folderOUT & "archivos\"
                                If procesarGeorref Then
                                    If documento.listaFicherosGeo.Count > 0 Then
                                        folderFileOut = folderOUT & "archivos\" & documento.nameFolder4CDD & "\"
                                    End If
                                End If
                                If Not IO.Directory.Exists(folderFileOut) Then IO.Directory.CreateDirectory(folderFileOut)

                                'Primero copiamos el JPG
                                pathOrigen = documento.rutaFicheroBajaRes
                                pathDestino = folderFileOut & documento.nameFile4CDD
                                Try
                                    IO.File.Copy(pathOrigen, pathDestino, True)
                                    'archivoLog.WriteLine("*COPIA CORRECTA*" & pathOrigen & "#" & pathDestino)
                                    'Por cada INE metemos una línea
                                    For Each muni As String In documento.listaCodMuniActual
                                        Application.DoEvents()
                                        If procesarGeorref Then
                                            If documento.listaFicherosGeo.Count = 0 Then
                                                archivoFileToMuni.WriteLine(documento.nameFile4CDD & ";" & muni)
                                            Else
                                                archivoFileToMuni.WriteLine(documento.nameFolder4CDD & ".zip" & ";" & muni)
                                            End If
                                        Else
                                            archivoFileToMuni.WriteLine(documento.nameFile4CDD & ";" & muni)
                                        End If
                                    Next
                                Catch ex As Exception
                                    archivoLog.WriteLine("#COPIA ERRONEA#" & pathOrigen & "#" & pathDestino & "#" & ex.Message)
                                End Try

                                If procesarGeorref Then
                                    'Después copiamos el ECW si los hay
                                    If documento.listaFicherosGeo.Count = 1 Then
                                        pathOrigen = documento.listaFicherosGeo.Item(0)
                                        pathDestino = folderFileOut & documento.nameFile4CDD.Replace(".jpg", ".ecw")
                                        'Y también el fichero PRJ
                                        pathDestinoPRJ = folderFileOut & documento.nameFile4CDD.Replace(".jpg", ".prj")
                                        Try
                                            IO.File.Copy(pathOrigen, pathDestino, True)
                                            IO.File.Copy(My.Application.Info.DirectoryPath & "\resources\proj23030.prj", pathDestinoPRJ, True)
                                            archivoLog.WriteLine("*COPIA CORRECTA*" & pathOrigen & "#" & pathDestino)
                                        Catch ex As Exception
                                            archivoLog.WriteLine("#COPIA ERRONEA#" & pathOrigen & "#" & pathDestino & "#" & ex.Message)
                                        End Try
                                    Else
                                        For Each docuECW As String In documento.listaFicherosGeo
                                            pathOrigen = docuECW
                                            pathDestino = folderFileOut & SacarFileDeRuta(pathOrigen)
                                            'Y también el fichero PRJ
                                            pathDestinoPRJ = folderFileOut & SacarFileDeRuta(pathOrigen).Replace(".ecw", ".prj")
                                            Try
                                                IO.File.Copy(pathOrigen, pathDestino, True)
                                                IO.File.Copy(My.Application.Info.DirectoryPath & "\resources\proj23030.prj", pathDestinoPRJ, True)
                                                archivoLog.WriteLine("*COPIA CORRECTA*" & pathOrigen & "#" & pathDestino)
                                            Catch ex As Exception
                                                archivoLog.WriteLine("#COPIA ERRONEA#" & pathOrigen & "#" & pathDestino & "#" & ex.Message)
                                            End Try
                                        Next
                                    End If
                                    If documento.listaFicherosGeo.Count > 0 Then
                                        archivoZIP.WriteLine("""" & path7zUtility & """ a " & folderOUT & "archivos\" & documento.nameFolder4CDD & ".zip """ & folderFileOut & """")
                                        archivoZIP.WriteLine("del /S /Q """ & folderFileOut & """")
                                        archivoZIP.WriteLine("rd """ & folderFileOut & """")
                                    End If
                                End If
                            Next
                        End Using
                    End Using
                End Using
            End Using
        End Using
        ProgressBar1.Value = lista.Count

        Me.Close()
        Return True


    End Function

    Function copyFilesThumb2Directory(ByVal lista As ArrayList, folderOUTfilesCopy As String, tiposdocu2CDD As ArrayList) As Boolean


        Dim indexProc As Integer
        Dim pathOrigen As String
        Dim pathDestino As String

        Me.Text = "Copia de los ficheros miniatura JPG para el Centro de Descargas"
        Label1.Text = "Copiando los ficheros miniatura JPG para el Centro de Descargas"
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = lista.Count
        Me.Show()

        If Not System.IO.Directory.Exists(folderOUTfilesCopy) Then
            MessageBox.Show("La ruta destino de los ficheros no existe", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Me.Close()
            Return False
        End If

        Using archivoLog As System.IO.StreamWriter = New System.IO.StreamWriter(folderOUTfilesCopy & "\_logCopy.log", False, System.Text.Encoding.Default)
            For Each documento As docCartoSEE In lista
                'Application.DoEvents()
                If tiposdocu2CDD.IndexOf(documento.tipoDocumento.idTipodoc) = -1 Then Continue For
                indexProc = indexProc + 1
                ProgressBar1.Value = indexProc
                pathOrigen = documento.rutaFicheroThumb
                pathDestino = folderOUTfilesCopy & documento.nameFile4CDD
                Try
                    System.IO.File.Copy(pathOrigen, pathDestino, True)
                Catch ex As Exception
                    archivoLog.WriteLine("#COPIA ERRONEA#" & pathOrigen & "#" & pathDestino & "#" & ex.Message)
                End Try
            Next
        End Using

        ProgressBar1.Value = lista.Count
        Me.Close()
        Return True


    End Function





End Class
