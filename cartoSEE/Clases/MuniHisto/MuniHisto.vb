Public Class MuniHisto

    Property indice As Integer
    Property nombreMuniHisto As String
    Property nombreMuniActual As String
    Property codineMuniHisto As Integer = 0
    Property codineMuniActual As Integer = 0
    Property provinciaNombre As String
    Property provinciaINE As Integer = 0
    Property comunidadINE As Integer = 0
    Property entidadID As Integer = 0

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

    Public Sub New(ByVal idmuniHisto As Integer)
        cargaTerritorio(idmuniHisto)
    End Sub

    Private Sub cargaTerritorio(ByVal idterritorio As Integer)

        Dim cadSQL As String
        Dim rcdTmp As DataTable
        Dim filas() As DataRow

        cadSQL = "SELECT munihisto.idmunihisto,munihisto.nombremunicipiohistorico,munihisto.provincia_id," &
                "munihisto.cod_muni,munihisto.cod_munihisto," &
                "listamunicipios.nombre as nombremuniactual, listamunicipios.inecorto,provincias.nombreprovincia,provincias.comautonoma_id,listamunicipios.identidad " &
                "FROM bdsidschema.munihisto " &
                "left JOIN ngmepschema.listamunicipios on munihisto.entidad_id=listamunicipios.identidad " &
                "left JOIN bdsidschema.provincias on provincias.idprovincia=munihisto.provincia_id " &
                "where idmunihisto = " & idterritorio
        rcdTmp = New DataTable
        If CargarRecordset(cadSQL, rcdTmp) = False Then Exit Sub
        filas = rcdTmp.Select
        rcdTmp.Dispose()
        rcdTmp = Nothing
        For Each fila As DataRow In filas
            indice = idterritorio
            nombreMuniHisto = fila.Item("nombremunicipiohistorico").ToString
            provinciaNombre = fila.Item("nombreprovincia").ToString
            provinciaINE = fila.Item("provincia_id")
            comunidadINE = fila.Item("comautonoma_id")
            nombreMuniActual = fila.Item("nombremuniactual").ToString
            entidadID = fila.Item("identidad")
            codineMuniActual = fila.Item("cod_muni")
            codineMuniHisto = fila.Item("cod_munihisto")
        Next
        Erase filas

    End Sub



End Class
