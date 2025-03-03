Public Class CustomMsgbox

    Property textCaption As String = ""
    Property messageText As String = ""



    Private Sub CustomMsgbox_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox1.Text = messageText
        TextBox1.Enabled = False
        Button2.Focus()
        Me.Text = textCaption
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.DialogResult = DialogResult.No
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.DialogResult = DialogResult.Yes
        Me.Close()
    End Sub

End Class