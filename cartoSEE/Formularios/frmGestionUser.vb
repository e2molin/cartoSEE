Public Class frmGestionUser

    Dim ListaUsuarios() As usuario

    Private Sub frmGestionUser_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Text = "Gestión de usuarios de BADASID"
        ComboBox1.Items.Add(New itemData("Consulta e impresión", 3))
        ComboBox1.Items.Add(New itemData("Administrador y edición", 1))
        ComboBox2.Items.Add(New itemData("Consulta e impresión", 3))
        ComboBox2.Items.Add(New itemData("Administrador y edición", 1))
        ListView1.Location = New Point(16, 22)
        ListView1.Size = New Point(500, 210)
        GroupBox1.Location = New Point(16, 22)
        GroupBox1.Size = New Point(500, 210)
        GroupBox1.Visible = False

        ListView1.FullRowSelect = True
        ListView1.View = View.Details
        ListView1.Columns.Add("Nombre", 90, HorizontalAlignment.Left)
        ListView1.Columns.Add("Apellidos", 120, HorizontalAlignment.Left)
        ListView1.Columns.Add("Teléfono", 70, HorizontalAlignment.Left)
        ListView1.Columns.Add("Usuario", 75, HorizontalAlignment.Left)
        ListView1.Columns.Add("Permiso", 100, HorizontalAlignment.Left)

        CargarUsuarios()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim cadInsert As String
        Dim okValid As Boolean
        Dim CodigoPermiso As Integer
        Dim LoginPass As String = ""


        If TextBox3.Text.Trim = "" Then
            ErrorProvider1.SetError(Me.TextBox3, "El usuario es obligatorio")
        Else
            ErrorProvider1.SetError(Me.TextBox3, "")
            okValid = True
        End If
        If ComboBox1.SelectedIndex = -1 Then
            ErrorProvider1.SetError(Me.ComboBox1, "El permiso es obligatorio")
        Else
            ErrorProvider1.SetError(Me.ComboBox1, "")
            okValid = True
        End If
        If okValid = False Then Exit Sub

        CodigoPermiso = CType(ComboBox1.SelectedItem, itemData).Valor
        If CodigoPermiso = 3 Then
            MessageBox.Show("El usuario de consulta no tiene asignada password", AplicacionTitulo, _
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoginPass = "tera"
        ElseIf CodigoPermiso = 1 Then
            If TextBox4.Text.Trim = "" Then
                ErrorProvider1.SetError(Me.TextBox4, "Asignar una password obligatoria")
            Else
                ErrorProvider1.SetError(Me.TextBox4, "")
                LoginPass = TextBox4.Text.Trim
            End If
        Else
            ErrorProvider1.SetError(Me.ComboBox1, "El permiso es obligatorio")
            Exit Sub
        End If
        If LoginPass = "" Then Exit Sub

        cadInsert = "INSERT INTO usuarios (iduser,nombre,apellidos,telefono,loginuser,loginpass,codigo_permiso) " & _
                    "VALUES (" & _
                    "nextval('usuarios_iduser_seq')," & _
                    "'" & TextBox1.Text.Trim & "'," & _
                    "'" & TextBox2.Text.Trim & "'," & _
                    "'" & TextBox5.Text.Trim & "'," & _
                    "'" & TextBox3.Text.Trim & "'," & _
                    "md5('" & LoginPass & "' || '" & TextBox3.Text.Trim & "' || 'dvmap')," & _
                    CodigoPermiso & ")"

        Dim okProc As Boolean = ExeSinTran(cadInsert)
        If okProc = True Then
            MessageBox.Show("Usuario creado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            CargarUsuarios()
        Else
            MessageBox.Show("No se creo ningún usuario", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub


    Sub CargarUsuarios()

        Dim rcdUsuarios As DataTable
        Dim filas() As DataRow
        Dim contador As Integer

        rcdUsuarios = New DataTable
        If CargarRecordset("SELECT * from usuarios ORDER BY iduser", rcdUsuarios) = True Then
            filas = rcdUsuarios.Select
            ReDim ListaUsuarios(filas.Length - 1)
            contador = -1
            For Each dR As DataRow In filas
                contador = contador + 1
                ListaUsuarios(contador).id = dR("iduser").ToString
                ListaUsuarios(contador).nombre = dR("nombre").ToString
                ListaUsuarios(contador).apellidos = dR("apellidos").ToString
                ListaUsuarios(contador).loginUser = dR("loginuser").ToString
                ListaUsuarios(contador).telefono = dR("telefono").ToString
                ListaUsuarios(contador).codPermiso = dR("codigo_permiso")
            Next
        End If
        rcdUsuarios.Dispose()
        rcdUsuarios = Nothing
        Erase filas
        RellenarLV()

    End Sub

    Private Sub RellenarLV()
        Dim elementoLV As ListViewItem
        Dim contador As Integer
        Dim mostrar As Boolean
        If ListaUsuarios Is Nothing Then Exit Sub
        ListView1.Items.Clear()

        contador = -1
        For Each usu As usuario In ListaUsuarios
            contador = contador + 1
            mostrar = False
            elementoLV = New ListViewItem
            elementoLV.Text = usu.nombre
            elementoLV.SubItems.Add(usu.apellidos)
            elementoLV.SubItems.Add(usu.telefono)
            elementoLV.SubItems.Add(usu.loginUser)
            If usu.codPermiso = 1 Then
                elementoLV.SubItems.Add("Administrador")
            Else
                elementoLV.SubItems.Add("Consulta y lectura")
            End If
            elementoLV.Tag = contador
            If ListView1.Items.Count Mod 2 = 0 Then
                elementoLV.BackColor = Color.White
            Else
                elementoLV.BackColor = Color.WhiteSmoke
            End If
            ListView1.Items.Add(elementoLV)
            elementoLV = Nothing

        Next
        ToolStripStatusLabel1.Text = "Usuarios: " & ListView1.Items.Count

    End Sub

    Private Sub ListView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick

        Dim elementoSel As Integer
        elementoSel = ListView1.SelectedItems(0).Tag
        GroupBox1.Visible = True
        GroupBox1.Text = "Edición usuario"
        TextBox6.Text = ListaUsuarios(elementoSel).nombre
        TextBox7.Text = ListaUsuarios(elementoSel).apellidos
        TextBox8.Text = ListaUsuarios(elementoSel).telefono
        TextBox9.Text = ListaUsuarios(elementoSel).loginUser
        If ListaUsuarios(elementoSel).codPermiso = 3 Then
            ComboBox2.SelectedIndex = 0
        End If
        If ListaUsuarios(elementoSel).codPermiso = 1 Then
            ComboBox2.SelectedIndex = 1
        End If
        GroupBox1.Tag = ListaUsuarios(elementoSel).id

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        GroupBox1.Visible = False
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

        Dim cadUpdate As String
        Dim okValid As Boolean
        Dim CodigoPermiso As Integer
        Dim LoginPass As String = ""


        If TextBox9.Text.Trim = "" Then
            ErrorProvider1.SetError(Me.TextBox9, "El usuario es obligatorio")
        Else
            ErrorProvider1.SetError(Me.TextBox9, "")
            okValid = True
        End If
        If ComboBox2.SelectedIndex = -1 Then
            ErrorProvider1.SetError(Me.ComboBox2, "El permiso es obligatorio")
        Else
            ErrorProvider1.SetError(Me.ComboBox2, "")
            okValid = True
        End If
        If okValid = False Then Exit Sub

        CodigoPermiso = CType(ComboBox2.SelectedItem, itemData).Valor
        If CodigoPermiso = 3 Then
            MessageBox.Show("El usuario de consulta no tiene asignada password", AplicacionTitulo, _
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoginPass = "tera"
        ElseIf CodigoPermiso = 1 Then
            If TextBox10.Text.Trim = "" Then
                ErrorProvider1.SetError(Me.TextBox10, "Asignar una password obligatoria")
            Else
                ErrorProvider1.SetError(Me.TextBox10, "")
                LoginPass = TextBox10.Text.Trim
            End If
        Else
            ErrorProvider1.SetError(Me.ComboBox2, "El permiso es obligatorio")
            Exit Sub
        End If
        If LoginPass = "" Then Exit Sub

        cadUpdate = "UPDATE usuarios SET " & _
                    "nombre='" & TextBox6.Text.Trim & "'," & _
                    "apellidos='" & TextBox7.Text.Trim & "'," & _
                    "telefono='" & TextBox8.Text.Trim & "'," & _
                    "loginuser='" & TextBox9.Text.Trim & "'," & _
                    "loginpass=md5('" & LoginPass & "' || '" & TextBox9.Text.Trim & "' || 'dvmap')," & _
                    "codigo_permiso=" & CodigoPermiso & " " & _
                    "WHERE iduser=" & GroupBox1.Tag.ToString

        Dim okProc As Boolean = ExeSinTran(cadUpdate)
        If okProc = True Then
            MessageBox.Show("Usuario modificado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            CargarUsuarios()
        Else
            MessageBox.Show("No se modificó ningún usuario", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click

        Dim cadDelete As String

        If MessageBox.Show("¿Desea borrar el usuario?", _
                        AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = _
                        Windows.Forms.DialogResult.No Then Exit Sub

        cadDelete = "DELETE FROM usuarios WHERE iduser=" & GroupBox1.Tag.ToString
        Dim okProc As Boolean = ExeSinTran(cadDelete)
        If okProc = True Then
            MessageBox.Show("Usuario eliminado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            CargarUsuarios()
            GroupBox1.Visible = False
        Else
            MessageBox.Show("No se eliminó ningún usuario", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If


    End Sub
End Class