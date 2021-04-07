<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDevelIGN
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDevelIGN))
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.procTranslate = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.txtEPSGout = New System.Windows.Forms.TextBox()
        Me.txtEPSGin = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtDeltaY = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtDeltaX = New System.Windows.Forms.TextBox()
        Me.lblInfoNumFiles = New System.Windows.Forms.Label()
        Me.btnSelectFolder = New System.Windows.Forms.Button()
        Me.txtPathGCP = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.procReproy = New System.Windows.Forms.Button()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.cboTipoTextExist = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.btnTextExist = New System.Windows.Forms.Button()
        Me.ListBox2 = New System.Windows.Forms.ListBox()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Location = New System.Drawing.Point(12, 12)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(779, 420)
        Me.TabControl1.TabIndex = 5
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Label7)
        Me.TabPage1.Controls.Add(Me.procTranslate)
        Me.TabPage1.Controls.Add(Me.Label6)
        Me.TabPage1.Controls.Add(Me.GroupBox2)
        Me.TabPage1.Controls.Add(Me.GroupBox1)
        Me.TabPage1.Controls.Add(Me.lblInfoNumFiles)
        Me.TabPage1.Controls.Add(Me.btnSelectFolder)
        Me.TabPage1.Controls.Add(Me.txtPathGCP)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Controls.Add(Me.ListBox1)
        Me.TabPage1.Controls.Add(Me.procReproy)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(771, 394)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Reproyectar GCPs"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.ForeColor = System.Drawing.Color.RoyalBlue
        Me.Label7.Location = New System.Drawing.Point(15, 30)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(443, 13)
        Me.Label7.TabIndex = 17
        Me.Label7.Text = "Dado un directorio con GCPs , aplica un parámetro de desplazamiento en  cada coor" & _
    "denada"
        '
        'procTranslate
        '
        Me.procTranslate.Image = CType(resources.GetObject("procTranslate.Image"), System.Drawing.Image)
        Me.procTranslate.Location = New System.Drawing.Point(642, 320)
        Me.procTranslate.Name = "procTranslate"
        Me.procTranslate.Size = New System.Drawing.Size(109, 41)
        Me.procTranslate.TabIndex = 16
        Me.procTranslate.Text = "Procesar"
        Me.procTranslate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.procTranslate.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ForeColor = System.Drawing.Color.RoyalBlue
        Me.Label6.Location = New System.Drawing.Point(15, 12)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(540, 13)
        Me.Label6.TabIndex = 15
        Me.Label6.Text = "Dado un directorio con GCPs en una proyección dada, los procesa reproyectándolos " & _
    "a otro sistema de referencia"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtEPSGout)
        Me.GroupBox2.Controls.Add(Me.txtEPSGin)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Location = New System.Drawing.Point(436, 167)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(200, 88)
        Me.GroupBox2.TabIndex = 13
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Sistemas de referencia"
        '
        'txtEPSGout
        '
        Me.txtEPSGout.Location = New System.Drawing.Point(114, 54)
        Me.txtEPSGout.Name = "txtEPSGout"
        Me.txtEPSGout.Size = New System.Drawing.Size(58, 20)
        Me.txtEPSGout.TabIndex = 10
        Me.txtEPSGout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtEPSGin
        '
        Me.txtEPSGin.Location = New System.Drawing.Point(114, 28)
        Me.txtEPSGin.Name = "txtEPSGin"
        Me.txtEPSGin.Size = New System.Drawing.Size(58, 20)
        Me.txtEPSGin.TabIndex = 8
        Me.txtEPSGin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(35, 57)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(73, 13)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "EPSG destino"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(40, 31)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(68, 13)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "EPSG origen"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.txtDeltaY)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.txtDeltaX)
        Me.GroupBox1.Location = New System.Drawing.Point(436, 261)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(200, 100)
        Me.GroupBox1.TabIndex = 12
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Aplicar desplazamiento"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(31, 62)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(42, 13)
        Me.Label4.TabIndex = 15
        Me.Label4.Text = "Delta Y"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtDeltaY
        '
        Me.txtDeltaY.Location = New System.Drawing.Point(79, 59)
        Me.txtDeltaY.Name = "txtDeltaY"
        Me.txtDeltaY.Size = New System.Drawing.Size(93, 20)
        Me.txtDeltaY.TabIndex = 14
        Me.txtDeltaY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(31, 36)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(42, 13)
        Me.Label5.TabIndex = 13
        Me.Label5.Text = "Delta X"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtDeltaX
        '
        Me.txtDeltaX.Location = New System.Drawing.Point(79, 33)
        Me.txtDeltaX.Name = "txtDeltaX"
        Me.txtDeltaX.Size = New System.Drawing.Size(93, 20)
        Me.txtDeltaX.TabIndex = 12
        Me.txtDeltaX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblInfoNumFiles
        '
        Me.lblInfoNumFiles.AutoSize = True
        Me.lblInfoNumFiles.Location = New System.Drawing.Point(512, 94)
        Me.lblInfoNumFiles.Name = "lblInfoNumFiles"
        Me.lblInfoNumFiles.Size = New System.Drawing.Size(78, 13)
        Me.lblInfoNumFiles.TabIndex = 7
        Me.lblInfoNumFiles.Text = "lblInfoNumFiles"
        Me.lblInfoNumFiles.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnSelectFolder
        '
        Me.btnSelectFolder.Image = CType(resources.GetObject("btnSelectFolder.Image"), System.Drawing.Image)
        Me.btnSelectFolder.Location = New System.Drawing.Point(671, 55)
        Me.btnSelectFolder.Name = "btnSelectFolder"
        Me.btnSelectFolder.Size = New System.Drawing.Size(80, 36)
        Me.btnSelectFolder.TabIndex = 6
        Me.btnSelectFolder.Text = "Abrir"
        Me.btnSelectFolder.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnSelectFolder.UseVisualStyleBackColor = True
        '
        'txtPathGCP
        '
        Me.txtPathGCP.Location = New System.Drawing.Point(18, 71)
        Me.txtPathGCP.Name = "txtPathGCP"
        Me.txtPathGCP.Size = New System.Drawing.Size(647, 20)
        Me.txtPathGCP.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 55)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(208, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Directorio con los ficheros GCP de entrada"
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(18, 123)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(412, 238)
        Me.ListBox1.TabIndex = 3
        '
        'procReproy
        '
        Me.procReproy.Image = CType(resources.GetObject("procReproy.Image"), System.Drawing.Image)
        Me.procReproy.Location = New System.Drawing.Point(642, 214)
        Me.procReproy.Name = "procReproy"
        Me.procReproy.Size = New System.Drawing.Size(109, 41)
        Me.procReproy.TabIndex = 2
        Me.procReproy.Text = "Procesar"
        Me.procReproy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.procReproy.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.Button1)
        Me.TabPage2.Controls.Add(Me.cboTipoTextExist)
        Me.TabPage2.Controls.Add(Me.Label8)
        Me.TabPage2.Controls.Add(Me.btnTextExist)
        Me.TabPage2.Controls.Add(Me.ListBox2)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(771, 394)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Comprobación de ficheros en el repositorio"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Image = CType(resources.GetObject("Button1.Image"), System.Drawing.Image)
        Me.Button1.Location = New System.Drawing.Point(616, 351)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(122, 37)
        Me.Button1.TabIndex = 8
        Me.Button1.Text = "Guarda informe"
        Me.Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button1.UseVisualStyleBackColor = True
        '
        'cboTipoTextExist
        '
        Me.cboTipoTextExist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTipoTextExist.FormattingEnabled = True
        Me.cboTipoTextExist.Location = New System.Drawing.Point(21, 35)
        Me.cboTipoTextExist.Name = "cboTipoTextExist"
        Me.cboTipoTextExist.Size = New System.Drawing.Size(276, 21)
        Me.cboTipoTextExist.TabIndex = 7
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(18, 19)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(206, 13)
        Me.Label8.TabIndex = 6
        Me.Label8.Text = "Comprobación de la existencia de ficheros"
        '
        'btnTextExist
        '
        Me.btnTextExist.Image = CType(resources.GetObject("btnTextExist.Image"), System.Drawing.Image)
        Me.btnTextExist.Location = New System.Drawing.Point(322, 19)
        Me.btnTextExist.Name = "btnTextExist"
        Me.btnTextExist.Size = New System.Drawing.Size(106, 37)
        Me.btnTextExist.TabIndex = 5
        Me.btnTextExist.Text = "Iniciar test"
        Me.btnTextExist.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnTextExist.UseVisualStyleBackColor = True
        '
        'ListBox2
        '
        Me.ListBox2.FormattingEnabled = True
        Me.ListBox2.Location = New System.Drawing.Point(21, 93)
        Me.ListBox2.Name = "ListBox2"
        Me.ListBox2.Size = New System.Drawing.Size(717, 238)
        Me.ListBox2.TabIndex = 3
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.Label11)
        Me.TabPage3.Controls.Add(Me.Button4)
        Me.TabPage3.Controls.Add(Me.Button3)
        Me.TabPage3.Controls.Add(Me.Button2)
        Me.TabPage3.Controls.Add(Me.TextBox1)
        Me.TabPage3.Controls.Add(Me.Label10)
        Me.TabPage3.Controls.Add(Me.Label9)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(771, 394)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Otros procedimientos"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Image = CType(resources.GetObject("Button3.Image"), System.Drawing.Image)
        Me.Button3.Location = New System.Drawing.Point(724, 44)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(44, 36)
        Me.Button3.TabIndex = 20
        Me.Button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Image = CType(resources.GetObject("Button2.Image"), System.Drawing.Image)
        Me.Button2.Location = New System.Drawing.Point(679, 44)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(44, 36)
        Me.Button2.TabIndex = 19
        Me.Button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button2.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(26, 60)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(647, 20)
        Me.TextBox1.TabIndex = 18
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(23, 44)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(208, 13)
        Me.Label10.TabIndex = 17
        Me.Label10.Text = "Directorio con los ficheros GCP de entrada"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.ForeColor = System.Drawing.Color.RoyalBlue
        Me.Label9.Location = New System.Drawing.Point(23, 19)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(574, 13)
        Me.Label9.TabIndex = 16
        Me.Label9.Text = "Dado un directorio con GCPs los fusiona en un único ASCII file etiquetando cada e" & _
    "ntidad con el nombre del fichero GCP"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 444)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(802, 22)
        Me.StatusStrip1.TabIndex = 6
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(121, 17)
        Me.ToolStripStatusLabel1.Text = "ToolStripStatusLabel1"
        '
        'SaveFileDialog1
        '
        '
        'Button4
        '
        Me.Button4.Image = CType(resources.GetObject("Button4.Image"), System.Drawing.Image)
        Me.Button4.Location = New System.Drawing.Point(26, 114)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(44, 36)
        Me.Button4.TabIndex = 21
        Me.Button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.ForeColor = System.Drawing.Color.RoyalBlue
        Me.Label11.Location = New System.Drawing.Point(23, 98)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(249, 13)
        Me.Label11.TabIndex = 22
        Me.Label11.Text = "Generar datos para la plantilla del WMS de Minutas"
        '
        'frmDevelIGN
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(802, 466)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.TabControl1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmDevelIGN"
        Me.Text = "Herramientas administración"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents txtEPSGout As System.Windows.Forms.TextBox
    Friend WithEvents txtEPSGin As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtDeltaY As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtDeltaX As System.Windows.Forms.TextBox
    Friend WithEvents lblInfoNumFiles As System.Windows.Forms.Label
    Friend WithEvents btnSelectFolder As System.Windows.Forms.Button
    Friend WithEvents txtPathGCP As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents procReproy As System.Windows.Forms.Button
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents btnTextExist As System.Windows.Forms.Button
    Friend WithEvents ListBox2 As System.Windows.Forms.ListBox
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents procTranslate As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents cboTipoTextExist As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Button4 As System.Windows.Forms.Button
End Class
