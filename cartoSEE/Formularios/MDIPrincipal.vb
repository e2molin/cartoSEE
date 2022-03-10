Imports System.Windows.Forms
Imports System.Configuration


Public Class MDIPrincipal

    Dim Autocompletar_municipios As Boolean
    Dim Autocompletar_colindantes As Boolean
    Dim Colindantes As New DataTable
    Dim quitarAcento As New Destildator
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



    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click, ToolStripButton19.Click, _
                            mnuDeveloperTools.Click, mnuOpenAppFolderSetting.Click, mnuAdminTools.Click

        If sender.name = "ToolStripButton1" Or sender.name = "mnuDeveloperTools" Then
            Dim Desarrollo As New frmDevel
            Desarrollo.MdiParent = Me
            Desarrollo.Show()
        ElseIf sender.name = "ToolStripButton19" Or sender.name = "mnuAdminTools" Then
            Dim Desarrollo As New frmDevelIGN
            Desarrollo.MdiParent = Me
            Desarrollo.Show()
        ElseIf sender.name = "mnuOpenAppFolderSetting" Then
            Try
                Process.Start(AppFolderSetting)
            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
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
        '-------------------------------------Autenticaci�n del usuario
        If modoDelegaciones = True Then
            Application.DoEvents()
            App_User = ""
            App_Pass = ""
        Else
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
        End If
        '-------------------------------------Resizing inicial
        If Screen.PrimaryScreen.Bounds.Width > 1025 Then
            Me.Size = New Point(1200, 800)
        End If
        Me.Text = AplicacionTitulo
        '--------------------------Conexi�n a la base de datos
        Dim startTicks As Long = DateTime.Now.Ticks
        If ConectarBD(TiposBase.PostgreSQL, DB_Servidor, DB_Port, DB_User, DB_Pass, DB_Instancia, "") = True Then
            'Validamos el usuario para conocer sus permisos
            App_Permiso = AutenticarUsuario(App_User, App_Pass)
            If App_Permiso = 0 Then
                If MainConex.State <> ConnectionState.Closed Then DesconectarBD()
                Application.DoEvents()
                MessageBox.Show("El usuario [" & App_User & "] no dispone de acceso a la aplicaci�n. " & System.Environment.NewLine & _
                                "Consulte con el administrador del sistema.", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End
            End If
            GenerarLOG("Inicio sesi�n en " & DB_Servidor)
            CargarListasProvincias()
            CargarListaTiposDocumento()
            ''----Lanzamos un hilo para la carga de los municipios, que es m�s lenta
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
            Me.Show()
            If App_Permiso = 2 Then
                ToolStripStatusLabel.Text = "Conectado a " & DB_Instancia & " en " & DB_Servidor & ". Usuario de administraci�n."
            Else
                ToolStripStatusLabel.Text = "Conectado al sistema. Usuario de consulta."
                ToolStripStatusLabel.ToolTipText = "Conectado a " & DB_Instancia & " en " & DB_Servidor & ""
            End If
        Else
            MessageBox.Show("No es posible conectarse a la base de datos", My.Application.Info.AssemblyName, _
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            ResizingElements()
            Button6.Enabled = False
            Button7.Enabled = False
            Button3.Enabled = False
            TextBox1.Enabled = False
            ToolStripButton19.Enabled = False
            ToolStripMenuItem2.Enabled = False
            mnuTool_Informes.Enabled = False
            ToolStripStatusLabel.Text = "Modo de Configuraci�n. Consultas no disponibles"
        End If

        'Dim endTicks As Long = DateTime.Now.Ticks
        'ToolStripStatusLabel.Text = "Tiempo: " & (endTicks - startTicks) / 100 & " ns."
        ToolStripStatusLabel1.Text = App_User
        ToolStripStatusLabel2.Text = Now.ToLongDateString.ToString
        My.Forms.SplashScreen.Dispose()
        TextBox7.Text = "575438"
        TextBox10.Text = "4518765"
        TextBox9.Text = "617612"
        TextBox3.Text = "4540893"

        mnuOpenPreferenceFolder.Visible = usuario_ISTARI
        mnuOpenLoggerFile.Visible = usuario_ISTARI
    End Sub

    Sub CargarFiltros()


        Dim Filtros As DataTable

        Filtros = New DataTable
        If CargarDatatable("Select idestadodoc,estadodoc FROM bdsidschema.tbestadodocumento", Filtros) = False Then
            MessageBox.Show("No se puede acceder a la tabla de Estados de la documentaci�n",
                            AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            CheckedListBox2.Items.Clear()
            For Each Filtro As DataRow In Filtros.Select
                CheckedListBox2.Items.Add(New itemData(Filtro.ItemArray(1).ToString, Filtro.ItemArray(0)))
            Next
        End If
        Filtros.Dispose()
        Filtros = Nothing

        Filtros = New DataTable
        If CargarDatatable("Select idtipodoc,tipodoc from bdsidschema.tbtipodocumento", Filtros) = False Then
            MessageBox.Show("No se puede acceder a la tabla de Tipos de documentaci�n",
                            AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            CheckedListBox1.Items.Clear()
            For Each Filtro As DataRow In Filtros.Select
                CheckedListBox1.Items.Add(New itemData(Filtro.ItemArray(1).ToString, Filtro.ItemArray(0)))
            Next
        End If
        Filtros.Dispose()
        Filtros = Nothing

        Filtros = New DataTable
        If CargarDatatable("Select idobservestandar,observestandar from bdsidschema.tbobservaciones", Filtros) = False Then
            MessageBox.Show("No se puede acceder a la tabla de Tipos de documentaci�n",
                            AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            ComboBox7.Items.Clear()
            For Each Filtro As DataRow In Filtros.Select
                ComboBox7.Items.Add(New itemData(Filtro.ItemArray(1).ToString, Filtro.ItemArray(0)))
            Next
        End If
        Filtros.Dispose()
        Filtros = Nothing

        'Plantilla de metadatos
        For iBucle As Integer = 0 To RutasPlantillasMetadatos.Length - 1
            RutasPlantillasMetadatos(iBucle).RutaMetadatosTipo = _
                        LeeIni("Metadatos", "RutaPlantilla" & RutasPlantillasMetadatos(iBucle).idTipodoc.ToString)
            RutasPlantillasMetadatos(iBucle).PrefijoNom = _
                        LeeIni("Metadatos", "PrefijoNom" & RutasPlantillasMetadatos(iBucle).idTipodoc.ToString)
        Next


    End Sub


    Sub ResizingElements()
        '--------------------------Resizing de elementos
        ListBox1.Location = New Point(6, 104)
        ListBox1.Size = New Point(189, 120)
        ListBox1.Visible = False

        RadioButton1.Size = New Point(200, 41)
        RadioButton1.Location = New Point(0, 645)
        RadioButton2.Size = New Point(200, 41)
        RadioButton2.Location = New Point(0, 605)

        PictureBox1.Visible = False
        PictureBox3.Visible = False
        PictureBox4.Visible = False
        Panel_DocSearch.Location = New Point(0, 0)
        Panel_GeoSearch.Location = New Point(0, 0)
        Panel_DocSearch.Size = New Point(200, 700)
        Panel_GeoSearch.Size = New Point(200, 700)
        Panel_DocSearch.Visible = True
        Panel_GeoSearch.Visible = False
        RadioButton1.Checked = True

        GroupBox5.Location = New Point(3, 260)
        Button3.Location = New Point(108, 660)
        Button4.Location = New Point(3, 660)
        PictureBox3.Location = New Point(89, Button3.Top)
        PictureBox3.Visible = False


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
        If App_Permiso <> 2 Then
            ToolStripButton9.Enabled = False
            mnuTool_AltaDoc.Enabled = False
            mnuTool_EditAtrib.Enabled = False
            mnuModDocuTiposDoc.Enabled = False
            mnuModDocuEstados.Enabled = False
            mnuModDocuObservaciones.Enabled = False
            mnuModDocuMedidas.Enabled = False
            itemGestionUser.Enabled = False
            mnuAddECW.Enabled = False
            mnuAddContornos.Enabled = False
            btnExportCdD.Enabled = False
            mnuExportCdD.Enabled = False
            ToolStripButton19.Visible = False
        End If

        mnuDeveloper.Visible = usuario_ISTARI
        ToolStripButton1.Visible = usuario_ISTARI
        mnuGenerarRejilla.Visible = usuario_ISTARI
        mnuLanzarPlantilla.Visible = usuario_ISTARI

        If usuario_ISTARI = False Then
            If LeeIni("Visores", "MostrarVisorCarto") <> "SI" Then
                Button7.Visible = False
            End If
        End If
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
        ComboBox6.Items.Add("-----")
        ComboBox6.Items.Add("S�")
        ComboBox6.Items.Add("No")
        ComboBox1.Text = "-----"
        ComboBox2.Text = "-----"
        ComboBox4.Text = "-----"
        ComboBox5.Text = "-----"
        ComboBox6.Text = "-----"
        If modoDelegaciones Then
            ToolStripButton5.Visible = False
            ToolStripButton6.Visible = False
            btnVisorMosaicos.Visible = False
            mnuBuscador.Visible = False
            mnuBuscadorGeo.Visible = False
            mnuVisorMosaicos.Visible = False
            mnuTool_Informes.Visible = False
            RadioButton1.Visible = False
            RadioButton2.Visible = False
        End If




    End Sub

    '----------------------------------------------------------------------------------------------------------------------
    'Control del Textbox de b�squeda
    '----------------------------------------------------------------------------------------------------------------------

    Private Sub TextBox1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox1.Click
        If TextBox1.Text.Trim <> "" Then
            TextBox1.SelectAll()
        End If
    End Sub

    Private Sub TextBox1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyUp
        Application.DoEvents()
        If e.KeyData = Keys.Down And ListBox1.Visible = True Then
            ListBox1.Focus()
            ListBox1.SelectedIndex = 0
        End If
        If e.KeyData = Keys.Enter Then
            LanzarConsulta(sender, e)
        End If
    End Sub


    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        Dim filas() As DataRow
        ListBox1.Visible = False
        Autocompletar_colindantes = False
        Autocompletar_colindantes = True
        If IsNumeric(TextBox1.Text.Trim) Then
            TextBox1.Tag = ""
            Exit Sub
        End If
        If TextBox1.Text.Length > 1 And Autocompletar_municipios = True Then
            PictureBox1.Visible = True
            Application.DoEvents()
            If CheckBox1.Checked = True Then
                'Trabajamos con los municipios actuales
                If ComboBox3.SelectedIndex = -1 Then
                    filas = ListaMunicipiosActual.Select("nombreSearch like '%" & quitarAcento.destildar(TextBox1.Text, False, False) & "%'")
                Else
                    Dim cProv As Integer = CType(ComboBox3.SelectedItem, itemData).Valor
                    filas = ListaMunicipiosActual.Select("provincia_id=" & cProv.ToString & " and nombreSearch like '%" & quitarAcento.destildar(TextBox1.Text, False, False) & "%'")
                End If
            Else
                'Trabajamos con los hist�ricos
                If ComboBox3.SelectedIndex = -1 Then
                    filas = ListaMunicipiosHisto.Select("nombreSearch like '%" & quitarAcento.destildar(TextBox1.Text, False, False) & "%'")
                Else
                    Dim cProv As Integer = CType(ComboBox3.SelectedItem, itemData).Valor
                    filas = ListaMunicipiosHisto.Select("provincia_id=" & cProv.ToString & " and nombreSearch like '%" & quitarAcento.destildar(TextBox1.Text, False, False) & "%'")
                End If

            End If


            ListBox1.Items.Clear()
            For Each dR As DataRow In filas
                If CheckBox1.Checked = True Then
                    'If dR("cod_munihisto").ToString.EndsWith("00") = True Then
                    ListBox1.Items.Add(New itemData(dR("nombre").ToString, dR("cod_munihisto") & "|" & dR("idmunihisto") & "|" & dR("inecortoActual")))
                    'End If
                Else
                    ListBox1.Items.Add(New itemData(dR("nombre").ToString, dR("cod_munihisto") & "|" & dR("idmunihisto") & "|" & dR("inecortoActual")))
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
                    MessageBox.Show("Repita la b�squeda", My.Application.Info.AssemblyName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End Try
            End If
            PictureBox1.Visible = False
        End If

    End Sub

    Private Sub SeleccionarElementoLB(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox1.DoubleClick, ListBox1.Click


        If ListBox1.SelectedItem Is Nothing Then Exit Sub
        Autocompletar_municipios = False
        TextBox1.Text = ListBox1.SelectedItem.ToString
        TextBox1.Tag = CType(ListBox1.Items(ListBox1.SelectedIndex), itemData).Valor
        Dim Indices() As String = TextBox1.Tag.Split("|")
        ListBox1.Tag = Indices(1)
        Autocompletar_municipios = True
        ListBox1.Visible = False
        If sender.name = "Listbox1" Then LanzarConsulta(sender, e)


    End Sub

    Private Sub ListBox1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ListBox1.KeyUp
        Application.DoEvents()
        If e.KeyData = Keys.Up And ListBox1.SelectedIndex = 0 Then
            TextBox1.Focus()
        ElseIf e.KeyData = Keys.Return And ListBox1.SelectedIndex >= 0 Then
            SeleccionarElementoLB(sender, e)
            LanzarConsulta(sender, e)
        End If

    End Sub

    Sub LanzarConsulta(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

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

        '-----------------------------------------------------------------------------------
        'Eval�o si se filtran los documentos por estado o por tipo
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
        ibucle = -1
        EstadosDocumento = "-"
        For Each Linea As itemData In CheckedListBox2.Items
            ibucle = ibucle + 1
            If CheckedListBox2.GetItemChecked(ibucle) = True Then
                EstadosDocumento = EstadosDocumento & Linea.Valor & "-"
            End If
        Next
        If EstadosDocumento = "-" Then EstadosDocumento = ""
        Application.DoEvents()

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
            FrmResult.CargarDatosSIDCARTO_By_Provincia(cProv, TiposDocumento, _
                                                            EstadosDocumento, CadFiltro)
            FrmResult.Show()
        ElseIf nSellado <> "" Then
            Dim FrmResult As New frmDocumentacion
            FrmResult.MdiParent = Me
            FrmResult.Text = "Documento n�: " & String.Format("{0:000000}", nSellado) & DescripFiltro
            FrmResult.CargarDatosSIDCARTO_By_Sellado(CType(nSellado, Integer))
            FrmResult.Show()
        ElseIf nSellado1 <> "" And nSellado2 <> "" Then
            Dim FrmResult As New frmDocumentacion
            FrmResult.MdiParent = Me
            FrmResult.Text = "Documentos entre n�: " & String.Format("{0:000000}", nSellado1) & " y " & String.Format("{0:000000}", nSellado2) & " " & DescripFiltro
            FrmResult.CargarDatosSIDCARTO_By_SelladoIntervalo(CType(nSellado1, Integer), CType(nSellado2, Integer))
            FrmResult.Show()
        ElseIf listaSellos.Count > 0 Then
            Dim FrmResult As New frmDocumentacion
            FrmResult.MdiParent = Me
            FrmResult.Text = "Documentos con n� de sellado: " & TextBox1.Text.Trim.Replace(";", ",") & " " & DescripFiltro
            FrmResult.CargarDatosSIDCARTO_By_ListaSellado(listaSellos)
            FrmResult.Show()
        ElseIf cProv = 0 Then
            CadFiltro = SumarFiltros("")
            Application.DoEvents()
            Dim FrmResult As New frmDocumentacion
            FrmResult.MdiParent = Me
            FrmResult.Text = "Todas las provincias. " & DescripFiltro
            FrmResult.CargarDatosSIDCARTO_By_Filtro(CadFiltro, EstadosDocumento, _
                                                            TiposDocumento)
            FrmResult.Show()
        End If


        PictureBox3.Visible = False
        Me.Cursor = Cursors.Default
        Application.DoEvents()

    End Sub

    Function SumarFiltros(ByVal cadenaSQLini As String) As String

        'A�adimos los filtros que se han seleccionado
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
        'Filtro por A�o Principal
        If ComboBox5.Text <> "" And ComboBox5.Text.StartsWith("-") = False And TextBox18.Text.Trim <> "" Then
            If ComboBox5.Text = "mayor" Then
                cadenaSQLini = cadenaSQLini & " AND TO_CHAR(FechaPrincipal,'YYYY')>'" & TextBox18.Text & "'"
            ElseIf ComboBox5.Text = "menor" Then
                cadenaSQLini = cadenaSQLini & " AND TO_CHAR(FechaPrincipal,'YYYY')<'" & TextBox18.Text & "'"
            ElseIf ComboBox5.Text = "igual" Then
                cadenaSQLini = cadenaSQLini & " AND TO_CHAR(FechaPrincipal,'YYYY')='" & TextBox18.Text & "'"
            End If
        End If

        'Filtro por A�o de Modificacion
        If TextBox21.Text.Trim <> "" Then
            cadenaSQLini = cadenaSQLini & " AND fechasmodificaciones like '%" & TextBox21.Text.ToUpper & "%'"
        End If

        'Filtro por subtipo
        If TextBox22.Text.Trim <> "" Then
            cadenaSQLini = cadenaSQLini & " AND subtipo ilike '%" & TextBox22.Text & "%'"
        End If

        'Filtro por Junta Estadistica
        If ComboBox6.Text = "S�" Then
            cadenaSQLini = cadenaSQLini & " AND juntaestadistica=1"
        ElseIf ComboBox6.Text = "No" Then
            cadenaSQLini = cadenaSQLini & " AND juntaestadistica=0"
        End If

        'Filtro por Observacion
        If ComboBox7.Text <> "-----" Then
            If ComboBox7.SelectedIndex <> -1 Then
                cadenaSQLini = cadenaSQLini & " AND idobservestandar=" & CType(ComboBox7.SelectedItem, itemData).Valor
            End If
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



    Private Sub LimpiarCampos(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                        Handles Button4.Click, Button5.Click

        Dim iBucle As Integer
        Application.DoEvents()
        For iBucle = 0 To CheckedListBox1.Items.Count - 1
            CheckedListBox1.SetItemChecked(iBucle, False)
        Next
        For iBucle = 0 To CheckedListBox2.Items.Count - 1
            CheckedListBox2.SetItemChecked(iBucle, False)
        Next
        TextBox1.Text = ""
        TextBox1.Tag = ""
        ListBox1.Tag = ""
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
        ComboBox1.Text = "-----"
        ComboBox2.Text = "-----"
        ComboBox3.SelectedIndex = -1
        ComboBox4.Text = "-----"
        ComboBox5.Text = "-----"
        ComboBox6.Text = "-----"
        ComboBox7.Text = "-----"
        ListBox1.Visible = False
        CheckBox1.Checked = False
        TextBox3.Text = ""
        TextBox7.Text = ""
        TextBox9.Text = ""
        TextBox10.Text = ""
        TextBox11.Text = ""
        TextBox12.Text = ""
        TextBox13.Text = ""

    End Sub

    Sub FuncionesMenuBusqueda(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                                    Handles mnuEjecutar.Click, mnuLimpiar.Click, _
                                    ToolStripButton12.Click, ToolStripButton17.Click

        If sender.name = "mnuEjecutar" Or sender.name = "ToolStripButton12" Then
            If Panel_DocSearch.Visible = True Then
                LanzarConsulta(sender, e)
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

    End Sub

    Sub MostrarPanelBusqueda(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                                    Handles ToolStripButton5.Click, mnuBuscador.Click, RadioButton1.Click

        Panel_DocSearch.Visible = True
        Panel_GeoSearch.Visible = False
        RadioButton1.Checked = True


    End Sub


    Sub ArranqueHerramientas(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                                    Handles ToolStripButton9.Click,
                                    mnuTool_AltaDoc.Click,
                                    mnuGenerarRejilla.Click,
                                    mnuLanzarPlantilla.Click, mnuAddECW.Click, mnuAddContornos.Click, mnuMuniHisto.Click, mnuOpenPreferenceFolder.Click, mnuOpenLoggerFile.Click



        If sender.name = "ToolStripButton9" Or sender.name = "mnuTool_AltaDoc" Then
            Dim FormularioCreacion As New frmEdicion
            FormularioCreacion.MdiParent = Me
            FormularioCreacion.ModoTrabajo("NUEVO", 0)
            FormularioCreacion.Show()
            FormularioCreacion.Visible = True
        ElseIf sender.name = "mnuOpenPreferenceFolder" Then
            Try
                Process.Start(AppFolderSetting)
            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        ElseIf sender.name = "mnuOpenLoggerFile" Then
            Try
                If System.IO.File.Exists("C:\Program Files (x86)\Notepad++\notepad++.exe") Then
                    Process.Start("C:\Program Files (x86)\Notepad++\notepad++.exe", ficheroLogger)
                Else
                    Process.Start(ficheroLogger)
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        ElseIf sender.name = "mnuMuniHisto" Then
            Dim frmMuniH As New frmMuniHisto
            frmMuniH.MdiParent = Me
            frmMuniH.Show()
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
            'Genera una rejilla v�lida para georreferenciaci�n
            Dim CooX As String = InputDialog.InputBox("Coordenada X m�nima: esquina Superior Izquierda", "Coordenada X m�nima: esquina Superior Izquierda", "")
            If Not IsNumeric(CooX) Then Exit Sub
            Dim CooY As String = InputDialog.InputBox("Coordenada Y m�xima: esquina Superior Izquierda", "Coordenada Y m�xima: esquina Superior Izquierda", "")
            If Not IsNumeric(CooY) Then Exit Sub
            Dim CeldasX As String = InputDialog.InputBox("N�mero de celdas en el eje X", "N�mero de celdas en el eje X", "")
            If Not IsNumeric(CeldasX) Then Exit Sub
            Dim CeldasY As String = InputDialog.InputBox("N�mero de celdas en el eje Y", "N�mero de celdas en el eje Y", "")
            If Not IsNumeric(CeldasY) Then Exit Sub
            Dim AnchuraXparam As String = InputDialog.InputBox("Anchura en metros de la celda", "Anchura en metros de la celda", "")
            If Not IsNumeric(AnchuraXparam) Then Exit Sub
            Dim AnchuraYparam As String = InputDialog.InputBox("Altura en metros de la celda", "Altura en metros de la celda", "")
            If Not IsNumeric(AnchuraYparam) Then Exit Sub
            Dim nPuntos As String = InputDialog.InputBox("N�mero de puntos por celda (4 � 8)", "N�mero de puntos por celda (4 � 8)", "")
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
            MessageBox.Show("Rejilla terminada", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)

        End If



    End Sub

    Sub LanzarInformes(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
                    mnuTool_Informes01.Click, mnuTool_Informes02.Click, mnuTool_Informes03.Click, mnuReportDocsNoContornos.Click, mnuPPCnoGeo.Click


        Dim Fecha_ini As String
        Dim Fecha_fin As String
        Dim codProv As Integer
        Dim textoInforme As String = ""

        If sender.name = "mnuReportDocsNoContornos" Then
            Try
                codProv = CType(InputDialog.InputBox("Introduce el c�digo INE  (1-50) de la provincia para elaborar el informe." & System.Environment.NewLine &
                                                     "Introduce 0 para hacer todas", AplicacionTitulo, ""), Integer)
            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End Try
            If codProv < 0 Or codProv > 50 Then
                MessageBox.Show("C�digo de provincia no v�lido", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
            If codProv = 0 Then
                If MessageBox.Show("Un informe de todas las provincias puede resultar lento. A�n as�, �desea continuar?", AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                    Exit Sub
                End If
                textoInforme = "Documentos de todas las provincias sin georreerenciar"
            Else
                textoInforme = "Documentos de " & DameProvinciaByINE(codProv) & " sin georreerenciar"
            End If

            LanzarSpinner()

            Me.Cursor = Cursors.WaitCursor
            Dim frmResult As New frmDocumentacion
            frmResult.MdiParent = Me
            frmResult.Text = textoInforme
            If codProv = 0 Then
                frmResult.getGeodocatDocsWithoutContour("geom Is null")
            Else
                frmResult.getGeodocatDocsWithoutContour("geom Is null And provincias.idprovincia=" & codProv)
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
                Fecha_ini = InputDialog.InputBox("Fecha inicio de la b�squeda AAAA-MM-DD", "Consultas avanzadas", "")
                Fecha_fin = InputDialog.InputBox("Fecha final de la b�squeda AAAA-MM-DD", "Consultas avanzadas", "")
            End If
            FormularioInformes.fecha1 = Fecha_ini
            FormularioInformes.fecha2 = Fecha_fin
            FormularioInformes.Text = "Inventario del SIDCARTO clasificado por Tipo de documento"
            FormularioInformes.Informe_Resumen_PorTipoDoc()
        ElseIf sender.name = "mnuTool_Informes02" Then
            FormularioInformes.Text = "Inventario del SIDCARTO clasificado por Estado de conservaci�n"
            FormularioInformes.Informe_Resumen_PorEstadoDoc()
        ElseIf sender.name = "mnuTool_Informes03" Then
            FormularioInformes.Text = "�ltimo n�mero de sellado asignado por provincia"
            FormularioInformes.Informe_UltimoDocumentoSellado()
        ElseIf sender.name = "mnuPPCnoGeo" Then
            FormularioInformes.Text = "Planos de poblaci�n en cuaderno no georreferenciados"
            FormularioInformes.ListarPPCnoGeo()
        Else

        End If




    End Sub


    Sub LanzarConsultaGEO_SIDCARTO(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click

        Dim Xmax As Double = 0
        Dim Ymax As Double = 0
        Dim Xmin As Double = 0
        Dim Ymin As Double = 0
        Dim XCentro As Double = 0
        Dim YCentro As Double = 0
        Dim Radio As Integer = 0
        Dim TituloConsulta As String = ""

        If TextBox11.Text <> "" And TextBox12.Text <> "" And TextBox13.Text <> "" Then
            'Compruebo si las coordenadas estan en geograficas, y si es as� las paso a UTMED50 para procesar
            XCentro = TextBox11.Text.Replace(".", ",")
            YCentro = TextBox12.Text.Replace(".", ",")

            'If DentroEspa�a(XCentro, YCentro) = True Then
            '    If ConversionLLWGS84_to_UTMED50_GSB(XCentro, YCentro) = True Then
            '        Application.DoEvents()
            '    Else
            '        MessageBox.Show("Las coordenadas no pueden convertirse a UTM", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            '        Exit Sub
            '    End If
            'Else
            '    XCentro = CType(TextBox11.Text.Replace(".", ","), Double)
            '    YCentro = CType(TextBox12.Text.Replace(".", ","), Double)
            'End If
            Radio = CType(TextBox13.Text, Integer)
            Xmax = XCentro + Radio * 1000
            Ymax = YCentro + Radio * 1000
            Xmin = XCentro - Radio * 1000
            Ymin = YCentro - Radio * 1000
            TituloConsulta = "B�squeda por entorno. " & _
                                "(" & XCentro.ToString & "," & YCentro.ToString & ") " & _
                                " y  radio " & Radio.ToString & " Km"
        ElseIf TextBox7.Text <> "" And TextBox3.Text <> "" And TextBox9.Text <> "" And TextBox10.Text <> "" Then
            Xmax = CType(TextBox7.Text.Replace(".", ","), Double)
            Ymax = CType(TextBox3.Text.Replace(".", ","), Double)
            Xmin = CType(TextBox9.Text.Replace(".", ","), Double)
            Ymin = CType(TextBox10.Text.Replace(".", ","), Double)
            'If DentroEspa�a(Xmax, Ymax) = True And DentroEspa�a(Xmin, Ymin) = True Then
            '    If ConversionLLWGS84_to_UTMED50_GSB(Xmax, Ymax) = True And ConversionLLWGS84_to_UTMED50_GSB(Xmin, Ymin) = True Then
            '        Application.DoEvents()
            '    Else
            '        MessageBox.Show("Las coordenadas no pueden convertirse a UTM", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            '        Exit Sub
            '    End If
            'Else
            '    Xmax = CType(TextBox7.Text.Replace(".", ","), Double)
            '    Ymax = CType(TextBox3.Text.Replace(".", ","), Double)
            '    Xmin = CType(TextBox9.Text.Replace(".", ","), Double)
            '    Ymin = CType(TextBox10.Text.Replace(".", ","), Double)
            'End If
            TituloConsulta = "B�squeda por entorno. " & _
                                "(" & Xmax.ToString & "," & Ymax.ToString & ") " & _
                                "(" & Xmin.ToString & "," & Ymin.ToString & ")"
        End If
        If Xmax = 0 Or Ymax = 0 Or Xmin = 0 Or Ymin = 0 Then Exit Sub
        If Xmax - Xmin > 200000 Or Ymax - Ymin > 200000 Then
            MessageBox.Show("Reduzca el entorno de la b�squeda", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
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


    Sub LanzarConfiguraciones(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolConfig.Click, mnuConfiguracion.Click
        frmSettings.ShowDialog()
    End Sub


    Private Sub IndexToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                                    Handles IndexToolStripMenuItem.Click, HelpToolStripButton.Click

        If System.IO.File.Exists(My.Application.Info.DirectoryPath & "\Ayuda.pdf") = False Then
            MessageBox.Show("Ayuda no disponible", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Process.Start(My.Application.Info.DirectoryPath & "\Ayuda.pdf")


    End Sub

    Sub LanzarTareasfrmDocumentacion(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                    Handles mnuExtraerContornos.Click, mnuExtraerCentroide.Click, _
                            ToolStripButton3.Click, mnuResConsulta1.Click, _
                            ToolMiniaturas.Click, mnuResconsulta3.Click, _
                            ToolStripButton4.Click, mnuResconsulta2.Click, _
                            ToolStripButton7.Click, mnuResconsulta4.Click, _
                            ToolStripButton16.Click, mnuResconsulta5.Click, _
                            ToolStripButton14.Click, mnuGenerarMetadatos.Click, _
                            ToolStripButton11.Click, mnuResconsulta7.Click, _
                            ToolStripButton10.Click, mnuResconsulta6.Click, _
                            ToolStripButton8.Click, mnuResconsulta8.Click, _
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
            If frmAccion.ListView1.SelectedItems.Count > 0 Then
                frmAccion.MostrarDetalle(frmAccion.ListView1.SelectedItems(0).Tag)
            Else
                frmAccion.MostrarDetalle(0)
            End If
        ElseIf sender.name = "ToolStripButton7" Or sender.name = "mnuResconsulta4" Then
            frmAccion.LanzarVisordeJPG(sender, e)
        ElseIf sender.name = "ToolStripButton16" Or sender.name = "mnuResconsulta5" Then
            frmAccion.LanzarECW(sender, e)
        ElseIf sender.name = "ToolStripButton18" Or sender.name = "mnuResconsulta9" Then
            frmAccion.LanzarImpresionJPG(sender, e)
        ElseIf sender.name = "ToolStripButton14" Or sender.name = "mnuGenerarMetadatos" Then
            frmAccion.ProcGenerarMetadatos(sender, e)
        ElseIf sender.name = "ToolStripButton11" Or sender.name = "mnuResconsulta7" Then
            frmAccion.LanzarTareaGuardarEnCarpeta(sender, e)
        ElseIf sender.name = "ToolStripButton10" Or sender.name = "mnuResconsulta6" Then
            frmAccion.SelectColumnas(sender, e)
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

    Sub LanzarConsultaAvanzada(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                        Handles Query_Advance01.Click, Query_Advance02.Click, Query_Advance03.Click, _
                        Query_Advance04.Click, Query_Advance05.Click, Query_Advance06.Click, _
                        Query_Advance07.Click, Query_Advance08.Click


        Dim FrmResult As New frmDocumentacion
        Dim Filtro As String
        Dim fechaActual As Date = Now

        If sender.name = "Query_Advance01" Then
            'Documentos modificados en los �ltimos 30 d�as
            Filtro = " And archivo.fechamodificacion between '" & fechaActual.AddDays(-30).Year & "-" & _
                            String.Format("{0:00}", fechaActual.AddDays(-30).Month) & "-" & _
                            String.Format("{0:00}", fechaActual.AddDays(-30).Day) & "' AND '" & fechaActual.AddDays(1).Year & "-" & _
                            String.Format("{0:00}", fechaActual.AddDays(1).Month) & "-" & _
                            String.Format("{0:00}", fechaActual.AddDays(1).Day) & "'"
            FrmResult.Text = "Documentos modificados en los �ltimos 30 d�as"
            FrmResult.CargarDatosSIDCARTO_By_Filtro(Filtro)
        ElseIf sender.name = "Query_Advance02" Then
            'Lanzar consulta documentos creados hoy
            Filtro = " and archivo.fechacreacion between '" & fechaActual.Year & "-" & _
                            String.Format("{0:00}", fechaActual.Month) & "-" & _
                            String.Format("{0:00}", fechaActual.Day) & " 00:00:00' AND '" & _
                            fechaActual.AddDays(1).Year & "-" & _
                            String.Format("{0:00}", fechaActual.AddDays(1).Month) & "-" & _
                            String.Format("{0:00}", fechaActual.AddDays(1).Day) & " 00:00:00'"
            FrmResult.Text = "Documentos creados hoy"
            FrmResult.CargarDatosSIDCARTO_By_Filtro(Filtro)
        ElseIf sender.name = "Query_Advance03" Then
            'Documentos modificados entre dos fechas
            Dim Fecha_ini As String = InputDialog.InputBox("Fecha inicio de la b�squeda AAAA-MM-DD", "Consultas avanzadas", "")
            Dim Fecha_fin As String = InputDialog.InputBox("Fecha final de la b�squeda AAAA-MM-DD", "Consultas avanzadas", "")
            If Fecha_fin = "" Or Fecha_ini = "" Then Exit Sub
            Filtro = "and fechamodificacion between '" & Fecha_ini & "' and '" & Fecha_fin & "'"
            FrmResult.Text = "Documentos modificados entre " & Fecha_ini & " y " & Fecha_fin
            FrmResult.CargarDatosSIDCARTO_By_Filtro(Filtro)
        ElseIf sender.name = "Query_Advance04" Then
            'Documentos creados entre dos fechas
            Dim Fecha_ini As String = InputDialog.InputBox("Fecha inicio de la b�squeda AAAA-MM-DD", "Consultas avanzadas", "")
            Dim Fecha_fin As String = InputDialog.InputBox("Fecha final de la b�squeda AAAA-MM-DD", "Consultas avanzadas", "")
            If Fecha_fin = "" Or Fecha_ini = "" Then Exit Sub
            Filtro = "and fechacreacion between '" & Fecha_ini & "' and '" & Fecha_fin & "'"
            FrmResult.Text = "Documentos dados de alta entre " & Fecha_ini & " y " & Fecha_fin
            FrmResult.CargarDatosSIDCARTO_By_Filtro(Filtro)
        ElseIf sender.name = "Query_Advance05" Then
            'Ultimos n registros creados
            Dim NumReg As String = InputDialog.InputBox("�Cuantos registros desea mostrar?", "Consultas avanzadas", "100")
            If NumReg.Trim = "" Then Exit Sub
            If Not IsNumeric(NumReg) Then Exit Sub
            FrmResult.Text = "�ltimos documentos introducidos"
            FrmResult.MdiParent = Me
            FrmResult.CargarUltimosDatos(CType(NumReg, Integer))
            FrmResult.Show()
            Exit Sub
        ElseIf sender.name = "Query_Advance06" Then
            'Documentos creados en los �ltimos 30 d�as
            Filtro = " and archivo.fechacreacion between '" & fechaActual.AddDays(-30).Year & "-" & _
                String.Format("{0:00}", fechaActual.AddDays(-30).Month) & "-" & _
                String.Format("{0:00}", fechaActual.AddDays(-30).Day) & "' AND '" & fechaActual.AddDays(1).Year & "-" & _
                String.Format("{0:00}", fechaActual.AddDays(1).Month) & "-" & _
                String.Format("{0:00}", fechaActual.AddDays(1).Day) & "'"
            FrmResult.Text = "Documentos creados en los �ltimos 30 d�as"
            FrmResult.CargarDatosSIDCARTO_By_Filtro(Filtro)
        ElseIf sender.name = "Query_Advance07" Then
            Dim Rango As String = InputDialog.InputBox("Introduzca un rango de n�meros de sellado (#####1-#####2)", "Consultas avanzadas", "100")
            If Rango = "" Then Exit Sub
            If Rango.IndexOf("-") = -1 Then Exit Sub
            Dim Sellados() As String = Rango.Split("-")
            If Sellados.Length = 2 Then
                Filtro = " and archivo.numdoc between '" & Sellados(0) & "' AND '" & Sellados(1) & "'"
                FrmResult.Text = "Documentos con sello entre " & Sellados(0) & " Y " & Sellados(1)
            Else
                Exit Sub
            End If
            FrmResult.CargarDatosSIDCARTO_By_Filtro(Filtro)
        ElseIf sender.name = "Query_Advance08" Then
            Dim fechaQuery As String = InputDialog.InputBox("Fecha de la b�squeda AAAA-MM-DD", "Consultas avanzadas", "")
            If fechaQuery = "" Then Exit Sub
            Filtro = " and archivo.fechacreacion between '" & fechaQuery & " 00:00:00' AND '" & _
                            fechaQuery & " 23:59:59'"
            FrmResult.Text = "Documentos creados el d�a " & fechaQuery
            FrmResult.CargarDatosSIDCARTO_By_Filtro(Filtro)
        Else
            Exit Sub
        End If
        FrmResult.MdiParent = Me
        FrmResult.Show()

    End Sub



    Private Sub ToolStripButton13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                Handles ToolStripButton13.Click, AboutToolStripMenuItem.Click
        AboutBox1.ShowDialog()
    End Sub

    Private Sub GestionCarrito(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButCarrito.Click, mnuCarrito.Click

        If CarritoCompra Is Nothing Then
            MessageBox.Show("El carrito esta vac�o", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        If CarritoCompra.Length = 0 Then
            MessageBox.Show("El carrito esta vac�o", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        Dim ExisteCarrito As Boolean
        For Each ChildForm As Form In Me.MdiChildren
            If ChildForm.Tag = "Carrito de la Compra" Then
                ChildForm.Focus()
                ExisteCarrito = True
                Exit For
            End If
        Next
        Dim FrmCestaCompra As New frmDocumentacion
        If ExisteCarrito = True Then
            Try
                FrmCestaCompra = Me.ActiveMdiChild
                If FrmCestaCompra Is Nothing Then Exit Sub
            Catch ex As Exception

            End Try
            FrmCestaCompra.MostrarElementosCarrito()
            FrmCestaCompra.Show()
        Else
            FrmCestaCompra.MdiParent = Me
            FrmCestaCompra.Text = " Carrito de la Compra"
            FrmCestaCompra.Tag = "Carrito de la Compra"
            FrmCestaCompra.Button7.Image = Me.ImageList1.Images(6)
            FrmCestaCompra.MostrarElementosCarrito()
            FrmCestaCompra.Show()

        End If
    End Sub

    Private Sub ComboBox3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox3.Click
        TextBox1.Text = ""
    End Sub

    Private Sub ModificarAtributosDocumentos(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                        Handles mnuModDocuMedidas.Click, mnuModDocuTiposDoc.Click, _
                        mnuModDocuEstados.Click, mnuModDocuObservaciones.Click, mnuVisorMosaicos.Click, _
                        btnVisorMosaicos.Click, btnExportCdD.Click, mnuExportCdD.Click


        If sender.name = "mnuModDocuTiposDoc" Then
            Dim FrmEditAtrib As New frmEdicionesTablas
            FrmEditAtrib.MdiParent = Me
            FrmEditAtrib.Text = "Tipos de documentaci�n"
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
            FrmEditAtrib.Text = "Estados de Conservaci�n"
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


    Private Sub itemUsermenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles itemUsermenu.Click

        frmIncidencias.MdiParent = Me
        frmIncidencias.Show()

    End Sub

    Private Sub itemGestionUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles itemGestionUser.Click

        frmGestionUser.MdiParent = Me
        frmGestionUser.Show()

    End Sub

    Private Sub MDIPrincipal_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        If Me.Size.Height < 820 Then
            RadioButton1.Visible = False
            RadioButton2.Visible = False
            If Me.Size.Height < 780 Then
                Button3.Visible = False
                Button4.Visible = False
            Else
                Button3.Visible = True
                Button4.Visible = True
            End If
        Else
            RadioButton1.Visible = True
            RadioButton2.Visible = True
            Button3.Visible = True
            Button4.Visible = True
        End If

    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        For Each ChildForm As Form In Me.MdiChildren
            If ChildForm.Tag = "visorcarto" Then
                ChildForm.Show()
                ChildForm.Focus()
                Exit Sub
            End If
        Next

        MessageBox.Show("Herranmienta no diaponible")
        'Dim visor As MapInstance
        'visor = New MapInstance
        'visor.Tag = "visorcarto"
        'visor.MdiParent = Me
        'visor.Show()
    End Sub


End Class
