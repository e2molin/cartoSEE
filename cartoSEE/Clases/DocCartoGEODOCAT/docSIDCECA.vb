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
    Property coordenadas As New ArrayList
    Property listaMosaicos As New ArrayList
    Dim getMosaicosConsultado As Boolean = False



    ReadOnly Property SupTotalM2() As Double

        Get
            Dim superficie As Integer
            superficie = SupHa * 10000 + SupA * 100 + SupM2 + SupDecM2 / 100
            Return superficie
        End Get

    End Property

    ReadOnly Property rutaFicheroPDF() As String

        Get
            Return $"{RutaRepoSIDCECA}{ListaPropietarios.CodMuniHisto}\{Replace(NombreDocumento, "A.JPG", ".pdf")}"
        End Get

    End Property

    ReadOnly Property rutaFicheroThumb() As String

        Get
            Return $"{RutaRepoSIDCECA}Thumb\{ListaPropietarios.CodMuniHisto}\{Replace(NombreDocumento, "A.JPG", ".jpg")}"
        End Get

    End Property

    ReadOnly Property nombreParcela() As String
        Get
            Return $"{Parcela}{SubParcela}"
        End Get
    End Property

    ReadOnly Property urlcdd() As String
        Get
            Return $"https://www.ign.es/cartoteca/HK/{ListaPropietarios.CodMuniHisto}/{Replace(NombreDocumento, "A.JPG", ".pdf")}"
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



    Sub New(idParcela As Integer)

        Dim consultasql As String = $"SELECT  
	                                    parcelasdatos.idparceladato,parcelasdatos.numero_doc,parcelasdatos.fecha_insert,parcelasdatos.fechamodificacion,parcelasdatos.nombre_archivo,
	                                    propietarios.nombre_completo AS propietario,
                                        parcelasdatos.listaprop_id,
                                        parcelasdatos.terminoid AS inemunihisto,
                                        parcelasdatos.numparcela AS parcela,
                                        parcelasdatos.sup_m2 AS superficie,
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
	                                   propietarios.nombre_completo,parcelasdatos.listaprop_id,parcelasdatos.terminoid,parcelasdatos.numparcela,parcelasdatos.sup_m2,parcelasdatos.distribuidor,
										parcelasdatos.sup_ha,parcelasdatos.sup_a,parcelasdatos.sup_m,parcelasdatos.sup_dec,parcelasdatos.ncc,parcelasdatos.calificador,parcelasdatos.incidencia
	                                    ORDER BY parcelasdatos.idparceladato"


        rellenarDataset(consultasql)

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
                Parcela = dR("ncc").ToString
                SubParcela = dR("calificador").ToString
                SupHa = dR("sup_ha")
                SupA = dR("sup_a")
                SupM2 = dR("sup_m")
                SupDecM2 = dR("sup_dec")
                Propietario = dR("propietario").ToString
                Distribuidor = dR("distribuidor").ToString
                Incidencia = dR("incidencia").ToString
                SelladoDocumento = dR("numero_doc").ToString
                FechaAlta = dR("fecha_insert").ToString
                FechaAlta = dR("fechamodificacion").ToString
                NombreDocumento = dR("nombre_archivo").ToString
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
