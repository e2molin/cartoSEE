Public Class itemData

    Private StrNombre As String
    Private IntCodigo As String

    Public Sub New()
        StrNombre = ""
        IntCodigo = 0
    End Sub

    Public Sub New(ByVal Name As String, ByVal ID As String)
        StrNombre = Name
        IntCodigo = ID
    End Sub

    Public Property Name() As String
        Get
            Return StrNombre
        End Get

        Set(ByVal sValue As String)
            StrNombre = sValue
        End Set

    End Property

    Public Property Valor() As String

        Get
            Return IntCodigo
        End Get

        Set(ByVal iValue As String)
            IntCodigo = iValue
        End Set

    End Property

    Public Overrides Function ToString() As String
        Return StrNombre
    End Function


End Class
