Public Class frmEdicion

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
    Dim editRegistro As docCartoSEE
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

    Private Sub frmEdicion_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

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
        'CargarFiltros()
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
        For Each Provincia As DataRow In ListaProvincias.Select
            contadorProv += 1
            ComboBox4.Items.Add(New itemData(Provincia.ItemArray(1).ToString, contadorProv))
            ComboBox6.Items.Add(New itemData(Provincia.ItemArray(1).ToString, contadorProv))
        Next
        ComboBox4.Items.Add(New itemData("(Todas)", 0))

        'Carga del Combo de Estado de documentos
        Dim Filtros As DataTable
        Filtros = New DataTable
        If CargarDatatable("Select idestadodoc,estadodoc FROM bdsidschema.tbestadodocumento", Filtros) = False Then
            ModalExclamation("No se puede acceder a la tabla de Estados de la documentación")
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
            ModalExclamation("No se puede acceder a la tabla de Tipos de documentación")
        Else
            ComboBox1.Items.Clear()
            For Each Filtro As DataRow In Filtros.Select
                ComboBox1.Items.Add(New itemData(Filtro.ItemArray(1).ToString, Filtro.ItemArray(0)))
            Next
        End If
        Filtros.Dispose()
        Filtros = Nothing

        ComboBox5.Items.Add(New itemData("No", 0))
        ComboBox5.Items.Add(New itemData("Sí", 1))

        ComboBox3.Items.Add("Exacta")
        ComboBox3.Items.Add("Aproximada")
        ComboBox3.Items.Add("Año-Mes")
        ComboBox3.Items.Add("Año")
        ComboBox3.Items.Add("No legible")
        ComboBox3.Items.Add("Desconocida")

        Dim listFragProps As New FlagsProperties

        For Each propItem In listFragProps.propertyList
            CheckedListBox1.Items.Add(propItem, CheckState.Unchecked)
        Next


    End Sub

    Private Sub PopulateControlsWithDocCartoSEE(docu As docCartoSEE)


        'Relleno los campos con los valores por defecto
        With docu

            TextBox4.Text = .Vertical
            TextBox5.Text = .Horizontal
            TextBox6.Text = .proceCarpeta
            TextBox7.Text = .Signatura
            TextBox8.Text = .Escala
            TextBox9.Text = .subTipoDoc
            TextBox10.Text = .proceHoja
            TextBox11.Text = .Tomo
            TextBox12.Text = .Subdivision
            TextBox13.Text = .fechasModificaciones
            TextBox14.Text = .Coleccion
            TextBox15.Text = .Anejo
            TextBox16.Text = .Observaciones
            TextBox17.Text = .Proyecto
            TextBox18.Text = .Comentarios
            TextBox19.Text = .encabezadoABSYSdoc
            TextBox20.Text = .autorEntidad
            TextBox24.Text = .autorPersona
            TextBox21.Text = .EdificiosCitados
            TextBox22.Text = .Sellado

            For Each item As itemData In ComboBox1.Items
                If item.Name = .Tipo Then ComboBox1.SelectedIndex = ComboBox1.Items.IndexOf(item)
            Next
            For Each item As itemData In ComboBox2.Items
                If item.Name = .Estado Then ComboBox2.SelectedIndex = ComboBox2.Items.IndexOf(item)
            Next

            For Each item As itemData In ComboBox5.Items
                If item.Name = .JuntaEstadistica Then ComboBox5.SelectedIndex = ComboBox5.Items.IndexOf(item)
            Next

            For Each item As String In ComboBox3.Items
                If item = .TipoFechaPrincipal Then ComboBox3.Text = .TipoFechaPrincipal
            Next




            For Each item As itemData In ComboBox6.Items
                Application.DoEvents()
                If item.Valor = .ProvinciaRepo Then ComboBox6.SelectedIndex = ComboBox6.Items.IndexOf(item)
            Next

            If Not .FechaPrincipal Is Nothing Then
                If .FechaPrincipal.ToString.Length = 10 Then
                    MaskedTextBox1.Text = .FechaPrincipal.Substring(8, 2) & "/" &
                                            .FechaPrincipal.Substring(5, 2) & "/" &
                                            .FechaPrincipal.Substring(0, 4)
                End If
            End If
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
                If IO.File.Exists(.rutaFicheroAltaRes) Then
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
                If IO.File.Exists(.rutaFicheroBajaRes) Then
                    Label35.Text = "Este recurso existe en el repositorio"
                    Label35.Tag = .rutaFicheroBajaRes
                    Label35.ForeColor = Color.DarkGreen
                    Button11.Enabled = True
                Else
                    Label35.Text = "Este recurso NO existe en el repositorio"
                    Label35.Tag = ""
                    Label35.ForeColor = Color.Crimson
                    Button11.Enabled = False
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

        Me.Text = "Crear nuevo documento"
        Button3.Text = "Crear"
        ToolStripStatusLabel1.Text = "Crear nuevo documento."
        Button9.Enabled = False
        Button10.Enabled = False
        Button13.Enabled = True
        Button14.Enabled = False
        Button15.Enabled = False
        Button4.Enabled = True
        CheckBox23.Visible = True
        Label34.Visible = False
        Label35.Visible = False
        Label36.Visible = False
        Button6.Enabled = False
        Button11.Enabled = False
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

    End Sub

    Sub SingleEditionMode()

        'Recibimos un Lote de documentos para actualizar


        Me.Tag = IDArchivoEdition
        editRegistro = New docCartoSEE(IDArchivoEdition)
        editRegistro.getGeoFiles()

        If editRegistro.docIndex = 0 Then
            ModalInfo($"No se localiza el documento con idarchivo = {IDArchivoEdition}")
            Me.Close()
            Exit Sub
        End If

        PopulateControlsWithDocCartoSEE(editRegistro)

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
        TextBox2.Text = ""
        TextBox3.Text = ""
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


        Dim cadUpBase As String = "UPDATE bdsidschema.archivo SET "
        Dim propsChanged As New ArrayList

        If CheckBox19.Checked Then propsChanged.Add($"juntaestadistica={IIf(ComboBox5.Text = "Sí", 1, 0)}")
        If CheckBox27.Checked Then propsChanged.Add($"encabezado={IIf(TextBox19.Text.Trim = "", "Null", $"E'{TextBox19.Text.Trim.Replace("'", "\'")}'")}")
        If CheckBox28.Checked Then propsChanged.Add($"autor={IIf(TextBox20.Text.Trim = "", "Null", $"E'{TextBox20.Text.Trim.Replace("'", "\'")}'")}")
        If CheckBox29.Checked Then propsChanged.Add($"nombreedificio={IIf(TextBox21.Text.Trim = "", "Null", $"E'{TextBox21.Text.Trim.Replace("'", "\'")}'")}")
        If CheckBox30.Checked Then propsChanged.Add($"autor_persona={IIf(TextBox24.Text.Trim = "", "Null", $"E'{TextBox24.Text.Trim.Replace("'", "\'")}'")}")
        If CheckBox16.Checked Then propsChanged.Add($"observaciones={IIf(TextBox16.Text.Trim = "", "Null", $"E'{TextBox16.Text.Trim.Replace("'", "\'")}'")}")
        If CheckBox29.Checked Then propsChanged.Add($"observ={IIf(TextBox18.Text.Trim = "", "Null", $"E'{TextBox18.Text.Trim.Replace("'", "\'")}'")}")
        If CheckBox3.Checked Then
            Dim flagPropos As New FlagsProperties()
            If flagPropos.assignByContainer(CheckedListBox1) Then propsChanged.Add($"extraprops={flagPropos.propertyCode}")
        End If

        cadUpBase &= $"{String.Join(",", propsChanged.ToArray)} WHERE idarchivo={editRegistro.docIndex}"
        ActualizacionAutorAndComentarios = ExeSinTran(cadUpBase)

    End Function

    Private Function ActualizacionAtributos() As Boolean


        Dim cadUpBase As String = "UPDATE bdsidschema.archivo SET "
        Dim propsChanged As New ArrayList
        Dim ListaSQL As New ArrayList
        If editRegistro.docIndex < 1 Then Exit Function

        If CheckBox24.Checked Then propsChanged.Add($"subtipo={IIf(TextBox9.Text.Trim = "", "Null", $"E'{TextBox9.Text.Trim.Replace("'", "\'")}'")}")
        If CheckBox25.Checked Then propsChanged.Add($"proyecto={IIf(TextBox17.Text.Trim = "", "Null", $"E'{TextBox17.Text.Trim.Replace("'", "\'")}'")}")
        If CheckBox14.Checked Then propsChanged.Add($"coleccion={IIf(TextBox14.Text.Trim = "", "Null", $"E'{TextBox14.Text.Trim.Replace("'", "\'")}'")}")
        If CheckBox12.Checked Then propsChanged.Add($"subdivision={IIf(TextBox12.Text.Trim = "", "Null", $"E'{TextBox12.Text.Trim.Replace("'", "\'")}'")}")
        If CheckBox13.Checked Then propsChanged.Add($"fechamodificacion={IIf(TextBox13.Text.Trim = "", "Null", $"E'{TextBox13.Text.Trim.Replace("'", "\'")}'")}")
        If CheckBox7.Checked Then propsChanged.Add($"signatura={IIf(TextBox7.Text.Trim = "", "Null", $"E'{TextBox7.Text.Trim.Replace("'", "\'")}'")}")
        If CheckBox11.Checked Then propsChanged.Add($"tomo={IIf(TextBox11.Text.Trim = "", "Null", $"E'{TextBox11.Text.Trim.Replace("'", "\'")}'")}")
        If CheckBox15.Checked Then propsChanged.Add($"anejo={IIf(TextBox15.Text.Trim = "", "Null", $"E'{TextBox15.Text.Trim.Replace("'", "\'")}'")}")
        If CheckBox6.Checked Then propsChanged.Add($"procecarpeta={IIf(TextBox6.Text.Trim = "", "Null", $"E'{TextBox6.Text.Trim.Replace("'", "\'")}'")}")
        If CheckBox31.Checked Then propsChanged.Add($"tipo_fechaprincipal={IIf(ComboBox3.SelectedIndex <> -1, $"E'{ComboBox3.Text}'", "null")}")


        If CheckBox31.Checked Then propsChanged.Add($"provincia_id={IIf(ComboBox3.SelectedIndex <> -1, $"E'{ComboBox3.Text}'", "null")}")

        If CheckBox20.Checked Then
            If ComboBox6.SelectedIndex = -1 Then
                ModalExclamation("Es necesario seleccionar la provincia para almacenar los documentos")
                Exit Function
            End If
            If CType(ComboBox6.SelectedItem, itemData).Valor <> CType(TextBox22.Text.Substring(0, 2), Integer) Then
                If ModalQuestion("Los dos primeros dígitos del número de sellado no coinciden con el código de provincia. ¿Continuar?") = Windows.Forms.DialogResult.No Then Exit Function
            End If
            propsChanged.Add($"provincia_id={CType(ComboBox6.SelectedItem, itemData).Valor}")
        End If

        If CheckBox17.Checked Then
            If ComboBox1.SelectedIndex = -1 Then
                ModalExclamation("Es necesario asociar un tipo de documento")
                Exit Function
            End If
            propsChanged.Add($"tipodoc_id={CType(ComboBox1.SelectedItem, itemData).Valor}")
        End If

        If CheckBox2.Checked Then
            If ComboBox2.SelectedIndex = -1 Then
                ModalExclamation("Es necesario asociar un estado de conservación al documento")
                Exit Function
            End If
            propsChanged.Add($"estadodoc_id={CType(ComboBox2.SelectedItem, itemData).Valor}")
        End If
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
            propsChanged.Add($"fechaprincipal='{cadFechaDoc}'")
        End If

        'Escala
        If CheckBox8.Checked Then propsChanged.Add($"escala={IIf(TextBox8.Text <> "", TextBox8.Text.Trim, 0)}")
        If CheckBox10.Checked Then propsChanged.Add($"procehoja={IIf(TextBox10.Text <> "", TextBox10.Text.Trim, 0)}")

        If CheckBox4.Checked Then propsChanged.Add($"vertical={IIf(TextBox4.Text <> "", Replace(TextBox4.Text, ",", "."), 0)}")
        If CheckBox5.Checked Then propsChanged.Add($"horizontal={IIf(TextBox5.Text <> "", Replace(TextBox5.Text, ",", "."), 0)}")


        ListaSQL.Add($"{cadUpBase}{String.Join(",", propsChanged.ToArray)} WHERE idarchivo={editRegistro.docIndex}")

        'Territorios
        If CheckBox18.Checked And ListView1.Items.Count > 0 Then
            ListaSQL.Add($"DELETE FROM bdsidschema.archivo2territorios WHERE archivo_id={editRegistro.docIndex}")
            For Each itemLV As ListViewItem In ListView1.Items
                Application.DoEvents()
                ListaSQL.Add($"INSERT INTO bdsidschema.archivo2territorios (territorio_id,archivo_id) VALUES ({itemLV.SubItems(3).Text},{editRegistro.docIndex})")
            Next
        ElseIf CheckBox18.Checked And ListView1.Items.Count = 0 Then
            ModalExclamation("Debe asociar el documento al menos a un municipio")
            Exit Function
        End If


        ActualizacionAtributos = ExeTran(ListaSQL)

    End Function





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
                MessageBox.Show("No dispone de permiso de escritura en el repositorio." & Environment.NewLine() &
                                "Los cambios requieren traslado de documentos en el disco.",
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


    Private Sub UpdateDigitalResources(nuevoDoc As docCartoSEE)

        Dim docJPGAlta As String = TextBox2.Text.Trim
        Dim docJPGBaja As String = TextBox3.Text.Trim
        Dim docPDF As String = TextBox23.Text.Trim
        Dim okAlta As Boolean
        Dim okBaja As Boolean
        Dim okPDF As Boolean

        If docJPGAlta <> "" Then
            If Not System.IO.File.Exists(docJPGAlta) Then
                If ModalQuestion($"No se localiza el fichero origen:{Environment.NewLine}{docJPGAlta}{Environment.NewLine}¿Continuar?") = DialogResult.No Then Exit Sub
                docJPGAlta = ""
            End If
        End If
        If docJPGBaja <> "" Then
            If Not System.IO.File.Exists(docJPGBaja) Then
                If ModalQuestion($"No se localiza el fichero origen:{Environment.NewLine}{docJPGBaja}{Environment.NewLine}¿Continuar?") = DialogResult.No Then Exit Sub
                docJPGBaja = ""
            End If
        End If
        If docPDF <> "" Then
            If Not System.IO.File.Exists(docPDF) Then
                If ModalQuestion($"No se localiza el fichero origen:{Environment.NewLine}{docPDF}{Environment.NewLine}¿Continuar?") = DialogResult.No Then Exit Sub
                docPDF = ""
            End If
        End If

        Me.Cursor = Cursors.WaitCursor

        Try
            If docJPGAlta <> "" Then
                ToolStripStatusLabel2.Text = "Copiando imagen calidad alta"
                IO.File.Copy(docJPGAlta, $"{rutaRepo}\_Scan400\{DirRepoProvinciaByINE(nuevoDoc.ProvinciaRepo)}\{nuevoDoc.Sellado}.jpg", True)
                okAlta = True
            End If
            If docJPGBaja <> "" Then
                ToolStripStatusLabel2.Text = "Copiando imagen calidad normal"
                IO.File.Copy(docJPGAlta, $"{rutaRepo}\_Scan250\{DirRepoProvinciaByINE(nuevoDoc.ProvinciaRepo)}250\{nuevoDoc.Sellado}.jpg", True)
                okBaja = True
            End If
            If docPDF <> "" Then
                ToolStripStatusLabel2.Text = "Copiando documento PDF"
                If Not IO.Directory.Exists($"{rutaRepo}\_pdf\{DirRepoProvinciaByINE(nuevoDoc.ProvinciaRepo)}") Then
                    IO.Directory.CreateDirectory($"{rutaRepo}\_pdf\{DirRepoProvinciaByINE(nuevoDoc.ProvinciaRepo)}")
                    okPDF = True
                End If
                Dim selladoFormat As String = String.Format("{0:00000000}", CType(nuevoDoc.Sellado, Integer))

                IO.File.Copy(docPDF, $"{rutaRepo}\_pdf\{String.Format("{0:00}", nuevoDoc.ProvinciaRepo)}\{nuevoDoc.tipoDocumento.prefijoNombreCDD}{selladoFormat}.pdf", True)

            End If
        Catch ex As Exception
            ModalError($"Se produjo un error al copiar.{Environment.NewLine}{ex.Message}")
        Finally
            Me.Cursor = Cursors.Default
            ToolStripStatusLabel2.Text = "Ficheros actualizados"
            If okAlta Or okBaja Or okPDF Then
                ModalInfo($"{IIf(okAlta = True, "Recurso JPG Alta actualizado", "El recurso JPG Alta NO se ha actualizado")}{Environment.NewLine}{IIf(okBaja = True, "Recurso JPG Baja actualizado", "El recurso JPG Baja NO se ha actualizado")}{Environment.NewLine}{IIf(okPDF = True, "Recurso PDF actualizado", "El recurso PDF NO se ha actualizado")}")
            End If
        End Try


    End Sub


    Private Sub CrearNuevoElemento()

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


        Dim cadInsBase As String
        Dim NuevoMuni As String = ""
        Dim NuevoTipo As Integer = -1
        Dim nuevoDoc As New docCartoSEE
        'Generamos la cadena base de la ejecución en lote
        cadInsBase = ValidarNuevoDocumentoGEODOCAT(nuevoDoc)


        '----------------------------------------------------------------------------------------------------------
        If cadInsBase = "" Then Exit Sub
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

        ListaSQL.Add(cadInsBase)
        'Si hay cambios en el municipio, se borran las asociaciones antiguas 
        'y se introducen las nuevas

        For Each itemLV As ListViewItem In ListView1.Items
            Application.DoEvents()
            ListaSQL.Add("INSERT INTO bdsidschema.archivo2territorios (territorio_id,archivo_id) " &
                        "VALUES (" & itemLV.SubItems(3).Text & "," & nuevoDoc.docIndex & ")")
        Next

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

        'Hasta aquí los datos. Ahora vamos por la información gráfica.
        If CheckBox23.Checked Then
            If ModalQuestion("¿Desea continuar sin agregar información gráfica?") = DialogResult.No Then Exit Sub
        Else
            UpdateDigitalResources(nuevoDoc)
        End If

        Me.Cursor = Cursors.Default
        Dim Ejecucion As Boolean = False
        Dim contador As Integer = 0

        If ModalQuestion("Se va a cargar información en la base dedatos.¿Desea continuar") = DialogResult.No Then Exit Sub

        Me.Cursor = Cursors.WaitCursor

        'Si llegamos aquí los renombres se han efectuado correctamente. Ahora ejecutamos la transacción
        Me.Cursor = Cursors.WaitCursor
        If ExeTran(ListaSQL) Then
            ModalInfo("El documento se ha cargado correctamente en la base de datos.")
        End If
        Me.Cursor = Cursors.Default

        'Ahora copiamos los documentos digitales al repositorio
        If CheckBox23.Checked Then
            ModalInfo($"No se han añadido documentos digitalizados al repositorio.{Environment.NewLine}Pueden añadirse manualmente en cualquier momento o editanto el documento.")
        End If

    End Sub

    Private Function ValidarNuevoDocumentoGEODOCAT(ByRef elementoInsert As docCartoSEE) As String

        Dim FechaCreacion As Date
        Dim cadFechaCreacion As String

        ValidarNuevoDocumentoGEODOCAT = ""
        FechaCreacion = Now
        cadFechaCreacion = FechaCreacion.Year &
                        "/" & String.Format("{0:00}", CInt(FechaCreacion.Month.ToString)) &
                        "/" & String.Format("{0:00}", CInt(FechaCreacion.Day.ToString)) & " " &
                        String.Format("{0:00}", CInt(FechaCreacion.Hour.ToString)) & ":" &
                        String.Format("{0:00}", CInt(FechaCreacion.Minute.ToString)) & ":00.00"

        If TextBox22.Text.Trim.Length <> 6 Then
            ModalExclamation("El número de sellado debe tener seis dígitos")
            Exit Function
        End If
        If CType(TextBox22.Text.Substring(0, 2), Integer) = 82 Then
            If CType(ComboBox6.SelectedItem, itemData).Valor <> 28 Then
                ModalExclamation("Los documentos con sellado 82nnnn pertenecen a la provincia de Madrid")
                Exit Function
            End If
        End If
        If CType(TextBox22.Text.Substring(0, 2), Integer) = 88 Then
            If CType(ComboBox6.SelectedItem, itemData).Valor <> 28 Then
                ModalExclamation("Los documentos con sellado 88nnnn pertenecen a la provincia de Madrid")
                Exit Function
            End If
        End If

        If ComboBox6.SelectedItem Is Nothing Then
            ModalExclamation("Es necesario seleccionar la provincia para almacenar los documentos")
            Exit Function
        End If

        If CType(ComboBox6.SelectedItem, itemData).Valor <> CType(TextBox22.Text.Substring(0, 2), Integer) Then
            If ModalQuestion("Los dos primeros dígitos del número de sellado no coinciden con el código de provincia. ¿Continuar?") = Windows.Forms.DialogResult.No Then Exit Function
        End If

        'Provincia
        If ListView1.Items.Count = 0 Then
            ModalExclamation("El documento debe tener asociado al menos un municipio.")
            Exit Function
        End If

        If CType(ComboBox6.SelectedItem, itemData).Valor <> ListView1.Items(0).SubItems(4).Text Then
            If ModalQuestion("La provincia no coincide con la del primer municipio asociado.¿Continuar?") = DialogResult.No Then Exit Function
        End If

        Dim Resultado As Integer = 0
        ObtenerEscalar($"SELECT idarchivo from bdsidschema.archivo where numdoc='{TextBox22.Text.Trim}'", Resultado)
        If Resultado > 0 Then ModalExclamation($"El número de sellado {TextBox22.Text.Trim} ya existe en GEODOCAT") : Exit Function
        elementoInsert.Sellado = TextBox22.Text.Trim

        If ComboBox1.SelectedIndex = -1 Then ModalExclamation("Seleccione un tipo de documento.") : Exit Function
        elementoInsert.CodTipo = CType(ComboBox1.SelectedItem, itemData).Valor

        For Each subItem As docCartoSEETipoDocu In tiposDocSIDCARTO
            If subItem.idTipodoc = elementoInsert.CodTipo Then
                elementoInsert.tipoDocumento = subItem
                Exit For
            End If
        Next

        If ComboBox2.SelectedIndex = -1 Then ModalExclamation("Asocie un estado de conservación al documento.") : Exit Function
        elementoInsert.CodEstado = CType(ComboBox2.SelectedItem, itemData).Valor

        'Escala
        elementoInsert.Escala = IIf(CType(TextBox8.Text.Trim, Integer) > 0, TextBox8.Text.Trim, "0").Replace("'", "\'")

        'Tomo
        If TextBox11.Text.Trim <> "" Then elementoInsert.Tomo = TextBox11.Text.Trim.Replace("'", "\'")

        'ProceHoja
        If TextBox10.Text.Trim <> "" Then
            If IsNumeric(TextBox10.Text.Trim) Then
                elementoInsert.proceHoja = TextBox10.Text.Trim
            Else
                ModalExclamation("El campo procedimiento por Hoja debe de ser un número")
                Exit Function
            End If
        End If
        'ProceCarpeta
        If TextBox6.Text.Trim <> "" Then elementoInsert.proceCarpeta = TextBox6.Text.Trim.Replace("'", "\'")

        'Subtipo de documento
        If TextBox9.Text.Trim <> "" Then elementoInsert.subTipoDoc = TextBox9.Text.Trim.Replace("'", "\'")

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
        elementoInsert.TipoFechaPrincipal = ComboBox3.Text

        'Fechas Modificaciones
        If TextBox13.Text.Trim <> "" Then elementoInsert.fechasModificaciones = TextBox13.Text.Trim.Replace("'", "\'")

        'Signatura
        If TextBox7.Text.Trim <> "" Then elementoInsert.Signatura = TextBox7.Text.Trim.Replace("'", "\'")

        'Coleccion
        If TextBox14.Text.Trim <> "" Then elementoInsert.Coleccion = TextBox14.Text.Trim.Replace("'", "\'")

        'Subdivisión
        If TextBox12.Text.Trim <> "" Then elementoInsert.Subdivision = TextBox12.Text.Trim.Replace("'", "\'")

        'Proyecto
        elementoInsert.Proyecto = IIf(TextBox17.Text.Trim <> "", TextBox17.Text.Trim.Replace(",", "."), "")

        'Vertical
        elementoInsert.Vertical = IIf(TextBox4.Text.Trim <> "" And IsNumeric(TextBox4.Text.Trim), TextBox4.Text.Trim.Replace(",", "."), "0")

        'Horizontal
        elementoInsert.Horizontal = IIf(TextBox5.Text.Trim <> "" And IsNumeric(TextBox5.Text.Trim), TextBox5.Text.Trim.Replace(",", "."), "0")

        'Anejos
        elementoInsert.Anejo = IIf(TextBox15.Text.Trim <> "", TextBox15.Text.Trim.Replace(",", "."), "")

        'Es Junta Estadística
        elementoInsert.JuntaEstadistica = IIf(ComboBox5.Text = "SI", "1", "0")

        'Observaciones
        elementoInsert.Observaciones = IIf(TextBox16.Text.Trim <> "", TextBox16.Text.Trim.Replace("'", "\'"), "")

        'Comentarios ABSYS
        elementoInsert.Comentarios = IIf(TextBox18.Text.Trim <> "", TextBox18.Text.Trim.Replace("'", "\'"), "")

        'Provincia repo
        elementoInsert.ProvinciaRepo = CType(ComboBox6.SelectedItem, itemData).Valor

        'Encabezado
        elementoInsert.encabezadoABSYSdoc = IIf(TextBox19.Text.Trim <> "", TextBox19.Text.Trim.Replace(",", "."), "")

        'Autoría entidad
        elementoInsert.autorEntidad = IIf(TextBox20.Text.Trim <> "", TextBox20.Text.Trim.Replace(",", "."), "")

        'Autoría persona
        elementoInsert.autorPersona = IIf(TextBox24.Text.Trim <> "", TextBox24.Text.Trim.Replace(",", "."), "")

        'Edificios citados
        elementoInsert.EdificiosCitados = IIf(TextBox21.Text.Trim <> "", TextBox21.Text.Trim.Replace(",", "."), "")

        'FlagProperties
        'elementoInsert.extraProps.propertyCode = 0

        Application.DoEvents()
        Dim flagPropos As New FlagsProperties()
        If flagPropos.assignByContainer(CheckedListBox1) Then
            elementoInsert.extraProps.propertyCode = flagPropos.propertyCode
            Application.DoEvents()
        Else
            ModalExclamation("Propiedades no aceptadas")
            Exit Function
        End If

        Dim Result As String
        ObtenerEscalar("SELECT nextval('bdsidschema.archivo_idarchivo_seq')", Result)
        elementoInsert.docIndex = IIf(IsNumeric(Result), CType(Result, Integer), 0)
        If elementoInsert.docIndex = 0 Then
            ModalExclamation("No se puede asignar un índice al documento")
            Exit Function
        End If

        ValidarNuevoDocumentoGEODOCAT = $"INSERT INTO bdsidschema.archivo 
                (idarchivo,numdoc,user_create,tipodoc_id,estadodoc_id,escala,tomo,procehoja,procecarpeta,subtipo,fechaprincipal,tipo_fechaprincipal,
                fechasmodificaciones,signatura,coleccion,subdivision,proyecto,vertical,horizontal,juntaestadistica,encabezado,autor,autor_persona,nombreedificio,
                anejo,observaciones,observ,extraprops,provincia_id) VALUES (
                {elementoInsert.docIndex},
                '{elementoInsert.Sellado}',
                '{usuarioMyApp.loginUser}',
                {elementoInsert.CodTipo},
                {elementoInsert.CodEstado},
                {elementoInsert.Escala},
                E'{elementoInsert.Tomo}',
                {IIf(elementoInsert.proceHoja = "", "Null", elementoInsert.proceHoja)},
                {IIf(elementoInsert.proceCarpeta = "", "Null", $"E'{elementoInsert.proceCarpeta}'")},
                {IIf(elementoInsert.subTipoDoc = "", "Null", $"E'{elementoInsert.subTipoDoc}'")},
                '{cadFechaDoc}',
                '{elementoInsert.TipoFechaPrincipal}',
                '{elementoInsert.fechasModificaciones}',
                {IIf(elementoInsert.Signatura = "", "Null", $"E'{elementoInsert.Signatura}'")},
                {IIf(elementoInsert.Coleccion = "", "Null", $"E'{elementoInsert.Coleccion}'")},
                {IIf(elementoInsert.Subdivision = "", "Null", $"E'{elementoInsert.Subdivision}'")},
                {IIf(elementoInsert.Proyecto = "", "Null", $"E'{elementoInsert.Proyecto}'")},
                {elementoInsert.Vertical},
                {elementoInsert.Horizontal},
                {elementoInsert.JuntaEstadistica},
                {IIf(elementoInsert.encabezadoABSYSdoc = "", "Null", $"E'{elementoInsert.encabezadoABSYSdoc}'")},
                {IIf(elementoInsert.autorEntidad = "", "Null", $"E'{elementoInsert.autorEntidad}'")},
                {IIf(elementoInsert.autorPersona = "", "Null", $"E'{elementoInsert.autorPersona}'")},
                {IIf(elementoInsert.EdificiosCitados = "", "Null", $"E'{elementoInsert.EdificiosCitados}'")},
                {IIf(elementoInsert.Anejo = "", "Null", $"E'{elementoInsert.Anejo}'")},
                {IIf(elementoInsert.Observaciones = "", "Null", $"E'{elementoInsert.Observaciones}'")},
                {IIf(elementoInsert.Comentarios = "", "Null", $"E'{elementoInsert.Comentarios}'")},
                {elementoInsert.extraProps.propertyCode},
                {elementoInsert.ProvinciaRepo})"



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

        If ModeEdition = TypeModeEdition.CreateDocument Then CrearNuevoElemento()
        If ModeEdition = TypeModeEdition.EditSingleDocument Then ActualizacionLote()

        'If Me.Tag = 0 Then
        '    CrearNuevoElemento(sender, e)
        'Else
        '    ActualizacionLote()
        'End If

    End Sub

    Private Sub SelecciónImagen(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click, Button7.Click, Button8.Click

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

        ListaSQL.Add($"INSERT INTO bdsidschema.archivohisto
	                    SELECT now() as fecha_elim, '{usuarioMyApp.loginUser}' as user_delete,
	                    archivo.idarchivo,archivo.numdoc,archivo.escala,archivo.tomo,archivo.coleccion,archivo.subdivision,archivo.fechaprincipal,archivo.tipo_fechaprincipal,
		                        archivo.fechasmodificaciones,archivo.anejo,archivo.vertical,archivo.horizontal,archivo.tipodoc_id,archivo.estadodoc_id,archivo.procecarpeta,archivo.procehoja,
		                        archivo.subtipo,archivo.juntaestadistica,archivo.signatura,archivo.observestandar_id,archivo.extraprops,archivo.observaciones,archivo.observ,archivo.proyecto,
		                        archivo.cdd_nomfich,archivo.cdd_url,archivo.cdd_producto,archivo.cdd_geometria,archivo.cdd_fecha,archivo.titn,archivo.autor,archivo.autor_persona,archivo.encabezado,archivo.nombreedificio,
		                        tbtipodocumento.tipodoc as Tipo,tbestadodocumento.estadodoc as Estado, tbobservaciones.observestandar,
                                archivo.provincia_id as repoprov, archivo.fechacreacion, archivo.fechamodificacion,
                                string_agg(territorios.idterritorio::character varying,'#') as listaIdTerris,
		                        string_agg(territorios.nombre,'#') as listaMuniHisto, string_agg(to_char(territorios.munihisto, 'FM0000009'::text),'#') as listaCodMuniHisto,
		                        string_agg(listamunicipios.nombre,'#') as listaMuniActual, string_agg(listamunicipios.inecorto,'#') as listaCodMuniActual, 
		                        string_agg(provincias.nombreprovincia,'#') as nombreprovincia 
                        FROM bdsidschema.archivo 
	                        LEFT JOIN bdsidschema.tbtipodocumento ON tbtipodocumento.idtipodoc=archivo.tipodoc_id 
	                        LEFT JOIN bdsidschema.tbestadodocumento ON tbestadodocumento.idestadodoc=archivo.estadodoc_id 
	                        LEFT JOIN bdsidschema.archivo2territorios  ON archivo2territorios.archivo_id=archivo.idarchivo 
	                        LEFT JOIN bdsidschema.tbobservaciones  ON tbobservaciones.idobservestandar=archivo.observestandar_id 
	                        LEFT JOIN bdsidschema.territorios on territorios.idterritorio= archivo2territorios.territorio_id 
	                        LEFT JOIN ngmepschema.listamunicipios on territorios.nomen_id= listamunicipios.identidad 
	                        LEFT JOIN bdsidschema.provincias on territorios.provincia= provincias.idprovincia 
                        WHERE archivo.idarchivo={editRegistro.docIndex}
                          GROUP BY archivo.idarchivo,archivo.numdoc,archivo.escala,archivo.tomo,archivo.coleccion,archivo.subdivision,archivo.fechaprincipal,archivo.tipo_fechaprincipal,
  	                        archivo.fechasmodificaciones,archivo.anejo,archivo.vertical, archivo.horizontal, archivo.tipodoc_id, archivo.estadodoc_id, archivo.procecarpeta, 
  	                        archivo.procehoja, archivo.subtipo,archivo.juntaestadistica, archivo.signatura, archivo.observestandar_id,archivo.extraprops, archivo.observaciones,archivo.observ,
                            archivo.proyecto,tbtipodocumento.tipodoc,archivo.cdd_nomfich,archivo.cdd_url,archivo.cdd_producto,archivo.titn,archivo.autor,archivo.autor_persona,archivo.encabezado,archivo.nombreedificio,
                            tbestadodocumento.estadodoc,tbobservaciones.observestandar")
        ListaSQL.Add($"DELETE FROM bdsidschema.contornos WHERE archivo_id={editRegistro.docIndex}")
        ListaSQL.Add($"DELETE FROM bdsidschema.archivo2territorios WHERE archivo_id={editRegistro.docIndex}")
        ListaSQL.Add($"DELETE FROM bdsidschema.archivo WHERE idarchivo={editRegistro.docIndex}")


        Try
            If IO.File.Exists(editRegistro.rutaFicheroAltaRes) Then ficherosDelete.Add(editRegistro.rutaFicheroAltaRes)
            If IO.File.Exists(editRegistro.rutaFicheroBajaRes) Then ficherosDelete.Add(editRegistro.rutaFicheroBajaRes)
            If IO.File.Exists(editRegistro.rutaFicheroThumb) Then ficherosDelete.Add(editRegistro.rutaFicheroThumb)
            If IO.File.Exists(editRegistro.rutaFicheroPDF) Then ficherosDelete.Add(editRegistro.rutaFicheroPDF)
            For Each fileGEO As String In editRegistro.listaFicherosGeo23030
                If IO.File.Exists(fileGEO) Then ficherosDelete.Add(fileGEO)
            Next
            For Each fileGEO As String In editRegistro.listaFicherosGeo25830
                If IO.File.Exists(fileGEO) Then ficherosDelete.Add(fileGEO)
            Next
        Catch ex As Exception
            ModalError(ex.Message)
            Exit Sub
        End Try

        If ModalQuestWriteDatabase($"¿Desea eliminar el documento con sellado nº {editRegistro.Sellado} y su información digital{Environment.NewLine}({ficherosDelete.Count} ficheros?") = DialogResult.No Then Exit Sub

        Me.Cursor = Cursors.WaitCursor

        'Borrado de ficheros
        For Each pathFileGeo As String In ficherosDelete
            If IsNothing(pathFileGeo) Then Continue For
            Try
                IO.File.Delete(pathFileGeo)
            Catch ex As Exception
                ModalError($"Se han producido errores al eliminar la información gráfica del documento.No se eliminó el documento.{Environment.NewLine}{ex.Message}")
            End Try
        Next

        IIf(ExeTran(ListaSQL),ModalInfo("Proceso de eliminación completado"),ModalExclamation("No se han podido eliminar los documentos. Consultar LOG"))

        Me.Cursor = Cursors.Default
        Me.Close()

    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click

        If ModalQuestion("¿Desea actualizar sólo los recursos del documento?") = DialogResult.No Then
            ModalInfo("No se ha actualizado ningún dato")
            Exit Sub
        End If

        UpdateDigitalResources(editRegistro)



    End Sub


    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged, TextBox3.TextChanged, TextBox23.TextChanged

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

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click, Button11.Click, Button12.Click

        Dim pathResource As String
        Dim ctrlSender As Windows.Forms.Button
        ctrlSender = sender
        If String.IsNullOrEmpty(ctrlSender.Text) Then ModalExclamation("No hay recurso asignado") : Exit Sub

        If ctrlSender.Name = "Button6" Then pathResource = Label34.Tag
        If ctrlSender.Name = "Button11" Then pathResource = Label35.Tag
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
        Dim docuImport As docCartoSEE
        Dim numSelladoImport As String = InputDialog.InputBox("Introduzca el número del documento que quiere importar", "Documento GEODOCAT", "")

        ObtenerEscalar($"SELECT idarchivo from bdsidschema.archivo where numdoc='{numSelladoImport}'", archivoIdImport)
            docuImport = New docCartoSEE(archivoIdImport)
        docuImport.getGeoFiles()

        If docuImport.docIndex = 0 Then
            ModalInfo($"No se localiza el documento con sellado nº {numSelladoImport}")
            Me.Close()
            Exit Sub
        End If

        CleanFields()
        PopulateControlsWithDocCartoSEE(docuImport)
        'Eliminamos los ficheros de recursos, porque estamos importando para crear un nuevo documento y no tiene sentido mantener estos ficheros
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox23.Text = ""
        TextBox2.Tag = ""
        TextBox3.Tag = ""
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

    Private Sub TextBox16_TextChanged(sender As Object, e As EventArgs) Handles TextBox16.TextChanged, TextBox18.TextChanged, TextBox19.TextChanged,
                    TextBox20.TextChanged, TextBox24.TextChanged, TextBox21.TextChanged, CheckedListBox1.SelectedIndexChanged, ComboBox5.SelectedIndexChanged,
                    TextBox9.TextChanged, ComboBox1.SelectedIndexChanged, ComboBox2.SelectedIndexChanged, ComboBox3.SelectedIndexChanged, ComboBox6.SelectedIndexChanged, TextBox17.TextChanged,
                    TextBox14.TextChanged, TextBox14.TextChanged, TextBox12.TextChanged, TextBox13.TextChanged, TextBox22.TextChanged, TextBox4.TextChanged, TextBox5.TextChanged,
                    TextBox6.TextChanged, TextBox7.TextChanged, TextBox8.TextChanged, TextBox10.TextChanged, TextBox11.TextChanged, TextBox15.TextChanged

        If Not autoCheckFlag Then Exit Sub
        If sender.name = "TextBox16" Then CheckBox16.Checked = True
        If sender.name = "TextBox18" Then CheckBox26.Checked = True
        If sender.name = "TextBox19" Then CheckBox27.Checked = True
        If sender.name = "TextBox20" Then CheckBox28.Checked = True
        If sender.name = "TextBox21" Then CheckBox29.Checked = True
        If sender.name = "TextBox24" Then CheckBox30.Checked = True
        If sender.name = "ComboBox5" Then CheckBox19.Checked = True
        If sender.name = "CheckedListBox1" Then CheckBox3.Checked = True
        If sender.name = "TextBox9" Then CheckBox24.Checked = True
        If sender.name = "ComboBox1" Then CheckBox17.Checked = True
        If sender.name = "ComboBox2" Then CheckBox2.Checked = True
        If sender.name = "ComboBox3" Then CheckBox31.Checked = True
        If sender.name = "ComboBox6" Then CheckBox20.Checked = True
        If sender.name = "TextBox17" Then CheckBox25.Checked = True
        If sender.name = "TextBox14" Then CheckBox14.Checked = True
        If sender.name = "TextBox14" Then CheckBox14.Checked = True
        If sender.name = "TextBox12" Then CheckBox12.Checked = True
        If sender.name = "TextBox13" Then CheckBox13.Checked = True
        If sender.name = "TextBox22" Then CheckBox22.Checked = True
        If sender.name = "TextBox4" Then CheckBox4.Checked = True
        If sender.name = "TextBox5" Then CheckBox5.Checked = True
        If sender.name = "TextBox6" Then CheckBox6.Checked = True
        If sender.name = "TextBox7" Then CheckBox7.Checked = True
        If sender.name = "TextBox8" Then CheckBox8.Checked = True
        If sender.name = "TextBox10" Then CheckBox10.Checked = True
        If sender.name = "TextBox11" Then CheckBox11.Checked = True
        If sender.name = "TextBox15" Then CheckBox15.Checked = True

    End Sub

#Region "Gestión Drag & Drop de los ficheros"

    Private Sub textBoxesEntries(sender As Object, e As DragEventArgs) Handles TextBox2.DragEnter, TextBox3.DragEnter, TextBox23.DragEnter

        e.Effect = DragDropEffects.Link

    End Sub

    Private Sub textBoxesDropping(sender As Object, e As DragEventArgs) Handles TextBox2.DragDrop, TextBox3.DragDrop, TextBox23.DragDrop

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
            If sender.name = "TextBox3" Then TextBox3.Text = Rutas(0)
            If sender.name = "TextBox23" Then TextBox23.Text = Rutas(0)

        Catch ex As Exception
            MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
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