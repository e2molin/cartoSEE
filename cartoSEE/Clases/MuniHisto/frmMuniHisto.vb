Public Class frmMuniHisto

    Dim rcdMuniHistos As DataView

    Const widthScrollLV As Integer = 30
    Dim FixedCols() As Integer = {0, 4, 5, 6} 'Columnas con ancho fijo aunque crezca el tamaño del datagrid
    Dim Hide_And_Show_Columns() As Integer = {1, 2, 3, 4, 5, 6, 7, 8, 9} ' Índices de columnas que pueden mostrarse u ocultarse



    Sub ResizeDatagridView()

        Dim iNumColVis As Integer
        Dim calcWidth As Integer
        Dim widthFixedCols As Integer = 0
        Dim visibleWidthFixedCols As Integer = 0
        Dim widthScrollControl As Integer = 60
        If DataGridView1.Columns.Count = 0 Then Exit Sub

        iNumColVis = 0
        For iCol = 0 To DataGridView1.Columns.Count - 1
            If DataGridView1.Columns(iCol).Visible = True Then
                iNumColVis += 1
            End If
        Next

        If iNumColVis = 0 Then Exit Sub

        'Columnas de ancho fijo
        For Each iCol In FixedCols
            If DataGridView1.Columns(iCol).Visible Then
                widthFixedCols += DataGridView1.Columns(iCol).Width
                visibleWidthFixedCols += 1
            End If
        Next

        calcWidth = (DataGridView1.Width - widthFixedCols - widthScrollControl) / (iNumColVis - visibleWidthFixedCols)
        Try
            For iCol = 0 To DataGridView1.Columns.Count - 1
                If FixedCols.Contains(iCol) Then Continue For
                DataGridView1.Columns(iCol).Width = calcWidth
            Next
        Catch ex As Exception
            ModalExclamation(ex.Message)
        End Try

    End Sub


    Sub RenombrarItemsColumnas()

        Dim iCol As Integer
        Dim iMenu As Integer = 0
        Dim nombreCtrl As String

        If Hide_And_Show_Columns.Length = 0 Then Exit Sub

        For Each numCol In Hide_And_Show_Columns
            iMenu += 1
            nombreCtrl = $"mnuColumna{iMenu}"
            Try
                Dim tsmi = ToolStripDropDownButton1.DropDownItems.Find(nombreCtrl, True).OfType(Of ToolStripMenuItem).FirstOrDefault()
                tsmi.Text = DataGridView1.Columns(numCol).HeaderText
                tsmi.Checked = DataGridView1.Columns(numCol).Visible
                tsmi.Tag = numCol
            Catch ex As Exception
                ModalError(ex.Message)
            End Try
        Next

        For iCol = iMenu + 1 To ToolStripDropDownButton1.DropDownItems.Count
            nombreCtrl = $"mnuColumna{iCol}"
            Dim tsmi = ToolStripDropDownButton1.DropDownItems.Find(nombreCtrl, True).OfType(Of ToolStripMenuItem).FirstOrDefault()
            tsmi.Visible = False
        Next

    End Sub

    Private Sub ManageVisibility(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                    Handles mnuColumna1.Click, mnuColumna2.Click, mnuColumna3.Click, mnuColumna4.Click, mnuColumna5.Click,
                    mnuColumna6.Click, mnuColumna7.Click, mnuColumna8.Click, mnuColumna9.Click, mnuColumna10.Click, mnuColumna11.Click,
                    mnuColumna12.Click, mnuColumna13.Click, mnuColumna14.Click, mnuColumna15.Click, mnuColumna16.Click, mnuColumna17.Click,
                    mnuColumna18.Click, mnuColumna19.Click, mnuColumna20.Click

        Dim nombreCtrl As String

        If DataGridView1.ColumnCount = 0 Then Exit Sub
        For iMenu = 1 To ToolStripDropDownButton1.DropDownItems.Count
            nombreCtrl = $"mnuColumna{iMenu}"
            If sender.name = nombreCtrl Then
                Dim tsmi = ToolStripDropDownButton1.DropDownItems.Find(nombreCtrl, True).OfType(Of ToolStripMenuItem).FirstOrDefault()
                If tsmi IsNot Nothing Then
                    DataGridView1.Columns(tsmi.Tag).Visible = tsmi.Checked
                End If
            End If
        Next





        ResizeDatagridView()

    End Sub

    Private Sub frmTerritorios_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Size = New Point(1024, 800)

        DataGridView1.Location = New Point(0, 60)
        DataGridView1.Size = New Point(Me.Width - 15, Me.Height - 130)
        DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Bottom Or AnchorStyles.Right

        TableLayoutPanel1.Location = New Point(0, 60)
        TableLayoutPanel1.Size = New Point(Me.Width - 15, Me.Height - 130)
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
                        cadFiltro = "provincia_ine = " & txtSearch.Text.Trim
                    Else
                        cadFiltro = "cod_munihisto = " & txtSearch.Text.Trim
                    End If
                Else
                    cadFiltro = "nombre like '%" & txtSearch.Text.Trim & "%' OR " &
                                "nombremunicipioactual like '%" & txtSearch.Text.Trim & "%' OR " &
                                "nombreprovincia like '%" & txtSearch.Text.Trim & "%'"
                End If
            ElseIf cboFiltros.SelectedIndex = 1 Then
                cadFiltro = "nombre like '%" & txtSearch.Text.Trim & "%'"
            ElseIf cboFiltros.SelectedIndex = 2 Then
                cadFiltro = "nombremunicipioactual like '%" & txtSearch.Text.Trim & "%'"
            ElseIf cboFiltros.SelectedIndex = 3 Then
                cadFiltro = "nombreprovincia like '%" & txtSearch.Text.Trim & "%'"
            ElseIf cboFiltros.SelectedIndex = 4 Then
                If IsNumeric(txtSearch.Text.Trim) Then
                    If txtSearch.Text.Length <= 2 Then
                        cadFiltro = "provincia_ine = " & txtSearch.Text.Trim
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
        Try
            rcdMuniHistos.RowFilter = cadfiltro
            DataGridView1.DataSource = rcdMuniHistos
            DataGridView1.Update()

        Catch ex As Exception
            ModalError(ex.Message)
        End Try
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
        Dim g2 As ListViewGroup
        Dim municipioINE_LongFormat As String = ""

        g1 = New ListViewGroup("Atributos")
        lvDetails.Groups.Add(g1)

        g2 = New ListViewGroup("Enlaces")
        lvDetails.Groups.Add(g2)

        lvDetails.Items.Clear()
        lvColin.Items.Clear()
        txtObserv.Text = ""
        indiceGrid = DataGridView1.CurrentCell.RowIndex
        idmuniH = DataGridView1.Item("idterritorio", indiceGrid).Value.ToString
        municipioINE_LongFormat = "34" &
                                    String.Format("{0:00}", DataGridView1.Item("comunidad_ine", indiceGrid).Value.ToString) &
                                    String.Format("{0:00}", DataGridView1.Item("provincia_ine", indiceGrid).Value.ToString) &
                                    String.Format("{0:00000}", DataGridView1.Item("codine", indiceGrid).Value.ToString)

        TableLayoutPanel1.Tag = idmuniH
        elementoLV = New ListViewItem : elementoLV.Text = "Municipio histórico ID" : elementoLV.ImageIndex = 4
        elementoLV.SubItems.Add(idmuniH) : elementoLV.Group = g1
        lvDetails.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem : elementoLV.Text = "Nombre histórico" : elementoLV.ImageIndex = 4
        elementoLV.SubItems.Add(DataGridView1.Item("nombre", indiceGrid).Value.ToString) : elementoLV.Group = g1
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
        elementoLV.SubItems.Add(String.Format("{0:00000}", DataGridView1.Item("codine", indiceGrid).Value)) : elementoLV.Group = g1
        lvDetails.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem : elementoLV.Text = "Tipo" : elementoLV.ImageIndex = 4
        elementoLV.SubItems.Add(DataGridView1.Item("tipo", indiceGrid).Value) : elementoLV.Group = g1
        lvDetails.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem : elementoLV.Text = "Nombre completo" : elementoLV.ImageIndex = 4
        elementoLV.SubItems.Add(DataGridView1.Item("nombremostrado", indiceGrid).Value) : elementoLV.Group = g1
        lvDetails.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem : elementoLV.Text = "Nombre en el CdD" : elementoLV.ImageIndex = 4
        elementoLV.SubItems.Add(DataGridView1.Item("nombrecdd", indiceGrid).Value) : elementoLV.Group = g1
        lvDetails.Items.Add(elementoLV) : elementoLV = Nothing

        'Enlaces

        elementoLV = New ListViewItem : elementoLV.Text = "Planimetrías CdD" : elementoLV.ImageIndex = 11 : elementoLV.Tag = "URL"
        elementoLV.SubItems.Add($"{urlCDDSearchEngine}filtro.codFamilia=MIPAC&filtro.codIne={municipioINE_LongFormat}") : elementoLV.Group = g2
        lvDetails.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem : elementoLV.Text = "Planos de población CdD" : elementoLV.ImageIndex = 11 : elementoLV.Tag = "URL"
        elementoLV.SubItems.Add($"{urlCDDSearchEngine}filtro.codFamilia=PLPOB&filtro.codIne={municipioINE_LongFormat}") : elementoLV.Group = g2
        lvDetails.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem : elementoLV.Text = "Planos de edificios CdD" : elementoLV.ImageIndex = 11 : elementoLV.Tag = "URL"
        elementoLV.SubItems.Add($"{urlCDDSearchEngine}filtro.codFamilia=PLEDI&filtro.codIne={municipioINE_LongFormat}") : elementoLV.Group = g2
        lvDetails.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem : elementoLV.Text = "Hojas kilométricas CdD" : elementoLV.ImageIndex = 11 : elementoLV.Tag = "URL"
        elementoLV.SubItems.Add($"{urlCDDSearchEngine}filtro.codFamilia=HKPUP&filtro.codIne={municipioINE_LongFormat}") : elementoLV.Group = g2
        lvDetails.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem : elementoLV.Text = "Gazetteer NGBE" : elementoLV.ImageIndex = 12 : elementoLV.Tag = "URL"
        elementoLV.SubItems.Add($"{urlGazetteerNGBESearchEngine}identidad={DataGridView1.Item("ngbe_id", indiceGrid).Value}") : elementoLV.Group = g2
        lvDetails.Items.Add(elementoLV) : elementoLV = Nothing



        txtObserv.Text = DataGridView1.Item("observaciones", indiceGrid).Value.ToString

        ToolStripStatusLabel2.Text = $"Atributos de {DataGridView1.Item("nombremostrado", indiceGrid).Value.ToString}"

    End Sub


    Sub RellenarDataview()
        Dim SQLBase As String

        SQLBase = "SELECT territorios.idterritorio,territorios.nombre,
                    listamunicipios.nombre As nombremunicipioactual,
                    provincias.nombreprovincia,
                    to_char(Territorios.munihisto, 'FM0000000'::text) AS cod_munihisto,
                    to_char(Territorios.Municipio, 'FM00000'::text) AS codine,
                    tipo,pertenencia, territorios.nombremostrado, nombrecdd,  
                    observaciones,to_char(territorios.provincia, 'FM00'::text) AS provincia_ine, ngbe_id,nomen_id as ngmep_id,poligono_carto,to_char(provincias.comautonoma_id, 'FM00'::text) AS comunidad_ine
                    FROM bdsidschema.territorios 
                    LEFT JOIN bdsidschema.provincias ON provincias.idprovincia=territorios.provincia 
                    LEFT JOIN ngmepschema.listamunicipios ON territorios.ngmep_muni_id = listamunicipios.identidad
                    WHERE tipo IN ('Municipio','Municipio histórico') order by territorios.nombremostrado, nombremunicipioactual"
        Try
            rcdMuniHistos = New DataView
            If Not CargarDataView(SQLBase, rcdMuniHistos) Then
                ModalExclamation("No se pueden cargar los datos")
                Exit Sub
            End If

        Catch ex As Exception
            ModalError(ex.Message)
        End Try


        DataGridView1.DataSource = rcdMuniHistos
        DataGridView1.Columns(0).Visible = False
        DataGridView1.Columns(1).HeaderText = "Nombre"
        DataGridView1.Columns(1).Width = 250
        DataGridView1.Columns(2).HeaderText = "Municipio Actual"
        DataGridView1.Columns(2).Width = 250
        DataGridView1.Columns(3).HeaderText = "Provincia"
        DataGridView1.Columns(3).Width = 120

        DataGridView1.Columns(4).HeaderText = "INE histórico"
        DataGridView1.Columns(4).Width = 100
        DataGridView1.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        DataGridView1.Columns(5).HeaderText = "INE actual"
        DataGridView1.Columns(5).Width = 100
        DataGridView1.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridView1.Columns(5).Visible = False

        DataGridView1.Columns(6).HeaderText = "Tipo"
        DataGridView1.Columns(6).Width = 120
        DataGridView1.Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

        DataGridView1.Columns(7).HeaderText = "Jurisdicción"
        DataGridView1.Columns(7).Width = 130
        DataGridView1.Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridView1.Columns(7).Visible = False

        DataGridView1.Columns(8).HeaderText = "Nombre mostrado"
        DataGridView1.Columns(8).Width = 130
        DataGridView1.Columns(8).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridView1.Columns(8).Visible = False

        DataGridView1.Columns(9).HeaderText = "Nombre en CdD"
        DataGridView1.Columns(9).Width = 130
        DataGridView1.Columns(9).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridView1.Columns(9).Visible = False

        'Ocultamos el resto de columnas
        For iCol = 10 To DataGridView1.ColumnCount - 1
            DataGridView1.Columns(iCol).Visible = False
        Next


        'Activamos las columnas que pueden mostrarse y apagarse
        RenombrarItemsColumnas()

        'Autoajustamos las columnas
        ResizeDatagridView()

        DataGridView1.RowsDefaultCellStyle.BackColor = Color.White
        DataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue

        ToolStripStatusLabel1.Text = "Número de municipios históricos: " & DataGridView1.RowCount

    End Sub



    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        If altaMuniH.muniHNewDialog = True Then
            Me.Cursor = Cursors.WaitCursor
            CargarListasMunicipios()
            Me.Cursor = Cursors.Default
            ModalInfo("Municipio histórico creado")
        End If
    End Sub


    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click

        Dim muniHEdit As MuniHisto
        Dim indiceGrid As Integer
        Dim idMuni As Integer

        If DataGridView1.CurrentCell.RowIndex < 0 Then Exit Sub

        indiceGrid = DataGridView1.CurrentCell.RowIndex
        idMuni = DataGridView1.Item("idterritorio", indiceGrid).Value.ToString
        If idMuni = 0 Then Exit Sub
        muniHEdit = New MuniHisto(idMuni)

        If muniHEdit.tipo <> "Municipio histórico" Then
            ModalExclamation("Sólo pueden modificarse territorios históricos")
            Exit Sub
        End If

        If altaMuniH.muniHEditorDialog(muniHEdit) = True Then
            Me.Cursor = Cursors.WaitCursor
            CargarListasMunicipios()
            Me.Cursor = Cursors.Default
            ModalInfo("Municipio histórico modificado")
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

    Private Sub asignarEnlaceCDD(sender As Object, e As EventArgs) Handles ddEnlacePlanis.Click, ddEnlacePLEDI.Click, ddEnlacePLPOB.Click, ddEnlaceHKPUP.Click

        Dim linkCdD As String = ""
        Dim indiceGrid As Integer

        If DataGridView1.CurrentCell.RowIndex < 0 Then Exit Sub
        Try

            indiceGrid = DataGridView1.CurrentCell.RowIndex
            Dim idTerritorio As Integer = DataGridView1.Item("idterritorio", indiceGrid).Value
            Dim muniHistoSel As New MuniHisto(idTerritorio)

            If sender.name = "ddEnlacePlanis" Then
                linkCdD = $"{urlCDDSearchEngine}filtro.codFamilia=MIPAC&filtro.codIne={muniHistoSel.municipioINE_LongFormat}"
            ElseIf sender.name = "ddEnlacePLPOB" Then
                linkCdD = $"{urlCDDSearchEngine}filtro.codFamilia=PLPOB&filtro.codIne={muniHistoSel.municipioINE_LongFormat}"
            ElseIf sender.name = "ddEnlacePLEDI" Then
                linkCdD = $"{urlCDDSearchEngine}filtro.codFamilia=PLEDI&filtro.codIne={muniHistoSel.municipioINE_LongFormat}"
            ElseIf sender.name = "ddEnlaceHKPUP" Then
                linkCdD = $"{urlCDDSearchEngine}filtro.codFamilia=HKPUP&filtro.codIne={muniHistoSel.municipioINE_LongFormat}"
            End If
            If linkCdD = "" Then
                ModalExclamation("Enlace no disponible")
            Else
                Process.Start(linkCdD)
            End If
        Catch ex As Exception
            ModalError(ex.Message)
            Exit Sub
        End Try
    End Sub



    Private Sub lvDetails_Click(sender As Object, e As EventArgs) Handles lvDetails.Click

        If Not lvDetails.SelectedItems Is Nothing Then

            Try
                For Each li As ListViewItem In lvDetails.SelectedItems
                    If li.Tag = "URL" Then
                        If li.SubItems(1).Text <> "" Then
                            My.Computer.Clipboard.SetText(li.SubItems(1).Text)
                            If ModalQuestion("URL copiada al portapapeles.¿Abrir en navegador?") = DialogResult.Yes Then
                                Process.Start(li.SubItems(1).Text)
                                Exit For
                            End If
                        End If
                    End If
                Next
            Catch ex As Exception
                ModalError(ex.Message)
            End Try

        End If



    End Sub

    Private Sub btnAjustar_Click(sender As Object, e As EventArgs) Handles btnAjustar.Click

        ResizeDatagridView()

    End Sub


End Class