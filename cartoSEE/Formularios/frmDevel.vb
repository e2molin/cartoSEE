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
            For Each Archivo As String In My.Computer.FileSystem.GetFiles(
                                    txtPathGCP.Text,
                                    FileIO.SearchOption.SearchTopLevelOnly,
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
        lblInfoNumFiles.Text = "Ficheros GCP encontrados: " & My.Computer.FileSystem.GetFiles(
                                    FolderBrowserDialog1.SelectedPath,
                                    FileIO.SearchOption.SearchTopLevelOnly,
                                    "*.gcp").Count



    End Sub

    Private Sub frmDevel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        txtPathGCP.Text = ""
        lblInfoNumFiles.Text = ""
        txtEPSGin.Text = "4230"
        txtEPSGout.Text = "25830"

        GroupBox3.Text = "Transferencia de ficheros georreferenciados por sellado a estructura GEODOCAT II"
        TextBox2.Text = "\\sbdignmad651.ign.fomento.es\ARCHIVO\ARCHIVO_Técnico\_000_PROYECTO_BORRADOR_DE_NUEVA_DISPOSICION_DE_CARPETAS\01_PROYECTOS\01_20260406_PLANIMETRIAS\23_JAEN\Georref"

        ' 1. Crear una instancia del control ToolTip
        Dim miToolTip As New ToolTip()

        ' 2. (Opcional) Configurar propiedades generales del ToolTip
        miToolTip.AutoPopDelay = 5000  ' Tiempo que permanece visible (en milisegundos)
        miToolTip.InitialDelay = 500   ' Tiempo que tarda en aparecer tras pasar el ratón
        miToolTip.ReshowDelay = 100    ' Tiempo de reacción al pasar de un control a otro
        miToolTip.ShowAlways = True    ' Mostrar incluso si la ventana no está activa


        Dim descripBtn As String
        descripBtn = "Transforma una estructura de georreferenciación de documentos agrupados por sellado a la estructura que necesita Geodocat II"
        miToolTip.SetToolTip(Button24, descripBtn)






    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

        'Vamos a comprobar que todo documento de Archivo con provincia_id e municipiohistorico1_id distinto de cero,
        'tienen georreferenciado al menos un ECW En el caso de los planos de población , puede tener más de 1
        Dim sqlBase As String
        Dim rcdTest As DataTable
        Dim pathProv As String
        Dim pathMuni As String
        Dim rutaECW23030 As String
        Dim rutaECW25830 As String
        Dim pathECW23030 As String
        Dim pathECW25830 As String
        Dim fileECW23030 As String
        Dim fileECW25830 As String
        Dim lastFile23030 As String
        Dim lastFile25830 As String
        Dim Existe_23030 As Boolean
        Dim Existe_25830 As Boolean
        Dim numgeoFiles23030 As Integer
        Dim numgeoFiles25830 As Integer
        Dim contDoc As Integer

        Using sw As New System.IO.StreamWriter($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\georef_report.txt", False, System.Text.Encoding.Unicode)
            sw.WriteLine($"idArchivo;Sellado;Provincia;Municipios;Tipo;Subtipo;epsg23030;epsg25830;Fichero23030;Fichero25830;NumgeoFiles23030;NumgeoFiles25830")
            For iBucle = 1 To 50
                ToolStripStatusLabel1.Text = $"Procesando idprovincia: {iBucle}"
                Application.DoEvents()
                sqlBase = $"SELECT archivo.idarchivo,archivo.numdoc,tbtipodocumento.tipodoc as tipo,coalesce(archivo.subtipo,'') as subtipo,archivo.tipodoc_id,
		                    string_agg(to_char(territorios.munihisto, 'FM0000009'::text),'#') as listaCodMuniHisto,string_agg(territorios.nombre,', ') as listaMuniHisto,
                                                string_agg(territorios.nombre,', ') as listaMuniHisto,string_agg(distinct provincias.nombreprovincia,', ') as nomprovin
                                        FROM bdsidschema.archivo 
	                                        LEFT JOIN bdsidschema.tbtipodocumento ON tbtipodocumento.idtipodoc=archivo.tipodoc_id 
	                                        LEFT JOIN bdsidschema.archivo2territorios  ON archivo2territorios.archivo_id=archivo.idarchivo 
	                                        LEFT JOIN bdsidschema.territorios on territorios.idterritorio= archivo2territorios.territorio_id 
	                                        LEFT JOIN bdsidschema.provincias on territorios.provincia= provincias.idprovincia 
                                        WHERE archivo.provincia_id= {iBucle}
                                          group by archivo.idarchivo,archivo.numdoc,tbtipodocumento.tipodoc,archivo.subtipo,archivo.tipodoc_id order by numdoc"
                'WHERE archivo.numdoc in ('180591','180592','180593','180594','180595','180596')
                'WHERE archivo.provincia_id= {iBucle}






                Try
                    rcdTest = New DataTable
                    If CargarRecordset(sqlBase, rcdTest) Then
                        For Each reg As DataRow In rcdTest.Select
                            contDoc += 1
                            ToolStripStatusLabel2.Text = $"Fichero: {contDoc}"
                            Application.DoEvents()
                            Existe_23030 = False
                            Existe_25830 = False
                            pathMuni = ""
                            fileECW23030 = ""
                            fileECW25830 = ""
                            pathECW23030 = ""
                            pathECW25830 = ""
                            numgeoFiles23030 = 0
                            numgeoFiles25830 = 0
                            'If reg("numdoc").ToString = "010030" Then
                            '    Application.DoEvents()
                            'End If
                            For Each codMuniHisto As String In reg("listaCodMuniHisto").ToString.Split("#")
                                'pathProv = String.Format("{0:00}", CType(reg("provincia_id").ToString, Integer))
                                If codMuniHisto.Length <> 7 Then
                                    sw.WriteLine($"{reg("idarchivo")};{reg("numdoc")};{reg("nomprovin")};{reg("listaMuniHisto")};{reg("tipo")};{reg("subtipo")};;;;;{numgeoFiles23030};{numgeoFiles25830}")
                                    Continue For
                                End If
                                pathProv = codMuniHisto.Substring(0, 2)
                                pathMuni = codMuniHisto
                                fileECW23030 = $"{reg("numdoc")}.ecw"
                                fileECW25830 = $"{reg("numdoc")}.ecw"
                                pathECW23030 = $"{rutaRepoGeorrefBase}\epsg23030\{DirRepoProvinciaByTipodoc(CType(reg("tipodoc_id").ToString, Integer))}\{pathProv}\{pathMuni}\"
                                pathECW25830 = $"{rutaRepoGeorrefBase}\epsg25830\{DirRepoProvinciaByTipodoc(CType(reg("tipodoc_id").ToString, Integer))}\{pathProv}\{pathMuni}\"
                                rutaECW23030 = $"{rutaRepoGeorrefBase}\epsg23030\{DirRepoProvinciaByTipodoc(CType(reg("tipodoc_id").ToString, Integer))}\{pathProv}\{pathMuni}\{fileECW23030}"
                                rutaECW25830 = $"{rutaRepoGeorrefBase}\epsg25830\{DirRepoProvinciaByTipodoc(CType(reg("tipodoc_id").ToString, Integer))}\{pathProv}\{pathMuni}\{fileECW25830}"

                                If Not Existe_23030 Then
                                    If IO.File.Exists(rutaECW23030) Then
                                        Existe_23030 = True
                                        lastFile23030 = fileECW23030
                                        numgeoFiles23030 = 1
                                    End If
                                End If
                                If Not Existe_25830 Then
                                    If IO.File.Exists(rutaECW25830) Then
                                        Existe_25830 = True
                                        lastFile25830 = fileECW25830
                                        numgeoFiles25830 = 1
                                    End If

                                End If

                                If Not Existe_23030 Then
                                    'Probamos si hay TIFF
                                    fileECW23030 = $"{reg("numdoc")}.tif"
                                    pathECW23030 = $"{rutaRepoGeorrefBase}\epsg23030\{DirRepoProvinciaByTipodoc(CType(reg("tipodoc_id").ToString, Integer))}\{pathProv}\{pathMuni}\"
                                    rutaECW23030 = $"{rutaRepoGeorrefBase}\epsg23030\{DirRepoProvinciaByTipodoc(CType(reg("tipodoc_id").ToString, Integer))}\{pathProv}\{pathMuni}\{fileECW23030}"
                                    If IO.File.Exists(rutaECW23030) Then
                                        Existe_23030 = True
                                        lastFile23030 = fileECW23030
                                        numgeoFiles23030 = 1
                                    End If
                                    If Not IO.File.Exists(rutaECW23030) Then
                                        'Pruebo una última vez por si existe con sufijo _geo
                                        fileECW23030 = $"{reg("numdoc")}_geo.tif"
                                        pathECW23030 = $"{rutaRepoGeorrefBase}\epsg23030\{DirRepoProvinciaByTipodoc(CType(reg("tipodoc_id").ToString, Integer))}\{pathProv}\{pathMuni}\"
                                        rutaECW23030 = $"{rutaRepoGeorrefBase}\epsg23030\{DirRepoProvinciaByTipodoc(CType(reg("tipodoc_id").ToString, Integer))}\{pathProv}\{pathMuni}\{fileECW23030}"
                                        If IO.File.Exists(rutaECW23030) Then
                                            Existe_23030 = True
                                            lastFile23030 = fileECW23030
                                            numgeoFiles23030 = 1
                                        End If
                                    End If
                                End If



                                If Not Existe_25830 Then
                                    'Probamos si hay TIFF
                                    fileECW25830 = $"{reg("numdoc")}.tif"
                                    pathECW25830 = $"{rutaRepoGeorrefBase}\epsg25830\{DirRepoProvinciaByTipodoc(CType(reg("tipodoc_id").ToString, Integer))}\{pathProv}\{pathMuni}\"
                                    rutaECW25830 = $"{rutaRepoGeorrefBase}\epsg25830\{DirRepoProvinciaByTipodoc(CType(reg("tipodoc_id").ToString, Integer))}\{pathProv}\{pathMuni}\{fileECW25830}"
                                    If IO.File.Exists(rutaECW25830) Then
                                        Existe_25830 = True
                                        lastFile25830 = fileECW25830
                                        numgeoFiles25830 = 1
                                    End If
                                    If Not IO.File.Exists(rutaECW25830) Then
                                        'Pruebo una última vez por si existe con sufijo _geo
                                        fileECW25830 = $"{reg("numdoc")}_geo.tif"
                                        pathECW25830 = $"{rutaRepoGeorrefBase}\epsg25830\{DirRepoProvinciaByTipodoc(CType(reg("tipodoc_id").ToString, Integer))}\{pathProv}\{pathMuni}\"
                                        rutaECW25830 = $"{rutaRepoGeorrefBase}\epsg25830\{DirRepoProvinciaByTipodoc(CType(reg("tipodoc_id").ToString, Integer))}\{pathProv}\{pathMuni}\{fileECW25830}"
                                        If IO.File.Exists(rutaECW25830) Then
                                            Existe_25830 = True
                                            lastFile25830 = fileECW25830
                                            numgeoFiles25830 = 1
                                        End If
                                    End If
                                End If


                                If Not Existe_23030 Then
                                    For contFich As Integer = 1 To 20
                                        fileECW23030 = $"{reg("numdoc")}_{String.Format("{0:00}", contFich)}.ecw"
                                        pathECW23030 = $"{rutaRepoGeorrefBase}\epsg23030\{DirRepoProvinciaByTipodoc(CType(reg("tipodoc_id").ToString, Integer))}\{pathProv}\{pathMuni}\"
                                        rutaECW23030 = $"{rutaRepoGeorrefBase}\epsg23030\{DirRepoProvinciaByTipodoc(CType(reg("tipodoc_id").ToString, Integer))}\{pathProv}\{pathMuni}\{fileECW23030}"
                                        If IO.File.Exists(rutaECW23030) Then
                                            numgeoFiles23030 = contFich
                                            lastFile23030 = fileECW23030
                                            Existe_23030 = True
                                        End If
                                    Next
                                End If
                                If Not Existe_25830 Then
                                    For contFich As Integer = 1 To 20
                                        fileECW25830 = $"{reg("numdoc")}_{String.Format("{0:00}", contFich)}.ecw"
                                        pathECW25830 = $"{rutaRepoGeorrefBase}\epsg25830\{DirRepoProvinciaByTipodoc(CType(reg("tipodoc_id").ToString, Integer))}\{pathProv}\{pathMuni}\"
                                        rutaECW25830 = $"{rutaRepoGeorrefBase}\epsg25830\{DirRepoProvinciaByTipodoc(CType(reg("tipodoc_id").ToString, Integer))}\{pathProv}\{pathMuni}\{fileECW25830}"
                                        If IO.File.Exists(rutaECW25830) Then
                                            numgeoFiles25830 = contFich
                                            lastFile25830 = fileECW25830
                                            Existe_25830 = True
                                        End If
                                    Next
                                End If
                                If Existe_25830 And Existe_23030 Then Exit For
                            Next

                            sw.WriteLine($"{reg("idarchivo")};{reg("numdoc")};{reg("nomprovin")};{reg("listaMuniHisto")};{reg("tipo")};{reg("subtipo")};{IIf(Existe_23030, pathECW23030, "")};{IIf(Existe_25830, pathECW25830, "")};{IIf(Existe_23030, lastFile23030, "")};{IIf(Existe_25830, lastFile25830, "")};{numgeoFiles23030};{numgeoFiles25830}")

                        Next
                        Application.DoEvents()
                    End If
                Catch ex As Exception
                    ModalError(ex.Message)
                Finally
                    rcdTest.Dispose()
                    rcdTest = Nothing
                End Try
            Next

            sw.Close()
            sw.Dispose()
        End Using


        ModalInfo("Proceso terminado")



        'Dim RutaDOC As String
        'Dim contador As Integer

        'ReDim Rutas(0)

        ''Si no hay acceso al repositorio Base, directamente salgo
        'If System.IO.Directory.Exists(rutaRepoGeorref) = False Then Exit Function

        ''Comprobamos el más simple de todos
        'Dim Munis() As String = Documento.MunicipiosINE.Split("#")
        'For Each Muni As String In Munis
        '    RutaDOC = rutaRepoGeorref &
        '                     "\" & DirRepoProvinciaByTipodoc(Documento.CodTipo) &
        '                     "\" & Muni.Substring(0, 2) &
        '                     "\" & Muni &
        '                    "\" & Documento.Sellado & ".ecw"
        '    If System.IO.File.Exists(RutaDOC) = True Then
        '        Rutas(0) = RutaDOC
        '        Return True
        '    End If
        'Next
        ''El nombre del documento es compuesto
        'ReDim Rutas(20)
        'For Each Muni As String In Munis
        '    RutaDOC = rutaRepoGeorref &
        '                     "\" & DirRepoProvinciaByTipodoc(Documento.CodTipo) &
        '                     "\" & Muni.Substring(0, 2) &
        '                     "\" & Muni &
        '                    "\" & Documento.Sellado & "_01.ecw"
        '    If System.IO.File.Exists(RutaDOC) = True Then
        '        Rutas(0) = RutaDOC
        '        contador = 0
        '        Do Until contador = 20
        '            contador = contador + 1
        '            RutaDOC = rutaRepoGeorref &
        '                             "\" & DirRepoProvinciaByTipodoc(Documento.CodTipo) &
        '                             "\" & Muni.Substring(0, 2) &
        '                             "\" & Muni &
        '                            "\" & Documento.Sellado & "_" & String.Format("{000}", contador + 1) & ".ecw"

        '            If System.IO.File.Exists(RutaDOC) = True Then
        '                Rutas(contador) = RutaDOC
        '            Else
        '                Exit Do
        '            End If
        '        Loop
        '        ReDim Preserve Rutas(contador - 1)
        '        Exit For
        '    End If
        'Next





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
            If Not System.IO.File.Exists(rutaFile.Replace("file", "")) Then
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


        cadsql = "SELECT archivo.numdoc, tbtipodocumento.tipodoc, " &
                "to_char(min(munihisto.cod_munihisto), 'FM0000009'::text) AS codinehisto, to_char(min(munihisto.cod_muni), 'FM00009'::text) AS codigoine, " &
                "date_part('year'::text, archivo.fechaprincipal) AS anyo, " &
                "((((((('file://sbdignmad650/geodocat_ii/'::text || tbtipodocumento.dirrepo::text) || '/'::text) || " &
                "substring(to_char(archivo.municipiohistorico1_id, 'FM0000009'::text), 0, 3)) || '/'::text) || " &
                "to_char(archivo.municipiohistorico1_id, 'FM0000009'::text)) || '/'::text) || contornos.nombre::text) || '.ecw'::text AS rutaresource, 'ECW' AS extension, archivo.provincia_id AS provin " &
                "FROM contornos " &
                "INNER JOIN archivo ON archivo.idarchivo = contornos.archivo_id " &
                "INNER JOIN provincias ON provincias.idprovincia = archivo.provincia_id " &
                "INNER JOIN tbtipodocumento ON archivo.tipodoc_id = tbtipodocumento.idtipodoc " &
                "INNER JOIN archivo2munihisto ON archivo.idarchivo = archivo2munihisto.archivo_id " &
                "INNER JOIN munihisto ON archivo2munihisto.munihisto_id = munihisto.idmunihisto " &
                "WHERE (archivo.tipodoc_id = 7) AND archivo.municipiohistorico1_id <> 0 " &
                "GROUP BY archivo.numdoc, tbtipodocumento.tipodoc, archivo.fechaprincipal, provincias.dirrepo, tbtipodocumento.dirrepo, archivo.provincia_id, archivo.municipiohistorico1_id, contornos.nombre"

        cadsql = "SELECT archivo.numdoc, tbtipodocumento.tipodoc, to_char(min(munihisto.cod_munihisto), 'FM0000009'::text) AS codinehisto, to_char(min(munihisto.cod_muni), 'FM00009'::text) AS codigoine, " &
                "date_part('year'::text, archivo.fechaprincipal) AS anyo, ((((((('file://sbdignmad650/geodocat_ii/'::text || tbtipodocumento.dirrepo::text) || " &
                "'/'::text) || substring(to_char(archivo.municipiohistorico1_id, 'FM0000009'::text), 0, 3)) || '/'::text) || to_char(archivo.municipiohistorico1_id, 'FM0000009'::text)) || '/'::text) || " &
                "contornos.nombre::text) || '.ecw'::text AS rutaresource,'ECW' AS extension, archivo.provincia_id AS provin " &
                "FROM contornos " &
                "INNER JOIN archivo ON archivo.idarchivo = contornos.archivo_id " &
                "INNER JOIN provincias ON provincias.idprovincia = archivo.provincia_id " &
                "INNER JOIN tbtipodocumento ON archivo.tipodoc_id = tbtipodocumento.idtipodoc " &
                "INNER JOIN archivo2munihisto ON archivo.idarchivo = archivo2munihisto.archivo_id " &
                "INNER JOIN munihisto ON archivo2munihisto.munihisto_id = munihisto.idmunihisto " &
                "WHERE (archivo.tipodoc_id = 6) AND archivo.municipiohistorico1_id <> 0 " &
                "GROUP BY archivo.numdoc, tbtipodocumento.tipodoc, archivo.fechaprincipal, provincias.dirrepo, tbtipodocumento.dirrepo, archivo.provincia_id, archivo.municipiohistorico1_id, contornos.nombre"
        cadsql = "SELECT archivo.numdoc, tbtipodocumento.tipodoc, to_char(min(munihisto.cod_munihisto), 'FM0000009'::text) AS codinehisto, to_char(min(munihisto.cod_muni), 'FM00009'::text) AS codigoine, " &
                "date_part('year'::text, archivo.fechaprincipal) AS anyo, ((((((('file://sbdignmad650/geodocat_ii/'::text || tbtipodocumento.dirrepo::text) || " &
                "'/'::text) || substring(to_char(archivo.municipiohistorico1_id, 'FM0000009'::text), 0, 3)) || '/'::text) || to_char(archivo.municipiohistorico1_id, 'FM0000009'::text)) || '/'::text) || " &
                "contornos.nombre::text) || '.ecw'::text AS rutaresource,'ECW' AS extension, archivo.provincia_id AS provin " &
                "FROM contornos " &
                "INNER JOIN archivo ON archivo.idarchivo = contornos.archivo_id " &
                "INNER JOIN provincias ON provincias.idprovincia = archivo.provincia_id " &
                "INNER JOIN tbtipodocumento ON archivo.tipodoc_id = tbtipodocumento.idtipodoc " &
                "INNER JOIN archivo2munihisto ON archivo.idarchivo = archivo2munihisto.archivo_id " &
                "INNER JOIN munihisto ON archivo2munihisto.munihisto_id = munihisto.idmunihisto " &
                "WHERE (archivo.tipodoc_id = 5) AND archivo.municipiohistorico1_id <> 0 " &
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
                LineaOUT = "EXPORT_RASTER FILENAME=""" &
                            rutaFile.Replace("file:", "").Replace("/", "\").Replace("\\sbdignmad650\geodocat_ii", "g:\25830") &
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
    Sub generate_conversionFile_planospoblacionEnCuaderno_report()

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


        cadsql = "SELECT archivo.numdoc, tbtipodocumento.tipodoc, " &
                "to_char(min(munihisto.cod_munihisto), 'FM0000009'::text) AS codinehisto, to_char(min(munihisto.cod_muni), 'FM00009'::text) AS codigoine, " &
                "date_part('year'::text, archivo.fechaprincipal) AS anyo, " &
                "((((((('file://sbdignmad650/geodocat_ii/'::text || tbtipodocumento.dirrepo::text) || '/'::text) || " &
                "substring(to_char(archivo.municipiohistorico1_id, 'FM0000009'::text), 0, 3)) || '/'::text) || " &
                "to_char(archivo.municipiohistorico1_id, 'FM0000009'::text)) || '/'::text) || contornos.nombre::text) || '.ecw'::text AS rutaresource, 'ECW' AS extension, archivo.provincia_id AS provin " &
                "FROM contornos " &
                "INNER JOIN archivo ON archivo.idarchivo = contornos.archivo_id " &
                "INNER JOIN provincias ON provincias.idprovincia = archivo.provincia_id " &
                "INNER JOIN tbtipodocumento ON archivo.tipodoc_id = tbtipodocumento.idtipodoc " &
                "INNER JOIN archivo2munihisto ON archivo.idarchivo = archivo2munihisto.archivo_id " &
                "INNER JOIN munihisto ON archivo2munihisto.munihisto_id = munihisto.idmunihisto " &
                "WHERE (archivo.tipodoc_id = 7) AND archivo.municipiohistorico1_id <> 0 " &
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
                LineaOUT = "EXPORT_RASTER FILENAME=""" &
                            rutaFile.Replace("file:", "").Replace("/", "\").Replace("\\sbdignmad650\geodocat_ii", "g:\25830") &
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


        cadsql = "SELECT archivo.numdoc, tbtipodocumento.tipodoc, " &
                "to_char(min(munihisto.cod_munihisto), 'FM0000009'::text) AS codinehisto, to_char(min(munihisto.cod_muni), 'FM00009'::text) AS codigoine, " &
                "date_part('year'::text, archivo.fechaprincipal) AS anyo, " &
                "((((((('file://sbdignmad650/geodocat_ii/'::text || tbtipodocumento.dirrepo::text) || '/'::text) || " &
                "substring(to_char(archivo.municipiohistorico1_id, 'FM0000009'::text), 0, 3)) || '/'::text) || " &
                "to_char(archivo.municipiohistorico1_id, 'FM0000009'::text)) || '/'::text) || contornos.nombre::text) || '.ecw'::text AS rutaresource, 'ECW' AS extension, archivo.provincia_id AS provin " &
                "FROM contornos " &
                "INNER JOIN archivo ON archivo.idarchivo = contornos.archivo_id " &
                "INNER JOIN provincias ON provincias.idprovincia = archivo.provincia_id " &
                "INNER JOIN tbtipodocumento ON archivo.tipodoc_id = tbtipodocumento.idtipodoc " &
                "INNER JOIN archivo2munihisto ON archivo.idarchivo = archivo2munihisto.archivo_id " &
                "INNER JOIN munihisto ON archivo2munihisto.munihisto_id = munihisto.idmunihisto " &
                "WHERE (archivo.tipodoc_id = 7) AND archivo.municipiohistorico1_id <> 0 " &
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
                LineaOUT = "EXPORT_RASTER FILENAME=""" &
                            rutaFile.Replace("file:", "").Replace("/", "\").Replace("\\sbdignmad650\geodocat_ii", "g:\25830") &
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


    Sub generate_conversionFile_conjuntas_report()

        Dim docPP2CD As DataTable
        Dim filas() As DataRow
        Dim contador As Integer
        Dim LineaOUT As String
        Dim cadsql As String
        docPP2CD = New DataTable

        Dim pathGMSFile As String
        Dim pathProcFile As String
        Dim pathProcess As String

        Try
            pathProcess = $"{txtFolderPath.Text}"
            pathGMSFile = $"{txtFolderPath.Text}\conjuntas\conversionConjuntas.gms"
            pathProcFile = $"{txtFolderPath.Text}\conjuntas\conversionConjuntas_mkdir.bat"
            If Not IO.Directory.Exists($"{txtFolderPath.Text}\conjuntas\") Then
                IO.Directory.CreateDirectory($"{txtFolderPath.Text}\conjuntas\")
            End If
        Catch ex As Exception
            ModalError(ex.Message)
        End Try


        Dim swConversion As New System.IO.StreamWriter(pathGMSFile, False, System.Text.Encoding.Unicode)
        swConversion.WriteLine("GLOBAL_MAPPER_SCRIPT VERSION=1.00 ENABLE_PROGRESS=YES")
        swConversion.WriteLine("UNLOAD_ALL")
        swConversion.WriteLine("SET_BG_COLOR COLOR=RGB(255, 255, 255)")

        cadsql = $"SELECT archivo.idarchivo,archivo.numdoc,tbtipodocumento.tipodoc as tipo,coalesce(archivo.subtipo,'') as subtipo,archivo.tipodoc_id,
		                    string_agg(to_char(territorios.munihisto, 'FM0000009'::text),'#') as listaCodMuniHisto,string_agg(territorios.nombre,', ') as listaMuniHisto,
                                                string_agg(territorios.nombre,', ') as listaMuniHisto,string_agg(distinct provincias.nombreprovincia,', ') as nomprovin
                                        FROM bdsidschema.archivo 
	                                        LEFT JOIN bdsidschema.tbtipodocumento ON tbtipodocumento.idtipodoc=archivo.tipodoc_id 
	                                        LEFT JOIN bdsidschema.archivo2territorios  ON archivo2territorios.archivo_id=archivo.idarchivo 
	                                        LEFT JOIN bdsidschema.territorios on territorios.idterritorio= archivo2territorios.territorio_id 
	                                        LEFT JOIN bdsidschema.provincias on territorios.provincia= provincias.idprovincia 
                                        WHERE archivo.tipodoc_id = 3
                                          group by archivo.idarchivo,archivo.numdoc,tbtipodocumento.tipodoc,archivo.subtipo,archivo.tipodoc_id order by numdoc"

        ListBox2.Items.Clear()
        If CargarDatatableMuni(cadsql, docPP2CD) = False Then
            ModalError("No se puede acceder a los datos de BADASID")
            Exit Sub
        End If
        filas = docPP2CD.Select
        contador = 0
        Dim pathProv As String
        Dim fileECW23030 As String
        Dim pathECW23030 As String
        Dim rutaECW23030 As String
        Dim filegeoFound As Integer = 0

        For Each fila As DataRow In filas
            contador += 1
            ToolStripStatusLabel1.Text = "Procesando " & contador
            Application.DoEvents()
            filegeoFound = 0
            For Each codmunihisto As String In fila("listaCodMuniHisto").ToString.Split("#")
                pathProv = codmunihisto.Substring(0, 2)
                fileECW23030 = $"{fila("numdoc")}.ecw"
                pathECW23030 = $"{rutaRepoGeorrefBase}\epsg23030\{DirRepoProvinciaByTipodoc(CType(fila("tipodoc_id").ToString, Integer))}\{pathProv}\{codmunihisto}\"
                rutaECW23030 = $"{rutaRepoGeorrefBase}\epsg23030\{DirRepoProvinciaByTipodoc(CType(fila("tipodoc_id").ToString, Integer))}\{pathProv}\{codmunihisto}\{fileECW23030}"
                If IO.File.Exists(rutaECW23030) Then
                    LineaOUT = $"IMPORT FILENAME=""{rutaECW23030}"" TYPE=AUTO ANTI_ALIAS=NO AUTO_CONTRAST=NO"
                    swConversion.WriteLine(LineaOUT)
                    LineaOUT = $"LOAD_PROJECTION FILENAME=""{pathProcess}\test25830.prj"""
                    swConversion.WriteLine(LineaOUT)
                    LineaOUT = $"EXPORT_RASTER FILENAME=""{txtFolderPath.Text}\conjuntas\epsg25830\CONJUNTAS\{pathProv}\{codmunihisto}\{fila("numdoc")}.ecw"" TYPE=ECW TARGET_COMPRESSION=1 GEN_WORLD_FILE=YES GEN_PRJ_FILE=YES"
                    swConversion.WriteLine(LineaOUT)
                    swConversion.WriteLine("UNLOAD_ALL")
                    Try
                        If Not IO.Directory.Exists($"{txtFolderPath.Text}\conjuntas\epsg25830\CONJUNTAS\{pathProv}\{codmunihisto}\") Then
                            IO.Directory.CreateDirectory($"{txtFolderPath.Text}\conjuntas\epsg25830\CONJUNTAS\{pathProv}\{codmunihisto}\")
                        End If
                    Catch ex As Exception
                        ModalError(ex.Message)
                    End Try
                    filegeoFound += 1
                End If
            Next
            If filegeoFound = 0 Then
                ListBox2.Items.Add($"No encuentro la georreferenciación en 23030 de la conjunta {fila("numdoc")}")
            End If
        Next

        Erase filas
        docPP2CD.Clear()
        docPP2CD = Nothing
        swConversion.Close()
        swConversion.Dispose()
        swConversion = Nothing
        ModalInfo("Fichero de conversión generado")



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

        cadsql = "SELECT archivo.numdoc, tbtipodocumento.tipodoc, " &
                "to_char(min(munihisto.cod_munihisto), 'FM0000009'::text) AS codinehisto, to_char(min(munihisto.cod_muni), 'FM00009'::text) AS codigoine, " &
                "date_part('year'::text, archivo.fechaprincipal) AS anyo, " &
                "((((((('file://sbdignmad650/geodocat_ii/'::text || tbtipodocumento.dirrepo::text) || '/'::text) || " &
                "substring(to_char(archivo.municipiohistorico1_id, 'FM0000009'::text), 0, 3)) || '/'::text) || " &
                "to_char(archivo.municipiohistorico1_id, 'FM0000009'::text)) || '/'::text) || contornos.nombre::text) || '.ecw'::text AS rutaresource, 'ECW' AS extension, archivo.provincia_id AS provin " &
                "FROM contornos " &
                "INNER JOIN archivo ON archivo.idarchivo = contornos.archivo_id " &
                "INNER JOIN provincias ON provincias.idprovincia = archivo.provincia_id " &
                "INNER JOIN tbtipodocumento ON archivo.tipodoc_id = tbtipodocumento.idtipodoc " &
                "INNER JOIN archivo2munihisto ON archivo.idarchivo = archivo2munihisto.archivo_id " &
                "INNER JOIN munihisto ON archivo2munihisto.munihisto_id = munihisto.idmunihisto " &
                "WHERE (archivo.tipodoc_id = " & tipodoc & ") AND archivo.municipiohistorico1_id <> 0 and archivo.provincia_id=" & codprovin & " " &
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

        cadsql = "SELECT archivo.numdoc, tbtipodocumento.tipodoc, " &
                "to_char(min(munihisto.cod_munihisto), 'FM0000009'::text) AS codinehisto, to_char(min(munihisto.cod_muni), 'FM00009'::text) AS codigoine, " &
                "date_part('year'::text, archivo.fechaprincipal) AS anyo, " &
                "((((((('file://sbdignmad650/geodocat_ii/'::text || tbtipodocumento.dirrepo::text) || '/'::text) || " &
                "substring(to_char(archivo.municipiohistorico1_id, 'FM0000009'::text), 0, 3)) || '/'::text) || " &
                "to_char(archivo.municipiohistorico1_id, 'FM0000009'::text)) || '/'::text) || contornos.nombre::text) || '.ecw'::text AS rutaresource, 'ECW' AS extension, archivo.provincia_id AS provin " &
                "FROM contornos " &
                "INNER JOIN archivo ON archivo.idarchivo = contornos.archivo_id " &
                "INNER JOIN provincias ON provincias.idprovincia = archivo.provincia_id " &
                "INNER JOIN tbtipodocumento ON archivo.tipodoc_id = tbtipodocumento.idtipodoc " &
                "INNER JOIN archivo2munihisto ON archivo.idarchivo = archivo2munihisto.archivo_id " &
                "INNER JOIN munihisto ON archivo2munihisto.munihisto_id = munihisto.idmunihisto " &
                "WHERE (archivo.tipodoc_id = 6) AND archivo.municipiohistorico1_id <> 0 " &
                "GROUP BY archivo.numdoc, tbtipodocumento.tipodoc, archivo.fechaprincipal, provincias.dirrepo, tbtipodocumento.dirrepo, archivo.provincia_id, archivo.municipiohistorico1_id, contornos.nombre " &
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
        For Each Archivo As String In My.Computer.FileSystem.GetFiles(
                                txtPathGCP.Text,
                                FileIO.SearchOption.SearchTopLevelOnly,
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

    Private Sub Button18_Click(sender As Object, e As EventArgs) Handles Button18.Click

        Dim rutaECWInput As String = "D:\Volcados\planpobcuad"
        Dim rutaECWOutput As String = "D:\Volcados\planpobcuadOUT"
        Dim rutaPROJ23030 As String = "D:\Volcados\test_20030_to 25830\proj23030.prj"
        Dim rutaPROJ25830 As String = "D:\Volcados\test_20030_to 25830\proj25830.prj"
        Dim rutaScriptGSW As String = "D:\Volcados\test_20030_to 25830\conversion.gms"
        Dim rutaSalida As String
        Dim ListaFicheros As New ArrayList
        Dim contador As Integer
        Try
            ' Obtener todos los archivos en el directorio y subdirectorios
            For Each archivo As String In IO.Directory.EnumerateFiles(rutaECWInput, "*.*", IO.SearchOption.AllDirectories)
                ' Aquí puedes procesar cada archivo
                Application.DoEvents()
                If archivo.ToLower.EndsWith(".ecw") Then ListaFicheros.Add(archivo)
            Next
            Application.DoEvents()



            Using sw As New System.IO.StreamWriter(rutaScriptGSW)
                sw.WriteLine("GLOBAL_MAPPER_SCRIPT VERSION=1.00 ENABLE_PROGRESS=YES")
                sw.WriteLine("UNLOAD_ALL")
                sw.WriteLine("SET_BG_COLOR COLOR=RGB(255,255,255)")

                For Each rutaFile As String In ListaFicheros
                    contador += 1
                    rutaSalida = rutaFile.ToLower.Replace(rutaECWInput.ToLower, rutaECWOutput.ToLower)
                    sw.WriteLine($"LOAD_PROJECTION FILENAME=""{rutaPROJ23030}""")
                    sw.WriteLine($"IMPORT FILENAME=""{rutaFile}"" TYPE=AUTO ANTI_ALIAS=NO AUTO_CONTRAST=NO")
                    sw.WriteLine($"LOAD_PROJECTION FILENAME=""{rutaPROJ25830}""")
                    sw.WriteLine($"EXPORT_RASTER FILENAME=""{rutaSalida}"" TYPE=ECW TARGET_COMPRESSION=1 GEN_WORLD_FILE=YES GEN_PRJ_FILE=YES")
                    sw.WriteLine("UNLOAD_ALL")
                    Try
                        If Not System.IO.File.Exists(SacarDirDeRuta(rutaSalida)) Then
                            IO.Directory.CreateDirectory(SacarDirDeRuta(rutaSalida))
                        End If
                        Application.DoEvents()
                    Catch ex As Exception
                        Application.DoEvents()
                    End Try
                    If contador = 10 Then Exit For
                Next
                sw.Close()
                sw.Dispose()
            End Using






            'For Each rutaFile As String In ListaFicheros
            '    contador = contador + 1
            '    ToolStripStatusLabel1.Text = "Procesando " & contador
            '    Application.DoEvents()
            '    If Not System.IO.File.Exists(rutaFile.Replace("file:", "")) Then
            '        ListBox2.Items.Add("Fichero no localizado: " & rutaFile)
            '    Else
            '        Application.DoEvents()
            '        LineaOUT = "IMPORT FILENAME=""" & rutaFile & """ TYPE=AUTO ANTI_ALIAS=NO AUTO_CONTRAST=NO"
            '        swConversion.WriteLine(LineaOUT)
            '        LineaOUT = "LOAD_PROJECTION FILENAME=""G:\test25830.prj"""
            '        swConversion.WriteLine(LineaOUT)
            '        LineaOUT = "EXPORT_RASTER FILENAME=""" &
            '                    rutaFile.Replace("file:", "").Replace("/", "\").Replace("\\sbdignmad650\geodocat_ii", "g:\25830") &
            '                    """ TYPE=ECW TARGET_COMPRESSION=1 GEN_WORLD_FILE=YES GEN_PRJ_FILE=YES"
            '        swConversion.WriteLine(LineaOUT)
            '        swConversion.WriteLine("UNLOAD_ALL")
            '        swMkdir.WriteLine("mkdir " & SacarDirDeRuta(rutaFile.Replace("file:", "").Replace("/", "\").Replace("\\sbdignmad650\geodocat_ii", "g:\25830")))
            '    End If
            '    Application.DoEvents()

            'Next




            ' Alternativamente, puedes usar GetFiles, pero puede ser menos eficiente para directorios grandes
            ' Dim archivos() As String = Directory.GetFiles(rutaDirectorio, "*.*", SearchOption.AllDirectories)
            ' For Each archivo As String In archivos
            '     Console.WriteLine(archivo)
            ' Next

        Catch ex As Exception
            Console.WriteLine($"Error al iterar el directorio: {ex.Message}")
        End Try


        ModalInfo("proceso terminado")



    End Sub


    Private Sub Button10_Click(sender As System.Object, e As System.EventArgs) Handles Button10.Click

        Dim iBucle As Integer
        For iBucle = 1 To 50
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



    Private Sub Button19_Click(sender As Object, e As EventArgs) Handles Button19.Click
        generate_conversionFile_conjuntas_report()
    End Sub

    Private Sub Button20_Click(sender As Object, e As EventArgs) Handles Button20.Click

        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
        If System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath) = False Then Exit Sub
        txtFolderPath.Text = FolderBrowserDialog1.SelectedPath


    End Sub

    Private Sub Button21_Click(sender As Object, e As EventArgs) Handles Button21.Click

        Using sw As New System.IO.StreamWriter($"{txtFolderPath.Text}\DocsNoGeorref.txt")
            For Each elem As Object In ListBox2.Items
                sw.WriteLine(elem.ToString())
            Next
        End Using

        ModalInfo("Fichero generado")



    End Sub

    Private Sub TextBox2_LostFocus(sender As Object, e As EventArgs) Handles TextBox2.LostFocus
        TextBox2.BackColor = Color.White
    End Sub

    Private Sub TextBox2_GotFocus(sender As Object, e As EventArgs) Handles TextBox2.GotFocus
        TextBox2.BackColor = Color.LightYellow
    End Sub

    Private Sub TextBox3_LostFocus(sender As Object, e As EventArgs) Handles TextBox3.LostFocus
        TextBox3.BackColor = Color.White
    End Sub

    Private Sub TextBox3_GotFocus(sender As Object, e As EventArgs) Handles TextBox3.GotFocus
        TextBox3.BackColor = Color.LightYellow
    End Sub

    Private Sub Button24_Click(sender As Object, e As EventArgs) Handles Button24.Click

        Dim lstFiles As New ArrayList
        ListBox3.Items.Clear()
        ToolStripStatusLabel1.Text = "Transferencia a formato GEODOCAT II"
        ToolStripStatusLabel2.Text = ""
        Try
            ' 1. Agregamos los archivos encontrados en la carpeta actual al ListBox
            For Each archivo As String In IO.Directory.EnumerateFiles(TextBox2.Text, "*.*", IO.SearchOption.AllDirectories)
                If IO.Path.GetExtension(archivo).ToLower() = ".ecw" Then lstFiles.Add(archivo.ToLower())
            Next

        Catch ex As UnauthorizedAccessException
            ' Si no hay permisos para una carpeta del sistema, la salta de forma segura
            Application.DoEvents()
        Catch ex As Exception
            Debug.WriteLine("Error: " & ex.Message)
        End Try
        If lstFiles.Count = 0 Then
            ModalExclamation("No se han encontrado ficheros ecw")
            Exit Sub
        Else
            If ModalQuestion($"Se van a intentar transferir {lstFiles.Count} archivos a la  estructura de GEODOCAT II.¿Continuar?") <> DialogResult.Yes Then Exit Sub

        End If
        'Ahora vamos a interntar formatear el fichero encontrado
        Application.DoEvents()
        Dim pathOrigen As String
        Dim nombreFile As String
        Dim nombreDestino As String
        Dim codMuniHistoList As String
        Dim pathSalida As String
        Dim contadorCopyFiles As Integer = 0
        Dim copySuccess As Integer = 0
        For Each archivo As String In lstFiles
            contadorCopyFiles += 1
            ToolStripStatusLabel2.Text = $"Procesando {contadorCopyFiles} de {lstFiles.Count}"
            Application.DoEvents()
            codMuniHistoList = ""
            pathOrigen = archivo
            nombreFile = IO.Path.GetFileName(archivo)
            nombreDestino = nombreFile.Replace("_modified", "").Replace("_original", "").Replace("_corr", "").Replace("plani", "").Replace("cc", "").Replace("_pp", "")
            If nombreDestino.StartsWith("00") Then
                nombreDestino = nombreDestino.Substring(2, nombreDestino.Length - 2)
            End If

            If System.Text.RegularExpressions.Regex.IsMatch(nombreDestino, "^\d{6}_\d{2}\.ecw$", System.Text.RegularExpressions.RegexOptions.IgnoreCase) Then
                'Se trata de un fichero con el formato XXXXXX_YY.ecw. Es válido. El número de sellado es XXXXXX
                Application.DoEvents()
            ElseIf System.Text.RegularExpressions.Regex.IsMatch(nombreDestino, "^\d{6}_\d{1}\.ecw$", System.Text.RegularExpressions.RegexOptions.IgnoreCase) Then
                'Se trata de un fichero con el formato XXXXXX_Y.ecw. No es válido, pero lo apañamos. El número de sellado es XXXXXX
                nombreDestino = nombreDestino.Replace("_", "_0")
            ElseIf System.Text.RegularExpressions.Regex.IsMatch(nombreDestino, "^\d{6}\.ecw$", System.Text.RegularExpressions.RegexOptions.IgnoreCase) Then
                'Se trata de un fichero con el formato XXXXXX.ecw. Es válido. El número de sellado es XXXXXX
                Application.DoEvents()
            Else
                'Si no tiene el formato esperado, el fichero se devuelve
                ListBox3.Items.Add($"{nombreFile}: formato no esperado")
                Continue For
            End If

            Application.DoEvents()
            codMuniHistoList = GetCodMuniHistoFromSellado(nombreDestino.Substring(0, 6))
            If codMuniHistoList = "" Then
                ' Si no obtenemos el municipìo histórico asociado, se devuelve el fichero
                ListBox3.Items.Add($"{nombreFile}")
                Continue For
            End If
            For Each codMuniHisto In codMuniHistoList.Split("#")
                pathSalida = $"{TextBox3.Text}\{codMuniHisto.Substring(0, 2)}\{codMuniHisto}\"
                Exit For ' Cogemos sólo el primero, para el caso de que haya más de uno
            Next
            Try
                If Not IO.Directory.Exists(pathSalida) Then
                    IO.Directory.CreateDirectory(pathSalida)
                End If
                If CheckBox1.Checked Then
                    IO.File.Move(pathOrigen, $"{pathSalida}{nombreDestino}")
                Else
                    IO.File.Copy(pathOrigen, $"{pathSalida}{nombreDestino}")
                End If

                copySuccess += 1
                'Ahora buscamos si existe el fichero de puntos asociado
                If IO.File.Exists(pathOrigen.Replace("_modified", "").Replace(".ecw", ".jpg.points")) Then
                    If CheckBox1.Checked Then
                        IO.File.Move(pathOrigen.Replace("_modified", "").Replace(".ecw", ".jpg.points"), $"{pathSalida}{nombreDestino.Replace(".ecw", ".jpg.points")}")
                    Else
                        IO.File.Copy(pathOrigen.Replace("_modified", "").Replace(".ecw", ".jpg.points"), $"{pathSalida}{nombreDestino.Replace(".ecw", ".jpg.points")}")
                    End If
                Else
                    ListBox3.Items.Add($"{nombreFile}: Falta GCP")
                End If

            Catch ex As Exception
                ListBox3.Items.Add($"{nombreFile}: Error en copia: {ex.Message}")
                ModalError(ex.Message)
            End Try

        Next

        ModalInfo($"Transferencia terminada. Ficheros encontrados: {lstFiles.Count}. Ficheros copiados: {copySuccess}")


    End Sub

    Private Sub Button22_Click(sender As Object, e As EventArgs) Handles Button22.Click

        Using fbd As New FolderBrowserDialog()
            fbd.Description = "Selecciona la carpeta que deseas procesar"
            fbd.ShowNewFolderButton = False ' Desactiva la opción de crear carpetas nuevas
            fbd.RootFolder = Environment.SpecialFolder.MyComputer ' Carpeta de inicio (Este Equipo)
            If fbd.ShowDialog() = DialogResult.OK Then
                TextBox2.Text = fbd.SelectedPath
            End If
        End Using

    End Sub

    Private Sub Button23_Click(sender As Object, e As EventArgs) Handles Button23.Click

        Using fbd As New FolderBrowserDialog()
            fbd.Description = "Selecciona la carpeta donde seseas mover los datos"
            fbd.ShowNewFolderButton = True ' Desactiva la opción de crear carpetas nuevas
            fbd.RootFolder = Environment.SpecialFolder.MyComputer ' Carpeta de inicio (Este Equipo)
            If fbd.ShowDialog() = DialogResult.OK Then
                TextBox3.Text = fbd.SelectedPath
            End If
        End Using

    End Sub

    Private Sub Button25_Click(sender As Object, e As EventArgs) Handles Button25.Click

        Using sw As New System.IO.StreamWriter($"{TextBox3.Text}\DocsNoCopiados.txt")
            For Each elem As Object In ListBox3.Items
                sw.WriteLine(elem.ToString())
            Next
        End Using

        ModalInfo("Fichero de incidencias generado")

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click

        Dim lstFiles As New ArrayList
        ListBox3.Items.Clear()
        ToolStripStatusLabel1.Text = "Transformación de Numdoc a formato GEODOCAT II"
        ToolStripStatusLabel2.Text = ""
        Try
            ' 1. Agregamos los archivos encontrados en la carpeta actual al ListBox
            For Each archivo As String In IO.Directory.EnumerateFiles(TextBox2.Text, "*.*", IO.SearchOption.AllDirectories)
                lstFiles.Add(archivo.ToLower())
            Next

        Catch ex As UnauthorizedAccessException
            ' Si no hay permisos para una carpeta del sistema, la salta de forma segura
            Application.DoEvents()
        Catch ex As Exception
            Debug.WriteLine("Error: " & ex.Message)
        End Try
        If lstFiles.Count = 0 Then
            ModalExclamation("No se han encontrado ficheros ecw")
            Exit Sub
        Else
            If ModalQuestion($"Se van a intentar transferir {lstFiles.Count} archivos a la  estructura de GEODOCAT II.¿Continuar?") <> DialogResult.Yes Then Exit Sub

        End If
        'Ahora vamos a interntar formatear el fichero encontrado
        Dim pathOrigen As String
        Dim nombreFile As String
        Dim nombreDestino As String
        Dim codMuniHistoList As String
        Dim pathSalida As String
        Dim contadorCopyFiles As Integer = 0
        Dim copySuccess As Integer = 0
        For Each archivo As String In lstFiles
            contadorCopyFiles += 1
            ToolStripStatusLabel2.Text = $"Procesando {contadorCopyFiles} de {lstFiles.Count}"
            Application.DoEvents()
            codMuniHistoList = ""
            pathOrigen = archivo
            nombreFile = IO.Path.GetFileName(archivo)
            nombreDestino = nombreFile.Replace("_modified", "").Replace("_original", "").Replace("_corr", "").Replace("plani", "").Replace("cc", "").Replace("_pp", "")
            If nombreDestino.StartsWith("00") Then
                nombreDestino = nombreDestino.Substring(2, nombreDestino.Length - 2)
            End If

            Application.DoEvents()
            codMuniHistoList = GetCodMuniHistoFromSellado(nombreDestino.Substring(0, 6))
            If codMuniHistoList = "" Then
                ' Si no obtenemos el municipìo histórico asociado, se devuelve el fichero
                ListBox3.Items.Add($"MUnicipio no localizado: {nombreFile}")
                Continue For
            End If
            For Each codMuniHisto In codMuniHistoList.Split("#")
                pathSalida = $"{TextBox3.Text}\{codMuniHisto.Substring(0, 2)}\{codMuniHisto}\"
                Exit For ' Cogemos sólo el primero, para el caso de que haya más de uno
            Next
            Try
                If Not IO.Directory.Exists(pathSalida) Then
                    IO.Directory.CreateDirectory(pathSalida)
                End If
                If CheckBox1.Checked Then
                    IO.File.Move(pathOrigen, $"{pathSalida}{nombreDestino}")
                Else
                    IO.File.Copy(pathOrigen, $"{pathSalida}{nombreDestino}")
                End If

            Catch ex As Exception
                ListBox3.Items.Add($"{nombreFile}: Error en copia: {ex.Message}")
                ModalError(ex.Message)
            End Try

        Next

        ModalInfo($"Transferencia terminada. Ficheros encontrados: {lstFiles.Count}. Ficheros copiados: {copySuccess}")


    End Sub
End Class