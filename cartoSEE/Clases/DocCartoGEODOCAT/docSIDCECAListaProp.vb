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
    Property NombreProvincia As String
    Property CodProv As Integer
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


    Sub New(idListProp As Integer)


        Dim consultaSQL As String = $"SELECT idlistaprop,ayuntamiento,part_jud,numcoleccion,COALESCE(coleccion,'Única') as coleccion,
	                                    t1.nombre as munihisto, t1.munihisto as codmunihisto,
	                                    t2.nombre as muniactual,t2.municipio as codmuniactual,
	                                    provincias.nombreprovincia,provincias.idprovincia,listaprop.dataset_tbl as dataset_tbl
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


                'Tomo = dR("tomo").ToString
                'FechaDoc = dR("fecha").ToString
                'FechaDocType = dR("nota_fecha").ToString
                ''item.FechaModificacion = dR("fechamodificacion").ToString

                ''item.docType4HR = dR("doctypehr").ToString

                'Try
                '    ProvinciaINE = dR("codprov")
                '    'item.ficheroPDF = rutaRepoCI & String.Format("{0:00}", item.ProvinciaINE).ToString & "\CMTN" & String.Format("{0:00000000}", dR("sellado")) & ".pdf"
                '    ficheroPDF = $"{rutaRepoCI}{String.Format("{0:00}", ProvinciaINE)}\CMTN{String.Format("{0:00000000}", dR("sellado"))}.pdf"

                'Catch ex As Exception
                '    GenerarLOG("El documento no tiene provincia asignada:" & dR("sellado").ToString)
                'End Try


                'Observaciones = dR("observaciones").ToString

                'Create_at = dR("create_at").ToString
                'Create_By = dR("create_by").ToString

                ''InfoCDD
                'ProductoCDD = "Cuadernos topográficos (interiores)"
                'NameFileCDD = dR("namefilecdd").ToString
                'FechaFileCDD = dR("fechafilecdd").ToString
                'cddURL = $"https://centrodedescargas.cnig.es/CentroDescargas/busquedaIdProductor.do?idProductor={Sellado}&Serie=CCINT"

                ''Contenido
                'Cuaderno = dR("cuaderno").ToString
                'CuadernoType = dR("cuad_tipo").ToString
                'DivZona = dR("zona_num").ToString
                'SubDivType = dR("subdivision_tipo").ToString
                'SubDivNum = dR("subdivision_num").ToString
                'ItinType = dR("itin_tipo").ToString
                'ItinNum = dR("itin_num").ToString
                'Ambito = dR("ambito").ToString
                'extraProps.propertyCode = dR("extraprops")


                'Observador = dR("observador").ToString
                'Instrumentos = dR("instrumentos").ToString
                'AutorEntidad = dR("autor_entidad").ToString '"Instituto Geográfico y Estadístico"
                'Encabezado = dR("encabezado").ToString '"Trabajos Topográficos"
                'Signatura = dR("signatura").ToString '"Archivo Compacto"

                'Dim idTerris() As String = dR("idTerris").ToString.Split("|")
                'Dim nombreTerris() As String = dR("nombreTerris").ToString.Split("|")
                'Dim tipoTerris() As String = dR("tipoTerris").ToString.Split("|")
                'Dim muniTerris() As String = dR("muniTerris").ToString.Split("|")
                'Dim poligosCarto() As String = dR("poligonocarto").ToString.Split("|")


                'For Each elem As String In dR("idTerris").ToString.Split("|")
                '    listaTerritorios.Add(New TerritorioBSID(CType(elem, Integer)))
                'Next

                'Anejos = dR("anejos").ToString
                'NombreOLD = dR("nombre_old").ToString
                'NombreNEW = dR("nombre_new").ToString
                'ProvinciaNombre = dR("nombreprovincia").ToString

                'For iBucle As Integer = 0 To idTerris.Count - 1
                '    Try
                '        Application.DoEvents()
                '        If tipoTerris(iBucle) = "Condominio" Then
                '            terriDeslin = New TerritorioBSID(idTerris(iBucle))
                '        ElseIf tipoTerris(iBucle) = "Accidente geográfico" Then
                '            terriDeslin = New TerritorioBSID(idTerris(iBucle))
                '        ElseIf tipoTerris(iBucle) = "Condominio histórico" Then
                '            Continue For
                '        ElseIf tipoTerris(iBucle) = "Desconocido" Then
                '            Continue For
                '        ElseIf tipoTerris(iBucle) = "País" Then
                '            terriDeslin = New TerritorioBSID
                '            terriDeslin.indice = idTerris(iBucle)
                '            terriDeslin.territorioNombre = nombreTerris(iBucle)
                '            terriDeslin.tipo = tipoTerris(iBucle)
                '            terriDeslin.codigoINE = muniTerris(iBucle)
                '            terriDeslin.municipioNombre = "No procede"
                '            terriDeslin.centroide_id = 0
                '            terriDeslin.provinciaINE = "99"
                '            terriDeslin.autonomiaINE = "20"
                '        Else
                '            terriDeslin = New TerritorioBSID
                '            terriDeslin.indice = idTerris(iBucle)
                '            terriDeslin.territorioNombre = nombreTerris(iBucle)
                '            terriDeslin.tipo = tipoTerris(iBucle)
                '            terriDeslin.codigoINE = muniTerris(iBucle)
                '            terriDeslin.municipioNombre = DameMunicipioByINE(muniTerris(iBucle))
                '            terriDeslin.centroide_id = poligosCarto(iBucle)
                '            terriDeslin.provinciaINE = String.Format("{0:00000}", CType(muniTerris(iBucle), Integer)).Substring(0, 2)
                '            terriDeslin.autonomiaINE = ListaProvincias.Rows(terriDeslin.provinciaINE - 1).Item(2).ToString()
                '        End If
                '        item.territorios.Add(terriDeslin)
                'Catch ex As Exception
                '        GenerarLOG("Error:" & ex.Message)
                '    Finally
                '        terriDeslin = Nothing
                '    End Try
                'Next
                'resultados.Add(item)
                'item = Nothing
            Next

        Catch ex As Exception
            ModalError(ex.Message)
        Finally
            rcdDoc.Dispose()
            rcdDoc = Nothing
        End Try







    End Sub


End Class
