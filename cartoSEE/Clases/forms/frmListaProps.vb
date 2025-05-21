Public Class frmListaProps

    Public Class DVMDataGridElement
        Property NameField As String
        Property HeaderText As String
        Property Visible As Boolean
        Property Hide_And_show As Boolean
        Property InitialShown As Boolean = True
        Property Width As Integer
        Property FixedWidth As Boolean
        Property AligmentText As DataGridViewContentAlignment
        Property Searchable As Boolean
    End Class


    Property SQLBase As String
    Property PKField As String
    Property OrderByField As String
    Property OrderByDirection As String
    Property LimitResults As Integer
    Property DefinitionColumns As New ArrayList
    Property Hide_And_Show_Columns As New List(Of Integer)
    Property filterFields As New Dictionary(Of String, String)
    Property fieldExternalDocument As String = ""
    Property lstExternalDocument As New ArrayList
    Property ObservacionesField As String

    Dim AutoAjustarColumnas As Boolean
    Dim recordSet As DataView
    Dim TimeFilter As String
    Dim AttributtesFilter As String
    Dim docSelect As docSIDCECAListaProp

#Region "Mostrar Ocultar columnas"

    Sub RenombrarItemsColumnas()

        Dim iCol As Integer
        Dim iMenu As Integer = 0
        Dim nombreCtrl As String

        For Each defCol As DVMDataGridElement In DefinitionColumns
            If defCol.Hide_And_show Then
                iMenu += 1
                nombreCtrl = $"mnuColumna{iMenu}"
                Try
                    Dim tsmi = ToolStripDropDownButton1.DropDownItems.Find(nombreCtrl, True).OfType(Of ToolStripMenuItem).FirstOrDefault()
                    tsmi.Text = defCol.HeaderText
                    tsmi.Checked = defCol.InitialShown
                    tsmi.Tag = defCol.NameField
                Catch ex As Exception
                    ModalError(ex.Message)
                End Try

            End If
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


