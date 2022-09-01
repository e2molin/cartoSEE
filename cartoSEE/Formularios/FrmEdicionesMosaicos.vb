Public Class FrmEdicionesMosaicos

    Dim rcdMosaicos As DataView

    Private Sub FrmEdicionesMosaicos_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        rcdMosaicos.Dispose()
        rcdMosaicos = Nothing

    End Sub

    Private Sub FrmEdicionesMosaicos_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Size = New Point(775, 400)
        DataGridView1.Size = New Point(630, 256)
        DataGridView1.Location = New Point(12, 60)
        ToolStripStatusLabel2.Text = ""
        GroupBox1.Size = New Point(630, 256)
        GroupBox1.Location = New Point(12, 60)
        GroupBox1.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Bottom Or AnchorStyles.Right
        GroupBox1.Visible = False

        Button2.Visible = False
        Button3.Visible = False

    End Sub

    Sub CargarDatos()

        ResizeLV()
        RellenarDataview()

    End Sub


    Sub ResizeLV()

        ListView2.Columns.Clear()
        ListView2.View = View.Details

        ListView2.FullRowSelect = True
        ListView2.SmallImageList = MDIPrincipal.ImageList2
        ListView2.Columns.Add("Documento", "Documento", 250, HorizontalAlignment.Left, 0)
        ListView2.Columns.Add("Fichero", "Fichero", 0, HorizontalAlignment.Left, 0)
        ListView2.Columns.Add("GeoFichero", "GeoFichero", 0, HorizontalAlignment.Left, 0)

    End Sub


    Sub RellenarDataview()

        rcdMosaicos = New DataView
        If CargarDataView("SELECT iddocdigital,titulo,coleccion,rutamosaico,codmuni FROM bdsidschema.docdigital", rcdMosaicos) = False Then
            MessageBox.Show("No se pueden cargar los mosaicos", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        DataGridView1.DataSource = rcdMosaicos
        DataGridView1.Columns(0).Visible = False
        DataGridView1.Columns(1).Width = 340
        DataGridView1.Columns(1).HeaderText = "Nombre del mosaico"
        DataGridView1.Columns(2).Width = 230
        DataGridView1.Columns(2).HeaderText = "Tipo de mosaico"
        DataGridView1.Columns(3).Visible = False
        DataGridView1.Columns(4).Visible = False
        DataGridView1.RowsDefaultCellStyle.BackColor = Color.White
        DataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue
        DataGridView1.Sort(DataGridView1.Columns(0), System.ComponentModel.ListSortDirection.Ascending)
        ToolStripStatusLabel1.Text = "Elementos: " & DataGridView1.RowCount

    End Sub

    Sub RellenarCampos()

        Dim Registros() As DataRow
        Dim resultSET As DataTable
        Dim cadSQL As String
        Dim elementoLV As ListViewItem
        Dim iRegistro As Integer

        Me.Cursor = Cursors.WaitCursor
        TextBox4.Enabled = False

        iRegistro = DataGridView1.Item(0, DataGridView1.CurrentCell.RowIndex).Value.ToString
        TextBox1.Text = DataGridView1.Item(1, DataGridView1.CurrentCell.RowIndex).Value.ToString
        TextBox2.Text = DataGridView1.Item(2, DataGridView1.CurrentCell.RowIndex).Value.ToString
        TextBox3.Text = DataGridView1.Item(3, DataGridView1.CurrentCell.RowIndex).Value.ToString
        TextBox5.Text = DataGridView1.Item(4, DataGridView1.CurrentCell.RowIndex).Value.ToString
        Button6.Tag = rutaRepoGeorref & "\MOSAICOS_DIGITALES\28\" & _
                            DataGridView1.Item(3, DataGridView1.CurrentCell.RowIndex).Value.ToString

        'Ahora Relleno LV con las imágenes que forman el mosaico
        ListView2.Items.Clear()
        resultSET = New DataTable
        cadSQL = "SELECT archivo.idarchivo,archivo.numdoc,archivo.subdivision,tbtipodocumento.tipodoc,tbtipodocumento.idtipodoc  " &
                "FROM bdsidschema.archivo INNER JOIN bdsidschema.docdigital2archivo ON " &
                "archivo.idarchivo=docdigital2archivo.archivo_id " &
                "INNER JOIN bdsidschema.tbtipodocumento ON archivo.tipodoc_id = tbtipodocumento.idtipodoc " &
                "WHERE docdigital2archivo.docdigital_id=" & iRegistro & " " &
                "ORDER BY archivo.subdivision"
        CargarRecordset(cadSQL, resultSET)
        Registros = resultSET.Select
        For Each Registro As DataRow In Registros
            elementoLV = New ListViewItem
            elementoLV.Text = Registro.Item("tipodoc").ToString & " " & _
                                Registro.Item("subdivision").ToString & _
                                " - " & Registro.Item("numdoc").ToString
            elementoLV.SubItems.Add(rutaRepo & IIf(CalidadFavorita = "Alta", "\_Scan400\", "\_Scan250\") & _
                                    DirRepoProvinciaByINE(Registro.Item("numdoc").ToString.Substring(0, 2)) & _
                                    "\" & Registro.Item("numdoc").ToString & ".jpg")
            elementoLV.SubItems.Add(rutaRepoGeorref & "\" & _
                        DirRepoProvinciaByTipodoc(Registro.Item("idtipodoc").ToString) & "\" & _
                        Registro.Item("numdoc").ToString.Substring(0, 2) & "\" & _
                        DataGridView1.Item(4, DataGridView1.CurrentCell.RowIndex).Value.ToString & _
                        "\" & Registro.Item("numdoc").ToString & ".ecw")
            If iRegistro Mod 2 = 0 Then
                elementoLV.BackColor = Color.White
            Else
                elementoLV.BackColor = Color.WhiteSmoke
            End If
            elementoLV.Tag = Registro.Item("idarchivo").ToString
            ListView2.Items.Add(elementoLV)
            elementoLV = Nothing
        Next
        Label4.Text = "Documentos en el mosaico: " & ListView2.Items.Count
        Erase Registros
        Me.Cursor = Cursors.Default



    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        If DataGridView1.RowCount = 0 Then Exit Sub
        GroupBox1.Visible = True
        RellenarCampos()

    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click

        DataGridView1.Visible = True
        GroupBox1.Visible = False
        TextBox4.Enabled = True

    End Sub

    Sub Funciones(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click, Button3.Click, Button4.Click

        Dim rutaOrigen As String
        Dim rutaDestino As String
        Dim copiados As Integer
        Dim cadenaLog As String
        Dim Hayfallos As Boolean = False

        If ListView2.SelectedItems.Count = 0 Then
            MessageBox.Show("Seleccione al menos una hoja", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim dlgOpcionExport As New OptionDialog()
        Dim TipoExport As String = ""
        Dim NombreTipoExport As String = ""
        If dlgOpcionExport.ShowDialog = Windows.Forms.DialogResult.OK Then
            dlgOpcionExport.CheckRadioButtonResult(TipoExport, NombreTipoExport)
        Else
            Exit Sub
        End If
        If TipoExport = "" Then Exit Sub

        FolderBrowserDialog1.ShowNewFolderButton = True
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
        If System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath) = False Then Exit Sub

        Dim sw As New System.IO.StreamWriter(FolderBrowserDialog1.SelectedPath & "\_Lista" & Now.Ticks.ToString & ".log", _
                        False, System.Text.Encoding.Unicode)
        sw.WriteLine("Carpeta de destino:" & FolderBrowserDialog1.SelectedPath)
        For Each elem As ListViewItem In ListView2.SelectedItems
            If TipoExport = "JPG424" Then
                rutaOrigen = elem.SubItems(1).Text.Replace("_Scan250", "_Scan400")
            ElseIf TipoExport = "JPG250" Then
                rutaOrigen = elem.SubItems(1).Text.Replace("_Scan400", "_Scan250")
            ElseIf TipoExport = "ECW" Then
                rutaOrigen = elem.SubItems(2).Text
            End If
            rutaDestino = FolderBrowserDialog1.SelectedPath & "\" & SacarFileDeRuta(rutaOrigen)

            Try

                System.IO.File.Copy(rutaOrigen, rutaDestino, True)
                copiados = copiados + 1
                cadenaLog = "Copiado: " & rutaOrigen
                sw.WriteLine(cadenaLog)
                Application.DoEvents()
                ToolStripStatusLabel2.Text = "Copiado " & copiados & " de " & ListView2.SelectedItems.Count
            Catch ex As Exception
                Me.Cursor = Cursors.Default
                Hayfallos = True
                cadenaLog = "Error copia: " & rutaOrigen
                sw.WriteLine(cadenaLog)
            End Try
        Next
        sw.Close()
        sw.Dispose()

        If Hayfallos = True Then
            MessageBox.Show("Se han producido errores en la transferencia de ficheros", _
            AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            MessageBox.Show("Copia de ficheros terminada", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If


    End Sub


    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click

        Dim Fichero As String
        Fichero = Button6.Tag
        If System.IO.File.Exists(Fichero) = False Then
            MessageBox.Show("Fichero mosaico no disponible", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        LanzarVisorExterno(Fichero)

    End Sub

    Private Sub ListView2_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView2.DoubleClick

        If ListView2.Items.Count = 0 Then Exit Sub
        If ListView2.SelectedItems.Count > 0 Then
            Me.Cursor = Cursors.WaitCursor
            Dim FrmResult As New frmDocumentacion
            FrmResult.MdiParent = MDIPrincipal
            FrmResult.Text = ListView2.SelectedItems(0).Text
            FrmResult.CargarDatosSIDCARTO_By_Indice(ListView2.SelectedItems(0).Tag)
            FrmResult.Show()
            Me.Cursor = Cursors.Default
        End If

    End Sub


    Private Sub DataGridView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView1.DoubleClick

        If DataGridView1.RowCount = 0 Then Exit Sub
        DataGridView1.Visible = False
        GroupBox1.Visible = True
        RellenarCampos()

    End Sub

    Private Sub TextBox4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox4.Click
        TextBox4.SelectAll()
    End Sub

    Private Sub TextBox4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox4.TextChanged
        Dim cadFiltro As String
        Me.Cursor = Cursors.WaitCursor
        If TextBox4.Text.Trim = "" Then
            rcdMosaicos.RowFilter = ""
            DataGridView1.DataSource = rcdMosaicos
            DataGridView1.Update()
            ToolStripStatusLabel1.Text = "Elementos: " & DataGridView1.RowCount
            ToolStripStatusLabel2.Text = ""
            Me.Cursor = Cursors.Default
            Exit Sub
        End If
        cadFiltro = "titulo like '%" & TextBox4.Text.Trim & "%'"
        rcdMosaicos.RowFilter = cadFiltro
        DataGridView1.DataSource = rcdMosaicos
        DataGridView1.Update()
        ToolStripStatusLabel2.text = "Elementos filtrados: " & DataGridView1.RowCount
        Me.Cursor = Cursors.Default

    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click

        Dim iRegistro As Integer
        Dim contorno As String


        With SaveFileDialog1
            .Title = "Introduzca el nombre del fichero"
            .Filter = "Archivos XYZ *.xyz|*.xyz"
            .ShowDialog()
        End With
        If SaveFileDialog1.FileName = "" Then Exit Sub


        Me.Cursor = Cursors.WaitCursor
        Dim sw As New System.IO.StreamWriter(SaveFileDialog1.FileName, False, System.Text.Encoding.Unicode)
        For i = 0 To DataGridView1.SelectedRows.Count - 1
            ToolStripStatusLabel2.Text = "Generando contorno de " & DataGridView1.Item(1, _
                                                DataGridView1.SelectedRows(i).Index).Value.ToString
            iRegistro = DataGridView1.Item(0, _
                            DataGridView1.SelectedRows(i).Index).Value.ToString
            contorno = FunctionDameContornoMosaico(iRegistro)
            If contorno <> "" Then
                contorno = contorno.Replace("MULTIPOLYGON((", "")
                contorno = contorno.Replace("POLYGON((", "")
                contorno = contorno.Replace("))", "")

                Application.DoEvents()
                Dim vertexList() As String = contorno.Split(",")
                sw.WriteLine("DESCRIPTION=Unknown Area Type")
                sw.WriteLine("NAME=" & DataGridView1.Item(1, _
                            DataGridView1.SelectedRows(i).Index).Value.ToString)
                sw.WriteLine("CLOSED=YES")
                For Each vertex As String In vertexList
                    sw.WriteLine(vertex & " -99999999")
                Next
                Application.DoEvents()
            End If
        Next
        sw.Close() : sw.Dispose() : sw = Nothing
        Me.Cursor = Cursors.Default
        ToolStripStatusLabel2.Text = ""
        MessageBox.Show("Fichero de contornos generado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click

        Dim iRegistro As Integer
        Dim RutasECW As ArrayList
        RutasECW = New ArrayList

        If DataGridView1.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione al menos un mosaico", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        For i = 0 To DataGridView1.SelectedRows.Count - 1
            RutasECW.Add(rutaRepoGeorref & "\MOSAICOS_DIGITALES\28\" & _
                            DataGridView1.Item(3, DataGridView1.SelectedRows(i).Index).Value.ToString)
        Next
        If RutasECW.Count > 0 Then
            If GenerarProyectoGM(RutasECW, False) = True Then
                LanzarVisorExterno(My.Application.Info.DirectoryPath & "\LaunchGM.gmw")
            End If
        End If



    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click

        Dim RutaOrigen As String
        Dim RutaDestino As String
        Dim copiados As Integer
        Dim cadenaLog As String
        Dim Hayfallos As Boolean = False
        If DataGridView1.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione al menos un mosaico", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If



        FolderBrowserDialog1.ShowNewFolderButton = True
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
        If System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath) = False Then Exit Sub

        Dim sw As New System.IO.StreamWriter(FolderBrowserDialog1.SelectedPath & "\_Lista" & Now.Ticks.ToString & ".log", _
                                False, System.Text.Encoding.Unicode)
        For i = 0 To DataGridView1.SelectedRows.Count - 1
            RutaOrigen = rutaRepoGeorref & "\MOSAICOS_DIGITALES\28\" & _
                            DataGridView1.Item(3, DataGridView1.SelectedRows(i).Index).Value.ToString
            RutaDestino = FolderBrowserDialog1.SelectedPath & "\" & SacarFileDeRuta(RutaOrigen)
            Try
                System.IO.File.Copy(RutaOrigen, RutaDestino, True)
                copiados = copiados + 1
                cadenaLog = "Copiado: " & RutaOrigen
                sw.WriteLine(cadenaLog)
                ToolStripStatusLabel2.Text = "Copiado " & copiados & " de " & DataGridView1.SelectedRows.Count
            Catch ex As Exception
                Me.Cursor = Cursors.Default
                Hayfallos = True
                cadenaLog = "Error copia: " & RutaOrigen
                sw.WriteLine(cadenaLog)
            End Try
        Next
        sw.Close()
        sw.Dispose()
        If Hayfallos = True Then
            MessageBox.Show("Se han producido errores en la transferencia de ficheros", _
            AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            MessageBox.Show("Copia de ficheros terminada", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If


    End Sub


    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        Dim iRegistro As Integer
        Dim contorno As String

        If DataGridView1.SelectedRows.Count < 1 Then
            MessageBox.Show("Selecciona al menos un mosaico", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        With SaveFileDialog1
            .Title = "Introduzca el nombre del fichero"
            .Filter = "Archivos XYZ *.xyz|*.xyz"
            .ShowDialog()
        End With
        If SaveFileDialog1.FileName = "" Then Exit Sub


        Me.Cursor = Cursors.WaitCursor
        For i = 0 To DataGridView1.SelectedRows.Count - 1
            Application.DoEvents()
            ToolStripStatusLabel2.Text = "Generando contornos de las hojas de " & DataGridView1.Item(1, _
                                                DataGridView1.SelectedRows(i).Index).Value.ToString
            iRegistro = DataGridView1.Item(0, DataGridView1.SelectedRows(i).Index).Value.ToString
            FunctionDameContornoHojasMosaico(iRegistro, SaveFileDialog1.FileName)
        Next
        Me.Cursor = Cursors.Default
        ToolStripStatusLabel2.Text = ""
        MessageBox.Show("Fichero de contornos generado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)



    End Sub
End Class