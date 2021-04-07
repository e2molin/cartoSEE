Public Class ImportContornos

    Dim Cancelar As Boolean
    Dim rcdTmp As DataTable

    Private Sub developer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Size = New Point(634, 401)

        TextBox1.Text = ""
        TextBox2.Text = "23030"
        ListView1.FullRowSelect = True
        ListView1.GridLines = False
        ListView1.View = View.Details
        ListView1.SmallImageList = MDIPrincipal.ImageList2
        ListView1.Columns.Clear()
        ListView1.Items.Clear()
        ListView1.Columns.Add("Sellado", "Sellado", 100, HorizontalAlignment.Left, 4)
        ListView1.Columns.Add("Cerrado", "Cerrado", 100, HorizontalAlignment.Left, 4)
        ListView1.Columns.Add("Puntos", "Puntos", 100, HorizontalAlignment.Left, 4)
        ListView1.Columns.Add("Geometría", "Geometría", 0, HorizontalAlignment.Left, 4)
        ListView1.Columns.Add("SQL", "SQL", 0, HorizontalAlignment.Left, 4)

        rcdTmp = New DataTable
        If CargarDatatable("Select idtipodoc,tipodoc from tbtipodocumento", rcdTmp) = False Then
            MessageBox.Show("No se puede acceder a la tabla de Tipos de documentación", _
                            AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            ComboBox1.Items.Clear()
            For Each Filtro As DataRow In rcdTmp.Select
                ComboBox1.Items.Add(New itemData(Filtro.ItemArray(1).ToString, Filtro.ItemArray(0)))
            Next
        End If
        rcdTmp.Dispose()
        rcdTmp = Nothing


    End Sub


    Private Sub CargarContenidoDirectorio()
        If ComboBox1.SelectedIndex = -1 Then
            MessageBox.Show("Selecciona el tipo de documentación de los contornos", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        If TextBox1.Text = "" Then
            MessageBox.Show("Selecciona directorio de contornos", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        If Not System.IO.Directory.Exists(TextBox1.Text) Then
            MessageBox.Show("Directorio de contornos no localizado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        If TextBox2.Text = "" Then
            MessageBox.Show("Selecciona un sistema de referencia", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        If Not IsNumeric(TextBox2.Text) Then
            MessageBox.Show("Selecciona un sistema de referencia", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        Dim listaFicheros() As String
        Dim tipoDoc As String
        Dim linea As String
        Dim listaPuntos As ArrayList
        Dim coordenadas() As String
        Dim coordenada As Punto
        Dim numSellado As String
        Dim coordenadaIni As Punto

        Me.Cursor = Cursors.WaitCursor
        LanzarSpinner("Cargando datos")
        tipoDoc = CType(ComboBox1.SelectedItem, itemData).Valor
        listaFicheros = System.IO.Directory.GetFiles(TextBox1.Text, "*.gcp")
        Application.DoEvents()
        For Each fichero As String In listaFicheros
            listaPuntos = New ArrayList
            Using sr As New System.IO.StreamReader(fichero)
                linea = sr.ReadLine
                coordenadas = linea.Split(",")
                If coordenadas.Length >= 4 Then
                    coordenadaIni.X = CType(coordenadas(2).Replace(".", ","), Double)
                    coordenadaIni.Y = CType(coordenadas(3).Replace(".", ","), Double)
                End If
                Do While (Not linea Is Nothing)
                    coordenadas = linea.Split(",")
                    If coordenadas.Length >= 4 Then
                        coordenada.X = CType(coordenadas(2).Replace(".", ","), Double)
                        coordenada.Y = CType(coordenadas(3).Replace(".", ","), Double)
                        listaPuntos.Add(coordenada)
                    End If
                    linea = sr.ReadLine
                Loop
                listaPuntos.Add(coordenadaIni)
                numSellado = SacarFileDeRuta(fichero).ToString.Substring(0, 6)
                RellenarEntidad(listaPuntos, numSellado, True)
                sr.Close()
                sr.Dispose()
            End Using
        Next
        CerrarSpinner()
        Me.Cursor = Cursors.Default
        ToolStripStatusLabel1.Text = "Documentación: " & ComboBox1.Items(ComboBox1.SelectedIndex).ToString & ". Ficheros encontrados:" & ListView1.Items.Count
        MessageBox.Show("Proceso terminado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)



    End Sub

    Private Sub CargarContenidoFichero(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        Dim sellado As String
        Dim listaPuntos As ArrayList
        Dim coordenadas() As String
        Dim coordenada As Punto
        Dim closed As Boolean = False
        'Dim cadGeom As String
        'Dim cadSQL As String
        'Dim elementoLv As ListViewItem
        'Dim nPunto As Integer
        Dim tipoDoc As Integer

        If ComboBox1.SelectedIndex = -1 Then
            MessageBox.Show("Selecciona el tipo de documentación de los contornos", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        If TextBox1.Text = "" Then
            MessageBox.Show("Selecciona fichero de contornos", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        If RadioButton1.Checked = True Then
            If Not System.IO.File.Exists(TextBox1.Text) Then
                MessageBox.Show("Fichero de contornos no localizado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
        End If
        If RadioButton2.Checked = True Then
            If Not System.IO.Directory.Exists(TextBox1.Text) Then
                MessageBox.Show("Directorio de contornos no localizado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
        End If
        If TextBox2.Text = "" Then
            MessageBox.Show("Selecciona un sistema de referencia", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        If Not IsNumeric(TextBox2.Text) Then
            MessageBox.Show("Selecciona un sistema de referencia", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Me.Cursor = Cursors.WaitCursor
        ListView1.Items.Clear()
        tipoDoc = CType(ComboBox1.SelectedItem, itemData).Valor

        Using sr As New System.IO.StreamReader(TextBox1.Text)
            Dim linea As String
            linea = sr.ReadLine
            Do While (Not linea Is Nothing)
                If linea.Trim = "" Then
                    linea = sr.ReadLine
                    Continue Do
                End If
                If linea.ToUpper.Substring(0, 5) = "NAME=" Then
                    If Not listaPuntos Is Nothing Then
                        Application.DoEvents()
                        RellenarEntidad(listaPuntos, sellado, closed)
                    End If
                    sellado = linea.Replace("NAME=", "")
                    listaPuntos = New ArrayList
                    closed = False
                    linea = sr.ReadLine
                    Continue Do
                End If
                If linea = "CLOSED=YES" Then
                    closed = True
                    linea = sr.ReadLine
                    Continue Do
                End If
                If linea.IndexOf("=") < 0 Then
                    If linea.IndexOf(",") < 0 Then
                        coordenadas = linea.Split(" ")
                    Else
                        coordenadas = linea.Split(",")
                    End If
                    If coordenadas.Length = 3 Then
                        coordenada.X = CType(coordenadas(0).Replace(".", ","), Double)
                        coordenada.Y = CType(coordenadas(1).Replace(".", ","), Double)
                        listaPuntos.Add(coordenada)
                    End If
                End If
                linea = sr.ReadLine
            Loop
            If Not listaPuntos Is Nothing Then
                Application.DoEvents()
                RellenarEntidad(listaPuntos, sellado, closed)
            End If
            sr.Close()
            sr.Dispose()
        End Using

        Me.Cursor = Cursors.Default
        ToolStripStatusLabel1.Text = "Documentación: " & ComboBox1.Items(ComboBox1.SelectedIndex).ToString & ". Contornos encontrados:" & ListView1.Items.Count
        MessageBox.Show("Análisis preliminar terminado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Sub RellenarEntidad(ByVal listaPuntos As ArrayList, ByVal sellado As String, ByVal closed As Boolean)

        Dim cadGeom As String
        Dim cadSQL As String
        Dim elementoLv As ListViewItem
        Dim nPunto As Integer
        Dim tipoDoc As Integer
        Application.DoEvents()
        cadGeom = ""
        cadSQL = ""
        If listaPuntos.Count > 2 Then
            cadGeom = "MULTIPOLYGON((("
            nPunto = 0
            For Each punto As Punto In listaPuntos
                nPunto = nPunto + 1
                cadGeom = cadGeom & punto.X.ToString.Replace(",", ".") & " " & punto.Y.ToString.Replace(",", ".") & ","
                If nPunto = listaPuntos.Count - 1 Then Exit For
            Next
            Dim puntoFin = listaPuntos.Item(0)
            cadGeom = cadGeom & puntoFin.x.ToString.Replace(",", ".") & " " & puntoFin.Y.ToString.Replace(",", ".")
            cadGeom = cadGeom & ")))"
            tipoDoc = CType(ComboBox1.SelectedItem, itemData).Valor
            cadSQL = "INSERT INTO contornos (nombre,sellado,tipodoc,geom) VALUES (" & _
                    "'" & sellado & "'," & _
                    "'" & sellado.Substring(0, 6) & "'," & _
                    "" & tipoDoc & "," & _
                    "ST_GeomFromText('" & cadGeom & "'," & TextBox2.Text & "))"
        End If
        'Aquí creo la SQL de carga
        If sellado.Length <> 6 Then
            If sellado.Length = 9 And sellado.Substring(6, 1) = "_" Then
                'Es un Plano de población o un Plano de población en cuaderno
                Application.DoEvents()
            Else
                sellado = sellado & " (NO VALIDO)"
            End If
        ElseIf Not IsNumeric(sellado) Then
            sellado = sellado & " (NO VALIDO)"
        ElseIf sellado.IndexOf("%") > 0 Or sellado.IndexOf("*") > 0 Then
            sellado = sellado & " (NO VALIDO)"
        End If
        elementoLv = New ListViewItem
        elementoLv.Text = sellado
        elementoLv.SubItems.Add(IIf(Closed = True, "Cerrado", "Abierto"))
        elementoLv.SubItems.Add(listaPuntos.Count)
        elementoLv.SubItems.Add(cadGeom)
        elementoLv.SubItems.Add(cadSQL)
        If closed = True And sellado.IndexOf("(") < 0 Then
            elementoLv.ForeColor = Color.Green
        Else
            elementoLv.ForeColor = Color.Red
        End If
        ListView1.Items.Add(elementoLv)
        elementoLv = Nothing
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        If RadioButton1.Checked = True Then
            OpenFileDialog1.Title = "Selecciona fichero de contornos"
            OpenFileDialog1.Filter = "Archivo de puntos (*.xyz)|*.xyz"
            If OpenFileDialog1.ShowDialog <> Windows.Forms.DialogResult.OK Then Exit Sub
            TextBox1.Text = OpenFileDialog1.FileName
            CargarContenidoFichero(sender, e)
        End If
        If RadioButton2.Checked = True Then
            FolderBrowserDialog1.ShowNewFolderButton = True
            If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
            If System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath) = False Then Exit Sub
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
            CargarContenidoDirectorio()
        End If




    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

        If ListView1.Items.Count = 0 Then Exit Sub
        If MessageBox.Show("Sólo se insertarán contornos cerrados con el número de sellado válido." & _
                        System.Environment.NewLine & "¿Desea continuar?", AplicacionTitulo, MessageBoxButtons.YesNo, _
                        MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then Exit Sub

        Dim ListaSQL As New ArrayList
        Dim numContornos As Integer
        Dim Ejecucion As Boolean

        For Each linea As ListViewItem In ListView1.Items
            If linea.SubItems(1).Text = "Cerrado" And linea.Text.IndexOf("(") < 0 Then
                If CheckBox1.Checked = True Then
                    If linea.Text.ToString.Length = 6 Then
                        ListaSQL.Add("DELETE FROM contornos where sellado='" & linea.Text & "'")
                    ElseIf linea.Text.ToString.Length = 9 And linea.Text.ToString.Substring(6, 1) = "_" Then
                        ListaSQL.Add("DELETE FROM contornos where sellado='" & linea.Text.Substring(0, 6) & "'")
                    Else
                        MessageBox.Show("Número de sellado no identificado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    End If
                End If
            End If
        Next
        For Each linea As ListViewItem In ListView1.Items
            If linea.SubItems(1).Text = "Cerrado" And linea.Text.IndexOf("(") < 0 Then
                ListaSQL.Add(linea.SubItems(4).Text)
            End If
        Next



        If CheckBox1.Checked = True Then
            numcontornos = ListaSQL.Count / 2
        Else
            numcontornos = ListaSQL.Count
        End If
        If MessageBox.Show("Se insertarán " & numcontornos & " contornos. ¿Continuar?", _
                           AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then Exit Sub

        ListaSQL.Add("UPDATE contornos SET archivo_id=(SELECT idarchivo FROM archivo WHERE numdoc=contornos.sellado) " & _
                        "WHERE contornos.archivo_id is null")
        Me.Cursor = Cursors.WaitCursor
        LanzarSpinner("Cargando contornos...")
        Application.DoEvents()
        Ejecucion = ExeTran(ListaSQL)
        CerrarSpinner()
        If Ejecucion = True Then
            MessageBox.Show("Contornos actualizados correctamente en la base de datos.", _
                                AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("Problemas al insertar contornos en la base de datos. Contornos no introducidos.", _
                    AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        Me.Cursor = Cursors.Default

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click

        If ListView1.Items.Count = 0 Then
            MessageBox.Show("No hay contornos para exportar", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim numsellado As String
        Dim cadGeom As String
        Dim coordenadas() As String

        Dim sw As New System.IO.StreamWriter(My.Computer.FileSystem.SpecialDirectories.Desktop & "\contornos" & Now.Ticks & ".xyz")

        For Each elementoLv As ListViewItem In ListView1.Items
            numsellado = elementoLv.Text
            cadGeom = elementoLv.SubItems(3).Text.Replace("MULTIPOLYGON(((", "").Replace(")))", "")
            Application.DoEvents()
            sw.WriteLine("DESCRIPTION=Unknown Area Type")
            sw.WriteLine("NAME=" & numsellado)
            sw.WriteLine("CLOSED=YES")
            coordenadas = cadGeom.Split(",")
            For Each coordenada As String In coordenadas
                sw.WriteLine(coordenada & " -9999999")
            Next
            sw.WriteLine("")
        Next
        sw.Close()
        sw = Nothing



    End Sub

End Class
