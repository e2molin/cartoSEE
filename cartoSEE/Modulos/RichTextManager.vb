Module RichTextManager
    Public Function AddRegtxt(ByVal RTC As RichTextBox, ByVal Txt2Send As String)
        With RTC
            .SelectionStart = RTC.TextLength
            .Text &= Txt2Send
            .SelectionStart = RTC.TextLength - Txt2Send.Length
            .SelectionLength = Txt2Send.Length
            .SelectionFont = New Font(RTC.SelectionFont, FontStyle.Regular)
            .SelectionFont = New Font("Verdana", 8, FontStyle.Regular)
        End With
    End Function
    Public Function AddBoldtxt(ByVal RTC As RichTextBox, ByVal Txt2Send As String)
        With RTC
            .SelectionStart = RTC.TextLength
            .Text &= Txt2Send
            .SelectionStart = RTC.TextLength - Txt2Send.Length
            .SelectionLength = Txt2Send.Length
            .SelectionFont = New Font(RTC.SelectionFont, FontStyle.Bold)
            .SelectionFont = New Font("Verdana", 8, FontStyle.Bold)
        End With
    End Function


    Public Function AddTitulo(RTC As RichTextBox, Txt2Send As String)
        With RTC
            .SelectionColor = Color.FromArgb(0, 0, 0)
            .SelectionFont = New Font("Segoe UI Semibold", 14, FontStyle.Bold)
            .AppendText(Txt2Send)
            .AppendText(Environment.NewLine)
        End With
    End Function

    Public Function AddSubTitulo(RTC As RichTextBox, Txt2Send As String)
        With RTC
            .SelectionColor = Color.FromArgb(65, 153, 235) 'Dark-Blue
            .SelectionFont = New Font("Segoe UI Semibold", 12, FontStyle.Bold)
            .AppendText(Txt2Send)
            .AppendText(Environment.NewLine)
        End With
    End Function

End Module
