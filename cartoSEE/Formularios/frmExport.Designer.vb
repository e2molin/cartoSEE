<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmExport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmExport))
        Me.txtDirTarget = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.chkCreateINDEX = New System.Windows.Forms.CheckBox()
        Me.chkThumb = New System.Windows.Forms.CheckBox()
        Me.chkCreateNEM = New System.Windows.Forms.CheckBox()
        Me.chkHTML = New System.Windows.Forms.CheckBox()
        Me.chkCopiaFicheros = New System.Windows.Forms.CheckBox()
        Me.chkMuniIndex = New System.Windows.Forms.CheckBox()
        Me.btnSpecialProc = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.chkLinkDocGeo = New System.Windows.Forms.CheckBox()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.DateTimePicker4 = New System.Windows.Forms.DateTimePicker()
        Me.DateTimePicker3 = New System.Windows.Forms.DateTimePicker()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.ComboBox2 = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cboProvincias = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.btnProcessProvin = New System.Windows.Forms.Button()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.chkCopyTest = New System.Windows.Forms.CheckBox()
        Me.chkOverWrite = New System.Windows.Forms.CheckBox()
        Me.StatusStrip1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtDirTarget
        '
        Me.txtDirTarget.Location = New System.Drawing.Point(21, 521)
        Me.txtDirTarget.Name = "txtDirTarget"
        Me.txtDirTarget.Size = New System.Drawing.Size(495, 20)
        Me.txtDirTarget.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(18, 498)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(96, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Carpeta de destino"
        '
        'Button1
        '
        Me.Button1.Image = CType(resources.GetObject("Button1.Image"), System.Drawing.Image)
        Me.Button1.Location = New System.Drawing.Point(522, 506)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(187, 36)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Carpeta del volcado del CdD"
        Me.Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button1.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripProgressBar1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 557)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(874, 22)
        Me.StatusStrip1.TabIndex = 6
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(119, 17)
        Me.ToolStripStatusLabel1.Text = "ToolStripStatusLabel1"
        '
        'ToolStripProgressBar1
        '
        Me.ToolStripProgressBar1.Name = "ToolStripProgressBar1"
        Me.ToolStripProgressBar1.Size = New System.Drawing.Size(200, 16)
        '
        'chkCreateINDEX
        '
        Me.chkCreateINDEX.AutoSize = True
        Me.chkCreateINDEX.Location = New System.Drawing.Point(362, 461)
        Me.chkCreateINDEX.Name = "chkCreateINDEX"
        Me.chkCreateINDEX.Size = New System.Drawing.Size(211, 17)
        Me.chkCreateINDEX.TabIndex = 8
        Me.chkCreateINDEX.Text = "Generar sólo el índice provincial HTML"
        Me.chkCreateINDEX.UseVisualStyleBackColor = True
        '
        'chkThumb
        '
        Me.chkThumb.AutoSize = True
        Me.chkThumb.Location = New System.Drawing.Point(362, 369)
        Me.chkThumb.Name = "chkThumb"
        Me.chkThumb.Size = New System.Drawing.Size(114, 17)
        Me.chkThumb.TabIndex = 10
        Me.chkThumb.Text = "Generar miniaturas"
        Me.chkThumb.UseVisualStyleBackColor = True
        Me.chkThumb.UseWaitCursor = True
        '
        'chkCreateNEM
        '
        Me.chkCreateNEM.AutoSize = True
        Me.chkCreateNEM.Location = New System.Drawing.Point(362, 415)
        Me.chkCreateNEM.Name = "chkCreateNEM"
        Me.chkCreateNEM.Size = New System.Drawing.Size(170, 17)
        Me.chkCreateNEM.TabIndex = 11
        Me.chkCreateNEM.Text = "Generar metadatos ISO 19115"
        Me.chkCreateNEM.UseVisualStyleBackColor = True
        '
        'chkHTML
        '
        Me.chkHTML.AutoSize = True
        Me.chkHTML.Location = New System.Drawing.Point(362, 484)
        Me.chkHTML.Name = "chkHTML"
        Me.chkHTML.Size = New System.Drawing.Size(306, 17)
        Me.chkHTML.TabIndex = 12
        Me.chkHTML.Text = "Generar HTML por documento con índice provincial HTML"
        Me.chkHTML.UseVisualStyleBackColor = True
        '
        'chkCopiaFicheros
        '
        Me.chkCopiaFicheros.AutoSize = True
        Me.chkCopiaFicheros.Checked = True
        Me.chkCopiaFicheros.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCopiaFicheros.Location = New System.Drawing.Point(21, 392)
        Me.chkCopiaFicheros.Name = "chkCopiaFicheros"
        Me.chkCopiaFicheros.Size = New System.Drawing.Size(184, 17)
        Me.chkCopiaFicheros.TabIndex = 13
        Me.chkCopiaFicheros.Text = "Copiar los ficheros de la provincia"
        Me.chkCopiaFicheros.UseVisualStyleBackColor = True
        Me.chkCopiaFicheros.Visible = False
        '
        'chkMuniIndex
        '
        Me.chkMuniIndex.AutoSize = True
        Me.chkMuniIndex.Location = New System.Drawing.Point(362, 392)
        Me.chkMuniIndex.Name = "chkMuniIndex"
        Me.chkMuniIndex.Size = New System.Drawing.Size(228, 17)
        Me.chkMuniIndex.TabIndex = 14
        Me.chkMuniIndex.Text = "Generar índice HTML para cada municipio"
        Me.chkMuniIndex.UseVisualStyleBackColor = True
        '
        'btnSpecialProc
        '
        Me.btnSpecialProc.Image = CType(resources.GetObject("btnSpecialProc.Image"), System.Drawing.Image)
        Me.btnSpecialProc.Location = New System.Drawing.Point(491, 268)
        Me.btnSpecialProc.Name = "btnSpecialProc"
        Me.btnSpecialProc.Size = New System.Drawing.Size(255, 36)
        Me.btnSpecialProc.TabIndex = 15
        Me.btnSpecialProc.Text = "Aplicar Filtro a cuadernos interiores"
        Me.btnSpecialProc.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnSpecialProc.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(6, 23)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(740, 239)
        Me.TextBox1.TabIndex = 16
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(6, 7)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 13)
        Me.Label3.TabIndex = 17
        Me.Label3.Text = "Filtro SQL "
        '
        'chkLinkDocGeo
        '
        Me.chkLinkDocGeo.AutoSize = True
        Me.chkLinkDocGeo.Location = New System.Drawing.Point(362, 438)
        Me.chkLinkDocGeo.Name = "chkLinkDocGeo"
        Me.chkLinkDocGeo.Size = New System.Drawing.Size(150, 17)
        Me.chkLinkDocGeo.TabIndex = 18
        Me.chkLinkDocGeo.Text = "Generar enlace con BDLL"
        Me.chkLinkDocGeo.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Location = New System.Drawing.Point(15, 11)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(816, 352)
        Me.TabControl1.TabIndex = 19
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Label11)
        Me.TabPage1.Controls.Add(Me.Button4)
        Me.TabPage1.Controls.Add(Me.Label9)
        Me.TabPage1.Controls.Add(Me.Label13)
        Me.TabPage1.Controls.Add(Me.Label12)
        Me.TabPage1.Controls.Add(Me.DateTimePicker4)
        Me.TabPage1.Controls.Add(Me.DateTimePicker3)
        Me.TabPage1.Controls.Add(Me.Button6)
        Me.TabPage1.Controls.Add(Me.Label10)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(808, 326)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Cuadernos interiores"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(238, 180)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(45, 13)
        Me.Label11.TabIndex = 41
        Me.Label11.Text = "Label11"
        '
        'Button4
        '
        Me.Button4.Image = CType(resources.GetObject("Button4.Image"), System.Drawing.Image)
        Me.Button4.Location = New System.Drawing.Point(22, 168)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(202, 36)
        Me.Button4.TabIndex = 40
        Me.Button4.Text = "Extraer modificados desde la última actualziación"
        Me.Button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(238, 51)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(261, 13)
        Me.Label9.TabIndex = 39
        Me.Label9.Text = "Documentos de este tipo que aún no están en el CdD"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(434, 148)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(38, 13)
        Me.Label13.TabIndex = 38
        Me.Label13.Text = "hasta"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(19, 148)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(186, 13)
        Me.Label12.TabIndex = 37
        Me.Label12.Text = "Documentos modificados desde"
        '
        'DateTimePicker4
        '
        Me.DateTimePicker4.Location = New System.Drawing.Point(488, 141)
        Me.DateTimePicker4.Name = "DateTimePicker4"
        Me.DateTimePicker4.Size = New System.Drawing.Size(200, 20)
        Me.DateTimePicker4.TabIndex = 35
        '
        'DateTimePicker3
        '
        Me.DateTimePicker3.Location = New System.Drawing.Point(216, 142)
        Me.DateTimePicker3.Name = "DateTimePicker3"
        Me.DateTimePicker3.Size = New System.Drawing.Size(200, 20)
        Me.DateTimePicker3.TabIndex = 34
        '
        'Button6
        '
        Me.Button6.Image = CType(resources.GetObject("Button6.Image"), System.Drawing.Image)
        Me.Button6.Location = New System.Drawing.Point(22, 51)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(202, 36)
        Me.Button6.TabIndex = 33
        Me.Button6.Text = "Extraer documentos nuevos"
        Me.Button6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button6.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(19, 26)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(258, 13)
        Me.Label10.TabIndex = 32
        Me.Label10.Text = "Cuadernos interiores - Itinerarios con brújula"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.TextBox1)
        Me.TabPage2.Controls.Add(Me.Label3)
        Me.TabPage2.Controls.Add(Me.btnSpecialProc)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(808, 326)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Por filtro SQL y Temporal"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.ComboBox2)
        Me.TabPage3.Controls.Add(Me.Label8)
        Me.TabPage3.Controls.Add(Me.ComboBox1)
        Me.TabPage3.Controls.Add(Me.Label7)
        Me.TabPage3.Controls.Add(Me.cboProvincias)
        Me.TabPage3.Controls.Add(Me.Label1)
        Me.TabPage3.Controls.Add(Me.Button3)
        Me.TabPage3.Controls.Add(Me.btnProcessProvin)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(808, 326)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Otra documentación"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'ComboBox2
        '
        Me.ComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.Location = New System.Drawing.Point(36, 140)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(385, 21)
        Me.ComboBox2.TabIndex = 36
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(39, 124)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(164, 13)
        Me.Label8.TabIndex = 35
        Me.Label8.Text = "Filtro subtipo de documento"
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(36, 88)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(385, 21)
        Me.ComboBox1.TabIndex = 34
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(33, 72)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(144, 13)
        Me.Label7.TabIndex = 33
        Me.Label7.Text = "Filtro tipo de documento"
        '
        'cboProvincias
        '
        Me.cboProvincias.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboProvincias.FormattingEnabled = True
        Me.cboProvincias.Location = New System.Drawing.Point(36, 45)
        Me.cboProvincias.Name = "cboProvincias"
        Me.cboProvincias.Size = New System.Drawing.Size(385, 21)
        Me.cboProvincias.TabIndex = 31
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(33, 29)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 13)
        Me.Label1.TabIndex = 32
        Me.Label1.Text = "Provincia"
        '
        'Button3
        '
        Me.Button3.Enabled = False
        Me.Button3.Image = CType(resources.GetObject("Button3.Image"), System.Drawing.Image)
        Me.Button3.Location = New System.Drawing.Point(571, 59)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(138, 36)
        Me.Button3.TabIndex = 29
        Me.Button3.Text = "Por tipo y provincia"
        Me.Button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button3.UseVisualStyleBackColor = True
        '
        'btnProcessProvin
        '
        Me.btnProcessProvin.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnProcessProvin.Enabled = False
        Me.btnProcessProvin.Image = CType(resources.GetObject("btnProcessProvin.Image"), System.Drawing.Image)
        Me.btnProcessProvin.Location = New System.Drawing.Point(571, 125)
        Me.btnProcessProvin.Name = "btnProcessProvin"
        Me.btnProcessProvin.Size = New System.Drawing.Size(138, 36)
        Me.btnProcessProvin.TabIndex = 6
        Me.btnProcessProvin.Text = "Provincia completa"
        Me.btnProcessProvin.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnProcessProvin.UseVisualStyleBackColor = True
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(21, 415)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(191, 17)
        Me.CheckBox1.TabIndex = 20
        Me.CheckBox1.Text = "Agrupar ficheros PDF por provincia"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'chkCopyTest
        '
        Me.chkCopyTest.AutoSize = True
        Me.chkCopyTest.Location = New System.Drawing.Point(21, 438)
        Me.chkCopyTest.Name = "chkCopyTest"
        Me.chkCopyTest.Size = New System.Drawing.Size(121, 17)
        Me.chkCopyTest.TabIndex = 21
        Me.chkCopyTest.Text = "Simulación de copia"
        Me.chkCopyTest.UseVisualStyleBackColor = True
        '
        'chkOverWrite
        '
        Me.chkOverWrite.AutoSize = True
        Me.chkOverWrite.Location = New System.Drawing.Point(21, 461)
        Me.chkOverWrite.Name = "chkOverWrite"
        Me.chkOverWrite.Size = New System.Drawing.Size(164, 17)
        Me.chkOverWrite.TabIndex = 22
        Me.chkOverWrite.Text = "Sobreescribir ficheros previos"
        Me.chkOverWrite.UseVisualStyleBackColor = True
        '
        'frmExport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(874, 579)
        Me.Controls.Add(Me.chkOverWrite)
        Me.Controls.Add(Me.chkCopyTest)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.chkLinkDocGeo)
        Me.Controls.Add(Me.chkMuniIndex)
        Me.Controls.Add(Me.chkCopiaFicheros)
        Me.Controls.Add(Me.chkHTML)
        Me.Controls.Add(Me.chkCreateNEM)
        Me.Controls.Add(Me.chkThumb)
        Me.Controls.Add(Me.chkCreateINDEX)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtDirTarget)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmExport"
        Me.Text = "Volcado Centro de descargas"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtDirTarget As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripProgressBar1 As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents chkCreateINDEX As System.Windows.Forms.CheckBox
    Friend WithEvents chkThumb As System.Windows.Forms.CheckBox
    Friend WithEvents chkCreateNEM As System.Windows.Forms.CheckBox
    Friend WithEvents chkHTML As System.Windows.Forms.CheckBox
    Friend WithEvents chkCopiaFicheros As System.Windows.Forms.CheckBox
    Friend WithEvents chkMuniIndex As System.Windows.Forms.CheckBox
    Friend WithEvents btnSpecialProc As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents chkLinkDocGeo As System.Windows.Forms.CheckBox
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents ComboBox2 As ComboBox
    Friend WithEvents Label8 As Label
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents Label7 As Label
    Friend WithEvents cboProvincias As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Button3 As Button
    Friend WithEvents btnProcessProvin As Button
    Friend WithEvents Button6 As Button
    Friend WithEvents Label10 As Label
    Friend WithEvents chkCopyTest As CheckBox
    Friend WithEvents Label13 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents DateTimePicker4 As DateTimePicker
    Friend WithEvents DateTimePicker3 As DateTimePicker
    Friend WithEvents chkOverWrite As CheckBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Button4 As Button
End Class
