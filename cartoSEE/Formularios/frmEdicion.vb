Public Class frmEdicion

    Dim Autocompletar_municipios As Boolean
    Public ElemEdit As New ArrayList
    'Dim ElemModify() As docSIDCARTO
    Dim elementsInEdition As docCartoSEEquery

    Private Sub frmEdicion_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Colocamos elementos
        Me.Size = New Point(1050, 800)
        ListBox1.Location = New Point(404, 127)
        ListBox1.Size = New Point(249, 150)
        ListBox1.Visible = False

        GroupBox1.Location = New Point(12, 53)
        GroupBox1.Size = New Point(980, 670)
        GroupBox1.Visible = True

        GroupBox2.Location = New Point(12, 53)
        GroupBox2.Size = New Point(980, 670)
        GroupBox2.Visible = False

        ListView1.FullRowSelect = True
        ListView1.View = View.Details
        ListView1.Columns.Add("Municipio", 145, HorizontalAlignment.Left)
        ListView1.Columns.Add("Provincia", 80, HorizontalAlignment.Left)
        ListView1.Columns.Add("INE", 0, HorizontalAlignment.Left)
        ListView1.Columns.Add("idMuni", 0, HorizontalAlignment.Left)
        ListView1.Columns.Add("provinciaId", 0, HorizontalAlignment.Left)
        MaskedTextBox1.Mask = "00/00/0000"

        'Cargamos Combos de selección
        'CargarFiltros()
        Autocompletar_municipios = True
        ToolStripStatusLabel2.Text = ""

        CheckBox21.Visible = usuarioMyApp.permisosLista.isUserISTARI


    End Sub

    'Carga de los Combos de seleccion
    Sub CargarFiltros()

        'Carga del Combo de Provincias
        Dim contadorProv As Integer
        For Each Provincia As DataRow In ListaProvincias.Select
            contadorProv = contadorProv + 1
            ComboBox4.Items.Add(New itemData(Provincia.ItemArray(1).ToString, contadorProv))
            ComboBox6.Items.Add(New itemData(Provincia.ItemArray(1).ToString, contadorProv))
        Next
        ComboBox4.Items.Add(New itemData("(Todas)", 0))

        'Carga del Combo de Estado de documentos
        Dim Filtros As DataTable
        Filtros = New DataTable
        If CargarDatatable("Select idestadodoc,estadodoc FROM bdsidschema.tbestadodocumento", Filtros) = False Then
            MessageBox.Show("No se puede acceder a la tabla de Estados de la documentación",
                            AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            ComboBox2.Items.Clear()
            For Each Filtro As DataRow In Filtros.Select
                ComboBox2.Items.Add(New itemData(Filtro.ItemArray(1).ToString, Filtro.ItemArray(0)))
            Next
        End If
        Filtros.Dispose()
        Filtros = Nothing

        'Carga del Combo de Tipos de documento
        Filtros = New DataTable
        If CargarDatatable("Select idtipodoc,tipodoc from bdsidschema.tbtipodocumento", Filtros) = False Then
            MessageBox.Show("No se puede acceder a la tabla de Tipos de documentación",
                            AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            ComboBox1.Items.Clear()
            For Each Filtro As DataRow In Filtros.Select
                ComboBox1.Items.Add(New itemData(Filtro.ItemArray(1).ToString, Filtro.ItemArray(0)))
            Next
        End If
        Filtros.Dispose()
        Filtros = Nothing

        'Carga del combo de Observaciones
        Filtros = New DataTable
        If CargarDatatable("Select idobservestandar,observestandar from bdsidschema.tbobservaciones", Filtros) = False Then
            MessageBox.Show("No se puede acceder a la tabla de Tipos de documentación",
                            AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            ComboBox3.Items.Clear()
            For Each Filtro As DataRow In Filtros.Select
                ComboBox3.Items.Add(New itemData(Filtro.ItemArray(1).ToString, Filtro.ItemArray(0)))
            Next
        End If
        Filtros.Dispose()
        Filtros = Nothing

        ComboBox5.Items.Add(New itemData("No", 0))
        ComboBox5.Items.Add(New itemData("Sí", 1))

        Dim listFragProps As New FlagsProperties

        For Each propItem In listFragProps.propertyList
            CheckedListBox1.Items.Add(propItem, CheckState.Unchecked)
        Next


    End Sub


    '----------------------------------------------------------------------------------------------------------------------
    'Control del Textbox de búsqueda
    '----------------------------------------------------------------------------------------------------------------------

    Private Sub TextBox1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox1.Click
        If TextBox1.Text.Trim <> "" Then
            TextBox1.SelectAll()
        End If
    End Sub

    Private Sub TextBox1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) _
            Handles TextBox1.KeyUp
        Application.DoEvents()
        If e.KeyData = Keys.Down And ListBox1.Visible = True Then
            ListBox1.Focus()
            ListBox1.SelectedIndex = 0
        ElseIf e.KeyData = Keys.Enter And ListBox1.Visible = False Then
            SeleccionMunicipios(sender, e)
        End If
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged

        Dim filas() As DataRow
        ListBox1.Visible = False
        If TextBox1.Text.Length > 1 And Autocompletar_municipios = True Then
            PictureBox1.Visible = True
            Application.DoEvents()
            If ComboBox4.SelectedIndex = -1 Or ComboBox4.Text = "(Todas)" Then
                filas = ListaMunicipiosHisto.Select("nombre like '%" & TextBox1.Text & "%'")
            Else
                Dim cProv As Integer = CType(ComboBox4.SelectedItem, itemData).Valor
                filas = ListaMunicipiosHisto.Select("provincia_id=" & cProv.ToString & " and nombre like '%" & TextBox1.Text & "%'")
            End If
            ListBox1.Items.Clear()
            For Each dR As DataRow In filas
                If CheckBox1.Checked = True Then
                    If dR("cod_munihisto").ToString.EndsWith("00") = True Then
                        ListBox1.Items.Add(New itemData(dR("nombre").ToString, String.Format("{0:0000000}", dR("cod_munihisto")) & "|" & dR("idmunihisto")))
                    End If
                Else
                    ListBox1.Items.Add(New itemData(dR("nombre").ToString, String.Format("{0:0000000}", dR("cod_munihisto")) & "|" & dR("idmunihisto")))
                End If
                ListBox1.Visible = True
            Next
            If ListBox1.Items.Count = 1 Then
                Try
                    Autocompletar_municipios = False
                    Dim Indices() As String = CType(ListBox1.Items(0), itemData).Valor.Split("|")
                    TextBox1.Text = ListBox1.Items(0).ToString
                    TextBox1.Tag = CType(ListBox1.Items(0), itemData).Valor
                    ListBox1.Tag = Indices(1)
                    ListBox1.Visible = False
                    Autocompletar_municipios = True
                Catch
                    MessageBox.Show("Repita la búsqueda", My.Application.Info.AssemblyName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End Try
            End If
            PictureBox1.Visible = False
        End If

    End Sub

    Private Sub SeleccionarElementoLB(ByVal sender As Object, ByVal e As System.EventArgs) _
            Handles ListBox1.DoubleClick, ListBox1.Click


        If ListBox1.SelectedItem Is Nothing Then Exit Sub
        Autocompletar_municipios = False
        TextBox1.Text = ListBox1.SelectedItem.ToString
        TextBox1.Tag = CType(ListBox1.Items(ListBox1.SelectedIndex), itemData).Valor
        Dim Indices() As String = TextBox1.Tag.Split("|")
        ListBox1.Tag = Indices(1)
        Autocompletar_municipios = True
        ListBox1.Visible = False

    End Sub

    Private Sub ListBox1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ListBox1.KeyUp
        Application.DoEvents()
        If e.KeyData = Keys.Up And ListBox1.SelectedIndex = 0 Then
            TextBox1.Focus()
        ElseIf e.KeyData = Keys.Return And ListBox1.SelectedIndex >= 0 Then
            SeleccionarElementoLB(sender, e)
            SeleccionMunicipios(sender, e)
            TextBox1.Focus()
        End If

    End Sub

    Private Sub SeleccionMunicipios(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                Handles Button1.Click, Button2.Click

        If sender.name = "Button1" Or sender.name = "ListBox1" Or sender.name = "TextBox1" Then
            If TextBox1.Text <> "" Then
                If String.IsNullOrEmpty(TextBox1.Tag) Then Exit Sub
                Dim CodigosMuni() As String = TextBox1.Tag.ToString.Split("|")
                If CodigosMuni.Length <> 2 Then Exit Sub
                Dim elementoLV As ListViewItem
                elementoLV = New ListViewItem
                elementoLV.Text = TextBox1.Text
                elementoLV.SubItems.Add(DameProvinciaByINE(CType(CodigosMuni(0).Substring(0, 2), Integer)))
                elementoLV.SubItems.Add(CodigosMuni(0))
                elementoLV.SubItems.Add(CodigosMuni(1))
                elementoLV.SubItems.Add(CType(CodigosMuni(0).Substring(0, 2), Integer))
                ListView1.Items.Add(elementoLV)
                elementoLV = Nothing
            End If
        ElseIf sender.name = "Button2" Then
            For Each item As ListViewItem In ListView1.SelectedItems
                item.Remove()
            Next
        End If
    End Sub


    Sub ModoTrabajo(ByVal modo As String, ByVal indice As Integer)

        Dim cadIndices As String
        'Recibimos un Lote de documentos para actualizar
        CargarFiltros()
        If modo = "LOTE" Then
            Me.Tag = indice
            Me.Text = "Edición en lote"
            elementsInEdition = New docCartoSEEquery
            elementsInEdition.flag_CargarFicherosGEO = True
            'cadIndices = String.Join(",", ElemEdit.ToArray)
            For Each indiceDoc As Integer In ElemEdit
                If cadIndices = "" Then cadIndices = indiceDoc : Continue For
                cadIndices = cadIndices & "," & indiceDoc
            Next
            elementsInEdition = New docCartoSEEquery
            elementsInEdition.flag_CargarFicherosGEO = True
            elementsInEdition.getDocsSIDDAE_ByFiltroSellado("archivo.idarchivo in(" & cadIndices & ")")

            ToolStripStatusLabel1.Text = "Edición en lote: " & elementsInEdition.resultados.Count & " elementos."
            GroupBox1.Text = "Edición en lote"
            CheckBox22.Visible = False
            TextBox22.Visible = False
            Button5.Visible = False
            Button6.Visible = False
            ComboBox3.Items.Add("SIN OBSERVACIONES")
            CheckBox23.Visible = False
        ElseIf modo = "SIMPLE" Then
            'ReDim ElemModify(ElemEdit.Count - 1)
            'ElemEdit.CopyTo(ElemModify)
            Me.Tag = indice
            elementsInEdition = New docCartoSEEquery
            elementsInEdition.flag_CargarFicherosGEO = True
            elementsInEdition.getDocsSIDDAE_ByFiltroSellado("archivo.idarchivo=" & indice)

            If elementsInEdition.resultados.Count <> 1 Then
                If ModalQuestion($"No se localiza el documento con idarchivo = {indice}.¿Desea crearlo?") = DialogResult.Yes Then
                    Me.Tag = "0"
                    Me.Text = "Nuevo documento"
                    ToolStripStatusLabel1.Text = "Nuevo documento."
                    GroupBox1.Text = "Nuevo documento"
                    Button5.Visible = True
                    Button6.Visible = True
                    Button9.Visible = False
                    CheckBox23.Visible = True
                    Exit Sub
                Else
                    Me.Close()
                    Exit Sub
                End If

            End If


            Me.Text = "Edición del documento nº: " & elementsInEdition.resultados(0).sellado
            ToolStripStatusLabel1.Text = "Edición del documento."
            GroupBox1.Text = "Edición del documento"
            CheckBox22.Visible = True
            TextBox22.Visible = True
            Button5.Visible = False
            Button6.Visible = False
            Button9.Visible = True
            ComboBox3.Items.Add("SIN OBSERVACIONES")
            'Relleno los campos con los valores por defecto
            TextBox4.Text = elementsInEdition.resultados(0).Vertical
            TextBox5.Text = elementsInEdition.resultados(0).Horizontal
            TextBox6.Text = elementsInEdition.resultados(0).proceCarpeta
            TextBox7.Text = elementsInEdition.resultados(0).Signatura
            TextBox8.Text = elementsInEdition.resultados(0).Escala
            TextBox9.Text = elementsInEdition.resultados(0).subTipoDoc
            TextBox10.Text = elementsInEdition.resultados(0).proceHoja
            TextBox11.Text = elementsInEdition.resultados(0).Tomo
            TextBox12.Text = elementsInEdition.resultados(0).Subdivision
            TextBox13.Text = elementsInEdition.resultados(0).fechasModificaciones
            TextBox14.Text = elementsInEdition.resultados(0).Coleccion
            TextBox15.Text = elementsInEdition.resultados(0).Anejo
            TextBox16.Text = elementsInEdition.resultados(0).Observaciones
            TextBox22.Text = elementsInEdition.resultados(0).Sellado
            ComboBox1.Text = elementsInEdition.resultados(0).Tipo
            ComboBox2.Text = elementsInEdition.resultados(0).Estado
            ComboBox3.Text = elementsInEdition.resultados(0).ObservacionesStandard
            ComboBox5.Text = elementsInEdition.resultados(0).JuntaEstadistica
            ComboBox6.Text = DameProvinciaByINE(elementsInEdition.resultados(0).ProvinciaRepo)

            If Not elementsInEdition.resultados(0).fechaPrincipal Is Nothing Then
                If elementsInEdition.resultados(0).fechaPrincipal.ToString.Length = 10 Then
                    MaskedTextBox1.Text = elementsInEdition.resultados(0).fechaPrincipal.Substring(8, 2) & "/" & _
                                            elementsInEdition.resultados(0).fechaPrincipal.Substring(5, 2) & "/" & _
                                            elementsInEdition.resultados(0).fechaPrincipal.Substring(0, 4)
                End If
            End If
            CheckBox23.Visible = False

            Application.DoEvents()
            'Rellenamos los items de propiedades

            Dim propPatron As String = elementsInEdition.resultados(0).extraProps.propertyPatron.ToString
            For itemCheck As Integer = 0 To CheckedListBox1.Items.Count - 1
                If propPatron.Substring(propPatron.Length - 1 - itemCheck, 1) = "1" Then
                    CheckedListBox1.SetItemChecked(itemCheck, True)
                Else
                    CheckedListBox1.SetItemChecked(itemCheck, False)
                End If
            Next

        ElseIf modo = "NUEVO" Then
            Me.Tag = indice
            Me.Text = "Nuevo documento"
            ToolStripStatusLabel1.Text = "Nuevo documento."
            GroupBox1.Text = "Nuevo documento"
            Button5.Visible = True
            Button6.Visible = True
            Button9.Visible = False
            CheckBox23.Visible = True
        End If

        Application.DoEvents()

    End Sub


    Private Sub LimpiarCampos(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If Me.Tag = 0 Then
            If CheckBox2.Checked = False Then ComboBox2.SelectedIndex = -1
            If CheckBox3.Checked = False Then ComboBox3.SelectedIndex = -1
            If CheckBox17.Checked = False Then ComboBox1.SelectedIndex = -1
            If CheckBox18.Checked = False Then ListView1.Items.Clear()
            If CheckBox19.Checked = False Then ComboBox5.SelectedIndex = -1
            If CheckBox20.Checked = False Then ComboBox6.SelectedIndex = -1
            If CheckBox4.Checked = False Then TextBox4.Text = ""
            If CheckBox5.Checked = False Then TextBox5.Text = ""
            If CheckBox6.Checked = False Then TextBox6.Text = ""
            If CheckBox7.Checked = False Then TextBox7.Text = ""
            If CheckBox8.Checked = False Then TextBox8.Text = ""
            If CheckBox24.Checked = False Then TextBox9.Text = ""
            If CheckBox9.Checked = False Then MaskedTextBox1.Text = ""
            If CheckBox10.Checked = False Then TextBox10.Text = ""
            If CheckBox11.Checked = False Then TextBox11.Text = ""
            If CheckBox12.Checked = False Then TextBox12.Text = ""
            If CheckBox13.Checked = False Then TextBox13.Text = ""
            If CheckBox14.Checked = False Then TextBox14.Text = ""
            If CheckBox15.Checked = False Then TextBox15.Text = ""
            If CheckBox16.Checked = False Then TextBox16.Text = ""
            If CheckBox22.Checked = True Then
                If TextBox22.Text.Trim <> "" Then
                    If IsNumeric(TextBox22.Text) Then
                        TextBox22.Text = String.Format("{0:000000}", CType(TextBox22.Text.Trim, Integer) + 1)
                    End If
                End If
            Else
                TextBox22.Text = ""
            End If

            TextBox22.Focus()
        Else
            CheckBox2.Checked = False
            CheckBox3.Checked = False
            CheckBox4.Checked = False
            CheckBox5.Checked = False
            CheckBox6.Checked = False
            CheckBox7.Checked = False
            CheckBox8.Checked = False
            CheckBox9.Checked = False
            CheckBox10.Checked = False
            CheckBox11.Checked = False
            CheckBox12.Checked = False
            CheckBox13.Checked = False
            CheckBox14.Checked = False
            CheckBox15.Checked = False
            CheckBox16.Checked = False
            CheckBox17.Checked = False
            CheckBox18.Checked = False
            CheckBox24.Checked = False
            ComboBox1.Text = ""
            ComboBox2.Text = ""
            ComboBox3.Text = ""
            ListView1.Items.Clear()
            TextBox15.Text = ""
            TextBox16.Text = ""
            TextBox2.Text = ""
            TextBox3.Text = ""
            TextBox4.Text = ""
            TextBox5.Text = ""
            TextBox6.Text = ""
            TextBox7.Text = ""
            TextBox8.Text = ""
            TextBox9.Text = ""
            TextBox10.Text = ""
            TextBox11.Text = ""
            TextBox12.Text = ""
            TextBox13.Text = ""
        End If

    End Sub

    Private Sub ActualizacionLote()

        'Si uno de estos campos está activo, compuebo que tengamos permiso
        'de escritura en disco de datos, ya que modificar estos campos
        'seguramente implica mover documentos.

        If CheckBox17.Checked = True Or CheckBox18.Checked = True Or CheckBox22.Checked = True Then
            Try
                Dim file As System.IO.FileStream
                file = System.IO.File.Create(rutaRepo & "\testdummy1234.txt")
                file.Close()
                System.IO.File.Delete(rutaRepoGeorref & "\testdummy1234.txt")
                file = System.IO.File.Create(rutaRepoGeorref & "\testdummy1234.txt")
                file.Close()
                System.IO.File.Delete(rutaRepoGeorref & "\testdummy1234.txt")
            Catch ex As Exception
                MessageBox.Show("No dispone de permiso de escritura en el repositorio." & Environment.NewLine() & _
                                "Los cambios requieren traslado de documentos en el disco.", _
                                AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End Try
        End If

        Dim cadUpBase As String
        Dim NuevoMuni As String = ""
        Dim NuevoTipo As Integer = -1
        'Generamos la cadena base de la ejecución en lote
        cadUpBase = GenerarCadUpdateLoteBase()
        If cadUpBase = "" And CheckBox18.Checked = False Then
            MessageBox.Show("No se definieron valores.No se realizará ninguna modificación", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If CheckBox17.Checked = True Then NuevoTipo = CType(CType(ComboBox1.SelectedItem, itemData).Valor, Integer)
        If CheckBox18.Checked = True Then
            If ListView1.Items.Count = 0 Then
                MessageBox.Show("No hay municipios asignados.No se realizará ninguna modificación", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            NuevoMuni = String.Format("{0:0000000}", ListView1.Items(0).SubItems(2).Text)
        End If

        Application.DoEvents()
        Dim ListaSQL As New ArrayList
        Dim ListaRenFich As New ArrayList
        Dim Renombre As RenFich

        For Each item As docCartoSEE In elementsInEdition.resultados
            If cadUpBase <> "" Then
                'GenerarLOG(cadUpBase & " WHERE idarchivo=" & item.docIndex)
                ListaSQL.Add(cadUpBase & " WHERE idarchivo=" & item.docIndex)
            End If
            'Si hay cambios en el municipio, se borran las asociaciones antiguas 
            'y se introducen las nuevas
            If CheckBox18.Checked = True And ListView1.Items.Count > 0 Then
                If item.ProvinciaRepo <> CType(ListView1.Items(0).SubItems(4).Text, Integer) Then
                    If item.ProvinciaRepo = 82 And CType(ListView1.Items(0).SubItems(4).Text, Integer) = 28 Then
                        Application.DoEvents()
                    Else
                        If CheckBox20.Checked = False Then
                            ModalExclamation("La Provincia del documento debe ser la misma a la que pertenece el primer municipio asignado. No se realizará ninguna modificación")
                            Exit Sub
                        End If
                        If CType(ComboBox6.SelectedItem, itemData).Valor <> ListView1.Items(0).SubItems(4).Text Then
                            ModalExclamation("La Provincia del documento debe ser la misma a la que pertenece el primer municipio asignado. No se realizará ninguna modificación")
                            Exit Sub
                        End If
                    End If
                End If



                ListaSQL.Add($"DELETE FROM bdsidschema.archivo2territorios where archivo_id={item.docIndex}")
                ListaSQL.Add($"INSERT INTO bdsidschema.archivolog (archivo_id, sellado, usuario_update, tabla, tipo_variacion, valor_old, valor_new) 
                                 VALUES ({item.docIndex},'{ item.Sellado}','{usuarioMyApp.loginUser}','archivo2territorios','Desasignación municipio histórico',
                                 E'{item.muniHistoLiteralConINEHistorico.Replace("'", "\'")}',null)")

                For Each itemLV As ListViewItem In ListView1.Items
                    ListaSQL.Add($"INSERT INTO bdsidschema.archivo2territorios (territorio_id,archivo_id) VALUES ({itemLV.SubItems(3).Text},{item.docIndex})")
                    ListaSQL.Add($"INSERT INTO bdsidschema.archivolog (archivo_id, sellado, usuario_update, tabla, tipo_variacion, valor_old, valor_new) 
                                VALUES ({item.docIndex},'{item.Sellado}','{usuarioMyApp.loginUser}','archivo2munihisto','Asignación municipio histórico',null,
                                E'{itemLV.SubItems(0).Text.Replace("'", "\'")}({itemLV.SubItems(2).Text})')")
                Next





            End If
            'Si se han producido cambios en el tipo de documento o en los municipios o en el número de sellado
            'en las ediciones individuales, puede originarse un cambio de 
            'Realizamos los renombres necesarios
            If CheckBox17.Checked = True Or CheckBox18.Checked = True Or CheckBox20.Checked = True Or CheckBox22.Checked = True Then
                Application.DoEvents()
                Dim RutasDoc() As String
                Erase RutasDoc
                'Añadimos a Rutasdoc las rutas con las imágenes JPG si se modifica el númeo de sellado
                If CheckBox20.Checked = True Or CheckBox22.Checked = True Then
                    ReDim Preserve RutasDoc(RutasDoc.Length - 1 + 2)
                    RutasDoc(RutasDoc.Length - 2) = rutaRepo & "\_Scan400\" & item.FicheroJPG
                    RutasDoc(RutasDoc.Length - 1) = rutaRepo & "\_Scan250\" & item.FicheroJPG.Replace("\", "250\")
                End If
                For Each Rutageo As DataRow In item.rcdgeoFiles.Select
                    If IsNothing(Rutageo) Then Continue For
                    Application.DoEvents()
                    'Renombre.NombreAntiguo = Rutageo
                    'Renombre.NombreNuevo = GenerarScriptRenombreFicheros(item, Rutageo, NuevoTipo, NuevoMuni, TextBox22.Text)
                    'Si el nombre generado es distinto
                    If Renombre.NombreAntiguo <> Renombre.NombreNuevo Then
                        GenerarLOG("Renombrar:" & Renombre.NombreAntiguo & " >> " & Renombre.NombreNuevo)
                        ListaRenFich.Add(Renombre)
                    Else
                        GenerarLOG("Los cambios no afectan al nombre")
                    End If
                Next
            End If
        Next

        Application.DoEvents()
        If CheckBox21.Checked = True Then
            If MessageBox.Show("Comprueba el log.¿Abrir?", AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                Process.Start(ficheroLogger)
            End If
            Exit Sub
        End If

        Dim Ejecucion As Boolean = False
        Dim contador As Integer = 0
        Me.Cursor = Cursors.WaitCursor
        'Primero realizamos proceso de copia de los antiguos a los nuevos ficheros
        If ListaRenFich.Count > 0 Then
            contador = 0
            For Each cambioNombre As RenFich In ListaRenFich
                contador = contador + 1
                ToolStripStatusLabel2.Text = "Copiando fichero " & contador & " de " & ListaRenFich.Count
                Application.DoEvents()
                Try
                    If System.IO.Directory.Exists(SacarDirDeRuta(cambioNombre.NombreNuevo)) = False Then
                        System.IO.Directory.CreateDirectory(SacarDirDeRuta(cambioNombre.NombreNuevo))
                    End If
                    System.IO.File.Copy(cambioNombre.NombreAntiguo, cambioNombre.NombreNuevo, True)
                    Ejecucion = True
                Catch ex As Exception
                    MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Ejecucion = False
                    Exit For
                End Try
            Next

            'Si la Ejecucion no ha sido satisfactoria, borramos todo lo copiado
            If Ejecucion = False Then
                contador = 0
                For Each cambioNombre As RenFich In ListaRenFich
                    contador = contador + 1
                    ToolStripStatusLabel2.Text = "Analizando fichero " & contador & " de " & ListaRenFich.Count
                    Try
                        If System.IO.File.Exists(cambioNombre.NombreNuevo) Then System.IO.File.Delete(cambioNombre.NombreNuevo)
                        Ejecucion = True
                    Catch ex As Exception
                        MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Ejecucion = False
                        Exit For
                    End Try
                Next
                MessageBox.Show("No se realizaron los cambios correctamente", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Me.Cursor = Cursors.Default
                Exit Sub
            End If
        End If

        'Si llegamos aquí los renombres se han efectuado correctamente. Ahora ejecutamos la transación
        Dim cadListaSQL() As String
        ReDim cadListaSQL(ListaSQL.Count - 1)
        ListaSQL.CopyTo(cadListaSQL)
        Application.DoEvents()
        Ejecucion = ExeTran(cadListaSQL)
        Application.DoEvents()


        'Ahora, en función de que se haya realizado bien o mal la transacción, borramos los nuevos o los viejos.
        If ListaRenFich.Count > 0 Then
            If Ejecucion = False Then
                contador = 0
                For Each cambioNombre As RenFich In ListaRenFich
                    contador = contador + 1
                    ToolStripStatusLabel2.Text = "Restaurando fichero " & contador & " de " & ListaRenFich.Count
                    Try
                        If System.IO.File.Exists(cambioNombre.NombreNuevo) Then System.IO.File.Delete(cambioNombre.NombreNuevo)
                        Ejecucion = True
                    Catch ex As Exception
                        MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Ejecucion = False
                        Exit For
                    End Try
                Next
                MessageBox.Show("No se realizaron los cambios correctamente", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            Else
                contador = 0
                For Each cambioNombre As RenFich In ListaRenFich
                    contador = contador + 1
                    ToolStripStatusLabel2.Text = "Reubicando fichero " & contador & " de " & ListaRenFich.Count
                    Try
                        If System.IO.File.Exists(cambioNombre.NombreAntiguo) Then System.IO.File.Delete(cambioNombre.NombreAntiguo)
                        Ejecucion = True
                    Catch ex As Exception
                        MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        MessageBox.Show("Algunos documentos no se eliminaron de su ubicación original", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Ejecucion = False
                        Exit For
                    End Try
                Next
            End If
        End If

        Me.Cursor = Cursors.Default
        If Ejecucion = True Then
            MessageBox.Show("Los cambios se realizaron correctamente en la base de datos y en el disco.", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If


    End Sub

    Private Function borrarElementosEnEdicion() As Boolean
        'Compuebo que tengamos permiso de escritura en disco de datos, ya que modificar estos campos
        'seguramente implica mover documentos.


        Dim ListaSQL As New ArrayList
        Dim ficherosDelete As New ArrayList

        For Each elem As docCartoSEE In elementsInEdition.resultados
            ListaSQL.Add($"DELETE FROM bdsidschema.contornos WHERE archivo_id={elem.docIndex}")
            ListaSQL.Add($"DELETE FROM bdsidschema.archivo2territorios WHERE archivo_id={elem.docIndex}")
            ListaSQL.Add($"DELETE FROM bdsidschema.archivo WHERE idarchivo={elem.docIndex}")

            ficherosDelete.Add(elem.rutaFicheroAltaRes)
            ficherosDelete.Add(elem.rutaFicheroBajaRes)
            ficherosDelete.Add(elem.rutaFicheroThumb)

            For Each fileGEO As String In elem.listaFicherosGeo23030
                ficherosDelete.Add(fileGEO)
            Next

        Next

        If CheckBox21.Checked = True Then
            For Each sentenciaSQL As String In ListaSQL
                GenerarLOG(sentenciaSQL)
            Next
            For Each ficheroParaBorrar As String In ficherosDelete
                GenerarLOG($"Se borrará el fichero: {ficheroParaBorrar}")
            Next
            Return True
        End If
        For Each pathFileGeo As String In ficherosDelete
            If IsNothing(pathFileGeo) Then Continue For
            Try
                System.IO.File.Delete(pathFileGeo)
            Catch ex As Exception
                ModalError($"Se han producido errores al eliminar la información gráfica del documento.No se eliminó el documento.{Environment.NewLine}{ex.Message}")
                Exit Function
            End Try
        Next

        If ExeTran(ListaSQL) Then
            Return True
        Else
            ModalExclamation("No se han podido eliminar los documentos")
            Return False
        End If


    End Function

    Private Sub CrearNuevoElemento(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'Compuebo que tengamos permiso de escritura en disco de datos, ya que modificar estos campos
        'seguramente implica mover documentos.
        If CheckBox23.Checked = False Then
            Try
                Dim file As System.IO.FileStream
                file = System.IO.File.Create(rutaRepo & "\testdummy1234.txt")
                file.Close()
                System.IO.File.Delete(rutaRepoGeorref & "\testdummy1234.txt")
                file = System.IO.File.Create(rutaRepoGeorref & "\testdummy1234.txt")
                file.Close()
                System.IO.File.Delete(rutaRepoGeorref & "\testdummy1234.txt")
            Catch ex As Exception
                MessageBox.Show("No dispone de permiso de escritura en el repositorio." & Environment.NewLine() &
                                "Los cambios requieren traslado de documentos en el disco.",
                                AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End Try
        End If


        Dim cadInsBase As String
        Dim NuevoMuni As String = ""
        Dim NuevoTipo As Integer = -1
        Dim nuevoDoc As docSIDCARTO
        'Generamos la cadena base de la ejecución en lote
        cadInsBase = GenerarCadInsertBase(nuevoDoc)
        '----------------------------------------------------------------------------------------------------------
        If cadInsBase = "" Then
            Exit Sub
        End If

        If CheckBox17.Checked = True Then NuevoTipo = CType(CType(ComboBox1.SelectedItem, itemData).Valor, Integer)
        If CheckBox18.Checked = True Then
            If ListView1.Items.Count = 0 Then
                MessageBox.Show("No hay municipios asignados.No se realizará ninguna modificación", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            NuevoMuni = String.Format("{0:0000000}", ListView1.Items(0).SubItems(2).Text)
        End If

        Application.DoEvents()
        Dim ListaSQL As New ArrayList
        Dim ListaRenFich As New ArrayList
        Dim Renombre As RenFich

        GenerarLOG(cadInsBase)
        ListaSQL.Add(cadInsBase)
        'Si hay cambios en el municipio, se borran las asociaciones antiguas 
        'y se introducen las nuevas

        For Each itemLV As ListViewItem In ListView1.Items
            Application.DoEvents()
            GenerarLOG("INSERT INTO bdsidschema.archivo2munihisto (idarchivo2muni,munihisto_id,archivo_id) " &
                        "VALUES (nextval('bdsidschema.archivo2munihisto_idarchivo2muni_seq')," &
                        itemLV.SubItems(3).Text & "," & nuevoDoc.Indice & ")")
            ListaSQL.Add("INSERT INTO bdsidschema.archivo2munihisto (idarchivo2muni,munihisto_id,archivo_id) " &
                        "VALUES (nextval('bdsidschema.archivo2munihisto_idarchivo2muni_seq')," &
                        itemLV.SubItems(3).Text & "," & nuevoDoc.Indice & ")")
            ListaSQL.Add("INSERT INTO bdsidschema.archivo2territorios (territorio_id,archivo_id) " &
                        "VALUES (" & itemLV.SubItems(3).Text & "," & nuevoDoc.Indice & ")")
        Next

        If CheckBox21.Checked = True Then
            Exit Sub
        End If

        'Hasta aquí los datos. Ahora vamos por la información gráfica.
        'Lo primero, compruebo que se hayan seleccionado ficheros
        If CheckBox23.Checked = False Then
            If TextBox2.Text.Trim = "" And TextBox3.Text.Trim = "" Then
                If MessageBox.Show("¿Desea continuar sin agregar información gráfica?", AplicacionTitulo,
                                   MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then Exit Sub
            Else
                If System.IO.File.Exists(TextBox2.Text) = False Then
                    MessageBox.Show("No se localiza el fichero origen:" & System.Environment.NewLine &
                                    TextBox2.Text.Trim, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End If
                If System.IO.File.Exists(TextBox3.Text) = False Then
                    MessageBox.Show("No se localiza el fichero origen:" & System.Environment.NewLine &
                                    TextBox3.Text.Trim, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End If
            End If
            'Comenzamos las copias de imágenes previo a insertar el registro en la base de datos
            Me.Cursor = Cursors.WaitCursor
            If System.IO.File.Exists(TextBox2.Text) Then
                ToolStripStatusLabel2.Text = "Copiando imagen calidad alta"
                Try
                    System.IO.File.Copy(TextBox2.Text,
                            rutaRepo & "\_Scan400\" &
                            DirRepoProvinciaByINE(nuevoDoc.ProvinciaRepo) & "\" &
                            nuevoDoc.Sellado & ".jpg")
                Catch ex As Exception
                    MessageBox.Show("Se produjo un error." & System.Environment.NewLine &
                                    ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End Try
            End If
            If System.IO.File.Exists(TextBox3.Text) Then
                ToolStripStatusLabel2.Text = "Copiando imagen calidad normal"
                Try
                    System.IO.File.Copy(TextBox3.Text,
                            rutaRepo & "\_Scan250\" &
                            DirRepoProvinciaByINE(nuevoDoc.ProvinciaRepo) & "250\" &
                            nuevoDoc.Sellado & ".jpg")
                Catch ex As Exception
                    MessageBox.Show("Se produjo un error." & System.Environment.NewLine &
                                    ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End Try
            End If
        End If

        Me.Cursor = Cursors.Default
        Dim Ejecucion As Boolean = False
        Dim contador As Integer = 0
        Me.Cursor = Cursors.WaitCursor

        'Si llegamos aquí los renombres se han efectuado correctamente. Ahora ejecutamos la transación
        Dim cadListaSQL() As String
        ReDim cadListaSQL(ListaSQL.Count - 1)
        ListaSQL.CopyTo(cadListaSQL)
        Application.DoEvents()
        Ejecucion = ExeTran(cadListaSQL)
        Application.DoEvents()
        Me.Cursor = Cursors.Default
        If Ejecucion = True Then
            MessageBox.Show("El documento se ha cargado correctamente en la base de datos.", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        LimpiarCampos(sender, e)


    End Sub

    Private Function GenerarCadInsertBase(ByRef indiceDoc As docSIDCARTO) As String

        Dim FechaCreacion As Date
        Dim cadFechaCreacion As String
        Dim elementoInsert As docSIDCARTO

        GenerarCadInsertBase = ""
        FechaCreacion = Now
        cadFechaCreacion = FechaCreacion.Year &
                        "/" & String.Format("{0:00}", CInt(FechaCreacion.Month.ToString)) &
                        "/" & String.Format("{0:00}", CInt(FechaCreacion.Day.ToString)) & " " &
                        String.Format("{0:00}", CInt(FechaCreacion.Hour.ToString)) & ":" &
                        String.Format("{0:00}", CInt(FechaCreacion.Minute.ToString)) & ":00.00"

        If TextBox22.Text.Trim.Length <> 6 Then
            MessageBox.Show("El número de sellado debe tener seis dígitos", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Function
        End If
        If CType(TextBox22.Text.Substring(0, 2), Integer) = 82 Then
            If CType(ComboBox6.SelectedItem, itemData).Valor <> 28 Then
                MessageBox.Show("Los documentos con sellado 82nnnn pertenecen a la provincia de Madrid", AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                Exit Function
            End If
        End If
        If CType(TextBox22.Text.Substring(0, 2), Integer) = 88 Then
            If CType(ComboBox6.SelectedItem, itemData).Valor <> 28 Then
                MessageBox.Show("Los documentos con sellado 88nnnn pertenecen a la provincia de Madrid", AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                Exit Function
            End If
        End If

        If ComboBox6.SelectedItem Is Nothing Then
            MessageBox.Show("Es necesario seleccionar la provincia para almacenar los documentos", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Function
        Else
            If CType(ComboBox6.SelectedItem, itemData).Valor <> CType(TextBox22.Text.Substring(0, 2), Integer) Then
                If MessageBox.Show("Los dos primeros dígitos del número de sellado no coinciden con el código de provincia. ¿Continuar?",
                                AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then Exit Function
            End If
        End If

        If CType(ComboBox6.SelectedItem, itemData).Valor <> CType(TextBox22.Text.Substring(0, 2), Integer) Then
            If MessageBox.Show("Los dos primeros dígitos del número de sellado no coinciden con el código de provincia. ¿Continuar?",
                            AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then Exit Function
        End If


        Dim Resultado As String = ""
        ObtenerEscalar("SELECT idarchivo from bdsidschema.archivo where numdoc='" & TextBox22.Text.Trim & "'",
                        Resultado)
        Application.DoEvents()
        If CType(Resultado, Integer) > 0 Then
            MessageBox.Show("El número de sellado ya existe", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Function
        Else
            elementoInsert.Sellado = TextBox22.Text.Trim
        End If
        If ComboBox1.SelectedIndex = -1 Then
            MessageBox.Show("Seleccione un tipo de documento.", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Function
        Else
            elementoInsert.CodTipo = CType(ComboBox1.SelectedItem, itemData).Valor
        End If
        If ComboBox2.SelectedIndex = -1 Then
            MessageBox.Show("Asocie un estado de conservación al documento.", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Function
        Else
            elementoInsert.CodEstado = CType(ComboBox2.SelectedItem, itemData).Valor
        End If
        'Escala
        If TextBox8.Text.Trim <> "" Then
            elementoInsert.Escala = CType(TextBox8.Text.Trim, Integer)
        Else
            elementoInsert.Escala = 0
        End If
        'Observaciones standard
        If ComboBox3.SelectedIndex = -1 Then
            elementoInsert.ObservacionesStandard = -1
        Else
            elementoInsert.ObservacionesStandard = CType(ComboBox3.SelectedItem, itemData).Valor
        End If
        'Tomo
        If TextBox11.Text.Trim <> "" Then
            elementoInsert.Tomo = "'" & TextBox11.Text.Trim & "'"
        Else
            elementoInsert.Tomo = "Null"
        End If
        'ProceHoja
        If TextBox10.Text.Trim <> "" Then
            If IsNumeric(TextBox10.Text.Trim) Then
                elementoInsert.proceHoja = CType(TextBox10.Text.Trim, Integer)
            Else
                MessageBox.Show("El campo Hoja debe de ser un número", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Function
            End If
        Else
            elementoInsert.proceHoja = "-1"
        End If
        'ProceCarpeta
        If TextBox6.Text.Trim <> "" Then
            elementoInsert.proceCarpeta = TextBox6.Text.Trim
        Else
            elementoInsert.proceCarpeta = ""
        End If
        'Subtipo de documento
        If TextBox9.Text.Trim <> "" Then
            elementoInsert.subTipoDoc = TextBox9.Text.Trim
        Else
            elementoInsert.subTipoDoc = ""
        End If
        'FechaPrincipal
        Dim fechaDoc As Date
        Dim cadFechaDoc As String
        If IsDate(MaskedTextBox1.Text) = True Then
            fechaDoc = CType(MaskedTextBox1.Text, Date)
            'MessageBox.Show("Fecha válida")
            cadFechaDoc = fechaDoc.Year & "/" &
                                String.Format("{0:00}", CInt(fechaDoc.Month.ToString)) & "/" &
                                String.Format("{0:00}", CInt(fechaDoc.Day.ToString))
        Else
            MessageBox.Show("Fecha no válida. Introduzca una fecha correcta", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Function
        End If
        'Fechas Modificaciones
        If TextBox13.Text.Trim <> "" Then
            elementoInsert.fechasModificaciones = "'" & TextBox13.Text.Trim & "'"
        Else
            elementoInsert.fechasModificaciones = "Null"
        End If
        'Signatura
        If TextBox7.Text.Trim <> "" Then
            elementoInsert.Signatura = "'" & TextBox7.Text.Trim & "'"
        Else
            elementoInsert.Signatura = "Null"
        End If
        'Coleccion
        If TextBox14.Text.Trim <> "" Then
            elementoInsert.Coleccion = "'" & TextBox14.Text.Trim.Replace("'", "\'") & "'"
        Else
            elementoInsert.Coleccion = "Null"
        End If
        'Subdivisión
        If TextBox12.Text.Trim <> "" Then
            elementoInsert.Subdivision = "'" & TextBox12.Text.Trim & "'"
        Else
            elementoInsert.Subdivision = "Null"
        End If
        'Vertical=
        If TextBox4.Text.Trim <> "" And IsNumeric(TextBox4.Text.Trim) Then
            elementoInsert.Vertical = TextBox4.Text.Trim.Replace(",", ".")
        Else
            elementoInsert.Vertical = "0"
        End If
        'Horizontal
        If TextBox5.Text.Trim <> "" And IsNumeric(TextBox5.Text.Trim) Then
            elementoInsert.Horizontal = TextBox5.Text.Trim.Replace(",", ".")
        Else
            elementoInsert.Horizontal = "0"
        End If
        'Anejos
        If TextBox15.Text.Trim <> "" Then
            elementoInsert.Anejo = "'" & TextBox15.Text.Trim.Replace("'", "\'") & "'"
        Else
            elementoInsert.Anejo = "Null"
        End If
        If ComboBox5.Text = "SI" Then
            elementoInsert.JuntaEstadistica = "1"
        Else
            elementoInsert.JuntaEstadistica = "0"
        End If
        If TextBox16.Text.Trim <> "" Then
            elementoInsert.Observaciones = "E'" & TextBox16.Text.Trim.Replace("'", "\'") & "'"
        Else
            elementoInsert.Observaciones = "Null"
        End If
        'Provincia
        If ListView1.Items.Count = 0 Then
            MessageBox.Show("El documento debe tener asociado al menos un municipio.",
                            AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Function
        End If
        If ComboBox6.SelectedIndex = -1 Then
            'Cogemos el nombre de la provincia de la lista de municipios
            elementoInsert.ProvinciaRepo = CType(ListView1.Items(0).SubItems(4).Text, Integer)
        Else
            If CType(ComboBox6.SelectedItem, itemData).Valor <> ListView1.Items(0).SubItems(4).Text Then
                MessageBox.Show("La provincia no coincide con la del primer municipio asociado.",
                                AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Function
            End If
            elementoInsert.ProvinciaRepo = CType(ComboBox6.SelectedItem, itemData).Valor
        End If

        'FlagProperties
        elementoInsert.extraProps = 0
        Dim flagPropos As New FlagsProperties()
        If flagPropos.assignByContainer(CheckedListBox1) Then
            elementoInsert.extraProps = flagPropos.propertyCode
        Else
            MessageBox.Show("Propiedades no aceptadas", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Function
        End If

        Dim Result As String
        ObtenerEscalar("SELECT nextval('bdsidschema.archivo_idarchivo_seq')", Result)
        elementoInsert.Indice = IIf(IsNumeric(Result), CType(Result, Integer), 0)
        If elementoInsert.Indice = 0 Then
            MessageBox.Show("No se puede asignar un índice al documento", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Function
        End If
        indiceDoc = elementoInsert

        GenerarCadInsertBase = "INSERT INTO bdsidschema.archivo " &
                "(idarchivo,numdoc,tipodoc_id,estadodoc_id,escala,tomo,procehoja,procecarpeta,subtipo,fechaprincipal," &
                "fechasmodificaciones,signatura,coleccion,subdivision,vertical,horizontal,juntaestadistica," &
                "anejo,observaciones,observestandar_id,provincia_id,fechacreacion) VALUES (" &
                "" & elementoInsert.Indice & "," &
                "'" & elementoInsert.Sellado & "'," &
                "" & elementoInsert.CodTipo & "," &
                "" & elementoInsert.CodEstado & "," &
                "" & elementoInsert.Escala & "," &
                "" & elementoInsert.Tomo & "," &
                "" & IIf(elementoInsert.proceHoja = "-1", "Null", elementoInsert.proceHoja) & "," &
                "" & IIf(elementoInsert.proceCarpeta = "", "Null", "'" & elementoInsert.proceCarpeta & "'") & "," &
                "" & IIf(elementoInsert.subTipoDoc = "", "Null", "E'" & elementoInsert.subTipoDoc.Replace("'", "\'") & "'") & "," &
                "'" & cadFechaDoc & "'," &
                "" & elementoInsert.fechasModificaciones & "," &
                "" & elementoInsert.Signatura & "," &
                "" & elementoInsert.Coleccion & "," &
                "" & elementoInsert.Subdivision & "," &
                "" & elementoInsert.Vertical & "," &
                "" & elementoInsert.Horizontal & "," &
                "" & elementoInsert.JuntaEstadistica & "," &
                "" & elementoInsert.Anejo & "," &
                "" & elementoInsert.Observaciones & "," &
                "" & IIf(elementoInsert.ObservacionesStandard = -1, "Null", elementoInsert.ObservacionesStandard) & "," &
                "" & elementoInsert.ProvinciaRepo & "," &
                "'" & cadFechaCreacion & "')"



    End Function

    Private Function GenerarCadUpdateLoteBase() As String

        Dim CadenaUpdateLote As String
        Dim FechaActualizacion As Date
        Dim cadFechaActualizacion As String

        GenerarCadUpdateLoteBase = ""
        FechaActualizacion = Now
        cadFechaActualizacion = FechaActualizacion.Year &
                        "/" & String.Format("{0:00}", CInt(FechaActualizacion.Month.ToString)) &
                        "/" & String.Format("{0:00}", CInt(FechaActualizacion.Day.ToString)) & " " &
                        String.Format("{0:00}", CInt(FechaActualizacion.Hour.ToString)) & ":" &
                        String.Format("{0:00}", CInt(FechaActualizacion.Minute.ToString)) & ":00.00"

        CadenaUpdateLote = "UPDATE bdsidschema.archivo SET "
        If CheckBox22.Checked And TextBox22.Text.Trim <> "" Then
            If TextBox22.Text.Trim.Length <> 6 Then
                MessageBox.Show("El número de sellado debe tener seis dígitos",
                                AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Function
            End If
            If CType(CType(ComboBox6.SelectedItem, itemData).Valor, Integer) <> CType(TextBox22.Text.Substring(0, 2), Integer) Then
                MessageBox.Show("Los dos primeros dígitos del nº de sellado han de coincidir con el código INE de la provincia",
                                AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Function
            Else
                CheckBox20.Checked = True
            End If


            Dim Resultado As String = ""
            ObtenerEscalar("SELECT idarchivo from bdsidschema.archivo where numdoc='" & TextBox22.Text.Trim & "'",
                            Resultado)
            Application.DoEvents()
            If CType(Resultado, Integer) > 0 Then
                MessageBox.Show("El número de sellado ya existe", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Function
            End If
            CadenaUpdateLote = CadenaUpdateLote & "numdoc='" & TextBox22.Text.Trim & "',"
        End If


        If CheckBox17.Checked And ComboBox1.SelectedIndex <> -1 Then
            CadenaUpdateLote = CadenaUpdateLote & "tipodoc_id=" & CType(ComboBox1.SelectedItem, itemData).Valor & ","
        End If
        If CheckBox2.Checked And ComboBox2.SelectedIndex <> -1 Then
            CadenaUpdateLote = CadenaUpdateLote & "estadodoc_id=" & CType(ComboBox2.SelectedItem, itemData).Valor & ","
        End If


        If CheckBox3.Checked Then
            Dim flagPropos As New FlagsProperties()
            If flagPropos.assignByContainer(CheckedListBox1) Then
                CadenaUpdateLote = CadenaUpdateLote & "extraprops=" & flagPropos.propertyCode & ","
            Else
                MessageBox.Show("Combinación de propiedades no aceptadas", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Function
            End If
        End If

        If CheckBox4.Checked And TextBox4.Text.Trim <> "" Then
            CadenaUpdateLote = CadenaUpdateLote & "vertical=" & CType(TextBox4.Text.Trim.Replace(".", ","), Integer) & ","
        End If
        If CheckBox5.Checked And TextBox5.Text.Trim <> "" Then
            CadenaUpdateLote = CadenaUpdateLote & "horizontal=" & CType(TextBox5.Text.Trim.Replace(".", ","), Integer) & ","
        End If
        If CheckBox6.Checked Then
            If TextBox6.Text.Trim <> "" Then
                CadenaUpdateLote = CadenaUpdateLote & "procecarpeta=E'" & TextBox6.Text.Trim.Replace("'", "\'") & "',"
            Else
                CadenaUpdateLote = CadenaUpdateLote & "procecarpeta=Null,"
            End If
        End If

        'Subtipo de documento
        If CheckBox24.Checked Then
            If TextBox9.Text.Trim <> "" Then
                CadenaUpdateLote = CadenaUpdateLote & "subtipo=E'" & TextBox9.Text.Trim.Replace("'", "\'") & "',"
            Else
                CadenaUpdateLote = CadenaUpdateLote & "subtipo=null,"
            End If
        End If

        If CheckBox7.Checked And TextBox7.Text.Trim <> "" Then
            CadenaUpdateLote = CadenaUpdateLote & "signatura='" & TextBox7.Text.Trim.Replace("'", "\'") & "',"
        End If
        If CheckBox8.Checked And TextBox8.Text.Trim <> "" Then
            CadenaUpdateLote = CadenaUpdateLote & "escala=" & CType(TextBox8.Text.Trim.Replace(".", ","), Integer) & ","
        End If
        If CheckBox9.Checked Then
            Dim fechadoc As Date
            If IsDate(MaskedTextBox1.Text) = True Then
                fechadoc = CType(MaskedTextBox1.Text, Date)
                'MessageBox.Show("Fecha válida")
            Else
                MessageBox.Show("Fecha no válida. Introduzca una fecha correcta", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Function
            End If
            Application.DoEvents()
            CadenaUpdateLote = CadenaUpdateLote & "fechaprincipal='" &
                                fechadoc.Year & "/" &
                                String.Format("{0:00}", CInt(fechadoc.Month.ToString)) & "/" &
                                String.Format("{0:00}", CInt(fechadoc.Day.ToString)) & "',"
        End If
        If CheckBox10.Checked Then
            If TextBox10.Text.Trim <> "" Then
                If IsNumeric(TextBox10.Text.Trim) Then
                    CadenaUpdateLote = CadenaUpdateLote & "procehoja=" & CType(TextBox10.Text.Trim.Replace(".", ","), Integer) & ","
                Else
                    MessageBox.Show("Valor no válido para el campo Hoja", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Function
                End If
            Else
                CadenaUpdateLote = CadenaUpdateLote & "procehoja=Null,"
            End If
        End If
        If CheckBox11.Checked And TextBox11.Text.Trim <> "" Then
            CadenaUpdateLote = CadenaUpdateLote & "tomo='" & TextBox11.Text.Trim.Replace("'", "\'") & "',"
        End If
        If CheckBox12.Checked And TextBox12.Text.Trim <> "" Then
            CadenaUpdateLote = CadenaUpdateLote & "subdivision='" & TextBox12.Text.Trim.Replace("'", "\'") & "',"
        End If
        If CheckBox13.Checked And TextBox13.Text.Trim <> "" Then
            CadenaUpdateLote = CadenaUpdateLote & "fechasmodificaciones='" & TextBox13.Text.Trim.Replace("'", "\'") & "',"
        End If
        If CheckBox14.Checked Then
            CadenaUpdateLote = CadenaUpdateLote & "coleccion='" & TextBox14.Text.Trim.Replace("'", "\'") & "',"
        End If
        If CheckBox15.Checked Then
            CadenaUpdateLote = CadenaUpdateLote & "anejo='" & TextBox15.Text.Trim.Replace("'", "\'") & "',"
        End If
        If CheckBox16.Checked Then
            CadenaUpdateLote = CadenaUpdateLote & "observaciones=E'" & TextBox16.Text.Trim.Replace("'", "\'") & "',"
        End If
        If CheckBox19.Checked And ComboBox5.SelectedIndex <> -1 Then
            CadenaUpdateLote = CadenaUpdateLote & "juntaestadistica=" & CType(ComboBox5.SelectedItem, itemData).Valor & ","
        End If
        If CheckBox20.Checked And ComboBox6.SelectedIndex <> -1 Then
            If CheckBox18.Checked = False Then
                MessageBox.Show("Si realiza cambios en la provincia, debe reasignar municipios.", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Function
            Else
                If CType(ComboBox6.SelectedItem, itemData).Valor <> ListView1.Items(0).SubItems(4).Text Then
                    MessageBox.Show("La provincia no coincide con la del primer municipio asociado.", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Function
                End If
            End If
            CadenaUpdateLote = CadenaUpdateLote & "provincia_id=" & CType(ComboBox6.SelectedItem, itemData).Valor & ","
        End If

        'Si cambia el estado de esta cadena, es que se han definido algunas modificaciones
        If CadenaUpdateLote <> "UPDATE bdsidschema.archivo SET " Then
            CadenaUpdateLote = CadenaUpdateLote & "fechamodificacion='" & cadFechaActualizacion & "' "
        Else
            Exit Function
        End If

        'Ahora que tenemos la base de la cadena de actualización.
        GenerarCadUpdateLoteBase = CadenaUpdateLote

    End Function


    Private Sub MaskedTextBox1_MaskInputRejected(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MaskInputRejectedEventArgs) Handles MaskedTextBox1.MaskInputRejected
        MessageBox.Show("Debe especificar la fecha en formato DD/MM/AAAA")
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

        If Me.Tag = 0 Then
            CrearNuevoElemento(sender, e)
        Else
            ActualizacionLote()
        End If

    End Sub

    Private Sub TogglePanels(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click, Button6.Click

        If sender.name = "Button5" Then
            GroupBox1.Visible = True
            GroupBox2.Visible = False
        Else
            GroupBox1.Visible = False
            GroupBox2.Visible = True
        End If
    End Sub

    Private Sub SelecciónImagen(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                    Handles Button7.Click, Button8.Click

        If sender.name = "Button7" Then
            OpenFileDialog1.Title = "Selecciona imagen resolución alta"
            OpenFileDialog1.Filter = "Archivos de imagen JPG (*.jpg)|*.jpg"
            If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                TextBox2.Text = OpenFileDialog1.FileName
            End If
        ElseIf sender.name = "Button8" Then
            OpenFileDialog1.Title = "Selecciona imagen resolución normal"
            OpenFileDialog1.Filter = "Archivos de imagen JPG (*.jpg)|*.jpg"
            If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                TextBox3.Text = OpenFileDialog1.FileName
            End If
        End If
    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click

        Dim okProc As Boolean

        Try
            Dim file As System.IO.FileStream
            file = System.IO.File.Create(rutaRepo & "\testdummy1234.txt")
            file.Close()
            System.IO.File.Delete(rutaRepoGeorref & "\testdummy1234.txt")
            file = System.IO.File.Create(rutaRepoGeorref & "\testdummy1234.txt")
            file.Close()
            System.IO.File.Delete(rutaRepoGeorref & "\testdummy1234.txt")
        Catch ex As Exception
            MessageBox.Show("No dispone de permiso de escritura en el repositorio." & Environment.NewLine() &
                            "Los cambios requieren eliminación de documentos en el disco.",
                            AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End Try
        If elementsInEdition.resultados.Count > 50 Then
            MessageBox.Show("No se pueden eliminar más de 50 documentos a la vez", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        If MessageBox.Show("¿Desea eliminar " & elementsInEdition.resultados.Count & " documentos y sus imágenes asociadas",
                        AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If
        Application.DoEvents()

        Me.Cursor = Cursors.WaitCursor
        If borrarElementosEnEdicion() Then
            If CheckBox1.Checked Then
                MessageBox.Show("Resultado del simulacro de eliminación en el fichero LOG." & System.Environment.NewLine & "No se han eliminado ni ficheros ni registros.", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Proceso de eliminación terminado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            Me.Cursor = Cursors.Default
            Me.Close()
        Else
            MessageBox.Show("Se han encontrado problemas en el proceso de borrado. Consultar LOG", AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
            Me.Cursor = Cursors.Default
        End If


    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Dim lstImagen As New ArrayList
        Application.DoEvents()
        If elementsInEdition.resultados.Count = 1 Then
            lstImagen.Add(elementsInEdition.resultados(0).rutaFicheroBajaRes)
        Else
            For Each elem As docCartoSEE In elementsInEdition.resultados
                lstImagen.Add(elem.rutaFicheroBajaRes)
            Next
        End If
        If lstImagen.Count > 1 Then
            If ModalQuestion($"Se abrirán {lstImagen.Count}, una por cada fichero. ¿Continuar?") = DialogResult.No Then Exit Sub
        End If
        Try
            For Each imgFile As String In lstImagen
                Process.Start(imgFile)
            Next
        Catch ex As Exception
            ModalError(ex.Message)
        End Try
    End Sub
End Class