<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DialogChangePassword
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DialogChangePassword))
        Me.txtOldPass = New System.Windows.Forms.TextBox()
        Me.txtNewPass = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnAccept = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.txtOldPassCheck = New System.Windows.Forms.CheckBox()
        Me.txtNewPassCheck = New System.Windows.Forms.CheckBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtOldPass
        '
        Me.txtOldPass.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOldPass.Location = New System.Drawing.Point(22, 76)
        Me.txtOldPass.Name = "txtOldPass"
        Me.txtOldPass.Size = New System.Drawing.Size(361, 23)
        Me.txtOldPass.TabIndex = 2
        '
        'txtNewPass
        '
        Me.txtNewPass.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNewPass.Location = New System.Drawing.Point(22, 143)
        Me.txtNewPass.Name = "txtNewPass"
        Me.txtNewPass.Size = New System.Drawing.Size(361, 23)
        Me.txtNewPass.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label1.Location = New System.Drawing.Point(19, 60)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(109, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Antigua contraseña"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label2.Location = New System.Drawing.Point(19, 127)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(101, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Nueva contraseña"
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Image = CType(resources.GetObject("btnCancel.Image"), System.Drawing.Image)
        Me.btnCancel.Location = New System.Drawing.Point(408, 208)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(90, 37)
        Me.btnCancel.TabIndex = 42
        Me.btnCancel.Text = "Cancelar"
        Me.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnAccept
        '
        Me.btnAccept.Image = CType(resources.GetObject("btnAccept.Image"), System.Drawing.Image)
        Me.btnAccept.Location = New System.Drawing.Point(504, 208)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(90, 37)
        Me.btnAccept.TabIndex = 41
        Me.btnAccept.Text = "Cambiar"
        Me.btnAccept.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnAccept.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(522, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(72, 68)
        Me.PictureBox1.TabIndex = 43
        Me.PictureBox1.TabStop = False
        '
        'txtOldPassCheck
        '
        Me.txtOldPassCheck.AutoSize = True
        Me.txtOldPassCheck.Location = New System.Drawing.Point(408, 79)
        Me.txtOldPassCheck.Name = "txtOldPassCheck"
        Me.txtOldPassCheck.Size = New System.Drawing.Size(61, 17)
        Me.txtOldPassCheck.TabIndex = 44
        Me.txtOldPassCheck.Text = "Mostrar"
        Me.txtOldPassCheck.UseVisualStyleBackColor = True
        '
        'txtNewPassCheck
        '
        Me.txtNewPassCheck.AutoSize = True
        Me.txtNewPassCheck.Location = New System.Drawing.Point(408, 146)
        Me.txtNewPassCheck.Name = "txtNewPassCheck"
        Me.txtNewPassCheck.Size = New System.Drawing.Size(61, 17)
        Me.txtNewPassCheck.TabIndex = 45
        Me.txtNewPassCheck.Text = "Mostrar"
        Me.txtNewPassCheck.UseVisualStyleBackColor = True
        '
        'DialogChangePassword
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(603, 256)
        Me.Controls.Add(Me.txtNewPassCheck)
        Me.Controls.Add(Me.txtOldPassCheck)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnAccept)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtNewPass)
        Me.Controls.Add(Me.txtOldPass)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DialogChangePassword"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Cambio de contraseña"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtOldPass As TextBox
    Friend WithEvents txtNewPass As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnAccept As Button
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents txtOldPassCheck As CheckBox
    Friend WithEvents txtNewPassCheck As CheckBox
End Class
