Public Class LoginForm


    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click

        Dim okProc As Boolean
        Application.DoEvents()
        accessUser = UsernameTextBox.Text.Trim
        accessPass = PasswordTextBox.Text.Trim
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        accessUser = ""
        accessPass = ""
        Me.DialogResult = Windows.Forms.DialogResult.OK

    End Sub



End Class
