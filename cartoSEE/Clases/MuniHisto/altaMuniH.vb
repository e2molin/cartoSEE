Public Class altaMuniH
    Inherits System.Windows.Forms.Form

    Dim BuscarNomen As Boolean
    Dim variacionMunis As Boolean
    Dim mDialogResp As Boolean = False
    Dim mModo As String
    Dim mMuniHEdit As MuniHisto


#Region "Definición propiedades"

    Property muniHEdit() As MuniHisto
        Get
            Return mMuniHEdit
        End Get
        Set(ByVal value As MuniHisto)
            mMuniHEdit = value
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
    Friend WithEvents txtcodINE As System.Windows.Forms.TextBox
    Friend WithEvents btnEliminar As System.Windows.Forms.Button
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
        Me.txtcodINE = New System.Windows.Forms.TextBox()
        Me.btnEliminar = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(34, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(44, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Nombre"
        '
        'txtNombre
        '
        Me.txtNombre.Location = New System.Drawing.Point(84, 16)
        Me.txtNombre.Multiline = True
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(325, 34)
        Me.txtNombre.TabIndex = 1
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.ListBox1)
        Me.GroupBox1.Controls.Add(Me.ListView1)
        Me.GroupBox1.Controls.Add(Me.TextBox2)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Location = New System.Drawing.Point(84, 110)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(325, 157)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Municipios con jurisdicción sobre el territorio"
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(79, 44)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(228, 95)
        Me.ListBox1.Sorted = True
        Me.ListBox1.TabIndex = 2
        '
        'ListView1
        '
        Me.ListView1.Location = New System.Drawing.Point(32, 67)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(275, 81)
        Me.ListView1.TabIndex = 3
        Me.ListView1.UseCompatibleStateImageBehavior = False
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(79, 25)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(228, 20)
        Me.TextBox2.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(29, 28)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(44, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Nombre"
        '
        'Button1
        '
        Me.Button1.Image = CType(resources.GetObject("Button1.Image"), System.Drawing.Image)
        Me.Button1.Location = New System.Drawing.Point(424, 238)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(91, 30)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = "Nuevo"
        Me.Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Image = CType(resources.GetObject("Button2.Image"), System.Drawing.Image)
        Me.Button2.Location = New System.Drawing.Point(424, 202)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(91, 30)
        Me.Button2.TabIndex = 4
        Me.Button2.Text = "Cerrar"
        Me.Button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(17, 59)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(61, 13)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "Código INE"
        '
        'txtcodINE
        '
        Me.txtcodINE.Location = New System.Drawing.Point(84, 56)
        Me.txtcodINE.Name = "txtcodINE"
        Me.txtcodINE.Size = New System.Drawing.Size(78, 20)
        Me.txtcodINE.TabIndex = 12
        '
        'btnEliminar
        '
        Me.btnEliminar.Image = CType(resources.GetObject("btnEliminar.Image"), System.Drawing.Image)
        Me.btnEliminar.Location = New System.Drawing.Point(424, 16)
        Me.btnEliminar.Name = "btnEliminar"
        Me.btnEliminar.Size = New System.Drawing.Size(91, 34)
        Me.btnEliminar.TabIndex = 13
        Me.btnEliminar.Text = "Eliminar"
        Me.btnEliminar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnEliminar.UseVisualStyleBackColor = True
        '
        'altaMuniH
        '
        Me.ClientSize = New System.Drawing.Size(522, 284)
        Me.Controls.Add(Me.btnEliminar)
        Me.Controls.Add(Me.txtcodINE)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.txtNombre)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "altaMuniH"
        Me.Text = "Alta de territorios históricos"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region


    Public Shared Function muniHNewDialog() As Boolean

        Dim dlg As New altaMuniH

        dlg.modo = "NEW"
        dlg.Text = "Alta de Municipio histórico"
        dlg.ShowDialog()
        Return dlg.dialogResp

    End Function

    Public Shared Function muniHEditorDialog(ByVal muniH As MuniHisto) As Boolean

        Dim dlg As New altaMuniH
        dlg.muniHEdit = muniH
        dlg.rellenarControles()

        dlg.modo = "EDIT"
        dlg.Text = "Edición de territorio histórico"
        dlg.ShowDialog()
        Return dlg.dialogResp

    End Function

    Private Sub altaTerritorio_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ListView1.Columns.Add("Nombre", 200, HorizontalAlignment.Left)
        ListView1.Columns.Add("INE", 55, HorizontalAlignment.Left)
        ListView1.FullRowSelect = True
        ListView1.GridLines = True
        ListView1.View = View.Details

        ListBox1.Location = New Point(79, 44)
        ListBox1.Size = New Point(228, 95)
        ListBox1.Visible = False
        BuscarNomen = True

    End Sub

    Private Sub rellenarControles()

        Dim elementoLV As ListViewItem

        txtNombre.Text = mMuniHEdit.nombreMuniHisto
        txtcodINE.Text = String.Format("{0:0000000}", mMuniHEdit.codineMuniHisto)
        elementoLV = New ListViewItem
        elementoLV.Text = mMuniHEdit.nombreMuniActual
        elementoLV.SubItems.Add(String.Format("{0:00000}", mMuniHEdit.codineMuniActual))
        elementoLV.Tag = mMuniHEdit.ngmep_id
        ListView1.Items.Add(elementoLV)
        elementoLV = Nothing

        Button1.Text = "Guardar"
        variacionMunis = False

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
        If CargarRecordset("SELECT identidad,nombre as nombremunicipio, inecorto as codine, codigoprov " & _
                     "FROM ngmepschema.listamunicipios " & _
                     "WHERE UPPER(translate(nombre, 'áéíóúÁÉÍÓÚ', 'aeiouAEIOU')) " & _
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

    Private Sub ejecutarAccion(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                                                                        Handles Button1.Click, Button2.Click, btnEliminar.Click

        If sender.name = "Button1" Then
            If mModo = "NEW" Then
                crearNewMuniH()
            ElseIf mModo = "EDIT" Then
                actualizarTerri()
            End If
        ElseIf sender.name = "btnEliminar" Then
            BorrarMuniHisto()
        End If
        Me.Close()

    End Sub

    '----------------------------------------------------------------------------------------
    'Procedimientos de edición en la base de datos
    '----------------------------------------------------------------------------------------

    Private Sub crearNewMuniH()

        Dim listaSQL As ArrayList
        Dim cadpertenencia As Integer
        Dim codProv As Integer
        Dim codMuni As Integer
        Dim idMuni As Integer
        Dim codigoINEHisto As String
        Dim resultado As String = ""

        'Validamos
        If txtNombre.Text.Trim = "" Then
            MessageBox.Show("Introduzca el nombre del territorio", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        If ListView1.Items.Count <> 1 Then
            MessageBox.Show("El municipio histórico debe asociarse a un municipio", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        If txtcodINE.Text.Trim.Length <> 7 Then
            MessageBox.Show("El código histórico tiene siete dígitos", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        codigoINEHisto = txtcodINE.Text.Trim
        codprov = CType(codigoINEHisto.Substring(0, 2), Integer)
        codMuni = CType(codigoINEHisto.Substring(0, 5), Integer)
        cadpertenencia = CType(ListView1.Items(0).SubItems(1).Text, Integer)
        idMuni = CType(ListView1.Items(0).Tag.ToString, Integer)

        If cadpertenencia <> codMuni Then
            MessageBox.Show("Los primeros 5 dígitos del código histórico deben coincidir con los del municipio al que pertenece", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        'Comprobamos que el código histórico no se encuentre asignado
        ObtenerEscalar("SELECT idmunihisto FROM bdsidschema.munihisto where cod_munihisto='" & codigoINEHisto & "'", resultado)
        If resultado <> "" Then
            MessageBox.Show("El código histórico ya se encuentra asignado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If MessageBox.Show("Desea crear el nuevo municipio histórico", AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then Exit Sub
        listaSQL = New ArrayList

        listaSQL.Add("INSERT INTO bdsidschema.munihisto(nombremunicipiohistorico,provincia_id,cod_muni,cod_munihisto,entidad_id) " &
                    " VALUES (E'" & txtNombre.Text.Trim.Replace("'", "\'") & "'," &
                    "" & codProv & "," &
                    "" & codMuni & "," &
                    "" & codigoINEHisto & "," &
                    "" & idMuni & ")")

        Me.Cursor = Cursors.WaitCursor
        Me.dialogResp = ExeTran(listaSQL)
        Me.Cursor = Cursors.Default

    End Sub

    Private Sub actualizarTerri()

        Dim listaSQL As ArrayList
        Dim cadpertenencia As Integer
        Dim codProv As Integer
        Dim codMuniActual As Integer
        Dim idMuni As Integer
        Dim codigoINEHisto As String
        Dim resultado As String = ""
        Dim renombrarDirectorios As Boolean = False

        'Validamos
        If txtNombre.Text.Trim = "" Then
            MessageBox.Show("Introduzca el nombre del territorio", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        If ListView1.Items.Count <> 1 Then
            MessageBox.Show("El municipio histórico debe asociarse a un municipio", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        If txtcodINE.Text.Trim.Length <> 7 Then
            MessageBox.Show("El código histórico tiene siete dígitos", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        codigoINEHisto = txtcodINE.Text.Trim
        codprov = CType(codigoINEHisto.Substring(0, 2), Integer)
        codMuniActual = CType(ListView1.Items(0).SubItems(1).Text, Integer)
        cadpertenencia = CType(ListView1.Items(0).SubItems(1).Text, Integer)
        idMuni = CType(ListView1.Items(0).Tag.ToString, Integer)

        If codigoINEHisto.Substring(0, 5) <> String.Format("{0:00000}", codMuniActual) Then
            If MessageBox.Show("Los primeros 5 dígitos del código histórico no coinciden con los del municipio al que pertenece. ¿Desea continuar?", _
                               AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then Exit Sub
        End If

        listaSQL = New ArrayList
        If muniHEdit.nombreMuniHisto <> txtNombre.Text.Trim Then
            listaSQL.Add("UPDATE bdsidschema.munihisto SET nombremunicipiohistorico=E'" & txtNombre.Text.Replace("'", "\'").Trim & "' WHERE idmunihisto=" & muniHEdit.indice)
        End If
        If muniHEdit.ngmep_id <> idMuni Then
            listaSQL.Add("UPDATE bdsidschema.munihisto SET entidad_id=" & idMuni & " WHERE idmunihisto=" & muniHEdit.indice)
        End If
        If muniHEdit.codineMuniActual <> codMuniActual Then
            listaSQL.Add("UPDATE bdsidschema.munihisto SET cod_muni=" & codMuniActual & " WHERE idmunihisto=" & muniHEdit.indice)
        End If
        If muniHEdit.codineMuniHisto <> codigoINEHisto Then
            If muniHEdit.codineMuniHisto.ToString.Substring(0, 2) = "28" Then
                MessageBox.Show("El cambio de INE puede afectar al funcionamiento del SIDCECA. Contacte antes con el administrador", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
            listaSQL.Add("UPDATE bdsidschema.munihisto SET cod_munihisto=" & codigoINEHisto & " WHERE idmunihisto=" & muniHEdit.indice)
            renombrarDirectorios = True
        End If
        If muniHEdit.codineMuniHisto <> codigoINEHisto Then
            listaSQL.Add("UPDATE bdsidschema.munihisto SET cod_munihisto=" & codigoINEHisto & " WHERE idmunihisto=" & muniHEdit.indice)
        End If

        Application.DoEvents()
        Me.Cursor = Cursors.WaitCursor
        Me.dialogResp = ExeTran(listaSQL)
        GenerarLOG("Modificaciones en " & muniHEdit.nombreMuniHisto)
        Me.Cursor = Cursors.Default

        If renombrarDirectorios = True Then
            If renombrarDirs(muniHEdit.codineMuniHisto, codigoINEHisto) = False Then
                MessageBox.Show("No se pudieron renombrar los directorios del municipio.Complete la operación manualmente", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End If

    End Sub

    Private Sub BorrarMuniHisto()

        If MessageBox.Show("Confirma borrado del municipio histórico?", AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then Exit Sub

        Dim listaSQL As New ArrayList
        GenerarLOG("DELETE FROM bdsidschema.munihisto WHERE idmunihisto=" & muniHEdit.indice & ". Eliminado " & muniHEdit.nombreMuniHisto)
        listaSQL.Add("DELETE FROM bdsidschema.munihisto WHERE idmunihisto=" & muniHEdit.indice)

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
            MessageBox.Show("El cambio de INE puede afectar al funcionamiento del SIDCECA. Contacte antes con el administrador", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
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
