Public Class FileGeorref
    Property NameFile As String
    Property PathFile As String
    Property EPSCode As String
    Property AliasFile As String ' Nombre largo que podemos usar para describir el fichero
    ''' <summary>
    ''' En los casos en los que hay documentos con varios ECW, almacena el número. Por ejemplo en _01 almacena 1
    ''' </summary>
    ''' <returns></returns>
    Property Index As Integer = 0
End Class
