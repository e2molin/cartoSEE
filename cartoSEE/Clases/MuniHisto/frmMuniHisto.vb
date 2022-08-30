Public Class frmMuniHisto

    Dim rcdMuniHistos As DataView

    Private Sub frmTerritorios_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Size = New Point(720, 430)

        DataGridView1.Location = New Point(0, 43)
        DataGridView1.Size = New Point(Me.Width - 15, Me.Height - 100)
        DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Bottom Or AnchorStyles.Right

        TableLayoutPanel1.Location = New Point(0, 43)
        TableLayoutPanel1.Size = New Point(Me.Width - 15, Me.Height - 100)
        TableLayoutPanel1.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Bottom Or AnchorStyles.Right
        TableLayoutPanel1.Visible = False

        lvDetails.Columns.Clear()
        lvDetails.Columns.Add("Atributo", 130, HorizontalAlignment.Center)
        lvDetails.Columns.Add("Valor", 240, HorizontalAlignment.Left)
        lvDetails.SmallImageList = MDIPrincipal.ImageList2
        lvDetails.FullRowSelect = True
        lvDetails.View = View.Details
        lvDetails.HeaderStyle = ColumnHeaderStyle.None

        lvColin.Columns.Clear()
        lvColin.Columns.Add("Atributo", 220, HorizontalAlignment.Center)
        lvColin.Columns.Add("Valor", 80, HorizontalAlignment.Left)
        lvColin.SmallImageList = MDIPrincipal.ImageList2
        lvColin.FullRowSelect = True
        lvColin.View = View.Details

        ToolStripStatusLabel1.Text = ""
        ToolStripStatusLabel2.Text = ""
        ToolStripStatusLabel3.Text = ""

        RellenarDataview()

        btnEdit.Enabled = usuarioMyApp.permisosLista.editarDocumentacion
        btnNew.Enabled = usuarioMyApp.permisosLista.editarDocumentacion


    End Sub


    Private Sub btnTodos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTodos.Click, btnDetalles.Click
        If IsNothing(DataGridView1.CurrentCell) Then Exit Sub

        DataGridView1.Visible = False
        TableLayoutPanel1.Visible = False

        If sender.name = "btnTodos" Then
            DataGridView1.Visible = True
        ElseIf sender.name = "btnDetalles" Then
            TableLayoutPanel1.Visible = True
            mostrarDetalle()
        End If
    End Sub

    Private Sub btnExportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportar.Click

        Dim ExportarSelect As Boolean = False
        Dim ExportarTodas As Boolean = True
        Dim cadLinea As String

        If DataGridView1.RowCount = 0 Then Exit Sub
        With SaveFileDialog1
            .Title = "Introduzca el nombre del fichero"
            .Filter = "Archivos CSV *.csv|*.csv"
            .ShowDialog()
        End With
        If SaveFileDialog1.FileName = "" Then Exit Sub

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
                        cadLinea = cadLinea & _
                                DataGridView1.Item(j, DataGridView1.SelectedRows(i).Index).Value.ToString & ";"
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
                        cadLinea = cadLinea & _
                                DataGridView1.Item(j, DataGridView1.Rows(i).Index).Value.ToString & ";"
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



    Private Sub procBotonera(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrev.Click, btnNext.Click

        If IsNothing(DataGridView1.CurrentCell) Then Exit Sub
        If sender.name = "btnNext" Then
            If DataGridView1.CurrentRow.Index < DataGridView1.Rows.Count - 1 Then
                DataGridView1.CurrentCell = DataGridView1(1, DataGridView1.CurrentRow.Index + 1)
                If TableLayoutPanel1.Visible = True Then mostrarDetalle()
            End If
        End If
        If sender.name = "btnPrev" Then
            If DataGridView1.CurrentRow.Index > 0 Then
                DataGridView1.CurrentCell = DataGridView1(1, DataGridView1.CurrentRow.Index - 1)
                If TableLayoutPanel1.Visible = True Then mostrarDetalle()
            End If
        End If

    End Sub

    Private Sub DataGridView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView1.DoubleClick

        If IsNothing(DataGridView1.CurrentCell) Then Exit Sub
        DataGridView1.Visible = False
        TableLayoutPanel1.Visible = True
        mostrarDetalle()

    End Sub

    Private Sub txtSearch_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged, cboFiltros.SelectedIndexChanged
        Dim cadFiltro As String = ""

        If txtSearch.Text.Trim <> "" Then
            If cboFiltros.SelectedIndex = 0 Or cboFiltros.SelectedIndex = -1 Then
                If IsNumeric(txtSearch.Text.Trim) Then
                    If txtSearch.Text.Length <= 2 Then
                        cadFiltro = "provincia_id = " & txtSearch.Text.Trim
                    Else
                        cadFiltro = "cod_munihisto = " & txtSearch.Text.Trim
                    End If
                Else
                    cadFiltro = "nombremunicipiohistorico like '%" & txtSearch.Text.Trim & "%' OR " & _
                                "nombremunicipioactual like '%" & txtSearch.Text.Trim & "%' OR " & _
                                "nombreprovincia like '%" & txtSearch.Text.Trim & "%'"
                End If
            ElseIf cboFiltros.SelectedIndex = 1 Then
                cadFiltro = "nombremunicipiohistorico like '%" & txtSearch.Text.Trim & "%'"
            ElseIf cboFiltros.SelectedIndex = 2 Then
                cadFiltro = "nombremunicipioactual like '%" & txtSearch.Text.Trim & "%'"
            ElseIf cboFiltros.SelectedIndex = 3 Then
                cadFiltro = "nombreprovincia like '%" & txtSearch.Text.Trim & "%'"
            ElseIf cboFiltros.SelectedIndex = 4 Then
                If IsNumeric(txtSearch.Text.Trim) Then
                    If txtSearch.Text.Length <= 2 Then
                        cadFiltro = "provincia_id = " & txtSearch.Text.Trim
                    Else
                        cadFiltro = "cod_munihisto = " & txtSearch.Text.Trim
                    End If
                End If
            End If
        End If
        MostrarResConsulta(cadFiltro)

    End Sub


    'Procedimientos
    Sub MostrarResConsulta(ByVal cadfiltro As String)

        If rcdMuniHistos Is Nothing Then Exit Sub
        rcdMuniHistos.RowFilter = cadfiltro
        DataGridView1.DataSource = rcdMuniHistos
        DataGridView1.Update()
        If cadfiltro = "" Then
            ToolStripStatusLabel2.Text = ""
        Else
            ToolStripStatusLabel2.Text = "Elementos filtrados: " & DataGridView1.RowCount
        End If
        DataGridView1.Visible = True
        TableLayoutPanel1.Visible = False

    End Sub


    Sub mostrarDetalle()

        Dim elementoLV As ListViewItem
        Dim idmuniH As Integer
        Dim indiceGrid As Integer
        Dim g1 As ListViewGroup

        g1 = New ListViewGroup("Atributos")
        lvDetails.Groups.Add(g1)

        lvDetails.Items.Clear()
        lvColin.Items.Clear()
        txtObserv.Text = ""
        indiceGrid = DataGridView1.CurrentCell.RowIndex
        idmuniH = DataGridView1.Item("idmunihisto", indiceGrid).Value.ToString

        TableLayoutPanel1.Tag = idmuniH
        elementoLV = New ListViewItem : elementoLV.Text = "Municipio histórico ID" : elementoLV.ImageIndex = 4
        elementoLV.SubItems.Add(idmuniH) : elementoLV.Group = g1
        lvDetails.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem : elementoLV.Text = "Nombre histórico" : elementoLV.ImageIndex = 4
        elementoLV.SubItems.Add(DataGridView1.Item("nombremunicipiohistorico", indiceGrid).Value.ToString) : elementoLV.Group = g1
        lvDetails.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem : elementoLV.Text = "Municipio actual" : elementoLV.ImageIndex = 4
        elementoLV.SubItems.Add(DataGridView1.Item("nombremunicipioactual", indiceGrid).Value.ToString) : elementoLV.Group = g1
        lvDetails.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem : elementoLV.Text = "Provincia" : elementoLV.ImageIndex = 4
        elementoLV.SubItems.Add(DataGridView1.Item("nombreprovincia", indiceGrid).Value.ToString) : elementoLV.Group = g1
        lvDetails.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem : elementoLV.Text = "Código INE Histo" : elementoLV.ImageIndex = 4
        elementoLV.SubItems.Add(String.Format("{0:0000000}", DataGridView1.Item("cod_munihisto", indiceGrid).Value)) : elementoLV.Group = g1
        lvDetails.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem : elementoLV.Text = "Código INE Actual" : elementoLV.ImageIndex = 4
        elementoLV.SubItems.Add(String.Format("{0:0000000}", DataGridView1.Item("codine", indiceGrid).Value)) : elementoLV.Group = g1
        lvDetails.Items.Add(elementoLV) : elementoLV = Nothing

        ToolStripStatusLabel2.Text = "Atributos del municipio histórico " & DataGridView1.Item("nombremunicipiohistorico", indiceGrid).Value.ToString

    End Sub


    'Sub rellenarColindantes(ByVal codPoligono As Integer)

    '    Dim listaColin As New ArrayList
    '    Dim elementoLV As ListViewItem
    '    Dim g1 As ListViewGroup

    '    DameColindantesByIdTerritorio(codPoligono, listaColin)
    '    lvColin.Items.Clear()
    '    g1 = New ListViewGroup("Territorios colindantes")
    '    lvColin.Groups.Add(g1)
    '    lvColin.HeaderStyle = ColumnHeaderStyle.None
    '    If listaColin.Count = 0 Then
    '        elementoLV = New ListViewItem : elementoLV.Text = "Colindantes no disponibles" : elementoLV.ImageIndex = 4
    '        elementoLV.Group = g1 : lvColin.Items.Add(elementoLV) : elementoLV = Nothing
    '    Else
    '        For Each terri As TerritorioBSID In listaColin
    '            elementoLV = New ListViewItem : elementoLV.Text = terri.nombre : elementoLV.ImageIndex = 4
    '            elementoLV.SubItems.Add(String.Format("{0:00000}", terri.municipioINE)) : elementoLV.Group = g1
    '            lvColin.Items.Add(elementoLV) : elementoLV = Nothing
    '        Next
    '    End If

    'End Sub

    Sub RellenarDataview()
        Dim SQLBase As String

        SQLBase = "SELECT munihisto.idmunihisto,munihisto.nombremunicipiohistorico,listamunicipios.nombre as nombremunicipioactual," & _
                "provincias.nombreprovincia,munihisto.cod_munihisto,munihisto.cod_muni,munihisto.provincia_id," & _
                "listamunicipios.inecorto as codine " & _
                "FROM munihisto " & _
                "left JOIN provincias on provincias.idprovincia=munihisto.provincia_id " & _
                "left JOIN ngmepschema.listamunicipios on munihisto.entidad_id=listamunicipios.identidad"

        rcdMuniHistos = New DataView
        If CargarDataView(SQLBase, rcdMuniHistos) = False Then
            MessageBox.Show("No se pueden cargar los datos", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        DataGridView1.DataSource = rcdMuniHistos
        DataGridView1.Columns(0).Visible = False
        DataGridView1.Columns(1).HeaderText = "Nombre"
        DataGridView1.Columns(1).Width = 250
        DataGridView1.Columns(2).HeaderText = "Municipio Actual"
        DataGridView1.Columns(2).Width = 250
        DataGridView1.Columns(3).HeaderText = "Provincia"
        DataGridView1.Columns(3).Width = 75
        DataGridView1.Columns(4).HeaderText = "Código"
        DataGridView1.Columns(4).Width = 50
        DataGridView1.Columns(5).Visible = False
        DataGridView1.Columns(6).Visible = False
        DataGridView1.Columns(7).Visible = False
        DataGridView1.ColumnHeadersVisible = True
        ToolStripStatusLabel1.Text = "Número de municipios históricos: " & DataGridView1.RowCount

    End Sub



    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        If altaMuniH.muniHNewDialog = True Then
            Me.Cursor = Cursors.WaitCursor
            CargarListasMunicipios()
            Me.Cursor = Cursors.Default
            MessageBox.Show("Municipio histórico creado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub


    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click

        Dim muniHEdit As MuniHisto
        Dim indiceGrid As Integer
        Dim idMuni As Integer

        indiceGrid = DataGridView1.CurrentCell.RowIndex
        idMuni = DataGridView1.Item("idmunihisto", indiceGrid).Value.ToString
        If idMuni = 0 Then Exit Sub
        muniHEdit = New MuniHisto(idMuni)

        Application.DoEvents()

        If altaMuniH.muniHEditorDialog(muniHEdit) = True Then
            Me.Cursor = Cursors.WaitCursor
            CargarListasMunicipios()
            Me.Cursor = Cursors.Default
            MessageBox.Show("Municipio histórico modificado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click

        Dim indiceGrid As Integer
        Dim idMuniH As Integer
        If TableLayoutPanel1.Visible = False Then
            RellenarDataview()
        Else
            indiceGrid = DataGridView1.CurrentCell.RowIndex
            idMuniH = DataGridView1.Item("idmunihisto", indiceGrid).Value.ToString
            RellenarDataview()
            MostrarResConsulta("idmunihisto=" & idMuniH)
            DataGridView1.Visible = False
            TableLayoutPanel1.Visible = True
            mostrarDetalle()

        End If

    End Sub

    Private Sub redimensionLV(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvDetails.Resize, lvColin.Resize
        If lvDetails.Columns.Count = 2 Then
            lvDetails.Columns(0).Width = 130
            lvDetails.Columns(1).Width = lvDetails.Width - 140
        End If
        If lvColin.Columns.Count = 2 Then
            lvColin.Columns(0).Width = lvColin.Width - 105
            lvColin.Columns(1).Width = 80
        End If

    End Sub

End Class