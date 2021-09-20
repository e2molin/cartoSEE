Public Class docCartoSEETipoDocu
    Property idTipodoc As Integer
    Property NombreTipo As String
    Property RepoTipo As String
    Property rutaMetadatosTipo As String
    Property prefijoMetadatos As String
    Property prefijoNombreCDD As String

    ReadOnly Property tematicaCdD As String
        Get
            Dim cadTematicaOut As String = ""
            Try
                If NombreTipo = "Directorio" Then
                    cadTematicaOut = "Plano director"
                ElseIf NombreTipo = "Parcelario urbano JE" Then
                    cadTematicaOut = "Parcelario urbano"
                ElseIf NombreTipo = "Hoja kilométrica" Then
                    cadTematicaOut = NombreTipo
                Else
                    cadTematicaOut = NombreTipo
                End If

                Return cadTematicaOut
            Catch ex As Exception
                GenerarLOG("e2m: Error en docCartoSEETipoDocu -> tematicaCdD -> " & ex.Message)
            End Try
            Return ""
        End Get
    End Property


End Class
