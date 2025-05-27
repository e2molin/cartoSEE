Public Class docSIDCECAListaProp

    Property IdListaProp As Integer
    Property Ayuntamiento As String
    Property PartidoJudicial As String
    Property MunicipioActual As String
    Property MunicipioHistorico As String
    Property NombreColeccion As String
    Property NumeroColeccion As Integer
    Property CodMuniActual As Integer
    Property CodMuniHisto As Integer
    Property IdTerritorio As Integer
    Property NombreProvincia As String
    Property CodProv As Integer
    Property CodAutonomia As Integer
    Property Tomo As String
    Property DatasetTbl As String


    ReadOnly Property pathListaPropietariosAlfabetica() As String

        Get

            Return $"{RutaRepoSIDCECA}LP\{CodMuniHisto}LPA{String.Format("{0:00}", NumeroColeccion)}.pdf"

        End Get

    End Property

    ReadOnly Property pathListaPropietariosNumerica() As String

        Get

            Return $"{RutaRepoSIDCECA}LP\{CodMuniHisto}LPN{String.Format("{0:00}", NumeroColeccion)}.pdf"

        End Get

    End Property

    ReadOnly Property municipioINE_LongFormat() As String
        Get

            Return $"34{String.Format("{0:00}", CodAutonomia)}{String.Format("{0:00}", CodProv)}{String.Format("{0:00000}", CodMuniActual)}"

        End Get

    End Property


    Sub New(idListProp As Integer)


        Dim consultaSQL As String = $"SELECT idlistaprop,ayuntamiento,part_jud,numcoleccion,COALESCE(coleccion,'Única') as coleccion,
                                        listaprop.territorio_id as territorio_id,
	                                    t1.nombre as munihisto, t1.munihisto as codmunihisto,
	                                    t2.nombre as muniactual,t2.municipio as codmuniactual,
	                                    provincias.nombreprovincia,provincias.idprovincia,provincias.comautonoma_id,listaprop.dataset_tbl as dataset_tbl
	                                    FROM bdsidschema.listaprop 
	                                    INNER JOIN bdsidschema.territorios t1 ON listaprop.territorio_id=t1.idterritorio
	                                    INNER JOIN bdsidschema.territorios t2 ON t1.munihisto/100=t2.municipio
	                                    INNER JOIN bdsidschema.provincias ON t1.provincia=provincias.idprovincia
	                                    where t2.tipo='Municipio' and idlistaprop={idListProp}"
        rellenarDataset(consultaSQL)



    End Sub


    Private Sub rellenarDataset(consultaSQL As String)

        Dim rcdDoc As DataTable
        Dim filas() As DataRow
        Dim contador As Long
        Dim Anterior As Integer
        Dim terriDeslin As TerritorioBSID


        Try
            rcdDoc = New DataTable
            If Not CargarRecordset(consultaSQL, rcdDoc) Then
                rcdDoc = Nothing
                Exit Sub
            End If

            filas = rcdDoc.Select

            For Each dR As DataRow In filas
                'Esto es necesario mientras haya documentos con el documento Principal sin especificar

                IdListaProp = dR("idlistaprop")
                Ayuntamiento = dR("ayuntamiento").ToString
                PartidoJudicial = dR("part_jud").ToString
                MunicipioActual = dR("muniactual").ToString
                MunicipioHistorico = dR("munihisto").ToString
                NombreColeccion = dR("coleccion").ToString
                NumeroColeccion = dR("numcoleccion").ToString
                CodMuniActual = dR("codmuniactual")
                CodMuniHisto = dR("codmunihisto")
                NombreProvincia = dR("nombreprovincia").ToString
                CodProv = dR("idprovincia")
                DatasetTbl = dR("dataset_tbl").ToString
                IdTerritorio = dR("territorio_id")
                CodAutonomia = dR("comautonoma_id")

            Next

        Catch ex As Exception
            ModalError(ex.Message)
        Finally
            rcdDoc.Dispose()
            rcdDoc = Nothing
        End Try







    End Sub


End Class
