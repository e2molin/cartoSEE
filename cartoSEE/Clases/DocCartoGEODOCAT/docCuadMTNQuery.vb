Public Class docCuadMTNQuery

    Property resultados As ArrayList
    Property consultaSQL As String
    Property resumenConsultaSQL As String

    'Respuestas devueltas
    Property OffsetResponse As Integer = 0
    Property LimitResponse As Integer = 0

    'Esta es la consulta base
    Sub New()
        resultados = New ArrayList
    End Sub

    Sub refreshQuery()
        resultados.Clear()
        rellenarDataset()
    End Sub

    Sub getByFiltroSQL(ByVal Filtro As String)

        _consultaSQL = $"SELECT archivodocmtn.idarchivodocmtn,archivodocmtn.tipo,archivodocmtn.subtipo,archivodocmtn.tomo,archivodocmtn.sellado,
                                        archivodocmtn.codprov,archivodocmtn.fecha,archivodocmtn.nota_fecha,archivodocmtn.pag,archivodocmtn.zona_num,
	                                archivodocmtn.subdivision_tipo,archivodocmtn.extraprops,archivodocmtn.encabezado,archivodocmtn.autor_entidad,
                                    archivodocmtn.subdivision_num,archivodocmtn.itin_tipo,archivodocmtn.itin_num,archivodocmtn.cuaderno,archivodocmtn.cuad_tipo, 
	                                archivodocmtn.anejos, archivodocmtn.nombre_old, archivodocmtn.nombre_new,archivodocmtn.signatura,archivodocmtn.observaciones,
	                                archivodocmtn.create_at,archivodocmtn.create_by,archivodocmtn.ambito,archivodocmtn.namefilecdd,archivodocmtn.fechafilecdd,
	                                archivodocmtn.observador,archivodocmtn.instrumentos,provincias.nombreprovincia,
                                                string_agg(territorios.idterritorio::text,'|') as idTerris,
                                                string_agg(Territorios.Nombre,'|') as nombreTerris,
                                                string_agg(Territorios.Tipo,'|') as tipoTerris,
                                                string_agg(Territorios.poligono_carto::text,'|') as poligonocarto,
                                                string_agg(Territorios.Municipio::text,'|') as muniTerris 
                                                FROM bdsidschema.archivodocmtn 
                                                LEFT JOIN bdsidschema.provincias on archivodocmtn.codprov= provincias.idprovincia 
                                                LEFT JOIN bdsidschema.archivodocmtn2terris ON archivodocmtn.idarchivodocmtn=archivodocmtn2terris.archivodocmtn_id 
                                                LEFT JOIN bdsidschema.territorios ON archivodocmtn2terris.territorio_id=territorios.idterritorio  
                                               WHERE {Filtro} 
                                                group by archivodocmtn.idarchivodocmtn,archivodocmtn.tipo,archivodocmtn.subtipo,archivodocmtn.tomo,
				                                archivodocmtn.sellado,
                                                archivodocmtn.codprov,archivodocmtn.fecha,archivodocmtn.nota_fecha,archivodocmtn.pag,archivodocmtn.zona_num,
				                                archivodocmtn.subdivision_tipo,
                                                archivodocmtn.subdivision_num,archivodocmtn.cuaderno,archivodocmtn.itin_tipo, archivodocmtn.itin_num, archivodocmtn.cuad_tipo,
				                                archivodocmtn.extraprops,
                                                archivodocmtn.encabezado,archivodocmtn.autor_entidad,
                                                archivodocmtn.anejos, archivodocmtn.nombre_old, archivodocmtn.nombre_new,archivodocmtn.signatura,archivodocmtn.observaciones,
				                                archivodocmtn.create_by,archivodocmtn.create_at,
                                                archivodocmtn.ambito,archivodocmtn.namefilecdd,archivodocmtn.fechafilecdd,provincias.nombreprovincia,
				                                archivodocmtn.instrumentos,archivodocmtn.observador"

        If LimitResponse > 0 Then _consultaSQL &= $" LIMIT {LimitResponse}"
        If OffsetResponse > 0 Then _consultaSQL &= $" OFFSET {OffsetResponse}"

        _resumenConsultaSQL = "getCuadernosInteriores_ByFiltroSQL:" & Filtro
        rellenarDataset()

    End Sub

    Private Sub rellenarDataset()

        Dim rcdDoc As DataTable
        Dim filas() As DataRow
        Dim terriDeslin As TerritorioBSID

        registrarDatabaseLog(_resumenConsultaSQL, _consultaSQL)

        rcdDoc = New DataTable
        If CargarRecordset(_consultaSQL, rcdDoc) = False Then
            rcdDoc = Nothing
            Exit Sub
        End If
        filas = rcdDoc.Select


        For Each dR As DataRow In filas
            'Esto es necesario mientras haya documentos con el documento Principal sin especificar
            Dim item As New docCuadMTN

            item.IdarchivodocMTN = dR("idarchivodocMTN")
            item.Sellado = dR("sellado")
            item.Tipo = dR("tipo").ToString
            item.Subtipo = dR("subtipo").ToString
            item.Tomo = dR("tomo").ToString
            item.FechaDoc = dR("fecha").ToString
            item.FechaDocType = dR("nota_fecha").ToString

            Try
                item.ProvinciaINE = dR("codprov")
                'item.ficheroPDF = rutaRepoCI & String.Format("{0:00}", item.ProvinciaINE).ToString & "\CMTN" & String.Format("{0:00000000}", dR("sellado")) & ".pdf"
                item.ficheroPDF = $"{rutaRepoCI}_pdf\{String.Format("{0:00}", item.ProvinciaINE)}\CMTN{String.Format("{0:00000000}", dR("sellado"))}.pdf"

            Catch ex As Exception
                GenerarLOG("El documento no tiene provincia asignada:" & dR("sellado").ToString)
            End Try


            item.Observaciones = dR("observaciones").ToString

            item.Create_at = dR("create_at").ToString
            item.Create_By = dR("create_by").ToString

            'InfoCDD
            item.ProductoCDD = "Cuadernos topográficos (interiores)"
            item.NameFileCDD = dR("namefilecdd").ToString
            item.FechaFileCDD = dR("fechafilecdd").ToString
            item.cddURL = $"https://centrodedescargas.cnig.es/CentroDescargas/busquedaIdProductor.do?idProductor={item.Sellado}&Serie=CCINT"

            'Contenido
            item.Cuaderno = dR("cuaderno").ToString
            item.CuadernoType = dR("cuad_tipo").ToString
            item.DivZona = dR("zona_num").ToString
            item.SubDivType = dR("subdivision_tipo").ToString
            item.SubDivNum = dR("subdivision_num").ToString
            item.ItinType = dR("itin_tipo").ToString
            item.ItinNum = dR("itin_num").ToString
            item.Ambito = dR("ambito").ToString
            item.extraProps.propertyCode = dR("extraprops")


            item.Observador = dR("observador").ToString
            item.Instrumentos = dR("instrumentos").ToString
            item.AutorEntidad = dR("autor_entidad").ToString '"Instituto Geográfico y Estadístico"
            item.Encabezado = dR("encabezado").ToString '"Trabajos Topográficos"
            item.Signatura = dR("signatura").ToString '"Archivo Compacto"

            Dim idTerris() As String = dR("idTerris").ToString.Split("|")
            Dim nombreTerris() As String = dR("nombreTerris").ToString.Split("|")
            Dim tipoTerris() As String = dR("tipoTerris").ToString.Split("|")
            Dim muniTerris() As String = dR("muniTerris").ToString.Split("|")
            Dim poligosCarto() As String = dR("poligonocarto").ToString.Split("|")


            For Each elem As String In dR("idTerris").ToString.Split("|")
                item.listaTerritorios.Add(New TerritorioBSID(CType(elem, Integer)))
            Next

            item.Anejos = dR("anejos").ToString
            item.NombreOLD = dR("nombre_old").ToString
            item.NombreNEW = dR("nombre_new").ToString
            item.ProvinciaNombre = dR("nombreprovincia").ToString

            resultados.Add(item)
            item = Nothing


        Next

    End Sub

End Class
