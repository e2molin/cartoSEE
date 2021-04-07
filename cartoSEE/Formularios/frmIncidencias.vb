Public Class frmIncidencias

    Dim ListaIncidencias() As Notificacion

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim cadInsert As String

        If TextBox2.Text.Trim = "" Then Exit Sub
        If TextBox1.Text.Trim = "" Then
            If MessageBox.Show("No ha indicado ninguna referencia o documento asociado.¿Continuar?", _
                            AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then Exit Sub
        End If

        cadInsert = "INSERT INTO incidencias (idincidencia,documento,usuario,estado,incidencia,aplicacion) " & _
                    "VALUES (" & _
                    "nextval('incidencias_idincidencia_seq')," & _
                    "'" & TextBox1.Text.Trim & "'," & _
                    "'" & App_User & "'," & _
                    "'Abierta'," & _
                    "'" & TextBox2.Text.Replace("'", "\'").Trim & "','SIDCARTO')"
        Dim okProc As Boolean = ExeSinTran(cadInsert)
        If okProc = True Then
            MessageBox.Show("Incidencia notificada", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            CargarIncidencias()
        Else
            MessageBox.Show("No se creo ninguna incidencia", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub

    Private Sub frmIncidencias_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        ListView1.Location = New Point(16, 22)
        ListView1.Size = New Point(360, 190)
        GroupBox1.Location = New Point(16, 22)
        GroupBox1.Size = New Point(360, 190)
        GroupBox1.Visible = False

        ListView1.FullRowSelect = True
        ListView1.View = View.Details
        ListView1.Columns.Add("Referencia", 90, HorizontalAlignment.Left)
        ListView1.Columns.Add("Fecha", 70, HorizontalAlignment.Left)
        ListView1.Columns.Add("Usuario", 100, HorizontalAlignment.Left)
        ListView1.Columns.Add("Estado", 60, HorizontalAlignment.Left)
        If App_Permiso <> 2 Then
            Button2.Enabled = False
            Button3.Enabled = False
        End If

        CargarIncidencias()

    End Sub

    Private Sub CargarIncidencias()

        Dim cadSQL As String
        Dim rcdIncidencia As DataTable
        Dim filas() As DataRow
        Dim contador As Integer

        rcdIncidencia = New DataTable
        If CargarRecordset("SELECT * from incidencias where aplicacion='SIDCARTO' ORDER BY idincidencia", rcdIncidencia) = True Then
            filas = rcdIncidencia.Select
            ReDim ListaIncidencias(filas.Length - 1)
            contador = -1
            For Each dR As DataRow In filas
                contador = contador + 1
                ListaIncidencias(contador).id = dR("idincidencia").ToString
                ListaIncidencias(contador).referencia = dR("documento").ToString
                ListaIncidencias(contador).usuario = dR("usuario").ToString
                ListaIncidencias(contador).estado = dR("estado").ToString
                ListaIncidencias(contador).fecha = dR("fechacreacion")
                ListaIncidencias(contador).descripcion = dR("incidencia")
            Next
        End If
        rcdIncidencia.Dispose()
        rcdIncidencia = Nothing
        Erase filas
        RellenarLV()

    End Sub

    Private Sub RellenarLV()
        Dim elementoLV As ListViewItem
        Dim contador As Integer
        Dim mostrar As Boolean
        If ListaIncidencias Is Nothing Then Exit Sub
        ListView1.Items.Clear()

        contador = -1
        For Each incidencia As Notificacion In ListaIncidencias
            contador = contador + 1
            mostrar = False
            If RadioButton1.Checked = True Then
                mostrar = True
            ElseIf RadioButton2.Checked = True Then
                If incidencia.estado = "Abierta" Then mostrar = True
            ElseIf RadioButton3.Checked = True Then
                If incidencia.estado = "Cerrada" Then mostrar = True
            End If
            If mostrar = True Then
                elementoLV = New ListViewItem
                elementoLV.Text = incidencia.referencia
                elementoLV.SubItems.Add(incidencia.fecha.ToString("d"))
                elementoLV.SubItems.Add(incidencia.usuario)
                elementoLV.SubItems.Add(incidencia.estado)
                elementoLV.Tag = contador
                If ListView1.Items.Count Mod 2 = 0 Then
                    elementoLV.BackColor = Color.White
                Else
                    elementoLV.BackColor = Color.WhiteSmoke
                End If
                ListView1.Items.Add(elementoLV)
                elementoLV = Nothing
            End If

        Next

    End Sub



    Private Sub ToggleMuestra(ByVal sender As Object, ByVal e As System.EventArgs) _
            Handles RadioButton1.Click, RadioButton2.Click, RadioButton3.Click
        RellenarLV()
    End Sub


    Private Sub ListView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick
        GroupBox1.Visible = True
        GroupBox1.Text = ListaIncidencias(ListView1.SelectedItems(0).Tag).referencia
        Label3.Text = ListaIncidencias(ListView1.SelectedItems(0).Tag).usuario
        Label4.Text = ListaIncidencias(ListView1.SelectedItems(0).Tag).fecha
        Label5.Text = ListaIncidencias(ListView1.SelectedItems(0).Tag).descripcion
        If ListaIncidencias(ListView1.SelectedItems(0).Tag).estado = "Cerrada" Then
            Label3.ForeColor = Color.DarkGreen
            Label4.ForeColor = Color.DarkGreen
        ElseIf ListaIncidencias(ListView1.SelectedItems(0).Tag).estado = "Abierta" Then
            Label3.ForeColor = Color.Red
            Label4.ForeColor = Color.Red
        End If
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        GroupBox1.Visible = False
    End Sub

    Private Sub CambiarEstado(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                    Handles Button2.Click, Button3.Click

        Dim okProc As Boolean
        Me.Cursor = Cursors.WaitCursor
        GroupBox1.Visible = False
        If sender.name = "Button2" Then
            okProc = ExeSinTran("UPDATE incidencias SET estado='Abierta' WHERE idincidencia=" & ListaIncidencias(ListView1.SelectedItems(0).Tag).id)
        ElseIf sender.name = "Button3" Then
            okProc = ExeSinTran("UPDATE incidencias SET estado='Cerrada' WHERE idincidencia=" & ListaIncidencias(ListView1.SelectedItems(0).Tag).id)
        End If
        If okProc = True Then CargarIncidencias()
        Me.Cursor = Cursors.Default

    End Sub
End Class