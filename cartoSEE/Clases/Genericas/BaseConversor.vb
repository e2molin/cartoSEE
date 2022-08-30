Option Strict On

Imports Microsoft.VisualBasic
Imports System

Imports System.Collections.Generic
Imports System.Text
'Imports System.Diagnostics

Namespace develmap.develcode
    Public Class BaseConversor
        ' 
        ' Funciones de conversión a otras bases
        '
        ''' <summary>
        ''' Convierte un número decimal en una base distinta
        ''' Las bases permitadas son de 2 a 36
        ''' </summary>
        ''' <param name="num"></param>
        ''' <param name="nBase">
        ''' Base a la que se convertirá (de 2 a 36)
        ''' (con los tipos de .NET ñla base 36 no es fiable)
        ''' </param>
        ''' <param name="conSeparador">
        ''' Si se añade un separador cada 4 cifras
        ''' </param>
        ''' <param name="trimStart">
        ''' Si se quitan los ceros a la izquierda
        ''' </param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ToNumBase(ByVal num As String, _
                                         ByVal nBase As Integer, _
                                         ByVal conSeparador As Boolean, _
                                         ByVal trimStart As Boolean) As String
            Dim s As New StringBuilder
            Dim n As ULong = CULng(num)
            If n = 0 AndAlso conSeparador = False AndAlso trimStart = False Then
                Return "0"
            End If

            ' La base debe ser como máximo 36
            ' (por las letras del alfabeto (26) + 10 dígitos)
            ' F = 70 - 55 + 1 = 16
            ' Z = 90 - 55 + 1 = 36
            If nBase < 2 OrElse nBase > 36 Then
                Throw New ArgumentOutOfRangeException( _
                            "La base debe ser como máximo 36 y como mínimo 2")
            End If

            Dim j As Integer = 0
            'Dim nu As Double = n
            Dim nu As Decimal = n

            While nu > 0
                'Dim k As Double = (nu / nBase)
                Dim k As Decimal = CDec(nu) / CDec(nBase)
                nu = Fix(k)
                Dim f As Integer = CInt((k - nu) * nBase)

                Select Case f
                    Case Is > 9 ' letras
                        s.Append(ChrW(f + 55))
                    Case Else ' números
                        s.Append(ChrW(f + 48))
                End Select
                If conSeparador Then
                    j = j + 1
                    If j = 4 Then
                        j = 0
                        s.Append(" ")
                    End If
                End If
            End While

            ' Hay que darle la vuelta a la cadena
            Dim ac() As Char = s.ToString.ToCharArray
            Array.Reverse(ac)
            s = New StringBuilder(ac)


            If trimStart Then
                Return s.ToString.TrimStart(" 0".ToCharArray).TrimEnd
            Else
                Return s.ToString.TrimEnd
            End If
        End Function


        ''' <summary>
        ''' Convierte de cualquier base a Double
        ''' </summary>
        ''' <param name="num">
        ''' El número en formato de la base
        ''' </param>
        ''' <param name="base">
        ''' La base del número a convertir
        ''' </param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        Public Shared Function FromNumBase(ByVal num As String, _
                                           ByVal base As Integer) As ULong
            Const aMay As Integer = AscW("A") - 10
            Const aMin As Integer = AscW("a") - 10
            Dim i As Integer = 0
            Dim n As ULong = 0

            num = num.TrimStart("0".ToCharArray)

            For j As Integer = num.Length - 1 To 0 Step -1
                Select Case num(j).ToString
                    Case "0"
                        i += 1
                    Case " "
                        ' nada
                    Case "1" To "9"
                        Dim k As Integer = CInt(num(j).ToString)
                        If k - base >= 0 Then Continue For
                        'n = CULng(n + k * System.Math.Pow(base, i))
                        n = n + CULng(k * System.Math.Pow(base, i))
                        i += 1
                    Case "A" To "Z"
                        Dim k As Integer = AscW(num(j)) - aMay
                        If k - base >= 0 Then Continue For
                        'n = CULng(n + k * System.Math.Pow(base, i))
                        n = n + CULng(k * System.Math.Pow(base, i))
                        i += 1
                    Case "a" To "z"
                        Dim k As Integer = AscW(num(j)) - aMin
                        If k - base >= 0 Then Continue For
                        'n = CULng(n + k * System.Math.Pow(base, i))
                        n = n + CULng(k * System.Math.Pow(base, i))
                        i += 1
                End Select
            Next

            Return n
        End Function


        '
        ' Sobrecargas
        '

        ''' <summary>
        ''' Convierte un número decimal en una base distinta
        ''' Las bases permitadas son de 2 a 36
        ''' No se muestran los ceros a la izquierda y
        ''' no se separan los dígitos en grupos de 4
        ''' </summary>
        ''' <param name="num"></param>
        ''' <param name="nBase"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ToNumBase(ByVal num As String, ByVal nBase As Integer) As String

            If num = "0" And nBase = 2 Then
                Return "0"
            End If

            Return ToNumBase(num, nBase, False, True)
        End Function

        ''' <summary>
        ''' Convierte un número decimal en una base distinta
        ''' Las bases permitadas son de 2 a 36
        ''' no se separan los dígitos en grupos de 4
        ''' </summary>
        ''' <param name="num"></param>
        ''' <param name="nBase"></param>
        ''' <param name="trimStart"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ToNumBase(ByVal num As String, _
                                         ByVal nBase As Integer, _
                                         ByVal trimStart As Boolean) As String
            Return ToNumBase(num, nBase, False, trimStart)
        End Function

        '
        ' Funciones específicas
        '

        ''' <summary>
        ''' Convierte de Decimal a binario (base 2)
        ''' </summary>
        ''' <param name="num">
        ''' El número a convertir en binario
        ''' se convierte internamente a ULong como máximo
        ''' </param>
        ''' <param name="conSeparador"></param>
        ''' <param name="trimStart"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        Public Shared Function DecToBin(ByVal num As String, _
                                        Optional ByVal conSeparador As Boolean = True, _
                                        Optional ByVal trimStart As Boolean = True) As String
            Return ToNumBase(num, 2, conSeparador, trimStart)
        End Function

        ''' <summary>
        ''' Convierte de Decimal a hexadecimal (base 16)
        ''' </summary>
        ''' <param name="num">
        ''' El número a convertir en hexadecimal
        ''' se convierte internamente a ULong como máximo
        ''' </param>
        ''' <param name="conSeparador"></param>
        ''' <param name="trimStart"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        Public Shared Function DecToHex(ByVal num As String, _
                                        Optional ByVal conSeparador As Boolean = True, _
                                        Optional ByVal trimStart As Boolean = True) As String
            Return ToNumBase(num, 16, conSeparador, trimStart)
        End Function

        ''' <summary>
        ''' Convierte de Decimal a octal (base 8)
        ''' </summary>
        ''' <param name="num">
        ''' El número a convertir en binario
        ''' se convierte internamente a ULong como máximo
        ''' </param>
        ''' <param name="conSeparador"></param>
        ''' <param name="trimStart"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        Public Shared Function DecToOctal(ByVal num As String, _
                                        Optional ByVal conSeparador As Boolean = True, _
                                        Optional ByVal trimStart As Boolean = True) As String
            Return ToNumBase(num, 8, conSeparador, trimStart)
        End Function


        ''' <summary>
        ''' Convierte de Hexadecimal a Double
        ''' </summary>
        ''' <param name="num"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        Public Shared Function FromHex(ByVal num As String) As Double
            Return FromNumBase(num, 16)
        End Function

        ''' <summary>
        ''' Convierte de Octal a Double
        ''' </summary>
        ''' <param name="num"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        Public Shared Function FromOct(ByVal num As String) As Double
            Return FromNumBase(num, 8)
        End Function


        ''' <summary>
        ''' Convierte de Binario a Double
        ''' </summary>
        ''' <param name="num"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        Public Shared Function FromBin(ByVal num As String) As Double
            Return FromNumBase(num, 2)
        End Function

    End Class
End Namespace
