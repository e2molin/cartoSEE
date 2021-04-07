Public Class LoginForm

    ' TODO: Insert code to perform custom authentication using the provided username and password 
    ' (See http://go.microsoft.com/fwlink/?LinkId=35339).  
    ' The custom principal can then be attached to the current thread's principal as follows: 
    '     My.User.CurrentPrincipal = CustomPrincipal
    ' where CustomPrincipal is the IPrincipal implementation used to perform authentication. 
    ' Subsequently, My.User will return identity information encapsulated in the CustomPrincipal object
    ' such as the username, display name, etc.

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click

        Application.DoEvents()
        'okProc = AutenticarUsuario(UsernameTextBox.Text.Trim, PasswordTextBox.Text.Trim)
        'okProc = True
        'La autenticación se hace por base de datos
        App_User = UsernameTextBox.Text.Trim
        App_Pass = PasswordTextBox.Text.Trim
        Me.DialogResult = Windows.Forms.DialogResult.OK
        'If okProc = True Then
        '    Me.DialogResult = Windows.Forms.DialogResult.OK
        'Else
        '    Me.DialogResult = Windows.Forms.DialogResult.Abort
        'End If
        Me.Close()

    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        App_User = ""
        App_Pass = ""
        Me.DialogResult = Windows.Forms.DialogResult.OK

    End Sub


End Class
