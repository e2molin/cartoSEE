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
        Me.txtDirTarget.Location = New System.Drawing.Point(15, 91)
        Me.txtDirTarget.Name = "txtDirTarget"
        Me.txtDirTarget.Size = New System.Drawing.Size(297, 20)
        Me.txtDirTarget.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 75)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(96, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Carpeta de destino"
        '
        'Button1
        '
        Me.Button1.Image = CType(resources.GetObject("Button1.Image"), System.Drawing.Image)
        Me.Button1.Location = New System.Drawing.Point(318, 75)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(87, 36)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Carpeta"
        Me.Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button1.UseVisualStyleBackColor = True
        '
        'btnProcess
        '
        Me.btnProcess.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnProcess.Image = CType(resources.GetObject("btnProcess.Image"), System.Drawing.Image)
        Me.btnProcess.Location = New System.Drawing.Point(540, 284)
        Me.btnProcess.Name = "btnProcess"
        Me.btnProcess.Size = New System.Drawing.Size(87, 36)
        Me.btnProcess.TabIndex = 5
        Me.btnProcess.Text = "Proceso"
        Me.btnProcess.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnProcess.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripProgressBar1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 335)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(641, 22)
        Me.StatusStrip1.TabIndex = 6
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(121, 17)
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
        Me.chkCreateINDEX.Location = New System.Drawing.Point(15, 156)
        Me.chkCreateINDEX.Name = "chkCreateINDEX"
        Me.chkCreateINDEX.Size = New System.Drawing.Size(178, 17)
        Me.chkCreateINDEX.TabIndex = 8
        Me.chkCreateINDEX.Text = "Generar sólo el índice provincial"
        Me.chkCreateINDEX.UseVisualStyleBackColor = True
        '
        'chkThumb
        '
        Me.chkThumb.AutoSize = True
        Me.chkThumb.Location = New System.Drawing.Point(15, 247)
        Me.chkThumb.Name = "chkThumb"
        Me.chkThumb.Size = New System.Drawing.Size(177, 17)
        Me.chkThumb.TabIndex = 10
        Me.chkThumb.Text = "Copiar los ficheros de miniaturas"
        Me.chkThumb.UseVisualStyleBackColor = True
        '
        'chkCreateNEM
        '
        Me.chkCreateNEM.AutoSize = True
        Me.chkCreateNEM.Location = New System.Drawing.Point(15, 202)
        Me.chkCreateNEM.Name = "chkCreateNEM"
        Me.chkCreateNEM.Size = New System.Drawing.Size(170, 17)
        Me.chkCreateNEM.TabIndex = 11
        Me.chkCreateNEM.Text = "Generar metadatos ISO 19115"
        Me.chkCreateNEM.UseVisualStyleBackColor = True
        '
        'chkHTML
        '
        Me.chkHTML.AutoSize = True
        Me.chkHTML.Location = New System.Drawing.Point(15, 133)
        Me.chkHTML.Name = "chkHTML"
        Me.chkHTML.Size = New System.Drawing.Size(273, 17)
        Me.chkHTML.TabIndex = 12
        Me.chkHTML.Text = "Generar HTML por documento con índice provincial"
        Me.chkHTML.UseVisualStyleBackColor = True
        '
        'chkCopiaFicherosImagen
        '
        Me.chkCopiaFicherosImagen.AutoSize = True
        Me.chkCopiaFicherosImagen.Location = New System.Drawing.Point(15, 225)
        Me.chkCopiaFicherosImagen.Name = "chkCopiaFicherosImagen"
        Me.chkCopiaFicherosImagen.Size = New System.Drawing.Size(207, 17)
        Me.chkCopiaFicherosImagen.TabIndex = 13
        Me.chkCopiaFicherosImagen.Text = "Copiar los ficheros JPG de la provincia"
        Me.chkCopiaFicherosImagen.UseVisualStyleBackColor = True
        '
        'chkMuniIndex
        '
        Me.chkMuniIndex.AutoSize = True
        Me.chkMuniIndex.Location = New System.Drawing.Point(15, 179)
        Me.chkMuniIndex.Name = "chkMuniIndex"
        Me.chkMuniIndex.Size = New System.Drawing.Size(277, 17)
        Me.chkMuniIndex.TabIndex = 14
        Me.chkMuniIndex.Text = "Generar índice para cada municipio histórico y actual"
        Me.chkMuniIndex.UseVisualStyleBackColor = True
        '
        'chkCopiaFicherosZIP
        '
        Me.chkCopiaFicherosZIP.AutoSize = True
        Me.chkCopiaFicherosZIP.Location = New System.Drawing.Point(15, 270)
        Me.chkCopiaFicherosZIP.Name = "chkCopiaFicherosZIP"
        Me.chkCopiaFicherosZIP.Size = New System.Drawing.Size(257, 17)
        Me.chkCopiaFicherosZIP.TabIndex = 15
        Me.chkCopiaFicherosZIP.Text = "Generar paquete ZIP de  los ficheros JPG+ECW "
        Me.chkCopiaFicherosZIP.UseVisualStyleBackColor = True
        '
        'CheckedListBox1
        '
        Me.CheckedListBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckedListBox1.FormattingEnabled = True
        Me.CheckedListBox1.Location = New System.Drawing.Point(324, 154)
        Me.CheckedListBox1.Name = "CheckedListBox1"
        Me.CheckedListBox1.Size = New System.Drawing.Size(303, 124)
        Me.CheckedListBox1.TabIndex = 16
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(321, 134)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(174, 13)
        Me.Label3.TabIndex = 17
        Me.Label3.Text = "Tipos de documentos para exportar"
        '
        'frmExportCdD
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(641, 357)
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
End Class
