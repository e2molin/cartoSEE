Public Class frmInformes

    Dim reportData As DataTable
    Dim cadSQL As String
    Dim filas() As DataRow
    Dim elementoLV As ListViewItem
    Dim FontPrint As Font
    Dim TableFont As Font
    Dim TituloInforme As String
    Dim Cancelar As Boolean
    Dim Y As Integer
    Dim itm As Integer
    Public fecha1 As String
    Public fecha2 As String


    Private Sub frmInformes_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Size = New Point(800, 600)
        ListView1.Location = New Point(15, 80)
        ListView1.Size = New Point(Me.Width - 30, Me.Height - 150)
        ToolStripStatusLabel1.Text = ""
        ToolStripStatusLabel2.Text = ""
        ListView1.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Bottom Or AnchorStyles.Right
        ListView1.Visible = True

    End Sub


    Sub Informe_Resumen_PorTipoDoc()

        Dim NProvincias As Integer = 0
        Dim NDocumentos As Integer = 0
        Dim iBucle As Integer
        Dim iBucle2 As Integer
        Dim NTipos As Integer = 0
        Dim CabeceroProv As String

        'Preparo el LV para mostrar los resultados
        ListView1.FullRowSelect = True
        ListView1.GridLines = False
        ListView1.View = View.Details
        ListView1.SmallImageList = MDIPrincipal.ImageList2
        ListView1.Columns.Clear()
        ListView1.Items.Clear()
        ListView1.Columns.Add("Provincia", "Provincia", 150, HorizontalAlignment.Left, 0)

        Dim TiposDoc() As DataRow
        TiposDoc = ListaTiposDocumento.Select()
        For Each fila As DataRow In TiposDoc
            NTipos = NTipos + 1
            ListView1.Columns.Add(fila.Item(1).ToString, 100, HorizontalAlignment.Right)
        Next
        ListView1.Columns.Add("TOTAL", 100, HorizontalAlignment.Right)
        ListView1.Tag = 1

        If fecha1 = "" Or fecha2 = "" Then
            cadSQL = "SELECT provincia_id,tipodoc_id,count(*) AS Numero FROM bdsidschema.archivo GROUP BY provincia_id,tipodoc_id ORDER BY provincia_id,tipodoc_id"
        Else
            cadSQL = "SELECT provincia_id,tipodoc_id,count(*) AS Numero FROM bdsidschema.archivo WHERE fechacreacion between '" & fecha1 & "' AND '" & fecha2 & "' " &
                    "GROUP BY provincia_id,tipodoc_id ORDER BY provincia_id,tipodoc_id"
        End If

        reportData = New DataTable
        If CargarRecordset(cadSQL, reportData) = True Then
            filas = reportData.Select
            If filas.Length > 0 Then
                Cancelar = False
                CabeceroProv = ""
                For Each registro As DataRow In filas
                    If CabeceroProv <> DameProvinciaByINE(registro("provincia_id").ToString) Then
                        iBucle = iBucle + 1
                        NProvincias = NProvincias + 1
                        If CabeceroProv <> "" Then
                            elementoLV.Tag = "0"
                            ListView1.Items.Add(elementoLV)
                            elementoLV = Nothing
                        End If
                        CabeceroProv = DameProvinciaByINE(registro("provincia_id").ToString)
                        elementoLV = New ListViewItem
                        elementoLV.Text = CabeceroProv
                        For iBucle2 = 1 To NTipos + 1
                            elementoLV.SubItems.Add("0")
                        Next
                        iBucle2 = 0
                        For Each fila As DataRow In TiposDoc
                            iBucle2 = iBucle2 + 1
                            If fila.Item(0).ToString = registro("tipodoc_id").ToString Then
                                elementoLV.SubItems(iBucle2).Text = registro("Numero").ToString
                                NDocumentos = NDocumentos + registro("Numero").ToString
                            End If
                        Next

                        Continue For
                    Else
                        iBucle2 = 0
                        For Each fila As DataRow In TiposDoc
                            iBucle2 = iBucle2 + 1
                            If fila.Item(0).ToString = registro("tipodoc_id").ToString Then
                                elementoLV.SubItems(iBucle2).Text = registro("Numero").ToString
                                NDocumentos = NDocumentos + registro("Numero").ToString
                            End If
                        Next
                    End If
                Next
                If CabeceroProv <> "" Then
                    elementoLV.Tag = "0"
                    ListView1.Items.Add(elementoLV)
                    elementoLV = Nothing
                End If


            End If
        End If
        reportData.Dispose()
        reportData = Nothing
        TiposDoc = Nothing

        Dim contProv As Integer
        Dim contTipo As Integer

        For Each elem As ListViewItem In ListView1.Items
            contProv = 0
            For iBucle = 1 To ListView1.Columns.Count - 2
                contProv = contProv + elem.SubItems(iBucle).Text
            Next
            Application.DoEvents()
            elem.SubItems(ListView1.Columns.Count - 1).Text = contProv
        Next

        elementoLV = New ListViewItem
        elementoLV.Text = "--- TOTALES ---"
        For iBucle2 = 1 To NTipos + 1
            elementoLV.SubItems.Add("0")
        Next
        elementoLV.Tag = "0"
        ListView1.Items.Add(elementoLV)
        elementoLV = Nothing


        For iBucle = 1 To ListView1.Columns.Count - 1
            contTipo = 0
            For Each elem As ListViewItem In ListView1.Items
                contTipo = contTipo + elem.SubItems(iBucle).Text
            Next
            ListView1.Items(ListView1.Items.Count - 1).SubItems(iBucle).Text = contTipo
        Next

        iBucle = 0
        For Each elem As ListViewItem In ListView1.Items
            iBucle = iBucle + 1
            If iBucle Mod 2 = 0 Then
                elem.BackColor = Color.White
            Else
                elem.BackColor = Color.WhiteSmoke
            End If
        Next

        ToolStripStatusLabel1.Text = "Nº total de documentos " & NDocumentos.ToString
        ToolStripStatusLabel2.Text = "Nº de Provincias " & NProvincias.ToString

    End Sub

    Sub Informe_Resumen_PorEstadoDoc()

        Dim NProvincias As Integer = 0
        Dim NDocumentos As Integer = 0
        Dim iBucle As Integer
        Dim iBucle2 As Integer
        Dim NTipos As Integer = 0
        Dim TiposDoc() As DataRow
        Dim CabeceroProv As String

        'Preparo el LV para mostrar los resultados
        ListView1.FullRowSelect = True
        ListView1.GridLines = False
        ListView1.View = View.Details
        ListView1.SmallImageList = MDIPrincipal.ImageList2
        ListView1.Columns.Clear()
        ListView1.Items.Clear()
        ListView1.Columns.Add("Provincia", "Provincia", 150, HorizontalAlignment.Left, 0)

        reportData = New DataTable
        If CargarRecordset("SELECT idestadodoc,estadodoc from bdsidschema.tbestadodocumento", reportData) = True Then
            TiposDoc = reportData.Select
            For Each fila As DataRow In TiposDoc
                NTipos = NTipos + 1
                ListView1.Columns.Add(fila.Item(1).ToString, 100, HorizontalAlignment.Right)
            Next
        End If

        ListView1.Tag = 2
        cadSQL = "SELECT provincia_id,estadodoc_id,count(*) AS Numero FROM bdsidschema.archivo GROUP BY provincia_id,estadodoc_id ORDER BY provincia_id,estadodoc_id"

        reportData = New DataTable
        If CargarRecordset(cadSQL, reportData) = True Then
            filas = reportData.Select
            If filas.Length > 0 Then
                Cancelar = False
                CabeceroProv = ""
                For Each registro As DataRow In filas
                    If CabeceroProv <> DameProvinciaByINE(registro("provincia_id").ToString) Then
                        iBucle = iBucle + 1
                        NProvincias = NProvincias + 1
                        If CabeceroProv <> "" Then
                            If iBucle Mod 2 = 0 Then
                                elementoLV.BackColor = Color.White
                            Else
                                elementoLV.BackColor = Color.WhiteSmoke
                            End If
                            elementoLV.Tag = "0"
                            ListView1.Items.Add(elementoLV)
                            elementoLV = Nothing
                        End If
                        CabeceroProv = DameProvinciaByINE(registro("provincia_id").ToString)
                        elementoLV = New ListViewItem
                        elementoLV.Text = CabeceroProv
                        For iBucle2 = 1 To NTipos
                            elementoLV.SubItems.Add("0")
                        Next
                        iBucle2 = 0
                        For Each fila As DataRow In TiposDoc
                            iBucle2 = iBucle2 + 1
                            If fila.Item(0).ToString = registro("estadodoc_id").ToString Then
                                elementoLV.SubItems(iBucle2).Text = registro("Numero").ToString
                                NDocumentos = NDocumentos + registro("Numero").ToString
                            End If
                        Next
                        Continue For
                    Else
                        iBucle2 = 0
                        For Each fila As DataRow In TiposDoc
                            iBucle2 = iBucle2 + 1
                            If fila.Item(0).ToString = registro("estadodoc_id").ToString Then
                                elementoLV.SubItems(iBucle2).Text = registro("Numero").ToString
                                NDocumentos = NDocumentos + registro("Numero").ToString
                            End If
                        Next
                    End If
                Next
            End If
        End If
        reportData.Dispose()
        reportData = Nothing
        TiposDoc = Nothing

        ToolStripStatusLabel1.Text = "Nº total de documentos " & NDocumentos.ToString
        ToolStripStatusLabel2.Text = "Nº de Provincias " & NProvincias.ToString



    End Sub

    Sub Informe_UltimoDocumentoSellado()

        'Preparo el LV para mostrar los resultados
        ListView1.FullRowSelect = True
        ListView1.GridLines = False
        ListView1.View = View.Details
        ListView1.SmallImageList = MDIPrincipal.ImageList2
        ListView1.Columns.Clear()
        ListView1.Items.Clear()
        ListView1.Columns.Add("Provincia", "Provincia", 150, HorizontalAlignment.Left, 0)
        ListView1.Columns.Add("Sellado", "Último nº asignado", 250, HorizontalAlignment.Right, 0)

        ListView1.Tag = 3
        cadSQL = "SELECT provincia_id,max(numdoc) as ultimosello FROM bdsidschema.archivo GROUP BY provincia_id order by provincia_id"

        reportData = New DataTable
        If CargarRecordset(cadSQL, reportData) = True Then
            filas = reportData.Select
            If filas.Length > 0 Then
                Cancelar = False
                For Each registro As DataRow In filas
                    elementoLV = New ListViewItem
                    elementoLV.Text = DameProvinciaByINE(registro("provincia_id").ToString)
                    elementoLV.SubItems.Add(registro("ultimosello").ToString)
                    If ListView1.Items.Count Mod 2 = 0 Then
                        elementoLV.BackColor = Color.White
                    Else
                        elementoLV.BackColor = Color.WhiteSmoke
                    End If
                    ListView1.Items.Add(elementoLV)
                    elementoLV = Nothing
                Next
            End If
        End If
        reportData.Dispose()
        reportData = Nothing

        ToolStripStatusLabel1.Text = "Nº total de Provincias " & ListView1.Items.Count.ToString
        ToolStripStatusLabel2.Text = ""



    End Sub

    Sub ListarPPCnoGeo()

        Dim resultsetGeodocat As docCartoSEEquery
        'Preparo el LV para mostrar los resultados
        ListView1.FullRowSelect = True
        ListView1.GridLines = False
        ListView1.View = View.Details
        ListView1.SmallImageList = MDIPrincipal.ImageList2
        ListView1.Columns.Clear()
        ListView1.Items.Clear()
        ListView1.Columns.Add("Provincia", "Provincia", 150, HorizontalAlignment.Left, 0)
        ListView1.Columns.Add("Sellado", "Sellado", 150, HorizontalAlignment.Right, 0)

        ListView1.Tag = 3
        cadSQL = "SELECT provincia_id,max(numdoc) as ultimosello FROM bdsidschema.archivo GROUP BY provincia_id order by provincia_id"


        resultsetGeodocat = New docCartoSEEquery
        resultsetGeodocat.flag_CargarFicherosGEO = True
        resultsetGeodocat.getDocsSIDDAE_ByFiltroSQL("archivo.tipodoc_id=8")

        Application.DoEvents()

        For Each doc As docCartoSEE In resultsetGeodocat.resultados
            Application.DoEvents()
            If doc.listaFicherosGeo23030.Count = 0 Then
                elementoLV = New ListViewItem
                elementoLV.Text = doc.Provincias
                elementoLV.SubItems.Add(doc.Sellado)
                If ListView1.Items.Count Mod 2 = 0 Then
                    elementoLV.BackColor = Color.White
                Else
                    elementoLV.BackColor = Color.WhiteSmoke
                End If
                ListView1.Items.Add(elementoLV)
                elementoLV = Nothing
            End If
        Next


        ToolStripStatusLabel1.Text = "Nº total de documentos " & ListView1.Items.Count.ToString
        ToolStripStatusLabel2.Text = ""

    End Sub


    Private Sub ListView1_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles ListView1.ColumnClick
        '
        ' ========================================
        ' Usando la clase ListViewColumnSortSimple
        ' ========================================
        '
        ' Crear una instancia de la clase que realizará la comparación
        Dim oCompare As New ListViewColumnSortSimple()
        '
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

        LineaOUT = ""
        For iBucle = 0 To ListView1.Columns.Count - 1
            LineaOUT = LineaOUT & ListView1.Columns(iBucle).Text & ";"
        Next
        sw.WriteLine(LineaOUT)

        For Each Linea As ListViewItem In ListView1.Items
            CeldaOUT = TratarCadenaExportarCSV(Linea.Text)
            LineaOUT = CeldaOUT
            For iBucle = 1 To Linea.SubItems.Count - 1
                LineaOUT = LineaOUT & ";" & TratarCadenaExportarCSV(Linea.SubItems(iBucle).Text)
            Next
            sw.WriteLine(LineaOUT)
        Next
        sw.Close()
        sw.Dispose()

        MessageBox.Show("Fichero de exportación generado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click

        If ListView1.Items.Count = 0 Then Exit Sub
        Dim pageWidth As Integer
        Dim newMargins As System.Drawing.Printing.Margins
        newMargins = New System.Drawing.Printing.Margins(0, 0, 0, 0)

        PrintDocument1.DefaultPageSettings.Margins = newMargins
        PrintDocument1.DefaultPageSettings.Landscape = True
        PageSetupDialog1.PageSettings = PrintDocument1.DefaultPageSettings
        PageSetupDialog1.AllowOrientation = True
        If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            PrintDocument1.DefaultPageSettings = PageSetupDialog1.PageSettings
        End If
        'TableFont = New Font("Arial", 8)
        'FontPrint = New Font("Arial", 10, FontStyle.Bold)

        With PrintDocument1.DefaultPageSettings
            pageWidth = .PaperSize.Width - .Margins.Left - .Margins.Right
        End With
        PrintPreviewDialog1.Document = PrintDocument1
        PrintPreviewDialog1.WindowState = FormWindowState.Maximized
        Application.DoEvents()
        PrintPreviewDialog1.ShowDialog()

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

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        If ListView1.Tag = 1 Then
            Informe_Resumen_PorTipoDoc()
        ElseIf ListView1.Tag = 2 Then
            Informe_Resumen_PorEstadoDoc()
        ElseIf ListView1.Tag = 3 Then
            Informe_UltimoDocumentoSellado()
        Else
            MessageBox.Show("Para actualizar los datos, seleccione el informe desde el menú", _
                                        AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
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
                            New Font("Arial", 18), Brushes.Red, TabINI, Y)
        Y = Y + 40

        'Cabeceras del informe
        For iBucle = 0 To ListView1.Columns.Count - 1
            TabGAP(iBucle) = CType(AnchuraMax / ListView1.Columns.Count, Integer)
            e.Graphics.DrawString(ListView1.Columns(iBucle).Text, _
                                FontPrint, Brushes.Black, TabINI + TabGAP(iBucle) * (iBucle), Y)
        Next

        Y = Y + 20
        Dim MaxLineasV As Integer
        MaxLineasV = 0
        'If itm > ListView1.Items.Count Then itm = 0
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
                e.Graphics.DrawString(str, TableFont, Brushes.Blue, TabINI, Y)
            Else
                e.Graphics.DrawString(str, TableFont, Brushes.Red, _
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
                    e.Graphics.DrawString(str, TableFont, Brushes.Blue, TabINI + TabGAP(iBucle) * (iBucle), Y)
                Else
                    e.Graphics.DrawString(str, TableFont, Brushes.Red, _
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
            '--------------------------------------------------------------------------------------------------
            'Cuando lee esta linea, si se ha impreso todo, sale
            If itm >= ListView1.Items.Count Then
                Exit While
            End If
            '--------------------------------------------------------------------------------------------------
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

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Cancelar = True
    End Sub


    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click


        If ListView1.Tag <> 1 Then
            MessageBox.Show("No hay exportación a formato JSON para este modelo de informe", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If


        Dim CeldaOUT As String
        Dim LineaOUT As String
        Dim iBucle As Integer

        If ListView1.Items.Count = 0 Then Exit Sub
        With SaveFileDialog1
            .Title = "Introduzca el nombre del fichero"
            .Filter = "Archivos JSON *.json|*.json"
            .ShowDialog()
        End With
        If SaveFileDialog1.FileName = "" Then Exit Sub

        Dim sw As New System.IO.StreamWriter(SaveFileDialog1.FileName, False, System.Text.Encoding.UTF8)
        'Dim sw As New System.IO.StreamWriter("C:\ms4w\Apache\htdocs\test\d3\siddaereporttest.json", False, System.Text.Encoding.UTF8)



        sw.WriteLine("{")
        sw.WriteLine("""name"":""GEODOCAT"",")
        sw.WriteLine("""children"":[")

        Dim cont As Integer = 0
        For Each Linea As ListViewItem In ListView1.Items
            Dim sumaDocProvin As Integer = 0
            cont = cont + 1
            For iBucle = 1 To Linea.SubItems.Count - 2
                sumaDocProvin = sumaDocProvin + CType(Linea.SubItems(iBucle).Text, Integer)
            Next
            sw.WriteLine("{")
            sw.WriteLine("""name"":""" & Linea.Text & ": " & sumaDocProvin & """,")
            sw.WriteLine("""children"":[")
            LineaOUT = ""
            For iBucle = 1 To Linea.SubItems.Count - 2
                If CType(Linea.SubItems(iBucle).Text, Integer) > 0 Then
                    'sw.WriteLine("""name"":""" & ListView1.Columns(iBucle).Text & ": " & CType(Linea.SubItems(iBucle).Text, Integer) & """,""size"":4000")
                    LineaOUT = LineaOUT & "{""name"":""" & ListView1.Columns(iBucle).Text & ": " & CType(Linea.SubItems(iBucle).Text, Integer) & """,""size"":4000},"
                End If
            Next
            sw.WriteLine(LineaOUT.Substring(0, LineaOUT.Length - 1))
            sw.WriteLine("]")
            If cont = ListView1.Items.Count - 1 Then
                sw.WriteLine("}")
                Exit For
            Else
                sw.WriteLine("},")
            End If
            'sw.WriteLine("},")
            'If cont = 3 Then Exit For
        Next
        sw.WriteLine("]")
        sw.WriteLine("}")

        sw.Close()
        sw.Dispose()

        MessageBox.Show("Fichero de exportación generado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub


End Class