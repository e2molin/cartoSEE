Public Class docSIDCECAQuery
    Property resultados As ArrayList
    Property consultaSQL As String
    Property resumenConsultaSQL As String
    Property datasetTbl As String
    'Respuestas devueltas
    Property OffsetResponse As Integer = 0
    Property LimitResponse As Integer = 0


    Sub New(Optional datasetTblParam = "parcelasdatos")
        resultados = New ArrayList
        datasetTbl = datasetTblParam
    End Sub

    Sub refreshQuery()
        resultados.Clear()
        rellenarDataset()
    End Sub

    Sub getByFiltroSQL(ByVal Filtro As String)

        If datasetTbl = "parcelasdatos" Then

            consultaSQL = $"SELECT  
	                            parcelasdatos.idparceladato,parcelasdatos.numero_doc,parcelasdatos.fecha_insert,parcelasdatos.fechamodificacion,parcelasdatos.nombre_archivo,
	                            propietarios.nombre_completo AS propietario,
                                parcelasdatos.listaprop_id,
                                parcelasdatos.terminoid AS inemunihisto,
                                parcelasdatos.numparcela AS parcela,
                                parcelasdatos.distribuidor,
	                            parcelasdatos.sup_ha,parcelasdatos.sup_a,parcelasdatos.sup_m,parcelasdatos.sup_dec,
	                            parcelasdatos.ncc,parcelasdatos.calificador,parcelasdatos.incidencia,
	                            ST_AsText(ST_UNION(parcelasgeotrans.the_geom)) as nparcegeom
                            FROM bdsidschema.parcelasdatos 
                                    LEFT JOIN bdsidschema.listaprop ON parcelasdatos.listaprop_id = listaprop.idlistaprop
                                    LEFT JOIN bdsidschema.propietarios ON parcelasdatos.propietario_id = propietarios.idpropietario
	                                LEFT JOIN bdsidschema.parcelasgeotrans ON parcelasdatos.idparceladato = parcelasgeotrans.parceladato_id
  	                        where {Filtro}
                            GROUP BY 
	                            parcelasdatos.idparceladato,parcelasdatos.numero_doc,parcelasdatos.fecha_insert,parcelasdatos.fechamodificacion,parcelasdatos.nombre_archivo,
	                            propietarios.nombre_completo,parcelasdatos.listaprop_id,parcelasdatos.terminoid,parcelasdatos.numparcela,parcelasdatos.distribuidor,
							    parcelasdatos.sup_ha,parcelasdatos.sup_a,parcelasdatos.sup_m,parcelasdatos.sup_dec,parcelasdatos.ncc,parcelasdatos.calificador,parcelasdatos.incidencia
	                            ORDER BY parcelasdatos.idparceladato"

        End If
        If datasetTbl = "parcelasdata" Then

            consultaSQL = $"SELECT  
	                            parcelasdata.idparceladata as idparceladato,parcelasdata.sellado as numero_doc,
	                            parcelasdata.ceduladigital as nombre_archivo,
	                            parcelasdata.propietario AS propietario,
	                            parcelasdata.listaprop_id,
	                            parcelasdata.parcela AS parcela,
	                            parcelasdata.subparcela AS subparcela,
	                            parcelasdata.distribuidor,
	                            parcelasdata.sup_ha,parcelasdata.sup_a,parcelasdata.sup_m,parcelasdata.sup_dec,
	                            parcelasdata.barrio,parcelasdata.calle_lugar,parcelasdata.finca_edificio,parcelasdata.nummanzana,
	                            parcelasdata.numedificio,parcelasdata.delegado_catastral,parcelasdata.encargado_levan,parcelasdata.numparcela_reverso,
                                parcelasdata.caja as signaturacaja,parcelasdata.tipo as tipocedula,
	                            parcelasdata.comentario as incidencia,
	                            create_at as fecha_insert,update_at as fechamodificacion,
	                            ST_AsText(ST_UNION(parcelasdatageo.the_geom)) as nparcegeom
                            FROM bdsidschema.parcelasdata 
	                             LEFT JOIN bdsidschema.listaprop ON parcelasdata.listaprop_id = listaprop.idlistaprop
	                             LEFT JOIN bdsidschema.parcelasdatageo ON parcelasdata.idparceladata = parcelasdatageo.parceladata_id
                            where {Filtro}
                            GROUP BY 
                               parcelasdata.idparceladata,parcelasdata.sellado,create_at,update_at,parcelasdata.ceduladigital,
                               parcelasdata.propietario,parcelasdata.listaprop_id,
	                            parcelasdata.distribuidor,
	                            parcelasdata.sup_ha,parcelasdata.sup_a,parcelasdata.sup_m,parcelasdata.sup_dec,
	                            parcelasdata.barrio,parcelasdata.calle_lugar,parcelasdata.finca_edificio,parcelasdata.nummanzana,
	                            parcelasdata.numedificio,parcelasdata.delegado_catastral,parcelasdata.encargado_levan,parcelasdata.numparcela_reverso,
                                parcelasdata.caja,parcelasdata.tipo,
	                            parcelasdata.parcela,parcelasdata.subparcela,parcelasdata.comentario"


        End If


        If LimitResponse > 0 Then _consultaSQL &= $" LIMIT {LimitResponse}"
        If OffsetResponse > 0 Then _consultaSQL &= $" OFFSET {OffsetResponse}"

        resumenConsultaSQL = "getCuadernosInteriores_ByFiltroSQL:" & Filtro
        rellenarDataset()

    End Sub


    Private Sub rellenarDataset()

        Dim rcdDoc As DataTable
        Dim filas() As DataRow
        Dim managCoor As String

        registrarDatabaseLog(resumenConsultaSQL, consultaSQL)

        rcdDoc = New DataTable
        If CargarRecordset(consultaSQL, rcdDoc) = False Then
            rcdDoc = Nothing
            Exit Sub
        End If
        filas = rcdDoc.Select


        For Each dR As DataRow In filas
            'Esto es necesario mientras haya documentos con el documento Principal sin especificar
            Dim item As New docSIDCECA

            'Esto es necesario mientras haya documentos con el documento Principal sin especificar
            item.IdParcela = dR("idparceladato")
            item.ListaPropietarios = New docSIDCECAListaProp(dR("listaprop_id"))
            If datasetTbl = "parcelasdatos" Then item.Parcela = dR("ncc").ToString
            If datasetTbl = "parcelasdatos" Then item.SubParcela = dR("calificador").ToString
            If datasetTbl = "parcelasdata" Then item.Parcela = dR("parcela").ToString
            If datasetTbl = "parcelasdata" Then item.SubParcela = dR("subparcela").ToString

            item.SupHa = dR("sup_ha")
            item.SupA = dR("sup_a")
            item.SupM2 = dR("sup_m")
            item.SupDecM2 = dR("sup_dec")
            item.Propietario = dR("propietario").ToString
            item.Distribuidor = dR("distribuidor").ToString
            item.Incidencia = dR("incidencia").ToString
            item.SelladoDocumento = dR("numero_doc").ToString
            item.FechaAlta = dR("fecha_insert").ToString
            item.FechaModificacion = dR("fechamodificacion").ToString
            item.NombreDocumento = dR("nombre_archivo").ToString
            If datasetTbl = "parcelasdata" Then
                item.Barrio = dR("barrio").ToString
                item.CalleLugar = dR("calle_lugar").ToString
                item.FincaEdificio = dR("finca_edificio").ToString
                item.NumManzana = dR("nummanzana").ToString
                item.NumEdificio = dR("numedificio").ToString
                item.DelegadoCatastral = dR("delegado_catastral").ToString
                item.AutorLevantamiento = dR("encargado_levan").ToString
                item.NumParcelaReverso = dR("numparcela_reverso").ToString
                item.TipoCedula = dR("tipocedula").ToString
                item.SignaturaCaja = dR("signaturacaja").ToString
            End If

            item.WKT_geom = dR("nparcegeom").ToString

            If item.WKT_geom.StartsWith("POINT(") Then
                item.coordenadas.Add(New GEOCoordenada(item.WKT_geom, GEOCoordenada.srs.GEO_WGS84))
            ElseIf item.WKT_geom.StartsWith("MULTIPOINT(") Then
                managCoor = Replace(item.WKT_geom, "MULTIPOINT(", "")
                managCoor = Replace(managCoor, ")", "")
                Dim duplas() As String = managCoor.Split(",")
                For Each dupla In duplas
                    Dim coors() As String = dupla.Split(" ")
                    If coors.Length = 2 Then item.coordenadas.Add(New GEOCoordenada(coors(0), coors(1), GEOCoordenada.srs.GEO_WGS84))
                Next
            End If


            resultados.Add(item)
            item = Nothing


        Next









    End Sub



End Class
