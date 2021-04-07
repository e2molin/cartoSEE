Public Class Destildator

    Dim reg_a As System.Text.RegularExpressions.Regex
    Dim reg_e As System.Text.RegularExpressions.Regex
    Dim reg_i As System.Text.RegularExpressions.Regex
    Dim reg_o As System.Text.RegularExpressions.Regex
    Dim reg_u As System.Text.RegularExpressions.Regex

    Dim reg_UPa As System.Text.RegularExpressions.Regex
    Dim reg_UPe As System.Text.RegularExpressions.Regex
    Dim reg_UPi As System.Text.RegularExpressions.Regex
    Dim reg_UPo As System.Text.RegularExpressions.Regex
    Dim reg_UPu As System.Text.RegularExpressions.Regex

    Dim reg_ny As System.Text.RegularExpressions.Regex
    Dim reg_cc As System.Text.RegularExpressions.Regex

    Dim param As String


    Public Sub New()
        reg_a = New System.Text.RegularExpressions.Regex("[à|á]", System.Text.RegularExpressions.RegexOptions.Compiled)
        reg_e = New System.Text.RegularExpressions.Regex("[è|é]", System.Text.RegularExpressions.RegexOptions.Compiled)
        reg_i = New System.Text.RegularExpressions.Regex("[ì|í|ï]", System.Text.RegularExpressions.RegexOptions.Compiled)
        reg_o = New System.Text.RegularExpressions.Regex("[ò|ó]", System.Text.RegularExpressions.RegexOptions.Compiled)
        reg_u = New System.Text.RegularExpressions.Regex("[ù|ú|ü]", System.Text.RegularExpressions.RegexOptions.Compiled)
        reg_UPa = New System.Text.RegularExpressions.Regex("[À|Á]", System.Text.RegularExpressions.RegexOptions.Compiled)
        reg_UPe = New System.Text.RegularExpressions.Regex("[È|É]", System.Text.RegularExpressions.RegexOptions.Compiled)
        reg_UPi = New System.Text.RegularExpressions.Regex("[Ì|Í|Ï]", System.Text.RegularExpressions.RegexOptions.Compiled)
        reg_UPo = New System.Text.RegularExpressions.Regex("[Ò|Ó]", System.Text.RegularExpressions.RegexOptions.Compiled)
        reg_UPu = New System.Text.RegularExpressions.Regex("[Ù|Ú|Ü]", System.Text.RegularExpressions.RegexOptions.Compiled)

        reg_ny = New System.Text.RegularExpressions.Regex("[ñ]", System.Text.RegularExpressions.RegexOptions.Compiled)
        reg_cc = New System.Text.RegularExpressions.Regex("[ç]", System.Text.RegularExpressions.RegexOptions.Compiled)
    End Sub

    Public Function destildar(ByVal entrada As String, Optional ByVal grafiaCatalanaOUT As Boolean = True, Optional ByVal formatoSYS As Boolean = True) As String
        param = entrada
        param = reg_a.Replace(param, "a")
        param = reg_e.Replace(param, "e")
        param = reg_i.Replace(param, "i")
        param = reg_o.Replace(param, "o")
        param = reg_u.Replace(param, "u")

        param = reg_UPa.Replace(param, "A")
        param = reg_UPe.Replace(param, "E")
        param = reg_UPi.Replace(param, "I")
        param = reg_UPo.Replace(param, "O")
        param = reg_UPu.Replace(param, "U")

        If grafiaCatalanaOUT Then
            param = reg_ny.Replace(param, "ni")
            param = reg_cc.Replace(param, "s")
        End If
        If formatoSYS Then
            param = param.Replace("/", "_")
            param = param.Replace("\", "_")
            param = param.Replace(":", "_")
            param = param.Replace("'", "")
            param = param.Replace("´", "")
            param = param.Replace("`", "")
        End If

        Return param
    End Function


    Protected Overrides Sub Finalize()
        reg_a = Nothing
        reg_e = Nothing
        reg_i = Nothing
        reg_o = Nothing
        reg_u = Nothing
        reg_ny = Nothing
        reg_cc = Nothing
        MyBase.Finalize()
    End Sub
End Class
