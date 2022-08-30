Public Class TerritorioBSID

    Property indice As Integer
    Property nombre As String
    Property provinciaINE As Integer = 0
    Property autonomiaINE As Integer = 0
    Property municipioINE As Integer
    Property municipioNombre As String
    Property tipo As String
    Property poligonoCarto As Integer
    Property observaciones As String
    Property jurisdiccion As ArrayList

    ReadOnly Property municipioINE_Format() As String
        Get
            Return String.Format("{0:00000}", _municipioINE)
        End Get
    End Property

    ReadOnly Property municipioINE_LongFormat() As String
        Get
            Return "34" & String.Format("{0:00}", autonomiaINE) & String.Format("{0:00}", provinciaINE) & String.Format("{0:00000}", _municipioINE)
        End Get
    End Property


    Public Sub New()

        _jurisdiccion = New ArrayList

    End Sub
    ''' <summary>
    ''' Código idTerritorio. Puede deveolver un municipio o un territorio exclave o condominio
    ''' </summary>
    ''' <param name="idTerri"></param>
    Public Sub New(ByVal idTerri As Integer)

        _jurisdiccion = New ArrayList
        cargaTerritorio(idTerri)

    End Sub

    ''' <summary>
    ''' Código INE de cinco dígitos. En este caso se carga el terriutorio de tipo municipio conn ese código INE
    ''' </summary>
    ''' <param name="idTerri"></param>
    Public Sub New(ByVal codigoINE As String)

        _jurisdiccion = New ArrayList
        cargaMunicipio(codigoINE)

    End Sub

    Function getNombreMunicipioFull() As String

        Return _nombre & " (" & String.Format("{0:00000}", _municipioINE) & ")"

    End Function


    Function getNombreExclaveFull() As String

        Return _municipioNombre & " en " & _nombre & " (" & String.Format("{0:00000}", _municipioINE) & ")"

    End Function

    Function getNombreCondominioFull() As String

        Return _nombre & " (" & String.Format("{0:00000}", _municipioINE) & ")"

    End Function

    Function getNombreFull() As String

        If _tipo = "Condominio" Then
            Return _nombre & " (" & String.Format("{0:00000}", _municipioINE) & ")"
        ElseIf _tipo = "Exclave" Then
            Return _municipioNombre & " en " & _nombre & " (" & String.Format("{0:00000}", _municipioINE) & ")"
        ElseIf _tipo = "Municipio" Then
            Return _nombre & " (" & String.Format("{0:00000}", _municipioINE) & ")"
        ElseIf _tipo = "Municipio extinto" Then
            Return _nombre & " (" & String.Format("{0:00000}", _municipioINE) & ")(Extinto)"
        ElseIf _tipo = "País" Then
            Return _nombre
        ElseIf _tipo = "Accidente geográfico" Then
            Return _nombre
        Else
            Application.DoEvents()
        End If


    End Function

    Function getCodigoINEFull() As String

        '34011111012

        Return "34" & String.Format("{0:00}", _autonomiaINE) & String.Format("{0:00}", _provinciaINE) & String.Format("{0:00000}", _municipioINE)


    End Function


    Private Sub cargaTerritorio(ByVal idterritorio As Integer)

        Dim cadSQL As String
        Dim rcdTmp As DataTable
        Dim filas() As DataRow
        Dim muni As MunicipioActual

        cadSQL = "SELECT territorios.idterritorio,territorios.nombre,territorios.tipo,territorios.municipio," &
                "territorios.poligono_carto,territorios.observaciones," &
                "municipiosactuales.idmuni,municipiosactuales.nombremunicipio, municipiosactuales.codine, territorios.provincia as provincia_id,provincias.nombreprovincia,provincias.comautonoma_id " &
                "FROM bdsidschema.territorios " &
                "left JOIN bdsidschema.provincias on provincias.idprovincia=territorios.provincia " &
                "left JOIN bdsidschema.territorios2muni on territorios.idterritorio=territorios2muni.territorio_id " &
                "left JOIN bdsidschema.municipiosactuales on territorios2muni.codine=municipiosactuales.codine " &
                "where idterritorio = " & idterritorio

        rcdTmp = New DataTable
        If CargarRecordset(cadSQL, rcdTmp) = True Then
            filas = rcdTmp.Select
            For Each fila As DataRow In filas
                _indice = idterritorio
                _nombre = fila.Item("nombre")
                _tipo = fila.Item("tipo")
                If fila.Item("tipo") = "Condominio" Then
                    _municipioNombre = fila.Item("nombre")
                End If
                _provinciaINE = IIf(fila.Item("provincia_id").ToString = "", 0, fila.Item("provincia_id"))
                _autonomiaINE = IIf(fila.Item("comautonoma_id").ToString = "", 0, fila.Item("comautonoma_id"))
                _municipioINE = IIf(fila.Item("municipio").ToString = "", 0, fila.Item("municipio"))
                _poligonoCarto = IIf(fila.Item("poligono_carto").ToString = "", 0, fila.Item("poligono_carto"))
                _observaciones = fila.Item("observaciones").ToString
                If fila.Item("idmuni").ToString <> "" Then
                    muni = New MunicipioActual
                    muni.indice = fila.Item("idmuni")
                    muni.nombre = fila.Item("nombremunicipio")
                    muni.codINE = fila.Item("codine")
                    muni.provinciaId = fila.Item("provincia_id")
                    muni.provinciaNombre = fila.Item("nombreprovincia")
                    _jurisdiccion.Add(muni)
                    muni = Nothing
                End If
            Next
            Erase filas
        End If
        rcdTmp.Dispose()
        rcdTmp = Nothing


    End Sub

    Private Sub cargaMunicipio(ByVal codigoine As Integer)

        Dim cadSQL As String
        Dim rcdTmp As DataTable
        Dim filas() As DataRow
        Dim muni As MunicipioActual

        cadSQL = "SELECT territorios.idterritorio,territorios.nombre,territorios.tipo,territorios.municipio," &
                "territorios.poligono_carto,territorios.observaciones," &
                "municipiosactuales.idmuni,municipiosactuales.nombremunicipio, municipiosactuales.codine, territorios.provincia as provincia_id,provincias.nombreprovincia,provincias.comautonoma_id " &
                "FROM bdsidschema.territorios " &
                "left JOIN bdsidschema.provincias on provincias.idprovincia=territorios.provincia " &
                "left JOIN bdsidschema.territorios2muni on territorios.idterritorio=territorios2muni.territorio_id " &
                "left JOIN bdsidschema.municipiosactuales on territorios2muni.codine=municipiosactuales.codine " &
                "where tipo='Municipio' and  municipio = " & codigoine

        rcdTmp = New DataTable
        If CargarRecordset(cadSQL, rcdTmp) = True Then
            filas = rcdTmp.Select
            For Each fila As DataRow In filas
                _indice = fila.Item("idterritorio")
                _nombre = fila.Item("nombre")
                _tipo = fila.Item("tipo")
                If fila.Item("tipo") = "Condominio" Then
                    _municipioNombre = fila.Item("nombre")
                End If
                _provinciaINE = IIf(fila.Item("provincia_id").ToString = "", 0, fila.Item("provincia_id"))
                _autonomiaINE = IIf(fila.Item("comautonoma_id").ToString = "", 0, fila.Item("comautonoma_id"))
                _municipioINE = IIf(fila.Item("municipio").ToString = "", 0, fila.Item("municipio"))
                _poligonoCarto = IIf(fila.Item("poligono_carto").ToString = "", 0, fila.Item("poligono_carto"))
                _observaciones = fila.Item("observaciones").ToString
                If fila.Item("idmuni").ToString <> "" Then
                    muni = New MunicipioActual
                    muni.indice = fila.Item("idmuni")
                    muni.nombre = fila.Item("nombremunicipio")
                    muni.codINE = fila.Item("codine")
                    muni.provinciaId = fila.Item("provincia_id")
                    muni.provinciaNombre = fila.Item("nombreprovincia")
                    _jurisdiccion.Add(muni)
                    muni = Nothing
                End If
            Next
            Erase filas
        End If
        rcdTmp.Dispose()
        rcdTmp = Nothing


    End Sub



End Class
