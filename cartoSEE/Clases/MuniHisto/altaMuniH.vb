Public Class altaMuniH
    Inherits System.Windows.Forms.Form

    Dim BuscarNomen As Boolean
    Dim variacionMunis As Boolean
    Dim mDialogResp As Boolean = False
    Dim mModo As String
    Dim territorioHistorico As MuniHisto


#Region "Definición propiedades"

    Property muniHEdit() As MuniHisto
        Get
            Return territorioHistorico
        End Get
        Set(ByVal value As MuniHisto)
            territorioHistorico = value
        End Set
    End Property

    Property dialogResp() As Boolean
        Get
            Return mDialogResp
        End Get
        Set(ByVal value As Boolean)
            mDialogResp = value
        End Set
    End Property

    Property modo() As String
        Get
            Return mModo
        End Get
        Set(ByVal value As String)
            mModo = value
        End Set
    End Property

#End Region

#Region " Windows Form Designer generated code "
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtNombre As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtcodINEHisto As System.Windows.Forms.TextBox
    Friend WithEvents btnEliminar As System.Windows.Forms.Button
    Friend WithEvents txtNGBE As TextBox
    Friend WithEvents lblNGBE As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents txtNGMEP As TextBox
    Friend WithEvents txtObservaciones As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents txtNombreCDD As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents cboTipos As ComboBox
    Friend WithEvents Label8 As Label
    Friend WithEvents txtCentroide As TextBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Label3 As Label
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents Label9 As Label
    Friend WithEvents txtOtrosCentroides As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents Label1 As System.Windows.Forms.Label

    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        'Add any initialization after the InitializeComponent() call
    End Sub
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(altaMuniH))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtNombre = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtcodINEHisto = New System.Windows.Forms.TextBox()
        Me.btnEliminar = New System.Windows.Forms.Button()
        Me.txtNGBE = New System.Windows.Forms.TextBox()
        Me.lblNGBE = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtNGMEP = New System.Windows.Forms.TextBox()
        Me.txtObservaciones = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtNombreCDD = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cboTipos = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtCentroide = New System.Windows.Forms.TextBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtOtrosCentroides = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(34, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(48, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Nombre"
        '
        'txtNombre
        '
        Me.txtNombre.Location = New System.Drawing.Point(84, 16)
        Me.txtNombre.Multiline = True
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(673, 34)
        Me.txtNombre.TabIndex = 1
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.ListBox1)
        Me.GroupBox1.Controls.Add(Me.ListView1)
        Me.GroupBox1.Controls.Add(Me.TextBox2)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Location = New System.Drawing.Point(13, 201)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(491, 240)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Municipios con jurisdicción sobre el territorio"
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(79, 44)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(406, 95)
        Me.ListBox1.Sorted = True
        Me.ListBox1.TabIndex = 2
        '
        'ListView1
        '
        Me.ListView1.CheckBoxes = True
        Me.ListView1.HideSelection = False
        Me.ListView1.Location = New System.Drawing.Point(32, 51)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(453, 183)
        Me.ListView1.TabIndex = 3
        Me.ListView1.UseCompatibleStateImageBehavior = False
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(79, 25)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(406, 22)
        Me.TextBox2.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(29, 28)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Nombre"
        '
        'Button1
        '
        Me.Button1.Image = CType(resources.GetObject("Button1.Image"), System.Drawing.Image)
        Me.Button1.Location = New System.Drawing.Point(962, 470)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(91, 50)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = "Nuevo"
        Me.Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Image = CType(resources.GetObject("Button2.Image"), System.Drawing.Image)
        Me.Button2.Location = New System.Drawing.Point(861, 470)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(91, 50)
        Me.Button2.TabIndex = 4
        Me.Button2.Text = "Cerrar"
        Me.Button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(591, 62)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(73, 13)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "INE Histórico"
        '
        'txtcodINEHisto
        '
        Me.txtcodINEHisto.Location = New System.Drawing.Point(670, 59)
        Me.txtcodINEHisto.Name = "txtcodINEHisto"
        Me.txtcodINEHisto.Size = New System.Drawing.Size(87, 22)
        Me.txtcodINEHisto.TabIndex = 12
        Me.txtcodINEHisto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'btnEliminar
        '
        Me.btnEliminar.Image = CType(resources.GetObject("btnEliminar.Image"), System.Drawing.Image)
        Me.btnEliminar.Location = New System.Drawing.Point(962, 12)
        Me.btnEliminar.Name = "btnEliminar"
        Me.btnEliminar.Size = New System.Drawing.Size(91, 63)
        Me.btnEliminar.TabIndex = 13
        Me.btnEliminar.Text = "Eliminar"
        Me.btnEliminar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnEliminar.UseVisualStyleBackColor = True
        '
        'txtNGBE
        '
        Me.txtNGBE.Location = New System.Drawing.Point(482, 59)
        Me.txtNGBE.Name = "txtNGBE"
        Me.txtNGBE.Size = New System.Drawing.Size(87, 22)
        Me.txtNGBE.TabIndex = 14
        Me.txtNGBE.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblNGBE
        '
        Me.lblNGBE.AutoSize = True
        Me.lblNGBE.Location = New System.Drawing.Point(403, 62)
        Me.lblNGBE.Name = "lblNGBE"
        Me.lblNGBE.Size = New System.Drawing.Size(76, 13)
        Me.lblNGBE.TabIndex = 15
        Me.lblNGBE.Text = "Código NGBE"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(394, 87)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(86, 13)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "Código NGMEP"
        '
        'txtNGMEP
        '
        Me.txtNGMEP.Location = New System.Drawing.Point(482, 84)
        Me.txtNGMEP.Name = "txtNGMEP"
        Me.txtNGMEP.Size = New System.Drawing.Size(87, 22)
        Me.txtNGMEP.TabIndex = 17
        Me.txtNGMEP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtObservaciones
        '
        Me.txtObservaciones.Location = New System.Drawing.Point(6, 19)
        Me.txtObservaciones.Multiline = True
        Me.txtObservaciones.Name = "txtObservaciones"
        Me.txtObservaciones.Size = New System.Drawing.Size(436, 209)
        Me.txtObservaciones.TabIndex = 18
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(10, 126)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(73, 13)
        Me.Label6.TabIndex = 20
        Me.Label6.Text = "Nombre CdD"
        '
        'txtNombreCDD
        '
        Me.txtNombreCDD.Location = New System.Drawing.Point(87, 123)
        Me.txtNombreCDD.Multiline = True
        Me.txtNombreCDD.Name = "txtNombreCDD"
        Me.txtNombreCDD.Size = New System.Drawing.Size(482, 41)
        Me.txtNombreCDD.TabIndex = 21
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(50, 62)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(30, 13)
        Me.Label7.TabIndex = 22
        Me.Label7.Text = "Tipo"
        '
        'cboTipos
        '
        Me.cboTipos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTipos.FormattingEnabled = True
        Me.cboTipos.Location = New System.Drawing.Point(87, 59)
        Me.cboTipos.Name = "cboTipos"
        Me.cboTipos.Size = New System.Drawing.Size(285, 21)
        Me.cboTipos.TabIndex = 23
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(591, 90)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(71, 13)
        Me.Label8.TabIndex = 24
        Me.Label8.Text = "Centroide Id"
        '
        'txtCentroide
        '
        Me.txtCentroide.Location = New System.Drawing.Point(670, 87)
        Me.txtCentroide.Name = "txtCentroide"
        Me.txtCentroide.Size = New System.Drawing.Size(87, 22)
        Me.txtCentroide.TabIndex = 25
        Me.txtCentroide.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtObservaciones)
        Me.GroupBox2.Location = New System.Drawing.Point(510, 201)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(448, 240)
        Me.GroupBox2.TabIndex = 26
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Observaciones"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(14, 453)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(490, 29)
        Me.Label3.TabIndex = 27
        Me.Label3.Text = "Label3"
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(87, 90)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(285, 21)
        Me.ComboBox1.TabIndex = 28
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(27, 93)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(53, 13)
        Me.Label9.TabIndex = 29
        Me.Label9.Text = "Provincia"
        '
        'txtOtrosCentroides
        '
        Me.txtOtrosCentroides.Location = New System.Drawing.Point(594, 142)
        Me.txtOtrosCentroides.Name = "txtOtrosCentroides"
        Me.txtOtrosCentroides.Size = New System.Drawing.Size(163, 22)
        Me.txtOtrosCentroides.TabIndex = 30
        Me.txtOtrosCentroides.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(593, 123)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(151, 13)
        Me.Label10.TabIndex = 31
        Me.Label10.Text = "Otros centroides vinculados"
        '
        'altaMuniH
        '
        Me.ClientSize = New System.Drawing.Size(1065, 532)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.txtOtrosCentroides)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.txtCentroide)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.cboTipos)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtNombreCDD)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtNGMEP)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lblNGBE)
        Me.Controls.Add(Me.txtNGBE)
        Me.Controls.Add(Me.btnEliminar)
        Me.Controls.Add(Me.txtcodINEHisto)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.txtNombre)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "altaMuniH"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Alta de territorios históricos"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region


    Public Shared Function muniHNewDialog() As Boolean

        Dim dlg As New altaMuniH

        dlg.modo = "NEW"
        dlg.formatearControles()
        dlg.Text = "Alta de territorio histórico"
        dlg.ShowDialog()
        Return dlg.dialogResp

    End Function

    Public Shared Function muniHEditorDialog(ByVal muniH As MuniHisto) As Boolean

        Dim dlg As New altaMuniH
        dlg.muniHEdit = muniH
        dlg.formatearControles()
        dlg.rellenarControles()
        dlg.modo = "EDIT"
        dlg.Text = $"Edición de territorio histórico"
        dlg.ShowDialog()
        Return dlg.dialogResp

    End Function

    Private Sub altaTerritorio_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ListView1.Columns.Add("Nombre", 350, HorizontalAlignment.Left)
        ListView1.Columns.Add("INE", 50, HorizontalAlignment.Left)
        ListView1.FullRowSelect = True
        ListView1.GridLines = True
        ListView1.View = View.Details

        ListBox1.Location = New Point(79, 44)
        ListBox1.Size = New Point(228, TextBox2.Size.Width)
        ListBox1.Visible = False
        BuscarNomen = True

        Label3.Text = "Debe seleccionarse al menos un municipio principal, cuyo código INE coincida con el código INE Histórico en sus cinco primeros dígitos"


    End Sub
    Private Sub formatearControles()

        txtNGBE.Text = "0"
        txtNGMEP.Text = "0"
        txtCentroide.Text = "0"
        For Each tipo As String In TipoTerritoriosEditables
            cboTipos.Items.Add(tipo)
        Next
        For Each Provincia As DataRow In ListaProvincias.Select
            ComboBox1.Items.Add(New itemData(Provincia.ItemArray(1).ToString, Provincia.ItemArray(0)))
        Next

    End Sub
    Private Sub rellenarControles()

        Dim elementoLV As ListViewItem

        txtNombre.Text = territorioHistorico.nombreMuniHisto

        txtNGBE.Text = territorioHistorico.ngbe_id
        txtNGMEP.Text = territorioHistorico.ngmep_id
        txtcodINEHisto.Text = String.Format("{0:0000000}", territorioHistorico.codineMuniHisto)
        txtObservaciones.Text = territorioHistorico.observaciones
        txtNombreCDD.Text = territorioHistorico.nombre4CdD
        txtCentroide.Text = territorioHistorico.centroide_id
        txtOtrosCentroides.Text = territorioHistorico.otros_centroides_id

        Dim iTipo As Integer = -1
        For Each tipo As String In TipoTerritoriosEditables
            iTipo += 1
            If territorioHistorico.tipo = tipo Then
                cboTipos.SelectedIndex = iTipo
                Exit For
            End If
        Next

        For iBucle As Integer = 0 To ComboBox1.Items.Count - 1
            If territorioHistorico.provinciaINE = CType(ComboBox1.Items(iBucle), itemData).Valor Then
                ComboBox1.SelectedItem = ComboBox1.Items(iBucle)
            End If
        Next




        For Each juris As String In territorioHistorico.lstJurisdiccion
            Dim tmpTrozos() As String = juris.Split("|")
            If tmpTrozos.Length = 4 Then
                elementoLV = New ListViewItem
                elementoLV.Text = tmpTrozos(2)
                elementoLV.SubItems.Add(tmpTrozos(1))
                elementoLV.Tag = tmpTrozos(3)
                elementoLV.Checked = IIf(tmpTrozos(1) = txtcodINEHisto.Text.Substring(0, 5), True, False)
                ListView1.Items.Add(elementoLV)
                elementoLV = Nothing
                End If
        Next

        Button1.Text = "Guardar"
        variacionMunis = False

        If Not TipoTerritoriosEditables.Contains(territorioHistorico.tipo, StringComparer.CurrentCultureIgnoreCase) Then 'case insensitive
            Button1.Enabled = False
            btnEliminar.Enabled = False
            ModalExclamation("Este territorio no es editable en CartoSEE")
            Exit Sub
        End If



    End Sub

