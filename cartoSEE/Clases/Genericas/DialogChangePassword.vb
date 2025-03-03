Public Class DialogChangePassword

    Property valorResponse As New ChangeCredentials

    Public Shared Function InputBox() As ChangeCredentials

        Dim dlg As New DialogChangePassword
        'dlg.showTypeDeslin = showTypeDeslin
        'dlg.param2 = param2
        dlg.ShowDialog()
        Return dlg.valorResponse

    End Function

    Private Sub SetCharacterProtection()

        txtOldPass.PasswordChar = IIf(txtOldPassCheck.Checked = False, "*", "")
        txtNewPass.PasswordChar = IIf(txtNewPassCheck.Checked = False, "*", "")

    End Sub
    Private Sub btnAccept_Click(sender As Object, e As EventArgs) Handles btnAccept.Click
        valorResponse.OldPassword = txtOldPass.Text.Trim

        valorResponse.NewPassword = txtNewPass.Text.Trim

        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        valorResponse.OldPassword = ""

        valorResponse.NewPassword = ""

        Me.Close()
    End Sub

    Private Sub DialogChangePassword_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetCharacterProtection()


    End Sub

    Private Sub txtOldPassCheck_CheckedChanged(sender As Object, e As EventArgs) Handles txtOldPassCheck.CheckedChanged
        SetCharacterProtection()
    End Sub

    Private Sub txtNewPassCheck_CheckedChanged(sender As Object, e As EventArgs) Handles txtNewPassCheck.CheckedChanged
        SetCharacterProtection()
    End Sub
End Class

Public Class ChangeCredentials

    Property OldPassword As String = ""
    Property NewPassword As String = ""


End Class