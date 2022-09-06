<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMuniHisto
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMuniHisto))
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnTodos = New System.Windows.Forms.ToolStripButton()
        Me.btnDetalles = New System.Windows.Forms.ToolStripButton()
        Me.btnExportar = New System.Windows.Forms.ToolStripButton()
        Me.btnRefresh = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabel2 = New System.Windows.Forms.ToolStripLabel()
        Me.cboFiltros = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.txtSearch = New System.Windows.Forms.ToolStripTextBox()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnPrev = New System.Windows.Forms.ToolStripButton()
        Me.btnNext = New System.Windows.Forms.ToolStripButton()
        Me.btnEdit = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripDropDownButton1 = New System.Windows.Forms.ToolStripDropDownButton()
        Me.mnuColumna1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuColumna2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuColumna3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuColumna4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuColumna5 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuColumna6 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuColumna7 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuColumna8 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuColumna9 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuColumna10 = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnNew = New System.Windows.Forms.ToolStripButton()
        Me.dropdownEnlacesCDD = New System.Windows.Forms.ToolStripDropDownButton()
        Me.ddEnlacePlanis = New System.Windows.Forms.ToolStripMenuItem()
        Me.ddEnlacePLPOB = New System.Windows.Forms.ToolStripMenuItem()
        Me.ddEnlacePLEDI = New System.Windows.Forms.ToolStripMenuItem()
        Me.ddEnlaceHKPUP = New System.Windows.Forms.ToolStripMenuItem()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.lvDetails = New System.Windows.Forms.ListView()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.txtObserv = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lvColin = New System.Windows.Forms.ListView()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel3 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.ToolStrip1.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnTodos, Me.btnDetalles, Me.btnExportar, Me.btnRefresh, Me.ToolStripSeparator2, Me.ToolStripLabel2, Me.cboFiltros, Me.ToolStripLabel1, Me.txtSearch, Me.ToolStripSeparator1, Me.btnPrev, Me.btnNext, Me.btnEdit, Me.ToolStripDropDownButton1, Me.btnNew, Me.dropdownEnlacesCDD})
        Me.ToolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1008, 53)
        Me.ToolStrip1.TabIndex = 16
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnTodos
        '
        Me.btnTodos.AutoSize = False
        Me.btnTodos.Image = CType(resources.GetObject("btnTodos.Image"), System.Drawing.Image)
        Me.btnTodos.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btnTodos.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnTodos.Name = "btnTodos"
        Me.btnTodos.Size = New System.Drawing.Size(65, 50)
        Me.btnTodos.Text = "Todos"
        Me.btnTodos.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnTodos.ToolTipText = "Muestra un listado de todos los registros"
        '
        'btnDetalles
        '
        Me.btnDetalles.AutoSize = False
        Me.btnDetalles.Image = CType(resources.GetObject("btnDetalles.Image"), System.Drawing.Image)
        Me.btnDetalles.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btnDetalles.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDetalles.Name = "btnDetalles"
        Me.btnDetalles.Size = New System.Drawing.Size(65, 50)
        Me.btnDetalles.Text = "Detalle"
        Me.btnDetalles.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnDetalles.ToolTipText = "Información detallada del registro"
        '
        'btnExportar
        '
        Me.btnExportar.AutoSize = False
        Me.btnExportar.Image = CType(resources.GetObject("btnExportar.Image"), System.Drawing.Image)
        Me.btnExportar.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btnExportar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnExportar.Name = "btnExportar"
        Me.btnExportar.Size = New System.Drawing.Size(65, 50)
        Me.btnExportar.Text = "Exportar"
        Me.btnExportar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnExportar.ToolTipText = "Exportar todos los resultados a CSV"
        '
        'btnRefresh
        '
        Me.btnRefresh.AutoSize = False
        Me.btnRefresh.Image = CType(resources.GetObject("btnRefresh.Image"), System.Drawing.Image)
        Me.btnRefresh.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(65, 50)
        Me.btnRefresh.Text = "Refrescar"
        Me.btnRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnRefresh.ToolTipText = "Actualizar datos"
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
        Me.cboFiltros.Items.AddRange(New Object() {"Todos", "Nombre", "Municipio", "Provincia", "Código"})
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
        Me.btnPrev.Size = New System.Drawing.Size(65, 50)
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
        Me.btnNext.Size = New System.Drawing.Size(65, 50)
        Me.btnNext.Text = "Siguiente"
        Me.btnNext.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnNext.ToolTipText = "Registro siguiente"
        '
        'btnEdit
        '
        Me.btnEdit.AutoSize = False
        Me.btnEdit.Image = CType(resources.GetObject("btnEdit.Image"), System.Drawing.Image)
        Me.btnEdit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btnEdit.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(65, 50)
        Me.btnEdit.Text = "Editar"
        Me.btnEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnEdit.ToolTipText = "Edición del documento"
        '
        'ToolStripDropDownButton1
        '
        Me.ToolStripDropDownButton1.AutoSize = False
        Me.ToolStripDropDownButton1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuColumna1, Me.mnuColumna2, Me.mnuColumna3, Me.mnuColumna4, Me.mnuColumna5, Me.mnuColumna6, Me.mnuColumna7, Me.mnuColumna8, Me.mnuColumna9, Me.mnuColumna10})
        Me.ToolStripDropDownButton1.Image = CType(resources.GetObject("ToolStripDropDownButton1.Image"), System.Drawing.Image)
        Me.ToolStripDropDownButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ToolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripDropDownButton1.Name = "ToolStripDropDownButton1"
        Me.ToolStripDropDownButton1.Size = New System.Drawing.Size(70, 50)
        Me.ToolStripDropDownButton1.Text = "Columnas"
        Me.ToolStripDropDownButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolStripDropDownButton1.ToolTipText = "Ocultar y mostrar columnas"
        Me.ToolStripDropDownButton1.Visible = False
        '
        'mnuColumna1
        '
        Me.mnuColumna1.Checked = True
        Me.mnuColumna1.CheckOnClick = True
        Me.mnuColumna1.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mnuColumna1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.mnuColumna1.Name = "mnuColumna1"
        Me.mnuColumna1.Size = New System.Drawing.Size(187, 22)
        Me.mnuColumna1.Text = "ToolStripMenuItem1"
        '
        'mnuColumna2
        '
        Me.mnuColumna2.Checked = True
        Me.mnuColumna2.CheckOnClick = True
        Me.mnuColumna2.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mnuColumna2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.mnuColumna2.Name = "mnuColumna2"
        Me.mnuColumna2.Size = New System.Drawing.Size(187, 22)
        Me.mnuColumna2.Text = "ToolStripMenuItem2"
        '
        'mnuColumna3
        '
        Me.mnuColumna3.Checked = True
        Me.mnuColumna3.CheckOnClick = True
        Me.mnuColumna3.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mnuColumna3.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.mnuColumna3.Name = "mnuColumna3"
        Me.mnuColumna3.Size = New System.Drawing.Size(187, 22)
        Me.mnuColumna3.Text = "ToolStripMenuItem3"
        '
        'mnuColumna4
        '
        Me.mnuColumna4.Checked = True
        Me.mnuColumna4.CheckOnClick = True
        Me.mnuColumna4.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mnuColumna4.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.mnuColumna4.Name = "mnuColumna4"
        Me.mnuColumna4.Size = New System.Drawing.Size(187, 22)
        Me.mnuColumna4.Text = "ToolStripMenuItem4"
        '
        'mnuColumna5
        '
        Me.mnuColumna5.Checked = True
        Me.mnuColumna5.CheckOnClick = True
        Me.mnuColumna5.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mnuColumna5.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.mnuColumna5.Name = "mnuColumna5"
        Me.mnuColumna5.Size = New System.Drawing.Size(187, 22)
        Me.mnuColumna5.Text = "ToolStripMenuItem5"
        '
        'mnuColumna6
        '
        Me.mnuColumna6.Checked = True
        Me.mnuColumna6.CheckOnClick = True
        Me.mnuColumna6.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mnuColumna6.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.mnuColumna6.Name = "mnuColumna6"
        Me.mnuColumna6.Size = New System.Drawing.Size(187, 22)
        Me.mnuColumna6.Text = "ToolStripMenuItem6"
        '
        'mnuColumna7
        '
        Me.mnuColumna7.Checked = True
        Me.mnuColumna7.CheckOnClick = True
        Me.mnuColumna7.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mnuColumna7.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.mnuColumna7.Name = "mnuColumna7"
        Me.mnuColumna7.Size = New System.Drawing.Size(187, 22)
        Me.mnuColumna7.Text = "ToolStripMenuItem7"
        '
        'mnuColumna8
        '
        Me.mnuColumna8.Checked = True
        Me.mnuColumna8.CheckOnClick = True
        Me.mnuColumna8.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mnuColumna8.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.mnuColumna8.Name = "mnuColumna8"
        Me.mnuColumna8.Size = New System.Drawing.Size(187, 22)
        Me.mnuColumna8.Text = "ToolStripMenuItem8"
        '
        'mnuColumna9
        '
        Me.mnuColumna9.Checked = True
        Me.mnuColumna9.CheckOnClick = True
        Me.mnuColumna9.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mnuColumna9.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.mnuColumna9.Name = "mnuColumna9"
        Me.mnuColumna9.Size = New System.Drawing.Size(187, 22)
        Me.mnuColumna9.Text = "ToolStripMenuItem9"
        '
        'mnuColumna10
        '
        Me.mnuColumna10.Checked = True
        Me.mnuColumna10.CheckOnClick = True
        Me.mnuColumna10.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mnuColumna10.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.mnuColumna10.Name = "mnuColumna10"
        Me.mnuColumna10.Size = New System.Drawing.Size(187, 22)
        Me.mnuColumna10.Text = "ToolStripMenuItem10"
        '
        'btnNew
        '
        Me.btnNew.AutoSize = False
        Me.btnNew.Image = CType(resources.GetObject("btnNew.Image"), System.Drawing.Image)
        Me.btnNew.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(65, 50)
        Me.btnNew.Text = "Nuevo"
        Me.btnNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnNew.ToolTipText = "Edición del documento"
        '
        'dropdownEnlacesCDD
        '
        Me.dropdownEnlacesCDD.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ddEnlacePlanis, Me.ddEnlacePLPOB, Me.ddEnlacePLEDI, Me.ddEnlaceHKPUP})
        Me.dropdownEnlacesCDD.Image = CType(resources.GetObject("dropdownEnlacesCDD.Image"), System.Drawing.Image)
        Me.dropdownEnlacesCDD.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.dropdownEnlacesCDD.Name = "dropdownEnlacesCDD"
        Me.dropdownEnlacesCDD.Size = New System.Drawing.Size(80, 43)
        Me.dropdownEnlacesCDD.Text = "Enlace CdD"
        Me.dropdownEnlacesCDD.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.dropdownEnlacesCDD.ToolTipText = "Enlaces CdD"
        '
        'ddEnlacePlanis
        '
        Me.ddEnlacePlanis.Name = "ddEnlacePlanis"
        Me.ddEnlacePlanis.Size = New System.Drawing.Size(372, 22)
        Me.ddEnlacePlanis.Text = "Planimetrías, altimetrías y conjunta"
        '
        'ddEnlacePLPOB
        '
        Me.ddEnlacePLPOB.Name = "ddEnlacePLPOB"
        Me.ddEnlacePLPOB.Size = New System.Drawing.Size(372, 22)
        Me.ddEnlacePLPOB.Text = "Planos de población"
        '
        'ddEnlacePLEDI
        '
        Me.ddEnlacePLEDI.Name = "ddEnlacePLEDI"
        Me.ddEnlacePLEDI.Size = New System.Drawing.Size(372, 22)
        Me.ddEnlacePLEDI.Text = "Planos de edificación"
        '
        'ddEnlaceHKPUP
        '
        Me.ddEnlaceHKPUP.Name = "ddEnlaceHKPUP"
        Me.ddEnlaceHKPUP.Size = New System.Drawing.Size(372, 22)
        Me.ddEnlaceHKPUP.Text = "Hojas kilométricas, parcelario urbano y planos directores"
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(225, 74)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(705, 528)
        Me.TableLayoutPanel1.TabIndex = 18
        '
        'lvDetails
        '
        Me.lvDetails.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvDetails.HideSelection = False
        Me.lvDetails.Location = New System.Drawing.Point(3, 3)
        Me.lvDetails.Name = "lvDetails"
        Me.lvDetails.ShowItemToolTips = True
        Me.lvDetails.Size = New System.Drawing.Size(346, 522)
        Me.lvDetails.TabIndex = 0
        Me.lvDetails.UseCompatibleStateImageBehavior = False
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(355, 3)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(347, 522)
        Me.TabControl1.TabIndex = 1
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.TableLayoutPanel2)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(339, 496)
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
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(333, 490)
        Me.TableLayoutPanel2.TabIndex = 0
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.txtObserv)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(3, 248)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(327, 239)
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
        Me.txtObserv.Size = New System.Drawing.Size(321, 220)
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
        Me.Panel1.Controls.Add(Me.lvColin)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(327, 239)
        Me.Panel1.TabIndex = 0
        '
        'lvColin
        '
        Me.lvColin.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvColin.HideSelection = False
        Me.lvColin.Location = New System.Drawing.Point(0, 0)
        Me.lvColin.Name = "lvColin"
        Me.lvColin.ShowItemToolTips = True
        Me.lvColin.Size = New System.Drawing.Size(327, 239)
        Me.lvColin.TabIndex = 0
        Me.lvColin.UseCompatibleStateImageBehavior = False
        '
        'TabPage2
        '
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(339, 496)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Documentación"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.DataGridView1.Location = New System.Drawing.Point(0, 55)
        Me.DataGridView1.MultiSelect = False
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(67, 40)
        Me.DataGridView1.TabIndex = 19
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripStatusLabel2, Me.ToolStripStatusLabel3})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 619)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1008, 22)
        Me.StatusStrip1.TabIndex = 20
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
        'frmMuniHisto
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1008, 641)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.DataGridView1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMuniHisto"
        Me.Text = "Gestión de municipios históricos"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnTodos As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnDetalles As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnExportar As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEdit As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnRefresh As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripLabel2 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents cboFiltros As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents txtSearch As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnPrev As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnNext As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripDropDownButton1 As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents mnuColumna1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuColumna2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuColumna3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuColumna4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuColumna5 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuColumna6 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuColumna7 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuColumna8 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuColumna9 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuColumna10 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lvDetails As System.Windows.Forms.ListView
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents txtObserv As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel2 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents btnNew As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripStatusLabel3 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents lvColin As System.Windows.Forms.ListView
    Friend WithEvents dropdownEnlacesCDD As ToolStripDropDownButton
    Friend WithEvents ddEnlacePlanis As ToolStripMenuItem
    Friend WithEvents ddEnlacePLPOB As ToolStripMenuItem
    Friend WithEvents ddEnlacePLEDI As ToolStripMenuItem
    Friend WithEvents ddEnlaceHKPUP As ToolStripMenuItem
End Class
