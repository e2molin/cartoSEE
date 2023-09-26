Public NotInheritable Class AboutBox1

    Dim URLcode As String = "https://github.com/e2molin/cartosee"


    Private Sub AboutBox1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Set the title of the form.
        Dim ApplicationTitle As String
        If My.Application.Info.AssemblyName <> "" Then
            ApplicationTitle = My.Application.Info.AssemblyName
        Else
            ApplicationTitle = IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If
        Me.Text = "Acerca de..."

        Label1.Text = "cartoSEE"
        Label5.Text = "Servicio de Biblioteca y documentación geográfica"
        Label6.Text = "Registro Central de Cartografía"
        Label7.Text = "Instituto Geográfico Nacional - MITMA"
        PictureBox1.Visible = usuarioMyApp.permisosLista.isUserISTARI

    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            Process.Start(URLcode)
        Catch ex As Exception
            ModalError(ex.Message)
        End Try
    End Sub
End Class
