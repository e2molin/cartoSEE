Public Class GestionUserForm
    Inherits System.Windows.Forms.Form

    Dim ListaUsuarios As New ArrayList
    Dim profile As New myAppProfile
    Dim NombreCampoPermisos As String

#Region "Definiciones Windows Forms"
    Friend WithEvents lvListaUser As System.Windows.Forms.ListView
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents TextBox6 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox7 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox8 As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TextBox9 As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents TextBox10 As System.Windows.Forms.TextBox
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents ListView2 As System.Windows.Forms.ListView
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents txtNewUserNombre As System.Windows.Forms.TextBox
    Friend WithEvents txtNewUserSurname As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtNewUserWinLogin As System.Windows.Forms.TextBox
    Friend WithEvents txtNewUserPass As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtNewUserTfno As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Private components As System.ComponentModel.IContainer
    Friend WithEvents Label14 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents ComboBox2 As ComboBox
    Friend WithEvents Label15 As Label
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents Label16 As Label
    Friend WithEvents Label17 As Label
    Friend WithEvents TextBox5 As TextBox
    Friend WithEvents TextBox11 As TextBox
    Friend WithEvents Label18 As Label
    Friend WithEvents TextBox12 As TextBox
    Friend WithEvents Label19 As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents RadioButton2 As RadioButton
    Friend WithEvents RadioButton1 As RadioButton
    Friend WithEvents Button6 As Button
    Friend WithEvents Button5 As Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(GestionUserForm))
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.TextBox11 = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.ComboBox2 = New System.Windows.Forms.ComboBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.txtNewUserTfno = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtNewUserPass = New System.Windows.Forms.TextBox()
        Me.txtNewUserWinLogin = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtNewUserSurname = New System.Windows.Forms.TextBox()
        Me.txtNewUserNombre = New System.Windows.Forms.TextBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.TextBox12 = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.ListView2 = New System.Windows.Forms.ListView()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.TextBox10 = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.TextBox9 = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TextBox8 = New System.Windows.Forms.TextBox()
        Me.TextBox7 = New System.Windows.Forms.TextBox()
        Me.TextBox6 = New System.Windows.Forms.TextBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.lvListaUser = New System.Windows.Forms.ListView()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(12, 26)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(984, 498)
        Me.TabControl1.TabIndex = 1
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.GroupBox2)
        Me.TabPage1.Controls.Add(Me.TextBox11)
        Me.TabPage1.Controls.Add(Me.Label18)
        Me.TabPage1.Controls.Add(Me.TextBox4)
        Me.TabPage1.Controls.Add(Me.Label16)
        Me.TabPage1.Controls.Add(Me.ComboBox2)
        Me.TabPage1.Controls.Add(Me.Label15)
        Me.TabPage1.Controls.Add(Me.TextBox3)
        Me.TabPage1.Controls.Add(Me.Label6)
        Me.TabPage1.Controls.Add(Me.Label13)
        Me.TabPage1.Controls.Add(Me.ListView1)
        Me.TabPage1.Controls.Add(Me.Button1)
        Me.TabPage1.Controls.Add(Me.txtNewUserTfno)
        Me.TabPage1.Controls.Add(Me.Label5)
        Me.TabPage1.Controls.Add(Me.Label4)
        Me.TabPage1.Controls.Add(Me.txtNewUserPass)
        Me.TabPage1.Controls.Add(Me.txtNewUserWinLogin)
        Me.TabPage1.Controls.Add(Me.Label3)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Controls.Add(Me.txtNewUserSurname)
        Me.TabPage1.Controls.Add(Me.txtNewUserNombre)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(976, 472)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Nuevo usuario"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.RadioButton2)
        Me.GroupBox2.Controls.Add(Me.RadioButton1)
        Me.GroupBox2.Location = New System.Drawing.Point(588, 385)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(244, 58)
        Me.GroupBox2.TabIndex = 38
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "El usuario por defecto se encuentra"
        '
        'RadioButton2
        '
        Me.RadioButton2.Image = CType(resources.GetObject("RadioButton2.Image"), System.Drawing.Image)
        Me.RadioButton2.Location = New System.Drawing.Point(118, 18)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(120, 34)
        Me.RadioButton2.TabIndex = 1
        Me.RadioButton2.Text = "Deshabilitado"
        Me.RadioButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'RadioButton1
        '
        Me.RadioButton1.Checked = True
        Me.RadioButton1.Image = CType(resources.GetObject("RadioButton1.Image"), System.Drawing.Image)
        Me.RadioButton1.Location = New System.Drawing.Point(6, 18)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(106, 34)
        Me.RadioButton1.TabIndex = 0
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "Habilitado"
        Me.RadioButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'TextBox11
        '
        Me.TextBox11.Location = New System.Drawing.Point(36, 285)
        Me.TextBox11.Name = "TextBox11"
        Me.TextBox11.Size = New System.Drawing.Size(260, 20)
        Me.TextBox11.TabIndex = 32
        Me.TextBox11.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(33, 269)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(93, 13)
        Me.Label18.TabIndex = 33
        Me.Label18.Text = "Correo electrónico"
        '
        'TextBox4
        '
        Me.TextBox4.Location = New System.Drawing.Point(36, 188)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(260, 20)
        Me.TextBox4.TabIndex = 30
        Me.TextBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(33, 172)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(87, 13)
        Me.Label16.TabIndex = 31
        Me.Label16.Text = "Nombre máquina"
        '
        'ComboBox2
        '
        Me.ComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.Items.AddRange(New Object() {"Microsoft Windows 10", "Microsoft Windows 11"})
        Me.ComboBox2.Location = New System.Drawing.Point(35, 343)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(260, 21)
        Me.ComboBox2.TabIndex = 29
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(32, 327)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(136, 13)
        Me.Label15.TabIndex = 27
        Me.Label15.Text = "Sistema Operativo máquina"
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(35, 238)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(260, 20)
        Me.TextBox3.TabIndex = 24
        Me.TextBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(32, 222)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(108, 13)
        Me.Label6.TabIndex = 25
        Me.Label6.Text = "Dirección IP máquina"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(299, 22)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(173, 13)
        Me.Label13.TabIndex = 23
        Me.Label13.Text = "Permisos aplicaciones de deslindes"
        '
        'ListView1
        '
        Me.ListView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView1.HideSelection = False
        Me.ListView1.Location = New System.Drawing.Point(302, 38)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(647, 326)
        Me.ListView1.TabIndex = 22
        Me.ListView1.UseCompatibleStateImageBehavior = False
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Image = CType(resources.GetObject("Button1.Image"), System.Drawing.Image)
        Me.Button1.Location = New System.Drawing.Point(838, 399)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(111, 42)
        Me.Button1.TabIndex = 12
        Me.Button1.Text = "Crear usuario"
        Me.Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button1.UseVisualStyleBackColor = True
        '
        'txtNewUserTfno
        '
        Me.txtNewUserTfno.Location = New System.Drawing.Point(35, 136)
        Me.txtNewUserTfno.Name = "txtNewUserTfno"
        Me.txtNewUserTfno.Size = New System.Drawing.Size(260, 20)
        Me.txtNewUserTfno.TabIndex = 2
        Me.txtNewUserTfno.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(32, 120)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(49, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Teléfono"
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(300, 391)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(137, 13)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Password para acceso App"
        '
        'txtNewUserPass
        '
        Me.txtNewUserPass.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtNewUserPass.Location = New System.Drawing.Point(302, 407)
        Me.txtNewUserPass.Name = "txtNewUserPass"
        Me.txtNewUserPass.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtNewUserPass.Size = New System.Drawing.Size(266, 20)
        Me.txtNewUserPass.TabIndex = 4
        '
        'txtNewUserWinLogin
        '
        Me.txtNewUserWinLogin.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtNewUserWinLogin.Location = New System.Drawing.Point(35, 407)
        Me.txtNewUserWinLogin.Name = "txtNewUserWinLogin"
        Me.txtNewUserWinLogin.Size = New System.Drawing.Size(261, 20)
        Me.txtNewUserWinLogin.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(32, 391)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(105, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Usuario en Windows"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(32, 71)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(49, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Apellidos"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(32, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(44, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Nombre"
        '
        'txtNewUserSurname
        '
        Me.txtNewUserSurname.Location = New System.Drawing.Point(35, 87)
        Me.txtNewUserSurname.Name = "txtNewUserSurname"
        Me.txtNewUserSurname.Size = New System.Drawing.Size(260, 20)
        Me.txtNewUserSurname.TabIndex = 1
        '
        'txtNewUserNombre
        '
        Me.txtNewUserNombre.Location = New System.Drawing.Point(35, 38)
        Me.txtNewUserNombre.Name = "txtNewUserNombre"
        Me.txtNewUserNombre.Size = New System.Drawing.Size(260, 20)
        Me.txtNewUserNombre.TabIndex = 0
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.GroupBox1)
        Me.TabPage2.Controls.Add(Me.lvListaUser)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(976, 472)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Lista de usuarios"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.Button6)
        Me.GroupBox1.Controls.Add(Me.Button5)
        Me.GroupBox1.Controls.Add(Me.TextBox12)
        Me.GroupBox1.Controls.Add(Me.Label19)
        Me.GroupBox1.Controls.Add(Me.Label17)
        Me.GroupBox1.Controls.Add(Me.TextBox5)
        Me.GroupBox1.Controls.Add(Me.Label14)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.TextBox2)
        Me.GroupBox1.Controls.Add(Me.TextBox1)
        Me.GroupBox1.Controls.Add(Me.CheckBox1)
        Me.GroupBox1.Controls.Add(Me.ListView2)
        Me.GroupBox1.Controls.Add(Me.Button4)
        Me.GroupBox1.Controls.Add(Me.Button3)
        Me.GroupBox1.Controls.Add(Me.TextBox10)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.TextBox9)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.TextBox8)
        Me.GroupBox1.Controls.Add(Me.TextBox7)
        Me.GroupBox1.Controls.Add(Me.TextBox6)
        Me.GroupBox1.Controls.Add(Me.Button2)
        Me.GroupBox1.Location = New System.Drawing.Point(127, 18)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(837, 438)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "GroupBox1"
        '
        'Button6
        '
        Me.Button6.Image = CType(resources.GetObject("Button6.Image"), System.Drawing.Image)
        Me.Button6.Location = New System.Drawing.Point(199, 387)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(135, 37)
        Me.Button6.TabIndex = 37
        Me.Button6.Text = "Deshabilitar usuario"
        Me.Button6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button6.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Image = CType(resources.GetObject("Button5.Image"), System.Drawing.Image)
        Me.Button5.Location = New System.Drawing.Point(18, 389)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(135, 37)
        Me.Button5.TabIndex = 36
        Me.Button5.Text = "Habilitar usuario"
        Me.Button5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button5.UseVisualStyleBackColor = True
        '
        'TextBox12
        '
        Me.TextBox12.Location = New System.Drawing.Point(86, 208)
        Me.TextBox12.Name = "TextBox12"
        Me.TextBox12.Size = New System.Drawing.Size(248, 20)
        Me.TextBox12.TabIndex = 31
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(43, 211)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(34, 13)
        Me.Label19.TabIndex = 30
        Me.Label19.Text = "e-mail"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(16, 133)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(61, 13)
        Me.Label17.TabIndex = 29
        Me.Label17.Text = "Nombre PC"
        '
        'TextBox5
        '
        Me.TextBox5.Location = New System.Drawing.Point(86, 182)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Size = New System.Drawing.Size(248, 20)
        Me.TextBox5.TabIndex = 28
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(16, 185)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(65, 13)
        Me.Label14.TabIndex = 27
        Me.Label14.Text = "OS máquina"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(21, 159)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(60, 13)
        Me.Label12.TabIndex = 25
        Me.Label12.Text = "IP máquina"
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(86, 156)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(248, 20)
        Me.TextBox2.TabIndex = 24
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(86, 130)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(248, 20)
        Me.TextBox1.TabIndex = 23
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(86, 361)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(155, 17)
        Me.CheckBox1.TabIndex = 22
        Me.CheckBox1.Text = "Redefinir usuario/password"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'ListView2
        '
        Me.ListView2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView2.HideSelection = False
        Me.ListView2.Location = New System.Drawing.Point(340, 52)
        Me.ListView2.Name = "ListView2"
        Me.ListView2.Size = New System.Drawing.Size(490, 318)
        Me.ListView2.TabIndex = 21
        Me.ListView2.UseCompatibleStateImageBehavior = False
        '
        'Button4
        '
        Me.Button4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button4.Image = CType(resources.GetObject("Button4.Image"), System.Drawing.Image)
        Me.Button4.Location = New System.Drawing.Point(635, 387)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(94, 32)
        Me.Button4.TabIndex = 20
        Me.Button4.Text = "Eliminar"
        Me.Button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button3.Image = CType(resources.GetObject("Button3.Image"), System.Drawing.Image)
        Me.Button3.Location = New System.Drawing.Point(737, 387)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(94, 32)
        Me.Button3.TabIndex = 19
        Me.Button3.Text = "Guardar"
        Me.Button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button3.UseVisualStyleBackColor = True
        '
        'TextBox10
        '
        Me.TextBox10.Location = New System.Drawing.Point(86, 335)
        Me.TextBox10.Name = "TextBox10"
        Me.TextBox10.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TextBox10.Size = New System.Drawing.Size(248, 20)
        Me.TextBox10.TabIndex = 16
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(28, 338)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(53, 13)
        Me.Label11.TabIndex = 15
        Me.Label11.Text = "Password"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(38, 312)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(43, 13)
        Me.Label10.TabIndex = 14
        Me.Label10.Text = "Usuario"
        '
        'TextBox9
        '
        Me.TextBox9.Location = New System.Drawing.Point(86, 309)
        Me.TextBox9.Name = "TextBox9"
        Me.TextBox9.Size = New System.Drawing.Size(248, 20)
        Me.TextBox9.TabIndex = 13
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(32, 81)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(49, 13)
        Me.Label9.TabIndex = 12
        Me.Label9.Text = "Teléfono"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(32, 107)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(49, 13)
        Me.Label8.TabIndex = 11
        Me.Label8.Text = "Apellidos"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(37, 55)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(44, 13)
        Me.Label7.TabIndex = 10
        Me.Label7.Text = "Nombre"
        '
        'TextBox8
        '
        Me.TextBox8.Location = New System.Drawing.Point(86, 78)
        Me.TextBox8.Name = "TextBox8"
        Me.TextBox8.Size = New System.Drawing.Size(248, 20)
        Me.TextBox8.TabIndex = 9
        '
        'TextBox7
        '
        Me.TextBox7.Location = New System.Drawing.Point(86, 104)
        Me.TextBox7.Name = "TextBox7"
        Me.TextBox7.Size = New System.Drawing.Size(248, 20)
        Me.TextBox7.TabIndex = 8
        '
        'TextBox6
        '
        Me.TextBox6.Location = New System.Drawing.Point(86, 52)
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.Size = New System.Drawing.Size(248, 20)
        Me.TextBox6.TabIndex = 7
        '
        'Button2
        '
        Me.Button2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button2.Image = CType(resources.GetObject("Button2.Image"), System.Drawing.Image)
        Me.Button2.Location = New System.Drawing.Point(790, 9)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(41, 32)
        Me.Button2.TabIndex = 6
        Me.Button2.UseVisualStyleBackColor = True
        '
        'lvListaUser
        '
        Me.lvListaUser.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvListaUser.HideSelection = False
        Me.lvListaUser.Location = New System.Drawing.Point(6, 18)
        Me.lvListaUser.MultiSelect = False
        Me.lvListaUser.Name = "lvListaUser"
        Me.lvListaUser.Size = New System.Drawing.Size(115, 438)
        Me.lvListaUser.TabIndex = 0
        Me.lvListaUser.UseCompatibleStateImageBehavior = False
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 539)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1008, 22)
        Me.StatusStrip1.TabIndex = 2
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(119, 17)
        Me.ToolStripStatusLabel1.Text = "ToolStripStatusLabel1"
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'GestionUserForm
        '
        Me.ClientSize = New System.Drawing.Size(1008, 561)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.TabControl1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "GestionUserForm"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region

    Public Sub New()
        InitializeComponent()
    End Sub


    Private Sub ResizeListView(container As ListView)

        If container.Columns.Count = 0 Then Exit Sub
        Try
            Dim anchoCol As Integer = 0
            anchoCol = (container.Width - 30) / container.Columns.Count
            For iCol = 0 To container.Columns.Count - 1
                container.Columns(iCol).Width = anchoCol
            Next
        Catch ex As Exception
            ModalError("Problemas al redimensionar el contenedor:" & ex.Message)
        End Try

    End Sub




    Private Sub GestionUserForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Text = "Gestión de usuarios de " & AplicacionTitulo

        Button5.Location = New Point(200, 385)
        Button6.Location = New Point(200, 385)
        Button5.Visible = False
        Button6.Visible = False

        lvListaUser.Location = New Point(14, 6)
        lvListaUser.Size = New Point(950, 450)
        lvListaUser.SmallImageList = MDIPrincipal.ImageList1
        GroupBox1.Location = New Point(14, 6)
        GroupBox1.Size = New Point(950, 450)
        GroupBox1.Visible = False

        lvListaUser.FullRowSelect = True
        lvListaUser.View = View.Details

        lvListaUser.Columns.Add("Nombre", 90, HorizontalAlignment.Left)
        lvListaUser.Columns.Add("Apellidos", 120, HorizontalAlignment.Left)
        lvListaUser.Columns.Add("Teléfono", 70, HorizontalAlignment.Left)
        lvListaUser.Columns.Add("Usuario", 75, HorizontalAlignment.Left)
        lvListaUser.Columns.Add("Permiso " & AplicacionTitulo, 100, HorizontalAlignment.Left)

        ListView1.FullRowSelect = True
        ListView1.View = View.Details
        ListView1.Columns.Add("Permiso", 300, HorizontalAlignment.Left)
        ListView1.CheckBoxes = True
        ListView2.FullRowSelect = True
        ListView2.View = View.Details
        ListView2.Columns.Add("Permiso", 300, HorizontalAlignment.Left)
        ListView2.CheckBoxes = True
        Me.Cursor = Cursors.WaitCursor
        NombreCampoPermisos = usuarioMyApp.permisos.FieldGrants
        CargarUsuarios()
        cargarPermisosBasico()
        Me.Cursor = Cursors.Default
        ResizeListView(lvListaUser)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim cadInsert As String
        Dim okValid As Boolean
        Dim LoginPass As String = ""
        Dim codUser As Integer = 0


        If txtNewUserWinLogin.Text.Trim = "" Then
            ErrorProvider1.SetError(Me.txtNewUserWinLogin, "El usuario es obligatorio")
        Else
            ErrorProvider1.SetError(Me.txtNewUserWinLogin, "")
            okValid = True
        End If
        If okValid = False Then Exit Sub

        If txtNewUserPass.Text.Trim = "" Then
            ErrorProvider1.SetError(Me.txtNewUserPass, "Asignar una password obligatoria. Por defecto, poner tera")
        Else
            ErrorProvider1.SetError(Me.txtNewUserPass, "")
            LoginPass = txtNewUserPass.Text.Trim
        End If
        If LoginPass = "" Then Exit Sub

        'Validamos si existe el usuario
        ObtenerEscalar("SELECT iduser from bdsidschema.usuarios where loginuser='" & txtNewUserWinLogin.Text.Trim & "'", codUser)
        If codUser > 0 Then
            ModalExclamation("El usuario ya existe en el sistema")
            Exit Sub
        End If

        'Asignar permisos
        Dim cadPermisos As String = ""
        Dim codPermisos As Integer = 0
        For Each elem As ListViewItem In ListView1.Items
            If elem.Checked = True Then
                cadPermisos &= "1"
            Else
                cadPermisos &= "0"
            End If
        Next

        If cadPermisos.Length = usuarioMyApp.permisos.numPermisosApp Then
            codPermisos = develmap.develcode.BaseConversor.FromNumBase(cadPermisos, 2)
        Else
            ModalExclamation("El número de los permisos asignados es distinto del número de permisos permitidos")
            Exit Sub
        End If

        cadInsert = "INSERT INTO bdsidschema.usuarios (iduser,nombre,apellidos,telefono,ipaddress,namemachine,email,enable,usersystem,loginuser,loginpassw," & NombreCampoPermisos & ") " &
                    "VALUES (" &
                    "nextval('bdsidschema.usuarios_iduser_seq')," &
                    "'" & txtNewUserNombre.Text.Trim.ToLower & "'," &
                    "'" & txtNewUserSurname.Text.Trim & "'," &
                    "'" & txtNewUserTfno.Text.Trim & "'," &
                    IIf(TextBox3.Text = "", "Null,", "'" & TextBox3.Text.Trim & "',") &
                    IIf(TextBox4.Text = "", "Null,", "'" & TextBox4.Text.Trim & "',") &
                    IIf(TextBox11.Text = "", "Null,", "'" & TextBox11.Text.Trim & "',") &
                    IIf(RadioButton1.Checked = True, "1,", "0,") &
                    IIf(ComboBox2.Text = "", "Null,", "'" & ComboBox2.Text & "',") &
                    "'" & txtNewUserWinLogin.Text.Trim & "'," &
                    "md5('" & LoginPass & "' || '" & txtNewUserWinLogin.Text.Trim & "' || 'dvmap')," &
                    codPermisos & ")"
        Me.Cursor = Cursors.WaitCursor
        If ExeSinTran(cadInsert) Then
            CargarUsuarios()
            ModalInfo("Usuario " & txtNewUserWinLogin.Text.Trim & " creado")
        Else
            ModalExclamation("No se creo ningún usuario")
        End If
        GroupBox1.Visible = False
        Me.Cursor = Cursors.Default

    End Sub



    Private Sub lvListaUser_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvListaUser.DoubleClick

        Dim elementoSel As Integer
        elementoSel = lvListaUser.SelectedItems(0).Tag
        LimpiarCampos()
        GroupBox1.Visible = True

        Dim userEdit As myAppUser
        userEdit = ListaUsuarios(elementoSel)

        TextBox6.Text = userEdit.nombre
        TextBox7.Text = userEdit.apellidos
        TextBox8.Text = userEdit.telefono
        TextBox9.Text = userEdit.loginUser

        TextBox1.Text = userEdit.machineName
        TextBox2.Text = userEdit.machineIP
        TextBox5.Text = userEdit.machineSO
        TextBox12.Text = userEdit.correo_electronico


        GroupBox1.Text = "Edición usuario: " & userEdit.nombre
        GroupBox1.Tag = userEdit.Id

        'Rellenamos el LV de permisos de Deslindes
        ListView2.Items(0).Checked = userEdit.Permisos.AccesoMyApp
        ListView2.Items(1).Checked = userEdit.Permisos.EditarDocumentacion
        ListView2.Items(2).Checked = userEdit.Permisos.AsignarPermisosUsuarios
        ListView2.Items(3).Checked = userEdit.Permisos.GenerarVersionCdD
        ListView2.Items(4).Checked = userEdit.Permisos.AsignarParamsWMS

        TextBox9.Enabled = False
        TextBox10.Enabled = False
        CheckBox1.Checked = False

        If userEdit.userEnabled Then
            Button5.Visible = False
            Button6.Visible = True
        Else
            Button5.Visible = True
            Button6.Visible = False
        End If

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        GroupBox1.Visible = False
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

        Dim cadUpdate As String
        Dim LoginPass As String = ""
        Dim LoginUser As String = ""
        Dim cadPermisos As String = ""
        Dim codPermisos As Integer

        If CheckBox1.Checked = True Then
            If TextBox9.Text.Trim = "" Then
                ErrorProvider1.SetError(Me.TextBox9, "El usuario es obligatorio")
            Else
                ErrorProvider1.SetError(Me.TextBox9, "")
                LoginUser = TextBox9.Text.Trim
            End If
            If TextBox10.Text.Trim = "" Then
                ErrorProvider1.SetError(Me.TextBox10, "Asignar una password obligatoria. Se aconseja por defecto, poner welcome")
            Else
                ErrorProvider1.SetError(Me.TextBox10, "")
                LoginPass = TextBox10.Text.Trim
            End If
            If LoginUser = "" Then Exit Sub
            If LoginPass = "" Then Exit Sub
        End If

        For Each elem As ListViewItem In ListView2.Items
            If elem.Checked = True Then
                cadPermisos = cadPermisos & "1"
            Else
                cadPermisos = cadPermisos & "0"
            End If
        Next
        If cadPermisos.Length = usuarioMyApp.permisos.numPermisosApp Then
            codPermisos = develmap.develcode.BaseConversor.FromNumBase(cadPermisos, 2)
        Else
            ModalExclamation("El número de los permisos asignados es distinto del número de permisos permitidos")
            Exit Sub
        End If

        If CheckBox1.Checked = True Then
            cadUpdate = "UPDATE bdsidschema.usuarios SET " &
                        "nombre='" & TextBox6.Text.Trim & "'," &
                        "apellidos='" & TextBox7.Text.Trim & "'," &
                        "telefono='" & TextBox8.Text.Trim & "'," &
                        "loginuser='" & TextBox9.Text.Trim & "'," &
                        "loginpassw=md5('" & LoginPass & "' || '" & TextBox9.Text.Trim & "' || 'dvmap')," &
                        "ipaddress='" & TextBox2.Text.Trim & "'," &
                        "email='" & TextBox12.Text.Trim & "'," &
                        "usersystem='" & TextBox5.Text.Trim & "'," &
                        "namemachine='" & TextBox1.Text.Trim & "'," &
                        NombreCampoPermisos & "=" & codPermisos & " " &
                        "WHERE iduser=" & GroupBox1.Tag.ToString
        Else
            cadUpdate = "UPDATE bdsidschema.usuarios SET " &
                        "nombre='" & TextBox6.Text.Trim & "'," &
                        "apellidos='" & TextBox7.Text.Trim & "'," &
                        "telefono='" & TextBox8.Text.Trim & "'," &
                        "ipaddress='" & TextBox2.Text.Trim & "'," &
                        "email='" & TextBox12.Text.Trim & "'," &
                        "usersystem='" & TextBox5.Text.Trim & "'," &
                        "namemachine='" & TextBox1.Text.Trim & "'," &
                        NombreCampoPermisos & "=" & codPermisos & " " &
                        "WHERE iduser=" & GroupBox1.Tag.ToString
        End If

        Me.Cursor = Cursors.WaitCursor
        If ExeSinTran(cadUpdate) Then
            CargarUsuarios()
            ModalInfo($"Usuario {TextBox9.Text.Trim} modificado")
        Else
            ModalExclamation("No se modificó ningún usuario")
        End If
        GroupBox1.Visible = False
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click

        Dim cadDelete As String

        If ModalQuestion("¿Desea borrar el usuario " & TextBox9.Text & "?") = DialogResult.No Then Exit Sub

        cadDelete = "DELETE FROM bdsidschema.usuarios WHERE iduser=" & GroupBox1.Tag.ToString
        If ExeSinTran(cadDelete) Then
            ModalInfo("Usuario eliminado")
            CargarUsuarios()
            GroupBox1.Visible = False
        Else
            ModalExclamation("No se eliminó ningún usuario")
        End If


    End Sub


    Private Sub CheckBox1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox1.Click
        If CheckBox1.Checked = True Then
            TextBox9.Enabled = True
            TextBox10.Enabled = True
        Else
            TextBox9.Enabled = False
            TextBox10.Enabled = False
        End If

    End Sub

    Sub CargarUsuarios()

        Dim rcdUsuarios As DataTable
        Dim filas() As DataRow
        Dim contador As Integer
        Dim usuario As myAppUser

        ListaUsuarios = New ArrayList

        Try
            rcdUsuarios = New DataTable
            If CargarRecordset("SELECT * from bdsidschema.usuarios ORDER BY iduser", rcdUsuarios) = True Then
                filas = rcdUsuarios.Select
                contador = -1
                For Each dR As DataRow In filas
                    contador += 1
                    usuario = New myAppUser With {
                        .id = dR("iduser"),
                        .nombre = dR("nombre").ToString,
                        .apellidos = dR("apellidos").ToString,
                        .loginUser = dR("loginuser").ToString,
                        .telefono = dR("telefono").ToString,
                        .codPermisoMultiple = dR(NombreCampoPermisos),
                        .machineIP = dR("ipaddress").ToString,
                        .machineSO = dR("usersystem").ToString,
                        .machineName = dR("namemachine").ToString,
                        .correo_electronico = dR("email").ToString,
                        .userEnabled = dR("enable").ToString
                    }
                    ListaUsuarios.Add(usuario)
                Next
                rcdUsuarios.Dispose()
            End If
        Catch ex As Exception
            ModalError(ex.Message)
            GenerarLOG(ex.Message)
        Finally
            rcdUsuarios = Nothing
            Erase filas
        End Try

        RellenarLV()

    End Sub

    Private Sub cargarPermisosBasico()

        Dim elementoLV1 As ListViewItem
        Dim elementoLV2 As ListViewItem


        For Each permiso As String In profile.ListaPermisosApp
            elementoLV1 = New ListViewItem
            elementoLV2 = New ListViewItem
            elementoLV1.Text = permiso
            elementoLV2.Text = permiso
            ListView1.Items.Add(elementoLV1)
            ListView2.Items.Add(elementoLV2)
            elementoLV1 = Nothing
            elementoLV2 = Nothing
        Next


    End Sub

    Private Sub RellenarLV()

        Dim elementoLV As ListViewItem
        Dim contador As Integer
        Dim mostrar As Boolean
        If ListaUsuarios Is Nothing Then Exit Sub
        lvListaUser.Items.Clear()

        contador = -1
        For Each usu As myAppUser In ListaUsuarios
            contador = contador + 1
            mostrar = False
            elementoLV = New ListViewItem
            elementoLV.Text = usu.nombre
            elementoLV.SubItems.Add(usu.apellidos)
            elementoLV.SubItems.Add(usu.telefono)
            elementoLV.SubItems.Add(usu.loginUser)
            elementoLV.SubItems.Add(IIf(usu.codPermisoMultiple > 0, "Tiene permisos", "No tiene permisos"))
            elementoLV.Tag = contador
            elementoLV.BackColor = IIf(lvListaUser.Items.Count Mod 2 = 0, Color.White, Color.WhiteSmoke)
            If usu.userEnabled Then
                elementoLV.ImageIndex = 2
            Else
                elementoLV.ImageIndex = 3
            End If
            lvListaUser.Items.Add(elementoLV)
            elementoLV = Nothing

        Next
        ToolStripStatusLabel1.Text = "Usuarios: " & lvListaUser.Items.Count

    End Sub

    Private Sub LimpiarCampos()

        GroupBox1.Tag = "0"
        txtNewUserNombre.Text = ""
        txtNewUserSurname.Text = ""
        txtNewUserWinLogin.Text = ""
        txtNewUserPass.Text = ""
        txtNewUserTfno.Text = ""
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox5.Text = ""
        TextBox6.Text = ""
        TextBox7.Text = ""
        TextBox8.Text = ""
        TextBox9.Text = ""
        TextBox10.Text = ""
        TextBox11.Text = ""
        TextBox12.Text = ""
        ComboBox2.Text = ""
        Button5.Visible = False
        Button6.Visible = False
        For Each elem As ListViewItem In ListView2.Items
            elem.Checked = False
        Next

    End Sub

    Private Sub GestionUserForm_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        ResizeListView(lvListaUser)
    End Sub

    Private Sub ToggleUserEnabled(sender As Object, e As EventArgs) Handles Button5.Click, Button6.Click

        Dim cadUpdate As String = ""

        If sender.name = "Button5" Then
            cadUpdate = "UPDATE bdsidschema.usuarios SET enable=1 WHERE iduser=" & GroupBox1.Tag.ToString
        ElseIf sender.name = "Button6" Then
            cadUpdate = "UPDATE bdsidschema.usuarios SET enable=0 WHERE iduser=" & GroupBox1.Tag.ToString
        Else
            Exit Sub
        End If

        Me.Cursor = Cursors.WaitCursor
        If ExeSinTran(cadUpdate) Then
            CargarUsuarios()
            ModalInfo("Usuario " & IIf(sender.name = "Button5", " habilitado", " deshabilitado"))
        Else
            ModalExclamation("No se modificó ningún usuario")
        End If
        GroupBox1.Visible = False
        Me.Cursor = Cursors.Default

    End Sub

End Class
