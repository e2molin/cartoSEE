<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmExportCdD
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmExportCdD))
        Me.cboProvincias = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtDirTarget = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.btnProcess = New System.Windows.Forms.Button()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.chkCreateINDEX = New System.Windows.Forms.CheckBox()
        Me.chkThumb = New System.Windows.Forms.CheckBox()
        Me.chkCreateNEM = New System.Windows.Forms.CheckBox()
        Me.chkHTML = New System.Windows.Forms.CheckBox()
        Me.chkCopiaFicherosImagen = New System.Windows.Forms.CheckBox()
        Me.chkMuniIndex = New System.Windows.Forms.CheckBox()
        Me.chkCopiaFicherosZIP = New System.Windows.Forms.CheckBox()
        Me.CheckedListBox1 = New System.Windows.Forms.CheckedListBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cboxOverwrite = New System.Windows.Forms.CheckBox()
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.DateTimePicker2 = New System.Windows.Forms.DateTimePicker()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cboProvincias
        '
        Me.cboProvincias.FormattingEnabled = True
        Me.cboProvincias.Location = New System.Drawing.Point(15, 42)
        Me.cboProvincias.Name = "cboProvincias"
        Me.cboProvincias.Size = New System.Drawing.Size(204, 21)
        Me.cboProvincias.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 26)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(51, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Provincia"
        '
        'txtDirTarget
        '
        Me.txtDirTarget.Location = New System.Drawing.Point(15, 394)
        Me.txtDirTarget.Name = "txtDirTarget"
        Me.txtDirTarget.Size = New System.Drawing.Size(667, 20)
        Me.txtDirTarget.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 378)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(96, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Carpeta de destino"
        '
        'Button1
        '
        Me.Button1.Image = CType(resources.GetObject("Button1.Image"), System.Drawing.Image)
        Me.Button1.Location = New System.Drawing.Point(704, 378)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(87, 36)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Carpeta"
        Me.Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button1.UseVisualStyleBackColor = True
        '
        'btnProcess
        '
        Me.btnProcess.Image = CType(resources.GetObject("btnProcess.Image"), System.Drawing.Image)
        Me.btnProcess.Location = New System.Drawing.Point(617, 28)
        Me.btnProcess.Name = "btnProcess"
        Me.btnProcess.Size = New System.Drawing.Size(162, 36)
        Me.btnProcess.TabIndex = 5
        Me.btnProcess.Text = "Procesar provincia"
        Me.btnProcess.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnProcess.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripProgressBar1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 447)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(836, 22)
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
        Me.chkCreateINDEX.Location = New System.Drawing.Point(395, 214)
        Me.chkCreateINDEX.Name = "chkCreateINDEX"
        Me.chkCreateINDEX.Size = New System.Drawing.Size(178, 17)
        Me.chkCreateINDEX.TabIndex = 8
        Me.chkCreateINDEX.Text = "Generar sólo el índice provincial"
        Me.chkCreateINDEX.UseVisualStyleBackColor = True
        Me.chkCreateINDEX.Visible = False
        '
        'chkThumb
        '
        Me.chkThumb.AutoSize = True
        Me.chkThumb.Location = New System.Drawing.Point(395, 305)
        Me.chkThumb.Name = "chkThumb"
        Me.chkThumb.Size = New System.Drawing.Size(177, 17)
        Me.chkThumb.TabIndex = 10
        Me.chkThumb.Text = "Copiar los ficheros de miniaturas"
        Me.chkThumb.UseVisualStyleBackColor = True
        Me.chkThumb.Visible = False
        '
        'chkCreateNEM
        '
        Me.chkCreateNEM.AutoSize = True
        Me.chkCreateNEM.Location = New System.Drawing.Point(395, 260)
        Me.chkCreateNEM.Name = "chkCreateNEM"
        Me.chkCreateNEM.Size = New System.Drawing.Size(170, 17)
        Me.chkCreateNEM.TabIndex = 11
        Me.chkCreateNEM.Text = "Generar metadatos ISO 19115"
        Me.chkCreateNEM.UseVisualStyleBackColor = True
        Me.chkCreateNEM.Visible = False
        '
        'chkHTML
        '
        Me.chkHTML.AutoSize = True
        Me.chkHTML.Location = New System.Drawing.Point(395, 191)
        Me.chkHTML.Name = "chkHTML"
        Me.chkHTML.Size = New System.Drawing.Size(273, 17)
        Me.chkHTML.TabIndex = 12
        Me.chkHTML.Text = "Generar HTML por documento con índice provincial"
        Me.chkHTML.UseVisualStyleBackColor = True
        Me.chkHTML.Visible = False
        '
        'chkCopiaFicherosImagen
        '
        Me.chkCopiaFicherosImagen.AutoSize = True
        Me.chkCopiaFicherosImagen.Checked = True
        Me.chkCopiaFicherosImagen.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCopiaFicherosImagen.Location = New System.Drawing.Point(395, 283)
        Me.chkCopiaFicherosImagen.Name = "chkCopiaFicherosImagen"
        Me.chkCopiaFicherosImagen.Size = New System.Drawing.Size(207, 17)
        Me.chkCopiaFicherosImagen.TabIndex = 13
        Me.chkCopiaFicherosImagen.Text = "Copiar los ficheros JPG de la provincia"
        Me.chkCopiaFicherosImagen.UseVisualStyleBackColor = True
        Me.chkCopiaFicherosImagen.Visible = False
        '
        'chkMuniIndex
        '
        Me.chkMuniIndex.AutoSize = True
        Me.chkMuniIndex.Location = New System.Drawing.Point(395, 237)
        Me.chkMuniIndex.Name = "chkMuniIndex"
        Me.chkMuniIndex.Size = New System.Drawing.Size(277, 17)
        Me.chkMuniIndex.TabIndex = 14
        Me.chkMuniIndex.Text = "Generar índice para cada municipio histórico y actual"
        Me.chkMuniIndex.UseVisualStyleBackColor = True
        Me.chkMuniIndex.Visible = False
        '
        'chkCopiaFicherosZIP
        '
        Me.chkCopiaFicherosZIP.AutoSize = True
        Me.chkCopiaFicherosZIP.Location = New System.Drawing.Point(395, 328)
        Me.chkCopiaFicherosZIP.Name = "chkCopiaFicherosZIP"
        Me.chkCopiaFicherosZIP.Size = New System.Drawing.Size(257, 17)
        Me.chkCopiaFicherosZIP.TabIndex = 15
        Me.chkCopiaFicherosZIP.Text = "Generar paquete ZIP de  los ficheros JPG+ECW "
        Me.chkCopiaFicherosZIP.UseVisualStyleBackColor = True
        Me.chkCopiaFicherosZIP.Visible = False
        '
        'CheckedListBox1
        '
        Me.CheckedListBox1.FormattingEnabled = True
        Me.CheckedListBox1.Location = New System.Drawing.Point(15, 195)
        Me.CheckedListBox1.Name = "CheckedListBox1"
        Me.CheckedListBox1.Size = New System.Drawing.Size(356, 154)
        Me.CheckedListBox1.TabIndex = 16
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 175)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(174, 13)
        Me.Label3.TabIndex = 17
        Me.Label3.Text = "Tipos de documentos para exportar"
        '
        'cboxOverwrite
        '
        Me.cboxOverwrite.AutoSize = True
        Me.cboxOverwrite.Location = New System.Drawing.Point(244, 44)
        Me.cboxOverwrite.Name = "cboxOverwrite"
        Me.cboxOverwrite.Size = New System.Drawing.Size(228, 17)
        Me.cboxOverwrite.TabIndex = 18
        Me.cboxOverwrite.Text = "Sobrescribir si existen los ficheros de salida"
        Me.cboxOverwrite.UseVisualStyleBackColor = True
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Location = New System.Drawing.Point(171, 101)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(200, 20)
        Me.DateTimePicker1.TabIndex = 19
        Me.DateTimePicker1.Value = New Date(2022, 12, 5, 0, 0, 0, 0)
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 107)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(153, 13)
        Me.Label4.TabIndex = 20
        Me.Label4.Text = "Documentos modificados entre"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(377, 107)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(12, 13)
        Me.Label5.TabIndex = 21
        Me.Label5.Text = "y"
        '
        'DateTimePicker2
        '
        Me.DateTimePicker2.Location = New System.Drawing.Point(395, 100)
        Me.DateTimePicker2.Name = "DateTimePicker2"
        Me.DateTimePicker2.Size = New System.Drawing.Size(200, 20)
        Me.DateTimePicker2.TabIndex = 22
        '
        'Button2
        '
        Me.Button2.Image = CType(resources.GetObject("Button2.Image"), System.Drawing.Image)
        Me.Button2.Location = New System.Drawing.Point(617, 86)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(162, 36)
        Me.Button2.TabIndex = 23
        Me.Button2.Text = "Procesar periodo"
        Me.Button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button2.UseVisualStyleBackColor = True
        '
        'frmExportCdD
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(836, 469)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.DateTimePicker2)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.DateTimePicker1)
        Me.Controls.Add(Me.cboxOverwrite)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.CheckedListBox1)
        Me.Controls.Add(Me.chkCopiaFicherosZIP)
        Me.Controls.Add(Me.chkMuniIndex)
        Me.Controls.Add(Me.chkCopiaFicherosImagen)
        Me.Controls.Add(Me.chkHTML)
        Me.Controls.Add(Me.chkCreateNEM)
        Me.Controls.Add(Me.chkThumb)
        Me.Controls.Add(Me.chkCreateINDEX)
        Me.Controls.Add(Me.btnProcess)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtDirTarget)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cboProvincias)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmExportCdD"
        Me.Text = "Volcado Centro de descargas"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cboProvincias As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtDirTarget As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents btnProcess As System.Windows.Forms.Button
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripProgressBar1 As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents chkCreateINDEX As System.Windows.Forms.CheckBox
    Friend WithEvents chkThumb As System.Windows.Forms.CheckBox
    Friend WithEvents chkCreateNEM As System.Windows.Forms.CheckBox
    Friend WithEvents chkHTML As System.Windows.Forms.CheckBox
    Friend WithEvents chkCopiaFicherosImagen As System.Windows.Forms.CheckBox
    Friend WithEvents chkMuniIndex As System.Windows.Forms.CheckBox
    Friend WithEvents chkCopiaFicherosZIP As System.Windows.Forms.CheckBox
    Friend WithEvents CheckedListBox1 As System.Windows.Forms.CheckedListBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cboxOverwrite As CheckBox
    Friend WithEvents DateTimePicker1 As DateTimePicker
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents DateTimePicker2 As DateTimePicker
    Friend WithEvents Button2 As Button
End Class
