Public Class MuniHisto

    ''' <summary>
    ''' Todos los municipios históricos tienen el campo nomen_id=0 porque en el NGMEP no se contemplan
    ''' Sí tienen relleno el campo ngmep_muni_id
    ''' </summary>
    ''' <returns></returns>

    Property indice As Integer = 0
    Property tipo As String = ""
    Property nombreMuniHisto As String = ""
    Property nombreMuniActual As String = ""
    Property nombreMostrado As String = ""
    Property codineMuniHisto As Integer = 0
    Property codineMuniActual As Integer = 0
    Property provinciaNombre As String
    Property provinciaINE As Integer = 0
    Property comunidadINE As Integer = 0
    Property ngmep_id As Integer = 0
    Property ngbe_id As Integer = 0
    ''' <summary>
    ''' En Municipios y municipios históricos almacena el código NGMEP del municipio al que pertenece
    ''' </summary>
    ''' <returns></returns>
    Property ngmep_muni_id As Integer = 0
    Property centroide_id As Integer = 0
    Property observaciones As String
    Property nombre4CdD As String = ""
    ''' <summary>
    ''' Almacena la secuencia de códigos INE que tienen jurisdiccion sobre el territorio
    ''' Vale 0 si no hay jurisdicción municipal sobre el territorio
    ''' Én los municipios actuales tiene un sólo valor
    ''' </summary>
    ''' <returns></returns>
    Property seqJurisdiccion As String
    Property lstJurisdiccion As New ArrayList


    ReadOnly Property codineMuniHisto_toString() As String
        Get
            Return String.Format("{0:0000000}", codineMuniHisto)
        End Get
    End Property

    ReadOnly Property codineMuniActual_toString() As String
        Get
            Return String.Format("{0:00000}", codineMuniActual)
        End Get
    End Property

    ReadOnly Property municipioINE_LongFormat() As String
        Get
            Return "34" & String.Format("{0:00}", comunidadINE) & String.Format("{0:00}", provinciaINE) & String.Format("{0:00000}", codineMuniActual)
        End Get
    End Property

    Public Sub New()

    End Sub

    Public Sub New(ByVal idTerritorio As Integer)

        cargaTerritorio(idTerritorio)

    End Sub

    Private Sub cargaTerritorio(ByVal idterritorio As Integer)

        Dim cadSQL As String
        Dim rcdTmp As DataTable
        Dim jurisdicciones() As String

        cadSQL = $"SELECT territorios.idterritorio,territorios.nombre,
                    listamunicipios.nombre As nombremunicipioactual,
                    provincias.nombreprovincia,provincias.comautonoma_id,
                    to_char(Territorios.munihisto, 'FM0000000'::text) AS cod_munihisto,
                    to_char(Territorios.Municipio, 'FM00000'::text) AS codine,
                    to_char(Territorios.Provincia, 'FM00'::text) AS provincia_id, tipo, pertenencia, poligono_carto,
					nombremostrado, observaciones,nombrecdd, ngbe_id, nomen_id as ngmep_id, ngmep_muni_id
                    FROM bdsidschema.territorios 
                    LEFT JOIN bdsidschema.provincias ON provincias.idprovincia=territorios.provincia 
                    LEFT JOIN ngmepschema.listamunicipios ON territorios.ngmep_muni_id = listamunicipios.identidad WHERE idterritorio={idterritorio}"

        Try
            rcdTmp = New DataTable
            If Not CargarRecordset(cadSQL, rcdTmp) Then Exit Sub
            For Each fila As DataRow In rcdTmp.Select
                indice = idterritorio
                nombreMuniHisto = fila.Item("nombre").ToString
                tipo = fila.Item("tipo").ToString
                nombreMostrado = fila.Item("nombremostrado").ToString
                provinciaNombre = fila.Item("nombreprovincia").ToString
                provinciaINE = fila.Item("provincia_id")
                comunidadINE = fila.Item("comautonoma_id")
                nombreMuniActual = fila.Item("nombremunicipioactual").ToString
                ngmep_id = fila.Item("ngmep_id")
                ngbe_id = fila.Item("ngbe_id")
                ngmep_muni_id = fila.Item("ngmep_muni_id")
                centroide_id = fila.Item("poligono_carto")
                codineMuniActual = fila.Item("codine")
                codineMuniHisto = fila.Item("cod_munihisto")
                observaciones = fila.Item("observaciones").ToString
                nombre4CdD = fila.Item("nombrecdd").ToString
                seqJurisdiccion = fila.Item("pertenencia").ToString

                jurisdicciones = seqJurisdiccion.Split(";")
                If jurisdicciones.Length = 1 And fila.Item("pertenencia").ToString <> 0 Then
                    lstJurisdiccion.Add($"{codineMuniActual}|{nombreMuniActual}|{ngmep_muni_id}")
                Else
                    getJurisdicciones()
                End If
            Next

        Catch ex As Exception
            ModalError(ex.Message)
        Finally
            rcdTmp.Dispose()
            rcdTmp = Nothing
        End Try

    End Sub


    Private Sub getJurisdicciones()

        Try

            Dim rcdJurisdiccion As New DataTable
            If Not CargarRecordset($"SELECT idterritorio,nombre,to_char(Territorios.Municipio, 'FM00000'::text) AS codine 
                                    FROM  bdsidschema.territorios 
                                    WHERE tipo='Municipio' and codine in ({seqJurisdiccion})", rcdJurisdiccion) Then Exit Sub
            For Each fila As DataRow In rcdJurisdiccion.Select

            Next
        Catch ex As Exception

        End Try
    End Sub


End Class
