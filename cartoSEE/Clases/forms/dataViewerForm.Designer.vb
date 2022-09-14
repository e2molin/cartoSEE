<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dataViewerForm
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dataViewerForm))
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel3 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnTodos = New System.Windows.Forms.ToolStripButton()
        Me.btnDetalles = New System.Windows.Forms.ToolStripButton()
        Me.btnExportar = New System.Windows.Forms.ToolStripButton()
        Me.btnLaunchDocument = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabel2 = New System.Windows.Forms.ToolStripLabel()
        Me.cboFiltros = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.txtSearch = New System.Windows.Forms.ToolStripTextBox()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnPrev = New System.Windows.Forms.ToolStripButton()
        Me.btnNext = New System.Windows.Forms.ToolStripButton()
        Me.btnAjustar = New System.Windows.Forms.ToolStripButton()
        Me.gBoxdetails = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.lvDetails = New System.Windows.Forms.ListView()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.txtObserv = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.txtAtributos = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.lvVinculos = New System.Windows.Forms.ListView()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.ImageList2 = New System.Windows.Forms.ImageList(Me.components)
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.StatusStrip1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.gBoxdetails.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripStatusLabel2, Me.ToolStripStatusLabel3})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 479)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(903, 22)
        Me.StatusStrip1.TabIndex = 0
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(119, 17)
        Me.ToolStripStatusLabel1.Text = "ToolStripStatusLabel1"
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.Size = New System.Drawing.Size(119, 17)
        Me.ToolStripStatusLabel2.Text = "ToolStripStatusLabel2"
        '
        'ToolStripStatusLabel3
        '
        Me.ToolStripStatusLabel3.Name = "ToolStripStatusLabel3"
        Me.ToolStripStatusLabel3.Size = New System.Drawing.Size(119, 17)
        Me.ToolStripStatusLabel3.Text = "ToolStripStatusLabel3"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnTodos, Me.btnDetalles, Me.btnExportar, Me.btnLaunchDocument, Me.ToolStripSeparator2, Me.ToolStripLabel2, Me.cboFiltros, Me.ToolStripLabel1, Me.txtSearch, Me.ToolStripSeparator1, Me.btnPrev, Me.btnNext, Me.btnAjustar})
        Me.ToolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(903, 53)
        Me.ToolStrip1.TabIndex = 16
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnTodos
        '
        Me.btnTodos.AutoSize = False
        Me.btnTodos.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTodos.Image = CType(resources.GetObject("btnTodos.Image"), System.Drawing.Image)
        Me.btnTodos.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btnTodos.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnTodos.Name = "btnTodos"
        Me.btnTodos.Size = New System.Drawing.Size(55, 50)
        Me.btnTodos.Text = "Lista"
        Me.btnTodos.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnTodos.ToolTipText = "Muestra un listado de todos los registros"
        '
        'btnDetalles
        '
        Me.btnDetalles.AutoSize = False
        Me.btnDetalles.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDetalles.Image = CType(resources.GetObject("btnDetalles.Image"), System.Drawing.Image)
        Me.btnDetalles.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btnDetalles.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDetalles.Name = "btnDetalles"
        Me.btnDetalles.Size = New System.Drawing.Size(55, 50)
        Me.btnDetalles.Text = "Detalle"
        Me.btnDetalles.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnDetalles.ToolTipText = "Información detallada del documento"
        '
        'btnExportar
        '
        Me.btnExportar.AutoSize = False
        Me.btnExportar.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExportar.Image = CType(resources.GetObject("btnExportar.Image"), System.Drawing.Image)
        Me.btnExportar.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btnExportar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnExportar.Name = "btnExportar"
        Me.btnExportar.Size = New System.Drawing.Size(55, 50)
        Me.btnExportar.Text = "Exportar"
        Me.btnExportar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnExportar.ToolTipText = "Exportar todos los resultados a CSV"
        '
        'btnLaunchDocument
        '
        Me.btnLaunchDocument.AutoSize = False
        Me.btnLaunchDocument.Image = CType(resources.GetObject("btnLaunchDocument.Image"), System.Drawing.Image)
        Me.btnLaunchDocument.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btnLaunchDocument.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnLaunchDocument.Name = "btnLaunchDocument"
        Me.btnLaunchDocument.Size = New System.Drawing.Size(55, 50)
        Me.btnLaunchDocument.Text = "CdD"
        Me.btnLaunchDocument.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnLaunchDocument.ToolTipText = "Preparar datos Centro de Descargas"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.AutoSize = False
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 41)
        '
        'ToolStripLabel2
        '
        Me.ToolStripLabel2.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripLabel2.Name = "ToolStripLabel2"
        Me.ToolStripLabel2.Size = New System.Drawing.Size(35, 12)
        Me.ToolStripLabel2.Text = "Filtrar"
        Me.ToolStripLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboFiltros
        '
        Me.cboFiltros.Margin = New System.Windows.Forms.Padding(-32, 16, 1, 0)
        Me.cboFiltros.Name = "cboFiltros"
        Me.cboFiltros.Size = New System.Drawing.Size(121, 23)
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.AutoSize = False
        Me.ToolStripLabel1.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(80, 13)
        Me.ToolStripLabel1.Text = "Buscar"
        Me.ToolStripLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtSearch
        '
        Me.txtSearch.AutoSize = False
        Me.txtSearch.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtSearch.Margin = New System.Windows.Forms.Padding(-76, 16, 1, 0)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(80, 21)
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.AutoSize = False
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 41)
        '
        'btnPrev
        '
        Me.btnPrev.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnPrev.AutoSize = False
        Me.btnPrev.Image = CType(resources.GetObject("btnPrev.Image"), System.Drawing.Image)
        Me.btnPrev.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btnPrev.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnPrev.Name = "btnPrev"
        Me.btnPrev.Size = New System.Drawing.Size(60, 50)
        Me.btnPrev.Text = "Anterior"
        Me.btnPrev.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnPrev.ToolTipText = "Registro anterior"
        '
        'btnNext
        '
        Me.btnNext.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnNext.AutoSize = False
        Me.btnNext.Image = CType(resources.GetObject("btnNext.Image"), System.Drawing.Image)
        Me.btnNext.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btnNext.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(60, 50)
        Me.btnNext.Text = "Siguiente"
        Me.btnNext.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnNext.ToolTipText = "Registro siguiente"
        '
        'btnAjustar
        '
        Me.btnAjustar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnAjustar.AutoSize = False
        Me.btnAjustar.Image = CType(resources.GetObject("btnAjustar.Image"), System.Drawing.Image)
        Me.btnAjustar.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btnAjustar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAjustar.Name = "btnAjustar"
        Me.btnAjustar.Size = New System.Drawing.Size(60, 50)
        Me.btnAjustar.Text = "Ajustar"
        Me.btnAjustar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnAjustar.ToolTipText = "Registro siguiente"
        '
        'gBoxdetails
        '
        Me.gBoxdetails.Controls.Add(Me.TableLayoutPanel1)
        Me.gBoxdetails.Location = New System.Drawing.Point(25, 70)
        Me.gBoxdetails.Name = "gBoxdetails"
        Me.gBoxdetails.Size = New System.Drawing.Size(645, 336)
        Me.gBoxdetails.TabIndex = 19
        Me.gBoxdetails.TabStop = False
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.lvDetails, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.TabControl1, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(6, 19)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(633, 311)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'lvDetails
        '
        Me.lvDetails.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvDetails.HideSelection = False
        Me.lvDetails.Location = New System.Drawing.Point(3, 3)
        Me.lvDetails.Name = "lvDetails"
        Me.lvDetails.Size = New System.Drawing.Size(310, 305)
        Me.lvDetails.TabIndex = 0
        Me.lvDetails.UseCompatibleStateImageBehavior = False
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(319, 3)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(311, 305)
        Me.TabControl1.TabIndex = 1
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.TableLayoutPanel2)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(303, 279)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Información"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.Panel2, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.Panel1, 0, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 2
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(297, 273)
        Me.TableLayoutPanel2.TabIndex = 0
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.txtObserv)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(3, 139)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(291, 131)
        Me.Panel2.TabIndex = 1
        '
        'txtObserv
        '
        Me.txtObserv.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtObserv.Location = New System.Drawing.Point(3, 16)
        Me.txtObserv.Multiline = True
        Me.txtObserv.Name = "txtObserv"
        Me.txtObserv.Size = New System.Drawing.Size(285, 112)
        Me.txtObserv.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(78, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Observaciones"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.txtAtributos)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(291, 130)
        Me.Panel1.TabIndex = 0
        '
        'txtAtributos
        '
        Me.txtAtributos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAtributos.Location = New System.Drawing.Point(3, 16)
        Me.txtAtributos.Multiline = True
        Me.txtAtributos.Name = "txtAtributos"
        Me.txtAtributos.Size = New System.Drawing.Size(285, 111)
        Me.txtAtributos.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(48, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Atributos"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.Button4)
        Me.TabPage2.Controls.Add(Me.Button3)
        Me.TabPage2.Controls.Add(Me.Button2)
        Me.TabPage2.Controls.Add(Me.Button1)
        Me.TabPage2.Controls.Add(Me.lvVinculos)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(303, 279)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Documentación"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button4.Image = CType(resources.GetObject("Button4.Image"), System.Drawing.Image)
        Me.Button4.Location = New System.Drawing.Point(151, 3)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(32, 29)
        Me.Button4.TabIndex = 4
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button3.Image = CType(resources.GetObject("Button3.Image"), System.Drawing.Image)
        Me.Button3.Location = New System.Drawing.Point(189, 3)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(32, 29)
        Me.Button3.TabIndex = 3
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button2.Image = CType(resources.GetObject("Button2.Image"), System.Drawing.Image)
        Me.Button2.Location = New System.Drawing.Point(227, 3)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(32, 29)
        Me.Button2.TabIndex = 2
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Image = CType(resources.GetObject("Button1.Image"), System.Drawing.Image)
        Me.Button1.Location = New System.Drawing.Point(265, 3)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(32, 29)
        Me.Button1.TabIndex = 1
        Me.Button1.UseVisualStyleBackColor = True
        '
        'lvVinculos
        '
        Me.lvVinculos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvVinculos.HideSelection = False
        Me.lvVinculos.Location = New System.Drawing.Point(6, 38)
        Me.lvVinculos.Name = "lvVinculos"
        Me.lvVinculos.Size = New System.Drawing.Size(291, 235)
        Me.lvVinculos.TabIndex = 0
        Me.lvVinculos.UseCompatibleStateImageBehavior = False
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.DataGridView1.Location = New System.Drawing.Point(744, 93)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(67, 40)
        Me.DataGridView1.TabIndex = 20
        '
        'ImageList1
        '
        Me.ImageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.ImageList1.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        '
        'ImageList2
        '
        Me.ImageList2.ImageStream = CType(resources.GetObject("ImageList2.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList2.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList2.Images.SetKeyName(0, "item16.png")
        '
        'dataViewerForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(903, 501)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.gBoxdetails)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "dataViewerForm"
        Me.Text = "dataViewer"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.gBoxdetails.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents btnTodos As ToolStripButton
    Friend WithEvents btnDetalles As ToolStripButton
    Friend WithEvents btnExportar As ToolStripButton
    Friend WithEvents btnLaunchDocument As ToolStripButton
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents ToolStripLabel2 As ToolStripLabel
    Friend WithEvents cboFiltros As ToolStripComboBox
    Friend WithEvents ToolStripLabel1 As ToolStripLabel
    Friend WithEvents txtSearch As ToolStripTextBox
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents btnPrev As ToolStripButton
    Friend WithEvents btnNext As ToolStripButton
    Friend WithEvents btnAjustar As ToolStripButton
    Friend WithEvents gBoxdetails As GroupBox
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents lvDetails As ListView
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents txtObserv As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents txtAtributos As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents Button4 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents lvVinculos As ListView
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents ToolStripStatusLabel2 As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel3 As ToolStripStatusLabel
    Friend WithEvents ImageList1 As ImageList
    Friend WithEvents ImageList2 As ImageList
    Friend WithEvents SaveFileDialog1 As SaveFileDialog
End Class
