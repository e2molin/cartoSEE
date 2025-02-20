Public Class resultCMTN
    Public Enum TypeDataSearch As Integer
        AllDocuments = 0                                'OK
        AllDocumentsByTerritorio = 1                    'OK
        AllDocumentsByProvincia = 2                     'OK
        AllDocumentsByTerritorioActual = 3
        AllDocsByFechaUpdate = 4
        AllDocsPorFechaUpdateEntreFechas = 5
        AllDocsPorFechaAltaEntreFechas = 6
        DocumentosFiltroGenerico = 7
        DocumentosBySellado = 8
        DocumentosByListaNumSellado = 9
        DocumentosByListaNumSelladoEntreLimites = 10
        DocumentosBySignatura = 11
        DocumentosByAnejo = 12
        DocumentosByColeccion = 13
        DocumentosByObservacion = 14
        DocumentosByComentario = 15
        DocumentosByPatron = 16
        DocumentosByBBOX = 17
        DocumentosByProcHojaCarpeta = 18
        DocumentosEnCarrito = 19
        DocumentoByIndice = 20
    End Enum

    Enum modeView As Integer
        PanelClose = 0
        PanelOpen = 1
    End Enum

    Property nameQuery As String
    Property EsCarritoCompra As Boolean = False
    Property typeSearch As TypeDataSearch
    Property authoritySearch As String
    Property sqlBase As String
    Property sqlSearchApplyed As String = ""
    Property sqlFilterDate As String = ""
    Property columnVisualiz As Integer = 0
    Property paramSQL1 As String
    Property paramSQL2 As String
    Property paramSQL3 As String

    'Filtros sobre los resultados
    Property filterTipoDoc As String = ""
    Property filterEstadoDoc As String = ""
    Property filterSubTipoDoc As String = ""
    Property filterTomo As String = ""
    Property filterFecha As String = ""
    Property filterObservaciones As String = ""
    Property filterEnABSYS As String = ""
    Property filterJGE As String = ""


    Dim rcdDataPrin As DataView
    Dim elemEntidadSel As docCuadMTN
    Dim useEnterOnFilter As Boolean
    Dim minRows4useEnterOnFilter As Integer = 50 'Número de resultados a partir de los cuales hay que pulsar Enter para buscar.

#Region "Columnas datagrid"
    'archivo.idarchivo,             0   Oculta siempre					archivo.idarchivo,
    'archivo.numdoc,                1   Visible siempre					archivo.numdoc,
    'tipo,                          2   Visible inicial					tbtipodocumento.tipodoc as tipo,
    'subtipo,                       3   Visible inicial					archivo.subtipo,
    'archivo.tomo,                  4   Visible inicial					archivo.tomo,
    'archivo.escala,                5   Visible inicial					archivo.escala,
    'fechaprincipal,                6   Visible inicial					to_char(archivo.fechaprincipal, 'YYYY-MM-DD') as fechaprincipal,
    'listaMuniHisto,                7   Visible inicial					string_agg(territorios.nombre,', ') as listaMuniHisto,
    'archivo.signatura,             8   Visible inicial					archivo.signatura,
    'dimensiones,                   9   Visible inicial					archivo.horizontal || ' cm × '|| archivo.vertical || ' cm' as dimensiones,
    'archivo.coleccion,             10  Visible inicial					archivo.coleccion,
    'archivo.subdivision,           11  Visible inicial					archivo.subdivision,
    'estado,                        12  Visible inicial					tbestadodocumento.estadodoc as Estado,
    'archivo.fechasmodificaciones,  13	Oculto inicio					archivo.fechasmodificaciones,
    'archivo.observaciones,         14	Oculto inicio					archivo.observaciones,
    'archivo.proyecto,              15	Oculto inicio					archivo.observaciones,
    'archivo.anejo,                 16	Oculto inicio					archivo.anejo,
    'archivo.procecarpeta,          17	Oculto inicio					archivo.procecarpeta,
    'archivo.procehoja,             18									archivo.procehoja,
    'archivo.juntaestadistica,      19	Oculto inicio					archivo.juntaestadistica,
    'archivo.extraprops,            20  Oculto inicio					archivo.extraprops,
    'archivo.cdd_url,               21									archivo.cdd_url,
    'archivo.titn,                  22									archivo.titn,
    'archivo.autor,                 23  Oculto inicio					archivo.autor,
    'archivo.encabezado,            24									archivo.encabezado,
    'nombreprovincia,               25  Oculto inicio					string_agg(provincias.nombreprovincia,'#') as nombreprovincia,
    'listaCodMuniHisto,             26  Oculto inicio					string_agg(to_char(territorios.munihisto, 'FM0000009'::text),'#') as listaCodMuniHisto,
    'listaMuniActual,               27									string_agg(listamunicipios.nombre,'#') as listaMuniActual,
    'listaCodMuniActual,            28									string_agg(listamunicipios.inecorto,'#') as listaCodMuniActual
#End Region

    Const widthScrollLV As Integer = 30
    Dim FixedCols() As Integer = {0, 1} 'Columnas con ancho fijo aunque crezca el tamaño del datagrid
    Dim Hide_And_Show_Columns() As Integer = {2, 3, 4, 5, 6, 7, 8, 9} ' Índices de columnas que pueden mostrarse u ocultarse

    Dim idArchivoLoaded As Integer = 0
    Dim idArchiveTagsLoaded As Integer = 0
    Dim idArchiveResourcesLoaded As Integer = 0
    Dim gettingDetails As Boolean = False
    Dim cancelDetails As Boolean = False

#Region "Mostrar Ocultar columnas"

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


