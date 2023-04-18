Public Class frmExportCdD

    Dim exportDocs As New docCartoSEEquery
    Dim cancelar As Boolean
    Dim procTilde As New Destildator

    Dim CallBack As New System.Drawing.Image.GetThumbnailImageAbort(AddressOf MycallBack)
    Function MycallBack() As Boolean
        Return False
    End Function




    Private Sub frmExport_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        procTilde = Nothing
    End Sub

    Private Sub frmExport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim contadorProv As Integer = 0
        cboProvincias.Items.Add(New itemData("(Procesar todas)", 0))
        For Each Provincia As DataRow In ListaProvincias.Select
            contadorProv = contadorProv + 1
            cboProvincias.Items.Add(New itemData(Provincia.ItemArray(1).ToString, contadorProv))
        Next

        txtDirTarget.Text = My.Computer.FileSystem.SpecialDirectories.Temp
        txtDirTarget.Text = "D:\volcadosCDD"

        cancelar = False
        ToolStripStatusLabel1.Text = "Seleccione provincia"

        For Each tipoDocu As docCartoSEETipoDocu In tiposDocSIDCARTO
            CheckedListBox1.Items.Add(New itemData(tipoDocu.NombreTipo, tipoDocu.idTipodoc), False)
        Next


    End Sub


    Private Sub btnProcess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcess.Click

        If cboProvincias.SelectedIndex = -1 Then
            ModalExclamation("Selecciona una provincia")
            Exit Sub
        End If
        If Not System.IO.Directory.Exists(txtDirTarget.Text.Trim) Then
            ModalExclamation("El directorio no existe")
            Exit Sub
        End If

        If cboProvincias.Text = "(Procesar todas)" Then
            ToolStripProgressBar1.Minimum = 0
            ToolStripProgressBar1.Maximum = 50
            For ibucle = 1 To 50
                ToolStripProgressBar1.Value = ibucle
                Application.DoEvents()
                procesarProvincia4CdD(ibucle, txtDirTarget.Text)
            Next
        Else
            ToolStripProgressBar1.Minimum = 0
            ToolStripProgressBar1.Maximum = 2
            ToolStripProgressBar1.Value = 1
            procesarProvincia4CdD(CType(cboProvincias.SelectedItem, itemData).Valor, txtDirTarget.Text)
        End If

        ModalInfo("Proceso terminado")

    End Sub


    Private Sub procesarProvincia4CdD(ByVal cProv As Integer, ByVal folderOUT As String)

        Dim listaTerris As New ArrayList
        Dim tiposdocu2CDD As New ArrayList
        Dim proce As procGenerateHTMLReport

        Me.Cursor = Cursors.WaitCursor

        If CheckedListBox1.CheckedItems.Count = 0 Then
            ModalExclamation("No se han seleccionado los tipos de documento a procesar")
            Me.Cursor = Cursors.Default
            Exit Sub
        End If

        'Seleccionamos los documentos de la provincia
        ToolStripStatusLabel1.Text = "Accediendo a la información de " & DameProvinciaByINE(cProv)

        Dim seqTiposDoc As String = ""
        tiposdocu2CDD.Clear()
        For Each elem As itemData In CheckedListBox1.CheckedItems
            tiposdocu2CDD.Add(CType(elem.Valor, Integer))
            If seqTiposDoc = "" Then seqTiposDoc = CType(elem.Valor, Integer) : Continue For
            seqTiposDoc = seqTiposDoc & "," & CType(elem.Valor, Integer)
        Next


        Dim resultDocumentos As New docCartoSEEquery
        resultDocumentos.flag_ActualizarInfoGeom = True
        resultDocumentos.flag_CargarFicherosGEO = True
        resultDocumentos.getDocsSIDDAE_ByFiltroSellado("archivo.provincia_id=" & cProv & " AND tipodoc_id IN (" & seqTiposDoc & ")")

        If resultDocumentos.resultados.Count = 0 Then Exit Sub

        If ModalQuestion("Se van a procesar " & resultDocumentos.resultados.Count & " documentos. ¿Continuar?") <> DialogResult.Yes Then
            Me.Cursor = Cursors.Default
            Exit Sub
        End If

        ToolStripStatusLabel1.Text = "Procesando " & DameProvinciaByINE(cProv) & ". Copiando ficheros para el CdD"
        folderOUT = IIf(folderOUT.EndsWith("\"), folderOUT, folderOUT & "\")
        Try
            If Not IO.Directory.Exists(folderOUT) Then
                IO.Directory.CreateDirectory(folderOUT)
            End If
        Catch ex As Exception
            ModalExclamation(ex.Message)
        End Try
        proce = New procGenerateHTMLReport
        proce.copyFiles2DirectoryToCDD(resultDocumentos.resultados, folderOUT, tiposdocu2CDD, RadioButton2.Checked)
        proce = Nothing
        ToolStripProgressBar1.Visible = False
        ToolStripStatusLabel1.Text = "Documentos procesados: " & resultDocumentos.resultados.Count
        resultDocumentos.resultados.Clear()
        resultDocumentos = Nothing
        Me.Cursor = Cursors.Default

    End Sub





    Function generarFichMunicipio(ByVal terriNombre As String, ByVal terriINE As Integer) As Boolean

        Dim RutaCSV As String
        Dim CarpetaCSV As String
        Dim ArchivoCSV As String
        Dim codINE As String

        'Aplicamos reglas de nomenclatura al fichero de salida
        codINE = String.Format("{0:00000}", terriINE)

        ArchivoCSV = codINE & "-" & terriNombre & ".csv"
        ArchivoCSV = procTilde.destildar(ArchivoCSV).Replace(" ", "_")
        CarpetaCSV = txtDirTarget.Text & "\" & codINE.Substring(0, 2) & "\CSV\"
        RutaCSV = CarpetaCSV & ArchivoCSV

        If Not System.IO.Directory.Exists(CarpetaCSV) Then
            Try
                System.IO.Directory.CreateDirectory(CarpetaCSV)
            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End Try
        End If

        Using archivo As System.IO.StreamWriter = New System.IO.StreamWriter(RutaCSV, False, System.Text.Encoding.Default)
            ' variable para almacenar la línea actual del dataview  
            Dim linea As String = String.Empty
            Me.Cursor = Cursors.WaitCursor
            Application.DoEvents()
            'Primero Generamos las cabeceras
            linea = String.Empty
            linea = "Número de documento" & ";" & _
                    "Tipo" & ";" & _
                    "Firmas" & ";" & _
                    "Provincia" & ";" & _
                    "Deslinde Principal" & ";" & _
                    "Deslindes Secundarios" & ";" & _
                    "Anejos" & ";" & _
                    "Cambios de nombre" & ";" & _
                    "Anulado" & ";" & _
                    "Adicionales" & ";" & _
                    "Comentarios"
            With archivo
                .WriteLine(linea.ToString)
            End With
            For Each doc As docCartoSEE In exportDocs.resultados
                'If doc.Tipo.nombreTipo = "Memoria" Then Continue For
                'If doc.Tipo.nombreTipo = "Disco de datos" Then Continue For
                '' vaciar la línea  
                'linea = String.Empty
                'linea = doc.Sellado.ToString & ";" & _
                '        doc.Tipo.nombreTipo & ";" & _
                '        doc.firmas & ";" & _
                '        doc.provinciaNombre & ";" & _
                '        doc.getTerritorioPrincipal & ";" & _
                '        doc.getTerritorioSecundario & ";" & _
                '        doc.anejos.Replace(";", ":") & ";" & _
                '        doc.renombres.Replace(";", ":") & ";" & _
                '        IIf(doc.estaAnulado = True, "Sí", "No") & ";" & _
                '        doc.Adicionales & ";" & _
                '        doc.comentarios.Replace(";", ":")
                'With archivo
                '    .WriteLine(linea.ToString)
                'End With
            Next
        End Using
        Return True

    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim DirRepo As String
        FolderBrowserDialog1.Description = "Selecciona el directorio para realizar el volcado"
        FolderBrowserDialog1.ShowNewFolderButton = False
        FolderBrowserDialog1.ShowDialog()
        Application.DoEvents()
        DirRepo = FolderBrowserDialog1.SelectedPath.ToString
        txtDirTarget.Text = DirRepo

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click, Button3.Click

        Dim listaTerris As New ArrayList
        Dim tiposdocu2CDD As New ArrayList
        Dim proce As procGenerateHTMLReport
        Dim folderOUT As String

        Me.Cursor = Cursors.WaitCursor

        If sender.name = "Button3" Then
            For i As Integer = 0 To CheckedListBox1.Items.Count - 1
                CheckedListBox1.SetItemChecked(i, True)
            Next
        End If


        If CheckedListBox1.CheckedItems.Count = 0 Then
            ModalExclamation("No se han seleccionado los tipos de documento a procesar")
            Me.Cursor = Cursors.Default
            Exit Sub
        End If

        If sender.name = "Button2" Then
            ToolStripStatusLabel1.Text = "Accediendo a la información modificada entre " & DateTimePicker1.Value & " y " & DateTimePicker2.Value
        End If

        If sender.name = "Button3" Then
            ToolStripStatusLabel1.Text = "Accediendo a la información marcada para el CdD"
        End If


        Dim seqTiposDoc As String = ""
        tiposdocu2CDD.Clear()
        For Each elem As itemData In CheckedListBox1.CheckedItems
            tiposdocu2CDD.Add(CType(elem.Valor, Integer))
            If seqTiposDoc = "" Then seqTiposDoc = CType(elem.Valor, Integer) : Continue For
            seqTiposDoc = seqTiposDoc & "," & CType(elem.Valor, Integer)
        Next


        Dim resultDocumentos As New docCartoSEEquery
        resultDocumentos.flag_ActualizarInfoGeom = True
        resultDocumentos.flag_CargarFicherosGEO = True
        If sender.name = "Button2" Then
            resultDocumentos.getDocsSIDDAE_ByFiltroSellado("archivo.fechamodificacion " &
                                                       "between '" & FormatearFecha(DateTimePicker1.Value, "ISO8601") & "' AND '" &
                                                       "" & FormatearFecha(DateTimePicker2.Value, "ISO8601") & "' " &
                                                        "AND tipodoc_id IN (" & seqTiposDoc & ")")
        End If
        If sender.name = "Button3" Then
            resultDocumentos.getDocsSIDDAE_ByFiltroSQLwithPatron(" idarchivo>0 ", "1_______________")
        End If





        If resultDocumentos.resultados.Count = 0 Then Exit Sub

        If ModalQuestion("Se van a procesar " & resultDocumentos.resultados.Count & " documentos. ¿Continuar?") <> DialogResult.Yes Then
            Me.Cursor = Cursors.Default
            Exit Sub
        End If

        folderOUT = txtDirTarget.Text
        folderOUT = IIf(folderOUT.EndsWith("\"), folderOUT, folderOUT & "\")
        Try
            If Not IO.Directory.Exists(folderOUT) Then
                IO.Directory.CreateDirectory(folderOUT)
            End If
        Catch ex As Exception
            ModalExclamation(ex.Message)
        End Try

        proce = New procGenerateHTMLReport
        proce.CopyFiles2DirectoryToCDD(resultDocumentos.resultados, folderOUT, tiposdocu2CDD, RadioButton2.Checked)
        proce = Nothing
        ToolStripProgressBar1.Visible = False
        ToolStripStatusLabel1.Text = "Documentos procesados: " & resultDocumentos.resultados.Count
        resultDocumentos.resultados.Clear()
        resultDocumentos = Nothing
        Me.Cursor = Cursors.Default
        ModalInfo("Proceso terminado")


    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click, Button5.Click

        Dim valorCheck As Boolean

        If sender.name = "Button4" Then valorCheck = True
        If sender.name = "Button5" Then valorCheck = False
        For i As Integer = 0 To CheckedListBox1.Items.Count - 1
            CheckedListBox1.SetItemChecked(i, valorCheck)
        Next

    End Sub
End Class