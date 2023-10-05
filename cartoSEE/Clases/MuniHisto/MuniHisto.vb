Public Class MuniHisto

    Property indice As Integer = 0
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
    Property centroide_id As Integer = 0
    Property observaciones As String
    Property nombre4CdD As String = ""

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

        cadSQL = $"SELECT territorios.idterritorio,territorios.nombre,
                    listamunicipios.nombre As nombremunicipioactual,
                    provincias.nombreprovincia,provincias.comautonoma_id,
                    to_char(Territorios.munihisto, 'FM0000000'::text) AS cod_munihisto,
                    to_char(Territorios.Municipio, 'FM00000'::text) AS codine,
                    to_char(Territorios.Provincia, 'FM00'::text) AS provincia_id, tipo, pertenencia, poligono_carto,
					nombremostrado, observaciones,nombrecdd,ngbe_id, nomen_id as ngmep_id
                    FROM bdsidschema.territorios 
                    LEFT JOIN bdsidschema.provincias ON provincias.idprovincia=territorios.provincia 
                    LEFT JOIN ngmepschema.listamunicipios ON territorios.nomen_id = listamunicipios.identidad WHERE idterritorio={idterritorio}"

        Try
            rcdTmp = New DataTable
            If Not CargarRecordset(cadSQL, rcdTmp) Then Exit Sub
            For Each fila As DataRow In rcdTmp.Select
                indice = idterritorio
                nombreMuniHisto = fila.Item("nombremunicipiohistorico").ToString
                nombreMostrado = fila.Item("nombremostrado").ToString
                provinciaNombre = fila.Item("nombreprovincia").ToString
                provinciaINE = fila.Item("provincia_id")
                comunidadINE = fila.Item("comautonoma_id")
                nombreMuniActual = fila.Item("nombremuniactual").ToString
                ngmep_id = fila.Item("ngmep_id")
                ngbe_id = fila.Item("ngbe_id")
                centroide_id = fila.Item("poligono_carto")
                codineMuniActual = fila.Item("cod_muni")
                codineMuniHisto = fila.Item("cod_munihisto")
                observaciones = fila.Item("observaciones").ToString
                nombre4CdD = fila.Item("observaciones").ToString
            Next

        Catch ex As Exception
            ModalError(ex.Message)
        Finally
            rcdTmp.Dispose()
            rcdTmp = Nothing
        End Try

    End Sub



End Class
