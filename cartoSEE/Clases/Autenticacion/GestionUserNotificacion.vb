Public Class GestionUserNotificacion
    Inherits System.Windows.Forms.Form

    Dim ListaIncidencias As ArrayList
    Const incidenciasAppName As String = "CARTOSEE"


#Region "Windows Controls Definition"
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton3 As System.Windows.Forms.RadioButton
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(GestionUserNotificacion))
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.RadioButton3 = New System.Windows.Forms.RadioButton()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(9, 10)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(487, 258)
        Me.TabControl1.TabIndex = 3
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Button1)
        Me.TabPage1.Controls.Add(Me.TextBox2)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Controls.Add(Me.TextBox1)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(479, 232)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Nueva incidencia"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Image = CType(resources.GetObject("Button1.Image"), System.Drawing.Image)
        Me.Button1.Location = New System.Drawing.Point(368, 177)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(105, 39)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Notificar"
        Me.Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TextBox2
        '
        Me.TextBox2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox2.Location = New System.Drawing.Point(39, 107)
        Me.TextBox2.Multiline = True
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(323, 109)
        Me.TextBox2.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(39, 91)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Descripción"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(39, 48)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(177, 20)
        Me.TextBox1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(39, 29)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(125, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Documento / Referencia"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.Button3)
        Me.TabPage2.Controls.Add(Me.Button2)
        Me.TabPage2.Controls.Add(Me.GroupBox1)
        Me.TabPage2.Controls.Add(Me.RadioButton3)
        Me.TabPage2.Controls.Add(Me.RadioButton2)
        Me.TabPage2.Controls.Add(Me.RadioButton1)
        Me.TabPage2.Controls.Add(Me.ListView1)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(479, 232)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Incidencias"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button3.Image = CType(resources.GetObject("Button3.Image"), System.Drawing.Image)
        Me.Button3.Location = New System.Drawing.Point(428, 177)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(45, 36)
        Me.Button3.TabIndex = 6
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button2.Image = CType(resources.GetObject("Button2.Image"), System.Drawing.Image)
        Me.Button2.Location = New System.Drawing.Point(382, 177)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(45, 36)
        Me.Button2.TabIndex = 5
        Me.Button2.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.PictureBox1)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 22)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(360, 191)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "GroupBox1"
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(328, 14)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(26, 27)
        Me.PictureBox1.TabIndex = 4
        Me.PictureBox1.TabStop = False
        '
        'Label5
        '
        Me.Label5.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.BackColor = System.Drawing.Color.Silver
        Me.Label5.Location = New System.Drawing.Point(23, 80)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(331, 98)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Label5"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(23, 49)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(39, 13)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Label4"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(23, 28)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(39, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Label3"
        '
        'RadioButton3
        '
        Me.RadioButton3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RadioButton3.AutoSize = True
        Me.RadioButton3.Location = New System.Drawing.Point(382, 69)
        Me.RadioButton3.Name = "RadioButton3"
        Me.RadioButton3.Size = New System.Drawing.Size(89, 17)
        Me.RadioButton3.TabIndex = 3
        Me.RadioButton3.TabStop = True
        Me.RadioButton3.Text = "Solucionadas"
        Me.RadioButton3.UseVisualStyleBackColor = True
        '
        'RadioButton2
        '
        Me.RadioButton2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(382, 46)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(78, 17)
        Me.RadioButton2.TabIndex = 2
        Me.RadioButton2.TabStop = True
        Me.RadioButton2.Text = "Pendientes"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'RadioButton1
        '
        Me.RadioButton1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Checked = True
        Me.RadioButton1.Location = New System.Drawing.Point(382, 23)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(55, 17)
        Me.RadioButton1.TabIndex = 1
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "Todas"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'ListView1
        '
        Me.ListView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView1.HideSelection = False
        Me.ListView1.Location = New System.Drawing.Point(16, 22)
        Me.ListView1.MultiSelect = False
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(360, 191)
        Me.ListView1.TabIndex = 0
        Me.ListView1.UseCompatibleStateImageBehavior = False
        '
        'GestionUserNotificacion
        '
        Me.ClientSize = New System.Drawing.Size(505, 278)
        Me.Controls.Add(Me.TabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "GestionUserNotificacion"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region


    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub frmIncidencias_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        ListView1.Location = New Point(16, 22)
        ListView1.Size = New Point(360, 190)
        GroupBox1.Location = New Point(16, 22)
        GroupBox1.Size = New Point(360, 190)
        GroupBox1.Visible = False

        ListView1.FullRowSelect = True
        ListView1.View = View.Details
        ListView1.Columns.Add("Referencia", 90, HorizontalAlignment.Left)
        ListView1.Columns.Add("Fecha", 70, HorizontalAlignment.Left)
        ListView1.Columns.Add("Usuario", 100, HorizontalAlignment.Left)
        ListView1.Columns.Add("Estado", 60, HorizontalAlignment.Left)
        If usuarioMyApp.permisosLista.isUserISTARI = False Then
            Button2.Enabled = False
            Button3.Enabled = False
        End If

        CargarIncidencias()

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim cadInsert As String

        If TextBox2.Text.Trim = "" Then Exit Sub
        If TextBox1.Text.Trim = "" Then
            If MessageBox.Show("No ha indicado ninguna referencia o documento asociado.¿Continuar?", _
                            AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then Exit Sub
        End If

        cadInsert = "INSERT INTO bdsidschema.incidencias (idincidencia,documento,usuario,estado,incidencia,aplicacion) " &
                    "VALUES (" &
                    "nextval('bdsidschema.incidencias_idincidencia_seq')," &
                    "'" & TextBox1.Text.Trim & "'," &
                    "'" & usuarioMyApp.loginUser & "'," &
                    "'Abierta'," &
                    "'" & TextBox2.Text.Replace("'", "\'").Trim & "','" & incidenciasAppName & "')"
        Dim okProc As Boolean = ExeSinTran(cadInsert)
        If okProc = True Then
            MessageBox.Show("Incidencia notificada", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            CargarIncidencias()
        Else
            MessageBox.Show("No se creo ninguna incidencia", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub



    Private Sub CargarIncidencias()

        Dim cadSQL As String
        Dim rcdIncidencia As DataTable
        Dim filas() As DataRow
        Dim notif As Notificacion

        ListaIncidencias = New ArrayList

        cadSQL = "SELECT * from bdsidschema.incidencias where aplicacion='" & incidenciasAppName & "' ORDER BY idincidencia"
        rcdIncidencia = New DataTable
        If CargarRecordset(cadSQL, rcdIncidencia) = True Then
            filas = rcdIncidencia.Select
            For Each dR As DataRow In filas
                notif = New Notificacion
                notif.id = dR("idincidencia").ToString
                notif.referencia = dR("documento").ToString
                notif.usuario = dR("usuario").ToString
                notif.estado = dR("estado").ToString
                notif.fecha = dR("fechacreacion")
                notif.descripcion = dR("incidencia")
                ListaIncidencias.Add(notif)
                notif = Nothing
            Next
        End If
        rcdIncidencia.Dispose()
        rcdIncidencia = Nothing
        Erase filas
        RellenarLV()

    End Sub





    Private Sub ToggleMuestra(ByVal sender As Object, ByVal e As System.EventArgs) _
            Handles RadioButton1.Click, RadioButton2.Click, RadioButton3.Click
        RellenarLV()
    End Sub


    Private Sub ListView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick
        GroupBox1.Visible = True
        GroupBox1.Text = ListaIncidencias(ListView1.SelectedItems(0).Tag).referencia
        Label3.Text = ListaIncidencias(ListView1.SelectedItems(0).Tag).usuario
        Label4.Text = ListaIncidencias(ListView1.SelectedItems(0).Tag).fecha
        Label5.Text = ListaIncidencias(ListView1.SelectedItems(0).Tag).descripcion
        If ListaIncidencias(ListView1.SelectedItems(0).Tag).estado = "Cerrada" Then
            Label3.ForeColor = Color.DarkGreen
            Label4.ForeColor = Color.DarkGreen
        ElseIf ListaIncidencias(ListView1.SelectedItems(0).Tag).estado = "Abierta" Then
            Label3.ForeColor = Color.Red
            Label4.ForeColor = Color.Red
        End If
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        GroupBox1.Visible = False
    End Sub


    Private Sub RellenarLV()
        Dim elementoLV As ListViewItem
        Dim contador As Integer
        Dim mostrar As Boolean
        If ListaIncidencias Is Nothing Then Exit Sub
        ListView1.Items.Clear()

        contador = -1
        For Each incidencia As Notificacion In ListaIncidencias
            contador = contador + 1
            mostrar = False
            If RadioButton1.Checked = True Then
                mostrar = True
            ElseIf RadioButton2.Checked = True Then
                If incidencia.estado = "Abierta" Then mostrar = True
            ElseIf RadioButton3.Checked = True Then
                If incidencia.estado = "Cerrada" Then mostrar = True
            End If
            If mostrar = True Then
                elementoLV = New ListViewItem
                elementoLV.Text = incidencia.referencia
                elementoLV.SubItems.Add(incidencia.fecha.ToString("d"))
                elementoLV.SubItems.Add(incidencia.usuario)
                elementoLV.SubItems.Add(incidencia.estado)
                elementoLV.Tag = contador
                If ListView1.Items.Count Mod 2 = 0 Then
                    elementoLV.BackColor = Color.White
                Else
                    elementoLV.BackColor = Color.WhiteSmoke
                End If
                ListView1.Items.Add(elementoLV)
                elementoLV = Nothing
            End If

        Next

    End Sub

    Private Sub CambiarEstado(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                    Handles Button2.Click, Button3.Click

        Dim okProc As Boolean
        Me.Cursor = Cursors.WaitCursor
        GroupBox1.Visible = False
        If sender.name = "Button2" Then
            okProc = ExeSinTran("UPDATE bdsidschema.incidencias SET estado='Abierta' WHERE idincidencia=" & ListaIncidencias(ListView1.SelectedItems(0).Tag).id)
        ElseIf sender.name = "Button3" Then
            okProc = ExeSinTran("UPDATE bdsidschema.incidencias SET estado='Cerrada' WHERE idincidencia=" & ListaIncidencias(ListView1.SelectedItems(0).Tag).id)
        End If
        If okProc = True Then CargarIncidencias()
        Me.Cursor = Cursors.Default

    End Sub




End Class
