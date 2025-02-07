Public Class GestionUserNotification

    Dim ListaIncidencias As ArrayList

    Property incidenciaInicial As String = ""
    Property appOrigenIncidencia As String = "CARTOSEE"

    Private Sub GestionUserNotification_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ListView1.Location = New Point(16, 22)
        ListView1.Size = New Point(479, 322)
        GroupBox1.Location = New Point(16, 22)
        GroupBox1.Size = New Point(479, 322)
        GroupBox1.Visible = False

        ListView1.FullRowSelect = True
        ListView1.View = View.Details
        ListView1.Columns.Add("Referencia", 90, HorizontalAlignment.Left)
        ListView1.Columns.Add("Fecha", 70, HorizontalAlignment.Left)
        ListView1.Columns.Add("Usuario", 100, HorizontalAlignment.Left)
        ListView1.Columns.Add("Estado", 60, HorizontalAlignment.Left)
        If usuarioMyApp.permisosLista.usuarioISTARI = False Then
            Button2.Enabled = False
            Button3.Enabled = False
        End If

        CargarIncidencias()

        If incidenciaInicial <> "" Then
            TextBox1.Text = incidenciaInicial
        End If

        Me.Text = $"Incidencias de {appOrigenIncidencia}"

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim cadInsert As String

        If TextBox2.Text.Trim = "" Then Exit Sub
        If TextBox1.Text.Trim = "" Then
            If MessageBox.Show("No ha indicado ninguna referencia o documento asociado.¿Continuar?",
                            AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then Exit Sub
        End If

        cadInsert = "INSERT INTO bdsidschema.incidencias (idincidencia,documento,usuario,estado,incidencia,aplicacion) " &
                    "VALUES (" &
                    "nextval('bdsidschema.incidencias_idincidencia_seq')," &
                    "'" & TextBox1.Text.Trim & "'," &
                    "'" & usuarioMyApp.loginUser & "'," &
                    "'Abierta'," &
                    "E'" & TextBox2.Text.Replace("'", "\'").Trim & "','" & appOrigenIncidencia & "')"
        Dim okProc As Boolean = ExeSinTran(cadInsert)
        If okProc = True Then
            MessageBox.Show("Incidencia notificada", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            CargarIncidencias()
        Else
            MessageBox.Show("No se creo ninguna incidencia", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub



    Private Sub CargarIncidencias()

        Dim cadSQL As String
        Dim rcdIncidencia As DataTable
        Dim filas() As DataRow
        Dim notif As Notificacion

        ListaIncidencias = New ArrayList

        cadSQL = "SELECT * from bdsidschema.incidencias where aplicacion='" & appOrigenIncidencia & "' ORDER BY idincidencia"
        rcdIncidencia = New DataTable
        If CargarRecordset(cadSQL, rcdIncidencia) = True Then
            filas = rcdIncidencia.Select
            For Each dR As DataRow In filas
                notif = New Notificacion
                notif.id = dR("idincidencia").ToString
                notif.referencia = dR("documento").ToString
                notif.usuario = dR("usuario").ToString
                notif.estado = dR("estado").ToString
                notif.fecha = dR("fechacreacion")
                notif.descripcion = dR("incidencia").ToString
                notif.solucion = dR("solucion").ToString
                ListaIncidencias.Add(notif)
                notif = Nothing
            Next
        End If
        rcdIncidencia.Dispose()
        rcdIncidencia = Nothing
        Erase filas
        RellenarLV()

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
        Label7.Text = ListaIncidencias(ListView1.SelectedItems(0).Tag).solucion
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

    Private Sub CambiarEstado(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                    Handles Button2.Click, Button3.Click


        Dim okProc As Boolean

        GroupBox1.Visible = False
        Dim respuestaIncidencia As String = InputDialog.InputBox("Introduce una contestación a la incidencia", "Observaciones a la incidencia", "")
        Me.Cursor = Cursors.WaitCursor
        If sender.name = "Button2" Then
            okProc = ExeSinTran("UPDATE bdsidschema.incidencias SET estado='Abierta'" & IIf(respuestaIncidencia = "", ",solucion=null ", ",solucion=E'" & respuestaIncidencia.Replace("'", "\'") & "' ") & "WHERE idincidencia=" & ListaIncidencias(ListView1.SelectedItems(0).Tag).id)
        ElseIf sender.name = "Button3" Then
            okProc = ExeSinTran("UPDATE bdsidschema.incidencias SET estado='Cerrada'" & IIf(respuestaIncidencia = "", ",solucion=null ", ",solucion=E'" & respuestaIncidencia.Replace("'", "\'") & "' ") & "WHERE idincidencia=" & ListaIncidencias(ListView1.SelectedItems(0).Tag).id)
        End If
        If okProc = True Then CargarIncidencias()
        Me.Cursor = Cursors.Default


    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click

        Dim CeldaOUT As String
        Dim LineaOUT As String
        Dim iBucle As Integer

        If ListView1.Items.Count = 0 Then Exit Sub

        Using svDialog As SaveFileDialog = New SaveFileDialog() With {
                                                            .Title = "Introduzca el nombre del fichero",
                                                            .Filter = "Archivos CSV *.csv|*.csv"
                                                                }
            If svDialog.ShowDialog = DialogResult.OK Then
                Dim sw As New System.IO.StreamWriter(svDialog.FileName, False, System.Text.Encoding.Unicode)

                For Each Linea As ListViewItem In ListView1.Items
                    CeldaOUT = TratarCadenaExportarCSV(Linea.Text)
                    LineaOUT = Linea.Text
                    For iBucle = 0 To Linea.SubItems.Count - 1
                        LineaOUT = LineaOUT & ";" & TratarCadenaExportarCSV(Linea.SubItems(iBucle).Text)
                    Next
                    sw.WriteLine(LineaOUT)
                Next
                sw.Close()
                sw.Dispose()
                ModalInfo("Fichero de exportación generado")
            End If
        End Using


    End Sub

    Private Sub btnLoadDoc_Click(sender As Object, e As EventArgs) Handles btnLoadDoc.Click


        Dim numDocu As Integer
        Dim actaIncidencia As String

        actaIncidencia = ListView1.SelectedItems(0).Text
        If Not IsNumeric(actaIncidencia) Then
            actaIncidencia = ListView1.SelectedItems(0).Text.ToLower.Replace("a", "").Replace("r", "").Replace("c", "")
        End If

        ModalInfo("REvisar")
        'Try
        '    numDocu = CType(actaIncidencia, Integer)
        '    Dim frmResultados As New resultSIDDAE
        '    frmResultados.MdiParent = MDIPrincipal
        '    LanzarSpinner("Cargando datos")
        '    With frmResultados
        '        .paramSQL1 = numDocu
        '        .typeSearch = resultSIDDAE.TypeDataSearch.DocumentosBySellado
        '        .Show()
        '    End With
        '    CerrarSpinner()
        'Catch ex As Exception
        '    ModalError($"No se pude identificar el documento: {ex.Message}")
        'End Try




    End Sub
End Class