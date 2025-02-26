Public Class QuestCollection

    Property textCaption As String = ""
    Property messageText As String = ""

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.DialogResult = DialogResult.Yes
        Me.Close()
    End Sub

    Private Sub frmCollection_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label2.Text = messageText
        Me.Text = textCaption
    End Sub
End Class