Public Class frmSettings


    Sub SeleccionDirectorios(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                            Handles Button4.Click

        Dim DirRepo As String
        FolderBrowserDialog1.Description = "Selecciona el directorio que contiene las carpetas del repositorio de imágenes"
        FolderBrowserDialog1.ShowNewFolderButton = False
        FolderBrowserDialog1.ShowDialog()
        Application.DoEvents()
        DirRepo = FolderBrowserDialog1.SelectedPath.ToString
        Label2.Text = DirRepo
        TestearDirectorios()

    End Sub

    Sub SwithPanels(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                            Handles Button1.Click, Button2.Click, Button3.Click, Button17.Click

        GroupBox2.Visible = False
        GroupBox3.Visible = False
        GroupBox4.Visible = False
        GroupBox5.Visible = False
        PictureBox1.Visible = False
        PictureBox2.Visible = False
        PictureBox3.Visible = False
        PictureBox4.Visible = False

        If sender.name = "Button1" Then
            GroupBox3.Visible = True
            PictureBox1.Visible = True
        ElseIf sender.name = "Button2" Then
            GroupBox2.Visible = True
            PictureBox2.Visible = True
        ElseIf sender.name = "Button3" Then
            GroupBox4.Visible = True
            PictureBox3.Visible = True
        ElseIf sender.name = "Button17" Then
            GroupBox5.Visible = True
            PictureBox4.Visible = True
        End If




    End Sub


    Sub TestearDirectorios()

        If System.IO.Directory.Exists(Label2.Text) = False Then
            ErrorProvider1.SetError(Me.Label2, "El directorio no existe")
        Else
            ErrorProvider1.SetError(Me.Label2, "")
        End If
        If System.IO.Directory.Exists(Label20.Text) = False Then
            ErrorProvider1.SetError(Me.Label20, "El directorio no existe")
        Else
            ErrorProvider1.SetError(Me.Label20, "")
        End If


    End Sub

    Private Sub frmSettings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim elementoLV As ListViewItem

        Label2.Text = rutaRepo
        Label20.Text = rutaRepoGeorref
        TextBox1.Text = DB_Servidor
        TextBox2.Text = DB_Port
        TextBox3.Text = DB_Instancia
        TextBox4.Text = DB_User
        TextBox5.Text = DB_Pass
        TextBox6.Text = rutaCentroDescargas
        TextBox7.Text = rutaRepoThumbs

        Label15.Text = VisorECW
        Label16.Text = VisorJPG
        Label22.Text = VisorPrint
        Label3.Text = PlantillaGIS
        Label5.Text = RutaRejillaNTV2
        If VisorECW = "" Then CheckBox1.Checked = True
        If PlantillaGIS = "" Then CheckBox2.Checked = True
        If VisorJPG = "" Then CheckBox3.Checked = True
        If VisorPrint = "" Then CheckBox4.Checked = True

        TestearDirectorios()

        Me.Size = New Point(780, 500)

        GroupBox2.Location = New Point(210, 12)
        GroupBox2.Size = New Point(555, 440)
        GroupBox2.Visible = False

        GroupBox3.Location = New Point(210, 12)
        GroupBox3.Size = New Point(555, 440)
        GroupBox3.Visible = False

        GroupBox4.Location = New Point(210, 12)
        GroupBox4.Size = New Point(555, 440)
        GroupBox4.Visible = False

        GroupBox5.Location = New Point(210, 12)
        GroupBox5.Size = New Point(555, 440)
        GroupBox5.Visible = False


        PictureBox1.Location = New Point(20, 20)
        PictureBox1.Size = New Point(128, 128)
        PictureBox1.Visible = False

        PictureBox2.Location = New Point(20, 20)
        PictureBox2.Size = New Point(128, 128)
        PictureBox2.Visible = False

        PictureBox3.Location = New Point(20, 20)
        PictureBox3.Size = New Point(128, 128)
        PictureBox3.Visible = False

        PictureBox4.Location = New Point(20, 20)
        PictureBox4.Size = New Point(128, 128)
        PictureBox4.Visible = False

        GroupBox2.Visible = True
        PictureBox2.Visible = True
        Label14.Text = "Si Usuario y Contraseña se dejan en blanco, se utilizarán los valores por defecto"
        Label19.Text = "Si no se seleccionan programas específicos, los documentos se abrirán con los visores por defecto de Windows"

        Label7.Text = MapaBase

        If CalidadFavorita = "Alta" Then
            RadioButton1.Checked = True
        Else
            RadioButton2.Checked = True
        End If

        'Metadatos
        ListView1.FullRowSelect = True
        ListView1.GridLines = False
        ListView1.View = View.Details
        ListView1.SmallImageList = MDIPrincipal.ImageList2
        ListView1.Items.Clear()
        ListView1.Columns.Clear()
        ListView1.Columns.Add("Tipo Documento", "Tipo Documento", 150, HorizontalAlignment.Left, 3)
        ListView1.Columns.Add("Prefijo Nombre", 100, HorizontalAlignment.Left)
        ListView1.Columns.Add("Ruta plantilla metadatos", 320, HorizontalAlignment.Left)

        Application.DoEvents()
        If Not IsNothing(RutasPlantillasMetadatos) Then
            For Each Tipo As TiposDocumento In RutasPlantillasMetadatos
                elementoLV = New ListViewItem
                elementoLV.Text = Tipo.NombreTipo
                elementoLV.SubItems.Add(Tipo.PrefijoNom)
                elementoLV.SubItems.Add(Tipo.RutaMetadatosTipo)
                elementoLV.Tag = Tipo.idTipodoc
                If System.IO.File.Exists(Tipo.RutaMetadatosTipo) = False Then
                    elementoLV.ForeColor = Color.Red
                Else
                    elementoLV.ForeColor = Color.Black
                End If
                ListView1.Items.Add(elementoLV)
                elementoLV = Nothing
            Next
        End If

        ToolStripStatusLabel1.Text = "Fichero de configuración: " & AppFolderSetting & "\" & My.Application.Info.AssemblyName & ".ini"


    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click

        EscribeIni("Repositorio", "rutaRepo", Label2.Text)
        EscribeIni("Repositorio", "rutaRepoGeorref", Label20.Text.Trim)
        EscribeIni("Visores", "MapaBase", Label7.Text)
        EscribeIni("Configuracion", "RutaRejillaNTV2", Label5.Text)
        If RadioButton1.Checked = True Then
            EscribeIni("Repositorio", "CalidadFavorita", "Alta")
        Else
            EscribeIni("Repositorio", "CalidadFavorita", "Media")
        End If
        LeerConfiguracionINI()
        MessageBox.Show("Configuración actualizada", "Opciones de Configuración", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click

        EscribeIni("Database", "Servidor", TextBox1.Text)
        EscribeIni("Database", "Port", TextBox2.Text)
        EscribeIni("Database", "Instancia", TextBox3.Text)
        'EscribeIni("Database", "User", TextBox4.Text)
        'EscribeIni("Database", "Pass", TextBox5.Text)
        LeerConfiguracionINI()
        MessageBox.Show("Configuración actualizada. Reinicie la aplicación para que los cambios sean efectivos", "Opciones de Configuración", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        Me.Close()
    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        Me.Close()
    End Sub

    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
        Me.Close()
    End Sub

    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
        EscribeIni("Visores", "VisorECW", Label15.Text)
        EscribeIni("Visores", "VisorJPG", Label16.Text)
        EscribeIni("Visores", "VisorPrint", Label22.Text)
        If CheckBox2.Checked = True Then
            EscribeIni("Visores", "PlantillaGIS", "")
        Else
            EscribeIni("Visores", "PlantillaGIS", Label3.Text)
        End If

        LeerConfiguracionINI()
        MessageBox.Show("Configuración actualizada", "Opciones de Configuración", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button14.Click

        OpenFileDialog1.Title = "Selecciona programa para abrir ficheros PDF"
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Label15.Text = OpenFileDialog1.FileName
            CheckBox1.Checked = False
        End If

    End Sub

    Private Sub Button15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button15.Click

        OpenFileDialog1.Title = "Selecciona programa para abrir ficheros JPG"
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Label16.Text = OpenFileDialog1.FileName
            CheckBox3.Checked = False
        End If

    End Sub


    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            Label15.Text = ""
        End If
    End Sub



    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click

        OpenFileDialog1.Title = "Selecciona plantilla GM para abrir ficheros"
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Label3.Text = OpenFileDialog1.FileName
            CheckBox2.Checked = False
        End If



    End Sub



    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked = True Then
            Label3.Text = ""
        End If
    End Sub

    Private Sub CheckBox3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox3.CheckedChanged
        If CheckBox3.Checked = True Then
            Label16.Text = ""
        End If
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click

        OpenFileDialog1.Title = "Selecciona rejilla NTv2"
        OpenFileDialog1.Filter = "Rejillas NTv2 (*.gsb)|*.gsb"
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            Label5.Text = OpenFileDialog1.FileName
        End If


    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        OpenFileDialog1.Title = "Selecciona mapa base"
        OpenFileDialog1.Filter = "Imágenes ECW (*.ecw)|*.ecw"
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            If System.IO.File.Exists(OpenFileDialog1.FileName) = True Then
                Label7.Text = OpenFileDialog1.FileName
            End If
        End If



    End Sub

    Private Sub Button18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button18.Click

        OpenFileDialog1.Title = "Selecciona plantilla metadatos NEM para " & ListView1.SelectedItems(0).Text
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.Filter = "Plantillas de metadatos (*.xml)|*.xml"
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            If System.IO.File.Exists(OpenFileDialog1.FileName) = True Then
                ListView1.SelectedItems(0).SubItems(2).Text = OpenFileDialog1.FileName
            End If
            Dim Prefijo As String
            Prefijo = InputDialog.InputBox("Introduce el prefijo para los metadatos de esta documentación.", "Metadatos de la documentación", "")
            ListView1.SelectedItems(0).SubItems(1).Text = Prefijo
            If System.IO.File.Exists(ListView1.SelectedItems(0).SubItems(2).Text) Then
                ListView1.SelectedItems(0).ForeColor = Color.Black
            Else
                ListView1.SelectedItems(0).ForeColor = Color.Red
            End If
        End If

    End Sub

    Private Sub Button22_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button22.Click

        For Each elementoLV As ListViewItem In ListView1.Items
            EscribeIni("Metadatos", "PrefijoNom" & elementoLV.Tag, elementoLV.SubItems(1).Text)
            EscribeIni("Metadatos", "RutaPlantilla" & elementoLV.Tag, elementoLV.SubItems(2).Text)
        Next
        EscribeIni("Metadatos", "rutaRepoThumbs", TextBox7.Text.Trim)
        EscribeIni("Metadatos", "rutaCentroDescargas", TextBox6.Text.Trim)
        For iBucle As Integer = 0 To RutasPlantillasMetadatos.Length - 1
            RutasPlantillasMetadatos(iBucle).RutaMetadatosTipo = _
                        LeeIni("Metadatos", "RutaPlantilla" & RutasPlantillasMetadatos(iBucle).idTipodoc.ToString)
            RutasPlantillasMetadatos(iBucle).PrefijoNom = _
                        LeeIni("Metadatos", "PrefijoNom" & RutasPlantillasMetadatos(iBucle).idTipodoc.ToString)
        Next
        LeerConfiguracionINI()
        MessageBox.Show("Configuración actualizada", "Opciones de Configuración", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub Button21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button21.Click
        Me.Close()
    End Sub


    Private Sub Button16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button16.Click
        Dim DirRepo As String
        FolderBrowserDialog1.Description = "Selecciona el directorio que contiene las carpetas del repositorio georreferenciado"
        FolderBrowserDialog1.ShowNewFolderButton = False
        FolderBrowserDialog1.ShowDialog()
        Application.DoEvents()
        DirRepo = FolderBrowserDialog1.SelectedPath.ToString
        Label20.Text = DirRepo
        TestearDirectorios()

    End Sub

    Private Sub Button19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button19.Click

        OpenFileDialog1.Title = "Selecciona Visor para el proceso de impresión"
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Label22.Text = OpenFileDialog1.FileName
            CheckBox4.Checked = False
        End If

    End Sub

    Private Sub CheckBox4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox4.CheckedChanged
        If CheckBox4.Checked = True Then
            Label22.Text = ""
        End If
    End Sub
End Class