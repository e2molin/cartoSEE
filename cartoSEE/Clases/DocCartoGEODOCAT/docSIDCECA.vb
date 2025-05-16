Public Class docSIDCECA

    Property IdParcela As Integer
    Property ListaPropietarios As docSIDCECAListaProp
    Property Parcela As String
    Property SubParcela As String
    Property SupHa As Integer
    Property SupA As Integer
    Property SupM2 As Integer
    Property SupDecM2 As Integer
    Property Propietario As String
    Property Distribuidor As String
    Property Incidencia As String
    Property SelladoDocumento As String
    Property NombreDocumento As String
    Property FechaAlta As String
    Property FechaModificacion As String
    Property WKT_geom As String
    Property Barrio As String = ""
    Property CalleLugar As String = ""
    Property FincaEdificio As String = ""
    Property NumManzana As String = ""
    Property NumEdificio As String = ""
    Property DelegadoCatastral As String = ""
    Property AutorLevantamiento As String = ""
    Property NumParcelaReverso As String = ""
    Property TipoCedula As String = ""
    Property SignaturaCaja As String = ""
    Property coordenadas As New ArrayList
    Property listaMosaicos As New ArrayList

    Dim getMosaicosConsultado As Boolean = False
    Dim datasetTblClass As String


    ReadOnly Property SupTotalM2() As Double

        Get
            Dim superficie As Integer
            superficie = SupHa * 10000 + SupA * 100 + SupM2 + SupDecM2 / 100
            Return superficie
        End Get

    End Property

    ReadOnly Property rutaFicheroPDF() As String

        Get
            If datasetTblClass = "parcelasdatos" Then Return $"{RutaRepoSIDCECA}{ListaPropietarios.CodMuniHisto}\{Replace(NombreDocumento, "A.JPG", ".pdf")}"
            If datasetTblClass = "parcelasmadriddata" Then Return $"{RutaRepoSIDCECA}{ListaPropietarios.CodMuniHisto}\{SignaturaCaja}\{String.Format("{0:00000000}", CType(SelladoDocumento, Integer))}.pdf"
        End Get

    End Property

    ReadOnly Property rutaFicheroThumb() As String

        Get
            Return $"{RutaRepoSIDCECA}Thumb\{ListaPropietarios.CodMuniHisto}\{Replace(NombreDocumento, "A.JPG", ".jpg")}"
        End Get

    End Property

    ReadOnly Property nombreParcela() As String
        Get

            Return $"{Parcela}{IIf(SubParcela <> "No definida", SubParcela, "")}"

        End Get
    End Property

    ReadOnly Property urlcdd() As String

        Get

            If datasetTblClass = "parcelasdatos" Then Return $"https://www.ign.es/cartoteca/HK/{ListaPropietarios.CodMuniHisto}/{Replace(NombreDocumento, "A.JPG", ".pdf")}"
            If datasetTblClass = "parcelasmadriddata" Then Return $"https://www.ign.es/cartoteca/HK/{ListaPropietarios.CodMuniHisto}/{SignaturaCaja}/{String.Format("{0:00000000}", CType(SelladoDocumento, Integer))}.pdf"
            '01998545
        End Get

    End Property

    ''' <summary>
    ''' Devuelve la URL que apunta a la coordenda. Cuando hay más de una, se puede especificar cualde ellas se quiere devolver
    ''' </summary>
    ''' <param name="indexCoord"></param>
    ''' <returns></returns>
    ReadOnly Property getURLVizMapasAntiguos(Optional indexCoord As Integer = 0) As String
        Get
            If indexCoord <= coordenadas.Count - 1 Then
                Return $"https://visualizadores.ign.es/comparadormapas?center={coordenadas(indexCoord).ToStringSeparatedBy(",")}&zoom=19&srs=EPSG:{coordenadas(indexCoord).EPSGcode}"
            Else
                If coordenadas.Count > 0 Then
                    Return $"https://visualizadores.ign.es/comparadormapas?center={coordenadas(0).ToStringSeparatedBy(",")}&zoom=19&srs=EPSG:{coordenadas(0).EPSGcode}"
                End If
            End If
            Return ""
        End Get
    End Property


    ''' <summary>
    ''' El parámetro opcional datasetTbL nos indica la tabla de la cual saca los datos.
    ''' La clase trabaja con las tablas parcelasdatos y parcelasmadriddata
    ''' </summary>
    ''' <param name="idParcela"></param>
    ''' <param name="datasetTbl"></param>
    Sub New(idParcela As Integer, Optional datasetTbl As String = "parcelasdatos")

        Dim consultaSQL As String

        datasetTblClass = datasetTbl

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
  	                        where parcelasdatos.idparceladato={idParcela}
                            GROUP BY 
	                            parcelasdatos.idparceladato,parcelasdatos.numero_doc,parcelasdatos.fecha_insert,parcelasdatos.fechamodificacion,parcelasdatos.nombre_archivo,
	                            propietarios.nombre_completo,parcelasdatos.listaprop_id,parcelasdatos.terminoid,parcelasdatos.numparcela,parcelasdatos.distribuidor,
							    parcelasdatos.sup_ha,parcelasdatos.sup_a,parcelasdatos.sup_m,parcelasdatos.sup_dec,parcelasdatos.ncc,parcelasdatos.calificador,parcelasdatos.incidencia
	                            ORDER BY parcelasdatos.idparceladato"

        End If
        If datasetTbl = "parcelasmadriddata" Then

            consultaSQL = $"SELECT  
	                            parcelasmadriddata.idparcelamadriddata as idparceladato,parcelasmadriddata.sellado as numero_doc,
	                            parcelasmadriddata.ceduladigital as nombre_archivo,
	                            parcelasmadriddata.propietario AS propietario,
	                            parcelasmadriddata.listaprop_id,
	                            parcelasmadriddata.parcela AS parcela,
	                            parcelasmadriddata.subparcela AS subparcela,
	                            parcelasmadriddata.distribuidor,
	                            parcelasmadriddata.sup_ha,parcelasmadriddata.sup_a,parcelasmadriddata.sup_m,parcelasmadriddata.sup_dec,
	                            parcelasmadriddata.barrio,parcelasmadriddata.calle_lugar,parcelasmadriddata.finca_edificio,parcelasmadriddata.nummanzana,
	                            parcelasmadriddata.numedificio,parcelasmadriddata.delegado_catastral,parcelasmadriddata.encargado_levan,parcelasmadriddata.numparcela_reverso,
                                parcelasmadriddata.caja as signaturacaja,parcelasmadriddata.tipo as tipocedula,
	                            parcelasmadriddata.comentario as incidencia,
	                            create_at as fecha_insert,update_at as fechamodificacion,
	                            ST_AsText(ST_UNION(parcelasmadrid.the_geom)) as nparcegeom
                            FROM bdsidschema.parcelasmadriddata 
	                             LEFT JOIN bdsidschema.listaprop ON parcelasmadriddata.listaprop_id = listaprop.idlistaprop
	                             LEFT JOIN bdsidschema.parcelasmadrid ON parcelasmadriddata.idparcelamadriddata = parcelasmadrid.parcelamadriddata_id
                            where parcelasmadriddata.idparcelamadriddata={idParcela}
                            GROUP BY 
                               parcelasmadriddata.idparcelamadriddata,parcelasmadriddata.sellado,create_at,update_at,parcelasmadriddata.ceduladigital,
                               parcelasmadriddata.propietario,parcelasmadriddata.listaprop_id,
	                            parcelasmadriddata.distribuidor,
	                            parcelasmadriddata.sup_ha,parcelasmadriddata.sup_a,parcelasmadriddata.sup_m,parcelasmadriddata.sup_dec,
	                            parcelasmadriddata.barrio,parcelasmadriddata.calle_lugar,parcelasmadriddata.finca_edificio,parcelasmadriddata.nummanzana,
	                            parcelasmadriddata.numedificio,parcelasmadriddata.delegado_catastral,parcelasmadriddata.encargado_levan,parcelasmadriddata.numparcela_reverso,
                                parcelasmadriddata.caja,parcelasmadriddata.tipo,
	                            parcelasmadriddata.parcela,parcelasmadriddata.subparcela,parcelasmadriddata.comentario"


        End If

        rellenarDataset(consultaSQL)

    End Sub

    Private Sub rellenarDataset(consultaSQL As String)

        Dim rcdDoc As DataTable
        Dim filas() As DataRow
        Dim managCoor As String

        Try
            rcdDoc = New DataTable
            If Not CargarRecordset(consultaSQL, rcdDoc) Then
                rcdDoc = Nothing
                Exit Sub
            End If

            filas = rcdDoc.Select
            For Each dR As DataRow In filas
                'Esto es necesario mientras haya documentos con el documento Principal sin especificar
                IdParcela = dR("idparceladato")
                ListaPropietarios = New docSIDCECAListaProp(dR("listaprop_id"))
                If datasetTblClass = "parcelasdatos" Then Parcela = dR("ncc").ToString
                If datasetTblClass = "parcelasdatos" Then SubParcela = dR("calificador").ToString
                If datasetTblClass = "parcelasmadriddata" Then Parcela = dR("parcela").ToString
                If datasetTblClass = "parcelasmadriddata" Then SubParcela = dR("subparcela").ToString

                SupHa = dR("sup_ha")
                SupA = dR("sup_a")
                SupM2 = dR("sup_m")
                SupDecM2 = dR("sup_dec")
                Propietario = dR("propietario").ToString
                Distribuidor = dR("distribuidor").ToString
                Incidencia = dR("incidencia").ToString
                SelladoDocumento = dR("numero_doc").ToString
                FechaAlta = dR("fecha_insert").ToString
                FechaModificacion = dR("fechamodificacion").ToString
                NombreDocumento = dR("nombre_archivo").ToString
                If datasetTblClass = "parcelasmadriddata" Then
                    Barrio = dR("barrio").ToString
                    CalleLugar = dR("calle_lugar").ToString
                    FincaEdificio = dR("finca_edificio").ToString
                    NumManzana = dR("nummanzana").ToString
                    NumEdificio = dR("numedificio").ToString
                    DelegadoCatastral = dR("delegado_catastral").ToString
                    AutorLevantamiento = dR("encargado_levan").ToString
                    NumParcelaReverso = dR("numparcela_reverso").ToString
                    TipoCedula = dR("tipocedula").ToString
                    SignaturaCaja = dR("signaturacaja").ToString
                End If

                WKT_geom = dR("nparcegeom").ToString

                If WKT_geom.StartsWith("POINT(") Then
                    coordenadas.Add(New GEOCoordenada(WKT_geom, GEOCoordenada.srs.GEO_WGS84))
                ElseIf WKT_geom.StartsWith("MULTIPOINT(") Then
                    managCoor = Replace(WKT_geom, "MULTIPOINT(", "")
                    managCoor = Replace(managCoor, ")", "")
                    Dim duplas() As String = managCoor.Split(",")
                    For Each dupla In duplas
                        Dim coors() As String = dupla.Split(" ")
                        If coors.Length = 2 Then coordenadas.Add(New GEOCoordenada(coors(0), coors(1), GEOCoordenada.srs.GEO_WGS84))
                    Next
                End If


            Next

        Catch ex As Exception
            ModalError(ex.Message)
        Finally
            rcdDoc.Dispose()
            rcdDoc = Nothing
        End Try



    End Sub


    Sub getMosaicoFiles()

        Dim RutaDoc As String
        Dim contador As Integer
        Dim rcdMosaicos As New DataTable
        If getMosaicosConsultado = True Then Exit Sub
        listaMosaicos.Clear()

        Try
            CargarDatatable($"SELECT * FROM bdsidschema.docdigital WHERE codmuni={ListaPropietarios.CodMuniHisto}", rcdMosaicos)
            For Each docu As DataRow In rcdMosaicos.Select()
                listaMosaicos.Add(New FileGeorref With {
                    .AliasFile = docu.Item("titulo").ToString,
                    .NameFile = docu.Item("coleccion").ToString,
                    .PathFile = $"{rutaRepoGeorrefBase}\epsg{docu.Item("epsgcode")}\MOSAICOS_DIGITALES\{String.Format("{0:00}", ListaPropietarios.CodProv)}\{docu.Item("rutamosaico").ToString}",
                    .EPSCode = docu.Item("epsgcode")
                })

            Next

        Catch ex As Exception
            ModalError(ex.Message)
        Finally
            rcdMosaicos.Dispose()
            rcdMosaicos = Nothing
        End Try

        getMosaicosConsultado = True

    End Sub




End Class
