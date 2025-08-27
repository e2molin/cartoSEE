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
        ToolStripStatusLabel1.Text = ""

    End Sub



    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click

        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
        If System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath) = False Then Exit Sub
        TextBox1.Text = FolderBrowserDialog1.SelectedPath
        lblInfoNumFiles.Text = "Ficheros GCP encontrados: " & My.Computer.FileSystem.GetFiles(
                                    FolderBrowserDialog1.SelectedPath,
                                    FileIO.SearchOption.SearchTopLevelOnly,
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
        For Each Archivo As String In My.Computer.FileSystem.GetFiles(
                                TextBox1.Text,
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

    Function getMunicipiosHTML(NombreMunis As String, codINEs As String) As String

        'urlplani=

        Dim partesCodMuni() As String = codINEs.Split("#")
        Dim partesNomMuni() As String = NombreMunis.Split("#")
        Dim urlBase As String = "http://centrodedescargas.cnig.es/CentroDescargas/buscar.do?" &
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


        Dim rutaTIFFInput As String = TextBox6.Text
        Dim rutaECWOutput As String = TextBox7.Text
        Dim rutaScriptGSW As String = $"{TextBox7.Text}\conversion.gms"
        Dim rutaPROJINPUT As String
        Dim rutaPROJOUTPUT As String
        Dim rutaSalida As String
        Dim ListaFicheros As New ArrayList
        Dim contador As Integer
        Try
            Using ofdIN As New OpenFileDialog With {
                        .Filter = "Ficheros de proyección (*.prj)|*.prj",
                        .Title = "Selecciona fichero de proyección de los datos de entrada"
            }
                If ofdIN.ShowDialog = DialogResult.OK Then rutaPROJINPUT = ofdIN.FileName : Else Exit Sub
            End Using
            Using ofdOUT As New OpenFileDialog With {
                        .Filter = "Ficheros de proyección (*.prj)|*.prj",
                        .Title = "Selecciona fichero de proyección de los datos de salida"
            }
                If ofdOUT.ShowDialog = DialogResult.OK Then rutaPROJOUTPUT = ofdOUT.FileName : Else Exit Sub
            End Using


            ToolStripStatusLabel1.Text = $"Analizando el directorio de entrada..."
            Application.DoEvents()
            Me.Cursor = Cursors.WaitCursor
            ' Obtener todos los archivos en el directorio y subdirectorios
            For Each archivo As String In IO.Directory.EnumerateFiles(rutaTIFFInput, "*.*", IO.SearchOption.AllDirectories)
                ' Aquí puedes procesar cada archivo
                If archivo.ToLower.EndsWith(".tif") Then ListaFicheros.Add(archivo)
            Next
            Me.Cursor = Cursors.Default
            If ModalQuestion($"Se han encontrado {ListaFicheros.Count} ficheros TIFF. ¿Generar script?") = DialogResult.No Then Exit Sub
            Me.Cursor = Cursors.WaitCursor

            Using sw As New System.IO.StreamWriter(rutaScriptGSW)
                sw.WriteLine("GLOBAL_MAPPER_SCRIPT VERSION=1.00 ENABLE_PROGRESS=YES")
                sw.WriteLine("UNLOAD_ALL")
                sw.WriteLine("SET_BG_COLOR COLOR=RGB(255,255,255)")
                For Each rutaFile As String In ListaFicheros
                    contador += 1
                    ToolStripStatusLabel1.Text = $"Procesando fichero nº {contador}"
                    Application.DoEvents()
                    rutaSalida = rutaFile.ToLower.Replace(rutaTIFFInput.ToLower, rutaECWOutput.ToLower)
                    rutaSalida = rutaSalida.Replace(".tif", ".ecw")
                    sw.WriteLine($"LOAD_PROJECTION FILENAME=""{rutaPROJINPUT}""")
                    sw.WriteLine($"IMPORT FILENAME=""{rutaFile}"" TYPE=AUTO ANTI_ALIAS=NO AUTO_CONTRAST=NO")
                    sw.WriteLine($"LOAD_PROJECTION FILENAME=""{rutaPROJOUTPUT}""")
                    sw.WriteLine($"EXPORT_RASTER FILENAME=""{rutaSalida}"" TYPE=ECW TARGET_COMPRESSION=1 GEN_WORLD_FILE=YES GEN_PRJ_FILE=YES")
                    sw.WriteLine("UNLOAD_ALL")
                    Try
                        If Not IO.File.Exists(SacarDirDeRuta(rutaSalida)) Then IO.Directory.CreateDirectory(SacarDirDeRuta(rutaSalida))
                    Catch ex As Exception
                        ModalError(ex.Message)
                        Application.DoEvents()
                    End Try
                    'If contador = 10 Then Exit For
                Next
                sw.Close()
                sw.Dispose()
                ToolStripStatusLabel1.Text = $"Ficheros: {contador}"
            End Using
        Catch ex As Exception
            ModalError(ex.Message)
            Application.DoEvents()
        End Try
        Me.Cursor = Cursors.Default

        ModalInfo("Proceso terminado")


    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click

        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
        If System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath) = False Then Exit Sub
        TextBox2.Text = FolderBrowserDialog1.SelectedPath

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click

        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
        If System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath) = False Then Exit Sub
        TextBox3.Text = FolderBrowserDialog1.SelectedPath

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click

        'TextBox2.Text = "D:\Volcados\planpobcuad"
        'TextBox3.Text = "D:\Volcados\planpobcuadOUT"
        'TextBox4.Text = "D:\Volcados\test_20030_to_25830\proj23030.prj"
        'TextBox5.Text = "D:\Volcados\test_20030_to_25830\proj25830.prj"


        Dim rutaECWInput As String = TextBox2.Text
        Dim rutaECWOutput As String = TextBox3.Text
        Dim rutaPROJ23030 As String = TextBox4.Text
        Dim rutaPROJ25830 As String = TextBox5.Text
        Dim rutaScriptGSW As String = $"{TextBox3.Text}\conversion.gms"
        Dim rutaSalida As String
        Dim ListaFicheros As New ArrayList
        Dim contador As Integer
        Try
            ToolStripStatusLabel1.Text = $"Analizando el directorio de entrada..."
            Application.DoEvents()
            Me.Cursor = Cursors.WaitCursor
            ' Obtener todos los archivos en el directorio y subdirectorios
            For Each archivo As String In IO.Directory.EnumerateFiles(rutaECWInput, "*.*", IO.SearchOption.AllDirectories)
                ' Aquí puedes procesar cada archivo
                If archivo.ToLower.EndsWith(".ecw") Then ListaFicheros.Add(archivo)
            Next
            Me.Cursor = Cursors.Default
            If ModalQuestion($"Se han encontrado {ListaFicheros.Count} ficeros ECW. ¿Generar script?") = DialogResult.No Then Exit Sub
            Me.Cursor = Cursors.WaitCursor

            Using sw As New System.IO.StreamWriter(rutaScriptGSW)
                sw.WriteLine("GLOBAL_MAPPER_SCRIPT VERSION=1.00 ENABLE_PROGRESS=YES")
                sw.WriteLine("UNLOAD_ALL")
                sw.WriteLine("SET_BG_COLOR COLOR=RGB(255,255,255)")
                For Each rutaFile As String In ListaFicheros
                    contador += 1
                    ToolStripStatusLabel1.Text = $"Procesando fichero nº {contador}"
                    Application.DoEvents()
                    rutaSalida = rutaFile.ToLower.Replace(rutaECWInput.ToLower, rutaECWOutput.ToLower)
                    sw.WriteLine($"LOAD_PROJECTION FILENAME=""{rutaPROJ23030}""")
                    sw.WriteLine($"IMPORT FILENAME=""{rutaFile}"" TYPE=AUTO ANTI_ALIAS=NO AUTO_CONTRAST=NO")
                    sw.WriteLine($"LOAD_PROJECTION FILENAME=""{rutaPROJ25830}""")
                    sw.WriteLine($"EXPORT_RASTER FILENAME=""{rutaSalida}"" TYPE=ECW TARGET_COMPRESSION=1 GEN_WORLD_FILE=YES GEN_PRJ_FILE=YES")
                    sw.WriteLine("UNLOAD_ALL")
                    Try
                        If Not IO.File.Exists(SacarDirDeRuta(rutaSalida)) Then IO.Directory.CreateDirectory(SacarDirDeRuta(rutaSalida))
                    Catch ex As Exception
                        ModalError(ex.Message)
                        Application.DoEvents()
                    End Try
                    'If contador = 10 Then Exit For
                Next
                sw.Close()
                sw.Dispose()
                ToolStripStatusLabel1.Text = $"Ficheros: {contador}"
            End Using
        Catch ex As Exception
            ModalError(ex.Message)
            Application.DoEvents()
        End Try
        Me.Cursor = Cursors.Default

        ModalInfo("Proceso terminado")



    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click, Button9.Click

        Dim openFileDialog1 As New OpenFileDialog()

        ' Configurar las propiedades del cuadro de diálogo
        openFileDialog1.InitialDirectory = "C:\" ' Establecer el directorio inicial
        openFileDialog1.Filter = "Archivos de proyecciones (*.prj)|*.prj|Todos los archivos (*.*)|*.*" ' Establecer el filtro de archivos
        openFileDialog1.FilterIndex = 1 ' Establecer el filtro predeterminado
        openFileDialog1.RestoreDirectory = True ' Restaurar el directorio al cerrar

        ' Mostrar el cuadro de diálogo y verificar si el usuario seleccionó un archivo
        If openFileDialog1.ShowDialog() = DialogResult.OK Then
            ' Obtener el nombre del archivo seleccionado
            If sender.name = "Button8" Then TextBox4.Text = openFileDialog1.FileName
            If sender.name = "Button9" Then TextBox5.Text = openFileDialog1.FileName

        End If

    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click

        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
        If System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath) = False Then Exit Sub
        TextBox6.Text = FolderBrowserDialog1.SelectedPath

    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click

        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
        If System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath) = False Then Exit Sub
        TextBox7.Text = FolderBrowserDialog1.SelectedPath

    End Sub
End Class