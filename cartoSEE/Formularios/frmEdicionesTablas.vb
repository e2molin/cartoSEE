Public Class frmEdicionesTablas

    Private Sub frmEdicionesTablas_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ListView1.Size = New Point(516, 256)
        ListView1.Location = New Point(12, 60)
        ListView1.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Bottom Or AnchorStyles.Right
        ListView1.Visible = True

        GroupBox1.Size = New Point(516, 256)
        GroupBox1.Location = New Point(12, 60)
        GroupBox1.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Bottom Or AnchorStyles.Right
        GroupBox1.Visible = False

    End Sub


    Sub CargarDatos(ByVal TipoElemento As Integer)

        ResizeLV(TipoElemento)
        RellenarLV(TipoElemento)
        PrepararPanel(TipoElemento)

    End Sub

    Sub ResizeLV(ByVal TipoElemento As Integer)

        ListView1.Columns.Clear()
        ListView1.View = View.Details
        ListView1.FullRowSelect = True
        If TipoElemento = 1 Then
            ListView1.Columns.Add("ID", "ID", 70, HorizontalAlignment.Left, 0)
            ListView1.Columns.Add("Tipo de Documento", "Tipo de Documento", 280, HorizontalAlignment.Left, 0)
            ListView1.Columns.Add("Carpeta en Repositorio", "Carpeta en Repositorio", 200, HorizontalAlignment.Left, 0)
        ElseIf TipoElemento = 2 Then
            ListView1.Columns.Add("ID", "ID", 70, HorizontalAlignment.Left, 0)
            ListView1.Columns.Add("Observación standard", "Observación standard", 480, HorizontalAlignment.Left, 0)
        ElseIf TipoElemento = 3 Then
            ListView1.Columns.Add("ID", "ID", 70, HorizontalAlignment.Left, 0)
            ListView1.Columns.Add("Estado del documento", "Estado del documento", 480, HorizontalAlignment.Left, 0)
        ElseIf TipoElemento = 4 Then
            ListView1.Columns.Add("Unidad", "Unidad", 250, HorizontalAlignment.Left, 0)
            ListView1.Columns.Add("Equivalencia", "Equivalencia m2", 100, HorizontalAlignment.Right, 0)
            ListView1.Columns.Add("Comentarios", "Comentarios", 140, HorizontalAlignment.Left, 0)

        End If


    End Sub

    Sub RellenarLV(ByVal TipoElemento As Integer)

        Dim cadSQL As String
        Dim reportData As DataTable
        Dim filas() As DataRow
        Dim iRegistro As Integer
        Dim elementoLV As ListViewItem

        Me.Cursor = Cursors.WaitCursor
        ListView1.Items.Clear()
        If TipoElemento = 1 Then
            'Carga de los tipos de documento
            cadSQL = "SELECT idtipodoc,tipodoc,dirrepo FROM bdsidschema.tbtipodocumento ORDER BY idtipodoc"
            reportData = New DataTable
            If CargarRecordset(cadSQL, reportData) = True Then
                filas = reportData.Select
                If filas.Length > 0 Then
                    iRegistro = 0
                    For Each registro As DataRow In filas
                        iRegistro = iRegistro + 1
                        elementoLV = New ListViewItem
                        elementoLV.Text = registro.Item("idtipodoc").ToString
                        elementoLV.SubItems.Add(registro("tipodoc").ToString)
                        elementoLV.SubItems.Add(registro("dirrepo").ToString)
                        If iRegistro Mod 2 = 0 Then
                            elementoLV.BackColor = Color.White
                        Else
                            elementoLV.BackColor = Color.WhiteSmoke
                        End If
                        elementoLV.Tag = registro.Item("idtipodoc").ToString
                        ListView1.Items.Add(elementoLV)
                        elementoLV = Nothing
                    Next
                End If
            End If
        ElseIf TipoElemento = 2 Then
            'Carga de las observaciones standard
            cadSQL = "SELECT idobservestandar,observestandar FROM bdsidschema.tbobservaciones ORDER BY idobservestandar"
            reportData = New DataTable
            If CargarRecordset(cadSQL, reportData) = True Then
                filas = reportData.Select
                If filas.Length > 0 Then
                    iRegistro = 0
                    For Each registro As DataRow In filas
                        iRegistro = iRegistro + 1
                        elementoLV = New ListViewItem
                        elementoLV.Text = registro.Item("idobservestandar").ToString
                        elementoLV.SubItems.Add(registro("observestandar").ToString)
                        If iRegistro Mod 2 = 0 Then
                            elementoLV.BackColor = Color.White
                        Else
                            elementoLV.BackColor = Color.WhiteSmoke
                        End If
                        elementoLV.Tag = registro.Item("idobservestandar").ToString
                        ListView1.Items.Add(elementoLV)
                        elementoLV = Nothing
                    Next
                End If
            End If
        ElseIf TipoElemento = 3 Then
            'Carga de los estados de conservación
            cadSQL = "SELECT idestadodoc,estadodoc FROM bdsidschema.tbestadodocumento ORDER BY idestadodoc"
            reportData = New DataTable
            If CargarRecordset(cadSQL, reportData) = True Then
                filas = reportData.Select
                If filas.Length > 0 Then
                    iRegistro = 0
                    For Each registro As DataRow In filas
                        iRegistro = iRegistro + 1
                        elementoLV = New ListViewItem
                        elementoLV.Text = registro.Item("idestadodoc").ToString
                        elementoLV.SubItems.Add(registro("estadodoc").ToString)
                        If iRegistro Mod 2 = 0 Then
                            elementoLV.BackColor = Color.White
                        Else
                            elementoLV.BackColor = Color.WhiteSmoke
                        End If
                        elementoLV.Tag = registro.Item("idestadodoc").ToString
                        ListView1.Items.Add(elementoLV)
                        elementoLV = Nothing
                    Next
                End If
            End If
        ElseIf TipoElemento = 4 Then
            'Carga de las equivalencias de medidas
            cadSQL = "SELECT idequivalencia,nombre,sup_m2,comentario FROM bdsidschema.tbequivalencias"
            reportData = New DataTable
            If CargarRecordset(cadSQL, reportData) = True Then
                filas = reportData.Select
                If filas.Length > 0 Then
                    iRegistro = 0
                    For Each registro As DataRow In filas
                        iRegistro = iRegistro + 1
                        elementoLV = New ListViewItem
                        elementoLV.Text = registro.Item("nombre").ToString
                        elementoLV.SubItems.Add(registro("sup_m2").ToString)
                        elementoLV.SubItems.Add(registro("comentario").ToString)
                        If iRegistro Mod 2 = 0 Then
                            elementoLV.BackColor = Color.White
                        Else
                            elementoLV.BackColor = Color.WhiteSmoke
                        End If
                        elementoLV.Tag = registro.Item("idequivalencia").ToString
                        ListView1.Items.Add(elementoLV)
                        elementoLV = Nothing
                    Next
                End If
            End If
        End If

        Me.Cursor = Cursors.Default
        ToolStripStatusLabel1.Text = "Registros:" & ListView1.Items.Count.ToString
    End Sub

    Sub PrepararPanel(ByVal TipoElemento As Integer)

        If TipoElemento = 1 Then
            Label1.Text = "Tipo de documento"
            Label2.Text = "Carpeta Repositorio"
            Label3.Visible = False
            TextBox3.Visible = False
        ElseIf TipoElemento = 2 Then
            Label1.Text = "Nombre de la Observación"
            Label2.Visible = False
            TextBox2.Visible = False
            Label3.Visible = False
            TextBox3.Visible = False
        ElseIf TipoElemento = 3 Then
            Label1.Text = "Estado del documento"
            Label2.Visible = False
            TextBox2.Visible = False
            Label3.Visible = False
            TextBox3.Visible = False
        ElseIf TipoElemento = 4 Then
            Label1.Text = "Nombre de la unidad"
            Label2.Text = "Equivalencia en metros cuadrados"
            Label3.Text = "Comentario"
        End If


    End Sub


    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click

        If GroupBox1.Tag = "" Then
            If CrearElemento() = True Then
                MessageBox.Show("Elemento creado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
                RellenarLV(Me.Tag)
                ListView1.Visible = True
                GroupBox1.Visible = False
                If Me.Tag <> 4 Then
                    CargarListaTiposDocumento()
                    MDIPrincipal.CargarFiltros()
                End If
            Else
                MessageBox.Show("Elemento no creado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        Else
            If ActualizarElemento() = True Then
                MessageBox.Show("Elemento actualizado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
                RellenarLV(Me.Tag)
                ListView1.Visible = True
                GroupBox1.Visible = False
                If Me.Tag <> 4 Then
                    CargarListaTiposDocumento()
                    MDIPrincipal.CargarFiltros()
                End If
            Else
                MessageBox.Show("Elemento no actualizado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End If


    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click

        GroupBox1.Visible = False
        ListView1.Visible = True

    End Sub

    Private Sub GestionFunciones(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                            Handles Button1.Click, Button2.Click, Button3.Click

        If sender.name = "Button1" Then
            If ListView1.SelectedItems.Count = 0 Then Exit Sub
            GroupBox1.Visible = True
            ListView1.Visible = False
            GroupBox1.Text = "Modificar atributo"
            RellenarCampos(Me.Tag)
        ElseIf sender.name = "Button2" Then
            GroupBox1.Visible = True
            ListView1.Visible = False
            GroupBox1.Text = "Crear atributo"
            TextBox1.Text = ""
            TextBox2.Text = ""
            TextBox3.Text = ""
            GroupBox1.Tag = ""
        ElseIf sender.name = "Button3" Then
            If ListView1.SelectedItems.Count = 0 Then Exit Sub
            If BorrarElemento() = True Then
                MessageBox.Show("Elemento eliminado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
                RellenarLV(Me.Tag)
                ListView1.Visible = True
                GroupBox1.Visible = False
                If Me.Tag <> 4 Then
                    CargarListaTiposDocumento()
                    MDIPrincipal.CargarFiltros()
                End If
            Else
                MessageBox.Show("El elemento no ha sido eliminado", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

        End If

    End Sub

    Sub RellenarCampos(ByVal TipoElemento As Integer)

        If TipoElemento = 1 Then
            TextBox1.Text = ListView1.SelectedItems(0).SubItems(1).Text
            TextBox2.Text = ListView1.SelectedItems(0).SubItems(2).Text
        ElseIf TipoElemento = 2 Then
            TextBox1.Text = ListView1.SelectedItems(0).SubItems(1).Text
        ElseIf TipoElemento = 3 Then
            TextBox1.Text = ListView1.SelectedItems(0).SubItems(1).Text
        ElseIf TipoElemento = 4 Then
            TextBox1.Text = ListView1.SelectedItems(0).Text
            TextBox2.Text = ListView1.SelectedItems(0).SubItems(1).Text
            TextBox3.Text = ListView1.SelectedItems(0).SubItems(2).Text
        End If
        GroupBox1.Tag = ListView1.SelectedItems(0).Tag

    End Sub

    Function BorrarElemento() As Boolean

        Dim ElemBorrar As Integer = 0
        Dim NumElementos As Integer = 0
        Dim Result As String = ""
        Dim CadSQL() As String

        If Me.Tag = 1 Then
            ElemBorrar = ListView1.SelectedItems(0).Tag
            ObtenerEscalar("SELECT count(*) FROM bdsidschema.archivo where tipodoc_id=" & ElemBorrar, Result)
            NumElementos = IIf(IsNumeric(Result), CType(Result, Integer), 0)
            If NumElementos > 0 Then
                MessageBox.Show("Hay elementos asociados a este tipo de documento. No se puede eliminar.", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Function
            End If
            If MessageBox.Show("¿Desea eliminar este tipo de documento:" & ListView1.SelectedItems(0).Text & "?", AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then Exit Function
            ReDim CadSQL(0)
            CadSQL(0) = "DELETE FROM bdsidschema.tbtipodocumento where idtipodoc=" & ElemBorrar.ToString
            BorrarElemento = ExeTran(CadSQL)
        ElseIf Me.Tag = 2 Then
            ElemBorrar = ListView1.SelectedItems(0).Tag
            ObtenerEscalar("SELECT count(*) FROM bdsidschema.archivo where idobservestandar=" & ElemBorrar, Result)
            NumElementos = IIf(IsNumeric(Result), CType(Result, Integer), 0)
            If NumElementos > 0 Then
                MessageBox.Show("Hay elementos asociados con esta observación. No se puede eliminar.", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Function
            End If
            If MessageBox.Show("¿Desea eliminar esta observación:" & ListView1.SelectedItems(0).Text & "?", AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then Exit Function
            ReDim CadSQL(0)
            CadSQL(0) = "DELETE FROM bdsidschema.tbobservaciones where idobservestandar=" & ElemBorrar.ToString
            BorrarElemento = ExeTran(CadSQL)
        ElseIf Me.Tag = 3 Then
            ElemBorrar = ListView1.SelectedItems(0).Tag
            ObtenerEscalar("SELECT count(*) FROM bdsidschema.archivo where idestadodoc=" & ElemBorrar, Result)
            NumElementos = IIf(IsNumeric(Result), CType(Result, Integer), 0)
            If NumElementos > 0 Then
                MessageBox.Show("Hay elementos asociados con este estado de conservación. No se puede eliminar.", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Function
            End If
            If MessageBox.Show("¿Desea eliminar este estado de conservación:" & ListView1.SelectedItems(0).Text & "?", AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then Exit Function
            ReDim CadSQL(0)
            CadSQL(0) = "DELETE FROM bdsidschema.tbestadodocumento where idestadodoc=" & ElemBorrar.ToString
            BorrarElemento = ExeTran(CadSQL)
        ElseIf Me.Tag = 4 Then
            ElemBorrar = ListView1.SelectedItems(0).Tag
            If MessageBox.Show("¿Desea eliminar esta conversión de unidades:" & ListView1.SelectedItems(0).Text & "?", AplicacionTitulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then Exit Function
            ReDim CadSQL(0)
            CadSQL(0) = "DELETE FROM bdsidschema.tbequivalencias where id_equivalencia=" & ElemBorrar.ToString
            BorrarElemento = ExeTran(CadSQL)
        End If

    End Function

    Function ActualizarElemento() As Boolean

        Dim ListaSQL() As String
        Dim idElem As Integer
        Dim Result As String

        If Me.Tag = 1 Then
            If TextBox1.Text.Trim = "" Then
                MessageBox.Show("Escriba un nombre para el tipo de documento", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Function
            End If
            idElem = ListView1.SelectedItems(0).Tag
            ReDim ListaSQL(0)
            ListaSQL(0) = "UPDATE bdsidschema.tbtipodocumento SET " &
                            "tipodoc=" & "'" & TextBox1.Text.Replace("'", "`") & "'," &
                            "dirrepo=" & "'" & TextBox2.Text.Replace("'", "`") & "' " &
                            "WHERE idtipodoc=" & idElem
        ElseIf Me.Tag = 2 Then
            If TextBox1.Text.Trim = "" Then
                MessageBox.Show("Escriba un nombre para la observación", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Function
            End If
            idElem = ListView1.SelectedItems(0).Tag
            ReDim ListaSQL(0)
            ListaSQL(0) = "UPDATE bdsidschema.tbobservaciones SET " &
                            "observestandar=" & "'" & TextBox1.Text.Replace("'", "`") & "' " &
                            "WHERE idobservestandar=" & idElem
        ElseIf Me.Tag = 3 Then
            If TextBox1.Text.Trim = "" Then
                MessageBox.Show("Escriba un nombre para el estado de conservación", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Function
            End If
            idElem = ListView1.SelectedItems(0).Tag
            ReDim ListaSQL(0)
            ListaSQL(0) = "UPDATE bdsidschema.tbestadodocumento SET " &
                            "estadodoc=" & "'" & TextBox1.Text.Replace("'", "`") & "' " &
                            "WHERE idestadodoc=" & idElem
        ElseIf Me.Tag = 4 Then
            If TextBox1.Text.Trim = "" Or TextBox2.Text.Trim = "" Then
                MessageBox.Show("Escriba un nombre y equivalencia para la unidad", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Function
            End If
            idElem = ListView1.SelectedItems(0).Tag
            ReDim ListaSQL(0)
            ListaSQL(0) = "UPDATE bdsidschema.tbequivalencias SET " &
                            "nombre=" & "'" & TextBox1.Text.Replace("'", "`") & "'," &
                            "sup_m2=" & "" & TextBox2.Text.Replace(",", ".") & "," &
                            "comentario=" & "'" & TextBox3.Text.Replace("'", "`") & "' " &
                            "WHERE idequivalencia=" & idElem
        End If
        Application.DoEvents()
        ActualizarElemento = ExeTran(ListaSQL)
        Application.DoEvents()





    End Function


    Function CrearElemento() As Boolean

        Dim ListaSQL() As String
        Dim idElem As Integer
        Dim Result As String

        If Me.Tag = 1 Then
            If TextBox1.Text.Trim = "" Then
                MessageBox.Show("Escriba un nombre para el tipo de documento", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Function
            End If
            ObtenerEscalar("SELECT nextval('bdsidschema.tbtipodocumento_idtipodoc_seq')", Result)
            idElem = IIf(IsNumeric(Result), CInt(Result), 0)
            ReDim ListaSQL(0)
            ListaSQL(0) = "INSERT INTO bdsidschema.tbtipodocumento (idtipodoc,tipodoc,dirrepo) VALUES (" & idElem & "," &
                            "'" & TextBox1.Text.Replace("'", "`") & "','" &
                            TextBox2.Text.Replace("'", "`") & "')"
        ElseIf Me.Tag = 2 Then
            If TextBox1.Text.Trim = "" Then
                MessageBox.Show("Escriba un nombre para la observación", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Function
            End If
            ObtenerEscalar("SELECT nextval('bdsidschema.tbobservaciones_idobservestandar_seq')", Result)
            idElem = IIf(IsNumeric(Result), CInt(Result), 0)
            ReDim ListaSQL(0)
            ListaSQL(0) = "INSERT INTO bdsidschema.tbobservaciones (idobservestandar,observestandar) VALUES (" &
                            idElem & ",'" & TextBox1.Text.Replace("'", "`") & "')"

        ElseIf Me.Tag = 3 Then
            If TextBox1.Text.Trim = "" Then
                MessageBox.Show("Escriba un nombre para el estado de conservación", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Function
            End If
            ObtenerEscalar("SELECT nextval('bdsidschema.tbestadodocumento_idestadodoc_seq')", Result)
            idElem = IIf(IsNumeric(Result), CInt(Result), 0)
            ReDim ListaSQL(0)
            ListaSQL(0) = "INSERT INTO bdsidschema.tbestadodocumento (idestadodoc,estadodoc) VALUES (" &
                            idElem & ",'" & TextBox1.Text.Replace("'", "`") & "')"

        ElseIf Me.Tag = 4 Then
            If TextBox1.Text.Trim = "" Or TextBox2.Text.Trim = "" Then
                MessageBox.Show("Escriba un nombre y equivalencia para la unidad", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Function
            End If
            ObtenerEscalar("SELECT nextval('bdsidschema.tbequivalencias_idequivalencia_seq')", Result)
            idElem = IIf(IsNumeric(Result), CInt(Result), 0)
            ReDim ListaSQL(0)
            ListaSQL(0) = "INSERT INTO bdsidschema.tbequivalencias (nombre,sup_m2,idequivalencia,comentario) VALUES (" &
                            "'" & TextBox1.Text.Replace("'", "`") & "'," &
                            "" & TextBox2.Text & "," &
                            "" & idElem & "," &
                            "'" & TextBox3.Text.Replace("'", "`") & "')"

        End If
        Application.DoEvents()
        CrearElemento = ExeTran(ListaSQL)
        Application.DoEvents()



    End Function

    Private Sub ListView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick

        If ListView1.SelectedItems.Count = 0 Then Exit Sub
        GroupBox1.Visible = True
        ListView1.Visible = False
        GroupBox1.Text = "Modificar atributo"
        RellenarCampos(Me.Tag)

    End Sub

End Class