#End Region

    Sub TaxonDetailView(mode As modeView)

        If DataGridView1.RowCount = 0 Then Exit Sub

        If mode = modeView.PanelClose Then
            TableLayoutPanel1.RowStyles(0).SizeType = SizeType.Percent
            TableLayoutPanel1.RowStyles(0).Height = 100
            TableLayoutPanel1.RowStyles(1).SizeType = SizeType.Percent
            TableLayoutPanel1.RowStyles(1).Height = 0
            ToolStripButton1.Enabled = False
        ElseIf mode = modeView.PanelOpen Then
            TableLayoutPanel1.RowStyles(0).SizeType = SizeType.Percent
            TableLayoutPanel1.RowStyles(0).Height = 50
            TableLayoutPanel1.RowStyles(1).SizeType = SizeType.Percent
            TableLayoutPanel1.RowStyles(1).Height = 50
            ToolStripButton1.Enabled = True
            If DataGridView1.CurrentCell.RowIndex <> -1 Then
                If Not DataGridView1.Rows(DataGridView1.CurrentCell.RowIndex).Displayed Then
                    DataGridView1.FirstDisplayedScrollingRowIndex = DataGridView1.CurrentCell.RowIndex
                End If

            End If
        End If


    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click, Button5.Click
        TaxonDetailView(modeView.PanelClose)
    End Sub


    Private Sub MostrarDetalle()
        TaxonDetailView(modeView.PanelOpen)
    End Sub


    Private Sub ClearInfoContainers()

        lvFastView.Items.Clear()
        PictureBox1.Image = Nothing

    End Sub


    Private Sub LoadThumb(container As PictureBox)

        Dim thumbImageName As String
        thumbImageName = elemEntidadSel.rutaFicheroThumb

        'Con esta herramienta configuraremos más aedlante cómo generar miniaturas
        'If usuarioMyApp.permisosLista.usuarioISTARI Then
        '    Try
        '        If IO.File.Exists(elemEntidadSel.ficheroPDF) Then
        '            If Not IO.File.Exists(thumbImageName) Then
        '                generaThumbPortada(elemEntidadSel.ficheroPDF, thumbImageName)
        '            End If
        '        End If
        '    Catch ex As Exception
        '        ModalError("Error al generar miniatura")
        '        GenerarLOG(ex.Message)
        '    End Try
        'End If

        container.Image = Nothing
        Try
            If IO.File.Exists(thumbImageName) Then
                cargarImagenFromWeb(container, thumbImageName, $"{My.Application.Info.DirectoryPath}\resources\thumbmap.jpg", False, elemEntidadSel.rutaFicheroThumb)
            End If
        Catch ex As Exception
            GenerarLOG(ex.Message)
            ModalError(ex.Message)
        End Try

    End Sub

    Private Sub FillRichText(container As RichTextBox, rowIdx As Integer)

        With container
            .Clear()

            'Con esto aplica un poco de padding en el interior del RichTextBox1
            .SelectAll()
            .SelectionIndent = 5
            .SelectionRightIndent = 5
            .SelectionLength = 0

            AddTitulo(container, $"{elemEntidadSel.Tipo} Nº {elemEntidadSel.Sellado}")

            .SelectionFont = New Font("Segoe UI Semibold", 10, FontStyle.Bold)
            .AppendText($"{elemEntidadSel.ProvinciaNombre}. Tomo: {elemEntidadSel.Tomo}.Fecha: {elemEntidadSel.FechaDoc}{Environment.NewLine}")
            .AppendText(Environment.NewLine)

            .SelectionColor = Color.FromArgb(47, 79, 79) 'DimSlateGray
            .AppendText(IIf(elemEntidadSel.Subtipo = "", "", $"Subtipo: {elemEntidadSel.Subtipo}"))
            .AppendText(Environment.NewLine)
            AddSubTitulo(container, "Contenido")
            .AppendText(IIf(elemEntidadSel.Contenido = "", "", $"{elemEntidadSel.Contenido}{Environment.NewLine}"))

            AddSubTitulo(container, "Notas / Observaciones originales")
            .SelectionColor = Color.FromArgb(47, 79, 79) 'DimSlateGray
            .SelectionFont = New Font("Segoe UI Semibold", 10, FontStyle.Bold)

            If elemEntidadSel.Observaciones <> "" Then
                .AppendText(IIf(elemEntidadSel.Observaciones <> "", elemEntidadSel.Observaciones, "No hay observaciones"))
                .AppendText(Environment.NewLine)
            End If

        End With


    End Sub

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

    Sub ResizeListViews()

        With lvFastView
            If .Columns.Count < 1 Then Exit Sub
            If .Columns.Count = 2 Then
                .Columns(0).Width = 150
                .Columns(1).Width = .Width - .Columns(0).Width - widthScrollLV
            End If
        End With

        With lvTagsM21
            If .Columns.Count < 1 Then Exit Sub
            If .Columns.Count = 2 Then
                .Columns(0).Width = 150
                .Columns(1).Width = .Width - .Columns(0).Width - widthScrollLV
            End If
        End With

        With lvDocEditions
            If .Columns.Count < 1 Then Exit Sub
            If .Columns.Count = 5 Then
                .Columns(0).Width = 140
                .Columns(1).Width = 80
                .Columns(2).Width = (.Width - .Columns(0).Width - .Columns(1).Width - widthScrollLV) / 3
                .Columns(3).Width = (.Width - .Columns(0).Width - .Columns(1).Width - widthScrollLV) / 3
                .Columns(4).Width = (.Width - .Columns(0).Width - .Columns(1).Width - widthScrollLV) / 3
            End If

        End With

    End Sub


    Private Sub FilterApplyOnDatagrid()

        Dim cadFiltro As String = ""
        Dim cadFiltroParte As String = ""
        Dim ffilter As String

        Me.Cursor = Cursors.WaitCursor


        If cboFields.SelectedIndex = -1 Then
            ffilter = "All"
        Else
            ffilter = CType(cboFields.SelectedItem, itemData).Valor
        End If

        Me.Cursor = Cursors.WaitCursor

        If txtFiltro.Text.Trim = "" Then
            If sqlFilterDate = "" Then
                rcdDataPrin.RowFilter = ""
            Else
                rcdDataPrin.RowFilter = sqlFilterDate
            End If
            DataGridView1.DataSource = rcdDataPrin
            DataGridView1.Update()
            If TabControl1.SelectedIndex = 0 Then
                ToolStripStatusLabel2.Text = ""
                If sqlFilterDate <> "" Then ToolStripStatusLabel2.Text = "Filtrados: " & DataGridView1.RowCount
            End If
            Me.Cursor = Cursors.Default
            Exit Sub
        End If

        If ffilter = "procehoja" Then
            cadFiltro = "CONVERT(" & ffilter & ", 'System.String') LIKE '" & txtFiltro.Text.Trim.Replace("'", "''") & "%'"
        ElseIf ffilter = "All" Then
            For Each cboItem In cboFields.Items
                cadFiltroParte = ""
                If CType(cboItem, itemData).Valor = "All" Then Continue For
                If CType(cboItem, itemData).Valor = "procehoja" Then
                    cadFiltroParte = "CONVERT(procehoja, 'System.String') LIKE '" & txtFiltro.Text.Trim.Replace("'", "''") & "%'"
                End If
                If cadFiltro = "" Then
                    cadFiltro = CType(cboItem, itemData).Valor & " like '%" & txtFiltro.Text.Trim.Replace("'", "''") & "%' "
                    Continue For
                End If
                If cadFiltroParte = "" Then
                    cadFiltro &= " OR " & CType(cboItem, itemData).Valor & " like '%" & txtFiltro.Text.Trim.Replace("'", "''") & "%' "
                Else
                    cadFiltro &= " OR " & cadFiltroParte
                End If

                Application.DoEvents()
            Next
        Else
            cadFiltro = ffilter & " like '%" & txtFiltro.Text.Trim.Replace("'", "''") & "%' "
        End If

        If cadFiltro = "" Then Exit Sub
        sqlSearchApplyed = cadFiltro

        cancelDetails = True
        Try
            If sqlFilterDate = "" Then
                rcdDataPrin.RowFilter = sqlSearchApplyed
            Else
                rcdDataPrin.RowFilter = sqlFilterDate & " and (" & sqlSearchApplyed & ")"
            End If
            DataGridView1.DataSource = rcdDataPrin
            DataGridView1.Update()
        Catch ex As Exception
            Application.DoEvents()
        End Try

        cancelDetails = False
        If DataGridView1.Rows.Count > 0 Then
            FillDetailsReduced(DataGridView1.Item("idarchivo", 0).Value.ToString)
        End If
        ToolStripStatusLabel2.Text = "Filtrados: " & DataGridView1.RowCount

        Me.Cursor = Cursors.Default

    End Sub

    Private Sub CargarConsulta()

        Dim complexFilter As String

        Me.Cursor = Cursors.WaitCursor
        If typeSearch = TypeDataSearch.AllDocuments Then
            FillDocCuadMTNEwithFilter("")
            Me.Text = "Todos los documentos"
        ElseIf typeSearch = TypeDataSearch.AllDocumentsByTerritorio Then
            FillDocCuadMTNEwithFilter($"archivo.idarchivo in (select archivo_id from bdsidschema.archivo2territorios where territorio_id={paramSQL1})")

        ElseIf typeSearch = TypeDataSearch.AllDocumentsByTerritorioActual Then
            FillDocCuadMTNEwithFilter($"archivo.idarchivo in (SELECT DISTINCT archivo_id 
    	                                FROM bdsidschema.archivo2territorios 
	                                    INNER JOIN bdsidschema.territorios ON archivo2territorios.territorio_id=territorios.idterritorio
	                                    WHERE territorios.municipio={paramSQL1})")
        ElseIf typeSearch = TypeDataSearch.AllDocumentsByProvincia Then
            If paramSQL1.ToString = "" Then
                ModalExclamation("Búsqueda por provincia no definida")
                Exit Sub
            End If
            If paramSQL1 = "0" Then
                FillDocCuadMTNEwithFilter($"archivo.provincia_id>0")
                Me.Text = $"Documentos CartoSEE de todas las provincias"
            Else
                FillDocCuadMTNEwithFilter($"archivo.provincia_id={paramSQL1}")
                Me.Text = $"Documentos CartoSEE de {DameProvinciaByINE(paramSQL1)}"
            End If
        ElseIf typeSearch = TypeDataSearch.DocumentosBySellado Then
            If paramSQL1.ToString = "" Then
                ModalExclamation("Búsqueda por número de sellado no definida")
                Exit Sub
            End If
            FillDocCuadMTNEwithFilter($"archivo.numdoc='{paramSQL1}'")
            Me.Text = $"Documento SIDDAE sellado nº {paramSQL1}"
        ElseIf typeSearch = TypeDataSearch.DocumentosByListaNumSellado Then
            If paramSQL1.ToString = "" Then
                ModalExclamation("Búsqueda por números de sellado no definida")
                Exit Sub
            End If
            For Each elem As String In paramSQL1.ToString.Split(",")
                paramSQL2 &= IIf(paramSQL2 = "", $"'{elem.Replace("'", "")}'", $",'{elem.Replace("'", "")}'")
            Next
            FillDocCuadMTNEwithFilter($"archivo.numdoc In ({paramSQL2})")
            Me.Text = $"Documentos con los nº de sellado: {paramSQL1}"
        ElseIf typeSearch = TypeDataSearch.DocumentosByListaNumSelladoEntreLimites Then
            If paramSQL1.ToString = "" Or paramSQL2.ToString = "" Then
                ModalExclamation("Búsqueda entre números de sellado no definida")
                Exit Sub
            End If
            FillDocCuadMTNEwithFilter($"archivo.numdoc >= '{paramSQL1}' and archivo.numdoc <= '{paramSQL2}'")
            Me.Text = $"Documentos con nº de sellado comprendidos entre {paramSQL1} y {paramSQL2}"
        ElseIf typeSearch = TypeDataSearch.DocumentosBySignatura Then
            If paramSQL1.ToString = "" Then
                ModalExclamation("No se ha definido ninguna signatura")
                Exit Sub
            End If
            FillDocCuadMTNEwithFilter($"archivo.signatura = '{paramSQL1}'")
            Me.Text = $"Documentos con signatura {paramSQL1}"
        ElseIf typeSearch = TypeDataSearch.DocumentosByAnejo Then
            If paramSQL1.ToString = "" Then
                ModalExclamation("No se ha definido ningún anejo")
                Exit Sub
            End If
            FillDocCuadMTNEwithFilter($"archivo.anejo ilike '%{paramSQL1}%'")
            Me.Text = $"Documentos con el anejo {paramSQL1}"
        ElseIf typeSearch = TypeDataSearch.DocumentosByColeccion Then
            If paramSQL1.ToString = "" Then
                ModalExclamation("No se ha especificado ninguna colección")
                Exit Sub
            End If
            FillDocCuadMTNEwithFilter($"archivo.coleccion ilike '%{paramSQL1}%'")
            Me.Text = $"Documentos asociados a la colección «{paramSQL1}»"
        ElseIf typeSearch = TypeDataSearch.DocumentosByComentario Then
            If paramSQL1.ToString = "" Then
                ModalExclamation("No se ha especificado ningún comentario")
                Exit Sub
            End If
            FillDocCuadMTNEwithFilter($"(archivo.observ ilike E'%{paramSQL1.Replace("'", "\'")}%' or archivo.observaciones ilike E'%{paramSQL1.Replace("'", "\'")}%')")
            Me.Text = $"Documentos con el comentario «{paramSQL1}»"
        ElseIf typeSearch = TypeDataSearch.DocumentosByPatron Then
            If paramSQL1.ToString = "" Then
                ModalExclamation("Búsqueda por patrón no definida")
                Exit Sub
            End If
            complexFilter = $"archivo.idarchivo in (
                                with dataprops as (
                                            select idarchivo,
                                            CASE 
                                                WHEN bdsidschema.number_to_base(extraprops,2) Is null THEN repeat('0',16) 
                                                ELSE repeat('0',16 - length(bdsidschema.number_to_base(extraprops,2))) || bdsidschema.number_to_base(extraprops,2) 
                                            END as patron 
                                            from bdsidschema.archivo 
                                            INNER JOIN bdsidschema.archivo2territorios ON archivo2territorios.archivo_id=archivo.idarchivo 
                                            INNER JOIN bdsidschema.territorios on territorios.idterritorio= archivo2territorios.territorio_id 
                                            LEFT JOIN ngmepschema.listamunicipios on territorios.nomen_id= listamunicipios.identidad 
                                            WHERE idarchivo>0 
                                ) select idarchivo from dataprops where patron like '{paramSQL1}'
                             )"
            FillDocCuadMTNEwithFilter(complexFilter)

        ElseIf typeSearch = TypeDataSearch.DocumentosByProcHojaCarpeta Then
            Dim hoja As Integer
            If paramSQL1.ToString = "" Then
                ModalExclamation("Búsqueda por hoja no definida")
                Exit Sub
            End If
            If Not Integer.TryParse(paramSQL1, hoja) Then
                ModalExclamation("La hoja debe ser un número")
                Exit Sub
            End If
            If paramSQL2.ToString = "" Then
                FillDocCuadMTNEwithFilter($"archivo.procehoja={paramSQL1}")
                Me.Text = $"Documentos de la hoja «{paramSQL1}»"
            Else
                FillDocCuadMTNEwithFilter($"archivo.procehoja={paramSQL1} and procecarpeta='{paramSQL2}'")
                Me.Text = $"Documentos de la hoja «{paramSQL1}», carpeta «{paramSQL2}»"
            End If
        ElseIf typeSearch = TypeDataSearch.DocumentosByBBOX Then
            If paramSQL1.ToString = "" Or paramSQL2.ToString = "" Then
                ModalExclamation("Búsqueda por entorno no definida")
                Exit Sub
            End If
            complexFilter = $"archivo.idarchivo IN (
                                SELECT archivo_id FROM bdsidschema.contornos 
                                    WHERE ST_Intersects(contornos.geom,ST_GeomFromText({paramSQL1},{paramSQL2}))
                                )"
            FillDocCuadMTNEwithFilter(complexFilter)
            Me.Text = $"Documentos dentro de BBOX({paramSQL1}) en epsg:{paramSQL2}"








            '------------------------------------------------------------------------------------------------------------------------


        ElseIf typeSearch = TypeDataSearch.AllDocsPorFechaAltaEntreFechas Then
            If paramSQL1.ToString = "" Or paramSQL2.ToString = "" Then
                ModalExclamation("Búsqueda por fecha de alta no definida correctamente")
                Exit Sub
            End If
            FillDocCuadMTNEwithFilter($"WHERE docsiddae.fecha_alta between '{paramSQL1}' AND '{paramSQL2}'")
            If nameQuery <> "" Then Me.Text = nameQuery
        ElseIf typeSearch = TypeDataSearch.AllDocsByFechaUpdate Then
            If paramSQL1.ToString = "" Then
                ModalExclamation("Búsqueda por fecha de actualización no definida correctamente")
                Exit Sub
            End If
            FillDocCuadMTNEwithFilter($"WHERE docsiddae.fechamodificacion = '{paramSQL1}'")
            If nameQuery <> "" Then Me.Text = nameQuery
        ElseIf typeSearch = TypeDataSearch.AllDocsPorFechaUpdateEntreFechas Then
            If paramSQL1.ToString = "" Or paramSQL2.ToString = "" Then
                ModalExclamation("Búsqueda por fecha de actualización no definida correctamente")
                Exit Sub
            End If
            FillDocCuadMTNEwithFilter($"WHERE docsiddae.fechamodificacion between '{paramSQL1}' AND '{paramSQL2}'")
            If nameQuery <> "" Then Me.Text = nameQuery
        ElseIf typeSearch = TypeDataSearch.DocumentosFiltroGenerico Then
            If paramSQL1.ToString = "" Then
                ModalExclamation("No se ha definido un filtro válido")
                Exit Sub
            End If
            If Not paramSQL1.ToLower.StartsWith("where ") Then paramSQL1 = $"WHERE {paramSQL1}"
            FillDocCuadMTNEwithFilter(paramSQL1)
            If nameQuery <> "" Then Me.Text = nameQuery

        ElseIf typeSearch = TypeDataSearch.DocumentoByIndice Then
            If paramSQL1.ToString = "" Then
                ModalExclamation("Búsqueda por número de índice no definida")
                Exit Sub
            End If
            FillDocCuadMTNEwithFilter($"where docsiddae.iddocsiddae={paramSQL1}")
            Me.Text = $"Documento SIDDAE con Iddocsiddae nº {paramSQL1}"

        ElseIf typeSearch = TypeDataSearch.DocumentosByComentario Then
            If paramSQL1.ToString = "" Then
                ModalExclamation("Búsqueda por comentario no definido")
                Exit Sub
            End If
            FillDocCuadMTNEwithFilter($"where docsiddae.comentario ilike '%{paramSQL1}%'")
            Me.Text = $"Documentos con comentario: {paramSQL1}"

        ElseIf typeSearch = TypeDataSearch.DocumentosEnCarrito Then
            If CarritoCompra.Count = 0 Then
                ModalExclamation("El carrito está vacío")
                Exit Sub
            End If
            Dim listaCarritoItems As String = String.Join(",", CarritoCompra.ToArray())
            FillDocCuadMTNEwithFilter($"archivo.idarchivo in ({listaCarritoItems})")
            Me.Text = "Carrito de la compra"
            Me.Tag = "Carrito de la Compra"
        End If

        ResizeDatagridView()
        'Si no es usuario ISTARI registramos la consulta
        If Not usuarioMyApp.permisosLista.usuarioISTARI Then
            registrarDatabaseLog($"Consulta: {typeSearch}", $"{paramSQL1} # {paramSQL2}# {paramSQL3}")
        End If

        Me.Cursor = Cursors.Default

    End Sub

    Private Sub FillMarc21Tags(idArchivo As Integer, rowId As Integer)


        Dim elementoLV As ListViewItem
        Dim territorio As String

        If idArchiveTagsLoaded = idArchivo Then Exit Sub


        lvTagsM21.Items.Clear()
        lvDocEditions.Items.Clear()

        Dim docGeneral As ListViewGroup : docGeneral = New ListViewGroup("General") : lvTagsM21.Groups.Add(docGeneral)
        Dim docUbicacion As ListViewGroup : docUbicacion = New ListViewGroup("Localización") : lvTagsM21.Groups.Add(docUbicacion)
        Dim docTerritorios As ListViewGroup : docTerritorios = New ListViewGroup("Territorios") : lvTagsM21.Groups.Add(docTerritorios)
        Dim docCDD As ListViewGroup : docCDD = New ListViewGroup("Centro de descargas") : lvTagsM21.Groups.Add(docCDD)
        Dim docABSYS As ListViewGroup : docABSYS = New ListViewGroup("Catalogación en ABSYS") : lvTagsM21.Groups.Add(docABSYS)


        Application.DoEvents()
        For Each terri As TerritorioBSID In elemEntidadSel.listaTerritorios

            elementoLV = New ListViewItem
            elementoLV.Text = terri.tipo
            elementoLV.Tag = terri.indice
            elementoLV.SubItems.Add(terri.nombre)
            elementoLV.ImageIndex = 4
            elementoLV.Group = docTerritorios
            lvTagsM21.Items.Add(elementoLV)
            elementoLV = Nothing


        Next

        If elemEntidadSel.Anejos <> "" Then
            elementoLV = New ListViewItem
            elementoLV.Text = "Anejos"
            elementoLV.SubItems.Add(elemEntidadSel.Anejos)
            elementoLV.ImageIndex = 4
            elementoLV.Group = docTerritorios
            lvTagsM21.Items.Add(elementoLV)
            elementoLV = Nothing
        End If

        elementoLV = New ListViewItem With {.Text = "Sellado", .ImageIndex = 4, .Group = docGeneral}
        elementoLV.SubItems.Add(elemEntidadSel.Sellado) : lvTagsM21.Items.Add(elementoLV) : elementoLV = Nothing
        elementoLV = New ListViewItem With {.Text = "Tipo", .ImageIndex = 4, .Group = docGeneral}
        elementoLV.SubItems.Add(elemEntidadSel.Tipo) : lvTagsM21.Items.Add(elementoLV) : elementoLV = Nothing
        elementoLV = New ListViewItem With {.Text = "Subtipo", .ImageIndex = 4, .Group = docGeneral}
        elementoLV.SubItems.Add(elemEntidadSel.Subtipo) : lvTagsM21.Items.Add(elementoLV) : elementoLV = Nothing
        elementoLV = New ListViewItem With {.Text = "Encabezado", .ImageIndex = 4, .Group = docGeneral}
        elementoLV.SubItems.Add(elemEntidadSel.Encabezado) : lvTagsM21.Items.Add(elementoLV) : elementoLV = Nothing
        elementoLV = New ListViewItem With {.Text = "Autoría", .ImageIndex = 4, .Group = docGeneral}
        elementoLV.SubItems.Add(elemEntidadSel.AutorEntidad) : lvTagsM21.Items.Add(elementoLV) : elementoLV = Nothing
        elementoLV = New ListViewItem With {.Text = "Observador", .ImageIndex = 4, .Group = docGeneral}
        elementoLV.SubItems.Add(elemEntidadSel.Observador) : lvTagsM21.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem With {.Text = "Itinerario/Perfil", .ImageIndex = 4, .Group = docGeneral}
        elementoLV.SubItems.Add(elemEntidadSel.ItinType) : lvTagsM21.Items.Add(elementoLV) : elementoLV = Nothing
        elementoLV = New ListViewItem With {.Text = "Fecha documento", .ImageIndex = 4, .Group = docGeneral}
        elementoLV.SubItems.Add(elemEntidadSel.FechaDoc) : lvTagsM21.Items.Add(elementoLV) : elementoLV = Nothing
        elementoLV = New ListViewItem With {.Text = "Tipo fecha", .ImageIndex = 4, .Group = docGeneral}
        elementoLV.SubItems.Add(elemEntidadSel.FechaDocType) : lvTagsM21.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem With {.Text = "Provincia", .ImageIndex = 4, .Group = docUbicacion}
        elementoLV.SubItems.Add($"{elemEntidadSel.ProvinciaNombre} ({elemEntidadSel.ProvinciaINE})") : lvTagsM21.Items.Add(elementoLV) : elementoLV = Nothing
        elementoLV = New ListViewItem With {.Text = "Tomo", .ImageIndex = 4, .Group = docUbicacion}
        elementoLV.SubItems.Add(elemEntidadSel.Tomo) : lvTagsM21.Items.Add(elementoLV) : elementoLV = Nothing
        elementoLV = New ListViewItem With {.Text = "Signatura", .ImageIndex = 4, .Group = docUbicacion}
        elementoLV.SubItems.Add(elemEntidadSel.Signatura) : lvTagsM21.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem With {.Text = "Alta GEODOCAT", .ImageIndex = 4, .Group = docGeneral}
        elementoLV.SubItems.Add($"{elemEntidadSel.Create_at}") : lvTagsM21.Items.Add(elementoLV) : elementoLV = Nothing
        elementoLV = New ListViewItem With {.Text = "Creado por", .ImageIndex = 4, .Group = docGeneral}
        elementoLV.SubItems.Add($"{elemEntidadSel.Create_By}") : lvTagsM21.Items.Add(elementoLV) : elementoLV = Nothing


        'elemEntidadSel.cargarHistorial()
        'For Each item As docCartoSEEVariacion In elemEntidadSel.historialCambios
        '    elementoLV = New ListViewItem
        '    elementoLV.Text = item.fechaVariacion
        '    elementoLV.SubItems.Add(item.usuario)
        '    elementoLV.SubItems.Add(item.tipoVariacion)
        '    elementoLV.SubItems.Add(item.valorOld)
        '    elementoLV.SubItems.Add(item.ValorNew)
        '    lvDocEditions.Items.Add(elementoLV)
        '    elementoLV = Nothing
        'Next

        elementoLV = New ListViewItem With {.Text = "Producto", .ImageIndex = 4, .Group = docCDD}
        elementoLV.SubItems.Add(elemEntidadSel.ProductoCDD) : lvTagsM21.Items.Add(elementoLV) : elementoLV = Nothing
        elementoLV = New ListViewItem With {.Text = "Recurso", .ImageIndex = 4, .Group = docCDD}
        elementoLV.SubItems.Add(elemEntidadSel.NameFileCDD) : lvTagsM21.Items.Add(elementoLV) : elementoLV = Nothing
        elementoLV = New ListViewItem With {.Text = "Fecha subida", .ImageIndex = 4, .Group = docCDD}
        elementoLV.SubItems.Add(elemEntidadSel.FechaFileCDD) : lvTagsM21.Items.Add(elementoLV) : elementoLV = Nothing
        elementoLV = New ListViewItem With {.Text = "Enlace CdD", .ImageIndex = 4, .Group = docCDD}
        elementoLV.SubItems.Add(elemEntidadSel.cddURL) : lvTagsM21.Items.Add(elementoLV) : elementoLV = Nothing


        'Rellenamos el cuadro de texto con algunos datos
        FillRichText(RichTextBox2, rowId)

        LoadThumb(PictureBox2)


        idArchiveTagsLoaded = idArchivo
        FillResources(idArchivo, rowId)




    End Sub

    Private Sub FillResources(idArchivo As Integer, rowId As Integer)

        Dim elementoLV As ListViewItem

        If idArchiveResourcesLoaded = idArchivo Then Exit Sub

        LoadThumb(PictureBox3)
        lvDocResources.Items.Clear()
        Dim docDigital As ListViewGroup : docDigital = New ListViewGroup("Documentos digitalizados") : lvDocResources.Groups.Add(docDigital)
        Dim docGeorref As ListViewGroup : docGeorref = New ListViewGroup("Documentos georreferenciados") : lvDocResources.Groups.Add(docGeorref)

        'Repositorio de documento imagen asociado
        '-----------------------------------------------------------------------------------------------------------------
        'elementoLV = New ListViewItem With {
        '        .Text = "Documento Digital",
        '        .ImageIndex = 4,
        '        .Tag = elemEntidadSel,
        '        .Group = docDigital
        '}
        'elementoLV.SubItems.Add(SacarFileDeRuta(elemEntidadSel.rutaFicheroAltaRes))
        'elementoLV.SubItems.Add(".JPG")
        'elementoLV.ForeColor = IIf(Not IO.File.Exists(elemEntidadSel.rutaFicheroAltaRes), Color.Red, Color.DarkGreen)
        'lvDocResources.Items.Add(elementoLV) : elementoLV = Nothing

        'elementoLV = New ListViewItem With {
        '        .Text = "Baja resolución",
        '        .ImageIndex = 4,
        '        .Tag = elemEntidadSel.rutaFicheroBajaRes,
        '        .Group = docDigital
        '}
        'elementoLV.SubItems.Add(SacarFileDeRuta(elemEntidadSel.rutaFicheroBajaRes))
        'elementoLV.SubItems.Add(".JPG")
        'elementoLV.ForeColor = IIf(Not IO.File.Exists(elemEntidadSel.rutaFicheroBajaRes), Color.Red, Color.DarkGreen)
        'lvDocResources.Items.Add(elementoLV) : elementoLV = Nothing

        If IO.File.Exists(elemEntidadSel.rutaFicheroPDF) Then
            elementoLV = New ListViewItem With {
                .Text = "Documento PDF",
                .ImageIndex = 4,
                .Tag = elemEntidadSel.rutaFicheroPDF,
                .Group = docDigital
        }
            elementoLV.SubItems.Add(SacarFileDeRuta(elemEntidadSel.rutaFicheroPDF))
            elementoLV.SubItems.Add(".PDF")
            elementoLV.ForeColor = IIf(Not IO.File.Exists(elemEntidadSel.rutaFicheroPDF), Color.Red, Color.DarkGreen)
            lvDocResources.Items.Add(elementoLV) : elementoLV = Nothing
        End If



        'Así rellenamos usando el escaneo de directorios georreferenciados
        'elemEntidadSel.getGeoFiles()
        'elemEntidadSel.getGeoFilesFromDatabase()

        'Dim pathGeorref As String
        'Dim epsgGeorref As String
        'For Each fila As DataRow In elemEntidadSel.rcdgeoFiles.Select()
        '    pathGeorref = ""
        '    epsgGeorref = ""
        '    For Each geoFichero As FileGeorref In elemEntidadSel.listaFicherosGeo
        '        If geoFichero.NameFile.ToLower = $"{fila.Item("nombre")}.ecw" Then
        '            pathGeorref = geoFichero.PathFile
        '            epsgGeorref = geoFichero.EPSCode
        '            elementoLV = New ListViewItem With {
        '                .Text = "Georreferenciado",
        '                .ImageIndex = 4,
        '                .Tag = pathGeorref,
        '                .Group = docGeorref
        '    }
        '            elementoLV.SubItems.Add($"{fila.Item("nombre")}.ecw")
        '            elementoLV.SubItems.Add(".ECW")
        '            elementoLV.SubItems.Add(epsgGeorref)
        '            elementoLV.SubItems.Add($"{fila.Item("mostrarwms")}")
        '            elementoLV.SubItems.Add($"{fila.Item("tipowms")}")
        '            elementoLV.SubItems.Add($"{fila.Item("zindex")}")
        '            elementoLV.SubItems.Add($"{fila.Item("idcontorno")}")
        '            elementoLV.ForeColor = IIf(Not IO.File.Exists(pathGeorref), Color.Red, Color.DarkGreen)
        '            lvDocResources.Items.Add(elementoLV) : elementoLV = Nothing
        '        End If
        '    Next
        'Next

        'Rellenamos el cuadro de texto con algunos datos
        FillRichText(RichTextBox3, rowId)

        Label1.Text = $"Total de recursos: {lvDocResources.Items.Count}"
        idArchiveResourcesLoaded = idArchivo

    End Sub

    Private Sub FillDetailsReduced(idArchivo As Integer)

        If idArchivoLoaded = idArchivo Then Exit Sub
        If gettingDetails Then Exit Sub
        If cancelDetails Then Exit Sub
        If Me.DataGridView1.CurrentCell Is Nothing Then Return
        gettingDetails = True

        Dim gTerri As ListViewGroup : gTerri = New ListViewGroup("Territorios") : lvFastView.Groups.Add(gTerri)
        Dim gMain As ListViewGroup : gMain = New ListViewGroup("Hoja de características") : lvFastView.Groups.Add(gMain)


        Dim rowIdx As Integer
        rowIdx = DataGridView1.CurrentCell.RowIndex
        idArchivoLoaded = idArchivo

        Dim elementoLV As ListViewItem

        Me.Cursor = Cursors.WaitCursor
        lvFastView.Items.Clear()
        elemEntidadSel = Nothing
        elemEntidadSel = New docCuadMTN(DataGridView1.Item("idarchivodocmtn", rowIdx).Value)

        elementoLV = New ListViewItem With {.Text = "Provincia", .ImageIndex = 4, .Group = gMain}
        elementoLV.SubItems.Add(elemEntidadSel.ProvinciaNombre)
        lvFastView.Items.Add(elementoLV) : elementoLV = Nothing
        elementoLV = New ListViewItem With {.Text = "Tomo", .ImageIndex = 4, .Group = gMain}
        elementoLV.SubItems.Add(DataGridView1.Item("tomo", rowIdx).Value.ToString)
        lvFastView.Items.Add(elementoLV) : elementoLV = Nothing
        elementoLV = New ListViewItem With {.Text = "Sellado", .ImageIndex = 4, .Group = gMain}
        elementoLV.SubItems.Add(DataGridView1.Item("sellado", rowIdx).Value.ToString)
        lvFastView.Items.Add(elementoLV) : elementoLV = Nothing
        elementoLV = New ListViewItem With {.Text = "Tipo", .ImageIndex = 4, .Group = gMain}
        elementoLV.SubItems.Add(DataGridView1.Item("tipo", rowIdx).Value.ToString)
        lvFastView.Items.Add(elementoLV) : elementoLV = Nothing
        elementoLV = New ListViewItem With {.Text = "Subtipo", .ImageIndex = 4, .Group = gMain}
        elementoLV.SubItems.Add(DataGridView1.Item("subtipo", rowIdx).Value.ToString)
        lvFastView.Items.Add(elementoLV) : elementoLV = Nothing

        If DataGridView1.Item("nombreTerris", rowIdx).Value.ToString <> "" Then
            Dim terris() As String = DataGridView1.Item("nombreTerris", rowIdx).Value.ToString.Split(",")
            For Each terri In terris
                elementoLV = New ListViewItem With {.Text = "Territorio", .ImageIndex = 4, .Group = gTerri}
                elementoLV.SubItems.Add(terri.Trim)
                lvFastView.Items.Add(elementoLV) : elementoLV = Nothing
            Next

        End If
        'Rellenamos el cuadro de texto con algunos datos
        FillRichText(RichTextBox1, rowIdx)

        ''Enlaces externos

        'Enlace al tomo del inventario
        Button3.Tag = $"{elemEntidadSel.ProvinciaINE}|{elemEntidadSel.Tomo}"

        ''Documento Cabina
        Button1.Enabled = IIf(elemEntidadSel.rutaFicheroBajaRes <> "", True, False)
        Button1.Tag = elemEntidadSel.rutaFicheroBajaRes

        Try
            Button4.Enabled = IIf(elemEntidadSel.rutaFicheroPDF <> "", True, False)
            Button4.Tag = elemEntidadSel.rutaFicheroPDF
        Catch ex As Exception

        End Try

        'CDD
        'https://centrodedescargas.cnig.es/CentroDescargas/busquedaIdProductor.do?idProductor=50940&Serie=ACLLI
        Button2.Enabled = IIf(elemEntidadSel.cddURL <> "", True, False)
        Button2.Tag = elemEntidadSel.cddURL

        'Cargamos la imagen en miniatura
        LoadThumb(PictureBox1)

        Me.Cursor = Cursors.Default
        TaxonDetailView(modeView.PanelOpen)
        gettingDetails = False

    End Sub

    Private Sub FillDocCuadMTNEwithFilter(mainFilter As String)

        'Ahora añadimos filtros.
        If mainFilter = "" Then mainFilter = "archivodocmtn.idarchivodocmtn>0"
        'Cuando hacemos filtro en la parte de territorios, activamos la claúsula inner join
        sqlBase = $"SELECT 
	                archivodocmtn.idarchivodocmtn, archivodocmtn.sellado,archivodocmtn.tipo,archivodocmtn.subtipo,
		                archivodocmtn.tomo,archivodocmtn.fecha,string_agg(Territorios.Nombre,', ') as nombreTerris,
		                'Zona ' || COALESCE (zona_num || '. ', 'no definida. ') || 
		                COALESCE (subdivision_tipo || ': ' || subdivision_num || '. ','') ||
		                'Cuaderno ' ||  COALESCE (cuaderno, 'no definido') || '. ' ||
		                COALESCE (cuad_tipo ||'. ','') || 
		                COALESCE (itin_tipo || ': ' || itin_num || '. ','') ||
		                COALESCE (archivodocmtn.observaciones || '.' ,'') as contenido,
		                provincias.nombreprovincia,archivodocmtn.create_at
                FROM bdsidschema.archivodocmtn 
				LEFT JOIN bdsidschema.provincias on archivodocmtn.codprov= provincias.idprovincia 
                LEFT JOIN bdsidschema.archivodocmtn2terris ON archivodocmtn.idarchivodocmtn=archivodocmtn2terris.archivodocmtn_id 
                LEFT JOIN bdsidschema.territorios ON archivodocmtn2terris.territorio_id=territorios.idterritorio  
                WHERE {mainFilter} 
                group by archivodocmtn.idarchivodocmtn,archivodocmtn.create_at,archivodocmtn.tipo,archivodocmtn.subtipo,archivodocmtn.tomo,
				archivodocmtn.sellado,archivodocmtn.fecha,provincias.nombreprovincia,archivodocmtn.zona_num"

        rcdDataPrin = New DataView
        If CargarDataView(sqlBase, rcdDataPrin) = False Then
            ModalExclamation("No se pueden cargar los datos")
            Exit Sub
        End If
        FormatDatagridCuadMTN()
        'If columnVisualiz = 1 Then FormatDatagridMapsReduced()


    End Sub

    'Este formto muestra todos los campos en columnas
    Private Sub FormatDatagridCuadMTN()

        DataGridView1.DataSource = rcdDataPrin
        'Seguidas ponemos las columnas visibles con su anchura
        DataGridView1.Columns(0).HeaderText = "idarchivodocmtn"
        DataGridView1.Columns(0).Visible = False
        DataGridView1.Columns("Sellado").HeaderText = "Sellado"
        DataGridView1.Columns("Sellado").Width = 70
        DataGridView1.Columns("tipo").HeaderText = "Tipo"
        DataGridView1.Columns("tipo").Width = 150
        DataGridView1.Columns("subtipo").HeaderText = "Subtipo"
        DataGridView1.Columns("subtipo").Width = 80
        DataGridView1.Columns("subtipo").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridView1.Columns("tomo").HeaderText = "Tomo"
        DataGridView1.Columns("tomo").Width = 60
        DataGridView1.Columns("tomo").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridView1.Columns("fecha").HeaderText = "Fecha"
        DataGridView1.Columns("fecha").Width = 80
        DataGridView1.Columns("nombreterris").HeaderText = "Territorios"
        DataGridView1.Columns("nombreterris").Width = 200
        DataGridView1.Columns("nombreterris").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridView1.Columns("contenido").HeaderText = "Contenido"
        DataGridView1.Columns("contenido").Width = 50
        DataGridView1.Columns("contenido").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridView1.Columns("nombreprovincia").HeaderText = "Provincia"
        DataGridView1.Columns("nombreprovincia").Width = 85
        DataGridView1.Columns("nombreprovincia").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridView1.Columns("create_at").HeaderText = "Fecha alta"
        DataGridView1.Columns("create_at").Width = 75
        DataGridView1.Columns("create_at").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

        'Ocultamos el resto de columnas
        For iCol = 10 To DataGridView1.ColumnCount - 1
            DataGridView1.Columns(iCol).Visible = False
        Next

        'Activamos las columnas que pueden mostrarse y apagarse
        RenombrarItemsColumnas()

        DataGridView1.RowsDefaultCellStyle.BackColor = Color.White
        DataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue
        'DataGridView1.Sort(DataGridView1.Columns(0), System.ComponentModel.ListSortDirection.Ascending)
        ToolStripStatusLabel1.Text = "Elementos: " & DataGridView1.RowCount

        If DataGridView1.RowCount > minRows4useEnterOnFilter Then
            useEnterOnFilter = True
            ToolStripLabel1.Text = "Búsqueda (Pulsa Enter)"
        Else
            useEnterOnFilter = False
            ToolStripLabel1.Text = "Buscar..."
        End If




    End Sub


    Private Sub DataGridView1_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DataGridView1.CellFormatting
        Dim estadoJuridic As String

        If DataGridView1.CurrentCell Is Nothing Then Return

        If DataGridView1.Columns(e.ColumnIndex).Name = "sellado" Then
            'Así podemos definir un color de fondo en función de un valor
            If e.Value.ToString.EndsWith("6") Then
                e.CellStyle.BackColor = Color.Pink
            Else
                e.CellStyle.BackColor = Color.White
            End If
        End If

        If DataGridView1.Columns(e.ColumnIndex).Name = "ColImg" Then
            If DataGridView1.Item("tipo", e.RowIndex).Value.ToString() = "Acta de Deslinde" Then
                estadoJuridic = DataGridView1.Item("provis_estado", e.RowIndex).Value.ToString()
                If estadoJuridic = "Provisionalidad completa" Or estadoJuridic = "Provisionalidad parcial" Or estadoJuridic = "Provisionalidad dudosa" Then
                    e.Value = MDIPrincipal.ImageList2.Images(6)
                ElseIf estadoJuridic = "Conformidad" Or estadoJuridic = "Conformidad dudosa" Then
                    e.Value = MDIPrincipal.ImageList2.Images(5)
                ElseIf estadoJuridic = "Sin estudiar" Then
                    e.Value = MDIPrincipal.ImageList2.Images(7)
                ElseIf estadoJuridic = "Anulada" Then
                    e.Value = MDIPrincipal.ImageList2.Images(8)
                Else
                    e.Value = MDIPrincipal.ImageList2.Images(0)
                End If
            Else
                e.Value = MDIPrincipal.ImageList2.Images(0)
            End If
        End If

    End Sub

    Private Sub resultCMTN_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.Size = New Point(1300, 780)

        PictureBox1.Dock = DockStyle.Fill
        TableLayoutPanel1.BackColor = Color.FromName("Control")
        TableLayoutPanel2.BackColor = Color.FromName("Control")
        TableLayoutPanel3.BackColor = Color.FromName("Control")
        TableLayoutPanel4.BackColor = Color.FromName("Control")
        TableLayoutPanel5.BackColor = Color.FromName("Control")
        TableLayoutPanel6.BackColor = Color.FromName("Control")

        RichTextBox1.BackColor = Color.FromArgb(255, 245, 238)
        RichTextBox2.BackColor = Color.FromArgb(255, 245, 238)

        lvFastView.Columns.Clear()
        lvFastView.Columns.Add("Atributo", 150, HorizontalAlignment.Left)
        lvFastView.Columns.Add("Valor", 350, HorizontalAlignment.Left)
        lvFastView.SmallImageList = MDIPrincipal.ImageList2
        lvFastView.FullRowSelect = True
        lvFastView.View = View.Details
        lvFastView.BackColor = Color.FromArgb(255, 245, 238)

        lvTagsM21.Columns.Clear()
        lvTagsM21.Columns.Add("Etiqueta", 200, HorizontalAlignment.Left)
        lvTagsM21.Columns.Add("Valor", 400, HorizontalAlignment.Left)
        lvTagsM21.SmallImageList = MDIPrincipal.ImageList2
        lvTagsM21.FullRowSelect = True
        lvTagsM21.View = View.Details
        lvTagsM21.BackColor = Color.FromArgb(255, 245, 238)

        lvDocResources.Columns.Clear()
        lvDocResources.Columns.Add("Descripción", 200, HorizontalAlignment.Left)
        lvDocResources.Columns.Add("Nombre", 150, HorizontalAlignment.Left)
        lvDocResources.Columns.Add("Formato", 100, HorizontalAlignment.Left)
        lvDocResources.Columns.Add("EPSG", 100, HorizontalAlignment.Left)
        lvDocResources.Columns.Add("Mostrar en WMS", 100, HorizontalAlignment.Left)
        lvDocResources.Columns.Add("Tipo WMS", 150, HorizontalAlignment.Left)
        lvDocResources.Columns.Add("Z-Index", 100, HorizontalAlignment.Left)
        lvDocResources.Columns.Add("IdContorno", 0, HorizontalAlignment.Left)
        lvDocResources.SmallImageList = MDIPrincipal.ImageList2
        lvDocResources.FullRowSelect = True
        lvDocResources.View = View.Details
        lvDocResources.BackColor = Color.FromArgb(255, 245, 238)

        lvDocEditions.Columns.Clear()
        lvDocEditions.Columns.Add("Fecha edición", 140, HorizontalAlignment.Left)
        lvDocEditions.Columns.Add("Usuario", 80, HorizontalAlignment.Left)
        lvDocEditions.Columns.Add("Variación", 120, HorizontalAlignment.Left)
        lvDocEditions.Columns.Add("Antes", 130, HorizontalAlignment.Left)
        lvDocEditions.Columns.Add("Después", 130, HorizontalAlignment.Left)
        lvDocEditions.SmallImageList = MDIPrincipal.ImageList2
        lvDocEditions.FullRowSelect = True
        lvDocEditions.View = View.Details
        lvDocEditions.Dock = DockStyle.Fill

        Button3.Tag = ""

        ToolStripStatusLabel1.Text = ""
        ToolStripStatusLabel2.Text = ""
        ToolStripStatusLabel3.Text = ""
        ToolStripStatusLabel4.Text = ""
        ToolStripStatusLabel5.Text = ""

        cboFields.Items.Add(New itemData("(Múltiple)", "All"))
        cboFields.Items.Add(New itemData("Sellado", "numdoc"))
        cboFields.Items.Add(New itemData("tipo", "tipo"))
        cboFields.Items.Add(New itemData("subtipo", "subtipo"))
        cboFields.Items.Add(New itemData("tomo", "tomo"))
        cboFields.Items.Add(New itemData("Fecha", "fechaprincipal"))
        cboFields.Items.Add(New itemData("Estado", "estado"))
        cboFields.Items.Add(New itemData("Territorios", "listaMuniHisto"))
        cboFields.Items.Add(New itemData("Signatura", "signatura"))
        cboFields.Items.Add(New itemData("Colección", "coleccion"))
        cboFields.Items.Add(New itemData("Subdivisión", "subdivision"))
        cboFields.Items.Add(New itemData("Proc./Hoja", "procehoja"))
        cboFields.Items.Add(New itemData("Proc./Carpeta", "procecarpeta"))
        cboFields.Items.Add(New itemData("Observaciones", "observaciones"))


        btnAddingCarrito.Enabled = Not EsCarritoCompra
        btnDeletingCarrito.Enabled = EsCarritoCompra
        btnEditar.Visible = usuarioMyApp.permisosLista.editarDocumentacion
        mnuGenerateThumb.Visible = usuarioMyApp.permisosLista.usuarioISTARI

        TaxonDetailView(modeView.PanelClose)
        ResizeListViews()
        CargarConsulta()
        CerrarSpinner()

    End Sub

    Private Sub btnExportCSV_Click(sender As Object, e As EventArgs) Handles btnExportCSV.Click

        Dim ExportarSelect As Boolean = False
        Dim cadLinea As String = ""

        If DataGridView1.RowCount = 0 Then Exit Sub

        Dim cadSeparator As String = InputBox("Introduzca un carácter separador." & Environment.NewLine & "Por defecto, punto y coma", AplicacionTitulo, ";")

        If cadSeparator = "" Or cadSeparator.Length <> 1 Then
            ModalExclamation("Carácter separador no válido.")
            Exit Sub
        End If

        If DataGridView1.SelectedRows.Count > 1 Then
            If ModalQuestion("¿Desea exportar sólo las filas seleccionadas?") = DialogResult.Yes Then ExportarSelect = True
        End If

        Using sfd As New SaveFileDialog() With {
                .Title = "Introduzca el nombre del fichero",
                .Filter = "Archivos CSV *.csv|*.csv",
                .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
            }
            If sfd.ShowDialog = DialogResult.OK Then
                Me.Cursor = Cursors.WaitCursor
                Using sw As New System.IO.StreamWriter(sfd.FileName, False, System.Text.Encoding.UTF8)
                    If ExportarSelect = True Then
                        ToolStripStatusLabel3.Text = "Generando fichero exportación"
                        For i = 1 To DataGridView1.ColumnCount - 1
                            If DataGridView1.Columns(i).Visible = True Then
                                cadLinea &= DataGridView1.Columns(i).HeaderText & cadSeparator
                            End If
                        Next
                        sw.WriteLine(cadLinea)
                        For i = 0 To DataGridView1.SelectedRows.Count - 1
                            cadLinea = ""
                            For j As Integer = 1 To DataGridView1.ColumnCount - 1
                                If DataGridView1.Columns(j).Visible = True Then
                                    cadLinea &= DataGridView1.Item(j, DataGridView1.SelectedRows(i).Index).Value.ToString & cadSeparator
                                End If
                            Next
                            sw.WriteLine(cadLinea)
                        Next
                    Else
                        ToolStripStatusLabel3.Text = "Generando fichero exportación"
                        For i = 1 To DataGridView1.ColumnCount - 1
                            If DataGridView1.Columns(i).Visible = True Then
                                cadLinea &= DataGridView1.Columns(i).HeaderText & cadSeparator
                            End If
                        Next
                        sw.WriteLine(cadLinea)
                        For i = 0 To DataGridView1.Rows.Count - 1
                            cadLinea = ""
                            For j As Integer = 1 To DataGridView1.ColumnCount - 1
                                If DataGridView1.Columns(j).Visible = True Then
                                    cadLinea &= DataGridView1.Item(j, DataGridView1.Rows(i).Index).Value.ToString & cadSeparator
                                End If
                            Next
                            sw.WriteLine(cadLinea)
                        Next
                    End If
                End Using
                Me.Cursor = Cursors.Default
                ToolStripStatusLabel3.Text = ""
            End If
        End Using

        ModalInfo("Fichero de exportación generado")

    End Sub

    Private Sub ButtonNavigation(sender As Object, e As EventArgs) Handles btnPrev.Click, btnNext.Click

        If DataGridView1.RowCount = 0 Then Exit Sub


        If DataGridView1.SelectedRows.Count = 0 Then
            DataGridView1.Rows(DataGridView1.CurrentCell.RowIndex).Selected = True
        End If
        If sender.name = "btnPrev" Then
            Dim _rowIndex = DataGridView1.SelectedRows(0).Index - 1
            If _rowIndex > -1 Then
                Dim prevRow As DataGridViewRow = DataGridView1.Rows(_rowIndex)
                ' Move the Glyph arrow to the previous row
                DataGridView1.CurrentCell = prevRow.Cells(1)
                DataGridView1.Rows(_rowIndex).Selected = True
            Else
                Exit Sub
            End If
        ElseIf sender.name = "btnNext" Then
            Dim _rowIndex = DataGridView1.SelectedRows(0).Index + 1
            If _rowIndex <= DataGridView1.Rows.Count - 1 Then
                Dim nextRow As DataGridViewRow = DataGridView1.Rows(_rowIndex)
                ' Move the Glyph arrow to the next row
                DataGridView1.CurrentCell = nextRow.Cells(1)
                DataGridView1.Rows(_rowIndex).Selected = True
            Else
                Exit Sub
            End If
        End If
        If DataGridView1.Item("idarchivo", DataGridView1.CurrentCell.RowIndex).Value.ToString = "" Then
            ModalExclamation("")
            Exit Sub
        End If
        FillDetailsReduced(DataGridView1.Item("idarchivo", DataGridView1.CurrentCell.RowIndex).Value.ToString)

        'If idArchivoLoaded <> titnMTagsLoaded Then
        '    FillMarc21Tags(DataGridView1.Item(0, DataGridView1.CurrentCell.RowIndex).Value.ToString, DataGridView1.CurrentCell.RowIndex)
        'End If

    End Sub

    Private Sub ExternalLinks(sender As Object, e As EventArgs) Handles Button1.Click, Button2.Click, btnLinkCdD.Click, btnTVCNIG.Click, btnLinkABSYS.Click


        If elemEntidadSel Is Nothing Then Exit Sub
        If sender.tag = "" Then
            ModalExclamation("No se dispone de la URL de acceso a esta información")
            Exit Sub
        End If

        Dim cadURL As String = sender.tag
        Try
            Process.Start(cadURL)
        Catch ex As Exception
            ModalError(ex.Message)
        End Try

    End Sub


    Private Sub btnAjustar_Click(sender As Object, e As EventArgs) Handles btnAjustar.Click

        ResizeDatagridView()

    End Sub


    Private Sub txtFiltro_TextChanged(sender As Object, e As EventArgs) Handles txtFiltro.TextChanged

        If Not useEnterOnFilter Then FilterApplyOnDatagrid()

    End Sub

    Private Sub txtFiltro_KeyDown(sender As Object, e As KeyEventArgs) Handles txtFiltro.KeyDown

        If e.KeyCode = Keys.Enter And useEnterOnFilter Then
            FilterApplyOnDatagrid()
        End If

    End Sub

    Private Sub DataGridView1_Click(sender As Object, e As EventArgs) Handles DataGridView1.Click
        If DataGridView1.Rows.Count = 0 Then Exit Sub
        FillDetailsReduced(DataGridView1.Item("idarchivodocmtn", DataGridView1.CurrentCell.RowIndex).Value.ToString)

    End Sub

    Private Sub DataGridView1_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellEnter

        Debug.Print("CellEnter")
        If DataGridView1.CurrentCell IsNot Nothing Then
            If DataGridView1.Item("idarchivodocmtn", DataGridView1.CurrentCell.RowIndex).Value.ToString = "" Then
                ModalExclamation("Índice TITN no definido")
                Exit Sub
            End If
            If Not gettingDetails Then FillDetailsReduced(DataGridView1.Item("idarchivodocmtn", DataGridView1.CurrentCell.RowIndex).Value.ToString)
        End If

    End Sub

    Private Sub DataGridView1_MouseUp(sender As Object, e As MouseEventArgs) Handles DataGridView1.MouseUp

        Dim info As DataGridView.HitTestInfo = DataGridView1.HitTest(e.X, e.Y)
        Dim row = info.RowIndex
        Dim column = info.ColumnIndex
        Application.DoEvents()
        'FillDetailsReduced(DataGridView1.Item("iddocsiddae", info.RowIndex).Value.ToString)
    End Sub

    Private Sub btnOpenIncidencia_Click(sender As Object, e As EventArgs) Handles btnOpenIncidencia.Click

        If DataGridView1.Rows.Count = 0 Then Exit Sub
        If DataGridView1.SelectedRows.Count > 1 Then
            ModalExclamation("Selecciona un único registro para ver sus atributos")
            Exit Sub
        End If

        Dim frmNotify As New GestionUserNotification
        frmNotify.MdiParent = MDIPrincipal
        frmNotify.incidenciaInicial = DataGridView1.Item("numdoc", DataGridView1.CurrentCell.RowIndex).Value.ToString
        frmNotify.Show()

    End Sub


    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        MostrarDetalle()
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click, PictureBox2.Click
        Try
            If IO.File.Exists(PictureBox1.Tag) Then Process.Start(PictureBox1.Tag)
        Catch ex As Exception
            GenerarLOG(ex.Message)
            ModalError(ex.Message)
        End Try
    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        txtFiltro.Text = ""
        cboFields.SelectedIndex = -1
        sqlSearchApplyed = ""
        FilterApplyOnDatagrid()
    End Sub


    Sub TabulacionVentanas(sender As Object, e As EventArgs) Handles btnDetail.Click, btnList.Click, btnResources.Click

        If DataGridView1.Rows.Count = 0 Then Exit Sub
        If DataGridView1.SelectedRows.Count > 1 Then
            ModalInfo("Selecciona un único registro para ver sus atributos")
            Exit Sub
        End If

        If sender.name = "btnList" Then
            TabControl1.SelectedIndex = 0
            txtFiltro.Enabled = True
            cboFields.Enabled = True
        ElseIf sender.name = "btnDetail" Then
            txtFiltro.Enabled = False
            cboFields.Enabled = False
            If idArchivoLoaded <> idArchiveTagsLoaded Then
                FillMarc21Tags(DataGridView1.Item("idarchivo", DataGridView1.CurrentCell.RowIndex).Value.ToString, DataGridView1.CurrentCell.RowIndex)
            End If
            TabControl1.SelectedIndex = 1
        ElseIf sender.name = "btnResources" Then
            txtFiltro.Enabled = False
            cboFields.Enabled = False
            If idArchivoLoaded <> idArchiveResourcesLoaded Then
                FillResources(DataGridView1.Item("idarchivo", DataGridView1.CurrentCell.RowIndex).Value.ToString, DataGridView1.CurrentCell.RowIndex)
            End If
            TabControl1.SelectedIndex = 2
        End If

    End Sub


    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl1.SelectedIndexChanged

        'On navigation complete, process information
        If TabControl1.SelectedIndex = 0 Then
            Application.DoEvents()
        ElseIf TabControl1.SelectedIndex = 1 Then
            If DataGridView1.CurrentCell Is Nothing Then Exit Sub
            If idArchivoLoaded <> idArchiveTagsLoaded Then
                FillMarc21Tags(DataGridView1.Item("idarchivodocmtn", DataGridView1.CurrentCell.RowIndex).Value.ToString, DataGridView1.CurrentCell.RowIndex)
            End If
        ElseIf TabControl1.SelectedIndex = 2 Then
            If DataGridView1.CurrentCell Is Nothing Then Exit Sub
            If idArchivoLoaded <> idArchiveResourcesLoaded Then
                FillResources(DataGridView1.Item("idarchivo", DataGridView1.CurrentCell.RowIndex).Value.ToString, DataGridView1.CurrentCell.RowIndex)
            End If
        End If
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click


        'Compruebo que no haya abierta previamente una ventana para modificar este documento
        Dim nIndiceEdit As Integer
        If DataGridView1.Rows.Count = 0 Then Exit Sub

        If DataGridView1.SelectedRows.Count <> 1 Then
            ModalExclamation("Seleccione un único registro para editar")
        End If
        If DataGridView1.Item("idarchivo", DataGridView1.CurrentCell.RowIndex).Value.ToString = "" Then
            ModalExclamation("")
            Exit Sub
        End If

        nIndiceEdit = DataGridView1.Item("idarchivo", DataGridView1.CurrentCell.RowIndex).Value

        For Each ChildForm As Form In MDIPrincipal.MdiChildren
            If ChildForm.Tag = nIndiceEdit Then
                ChildForm.Focus()
                Exit Sub
            End If
        Next

        Dim FormularioCreacion As New frmEdicion With {
           .MdiParent = MDIPrincipal,
           .IDArchivoEdition = nIndiceEdit,
           .ModeEdition = frmEdicion.ModeEdition.EditSingleDocument
        }
        FormularioCreacion.Show()

    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click

        DataGridView1.DataSource = Nothing
        DataGridView1.Rows.Clear()
        DataGridView1.Columns.Clear()
        idArchivoLoaded = 0
        idArchiveTagsLoaded = 0
        LanzarSpinner()
        CargarConsulta()
        'DataGridView1.Refresh()
        CerrarSpinner()

    End Sub

    Private Sub resultCMTN_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        ResizeListViews()
    End Sub



    Private Sub lvTagsM21_Click(sender As Object, e As EventArgs) Handles lvTagsM21.Click

        If Not lvTagsM21.SelectedItems Is Nothing Then
            For Each li As ListViewItem In lvTagsM21.SelectedItems
                If li.SubItems(0).Text = "Enlace CdD" Then
                    If li.SubItems(1).Text <> "" Then
                        My.Computer.Clipboard.SetText(li.SubItems(1).Text)
                        ModalInfo("URL del Centro de Descargas copiada al portapapeles")
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub resultCMTN_Resize(sender As Object, e As EventArgs) Handles Me.Resize

        Dim f As Form
        f = sender
        If f.WindowState = FormWindowState.Maximized Then ResizeListViews()

    End Sub


    Private Sub PictureBox1_MouseMove(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseMove

        Dim DragPaths As New DataObject()
        Dim DropList As New Specialized.StringCollection
        Dim strPath, strName, strFullPath As String

        If (e.Button = MouseButtons.Left) Then
            If PictureBox1.Tag = "" Then
                ModalExclamation("No hay documento digitalizado")
            Else
                Try
                    If IO.File.Exists(PictureBox1.Tag) Then
                        strFullPath = PictureBox1.Tag
                        DropList.Add(strFullPath)
                        DragPaths.SetFileDropList(DropList)
                        DoDragDrop(DragPaths, DragDropEffects.Copy)
                    End If
                Catch ex As Exception
                    ModalError($"Mensaje: {ex.Message}")
                End Try
            End If
        End If

    End Sub

    Private Sub PictureBox2_MouseMove(sender As Object, e As MouseEventArgs) Handles PictureBox2.MouseMove
        Dim DragPaths As New DataObject()
        Dim DropList As New Specialized.StringCollection
        Dim strPath, strName, strFullPath As String

        If (e.Button = MouseButtons.Left) Then
            If PictureBox2.Tag = "" Then
                ModalExclamation("No hay documento digitalizado")
            Else
                Try
                    If IO.File.Exists(PictureBox2.Tag) Then
                        strFullPath = PictureBox2.Tag
                        DropList.Add(strFullPath)
                        DragPaths.SetFileDropList(DropList)
                        DoDragDrop(DragPaths, DragDropEffects.Copy)
                    End If
                Catch ex As Exception
                    ModalError($"Mensaje: {ex.Message}")
                End Try
            End If
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click

        If DataGridView1.RowCount = 0 Then Exit Sub
        If DataGridView1.SelectedRows.Count = 0 Then Exit Sub

        Dim cadSeparator As String = ";"
        If ModalQuestion($"Se han seleccionado para exportar {DataGridView1.SelectedRows.Count} documentos. ¿Continuar?") = DialogResult.No Then Exit Sub

        Using folderDialogSelection As New FolderBrowserDialog With {
                    .Description = "Seleccione la carpeta de salida",
                    .ShowNewFolderButton = True
            }
            If folderDialogSelection.ShowDialog = DialogResult.OK Then
                Me.Cursor = Cursors.WaitCursor
                Using sw As New IO.StreamWriter(folderDialogSelection.SelectedPath & "\listado.txt", False, System.Text.Encoding.UTF8)
                    For i = 0 To DataGridView1.SelectedRows.Count - 1
                        ToolStripStatusLabel2.Text = $"Copiando {i + 1} de {DataGridView1.SelectedRows.Count}"
                        Application.DoEvents()
                        Try
                            Dim docExport As New docCartoSEE(DataGridView1.Item("idarchivo", DataGridView1.SelectedRows(i).Index).Value)
                            docExport.getGeoFiles()
                            Application.DoEvents()

                            If IO.Directory.Exists($"{folderDialogSelection.SelectedPath}\{docExport.Sellado}") Then
                                sw.WriteLine($"Los datos del documento {docExport.Sellado} no se han exportado porque ya existe una carpeta con ese nombre")
                                Continue For
                            End If
                            IO.Directory.CreateDirectory($"{folderDialogSelection.SelectedPath}\{docExport.Sellado}")
                            IO.Directory.CreateDirectory($"{folderDialogSelection.SelectedPath}\{docExport.Sellado}\JPG")
                            IO.Directory.CreateDirectory($"{folderDialogSelection.SelectedPath}\{docExport.Sellado}\Georef")
                            If IO.File.Exists(docExport.rutaFicheroAltaRes) Then
                                IO.File.Copy(docExport.rutaFicheroAltaRes, $"{folderDialogSelection.SelectedPath}\{docExport.Sellado}\JPG\Alta_{SacarFileDeRuta(docExport.rutaFicheroAltaRes)}")
                            End If
                            If IO.File.Exists(docExport.rutaFicheroBajaRes) Then
                                IO.File.Copy(docExport.rutaFicheroBajaRes, $"{folderDialogSelection.SelectedPath}\{docExport.Sellado}\JPG\Baja_{SacarFileDeRuta(docExport.rutaFicheroBajaRes)}")
                            End If
                            If IO.File.Exists(docExport.rutaFicheroPDF) Then
                                IO.Directory.CreateDirectory($"{folderDialogSelection.SelectedPath}\{docExport.Sellado}\PDF")
                                IO.File.Copy(docExport.rutaFicheroPDF, $"{folderDialogSelection.SelectedPath}\{docExport.Sellado}\PDF\{SacarFileDeRuta(docExport.rutaFicheroPDF)}")
                            End If
                            For Each resource As FileGeorref In docExport.listaFicherosGeo
                                If Not IO.Directory.Exists($"{folderDialogSelection.SelectedPath}\{docExport.Sellado}\Georef\{resource.EPSCode}") Then
                                    IO.Directory.CreateDirectory($"{folderDialogSelection.SelectedPath}\{docExport.Sellado}\Georef\{resource.EPSCode}")
                                End If
                                If IO.File.Exists(resource.PathFile) Then
                                    IO.File.Copy(resource.PathFile, $"{folderDialogSelection.SelectedPath}\{docExport.Sellado}\Georef\{resource.EPSCode}\{SacarFileDeRuta(resource.PathFile)}")
                                End If
                            Next
                            sw.WriteLine($"Copiados los recursos asociados al documento {docExport.Sellado}")
                        Catch ex As Exception
                            ModalError(ex.Message)
                        End Try
                    Next
                End Using
                Me.Cursor = Cursors.Default
                ToolStripStatusLabel2.Text = ""
                If ModalQuestion("La documentación ha sido exportada. ¿Desea abrir la carpeta?") = DialogResult.Yes Then
                    Try
                        Process.Start(folderDialogSelection.SelectedPath)
                    Catch ex As Exception
                        ModalError(ex.Message)
                    End Try
                End If
            End If

        End Using




    End Sub

    Private Sub OpenDocInventary(sender As Object, e As EventArgs) Handles Button3.Click, btnOpenInventary.Click

        Dim nTomo As String
        Dim cProv As Integer
        Dim filePDF As String
        If DataGridView1.Rows.Count = 0 Then Exit Sub

        Try
            If Button3.Tag.ToString <> "" Then
                Dim tomoParams() As String = Button3.Tag.ToString.Split("|")
                If tomoParams.Length = 2 Then
                    cProv = tomoParams(0)
                    nTomo = tomoParams(1)
                End If
            End If


            Application.DoEvents()
            ' A partir de los datos del documento componemos la ruta donde deberían almacenarse la digitalización del libro de registro donde viene detallado
            If nTomo.Length = 1 Then
                nTomo = "00" & nTomo.ToLower
            ElseIf nTomo.Length = 2 Then
                nTomo = "0" & nTomo.ToLower
            ElseIf nTomo.Length = 4 And nTomo.ToLower.EndsWith("bis") Then
                nTomo = "00" & nTomo.ToLower
            ElseIf nTomo.Length = 4 And nTomo.ToLower.EndsWith("ter") Then
                nTomo = "00" & nTomo.ToLower
            ElseIf nTomo.Length = 5 And nTomo.ToLower.EndsWith("bis") Then
                nTomo = "0" & nTomo.ToLower
            ElseIf nTomo.Length = 5 And nTomo.ToLower.EndsWith("ter") Then
                nTomo = "0" & nTomo.ToLower
            Else
                nTomo = nTomo.ToLower
            End If

            filePDF = rutaRepoInventarioInfo & "\" & String.Format("{0:00}", cProv) & "\P" & String.Format("{0:00}", cProv) & "T" & nTomo & ".pdf"
            If System.IO.File.Exists(filePDF) Then
                Process.Start(filePDF)
            Else
                ModalExclamation($"No hay información del tomo {nTomo} de la provincia {DameProvinciaByINE(cProv)}")
            End If

        Catch ex As Exception
            ModalError(ex.Message)
            Exit Sub
        End Try

    End Sub

    Private Sub AddingItemToCarrito(sender As Object, e As EventArgs) Handles btnAddingCarrito.Click, btnDeletingCarrito.Click

        If DataGridView1.RowCount = 0 Then Exit Sub
        Dim indexForShop As Integer = 0
        If sender.name = "btnDeletingCarrito" Then
            For Each item As DataGridViewRow In DataGridView1.SelectedRows
                CarritoCompra.Remove(item.Cells(indexForShop).Value)
            Next
            If DataGridView1.RowCount <> CarritoCompra.Count Then btnRefresh.PerformClick()
        Else
            If DataGridView1.SelectedRows.Count > 50 Then
                ModalExclamation("No se pueden añadir más de 50 elementos de una tacada al carrito")
                Exit Sub
            End If
            Dim nDocsAdded As Integer = 0
            For Each item As DataGridViewRow In DataGridView1.SelectedRows
                If Not CarritoCompra.Contains(item.Cells(indexForShop).Value) Then
                    CarritoCompra.Add(item.Cells(indexForShop).Value)
                    nDocsAdded += 1
                Else
                    ModalInfo("El documento ya se encuentra en el carrito")
                End If

            Next
            If nDocsAdded = 0 Then Exit Sub
            ModalInfo($"Se han añadido al carrito {nDocsAdded} documentos")
            For Each ChildForm As resultCMTN In Me.MdiChildren
                If ChildForm.EsCarritoCompra Then
                    If ChildForm.DataGridView1.RowCount <> CarritoCompra.Count Then ChildForm.btnRefresh.PerformClick()
                    Exit For
                End If
            Next
        End If



    End Sub

    Private Sub GenerarMetadatoNEMToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GenerarMetadatoNEMToolStripMenuItem.Click

        ModalInfo("Función no disponible")

        'Dim NombreFichOUT As String
        'Dim contador As Integer
        'Dim NumSelect As Integer
        'Dim folderOut As String = ""
        'Dim itemForMetadata As documentoSIDDAE

        'If DataGridView1.RowCount = 0 Then Exit Sub

        'Using folderDialogSelection As New FolderBrowserDialog With {
        '            .Description = "Seleccione la carpeta de salida",
        '            .SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
        '            .ShowNewFolderButton = True
        '    }
        '    If folderDialogSelection.ShowDialog = DialogResult.OK Then
        '        folderOut = folderDialogSelection.SelectedPath
        '    End If
        'End Using


        'Me.Cursor = Cursors.WaitCursor
        'LanzarSpinner()
        'For Each item As DataGridViewRow In DataGridView1.SelectedRows

        '    Try
        '        itemForMetadata = New documentoSIDDAE(item.Cells(1).Value)
        '        GenerarMetadatosNEM(itemForMetadata, folderOut)
        '    Catch ex As Exception
        '        GenerarLOG(ex.Message)
        '    End Try


        'Next
        'CerrarSpinner()
        'Me.Cursor = Cursors.Default
        'ToolStripStatusLabel1.Text = ""

        'If ModalQuestion($"Metadatos generados en {Environment.NewLine}{folderOut}{Environment.NewLine}¿Desea abrir la carpeta?") = DialogResult.Yes Then
        '    Try
        '        Process.Start(folderOut)
        '    Catch ex As Exception
        '        ModalError(ex.Message)
        '    End Try
        'End If



    End Sub

    Private Sub mnuGenerateThumb_Click(sender As Object, e As EventArgs) Handles mnuGenerateThumb.Click

        ModalInfo("En desarrollo")
        Exit Sub

        Dim idDoc As Integer
        Dim outputFolder As String
        Dim pathMiniatura As String
        Dim docu As docCartoSEE
        Dim hechos As Integer = 0
        Dim noHechos As Integer = 0

        Me.Cursor = Cursors.WaitCursor
        PictureBox1.Image = Nothing
        PictureBox2.Image = Nothing
        For i = 0 To DataGridView1.SelectedRows.Count - 1
            ToolStripStatusLabel2.Text = $"Generando miniatura {i + 1} de {DataGridView1.SelectedRows.Count}"
            Application.DoEvents()
            Me.Cursor = Cursors.WaitCursor
            Try
                'DataGridView1.Item(0, DataGridView1.SelectedRows(i).Index).Value.ToString
                idDoc = DataGridView1.Item(0, DataGridView1.SelectedRows(i).Index).Value
                docu = New docCartoSEE(idDoc)
                Application.DoEvents()

                If Not docu.rutaFicheroPDF.ToLower.EndsWith(".pdf") Then Continue For
                If Not IO.File.Exists(docu.rutaFicheroPDF) Then GenerarLOG($"Fichero PDF no localizado {docu.rutaFicheroPDF}") : noHechos += 1 : Continue For
                pathMiniatura = docu.rutaFicheroThumb
                If IO.File.Exists(pathMiniatura) Then IO.File.Delete(pathMiniatura)
                If Ghost_ExtractPagesPDF2JPG(docu.rutaFicheroPDF, pathMiniatura, True) Then hechos += 1
                docu = Nothing

            Catch ex As Exception
                ModalError("Error al generar miniatura")
                GenerarLOG(ex.Message)
                noHechos += 1
            End Try
        Next
        Me.Cursor = Cursors.Default

        ModalInfo($"Miniaturas generadas: {hechos}. No generadas: {noHechos}")

    End Sub



    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        Dim rutaPDF As String

        Try
            Me.Cursor = Cursors.WaitCursor
            rutaPDF = Button4.Tag
            If IO.File.Exists(rutaPDF) Then Process.Start(rutaPDF)
        Catch ex As Exception
            ModalError($"Error: {ex.Message}")
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub ProcResources(sender As Object, e As EventArgs) Handles Button6.Click, Button7.Click

        If lvDocResources.SelectedItems.Count = 0 Then Exit Sub
        If lvDocResources.SelectedItems.Count > 1 Then ModalExclamation("Seleccione un único recurso")

        Try
            Me.Cursor = Cursors.WaitCursor
            If IO.File.Exists(lvDocResources.SelectedItems(0).Tag) Then
                If sender.name = "Button6" Then Process.Start(lvDocResources.SelectedItems(0).Tag)
                If sender.name = "Button7" Then Process.Start(SacarDirDeRuta(lvDocResources.SelectedItems(0).Tag))
            End If
        Catch ex As Exception
            ModalError($"Error: {ex.Message}")
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub lvDocResources_DoubleClick(sender As Object, e As EventArgs) Handles lvDocResources.DoubleClick

        If lvDocResources.SelectedItems.Count = 0 Then Exit Sub
        Dim RutaFichero As String = lvDocResources.SelectedItems(0).Tag
        Me.Cursor = Cursors.WaitCursor
        LanzarVisorExterno(RutaFichero)
        Me.Cursor = Cursors.Default

    End Sub

    Private Sub lvDocResources_ItemDrag(sender As Object, e As ItemDragEventArgs) Handles lvDocResources.ItemDrag

        Dim strPath, strName, strFullPath As String

        'Put the dragged ListView Item or Items into a variable
        Dim items As ListView.SelectedListViewItemCollection = lvDocResources.SelectedItems

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

    Private Sub lvDocResources_DragDrop(sender As Object, e As DragEventArgs) Handles lvDocResources.DragDrop

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

    Private Sub mnuLaunchECWCrop_Click(sender As Object, e As EventArgs) Handles mnuLaunchECWCrop.Click


        Dim RutasECW As New ArrayList
        Dim archivoId As Integer
        Dim elemEntidadgeo As docCartoSEE

        ModalInfo("La composición se generará en epsg:23030")

        For i = 0 To DataGridView1.SelectedRows.Count - 1
            archivoId = DataGridView1.Item(0, DataGridView1.SelectedRows(i).Index).Value
            elemEntidadgeo = New docCartoSEE(archivoId)
            elemEntidadgeo.getGeoFiles()
            For Each rutaDoc As FileGeorref In elemEntidadgeo.listaFicherosGeo
                Application.DoEvents()
                If rutaDoc.EPSCode = "epsg23030" Then RutasECW.Add(rutaDoc.PathFile)
            Next
        Next

        If RutasECW.Count = 0 Then
            ModalInfo("No se ha localizado ningún documento georreferenciado")
        Else
            If GenerarProyectoGM(RutasECW, True) = True Then
                LanzarVisorExterno(AppFolderSetting & "\LaunchGM.gmw")
            End If
        End If
        RutasECW.Clear()
        RutasECW = Nothing


    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click

        Dim URLIberPIX As String
        Dim centerForIberPix As String

        If lvDocResources.Items.Count = 0 Then Exit Sub
        If lvDocResources.SelectedItems.Count = 0 Then Exit Sub
        If lvDocResources.SelectedItems(0).SubItems.Count <> 8 Then
            ModalInfo("No se ha encontrado información georreferenciada del documento")
            Exit Sub
        End If

        Try
            If IsNumeric(lvDocResources.SelectedItems(0).SubItems(7).Text) Then
                If ObtenerEscalar($"SELECT ST_X(ST_Centroid(ST_Transform(geom,3857)))::integer || ',' ||  ST_Y(ST_Centroid(ST_Transform(geom,3857)))::integer FROM bdsidschema.contornos WHERE idcontorno={lvDocResources.SelectedItems(0).SubItems(7).Text}", centerForIberPix) Then
                    If centerForIberPix <> "" Then
                        URLIberPIX = $"https://iberpix.cnig.es/iberpix/visor?center={centerForIberPix}&zoom=15&layers=WMTS*https://www.ign.es/wmts/minutas-cartograficas*Minutas*GoogleMapsCompatible*Planimetrías (1870-1950)*true*image/png*true*true*true"
                        Process.Start(URLIberPIX)
                    End If
                End If
            End If
        Catch ex As Exception
            ModalError(ex.Message)
        End Try
        If centerForIberPix = "" Then
            ModalInfo("No se ha encontrado información georreferenciada del documento")
        End If

    End Sub

    Sub ExtraerContornos(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuContornosExtract.Click

        Dim NomFich As String = ""
        Dim contador As Integer
        Dim ListaDocs As New ArrayList

        If DataGridView1.Rows.Count = 0 Then Exit Sub
        If DataGridView1.SelectedRows.Count = 0 Then
            ModalExclamation("Seleccione los documentos que quiere exportar")
            Exit Sub
        End If

        Using sfd As New SaveFileDialog With {
                            .Title = "Introduzca el nombre del fichero para los contornos",
                            .Filter = "Archivos GeoJSON *.geojson|*.geojson"
            }
            If sfd.ShowDialog() = Windows.Forms.DialogResult.OK Then
                NomFich = sfd.FileName
            End If
        End Using
        If NomFich = "" Then Exit Sub

        LanzarSpinner("Procesando información...")
        Me.Cursor = Cursors.WaitCursor

        For i = 0 To DataGridView1.SelectedRows.Count - 1
            ListaDocs.Add(DataGridView1.Item(0, DataGridView1.SelectedRows(i).Index).Value.ToString)
        Next

        ExportarContourDocTsoGJSON(ListaDocs, NomFich)

        Me.Cursor = Cursors.Default
        CerrarSpinner()

        ModalInfo("Fichero de contornos generado")

    End Sub

End Class