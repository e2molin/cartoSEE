Public Class frmDocumentacion

    'Variables para almacenar información

    Dim resultsetGeodocat As docCartoSEEquery

    Dim qryLoaded As String
    Dim NumPaginas_Album As Integer
    Dim NumPaginas_Query As Integer
    Dim ImagenesIniciadas As Boolean
    Dim filtroTiposDoc As String = ""
    Dim filtroEstadoDoc As String = ""

    'Variables para la impresion
    Dim FontPrint As Font
    Dim TableFont As Font
    Dim TituloInforme As String
    Dim Y As Integer
    Dim itm As Integer

    Dim ListaDocumentos() As docSIDCARTO

    Private Sub frmDocumentacion_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Size = New Point(1024, 700)
        ListView1.Location = New Point(5, 66)
        GroupBox1.Location = New Point(5, 66)
        GroupBox3.Location = New Point(5, 66)
        ListView1.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Bottom Or AnchorStyles.Right
        GroupBox1.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Bottom Or AnchorStyles.Right
        GroupBox3.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Bottom Or AnchorStyles.Right
        GroupBox1.Visible = False
        GroupBox3.Visible = False
        GroupBox2.Visible = False
        ListView1.Visible = True
        GroupBox2.Location = New Point(243, 72)
        Label14.Text = "Para ver los detalles de la imagen, pulsa sobre el nombre. Para añadirla al carro de compra, seleccione primero la imagen haciendo clic sobre ella"

        ' Si el resultado es único, muestro directamente los datos completos
        If ListView1.Items.Count = 1 Then
            ListView1.Visible = False
            GroupBox1.Visible = True
            MostrarDetalle(0)
            ListView1.Items(0).Selected = True
            Button1.Enabled = True
            Button2.Enabled = False
            Button8.Enabled = True
        Else
            Button1.Enabled = False
            Button2.Enabled = True
            Button8.Enabled = True
        End If
        If App_Permiso <> 2 Then
            Button13.Enabled = False
            mnuOnWMS.Enabled = False
            mnuOffWMS.Enabled = False
            menuTipoWMS0.Enabled = False
            menuTipoWMS1.Enabled = False
            menuTipoWMS2.Enabled = False
            menuTipoWMS3.Enabled = False
            menuTipoWMS4.Enabled = False
            mnuDetailMostrarWMS.Enabled = False
            mnuDetailOcultarWMS.Enabled = False
            mnuDetailTipoWMS0.Enabled = False
            mnuDetailTipoWMS1.Enabled = False
            mnuDetailTipoWMS2.Enabled = False
            mnuDetailTipoWMS3.Enabled = False
            mnuDetailTipoWMS4.Enabled = False
        End If
        If usuario_ISTARI = True Then
            Button20.Visible = True
        End If

        CheckBox1.Checked = IIf(Encabezados(1).Visible = True, True, False)
        CheckBox2.Checked = IIf(Encabezados(2).Visible = True, True, False)
        CheckBox3.Checked = IIf(Encabezados(3).Visible = True, True, False)
        CheckBox4.Checked = IIf(Encabezados(4).Visible = True, True, False)
        CheckBox5.Checked = IIf(Encabezados(5).Visible = True, True, False)
        CheckBox6.Checked = IIf(Encabezados(6).Visible = True, True, False)
        CheckBox7.Checked = IIf(Encabezados(7).Visible = True, True, False)
        CheckBox8.Checked = IIf(Encabezados(8).Visible = True, True, False)
        CheckBox9.Checked = IIf(Encabezados(9).Visible = True, True, False)
        CheckBox10.Checked = IIf(Encabezados(10).Visible = True, True, False)
        CheckBox11.Checked = IIf(Encabezados(11).Visible = True, True, False)
        CheckBox12.Checked = IIf(Encabezados(12).Visible = True, True, False)
        CheckBox13.Checked = IIf(Encabezados(13).Visible = True, True, False)
        CheckBox14.Checked = IIf(Encabezados(14).Visible = True, True, False)
        CheckBox15.Checked = IIf(Encabezados(15).Visible = True, True, False)
        CheckBox16.Checked = IIf(Encabezados(16).Visible = True, True, False)
        CheckBox17.Checked = IIf(Encabezados(17).Visible = True, True, False)
        CheckBox18.Checked = IIf(Encabezados(18).Visible = True, True, False)

        Application.DoEvents()
        ToolStripStatusLabel2.Text = ""
        Label2.Text = ""
        Label3.Text = ""
        Label8.Text = ""
        Label9.Text = ""
        Label39.Text = ""
        Label40.Text = ""
        PictureBox2.BackColor = Color.LightGray
        PictureBox3.BackColor = Color.LightGray
        PictureBox4.BackColor = Color.LightGray
        PictureBox5.BackColor = Color.LightGray
        PictureBox6.BackColor = Color.LightGray
        PictureBox7.BackColor = Color.LightGray


        lvAtributos.Columns.Clear()
        lvAtributos.Columns.Add("Propiedad", 120, HorizontalAlignment.Left)
        lvAtributos.Columns.Add("Valor", 250, HorizontalAlignment.Left)
        lvAtributos.SmallImageList = MDIPrincipal.ImageList2
        lvAtributos.FullRowSelect = True
        lvAtributos.View = View.Details
        lvAtributos.HeaderStyle = ColumnHeaderStyle.None

    End Sub


    Private Sub frmDocumentacion_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize

        ListView1.Size = New Point(Me.Width - 25, Me.Height - 150)
        GroupBox1.Size = New Point(Me.Width - 25, Me.Height - 150)
        GroupBox3.Size = New Point(Me.Width - 25, Me.Height - 150)
        If Me.Height < 700 Then
            picThumb.Visible = False
        Else
            picThumb.Visible = True
        End If

    End Sub

    Sub TabulacionVentanas(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click, Button8.Click

        If sender.name = "Button1" Or sender.name = "ToolStripButton3" Or sender.name = "mnuResconsulta1" Then
            ListView1.Visible = True
            GroupBox1.Visible = False
            GroupBox3.Visible = False
            ToolStripStatusLabel1.Text = "Resultados : " & ListView1.Items.Count.ToString & " documentos"
            Button1.Enabled = False
            Button2.Enabled = True
            Button8.Enabled = True
        ElseIf sender.name = "Button8" Or sender.name = "ToolMiniaturas" Or sender.name = "mnuResconsulta3" Then
            GroupBox1.Visible = False
            ListView1.Visible = False
            GroupBox3.Visible = True
            Button1.Enabled = True
            Button2.Enabled = True
            Button8.Enabled = False
            Me.Cursor = Cursors.WaitCursor
            If ImagenesIniciadas = False Then MostrarMiniaturas()
            ImagenesIniciadas = True
            Me.Cursor = Cursors.Default
        End If
        ToolStripStatusLabel1.Text = "Resultados: " & ListView1.Items.Count

    End Sub


    Private Sub ListView1_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles ListView1.ColumnClick
        '
        ' ========================================
        ' Usando la clase ListViewColumnSortSimple
        ' ========================================
        '
        ' Crear una instancia de la clase que realizará la comparación
        Dim oCompare As New ListViewColumnSortSimple()

        ' Asignar el orden de clasificación
        If ListView1.Sorting = SortOrder.Ascending Then
            oCompare.Sorting = SortOrder.Descending
        Else
            oCompare.Sorting = SortOrder.Ascending
        End If
        ListView1.Sorting = oCompare.Sorting
        '
        ' La columna en la que se ha pulsado
        oCompare.ColumnIndex = e.Column
        ' Asignar la clase que implementa IComparer
        ' y que se usará para realizar la comparación de cada elemento
        ListView1.ListViewItemSorter = oCompare
        '
        ' Cuando se asigna ListViewItemSorter no es necesario llamar al método Sort
        'ListView1.Sort()

    End Sub

    '---------------------------------------------------------------------------------------------------------
    'Procedimientos para imprimir
    '---------------------------------------------------------------------------------------------------------
#Region "Procedimientos para imprimir"


    Sub PrepararVistaPreliminar(ByVal sender As System.Object, ByVal e As System.EventArgs)

        If ListView1.Items.Count = 0 Then Exit Sub
        Dim pageWidth As Integer
        Dim newMargins As System.Drawing.Printing.Margins
        newMargins = New System.Drawing.Printing.Margins(0, 0, 0, 0)

        PrintDocument1.DefaultPageSettings.Margins = newMargins
        PrintDocument1.DefaultPageSettings.Landscape = True
        Try
            PageSetupDialog1.PageSettings = PrintDocument1.DefaultPageSettings
            PageSetupDialog1.PrinterSettings = PrintDialog1.PrinterSettings
            If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                PrintDocument1.DefaultPageSettings = PageSetupDialog1.PageSettings
                PrintDocument1.PrinterSettings = PageSetupDialog1.PrinterSettings
            Else
                Exit Sub
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        With PrintDocument1.DefaultPageSettings
            pageWidth = .PaperSize.Width - .Margins.Left - .Margins.Right
        End With
        PrintPreviewDialog1.Document = PrintDocument1
        PrintPreviewDialog1.WindowState = FormWindowState.Maximized
        Application.DoEvents()
        PrintPreviewDialog1.ShowDialog()

    End Sub


    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        Dim Y As Integer
        Dim iBucle As Integer
        Dim TabINI As Integer
        Dim TabGAP() As Integer
        Dim AnchuraMax As Integer
        Dim lines, cols As Integer

        'Definimos las fuentes
        TableFont = New Font("Arial", 8)
        FontPrint = New Font("Arial", 10, FontStyle.Bold)

        'Marcamos Primer Tabulador
        TabINI = PrintDocument1.DefaultPageSettings.Margins.Left

        'Calculamos la anchura de la que disponemos para escribir
        AnchuraMax = PrintDocument1.DefaultPageSettings.Bounds.Width - _
                    PrintDocument1.DefaultPageSettings.Margins.Left - _
                    PrintDocument1.DefaultPageSettings.Margins.Right

        'En este array guardamos las sucesivas
        ReDim TabGAP(ListView1.Columns.Count)

        'Aqui es donde comenzamos a escribir en sentido vertical
        Y = PrintDocument1.DefaultPageSettings.Margins.Top
        Application.DoEvents()

        'Titulo del Informe
        e.Graphics.DrawString(Me.Text, _
                            New Font("Arial", 18), Brushes.Blue, TabINI, Y)
        Y = Y + 40

        'Cabeceras del informe
        For iBucle = 0 To ListView1.Columns.Count - 1
            TabGAP(iBucle) = CType(AnchuraMax / ListView1.Columns.Count, Integer)
            e.Graphics.DrawString(ListView1.Columns(iBucle).Text, _
                                FontPrint, Brushes.Blue, TabINI + TabGAP(iBucle) * (iBucle), Y)
        Next

        Y = Y + 20
        Dim MaxLineasV As Integer
        MaxLineasV = 0
        If itm >= ListView1.Items.Count Then itm = 0
        While itm < ListView1.Items.Count
            'For Each linea As ListViewItem In ListView1.Items
            Dim str As String
            str = ListView1.Items(itm).Text
            'str = linea.Text
            Dim SizeString As New SizeF
            Dim NuevoFormato As New StringFormat
            Dim LineasV As Integer

            MaxLineasV = 0
            LineasV = CalcularAlturaCajetin(str, TableFont, TabGAP(0), e)
            If LineasV = 1 Then
                e.Graphics.DrawString(str, TableFont, Brushes.Black, TabINI, Y)
            Else
                e.Graphics.DrawString(str, TableFont, Brushes.Black, _
                            New RectangleF(TabINI, Y, TabGAP(0), LineasV * TableFont.Height), NuevoFormato)
            End If
            If MaxLineasV < LineasV Then MaxLineasV = LineasV

            e.Graphics.DrawLine(Pens.Black, TabINI, Y, AnchuraMax + TabINI, Y)
            'Pintamos la lista de valores de columnas
            For iBucle = 1 To ListView1.Columns.Count - 1
                str = ListView1.Items(itm).SubItems(iBucle).Text
                'str = linea.SubItems(iBucle).Text
                ''e.Graphics.DrawString(str, tableFOnt, Brushes.Black, TabINI + TabGAP * (iBucle), Y)
                LineasV = CalcularAlturaCajetin(str, TableFont, TabGAP(iBucle), e)
                If LineasV = 1 Then
                    e.Graphics.DrawString(str, TableFont, Brushes.Black, TabINI + TabGAP(iBucle) * (iBucle), Y)
                Else
                    e.Graphics.DrawString(str, TableFont, Brushes.Black, _
                                New RectangleF(TabINI + TabGAP(iBucle) * (iBucle), Y, TabGAP(iBucle), LineasV * TableFont.Height), NuevoFormato)
                End If
                If MaxLineasV < LineasV Then MaxLineasV = LineasV
            Next
            lines = 0
            cols = 0
            e.Graphics.MeasureString(str, TableFont, New SizeF(TabGAP(ListView1.Columns.Count - 1), LineasV), New StringFormat, cols, lines)
            Dim Yc As Integer
            Yc = Y
            Yc = Yc + TableFont.Height * 2 + 2 * 2
            Y = Y + MaxLineasV * TableFont.Height + 5
            Y = Math.Max(Y, Yc)
            itm = itm + 1
            With PrintDocument1.DefaultPageSettings
                ''e.Graphics.DrawLine(Pens.Black, .Margins.Left, Y, .PaperSize.Height, Y)
                e.Graphics.DrawLine(Pens.Black, TabINI, Y, AnchuraMax + TabINI, Y)
                If PrintDocument1.DefaultPageSettings.Landscape = True Then
                    If Y > 0.95 * (.PaperSize.Width - .Margins.Bottom) Then
                        e.HasMorePages = True
                        Exit Sub
                    End If
                Else
                    If Y > 0.95 * (.PaperSize.Height - .Margins.Bottom) Then
                        e.HasMorePages = True
                        Exit Sub
                    End If
                End If
            End With
        End While
        'Next
        e.HasMorePages = False

    End Sub

    Function CalcularAlturaCajetin(ByVal cadena As String, ByVal Fuente As Font, _
                                ByVal AnchoTabular As Integer, _
                                ByVal e As System.Drawing.Printing.PrintPageEventArgs) As Integer

        Dim SizeString As SizeF

        SizeString = e.Graphics.MeasureString(cadena, Fuente)
        Application.DoEvents()
        If AnchoTabular > SizeString.Width Then
            CalcularAlturaCajetin = 1
        Else
            CalcularAlturaCajetin = Math.Ceiling(SizeString.Width / AnchoTabular)
        End If


    End Function

#End Region

    Sub ExportarListaResultados2CSV(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click

        Dim CeldaOUT As String
        Dim LineaOUT As String
        Dim iBucle As Integer

        If ListView1.Items.Count = 0 Then Exit Sub
        With SaveFileDialog1
            .Title = "Introduzca el nombre del fichero"
            .Filter = "Archivos CSV *.csv|*.csv"
            .ShowDialog()
        End With
        If SaveFileDialog1.FileName = "" Then Exit Sub

        Dim sw As New System.IO.StreamWriter(SaveFileDialog1.FileName, False, System.Text.Encoding.Unicode)

        Dim Encabezado As String = ""
        For Each Cabecero As ColumnHeader In ListView1.Columns
            Application.DoEvents()
            Encabezado = Encabezado & Cabecero.Text & ";"
        Next
        If Encabezado.Length > 0 Then Encabezado = Encabezado.Substring(0, Encabezado.Length - 1)
        sw.WriteLine(Encabezado)


        For Each Linea As ListViewItem In ListView1.Items
            CeldaOUT = TratarCadenaExportarCSV(Linea.Text)
            LineaOUT = CeldaOUT
            For iBucle = 0 To Linea.SubItems.Count - 1
                LineaOUT = LineaOUT & ";" & TratarCadenaExportarCSV(Linea.SubItems(iBucle).Text)
            Next
            sw.WriteLine(LineaOUT)
        Next
        sw.Close()
        sw.Dispose()

        MessageBox.Show("Fichero de exportación generado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub resizingElements()

        ListView1.FullRowSelect = True
        ListView1.GridLines = False
        ListView1.View = View.Details
        ListView1.SmallImageList = MDIPrincipal.ImageList2
        ListView1.Items.Clear()
        PrepararEncabezados()
    End Sub

    Private Sub PrepararEncabezados()

        'Esta es fija en todas las consultas
        ListView1.Columns.Clear()
        ListView1.Columns.Add("Sellado", "Sellado", 80, HorizontalAlignment.Left, 4)
        For ibucle As Integer = 1 To Encabezados.Length - 1
            If Encabezados(ibucle).Visible = True Then
                ListView1.Columns.Add("column" & ibucle, Encabezados(ibucle).NombreEncabezado, Encabezados(ibucle).Anchura, HorizontalAlignment.Left, 4)
            End If
        Next

    End Sub

    Sub CargarDatosSIDCARTO_By_Filtro(ByVal filterSQL As String, _
                                    Optional ByVal estadosDoc As String = "", _
                                    Optional ByVal tiposDoc As String = "")

        registrarDatabaseLog("CargarDatosSIDCARTO_By_Filtro:" & filterSQL)
        If tiposDoc <> "" Then
            tiposDoc = tiposDoc.Substring(1, tiposDoc.Length - 2)
            filtroTiposDoc = " AND archivo.tipodoc_id IN (" & tiposDoc.Replace("-", ",") & ") "
        End If
        If estadosDoc <> "" Then
            estadosDoc = estadosDoc.Substring(1, estadosDoc.Length - 2)
            filtroEstadoDoc = " AND archivo.estadodoc_id IN (" & estadosDoc.Replace("-", ",") & ") "
        End If

        resultsetGeodocat = New docCartoSEEquery
        resultsetGeodocat.flag_CargarFicherosGEO = True
        resultsetGeodocat.getDocsSIDDAE_ByFiltroSellado("archivo.idarchivo>0 " & filtroTiposDoc & filtroEstadoDoc & filterSQL)
        resizingElements()
        populateListView()


    End Sub

    Sub CargarDatosSIDCARTO_By_Sellado(ByVal numSellado As Integer)
        registrarDatabaseLog("CargarDatosSIDCARTO_By_Sellado:" & numSellado)
        resultsetGeodocat = New docCartoSEEquery
        resultsetGeodocat.flag_CargarFicherosGEO = True
        resultsetGeodocat.getDocsSIDDAE_ByFiltroSellado("archivo.numdoc='" & String.Format("{0:000000}", numSellado) & "'")
        resizingElements()
        populateListView()

    End Sub

    Sub CargarDatosSIDCARTO_By_SelladoIntervalo(ByVal numSellado1 As Integer, ByVal numSellado2 As Integer)

        resultsetGeodocat = New docCartoSEEquery
        resultsetGeodocat.flag_CargarFicherosGEO = True
        resultsetGeodocat.getDocsSIDDAE_ByFiltroSellado("archivo.numdoc>='" & String.Format("{0:000000}", numSellado1) & "' and archivo.numdoc<='" & String.Format("{0:000000}", numSellado2) & "'")
        resizingElements()
        populateListView()

    End Sub

    Sub CargarDatosSIDCARTO_By_ListaSellado(ByVal listaSellado As ArrayList)

        Dim selloFormato As String = ""

        For Each elemSello In listaSellado
            If selloFormato = "" Then
                selloFormato = "'" & String.Format("{0:000000}", elemSello) & "'"
                Continue For
            End If
            selloFormato = selloFormato & ",'" & String.Format("{0:000000}", elemSello) & "'"
        Next
        If selloFormato = "" Then
            CerrarSpinner()
            Exit Sub
        End If

        resultsetGeodocat = New docCartoSEEquery
        resultsetGeodocat.flag_CargarFicherosGEO = True
        resultsetGeodocat.getDocsSIDDAE_ByFiltroSellado("archivo.numdoc in(" & selloFormato & ")")
        resizingElements()
        populateListView()

    End Sub


    Sub CargarDatosSIDCARTO_By_Indice(ByVal indiceDoc As Integer)
        registrarDatabaseLog("CargarDatosSIDCARTO_By_Indice:" & indiceDoc)
        resultsetGeodocat = New docCartoSEEquery
        resultsetGeodocat.flag_CargarFicherosGEO = True
        resultsetGeodocat.getDocsSIDDAE_ByFiltroSellado("archivo.idarchivo=" & indiceDoc)
        resizingElements()
        populateListView()

    End Sub

    Sub CargarDatosSIDCARTO_By_Provincia(ByVal codProv As Integer, _
                                    Optional ByVal tiposDoc As String = "", _
                                    Optional ByVal estadosDoc As String = "", _
                                    Optional ByVal filterSQL As String = "")

        If tiposDoc <> "" Then
            tiposDoc = tiposDoc.Substring(1, tiposDoc.Length - 2)
            filtroTiposDoc = " AND archivo.tipodoc_id IN (" & tiposDoc.Replace("-", ",") & ") "
        End If
        If estadosDoc <> "" Then
            estadosDoc = estadosDoc.Substring(1, estadosDoc.Length - 2)
            filtroEstadoDoc = " AND archivo.estadodoc_id IN (" & estadosDoc.Replace("-", ",") & ") "
        End If

        registrarDatabaseLog("CargarDatosSIDCARTO_By_Provincia:" & codProv)
        resultsetGeodocat = New docCartoSEEquery
        resultsetGeodocat.flag_CargarFicherosGEO = True
        resultsetGeodocat.getDocsSIDDAE_ByFiltroSellado("archivo.provincia_id=" & codProv & filtroTiposDoc & filtroEstadoDoc & filterSQL)
        resizingElements()
        populateListView()

    End Sub

    Sub CargarDatosSIDCARTO_By_MunicipioID(ByVal codMuniHisto As Integer, _
                                    Optional ByVal tiposDoc As String = "", _
                                    Optional ByVal estadosDoc As String = "", _
                                    Optional ByVal filterSQL As String = "")


        If tiposDoc <> "" Then
            tiposDoc = tiposDoc.Substring(1, tiposDoc.Length - 2)
            filtroTiposDoc = " AND archivo.tipodoc_id IN (" & tiposDoc.Replace("-", ",") & ") "
        End If
        If estadosDoc <> "" Then
            estadosDoc = estadosDoc.Substring(1, estadosDoc.Length - 2)
            filtroEstadoDoc = " AND archivo.estadodoc_id IN (" & estadosDoc.Replace("-", ",") & ") "
        End If

        registrarDatabaseLog("CargarDatosSIDCARTO_By_MunicipioID:" & codMuniHisto)
        resultsetGeodocat = New docCartoSEEquery
        resultsetGeodocat.flag_CargarFicherosGEO = True
        resultsetGeodocat.getDocsSIDDAE_ByFiltroSQL("munihisto.idmunihisto=" & codMuniHisto & filtroTiposDoc & filtroEstadoDoc & filterSQL)
        resizingElements()
        populateListView()

    End Sub

    Sub CargarDatosSIDCARTO_By_MunicipioINEActual(ByVal codINEActual As Integer, _
                                    Optional ByVal tiposDoc As String = "", _
                                    Optional ByVal estadosDoc As String = "", _
                                    Optional ByVal filterSQL As String = "")


        If tiposDoc <> "" Then
            tiposDoc = tiposDoc.Substring(1, tiposDoc.Length - 2)
            filtroTiposDoc = " AND archivo.tipodoc_id IN (" & tiposDoc.Replace("-", ",") & ") "
        End If
        If estadosDoc <> "" Then
            estadosDoc = estadosDoc.Substring(1, estadosDoc.Length - 2)
            filtroEstadoDoc = " AND archivo.estadodoc_id IN (" & estadosDoc.Replace("-", ",") & ") "
        End If

        registrarDatabaseLog("CargarDatosSIDCARTO_By_MunicipioINEActual:" & codINEActual)
        resultsetGeodocat = New docCartoSEEquery
        resultsetGeodocat.flag_CargarFicherosGEO = True
        resultsetGeodocat.getDocsSIDDAE_ByFiltroSQL("listamunicipios.inecorto='" & String.Format("{0:00000}", codINEActual) & "'" & filtroTiposDoc & filtroEstadoDoc & filterSQL)
        resizingElements()
        populateListView()

    End Sub



    Sub CargarDatosSIDCARTO_By_Entorno(ByVal Xmax As Integer, ByVal Ymax As Integer, _
                                ByVal Xmin As Integer, ByVal Ymin As Integer)

        resizingElements()
        DameDocumentacionSIDCARTO_byEntorno(Xmax, Ymax, Xmin, Ymin, ListaDocumentos, qryLoaded)
        Application.DoEvents()
        If IsNothing(ListaDocumentos) Then Exit Sub
        RellenarLisview(ListaDocumentos)

    End Sub

    Sub CargarUltimosDatos(ByVal NumReg As Integer)
        resizingElements()
        DameDocumentacionSIDCARTO_UltimosRegistros(ListaDocumentos, NumReg, qryLoaded)
        Application.DoEvents()
        If IsNothing(ListaDocumentos) Then Exit Sub
        RellenarLisview(ListaDocumentos)
    End Sub


    Private Sub RellenarLisview(ByRef ListaDocumentos() As docSIDCARTO, _
                                    Optional ByVal EstadoDocumento As String = "", _
                                    Optional ByVal TipoDocumento As String = "")

        Dim elementoLV As ListViewItem
        Dim iBucle As Integer
        Dim iBucleTMP As Integer
        Dim TmpDoc() As docSIDCARTO
        If ListaDocumentos Is Nothing Then Exit Sub
        ReDim TmpDoc(ListaDocumentos.GetUpperBound(0))

        iBucle = -1
        iBucleTMP = -1
        NumPaginas_Query = 0

        For Each Documento As docSIDCARTO In ListaDocumentos
            iBucle = iBucle + 1
            If EstadoDocumento <> "" Then
                If EstadoDocumento.Contains("-" & Documento.CodEstado & "-") = False Then _
                                                                                Continue For
            End If
            If TipoDocumento <> "" Then
                If TipoDocumento.Contains("-" & Documento.CodTipo & "-") = False Then _
                                                                                Continue For
            End If
            iBucleTMP = iBucleTMP + 1
            TmpDoc(iBucleTMP) = ListaDocumentos(iBucle)
            elementoLV = New ListViewItem
            elementoLV.Text = Documento.Sellado

            'For ibucle As Integer = 1 To 10
            '    If Encabezados(ibucle).Visible = True Then
            '        ListView1.Columns.Add(Encabezados(ibucle).NombreEncabezado, Encabezados(ibucle).Anchura, HorizontalAlignment.Right)
            '    End If
            'Next
            If Encabezados(1).Visible = True Then elementoLV.SubItems.Add(Documento.Tipo & IIf(Documento.subTipoDoc = "", "", "/" & Documento.subTipoDoc))
            If Encabezados(2).Visible = True Then elementoLV.SubItems.Add(Documento.Tomo)
            If Encabezados(3).Visible = True Then elementoLV.SubItems.Add(Documento.Estado)
            If Encabezados(4).Visible = True Then elementoLV.SubItems.Add(Documento.Escala)
            If Encabezados(5).Visible = True Then elementoLV.SubItems.Add(Documento.fechaPrincipal)
            If Encabezados(6).Visible = True Then elementoLV.SubItems.Add(Documento.Municipios.Replace("#", ","))
            If Encabezados(7).Visible = True Then elementoLV.SubItems.Add(Documento.Signatura)
            If Encabezados(8).Visible = True Then elementoLV.SubItems.Add(Documento.Coleccion)
            If Encabezados(9).Visible = True Then elementoLV.SubItems.Add(Documento.Subdivision)
            If Encabezados(10).Visible = True Then elementoLV.SubItems.Add(Documento.Anejo)
            If Encabezados(11).Visible = True Then elementoLV.SubItems.Add(Documento.ObservacionesStandard)

            If Encabezados(12).Visible = True Then elementoLV.SubItems.Add(Documento.proceCarpeta)
            If Encabezados(13).Visible = True Then elementoLV.SubItems.Add(Documento.proceHoja)
            If Encabezados(14).Visible = True Then elementoLV.SubItems.Add(Documento.fechasModificaciones)
            If Encabezados(15).Visible = True Then elementoLV.SubItems.Add(Documento.Horizontal & " x " & Documento.Vertical)
            If Encabezados(16).Visible = True Then elementoLV.SubItems.Add(Documento.JuntaEstadistica)
            If Encabezados(17).Visible = True Then elementoLV.SubItems.Add(Documento.Provincias)
            If Encabezados(18).Visible = True Then elementoLV.SubItems.Add(Documento.Observaciones)

            elementoLV.Tag = iBucleTMP
            If iBucle Mod 2 = 0 Then
                elementoLV.BackColor = Color.White
            Else
                elementoLV.BackColor = Color.WhiteSmoke
            End If
            ListView1.Items.Add(elementoLV)
            elementoLV = Nothing
        Next

        If EstadoDocumento <> "" Or TipoDocumento <> "" Then
            ReDim Preserve TmpDoc(iBucleTMP)
            Erase ListaDocumentos
            ReDim ListaDocumentos(iBucleTMP)
            ListaDocumentos = TmpDoc
        End If

        If ListView1.Items.Count > 0 Then
            NumPaginas_Album = CType(Math.Truncate((ListView1.Items.Count - 1) / 6) + 1, Integer)
        End If
        ToolStripStatusLabel1.Text = "Resultados : " & ListView1.Items.Count.ToString & " documentos"
        CerrarSpinner()

    End Sub


    Private Sub populateListView()

        Dim elementoLV As ListViewItem
        Dim iBucle As Integer
        Dim iBucleTMP As Integer
        'Dim TmpDoc() As docSIDCARTO
        If resultsetGeodocat.resultados.Count = 0 Then Exit Sub
        'ReDim TmpDoc(ListaDocumentos.GetUpperBound(0))

        iBucle = -1
        iBucleTMP = -1
        NumPaginas_Query = 0

        For Each docu As docCartoSEE In resultsetGeodocat.resultados
            iBucle = iBucle + 1
            'If EstadoDocumento <> "" Then
            '    If EstadoDocumento.Contains("-" & Documento.CodEstado & "-") = False Then _
            '                                                                    Continue For
            'End If
            'If TipoDocumento <> "" Then
            '    If TipoDocumento.Contains("-" & Documento.CodTipo & "-") = False Then _
            '                                                                    Continue For
            'End If
            iBucleTMP = iBucleTMP + 1
            'TmpDoc(iBucleTMP) = ListaDocumentos(iBucle)
            elementoLV = New ListViewItem
            elementoLV.Text = docu.Sellado

            'For ibucle As Integer = 1 To 10
            '    If Encabezados(ibucle).Visible = True Then
            '        ListView1.Columns.Add(Encabezados(ibucle).NombreEncabezado, Encabezados(ibucle).Anchura, HorizontalAlignment.Right)
            '    End If
            'Next
            If Encabezados(1).Visible = True Then elementoLV.SubItems.Add(docu.Tipo & IIf(docu.subTipoDoc = "", "", "/" & docu.subTipoDoc))
            If Encabezados(2).Visible = True Then elementoLV.SubItems.Add(docu.Tomo)
            If Encabezados(3).Visible = True Then elementoLV.SubItems.Add(docu.Estado)
            If Encabezados(4).Visible = True Then elementoLV.SubItems.Add(docu.Escala)
            If Encabezados(5).Visible = True Then elementoLV.SubItems.Add(docu.fechaPrincipal)
            If Encabezados(6).Visible = True Then elementoLV.SubItems.Add(docu.municipiosHistoLiteral)
            If Encabezados(7).Visible = True Then elementoLV.SubItems.Add(docu.Signatura)
            If Encabezados(8).Visible = True Then elementoLV.SubItems.Add(docu.Coleccion)
            If Encabezados(9).Visible = True Then elementoLV.SubItems.Add(docu.Subdivision)
            If Encabezados(10).Visible = True Then elementoLV.SubItems.Add(docu.Anejo)
            If Encabezados(11).Visible = True Then elementoLV.SubItems.Add(docu.ObservacionesStandard)
            If Encabezados(12).Visible = True Then elementoLV.SubItems.Add(docu.proceCarpeta)
            If Encabezados(13).Visible = True Then elementoLV.SubItems.Add(docu.proceHoja)
            If Encabezados(14).Visible = True Then elementoLV.SubItems.Add(docu.fechasModificaciones)
            If Encabezados(15).Visible = True Then elementoLV.SubItems.Add(docu.Horizontal & " x " & docu.Vertical)
            If Encabezados(16).Visible = True Then elementoLV.SubItems.Add(docu.JuntaEstadistica)
            If Encabezados(17).Visible = True Then elementoLV.SubItems.Add(docu.Provincias)
            If Encabezados(18).Visible = True Then elementoLV.SubItems.Add(docu.Observaciones)
            If Encabezados(19).Visible = True Then elementoLV.SubItems.Add(docu.getInfoWMS)
            If Encabezados(20).Visible = True Then elementoLV.SubItems.Add(docu.getZIndex)

            elementoLV.Tag = iBucleTMP
            If iBucle Mod 2 = 0 Then
                elementoLV.BackColor = Color.White
            Else
                elementoLV.BackColor = Color.WhiteSmoke
            End If
            ListView1.Items.Add(elementoLV)
            elementoLV = Nothing
        Next

        'If EstadoDocumento <> "" Or TipoDocumento <> "" Then
        '    ReDim Preserve TmpDoc(iBucleTMP)
        '    Erase ListaDocumentos
        '    ReDim ListaDocumentos(iBucleTMP)
        '    ListaDocumentos = TmpDoc
        'End If

        If ListView1.Items.Count > 0 Then
            NumPaginas_Album = CType(Math.Truncate((ListView1.Items.Count - 1) / 6) + 1, Integer)
        End If
        ToolStripStatusLabel1.Text = "Resultados : " & ListView1.Items.Count.ToString & " documentos"
        CerrarSpinner()

    End Sub


    '---------------------------------------------------------------------------------------------------------
    'Procedimientos para lanzar las tareas de impresion y visualización
    '---------------------------------------------------------------------------------------------------------
    Sub MostrarDetalles(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
                                ListView1.DoubleClick, Button2.Click, Button4.Click, Button5.Click


        If ListView1.Items.Count = 0 Then Exit Sub
        If ListView1.SelectedItems.Count = 0 Then Exit Sub

        'Segun el control que llega aqui, ejecuto una opción u otra
        If sender.name.ToString = "Button2" Then
            MostrarDetalle(ListView1.SelectedItems(0).Tag)

        ElseIf sender.name.ToString = "Button4" Then
            MostrarDetalle(Button4.Tag.ToString)
        ElseIf sender.name.ToString = "Button5" Then
            MostrarDetalle(Button5.Tag.ToString)
        ElseIf sender.name.ToString = "ListView1" Then
            MostrarDetalle(ListView1.SelectedItems(0).Tag)
        End If
        Button1.Enabled = True
        Button2.Enabled = False
        Button8.Enabled = True
        ToolStripStatusLabel1.Text = "Vista en detalle"

    End Sub

    Sub MostrarDetalle(ByVal NumElemento As Integer)

        Dim elementoLV As ListViewItem
        Dim g1 As ListViewGroup
        Dim g2 As ListViewGroup
        Dim g3 As ListViewGroup

        If NumElemento < 0 Or NumElemento > resultsetGeodocat.resultados.Count - 1 Then Exit Sub
        Label4.Text = ""
        Label5.Text = ""
        Label6.Text = ""
        Label7.Text = ""
        Label19.Text = ""
        Label1.Text = ""
        Button10.Tag = ""


        ListView1.Visible = False
        GroupBox3.Visible = False
        Button4.Tag = NumElemento - 1
        Button5.Tag = NumElemento + 1
        GroupBox1.Visible = True
        GroupBox1.Tag = NumElemento
        GroupBox1.Text = resultsetGeodocat.resultados(NumElemento).Tipo & _
                            IIf(resultsetGeodocat.resultados(NumElemento).subTipoDoc = "", "", " - " & resultsetGeodocat.resultados(NumElemento).subTipoDoc) & " " & resultsetGeodocat.resultados(NumElemento).Sellado

        g1 = New ListViewGroup("Hoja de características")
        lvAtributos.Groups.Add(g1)
        g2 = New ListViewGroup("Georreferenciación")
        lvAtributos.Groups.Add(g2)
        lvAtributos.Items.Clear()
        'Hoja de características

        elementoLV = New ListViewItem : elementoLV.Text = "Subtipo" : elementoLV.ImageIndex = 4
        elementoLV.SubItems.Add(resultsetGeodocat.resultados(NumElemento).subTipoDoc) : elementoLV.Group = g1
        lvAtributos.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem : elementoLV.Text = "Horizontal" : elementoLV.ImageIndex = 4
        elementoLV.SubItems.Add(resultsetGeodocat.resultados(NumElemento).Horizontal & "cm.") : elementoLV.Group = g1
        lvAtributos.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem : elementoLV.Text = "Vertical" : elementoLV.ImageIndex = 4
        elementoLV.SubItems.Add(resultsetGeodocat.resultados(NumElemento).Vertical & "cm.") : elementoLV.Group = g1
        lvAtributos.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem : elementoLV.Text = "Escala" : elementoLV.ImageIndex = 4
        elementoLV.SubItems.Add("1:" & resultsetGeodocat.resultados(NumElemento).Escala) : elementoLV.Group = g1
        lvAtributos.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem : elementoLV.Text = "Estado" : elementoLV.ImageIndex = 4
        elementoLV.SubItems.Add(resultsetGeodocat.resultados(NumElemento).Estado) : elementoLV.Group = g1
        lvAtributos.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem : elementoLV.Text = "Standard" : elementoLV.ImageIndex = 4
        elementoLV.SubItems.Add(resultsetGeodocat.resultados(NumElemento).ObservacionesStandard) : elementoLV.Group = g1
        lvAtributos.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem : elementoLV.Text = "Carpeta" : elementoLV.ImageIndex = 4
        elementoLV.SubItems.Add(resultsetGeodocat.resultados(NumElemento).proceCarpeta) : elementoLV.Group = g1
        lvAtributos.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem : elementoLV.Text = "Hoja" : elementoLV.ImageIndex = 4
        elementoLV.SubItems.Add(resultsetGeodocat.resultados(NumElemento).proceHoja) : elementoLV.Group = g1
        lvAtributos.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem : elementoLV.Text = "Signatura" : elementoLV.ImageIndex = 4
        elementoLV.SubItems.Add(resultsetGeodocat.resultados(NumElemento).Signatura) : elementoLV.Group = g1
        lvAtributos.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem : elementoLV.Text = "Tomo" : elementoLV.ImageIndex = 4
        elementoLV.SubItems.Add(resultsetGeodocat.resultados(NumElemento).tomo) : elementoLV.Group = g1
        lvAtributos.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem : elementoLV.Text = "Modificado en" : elementoLV.ImageIndex = 4
        elementoLV.SubItems.Add(resultsetGeodocat.resultados(NumElemento).fechasModificaciones) : elementoLV.Group = g1
        lvAtributos.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem : elementoLV.Text = "Colección" : elementoLV.ImageIndex = 4
        elementoLV.SubItems.Add(resultsetGeodocat.resultados(NumElemento).Coleccion) : elementoLV.Group = g1
        lvAtributos.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem : elementoLV.Text = "Subdivisión" : elementoLV.ImageIndex = 4
        elementoLV.SubItems.Add(resultsetGeodocat.resultados(NumElemento).Subdivision) : elementoLV.Group = g1
        lvAtributos.Items.Add(elementoLV) : elementoLV = Nothing

        elementoLV = New ListViewItem : elementoLV.Text = "Junta" : elementoLV.ImageIndex = 4
        elementoLV.SubItems.Add(resultsetGeodocat.resultados(NumElemento).JuntaEstadistica) : elementoLV.Group = g1
        lvAtributos.Items.Add(elementoLV) : elementoLV = Nothing


        'Metemos el resto de las propiedades
        ListView2.Items.Clear()
        ListView2.View = View.LargeIcon
        ListView2.LargeImageList = MDIPrincipal.ImageList1

        Label1.Text = resultsetGeodocat.resultados(NumElemento).Tomo
        Label4.Text = resultsetGeodocat.resultados(NumElemento).Provincias
        Label5.Text = resultsetGeodocat.resultados(NumElemento).municipiosHistoLiteralFull
        Label6.Text = resultsetGeodocat.resultados(NumElemento).Anejo
        Label7.Text = resultsetGeodocat.resultados(NumElemento).fechaPrincipal
        Label19.Text = resultsetGeodocat.resultados(NumElemento).Observaciones


        'Repositorio de documento imagen asociado
        '-----------------------------------------------------------------------------------------------------------------
        elementoLV = New ListViewItem
        elementoLV.Text = "Alta"
        elementoLV.ImageIndex = 4
        elementoLV.Tag = resultsetGeodocat.resultados(NumElemento).rutaFicheroAltaRes
        If System.IO.File.Exists(resultsetGeodocat.resultados(NumElemento).rutaFicheroAltaRes) = False Then
            elementoLV.ForeColor = Color.Red
        Else
            elementoLV.ForeColor = Color.Black
        End If
        ListView2.Items.Add(elementoLV)
        elementoLV = Nothing

        elementoLV = New ListViewItem
        elementoLV.Text = "Media"
        elementoLV.ImageIndex = 4
        elementoLV.Tag = resultsetGeodocat.resultados(NumElemento).rutaFicheroBajaRes

        If System.IO.File.Exists(resultsetGeodocat.resultados(NumElemento).rutaFicheroBajaRes) = False Then
            elementoLV.ForeColor = Color.Red
        Else
            elementoLV.ForeColor = Color.Black
        End If
        ListView2.Items.Add(elementoLV)
        elementoLV = Nothing

        'Repositorio de documento georreferenciado asociado
        '-----------------------------------------------------------------------------------------------------------------
        'Dim RutasDoc() As String
        Dim iECW As Integer
        'DameRutasFicherosECW(resultsetGeodocat.resultados(NumElemento), RutasDoc)
        'If RutasDoc.Length > 0 Then
        '    Button10.Tag = SacarDirDeRuta(RutasDoc(0))
        'End If

        Application.DoEvents()
        iECW = 0
        For Each fila As DataRow In resultsetGeodocat.resultados(NumElemento).rcdgeoFiles.Select()
            iECW = iECW + 1

            elementoLV = New ListViewItem : elementoLV.Text = "Fichero " & iECW : elementoLV.ImageIndex = 4 : elementoLV.BackColor = IIf(iECW Mod 2 = 0, Color.GhostWhite, Color.White)
            elementoLV.SubItems.Add(fila.Item("nombre").ToString & ".ecw") : elementoLV.Tag = fila.Item("idcontorno").ToString : elementoLV.Group = g2
            lvAtributos.Items.Add(elementoLV) : elementoLV = Nothing

            elementoLV = New ListViewItem : elementoLV.Text = "WMS " & iECW : elementoLV.ImageIndex = 4 : elementoLV.BackColor = IIf(iECW Mod 2 = 0, Color.GhostWhite, Color.White)
            elementoLV.SubItems.Add(fila.Item("tipowms").ToString & " (" & IIf(fila.Item("mostrarwms") = 1, "Sí", "No") & ")") : elementoLV.Tag = fila.Item("idcontorno").ToString : elementoLV.Group = g2
            lvAtributos.Items.Add(elementoLV) : elementoLV = Nothing

            elementoLV = New ListViewItem : elementoLV.Text = "Z-Index " & iECW : elementoLV.ImageIndex = 4 : elementoLV.BackColor = IIf(iECW Mod 2 = 0, Color.GhostWhite, Color.White)
            elementoLV.SubItems.Add(fila.Item("zindex").ToString) : elementoLV.Tag = fila.Item("idcontorno").ToString : elementoLV.Group = g2
            lvAtributos.Items.Add(elementoLV) : elementoLV = Nothing

            elementoLV = Nothing

        Next

        'resultsetGeodocat.resultados(NumElemento).getGeoFiles()
        iECW = 0
        For Each Rutageo As String In resultsetGeodocat.resultados(NumElemento).listaFicherosGeo
            Application.DoEvents()
            '    If IsNothing(Rutageo) Then Continue For
            iECW = iECW + 1
            elementoLV = New ListViewItem
            If resultsetGeodocat.resultados(NumElemento).listaFicherosGeo.count = 1 Then
                elementoLV.Text = "ECW"
            Else
                'elementoLV.Text = "ECW - " & iECW.ToString
                elementoLV.Text = SacarFileDeRuta(Rutageo)
            End If
            elementoLV.ImageIndex = 5
            elementoLV.Tag = Rutageo
            If System.IO.File.Exists(Rutageo) = False Then
                elementoLV.ForeColor = Color.Red
            Else
                elementoLV.ForeColor = Color.Black
            End If
            ListView2.Items.Add(elementoLV)
            elementoLV = Nothing
        Next
        'resultsetGeodocat.resultados(NumElemento).rutaFicheroThumb
        Application.DoEvents()
        If System.IO.File.Exists(resultsetGeodocat.resultados(NumElemento).rutaFicheroThumb) Then
            CargarImagen(picThumb, resultsetGeodocat.resultados(NumElemento).rutaFicheroThumb)
        Else
            CargarImagen(picThumb, My.Application.Info.DirectoryPath & "\default.png")
        End If

        ToolStripStatusLabel1.Text = "Documento " & (NumElemento + 1).ToString & " documentos" & " de " & resultsetGeodocat.resultados.Count

    End Sub

    Sub MostrarMiniaturas(Optional ByVal Pagina As Integer = 0)

        Dim DiapoInicio As Integer = Pagina * 6
        Dim DiapoFinal As Integer = Pagina * 6 + 5
        Dim RutaImagen As String

        If Pagina < 0 Then Exit Sub
        Button18.Tag = -1
        'If DiapoInicio > ListaDocumentos.GetUpperBound(0) Then Exit Sub

        LimpiarImagenes()
        Label2.Text = ""
        Label3.Text = ""
        Label8.Text = ""
        Label9.Text = ""
        Label39.Text = ""
        Label40.Text = ""


        Button14.Tag = Pagina - 1
        GroupBox3.Text = "Página " & (Pagina + 1).ToString & " de " & NumPaginas_Album

        'Diapo1
        RutaImagen = rutaRepo & "\_Miniaturas\" & resultsetGeodocat.resultados(DiapoInicio).FicheroJPG
        Label2.Text = resultsetGeodocat.resultados(DiapoInicio).Tipo & _
                        IIf(resultsetGeodocat.resultados(DiapoInicio).subTipoDoc = "", "", "/" & resultsetGeodocat.resultados(DiapoInicio).subTipoDoc) & _
                        ": " & resultsetGeodocat.resultados(DiapoInicio).Sellado
        Label2.Tag = DiapoInicio
        CargarImagen(PictureBox2, RutaImagen)

        'Diapo2
        If DiapoInicio + 1 > resultsetGeodocat.resultados.Count - 1 Then Exit Sub
        RutaImagen = rutaRepo & "\_Miniaturas\" & resultsetGeodocat.resultados(DiapoInicio + 1).FicheroJPG
        Label3.Text = resultsetGeodocat.resultados(DiapoInicio + 1).Tipo & _
                        IIf(resultsetGeodocat.resultados(DiapoInicio + 1).subTipoDoc = "", "", "/" & resultsetGeodocat.resultados(DiapoInicio + 1).subTipoDoc) & _
                        ": " & resultsetGeodocat.resultados(DiapoInicio + 1).Sellado
        Label3.Tag = DiapoInicio + 1
        CargarImagen(PictureBox3, RutaImagen)

        'Diapo3
        If DiapoInicio + 2 > resultsetGeodocat.resultados.Count - 1 Then Exit Sub
        RutaImagen = rutaRepo & "\_Miniaturas\" & resultsetGeodocat.resultados(DiapoInicio + 2).FicheroJPG
        Label8.Text = resultsetGeodocat.resultados(DiapoInicio + 2).Tipo & _
                        IIf(resultsetGeodocat.resultados(DiapoInicio + 2).subTipoDoc = "", "", "/" & resultsetGeodocat.resultados(DiapoInicio + 2).subTipoDoc) & _
                        ": " & resultsetGeodocat.resultados(DiapoInicio + 2).Sellado
        Label8.Tag = DiapoInicio + 2
        CargarImagen(PictureBox4, RutaImagen)

        'Diapo4
        If DiapoInicio + 3 > resultsetGeodocat.resultados.Count - 1 Then Exit Sub
        RutaImagen = rutaRepo & "\_Miniaturas\" & resultsetGeodocat.resultados(DiapoInicio + 3).FicheroJPG
        Label9.Text = resultsetGeodocat.resultados(DiapoInicio + 3).Tipo & _
                        IIf(resultsetGeodocat.resultados(DiapoInicio + 3).subTipoDoc = "", "", "/" & resultsetGeodocat.resultados(DiapoInicio + 3).subTipoDoc) & _
                        ": " & resultsetGeodocat.resultados(DiapoInicio + 3).Sellado
        Label9.Tag = DiapoInicio + 3
        CargarImagen(PictureBox5, RutaImagen)

        'Diapo5
        If DiapoInicio + 4 > resultsetGeodocat.resultados.Count - 1 Then Exit Sub
        RutaImagen = rutaRepo & "\_Miniaturas\" & resultsetGeodocat.resultados(DiapoInicio + 4).FicheroJPG
        Label39.Text = resultsetGeodocat.resultados(DiapoInicio + 4).Tipo & _
                        IIf(resultsetGeodocat.resultados(DiapoInicio + 4).subTipoDoc = "", "", "/" & resultsetGeodocat.resultados(DiapoInicio + 4).subTipoDoc) & _
                        ": " & resultsetGeodocat.resultados(DiapoInicio + 4).Sellado
        Label39.Tag = DiapoInicio + 4
        CargarImagen(PictureBox6, RutaImagen)

        'Diapo6
        If DiapoInicio + 5 > resultsetGeodocat.resultados.Count - 1 Then Exit Sub
        RutaImagen = rutaRepo & "\_Miniaturas\" & resultsetGeodocat.resultados(DiapoInicio + 5).FicheroJPG
        Label40.Text = resultsetGeodocat.resultados(DiapoInicio + 5).Tipo & _
                        IIf(resultsetGeodocat.resultados(DiapoInicio + 5).subTipoDoc = "", "", "/" & resultsetGeodocat.resultados(DiapoInicio + 5).subTipoDoc) & _
                        ": " & resultsetGeodocat.resultados(DiapoInicio + 5).Sellado
        Label40.Tag = DiapoInicio + 5
        CargarImagen(PictureBox7, RutaImagen)

        Button18.Tag = Pagina + 1

        PictureBox2.BorderStyle = BorderStyle.FixedSingle
        PictureBox3.BorderStyle = BorderStyle.FixedSingle
        PictureBox4.BorderStyle = BorderStyle.FixedSingle
        PictureBox5.BorderStyle = BorderStyle.FixedSingle
        PictureBox6.BorderStyle = BorderStyle.FixedSingle
        PictureBox7.BorderStyle = BorderStyle.FixedSingle




    End Sub

    Sub DesplazarAlbum(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button14.Click, Button18.Click

        If sender.name = "Button14" Then
            MostrarMiniaturas(Button14.Tag)
        ElseIf sender.name = "Button18" Then
            MostrarMiniaturas(Button18.Tag)
        End If
    End Sub


    Sub LanzarVisordeJPG(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click, picThumb.Click

        Dim RutaDoc As String
        Dim idDoc As Integer
        RutaDoc = ""
        If GroupBox1.Visible = True Then
            idDoc = CType(GroupBox1.Tag, Integer)
        Else
            If ListView1.Items.Count = 0 Then Exit Sub
            If ListView1.Items.Count = 1 Then
                idDoc = 0
            End If
            If ListView1.SelectedItems.Count > 0 Then
                idDoc = CType(ListView1.SelectedItems(0).Tag, Integer)
            End If
        End If

        If CalidadFavorita = "Alta" Then
            RutaDoc = resultsetGeodocat.resultados(idDoc).rutaFicheroAltaRes
        Else
            RutaDoc = resultsetGeodocat.resultados(idDoc).rutaFicheroBajaRes
        End If
        Me.Cursor = Cursors.WaitCursor
        LanzarVisorExterno(RutaDoc)
        Me.Cursor = Cursors.Default

    End Sub


    Sub LanzarImpresionJPG(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button21.Click

        Dim RutaDoc As String
        Dim idDoc As Integer
        RutaDoc = ""
        If GroupBox1.Visible = True Then
            idDoc = CType(GroupBox1.Tag, Integer)
        Else
            If ListView1.Items.Count = 0 Then Exit Sub
            If ListView1.Items.Count = 1 Then
                idDoc = 0
            End If
            If ListView1.SelectedItems.Count > 0 Then
                idDoc = CType(ListView1.SelectedItems(0).Tag, Integer)
            End If
        End If

        If CalidadFavorita = "Alta" Then
            RutaDoc = rutaRepo & "\_Scan400\" & ListaDocumentos(idDoc).FicheroJPG
        Else
            RutaDoc = rutaRepo & "\_Scan250\" & ListaDocumentos(idDoc).FicheroJPG.Replace("\", "250\")
        End If
        If Not System.IO.File.Exists(RutaDoc) Then
            MessageBox.Show("No se localiza el fichero " & RutaDoc, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        Me.Cursor = Cursors.WaitCursor

        Try
            If VisorPrint <> "" Then
                Process.Start(VisorPrint, RutaDoc)
            Else
                Process.Start(RutaDoc)
            End If

        Catch ex As Exception
            MessageBox.Show("Error:" & ex.Message & System.Environment.NewLine & _
                            RutaDoc, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
        Me.Cursor = Cursors.Default

    End Sub

    Sub LanzarTareaGuardarEnCarpeta(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                Handles Button12.Click, ToolStripMenuItem5.Click

        Dim nCopy As Integer
        Dim RutaSalida As String
        Dim okProc As Boolean
        Dim contador As Integer
        Dim NoCopiados As Integer
        Dim Copiados = 0
        Dim CadenaLOG As String
        Dim NumSelect As Integer
        Dim FicheroCopiaOrigen As New ArrayList
        Dim generarLink As Boolean = False

        If ListView1.Items.Count = 0 Then Exit Sub
        If ListView1.Items.Count = 1 And ListView1.SelectedItems.Count = 0 Then
            ListView1.Items(0).Selected = True
        End If
        If ListView1.SelectedItems.Count = 0 Then Exit Sub


        Dim dlgOpcionExport As New OptionDialog
        Dim TipoExport As String = ""
        Dim NombreTipoExport As String = ""
        If dlgOpcionExport.ShowDialog = Windows.Forms.DialogResult.OK Then
            dlgOpcionExport.CheckRadioButtonResult(TipoExport, NombreTipoExport)
        Else
            Exit Sub
        End If

        FolderBrowserDialog1.ShowNewFolderButton = True
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
        If System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath) = False Then Exit Sub
        okProc = True
        contador = 0
        NoCopiados = 0
        Copiados = 0
        'Creamos un fichero de texto con el resultado de nuestra copia
        Dim sw As New System.IO.StreamWriter(FolderBrowserDialog1.SelectedPath & "\_Lista.csv", _
                                False, System.Text.Encoding.Unicode)

        CadenaLOG = "Nº de sellado" & ";" & "Tipo" & ";" & "Municipios" & ";" & "Fecha principal"
        sw.WriteLine(CadenaLOG)
        NumSelect = ListView1.SelectedItems.Count
        If MessageBox.Show("¿Desa crear un link para cada archivo?", AplicacionTitulo, _
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            generarLink = True
        End If

        Me.Cursor = Cursors.WaitCursor
        For Each Linea As ListViewItem In ListView1.SelectedItems
            contador = contador + 1
            ToolStripStatusLabel1.Text = "Copiando " & contador.ToString & " de " & NumSelect.ToString
            Application.DoEvents()
            nCopy = Linea.Tag
            'FicheroCopiaOrigen = ListaDocumentos(nCopy).FicheroJPG

            If TipoExport = "JPG424" Then
                FicheroCopiaOrigen.Add(resultsetGeodocat.resultados(nCopy).rutaFicheroAltaRes())
            ElseIf TipoExport = "JPG250" Then
                FicheroCopiaOrigen.Add(resultsetGeodocat.resultados(nCopy).rutaFicheroBajaRes())
            ElseIf TipoExport = "ECW" Then
                For Each Rutageo As String In resultsetGeodocat.resultados(nCopy).listaFicherosGeo
                    Application.DoEvents()
                    FicheroCopiaOrigen.Add(Rutageo)
                Next
            Else
                Continue For
            End If

            For Each Fichero As String In FicheroCopiaOrigen
                If IsNothing(Fichero) Then
                    Fichero = ""
                End If
                RutaSalida = FolderBrowserDialog1.SelectedPath & "\" & SacarFileDeRuta(Fichero)
                CadenaLOG = resultsetGeodocat.resultados(nCopy).Sellado & ";" & _
                            resultsetGeodocat.resultados(nCopy).Tipo & ";" & _
                            resultsetGeodocat.resultados(nCopy).municipiosHistoLiteral & ";" & _
                            resultsetGeodocat.resultados(nCopy).fechaPrincipal & ";"
                If System.IO.File.Exists(Fichero) = True Then
                    Try
                        System.IO.File.Copy(Fichero, RutaSalida, True)
                        If generarLink = True Then
                            CreateLink(FolderBrowserDialog1.SelectedPath & "\" & SacarFileDeRuta(Fichero) & ".lnk", _
                                            Fichero)
                        End If
                        Copiados = Copiados + 1
                        CadenaLOG = CadenaLOG & "Copiado"
                        sw.WriteLine(CadenaLOG)
                    Catch ex As Exception
                        Me.Cursor = Cursors.Default
                        MessageBox.Show("Se han producido errores en la transferencia de ficheros", _
                                    AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        okProc = False
                        CadenaLOG = CadenaLOG & "Error.Proceso finalizado"
                        sw.WriteLine(CadenaLOG)
                        Exit For
                    End Try
                Else
                    NoCopiados = NoCopiados + 1
                    CadenaLOG = CadenaLOG & "No Copiado"
                    sw.WriteLine(CadenaLOG)
                End If
            Next
        Next
        'Cerramos el documento de log
        sw.Close()
        sw.Dispose()
        Me.Cursor = Cursors.Default
        If okProc = True Then
            If NoCopiados > 0 Then
                MessageBox.Show("Se han transferido " & Copiados.ToString & " documentos." & _
                                System.Environment.NewLine & NoCopiados.ToString & " ficheros no se copiaron.", _
                                AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)

            Else
                MessageBox.Show("Se han transferido " & Copiados.ToString & " documentos.", _
                                AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If



    End Sub


    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click

        'Compruebo que no haya abierta previamente una ventana para modificar este documento
        'Dim nIndiceEdit As Integer = ListaDocumentos(GroupBox1.Tag).Indice
        Dim nIndiceEdit As Integer = resultsetGeodocat.resultados(GroupBox1.Tag).docIndex
        For Each ChildForm As Form In MDIPrincipal.MdiChildren
            If ChildForm.Tag = nIndiceEdit Then
                ChildForm.Focus()
                Exit Sub
            End If
        Next

        Dim FormularioCreacion As New frmEdicion
        FormularioCreacion.MdiParent = MDIPrincipal
        FormularioCreacion.ModoTrabajo("SIMPLE", resultsetGeodocat.resultados(GroupBox1.Tag).docIndex)
        FormularioCreacion.Show()
        FormularioCreacion.Visible = True

    End Sub


    Sub SelectColumnas(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button15.Click

        If GroupBox1.Visible = True Then Exit Sub
        If GroupBox2.Visible = True Then
            GroupBox2.Visible = False
        Else
            GroupBox2.Visible = True
        End If

    End Sub

    Private Sub GuardarEncabezados()

        For iBucle As Integer = 1 To Encabezados.Length - 1
            EscribeIni("QueryFields", "Visible" & iBucle.ToString, IIf(Encabezados(iBucle).Visible = True, "SI", "NO"))
        Next

    End Sub

    Private Sub ActualizarCabeceras(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                    Handles Button17.Click

        If ListView1.Items.Count = 0 Then Exit Sub
        Me.Cursor = Cursors.WaitCursor
        GroupBox2.Visible = False
        resizingElements()
        populateListView()
        'resizingElements()
        'RellenarLisview(ListaDocumentos)
        Me.Cursor = Cursors.Default
        GuardarEncabezados()

    End Sub


    Private Sub ActualizarConsulta(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                    Handles Button16.Click

        If Me.Tag = "Carrito de la Compra" Then
            MessageBox.Show("Función no disponible en el carrito", AplicacionTitulo, _
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Me.Cursor = Cursors.WaitCursor
        LanzarSpinner()
        resultsetGeodocat.dataRefresh()
        resizingElements()
        populateListView()
        Application.DoEvents()
        GroupBox2.Visible = False
        'Erase ListaDocumentos
        'DameDocumentacionSIDCARTO_ByConsulta(qryLoaded, ListaDocumentos)
        'resizingElements()
        'RellenarLisview(ListaDocumentos)
        'GuardarEncabezados()
        ToolStripStatusLabel1.Text = "Resultados : " & ListView1.Items.Count.ToString & " documentos"
        If ListView1.Items.Count = 1 Then
            ListView1.Visible = False
            GroupBox1.Visible = True
            MostrarDetalle(0)
            ListView1.Items(0).Selected = True
            Button1.Enabled = True
            Button2.Enabled = False
            Button8.Enabled = True
        Else
            Button1.Enabled = False
            Button2.Enabled = True
            Button8.Enabled = True
        End If
        CerrarSpinner()
        Me.Cursor = Cursors.Default

    End Sub

    Private Sub CambiarColumnas(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                        Handles CheckBox1.Click, CheckBox2.Click, CheckBox3.Click, CheckBox4.Click, CheckBox5.Click, CheckBox6.Click, _
                        CheckBox7.Click, CheckBox8.Click, CheckBox9.Click, CheckBox10.Click, CheckBox11.Click, CheckBox12.Click, _
                        CheckBox13.Click, CheckBox14.Click, CheckBox15.Click, CheckBox16.Click, CheckBox17.Click, CheckBox18.Click

        If sender.name = "CheckBox1" Then
            Encabezados(1).Visible = IIf(CheckBox1.Checked = True, True, False)
        ElseIf sender.name = "CheckBox2" Then
            Encabezados(2).Visible = IIf(CheckBox2.Checked = True, True, False)
        ElseIf sender.name = "CheckBox3" Then
            Encabezados(3).Visible = IIf(CheckBox3.Checked = True, True, False)
        ElseIf sender.name = "CheckBox4" Then
            Encabezados(4).Visible = IIf(CheckBox4.Checked = True, True, False)
        ElseIf sender.name = "CheckBox5" Then
            Encabezados(5).Visible = IIf(CheckBox5.Checked = True, True, False)
        ElseIf sender.name = "CheckBox6" Then
            Encabezados(6).Visible = IIf(CheckBox6.Checked = True, True, False)
        ElseIf sender.name = "CheckBox7" Then
            Encabezados(7).Visible = IIf(CheckBox7.Checked = True, True, False)
        ElseIf sender.name = "CheckBox8" Then
            Encabezados(8).Visible = IIf(CheckBox8.Checked = True, True, False)
        ElseIf sender.name = "CheckBox9" Then
            Encabezados(9).Visible = IIf(CheckBox9.Checked = True, True, False)
        ElseIf sender.name = "CheckBox10" Then
            Encabezados(10).Visible = IIf(CheckBox10.Checked = True, True, False)
        ElseIf sender.name = "CheckBox11" Then
            Encabezados(11).Visible = IIf(CheckBox11.Checked = True, True, False)

        ElseIf sender.name = "CheckBox12" Then
            Encabezados(12).Visible = IIf(CheckBox12.Checked = True, True, False)
        ElseIf sender.name = "CheckBox13" Then
            Encabezados(13).Visible = IIf(CheckBox13.Checked = True, True, False)
        ElseIf sender.name = "CheckBox14" Then
            Encabezados(14).Visible = IIf(CheckBox14.Checked = True, True, False)
        ElseIf sender.name = "CheckBox15" Then
            Encabezados(15).Visible = IIf(CheckBox15.Checked = True, True, False)
        ElseIf sender.name = "CheckBox16" Then
            Encabezados(16).Visible = IIf(CheckBox16.Checked = True, True, False)
        ElseIf sender.name = "CheckBox17" Then
            Encabezados(17).Visible = IIf(CheckBox17.Checked = True, True, False)
        ElseIf sender.name = "CheckBox18" Then
            Encabezados(18).Visible = IIf(CheckBox18.Checked = True, True, False)
        ElseIf sender.name = "CheckBox19" Then
            Encabezados(19).Visible = IIf(CheckBox19.Checked = True, True, False)


        End If

    End Sub


    Sub ProcGenerarMetadatos(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                    Handles Button11.Click, ToolStripMenuItem4.Click

        Dim NombreFichOUT As String

        If ListView1.Items.Count = 0 Then Exit Sub
        If ListView1.Items.Count = 1 Then ListView1.Items(0).Selected = True
        If ListView1.SelectedItems.Count = 0 Then Exit Sub
        FolderBrowserDialog1.ShowNewFolderButton = True
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
        If System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath) = False Then Exit Sub
        Me.Cursor = Cursors.WaitCursor
        If GroupBox1.Visible = True Then
            GenerarMetadatosNEM(ListaDocumentos(GroupBox1.Tag), FolderBrowserDialog1.SelectedPath)
        Else
            For Each Linea As ListViewItem In ListView1.SelectedItems
                NombreFichOUT = Linea.SubItems(1).Text & ".xml"
                GenerarMetadatosNEM(ListaDocumentos(Linea.Tag), FolderBrowserDialog1.SelectedPath)
            Next
        End If
        Me.Cursor = Cursors.Default
        MessageBox.Show("Metadatos generados en " & System.Environment.NewLine & _
                        FolderBrowserDialog1.SelectedPath, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Sub LanzarECWREcortado(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button19.Click

        Dim RutasDoc() As String
        Dim RutasECW As ArrayList
        Dim idDoc As Integer

        If ListView1.Items.Count = 0 Then Exit Sub
        If ListView1.SelectedItems.Count = 0 Then Exit Sub
        If GroupBox1.Visible = True Then
            'Estamos en modo detalle
            idDoc = CType(GroupBox1.Tag, Integer)
            RutasECW = New ArrayList
            If ListView2.SelectedItems.Count > 0 Then
                'Estamos seleccionando alguno de los ECW del documento
                For Each item As ListViewItem In ListView2.SelectedItems
                    If item.Tag.ToString.ToUpper.EndsWith("ECW") Then RutasECW.Add(item.Tag)
                Next
            Else
                'Si no selecciono ninguno ,supongo que quiero ver todos
                For Each item As ListViewItem In ListView2.Items
                    If item.Tag.ToString.ToUpper.EndsWith("ECW") Then RutasECW.Add(item.Tag)
                Next
            End If
        Else
            RutasECW = New ArrayList
            For Each item As ListViewItem In ListView1.SelectedItems
                'DameRutasFicherosECW(ListaDocumentos(item.Tag), RutasDoc)
                'If DameRutasFicherosECW(ListaDocumentos(item.Tag), RutasDoc) Then
                Application.DoEvents()
                '.getGeoFiles()
                If resultsetGeodocat.resultados(item.Tag).listaFicherosGeo.count = 0 Then resultsetGeodocat.resultados(item.Tag).getGeoFiles()
                For Each rutaDoc As String In resultsetGeodocat.resultados(item.Tag).listaFicherosGeo
                    If rutaDoc.ToUpper.EndsWith("ECW") Then RutasECW.Add(rutaDoc)
                Next
                'End If
            Next
        End If

        If RutasECW.Count = 0 Then
            MessageBox.Show("No se ha localizado ningún documento georreferenciado", _
                       AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            If GenerarProyectoGM(RutasECW, True) = True Then
                LanzarVisorExterno(AppFolderSetting & "\LaunchGM.gmw")
            End If
        End If
        RutasECW.Clear()
        RutasECW = Nothing
    End Sub

    Sub LanzarECW(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                            Handles Button6.Click

        Dim RutasDoc() As String
        Dim RutasECW As ArrayList
        Dim idDoc As Integer

        If ListView1.Items.Count = 0 Then Exit Sub
        If ListView1.SelectedItems.Count = 0 Then Exit Sub
        If GroupBox1.Visible = True Then
            'Estamos en modo detalle
            idDoc = CType(GroupBox1.Tag, Integer)
            RutasECW = New ArrayList
            If ListView2.SelectedItems.Count > 0 Then
                'Estamos seleccionando alguno de los ECW del documento
                For Each item As ListViewItem In ListView2.SelectedItems
                    If item.Tag.ToString.ToUpper.EndsWith("ECW") Then RutasECW.Add(item.Tag)
                Next
            Else
                'Si no selecciono ninguno ,supongo que quiero ver todos
                For Each item As ListViewItem In ListView2.Items
                    If item.Tag.ToString.ToUpper.EndsWith("ECW") Then RutasECW.Add(item.Tag)
                Next
            End If
        Else
            RutasECW = New ArrayList
            For Each item As ListViewItem In ListView1.SelectedItems
                DameRutasFicherosECW(ListaDocumentos(item.Tag), RutasDoc)
                If RutasDoc.Length > 0 Then
                    For Each rutaDoc As String In RutasDoc
                        If rutaDoc.ToUpper.EndsWith("ECW") Then RutasECW.Add(rutaDoc)
                    Next
                End If
            Next
        End If

        If RutasECW.Count = 0 Then
            MessageBox.Show("No se ha localizado ningún documento georreferenciado", _
                       AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf RutasECW.Count = 1 Then
            LanzarVisorExterno(RutasECW.Item(0).ToString)
        Else
            If GenerarProyectoGM(RutasECW, False) = True Then
                LanzarVisorExterno(My.Application.Info.DirectoryPath & "\LaunchGM.gmw")
            End If
        End If
        RutasECW.Clear()
        RutasECW = Nothing

    End Sub

    Sub ExtraerContornos(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim NomFich As String = ""
        Dim contador As Integer

        If ListView1.Items.Count = 0 Then Exit Sub
        If ListView1.Items.Count = 1 Then ListView1.Items(0).Selected = True
        If ListView1.SelectedItems.Count = 0 Then
            MessageBox.Show("Seleccione los documentos que quiere exportar", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        'FolderBrowserDialog1.ShowNewFolderButton = True
        'If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
        'If System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath) = False Then Exit Sub

        With SaveFileDialog1
            .Title = "Introduzca el nombre del fichero para los contornos"
            .Filter = "Archivos XYZ *.xyz|*.xyz"
        End With
        If SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            NomFich = SaveFileDialog1.FileName
        End If
        If NomFich = "" Then Exit Sub

        LanzarSpinner("Procesando información...")
        Me.Cursor = Cursors.WaitCursor
        Dim listaCampos As String = ""
        For iCab As Integer = 1 To Encabezados.Length - 1
            If Encabezados(iCab).Visible = True Then listaCampos = listaCampos & Encabezados(iCab).NombreEncabezado & ","
        Next

        For Each Linea As ListViewItem In ListView1.SelectedItems
            contador = contador + 1
            ToolStripStatusLabel1.Text = "Exportando contorno " & contador.ToString & " de " & ListView1.SelectedItems.Count.ToString
            Application.DoEvents()
            ExportarContornosDocumento2XYZ(ListaDocumentos(Linea.Tag), NomFich, True, listaCampos)
        Next
        'End If
        Me.Cursor = Cursors.Default
        CerrarSpinner()

        MessageBox.Show("Fichero de contornos generado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Sub ExtraerCentroides(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim NomFich As String
        Dim contador As Integer
        If ListView1.Items.Count = 0 Then Exit Sub
        If ListView1.Items.Count = 1 Then ListView1.Items(0).Selected = True
        If ListView1.SelectedItems.Count = 0 Then Exit Sub
        FolderBrowserDialog1.ShowNewFolderButton = True
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
        If System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath) = False Then Exit Sub
        LanzarSpinner("Procesando información...")
        Me.Cursor = Cursors.WaitCursor
        Dim listaCampos As String = ""
        For iCab As Integer = 1 To Encabezados.Length - 1
            If Encabezados(iCab).Visible = True Then listaCampos = listaCampos & Encabezados(iCab).NombreEncabezado & ","
        Next
        If GroupBox1.Visible = True Then
            ExportarCentroidesDocumento2XYZ(ListaDocumentos(GroupBox1.Tag), _
                                            FolderBrowserDialog1.SelectedPath & "\" & ListaDocumentos(GroupBox1.Tag).Sellado & ".xyz", False, listaCampos)
        Else
            NomFich = FolderBrowserDialog1.SelectedPath & "\" & DameAutoNomFichero(".xyz")
            contador = 0
            For Each Linea As ListViewItem In ListView1.SelectedItems
                contador = contador + 1
                ToolStripStatusLabel1.Text = "Calculando centroide " & contador.ToString & " de " & ListView1.SelectedItems.Count.ToString
                Application.DoEvents()
                ExportarCentroidesDocumento2XYZ(ListaDocumentos(Linea.Tag), NomFich, True, listaCampos)
            Next
        End If
        Me.Cursor = Cursors.Default
        CerrarSpinner()
        MessageBox.Show("Fichero de centroides generado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Sub GenerarMiniaturas(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim pathDoc250ppp As String
        Dim pathDocThumb As String
        Dim idDoc As Integer
        pathDoc250ppp = ""
        If GroupBox1.Visible = True Then
            idDoc = CType(GroupBox1.Tag, Integer)
        Else
            If ListView1.Items.Count = 0 Then Exit Sub
            If ListView1.Items.Count = 1 Then
                idDoc = 0
            End If
            If ListView1.SelectedItems.Count > 0 Then
                idDoc = CType(ListView1.SelectedItems(0).Tag, Integer)
            End If
        End If

        pathDoc250ppp = resultsetGeodocat.resultados(idDoc).rutaFicheroBajaRes
        pathDocThumb = pathDoc250ppp.Replace("\_Scan250\", "\_Miniaturas\").Replace("250\", "\")

        If Not System.IO.File.Exists(pathDoc250ppp) Then
            MessageBox.Show("Documento a 250 ppp para generar la miniatura no encontrado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        Else
            MessageBox.Show("La miniatura se generará a partir del documento a 250ppp", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            If System.IO.File.Exists(pathDocThumb) Then
                If MessageBox.Show("La imagen ya existe,¿desea sustituirla?", AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then Exit Sub
            End If
        End If

        If generaThumbImagen(pathDoc250ppp, pathDocThumb) = True Then
            MessageBox.Show("Miniatura generada. Refresque los datos para ver los cambios", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("Problemas al generar la miniatura", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub


    Private Sub ListView1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListView1.MouseUp
        If ListView1.SelectedItems.Count = 0 Then
            ToolStripMenuItem1.Enabled = False
            ToolStripMenuItem2.Enabled = False
        ElseIf ListView1.SelectedItems.Count = 1 Then
            ToolStripMenuItem1.Enabled = False
            ToolStripMenuItem2.Enabled = True
        ElseIf ListView1.SelectedItems.Count > 1 Then
            ToolStripMenuItem1.Enabled = True
            ToolStripMenuItem2.Enabled = False
        End If
        If App_Permiso <> 2 Then
            ToolStripMenuItem1.Enabled = False
            ToolStripMenuItem2.Enabled = False
            mnuOnOffWMS.Enabled = False
            menuTipoWMSRoot.Enabled = False
        End If
        If e.Button = MouseButtons.Right Then
            ContextMenuStrip1.Show()
        End If

    End Sub

    Private Sub ListView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged

        ToolStripStatusLabel2.Text = "Documentos seleccionados " & ListView1.SelectedItems.Count

    End Sub

    Private Sub ListView2_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView2.DoubleClick

        If ListView2.SelectedItems.Count = 0 Then Exit Sub
        Dim RutaFichero As String = ListView2.SelectedItems(0).Tag
        Me.Cursor = Cursors.WaitCursor
        LanzarVisorExterno(RutaFichero)
        Me.Cursor = Cursors.Default

    End Sub

    Private Sub OnMouseDrag(ByVal sender As Object, ByVal m As System.Windows.Forms.ItemDragEventArgs) _
                            Handles ListView2.ItemDrag

        'The strings should explain themselves, they will be
        ' used to recreate full file and folder paths
        Dim strPath, strName, strFullPath As String

        'Determine which ListView is being Dragged From
        'so we can get the correct parent path from the 
        'class variable
        'Dim LView As ListView = sender
        'If LView Is ListView1 Then
        '    strPath = LView1Path
        'ElseIf LView Is ListView2 Then
        '    strPath = LView2Path
        'Else
        '    Exit Sub
        'End If

        'Put the dragged ListView Item or Items into a variable
        Dim items As ListView.SelectedListViewItemCollection = ListView2.SelectedItems

        'Create a string list of File and Folder Paths
        Dim DropList As New System.Collections.Specialized.StringCollection

        ' The "DataObject"...critical to the operation
        Dim DragPaths As New DataObject()

        'Iterate through the dragged (selected) items
        'Since the item is selected and a item is being
        'dragged....which triggered this event, we will
        'assumed all selected items want to be dragged
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


    Private Sub LView_ItemDropped(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) _
                                    Handles ListView2.DragDrop

        'Now add the final routine...the dragdrop event
        'To copy or move a file, we need two Paths: 
        'The current path and destination path.
        'The current path is obtained from the file to be dropped
        'The destination path is obtained from the class
        '   variable set up for the ListViews

        'This extracts the collection of path strings from the
        'DataObject
        Dim Paths As String() = DirectCast(e.Data.GetData(DataFormats.FileDrop), String())

        'To get the destination path, find out which ListView
        'called this Event, then use the appropiate class variable
        Dim strDestination As String
        'Dim LView As ListView = sender
        'If LView Is ListView1 Then
        '    strDestination = LView1Path
        'ElseIf LView Is ListView2 Then
        '    strDestination = LView2Path
        'Else
        '    Exit Sub
        'End If

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


    '--------------------------------------------------------------------------------------------------------------------------
    'Gestión de miniaturas
    '--------------------------------------------------------------------------------------------------------------------------
#Region "Gestión miniaturas"

    Sub LimpiarImagenes()

        PictureBox2.Image = Nothing
        PictureBox3.Image = Nothing
        PictureBox4.Image = Nothing
        PictureBox5.Image = Nothing
        PictureBox6.Image = Nothing
        PictureBox7.Image = Nothing
        PictureBox2.Refresh()
        PictureBox3.Refresh()
        PictureBox4.Refresh()
        PictureBox5.Refresh()
        PictureBox6.Refresh()
        PictureBox7.Refresh()
        PictureBox2.Tag = ""
        PictureBox3.Tag = ""
        PictureBox4.Tag = ""
        PictureBox5.Tag = ""
        PictureBox6.Tag = ""
        PictureBox7.Tag = ""

    End Sub

    Sub CargarImagen(ByVal Lienzo As PictureBox, ByVal RutaImagen As String)

        Dim fs As System.IO.FileStream
        Dim RutaDefault As String = My.Application.Info.DirectoryPath & "\default.png"

        If System.IO.File.Exists(RutaImagen) And ActivarMiniaturas = True Then
            fs = New System.IO.FileStream(RutaImagen, IO.FileMode.Open, IO.FileAccess.Read)
            'fs = New System.IO.FileStream(RutaDefault, IO.FileMode.Open, IO.FileAccess.Read)
            Lienzo.Image = System.Drawing.Image.FromStream(fs)
            Lienzo.SizeMode = PictureBoxSizeMode.StretchImage
            'Lienzo.Image = System.Drawing.Image.FromStream(fs).GetThumbnailImage(Lienzo.Width, Lienzo.Height, Nothing, New IntPtr())
            Lienzo.Tag = RutaImagen
            fs.Close()
            fs = Nothing
        Else
            If Not System.IO.File.Exists(RutaImagen) Then
                fs = New System.IO.FileStream(RutaDefault, IO.FileMode.Open, IO.FileAccess.Read)
                Lienzo.Image = System.Drawing.Image.FromStream(fs)
                Lienzo.SizeMode = PictureBoxSizeMode.StretchImage
                fs.Close()
                fs = Nothing
            End If
            Lienzo.Tag = RutaImagen
        End If

    End Sub


    Private Sub selectMiniatura(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                            Handles PictureBox2.MouseClick, PictureBox3.MouseClick, PictureBox4.MouseClick, PictureBox5.MouseClick, PictureBox6.MouseClick, PictureBox7.MouseClick

        If sender.name = "PictureBox2" Or sender.name = "PictureBox3" Or sender.name = "PictureBox4" Or sender.name = "PictureBox5" Or sender.name = "PictureBox6" Or sender.name = "PictureBox7" Then
            If sender.BorderStyle = BorderStyle.Fixed3D Then
                sender.BorderStyle = BorderStyle.FixedSingle
            Else
                sender.BorderStyle = BorderStyle.Fixed3D
            End If
        End If

    End Sub



#End Region

    '--------------------------------------------------------------------------------------------------------------------------
    'Herramientas para el Carrito de la Compra
    '--------------------------------------------------------------------------------------------------------------------------
#Region "Herramientas para el Carrito de la compra"

    Private Sub ProcesoCarrito(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                        Handles Button7.Click, ToolStripMenuItem3.Click

        MessageBox.Show("En desarrollo", AplicacionTitulo)
        Exit Sub
        If Me.Tag = "Carrito de la Compra" Then
            'Proceso de Borrado
            BorrarElementoFromCarro()
        Else
            If GroupBox3.Visible = True Then
                SumarMiniatura2Carro()
            Else
                SumarElemento2Carro()
            End If
        End If

    End Sub

    Private Sub BorrarElementoFromCarro()

        Dim ElementosBorrar() As Integer
        Dim contador As Integer

        If ListView1.SelectedItems.Count = 0 Then Exit Sub
        If CarritoCompra Is Nothing Then
            MessageBox.Show("El carrito esta vacío", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        If CarritoCompra.Length = 0 Then
            MessageBox.Show("El carrito esta vacío", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        ReDim ElementosBorrar(0 To ListView1.SelectedItems.Count - 1)
        contador = -1
        For Each Linea As ListViewItem In ListView1.SelectedItems
            'array.
            contador = contador + 1
            ElementosBorrar(contador) = ListaDocumentos(Linea.Tag).Indice
        Next
        contador = -1
        For Each docu As docSIDCARTO In ListaDocumentos
            If Array.IndexOf(ElementosBorrar, docu.Indice) = -1 Then
                contador = contador + 1
                CarritoCompra(contador) = docu
            End If
        Next
        Application.DoEvents()
        If contador = -1 Then
            Erase CarritoCompra
            Me.Close()
        Else
            ReDim Preserve CarritoCompra(0 To contador)
            Application.DoEvents()
            MostrarElementosCarrito()
        End If

    End Sub

    Private Sub SumarElemento2Carro()

        Dim ExisteCarrito As Boolean
        Dim ElementosCompra() As docSIDCARTO
        Dim contador As Integer
        Dim dimInicial As Integer

        Dim indexShopping As New ArrayList

        If ListView1.SelectedItems.Count = 0 Then Exit Sub
        ReDim ElementosCompra(ListView1.SelectedItems.Count - 1)
        contador = -1
        For Each item As ListViewItem In ListView1.SelectedItems
            contador = contador + 1
            'ElementosCompra(contador) = ListaDocumentos(Linea.Tag)
            ElementosCompra(contador) = resultsetGeodocat.resultados(item.Tag)
            indexShopping.Add(resultsetGeodocat.resultados(item.Tag).docIndex)
        Next
        If CarritoCompra Is Nothing Then
            'Array.Resize(CarritoCompra, ElementosCompra.Length)
            dimInicial = 0
        Else
            dimInicial = indexShopping.Count
        End If

        ''ReDim Preserve CarritoCompra(dimInicial + ElementosCompra.Length - 1)
        ''Array.ConstrainedCopy(ElementosCompra, 0, CarritoCompra, dimInicial, ElementosCompra.Length)

        'Limpiamos nuestro carrito de la compra de duplicados
        'CarritoCompra = ArrayEliminarDuplicados(CarritoCompra)

        For Each ChildForm As Form In MDIPrincipal.MdiChildren
            If ChildForm.Tag = "Carrito de la Compra" Then
                ChildForm.Focus()
                ExisteCarrito = True
                Exit For
            End If
        Next

        If ExisteCarrito = True Then
            Dim FrmCestaCompra As New frmDocumentacion
            Try
                FrmCestaCompra = MDIPrincipal.ActiveMdiChild
                If FrmCestaCompra Is Nothing Then Exit Sub
            Catch ex As Exception

            End Try
            FrmCestaCompra.MostrarElementosCarrito()
            FrmCestaCompra.Show()
        End If

        MessageBox.Show("Elementos añadidos al carrito de la compra", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub SumarMiniatura2Carro()

        Dim ExisteCarrito As Boolean
        Dim ElementosCompra() As docSIDCARTO
        Dim contador As Integer
        Dim dimInicial As Integer
        Dim miniaturasSelect As Integer = 0

        If PictureBox2.BorderStyle = BorderStyle.Fixed3D And Not Label2.Tag Is Nothing Then miniaturasSelect = miniaturasSelect + 1
        If PictureBox3.BorderStyle = BorderStyle.Fixed3D And Not Label3.Tag Is Nothing Then miniaturasSelect = miniaturasSelect + 1
        If PictureBox4.BorderStyle = BorderStyle.Fixed3D And Not Label8.Tag Is Nothing Then miniaturasSelect = miniaturasSelect + 1
        If PictureBox5.BorderStyle = BorderStyle.Fixed3D And Not Label9.Tag Is Nothing Then miniaturasSelect = miniaturasSelect + 1
        If PictureBox6.BorderStyle = BorderStyle.Fixed3D And Not Label39.Tag Is Nothing Then miniaturasSelect = miniaturasSelect + 1
        If PictureBox7.BorderStyle = BorderStyle.Fixed3D And Not Label40.Tag Is Nothing Then miniaturasSelect = miniaturasSelect + 1

        If miniaturasSelect = 0 Then Exit Sub
        ReDim ElementosCompra(miniaturasSelect - 1)
        contador = -1
        If PictureBox2.BorderStyle = BorderStyle.Fixed3D And Not Label2.Tag Is Nothing Then contador = contador + 1 : ElementosCompra(contador) = ListaDocumentos(Label2.Tag)
        If PictureBox3.BorderStyle = BorderStyle.Fixed3D And Not Label3.Tag Is Nothing Then contador = contador + 1 : ElementosCompra(contador) = ListaDocumentos(Label3.Tag)
        If PictureBox4.BorderStyle = BorderStyle.Fixed3D And Not Label8.Tag Is Nothing Then contador = contador + 1 : ElementosCompra(contador) = ListaDocumentos(Label8.Tag)
        If PictureBox5.BorderStyle = BorderStyle.Fixed3D And Not Label9.Tag Is Nothing Then contador = contador + 1 : ElementosCompra(contador) = ListaDocumentos(Label9.Tag)
        If PictureBox6.BorderStyle = BorderStyle.Fixed3D And Not Label39.Tag Is Nothing Then contador = contador + 1 : ElementosCompra(contador) = ListaDocumentos(Label39.Tag)
        If PictureBox7.BorderStyle = BorderStyle.Fixed3D And Not Label40.Tag Is Nothing Then contador = contador + 1 : ElementosCompra(contador) = ListaDocumentos(Label40.Tag)
        If CarritoCompra Is Nothing Then
            Array.Resize(CarritoCompra, ElementosCompra.Length)
            dimInicial = 0
        Else
            dimInicial = CarritoCompra.Length
        End If

        ReDim Preserve CarritoCompra(dimInicial + ElementosCompra.Length - 1)
        Array.ConstrainedCopy(ElementosCompra, 0, CarritoCompra, dimInicial, ElementosCompra.Length)

        'Limpiamos nuestro carrito de la compra de duplicados
        CarritoCompra = ArrayEliminarDuplicados(CarritoCompra)

        For Each ChildForm As Form In MDIPrincipal.MdiChildren
            If ChildForm.Tag = "Carrito de la Compra" Then
                ChildForm.Focus()
                ExisteCarrito = True
                Exit For
            End If
        Next

        If ExisteCarrito = True Then
            Dim FrmCestaCompra As New frmDocumentacion
            Try
                FrmCestaCompra = MDIPrincipal.ActiveMdiChild
                If FrmCestaCompra Is Nothing Then Exit Sub
            Catch ex As Exception

            End Try
            FrmCestaCompra.MostrarElementosCarrito()
            FrmCestaCompra.Show()
        End If

        MessageBox.Show("Elementos añadidos al carrito de la compra", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub


    Sub MostrarElementosCarrito()

        Dim iBucle As Integer
        Dim iBucleTMP As Integer
        Dim TmpDoc() As docSIDCARTO
        Dim SumaSuperficie As Integer
        ReDim TmpDoc(CarritoCompra.GetUpperBound(0))
        ListView1.Columns.Clear()
        ListView1.Items.Clear()
        resizingElements()
        iBucle = -1
        iBucleTMP = -1
        SumaSuperficie = 0
        Array.Resize(ListaDocumentos, CarritoCompra.GetUpperBound(0))
        ListaDocumentos = CarritoCompra
        RellenarLisview(ListaDocumentos)

    End Sub

#End Region

    Private Sub GestionDetailContextMenu(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuDetailMostrarWMS.Click, mnuDetailOcultarWMS.Click, _
        mnuDetailTipoWMS0.Click, mnuDetailTipoWMS1.Click, mnuDetailTipoWMS2.Click, mnuDetailTipoWMS3.Click, mnuDetailTipoWMS4.Click, mnuSetZIndex.Click

        If lvAtributos.SelectedItems.Count <> 1 Then
            MessageBox.Show("Seleccione un único fichero del apartado de Georreferenciación", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        If lvAtributos.SelectedItems(0).Group.ToString <> "Georreferenciación" Then
            MessageBox.Show("Seleccione un único fichero del apartado de Georreferenciación", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Dim listaSQLs As ArrayList
        Dim typeWMS As String = ""
        If sender.name = "mnuSetZIndex" Then
            Dim respZindex As String = InputBox("Introduce un valor para el Z-Index (1-1000)", AplicacionTitulo)
            If respZindex = "" Then Exit Sub
            Try
                listaSQLs = New ArrayList
                If Not IsNumeric(respZindex) Then
                    MessageBox.Show("El valor no es correcto", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End If

                listaSQLs.Add("UPDATE bdsidschema.contornos set zindex=" & respZindex & " WHERE idcontorno=" & lvAtributos.SelectedItems(0).Tag)
                If ExeTran(listaSQLs) Then
                    MessageBox.Show("Cambios realizados", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    resultsetGeodocat.resultados(GroupBox1.Tag).getGeoFilesFromDatabase()
                    MostrarDetalle(GroupBox1.Tag)
                End If

            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Finally
                listaSQLs.Clear()
                listaSQLs = Nothing
            End Try

        ElseIf sender.name = "mnuDetailOcultarWMS" Or sender.name = "mnuDetailMostrarWMS" Then
            Try
                listaSQLs = New ArrayList
                listaSQLs.Add("UPDATE bdsidschema.contornos set mostrarwms=" & IIf(sender.name = "mnuDetailOcultarWMS", "0", "1") & " WHERE idcontorno=" & lvAtributos.SelectedItems(0).Tag)
                If ExeTran(listaSQLs) Then
                    MessageBox.Show("Cambios realizados", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    resultsetGeodocat.resultados(GroupBox1.Tag).getGeoFilesFromDatabase()
                    MostrarDetalle(GroupBox1.Tag)
                End If

            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Finally
                listaSQLs.Clear()
                listaSQLs = Nothing
            End Try
        ElseIf sender.name = "mnuDetailTipoWMS0" Or sender.name = "mnuDetailTipoWMS1" Or sender.name = "mnuDetailTipoWMS2" Or sender.name = "mnuDetailTipoWMS3" Or sender.name = "mnuDetailTipoWMS4" Then

            listaSQLs = New ArrayList
            If sender.name = "mnuDetailTipoWMS0" Then typeWMS = "No asignado"
            If sender.name = "mnuDetailTipoWMS1" Then typeWMS = "Planimetrías"
            If sender.name = "mnuDetailTipoWMS2" Then typeWMS = "Altimetrías"
            If sender.name = "mnuDetailTipoWMS3" Then typeWMS = "Hojas kilométricas"
            If sender.name = "mnuDetailTipoWMS4" Then typeWMS = "Mixto Alti & Plani"
            Try
                listaSQLs = New ArrayList
                listaSQLs.Add("UPDATE bdsidschema.contornos set tipowms='" & typeWMS & "' WHERE idcontorno=" & lvAtributos.SelectedItems(0).Tag)
                If ExeTran(listaSQLs) Then
                    MessageBox.Show("Cambios realizados", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    resultsetGeodocat.resultados(GroupBox1.Tag).getGeoFilesFromDatabase()
                    MostrarDetalle(GroupBox1.Tag)
                    resizingElements()
                    populateListView()
                End If

            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Finally
                listaSQLs.Clear()
                listaSQLs = Nothing
            End Try
        Else
            MessageBox.Show("Desarrollando")
        End If

    End Sub

    Private Sub GestionContextMenu(ByVal sender As Object, ByVal e As System.EventArgs) _
                Handles ToolStripMenuItem1.Click, ToolStripMenuItem2.Click, mnuOffWMS.Click, mnuOnWMS.Click, menuTipoWMS0.Click, menuTipoWMS1.Click, menuTipoWMS2.Click, menuTipoWMS3.Click, menuTipoWMS4.Click, _
                mnuSetZIndexBulk.Click

        Dim listaSQLs As ArrayList
        Dim typeWMS As String = ""
        If sender.name = "ToolStripMenuItem1" Then
            'Dim nIndiceEdit As Integer = ListaDocumentos(GroupBox1.Tag).Indice
            For Each ChildForm As Form In MDIPrincipal.MdiChildren
                If ChildForm.Tag = "Carrito de la Compra" Then Continue For
                If ChildForm.Tag = -1 Then
                    ChildForm.Focus()
                    MessageBox.Show("No pueden abrirse varios procesos de edición de lote simultáneamente", _
                                    AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
            Next
            'Editar en lote
            If ListView1.SelectedItems.Count = 0 Then Exit Sub
            If ListView1.SelectedItems.Count > 50 Then
                MessageBox.Show("No se pueden editar en lote más de 50 elementos a la vez", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            Dim conta As Integer = -1
            'Erase docListaLote
            'ReDim docListaLote(ListView1.SelectedItems.Count - 1)
            Dim FormularioCreacion As New frmEdicion
            FormularioCreacion.MdiParent = MDIPrincipal
            For Each item As ListViewItem In ListView1.SelectedItems
                conta = conta + 1
                FormularioCreacion.ElemEdit.Add(resultsetGeodocat.resultados(item.Tag).docindex)
            Next
            FormularioCreacion.ModoTrabajo("LOTE", -1)
            FormularioCreacion.Show()
            FormularioCreacion.Visible = True
        ElseIf sender.name = "ToolStripMenuItem2" Then
            'Editar un elemento
            If ListView1.SelectedItems.Count <> 1 Then Exit Sub
            Dim FormularioCreacion As New frmEdicion
            FormularioCreacion.MdiParent = MDIPrincipal
            For Each item As ListViewItem In ListView1.SelectedItems
                'FormularioCreacion.ElemEdit.Add(ListaDocumentos(item.Tag))
                'FormularioCreacion.ElemEdit.Add(resultsetGeodocat.resultados(item.Tag))
                FormularioCreacion.ModoTrabajo("SIMPLE", resultsetGeodocat.resultados(item.Tag).docIndex)
                Exit For
            Next
            FormularioCreacion.Show()
            FormularioCreacion.Visible = True
        ElseIf sender.name = "mnuOffWMS" Or sender.name = "mnuOnWMS" Then
            listaSQLs = New ArrayList
            Try
                For Each item As ListViewItem In ListView1.SelectedItems
                    If resultsetGeodocat.resultados(item.Tag).getInfoWMS() = "Georreferenciación no disponible" Then Continue For
                    listaSQLs.Add("UPDATE bdsidschema.contornos set mostrarwms=" & IIf(sender.name = "mnuOffWMS", "0", "1") & " WHERE archivo_id=" & resultsetGeodocat.resultados(item.Tag).docIndex)
                    For Each fila As DataRow In resultsetGeodocat.resultados(item.Tag).rcdgeoFiles.select()
                        fila.Item("mostrarwms") = IIf(sender.name = "mnuOffWMS", 0, 1)
                    Next
                    item.SubItems(item.SubItems.Count - 2).Text = resultsetGeodocat.resultados(item.Tag).getInfoWMS()
                Next
                If listaSQLs.Count > 0 Then
                    If ExeTran(listaSQLs) Then MessageBox.Show("Cambios realizados", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Finally
                listaSQLs.Clear()
                listaSQLs = Nothing
            End Try

        ElseIf sender.name = "mnuSetZIndexBulk" Then

            Dim respZindex As String = InputBox("Introduce un valor para el Z-Index (1-1000)", AplicacionTitulo)
            If respZindex = "" Then Exit Sub
            Try
                listaSQLs = New ArrayList
                If Not IsNumeric(respZindex) Then
                    MessageBox.Show("El valor no es correcto", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End If
                For Each item As ListViewItem In ListView1.SelectedItems
                    If resultsetGeodocat.resultados(item.Tag).getInfoWMS() = "Georreferenciación no disponible" Then Continue For
                    listaSQLs.Add("UPDATE bdsidschema.contornos set zindex=" & respZindex & " WHERE archivo_id=" & resultsetGeodocat.resultados(item.Tag).docIndex)
                    For Each fila As DataRow In resultsetGeodocat.resultados(item.Tag).rcdgeoFiles.select()
                        fila.Item("zindex") = respZindex
                    Next
                    item.SubItems(item.SubItems.Count - 1).Text = resultsetGeodocat.resultados(item.Tag).getZIndex()
                Next
                If listaSQLs.Count > 0 Then
                    If ExeTran(listaSQLs) Then MessageBox.Show("Cambios realizados", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Finally
                listaSQLs.Clear()
                listaSQLs = Nothing
            End Try


        ElseIf sender.name = "menuTipoWMS0" Or sender.name = "menuTipoWMS1" Or sender.name = "menuTipoWMS2" Or sender.name = "menuTipoWMS3" Or sender.name = "menuTipoWMS4" Then

            listaSQLs = New ArrayList
            If sender.name = "menuTipoWMS0" Then typeWMS = "No asignado"
            If sender.name = "menuTipoWMS1" Then typeWMS = "Planimetrías"
            If sender.name = "menuTipoWMS2" Then typeWMS = "Altimetrías"
            If sender.name = "menuTipoWMS3" Then typeWMS = "Hojas kilométricas"
            If sender.name = "menuTipoWMS4" Then typeWMS = "Mixto Alti & Plani"
            Try
                For Each item As ListViewItem In ListView1.SelectedItems
                    If resultsetGeodocat.resultados(item.Tag).getInfoWMS() = "Georreferenciación no disponible" Then Continue For
                    listaSQLs.Add("UPDATE bdsidschema.contornos set tipowms='" & typeWMS & "' WHERE archivo_id=" & resultsetGeodocat.resultados(item.Tag).docIndex)
                    For Each fila As DataRow In resultsetGeodocat.resultados(item.Tag).rcdgeoFiles.select()
                        fila.Item("tipowms") = typeWMS
                    Next
                    item.SubItems(item.SubItems.Count - 2).Text = resultsetGeodocat.resultados(item.Tag).getInfoWMS()
                Next
                If listaSQLs.Count > 0 Then
                    If ExeTran(listaSQLs) Then MessageBox.Show("Cambios realizados", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Finally
                listaSQLs.Clear()
                listaSQLs = Nothing
            End Try
        Else
            MessageBox.Show("Desarrollando")
        End If

    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click

        If ListView2.SelectedItems.Count = 0 Then
            MessageBox.Show("Selecciona uno de los ficheros raster para localizar su carpeta", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Me.Cursor = Cursors.WaitCursor
        Try
            Process.Start(SacarDirDeRuta(ListView2.SelectedItems(0).Tag))
        Catch ex As Exception
            MessageBox.Show("Error:" & ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
        Me.Cursor = Cursors.Default

    End Sub

    Private Sub Button20_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button20.Click
        With SaveFileDialog1
            .Title = "Introduzca el nombre del fichero SQL"
            .Filter = "Archivos SQL *.sql|*.Sql"
            .ShowDialog()
        End With
        If SaveFileDialog1.FileName = "" Then Exit Sub

        Dim sw As New System.IO.StreamWriter(SaveFileDialog1.FileName, False, System.Text.Encoding.Unicode)

        sw.WriteLine(qryLoaded)

        sw.Close()
        sw.Dispose()

        MessageBox.Show("SQL sentence guardada", AplicacionTitulo, _
                        MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub


    Private Sub lvAtributos_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvAtributos.Resize
        If lvAtributos.Columns.Count = 2 Then
            lvAtributos.Columns(0).Width = 120
            lvAtributos.Columns(1).Width = lvAtributos.Width - 120 - 25
        End If
    End Sub

    Private Sub Button22_Click(sender As System.Object, e As System.EventArgs)
        SumarMiniatura2Carro()
    End Sub

    Private Sub Label3_Click(sender As System.Object, e As System.EventArgs) Handles Label2.Click, Label3.Click, Label8.Click, Label9.Click, Label39.Click, Label40.Click

        If sender.name = "Label2" And Not Label2.Tag Is Nothing Then MostrarDetalle(Label2.Tag)
        If sender.name = "Label3" And Not Label3.Tag Is Nothing Then MostrarDetalle(Label3.Tag)
        If sender.name = "Label8" And Not Label8.Tag Is Nothing Then MostrarDetalle(Label8.Tag)
        If sender.name = "Label9" And Not Label9.Tag Is Nothing Then MostrarDetalle(Label9.Tag)
        If sender.name = "Label39" And Not Label39.Tag Is Nothing Then MostrarDetalle(Label39.Tag)
        If sender.name = "Label40" And Not Label40.Tag Is Nothing Then MostrarDetalle(Label40.Tag)
        Button1.Enabled = True
        Button2.Enabled = False
        Button8.Enabled = True
        ToolStripStatusLabel1.Text = "Vista en detalle"
    End Sub

    Private Sub Button22_Click_1(sender As System.Object, e As System.EventArgs) Handles Button22.Click

        Dim nTomo As String = ListaDocumentos(GroupBox1.Tag).Tomo
        Dim cProv As Integer = ListaDocumentos(GroupBox1.Tag).MunicipiosINE.Substring(0, 2)
        Dim fileimagen As String
        Dim folderOutput As String

        If MessageBox.Show("¿Desea copiar los documentos digitalizados encontrados a un directorio?", AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            FolderBrowserDialog1.ShowNewFolderButton = True
            If FolderBrowserDialog1.ShowDialog = Windows.Forms.DialogResult.Cancel Then Exit Sub
            folderOutput = FolderBrowserDialog1.SelectedPath
        End If



        If (cProv >= 1 And cProv <= 50) And nTomo <> "" Then
            Dim rcdInventario As New DataTable
            Dim filasInvent() As DataRow
            Dim contImagen As Integer = 0

            nTomo = nTomo.ToLower
            If nTomo.StartsWith("00") Then nTomo = nTomo.Substring(2, nTomo.Length - 2)
            If nTomo.StartsWith("0") Then nTomo = nTomo.Substring(1, nTomo.Length - 1)
            nTomo = nTomo.Replace("tris", "ter")

            Try
                If CargarRecordset("SELECT * from inventario where codprov=" & cProv & " AND lower(tomo)='" & nTomo.ToLower & "'", rcdInventario) Then
                    If rcdInventario.Rows.Count = 0 Then
                        MessageBox.Show("No hay información del tomo " & nTomo & " de la provincia " & DameProvinciaByINE(cProv), AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Else

                        filasInvent = rcdInventario.Select
                        For Each filaInvent As DataRow In filasInvent
                            Application.DoEvents()
                            fileimagen = rutaRepoInventarioInfo & "\" & filaInvent("tipo").ToString & "\" & String.Format("{0:00}", cProv) & "\" & filaInvent("fichero").ToString
                            If System.IO.File.Exists(fileimagen) Then
                                Process.Start(fileimagen)
                                contImagen = contImagen + 1
                                If folderOutput <> "" Then
                                    System.IO.File.Copy(fileimagen, folderOutput & "\" & String.Format("{0:00}", cProv) & "-" & filaInvent("fichero").ToString)
                                End If
                            End If
                        Next
                        MessageBox.Show("Se encontraron " & contImagen & " documentos", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If
                End If

            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Finally
                Erase filasInvent
                rcdInventario.Dispose()
                rcdInventario = Nothing
            End Try


        End If
    End Sub

End Class