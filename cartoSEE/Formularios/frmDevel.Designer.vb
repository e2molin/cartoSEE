<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDevel
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDevel))
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.Button7 = New System.Windows.Forms.Button()
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
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.Button25 = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.ListBox3 = New System.Windows.Forms.ListBox()
        Me.Button24 = New System.Windows.Forms.Button()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Button23 = New System.Windows.Forms.Button()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Button22 = New System.Windows.Forms.Button()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Button21 = New System.Windows.Forms.Button()
        Me.Button20 = New System.Windows.Forms.Button()
        Me.txtFolderPath = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Button19 = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Button18 = New System.Windows.Forms.Button()
        Me.Button16 = New System.Windows.Forms.Button()
        Me.Button15 = New System.Windows.Forms.Button()
        Me.Button14 = New System.Windows.Forms.Button()
        Me.Button13 = New System.Windows.Forms.Button()
        Me.Button12 = New System.Windows.Forms.Button()
        Me.Button11 = New System.Windows.Forms.Button()
        Me.Button10 = New System.Windows.Forms.Button()
        Me.Button9 = New System.Windows.Forms.Button()
        Me.Button8 = New System.Windows.Forms.Button()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.ListBox2 = New System.Windows.Forms.ListBox()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.StatusStrip1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.SuspendLayout()
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripStatusLabel2})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 712)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1238, 22)
        Me.StatusStrip1.TabIndex = 1
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(120, 17)
        Me.ToolStripStatusLabel1.Text = "ToolStripStatusLabel1"
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.Size = New System.Drawing.Size(120, 17)
        Me.ToolStripStatusLabel2.Text = "ToolStripStatusLabel2"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(645, 233)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(109, 41)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "Procesar"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(21, 142)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(412, 238)
        Me.ListBox1.TabIndex = 3
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Location = New System.Drawing.Point(12, 12)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1214, 697)
        Me.TabControl1.TabIndex = 4
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Button7)
        Me.TabPage1.Controls.Add(Me.Label6)
        Me.TabPage1.Controls.Add(Me.GroupBox2)
        Me.TabPage1.Controls.Add(Me.GroupBox1)
        Me.TabPage1.Controls.Add(Me.lblInfoNumFiles)
        Me.TabPage1.Controls.Add(Me.btnSelectFolder)
        Me.TabPage1.Controls.Add(Me.txtPathGCP)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Controls.Add(Me.ListBox1)
        Me.TabPage1.Controls.Add(Me.Button2)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(1206, 671)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Reproyectar GCPs"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Button7
        '
        Me.Button7.Location = New System.Drawing.Point(607, 129)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(147, 51)
        Me.Button7.TabIndex = 15
        Me.Button7.Text = "Generar Fichero rejilla a partir de ficheros GCP"
        Me.Button7.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ForeColor = System.Drawing.Color.RoyalBlue
        Me.Label6.Location = New System.Drawing.Point(18, 12)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(540, 13)
        Me.Label6.TabIndex = 14
        Me.Label6.Text = "Dado un directorio con GCPs en una proyección dada, los procesa reproyectándolos " &
    "a otro sistema de referencia"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtEPSGout)
        Me.GroupBox2.Controls.Add(Me.txtEPSGin)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Location = New System.Drawing.Point(439, 186)
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
        Me.GroupBox1.Location = New System.Drawing.Point(439, 280)
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
        Me.lblInfoNumFiles.Location = New System.Drawing.Point(490, 113)
        Me.lblInfoNumFiles.Name = "lblInfoNumFiles"
        Me.lblInfoNumFiles.Size = New System.Drawing.Size(78, 13)
        Me.lblInfoNumFiles.TabIndex = 7
        Me.lblInfoNumFiles.Text = "lblInfoNumFiles"
        Me.lblInfoNumFiles.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnSelectFolder
        '
        Me.btnSelectFolder.Location = New System.Drawing.Point(645, 74)
        Me.btnSelectFolder.Name = "btnSelectFolder"
        Me.btnSelectFolder.Size = New System.Drawing.Size(109, 36)
        Me.btnSelectFolder.TabIndex = 6
        Me.btnSelectFolder.Text = "Abrir"
        Me.btnSelectFolder.UseVisualStyleBackColor = True
        '
        'txtPathGCP
        '
        Me.txtPathGCP.Location = New System.Drawing.Point(21, 90)
        Me.txtPathGCP.Name = "txtPathGCP"
        Me.txtPathGCP.Size = New System.Drawing.Size(618, 20)
        Me.txtPathGCP.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(18, 74)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(208, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Directorio con los ficheros GCP de entrada"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.GroupBox3)
        Me.TabPage2.Controls.Add(Me.Button21)
        Me.TabPage2.Controls.Add(Me.Button20)
        Me.TabPage2.Controls.Add(Me.txtFolderPath)
        Me.TabPage2.Controls.Add(Me.Label9)
        Me.TabPage2.Controls.Add(Me.Button19)
        Me.TabPage2.Controls.Add(Me.Label8)
        Me.TabPage2.Controls.Add(Me.Button18)
        Me.TabPage2.Controls.Add(Me.Button16)
        Me.TabPage2.Controls.Add(Me.Button15)
        Me.TabPage2.Controls.Add(Me.Button14)
        Me.TabPage2.Controls.Add(Me.Button13)
        Me.TabPage2.Controls.Add(Me.Button12)
        Me.TabPage2.Controls.Add(Me.Button11)
        Me.TabPage2.Controls.Add(Me.Button10)
        Me.TabPage2.Controls.Add(Me.Button9)
        Me.TabPage2.Controls.Add(Me.Button8)
        Me.TabPage2.Controls.Add(Me.Button6)
        Me.TabPage2.Controls.Add(Me.ListBox2)
        Me.TabPage2.Controls.Add(Me.Button4)
        Me.TabPage2.Controls.Add(Me.Button3)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(1206, 671)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "TabPage2"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Button5)
        Me.GroupBox3.Controls.Add(Me.CheckBox1)
        Me.GroupBox3.Controls.Add(Me.Button25)
        Me.GroupBox3.Controls.Add(Me.Label10)
        Me.GroupBox3.Controls.Add(Me.ListBox3)
        Me.GroupBox3.Controls.Add(Me.Button24)
        Me.GroupBox3.Controls.Add(Me.TextBox2)
        Me.GroupBox3.Controls.Add(Me.Button23)
        Me.GroupBox3.Controls.Add(Me.Label11)
        Me.GroupBox3.Controls.Add(Me.Button22)
        Me.GroupBox3.Controls.Add(Me.TextBox3)
        Me.GroupBox3.Controls.Add(Me.Label12)
        Me.GroupBox3.Location = New System.Drawing.Point(569, 6)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(621, 427)
        Me.GroupBox3.TabIndex = 32
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Importar ficheros georreferenciados a formato GEODOCAT II"
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(322, 201)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(148, 17)
        Me.CheckBox1.TabIndex = 36
        Me.CheckBox1.Text = "Mover (Eliminar en origen)"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Button25
        '
        Me.Button25.Image = CType(resources.GetObject("Button25.Image"), System.Drawing.Image)
        Me.Button25.Location = New System.Drawing.Point(476, 376)
        Me.Button25.Name = "Button25"
        Me.Button25.Size = New System.Drawing.Size(92, 34)
        Me.Button25.TabIndex = 35
        Me.Button25.Text = "Guardar"
        Me.Button25.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button25.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(16, 208)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(79, 13)
        Me.Label10.TabIndex = 34
        Me.Label10.Text = "No procesados"
        '
        'ListBox3
        '
        Me.ListBox3.FormattingEnabled = True
        Me.ListBox3.Location = New System.Drawing.Point(19, 224)
        Me.ListBox3.Name = "ListBox3"
        Me.ListBox3.Size = New System.Drawing.Size(451, 186)
        Me.ListBox3.TabIndex = 33
        '
        'Button24
        '
        Me.Button24.Image = CType(resources.GetObject("Button24.Image"), System.Drawing.Image)
        Me.Button24.Location = New System.Drawing.Point(6, 93)
        Me.Button24.Name = "Button24"
        Me.Button24.Size = New System.Drawing.Size(269, 33)
        Me.Button24.TabIndex = 32
        Me.Button24.Text = "Estructura Sellado -> Estructura GEODOCAT II"
        Me.Button24.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button24.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button24.UseVisualStyleBackColor = True
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(64, 35)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(406, 20)
        Me.TextBox2.TabIndex = 26
        '
        'Button23
        '
        Me.Button23.Image = CType(resources.GetObject("Button23.Image"), System.Drawing.Image)
        Me.Button23.Location = New System.Drawing.Point(480, 59)
        Me.Button23.Name = "Button23"
        Me.Button23.Size = New System.Drawing.Size(45, 23)
        Me.Button23.TabIndex = 31
        Me.Button23.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(13, 38)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(38, 13)
        Me.Label11.TabIndex = 27
        Me.Label11.Text = "Origen"
        '
        'Button22
        '
        Me.Button22.Image = CType(resources.GetObject("Button22.Image"), System.Drawing.Image)
        Me.Button22.Location = New System.Drawing.Point(480, 33)
        Me.Button22.Name = "Button22"
        Me.Button22.Size = New System.Drawing.Size(45, 23)
        Me.Button22.TabIndex = 30
        Me.Button22.UseVisualStyleBackColor = True
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(64, 61)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(406, 20)
        Me.TextBox3.TabIndex = 28
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(13, 64)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(43, 13)
        Me.Label12.TabIndex = 29
        Me.Label12.Text = "Destino"
        '
        'Button21
        '
        Me.Button21.Image = CType(resources.GetObject("Button21.Image"), System.Drawing.Image)
        Me.Button21.Location = New System.Drawing.Point(442, 399)
        Me.Button21.Name = "Button21"
        Me.Button21.Size = New System.Drawing.Size(92, 34)
        Me.Button21.TabIndex = 24
        Me.Button21.Text = "Guardar"
        Me.Button21.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button21.UseVisualStyleBackColor = True
        '
        'Button20
        '
        Me.Button20.Image = CType(resources.GetObject("Button20.Image"), System.Drawing.Image)
        Me.Button20.Location = New System.Drawing.Point(459, 230)
        Me.Button20.Name = "Button20"
        Me.Button20.Size = New System.Drawing.Size(75, 37)
        Me.Button20.TabIndex = 23
        Me.Button20.Text = "Abrir"
        Me.Button20.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button20.UseVisualStyleBackColor = True
        '
        'txtFolderPath
        '
        Me.txtFolderPath.Location = New System.Drawing.Point(21, 246)
        Me.txtFolderPath.Name = "txtFolderPath"
        Me.txtFolderPath.Size = New System.Drawing.Size(415, 20)
        Me.txtFolderPath.TabIndex = 22
        Me.txtFolderPath.Text = "D:\Volcados\reproyeccion"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(18, 230)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(97, 13)
        Me.Label9.TabIndex = 21
        Me.Label9.Text = "Directorio de salida"
        '
        'Button19
        '
        Me.Button19.Image = CType(resources.GetObject("Button19.Image"), System.Drawing.Image)
        Me.Button19.Location = New System.Drawing.Point(21, 427)
        Me.Button19.Name = "Button19"
        Me.Button19.Size = New System.Drawing.Size(176, 72)
        Me.Button19.TabIndex = 20
        Me.Button19.Text = "Obtener fichero conversión 23030->25830 para Conjuntas"
        Me.Button19.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button19.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(18, 552)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(418, 13)
        Me.Label8.TabIndex = 19
        Me.Label8.Text = "Test para comprobar que cada documento tiene un fichero georreferenciado localiza" &
    "do"
        '
        'Button18
        '
        Me.Button18.Location = New System.Drawing.Point(21, 64)
        Me.Button18.Name = "Button18"
        Me.Button18.Size = New System.Drawing.Size(264, 23)
        Me.Button18.TabIndex = 18
        Me.Button18.Text = "Obtener fichero conversión 23030->25830 para PPc"
        Me.Button18.UseVisualStyleBackColor = True
        '
        'Button16
        '
        Me.Button16.Location = New System.Drawing.Point(291, 180)
        Me.Button16.Name = "Button16"
        Me.Button16.Size = New System.Drawing.Size(195, 23)
        Me.Button16.TabIndex = 14
        Me.Button16.Text = "Plantilla Hojas kilométricas Cutter"
        Me.Button16.UseVisualStyleBackColor = True
        '
        'Button15
        '
        Me.Button15.Location = New System.Drawing.Point(291, 151)
        Me.Button15.Name = "Button15"
        Me.Button15.Size = New System.Drawing.Size(195, 23)
        Me.Button15.TabIndex = 13
        Me.Button15.Text = "Plantilla Directorios Cutter"
        Me.Button15.UseVisualStyleBackColor = True
        '
        'Button14
        '
        Me.Button14.Location = New System.Drawing.Point(291, 122)
        Me.Button14.Name = "Button14"
        Me.Button14.Size = New System.Drawing.Size(195, 23)
        Me.Button14.TabIndex = 12
        Me.Button14.Text = "Plantilla Edificación Cutter"
        Me.Button14.UseVisualStyleBackColor = True
        '
        'Button13
        '
        Me.Button13.Location = New System.Drawing.Point(291, 93)
        Me.Button13.Name = "Button13"
        Me.Button13.Size = New System.Drawing.Size(195, 23)
        Me.Button13.TabIndex = 11
        Me.Button13.Text = "Plantilla Conjuntas Cutter"
        Me.Button13.UseVisualStyleBackColor = True
        '
        'Button12
        '
        Me.Button12.Location = New System.Drawing.Point(291, 64)
        Me.Button12.Name = "Button12"
        Me.Button12.Size = New System.Drawing.Size(195, 23)
        Me.Button12.TabIndex = 10
        Me.Button12.Text = "Plantilla Planimetrías Cutter"
        Me.Button12.UseVisualStyleBackColor = True
        '
        'Button11
        '
        Me.Button11.Location = New System.Drawing.Point(291, 35)
        Me.Button11.Name = "Button11"
        Me.Button11.Size = New System.Drawing.Size(195, 23)
        Me.Button11.TabIndex = 9
        Me.Button11.Text = "Plantilla Altimetrías Cutter"
        Me.Button11.UseVisualStyleBackColor = True
        '
        'Button10
        '
        Me.Button10.Location = New System.Drawing.Point(291, 6)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(195, 23)
        Me.Button10.TabIndex = 8
        Me.Button10.Text = "Plantilla planos de población Cutter"
        Me.Button10.UseVisualStyleBackColor = True
        '
        'Button9
        '
        Me.Button9.Location = New System.Drawing.Point(21, 35)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(264, 23)
        Me.Button9.TabIndex = 7
        Me.Button9.Text = "Obtener fichero conversión 23030->25830 para HK"
        Me.Button9.UseVisualStyleBackColor = True
        '
        'Button8
        '
        Me.Button8.Location = New System.Drawing.Point(21, 89)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(264, 27)
        Me.Button8.TabIndex = 6
        Me.Button8.Text = "Obtener fichero conversión 23030->25830 para PP"
        Me.Button8.UseVisualStyleBackColor = True
        '
        'Button6
        '
        Me.Button6.Location = New System.Drawing.Point(21, 122)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(194, 46)
        Me.Button6.TabIndex = 5
        Me.Button6.Text = "Test_cdd_planosedificacion_report"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'ListBox2
        '
        Me.ListBox2.FormattingEnabled = True
        Me.ListBox2.Location = New System.Drawing.Point(21, 272)
        Me.ListBox2.Name = "ListBox2"
        Me.ListBox2.Size = New System.Drawing.Size(513, 121)
        Me.ListBox2.TabIndex = 3
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(21, 174)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(194, 46)
        Me.Button4.TabIndex = 2
        Me.Button4.Text = "Test_cdd_planospoblacion_report"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Image = CType(resources.GetObject("Button3.Image"), System.Drawing.Image)
        Me.Button3.Location = New System.Drawing.Point(21, 568)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(118, 61)
        Me.Button3.TabIndex = 1
        Me.Button3.Text = "TestECWExist"
        Me.Button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button3.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.Button1)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(1206, 671)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "TabPage3"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(20, 22)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(160, 40)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Generar script para eliminar contornos duplicados"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Image = CType(resources.GetObject("Button5.Image"), System.Drawing.Image)
        Me.Button5.Location = New System.Drawing.Point(6, 132)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(269, 33)
        Me.Button5.TabIndex = 37
        Me.Button5.Text = "Numdoc -> Estructura GEODOCAT II"
        Me.Button5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button5.UseVisualStyleBackColor = True
        '
        'frmDevel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1238, 734)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Name = "frmDevel"
        Me.Text = "frmDevel"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents btnSelectFolder As System.Windows.Forms.Button
    Friend WithEvents txtPathGCP As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents lblInfoNumFiles As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtEPSGout As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtEPSGin As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtDeltaY As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtDeltaX As System.Windows.Forms.TextBox
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents ListBox2 As System.Windows.Forms.ListBox
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents Button8 As System.Windows.Forms.Button
    Friend WithEvents Button9 As System.Windows.Forms.Button
    Friend WithEvents Button10 As System.Windows.Forms.Button
    Friend WithEvents Button12 As System.Windows.Forms.Button
    Friend WithEvents Button11 As System.Windows.Forms.Button
    Friend WithEvents Button13 As System.Windows.Forms.Button
    Friend WithEvents Button14 As System.Windows.Forms.Button
    Friend WithEvents Button15 As System.Windows.Forms.Button
    Friend WithEvents Button16 As System.Windows.Forms.Button
    Friend WithEvents Button18 As Button
    Friend WithEvents ToolStripStatusLabel2 As ToolStripStatusLabel
    Friend WithEvents Label8 As Label
    Friend WithEvents Button19 As Button
    Friend WithEvents txtFolderPath As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Button20 As Button
    Friend WithEvents Button21 As Button
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents Button24 As Button
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents Button23 As Button
    Friend WithEvents Label11 As Label
    Friend WithEvents Button22 As Button
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents ListBox3 As ListBox
    Friend WithEvents Button25 As Button
    Friend WithEvents CheckBox1 As CheckBox
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents Button1 As Button
    Friend WithEvents Button5 As Button
End Class
