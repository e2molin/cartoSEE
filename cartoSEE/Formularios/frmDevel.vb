Public Class frmDevel

    Private Function reproyectarGCP(ByVal cadIN As String, ByVal epsgIN As Integer, ByVal epsgOUT As Integer) As String
        '23147.99579232,1585.87754462,-6.1861555556,43.6677916667,"Point 1",0.00
        Dim parts() As String = cadIN.Split(",")
        Dim cadenaConv As String = ""
        Application.DoEvents()
        ObtenerEscalar("select AsText(ST_Transform(GeomFromText('POINT(" & parts(2) & " " & parts(3) & ")'," & epsgIN & ")," & epsgOUT & ")) as Resultado", cadenaConv)
        Return parts(0) & "," & parts(1) & "," & cadenaConv.Replace("POINT(", "").Replace(")", "").Replace(" ", ",") & "," & parts(4) & "," & parts(5)
        Application.DoEvents()
    End Function

    Private Function extractCoordenadas(ByVal cadIN As String) As String
        '23147.99579232,1585.87754462,-6.1861555556,43.6677916667,"Point 1",0.00
        Dim parts() As String = cadIN.Split(",")
        Dim cadenaConv As String = ""

        Return parts(2) & " " & parts(3)
        Application.DoEvents()
    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim rcdTMP As New DataTable
        Dim results() As DataRow
        Dim sellado As String
        Dim doc As docSIDCARTO
        Dim contador As Integer
        Dim sw As New System.IO.StreamWriter(My.Application.Info.DirectoryPath & "\scriptBD.sql")

        CargarRecordset("select * from contornos where nombre in (select nombre from contornos group by nombre having count(*)>1) order by nombre", rcdTMP)
        results = rcdTMP.Select
        rcdTMP.Dispose()
        rcdTMP = Nothing

        For Each fila As DataRow In results
            contador = contador + 1
            ToolStripStatusLabel1.Text = "Analizando " & contador
            Application.DoEvents()
            sellado = fila.Item("sellado").ToString
            doc = DameDocumentacionSIDCARTO_bySellado(sellado)

            If doc.CodTipo <> fila.Item("tipodoc") Then
                'Este no coincide
                sw.WriteLine("DELETE FROM contornos WHERE tipodoc=" & fila.Item("tipodoc") & " AND nombre='" & fila.Item("nombre").ToString & "';")
            End If
        Next
        Erase results
        sw.Close()
        sw.Dispose()
        sw = Nothing
        MessageBox.Show("Proceso terminado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)



    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        ListBox1.Items.Clear()
        Dim epsgIN As String = txtEPSGin.Text.Trim
        Dim epsgOUT As String = txtEPSGout.Text.Trim


        Dim newFolder As String = txtPathGCP.Text & "\GCP" & epsgOUT
        Dim sentencesOUT As ArrayList
        Dim lineIN As String
        Dim nomFich As String
        Dim deltaX As Double = 0
        Dim deltaY As Double = 0
        'Creamos la carpeta de salida si no existe
        If Not System.IO.Directory.Exists(newFolder) Then
            Try
                System.IO.Directory.CreateDirectory(newFolder)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
                Exit Sub
            End Try
        End If

        Try
            If txtDeltaX.Text.Trim <> "" Then
                deltaX = CType(txtDeltaX.Text.Replace(".", ","), Double)
            End If
            If txtDeltaY.Text.Trim <> "" Then
                deltaY = CType(txtDeltaY.Text.Replace(".", ","), Double)
            End If
        Catch ex As Exception
            MessageBox.Show("Problemas con los parámetros de desplazamiento", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try



        Try
            ' lista todos los archivos gcp del directorio windows _  
            ' SearchAllSubDirectories : incluye los Subdirectorios  
            ' SearchTopLevelOnly : para buscar solo en el nivel actual  
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''  
            For Each Archivo As String In My.Computer.FileSystem.GetFiles( _
                                    txtPathGCP.Text, _
                                    FileIO.SearchOption.SearchTopLevelOnly, _
                                    "*.gcp")

                ListBox1.Items.Add(Archivo)
                sentencesOUT = New ArrayList

                'Leemos las frases del archivo de entrada y las covertimos
                Using sr As New System.IO.StreamReader(Archivo)
                    lineIN = sr.ReadLine
                    Do While (Not lineIN Is Nothing)
                        sentencesOUT.Add(reproyectarGCP(lineIN, epsgIN, epsgOUT))
                        lineIN = sr.ReadLine
                    Loop
                    sr.Close()
                    sr.Dispose()
                End Using

                nomFich = SacarFileDeRuta(Archivo)

                Using sw As New System.IO.StreamWriter(newFolder & "\" & nomFich)
                    For Each sentence As String In sentencesOUT
                        sw.WriteLine(sentence)
                    Next
                    sw.Close()
                    sw.Dispose()
                End Using

                sentencesOUT.Clear()
                sentencesOUT = Nothing

            Next
        Catch oe As Exception
            MsgBox(oe.Message, MsgBoxStyle.Critical)
        End Try

        MsgBox("Terminado")



    End Sub

    Private Sub btnSelectFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectFolder.Click

        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
        If System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath) = False Then Exit Sub
        txtPathGCP.Text = FolderBrowserDialog1.SelectedPath
        lblInfoNumFiles.Text = "Ficheros GCP encontrados: " & My.Computer.FileSystem.GetFiles( _
                                    FolderBrowserDialog1.SelectedPath, _
                                    FileIO.SearchOption.SearchTopLevelOnly, _
                                    "*.gcp").Count



    End Sub

    Private Sub frmDevel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        txtPathGCP.Text = ""
        lblInfoNumFiles.Text = ""
        txtEPSGin.Text = "4230"
        txtEPSGout.Text = "25830"

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

        For ibucle As Integer = 1 To 50
            TestECWExist(ibucle)
        Next

        MessageBox.Show("Proceso terminado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)



    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click

        Test_cdd_planospoblacion_report()

    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Test_cdd_edificacion_report()
    End Sub

    Sub TestECWExist(ByVal codProvin As Integer)

        Dim ListaDocumentosProv() As docSIDCARTO

        DameDocumentacionSIDCARTO_ByProvincia(codProvin, ListaDocumentosProv, "", "")


        'Repositorio de documento georreferenciado asociado
        '-----------------------------------------------------------------------------------------------------------------
        Dim RutasDoc() As String
        Dim iCont As Integer = 0
        Dim noFind As Integer = 0
        Dim cadsql As String
        For Each documento As docSIDCARTO In ListaDocumentosProv
            Application.DoEvents()
            iCont = iCont + 1
            ToolStripStatusLabel1.Text = "Procesando " & iCont
            Erase RutasDoc
            DameRutasFicherosECW(documento, RutasDoc)
            If RutasDoc.Length > 0 And Not RutasDoc(0) Is Nothing Then
                Dim partes() As String = RutasDoc(0).ToString.Split("\")
                Application.DoEvents()
                cadsql = "UPDATE archivo set municipiohistorico1_id=" & partes(6) & " WHERE idarchivo=" & documento.Indice
                ExeSinTran(cadsql)
                Erase partes
            Else
                noFind = noFind + 1
                cadsql = "UPDATE archivo set municipiohistorico1_id=0 WHERE idarchivo=" & documento.Indice
                GenerarLOG("No encontrado nº " & documento.Sellado & ";" & documento.Tipo & ";" & documento.ProvinciaRepo)
                ExeSinTran(cadsql)
            End If
        Next
        ToolStripStatusLabel1.Text = "procesados " & iCont & ". No encontrados " & noFind
        Erase ListaDocumentosProv

    End Sub

    ''' <summary>
    ''' Comprueba que las rutas de esta vista existen
    ''' </summary>
    ''' <remarks></remarks>
    Sub Test_cdd_planospoblacion_report()

        Dim docPP2CD As DataTable
        Dim filas() As DataRow
        docPP2CD = New DataTable
        Dim rutaFile As String
        Dim contador As Integer

        ListBox2.Items.Clear()
        If CargarDatatableMuni("select * from cdd_planospoblacion_report", docPP2CD) = False Then
            MessageBox.Show("No se puede acceder a la vista de cdd_planospoblacion_report", My.Application.Info.AssemblyName, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        filas = docPP2CD.Select
        contador = 0
        For Each fila As DataRow In filas
            contador = contador + 1
            ToolStripStatusLabel1.Text = "Procesando " & contador
            Application.DoEvents()
            rutaFile = fila.Item("rutaresource").ToString
            If Not System.IO.File.Exists(rutaFile.Replace("file:", "")) Then
                ListBox2.Items.Add("Fichero no localizado: " & rutaFile)
            End If
            Application.DoEvents()

        Next

        Erase filas
        docPP2CD.Clear()
        docPP2CD = Nothing
        MessageBox.Show("Proceso de verificación terminado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)



    End Sub

    'Generar fichero conversion 23030 a 25830 para HK
    Sub generate_conversionFile_hojaskilometricas_report()

        Dim docHK2CD As DataTable
        Dim filas() As DataRow
        Dim rutaFile As String
        Dim contador As Integer
        Dim LineaOUT As String
        Dim cadsql As String
        docHK2CD = New DataTable

        Dim swConversion As New System.IO.StreamWriter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\conversionEdi.gms", False, System.Text.Encoding.Unicode)
        Dim swMkdir As New System.IO.StreamWriter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\conversionEdi_mkdir.bat", False, System.Text.Encoding.Unicode)
        swConversion.WriteLine("GLOBAL_MAPPER_SCRIPT VERSION=1.00 ENABLE_PROGRESS=YES")
        swConversion.WriteLine("UNLOAD_ALL")
        swConversion.WriteLine("SET_BG_COLOR COLOR=RGB(255,255,255)")


        cadsql = "SELECT archivo.numdoc, tbtipodocumento.tipodoc, " & _
                "to_char(min(munihisto.cod_munihisto), 'FM0000009'::text) AS codinehisto, to_char(min(munihisto.cod_muni), 'FM00009'::text) AS codigoine, " & _
                "date_part('year'::text, archivo.fechaprincipal) AS anyo, " & _
                "((((((('file://sbdignmad650/geodocat_ii/'::text || tbtipodocumento.dirrepo::text) || '/'::text) || " & _
                "substring(to_char(archivo.municipiohistorico1_id, 'FM0000009'::text), 0, 3)) || '/'::text) || " & _
                "to_char(archivo.municipiohistorico1_id, 'FM0000009'::text)) || '/'::text) || contornos.nombre::text) || '.ecw'::text AS rutaresource, 'ECW' AS extension, archivo.provincia_id AS provin " & _
                "FROM contornos " & _
                "INNER JOIN archivo ON archivo.idarchivo = contornos.archivo_id " & _
                "INNER JOIN provincias ON provincias.idprovincia = archivo.provincia_id " & _
                "INNER JOIN tbtipodocumento ON archivo.tipodoc_id = tbtipodocumento.idtipodoc " & _
                "INNER JOIN archivo2munihisto ON archivo.idarchivo = archivo2munihisto.archivo_id " & _
                "INNER JOIN munihisto ON archivo2munihisto.munihisto_id = munihisto.idmunihisto " & _
                "WHERE (archivo.tipodoc_id = 7) AND archivo.municipiohistorico1_id <> 0 " & _
                "GROUP BY archivo.numdoc, tbtipodocumento.tipodoc, archivo.fechaprincipal, provincias.dirrepo, tbtipodocumento.dirrepo, archivo.provincia_id, archivo.municipiohistorico1_id, contornos.nombre"

        cadsql = "SELECT archivo.numdoc, tbtipodocumento.tipodoc, to_char(min(munihisto.cod_munihisto), 'FM0000009'::text) AS codinehisto, to_char(min(munihisto.cod_muni), 'FM00009'::text) AS codigoine, " & _
                "date_part('year'::text, archivo.fechaprincipal) AS anyo, ((((((('file://sbdignmad650/geodocat_ii/'::text || tbtipodocumento.dirrepo::text) || " & _
                "'/'::text) || substring(to_char(archivo.municipiohistorico1_id, 'FM0000009'::text), 0, 3)) || '/'::text) || to_char(archivo.municipiohistorico1_id, 'FM0000009'::text)) || '/'::text) || " & _
                "contornos.nombre::text) || '.ecw'::text AS rutaresource,'ECW' AS extension, archivo.provincia_id AS provin " & _
                "FROM contornos " & _
                "INNER JOIN archivo ON archivo.idarchivo = contornos.archivo_id " & _
                "INNER JOIN provincias ON provincias.idprovincia = archivo.provincia_id " & _
                "INNER JOIN tbtipodocumento ON archivo.tipodoc_id = tbtipodocumento.idtipodoc " & _
                "INNER JOIN archivo2munihisto ON archivo.idarchivo = archivo2munihisto.archivo_id " & _
                "INNER JOIN munihisto ON archivo2munihisto.munihisto_id = munihisto.idmunihisto " & _
                "WHERE (archivo.tipodoc_id = 6) AND archivo.municipiohistorico1_id <> 0 " & _
                "GROUP BY archivo.numdoc, tbtipodocumento.tipodoc, archivo.fechaprincipal, provincias.dirrepo, tbtipodocumento.dirrepo, archivo.provincia_id, archivo.municipiohistorico1_id, contornos.nombre"
        cadsql = "SELECT archivo.numdoc, tbtipodocumento.tipodoc, to_char(min(munihisto.cod_munihisto), 'FM0000009'::text) AS codinehisto, to_char(min(munihisto.cod_muni), 'FM00009'::text) AS codigoine, " & _
                "date_part('year'::text, archivo.fechaprincipal) AS anyo, ((((((('file://sbdignmad650/geodocat_ii/'::text || tbtipodocumento.dirrepo::text) || " & _
                "'/'::text) || substring(to_char(archivo.municipiohistorico1_id, 'FM0000009'::text), 0, 3)) || '/'::text) || to_char(archivo.municipiohistorico1_id, 'FM0000009'::text)) || '/'::text) || " & _
                "contornos.nombre::text) || '.ecw'::text AS rutaresource,'ECW' AS extension, archivo.provincia_id AS provin " & _
                "FROM contornos " & _
                "INNER JOIN archivo ON archivo.idarchivo = contornos.archivo_id " & _
                "INNER JOIN provincias ON provincias.idprovincia = archivo.provincia_id " & _
                "INNER JOIN tbtipodocumento ON archivo.tipodoc_id = tbtipodocumento.idtipodoc " & _
                "INNER JOIN archivo2munihisto ON archivo.idarchivo = archivo2munihisto.archivo_id " & _
                "INNER JOIN munihisto ON archivo2munihisto.munihisto_id = munihisto.idmunihisto " & _
                "WHERE (archivo.tipodoc_id = 5) AND archivo.municipiohistorico1_id <> 0 " & _
                "GROUP BY archivo.numdoc, tbtipodocumento.tipodoc, archivo.fechaprincipal, provincias.dirrepo, tbtipodocumento.dirrepo, archivo.provincia_id, archivo.municipiohistorico1_id, contornos.nombre"

        Application.DoEvents()



        ListBox2.Items.Clear()
        If CargarDatatableMuni(cadsql, docHK2CD) = False Then
            MessageBox.Show("No se puede acceder a la vista de cdd_planospoblacion_report", My.Application.Info.AssemblyName, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        filas = docHK2CD.Select
        contador = 0
        For Each fila As DataRow In filas
            contador = contador + 1
            ToolStripStatusLabel1.Text = "Procesando " & contador
            Application.DoEvents()
            rutaFile = fila.Item("rutaresource").ToString
            If Not System.IO.File.Exists(rutaFile.Replace("file:", "")) Then
                ListBox2.Items.Add("Fichero no localizado: " & rutaFile)
            Else
                Application.DoEvents()
                LineaOUT = "IMPORT FILENAME=""" & rutaFile.Replace("file:", "") & """ TYPE=AUTO ANTI_ALIAS=NO AUTO_CONTRAST=NO"
                swConversion.WriteLine(LineaOUT)
                LineaOUT = "LOAD_PROJECTION FILENAME=""G:\test25830.prj"""
                swConversion.WriteLine(LineaOUT)
                LineaOUT = "EXPORT_RASTER FILENAME=""" & _
                            rutaFile.Replace("file:", "").Replace("/", "\").Replace("\\sbdignmad650\geodocat_ii", "g:\25830") & _
                            """ TYPE=ECW TARGET_COMPRESSION=1 GEN_WORLD_FILE=YES GEN_PRJ_FILE=YES"
                swConversion.WriteLine(LineaOUT)
                swConversion.WriteLine("UNLOAD_ALL")
                swMkdir.WriteLine("mkdir " & SacarDirDeRuta(rutaFile.Replace("file:", "").Replace("/", "\").Replace("\\sbdignmad650\geodocat_ii", "g:\25830")))
            End If
            Application.DoEvents()

        Next

        Erase filas
        docHK2CD.Clear()
        docHK2CD = Nothing
        swConversion.Close()
        swConversion.Dispose()
        swConversion = Nothing
        swMkdir.Close()
        swMkdir.Dispose()
        swMkdir = Nothing
        MessageBox.Show("Fichero de conversión generado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)



    End Sub

    'Generar fichero conversion 23030 a 25830 para PP
    Sub generate_conversionFile_planospoblacion_report()

        Dim docPP2CD As DataTable
        Dim filas() As DataRow
        Dim rutaFile As String
        Dim contador As Integer
        Dim LineaOUT As String
        Dim cadsql As String
        docPP2CD = New DataTable

        Dim swConversion As New System.IO.StreamWriter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\conversionPP.gms", False, System.Text.Encoding.Unicode)
        Dim swMkdir As New System.IO.StreamWriter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\conversionPP_mkdir.bat", False, System.Text.Encoding.Unicode)
        swConversion.WriteLine("GLOBAL_MAPPER_SCRIPT VERSION=1.00 ENABLE_PROGRESS=YES")
        swConversion.WriteLine("UNLOAD_ALL")
        swConversion.WriteLine("SET_BG_COLOR COLOR=RGB(255,255,255)")


        cadsql = "SELECT archivo.numdoc, tbtipodocumento.tipodoc, " & _
                "to_char(min(munihisto.cod_munihisto), 'FM0000009'::text) AS codinehisto, to_char(min(munihisto.cod_muni), 'FM00009'::text) AS codigoine, " & _
                "date_part('year'::text, archivo.fechaprincipal) AS anyo, " & _
                "((((((('file://sbdignmad650/geodocat_ii/'::text || tbtipodocumento.dirrepo::text) || '/'::text) || " & _
                "substring(to_char(archivo.municipiohistorico1_id, 'FM0000009'::text), 0, 3)) || '/'::text) || " & _
                "to_char(archivo.municipiohistorico1_id, 'FM0000009'::text)) || '/'::text) || contornos.nombre::text) || '.ecw'::text AS rutaresource, 'ECW' AS extension, archivo.provincia_id AS provin " & _
                "FROM contornos " & _
                "INNER JOIN archivo ON archivo.idarchivo = contornos.archivo_id " & _
                "INNER JOIN provincias ON provincias.idprovincia = archivo.provincia_id " & _
                "INNER JOIN tbtipodocumento ON archivo.tipodoc_id = tbtipodocumento.idtipodoc " & _
                "INNER JOIN archivo2munihisto ON archivo.idarchivo = archivo2munihisto.archivo_id " & _
                "INNER JOIN munihisto ON archivo2munihisto.munihisto_id = munihisto.idmunihisto " & _
                "WHERE (archivo.tipodoc_id = 7) AND archivo.municipiohistorico1_id <> 0 " & _
                "GROUP BY archivo.numdoc, tbtipodocumento.tipodoc, archivo.fechaprincipal, provincias.dirrepo, tbtipodocumento.dirrepo, archivo.provincia_id, archivo.municipiohistorico1_id, contornos.nombre"


        ListBox2.Items.Clear()
        If CargarDatatableMuni(cadsql, docPP2CD) = False Then
            MessageBox.Show("No se puede acceder a la vista de cdd_planospoblacion_report", My.Application.Info.AssemblyName, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        filas = docPP2CD.Select
        contador = 0
        For Each fila As DataRow In filas
            contador = contador + 1
            ToolStripStatusLabel1.Text = "Procesando " & contador
            Application.DoEvents()
            rutaFile = fila.Item("rutaresource").ToString
            If Not System.IO.File.Exists(rutaFile.Replace("file:", "")) Then
                ListBox2.Items.Add("Fichero no localizado: " & rutaFile)
            Else
                Application.DoEvents()
                LineaOUT = "IMPORT FILENAME=""" & rutaFile.Replace("file:", "") & """ TYPE=AUTO ANTI_ALIAS=NO AUTO_CONTRAST=NO"
                swConversion.WriteLine(LineaOUT)
                LineaOUT = "LOAD_PROJECTION FILENAME=""G:\test25830.prj"""
                swConversion.WriteLine(LineaOUT)
                LineaOUT = "EXPORT_RASTER FILENAME=""" & _
                            rutaFile.Replace("file:", "").Replace("/", "\").Replace("\\sbdignmad650\geodocat_ii", "g:\25830") & _
                            """ TYPE=ECW TARGET_COMPRESSION=1 GEN_WORLD_FILE=YES GEN_PRJ_FILE=YES"
                swConversion.WriteLine(LineaOUT)
                swConversion.WriteLine("UNLOAD_ALL")
                swMkdir.WriteLine("mkdir " & SacarDirDeRuta(rutaFile.Replace("file:", "").Replace("/", "\").Replace("\\sbdignmad650\geodocat_ii", "g:\25830")))
            End If
            Application.DoEvents()

        Next

        Erase filas
        docPP2CD.Clear()
        docPP2CD = Nothing
        swConversion.Close()
        swConversion.Dispose()
        swConversion = Nothing
        swMkdir.Close()
        swMkdir.Dispose()
        swMkdir = Nothing
        MessageBox.Show("Fichero de conversión generado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)



    End Sub


    'Generar platilla con los recortes por tipo de documento y provincia
    Sub generate_template_cutter(nombreScript As String, codprovin As Integer, tipodoc As Integer)

        Dim docPP2CD As DataTable
        Dim filas() As DataRow
        Dim rutaFile As String
        Dim contador As Integer
        Dim cadsql As String
        docPP2CD = New DataTable
        Dim rutasECW As New ArrayList

        cadsql = "SELECT archivo.numdoc, tbtipodocumento.tipodoc, " & _
                "to_char(min(munihisto.cod_munihisto), 'FM0000009'::text) AS codinehisto, to_char(min(munihisto.cod_muni), 'FM00009'::text) AS codigoine, " & _
                "date_part('year'::text, archivo.fechaprincipal) AS anyo, " & _
                "((((((('file://sbdignmad650/geodocat_ii/'::text || tbtipodocumento.dirrepo::text) || '/'::text) || " & _
                "substring(to_char(archivo.municipiohistorico1_id, 'FM0000009'::text), 0, 3)) || '/'::text) || " & _
                "to_char(archivo.municipiohistorico1_id, 'FM0000009'::text)) || '/'::text) || contornos.nombre::text) || '.ecw'::text AS rutaresource, 'ECW' AS extension, archivo.provincia_id AS provin " & _
                "FROM contornos " & _
                "INNER JOIN archivo ON archivo.idarchivo = contornos.archivo_id " & _
                "INNER JOIN provincias ON provincias.idprovincia = archivo.provincia_id " & _
                "INNER JOIN tbtipodocumento ON archivo.tipodoc_id = tbtipodocumento.idtipodoc " & _
                "INNER JOIN archivo2munihisto ON archivo.idarchivo = archivo2munihisto.archivo_id " & _
                "INNER JOIN munihisto ON archivo2munihisto.munihisto_id = munihisto.idmunihisto " & _
                "WHERE (archivo.tipodoc_id = " & tipodoc & ") AND archivo.municipiohistorico1_id <> 0 and archivo.provincia_id=" & codprovin & " " & _
                "GROUP BY archivo.numdoc, tbtipodocumento.tipodoc, archivo.fechaprincipal, provincias.dirrepo, tbtipodocumento.dirrepo, archivo.provincia_id, archivo.municipiohistorico1_id, contornos.nombre"
        Application.DoEvents()


        If CargarDatatableMuni(cadsql, docPP2CD) = False Then
            MessageBox.Show("No se puede acceder a la vista de cdd_planospoblacion_report", My.Application.Info.AssemblyName, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        filas = docPP2CD.Select
        contador = 0
        For Each fila As DataRow In filas
            contador = contador + 1
            ToolStripStatusLabel1.Text = "Procesando los documentos " & tipodoc & " de la provincia " & codprovin & " Documento nº " & contador
            Application.DoEvents()
            rutaFile = fila.Item("rutaresource").ToString
            If Not System.IO.File.Exists(rutaFile.Replace("file:", "")) Then
                ListBox2.Items.Add("Fichero no localizado: " & rutaFile)
            Else
                Application.DoEvents()
                rutasECW.Add(rutaFile.Replace("file:", "").Replace("/", "\"))
            End If
            Application.DoEvents()

        Next

        Erase filas
        docPP2CD.Clear()
        docPP2CD = Nothing
        If rutasECW.Count = 0 Then Exit Sub
        GenerarProyectoGM(rutasECW, True, nombreScript)
        rutasECW.Clear()
        rutasECW = Nothing

    End Sub

    'Generar platilla con los recortes por tipo de documento y provincia
    Sub generate_templateHK_cutter(nombreScript As String, codmuniHisto As String)

        Dim docPP2CD As DataTable
        Dim filas() As DataRow
        Dim rutaFile As String
        Dim contador As Integer
        Dim cadsql As String
        docPP2CD = New DataTable
        Dim rutasECW As New ArrayList

        cadsql = "SELECT archivo.numdoc, tbtipodocumento.tipodoc, " & _
                "to_char(min(munihisto.cod_munihisto), 'FM0000009'::text) AS codinehisto, to_char(min(munihisto.cod_muni), 'FM00009'::text) AS codigoine, " & _
                "date_part('year'::text, archivo.fechaprincipal) AS anyo, " & _
                "((((((('file://sbdignmad650/geodocat_ii/'::text || tbtipodocumento.dirrepo::text) || '/'::text) || " & _
                "substring(to_char(archivo.municipiohistorico1_id, 'FM0000009'::text), 0, 3)) || '/'::text) || " & _
                "to_char(archivo.municipiohistorico1_id, 'FM0000009'::text)) || '/'::text) || contornos.nombre::text) || '.ecw'::text AS rutaresource, 'ECW' AS extension, archivo.provincia_id AS provin " & _
                "FROM contornos " & _
                "INNER JOIN archivo ON archivo.idarchivo = contornos.archivo_id " & _
                "INNER JOIN provincias ON provincias.idprovincia = archivo.provincia_id " & _
                "INNER JOIN tbtipodocumento ON archivo.tipodoc_id = tbtipodocumento.idtipodoc " & _
                "INNER JOIN archivo2munihisto ON archivo.idarchivo = archivo2munihisto.archivo_id " & _
                "INNER JOIN munihisto ON archivo2munihisto.munihisto_id = munihisto.idmunihisto " & _
                "WHERE (archivo.tipodoc_id = 6) AND archivo.municipiohistorico1_id <> 0 " & _
                "GROUP BY archivo.numdoc, tbtipodocumento.tipodoc, archivo.fechaprincipal, provincias.dirrepo, tbtipodocumento.dirrepo, archivo.provincia_id, archivo.municipiohistorico1_id, contornos.nombre " & _
                "having to_char(min(munihisto.cod_munihisto), 'FM0000009'::text)='" & codmuniHisto & "'"
        Application.DoEvents()


        If CargarDatatableMuni(cadsql, docPP2CD) = False Then
            MessageBox.Show("No se puede acceder a la vista de cdd_planospoblacion_report", My.Application.Info.AssemblyName, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        filas = docPP2CD.Select
        contador = 0
        For Each fila As DataRow In filas
            contador = contador + 1
            ToolStripStatusLabel1.Text = "Procesando las HK del municipio " & codmuniHisto & ". Documento nº " & contador
            Application.DoEvents()
            rutaFile = fila.Item("rutaresource").ToString
            If Not System.IO.File.Exists(rutaFile.Replace("file:", "")) Then
                ListBox2.Items.Add("Fichero no localizado: " & rutaFile)
            Else
                Application.DoEvents()
                rutasECW.Add(rutaFile.Replace("file:", "").Replace("/", "\"))
            End If
            Application.DoEvents()

        Next

        Erase filas
        docPP2CD.Clear()
        docPP2CD = Nothing
        If rutasECW.Count = 0 Then Exit Sub
        GenerarProyectoGM(rutasECW, True, nombreScript)
        rutasECW.Clear()
        rutasECW = Nothing

    End Sub


    ''' <summary>
    ''' Comprueba que las rutas de esta vista existen
    ''' </summary>
    ''' <remarks></remarks>
    Sub Test_cdd_edificacion_report()

        Dim docEdi2CD As DataTable
        Dim filas() As DataRow
        docEdi2CD = New DataTable
        Dim rutaFile As String
        Dim contador As Integer

        ListBox2.Items.Clear()
        If CargarDatatableMuni("select * from cdd_edificacion_report", docEdi2CD) = False Then
            MessageBox.Show("No se puede acceder a la vista de cdd_edificacion_report", My.Application.Info.AssemblyName, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        filas = docEdi2CD.Select
        contador = 0
        For Each fila As DataRow In filas
            contador = contador + 1
            ToolStripStatusLabel1.Text = "Procesando " & contador
            Application.DoEvents()
            rutaFile = fila.Item("rutaresource").ToString
            If Not System.IO.File.Exists(rutaFile.Replace("file:", "")) Then
                ListBox2.Items.Add("Fichero no localizado: " & rutaFile)
            End If
            Application.DoEvents()

        Next

        Erase filas
        docEdi2CD.Clear()
        docEdi2CD = Nothing
        MessageBox.Show("Proceso de verificación terminado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)



    End Sub


    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click

        Dim LineaOUT As String

        If ListBox2.Items.Count = 0 Then Exit Sub
        With SaveFileDialog1
            .Title = "Introduzca el nombre del fichero"
            .Filter = "Archivos CSV *.csv|*.csv"
            .ShowDialog()
        End With
        If SaveFileDialog1.FileName = "" Then Exit Sub

        Dim sw As New System.IO.StreamWriter(SaveFileDialog1.FileName, False, System.Text.Encoding.Unicode)

        For l_index As Integer = 0 To ListBox2.Items.Count - 1
            LineaOUT = ListBox2.Items(l_index).ToString
            sw.WriteLine(LineaOUT)

        Next
        sw.Close()
        sw.Dispose()
        MessageBox.Show("Fichero de ibnformes generado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub


    Private Sub Button7_Click(sender As System.Object, e As System.EventArgs) Handles Button7.Click

        ListBox1.Items.Clear()
        Dim epsgIN As String = txtEPSGin.Text.Trim
        Dim epsgOUT As String = txtEPSGout.Text.Trim


        Dim ficheroFusion As String = txtPathGCP.Text & "\fusion.xyz"
        Dim sentencesOUT As ArrayList
        Dim lineIN As String
        Dim nomFich As String
        Dim deltaX As Double = 0
        Dim deltaY As Double = 0
        'Creamos la carpeta de salida si no existe
        If System.IO.File.Exists(ficheroFusion) Then
            Try
                System.IO.File.Delete(ficheroFusion)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
                Exit Sub
            End Try
        End If


        'Try
        ' lista todos los archivos gcp del directorio windows _  
        ' SearchAllSubDirectories : incluye los Subdirectorios  
        ' SearchTopLevelOnly : para buscar solo en el nivel actual  
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''  
        For Each Archivo As String In My.Computer.FileSystem.GetFiles( _
                                txtPathGCP.Text, _
                                FileIO.SearchOption.SearchTopLevelOnly, _
                                "*.gcp")

            ListBox1.Items.Add(Archivo)
            sentencesOUT = New ArrayList

            'Leemos las frases del archivo de entrada y las convertimos
            Using sr As New System.IO.StreamReader(Archivo)
                lineIN = sr.ReadLine
                Do While (Not lineIN Is Nothing)
                    If Not lineIN.StartsWith("Method=") And lineIN <> "" Then
                        sentencesOUT.Add(extractCoordenadas(lineIN))
                    End If
                    lineIN = sr.ReadLine
                Loop
                sr.Close()
                sr.Dispose()
            End Using

            nomFich = SacarFileDeRuta(Archivo)

            Using sw As New System.IO.StreamWriter(ficheroFusion, True)
                sw.WriteLine("NAME=" & SacarFileDeRuta(Archivo))
                sw.WriteLine("CLOSED=YES")
                For Each sentence As String In sentencesOUT
                    sw.WriteLine(sentence)
                Next
                sw.Close()
                sw.Dispose()
            End Using

            sentencesOUT.Clear()
            sentencesOUT = Nothing

        Next
        'Catch oe As Exception
        '    MsgBox(oe.Message, MsgBoxStyle.Critical)

        'End Try

        MsgBox("Terminado")
    End Sub



    Private Sub Button8_Click(sender As System.Object, e As System.EventArgs) Handles Button8.Click
        generate_conversionFile_planospoblacion_report()

    End Sub

    Private Sub Button9_Click(sender As System.Object, e As System.EventArgs) Handles Button9.Click
        generate_conversionFile_hojaskilometricas_report()
    End Sub

    Private Sub Button10_Click(sender As System.Object, e As System.EventArgs) Handles Button10.Click

        Dim iBucle As Integer
        For ibucle = 1 To 50
            generate_template_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\PlanosPoblacion\" & String.Format("{0:00}", iBucle) & ".gmw", iBucle, 7)
        Next
        MessageBox.Show("Proyectos de recorte generados", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub Button11_Click(sender As System.Object, e As System.EventArgs) Handles Button11.Click

        Dim iBucle As Integer
        ListBox2.Items.Clear()
        For iBucle = 1 To 50
            generate_template_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\Altimetrias\" & String.Format("{0:00}", iBucle) & ".gmw", iBucle, 1)
        Next
        MessageBox.Show("Proyectos de recorte generados", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub Button12_Click(sender As System.Object, e As System.EventArgs) Handles Button12.Click
        Dim iBucle As Integer
        ListBox2.Items.Clear()
        For iBucle = 1 To 50
            generate_template_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\Planimetrias\" & String.Format("{0:00}", iBucle) & ".gmw", iBucle, 2)
        Next
        MessageBox.Show("Proyectos de recorte generados", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub Button13_Click(sender As System.Object, e As System.EventArgs) Handles Button13.Click
        Dim iBucle As Integer
        ListBox2.Items.Clear()
        For iBucle = 1 To 50
            generate_template_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\Conjunta\" & String.Format("{0:00}", iBucle) & ".gmw", iBucle, 3)
        Next
        MessageBox.Show("Proyectos de recorte generados", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub Button14_Click(sender As System.Object, e As System.EventArgs) Handles Button14.Click
        Dim iBucle As Integer
        ListBox2.Items.Clear()
        For iBucle = 1 To 50
            generate_template_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\Edificacion\" & String.Format("{0:00}", iBucle) & ".gmw", iBucle, 5)
        Next
        MessageBox.Show("Proyectos de recorte generados", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub Button15_Click(sender As System.Object, e As System.EventArgs) Handles Button15.Click
        Dim iBucle As Integer
        ListBox2.Items.Clear()
        For iBucle = 1 To 50
            generate_template_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\Directorios\" & String.Format("{0:00}", iBucle) & ".gmw", iBucle, 4)
        Next
        MessageBox.Show("Proyectos de recorte generados", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub Button16_Click(sender As System.Object, e As System.EventArgs) Handles Button16.Click
        Dim iBucle As Integer = 28
        ListBox2.Items.Clear()
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2816900.gmw", 2816900)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2807905.gmw", 2807905)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2813401.gmw", 2813401)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2810600.gmw", 2810600)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2806600.gmw", 2806600)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2815200.gmw", 2815200)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2805701.gmw", 2805701)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2807915.gmw", 2807915)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2805301.gmw", 2805301)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2806500.gmw", 2806500)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2817800.gmw", 2817800)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2801300.gmw", 2801300)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2813000.gmw", 2813000)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2801700.gmw", 2801700)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2807909.gmw", 2807909)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2808901.gmw", 2808901)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2807907.gmw", 2807907)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2807500.gmw", 2807500)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2807908.gmw", 2807908)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2800800.gmw", 2800800)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2813402.gmw", 2813402)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2817600.gmw", 2817600)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2818101.gmw", 2818101)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2809600.gmw", 2809600)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2816700.gmw", 2816700)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2803600.gmw", 2803600)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2802600.gmw", 2802600)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2809100.gmw", 2809100)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2814800.gmw", 2814800)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2801400.gmw", 2801400)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2805000.gmw", 2805000)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2812302.gmw", 2812302)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2814000.gmw", 2814000)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2818100.gmw", 2818100)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2814700.gmw", 2814700)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2807910.gmw", 2807910)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2807901.gmw", 2807901)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2810400.gmw", 2810400)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2807906.gmw", 2807906)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2808600.gmw", 2808600)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2800500.gmw", 2800500)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2801500.gmw", 2801500)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2807904.gmw", 2807904)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2805400.gmw", 2805400)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2813400.gmw", 2813400)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2800600.gmw", 2800600)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2802201.gmw", 2802201)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2812900.gmw", 2812900)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2807200.gmw", 2807200)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2803000.gmw", 2803000)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2800400.gmw", 2800400)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2802202.gmw", 2802202)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2808000.gmw", 2808000)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2800200.gmw", 2800200)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2807913.gmw", 2807913)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2811501.gmw", 2811501)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2811500.gmw", 2811500)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2805300.gmw", 2805300)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2807902.gmw", 2807902)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2807911.gmw", 2807911)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2804500.gmw", 2804500)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2807912.gmw", 2807912)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2803200.gmw", 2803200)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2807300.gmw", 2807300)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2807914.gmw", 2807914)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2806501.gmw", 2806501)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2813100.gmw", 2813100)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2815000.gmw", 2815000)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2816100.gmw", 2816100)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2817601.gmw", 2817601)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2814100.gmw", 2814100)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2804000.gmw", 2804000)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2805700.gmw", 2805700)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2807903.gmw", 2807903)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2813200.gmw", 2813200)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2812301.gmw", 2812301)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2802200.gmw", 2802200)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2810700.gmw", 2810700)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2806601.gmw", 2806601)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2812700.gmw", 2812700)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2812201.gmw", 2812201)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2814900.gmw", 2814900)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2807900.gmw", 2807900)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2807400.gmw", 2807400)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2804900.gmw", 2804900)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2808900.gmw", 2808900)
        generate_templateHK_cutter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HK\2804700.gmw", 2804700)

        MessageBox.Show("Proyectos de recorte generados", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click

        Try
            Dim FormularioCreacion As New frmEdicion
            FormularioCreacion.MdiParent = MDIPrincipal
            FormularioCreacion.ModoTrabajo("SIMPLE", TextBox1.Text)
            FormularioCreacion.Show()
            FormularioCreacion.Visible = True
        Catch ex As Exception
            ModalError(ex.Message)
        End Try
    End Sub
End Class