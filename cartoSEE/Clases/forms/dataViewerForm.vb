Public Class dataViewerForm

    Property cadSQL As String
    Property camposVisibles As New List(Of String)
    Property filterFields As New Dictionary(Of String, String)
    Property headerFields As New Dictionary(Of String, String)
    Property fieldExternalDocument As String = ""
    Property lstExternalDocument As New ArrayList




    Dim recordSet As DataView
    Dim ajusteColumnas As Boolean



    Private Sub mostrarDetalle()

        Dim indiceGrid As Integer
        Dim elementoLV As ListViewItem

        lvDetails.Items.Clear()

        If DataGridView1.SelectedRows.Count = 0 Then
            indiceGrid = 0
        Else
            indiceGrid = DataGridView1.CurrentCell.RowIndex
        End If

        For ibucle = 0 To DataGridView1.Columns.Count - 1
            Application.DoEvents()
            elementoLV = New ListViewItem
            elementoLV.Text = DataGridView1.Columns(ibucle).HeaderText
            elementoLV.ImageIndex = 0
            elementoLV.SubItems.Add(DataGridView1.Item(ibucle, indiceGrid).Value.ToString)
            'elementoLV.Group = g1
            lvDetails.Items.Add(elementoLV)
            elementoLV = Nothing

        Next

        'idmtn25 = DataGridView1.Item("idmtn25", indiceGrid).Value.ToString

    End Sub


    Private Sub resizeListVDetails()

        If lvDetails.Columns.Count < 2 Then Exit Sub
        lvDetails.Columns(0).Width = 120
        lvDetails.Columns(1).Width = lvDetails.Width - lvDetails.Columns(0).Width - 50

    End Sub

    Private Sub textFilterApply()

        Dim cadFiltro As String = ""
        Dim fieldSearch As String = ""
        If txtSearch.Text.Trim = "" Or cboFiltros.SelectedIndex = -1 Then
            MostrarResConsulta(cadFiltro)
            Return
        End If

        If cboFiltros.SelectedIndex = 0 Then
            For Each field In filterFields
                cadFiltro = IIf(cadFiltro = "", field.Key & " like '%" & txtSearch.Text.Trim & "%'", cadFiltro & " AND " & field.Key & " like '%" & txtSearch.Text.Trim & "%'")
            Next
        Else
            fieldSearch = CType(cboFiltros.SelectedItem, itemData).Valor
            cadFiltro = fieldSearch & " like '%" & txtSearch.Text.Trim & "%'"
        End If

        MostrarResConsulta(cadFiltro)

    End Sub


    Sub MostrarResConsulta(ByVal cadfiltro As String)

        If recordSet Is Nothing Then Exit Sub
        recordSet.RowFilter = cadfiltro
        DataGridView1.DataSource = recordSet
        DataGridView1.Update()
        If cadfiltro = "" Then
            ToolStripStatusLabel2.Text = ""
        Else
            ToolStripStatusLabel2.Text = "Elementos filtrados: " & DataGridView1.RowCount
        End If
        DataGridView1.Visible = True
        gBoxdetails.Visible = False

    End Sub

    Sub cargarDatos()

        recordSet = New DataView
        If CargarDataView(_cadSQL, recordSet) = False Then
            MessageBox.Show("No se puede cargar los datos", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        DataGridView1.DataSource = recordSet
        For Each col As DataGridViewColumn In DataGridView1.Columns
            col.Visible = camposVisibles.Contains(col.DataPropertyName)
            If headerFields.ContainsKey(col.DataPropertyName) Then
                headerFields.TryGetValue(col.DataPropertyName, col.HeaderText)
            End If
        Next

        ajusteColumnas = True
        ToolStripStatusLabel1.Text = "Resultados: " & DataGridView1.Rows.Count

    End Sub

    Private Sub dataViewer_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.Size = New Point(875, 480)
        ajusteColumnas = False
        Me.DataGridView1.Dock = DockStyle.Fill
        Me.gBoxdetails.Dock = DockStyle.Fill

        lvDetails.Columns.Clear()
        lvDetails.Columns.Add("Atributo", 120, HorizontalAlignment.Center)
        lvDetails.Columns.Add("Valor", 150, HorizontalAlignment.Left)
        lvDetails.SmallImageList = ImageList2
        lvDetails.FullRowSelect = True
        lvDetails.View = View.Details

        lvVinculos.LargeImageList = ImageList1
        lvVinculos.SmallImageList = ImageList2
        lvVinculos.FullRowSelect = True
        lvVinculos.View = View.Tile
        lvVinculos.Columns.Add("Nombre", 150, HorizontalAlignment.Left)
        lvVinculos.Columns.Add("Tipo", 150, HorizontalAlignment.Left)
        lvVinculos.Columns.Add("Tamaño", 90, HorizontalAlignment.Right)

        TabControl1.ImageList = ImageList2
        TabControl1.TabPages(0).ImageIndex = 4
        TabControl1.TabPages(1).ImageIndex = 6
        gBoxdetails.Visible = False

        ToolStripStatusLabel1.Text = ""
        ToolStripStatusLabel2.Text = ""
        ToolStripStatusLabel3.Text = ""

        cargarDatos()
        cboFiltros.Items.Add(New itemData("(Todos)", ""))
        For Each definition In filterFields
            Application.DoEvents()
            cboFiltros.Items.Add(New itemData(definition.Value, definition.Key))
        Next

        btnLaunchDocument.Visible = IIf(fieldExternalDocument = "", False, True)

        If lstExternalDocument.Count = 0 Then
            TabControl1.TabPages.Remove(TabPage2)
        End If

    End Sub

    Private Sub dataViewer_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        If Not recordSet Is Nothing Then
            recordSet.Dispose()
            recordSet = Nothing
            DataGridView1.DataSource = Nothing
            DataGridView1.Dispose()
            DataGridView1 = Nothing
        End If
        MyBase.Dispose()
        MyBase.Finalize()

    End Sub

    Private Sub manageToolbar(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTodos.Click, btnDetalles.Click

        If IsNothing(DataGridView1.CurrentCell) Then Exit Sub
        DataGridView1.Visible = False
        gBoxdetails.Visible = False
        ToolStripStatusLabel2.Text = ""
        If sender.name = "btnTodos" Then
            DataGridView1.Visible = True
        ElseIf sender.name = "btnDetalles" Then
            gBoxdetails.Visible = True
            mostrarDetalle()
            'ResizeLVDetails()
        End If

    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrev.Click, btnNext.Click

        If sender.name = "btnPrev" Then
            If DataGridView1.SelectedRows.Count = 0 Then
                DataGridView1.Rows(DataGridView1.CurrentCell.RowIndex).Selected = True
            End If
            Dim _rowIndex = DataGridView1.SelectedRows(0).Index - 1
            If _rowIndex > -1 Then
                Dim prevRow As DataGridViewRow = DataGridView1.Rows(_rowIndex)
                ' Move the Glyph arrow to the previous row
                DataGridView1.CurrentCell = prevRow.Cells(2)
                DataGridView1.Rows(_rowIndex).Selected = True

            End If
        ElseIf sender.name = "btnNext" Then
            If DataGridView1.SelectedRows.Count = 0 Then
                DataGridView1.Rows(DataGridView1.CurrentCell.RowIndex).Selected = True
            End If
            Dim _rowIndex = DataGridView1.SelectedRows(0).Index + 1
            If _rowIndex <= DataGridView1.Rows.Count - 1 Then
                Dim nextRow As DataGridViewRow = DataGridView1.Rows(_rowIndex)
                ' Move the Glyph arrow to the next row
                DataGridView1.CurrentCell = nextRow.Cells(2)
                DataGridView1.Rows(_rowIndex).Selected = True

            End If
        End If


        If gBoxdetails.Visible = True Then
            Me.Cursor = Cursors.WaitCursor
            mostrarDetalle()
            'ResizeLVDetails()
            Me.Cursor = Cursors.Default
        End If


    End Sub

    Private Sub DataGridView1_Click(sender As Object, e As System.EventArgs) Handles DataGridView1.Click
        If DataGridView1.Rows.Count = 0 Then Exit Sub
        If DataGridView1.SelectedRows.Count = 0 Then
            ToolStripStatusLabel3.Text = ""
        Else
            ToolStripStatusLabel3.Text = "Seleccionados: " & DataGridView1.SelectedRows.Count
        End If
    End Sub

    Private Sub DataGridView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView1.DoubleClick
        If IsNothing(DataGridView1.CurrentCell) Then Exit Sub
        DataGridView1.Visible = False
        gBoxdetails.Visible = True
        mostrarDetalle()
        'ResizeLVDetails()
    End Sub

    Sub ResizeDatagridView()

        Dim iNumColVis As Integer
        Dim calcWidth As Integer
        If ajusteColumnas = False Then Exit Sub
        If DataGridView1.Columns.Count = 0 Then Exit Sub

        iNumColVis = 0
        For iBucle = 0 To DataGridView1.Columns.Count - 1
            If DataGridView1.Columns(iBucle).Visible = True Then
                iNumColVis = iNumColVis + 1
            End If
        Next

        If iNumColVis = 0 Then Exit Sub
        calcWidth = (DataGridView1.Width - 70) / iNumColVis
        Try
            For iBucle = 0 To DataGridView1.Columns.Count - 1
                DataGridView1.Columns(iBucle).Width = calcWidth
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try

    End Sub

    Private Sub ResizeDatagridView(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView1.Resize

        ResizeDatagridView()

    End Sub

    Private Sub ResizeLVDetails()

        If lvDetails.Columns.Count < 2 Then Exit Sub
        lvDetails.Columns(0).Width = 120
        lvDetails.Columns(1).Width = lvDetails.Width - lvDetails.Columns(0).Width - 50

    End Sub

    Private Sub btnAjustar_Click(sender As System.Object, e As System.EventArgs) Handles btnAjustar.Click
        If btnAjustar.Text = "Ajustar" Then
            btnAjustar.Text = "No ajustar"
            ajusteColumnas = True
            ResizeDatagridView()
        Else
            btnAjustar.Text = "Ajustar"
            ajusteColumnas = False
        End If
    End Sub

    Private Sub txtSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyUp
        textFilterApply()
    End Sub

    Private Sub btnLaunchDocument_Click(sender As Object, e As EventArgs) Handles btnLaunchDocument.Click

        Dim pathExternalFile As String = ""
        Dim indiceGrid As Integer

        If DataGridView1.SelectedRows.Count = 0 Then
            indiceGrid = 0
        Else
            indiceGrid = DataGridView1.CurrentCell.RowIndex
        End If

        Try
            pathExternalFile = DataGridView1.Item(fieldExternalDocument, indiceGrid).Value.ToString
            If System.IO.File.Exists(pathExternalFile) Then
                Process.Start(pathExternalFile)
            Else
                MessageBox.Show("No se puede acceder al documento" & Environment.NewLine & pathExternalFile, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click

        Dim ExportarSelect As Boolean
        Dim ExportarTodas As Boolean
        Dim cadLinea As String = ""


        If DataGridView1.RowCount = 0 Then Exit Sub
        With SaveFileDialog1
            .Title = "Introduzca el nombre del fichero"
            .Filter = "Archivos CSV *.csv|*.csv"
            .ShowDialog()
        End With
        If SaveFileDialog1.FileName = "" Then Exit Sub

        If DataGridView1.SelectedRows.Count > 1 Then
            ExportarSelect = True
            ExportarTodas = False
        Else
            ExportarSelect = False
            ExportarTodas = True
        End If


        Me.Cursor = Cursors.WaitCursor
        Dim sw As New System.IO.StreamWriter(SaveFileDialog1.FileName, False, System.Text.Encoding.Unicode)

        If ExportarSelect = True Then
            ToolStripStatusLabel3.Text = "Generando fichero exportación"
            For i = 1 To DataGridView1.ColumnCount - 1
                If DataGridView1.Columns(i).Visible = True Then
                    cadLinea = cadLinea & DataGridView1.Columns(i).HeaderText & ";"
                End If
            Next
            sw.WriteLine(cadLinea)
            For i = 0 To DataGridView1.SelectedRows.Count - 1
                cadLinea = ""
                For j As Integer = 1 To DataGridView1.ColumnCount - 1
                    If DataGridView1.Columns(j).Visible = True Then
                        cadLinea = cadLinea & DataGridView1.Item(j, DataGridView1.SelectedRows(i).Index).Value.ToString & ";"
                    End If
                Next
                sw.WriteLine(cadLinea)
            Next
        End If
        If ExportarTodas = True Then
            ToolStripStatusLabel3.Text = "Generando fichero exportación"
            For i = 1 To DataGridView1.ColumnCount - 1
                If DataGridView1.Columns(i).Visible = True Then
                    cadLinea = cadLinea & DataGridView1.Columns(i).HeaderText & ";"
                End If
            Next
            sw.WriteLine(cadLinea)
            For i = 0 To DataGridView1.Rows.Count - 1
                cadLinea = ""
                For j As Integer = 1 To DataGridView1.ColumnCount - 1
                    If DataGridView1.Columns(j).Visible = True Then
                        cadLinea = cadLinea & DataGridView1.Item(j, DataGridView1.Rows(i).Index).Value.ToString & ";"
                    End If
                Next
                sw.WriteLine(cadLinea)
            Next
        End If

        sw.Close()
        sw.Dispose()
        Me.Cursor = Cursors.Default
        MessageBox.Show("Fichero de exportación generado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
End Class