#End Region

    Sub ResizeDatagridView(Optional AllowFixedColums As Boolean = True)

        Dim fixedCols As Integer = 0
        Dim shownCols As Integer = 0
        Dim calcWidth As Integer
        Dim MaxDatagridWidth As Integer = DataGridView1.Width - 70 'Ancho del control menos el ancho aprox del scroll


        If AutoAjustarColumnas = False Then Exit Sub
        If DataGridView1.Columns.Count = 0 Then Exit Sub

        If Not AllowFixedColums Then
            For Each defCol As DVMDataGridElement In DefinitionColumns
                If DataGridView1.Columns(defCol.NameField).Visible Then shownCols += 1
            Next
            calcWidth = MaxDatagridWidth / shownCols
            For Each defCol As DVMDataGridElement In DefinitionColumns
                DataGridView1.Columns(defCol.NameField).Width = calcWidth
            Next
            Exit Sub
        End If



        For Each defCol As DVMDataGridElement In DefinitionColumns
            If defCol.Hide_And_show And defCol.FixedWidth Then
                If DataGridView1.Columns(defCol.NameField).Visible Then
                    fixedCols += 1
                    MaxDatagridWidth -= defCol.Width
                End If
            End If
            If DataGridView1.Columns(defCol.NameField).Visible Then shownCols += 1
        Next


        If (shownCols - fixedCols) <= 0 Then
            Exit Sub
        End If

        calcWidth = MaxDatagridWidth / (shownCols - fixedCols)



        Try
            For Each defCol As DVMDataGridElement In DefinitionColumns
                If defCol.FixedWidth And DataGridView1.Columns(defCol.NameField).Visible Then
                    DataGridView1.Columns(defCol.NameField).Width = defCol.Width
                Else
                    DataGridView1.Columns(defCol.NameField).Width = calcWidth
                End If
            Next

        Catch ex As Exception
            MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try


    End Sub

    Private Sub mostrarDetalle()

        Dim indiceGrid As Integer
        Dim elementoLV As ListViewItem

        lvDetails.Items.Clear()
        lvVinculos.Items.Clear()
        gBoxdetails.Tag = ""

        If DataGridView1.SelectedRows.Count = 0 Then
            indiceGrid = 0
        Else
            indiceGrid = DataGridView1.CurrentCell.RowIndex
        End If

        For ibucle = 0 To DataGridView1.Columns.Count - 1
            Application.DoEvents()
            If DataGridView1.Columns(ibucle).Name = ObservacionesField Then
                txtObserv.Text = DataGridView1.Item(ibucle, indiceGrid).Value.ToString
                Continue For
            End If
            If DataGridView1.Columns(ibucle).Name = PKField Then
                docSelect = Nothing
                docSelect = New docSIDCECAListaProp(DataGridView1.Item(ibucle, indiceGrid).Value.ToString)
            End If
            elementoLV = New ListViewItem
            elementoLV.Text = DataGridView1.Columns(ibucle).HeaderText
            elementoLV.ImageIndex = 0
            elementoLV.SubItems.Add(DataGridView1.Item(ibucle, indiceGrid).Value.ToString)
            'elementoLV.Group = g1
            lvDetails.Items.Add(elementoLV)
            elementoLV = Nothing
        Next

        elementoLV = New ListViewItem
        elementoLV.Text = "Lista alfabética"
        elementoLV.ImageIndex = 0
        elementoLV.SubItems.Add(SacarFileDeRuta(docSelect.pathListaPropietariosAlfabetica))
        elementoLV.Tag = docSelect.pathListaPropietariosAlfabetica
        lvVinculos.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem
        elementoLV.Text = "Lista numérica"
        elementoLV.ImageIndex = 0
        elementoLV.SubItems.Add(SacarFileDeRuta(docSelect.pathListaPropietariosNumerica))
        elementoLV.Tag = docSelect.pathListaPropietariosAlfabetica
        lvVinculos.Items.Add(elementoLV) : elementoLV = Nothing

        gBoxdetails.Tag = docSelect.IdListaProp

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

    Sub CargarConsulta()

        Dim cadSQL As String = $"{SQLBase}"
        If TimeFilter <> "" Then
            cadSQL &= $" WHERE idlistaprop>0 AND t2.tipo='Municipio' AND {TimeFilter}"
        Else
            cadSQL &= $" WHERE idlistaprop>0 AND t2.tipo='Municipio'"
            ToolStripStatusLabel4.Text = Date.Now.ToString
        End If

        cadSQL &= $"{AttributtesFilter}"

        cadSQL &= IIf(OrderByField <> "", $" ORDER BY {OrderByField} {OrderByDirection}", "")
        cadSQL &= IIf(LimitResults > 0, $" LIMIT {LimitResults}", "")

        recordSet = New DataView
        If CargarDataView(cadSQL, recordSet) = False Then
            MessageBox.Show("No se puede cargar los datos", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        DataGridView1.DataSource = recordSet

        For Each field As DVMDataGridElement In DefinitionColumns
            DataGridView1.Columns(field.NameField).HeaderText = field.HeaderText
            DataGridView1.Columns(field.NameField).Width = field.Width
            DataGridView1.Columns(field.NameField).Visible = field.Visible
            DataGridView1.Columns(field.NameField).DefaultCellStyle.Alignment = field.AligmentText
        Next

        RenombrarItemsColumnas()

        AutoAjustarColumnas = True

        ToolStripStatusLabel1.Text = $"Resultados: {DataGridView1.Rows.Count}"

        ResizeDatagridView()

    End Sub

    Private Sub dataViewer_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.Size = New Point(875, 480)
        AutoAjustarColumnas = False
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
        lvVinculos.View = View.Details
        lvVinculos.Columns.Add("Nombre", 150, HorizontalAlignment.Left)
        lvVinculos.Columns.Add("Fichero", 200, HorizontalAlignment.Left)


        TabControl1.ImageList = ImageList2
        TabControl1.TabPages(0).ImageIndex = 4
        TabControl1.TabPages(1).ImageIndex = 6
        gBoxdetails.Visible = False

        ToolStripStatusLabel1.Text = ""
        ToolStripStatusLabel2.Text = ""
        ToolStripStatusLabel3.Text = ""
        ToolStripStatusLabel4.Text = ""

        CargarConsulta()
        cboFiltros.Items.Add(New itemData("(Todos)", ""))


        For Each defCol As DVMDataGridElement In DefinitionColumns
            If defCol.Searchable Then
                cboFiltros.Items.Add(New itemData(defCol.HeaderText, defCol.NameField))
            End If
        Next

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
            ToolStripDropDownButton1.Enabled = True
        ElseIf sender.name = "btnDetalles" Then
            gBoxdetails.Visible = True
            mostrarDetalle()
            ToolStripDropDownButton1.Enabled = False
            ResizeLVDetails()
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
            ResizeLVDetails()
            Me.Cursor = Cursors.Default
        End If


    End Sub

    Private Sub DataGridView1_Click(sender As Object, e As System.EventArgs) Handles DataGridView1.Click
        If DataGridView1.Rows.Count = 0 Then Exit Sub
        If DataGridView1.SelectedRows.Count = 0 Then
            ToolStripStatusLabel2.Text = ""
        Else
            ToolStripStatusLabel2.Text = "Seleccionados: " & DataGridView1.SelectedRows.Count
        End If
    End Sub

    Private Sub DataGridView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView1.DoubleClick
        If IsNothing(DataGridView1.CurrentCell) Then Exit Sub
        DataGridView1.Visible = False
        gBoxdetails.Visible = True
        mostrarDetalle()
        ResizeLVDetails()
    End Sub



    Private Sub ResizeDatagridView(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView1.Resize

        ResizeDatagridView()

    End Sub

    Private Sub ResizeLVDetails()

        If lvDetails.Columns.Count < 2 Then Exit Sub
        lvDetails.Columns(0).Width = 120
        lvDetails.Columns(1).Width = lvDetails.Width - lvDetails.Columns(0).Width - 50

    End Sub

    Private Sub btnAjustar_Click(sender As System.Object, e As System.EventArgs) Handles btnAjustar.Click, btnRefresh.Click

        If sender.name = "btnAjustar" Then
            If btnAjustar.Text = "Ajustar" Then
                btnAjustar.Text = "No ajustar"
                AutoAjustarColumnas = True
                ResizeDatagridView()
            Else
                btnAjustar.Text = "Ajustar"
                AutoAjustarColumnas = False
            End If
        ElseIf sender.name = "btnRefresh" Then
            DataGridView1.DataSource = Nothing
            DataGridView1.Rows.Clear()
            DataGridView1.Columns.Clear()
            LanzarSpinner()
            CargarConsulta()
            CerrarSpinner()
            DataGridView1.Visible = True
            ToolStripDropDownButton1.Enabled = True
        End If

    End Sub

    Private Sub txtSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyUp
        textFilterApply()
    End Sub

    Private Sub btnLaunchDocument_Click(sender As Object, e As EventArgs) Handles btnLaunchDocument.Click

        If gBoxdetails.Visible Then
            TabControl1.SelectedIndex = 1
            Exit Sub
        End If

        If IsNothing(DataGridView1.CurrentCell) Then Exit Sub
        DataGridView1.Visible = False
        gBoxdetails.Visible = False
        ToolStripStatusLabel2.Text = ""
        gBoxdetails.Visible = True
        mostrarDetalle()
        ToolStripDropDownButton1.Enabled = False
        ResizeLVDetails()
        TabControl1.SelectedIndex = 1


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
            ToolStripStatusLabel2.Text = "Generando fichero exportación"
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
        ToolStripStatusLabel2.Text = ""
        Me.Cursor = Cursors.Default
        MessageBox.Show("Fichero de exportación generado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnSetFilterFechas_Click(sender As Object, e As EventArgs) Handles btnQueryParcelas.Click

        Dim indiceGrid As Integer
        Dim docParce As docSIDCECAListaProp
        If DataGridView1.SelectedRows.Count = 0 Then
            indiceGrid = 0
        Else
            indiceGrid = DataGridView1.CurrentCell.RowIndex
        End If
        DataGridView1.Item(PKField, indiceGrid).Value.ToString()

        If gBoxdetails.Tag <> DataGridView1.Item(PKField, indiceGrid).Value.ToString() Then
            docParce = New docSIDCECAListaProp(DataGridView1.Item(PKField, indiceGrid).Value)
        Else
            docParce = docSelect
        End If

        'ModalInfo($"Parcelas del municipio {docParce.CodMuniHisto}, colección {docParce.NumeroColeccion}")


        Try
            Me.Cursor = Cursors.WaitCursor
            LanzarSpinner()
            Dim frmResultadosSIDCECA As New resultSIDCECA
            With frmResultadosSIDCECA
                .MdiParent = MDIPrincipal
                .paramSQL1 = docParce.CodMuniHisto
                .paramSQL2 = docParce.NumeroColeccion
                .datasetTbL = docParce.DatasetTbl
                If .datasetTbL = "parcelasdata" Then
                    .OrderField = "parcelasdata.idparceladata"
                    .PKField = "idparceladata"
                End If
                If .datasetTbL = "parcelasdatos" Then
                    .OrderField = "parcelasdatos.idparceladato"
                    .PKField = "idparceladato"
                End If
                .typeSearch = resultSIDCECA.TypeDataSearch.AllDocsByCodMuniHistoAndNumCol
                .Text = $"Documentos asociados a la colección {docParce.NombreColeccion} de {docParce.MunicipioHistorico} en {docParce.NombreProvincia}"
                .Show()
            End With
        Catch ex As Exception
            ModalError($"No se pueden identificar los documento: {ex.Message}")
        Finally
            CerrarSpinner()
            Me.Cursor = Cursors.Default
        End Try


    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs)
        TimeFilter = ""
        AttributtesFilter = ""
        LanzarSpinner()
        CargarConsulta()
        CerrarSpinner()

    End Sub



    Private Sub DataGridView1_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DataGridView1.CellFormatting

        Dim alpha As Double = 1.0

        If DataGridView1.CurrentCell Is Nothing Then Return

        If DataGridView1.Columns(e.ColumnIndex).Name = "aplicacion" Then
            'Así podemos definir un color de fondo en función de un valor
            If e.Value.ToString.ToUpper = "SIDDAE" Then
                e.CellStyle.BackColor = Color.FromArgb(160, 206, 217)
            ElseIf e.Value.ToString.ToUpper = "NOMENMANAGER" Then
                e.CellStyle.BackColor = Color.FromArgb(255, 182, 193)
            ElseIf e.Value.ToString.ToUpper = "SIDDES" Then
                e.CellStyle.BackColor = Color.FromArgb(173, 247, 182)
            ElseIf e.Value.ToString.ToUpper = "CARTOSEE" Then
                e.CellStyle.BackColor = Color.FromArgb(255, 238, 147)
            ElseIf e.Value.ToString.ToUpper = "MAPSEE 2" Then
                e.CellStyle.BackColor = Color.FromArgb(255, 165, 0)
            ElseIf e.Value.ToString.ToUpper = "GEOLIM" Then
                e.CellStyle.BackColor = Color.FromArgb(135, 206, 250)
            Else
                e.CellStyle.BackColor = Color.White
            End If
        End If


    End Sub


#Region "Manejo de los recursos asociados al documento"

    Private Sub lvVinculos_DoubleClick(sender As Object, e As EventArgs) Handles lvVinculos.DoubleClick

        If lvVinculos.SelectedItems.Count = 0 Then Exit Sub
        If lvVinculos.SelectedItems.Count > 1 Then ModalExclamation("Seleccione un único recurso")

        Dim RutaFichero As String = lvVinculos.SelectedItems(0).Tag
        Me.Cursor = Cursors.WaitCursor
        LanzarVisorExterno(RutaFichero)
        Me.Cursor = Cursors.Default

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        If lvVinculos.SelectedItems.Count = 0 Then Exit Sub
        If lvVinculos.SelectedItems.Count > 1 Then ModalExclamation("Seleccione un único recurso")

        Try
            Me.Cursor = Cursors.WaitCursor
            If IO.File.Exists(lvVinculos.SelectedItems(0).Tag) Then
                Process.Start(SacarDirDeRuta(lvVinculos.SelectedItems(0).Tag))
            End If
        Catch ex As Exception
            ModalError($"Error: {ex.Message}")
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub lvVinculos_ItemDrag(sender As Object, e As ItemDragEventArgs) Handles lvVinculos.ItemDrag

        Dim strPath, strName, strFullPath As String

        'Put the dragged ListView Item or Items into a variable
        Dim items As ListView.SelectedListViewItemCollection = lvVinculos.SelectedItems

        'Create a string list of File and Folder Paths
        Dim DropList As New System.Collections.Specialized.StringCollection

        ' The "DataObject"...critical to the operation
        Dim DragPaths As New DataObject()

        For Each item As ListViewItem In items
            strName = item.Text
            strFullPath = item.Tag
            '...."FileInfo".... Another critical type
            Dim FtoDrop As System.IO.FileInfo = New System.IO.FileInfo(strFullPath)

            'DropList...StringCollection
            DropList.Add(strFullPath)
        Next

        'Now we use the ".SetFileDropList()" Method to set the 
        'File drop list into the "DataObject"
        DragPaths.SetFileDropList(DropList)

        'The standard dragdrop method. If you do not need to 
        'limit dragdrop effects for a reason, then set to "All"
        DoDragDrop(DragPaths, DragDropEffects.Copy)

        'At this point we can drag from our listviews, into
        'windows explorer and move files around

    End Sub

    Private Sub lvVinculos_DragDrop(sender As Object, e As DragEventArgs) Handles lvVinculos.DragDrop

        Dim Paths As String() = DirectCast(e.Data.GetData(DataFormats.FileDrop), String())

        'To get the destination path, find out which ListView
        'called this Event, then use the appropiate class variable
        Dim strDestination As String

        'iterate through the path strings
        For Each path As String In Paths

            'The following "If" block will determine if the 
            'path is a file or directory
            If System.IO.File.Exists(path) Then

                'This same method was used to populate 
                'the ListView
                Dim _file As System.IO.FileInfo = New System.IO.FileInfo(path)
                Dim _fileName As String = _file.Name

                'Create a path from the peices
                Dim newPath As String = strDestination & "\" & _fileName
                Try
                    System.IO.File.Copy(path, newPath)

                Catch ex As Exception
                    ' This is not about error handlers so we 
                    ' will just ignore the error and keep on
                    ' going
                    Continue For
                End Try
                'If it isn't a file, then it should be a 
                'directory
            ElseIf System.IO.Directory.Exists(path) Then

                Dim dir As System.IO.DirectoryInfo = New System.IO.DirectoryInfo(path)
                Dim dirName As String = dir.Name
                Dim newPath As String = strDestination & "\" & dirName

                Try
                    System.IO.Directory.Move(path, newPath)
                Catch ex As Exception
                    Continue For
                End Try
            End If

        Next

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        If lvVinculos.SelectedItems.Count = 0 Then Exit Sub
        If lvVinculos.SelectedItems.Count > 1 Then ModalExclamation("Seleccione un único recurso")

        Dim filePath As String = lvVinculos.SelectedItems(0).Tag
        Dim infoDescript As String

        Try
            If System.IO.File.Exists(filePath) Then
                Dim file As New System.IO.FileInfo(filePath)
                Dim nombre As String = file.Name
                Dim rutaCompleta As String = file.FullName
                Dim sizeFile As Long = file.Length
                Dim fechaCreacion As DateTime = file.CreationTime
                Dim fechaModificacion As DateTime = file.LastWriteTime
                infoDescript = $"Nombre: {nombre}
                        Tamaño (bytes): {sizeFile}
                        Fecha de creación: {fechaCreacion}
                        Fecha de modificación: {fechaModificacion}"

                ModalInfo(infoDescript)
            Else
                ModalExclamation($"No se encuentra el fichero {ModalInfo(infoDescript)}")
            End If
        Catch ex As Exception
            ModalError(ex.Message)
        End Try



    End Sub




#End Region

End Class