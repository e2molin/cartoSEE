Public NotInheritable Class AboutBox1

    Private Sub AboutBox1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Set the title of the form.
        Dim ApplicationTitle As String
        If My.Application.Info.AssemblyName <> "" Then
            ApplicationTitle = My.Application.Info.AssemblyName
        Else
            ApplicationTitle = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If
        Me.Text = "Acerca de..."
        ' Initialize all of the text displayed on the About Box.
        ' TODO: Customize the application's assembly information in the "Application" pane of the project 
        '    properties dialog (under the "Project" menu).
        Label1.Text = "CartoSEE"
        Try
            Label3.Text = System.IO.File.GetLastWriteTime(Application.ExecutablePath)
        Catch ex As Exception

        End Try
        Label5.Text = "Servicio de Documentación Geográfica y Biblioteca"
        Label6.Text = "Secretaría General"
        Label7.Text = "Instituto Geográfico Nacional - Ministerio de Fomento"

    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
        Me.Size = New Point(575, 430)
    End Sub


End Class
