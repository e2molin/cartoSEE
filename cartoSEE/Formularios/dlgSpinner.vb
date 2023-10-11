Imports System.Windows.Forms

Public Class dlgSpinner

    Private Sub dlgSpinner_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Application.DoEvents()
    End Sub

    Private Sub dlgSpinner_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        Application.DoEvents()
    End Sub
End Class
