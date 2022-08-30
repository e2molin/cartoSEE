Public Class GestionUserForm
    Inherits System.Windows.Forms.Form

    Dim ListaUsuarios As New ArrayList
    Const grantedFieldName As String = "permisocartosee"

    '00.Acceder aplicación
    '01.Editar documentos
    '02.Asignar permisos a users
    '03.Generar versión CdD
    '04.Asignar parámetros del WMS
    '05.No asignado
    '06.No asignado
    '07.No asignado
    '08.No asignado
    '09.No asignado
    '10.No asignado
    Dim ListaPermisosApp() As String = {"Acceso a aplicación cartoSEE", "Edición de documentos", "Asignar permisos de usuarios", "Generar versión CdD", "Asignar parámetros del WMS", "No asignado",
                                         "No asignado", "No asignado", "No asignado", "No asignado", "No asignado"}





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
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox5 As TextBox
    Friend WithEvents Label16 As Label
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents Label15 As Label
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents Label14 As Label
    Friend WithEvents TextBox11 As TextBox
    Friend WithEvents Label17 As Label
    Friend WithEvents CheckBox2 As CheckBox
    Friend WithEvents Label18 As Label
    Friend WithEvents TextBox12 As TextBox
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(GestionUserForm))
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
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
        Me.CheckBox2 = New System.Windows.Forms.CheckBox()
        Me.TextBox11 = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
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
        Me.Label18 = New System.Windows.Forms.Label()
        Me.TextBox12 = New System.Windows.Forms.TextBox()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(12, 26)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(668, 466)
        Me.TabControl1.TabIndex = 1
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.TextBox5)
        Me.TabPage1.Controls.Add(Me.Label16)
        Me.TabPage1.Controls.Add(Me.TextBox4)
        Me.TabPage1.Controls.Add(Me.Label15)
        Me.TabPage1.Controls.Add(Me.TextBox3)
        Me.TabPage1.Controls.Add(Me.Label14)
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
        Me.TabPage1.Size = New System.Drawing.Size(660, 440)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Nuevo usuario"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TextBox5
        '
        Me.TextBox5.Location = New System.Drawing.Point(36, 280)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Size = New System.Drawing.Size(203, 20)
        Me.TextBox5.TabIndex = 28
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(33, 264)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(93, 13)
        Me.Label16.TabIndex = 29
        Me.Label16.Text = "Correo electrónico"
        '
        'TextBox4
        '
        Me.TextBox4.Location = New System.Drawing.Point(36, 228)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(203, 20)
        Me.TextBox4.TabIndex = 26
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(33, 212)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(86, 13)
        Me.Label15.TabIndex = 27
        Me.Label15.Text = "Máquina nombre"
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(35, 184)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(203, 20)
        Me.TextBox3.TabIndex = 24
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(32, 168)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(61, 13)
        Me.Label14.TabIndex = 25
        Me.Label14.Text = "Máquina IP"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(299, 22)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(104, 13)
        Me.Label13.TabIndex = 23
        Me.Label13.Text = "Permisos disponibles"
        '
        'ListView1
        '
        Me.ListView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView1.HideSelection = False
        Me.ListView1.Location = New System.Drawing.Point(302, 38)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(331, 337)
        Me.ListView1.TabIndex = 22
        Me.ListView1.UseCompatibleStateImageBehavior = False
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Image = CType(resources.GetObject("Button1.Image"), System.Drawing.Image)
        Me.Button1.Location = New System.Drawing.Point(522, 383)
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
        Me.txtNewUserTfno.Size = New System.Drawing.Size(203, 20)
        Me.txtNewUserTfno.TabIndex = 2
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
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(33, 389)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 13)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Password"
        '
        'txtNewUserPass
        '
        Me.txtNewUserPass.Location = New System.Drawing.Point(35, 405)
        Me.txtNewUserPass.Name = "txtNewUserPass"
        Me.txtNewUserPass.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtNewUserPass.Size = New System.Drawing.Size(164, 20)
        Me.txtNewUserPass.TabIndex = 4
        '
        'txtNewUserWinLogin
        '
        Me.txtNewUserWinLogin.Location = New System.Drawing.Point(35, 355)
        Me.txtNewUserWinLogin.Name = "txtNewUserWinLogin"
        Me.txtNewUserWinLogin.Size = New System.Drawing.Size(164, 20)
        Me.txtNewUserWinLogin.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(32, 339)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(90, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Usuario Windows"
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
        Me.txtNewUserSurname.Size = New System.Drawing.Size(203, 20)
        Me.txtNewUserSurname.TabIndex = 1
        '
        'txtNewUserNombre
        '
        Me.txtNewUserNombre.Location = New System.Drawing.Point(35, 38)
        Me.txtNewUserNombre.Name = "txtNewUserNombre"
        Me.txtNewUserNombre.Size = New System.Drawing.Size(203, 20)
        Me.txtNewUserNombre.TabIndex = 0
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.GroupBox1)
        Me.TabPage2.Controls.Add(Me.lvListaUser)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(660, 440)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Lista de usuarios"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.Label18)
        Me.GroupBox1.Controls.Add(Me.TextBox12)
        Me.GroupBox1.Controls.Add(Me.CheckBox2)
        Me.GroupBox1.Controls.Add(Me.TextBox11)
        Me.GroupBox1.Controls.Add(Me.Label17)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.TextBox2)
        Me.GroupBox1.Controls.Add(Me.Label6)
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
        Me.GroupBox1.Location = New System.Drawing.Point(14, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(650, 414)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "GroupBox1"
        '
        'CheckBox2
        '
        Me.CheckBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBox2.Appearance = System.Windows.Forms.Appearance.Button
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckBox2.Location = New System.Drawing.Point(250, 367)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(86, 23)
        Me.CheckBox2.TabIndex = 29
        Me.CheckBox2.Text = "Usuario Activo"
        Me.CheckBox2.UseVisualStyleBackColor = True
        '
        'TextBox11
        '
        Me.TextBox11.Location = New System.Drawing.Point(86, 209)
        Me.TextBox11.Name = "TextBox11"
        Me.TextBox11.Size = New System.Drawing.Size(137, 20)
        Me.TextBox11.TabIndex = 28
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(45, 212)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(35, 13)
        Me.Label17.TabIndex = 27
        Me.Label17.Text = "E-mail"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(21, 159)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(65, 13)
        Me.Label12.TabIndex = 26
        Me.Label12.Text = "OS máquina"
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(86, 156)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(137, 20)
        Me.TextBox2.TabIndex = 25
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(21, 133)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(60, 13)
        Me.Label6.TabIndex = 24
        Me.Label6.Text = "IP máquina"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(86, 130)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(137, 20)
        Me.TextBox1.TabIndex = 23
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(86, 329)
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
        Me.ListView2.Location = New System.Drawing.Point(250, 52)
        Me.ListView2.Name = "ListView2"
        Me.ListView2.Size = New System.Drawing.Size(331, 294)
        Me.ListView2.TabIndex = 21
        Me.ListView2.UseCompatibleStateImageBehavior = False
        '
        'Button4
        '
        Me.Button4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button4.Image = CType(resources.GetObject("Button4.Image"), System.Drawing.Image)
        Me.Button4.Location = New System.Drawing.Point(385, 362)
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
        Me.Button3.Location = New System.Drawing.Point(487, 362)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(94, 32)
        Me.Button3.TabIndex = 19
        Me.Button3.Text = "Guardar"
        Me.Button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button3.UseVisualStyleBackColor = True
        '
        'TextBox10
        '
        Me.TextBox10.Location = New System.Drawing.Point(86, 303)
        Me.TextBox10.Name = "TextBox10"
        Me.TextBox10.Size = New System.Drawing.Size(137, 20)
        Me.TextBox10.TabIndex = 16
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(28, 306)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(53, 13)
        Me.Label11.TabIndex = 15
        Me.Label11.Text = "Password"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(12, 280)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(68, 13)
        Me.Label10.TabIndex = 14
        Me.Label10.Text = "User Intranet"
        '
        'TextBox9
        '
        Me.TextBox9.Location = New System.Drawing.Point(86, 277)
        Me.TextBox9.Name = "TextBox9"
        Me.TextBox9.Size = New System.Drawing.Size(137, 20)
        Me.TextBox9.TabIndex = 13
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(32, 107)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(49, 13)
        Me.Label9.TabIndex = 12
        Me.Label9.Text = "Teléfono"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(32, 81)
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
        Me.TextBox8.Location = New System.Drawing.Point(86, 104)
        Me.TextBox8.Name = "TextBox8"
        Me.TextBox8.Size = New System.Drawing.Size(137, 20)
        Me.TextBox8.TabIndex = 9
        '
        'TextBox7
        '
        Me.TextBox7.Location = New System.Drawing.Point(86, 78)
        Me.TextBox7.Name = "TextBox7"
        Me.TextBox7.Size = New System.Drawing.Size(137, 20)
        Me.TextBox7.TabIndex = 8
        '
        'TextBox6
        '
        Me.TextBox6.Location = New System.Drawing.Point(86, 52)
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.Size = New System.Drawing.Size(137, 20)
        Me.TextBox6.TabIndex = 7
        '
        'Button2
        '
        Me.Button2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button2.Image = CType(resources.GetObject("Button2.Image"), System.Drawing.Image)
        Me.Button2.Location = New System.Drawing.Point(599, 9)
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
        Me.lvListaUser.Location = New System.Drawing.Point(6, 6)
        Me.lvListaUser.MultiSelect = False
        Me.lvListaUser.Name = "lvListaUser"
        Me.lvListaUser.Size = New System.Drawing.Size(65, 428)
        Me.lvListaUser.TabIndex = 0
        Me.lvListaUser.UseCompatibleStateImageBehavior = False
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 511)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(692, 22)
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
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(32, 185)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(48, 13)
        Me.Label18.TabIndex = 31
        Me.Label18.Text = "Máquina"
        '
        'TextBox12
        '
        Me.TextBox12.Location = New System.Drawing.Point(86, 182)
        Me.TextBox12.Name = "TextBox12"
        Me.TextBox12.Size = New System.Drawing.Size(137, 20)
        Me.TextBox12.TabIndex = 30
        '
        'GestionUserForm
        '
        Me.ClientSize = New System.Drawing.Size(692, 533)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.TabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "GestionUserForm"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
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

    Private Sub GestionUserForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Text = "Gestión de usuarios de " & AplicacionTitulo
        lvListaUser.Location = New Point(6, 6)
        lvListaUser.Size = New Point(650, 430)
        GroupBox1.Location = New Point(6, 6)
        GroupBox1.Size = New Point(650, 430)
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
        CargarUsuarios()
        cargarPermisosBasico()
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
            ErrorProvider1.SetError(Me.txtNewUserPass, "Asignar una password obligatoria. Por defecto, poner usuario + pass")
        Else
            ErrorProvider1.SetError(Me.txtNewUserPass, "")
            LoginPass = txtNewUserPass.Text.Trim
        End If
        If LoginPass = "" Then Exit Sub

        'Validamos si existe el usuario
        ObtenerEscalar("SELECT iduser from usuarios where loginuser='" & txtNewUserWinLogin.Text.Trim & "'", codUser)
        If codUser > 0 Then
            MessageBox.Show("El usuario ya existe en el sistema", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        'Asignar permisos
        Dim cadPermisos As String = ""
        Dim codPermisos As Integer = 0
        For Each elem As ListViewItem In ListView1.Items
            If elem.Checked = True Then
                cadPermisos = cadPermisos & "1"
            Else
                cadPermisos = cadPermisos & "0"
            End If
        Next
        If cadPermisos.Length = 11 Then
            codPermisos = develmap.develcode.BaseConversor.FromNumBase(cadPermisos, 2)
        End If

        cadInsert = "INSERT INTO bdsidschema.usuarios (iduser,nombre,apellidos,enable,telefono,ipaddress,email,namemachine,loginuser,loginpassw," & grantedFieldName & ") " &
                    "VALUES (" &
                    "nextval('usuarios_iduser_seq')," &
                    "'" & txtNewUserNombre.Text.Trim.ToLower & "'," &
                    "'" & txtNewUserSurname.Text.Trim & "'," &
                    "1," &
                    "'" & txtNewUserTfno.Text.Trim & "'," &
                    "'" & TextBox3.Text.Trim & "'," &
                    "'" & TextBox5.Text.Trim & "'," &
                    "'" & TextBox4.Text.Trim & "'," &
                    "'" & txtNewUserWinLogin.Text.Trim & "'," &
                    "md5('" & LoginPass & "' || '" & txtNewUserWinLogin.Text.Trim & "' || 'dvmap')," &
                    codPermisos & ")"

        Dim okProc As Boolean = ExeSinTran(cadInsert)
        If okProc = True Then
            MessageBox.Show("Usuario creado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            CargarUsuarios()
        Else
            MessageBox.Show("No se creo ningún usuario", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub



    Private Sub lvListaUser_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvListaUser.DoubleClick

        Dim elementoSel As Integer
        elementoSel = lvListaUser.SelectedItems(0).Tag
        limpiarCampos()
        GroupBox1.Visible = True

        Dim userEdit As myAppUser
        userEdit = ListaUsuarios(elementoSel)

        TextBox6.Text = userEdit.nombre
        TextBox7.Text = userEdit.apellidos
        TextBox8.Text = userEdit.telefono
        TextBox9.Text = userEdit.loginUser
        TextBox1.Text = userEdit.machineIP
        TextBox2.Text = userEdit.machineSO
        TextBox11.Text = userEdit.correoElectronico
        TextBox12.Text = userEdit.machineName

        GroupBox1.Text = "Edición usuario: " & userEdit.nombre
        GroupBox1.Tag = userEdit.id

        'Rellenamos el LV de permisos de Deslindes
        ListView2.Items(0).Checked = userEdit.permisosLista.hayAccesoMyApp
        ListView2.Items(1).Checked = userEdit.permisosLista.editarDocumentacion
        ListView2.Items(2).Checked = userEdit.permisosLista.asignarPermisosUsuarios
        ListView2.Items(3).Checked = userEdit.permisosLista.generarVersionCdD
        ListView2.Items(4).Checked = userEdit.permisosLista.asignarParamsWMS
        ListView2.Items(5).Checked = userEdit.permisosLista.disponible1
        ListView2.Items(6).Checked = userEdit.permisosLista.disponible2
        ListView2.Items(7).Checked = userEdit.permisosLista.disponible3
        ListView2.Items(8).Checked = userEdit.permisosLista.disponible4
        ListView2.Items(9).Checked = userEdit.permisosLista.disponible5
        ListView2.Items(10).Checked = userEdit.permisosLista.disponible6


        TextBox9.Enabled = False
        TextBox10.Enabled = False
        CheckBox1.Checked = False
        If userEdit.enabled Then
            CheckBox2.Checked = True
        Else
            CheckBox2.Checked = False
        End If

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        GroupBox1.Visible = False
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

        Dim cadUpdate As String
        Dim okValid As Boolean
        Dim LoginPass As String = ""
        Dim LoginUser As String = ""

        If CheckBox1.Checked = True Then
            If TextBox9.Text.Trim = "" Then
                ErrorProvider1.SetError(Me.TextBox9, "El usuario es obligatorio")
            Else
                ErrorProvider1.SetError(Me.TextBox9, "")
                okValid = True
                LoginUser = TextBox9.Text.Trim
            End If
            If TextBox10.Text.Trim = "" Then
                ErrorProvider1.SetError(Me.TextBox10, "Asignar una password obligatoria. Se aconseja por defecto, poner welcome1")
            Else
                ErrorProvider1.SetError(Me.TextBox10, "")
                LoginPass = TextBox10.Text.Trim
            End If
            If LoginUser = "" Then Exit Sub
            If LoginPass = "" Then Exit Sub
        End If


        If okValid = False And CheckBox1.Checked = True Then Exit Sub

        'Asignar permisos
        Dim cadPermisos As String = ""
        Dim codPermisos As Integer = 0

        For Each elem As ListViewItem In ListView2.Items
            If elem.Checked = True Then
                cadPermisos = cadPermisos & "1"
            Else
                cadPermisos = cadPermisos & "0"
            End If
        Next
        If cadPermisos.Length = 11 Then
            codPermisos = develmap.develcode.BaseConversor.FromNumBase(cadPermisos, 2)
        End If

        If CheckBox1.Checked = True Then
            cadUpdate = "UPDATE bdsidschema.usuarios SET " &
                        "enable=" & IIf(CheckBox2.Checked = True, 1, 0) & "," &
                        "nombre='" & TextBox6.Text.Trim & "'," &
                        "apellidos='" & TextBox7.Text.Trim & "'," &
                        "email='" & TextBox11.Text.Trim & "'," &
                        "telefono='" & TextBox8.Text.Trim & "'," &
                        "ipaddress='" & TextBox1.Text.Trim & "'," &
                        "namemachine='" & TextBox12.Text.Trim & "'," &
                        "usersystem='" & TextBox2.Text.Trim & "'," &
                        "loginuser='" & TextBox9.Text.Trim & "'," &
                        "loginpassw=md5('" & LoginPass & "' || '" & TextBox9.Text.Trim & "' || 'dvmap')," &
                        grantedFieldName & "=" & codPermisos & " " &
                        "WHERE iduser=" & GroupBox1.Tag.ToString
        Else
            cadUpdate = "UPDATE bdsidschema.usuarios SET " &
                        "enable=" & IIf(CheckBox2.Checked = True, 1, 0) & "," &
                        "nombre='" & TextBox6.Text.Trim & "'," &
                        "apellidos='" & TextBox7.Text.Trim & "'," &
                        "email='" & TextBox11.Text.Trim & "'," &
                        "telefono='" & TextBox8.Text.Trim & "'," &
                        "ipaddress='" & TextBox1.Text.Trim & "'," &
                        "namemachine='" & TextBox12.Text.Trim & "'," &
                        "usersystem='" & TextBox2.Text.Trim & "'," &
                        grantedFieldName & "=" & codPermisos & " " &
                        "WHERE iduser=" & GroupBox1.Tag.ToString
        End If

        Dim okProc As Boolean = ExeSinTran(cadUpdate)
        If okProc = True Then
            MessageBox.Show("Usuario modificado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            CargarUsuarios()
        Else
            MessageBox.Show("No se modificó ningún usuario", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click

        Dim cadDelete As String

        If MessageBox.Show("¿Desea borrar el usuario?",
                        AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) =
                        Windows.Forms.DialogResult.No Then Exit Sub

        cadDelete = "DELETE FROM bdsidschema.usuarios WHERE iduser=" & GroupBox1.Tag.ToString
        Dim okProc As Boolean = ExeSinTran(cadDelete)
        If okProc = True Then
            MessageBox.Show("Usuario eliminado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            CargarUsuarios()
            GroupBox1.Visible = False
        Else
            MessageBox.Show("No se eliminó ningún usuario", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
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

        rcdUsuarios = New DataTable
        If CargarRecordset("SELECT * from bdsidschema.usuarios ORDER BY iduser", rcdUsuarios) = True Then
            filas = rcdUsuarios.Select
            contador = -1
            For Each dR As DataRow In filas
                contador = contador + 1
                usuario = New myAppUser
                usuario.id = dR("iduser")
                usuario.nombre = dR("nombre").ToString
                usuario.apellidos = dR("apellidos").ToString
                usuario.loginUser = dR("loginuser").ToString
                usuario.telefono = dR("telefono").ToString
                usuario.codPermisoMultiple = dR(grantedFieldName)
                usuario.machineIP = dR("ipaddress").ToString
                usuario.machineSO = dR("usersystem").ToString
                usuario.correoElectronico = dR("email").ToString
                usuario.machineName = dR("namemachine").ToString
                usuario.enabled = dR("enable")
                ListaUsuarios.Add(usuario)
            Next
        End If
        rcdUsuarios.Dispose()
        rcdUsuarios = Nothing
        Erase filas
        RellenarLV()

    End Sub

    Private Sub cargarPermisosBasico()

        Dim elementoLV1 As ListViewItem
        Dim elementoLV2 As ListViewItem

        For Each permiso As String In ListaPermisosApp
            elementoLV1 = New ListViewItem
            elementoLV2 = New ListViewItem
            elementoLV1.Text = permiso
            elementoLV2.Text = permiso
            ListView1.Items.Add(elementoLV1)
            ListView2.Items.Add(elementoLV2)
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
            If usu.codPermisoMultiple > 0 Then
                elementoLV.SubItems.Add("Tiene permisos")
            Else
                elementoLV.SubItems.Add("No tiene permisos")
            End If
            elementoLV.Tag = contador
            If lvListaUser.Items.Count Mod 2 = 0 Then
                elementoLV.BackColor = Color.White
            Else
                elementoLV.BackColor = Color.WhiteSmoke
            End If
            If usu.enabled Then elementoLV.ForeColor = Color.Green
            If Not usu.enabled Then elementoLV.ForeColor = Color.Red
            lvListaUser.Items.Add(elementoLV)
            elementoLV = Nothing

        Next
        ToolStripStatusLabel1.Text = "Usuarios: " & lvListaUser.Items.Count

    End Sub

    Private Sub limpiarCampos()

        GroupBox1.Tag = "0"
        txtNewUserNombre.Text = ""
        txtNewUserSurname.Text = ""
        txtNewUserWinLogin.Text = ""
        txtNewUserPass.Text = ""
        txtNewUserTfno.Text = ""
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox6.Text = ""
        TextBox7.Text = ""
        TextBox8.Text = ""
        TextBox9.Text = ""
        TextBox10.Text = ""
        For Each elem As ListViewItem In ListView2.Items
            elem.Checked = False
        Next

    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged


    End Sub

    Private Sub CheckBox2_CheckStateChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckStateChanged
        If CheckBox2.Checked Then
            CheckBox2.BackColor = Color.Green
            CheckBox2.ForeColor = Color.Black
            CheckBox2.Text = "Usuario Activo: SI"
        Else
            CheckBox2.BackColor = Color.Red
            CheckBox2.ForeColor = Color.White
            CheckBox2.Text = "Usuario Activo: NO"
        End If
    End Sub


    Private Sub lvListaUser_ColumnClick(sender As Object, e As ColumnClickEventArgs) Handles lvListaUser.ColumnClick
        '
        ' ========================================
        ' Usando la clase ListViewColumnSortSimple
        ' ========================================
        '
        ' Crear una instancia de la clase que realizará la comparación
        Dim oCompare As New ListViewColumnSortSimple()

        ' Asignar el orden de clasificación
        If lvListaUser.Sorting = SortOrder.Ascending Then
            oCompare.Sorting = SortOrder.Descending
        Else
            oCompare.Sorting = SortOrder.Ascending
        End If
        lvListaUser.Sorting = oCompare.Sorting
        '
        ' La columna en la que se ha pulsado
        oCompare.ColumnIndex = e.Column
        ' Asignar la clase que implementa IComparer
        ' y que se usará para realizar la comparación de cada elemento
        lvListaUser.ListViewItemSorter = oCompare
        '
        ' Cuando se asigna ListViewItemSorter no es necesario llamar al método Sort
        'lvListaUser.Sort()
    End Sub
End Class
