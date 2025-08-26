Public Class frmEditCuad

    Public Enum TypeModeEdition As Integer
        CreateDocument = 0
        EditSingleDocument = 1
        EditMUltiDocument = 2
    End Enum

    Property ModeEdition As TypeModeEdition
    Property IDArchivoEdition As Integer = 0

    Dim Autocompletar_municipios As Boolean
    Public ElemEdit As New ArrayList
    'Dim ElemModify() As docSIDCARTO
    Dim elementsInEdition As docCartoSEEquery
    Dim editRegistro As docCuadMTN
    Dim autoCheckFlag As Boolean

    '----------------------------------------------------------------------------------------------------------------------
    'Control del Textbox de búsqueda de territorio
    '----------------------------------------------------------------------------------------------------------------------
#Region "Autoselect de municipios"
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

    Private Sub SeleccionMunicipios(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click, Button2.Click

        If sender.name = "Button1" Or sender.name = "ListBox1" Or sender.name = "TextBox1" Then
            If TextBox1.Text <> "" Then
                If String.IsNullOrEmpty(TextBox1.Tag) Then Exit Sub
                Dim CodigosMuni() As String = TextBox1.Tag.ToString.Split("|")
                If CodigosMuni.Length <> 2 Then Exit Sub
                For Each elem As ListViewItem In ListView1.Items
                    If elem.SubItems(3).Text = CodigosMuni(1) Then
                        ModalExclamation("El territorio ya se encuenytra asociado al documento")
                        Exit Sub
                    End If
                Next
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
        CheckBox18.Checked = True

    End Sub
#End Region

    Private Sub frmEditCuad_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Colocamos elementos
        Me.Size = New Point(1031, 625)
        ListBox1.Location = New Point(TextBox1.Location.X, TextBox1.Location.Y + TextBox1.Height)
        ListBox1.Size = New Point(TextBox1.Width, 150)
        ListBox1.Visible = False

        TabControl1.ImageList = MDIPrincipal.ImageList2

        ListView1.FullRowSelect = True
        ListView1.View = View.Details
        ListView1.Columns.Add("Municipio", 145, HorizontalAlignment.Left)
        ListView1.Columns.Add("Provincia", 80, HorizontalAlignment.Left)
        ListView1.Columns.Add("INE", 0, HorizontalAlignment.Left)
        ListView1.Columns.Add("idMuni", 0, HorizontalAlignment.Left)
        ListView1.Columns.Add("provinciaId", 0, HorizontalAlignment.Left)
        MaskedTextBox1.Mask = "00/00/0000"

        'Cargamos Combos de selección

        Autocompletar_municipios = True
        ToolStripStatusLabel2.Text = ""

        CargarFiltros()

        If ModeEdition = TypeModeEdition.CreateDocument Then
            CreationMode()
        ElseIf ModeEdition = TypeModeEdition.EditSingleDocument Then
            SingleEditionMode()
        ElseIf ModeEdition = TypeModeEdition.EditMUltiDocument Then
            ModalInfo("En dsarrollo")
        End If

        CheckBox21.Visible = usuarioMyApp.permisosLista.isUserISTARI

    End Sub

    'Carga de los Combos de seleccion
    Private Sub CargarFiltros()

        'Carga del Combo de Provincias
        Dim contadorProv As Integer
        Dim Filtros As DataTable


        For Each Provincia As DataRow In ListaProvincias.Select
            contadorProv += 1
            ComboBox4.Items.Add(New itemData(Provincia.ItemArray(1).ToString, contadorProv))
            ComboBox6.Items.Add(New itemData(Provincia.ItemArray(1).ToString, contadorProv))
        Next
        ComboBox4.Items.Add(New itemData("(Todas)", 0))



        Filtros = New DataTable
        If CargarDatatable("Select distinct tipo FROM bdsidschema.archivodocmtn", Filtros) = False Then
            ModalExclamation("No se puede acceder a la tabla de la documentación")
        Else
            ComboBox1.Items.Clear()
            For Each Filtro As DataRow In Filtros.Select
                ComboBox1.Items.Add(New itemData(Filtro.ItemArray(0).ToString, 1))
            Next
        End If
        Filtros.Dispose()
        Filtros = Nothing

        Filtros = New DataTable
        If CargarDatatable("Select distinct subtipo FROM bdsidschema.archivodocmtn", Filtros) = False Then
            ModalExclamation("No se puede acceder a la tabla de la documentación")
        Else
            ComboBox2.Items.Clear()
            For Each Filtro As DataRow In Filtros.Select
                ComboBox2.Items.Add(New itemData(Filtro.ItemArray(0).ToString, 1))
            Next
        End If
        Filtros.Dispose()
        Filtros = Nothing

        'Carga del Combo de Tipos de documento
        ComboBox3.Items.Add("Exacta")
        ComboBox3.Items.Add("Aproximada")
        ComboBox3.Items.Add("Año-Mes")
        ComboBox3.Items.Add("Año")
        ComboBox3.Items.Add("No legible")
        ComboBox3.Items.Add("Desconocida")
        ComboBox3.Items.Add("Entrada en registro")

        ComboBox5.Items.Add("Itinerarios")
        ComboBox5.Items.Add("Perfiles")
        ComboBox5.Items.Add("Itinerarios y perfiles")
        ComboBox5.Items.Add("Poligonación")
        ComboBox5.Items.Add("Sin definir")


        Filtros = New DataTable
        If CargarDatatable("select distinct cuad_tipo FROM bdsidschema.archivodocmtn", Filtros) = False Then
            ModalExclamation("No se puede acceder a la tabla de la documentación")
        Else
            ComboBox7.Items.Clear()
            ComboBox7.Items.Add(New itemData("Sin tipo definido", 1))
            For Each Filtro As DataRow In Filtros.Select
                ComboBox7.Items.Add(New itemData(Filtro.ItemArray(0).ToString, 1))
            Next
        End If
        Filtros.Dispose()
        Filtros = Nothing


        Dim listFragProps As New FlagsPropertiesCuaderno

        For Each propItem In listFragProps.propertyList
            CheckedListBox1.Items.Add(propItem, CheckState.Unchecked)
        Next

    End Sub

    Private Sub PopulateControlsWithDocCuadMTN(docu As docCuadMTN)


        'Relleno los campos con los valores por defecto
        With docu
            TextBox22.Text = .Sellado
            For Each item As itemData In ComboBox1.Items
                If item.Name = .Tipo Then ComboBox1.SelectedIndex = ComboBox1.Items.IndexOf(item)
            Next
            For Each item As itemData In ComboBox2.Items
                If item.Name = .Subtipo Then ComboBox2.SelectedIndex = ComboBox2.Items.IndexOf(item)
            Next
            If Not .FechaDoc Is Nothing Then
                If .FechaDoc.ToString.Length = 10 Then
                    MaskedTextBox1.Text = .FechaDoc.Substring(8, 2) & "/" &
                                            .FechaDoc.Substring(5, 2) & "/" &
                                            .FechaDoc.Substring(0, 4)
                End If
            End If
            For Each item In ComboBox3.Items
                If item = .FechaDocType Then ComboBox3.SelectedItem = item : Exit For
            Next
            TextBox17.Text = .DivZona
            TextBox14.Text = .SubDivType
            TextBox12.Text = .SubDivNum

            For Each item In ComboBox5.Items
                If item = .ItinType Then ComboBox5.SelectedItem = item : Exit For
            Next
            For Each item As itemData In ComboBox6.Items
                Application.DoEvents()
                If item.Valor = .ProvinciaINE Then ComboBox6.SelectedIndex = ComboBox6.Items.IndexOf(item)
            Next

            TextBox8.Text = .ItinNum
            For Each item As itemData In ComboBox7.Items
                If item.Name = .CuadernoType Then ComboBox7.SelectedIndex = ComboBox7.Items.IndexOf(item)
            Next

            TextBox7.Text = .Signatura
            TextBox11.Text = .Tomo
            TextBox4.Text = .Cuaderno
            TextBox15.Text = .Anejos
            TextBox16.Text = .Observaciones
            TextBox20.Text = .AutorEntidad
            TextBox24.Text = .Observador
            TextBox3.Text = .Instrumentos
            TextBox19.Text = .Encabezado

            CheckBox23.Visible = False

            'Rellenamos los items de propiedades
            Dim propPatron As String = .extraProps.propertyPatron.ToString
            For itemCheck As Integer = 0 To CheckedListBox1.Items.Count - 1
                If propPatron.Substring(propPatron.Length - 1 - itemCheck, 1) = "1" Then
                    CheckedListBox1.SetItemChecked(itemCheck, True)
                Else
                    CheckedListBox1.SetItemChecked(itemCheck, False)
                End If
            Next

            Dim elementoLV As ListViewItem
            'Añadimos territorios

            For Each terri As TerritorioBSID In .listaTerritorios

                elementoLV = New ListViewItem
                elementoLV.Text = terri.nombre
                elementoLV.SubItems.Add(DameProvinciaByINE(terri.provinciaINE))
                elementoLV.SubItems.Add(terri.CodMuniHisto)
                elementoLV.SubItems.Add(terri.indice)
                elementoLV.SubItems.Add(terri.provinciaINE)
                ListView1.Items.Add(elementoLV)
                elementoLV = Nothing

            Next


            Try
                If IO.File.Exists(.rutaFicheroThumb) Then
                    Label34.Text = "Este recurso existe en el repositorio"
                    Label34.Tag = .rutaFicheroAltaRes
                    Label34.ForeColor = Color.DarkGreen
                    Button6.Enabled = True
                Else
                    Label34.Text = "Este recurso NO existe en el repositorio"
                    Label34.Tag = ""
                    Label34.ForeColor = Color.Crimson
                    Button6.Enabled = False
                End If
                If IO.File.Exists(.rutaFicheroPDF) Then
                    Label36.Text = "Este recurso existe en el repositorio"
                    Label36.Tag = .rutaFicheroPDF
                    Label36.ForeColor = Color.DarkGreen
                    Button12.Enabled = True
                Else
                    Label36.Text = "Este recurso NO existe en el repositorio"
                    Label36.Tag = ""
                    Label36.ForeColor = Color.Crimson
                    Button12.Enabled = False
                End If
            Catch ex As Exception
                ModalError(ex.Message)
            End Try

        End With

        autoCheckFlag = True


    End Sub

    Sub CreationMode()

        Me.Text = "Nuevo cuaderno interior"
        Button3.Text = "Crear"
        ToolStripStatusLabel1.Text = "Nuevo cuaderno interior"
        Button9.Enabled = False
        Button10.Enabled = False
        Button13.Enabled = True
        Button14.Enabled = False
        Button15.Enabled = False
        Button4.Enabled = True
        Label34.Visible = False
        Label36.Visible = False
        Button6.Enabled = False
        Button12.Enabled = False
        CleanFields()
        For Each ctrl As Control In Me.TabPage1.Controls
            If TypeOf ctrl Is CheckBox Then
                DirectCast(ctrl, CheckBox).Visible = False
            End If
        Next
        For Each ctrl As Control In Me.TabPage2.Controls
            If TypeOf ctrl Is CheckBox Then
                DirectCast(ctrl, CheckBox).Visible = False
            End If
        Next
        For Each ctrl As Control In Me.TabPage3.Controls
            If TypeOf ctrl Is CheckBox Then
                DirectCast(ctrl, CheckBox).Visible = False
            End If
        Next

        CheckBox21.Visible = True
        CheckBox23.Visible = True


    End Sub

    Sub SingleEditionMode()

        'Recibimos un Lote de documentos para actualizar


        Me.Tag = IDArchivoEdition
        editRegistro = New docCuadMTN(IDArchivoEdition)

        If editRegistro.IdarchivodocMTN = 0 Then
            ModalInfo($"No se localiza el documento con IDArchivoEdition = {IDArchivoEdition}")
            Me.Close()
            Exit Sub
        End If

        PopulateControlsWithDocCuadMTN(editRegistro)

        Me.Text = "Edición del documento nº: " & editRegistro.Sellado
        ToolStripStatusLabel1.Text = "Edición del documento."
        CheckBox22.Visible = True
        TextBox22.Visible = True
        Button9.Visible = True
        Button10.Enabled = True

        Button13.Enabled = False
        Button4.Enabled = False
    End Sub

    Sub MultiEditionMode(ByVal indice As Integer)

        Dim cadIndices As String

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
        CheckBox22.Visible = False
        TextBox22.Visible = False
        CheckBox23.Visible = False


    End Sub


    Sub ModoTrabajo(ByVal modo As String, ByVal indice As Integer)

        ModalInfo("Deprecated")

    End Sub

    Private Sub CleanFields()

        ComboBox1.SelectedIndex = -1
        ComboBox2.SelectedIndex = -1
        ComboBox3.SelectedIndex = -1
        ComboBox4.SelectedIndex = -1
        ComboBox5.SelectedIndex = -1
        ComboBox6.SelectedIndex = -1
        ComboBox7.SelectedIndex = 0

        TextBox2.Text = ""
        TextBox23.Text = ""
        ListView1.Items.Clear()
        MaskedTextBox1.Text = ""

        For Each ctrl As Control In Me.TabPage1.Controls
            If TypeOf ctrl Is CheckBox Then
                DirectCast(ctrl, CheckBox).Checked = False
            End If
        Next
        For Each ctrl As Control In Me.TabPage1.Controls
            If TypeOf ctrl Is TextBox Then
                DirectCast(ctrl, TextBox).Text = ""
            End If
        Next
        For Each ctrl As Control In Me.TabPage3.Controls
            If TypeOf ctrl Is CheckBox Then
                DirectCast(ctrl, CheckBox).Checked = False
            End If
        Next
        For Each ctrl As Control In Me.TabPage3.Controls
            If TypeOf ctrl Is TextBox Then
                DirectCast(ctrl, TextBox).Text = ""
            End If
        Next

        For iter As Integer = 0 To CheckedListBox1.Items.Count - 1
            CheckedListBox1.SetItemChecked(iter, False)
        Next

    End Sub

    Private Sub LimpiarCampos(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click

        CleanFields()

    End Sub


    Private Function ActualizacionAutorAndComentarios() As Boolean


        Dim cadUpBase As String = "UPDATE bdsidschema.archivodocmtn SET "
        Dim propsChanged As New ArrayList

        If CheckBox27.Checked Then propsChanged.Add($"encabezado={IIf(TextBox19.Text.Trim = "", "Null", $"E'{TextBox19.Text.Trim.Replace("'", "\'")}'")}")
        If CheckBox28.Checked Then propsChanged.Add($"autor_entidad={IIf(TextBox20.Text.Trim = "", "Null", $"E'{TextBox20.Text.Trim.Replace("'", "\'")}'")}")
        If CheckBox30.Checked Then propsChanged.Add($"observador={IIf(TextBox24.Text.Trim = "", "Null", $"E'{TextBox24.Text.Trim.Replace("'", "\'")}'")}")
        If CheckBox5.Checked Then propsChanged.Add($"instrumentos={IIf(TextBox3.Text.Trim = "", "Null", $"E'{TextBox3.Text.Trim.Replace("'", "\'")}'")}")
        If CheckBox16.Checked Then propsChanged.Add($"observaciones={IIf(TextBox16.Text.Trim = "", "Null", $"E'{TextBox16.Text.Trim.Replace("'", "\'")}'")}")
        If CheckBox3.Checked Then
            Dim flagPropos As New FlagsProperties()
            If flagPropos.assignByContainer(CheckedListBox1) Then propsChanged.Add($"extraprops={flagPropos.propertyCode}")
        End If
        If propsChanged.ToArray.Length = 0 Then
            Exit Function
        End If
        cadUpBase &= $"{String.Join(",", propsChanged.ToArray)} WHERE idarchivodocmtn={editRegistro.IdarchivodocMTN}"
        ActualizacionAutorAndComentarios = ExeSinTran(cadUpBase)

    End Function

    Private Function ActualizacionAtributos() As Boolean


        Dim cadUpBase As String = "UPDATE bdsidschema.archivodocmtn SET "
        Dim propsChanged As New ArrayList
        Dim ListaSQL As New ArrayList

        If editRegistro.IdarchivodocMTN < 1 Then Exit Function

        If CheckBox17.Checked Then propsChanged.Add($"tipo={IIf(ComboBox1.SelectedIndex = -1, "Null", $"E'{ComboBox1.Text}'")}")
        If CheckBox24.Checked Then propsChanged.Add($"subtipo={IIf(ComboBox2.SelectedIndex = -1, "Null", $"E'{ComboBox2.Text}'")}")
        If CheckBox9.Checked Then
            Dim fechaDoc As Date
            Dim cadFechaDoc As String
            Try
                If IsDate(MaskedTextBox1.Text) Then
                    fechaDoc = CType(MaskedTextBox1.Text, Date)
                    cadFechaDoc = $"{fechaDoc.Year}-{String.Format("{0:00}", CInt(fechaDoc.Month.ToString))}-{String.Format("{0:00}", CInt(fechaDoc.Day.ToString))}"
                Else
                    ModalExclamation("Fecha no válida. Introduzca una fecha correcta")
                    Exit Function
                End If

            Catch ex As Exception
                ModalError(ex.Message)
                Exit Function
            End Try
            propsChanged.Add($"fecha='{cadFechaDoc}'")
        End If
        If CheckBox31.Checked Then propsChanged.Add($"nota_fecha={IIf(ComboBox3.SelectedIndex <> -1, $"E'{ComboBox3.Text}'", "null")}")
        If CheckBox25.Checked Then propsChanged.Add($"zona_num={IIf(TextBox17.Text.Trim = "", "Null", $"E'{TextBox17.Text.Trim.Replace("'", "\'")}'")}")
        If CheckBox14.Checked Then propsChanged.Add($"subdivision_tipo={IIf(TextBox14.Text.Trim = "", "Null", $"E'{TextBox14.Text.Trim.Replace("'", "\'")}'")}")
        If CheckBox12.Checked Then propsChanged.Add($"subdivision_num={IIf(TextBox12.Text.Trim = "", "Null", $"E'{TextBox12.Text.Trim.Replace("'", "\'")}'")}")
        If CheckBox19.Checked Then
            propsChanged.Add($"itin_tipo={IIf(ComboBox5.SelectedIndex <> -1, $"E'{ComboBox5.Text}'", "null")}")
            propsChanged.Add($"itin_num={IIf(TextBox8.Text.Trim = "", "Null", $"E'{TextBox8.Text.Trim.Replace("'", "\'")}'")}")
        End If
        If CheckBox2.Checked Then propsChanged.Add($"cuad_tipo={IIf(ComboBox7.SelectedIndex <> -1, $"E'{ComboBox7.Text}'", "null")}")
        If CheckBox4.Checked Then propsChanged.Add($"cuaderno={IIf(TextBox4.Text.Trim = "", "Null", $"E'{TextBox4.Text.Trim.Replace("'", "\'")}'")}")

        If CheckBox20.Checked Then
            If ComboBox6.SelectedIndex = -1 Then
                ModalExclamation("Es necesario seleccionar la provincia para almacenar los documentos")
                Exit Function
            End If
            propsChanged.Add($"codprov={CType(ComboBox6.SelectedItem, itemData).Valor}")
        End If



        If CheckBox7.Checked Then propsChanged.Add($"signatura={IIf(TextBox7.Text.Trim = "", "Null", $"E'{TextBox7.Text.Trim.Replace("'", "\'")}'")}")
        If CheckBox11.Checked Then propsChanged.Add($"tomo={IIf(TextBox11.Text.Trim = "", "Null", $"E'{TextBox11.Text.Trim.Replace("'", "\'")}'")}")
        If CheckBox15.Checked Then propsChanged.Add($"anejos={IIf(TextBox15.Text.Trim = "", "Null", $"E'{TextBox15.Text.Trim.Replace("'", "\'")}'")}")

        If propsChanged.Count > 0 Then
            ListaSQL.Add($"{cadUpBase}{String.Join(",", propsChanged.ToArray)} WHERE idarchivodocmtn={editRegistro.IdarchivodocMTN}")
        End If
        'Territorios
        If CheckBox18.Checked And ListView1.Items.Count > 0 Then
            Dim nuevosNombres As String = ""
            ListaSQL.Add($"DELETE FROM bdsidschema.archivodocmtn2terris WHERE archivodocmtn_id={editRegistro.IdarchivodocMTN}")
            For Each itemLV As ListViewItem In ListView1.Items
                ListaSQL.Add($"INSERT INTO bdsidschema.archivodocmtn2terris (territorio_id,archivodocmtn_id) VALUES ({itemLV.SubItems(3).Text},{editRegistro.IdarchivodocMTN})")
                If nuevosNombres <> "" Then nuevosNombres &= $"/{ itemLV.Text}" : Continue For
                nuevosNombres = itemLV.Text
            Next
            'Ahora añadimos un registro para el Log de Cambio de territorios
            ListaSQL.Add($"INSERT INTO bdsidschema.archivodocmtnlog (archivodocmtn_id, sellado, usuario_update, tabla, tipo_variacion, valor_old, valor_new) 
						VALUES(
                            {editRegistro.IdarchivodocMTN},
                            {editRegistro.Sellado},
                            '{usuarioMyApp.LoginUser}',
                            'archivodocmtn2terris',
                            'Variación territorios',
                            E'{editRegistro.getListaNombresTerritorios("/").Replace("'", "\'")}',
                            E'{nuevosNombres.Replace("'", "\'")}')")







        ElseIf CheckBox18.Checked And ListView1.Items.Count = 0 Then
            ModalExclamation("Debe asociar el documento al menos a un municipio")
            Exit Function
        End If


        ActualizacionAtributos = ExeTran(ListaSQL)

    End Function


    Private Sub ActualizacionLote()

        ModalInfo("Edición en lote no disponible")

        'Si uno de estos campos está activo, compuebo que tengamos permiso
        'de escritura en disco de datos, ya que modificar estos campos
        'seguramente implica mover documentos.

        'If CheckBox17.Checked = True Or CheckBox18.Checked = True Or CheckBox22.Checked = True Then
        '    Try
        '        Dim file As System.IO.FileStream
        '        file = System.IO.File.Create(rutaRepo & "\testdummy1234.txt")
        '        file.Close()
        '        System.IO.File.Delete(rutaRepoGeorref & "\testdummy1234.txt")
        '        file = System.IO.File.Create(rutaRepoGeorref & "\testdummy1234.txt")
        '        file.Close()
        '        System.IO.File.Delete(rutaRepoGeorref & "\testdummy1234.txt")
        '    Catch ex As Exception
        '        MessageBox.Show("No dispone de permiso de escritura en el repositorio." & Environment.NewLine() &
        '                        "Los cambios requieren traslado de documentos en el disco.",
        '                        AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '        Exit Sub
        '    End Try
        'End If

        'Dim cadUpBase As String
        'Dim NuevoMuni As String = ""
        'Dim NuevoTipo As Integer = -1
        ''Generamos la cadena base de la ejecución en lote
        'cadUpBase = GenerarCadUpdateLoteBase()
        'If cadUpBase = "" And CheckBox18.Checked = False Then
        '    MessageBox.Show("No se definieron valores.No se realizará ninguna modificación", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
        '    Exit Sub
        'End If

        'If CheckBox17.Checked = True Then NuevoTipo = CType(CType(ComboBox1.SelectedItem, itemData).Valor, Integer)
        'If CheckBox18.Checked = True Then
        '    If ListView1.Items.Count = 0 Then
        '        MessageBox.Show("No hay municipios asignados.No se realizará ninguna modificación", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
        '        Exit Sub
        '    End If
        '    NuevoMuni = String.Format("{0:0000000}", ListView1.Items(0).SubItems(2).Text)
        'End If

        'Application.DoEvents()
        'Dim ListaSQL As New ArrayList
        'Dim ListaRenFich As New ArrayList
        'Dim Renombre As RenFich

        'For Each item As docCuadMTN In elementsInEdition.resultados
        '    If cadUpBase <> "" Then
        '        'GenerarLOG(cadUpBase & " WHERE idarchivo=" & item.docIndex)
        '        ListaSQL.Add(cadUpBase & " WHERE idarchivo=" & item.docIndex)
        '    End If
        '    'Si hay cambios en el municipio, se borran las asociaciones antiguas 
        '    'y se introducen las nuevas
        '    If CheckBox18.Checked = True And ListView1.Items.Count > 0 Then
        '        If item.ProvinciaRepo <> CType(ListView1.Items(0).SubItems(4).Text, Integer) Then
        '            If item.ProvinciaRepo = 82 And CType(ListView1.Items(0).SubItems(4).Text, Integer) = 28 Then
        '                Application.DoEvents()
        '            Else
        '                If CheckBox20.Checked = False Then
        '                    ModalExclamation("La Provincia del documento debe ser la misma a la que pertenece el primer municipio asignado. No se realizará ninguna modificación")
        '                    Exit Sub
        '                End If
        '                If CType(ComboBox6.SelectedItem, itemData).Valor <> ListView1.Items(0).SubItems(4).Text Then
        '                    ModalExclamation("La Provincia del documento debe ser la misma a la que pertenece el primer municipio asignado. No se realizará ninguna modificación")
        '                    Exit Sub
        '                End If
        '            End If
        '        End If



        '        ListaSQL.Add($"DELETE FROM bdsidschema.archivo2territorios where archivo_id={item.docIndex}")
        '        ListaSQL.Add($"INSERT INTO bdsidschema.archivolog (archivo_id, sellado, usuario_update, tabla, tipo_variacion, valor_old, valor_new) 
        '                         VALUES ({item.docIndex},'{ item.Sellado}','{usuarioMyApp.LoginUser}','archivo2territorios','Desasignación municipio histórico',
        '                         E'{item.muniHistoLiteralConINEHistorico.Replace("'", "\'")}',null)")

        '        For Each itemLV As ListViewItem In ListView1.Items
        '            ListaSQL.Add($"INSERT INTO bdsidschema.archivo2territorios (territorio_id,archivo_id) VALUES ({itemLV.SubItems(3).Text},{item.docIndex})")
        '            ListaSQL.Add($"INSERT INTO bdsidschema.archivolog (archivo_id, sellado, usuario_update, tabla, tipo_variacion, valor_old, valor_new) 
        '                        VALUES ({item.docIndex},'{item.Sellado}','{usuarioMyApp.LoginUser}','archivo2munihisto','Asignación municipio histórico',null,
        '                        E'{itemLV.SubItems(0).Text.Replace("'", "\'")}({itemLV.SubItems(2).Text})')")
        '        Next





        '    End If
        '    'Si se han producido cambios en el tipo de documento o en los municipios o en el número de sellado
        '    'en las ediciones individuales, puede originarse un cambio de 
        '    'Realizamos los renombres necesarios
        '    If CheckBox17.Checked = True Or CheckBox18.Checked = True Or CheckBox20.Checked = True Or CheckBox22.Checked = True Then
        '        Application.DoEvents()
        '        Dim RutasDoc() As String
        '        Erase RutasDoc
        '        'Añadimos a Rutasdoc las rutas con las imágenes JPG si se modifica el númeo de sellado
        '        If CheckBox20.Checked = True Or CheckBox22.Checked = True Then
        '            ReDim Preserve RutasDoc(RutasDoc.Length - 1 + 2)
        '            RutasDoc(RutasDoc.Length - 2) = rutaRepo & "\_Scan400\" & item.FicheroJPG
        '            RutasDoc(RutasDoc.Length - 1) = rutaRepo & "\_Scan250\" & item.FicheroJPG.Replace("\", "250\")
        '        End If
        '        For Each Rutageo As DataRow In item.rcdgeoFiles.Select
        '            If IsNothing(Rutageo) Then Continue For
        '            Application.DoEvents()
        '            'Renombre.NombreAntiguo = Rutageo
        '            'Renombre.NombreNuevo = GenerarScriptRenombreFicheros(item, Rutageo, NuevoTipo, NuevoMuni, TextBox22.Text)
        '            'Si el nombre generado es distinto
        '            If Renombre.NombreAntiguo <> Renombre.NombreNuevo Then
        '                GenerarLOG("Renombrar:" & Renombre.NombreAntiguo & " >> " & Renombre.NombreNuevo)
        '                ListaRenFich.Add(Renombre)
        '            Else
        '                GenerarLOG("Los cambios no afectan al nombre")
        '            End If
        '        Next
        '    End If
        'Next

        'Application.DoEvents()
        'If CheckBox21.Checked = True Then
        '    If MessageBox.Show("Comprueba el log.¿Abrir?", AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
        '        Process.Start(ficheroLogger)
        '    End If
        '    Exit Sub
        'End If

        'Dim Ejecucion As Boolean = False
        'Dim contador As Integer = 0
        'Me.Cursor = Cursors.WaitCursor
        ''Primero realizamos proceso de copia de los antiguos a los nuevos ficheros
        'If ListaRenFich.Count > 0 Then
        '    contador = 0
        '    For Each cambioNombre As RenFich In ListaRenFich
        '        contador = contador + 1
        '        ToolStripStatusLabel2.Text = "Copiando fichero " & contador & " de " & ListaRenFich.Count
        '        Application.DoEvents()
        '        Try
        '            If System.IO.Directory.Exists(SacarDirDeRuta(cambioNombre.NombreNuevo)) = False Then
        '                System.IO.Directory.CreateDirectory(SacarDirDeRuta(cambioNombre.NombreNuevo))
        '            End If
        '            System.IO.File.Copy(cambioNombre.NombreAntiguo, cambioNombre.NombreNuevo, True)
        '            Ejecucion = True
        '        Catch ex As Exception
        '            MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '            Ejecucion = False
        '            Exit For
        '        End Try
        '    Next

        '    'Si la Ejecucion no ha sido satisfactoria, borramos todo lo copiado
        '    If Ejecucion = False Then
        '        contador = 0
        '        For Each cambioNombre As RenFich In ListaRenFich
        '            contador = contador + 1
        '            ToolStripStatusLabel2.Text = "Analizando fichero " & contador & " de " & ListaRenFich.Count
        '            Try
        '                If System.IO.File.Exists(cambioNombre.NombreNuevo) Then System.IO.File.Delete(cambioNombre.NombreNuevo)
        '                Ejecucion = True
        '            Catch ex As Exception
        '                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '                Ejecucion = False
        '                Exit For
        '            End Try
        '        Next
        '        MessageBox.Show("No se realizaron los cambios correctamente", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '        Me.Cursor = Cursors.Default
        '        Exit Sub
        '    End If
        'End If

        ''Si llegamos aquí los renombres se han efectuado correctamente. Ahora ejecutamos la transación
        'Dim cadListaSQL() As String
        'ReDim cadListaSQL(ListaSQL.Count - 1)
        'ListaSQL.CopyTo(cadListaSQL)
        'Application.DoEvents()
        'Ejecucion = ExeTran(cadListaSQL)
        'Application.DoEvents()


        ''Ahora, en función de que se haya realizado bien o mal la transacción, borramos los nuevos o los viejos.
        'If ListaRenFich.Count > 0 Then
        '    If Ejecucion = False Then
        '        contador = 0
        '        For Each cambioNombre As RenFich In ListaRenFich
        '            contador = contador + 1
        '            ToolStripStatusLabel2.Text = "Restaurando fichero " & contador & " de " & ListaRenFich.Count
        '            Try
        '                If System.IO.File.Exists(cambioNombre.NombreNuevo) Then System.IO.File.Delete(cambioNombre.NombreNuevo)
        '                Ejecucion = True
        '            Catch ex As Exception
        '                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '                Ejecucion = False
        '                Exit For
        '            End Try
        '        Next
        '        MessageBox.Show("No se realizaron los cambios correctamente", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '        Exit Sub
        '    Else
        '        contador = 0
        '        For Each cambioNombre As RenFich In ListaRenFich
        '            contador = contador + 1
        '            ToolStripStatusLabel2.Text = "Reubicando fichero " & contador & " de " & ListaRenFich.Count
        '            Try
        '                If System.IO.File.Exists(cambioNombre.NombreAntiguo) Then System.IO.File.Delete(cambioNombre.NombreAntiguo)
        '                Ejecucion = True
        '            Catch ex As Exception
        '                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '                MessageBox.Show("Algunos documentos no se eliminaron de su ubicación original", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '                Ejecucion = False
        '                Exit For
        '            End Try
        '        Next
        '    End If
        'End If

        'Me.Cursor = Cursors.Default
        'If Ejecucion = True Then
        '    MessageBox.Show("Los cambios se realizaron correctamente en la base de datos y en el disco.", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
        'End If


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


    Private Sub UpdateDigitalResources(nuevoDoc As docCuadMTN, Optional showResultMessages As Boolean = False)

        Dim docPDF As String = TextBox23.Text.Trim
        Dim docThumb As String = TextBox2.Text.Trim
        Dim okThumb As Boolean
        Dim okPDF As Boolean

        If docThumb <> "" Then
            If Not IO.File.Exists(docThumb) Then
                If showResultMessages Then
                    If ModalQuestion($"No se localiza el fichero origen:{Environment.NewLine}{docThumb}{Environment.NewLine}¿Continuar?") = DialogResult.No Then Exit Sub
                End If
                docThumb = ""
            End If
        End If
        If docPDF <> "" Then
            If Not IO.File.Exists(docPDF) Then
                If showResultMessages Then
                    If ModalQuestion($"No se localiza el fichero origen:{Environment.NewLine}{docPDF}{Environment.NewLine}¿Continuar?") = DialogResult.No Then Exit Sub
                End If
                docPDF = ""
            End If
        End If

        Me.Cursor = Cursors.WaitCursor

        Try
            Dim selladoFormat As String = String.Format("{0:00000000}", CType(nuevoDoc.Sellado, Integer))
            If docThumb <> "" Then
                ToolStripStatusLabel2.Text = "Copiando imagen miniatura"
                If Not IO.Directory.Exists($"{rutaRepoCI}\_Miniaturas\{String.Format("{0:00}", nuevoDoc.ProvinciaINE)}") Then
                    IO.Directory.CreateDirectory($"{rutaRepoCI}\_Miniaturas\{String.Format("{0:00}", nuevoDoc.ProvinciaINE)}")
                End If
                IO.File.Copy(docThumb, $"{rutaRepoCI}\_Miniaturas\{String.Format("{0:00}", nuevoDoc.ProvinciaINE)}\CMTN{selladoFormat}.jpg", True)
                okThumb = True
            End If

            If docPDF <> "" Then
                ToolStripStatusLabel2.Text = "Copiando documento PDF"
                If Not IO.Directory.Exists($"{rutaRepoCI}\_pdf\{String.Format("{0:00}", nuevoDoc.ProvinciaINE)}") Then
                    IO.Directory.CreateDirectory($"{rutaRepoCI}\_pdf\{String.Format("{0:00}", nuevoDoc.ProvinciaINE)}")
                End If
                IO.File.Copy(docPDF, $"{rutaRepoCI}\_pdf\{String.Format("{0:00}", nuevoDoc.ProvinciaINE)}\CMTN{selladoFormat}.pdf", True)
                okPDF = True
            End If
        Catch ex As Exception
            ModalError($"Se produjo un error al copiar.{Environment.NewLine}{ex.Message}")
        Finally
            Me.Cursor = Cursors.Default
            ToolStripStatusLabel2.Text = "Ficheros actualizados"
            If okThumb Or okPDF Then
                If showResultMessages Then
                    ModalInfo($"{IIf(okThumb = True, "Recurso JPG Miniatura actualizado", "El recurso JPG Miniatura NO se ha actualizado")}{Environment.NewLine}{IIf(okPDF = True, "Recurso PDF actualizado", "El recurso PDF NO se ha actualizado")}")
                End If
            End If
            ToolStripStatusLabel2.Text = ""
        End Try


    End Sub


    Private Sub CrearNuevoCuadernoMTN()


        Dim finalStatus As String
        Dim cadInsBase As String
        Dim NuevoMuni As String = ""
        Dim NuevoTipo As Integer = -1
        Dim ListaSQL As New ArrayList
        Dim ListaRenFich As New ArrayList
        Dim Renombre As RenFich
        Dim Ejecucion As Boolean = False
        Dim contador As Integer = 0



        'Compuebo que tengamos permiso de escritura en disco de datos, ya que modificar estos campos
        'seguramente implica mover documentos.
        If CheckBox23.Checked Then
            If ModalQuestion($"El documento se creará sin documentación digitalizada{Environment.NewLine}La documentación digital puede incorporarse más tarde{Environment.NewLine}¿Desea continuar?") = DialogResult.No Then Exit Sub
        End If

        If Not CheckBox23.Checked Then
            Try
                Dim file As System.IO.FileStream
                file = System.IO.File.Create(rutaRepo & "\testdummy1234.txt")
                file.Close()
                System.IO.File.Delete(rutaRepoGeorref & "\testdummy1234.txt")
                file = System.IO.File.Create(rutaRepoGeorref & "\testdummy1234.txt")
                file.Close()
                System.IO.File.Delete(rutaRepoGeorref & "\testdummy1234.txt")
            Catch ex As Exception
                ModalExclamation($"No dispone de permiso de escritura en el repositorio.{Environment.NewLine}Los cambios requieren traslado de documentos en el disco.")
                Exit Sub
            End Try
        End If



        Dim nuevoDoc As New docCuadMTN
        'Generamos la cadena base de la ejecución en lote
        cadInsBase = ValidarNuevoCuadernoMTN(nuevoDoc)


        '----------------------------------------------------------------------------------------------------------
        If cadInsBase = "" Then Exit Sub
        If CheckBox17.Checked = True Then NuevoTipo = CType(CType(ComboBox1.SelectedItem, itemData).Valor, Integer)
        If CheckBox18.Checked = True Then
            If ListView1.Items.Count = 0 Then
                ModalExclamation("No hay municipios asignados. Es necsario que asigne el documento a un territorio")
                Exit Sub
            End If
            NuevoMuni = String.Format("{0:0000000}", ListView1.Items(0).SubItems(2).Text)
        End If

        ListaSQL.Add(cadInsBase)

        For Each itemLV As ListViewItem In ListView1.Items
            ListaSQL.Add($"INSERT INTO bdsidschema.archivodocmtn2terris (territorio_id,archivodocmtn_id) VALUES ({itemLV.SubItems(3).Text},(SELECT idarchivodocmtn FROM bdsidschema.archivodocmtn WHERE sellado={nuevoDoc.Sellado}))")
        Next

        'Hasta aquí los datos. Ahora vamos por la información gráfica.
        If TextBox2.Text.Trim = "" And TextBox23.Text.Trim = "" Then
            ModalInfo($"No ha asignado ninguna información gráfica.{Environment.NewLine}Puede agregar los recursos digitales más adelante")
        End If


        If CheckBox21.Checked = True Then
            If ModalQuestion("Simulación de carga terminada sin errores. ¿Desea sacar las sentencias SQL en un fichero de texto?") = DialogResult.No Then Exit Sub
            Try
                Using archivo As New System.IO.StreamWriter($"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\simulacion.sql", False, System.Text.Encoding.Default)
                    For Each sentenceSQL As String In ListaSQL
                        archivo.WriteLine(sentenceSQL.Replace(vbCrLf, ""))
                    Next
                End Using

            Catch ex As Exception
                ModalError(ex.Message)
            End Try
            ModalInfo($"Esto ha sido una simulación. No se han cargado datos.{Environment.NewLine}Fichero generado en {Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\simulacion.sql")
            Exit Sub
        End If


        If ModalQuestWriteDatabase("Se va a cargar información en la base dedatos.¿Desea continuar") = DialogResult.No Then Exit Sub

        'Si llegamos aquí los renombres se han efectuado correctamente. Ahora ejecutamos la transacción
        Me.Cursor = Cursors.WaitCursor

        If ExeTran(ListaSQL) Then
            finalStatus = $"El documento se ha creado correctamente en la base de datos{Environment.NewLine}"
            If Not CheckBox23.Checked Then
                Try
                    UpdateDigitalResources(nuevoDoc)
                    If Not IO.File.Exists(nuevoDoc.rutaFicheroPDF) And TextBox23.Text.Trim <> "" Then
                        finalStatus &= $"No se ha podido guardar el documento PDF en el repositorio{Environment.NewLine}"
                    Else
                        finalStatus &= $"El fichero PDF se ha guardado en el repositorio{Environment.NewLine}"
                    End If
                    If Not IO.File.Exists(nuevoDoc.rutaFicheroThumb) And TextBox2.Text.Trim <> "" Then
                        finalStatus &= $"No se ha podido guardar la miniatura JPG en el repositorio{Environment.NewLine}"
                    Else
                        finalStatus &= $"La miniatura JPG se ha guardado en el repositorio{Environment.NewLine}"
                    End If
                Catch ex As Exception
                    ModalError(ex.Message)
                End Try
            End If
            ModalInfo(finalStatus)
        End If
        Me.Cursor = Cursors.Default






    End Sub

    Private Function ValidarNuevoCuadernoMTN(ByRef elementoInsert As docCuadMTN) As String
        'Esto es lo último que escribo
        Dim FechaCreacion As Date
        Dim cadFechaCreacion As String

        ValidarNuevoCuadernoMTN = ""
        FechaCreacion = Now
        cadFechaCreacion = FechaCreacion.Year &
                        "/" & String.Format("{0:00}", CInt(FechaCreacion.Month.ToString)) &
                        "/" & String.Format("{0:00}", CInt(FechaCreacion.Day.ToString)) & " " &
                        String.Format("{0:00}", CInt(FechaCreacion.Hour.ToString)) & ":" &
                        String.Format("{0:00}", CInt(FechaCreacion.Minute.ToString)) & ":00.00"


        If ComboBox6.SelectedItem Is Nothing Then
            ModalExclamation("Es necesario seleccionar la provincia para almacenar los documentos")
            Exit Function
        End If


        'Provincia
        If ListView1.Items.Count = 0 Then
            ModalExclamation("El documento debe tener asociado al menos un municipio.")
            Exit Function
        End If

        Dim Resultado As Integer = 0
        ObtenerEscalar($"SELECT idarchivodocmtn from bdsidschema.archivodocmtn where sellado={TextBox22.Text.Trim}", Resultado)
        If Resultado > 0 Then ModalExclamation($"El número de sellado {TextBox22.Text.Trim} ya existe") : Exit Function
        elementoInsert.Sellado = TextBox22.Text.Trim

        If ComboBox1.SelectedIndex = -1 Then ModalExclamation("Seleccione un tipo de documento.") : Exit Function
        elementoInsert.Tipo = CType(ComboBox1.SelectedItem, itemData).Name

        If ComboBox2.SelectedIndex = -1 Then ModalExclamation("Seleccione un subtipo de documento.") : Exit Function
        elementoInsert.Subtipo = CType(ComboBox2.SelectedItem, itemData).Name


        'Tomo
        If TextBox11.Text.Trim <> "" Then elementoInsert.Tomo = TextBox11.Text.Trim.Replace("'", "\'")


        'FechaPrincipal
        Dim fechaDoc As Date
        Dim cadFechaDoc As String
        If IsDate(MaskedTextBox1.Text) = True Then
            fechaDoc = CType(MaskedTextBox1.Text, Date)
            cadFechaDoc = $"{fechaDoc.Year}-{String.Format("{0:00}", CInt(fechaDoc.Month.ToString))}-{String.Format("{0:00}", CInt(fechaDoc.Day.ToString))}"
        Else
            ModalExclamation("Fecha no válida. Introduzca una fecha correcta")
            Exit Function
        End If
        If ComboBox3.SelectedIndex = -1 Then ModalExclamation("Seleccione la precisión de la fecha.") : Exit Function
        elementoInsert.FechaDocType = ComboBox3.Text


        If ComboBox5.SelectedIndex = -1 Then ModalExclamation("Seleccione si el documento contiene itinerartios o perfiles.") : Exit Function
        elementoInsert.ItinType = ComboBox5.SelectedItem.ToString
        elementoInsert.ItinNum = TextBox8.Text.Trim.Replace(",", ".")

        elementoInsert.DivZona = TextBox17.Text.Trim.Replace(",", ".")
        elementoInsert.SubDivType = TextBox14.Text.Trim.Replace(",", ".")
        elementoInsert.SubDivNum = TextBox12.Text.Trim.Replace(",", ".")
        elementoInsert.CuadernoType = IIf(ComboBox7.SelectedIndex <> -1, ComboBox7.Text, "")
        elementoInsert.Cuaderno = TextBox4.Text.Trim.Replace(",", ".")

        'Signatura
        elementoInsert.Signatura = TextBox7.Text.Trim.Replace("'", "\'")

        'Anejos
        elementoInsert.Anejos = TextBox15.Text.Trim.Replace(",", ".")

        'Provincia repo
        elementoInsert.ProvinciaINE = CType(ComboBox6.SelectedItem, itemData).Valor

        'Encabezado
        elementoInsert.Encabezado = TextBox19.Text.Trim.Replace(",", ".")

        'Autoría entidad
        elementoInsert.AutorEntidad = TextBox20.Text.Trim.Replace(",", ".")

        'Autoría persona
        elementoInsert.Observador = TextBox24.Text.Trim.Replace(",", ".")

        'Autoría persona
        elementoInsert.Instrumentos = TextBox3.Text.Trim.Replace(",", ".")

        'Observaciones
        elementoInsert.Observaciones = TextBox16.Text.Trim.Replace("'", "\'")


        'FlagProperties
        Dim flagPropos As New FlagsProperties()
        If flagPropos.assignByContainer(CheckedListBox1) Then
            elementoInsert.extraProps.propertyCode = flagPropos.propertyCode
            Application.DoEvents()
        Else
            ModalExclamation("Propiedades no aceptadas")
            Exit Function
        End If

        ValidarNuevoCuadernoMTN = $"INSERT INTO bdsidschema.archivodocmtn
                (sellado,tomo,tipo,subtipo,fecha,nota_fecha,pag,zona_num,subdivision_tipo,subdivision_num,cuaderno,cuad_tipo,anejos,
                encabezado,nombre_old,nombre_new,itin_tipo,itin_num,observaciones,ambito,autor_entidad,signatura,create_at,create_by,codprov,observador,instrumentos,extraprops) VALUES (
                {elementoInsert.Sellado},
                {IIf(elementoInsert.Tomo = "", "Null", $"E'{elementoInsert.Tomo}'")},
                {IIf(elementoInsert.Tipo = "", "Null", $"E'{elementoInsert.Tipo}'")},
                {IIf(elementoInsert.Subtipo = "", "Null", $"E'{elementoInsert.Subtipo}'")},
                '{cadFechaDoc}',
                '{elementoInsert.FechaDocType}',
                {elementoInsert.NumPag},
                {IIf(elementoInsert.DivZona = "", "Null", $"E'{elementoInsert.DivZona}'")},
                {IIf(elementoInsert.SubDivType = "", "Null", $"E'{elementoInsert.SubDivType}'")},
                {IIf(elementoInsert.SubDivNum = "", "Null", $"E'{elementoInsert.SubDivNum}'")},
                {IIf(elementoInsert.Cuaderno = "", "Null", $"E'{elementoInsert.Cuaderno}'")},
                {IIf(elementoInsert.CuadernoType = "", "Null", $"E'{elementoInsert.CuadernoType}'")},
                {IIf(elementoInsert.Anejos = "", "Null", $"E'{elementoInsert.Anejos}'")},
                {IIf(elementoInsert.Encabezado = "", "Null", $"E'{elementoInsert.Encabezado}'")},
                {IIf(elementoInsert.NombreOLD = "", "Null", $"E'{elementoInsert.NombreOLD}'")},
                {IIf(elementoInsert.NombreNEW = "", "Null", $"E'{elementoInsert.NombreNEW}'")},
                {IIf(elementoInsert.ItinType = "", "Null", $"E'{elementoInsert.ItinType}'")},
                {IIf(elementoInsert.ItinNum = "", "Null", $"E'{elementoInsert.ItinNum}'")},
                {IIf(elementoInsert.Observaciones = "", "Null", $"E'{elementoInsert.Observaciones}'")},
                {IIf(elementoInsert.Ambito = "", "Null", $"E'{elementoInsert.Ambito}'")},
                {IIf(elementoInsert.AutorEntidad = "", "Null", $"E'{elementoInsert.AutorEntidad}'")},
                {IIf(elementoInsert.Signatura = "", "Null", $"E'{elementoInsert.Signatura}'")},
                now(),
                '{usuarioMyApp.loginUser}',
                {elementoInsert.ProvinciaINE},
                {IIf(elementoInsert.Observador = "", "Null", $"E'{elementoInsert.Observador}'")},
                {IIf(elementoInsert.Instrumentos = "", "Null", $"E'{elementoInsert.Instrumentos}'")},
                {elementoInsert.extraProps.propertyCode})"

        Application.DoEvents()


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

        CadenaUpdateLote = "UPDATE bdsidschema.archivodocmtn SET "
        If CheckBox22.Checked And TextBox22.Text.Trim <> "" Then



            Dim Resultado As String = ""
            ObtenerEscalar($"SELECT idarchivodocmtn from bdsidschema.archivodocmtn where sellado={TextBox22.Text.Trim}", Resultado)
            Application.DoEvents()
            If CType(Resultado, Integer) > 0 Then
                ModalInfo("El número de sellado ya existe")
                Exit Function
            End If
            CadenaUpdateLote = CadenaUpdateLote & "sellado='" & TextBox22.Text.Trim & "',"
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

        If CheckBox7.Checked And TextBox7.Text.Trim <> "" Then
            CadenaUpdateLote = CadenaUpdateLote & "signatura='" & TextBox7.Text.Trim.Replace("'", "\'") & "',"
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

        If CheckBox11.Checked And TextBox11.Text.Trim <> "" Then
            CadenaUpdateLote = CadenaUpdateLote & "tomo='" & TextBox11.Text.Trim.Replace("'", "\'") & "',"
        End If
        If CheckBox12.Checked And TextBox12.Text.Trim <> "" Then
            CadenaUpdateLote = CadenaUpdateLote & "subdivision='" & TextBox12.Text.Trim.Replace("'", "\'") & "',"
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

        If Not usuarioMyApp.permisosLista.EditarDocumentacion Then Exit Sub
        If ModeEdition = TypeModeEdition.CreateDocument Then CrearNuevoCuadernoMTN()
        If ModeEdition = TypeModeEdition.EditSingleDocument Then
            If ModalQuestWriteDatabase("¿Desea actualizar la información del documento?") = DialogResult.No Then Exit Sub
            ActualizacionAtributos()
            ActualizacionAutorAndComentarios()
            UpdateDigitalResources(editRegistro, True)
            ModalInfo("Documento actualizado")
        End If

    End Sub

    Private Sub SelecciónImagen(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click, Button7.Click

        If sender.name = "Button7" Then
            OpenFileDialog1.Title = "Selecciona imagen resolución alta"
            OpenFileDialog1.Filter = "Archivos de imagen JPG (*.jpg)|*.jpg"
            If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                TextBox2.Text = OpenFileDialog1.FileName
            End If
        ElseIf sender.name = "Button5" Then
            OpenFileDialog1.Title = "Selecciona documento PDF"
            OpenFileDialog1.Filter = "Archivos documento PD (*.pdf)|*.pdf"
            If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                TextBox23.Text = OpenFileDialog1.FileName
            End If
        End If
    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click

        Dim okProc As Boolean
        Dim ListaSQL As New ArrayList
        Dim ficherosDelete As New ArrayList

        If Not usuarioMyApp.permisosLista.editarDocumentacion Then Exit Sub
        If ModeEdition <> TypeModeEdition.EditSingleDocument Then Exit Sub
        Try
            Dim file As System.IO.FileStream
            file = System.IO.File.Create(rutaRepo & "\testdummy1234.txt")
            file.Close()
            System.IO.File.Delete(rutaRepoGeorref & "\testdummy1234.txt")
            file = System.IO.File.Create(rutaRepoGeorref & "\testdummy1234.txt")
            file.Close()
            System.IO.File.Delete(rutaRepoGeorref & "\testdummy1234.txt")
        Catch ex As Exception
            ModalError($"No dispone de permiso de escritura en el repositorio.{Environment.NewLine}Los cambios requieren eliminación de documentos en el disco.")
            Exit Sub
        End Try

        ListaSQL.Add($"INSERT INTO bdsidschema.archivodocmtnhisto
                     SELECT now() as fecha_elim,'{usuarioMyApp.loginUser}' as user_delete,archivodocmtn.idarchivodocmtn,archivodocmtn.tipo,archivodocmtn.subtipo,archivodocmtn.tomo,archivodocmtn.sellado,
                                archivodocmtn.codprov,archivodocmtn.fecha,archivodocmtn.nota_fecha,archivodocmtn.pag,archivodocmtn.zona_num,
	                        archivodocmtn.subdivision_tipo,archivodocmtn.extraprops,archivodocmtn.encabezado,archivodocmtn.autor_entidad,
                            archivodocmtn.subdivision_num,archivodocmtn.itin_tipo,archivodocmtn.itin_num,archivodocmtn.cuaderno,archivodocmtn.cuad_tipo, 
	                        archivodocmtn.anejos, archivodocmtn.nombre_old, archivodocmtn.nombre_new,archivodocmtn.signatura,archivodocmtn.observaciones,
	                        archivodocmtn.create_at,archivodocmtn.create_by,archivodocmtn.ambito,archivodocmtn.namefilecdd,archivodocmtn.fechafilecdd,
	                        archivodocmtn.observador,archivodocmtn.instrumentos,provincias.nombreprovincia,
                            string_agg(territorios.idterritorio::text,'|') as idTerris,
                            string_agg(Territorios.Nombre,'|') as nombreTerris,
                            string_agg(Territorios.Tipo,'|') as tipoTerris,
                            string_agg(Territorios.poligono_carto::text,'|') as poligonocarto,
                            string_agg(Territorios.Municipio::text,'|') as muniTerris 
                            FROM bdsidschema.archivodocmtn 
                            LEFT JOIN bdsidschema.provincias on archivodocmtn.codprov= provincias.idprovincia 
                            LEFT JOIN bdsidschema.archivodocmtn2terris ON archivodocmtn.idarchivodocmtn=archivodocmtn2terris.archivodocmtn_id 
                            LEFT JOIN bdsidschema.territorios ON archivodocmtn2terris.territorio_id=territorios.idterritorio  
                            WHERE archivodocmtn.idarchivodocmtn = {editRegistro.IdarchivodocMTN}
                            group by archivodocmtn.idarchivodocmtn,archivodocmtn.tipo,archivodocmtn.subtipo,archivodocmtn.tomo,
				            archivodocmtn.sellado,
                            archivodocmtn.codprov,archivodocmtn.fecha,archivodocmtn.nota_fecha,archivodocmtn.pag,archivodocmtn.zona_num,
				            archivodocmtn.subdivision_tipo,
                            archivodocmtn.subdivision_num,archivodocmtn.cuaderno,archivodocmtn.itin_tipo, archivodocmtn.itin_num, archivodocmtn.cuad_tipo,
				            archivodocmtn.extraprops,
                            archivodocmtn.encabezado,archivodocmtn.autor_entidad,
                            archivodocmtn.anejos, archivodocmtn.nombre_old, archivodocmtn.nombre_new,archivodocmtn.signatura,archivodocmtn.observaciones,
				            archivodocmtn.create_by,archivodocmtn.create_at,
                            archivodocmtn.ambito,archivodocmtn.namefilecdd,archivodocmtn.fechafilecdd,provincias.nombreprovincia,
				            archivodocmtn.instrumentos,archivodocmtn.observador")
        ListaSQL.Add($"DELETE FROM bdsidschema.archivodocmtn2terris WHERE archivodocmtn_id={editRegistro.IdarchivodocMTN}")
        ListaSQL.Add($"DELETE FROM bdsidschema.archivodocmtn WHERE idarchivodocmtn={editRegistro.IdarchivodocMTN}")

        Try
            If IO.File.Exists(editRegistro.rutaFicheroThumb) Then ficherosDelete.Add(editRegistro.rutaFicheroThumb)
            If IO.File.Exists(editRegistro.rutaFicheroPDF) Then ficherosDelete.Add(editRegistro.rutaFicheroPDF)
        Catch ex As Exception
            ModalError(ex.Message)
            Exit Sub
        End Try

        If ModalQuestWriteDatabase($"¿Desea eliminar el documento con sellado nº {editRegistro.Sellado} y su información digital{Environment.NewLine}({ficherosDelete.Count} ficheros?") = DialogResult.No Then Exit Sub

        Me.Cursor = Cursors.WaitCursor

        'Borrado de ficheros
        For Each pathFileResource As String In ficherosDelete
            If IsNothing(pathFileResource) Then Continue For
            Try
                If IO.File.Exists(pathFileResource) Then IO.File.Delete(pathFileResource)
            Catch ex As Exception
                ModalError($"Se han producido errores al eliminar la información gráfica del documento.No se eliminó el documento.{Environment.NewLine}{ex.Message}")
            End Try
        Next

        IIf(ExeTran(ListaSQL) = True, ModalInfo("Proceso de eliminación completado"), ModalExclamation("No se han podido eliminar los documentos. Consultar LOG"))

        Me.Cursor = Cursors.Default
        Me.Close()

    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click

        If Not usuarioMyApp.permisosLista.editarDocumentacion Then Exit Sub
        If ModalQuestion("¿Desea actualizar sólo los recursos del documento?") = DialogResult.No Then
            ModalInfo("No se ha actualizado ningún dato")
            Exit Sub
        End If
        UpdateDigitalResources(editRegistro, True)

    End Sub


    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged, TextBox23.TextChanged

        Dim ctrlSender As Windows.Forms.TextBox
        ctrlSender = sender

        If String.IsNullOrEmpty(ctrlSender.Text) Then Exit Sub

        Try
            If Not IO.File.Exists(sender.Text.Trim) Then
                ErrorProvider1.SetError(sender, "El fichero no se encuentra accesible")
            Else
                ErrorProvider1.SetError(sender, String.Empty)
            End If
        Catch ex As Exception
            ModalError(ex.Message)
        End Try

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click, Button12.Click

        Dim pathResource As String
        Dim ctrlSender As Windows.Forms.Button
        ctrlSender = sender
        If String.IsNullOrEmpty(ctrlSender.Text) Then ModalExclamation("No hay recurso asignado") : Exit Sub

        If ctrlSender.Name = "Button6" Then pathResource = Label34.Tag
        If ctrlSender.Name = "Button12" Then pathResource = Label36.Tag

        Try
            If pathResource = "" Then ModalExclamation("El recurso no se encuentra en el repositorio") : Exit Sub
            If System.IO.File.Exists(pathResource) Then Process.Start(pathResource)
        Catch ex As Exception
            ModalError(ex.Message)
        End Try



    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click

        Dim archivoIdImport As Integer = 0
        Dim docuImport As docCuadMTN
        Dim numSelladoImport As String = InputDialog.InputBox("Introduzca el número del documento que quiere importar", "Documento GEODOCAT", "")

        ObtenerEscalar($"SELECT idarchivodocmtn from bdsidschema.archivodocmtn where sellado={numSelladoImport}", archivoIdImport)
        docuImport = New docCuadMTN(archivoIdImport)

        If docuImport.IdarchivodocMTN = 0 Then
            ModalInfo($"No se localiza el documento con sellado nº {numSelladoImport}")
            Me.Close()
            Exit Sub
        End If

        CleanFields()
        PopulateControlsWithDocCuadMTN(docuImport)
        'Eliminamos los ficheros de recursos, porque estamos importando para crear un nuevo documento y no tiene sentido mantener estos ficheros
        TextBox2.Text = ""
        TextBox23.Text = ""
        TextBox2.Tag = ""
        TextBox23.Tag = ""

    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click

        If Not usuarioMyApp.permisosLista.editarDocumentacion Then Exit Sub
        If ModeEdition <> TypeModeEdition.EditSingleDocument Then Exit Sub
        If ModalQuestWriteDatabase("¿Desea actualizar la información de la sección «Autoría y observaciones»?") = DialogResult.No Then Exit Sub

        Me.Cursor = Cursors.WaitCursor
        If ActualizacionAutorAndComentarios() Then
            ModalInfo("Los datos se han actualizado correctamente.")
        End If
        Me.Cursor = Cursors.Default

    End Sub

    Private Sub TextBox16_TextChanged(sender As Object, e As EventArgs) Handles TextBox16.TextChanged, TextBox19.TextChanged,
                    TextBox20.TextChanged, TextBox24.TextChanged, CheckedListBox1.SelectedIndexChanged, ComboBox5.SelectedIndexChanged, ComboBox1.SelectedIndexChanged, ComboBox2.SelectedIndexChanged, ComboBox3.SelectedIndexChanged, ComboBox6.SelectedIndexChanged, TextBox17.TextChanged,
                    TextBox14.TextChanged, TextBox14.TextChanged, TextBox12.TextChanged, TextBox22.TextChanged, TextBox7.TextChanged, TextBox8.TextChanged, TextBox11.TextChanged, TextBox15.TextChanged, MaskedTextBox1.TextChanged, ComboBox7.SelectedIndexChanged, TextBox4.TextChanged

        If Not autoCheckFlag Then Exit Sub
        If sender.name = "TextBox16" Then CheckBox16.Checked = True
        'If sender.name = "TextBox18" Then CheckBox26.Checked = True
        If sender.name = "TextBox19" Then CheckBox27.Checked = True
        If sender.name = "TextBox20" Then CheckBox28.Checked = True
        'If sender.name = "TextBox21" Then CheckBox29.Checked = True
        If sender.name = "TextBox24" Then CheckBox30.Checked = True
        If sender.name = "ComboBox5" Then CheckBox19.Checked = True
        If sender.name = "CheckedListBox1" Then CheckBox3.Checked = True
        If sender.name = "TextBox9" Then CheckBox24.Checked = True
        If sender.name = "ComboBox1" Then CheckBox17.Checked = True
        If sender.name = "ComboBox2" Then CheckBox24.Checked = True
        If sender.name = "ComboBox3" Then CheckBox31.Checked = True
        If sender.name = "ComboBox6" Then CheckBox20.Checked = True
        If sender.name = "ComboBox7" Then CheckBox2.Checked = True
        If sender.name = "TextBox17" Then CheckBox25.Checked = True
        If sender.name = "TextBox14" Then CheckBox14.Checked = True
        If sender.name = "TextBox14" Then CheckBox14.Checked = True
        If sender.name = "TextBox12" Then CheckBox12.Checked = True
        'If sender.name = "TextBox13" Then CheckBox13.Checked = True
        If sender.name = "TextBox22" Then CheckBox22.Checked = True
        If sender.name = "TextBox4" Then CheckBox4.Checked = True
        'If sender.name = "TextBox5" Then CheckBox5.Checked = True
        If sender.name = "TextBox7" Then CheckBox7.Checked = True
        'If sender.name = "TextBox8" Then CheckBox8.Checked = True
        If sender.name = "TextBox11" Then CheckBox11.Checked = True
        If sender.name = "TextBox15" Then CheckBox15.Checked = True
        If sender.name = "MaskedTextBox1" Then CheckBox9.Checked = True

    End Sub

#Region "Gestión Drag & Drop de los ficheros"

    Private Sub textBoxesEntries(sender As Object, e As DragEventArgs) Handles TextBox2.DragEnter, TextBox23.DragEnter

        e.Effect = DragDropEffects.Link

    End Sub

    Private Sub textBoxesDropping(sender As Object, e As DragEventArgs) Handles TextBox2.DragDrop, TextBox23.DragDrop

        Try
            Dim Rutas As String() = DirectCast(e.Data.GetData(DataFormats.FileDrop), String())
            If Rutas.Length <> 1 Then
                ModalExclamation("Arrastre un único fichero SHP")
                Exit Sub
            End If
            Dim extension As String = System.IO.Path.GetExtension(Rutas(0)).ToLower

            If sender.name = "TextBox2" Or sender.name = "TextBox3" Then
                If extension <> ".jpg" Then ModalExclamation("Arrastre un único fichero JPG") : Exit Sub
            ElseIf sender.name = "TextBox23" Then
                If extension <> ".pdf" Then ModalExclamation("Arrastre un único fichero JPG") : Exit Sub
            Else
                Exit Sub
            End If

            If sender.name = "TextBox2" Then TextBox2.Text = Rutas(0)
            If sender.name = "TextBox23" Then TextBox23.Text = Rutas(0)

        Catch ex As Exception
            ModalError(ex.Message)
        End Try


    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click

        If Not usuarioMyApp.permisosLista.editarDocumentacion Then Exit Sub
        If ModeEdition <> TypeModeEdition.EditSingleDocument Then Exit Sub
        If ModalQuestWriteDatabase("¿Desea actualizar la información de la sección «Atributos GEODOCAT»?") = DialogResult.No Then Exit Sub

        Me.Cursor = Cursors.WaitCursor
        If ActualizacionAtributos() Then
            ModalInfo("Los datos se han actualizado correctamente.")
        End If
        Me.Cursor = Cursors.Default

    End Sub

#End Region

End Class