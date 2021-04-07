Public Class frmDevelIGN

    ''' <summary>
    ''' Reproyecta los puntos de control en el formato generado por Global Mapper 13
    ''' 23147.99579232,1585.87754462,-6.1861555556,43.6677916667,"Point 1",0.00
    ''' </summary>
    ''' <param name="cadIN"></param>
    ''' <param name="epsgIN"></param>
    ''' <param name="epsgOUT"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function reproyectarGCP(ByVal cadIN As String, ByVal epsgIN As Integer, ByVal epsgOUT As Integer) As String

        Dim parts() As String = cadIN.Split(",")
        Dim cadenaConv As String = ""

        ObtenerEscalar("select AsText(ST_Transform(GeomFromText('POINT(" & parts(2) & " " & parts(3) & ")'," & epsgIN & ")," & epsgOUT & ")) as Resultado", cadenaConv)
        Return parts(0) & "," & parts(1) & "," & cadenaConv.Replace("POINT(", "").Replace(")", "").Replace(" ", ",") & "," & parts(4) & "," & parts(5)


    End Function


    ''' <summary>
    ''' Extrae de una linea de un fichero FCP las coordenadas cartográficas
    ''' </summary>
    ''' <param name="cadIN"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function extractCoordenadas(ByVal cadIN As String) As String
        '23147.99579232,1585.87754462,-6.1861555556,43.6677916667,"Point 1",0.00
        Dim parts() As String = cadIN.Split(",")
        Dim cadenaConv As String = ""

        Return parts(2) & " " & parts(3)
        Application.DoEvents()
    End Function


    ''' <summary>
    ''' Aplica una traslación a las coordendas georreferenciadas de los puntos de control
    ''' </summary>
    ''' <param name="cadIN"></param>
    ''' <param name="deltaX"></param>
    ''' <param name="deltaY"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function trasladarGCP(ByVal cadIN As String, ByVal deltaX As Double, ByVal deltaY As Double) As String

        Dim parts() As String = cadIN.Split(",")
        Dim coo1 As Double
        Dim coo2 As Double
        coo1 = CType(parts(2).ToString.Replace(".", ","), Double) + deltaX
        coo2 = CType(parts(3).ToString.Replace(".", ","), Double) + deltaY

        Return parts(0) & "," & parts(1) & "," & coo1.ToString.Replace(",", ".") & "," & coo2.ToString.Replace(",", ".") & "," & parts(4) & "," & parts(5)


    End Function


    ''' <summary>
    ''' Comprueba que existan las rutas de la consulta de planos de población para el centro de descargas
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Test_cdd_planospoblacion_report()

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
        Me.Cursor = Cursors.WaitCursor
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
        Me.Cursor = Cursors.Default
        Erase filas
        docPP2CD.Clear()
        docPP2CD = Nothing
        MessageBox.Show("Proceso de verificación terminado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)



    End Sub


    ''' <summary>
    ''' Comprueba que existan las rutas de la consulta de planos de edificación para el centro de descargas
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
        Me.Cursor = Cursors.WaitCursor
        For Each fila As DataRow In filas
            contador = contador + 1
            ToolStripStatusLabel1.Text = "Procesando " & contador
            Application.DoEvents()
            rutaFile = fila.Item("rutaresource").ToString
            If Not System.IO.File.Exists(rutaFile.Replace("file:", "")) Then
                ListBox2.Items.Add("Fichero no localizado: " & rutaFile)
            End If
        Next
        Me.Cursor = Cursors.Default

        Erase filas
        docEdi2CD.Clear()
        docEdi2CD = Nothing
        MessageBox.Show("Proceso de verificación terminado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub


    ''' <summary>
    ''' Comprueba que existan las rutas de la consulta de minutas cartográficas para el centro de descargas
    ''' </summary>
    ''' <remarks></remarks>
    Sub Test_cdd_minutascartograficas_report()

        Dim docMinutas2CD As DataTable
        Dim filas() As DataRow
        docMinutas2CD = New DataTable
        Dim rutaFile As String
        Dim contador As Integer

        ListBox2.Items.Clear()
        If CargarDatatableMuni("select * from cdd_minutascarto_report", docMinutas2CD) = False Then
            MessageBox.Show("No se puede acceder a la vista de cdd_minutascarto_report", My.Application.Info.AssemblyName, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        filas = docMinutas2CD.Select
        contador = 0
        Me.Cursor = Cursors.WaitCursor
        For Each fila As DataRow In filas
            contador = contador + 1
            ToolStripStatusLabel1.Text = "Procesando " & contador
            Application.DoEvents()
            rutaFile = fila.Item("rutaresource").ToString
            If Not System.IO.File.Exists(rutaFile.Replace("file:", "")) Then
                ListBox2.Items.Add("Fichero no localizado: " & rutaFile)
            End If
        Next
        Me.Cursor = Cursors.Default

        Erase filas
        docMinutas2CD.Clear()
        docMinutas2CD = Nothing
        MessageBox.Show("Proceso de verificación terminado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub



    Sub Test_cdd_minutascartograficas_conversionScript()

        Dim docMinutas2CD As DataTable
        Dim filas() As DataRow
        docMinutas2CD = New DataTable
        Dim rutaFile As String
        Dim contador As Integer
        Dim ficheroSalida As String

        ListBox2.Items.Clear()
        If CargarDatatableMuni("select * from cdd_minutascarto_report order by numdoc", docMinutas2CD) = False Then
            MessageBox.Show("No se puede acceder a la vista de cdd_minutascarto_report", My.Application.Info.AssemblyName, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        filas = docMinutas2CD.Select
        contador = 0
        Me.Cursor = Cursors.WaitCursor
        Using sw As New System.IO.StreamWriter("d:\conversion.gms")
            sw.WriteLine("GLOBAL_MAPPER_SCRIPT VERSION=1.00 ENABLE_PROGRESS=YES")
            sw.WriteLine("UNLOAD_ALL")
            sw.WriteLine("SET_BG_COLOR COLOR=RGB(255,255,255)")

            For Each fila As DataRow In filas
                contador = contador + 1
                ToolStripStatusLabel1.Text = "Procesando " & contador
                Application.DoEvents()
                rutaFile = fila.Item("rutaresource").ToString
                If Not System.IO.File.Exists(rutaFile.Replace("file:", "")) Then
                    ListBox2.Items.Add("Fichero no localizado: " & rutaFile)
                Else
                    Application.DoEvents()
                    If fila.Item("extension").ToString = "ECW" Then
                        sw.WriteLine("IMPORT FILENAME=""" & rutaFile.Replace("file:", "") & """ TYPE=AUTO ANTI_ALIAS=NO AUTO_CONTRAST=NO")
                        ficheroSalida = "h:\25830\" & rutaFile.Replace("file://sbdignmad650/geodocat_ii/", "").Replace("/", "\")
                        Dim nombreDir As String
                        nombreDir = SacarDirDeRuta(ficheroSalida)
                        If Not System.IO.Directory.Exists(nombreDir) Then
                            System.IO.Directory.CreateDirectory(nombreDir)
                        End If
                        sw.WriteLine("LOAD_PROJECTION FILENAME=""G:\test25830.prj""")
                        sw.WriteLine("EXPORT_RASTER FILENAME=""" & ficheroSalida & """ TYPE=ECW TARGET_COMPRESSION=1 GEN_WORLD_FILE=YES GEN_PRJ_FILE=YES")
                        sw.WriteLine("UNLOAD_ALL")
                    End If
                End If
            Next
        End Using
        Me.Cursor = Cursors.Default

        Erase filas
        docMinutas2CD.Clear()
        docMinutas2CD = Nothing
        MessageBox.Show("Proceso de verificación terminado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)

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

    Private Sub procReproy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles procReproy.Click

        ListBox1.Items.Clear()
        Dim epsgIN As String = txtEPSGin.Text.Trim
        Dim epsgOUT As String = txtEPSGout.Text.Trim


        Dim newFolder As String = txtPathGCP.Text & "\GCP" & epsgOUT
        Dim sentencesOUT As ArrayList
        Dim lineIN As String
        Dim nomFich As String
        'Creamos la carpeta de salida si no existe
        If Not System.IO.Directory.Exists(newFolder) Then
            Try
                System.IO.Directory.CreateDirectory(newFolder)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
                Exit Sub
            End Try
        End If

        Me.Cursor = Cursors.WaitCursor
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

        Me.Cursor = Cursors.Default
        MessageBox.Show("Proceso terminado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub procTranslate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles procTranslate.Click

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


        Me.Cursor = Cursors.WaitCursor
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
                        sentencesOUT.Add(trasladarGCP(lineIN, deltaX, deltaY))
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
            MessageBox.Show(oe.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Me.Cursor = Cursors.Default
        MessageBox.Show("Proceso terminado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub frmDevelIGN_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lblInfoNumFiles.Text = ""
        cboTipoTextExist.Items.Add("Minutas cartográficas para el CDD")
        cboTipoTextExist.Items.Add("Planos de población para el CDD")
        cboTipoTextExist.Items.Add("Planos de edificación para el CDD")
        ToolStripStatusLabel1.Text = ""
    End Sub

    Private Sub btnTextExist_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTextExist.Click

        If cboTipoTextExist.Text = "Minutas cartográficas para el CDD" Then
            'Test_cdd_minutascartograficas_report()
            Test_cdd_minutascartograficas_conversionScript()
        ElseIf cboTipoTextExist.Text = "Planos de población para el CDD" Then
            Test_cdd_planospoblacion_report()
        ElseIf cboTipoTextExist.Text = "Planos de edificación para el CDD" Then
            Test_cdd_edificacion_report()
        End If

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

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

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click

        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
        If System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath) = False Then Exit Sub
        TextBox1.Text = FolderBrowserDialog1.SelectedPath
        lblInfoNumFiles.Text = "Ficheros GCP encontrados: " & My.Computer.FileSystem.GetFiles( _
                                    FolderBrowserDialog1.SelectedPath, _
                                    FileIO.SearchOption.SearchTopLevelOnly, _
                                    "*.gcp").Count

    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click

        ListBox1.Items.Clear()
        Dim epsgIN As String = txtEPSGin.Text.Trim
        Dim epsgOUT As String = txtEPSGout.Text.Trim


        Dim ficheroFusion As String = TextBox1.Text & "\fusion.xyz"
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
                                TextBox1.Text, _
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

    Function getMunicipiosHTML(NombreMunis As String, codINEs As String) As String

        'urlplani=

        Dim partesCodMuni() As String = codINEs.Split("#")
        Dim partesNomMuni() As String = NombreMunis.Split("#")
        Dim urlBase As String = "http://centrodedescargas.cnig.es/CentroDescargas/buscar.do?" & _
                                "filtro.checkCoord=N&filtro.codFamilia=MIPAC&filtro.codCA=&filtro.codProv=&filtro.nombreBis=Municipio&filtro.codIne=XXXXXXXXXXX&filtro.numeroHoja="
        Dim urlMuni As String
        Dim cadOUT As String = ""
        For ibucle As Integer = 0 To partesCodMuni.Count - 1
            urlMuni = urlBase.Replace("XXXXXXXXXXX", partesCodMuni(ibucle))
            If cadOUT = "" Then
                cadOUT = "<a href=""" & urlMuni & """ class=""linkcdd"" target=""_blank"">" & partesNomMuni(ibucle) & "</a><br/>"
                Continue For
            End If
            cadOUT = cadOUT & "<a href=""" & urlMuni & """ class=""linkcdd"" target=""_blank"">" & partesNomMuni(ibucle) & "</a><br/>"
        Next

        Return cadOUT
    End Function


    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click

        Dim docPP2CD As DataTable
        Dim filas() As DataRow
        docPP2CD = New DataTable
        Dim contador As Integer
        Dim procSQL As String
        Dim fechaDoc As Date
        procSQL = "SELECT archivo.idarchivo,archivo.numdoc,archivo.fechaprincipal,tbtipodocumento.tipodoc as Tipo," & _
                    "string_agg(distinct listamunicipios.nombre || ' (' || provincias.nombreprovincia || ')','#') as munisactual," & _
                    "string_agg(distinct '34' || to_char(provincias.comAutonoma_id, 'FM09'::text) || to_char(provincias.idprovincia, 'FM09'::text) || listamunicipios.inecorto,'#') as inecortocdd " & _
                    "FROM archivo " & _
                    "INNER JOIN tbtipodocumento on tbtipodocumento.idtipodoc=archivo.tipodoc_id " & _
                    "INNER JOIN archivo2munihisto  on archivo2munihisto.archivo_id=archivo.idarchivo " & _
                    "INNER JOIN munihisto on munihisto.idmunihisto= archivo2munihisto.munihisto_id " & _
                    "INNER JOIN ngmepschema.listamunicipios on listamunicipios.inecorto::integer= munihisto.cod_muni " & _
                    "INNER JOIN provincias on munihisto.provincia_id = provincias.idprovincia " & _
                    "WHERE tbtipodocumento.tipodoc in ('Planimetría','Altimetría','Conjunta') " & _
                    "group by idarchivo,numdoc,fechaprincipal,tbtipodocumento.tipodoc  order by numdoc"


        Try
            If CargarDatatableMuni(procSQL, docPP2CD) = False Then
                MessageBox.Show("No se puede acceder a los datos para el proceso", My.Application.Info.AssemblyName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            filas = docPP2CD.Select
            contador = 0
            Me.Cursor = Cursors.WaitCursor
            Using sw As New System.IO.StreamWriter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\script4templateWMS.sql", False)
                For Each fila As DataRow In filas
                    contador = contador + 1
                    ToolStripStatusLabel1.Text = "Procesando " & contador
                    Application.DoEvents()
                    fechaDoc = fila.Item("fechaprincipal")

                    sw.WriteLine("UPDATE minutas_cdd SET tipodoc=E'" & fila.Item("Tipo").ToString.Replace("'", "\'") & "'," & _
                                        "fechadoc='" & FormatearFecha(fila.Item("fechaprincipal"), "GERMAN") & "'," & _
                                        "municipioshtml=E'" & getMunicipiosHTML(fila.Item("munisactual").ToString, fila.Item("inecortocdd").ToString).Replace("'", "\'") & "' " & _
                                        "WHERE sellado='" & fila.Item("numdoc").ToString & "';")
                Next



            End Using
           
            Me.Cursor = Cursors.Default



        Catch ex As Exception
            MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            Erase filas
            docPP2CD.Dispose()
            docPP2CD = Nothing
        End Try


        MessageBox.Show("Proceso terminado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)


    End Sub


End Class