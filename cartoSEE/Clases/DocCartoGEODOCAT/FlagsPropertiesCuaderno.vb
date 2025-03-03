Public Class FlagsPropertiesCuaderno

    Private valorDecimal As Integer = 0
    Private valorBinario As String = ""
    Private valorBinarioFormat As String = ""
    Enum MultiProperty As Integer
        Destacado = 1
        Estrellas5 = 2
        Peculiar = 3
        SinCatalogar = 4
        Investigar = 5
        Disponible6 = 6
        Disponible7 = 7
        Disponible8 = 8
        Disponible9 = 9
        Disponible10 = 10
        Disponible11 = 11
        Disponible12= 12
        Disponible13 = 13
        Disponible14 = 14
        Disponible15 = 15
        Disponible16 = 16

    End Enum

    ReadOnly Property propertyList As New List(Of String) From {
                                                    "Destacado",            			'1  
                                                    "5 estrellas",                      '2  
                                                    "Peculiar",             			'3  
                                                    "SinCatalogar",                     '4  
                                                    "Investigar",                       '5  
                                                    "Disponible 6", 				    '6  
                                                    "Disponible 7",       				'7  
                                                    "Disponible 8",                   	'8
                                                    "Disponible 9",                     '9 
                                                    "Disponible 10",                    '10
                                                    "Disponible 11",                    '11
                                                    "Disponible 12",                    '12
                                                    "Disponible 13",                    '13
                                                    "Disponible 14",                    '14
                                                    "Disponible 15",                    '15
                                                    "Disponible 16"                     '16
    }

    ' Define the property.
    Public Property propertyCode() As Integer
        Get
            Return valorDecimal
        End Get
        Set(ByVal value As Integer)
            If develmap.develcode.BaseConversor.ToNumBase(value, 2).Length > propertyList.Count Then
                Throw New System.Exception("Valor decimal excesivo")
            End If
            valorDecimal = value
            valorBinario = develmap.develcode.BaseConversor.ToNumBase(value, 2)
            valorBinarioFormat = StrDup(propertyList.Count - valorBinario.Length, "0") & valorBinario
            Application.DoEvents()
        End Set
    End Property

    Public Property propertyBinary() As String
        Get
            Return valorBinario
        End Get
        Set(ByVal value As String)
            If value.Length > propertyList.Count Then
                Throw New System.Exception("Valor binario excesivo")
            End If
            valorBinario = value
            valorDecimal = develmap.develcode.BaseConversor.FromNumBase(value, 2)
            valorBinarioFormat = StrDup(propertyList.Count - valorBinario.Length, "0") & valorBinario
        End Set
    End Property

    Public Function propertyPatron() As String
        Return valorBinarioFormat
    End Function

    Sub New()

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="value">Valor decimal que codifica las propiedades</param>
    Sub New(value As Integer)
        If develmap.develcode.BaseConversor.ToNumBase(value, 2).Length > propertyList.Count Then
            Throw New System.Exception("Valor decimal excesivo")
        End If
        valorDecimal = value
        valorBinario = develmap.develcode.BaseConversor.ToNumBase(value, 2)
        valorBinarioFormat = StrDup(propertyList.Count - valorBinario.Length, "0") & valorBinario
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="value">Valor binario que codifica las propiedades</param>
    Sub New(value As String)

        If value.Length > propertyList.Count Then
            Throw New System.Exception("Valor binario excesivo")
        End If
        valorBinario = value
        valorBinario = develmap.develcode.BaseConversor.FromNumBase(value, 10)
        valorBinarioFormat = StrDup(propertyList.Count - valorBinario.Length, "0") & valorBinario

    End Sub


    Public Function getFalseProperties() As ArrayList
        Dim respuestas As New ArrayList
        For position As Integer = 0 To propertyList.Count - 1
            Try
                If valorBinarioFormat.Substring(valorBinarioFormat.Length - position, 1) = "0" Then
                    respuestas.Add(propertyList(position))
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Next
        Return respuestas
    End Function

    Public Function getTrueProperties() As ArrayList
        Dim respuestas As New ArrayList
        For position As Integer = 0 To propertyList.Count - 1
            Try
                If valorBinarioFormat.Substring(valorBinarioFormat.Length - 1 - position, 1) = "1" Then
                    respuestas.Add(propertyList(position))
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Next
        Return respuestas
    End Function

    Public Function getTruePropertiesDescript() As String

        Dim cadOUT As String = ""
        For Each propItem As String In getTrueProperties()
            cadOUT = IIf(cadOUT = "", propItem, cadOUT & ", " & propItem)
        Next
        If cadOUT.Trim = "" Then cadOUT = "Nada destacable"
        Return cadOUT
    End Function

    Public Function getValueByProperty(propiedad As MultiProperty) As Boolean

        Dim position As Integer = 0
        Dim cadValue As String = ""
        Dim seqProps As String = ""
        Try
            position = Convert.ToInt32(propiedad)
            seqProps = propertyPatron()
            cadValue = seqProps.Substring(propertyList.Count - Convert.ToInt32(propiedad), 1)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        Return IIf(cadValue = "1", True, False)


    End Function



    Public Function validate(sequencia As String) As Boolean
        Return True
    End Function

    Public Function assignByContainer(checksContainer) As Boolean

        Dim cadProperties As String = ""
        For iPropCheck As Integer = 0 To checksContainer.Items.Count - 1
            Application.DoEvents()
            If cadProperties = "" Then
                cadProperties = IIf(checksContainer.GetItemChecked(iPropCheck) = True, "1", "0")
                Continue For
            End If
            cadProperties = IIf(checksContainer.GetItemChecked(iPropCheck) = True, "1", "0") & cadProperties

        Next

        Try
            If validate(cadProperties) Then
                propertyBinary = cadProperties
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            MessageBox.Show("Error:" & ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try

    End Function

    Public Function assignByProperty(propiedad As MultiProperty, value As Boolean) As Boolean

        Dim seqInicio As String
        Dim seqAsignado As String
        seqInicio = propertyPatron()
        seqAsignado = seqInicio.Substring(0, propertyList.Count - Convert.ToInt32(propiedad)) &
                            IIf(value = True, "1", "0") &
                            seqInicio.Substring(propertyList.Count - Convert.ToInt32(propiedad) + 1, propertyList.Count - (propertyList.Count - Convert.ToInt32(propiedad) + 1))


        Try
            If validate(seqAsignado) Then
                propertyBinary = seqAsignado
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            MessageBox.Show("Error:" & ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try

    End Function

    Public Function toggleProperty(propiedad As MultiProperty) As Boolean

        Dim seqInicio As String
        Dim seqAsignado As String
        seqInicio = propertyPatron()
        Dim initialValue As Boolean
        If seqInicio.Substring(propertyList.Count - Convert.ToInt32(propiedad), 1) = "1" Then
            initialValue = True
        Else
            initialValue = False
        End If

        seqAsignado = seqInicio.Substring(0, propertyList.Count - Convert.ToInt32(propiedad)) &
                            IIf(initialValue = True, "0", "1") &
                            seqInicio.Substring(propertyList.Count - Convert.ToInt32(propiedad) + 1, propertyList.Count - (propertyList.Count - Convert.ToInt32(propiedad) + 1))

        Try
            If validate(seqAsignado) Then
                propertyBinary = seqAsignado
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            MessageBox.Show("Error:" & ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try

    End Function

End Class
