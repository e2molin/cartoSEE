Public Class ImportECW

    Private Function ComponerRutaECW(ByVal docu As docSIDCARTO, ByVal fichero As String) As String

        'Function DameRutasFicherosECW(ByVal documento As docSIDCARTO, ByRef Rutas() As String) As Boolean
        Dim Rutas() As String
        Dim rutaOUT As String
        Dim muni As String

        fichero = fichero.Replace("_geo", "")
        If fichero.Substring(0, 6) <> docu.Sellado Then Return ""
        If fichero.Length > 10 Then
            If Not IsNumeric(fichero.Substring(7, 2)) Then Return ""
            fichero = fichero.Substring(0, 9) & ".ecw"
        End If

        DameRutasFicherosECW(docu, Rutas)

        If Rutas(0) Is Nothing Then
            If docu.Municipios.IndexOf("#") > -1 Then
                Dim munis() As String = docu.MunicipiosINE.Split("#")
                If munis.Length > 0 Then muni = munis(0)
                Erase munis
            Else
                muni = docu.MunicipiosINE
            End If
            rutaOUT = rutaRepoGeorref & _
                             "\" & DirRepoProvinciaByTipodoc(docu.CodTipo) & _
                             "\" & muni.Substring(0, 2) & _
                             "\" & muni & _
                            "\" & fichero
        Else
            rutaOUT = SacarDirDeRuta(Rutas(0)) & fichero
        End If
        Erase Rutas
        Return rutaOUT

    End Function


    Private Sub CargarContenidoDirectorio()

        If txtFolder.Text = "" Then
            MessageBox.Show("Selecciona directorio de contornos", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        If Not System.IO.Directory.Exists(txtFolder.Text) Then
            MessageBox.Show("Directorio de contornos no localizado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        Dim listaFicheros() As String
        Dim contador As Integer
        Dim elementoLV As ListViewItem
        Dim sellado As String
        Dim nomfich As String
        Dim docu As docSIDCARTO
        Dim rutaDestino As String

        Me.Cursor = Cursors.WaitCursor
        LanzarSpinner("Cargando datos")
        ListView1.Items.Clear()
        listaFicheros = System.IO.Directory.GetFiles(txtFolder.Text, "*.ecw")
        ToolStripStatusLabel1.Text = "Ficheros encontrados " & listaFicheros.Length
        ToolStripProgressBar1.Minimum = 1
        ToolStripProgressBar1.Maximum = listaFicheros.Length
        ToolStripProgressBar1.Visible = True
        contador = 0
        Application.DoEvents()
        For Each fichero As String In listaFicheros
            contador = contador + 1
            ToolStripProgressBar1.Value = contador
            Application.DoEvents()
            nomfich = SacarFileDeRuta(fichero)
            sellado = nomfich.Substring(0, 6).ToString
            If Not IsNumeric(sellado) Then
                elementoLV = New ListViewItem
                elementoLV.Tag = fichero
                elementoLV.Text = nomfich
                elementoLV.SubItems.Add(sellado)
                ListView1.Items.Add(elementoLV)
                elementoLV = Nothing
                Continue For
            End If
            docu = DameDocumentacionSIDCARTO_bySellado(sellado)

            elementoLV = New ListViewItem
            elementoLV.Tag = fichero
            elementoLV.Text = nomfich

            elementoLV.SubItems.Add(sellado)

            If docu.Sellado = "" Then
                ListView1.Items.Add(elementoLV)
                elementoLV = Nothing
                Continue For
            End If
            elementoLV.SubItems.Add(docu.Tipo)
            elementoLV.SubItems.Add(docu.Municipios)
            rutaDestino = ComponerRutaECW(docu, nomfich)
            elementoLV.SubItems.Add(rutaDestino)

            If rutaDestino = "" Then
                elementoLV.ForeColor = Color.Black
            Else
                If System.IO.File.Exists(rutaDestino) Then
                    elementoLV.ForeColor = Color.Red
                Else
                    elementoLV.ForeColor = Color.Blue
                End If
            End If
            ListView1.Items.Add(elementoLV)
            elementoLV = Nothing
        Next
        CerrarSpinner()
        Me.Cursor = Cursors.Default
        ToolStripProgressBar1.Visible = False
        ToolStripStatusLabel1.Text = "Ficheros analizados:" & ListView1.Items.Count
        MessageBox.Show("Proceso terminado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)



    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenFolder.Click


        FolderBrowserDialog1.ShowNewFolderButton = True
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
        If System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath) = False Then Exit Sub
        txtFolder.Text = FolderBrowserDialog1.SelectedPath
        CargarContenidoDirectorio()

    End Sub




    Private Sub ImportECW_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ListView1.Columns.Add("Fichero", 100, HorizontalAlignment.Left)
        ListView1.Columns.Add("Sellado", 75, HorizontalAlignment.Left)
        ListView1.Columns.Add("Tipo", 100, HorizontalAlignment.Left)
        ListView1.Columns.Add("Municipio", 100, HorizontalAlignment.Left)
        ListView1.Columns.Add("RutaDestino", 120, HorizontalAlignment.Left)

        ListView1.View = View.Details
        ListView1.FullRowSelect = True
        ToolStripProgressBar1.Visible = False
        ToolStripStatusLabel1.Text = "Seleccione un directorio con ECW"

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        CargarContenidoDirectorio()

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

        Dim contador As Integer
        If Not testWriteInPath(rutaRepoGeorref) Then Exit Sub
        If ListView1.Items.Count = 0 Then Exit Sub
        If MessageBox.Show("Elementos en azul se incorporarán al repositorio como nuevos documentos." & System.Environment.NewLine & _
                        "Elementos en rojo se incorporarán al repositorio sustituyendo el existente." & System.Environment.NewLine & _
                        "Elementos en negro no serán procesados.¿Desea continuar?", AplicacionTitulo, MessageBoxButtons.YesNo, _
                        MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then Exit Sub

        contador = 0
        ToolStripProgressBar1.Maximum = ListView1.Items.Count
        ToolStripProgressBar1.Visible = True
        Me.Cursor = Cursors.WaitCursor
        For Each elem As ListViewItem In ListView1.Items
            If elem.SubItems.Count <> 5 Then Continue For
            Application.DoEvents()
            Try
                If Not System.IO.Directory.Exists(SacarDirDeRuta(elem.SubItems(4).Text)) Then _
                                        System.IO.Directory.CreateDirectory(SacarDirDeRuta(elem.SubItems(4).Text))
                System.IO.File.Copy(elem.Tag, elem.SubItems(4).Text, True)
                GenerarLOG("Copia:" & elem.Tag & " >> " & elem.SubItems(4).Text)
                contador = contador + 1
                ToolStripStatusLabel1.Text = "Copiando fichero nº " & contador
                ToolStripProgressBar1.Value = contador
            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                If MessageBox.Show("¿Desea detener proceso de copia?", AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then Exit For
            End Try
        Next
        Me.Cursor = Cursors.Default
        ToolStripProgressBar1.Visible = False
        ToolStripStatusLabel1.Text = "Ficheros copiados " & contador
        MessageBox.Show("Proceso terminado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub ListView1_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles ListView1.ColumnClick
        ' Crear una instancia de la clase que realizará la comparación
        Dim oCompare As New ListViewColumnSortSimple()

        ' Asignar el orden de clasificación
        If ListView1.Sorting = SortOrder.Ascending Then
            oCompare.Sorting = SortOrder.Descending
        Else
            oCompare.Sorting = SortOrder.Ascending
        End If
        ListView1.Sorting = oCompare.Sorting
        oCompare.ColumnIndex = e.Column
        ListView1.ListViewItemSorter = oCompare

    End Sub

End Class