#Region "Selección de municipios asociados"

    Private Sub TextBox2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox2.Click
        If TextBox2.Text.Trim <> "" Then
            TextBox2.SelectAll()
        End If
    End Sub

    Private Sub TextBox2_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox2.KeyUp
        If e.KeyData = Keys.Down And ListBox1.Visible = True Then
            ListBox1.Focus()
            ListBox1.SelectedIndex = 0
        End If
    End Sub


    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        If BuscarNomen = False Then
            BuscarNomen = True
            Exit Sub
        End If
        If TextBox2.Text.Length > 1 Then
            ListBox1.Visible = True
            RellenarListBox(TextBox2.Text.Trim)
            If ListBox1.Items.Count = 1 Then
                ListBox1.SelectedIndex = 0
                SeleccionarMuni(sender, e)
                BuscarNomen = False
                TextBox2.Text = ListBox1.SelectedItem.ToString
            End If
        Else
            ListBox1.Visible = False
        End If
    End Sub

    Private Sub RellenarListBox(ByVal Discri As String)

        Dim ListaNom As DataTable
        Dim filas() As DataRow

        ListaNom = New DataTable
        If CargarRecordset("SELECT identidad,nombre as nombremunicipio, inecorto as codine, codigoprov " &
                     "FROM ngmepschema.listamunicipios " &
                     "WHERE UPPER(translate(nombre, 'áéíóúÁÉÍÓÚ', 'aeiouAEIOU')) " &
                     "LIKE UPPER(translate('%" & Discri & "%', 'áéíóúÁÉÍÓÚ', 'aeiouAEIOU')) limit 40", ListaNom) = True Then
            If ListaNom.Rows.Count = 0 Then Exit Sub
            filas = ListaNom.Select
            ListBox1.Items.Clear()
            For Each dR As DataRow In filas
                ListBox1.Items.Add(New itemData(dR("nombremunicipio").ToString, dR("identidad") & "|" & dR("nombremunicipio").ToString & "|" & dR("codine")))
            Next
            filas = Nothing
            ListaNom.Dispose()
            ListaNom = Nothing
        End If


    End Sub

    Private Sub ListBox1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ListBox1.KeyUp

        If e.KeyData = Keys.Up And ListBox1.SelectedIndex = 0 Then
            TextBox2.Focus()
        ElseIf e.KeyData = Keys.Return And ListBox1.SelectedIndex >= 0 Then
            SeleccionarMuni(sender, e)
            BuscarNomen = False
            TextBox2.Text = ListBox1.SelectedItem.ToString
        End If

    End Sub

    Private Sub ListView1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ListView1.KeyUp

        If ListView1.SelectedItems.Count = 0 Then Exit Sub
        If e.KeyData = Keys.Delete Then
            For iBucle As Integer = ListView1.Items.Count - 1 To 0 Step -1
                If ListView1.Items(iBucle).Selected = True Then ListView1.Items(iBucle).Remove()
            Next
            variacionMunis = True
        End If

    End Sub

    Private Sub SeleccionarMuni(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.Click, ListBox1.DoubleClick

        Dim elementoLV As ListViewItem
        Dim params() As String

        If ListBox1.SelectedIndex = -1 Then Exit Sub
        TextBox2.Text = ListBox1.SelectedItem.ToString
        TextBox2.Tag = CType(ListBox1.Items(ListBox1.SelectedIndex), itemData).Valor
        ListBox1.Visible = False

        params = TextBox2.Tag.ToString.Split("|")
        For Each elem As ListViewItem In ListView1.Items
            If elem.Tag = params(0) Then Exit Sub
        Next
        If params.Length = 3 Then
            elementoLV = New ListViewItem
            elementoLV.Text = params(1)
            elementoLV.SubItems.Add(String.Format("{0:00000}", CType(params(2), Integer)))
            elementoLV.Tag = params(0)
            ListView1.Items.Add(elementoLV)
            elementoLV = Nothing
            variacionMunis = True
        End If
        Application.DoEvents()

    End Sub

#End Region

    '----------------------------------------------------------------------------------------
    'Acciones
    '----------------------------------------------------------------------------------------

    Private Sub ejecutarAccion(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click, Button2.Click, btnEliminar.Click

        If sender.name = "Button1" Then
            If mModo = "NEW" Then
                crearNewMuniH()
            ElseIf mModo = "EDIT" Then
                actualizarTerri()
            End If
        ElseIf sender.name = "btnEliminar" Then

            Try
                Dim numDocs As Integer = 0

                ObtenerEscalar($"SELECT count(*) FROM bdsidschema.archivo2territorios where territorio_id={muniHEdit.indice}", numDocs)
                If numDocs > 0 Then
                    ModalExclamation($"Hay documentos {numDocs} de CartoSEE asociados a este municipio. Elimine estas asociaciones antes de borrarlo.")
                    Exit Sub
                End If

                numDocs = 0
                ObtenerEscalar($"SELECT count(*) FROM deslinschema.citaciones2territorios where territorio_id={muniHEdit.indice}", numDocs)
                If numDocs > 0 Then
                    ModalExclamation($"Hay documentos {numDocs} de citaciones de SIDDES asociados a este territorio.{Environment.NewLine}Elimine estas asociaciones antes de borrarlo.")
                    Exit Sub
                End If

                numDocs = 0
                ObtenerEscalar($"SELECT count(*) FROM deslinschema.correspondencia2territorios where territorio_id={muniHEdit.indice}", numDocs)
                If numDocs > 0 Then
                    ModalExclamation($"Hay documentos {numDocs} de correspondencias de SIDDES asociadas a este territorio.{Environment.NewLine}Elimine estas asociaciones antes de borrarlo.")
                    Exit Sub
                End If

                numDocs = 0
                ObtenerEscalar($"SELECT count(*) FROM deslinschema.cuadernos2territorios where territorio_id={muniHEdit.indice}", numDocs)
                If numDocs > 0 Then
                    ModalExclamation($"Hay documentos {numDocs} de cuadernos de SIDDES asociadas a este territorio.{Environment.NewLine}Elimine estas asociaciones antes de borrarlo.")
                    Exit Sub
                End If

                numDocs = 0
                ObtenerEscalar($"SELECT count(*) FROM deslinschema.expedientes2territorios where territorio_id={muniHEdit.indice}", numDocs)
                If numDocs > 0 Then
                    ModalExclamation($"Hay documentos {numDocs} de expedientes de SIDDES asociadas a este territorio.{Environment.NewLine}Elimine estas asociaciones antes de borrarlo.")
                    Exit Sub
                End If

                numDocs = 0
                ObtenerEscalar($"SELECT count(*) FROM deslinschema.inventario2terri where terri_id_a={muniHEdit.indice} or terri_id_b={muniHEdit.indice}", numDocs)
                If numDocs > 0 Then
                    ModalExclamation($"Hay documentos {numDocs} de inventario de SIDDES asociadas a este territorio.{Environment.NewLine}Elimine estas asociaciones antes de borrarlo.")
                    Exit Sub
                End If

            Catch ex As Exception
                ModalError(ex.Message)
                Exit Sub
            End Try

            BorrarMuniHisto()
        End If
        'Si cerramos aquí, cada vez que hay un fallo obligamos al usuario a meter de nuevo todos los datos
        'Me.Close()

    End Sub

    '----------------------------------------------------------------------------------------
    'Procedimientos de edición en la base de datos
    '----------------------------------------------------------------------------------------

    Private Sub crearNewMuniH()

        Dim listaSQL As ArrayList
        Dim cadpertenencia As String
        Dim codProv As Integer
        Dim codProvINE As String
        Dim codMuniActual As Integer
        Dim codigoINEHisto As String
        Dim renombrarDirectorios As Boolean = False
        Dim codNGMEP As Integer = 0
        Dim codMuniNGMEP As Integer = 0
        Dim codNGBE As Integer = 0
        Dim idCentroide As Integer = 0
        Dim otros_idCentroide As String = ""
        Dim numCoincidentes As Integer = 0
        Dim nombreShow As String = ""

        'Validamos
        If txtNombre.Text.Trim = "" Then
            ModalExclamation("Introduzca el nombre del territorio")
            Exit Sub
        End If
        If ListView1.Items.Count = 0 Then
            ModalExclamation("El territorio debe asociarse al menos a un municipio")
            Exit Sub
        End If

        If cboTipos.Text = "Municipio histórico" Then
            If ListView1.Items.Count <> 1 Then
                ModalExclamation("El municipio histórico debe asociarse a un único municipio")
                Exit Sub
            End If
        End If

        If ListView1.CheckedItems.Count <> 1 Then
            ModalExclamation("Debe marcarse un municipio principal")
            Exit Sub
        End If

        If txtcodINEHisto.Text.Trim.Length <> 7 Then
            ModalExclamation("El código histórico tiene siete dígitos")
            Exit Sub
        End If
        If Not IsNumeric(txtNGBE.Text) Then
            ModalExclamation("El código NGBE tiene que ser numérico")
            Exit Sub
        End If

        If cboTipos.SelectedIndex = -1 Then
            ModalExclamation("Indique el tipo de territorio")
            Exit Sub
        End If

        If ComboBox1.SelectedIndex = -1 Then
            ModalExclamation("Indique la provincia del territorio")
            Exit Sub
        End If

        Try
            codProv = CType(ComboBox1.SelectedItem, itemData).Valor
            codProvINE = String.Format("{0:00}", codProv)
            codigoINEHisto = txtcodINEHisto.Text.Trim
            If codProvINE <> codigoINEHisto.Substring(0, 2) Then
                ModalExclamation("El código INE Histórico no pertenece a la provincia seleccionada")
                Exit Sub
            End If
            codProv = CType(codigoINEHisto.Substring(0, 2), Integer)
            codMuniActual = CType(codigoINEHisto.Substring(0, 5), Integer)
            For Each item As ListViewItem In ListView1.Items
                If cadpertenencia = "" Then
                    cadpertenencia = item.SubItems(1).Text
                    Continue For
                End If
                cadpertenencia &= $",{item.SubItems(1).Text}"
                If item.Checked Then
                    If codigoINEHisto.Substring(0, 5) <> item.SubItems(1).Text Then
                        ModalExclamation("El código INE del municipio principal debe coincidir con los 5 primeros dígitos del código INE histórico")
                        Exit Sub
                    End If
                    codMuniNGMEP = CType(item.Tag.ToString, Integer)
                End If
            Next
            codNGMEP = CType(txtNGMEP.Text.Trim, Integer)
            codNGBE = CType(txtNGBE.Text.Trim, Integer)
            idCentroide = CType(txtCentroide.Text.Trim, Integer)
            otros_idCentroide = txtOtrosCentroides.Text.Trim
        Catch ex As Exception
                ModalError(ex.Message)
        End Try

        'Comprobamos que el código histórico no se encuentre asignado
        ObtenerEscalar($"SELECT idterritorio FROM bdsidschema.territorios where to_char(munihisto, 'FM0000000'::text)='{codigoINEHisto}'", numCoincidentes)
        If numCoincidentes > 0 Then
            ModalExclamation($"El código histórico {codigoINEHisto} ya se encuentra asignado")
            Exit Sub
        End If

        If ModalQuestion("Desea crear el nuevo municipio histórico") = DialogResult.No Then Exit Sub

        listaSQL = New ArrayList
        listaSQL.Add($"INSERT INTO bdsidschema.territorios(nombre,provincia,municipio,munihisto,tipo,pertenencia,poligono_carto,nombremostrado,observaciones,
                        nomen_id,nombrecdd,ngbe_id,otros_centroid_id,ngmep_muni_id)  VALUES (
                        E'{txtNombre.Text.Trim.Replace("'", "\'")}',{codProv},{codMuniActual},{codigoINEHisto},'{cboTipos.Text}','{cadpertenencia}',
                        {idCentroide},E'{nombreShow.Replace("'", "\'")}',E'{txtObservaciones.Text.Trim.Replace("'", "\'")}',{codNGMEP},E'{txtNombreCDD.Text.Replace("'", "\'")}',
                        {codNGBE},{IIf(otros_idCentroide = "", "Null", $"'{otros_idCentroide}'")},{codMuniNGMEP})")

        Me.Cursor = Cursors.WaitCursor
        Me.dialogResp = ExeTran(listaSQL)
        registrarDatabaseLog($"Creado municipiohistórico {txtNombre.Text.Trim.Replace("'", "\'")} con INE {codigoINEHisto}")
        Me.Cursor = Cursors.Default
        Me.Close()

    End Sub

    Private Sub actualizarTerri()

        Dim listaSQL As ArrayList
        Dim cadpertenencia As String = ""
        Dim codProv As Integer
        Dim codMuniActual As Integer
        Dim codigoINEHisto As String
        Dim renombrarDirectorios As Boolean = False
        Dim codNGMEP As Integer = 0
        Dim codMuniNGMEP As Integer = 0
        Dim codNGBE As Integer = 0
        Dim idCentroide As Integer = 0
        Dim otros_idCentroide As String = ""
        Dim codprovINE As String = ""

        'Validamos
        If txtNombre.Text.Trim = "" Then
            ModalExclamation("Introduzca el nombre del territorio")
            Exit Sub
        End If
        If ListView1.Items.Count = 0 Then
            ModalExclamation("El territorio debe asociarse al menos a un municipio")
            Exit Sub
        End If

        If cboTipos.Text = "Municipio histórico" Then
            If ListView1.Items.Count <> 1 Then
                ModalExclamation("El municipio histórico debe asociarse a un único municipio")
                Exit Sub
            End If
        End If

        If ListView1.CheckedItems.Count <> 1 Then
            ModalExclamation("Debe marcarse un único municipio principal")
            Exit Sub
        End If

        If txtcodINEHisto.Text.Trim.Length <> 7 Then
            ModalExclamation("El código histórico tiene siete dígitos")
            Exit Sub
        End If
        If Not IsNumeric(txtNGBE.Text) Then
            ModalExclamation("El código NGBE tiene que ser numérico")
            Exit Sub
        End If

        If ComboBox1.SelectedIndex = -1 Then
            ModalExclamation("Indique la provincia del territorio")
            Exit Sub
        End If

        Try
            codProv = CType(ComboBox1.SelectedItem, itemData).Valor
            codProvINE = String.Format("{0:00}", codProv)
            codigoINEHisto = txtcodINEHisto.Text.Trim
            codMuniActual = CType(codigoINEHisto.Substring(0, 5), Integer)
            For Each item As ListViewItem In ListView1.Items
                If cadpertenencia = "" Then
                    cadpertenencia = item.SubItems(1).Text
                    Continue For
                End If
                cadpertenencia &= $",{item.SubItems(1).Text}"
                If item.Checked Then
                    If codigoINEHisto.Substring(0, 5) <> item.SubItems(1).Text Then
                        ModalExclamation("El código INE del municipio principal debe coincidir con los 5 primeros dígitos del código INE histórico")
                        Exit Sub
                    End If
                    codMuniNGMEP = CType(item.Tag.ToString, Integer)
                End If
            Next
            codNGMEP = CType(txtNGMEP.Text.Trim, Integer)
            codNGBE = CType(txtNGBE.Text.Trim, Integer)
            idCentroide = CType(txtCentroide.Text.Trim, Integer)
            otros_idCentroide = txtOtrosCentroides.Text.Trim
        Catch ex As Exception
            ModalError(ex.Message)
            Exit Sub
        End Try

        listaSQL = New ArrayList
        If muniHEdit.nombreMuniHisto <> txtNombre.Text.Trim Then
            listaSQL.Add($"UPDATE bdsidschema.territorios SET nombre=E'{txtNombre.Text.Replace("'", "\'").Trim}' WHERE idterritorio={muniHEdit.indice}")
        End If
        If muniHEdit.nombre4CdD <> txtNombreCDD.Text.Trim Then
            listaSQL.Add($"UPDATE bdsidschema.territorios SET nombrecdd=E'{txtNombreCDD.Text.Replace("'", "\'").Trim}' WHERE idterritorio={muniHEdit.indice}")
        End If
        If muniHEdit.ngbe_id <> codNGBE Then
            listaSQL.Add($"UPDATE bdsidschema.territorios SET ngbe_id={codNGBE} WHERE idterritorio={muniHEdit.indice}")
        End If
        If muniHEdit.ngmep_id <> codNGMEP Then
            listaSQL.Add($"UPDATE bdsidschema.territorios SET nomen_id={codNGMEP} WHERE idterritorio={muniHEdit.indice}")
        End If
        If muniHEdit.ngmep_muni_id <> codMuniNGMEP Then
            listaSQL.Add($"UPDATE bdsidschema.territorios SET ngmep_muni_id={codNGMEP} WHERE idterritorio={muniHEdit.indice}")
        End If

        If muniHEdit.codineMuniHisto <> codigoINEHisto Then
            If muniHEdit.codineMuniHisto.ToString.Substring(0, 2) = "28" Then
                ModalExclamation("El cambio de INE puede afectar al funcionamiento del SIDCECA. Contacte antes con el administrador")
                Exit Sub
            End If
            listaSQL.Add($"UPDATE bdsidschema.territorios SET munihisto={codigoINEHisto} WHERE idterritorio={muniHEdit.indice}")
            listaSQL.Add($"UPDATE bdsidschema.territorios SET municipio={codMuniActual} WHERE idterritorio={muniHEdit.indice}")
            listaSQL.Add($"UPDATE bdsidschema.territorios SET provincia={codProv} WHERE idterritorio={muniHEdit.indice}")
            renombrarDirectorios = True
        End If

        If muniHEdit.observaciones <> txtObservaciones.Text.Trim Then
            listaSQL.Add($"UPDATE bdsidschema.territorios SET observaciones=E'{txtObservaciones.Text.Replace("'", "\'").Trim}' WHERE idterritorio={muniHEdit.indice}")
        End If

        If muniHEdit.centroide_id <> idCentroide Then
            listaSQL.Add($"UPDATE bdsidschema.territorios SET poligono_carto={idCentroide} WHERE idterritorio={muniHEdit.indice}")
        End If
        If muniHEdit.otros_centroides_id <> otros_idCentroide Then
            listaSQL.Add($"UPDATE bdsidschema.territorios SET otros_centroid_id={IIf(otros_idCentroide = "", "Null", $"'{otros_idCentroide}'")} WHERE idterritorio={muniHEdit.indice}")
        End If


        If variacionMunis Then
            listaSQL.Add($"UPDATE bdsidschema.territorios SET pertenencia='{cadpertenencia}' WHERE idterritorio={muniHEdit.indice}")
        End If

        If listaSQL.Count > 0 Then
            Me.Cursor = Cursors.WaitCursor

            If ExeTran(listaSQL) Then
                registrarDatabaseLog($"Modificado municipiohistórico {txtNombre.Text.Trim.Replace("'", "\'")} con INE {codigoINEHisto}")
                GenerarLOG("Modificaciones en " & muniHEdit.nombreMuniHisto)
                Me.dialogResp = True
                If renombrarDirectorios = True Then
                    If renombrarDirs(muniHEdit.codineMuniHisto, codigoINEHisto) = False Then
                        ModalExclamation("No se pudieron renombrar los directorios del municipio.Complete la operación manualmente")
                    End If
                End If
                Me.Cursor = Cursors.Default
                Me.Close()
            Else
                ModalInfo("No se modificó ningún atributo")
                Me.dialogResp = False
            End If
        Else
            ModalInfo("No se modificó ningún atributo")
            Me.dialogResp = False
        End If


    End Sub

    Private Sub BorrarMuniHisto()

        If ModalQuestion("Confirma el borrado del territorio histórico?") = DialogResult.No Then Exit Sub

        Dim listaSQL As New ArrayList
        GenerarLOG($"DELETE FROM bdsidschema.munihisto WHERE idmunihisto={muniHEdit.indice}. Eliminado {muniHEdit.nombreMuniHisto}")
        listaSQL.Add($"DELETE FROM bdsidschema.territorios WHERE idterritorio={muniHEdit.indice}")

        Me.Cursor = Cursors.WaitCursor
        Me.dialogResp = ExeTran(listaSQL)
        Me.Cursor = Cursors.Default

    End Sub

    Function renombrarDirs(ByVal codOLD As Integer, ByVal codNEW As String) As Boolean

        Dim Antiguo As String
        Dim Nuevo As String
        Dim rutaBase As String
        Dim rutaNueva As String
        Dim rcdTipos As DataTable
        Dim listTipos() As DataRow

        Antiguo = String.Format("{0:0000000}", codOLD)
        Nuevo = codNEW
        If Antiguo.Length <> 7 Or Nuevo.Length <> 7 Then Return False
        If Antiguo.Substring(0, 2) = "28" Then
            ModalExclamation("El cambio de INE puede afectar al funcionamiento del SIDCECA. Contacte antes con el administrador")
            Return False
        End If

        rcdTipos = New DataTable
        If CargarDatatable("Select idtipodoc,tipodoc from bdsidschema.tbtipodocumento", rcdTipos) = False Then
            Return False
            rcdTipos = Nothing
        End If
        listTipos = rcdTipos.Select
        rcdTipos.Dispose()
        rcdTipos = Nothing

        For Each tipo As DataRow In listTipos
            rutaBase = rutaRepoGeorref & "\" & DirRepoProvinciaByTipodoc(tipo.Item("idtipodoc")) & "\" & Antiguo.Substring(0, 2) & "\" & Antiguo
            rutaNueva = rutaRepoGeorref & "\" & DirRepoProvinciaByTipodoc(tipo.Item("idtipodoc")) & "\" & Nuevo.Substring(0, 2) & "\" & Nuevo
            Try
                If System.IO.Directory.Exists(RutaBase) Then
                    System.IO.Directory.Move(rutaBase, rutaNueva)
                    GenerarLOG("Renombrado directorio: " & rutaBase & " >>> " & rutaNueva)
                End If
            Catch ex As Exception
                GenerarLOG("Error al renombrar directorio: " & rutaBase & " >>> " & rutaNueva & "." & ex.Message)
                Return False
            End Try
        Next

        Return True


    End Function



End Class
