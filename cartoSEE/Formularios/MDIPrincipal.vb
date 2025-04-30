Public Class MDIPrincipal

    Dim Autocompletar_municipios As Boolean
    Dim Autocompletar_colindantes As Boolean
    Dim Colindantes As New DataTable
    Dim quitarAcento As New Destildator
    Dim pendingNotify As Integer = 0

    Private m_ChildFormNumber As Integer = 0


    Private Sub ToolBarToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ToolBarToolStripMenuItem.Click
        Me.ToolStrip.Visible = Me.ToolBarToolStripMenuItem.Checked
    End Sub

    Private Sub StatusBarToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles StatusBarToolStripMenuItem.Click
        Me.StatusStrip.Visible = Me.StatusBarToolStripMenuItem.Checked
    End Sub

    Private Sub PanelLateralIzquierdoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PanelLateralIzquierdoToolStripMenuItem.Click

        Me.Panel1.Visible = Me.PanelLateralIzquierdoToolStripMenuItem.Checked

    End Sub

    Private Sub CascadeToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CascadeToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub

    Private Sub TileVerticleToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileVerticalToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub

    Private Sub TileHorizontalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileHorizontalToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub

    Private Sub ArrangeIconsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ArrangeIconsToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.ArrangeIcons)
    End Sub

    Private Sub CloseAllToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CloseAllToolStripMenuItem.Click
        ' Close all child forms of the parent.
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next
    End Sub

    Private Sub SalirToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalirToolStripMenuItem.Click

        Global.System.Windows.Forms.Application.Exit()
        End

    End Sub



    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click, ToolStripButton19.Click,
                            mnuDeveloperTools.Click, mnuOpenAppFolderSetting.Click, mnuAdminTools.Click

        If sender.name = "ToolStripButton1" Or sender.name = "mnuDeveloperTools" Then
            If usuarioMyApp.permisosLista.isUserISTARI Then
                Dim Desarrollo As New frmDevel
                Desarrollo.MdiParent = Me
                Desarrollo.Show()
            End If
        ElseIf sender.name = "ToolStripButton19" Or sender.name = "mnuAdminTools" Then
            If usuarioMyApp.permisosLista.isUserISTARI Then
                Dim Desarrollo As New frmDevelIGN
                Desarrollo.MdiParent = Me
                Desarrollo.Show()
            End If
        ElseIf sender.name = "mnuOpenAppFolderSetting" Then
            Try
                Process.Start(AppFolderSetting)
            Catch ex As Exception
                ModalError(ex.Message)
            End Try
        End If

    End Sub

    Private Sub MDIPrincipal_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        If MainConex.State <> ConnectionState.Closed Then DesconectarBD()
    End Sub

    Private Sub MDIParent1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        '-------------------------------------Pantallazo de Bienvenida
        My.Forms.SplashScreen.ShowDialog()
        '-------------------------------------Leemos parametros del INI
        LeerConfiguracionINI()
        '-------------------------------------Autenticación del usuario

        Try
            My.Forms.LoginForm.ShowDialog()
        Catch
            Application.DoEvents()
        End Try
        If My.Forms.LoginForm.DialogResult = Windows.Forms.DialogResult.OK Then
            My.Forms.LoginForm.Dispose()
            Application.DoEvents()
        Else
            My.Forms.LoginForm.Dispose()
            Application.DoEvents()
            MessageBox.Show("No dispone de acceso al programa", My.Application.Info.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End
        End If
        Me.Hide()

        '-------------------------------------Resizing inicial
        If Screen.PrimaryScreen.Bounds.Width > 1025 Then
            Me.Size = New Point(1200, 800)
        End If
        Me.Text = AplicacionTitulo
        '--------------------------Conexión a la base de datos
        Dim startTicks As Long = DateTime.Now.Ticks
        If ConectarBD(TiposBase.PostgreSQL, DB_Servidor, DB_Port, DB_User, DB_Pass, DB_Instancia, "") = True Then
            'Validamos el usuario para conocer sus permisos
            usuarioMyApp = New myAppUser
            If usuarioMyApp.Permisos.AutenticarUsuario(accessUser, accessPass) > 0 Then
                ModalExclamation($"No dispone de acceso al programa.{Environment.NewLine}Consulte con el administrador del sistema")
                End
            End If

            GenerarLOG("Inicio sesión en " & DB_Servidor)
            CargarListasProvincias()
            CargarListaTiposDocumento()
            ''----Lanzamos un hilo para la carga de los municipios, que es más lenta
            CargarListasMunicipios()
            CargarListasMunicipiosActual()
            'Dim p As New System.Threading.Thread(AddressOf CargarListasMunicipios)
            'p.Start()
            Dim contadorProv As Integer = 0
            For Each Provincia As DataRow In ListaProvincias.Select
                contadorProv = contadorProv + 1
                ComboBox3.Items.Add(New itemData(Provincia.ItemArray(1).ToString, contadorProv))
            Next
            ComboBox3.Items.Add(New itemData("(Todas)", 0))
            ResizingElements()
            CargarFiltros()

            ToolStripStatusLabel3.Text = ""
            If usuarioMyApp.permisosLista.EditarDocumentacion Then
                If ObtenerEscalar("SELECT count(*) from bdsidschema.incidencias where aplicacion='CARTOSEE' and estado='Abierta'", pendingNotify) Then
                    If pendingNotify > 0 Then
                        ToolStripStatusLabel3.Text = "Hay " & pendingNotify & " incidencias abiertas"
                        ToolStripStatusLabel3.Visible = True
                    Else
                        ToolStripStatusLabel3.Text = "No hay notificaciones pendientes"
                        ToolStripStatusLabel3.Visible = True
                    End If
                End If
                ToolStripStatusLabel.Text = "Conectado a " & DB_Instancia & " en " & DB_Servidor & ". Usuario de administración."
            Else
                ToolStripStatusLabel.Text = "Conectado al sistema. Usuario de consulta."
                ToolStripStatusLabel.ToolTipText = "Conectado a " & DB_Instancia & " en " & DB_Servidor & ""
            End If

            Me.Show()

        Else
            MessageBox.Show("No es posible conectarse a la base de datos", My.Application.Info.AssemblyName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            ResizingElements()
            Button6.Enabled = False
            Button7.Enabled = False
            TextBox1.Enabled = False
            ToolStripButton19.Enabled = False
            ToolStripMenuItem2.Enabled = False
            mnuTool_Informes.Enabled = False
            ToolStripStatusLabel.Text = "Modo de Configuración. Consultas no disponibles"
        End If

        'Dim endTicks As Long = DateTime.Now.Ticks
        ToolStripStatusLabel1.Text = $"{usuarioMyApp.LoginUser} / {usuarioMyApp.permisosLista.getNombrePermiso}"
        ToolStripStatusLabel.Text = $"{DB_Instancia} en {DB_Servidor} {IIf(usuarioMyApp.permisosLista.isUserISTARI, $" .EXE:{My.Application.Info.DirectoryPath}{IIf(TestMode, "MODO SEGURO", "PELIGRO. EDICIONES ACTIVAS")}", "")}"
        If usuarioMyApp.permisosLista.isUserISTARI Then
            ToolStripStatusLabel.ForeColor = IIf(TestMode, Color.Green, Color.Red)
        End If

        ToolStripStatusLabel2.Text = Now.ToLongDateString.ToString
        'My.Application.Info.AssemblyName.
        My.Forms.SplashScreen.Dispose()
        TextBox7.Text = "575438"
        TextBox10.Text = "4518765"
        TextBox9.Text = "617612"
        TextBox3.Text = "4540893"

        mnuOpenPreferenceFolder.Visible = usuarioMyApp.permisosLista.isUserISTARI
        mnuOpenLoggerFile.Visible = usuarioMyApp.permisosLista.isUserISTARI

    End Sub

    Sub CargarFiltros()


        Dim Filtros As DataTable

        Filtros = New DataTable
        If CargarDatatable("Select idestadodoc,estadodoc FROM bdsidschema.tbestadodocumento", Filtros) = False Then
            MessageBox.Show("No se puede acceder a la tabla de Estados de la documentación",
                            AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            ComboBox7.Items.Clear()
            For Each Filtro As DataRow In Filtros.Select
                ComboBox7.Items.Add(New itemData(Filtro.ItemArray(1).ToString, Filtro.ItemArray(0)))
            Next
        End If
        Filtros.Dispose()
        Filtros = Nothing

        Filtros = New DataTable
        If CargarDatatable("Select idtipodoc,tipodoc from bdsidschema.tbtipodocumento", Filtros) = False Then
            MessageBox.Show("No se puede acceder a la tabla de Tipos de documentación",
                            AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            CheckedListBox1.Items.Clear()
            For Each Filtro As DataRow In Filtros.Select
                CheckedListBox1.Items.Add(New itemData(Filtro.ItemArray(1).ToString, Filtro.ItemArray(0)))
            Next
        End If
        Filtros.Dispose()
        Filtros = Nothing

        'Plantilla de metadatos
        For iBucle As Integer = 0 To RutasPlantillasMetadatos.Length - 1
            RutasPlantillasMetadatos(iBucle).RutaMetadatosTipo =
                        LeeIni("Metadatos", "RutaPlantilla" & RutasPlantillasMetadatos(iBucle).idTipodoc.ToString)
            RutasPlantillasMetadatos(iBucle).PrefijoNom =
                        LeeIni("Metadatos", "PrefijoNom" & RutasPlantillasMetadatos(iBucle).idTipodoc.ToString)
        Next


    End Sub


    Sub ResizingElements()

        Panel1.Width = 250
        RadioButton1.Width = 250
        RadioButton2.Width = 250

        '--------------------------Resizing de elementos
        lvMunicipios.Columns.Clear()
        lvMunicipios.Columns.Add("Propiedad", 215, HorizontalAlignment.Left)
        lvMunicipios.SmallImageList = ImageList2
        lvMunicipios.FullRowSelect = True
        lvMunicipios.View = View.Details
        lvMunicipios.HeaderStyle = ColumnHeaderStyle.None
        lvMunicipios.Location = New Point(6, 116)
        lvMunicipios.Size = New Point(237, 170)
        lvMunicipios.Visible = False

        PictureBox1.Visible = False
        PictureBox3.Visible = False
        PictureBox4.Visible = False
        Panel_DocSearch.Location = New Point(0, 0)
        Panel_GeoSearch.Location = New Point(0, 0)
        Panel_DocSearch.Size = New Point(250, 800)
        Panel_GeoSearch.Size = New Point(250, 800)
        Panel_DocSearch.Visible = True
        Panel_GeoSearch.Visible = False
        RadioButton1.Checked = True

        TextBox1.Tag = ""
        TextBox3.Text = "" '"4545100"
        TextBox7.Text = "" '"594250"
        TextBox9.Text = "" '"597250"
        TextBox10.Text = "" '"4542500"

        '--------------------------Inicializacion de variables
        Autocompletar_municipios = True
        Autocompletar_colindantes = True

        Panel_DocSearch.BackColor = Panel1.BackColor
        Panel_GeoSearch.BackColor = Panel1.BackColor

        'Si el usuario no es administrador
        ToolStripButton9.Enabled = usuarioMyApp.permisosLista.EditarDocumentacion
        ToolStripButton21.Enabled = usuarioMyApp.permisosLista.EditarDocumentacion
        mnuTool_EditAtrib.Enabled = usuarioMyApp.permisosLista.EditarDocumentacion
        mnuModDocuTiposDoc.Enabled = usuarioMyApp.permisosLista.EditarDocumentacion
        mnuModDocuEstados.Enabled = usuarioMyApp.permisosLista.EditarDocumentacion
        mnuModDocuObservaciones.Enabled = usuarioMyApp.permisosLista.EditarDocumentacion
        mnuModDocuMedidas.Enabled = usuarioMyApp.permisosLista.EditarDocumentacion
        itemGestionUser.Enabled = usuarioMyApp.permisosLista.AsignarPermisosUsuarios
        mnuAddECW.Enabled = usuarioMyApp.permisosLista.EditarDocumentacion
        mnuAddContornos.Enabled = usuarioMyApp.permisosLista.EditarDocumentacion
        btnExportCdD.Enabled = usuarioMyApp.permisosLista.GenerarVersionCdD
        mnuExportCdD.Enabled = usuarioMyApp.permisosLista.GenerarVersionCdD
        ToolStripButton19.Enabled = usuarioMyApp.permisosLista.EditarDocumentacion
        mnuAdminTools.Enabled = usuarioMyApp.permisosLista.EditarDocumentacion

        ToolStripButton1.Enabled = usuarioMyApp.permisosLista.isUserISTARI
        ToolStripButton19.Enabled = usuarioMyApp.permisosLista.isUserISTARI
        mnuDeveloperTools.Enabled = usuarioMyApp.permisosLista.isUserISTARI
        mnuAdminTools.Enabled = usuarioMyApp.permisosLista.isUserISTARI
        mnuTextModeToggle.Visible = usuarioMyApp.permisosLista.isUserISTARI

        mnuDeveloper.Visible = usuarioMyApp.permisosLista.isUserISTARI
        ToolStripButton1.Visible = usuarioMyApp.permisosLista.isUserISTARI
        mnuGenerarRejilla.Visible = usuarioMyApp.permisosLista.isUserISTARI
        mnuLanzarPlantilla.Visible = usuarioMyApp.permisosLista.isUserISTARI
        mnuActivity.Visible = usuarioMyApp.permisosLista.isUserISTARI

        Me.WindowState = FormWindowState.Maximized

        ComboBox1.Items.Add("-----")
        ComboBox1.Items.Add("igual")
        ComboBox1.Items.Add("mayor")
        ComboBox1.Items.Add("menor")
        ComboBox2.Items.Add("-----")
        ComboBox2.Items.Add("igual")
        ComboBox2.Items.Add("mayor")
        ComboBox2.Items.Add("menor")
        ComboBox4.Items.Add("-----")
        ComboBox4.Items.Add("igual")
        ComboBox4.Items.Add("mayor")
        ComboBox4.Items.Add("menor")
        ComboBox5.Items.Add("-----")
        ComboBox5.Items.Add("igual")
        ComboBox5.Items.Add("mayor")
        ComboBox5.Items.Add("menor")


        ComboBox6.Items.Add("Indistinto")
        ComboBox6.Items.Add("Sí")
        ComboBox6.Items.Add("No")

        ComboBox8.Items.Add("Indistinto")
        ComboBox8.Items.Add("Cargados")
        ComboBox8.Items.Add("Faltan")

        ComboBox1.Text = "-----"
        ComboBox2.Text = "-----"
        ComboBox4.Text = "-----"
        ComboBox5.Text = "-----"
        ComboBox6.Text = "-----"

        ToolStripMenuItem1.Visible = False

        mnuTextModeToggle.Text = IIf(TestMode, "Modo Test activado - SEGURIDAD", "Modo test desactivado - PELIGRO")
        mnuTextModeToggle.ForeColor = IIf(TestMode, Color.Green, Color.Red)



    End Sub

    '----------------------------------------------------------------------------------------------------------------------
    'Control del Textbox de búsqueda
    '----------------------------------------------------------------------------------------------------------------------

    Private Sub TextBox1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox1.Click
        If TextBox1.Text.Trim <> "" Then
            TextBox1.SelectAll()
        End If
    End Sub

    Private Sub TextBox1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyUp

        If e.KeyData = Keys.Down And lvMunicipios.Visible = True Then
            lvMunicipios.Focus()
            lvMunicipios.Items(0).Selected = True
        End If
        If e.KeyData = Keys.Enter Then
            Dim resp = ModalQuestCollection("¿Qué desea buscar?")
            If resp = DialogResult.OK Then LaunchQuery(sender, e)
            If resp = DialogResult.Yes Then LaunchQueryCuadMTN(sender, e)
        End If

    End Sub

    Private Sub TextBox23_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox23.KeyUp

        If e.KeyData = Keys.Enter Then
            Dim resp = ModalQuestCollection("¿Qué desea buscar?")
            If resp = DialogResult.OK Then LaunchQuery(sender, e)
            If resp = DialogResult.Yes Then LaunchQueryCuadMTN(sender, e)
        End If

    End Sub




    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged


        Dim filas() As DataRow
        Dim elementoLV As ListViewItem

        lvMunicipios.Visible = False
        Autocompletar_colindantes = False
        Autocompletar_colindantes = True
        If IsNumeric(TextBox1.Text.Trim) Then
            TextBox1.Tag = ""
            Exit Sub
        End If
        If TextBox1.Text.Length > 1 And Autocompletar_municipios = True Then
            PictureBox1.Visible = True
            If CheckBox1.Checked = True Then
                'Trabajamos con los municipios actuales
                If ComboBox3.SelectedIndex = -1 Then
                    filas = ListaMunicipiosActual.Select("nombreSearch like '%" & quitarAcento.destildar(TextBox1.Text, False, False) & "%'")
                Else
                    Dim cProv As Integer = CType(ComboBox3.SelectedItem, itemData).Valor
                    filas = ListaMunicipiosActual.Select("provincia_id=" & cProv.ToString & " and nombreSearch like '%" & quitarAcento.destildar(TextBox1.Text, False, False) & "%'")
                End If
            Else
                'Trabajamos con los históricos
                If ComboBox3.SelectedIndex = -1 Then
                    filas = ListaMunicipiosHisto.Select("nombreSearch like '%" & quitarAcento.destildar(TextBox1.Text, False, False) & "%'")
                Else
                    Dim cProv As Integer = CType(ComboBox3.SelectedItem, itemData).Valor
                    filas = ListaMunicipiosHisto.Select("provincia_id=" & cProv.ToString & " and nombreSearch like '%" & quitarAcento.destildar(TextBox1.Text, False, False) & "%'")
                End If

            End If

            lvMunicipios.Items.Clear()

            For Each dR As DataRow In filas
                elementoLV = New ListViewItem : elementoLV.Text = dR("nombre").ToString : elementoLV.Tag = dR("cod_munihisto") & "|" & dR("idmunihisto") & "|" & dR("inecortoActual")
                If dR("cod_munihisto").ToString.EndsWith("00") Then
                    elementoLV.ImageIndex = 7
                Else
                    elementoLV.ImageIndex = 6
                End If
                elementoLV.ToolTipText = dR("inecortoActual")

                lvMunicipios.Items.Add(elementoLV) : elementoLV = Nothing
                lvMunicipios.Visible = True
            Next
            If lvMunicipios.Items.Count = 1 Then
                Try
                    Autocompletar_municipios = False
                    Dim Indices() As String = lvMunicipios.Items(0).Tag.Split("|")
                    TextBox1.Text = lvMunicipios.Items(0).Text
                    TextBox1.Tag = lvMunicipios.Items(0).Tag
                    lvMunicipios.Tag = Indices(1)
                    lvMunicipios.Visible = False
                    Autocompletar_municipios = True
                Catch
                    MessageBox.Show("Repita la búsqueda de nuevo", My.Application.Info.AssemblyName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End Try
            End If
            PictureBox1.Visible = False
        End If

    End Sub

    Private Sub SeleccionarElementoLV(sender As Object, e As EventArgs) Handles lvMunicipios.Click, lvMunicipios.DoubleClick

        If lvMunicipios.SelectedItems.Count = 0 Then Exit Sub
        Autocompletar_municipios = False
        TextBox1.Text = lvMunicipios.SelectedItems(0).Text
        TextBox1.Tag = lvMunicipios.SelectedItems(0).Tag
        Dim Indices() As String = TextBox1.Tag.Split("|")
        lvMunicipios.Tag = Indices(1)
        Autocompletar_municipios = True
        lvMunicipios.Visible = False
        If sender.name = "lvMunicipios" Then
            Dim resp = ModalQuestCollection("¿Qué desea buscar?")
            If resp = DialogResult.OK Then LaunchQuery(sender, e)
            If resp = DialogResult.Yes Then LaunchQueryCuadMTN(sender, e)
        End If

    End Sub


    Private Sub lvMunicipios_KeyUp(sender As Object, e As KeyEventArgs) Handles lvMunicipios.KeyUp

        If lvMunicipios.SelectedItems.Count = 0 Then Exit Sub
        If e.KeyData = Keys.Up And lvMunicipios.SelectedItems(0).Index = 0 Then
            TextBox1.Focus()
        ElseIf e.KeyData = Keys.Return And lvMunicipios.SelectedItems(0).Index >= 0 Then
            SeleccionarElementoLV(sender, e)
            Dim resp = ModalQuestCollection("¿Qué desea buscar?")
            If resp = DialogResult.OK Then LaunchQuery(sender, e)
            If resp = DialogResult.Yes Then LaunchQueryCuadMTN(sender, e)
        End If

    End Sub


    Sub LanzarConsulta(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOldSearch.Click


        Dim FirmaYear As String = ""
        Dim EstadosDocumento As String = ""
        Dim TiposDocumento As String = ""
        Dim NumTomo As String = ""
        Dim DocAnulada As String = ""
        Dim DocAdicional As String = ""
        Dim DescripFiltro As String = ". "
        Dim MunicipioID As Integer = 0
        Dim CodMunicipioINEHistorico As Integer = 0
        Dim CodMunicipioINEActual As Integer = 0
        Dim nSellado As String = ""
        Dim nSellado1 As String = ""
        Dim nSellado2 As String = ""
        Dim listaSellos As New ArrayList
        Dim CadFiltro As String = ""
        Dim ibucle As Integer
        Dim cProv As Integer = 0

        Me.Cursor = Cursors.WaitCursor
        Application.DoEvents()
        If Not String.IsNullOrEmpty(TextBox1.Tag) Then
            Dim CodigosMuni() As String = TextBox1.Tag.ToString.Split("|")
            CodMunicipioINEHistorico = CodigosMuni(0)
            MunicipioID = CodigosMuni(1)
            CodMunicipioINEActual = CodigosMuni(2)
        ElseIf Not String.IsNullOrEmpty(TextBox23.Text) Then
            If IsNumeric(TextBox23.Text.Trim) Then
                nSellado = TextBox23.Text.Trim
            ElseIf obtenerIntervalo(TextBox23.Text.Trim, "-", nSellado1, nSellado2) = True Then
                Application.DoEvents()
            ElseIf obtenerIntervalo(TextBox23.Text.Trim, "#", nSellado1, nSellado2) = True Then
                Application.DoEvents()
            ElseIf obtenerIntervalo(TextBox23.Text.Trim, ";", listaSellos) = True Then
                Application.DoEvents()
            Else
                Exit Sub
            End If

        Else

            If ComboBox3.SelectedIndex <> -1 Then
                cProv = CType(ComboBox3.SelectedItem, itemData).Valor
            ElseIf IsNumeric(TextBox1.Text.Trim) Then
                nSellado = TextBox1.Text.Trim
            ElseIf obtenerIntervalo(TextBox1.Text.Trim, "-", nSellado1, nSellado2) = True Then
                Application.DoEvents()
            ElseIf obtenerIntervalo(TextBox1.Text.Trim, "#", nSellado1, nSellado2) = True Then
                Application.DoEvents()
            ElseIf obtenerIntervalo(TextBox1.Text.Trim, ";", listaSellos) = True Then
                Application.DoEvents()
            Else
                Exit Sub
            End If
        End If
        PictureBox3.Visible = True
        LanzarSpinner()



        If sender.name = "btnGetImportant" Then
            Dim FrmResult As New frmDocumentacion
            FrmResult.MdiParent = Me
            FrmResult.Text = "Documentos destacados"
            FrmResult.CargarDatosSIDCARTO_By_PropsPatron("__1_____________")
            FrmResult.Show()
            PictureBox3.Visible = False
            Me.Cursor = Cursors.Default
            Exit Sub
        ElseIf sender.name = "btnGetStar" Then
            Dim FrmResult As New frmDocumentacion
            FrmResult.MdiParent = Me
            FrmResult.Text = "Documentos cinco estrellas"
            FrmResult.CargarDatosSIDCARTO_By_PropsPatron("____1___________")
            FrmResult.Show()
            PictureBox3.Visible = False
            Me.Cursor = Cursors.Default
            Exit Sub
        ElseIf sender.name = "btnGetWeird" Then
            Dim FrmResult As New frmDocumentacion
            FrmResult.MdiParent = Me
            FrmResult.Text = "Documentos raros o extraños"
            FrmResult.CargarDatosSIDCARTO_By_PropsPatron("_1______________")
            FrmResult.Show()
            PictureBox3.Visible = False
            Me.Cursor = Cursors.Default
            Exit Sub
        ElseIf sender.name = "btnGetCdD" Then
            Dim FrmResult As New frmDocumentacion
            FrmResult.MdiParent = Me
            FrmResult.Text = "Documentos a revisar para el CdD"
            FrmResult.CargarDatosSIDCARTO_By_PropsPatron("1_______________")
            FrmResult.Show()
            PictureBox3.Visible = False
            Me.Cursor = Cursors.Default
            Exit Sub
        End If




        '-----------------------------------------------------------------------------------
        'Evalúo si se filtran los documentos por estado o por tipo
        '-----------------------------------------------------------------------------------
        ibucle = -1
        TiposDocumento = "-"
        For Each Linea As itemData In CheckedListBox1.Items
            ibucle = ibucle + 1
            If CheckedListBox1.GetItemChecked(ibucle) = True Then
                TiposDocumento = TiposDocumento & Linea.Valor & "-"
            End If
        Next
        If TiposDocumento = "-" Then TiposDocumento = ""

        EstadosDocumento = ""
        If ComboBox7.SelectedIndex <> -1 Then EstadosDocumento = CType(ComboBox7.SelectedItem, itemData).Valor

        If MunicipioID > 0 Then
            CadFiltro = SumarFiltros("")
            Application.DoEvents()
            Dim FrmResult As New frmDocumentacion
            FrmResult.MdiParent = Me
            If CheckBox1.Checked = True Then
                FrmResult.Text = "Municipio Actual: " & TextBox1.Text & DescripFiltro
                FrmResult.CargarDatosSIDCARTO_By_MunicipioINEActual(CodMunicipioINEActual, TiposDocumento, EstadosDocumento, CadFiltro)
            Else
                FrmResult.Text = "Municipio: " & TextBox1.Text & DescripFiltro
                FrmResult.CargarDatosSIDCARTO_By_MunicipioID(MunicipioID, TiposDocumento, EstadosDocumento, CadFiltro)
            End If
            FrmResult.Show()
        ElseIf cProv > 0 Then
            CadFiltro = SumarFiltros("")
            Application.DoEvents()
            Dim FrmResult As New frmDocumentacion
            FrmResult.MdiParent = Me
            FrmResult.Text = "Provincia: " & DameProvinciaByINE(cProv.ToString) & DescripFiltro
            FrmResult.CargarDatosSIDCARTO_By_Provincia(cProv, TiposDocumento, EstadosDocumento, CadFiltro)
            FrmResult.Show()
        ElseIf nSellado <> "" Then
            Dim FrmResult As New frmDocumentacion
            FrmResult.MdiParent = Me
            FrmResult.Text = "Documento nº: " & String.Format("{0:000000}", nSellado) & DescripFiltro
            FrmResult.CargarDatosSIDCARTO_By_Sellado(CType(nSellado, Integer))
            FrmResult.Show()
        ElseIf nSellado1 <> "" And nSellado2 <> "" Then
            Dim FrmResult As New frmDocumentacion
            FrmResult.MdiParent = Me
            FrmResult.Text = "Documentos entre nº: " & String.Format("{0:000000}", nSellado1) & " y " & String.Format("{0:000000}", nSellado2) & " " & DescripFiltro
            FrmResult.CargarDatosSIDCARTO_By_SelladoIntervalo(CType(nSellado1, Integer), CType(nSellado2, Integer))
            FrmResult.Show()
        ElseIf listaSellos.Count > 0 Then
            Dim FrmResult As New frmDocumentacion
            FrmResult.MdiParent = Me
            FrmResult.Text = "Documentos con nº de sellado: " & TextBox1.Text.Trim.Replace(";", ",") & " " & DescripFiltro
            FrmResult.CargarDatosSIDCARTO_By_ListaSellado(listaSellos)
            FrmResult.Show()
        ElseIf cProv = 0 Then
            CadFiltro = SumarFiltros("")
            Application.DoEvents()
            Dim FrmResult As New frmDocumentacion
            FrmResult.MdiParent = Me
            FrmResult.Text = "Todas las provincias. " & DescripFiltro
            FrmResult.CargarDatosSIDCARTO_By_Filtro(CadFiltro, EstadosDocumento, TiposDocumento)
            FrmResult.Show()
        End If


        PictureBox3.Visible = False
        Me.Cursor = Cursors.Default

    End Sub

    Function SumarFiltros(ByVal cadenaSQLini As String) As String

        'Añadimos los filtros que se han seleccionado
        If TextBox5.Text.Trim <> "" Then
            cadenaSQLini = cadenaSQLini & " AND tomo='" & TextBox5.Text.Trim & "'"
        End If
        If TextBox2.Text.Trim <> "" Then
            cadenaSQLini = cadenaSQLini & " AND UPPER(procecarpeta) like '%" & TextBox5.Text.Trim.ToUpper & "%'"
        End If
        If TextBox4.Text.Trim <> "" Then
            cadenaSQLini = cadenaSQLini & " AND procehoja=" & TextBox4.Text.Trim & ""
        End If
        If TextBox6.Text.Trim <> "" Then
            cadenaSQLini = cadenaSQLini & " AND signatura='" & TextBox6.Text.Trim & "'"
        End If
        If TextBox8.Text.Trim <> "" Then
            cadenaSQLini = cadenaSQLini & " AND UPPER(Coleccion) like '%" & TextBox8.Text.Trim.ToUpper & "%'"
        End If

        'Filtro por subdivision------------------------------------------------------------------------
        If TextBox14.Text.Trim <> "" Then
            cadenaSQLini = cadenaSQLini & " AND UPPER(Subdivision) like '%" & TextBox14.Text.Trim.ToUpper & "%'"
        End If
        If ComboBox1.Text <> "" And ComboBox1.Text.StartsWith("-") = False And TextBox15.Text.Trim <> "" Then
            If ComboBox1.Text = "mayor" Then
                cadenaSQLini = cadenaSQLini & " AND Vertical>" & TextBox15.Text
            ElseIf ComboBox1.Text = "menor" Then
                cadenaSQLini = cadenaSQLini & " AND Vertical<" & TextBox15.Text
            ElseIf ComboBox1.Text = "igual" Then
                cadenaSQLini = cadenaSQLini & " AND Vertical=" & TextBox15.Text
            End If
        End If
        If ComboBox2.Text <> "" And ComboBox2.Text.StartsWith("-") = False And TextBox16.Text.Trim <> "" Then
            If ComboBox2.Text = "mayor" Then
                cadenaSQLini = cadenaSQLini & " AND Horizontal>" & TextBox16.Text
            ElseIf ComboBox2.Text = "menor" Then
                cadenaSQLini = cadenaSQLini & " AND Horizontal<" & TextBox16.Text
            ElseIf ComboBox2.Text = "igual" Then
                cadenaSQLini = cadenaSQLini & " AND Horizontal=" & TextBox16.Text
            End If
        End If
        'Filtro por Escala
        If ComboBox4.Text <> "" And ComboBox4.Text.StartsWith("-") = False And TextBox17.Text.Trim <> "" Then
            If ComboBox4.Text = "mayor" Then
                cadenaSQLini = cadenaSQLini & " AND Escala<" & TextBox17.Text
            ElseIf ComboBox4.Text = "menor" Then
                cadenaSQLini = cadenaSQLini & " AND Escala>" & TextBox17.Text
            ElseIf ComboBox4.Text = "igual" Then
                cadenaSQLini = cadenaSQLini & " AND Escala=" & TextBox17.Text
            End If
        End If
        'Filtro por Año Principal
        If ComboBox5.Text <> "" And ComboBox5.Text.StartsWith("-") = False And TextBox18.Text.Trim <> "" Then
            If ComboBox5.Text = "mayor" Then
                cadenaSQLini = cadenaSQLini & " AND TO_CHAR(FechaPrincipal,'YYYY')>'" & TextBox18.Text & "'"
            ElseIf ComboBox5.Text = "menor" Then
                cadenaSQLini = cadenaSQLini & " AND TO_CHAR(FechaPrincipal,'YYYY')<'" & TextBox18.Text & "'"
            ElseIf ComboBox5.Text = "igual" Then
                cadenaSQLini = cadenaSQLini & " AND TO_CHAR(FechaPrincipal,'YYYY')='" & TextBox18.Text & "'"
            End If
        End If

        'Filtro por Año de Modificacion
        If TextBox21.Text.Trim <> "" Then
            cadenaSQLini = cadenaSQLini & " AND fechasmodificaciones like '%" & TextBox21.Text.ToUpper & "%'"
        End If

        'Filtro por subtipo
        If TextBox22.Text.Trim <> "" Then
            cadenaSQLini = cadenaSQLini & " AND subtipo ilike '%" & TextBox22.Text & "%'"
        End If

        'Filtro por Junta Estadistica
        If ComboBox6.Text = "Sí" Then
            cadenaSQLini = cadenaSQLini & " AND juntaestadistica=1"
        ElseIf ComboBox6.Text = "No" Then
            cadenaSQLini = cadenaSQLini & " AND juntaestadistica=0"
        End If


        If ComboBox8.SelectedIndex = 1 Then
            cadenaSQLini = cadenaSQLini & " AND subidoabsys=true"
        ElseIf ComboBox8.SelectedIndex = 2 Then
            cadenaSQLini = cadenaSQLini & " AND subidoabsys=false"
        End If


        'Filtro Por Anejo
        If TextBox19.Text.Trim <> "" Then
            cadenaSQLini = cadenaSQLini & " AND UPPER(anejo) like '%" & TextBox19.Text.ToUpper & "%'"
        End If
        If TextBox20.Text.Trim <> "" Then
            cadenaSQLini = cadenaSQLini & " AND UPPER(observ) like '%" & TextBox20.Text.ToUpper & "%'"
        End If

        SumarFiltros = cadenaSQLini


    End Function



    Private Sub LimpiarCampos(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim iBucle As Integer
        Application.DoEvents()
        For iBucle = 0 To CheckedListBox1.Items.Count - 1
            CheckedListBox1.SetItemChecked(iBucle, False)
        Next
        TextBox1.Text = ""
        TextBox1.Tag = ""
        'ListBox1.Tag = ""
        lvMunicipios.Tag = ""
        TextBox2.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox6.Text = ""
        TextBox8.Text = ""
        TextBox14.Text = ""
        TextBox15.Text = ""
        TextBox16.Text = ""
        TextBox17.Text = ""
        TextBox18.Text = ""
        TextBox19.Text = ""
        TextBox20.Text = ""
        TextBox21.Text = ""
        TextBox22.Text = ""
        TextBox23.Text = ""
        ComboBox1.Text = "-----"
        ComboBox2.Text = "-----"
        ComboBox3.SelectedIndex = -1
        ComboBox4.Text = "-----"
        ComboBox5.Text = "-----"
        ComboBox6.SelectedIndex = -1
        ComboBox7.SelectedIndex = -1
        ComboBox8.SelectedIndex = -1
        'ListBox1.Visible = False
        lvMunicipios.Visible = False
        CheckBox1.Checked = False
        TextBox3.Text = ""
        TextBox7.Text = ""
        TextBox9.Text = ""
        TextBox10.Text = ""
        TextBox11.Text = ""
        TextBox12.Text = ""
        TextBox13.Text = ""

    End Sub

    Sub FuncionesMenuBusqueda(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEjecutar.Click, mnuLimpiar.Click, ToolStripButton17.Click

        If sender.name = "mnuEjecutar" Then
            If Panel_DocSearch.Visible = True Then
                Dim resp = ModalQuestCollection("¿Qué desea buscar?")
                If resp = DialogResult.OK Then LaunchQuery(sender, e)
                If resp = DialogResult.Yes Then LaunchQueryCuadMTN(sender, e)
            End If
            If Panel_GeoSearch.Visible = True Then
                LanzarConsultaGEO_SIDCARTO(sender, e)
            End If

        ElseIf sender.name = "mnuLimpiar" Or sender.name = "ToolStripButton17" Then
            LimpiarCampos(sender, e)
        End If

    End Sub


    Sub MostrarPanelBusquedaGeografica(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                                    Handles ToolStripButton6.Click, mnuBuscadorGeo.Click, RadioButton2.Click
        Panel_DocSearch.Visible = False
        Panel_GeoSearch.Visible = True
        RadioButton2.Checked = True
        'Como no aceptamos filtros en búsqueda geográfica por ahora, replegamos el panel
        Panel1.Width = 250
        RadioButton1.Width = 250
        RadioButton2.Width = 250

    End Sub

    Sub MostrarPanelBusqueda(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                                    Handles ToolStripButton5.Click, mnuBuscador.Click, RadioButton1.Click

        Panel_DocSearch.Visible = True
        Panel_GeoSearch.Visible = False
        RadioButton1.Checked = True

    End Sub

    Private Sub toogleFilterOptions(sender As Object, e As EventArgs) Handles ToolStripButton15.Click, Button2.Click, mnuOpenFilters.Click
        If RadioButton2.Checked Then Exit Sub
        If Panel1.Width = 250 Then
            Panel1.Width = 500
            RadioButton1.Width = 500
            RadioButton2.Width = 500
        Else
            If sender.name = "mnuOpenFilters" Then Exit Sub
            Panel1.Width = 250
            RadioButton1.Width = 250
            RadioButton2.Width = 250

        End If
    End Sub


    Sub ArranqueHerramientas(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                                    Handles ToolStripButton10.Click, mnuGenerarRejilla.Click, ToolStripButton9.Click, mnuLanzarPlantilla.Click,
                                    mnuAddECW.Click, mnuAddContornos.Click, mnuMuniHisto.Click,
                                    mnuQueryLibrosRegistro.Click, ToolStripButton20.Click, ToolStripButton21.Click, mnuActivity.Click, mnuTextModeToggle.Click

        If sender.name = "ToolStripButton9" Then
            If usuarioMyApp.permisosLista.EditarDocumentacion Then
                Dim FormularioCreacion As New frmEdicion With {
                   .MdiParent = Me,
                   .ModeEdition = frmEdicion.ModeEdition.CreateDocument
                }
                FormularioCreacion.Show()
            End If
        ElseIf sender.name = "ToolStripButton21" Then
            If usuarioMyApp.permisosLista.EditarDocumentacion Then
                Dim FormularioCreacionCuadernoMTN As New frmEditCuad With {
                       .MdiParent = Me,
                       .ModeEdition = FormularioCreacionCuadernoMTN.ModeEdition.CreateDocument
                    }
                FormularioCreacionCuadernoMTN.Show()
            End If
        ElseIf sender.name = "mnuTextModeToggle" Then
            Application.DoEvents()
            TestMode = IIf(TestMode, False, True)
            mnuTextModeToggle.Text = IIf(TestMode, "Modo Test activado - SEGURIDAD", "Modo test desactivado - PELIGRO")
            mnuTextModeToggle.ForeColor = IIf(TestMode, Color.Green, Color.Red)
            ToolStripStatusLabel.Text = $"{DB_Instancia} en {DB_Servidor} {IIf(usuarioMyApp.permisosLista.isUserISTARI, $" .EXE:{My.Application.Info.DirectoryPath}{IIf(TestMode, "MODO SEGURO", "PELIGRO. EDICIONES ACTIVAS")}", "")}"
            If usuarioMyApp.permisosLista.isUserISTARI Then
                ToolStripStatusLabel.ForeColor = IIf(TestMode, Color.Green, Color.Red)
            End If

        ElseIf sender.name = "mnuActivity" Then
            Dim frmVista As New frmActivity

            frmVista.MdiParent = Me
            frmVista.Text = "Actividad en BADASID"
            frmVista.SQLBase = "SELECT idlogactivity, usuario, maquina, aplicacion,fecha, descrip,consulta FROM bdsidschema.logactivity"
            frmVista.PKField = "idlogactivity"
            frmVista.OrderByField = "idlogactivity"
            frmVista.OrderByDirection = "DESC"
            frmVista.LimitResults = 3000


            frmVista.DefinitionColumns.Add(New frmActivity.DVMDataGridElement With {
                                                                                .NameField = "idlogactivity",
                                                                                .HeaderText = "idlogactivity",
                                                                                .Visible = False,
                                                                                .Hide_And_show = False,
                                                                                .Searchable = False,
                                                                                .AligmentText = DataGridViewContentAlignment.BottomRight,
                                                                                .Width = 0,
                                                                                .FixedWidth = False
                                                        })
            frmVista.DefinitionColumns.Add(New frmActivity.DVMDataGridElement With {
                                                                                .NameField = "usuario",
                                                                                .HeaderText = "Usuario",
                                                                                .Visible = True,
                                                                                .Hide_And_show = True,
                                                                                .Searchable = True,
                                                                                .AligmentText = DataGridViewContentAlignment.BottomLeft,
                                                                                .Width = 120,
                                                                                .FixedWidth = True
                                                        })
            frmVista.DefinitionColumns.Add(New frmActivity.DVMDataGridElement With {
                                                                                .NameField = "maquina",
                                                                                .HeaderText = "Máquina",
                                                                                .Visible = True,
                                                                                .Hide_And_show = True,
                                                                                .Searchable = True,
                                                                                .AligmentText = DataGridViewContentAlignment.BottomLeft,
                                                                                .Width = 120,
                                                                                .FixedWidth = True
                                                        })
            frmVista.DefinitionColumns.Add(New frmActivity.DVMDataGridElement With {
                                                                                .NameField = "aplicacion",
                                                                                .HeaderText = "Aplicación",
                                                                                .Visible = True,
                                                                                .Hide_And_show = True,
                                                                                .Searchable = False,
                                                                                .AligmentText = DataGridViewContentAlignment.BottomLeft,
                                                                                .Width = 120,
                                                                                .FixedWidth = True
                                                        })
            frmVista.DefinitionColumns.Add(New frmActivity.DVMDataGridElement With {
                                                                                .NameField = "fecha",
                                                                                .HeaderText = "Fecha",
                                                                                .Visible = True,
                                                                                .Hide_And_show = True,
                                                                                .Searchable = False,
                                                                                .AligmentText = DataGridViewContentAlignment.BottomLeft,
                                                                                .Width = 120,
                                                                                .FixedWidth = True
                                                        })
            frmVista.DefinitionColumns.Add(New frmActivity.DVMDataGridElement With {
                                                                                .NameField = "descrip",
                                                                                .HeaderText = "Descripción",
                                                                                .Visible = True,
                                                                                .Hide_And_show = True,
                                                                                .Searchable = False,
                                                                                .AligmentText = DataGridViewContentAlignment.BottomLeft,
                                                                                .Width = 150,
                                                                                .FixedWidth = False
                                                        })
            frmVista.DefinitionColumns.Add(New frmActivity.DVMDataGridElement With {
                                                                                .NameField = "consulta",
                                                                                .HeaderText = "Consulta",
                                                                                .Visible = True,
                                                                                .Hide_And_show = True,
                                                                                .Searchable = False,
                                                                                .AligmentText = DataGridViewContentAlignment.BottomLeft,
                                                                                .Width = 150,
                                                                                .FixedWidth = False
                                                        })
            frmVista.Show()

        ElseIf sender.name = "mnuQueryLibrosRegistro" Or sender.name = "ToolStripButton20" Then

            Dim frmVista As New dataViewerForm

            frmVista.MdiParent = Me
            frmVista.Text = "Libros de registro del Archivo Técnico"
            frmVista.cadSQL = "SELECT idregistro, contenido, tomo, nombreprovincia,fichpdf," &
                    "'" & rutaRepoInventarioInfo & "' || '\' || " & "to_char(codprov,'FM00')" & " || '\' || fichpdf as pathresopurce," &
                    "paginas FROM bdsidschema.librosderegistro inner join bdsidschema.provincias on idprovincia=librosderegistro.codprov"
            frmVista.fieldExternalDocument = "pathresopurce"
            frmVista.camposVisibles = New List(Of String) From {"contenido", "tomo", "nombreprovincia", "ruta", "paginas"}
            frmVista.headerFields = New Dictionary(Of String, String) From {
                                                                                    {"contenido", "Contenido"},
                                                                                    {"tomo", "Tomo"},
                                                                                    {"nombreprovincia", "Provincia"},
                                                                                    {"paginas", "Páginas"}
                                                                                }
            frmVista.filterFields = New Dictionary(Of String, String) From {
                                                                                    {"nombreprovincia", "Provincia"},
                                                                                    {"tomo", "Tomo"}
                                                                                }
            frmVista.Show()
        ElseIf sender.name = "mnuMuniHisto" Or sender.name = "ToolStripButton10" Then
            Me.Cursor = Cursors.WaitCursor
            LanzarSpinner()
            Dim frmMuniH As New frmMuniHisto
            frmMuniH.MdiParent = Me
            frmMuniH.Show()
            Me.Cursor = Cursors.Default
        ElseIf sender.name = "mnuAddECW" Then
            Dim frmAddECWs As New ImportECW
            frmAddECWs.MdiParent = Me
            frmAddECWs.Show()
        ElseIf sender.name = "mnuAddContornos" Then
            Dim Desarrollo As New ImportContornos
            Desarrollo.MdiParent = Me
            Desarrollo.Show()
        ElseIf sender.name = "mnuLanzarPlantilla" Then
            If PlantillaGIS = "" Then
                MessageBox.Show("No se ha definido plantilla", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            LanzarVisorExterno(PlantillaGIS)
        ElseIf sender.name = "mnuGenerarRejilla" Then
            'Genera una rejilla válida para georreferenciación
            Dim CooX As String = InputDialog.InputBox("Coordenada X mínima: esquina Superior Izquierda", "Coordenada X mínima: esquina Superior Izquierda", "")
            If Not IsNumeric(CooX) Then Exit Sub
            Dim CooY As String = InputDialog.InputBox("Coordenada Y máxima: esquina Superior Izquierda", "Coordenada Y máxima: esquina Superior Izquierda", "")
            If Not IsNumeric(CooY) Then Exit Sub
            Dim CeldasX As String = InputDialog.InputBox("Número de celdas en el eje X", "Número de celdas en el eje X", "")
            If Not IsNumeric(CeldasX) Then Exit Sub
            Dim CeldasY As String = InputDialog.InputBox("Número de celdas en el eje Y", "Número de celdas en el eje Y", "")
            If Not IsNumeric(CeldasY) Then Exit Sub
            Dim AnchuraXparam As String = InputDialog.InputBox("Anchura en metros de la celda", "Anchura en metros de la celda", "")
            If Not IsNumeric(AnchuraXparam) Then Exit Sub
            Dim AnchuraYparam As String = InputDialog.InputBox("Altura en metros de la celda", "Altura en metros de la celda", "")
            If Not IsNumeric(AnchuraYparam) Then Exit Sub
            Dim nPuntos As String = InputDialog.InputBox("Número de puntos por celda (4 ó 8)", "Número de puntos por celda (4 ó 8)", "")
            If nPuntos <> "4" And nPuntos <> "8" Then Exit Sub
            Dim iBucleX As Double
            Dim iBucleY As Double
            Dim AnchuraX As Double = AnchuraXparam.Replace(".", ",")
            Dim AnchuraY As Double = AnchuraYparam.Replace(".", ",")
            Dim sw As New System.IO.StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) & "\rejilla.xyz", False, System.Text.Encoding.Unicode)

            For iBucleX = CType(CooX.Replace(".", ","), Double) To CType(CooX.Replace(".", ","), Double) + AnchuraX * (CeldasX - 1) Step AnchuraX
                For iBucleY = CType(CooY.Replace(".", ","), Double) To CType(CooY.Replace(".", ","), Double) - AnchuraY * (CeldasY - 1) Step AnchuraY * (-1)
                    sw.WriteLine("DESCRIPTION=Unknown Area Type")
                    sw.WriteLine("NAME=" & "")
                    sw.WriteLine("CLOSED=YES")
                    sw.WriteLine(CType(iBucleX, String).Replace(",", ".") & "," & CType(iBucleY, String).Replace(",", ".") & ",-999999")
                    If nPuntos = "8" Then sw.WriteLine(CType(iBucleX + AnchuraX / 2, String).Replace(",", ".") & "," & CType(iBucleY, String).Replace(",", ".") & ",-999999")
                    sw.WriteLine(CType(iBucleX + AnchuraX, String).Replace(",", ".") & "," & CType(iBucleY, String).Replace(",", ".") & ",-999999")
                    If nPuntos = "8" Then sw.WriteLine(CType(iBucleX + AnchuraX, String).Replace(",", ".") & "," & CType(iBucleY - AnchuraY / 2, String).Replace(",", ".") & ",-999999")
                    sw.WriteLine(CType(iBucleX + AnchuraX, String).Replace(",", ".") & "," & CType(iBucleY - AnchuraY, String).Replace(",", ".") & ",-899999")
                    If nPuntos = "8" Then sw.WriteLine(CType(iBucleX + AnchuraX / 2, String).Replace(",", ".") & "," & CType(iBucleY - AnchuraY, String).Replace(",", ".") & ",-999999")
                    sw.WriteLine(CType(iBucleX, String).Replace(",", ".") & "," & CType(iBucleY - AnchuraY, String).Replace(",", ".") & ",-999999")
                    If nPuntos = "8" Then sw.WriteLine(CType(iBucleX, String).Replace(",", ".") & "," & CType(iBucleY - AnchuraY / 2, String).Replace(",", ".") & ",-999999")
                    sw.WriteLine(CType(iBucleX, String).Replace(",", ".") & "," & CType(iBucleY, String).Replace(",", ".") & ",-999999")
                Next iBucleY
            Next iBucleX
            sw.Close()
            sw.Dispose()
            sw = Nothing
            ModalInfo("Rejilla terminada")

        End If

    End Sub

    Sub LanzarInformes(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
                    mnuTool_Informes01.Click, mnuTool_Informes02.Click, mnuTool_Informes03.Click, mnuTool_Informes04.Click, mnuReportDocsNoContornos.Click, mnuPPCnoGeo.Click


        Dim Fecha_ini As String
        Dim Fecha_fin As String
        Dim codProv As Integer
        Dim textoInforme As String = ""

        If sender.name = "mnuReportDocsNoContornos" Then
            Try
                codProv = CType(InputDialog.InputBox("Introduce el código INE  (1-50) de la provincia para elaborar el informe." & System.Environment.NewLine &
                                                     "Introduce 0 para hacer todas", AplicacionTitulo, ""), Integer)
            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End Try
            If codProv < 0 Or codProv > 50 Then
                MessageBox.Show("Código de provincia no válido", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
            If codProv = 0 Then
                If MessageBox.Show("Un informe de todas las provincias puede resultar lento. Aún así, ¿desea continuar?", AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                    Exit Sub
                End If
                textoInforme = "Documentos de todas las provincias sin georeferenciar"
            Else
                textoInforme = "Documentos de " & DameProvinciaByINE(codProv) & " sin georreerenciar"
            End If

            LanzarSpinner()

            Me.Cursor = Cursors.WaitCursor
            Dim frmResult As New frmDocumentacion
            frmResult.MdiParent = Me
            frmResult.Text = textoInforme
            If codProv = 0 Then
                frmResult.getGeodocatDocsWithoutContour("the_geom Is null")
            Else
                frmResult.getGeodocatDocsWithoutContour("the_geom Is null And provincias.idprovincia=" & codProv)
            End If

            frmResult.Show()
            Me.Cursor = Cursors.Default
            Exit Sub
        End If


        Dim FormularioInformes As New frmInformes
        FormularioInformes.MdiParent = Me
        FormularioInformes.Show()

        If sender.name = "mnuTool_Informes01" Then
            If MessageBox.Show("Informe de todos los documentos", AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                Fecha_ini = ""
                Fecha_fin = ""
            Else
                Fecha_ini = InputDialog.InputBox("Fecha inicio de la búsqueda AAAA-MM-DD", "Consultas avanzadas", "")
                Fecha_fin = InputDialog.InputBox("Fecha final de la búsqueda AAAA-MM-DD", "Consultas avanzadas", "")
            End If
            FormularioInformes.fecha1 = Fecha_ini
            FormularioInformes.fecha2 = Fecha_fin
            FormularioInformes.Text = "Inventario del SIDCARTO clasificado por Tipo de documento"
            FormularioInformes.Informe_Resumen_PorTipoDoc()
        ElseIf sender.name = "mnuTool_Informes02" Then
            FormularioInformes.Text = "Inventario del SIDCARTO clasificado por Estado de conservación"
            FormularioInformes.Informe_Resumen_PorEstadoDoc()
        ElseIf sender.name = "mnuTool_Informes03" Then
            FormularioInformes.Text = "Último número de sellado semántico catalogado por provincia"
            FormularioInformes.Informe_UltimoDocumentoSelladoSemantico()
        ElseIf sender.name = "mnuTool_Informes04" Then
            FormularioInformes.Text = "Último número de sellado consecutivo catalogado"
            FormularioInformes.Informe_UltimoDocumentoSelladoConsecutivo()
        ElseIf sender.name = "mnuPPCnoGeo" Then
            FormularioInformes.Text = "Planos de población en cuaderno no georreferenciados"
            FormularioInformes.ListarPPCnoGeo()
        Else

        End If

    End Sub


    Sub LanzarConsultaGEO_SIDCARTO(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click

        Dim Xmax As Double = 0
        Dim Ymax As Double = 0
        Dim Xmin As Double = 0
        Dim Ymin As Double = 0
        Dim TituloConsulta As String = ""

        If TextBox11.Text <> "" And TextBox12.Text <> "" And TextBox13.Text <> "" And TextBox24.Text <> "" Then
            Dim centroUTM30 As GEOCoordenada
            Try
                Dim searchCenter As New GEOCoordenada(TextBox11.Text, TextBox12.Text, TextBox24.Text)
                centroUTM30 = searchCenter.convertTo(GEOCoordenada.srs.UTM30_SobreED50)
                Dim radioSearch = CType(TextBox13.Text, Integer)
                Xmax = centroUTM30.EastingCoord + radioSearch
                Ymax = centroUTM30.NorthingCoord + radioSearch
                Xmin = centroUTM30.EastingCoord - radioSearch
                Ymin = centroUTM30.NorthingCoord - radioSearch
            Catch ex As Exception
                ModalError(ex.Message)
                Exit Sub
            End Try

            'Compruebo si las coordenadas estan en geograficas, y si es así las paso a UTMED50 para procesar
            TituloConsulta = "Búsqueda por entorno. (" & TextBox11.Text & "," & TextBox12.Text & ") y  radio " & TextBox13.Text & " m"
        ElseIf TextBox7.Text <> "" And TextBox3.Text <> "" And TextBox9.Text <> "" And TextBox10.Text <> "" Then
            Xmax = CType(TextBox7.Text.Replace(".", ","), Double)
            Ymax = CType(TextBox3.Text.Replace(".", ","), Double)
            Xmin = CType(TextBox9.Text.Replace(".", ","), Double)
            Ymin = CType(TextBox10.Text.Replace(".", ","), Double)
            TituloConsulta = "Búsqueda por entorno. (" & Xmax.ToString & "," & Ymax.ToString & ") (" & Xmin.ToString & "," & Ymin.ToString & ")"
        End If


        If Xmax = 0 Or Ymax = 0 Or Xmin = 0 Or Ymin = 0 Then Exit Sub
        If Xmax - Xmin > 200000 Or Ymax - Ymin > 200000 Then
            ModalExclamation("Reduzca el entorno de la búsqueda")
            Exit Sub
        End If

        Dim cadWKTPolygon = $"'POLYGON(({CType(Xmin, Integer)} {CType(Ymax, Integer)},{CType(Xmax, Integer)} {CType(Ymax, Integer)},{CType(Xmax, Integer)} {CType(Ymin, Integer)},{ CType(Xmin, Integer)} {CType(Ymin, Integer)},{ CType(Xmin, Integer)} {CType(Ymax, Integer)}))'"


        Try
            Dim resp = ModalQuestCollection("¿Qué desea buscar?")
            PictureBox3.Visible = True
            Me.Cursor = Cursors.WaitCursor
            LanzarSpinner("Cargando datos")
            If resp = DialogResult.OK Then
                Dim frmResultados As New resultGEODOCAT
                frmResultados.MdiParent = Me
                frmResultados.typeSearch = resultGEODOCAT.TypeDataSearch.DocumentosByBBOX
                frmResultados.paramSQL1 = cadWKTPolygon
                frmResultados.paramSQL2 = "23030"
                frmResultados.Show()
            End If
            If resp = DialogResult.Yes Then
                Dim frmResultados As New resultCMTN
                frmResultados.MdiParent = Me
                frmResultados.typeSearch = resultCMTN.TypeDataSearch.DocumentosByBBOX
                frmResultados.paramSQL1 = cadWKTPolygon
                frmResultados.paramSQL2 = "23030"
                frmResultados.Show()
            End If
        Catch ex As Exception
            ModalError($"No se pueden identificar los documento: {ex.Message}")
        Finally
            CerrarSpinner()
            PictureBox3.Visible = False
            Me.Cursor = Cursors.Default

        End Try

    End Sub


    Sub LanzarConfiguraciones(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolConfig.Click, mnuConfiguracion.Click

        ModalInfo("La herramienta se encuentra configurada")
        Exit Sub
        Dim frmSettings As New frmSettings
        frmSettings.Show()

    End Sub


    Private Sub IndexToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                                    Handles IndexToolStripMenuItem.Click, HelpToolStripButton.Click

        Try
            If System.IO.File.Exists(My.Application.Info.DirectoryPath & "\Ayuda.pdf") = False Then
                ModalInfo("Fichero de ayuda no disponible")
            Else
                Process.Start(My.Application.Info.DirectoryPath & "\Ayuda.pdf")
            End If
        Catch ex As Exception
            ModalError(ex.Message)
        End Try


    End Sub

    Private Sub ToolStripButton11_Click(sender As Object, e As EventArgs) Handles ToolStripButton11.Click
        Try
            If Not IO.File.Exists($"{My.Application.Info.DirectoryPath}\resources\nueva-ventana-consulta-cartosee.png") Then
                ModalInfo("Fichero de ayuda no disponible")
            Else
                Process.Start($"{My.Application.Info.DirectoryPath}\resources\nueva-ventana-consulta-cartosee.png")
            End If
        Catch ex As Exception
            ModalError(ex.Message)
        End Try
    End Sub

    Sub LanzarTareasfrmDocumentacion(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                    Handles mnuExtraerContornos.Click, mnuExtraerCentroide.Click,
                            ToolStripButton3.Click, mnuResConsulta1.Click,
                            ToolMiniaturas.Click, mnuResconsulta3.Click,
                            ToolStripButton4.Click, mnuResconsulta2.Click,
                            ToolStripButton7.Click, mnuResconsulta4.Click,
                            ToolStripButton16.Click, mnuResconsulta5.Click,
                            ToolStripButton14.Click, mnuGenerarMetadatos.Click,
                            mnuResconsulta7.Click,
                            ToolStripButton8.Click, mnuResconsulta8.Click,
                            ToolStripButton18.Click, mnuResconsulta9.Click, mnuGenMiniatura.Click

        Dim frmAccion As frmDocumentacion
        Dim procesarAccion As Boolean

        Try
            frmAccion = Me.ActiveMdiChild
            If Not frmAccion Is Nothing Then procesarAccion = True
        Catch ex As Exception
            Exit Sub
        End Try

        If procesarAccion = False Then Exit Sub

        If sender.name = "ToolStripButton3" Or sender.name = "mnuResconsulta1" Then
            frmAccion.TabulacionVentanas(sender, e)
        ElseIf sender.name = "ToolMiniaturas" Or sender.name = "mnuResconsulta3" Then
            frmAccion.TabulacionVentanas(sender, e)
        ElseIf sender.name = "ToolStripButton4" Or sender.name = "mnuResconsulta2" Then
            'If frmAccion.ListView1.SelectedItems.Count > 0 Then
            '    frmAccion.TabulacionVentanas(frmAccion.ListView1.SelectedItems(0).Tag)
            'Else
            '    frmAccion.MostrarDetalle(0)
            'End If
        ElseIf sender.name = "ToolStripButton7" Or sender.name = "mnuResconsulta4" Then
            frmAccion.LanzarVisordeJPG(sender, e)
        ElseIf sender.name = "ToolStripButton16" Or sender.name = "mnuResconsulta5" Then
            frmAccion.LanzarECW(sender, e)
        ElseIf sender.name = "ToolStripButton18" Or sender.name = "mnuResconsulta9" Then
            frmAccion.LanzarImpresionJPG(sender, e)
        ElseIf sender.name = "ToolStripButton14" Or sender.name = "mnuGenerarMetadatos" Then
            frmAccion.ProcGenerarMetadatos(sender, e)
        ElseIf sender.name = "ToolStripButton11" Then


        ElseIf sender.name = "ToolStripButton8" Or sender.name = "mnuResconsulta8" Then
            frmAccion.ExportarListaResultados2CSV(sender, e)
        ElseIf sender.name = "mnuExtraerContornos" Then
            frmAccion.ExtraerContornos(sender, e)
        ElseIf sender.name = "mnuExtraerCentroide" Then
            frmAccion.ExtraerCentroides(sender, e)
        ElseIf sender.name = "mnuGenMiniatura" Then
            frmAccion.GenerarMiniaturas(sender, e)
        End If

    End Sub

    Sub LanzarConsultaAvanzada(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Query_Advance01.Click, Query_Advance02.Click, Query_Advance03.Click, Query_Advance05.Click

        Dim FrmResult As New frmDocumentacion
        Dim Filtro As String
        Dim fechaActual As Date = Now
        Dim Fecha_ini As String
        Dim Fecha_fin As String


        If sender.name = "Query_Advance01" Then
            Fecha_ini = InputDialog.InputBox("Fecha inicio de la búsqueda AAAA-MM-DD", "Consultas avanzadas", "")
            Fecha_fin = InputDialog.InputBox("Fecha final de la búsqueda AAAA-MM-DD", "Consultas avanzadas", "")
        ElseIf sender.name = "Query_Advance02" Or sender.name = "Query_Advance03" Then
            Dim fechas() As String = InputDateDialog.InputBox("Establece fechas de búsqueda")
            Fecha_ini = fechas(0)
            Fecha_fin = fechas(1)
            If Fecha_ini.Length = 4 Then Fecha_ini &= "-01-01"
            If Fecha_fin.Length = 4 Then Fecha_fin &= "-12-31"
        End If

        Dim resp = ModalQuestCollection("¿Qué desea buscar?")

        If resp = DialogResult.Yes Then
            'Consulto por Cuadernos
            Try
                PictureBox3.Visible = True
                Me.Cursor = Cursors.WaitCursor
                LanzarSpinner("Cargando datos")
                Dim frmResultadosCuadMTN As New resultCMTN
                With frmResultadosCuadMTN
                    .MdiParent = Me
                    If sender.name = "Query_Advance01" Then
                        .paramSQL1 = Fecha_ini & " 00:00:00"
                        .paramSQL2 = Fecha_fin & " 23:59:59"
                        .typeSearch = resultCMTN.TypeDataSearch.AllDocsPorFechaAlta
                    ElseIf sender.name = "Query_Advance02" Then
                        .paramSQL1 = Fecha_ini & " 00:00:00"
                        .paramSQL2 = Fecha_fin & " 23:59:59"
                        .typeSearch = resultCMTN.TypeDataSearch.AllDocsByFechaUpdate
                    ElseIf sender.name = "Query_Advance03" Then
                        .paramSQL1 = Fecha_ini
                        .paramSQL2 = Fecha_fin
                        .typeSearch = resultCMTN.TypeDataSearch.AllDocsPorFechaDocumento
                    ElseIf sender.name = "Query_Advance05" Then
                        .typeSearch = resultCMTN.TypeDataSearch.AllDocuments
                        .limitResults = "100"
                        .OrderDirection = "DESC"

                    End If
                    .Show()
                End With
            Catch ex As Exception
                ModalError($"No se pueden identificar los documento: {ex.Message}")
            Finally
                CerrarSpinner()
                PictureBox3.Visible = False
                Me.Cursor = Cursors.Default
            End Try

        End If
        If resp = DialogResult.OK Then
            'Consulto por GEODOCAT
            Try
                PictureBox3.Visible = True
                Me.Cursor = Cursors.WaitCursor
                LanzarSpinner("Cargando datos")
                Dim frmResultadosCuadMTN As New resultGEODOCAT
                With frmResultadosCuadMTN
                    .MdiParent = Me
                    If sender.name = "Query_Advance01" Then
                        .paramSQL1 = Fecha_ini & " 00:00:00"
                        .paramSQL2 = Fecha_fin & " 23:59:59"
                        .typeSearch = resultGEODOCAT.TypeDataSearch.AllDocsPorFechaAlta
                    ElseIf sender.name = "Query_Advance02" Then
                        .paramSQL1 = Fecha_ini & " 00:00:00"
                        .paramSQL2 = Fecha_fin & " 23:59:59"
                        .typeSearch = resultGEODOCAT.TypeDataSearch.AllDocsByFechaUpdate
                    ElseIf sender.name = "Query_Advance03" Then
                        .paramSQL1 = Fecha_ini
                        .paramSQL2 = Fecha_fin
                        .typeSearch = resultGEODOCAT.TypeDataSearch.AllDocsPorFechaDocumento
                    ElseIf sender.name = "Query_Advance05" Then
                        .typeSearch = resultGEODOCAT.TypeDataSearch.AllDocuments
                        .limitResults = "100"
                        .OrderDirection = "DESC"

                    End If
                    .Show()
                End With
            Catch ex As Exception
                ModalError($"No se pueden identificar los documento: {ex.Message}")
            Finally
                CerrarSpinner()
                PictureBox3.Visible = False
                Me.Cursor = Cursors.Default
            End Try



        End If


        Exit Sub


    End Sub



    Private Sub ToolStripButton13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                Handles ToolStripButton13.Click, AboutToolStripMenuItem.Click
        AboutBox1.ShowDialog()
    End Sub

    Private Sub GestionCarrito(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButCarrito.Click, mnuCarrito.Click

        If CarritoCompra Is Nothing Then ModalExclamation("El carrito esta vacío") : Exit Sub
        If CarritoCompra.Count = 0 Then ModalExclamation("El carrito esta vacío") : Exit Sub

        'Comprobamos si el carrito está ya activo
        Dim ExisteCarrito As Boolean
        For Each ChildForm As resultGEODOCAT In Me.MdiChildren
            If ChildForm.EsCarritoCompra Then
                If ChildForm.DataGridView1.RowCount <> CarritoCompra.Count Then ChildForm.btnRefresh.PerformClick()
                ChildForm.Focus()
                ExisteCarrito = True
                Exit For
            End If
        Next


        If ExisteCarrito = True Then

            Exit Sub
        End If

        Dim frmResultados As New resultGEODOCAT
        With frmResultados
            .MdiParent = Me
            .EsCarritoCompra = True
            .typeSearch = resultGEODOCAT.TypeDataSearch.DocumentosEnCarrito
            .Show()
        End With

    End Sub

    Private Sub ComboBox3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox3.Click
        TextBox1.Text = ""
    End Sub

    Private Sub ModificarAtributosDocumentos(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                        Handles mnuModDocuMedidas.Click, mnuModDocuTiposDoc.Click,
                        mnuModDocuEstados.Click, mnuModDocuObservaciones.Click, mnuVisorMosaicos.Click,
                        btnVisorMosaicos.Click, btnExportCdD.Click, mnuExportCdD.Click


        If sender.name = "mnuModDocuTiposDoc" Then
            Dim FrmEditAtrib As New frmEdicionesTablas
            FrmEditAtrib.MdiParent = Me
            FrmEditAtrib.Text = "Tipos de documentación"
            FrmEditAtrib.Tag = 1
            FrmEditAtrib.CargarDatos(1)
            FrmEditAtrib.Show()
        ElseIf sender.name = "mnuModDocuObservaciones" Then
            Dim FrmEditAtrib As New frmEdicionesTablas
            FrmEditAtrib.MdiParent = Me
            FrmEditAtrib.Text = "Oservaciones standard"
            FrmEditAtrib.Tag = 2
            FrmEditAtrib.CargarDatos(2)
            FrmEditAtrib.Show()
        ElseIf sender.name = "mnuModDocuEstados" Then
            Dim FrmEditAtrib As New frmEdicionesTablas
            FrmEditAtrib.MdiParent = Me
            FrmEditAtrib.Text = "Estados de Conservación"
            FrmEditAtrib.Tag = 3
            FrmEditAtrib.CargarDatos(3)
            FrmEditAtrib.Show()
        ElseIf sender.name = "mnuModDocuMedidas" Then
            Dim FrmEditAtrib As New frmEdicionesTablas
            FrmEditAtrib.MdiParent = Me
            FrmEditAtrib.Text = "Unidades de Medida"
            FrmEditAtrib.Tag = 4
            FrmEditAtrib.CargarDatos(4)
            FrmEditAtrib.Show()
        ElseIf sender.name = "mnuVisorMosaicos" Or sender.name = "btnVisorMosaicos" Then
            Dim FrmEditAtrib As New FrmEdicionesMosaicos
            FrmEditAtrib.MdiParent = Me
            FrmEditAtrib.CargarDatos()
            FrmEditAtrib.Show()
        ElseIf sender.name = "mnuExportCdD" Or sender.name = "btnExportCdD" Then
            Dim frmExportacion As New frmExportCdD
            frmExportacion.MdiParent = Me
            frmExportacion.Show()
        End If
    End Sub


    Private Sub itemUsermenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click, ToolStripStatusLabel3.Click


        Dim frmUser As New GestionUserNotificacion
        frmUser.MdiParent = Me
        frmUser.Show()

    End Sub


    Private Sub ManagerUserActions(sender As Object, e As EventArgs) Handles itemChangePass.Click, itemGestionUser.Click, mnuListaPermisos.Click, mnuOpenPreferenceFolder.Click, mnuOpenLoggerFile.Click

        If sender.name = "itemChangePass" Then
            Dim procChangePass As ChangeCredentials
            procChangePass = DialogChangePassword.InputBox()
            usuarioMyApp.cambiarPassword(procChangePass.OldPassword, procChangePass.NewPassword)
        End If
        If sender.name = "itemGestionUser" Then
            If usuarioMyApp.permisosLista.AsignarPermisosUsuarios = False Then Exit Sub
            Dim frmUser As GestionUserForm
            frmUser = New GestionUserForm
            frmUser.MdiParent = Me
            frmUser.Show()
        End If
        If sender.name = "mnuListaPermisos" Then
            Try
                ListDialog.ListaBox(usuarioMyApp.permisosLista.DameResumenProps(), $"Usuario {usuarioMyApp.LoginUser}", "Lista de permisos")
            Catch ex As Exception
                ModalExclamation(ex.Message)
            End Try
        End If

        If sender.name = "mnuOpenPreferenceFolder" Then
            Try
                Process.Start(AppFolderSetting)
            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
        If sender.name = "mnuOpenLoggerFile" Then
            Try
                If System.IO.File.Exists("C: \Program Files (x86)\Notepad++\notepad++.exe") Then
                    Process.Start("C:\Program Files (x86)\Notepad++\notepad++.exe", ficheroLogger)
                Else
                    Process.Start(ficheroLogger)
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If




    End Sub


    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click

        Try
            Process.Start(visorIberpix)
        Catch ex As Exception
            ModalError(ex.Message)
            Exit Sub
        End Try

    End Sub

    Private Sub mnuLinkCdDMinutas_Click(sender As Object, e As EventArgs) Handles mnuLinkCdDMIPAC.Click, mnuLinkCdDPLPOB.Click, mnuLinkCdDPLEDI.Click, mnuLinkCdDHKPUP.Click, mnuLinkCdDAT.Click, mnuLinkCdDCCINT.Click


        If sender.name = "mnuLinkCdDAT" Then
            Try
                Process.Start("https://centrodedescargas.cnig.es/CentroDescargas/catalogo.do?Serie=HKPUP")
            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End Try
            Exit Sub
        End If
        If String.IsNullOrEmpty(TextBox1.Tag) Then
            MessageBox.Show("Seleccione un municipio", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If


        Dim terriLink As TerritorioBSID
        Dim CodMunicipioINEHistorico As String
        Dim CodMunicipioINEActual As String
        Dim MunicipioID As String
        Dim linkCdD As String = "https://centrodedescargas.cnig.es/CentroDescargas/buscar.do?"

        Try
            Dim CodigosMuni() As String = TextBox1.Tag.ToString.Split("|")
            If CodigosMuni.Length = 3 Then
                CodMunicipioINEHistorico = CodigosMuni(0)
                MunicipioID = CodigosMuni(1)
                CodMunicipioINEActual = CodigosMuni(2)
            End If
            Application.DoEvents()

            terriLink = New TerritorioBSID(CodMunicipioINEActual)
            Application.DoEvents()

            If sender.name = "mnuLinkCdDMIPAC" Then
                linkCdD &= $"filtro.codFamilia=MIPAC&filtro.codIne={terriLink.getCodigoINEFull}"
            ElseIf sender.name = "mnuLinkCdDPLPOB" Then
                linkCdD &= $"filtro.codFamilia=PLPOB&filtro.codIne={terriLink.getCodigoINEFull}"
            ElseIf sender.name = "mnuLinkCdDPLEDI" Then
                linkCdD &= $"filtro.codFamilia=PLEDI&filtro.codIne={terriLink.getCodigoINEFull}"
            ElseIf sender.name = "mnuLinkCdDHKPUP" Then
                linkCdD &= $"filtro.codFamilia=HKPUP&filtro.codIne={terriLink.getCodigoINEFull}"
            ElseIf sender.name = "mnuLinkCdDCCINT" Then
                linkCdD &= $"filtro.codFamilia=CCINT&filtro.codIne={terriLink.getCodigoINEFull}"
            End If
            Process.Start(linkCdD)
        Catch ex As Exception
            ModalError(ex.Message)
        End Try





    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Dim linkCoordenadas As String = ""
        Dim cadAnaliz As String = ""
        Dim partesAnaliz() As String
        Dim cadCoors() As String
        Dim cadZoom As String
        Dim cadEPSG As String
        If My.Computer.Clipboard.ContainsText Then

            linkCoordenadas = My.Computer.Clipboard.GetText()
            'MessageBox.Show(linkCoordenadas)
        End If


        If linkCoordenadas.StartsWith("https://www.cartociudad.es/visor?") Then
            cadAnaliz = linkCoordenadas.Replace("https://www.cartociudad.es/visor?", "")
        ElseIf linkCoordenadas.StartsWith("http://www.cartociudad.es/visor?") Then
            cadAnaliz = linkCoordenadas.Replace("http://www.cartociudad.es/visor?", "")
        ElseIf linkCoordenadas.StartsWith("https://www.cartociudad.es/visor/?") Then
            cadAnaliz = linkCoordenadas.Replace("https://www.cartociudad.es/visor/?", "")
        ElseIf linkCoordenadas.StartsWith("http://www.cartociudad.es/visor/?") Then
            cadAnaliz = linkCoordenadas.Replace("http://www.cartociudad.es/visor/?", "")
        Else
            'Probamos a parsearlas
            partesAnaliz = linkCoordenadas.Split(",")
            If partesAnaliz.Length = 4 Then
                Application.DoEvents()
                If partesAnaliz(3).ToUpper.StartsWith("EPSG:") Then
                    TextBox11.Text = partesAnaliz(0)
                    TextBox12.Text = partesAnaliz(1)
                    TextBox13.Text = "5000"
                    TextBox24.Text = partesAnaliz(3).ToUpper.Replace("EPSG:", "")
                End If
                Exit Sub
            Else
                ModalExclamation("No se encuentran coordendas en el enlace  del portapapeles")
                Exit Sub
            End If
        End If
        partesAnaliz = cadAnaliz.Split("&")
        If partesAnaliz(0).StartsWith("center=") = False Or partesAnaliz(1).StartsWith("zoom=") = False Or partesAnaliz(2).StartsWith("srs=") = False Then
            MessageBox.Show("No se encuentran coordendas en el enlace  del portapapeles." & System.Environment.NewLine & linkCoordenadas, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        cadCoors = partesAnaliz(0).Replace("center=", "").Split(",")
        cadZoom = partesAnaliz(1).Replace("zoom=", "")
        cadEPSG = partesAnaliz(2).Replace("srs=", "")

        Dim posicionCaptur As New GEOCoordenada
        Dim salida As New GEOCoordenada
        Try
            posicionCaptur = New GEOCoordenada(cadCoors(0), cadCoors(1), GEOCoordenada.srs.GOOGLE_SphericalMercator)
            salida = posicionCaptur.convertTo(GEOCoordenada.srs.GEO_ETRS89)
            TextBox11.Text = Math.Round(posicionCaptur.EastingCoord, 0)
            TextBox12.Text = Math.Round(posicionCaptur.NorthingCoord, 0)
            'Monitores con resolución 1920x1080 -> Altura de la venta de cartociudad en navegador maximizado -> 800 píxeles
            'Monitores con resolución 3440x1440 -> Altura de la venta de cartociudad en navegador maximizado -> 1200 píxeles
            TextBox13.Text = Math.Round(salida.getPixelSize(cadZoom, 256) * System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height * 0.85, 0) ' Considero tamaños de Tile de 256 píxeles y que el mapa ocupaba el 85% de la altura del navegador
            TextBox24.Text = cadEPSG.ToLower.Replace("*m", "").Replace("epsg:", "")
        Catch ex As Exception
            MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try




    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click

        Dim urlCdD As String

        If TextBox11.Text <> "" And TextBox12.Text <> "" And TextBox24.Text <> "" Then

            'Dim searchCenter As New GEOCoordenada(TextBox11.Text, TextBox12.Text, TextBox24.Text)
            Dim centroGeoWGS84 As GEOCoordenada
            Try
                Dim searchCenter As New GEOCoordenada(TextBox11.Text, TextBox12.Text, TextBox24.Text)
                centroGeoWGS84 = searchCenter.convertTo(GEOCoordenada.srs.GEO_WGS84)
                urlCdD = "https://centrodedescargas.cnig.es/CentroDescargas/buscador.do?crs=EPSG:3857&BBOX=" &
                                centroGeoWGS84.EastingCoord.ToString.Replace(",", ".") & "," &
                                centroGeoWGS84.NorthingCoord.ToString.Replace(",", ".") & "," &
                                centroGeoWGS84.EastingCoord.ToString.Replace(",", ".") & "," &
                                centroGeoWGS84.NorthingCoord.ToString.Replace(",", ".")

                Process.Start(urlCdD)
            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo)
            End Try

        End If

    End Sub

    Private Sub LaunchQuery(sender As Object, e As EventArgs) Handles Button12.Click, btnGetStar.Click, btnGetImportant.Click, btnGetWeird.Click, btnGetCdD.Click, ToolStripButton12.Click

        Dim FirmaYear As String = ""
        Dim EstadosDocumento As String = ""
        Dim TiposDocumento As String = ""
        Dim numTomo As String = ""
        Dim DescripFiltro As String = ". "
        Dim territorioId As Integer = 0
        Dim CodMunicipioINEHistorico As Integer = 0
        Dim CodMunicipioINEActual As Integer = 0
        Dim nSellado As String = ""
        Dim nSellado1 As String = ""
        Dim nSellado2 As String = ""
        Dim listaSellos As New ArrayList
        Dim CadFiltro As String = ""
        Dim ibucle As Integer
        Dim cProv As Integer = 0
        Dim proceHoja As String
        Dim proceCarpeta As String

        If Not String.IsNullOrEmpty(TextBox1.Tag) Then
            Dim CodigosMuni() As String = TextBox1.Tag.ToString.Split("|")
            CodMunicipioINEHistorico = CodigosMuni(0)
            territorioId = CodigosMuni(1)
            CodMunicipioINEActual = CodigosMuni(2)
        ElseIf Not String.IsNullOrEmpty(TextBox23.Text) Then
            If IsNumeric(TextBox23.Text.Trim) Then
                nSellado = TextBox23.Text.Trim
            ElseIf obtenerIntervalo(TextBox23.Text.Trim, "-", nSellado1, nSellado2) = True Then
                Application.DoEvents()
            ElseIf obtenerIntervalo(TextBox23.Text.Trim, "#", nSellado1, nSellado2) = True Then
                Application.DoEvents()
            ElseIf obtenerIntervalo(TextBox23.Text.Trim, ";", listaSellos) = True Then
                Application.DoEvents()
            Else
                Exit Sub
            End If

        Else

            If ComboBox3.SelectedIndex <> -1 Then
                cProv = CType(ComboBox3.SelectedItem, itemData).Valor
            ElseIf IsNumeric(TextBox1.Text.Trim) Then
                nSellado = TextBox1.Text.Trim
            ElseIf obtenerIntervalo(TextBox1.Text.Trim, "-", nSellado1, nSellado2) = True Then
                Application.DoEvents()
            ElseIf obtenerIntervalo(TextBox1.Text.Trim, "#", nSellado1, nSellado2) = True Then
                Application.DoEvents()
            ElseIf obtenerIntervalo(TextBox1.Text.Trim, ";", listaSellos) = True Then
                Application.DoEvents()
            Else
                Exit Sub
            End If
        End If

        '-----------------------------------------------------------------------------------
        'Evalúo si se filtran los documentos por estado o por tipo
        '-----------------------------------------------------------------------------------
        TiposDocumento = ""
        For Each Linea As itemData In CheckedListBox1.Items
            If CheckedListBox1.GetItemChecked(CheckedListBox1.Items.IndexOf(Linea)) Then
                TiposDocumento &= $"{IIf(TiposDocumento = "", Linea.Valor, $",{Linea.Valor}")}"
            End If
        Next
        If CheckedListBox1.Items.Count = CheckedListBox1.CheckedItems.Count Then TiposDocumento = ""

        EstadosDocumento = ""
        If ComboBox7.SelectedIndex <> -1 Then EstadosDocumento = CType(ComboBox7.SelectedItem, itemData).Valor






        'Extraemos otros filtros
        numTomo = TextBox5.Text.Trim
        proceHoja = TextBox4.Text.Trim
        proceCarpeta = TextBox2.Text.Trim


        Try
            PictureBox3.Visible = True
            Me.Cursor = Cursors.WaitCursor
            LanzarSpinner("Cargando datos")
            Dim frmResultados As New resultGEODOCAT
            With frmResultados
                .MdiParent = Me
                .filterTipoDoc = TiposDocumento
                .filterSubTipoDoc = TextBox22.Text.Trim
                .filterEstadoDoc = EstadosDocumento
                .filterFecha = FirmaYear
                .filterJGE = IIf(ComboBox6.SelectedIndex > 0, ComboBox6.Text, "")
                .filterEnABSYS = IIf(ComboBox8.SelectedIndex > 0, ComboBox8.Text, "")




                If TextBox23.Text.Trim <> "" Then
                    If IsNumeric(TextBox23.Text) Then
                        .paramSQL1 = TextBox23.Text.Trim
                        .typeSearch = resultGEODOCAT.TypeDataSearch.DocumentosBySellado
                    ElseIf obtenerIntervalo(TextBox23.Text.Trim, "-", nSellado1, nSellado2) = True Then
                        Application.DoEvents()
                        .paramSQL1 = nSellado1
                        .paramSQL2 = nSellado2
                        .typeSearch = resultGEODOCAT.TypeDataSearch.DocumentosByListaNumSelladoEntreLimites
                    ElseIf obtenerIntervalo(TextBox23.Text.Trim, "#", nSellado1, nSellado2) = True Then
                        Application.DoEvents()
                        .paramSQL1 = nSellado1
                        .paramSQL2 = nSellado2
                        .typeSearch = resultGEODOCAT.TypeDataSearch.DocumentosByListaNumSelladoEntreLimites
                    ElseIf obtenerIntervalo(TextBox23.Text.Trim, ";", nSellado1, nSellado2) = True Then
                        Application.DoEvents()
                        .paramSQL1 = nSellado1
                        .paramSQL2 = nSellado2
                        .typeSearch = resultGEODOCAT.TypeDataSearch.DocumentosByListaNumSelladoEntreLimites
                    End If
                ElseIf TextBox6.Text.Trim <> "" Then
                    Application.DoEvents()
                    .paramSQL1 = TextBox6.Text.Trim
                    .typeSearch = resultGEODOCAT.TypeDataSearch.DocumentosBySignatura
                ElseIf TextBox8.Text.Trim <> "" Then
                    Application.DoEvents()
                    .paramSQL1 = TextBox8.Text.Trim
                    .typeSearch = resultGEODOCAT.TypeDataSearch.DocumentosByColeccion
                ElseIf TextBox19.Text.Trim <> "" Then
                    Application.DoEvents()
                    .paramSQL1 = TextBox19.Text.Trim
                    .typeSearch = resultGEODOCAT.TypeDataSearch.DocumentosByAnejo
                ElseIf TextBox20.Text.Trim <> "" Then
                    Application.DoEvents()
                    .paramSQL1 = TextBox20.Text.Trim
                    .typeSearch = resultGEODOCAT.TypeDataSearch.DocumentosByComentario
                ElseIf sender.name = "btnGetStar" Then
                    Application.DoEvents()
                    .paramSQL1 = "____1___________"
                    .typeSearch = resultGEODOCAT.TypeDataSearch.DocumentosByPatron
                    .Text = "Documentos importantes"
                ElseIf sender.name = "btnGetImportant" Then
                    Application.DoEvents()
                    .paramSQL1 = "__1_____________"
                    .typeSearch = resultGEODOCAT.TypeDataSearch.DocumentosByPatron
                    .Text = "Documentos destacados"
                ElseIf sender.name = "btnGetWeird" Then
                    Application.DoEvents()
                    .paramSQL1 = "_1______________"
                    .typeSearch = resultGEODOCAT.TypeDataSearch.DocumentosByPatron
                    .Text = "Documentos raros"
                ElseIf sender.name = "btnGetCdD" Then
                    Application.DoEvents()
                    .paramSQL1 = "1_______________"
                    .typeSearch = resultGEODOCAT.TypeDataSearch.DocumentosByPatron
                    .Text = "Documentos  pendientes CdD"

                ElseIf numTomo <> "" Then
                    Application.DoEvents()
                    If cProv = 0 Then
                        ModalExclamation("Para buscar por Tomo, seleccione primero una provincia")
                        frmResultados.Close()
                        frmResultados.Dispose()
                        frmResultados = Nothing
                        Exit Sub
                    End If
                    .paramSQL1 = cProv
                    .filterTomo = numTomo
                    .typeSearch = resultGEODOCAT.TypeDataSearch.AllDocumentsByProvincia
                ElseIf proceCarpeta <> "" Or proceHoja <> "" Then
                    If proceHoja = "" And proceCarpeta <> "" Then
                        ModalExclamation("Si busca una carpeta, debe especificar la Hoja")
                        frmResultados.Close()
                        frmResultados.Dispose()
                        frmResultados = Nothing
                        Exit Sub
                    End If
                    .paramSQL1 = proceHoja
                    .paramSQL2 = proceCarpeta
                    .typeSearch = resultGEODOCAT.TypeDataSearch.DocumentosByProcHojaCarpeta
                ElseIf TextBox1.Tag.Trim <> "" Then
                    If CheckBox1.Checked Then
                        'Búsqueda por territorio/municipio actual. Usamos en la búsqueda el códigoINE actual
                        .paramSQL1 = CodMunicipioINEActual
                        .typeSearch = resultGEODOCAT.TypeDataSearch.AllDocumentsByTerritorioActual
                        .Text = $"Documentos asociados al municipio actual {TextBox1.Text.Trim}"
                    Else
                        'Búsqueda por territorio/municipio histórico. Usamos en la búsqueda el idTerritorio
                        .paramSQL1 = territorioId
                        .typeSearch = resultGEODOCAT.TypeDataSearch.AllDocumentsByTerritorio
                        .Text = $"Documentos asociados al municipio {TextBox1.Text.Trim}"
                    End If
                Else
                    If cProv > 0 Then
                        .paramSQL1 = cProv
                        .typeSearch = resultGEODOCAT.TypeDataSearch.AllDocumentsByProvincia
                    Else
                        .typeSearch = resultGEODOCAT.TypeDataSearch.AllDocuments
                    End If
                End If


                .Show()
            End With






        Catch ex As Exception
            ModalError($"No se pueden identificar los documento: {ex.Message}")
        Finally
            CerrarSpinner()
            PictureBox3.Visible = False
            Me.Cursor = Cursors.Default

        End Try








    End Sub

    Private Sub OldSearchGEO(sender As Object, e As EventArgs) Handles mnuOldSearchGEO.Click

        Dim Xmax As Double = 0
        Dim Ymax As Double = 0
        Dim Xmin As Double = 0
        Dim Ymin As Double = 0
        Dim TituloConsulta As String = ""

        If TextBox11.Text <> "" And TextBox12.Text <> "" And TextBox13.Text <> "" And TextBox24.Text <> "" Then

            'Dim searchCenter As New GEOCoordenada(TextBox11.Text, TextBox12.Text, TextBox24.Text)
            Dim centroUTM30 As GEOCoordenada
            Try
                Dim searchCenter As New GEOCoordenada(TextBox11.Text, TextBox12.Text, TextBox24.Text)
                centroUTM30 = searchCenter.convertTo(GEOCoordenada.srs.UTM30_SobreED50)
                Dim radioSearch = CType(TextBox13.Text, Integer)
                Xmax = centroUTM30.EastingCoord + radioSearch
                Ymax = centroUTM30.NorthingCoord + radioSearch
                Xmin = centroUTM30.EastingCoord - radioSearch
                Ymin = centroUTM30.NorthingCoord - radioSearch
            Catch ex As Exception
                ModalError(ex.Message)
                Exit Sub
            End Try

            'Compruebo si las coordenadas estan en geograficas, y si es así las paso a UTMED50 para procesar
            TituloConsulta = "Búsqueda por entorno. (" & TextBox11.Text & "," & TextBox12.Text & ") y  radio " & TextBox13.Text & " m"
        ElseIf TextBox7.Text <> "" And TextBox3.Text <> "" And TextBox9.Text <> "" And TextBox10.Text <> "" Then
            Xmax = CType(TextBox7.Text.Replace(".", ","), Double)
            Ymax = CType(TextBox3.Text.Replace(".", ","), Double)
            Xmin = CType(TextBox9.Text.Replace(".", ","), Double)
            Ymin = CType(TextBox10.Text.Replace(".", ","), Double)
            TituloConsulta = "Búsqueda por entorno. (" & Xmax.ToString & "," & Ymax.ToString & ") (" & Xmin.ToString & "," & Ymin.ToString & ")"
        End If

        Application.DoEvents()

        If Xmax = 0 Or Ymax = 0 Or Xmin = 0 Or Ymin = 0 Then Exit Sub
        If Xmax - Xmin > 200000 Or Ymax - Ymin > 200000 Then
            ModalExclamation("Reduzca el entorno de la búsqueda")
            Exit Sub
        End If

        PictureBox4.Visible = True
        Dim FrmResult As New frmDocumentacion
        FrmResult.MdiParent = Me
        FrmResult.Text = TituloConsulta
        FrmResult.CargarDatosSIDCARTO_By_Entorno(CType(Xmax, Integer), CType(Ymax, Integer), CType(Xmin, Integer), CType(Ymin, Integer))
        FrmResult.Show()
        PictureBox4.Visible = False

    End Sub

    Private Sub LaunchQueryCuadMTN(sender As Object, e As EventArgs) Handles Button13.Click, ToolStripButton22.Click, btnPendingCatalog.Click, btnInvestigar.Click

        Dim FirmaYear As String = ""
        Dim EstadosDocumento As String = ""
        Dim TiposDocumento As String = ""
        Dim numTomo As String = ""
        Dim DescripFiltro As String = ". "
        Dim territorioId As Integer = 0
        Dim CodMunicipioINEHistorico As Integer = 0
        Dim CodMunicipioINEActual As Integer = 0
        Dim nSellado As String = ""
        Dim nSellado1 As String = ""
        Dim nSellado2 As String = ""
        Dim listaSellos As New ArrayList
        Dim CadFiltro As String = ""
        Dim ibucle As Integer
        Dim cProv As Integer = 0
        Dim proceHoja As String
        Dim proceCarpeta As String


        If Not String.IsNullOrEmpty(TextBox1.Tag) Then
            Dim CodigosMuni() As String = TextBox1.Tag.ToString.Split("|")
            CodMunicipioINEHistorico = CodigosMuni(0)
            territorioId = CodigosMuni(1)
            CodMunicipioINEActual = CodigosMuni(2)
        ElseIf Not String.IsNullOrEmpty(TextBox23.Text) Then
            If IsNumeric(TextBox23.Text.Trim) Then
                nSellado = TextBox23.Text.Trim
            ElseIf obtenerIntervalo(TextBox23.Text.Trim, "-", nSellado1, nSellado2) = True Then
                Application.DoEvents()
            ElseIf obtenerIntervalo(TextBox23.Text.Trim, "#", nSellado1, nSellado2) = True Then
                Application.DoEvents()
            ElseIf obtenerIntervalo(TextBox23.Text.Trim, ";", listaSellos) = True Then
                Application.DoEvents()
            Else
                Exit Sub
            End If

        Else

            If ComboBox3.SelectedIndex <> -1 Then
                cProv = CType(ComboBox3.SelectedItem, itemData).Valor
            ElseIf IsNumeric(TextBox1.Text.Trim) Then
                nSellado = TextBox1.Text.Trim
            ElseIf obtenerIntervalo(TextBox1.Text.Trim, "-", nSellado1, nSellado2) = True Then
                Application.DoEvents()
            ElseIf obtenerIntervalo(TextBox1.Text.Trim, "#", nSellado1, nSellado2) = True Then
                Application.DoEvents()
            ElseIf obtenerIntervalo(TextBox1.Text.Trim, ";", listaSellos) = True Then
                Application.DoEvents()
            Else
                Exit Sub
            End If
        End If


        'Extraemos otros filtros
        numTomo = TextBox5.Text.Trim
        proceHoja = TextBox4.Text.Trim
        proceCarpeta = TextBox2.Text.Trim

        Try
            PictureBox3.Visible = True
            Me.Cursor = Cursors.WaitCursor
            LanzarSpinner("Cargando datos")
            Dim frmResultadosCuadMTN As New resultCMTN
            With frmResultadosCuadMTN
                .MdiParent = Me
                .filterFecha = FirmaYear
                If TextBox23.Text.Trim <> "" Then
                    If IsNumeric(TextBox23.Text.Replace(",", "_").Replace(".", "_")) Then
                        .paramSQL1 = TextBox23.Text.Trim
                        .typeSearch = resultCMTN.TypeDataSearch.DocumentosBySellado
                    ElseIf obtenerIntervalo(TextBox23.Text.Trim, ",", nSellado1, nSellado2) = True Then
                        .paramSQL1 = nSellado1
                        .paramSQL2 = nSellado2
                        .typeSearch = resultCMTN.TypeDataSearch.DocumentosByListaNumSelladoEntreLimites
                    ElseIf obtenerIntervalo(TextBox23.Text.Trim, "-", nSellado1, nSellado2) = True Then
                        .paramSQL1 = nSellado1
                        .paramSQL2 = nSellado2
                        .typeSearch = resultCMTN.TypeDataSearch.DocumentosByListaNumSelladoEntreLimites
                    ElseIf obtenerIntervalo(TextBox23.Text.Trim, "#", nSellado1, nSellado2) = True Then
                        .paramSQL1 = nSellado1
                        .paramSQL2 = nSellado2
                        .typeSearch = resultCMTN.TypeDataSearch.DocumentosByListaNumSelladoEntreLimites
                    ElseIf obtenerIntervalo(TextBox23.Text.Trim, ";", nSellado1, nSellado2) = True Then
                        .paramSQL1 = nSellado1
                        .paramSQL2 = nSellado2
                        .typeSearch = resultCMTN.TypeDataSearch.DocumentosByListaNumSelladoEntreLimites
                    End If
                ElseIf sender.name = "btnPendingCatalog" Then
                    Application.DoEvents()
                    .paramSQL1 = "____________1___"
                    .typeSearch = resultCMTN.TypeDataSearch.DocumentosByPatron
                    .Text = "Documentos pendientes de ser catalogados"
                ElseIf sender.name = "btnInvestigar" Then
                    Application.DoEvents()
                    .paramSQL1 = "___________1____"
                    .typeSearch = resultCMTN.TypeDataSearch.DocumentosByPatron
                    .Text = "Documentos incorporados desde SIDDAE para revisar su catalogación"
                ElseIf TextBox6.Text.Trim <> "" Then
                    Application.DoEvents()
                    .paramSQL1 = TextBox6.Text.Trim
                    .typeSearch = resultCMTN.TypeDataSearch.DocumentosBySignatura
                ElseIf TextBox19.Text.Trim <> "" Then
                    Application.DoEvents()
                    .paramSQL1 = TextBox19.Text.Trim
                    .typeSearch = resultCMTN.TypeDataSearch.DocumentosByAnejo
                ElseIf TextBox20.Text.Trim <> "" Then
                    Application.DoEvents()
                    .paramSQL1 = TextBox20.Text.Trim
                    .typeSearch = resultCMTN.TypeDataSearch.DocumentosByComentario
                ElseIf numTomo <> "" Then
                    If cProv = 0 Then
                        ModalExclamation("Para buscar por Tomo, seleccione primero una provincia")
                        frmResultadosCuadMTN.Close()
                        frmResultadosCuadMTN.Dispose()
                        frmResultadosCuadMTN = Nothing
                        Exit Sub
                    End If
                    .paramSQL1 = cProv
                    .filterTomo = numTomo
                    .typeSearch = resultCMTN.TypeDataSearch.AllDocumentsByProvincia
                ElseIf proceCarpeta <> "" Or proceHoja <> "" Then
                    If proceHoja = "" And proceCarpeta <> "" Then
                        ModalExclamation("Si busca una carpeta, debe especificar la Hoja")
                        frmResultadosCuadMTN.Close()
                        frmResultadosCuadMTN.Dispose()
                        frmResultadosCuadMTN = Nothing
                        Exit Sub
                    End If
                    .paramSQL1 = proceHoja
                    .paramSQL2 = proceCarpeta
                    .typeSearch = resultCMTN.TypeDataSearch.DocumentosByProcHojaCarpeta
                ElseIf TextBox1.Tag.Trim <> "" Then
                    If CheckBox1.Checked Then
                        'Búsqueda por territorio/municipio actual. Usamos en la búsqueda el códigoINE actual
                        .paramSQL1 = CodMunicipioINEActual
                        .typeSearch = resultCMTN.TypeDataSearch.AllDocumentsByTerritorioActual
                        .Text = $"Documentos asociados al municipio actual {TextBox1.Text.Trim}"
                    Else
                        'Búsqueda por territorio/municipio histórico. Usamos en la búsqueda el idTerritorio
                        .paramSQL1 = territorioId
                        .typeSearch = resultCMTN.TypeDataSearch.AllDocumentsByTerritorio
                        .Text = $"Documentos asociados al municipio {TextBox1.Text.Trim}"
                    End If
                Else
                    If cProv > 0 Then
                        .paramSQL1 = cProv
                        .typeSearch = resultCMTN.TypeDataSearch.AllDocumentsByProvincia
                    Else
                        .typeSearch = resultCMTN.TypeDataSearch.AllDocuments
                    End If
                End If

                .Show()
            End With
        Catch ex As Exception
            ModalError($"No se pueden identificar los documento: {ex.Message}")
        Finally
            CerrarSpinner()
            PictureBox3.Visible = False
            Me.Cursor = Cursors.Default
        End Try


    End Sub

End Class
