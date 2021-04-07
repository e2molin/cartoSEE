Public Class OptionDialog

    Private _LoginName As String
    Private _LoginPassword As String
    Private _codResult As Integer


    Public Property Titulo() As String
        Get
            Return Me.Text
        End Get

        Set(ByVal sValue As String)
            Me.Text = sValue
        End Set

    End Property


    Public Function CheckRadioButtonResult(ByRef Tipo As String, ByRef Nombre As String) As Boolean

        If RadioButton1.Checked = True Then
            Tipo = RadioButton1.Tag
            Nombre = RadioButton1.Text
        ElseIf RadioButton2.Checked = True Then
            Tipo = RadioButton2.Tag
            Nombre = RadioButton2.Text
        ElseIf RadioButton3.Checked = True Then
            Tipo = RadioButton3.Tag
            Nombre = RadioButton3.Text
        Else
            Tipo = ""
            Nombre = ""
        End If
        CheckRadioButtonResult = True
    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Me.DialogResult = Windows.Forms.DialogResult.OK

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        Me.DialogResult = Windows.Forms.DialogResult.Cancel

    End Sub

    Private Sub OptionDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        RadioButton1.Tag = "JPG424"
        RadioButton2.Tag = "JPG250"
        RadioButton3.Tag = "ECW"
        Me.Text = titulo

    End Sub

End Class