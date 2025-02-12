Public Class TerritorioBSID

    Property indice As Integer
    Property nombre As String
    Property nombreMunicipioActual As String
    Property provinciaINE As Integer = 0
    Property autonomiaINE As Integer = 0
    Property ineCorto As Integer
    Property ineLargo As String
    Property CodMuniHisto As String
    Property municipioNombre As String
    Property tipo As String
    Property centroideId As Integer
    Property observaciones As String
    Property jurisdiccion As New ArrayList
    Property ngbe_id As Integer
    Property ngmep_id As Integer
    Property ngmep_muni_id As Integer


    ReadOnly Property municipioINE_Format() As String
        Get
            Return String.Format("{0:00000}", ineCorto)
        End Get
    End Property

    ReadOnly Property municipioINE_LongFormat() As String
        Get
            Return "34" & String.Format("{0:00}", autonomiaINE) & String.Format("{0:00}", provinciaINE) & String.Format("{0:00000}", _ineCorto)
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

        Return _nombre & " (" & String.Format("{0:00000}", _ineCorto) & ")"

    End Function


    Function getNombreExclaveFull() As String

        Return _municipioNombre & " en " & _nombre & " (" & String.Format("{0:00000}", _ineCorto) & ")"

    End Function

    Function getNombreCondominioFull() As String

        Return _nombre & " (" & String.Format("{0:00000}", _ineCorto) & ")"

    End Function

    Function getNombreFull() As String

        If _tipo = "Condominio" Then
            Return _nombre & " (" & String.Format("{0:00000}", _ineCorto) & ")"
        ElseIf _tipo = "Exclave" Then
            Return _municipioNombre & " en " & _nombre & " (" & String.Format("{0:00000}", _ineCorto) & ")"
        ElseIf _tipo = "Municipio" Then
            Return _nombre & " (" & String.Format("{0:00000}", _ineCorto) & ")"
        ElseIf _tipo = "Municipio extinto" Then
            Return _nombre & " (" & String.Format("{0:00000}", _ineCorto) & ")(Extinto)"
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

        Return "34" & String.Format("{0:00}", _autonomiaINE) & String.Format("{0:00}", _provinciaINE) & String.Format("{0:00000}", _ineCorto)


    End Function


    Private Sub cargaTerritorio(ByVal idterritorio As Integer)

        Dim cadSQL As String
        Dim rcdTmp As DataTable
        Dim filas() As DataRow
        Dim muni As MunicipioActual

        cadSQL = $"SELECT territorios.idterritorio,territorios.nombre,territorios.tipo,territorios.municipio,listamunicipios.codigoine,listamunicipios.nombre as nombreMuniActual,
                    territorios.poligono_carto as centroide_id,territorios.pertenencia,territorios.munihisto,
                    territorios.nomen_id as ngmep_id,territorios.ngbe_id as ngbe_id,territorios.ngmep_muni_id as ngmep_muni_id,
				    territorios.provincia as provincia_id,provincias.nombreprovincia,provincias.comautonoma_id,territorios.observaciones
                    FROM bdsidschema.territorios 
                    left JOIN bdsidschema.provincias on provincias.idprovincia=territorios.provincia 
                    left JOIN ngmepschema.listamunicipios on territorios.ngmep_muni_id=listamunicipios.identidad WHERE idterritorio={idterritorio}"

        rcdTmp = New DataTable
        If CargarRecordset(cadSQL, rcdTmp) = True Then
            filas = rcdTmp.Select
            For Each fila As DataRow In filas
                indice = fila.Item("idterritorio")
                nombre = fila.Item("nombre").ToString
                tipo = fila.Item("tipo").ToString
                ineCorto = IIf(fila.Item("municipio").ToString = "", 0, fila.Item("municipio"))
                ineLargo = IIf(fila.Item("codigoine").ToString = "", 0, fila.Item("codigoine"))
                CodMuniHisto = String.Format("{0:0000000}", fila.Item("munihisto"))
                nombreMunicipioActual = IIf(fila.Item("nombreMuniActual").ToString = "", 0, fila.Item("nombreMuniActual").ToString)
                provinciaINE = IIf(fila.Item("provincia_id").ToString = "", 0, fila.Item("provincia_id"))
                autonomiaINE = IIf(fila.Item("comautonoma_id").ToString = "", 0, fila.Item("comautonoma_id"))
                centroideId = IIf(fila.Item("centroide_id").ToString = "", 0, fila.Item("centroide_id"))
                observaciones = fila.Item("observaciones").ToString
                ngbe_id = fila.Item("ngbe_id").ToString
                ngmep_id = fila.Item("ngmep_id").ToString
                ngmep_muni_id = fila.Item("ngmep_muni_id").ToString

                If tipo = "Condominio" Then
                    For Each muniOwner As String In fila.Item("pertenencia").ToString.Split(",")
                        jurisdiccion.Add(New TerritorioBSID(muniOwner))
                    Next
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

        cadSQL = $"SELECT territorios.idterritorio,territorios.nombre,territorios.tipo,territorios.municipio,listamunicipios.codigoine,listamunicipios.nombre as nombreMuniActual,
                    territorios.poligono_carto as centroide_id,territorios.pertenencia,,territorios.munihisto,
                    territorios.nomen_id as ngmep_id,territorios.ngbe_id as ngbe_id,territorios.ngmep_muni_id as ngmep_muni_id,
				    territorios.provincia as provincia_id,provincias.nombreprovincia,provincias.comautonoma_id,territorios.observaciones
                    FROM bdsidschema.territorios 
                    left JOIN bdsidschema.provincias on provincias.idprovincia=territorios.provincia 
                    left JOIN ngmepschema.listamunicipios on territorios.ngmep_muni_id=listamunicipios.identidad WHERE tipo='Municipio' and municipio={codigoine}"

        rcdTmp = New DataTable
        If CargarRecordset(cadSQL, rcdTmp) = True Then
            filas = rcdTmp.Select
            For Each fila As DataRow In filas
                indice = fila.Item("idterritorio")
                nombre = fila.Item("nombre").ToString
                tipo = fila.Item("tipo").ToString
                ineCorto = IIf(fila.Item("municipio").ToString = "", 0, fila.Item("municipio"))
                ineLargo = IIf(fila.Item("codigoine").ToString = "", 0, fila.Item("codigoine"))
                CodMuniHisto = String.Format("{0:0000000}", fila.Item("munihisto"))
                nombreMunicipioActual = IIf(fila.Item("nombreMuniActual").ToString = "", 0, fila.Item("nombreMuniActual").ToString)
                provinciaINE = IIf(fila.Item("provincia_id").ToString = "", 0, fila.Item("provincia_id"))
                autonomiaINE = IIf(fila.Item("comautonoma_id").ToString = "", 0, fila.Item("comautonoma_id"))
                centroideId = IIf(fila.Item("centroide_id").ToString = "", 0, fila.Item("centroide_id"))
                observaciones = fila.Item("observaciones").ToString
                ngbe_id = fila.Item("ngbe_id").ToString
                ngmep_id = fila.Item("ngmep_id").ToString
                ngmep_muni_id = fila.Item("ngmep_muni_id").ToString
            Next
            Erase filas
        End If
        rcdTmp.Dispose()
        rcdTmp = Nothing


    End Sub



End Class
