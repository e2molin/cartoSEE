Public Class docCartoSEEquery
    Property resultados As ArrayList
    Property consultaSQL As String
    Property resumenConsultaSQL As String
    Property flag_ActualizarInfoGeom As Boolean = False
    Property flag_CargarFicherosGEO As Boolean = False
    'Filtros
    Property Año As String = ""
    Property TipoDoc As String = ""
    Property Tomo As String = ""
    Property DocAnulado As String = ""
    Property DocAdicional As String = ""
    Property Comentario As String = ""

    Const consultaSQLBaseWithContour As String =
        "SELECT archivo.idarchivo,archivo.numdoc,archivo.escala,archivo.tomo,archivo.coleccion,archivo.subdivision,archivo.fechaprincipal," &
                "archivo.fechasmodificaciones,archivo.anejo,archivo.vertical,archivo.horizontal,archivo.tipodoc_id,archivo.estadodoc_id,archivo.procecarpeta,archivo.procehoja," &
                "archivo.subtipo,archivo.juntaestadistica,archivo.signatura,archivo.observestandar_id,archivo.extraprops,archivo.observaciones," &
                "archivo.cdd_nomfich,archivo.cdd_url,archivo.cdd_producto,archivo.titn,archivo.autor,archivo.encabezado," &
                "tbtipodocumento.tipodoc as Tipo,tbestadodocumento.estadodoc as Estado, tbobservaciones.observestandar," &
                "string_agg(munihisto.nombremunicipiohistorico,'#') as listaMuniHisto,string_agg(to_char(munihisto.cod_munihisto, 'FM0000009'::text),'#') as listaCodMuniHisto," &
                "string_agg(listamunicipios.nombre,'#') as listaMuniActual, string_agg(listamunicipios.inecorto,'#') as listaCodMuniActual, string_agg(provincias.nombreprovincia,'#') as nombreprovincia " &
        "FROM bdsidschema.archivo " &
            "INNER JOIN bdsidschema.tbtipodocumento ON tbtipodocumento.idtipodoc=archivo.tipodoc_id " &
            "INNER JOIN bdsidschema.tbestadodocumento ON tbestadodocumento.idestadodoc=archivo.estadodoc_id " &
            "INNER JOIN  bdsidschema.archivo2munihisto  ON archivo2munihisto.archivo_id=archivo.idarchivo " &
            "LEFT JOIN  bdsidschema.tbobservaciones  ON tbobservaciones.idobservestandar=archivo.observestandar_id " &
            "INNER JOIN bdsidschema.munihisto on munihisto.idmunihisto= archivo2munihisto.munihisto_id " &
            "LEFT JOIN ngmepschema.listamunicipios on munihisto.entidad_id= listamunicipios.identidad " &
            "INNER JOIN bdsidschema.provincias on munihisto.provincia_id= provincias.idprovincia " &
            "LEFT JOIN bdsidschema.contornos ON archivo.idarchivo=contornos.archivo_id "

    Const consultaSQLBaseWithoutContour As String =
        "SELECT archivo.idarchivo,archivo.numdoc,archivo.escala,archivo.tomo,archivo.coleccion,archivo.subdivision,archivo.fechaprincipal," &
                "archivo.fechasmodificaciones,archivo.anejo,archivo.vertical,archivo.horizontal,archivo.tipodoc_id,archivo.estadodoc_id,archivo.procecarpeta,archivo.procehoja," &
                "archivo.subtipo,archivo.juntaestadistica,archivo.signatura,archivo.observestandar_id,archivo.extraprops,archivo.observaciones," &
                "archivo.cdd_nomfich,archivo.cdd_url,archivo.cdd_producto,archivo.titn,archivo.autor,archivo.encabezado," &
                "tbtipodocumento.tipodoc as Tipo,tbestadodocumento.estadodoc as Estado, tbobservaciones.observestandar," &
                "string_agg(munihisto.nombremunicipiohistorico,'#') as listaMuniHisto,string_agg(to_char(munihisto.cod_munihisto, 'FM0000009'::text),'#') as listaCodMuniHisto," &
                "string_agg(listamunicipios.nombre,'#') as listaMuniActual, string_agg(listamunicipios.inecorto,'#') as listaCodMuniActual, string_agg(provincias.nombreprovincia,'#') as nombreprovincia " &
        "FROM bdsidschema.archivo " &
            "INNER JOIN bdsidschema.tbtipodocumento ON tbtipodocumento.idtipodoc=archivo.tipodoc_id " &
            "INNER JOIN bdsidschema.tbestadodocumento ON tbestadodocumento.idestadodoc=archivo.estadodoc_id " &
            "INNER JOIN  bdsidschema.archivo2munihisto  ON archivo2munihisto.archivo_id=archivo.idarchivo " &
            "LEFT JOIN  bdsidschema.tbobservaciones  ON tbobservaciones.idobservestandar=archivo.observestandar_id " &
            "INNER JOIN bdsidschema.munihisto on munihisto.idmunihisto= archivo2munihisto.munihisto_id " &
            "LEFT JOIN ngmepschema.listamunicipios on munihisto.entidad_id= listamunicipios.identidad " &
            "INNER JOIN bdsidschema.provincias on munihisto.provincia_id= provincias.idprovincia "



    Sub New()

        resultados = New ArrayList

    End Sub

    Sub getDocsGEODOCAT_WithoutContour(ByVal Filtro As String)

        _consultaSQL = consultaSQLBaseWithoutContour &
                    "WHERE " & Filtro & " " &
                      "group by archivo.idarchivo,archivo.numdoc,archivo.escala,archivo.tomo,archivo.coleccion,archivo.subdivision,archivo.fechaprincipal," &
                      "archivo.fechasmodificaciones,archivo.anejo,archivo.vertical, archivo.horizontal, archivo.tipodoc_id, archivo.estadodoc_id, archivo.procecarpeta, " &
                      "archivo.procehoja, archivo.subtipo,archivo.juntaestadistica, archivo.signatura, archivo.observestandar_id,archivo.extraprops , archivo.observaciones,tbtipodocumento.tipodoc," &
                    "archivo.cdd_nomfich,archivo.cdd_url,archivo.cdd_producto,archivo.titn,archivo.autor,archivo.encabezado,tbestadodocumento.estadodoc, tbobservaciones.observestandar order by archivo.idarchivo"


        rellenarDataset()

    End Sub



    Sub getDocsSIDDAE_ByFiltroSellado(ByVal Filtro As String)

        _consultaSQL = consultaSQLBaseWithoutContour &
                    "WHERE " & Filtro & " " &
                      "group by archivo.idarchivo,archivo.numdoc,archivo.escala,archivo.tomo,archivo.coleccion,archivo.subdivision,archivo.fechaprincipal," &
                      "archivo.fechasmodificaciones,archivo.anejo,archivo.vertical, archivo.horizontal, archivo.tipodoc_id, archivo.estadodoc_id, archivo.procecarpeta, " &
                      "archivo.procehoja, archivo.subtipo,archivo.juntaestadistica, archivo.signatura, archivo.observestandar_id,archivo.extraprops, archivo.observaciones,tbtipodocumento.tipodoc," &
                    "archivo.cdd_nomfich,archivo.cdd_url,archivo.cdd_producto,archivo.titn,archivo.autor,archivo.encabezado,tbestadodocumento.estadodoc, tbobservaciones.observestandar order by archivo.idarchivo"


        rellenarDataset()

    End Sub

    Sub getDocsSIDDAE_ByFiltroSQL(ByVal Filtro As String)

        _consultaSQL = consultaSQLBaseWithoutContour &
                    "WHERE archivo.idarchivo in (" &
                        "SELECT distinct archivo.idarchivo FROM bdsidschema.archivo " &
                        "INNER JOIN bdsidschema.archivo2munihisto ON archivo2munihisto.archivo_id=archivo.idarchivo " &
                        "INNER JOIN bdsidschema.munihisto on munihisto.idmunihisto= archivo2munihisto.munihisto_id " &
                        "LEFT JOIN ngmepschema.listamunicipios on munihisto.entidad_id= listamunicipios.identidad " &
                        "WHERE " & Filtro & " " &
                    ") " &
                    "group by archivo.idarchivo,archivo.numdoc,archivo.escala,archivo.tomo,archivo.coleccion,archivo.subdivision,archivo.fechaprincipal," &
                    "archivo.fechasmodificaciones,archivo.anejo,archivo.vertical, archivo.horizontal, archivo.tipodoc_id, archivo.estadodoc_id, archivo.procecarpeta, " &
                    "archivo.procehoja, archivo.subtipo,archivo.juntaestadistica, archivo.signatura, archivo.observestandar_id,archivo.extraprops, archivo.observaciones,tbtipodocumento.tipodoc," &
                    "archivo.cdd_nomfich,archivo.cdd_url,archivo.cdd_producto,archivo.titn,archivo.autor,archivo.encabezado,tbestadodocumento.estadodoc, tbobservaciones.observestandar  order by archivo.idarchivo"


        rellenarDataset()
    End Sub

    Sub getDocsSIDDAE_ByFiltroSQLwithPatron(ByVal Filtro As String, propsPatron As String)


        _consultaSQL = consultaSQLBaseWithoutContour &
                    "WHERE archivo.idarchivo in (" &
                        "with dataprops as (" &
                                    "select idarchivo," &
                                    "CASE" &
                                        "WHEN bdsidschema.number_to_base(extraprops,2) is null THEN repeat('0',16)" &
                                        "ELSE repeat('0',16 - length(bdsidschema.number_to_base(extraprops,2))) || bdsidschema.number_to_base(extraprops,2)" &
                                    "END as patron" &
                                    "from bdsidschema.archivo " &
                                    "INNER JOIN bdsidschema.archivo2munihisto ON archivo2munihisto.archivo_id=archivo.idarchivo " &
                                    "INNER JOIN bdsidschema.munihisto on munihisto.idmunihisto= archivo2munihisto.munihisto_id " &
                                    "LEFT JOIN ngmepschema.listamunicipios on munihisto.entidad_id= listamunicipios.identidad " &
                                    "WHERE " & Filtro & " " &
                        ") select idarchivo from dataprops where patron like '" & propsPatron & "'" &
                    ")" &
                    "group by archivo.idarchivo,archivo.numdoc,archivo.escala,archivo.tomo,archivo.coleccion,archivo.subdivision,archivo.fechaprincipal," &
                    "archivo.fechasmodificaciones,archivo.anejo,archivo.vertical, archivo.horizontal, archivo.tipodoc_id, archivo.estadodoc_id, archivo.procecarpeta, " &
                    "archivo.procehoja, archivo.subtipo,archivo.juntaestadistica, archivo.signatura, archivo.observestandar_id,archivo.extraprops, archivo.observaciones,tbtipodocumento.tipodoc," &
                    "archivo.cdd_nomfich,archivo.cdd_url,archivo.cdd_producto,archivo.titn,archivo.autor,archivo.encabezado,tbestadodocumento.estadodoc, tbobservaciones.observestandar  order by archivo.idarchivo"



        rellenarDataset()
    End Sub

    Sub getDocsSIDDAE_ByEntorno(ByVal Xmax As Integer, ByVal Ymax As Integer, ByVal Xmin As Integer, ByVal Ymin As Integer)

        Dim CadenaSQLSpacial As String
        'Obtenemos las líneas límite dentro del entorno
        CadenaSQLSpacial = "SELECT archivo_id FROM bdsidschema.contornos WHERE ST_Intersects(" &
                            "contornos.geom," &
                            "ST_GeomFromText('POLYGON((" &
                            Xmin & " " & Ymax & "," &
                            Xmax & " " & Ymax & "," &
                            Xmax & " " & Ymin & "," &
                            Xmin & " " & Ymin & "," &
                            Xmin & " " & Ymax & "))',23030))"

        _consultaSQL = consultaSQLBaseWithoutContour &
                    "WHERE archivo.idarchivo in ( " &
                        CadenaSQLSpacial & " " &
                    ") " &
                    "group by archivo.idarchivo,archivo.numdoc,archivo.escala,archivo.tomo,archivo.coleccion,archivo.subdivision,archivo.fechaprincipal," &
                    "archivo.fechasmodificaciones,archivo.anejo,archivo.vertical, archivo.horizontal, archivo.tipodoc_id, archivo.estadodoc_id, archivo.procecarpeta, " &
                    "archivo.procehoja, archivo.subtipo,archivo.juntaestadistica, archivo.signatura, archivo.observestandar_id,archivo.extraprops, archivo.observaciones,tbtipodocumento.tipodoc," &
                    "archivo.cdd_nomfich,archivo.cdd_url,archivo.cdd_producto,archivo.titn,archivo.autor,archivo.encabezado,tbestadodocumento.estadodoc, tbobservaciones.observestandar  order by archivo.idarchivo"


        rellenarDataset()

    End Sub





    Sub getDocsCartoSEE_BySQLSentence(ByVal cadSQL As String)

        _consultaSQL = cadSQL
        rellenarDataset()

    End Sub

    Sub dataRefresh()
        resultados.Clear()
        resultados = Nothing
        resultados = New ArrayList
        rellenarDataset()
    End Sub



    ''' <summary>
    ''' Rellena un dataset con todos los datos
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub rellenarDataset()

        Dim rcdDoc As DataTable
        Dim filas() As DataRow
        Dim contador As Long
        Dim Anterior As Integer
        Dim item As docCartoSEE



        rcdDoc = New DataTable
        If CargarRecordset(_consultaSQL, rcdDoc) = False Then
            rcdDoc = Nothing
            Exit Sub
        End If
        filas = rcdDoc.Select
        contador = -1
        Anterior = 0
        For Each dR As DataRow In filas
            item = New docCartoSEE
            item.docIndex = dR("idarchivo").ToString
            item.Sellado = dR("numdoc").ToString
            If flag_ActualizarInfoGeom = True Then item.getFootprint()
            'Maquillamos el tomo
            If dR("tomo").ToString = "" Then
                item.Tomo = dR("tomo").ToString
            Else
                If IsNumeric(dR("tomo")) Then
                    item.Tomo = String.Format("{0:000}", CType(dR("tomo").ToString, Integer))
                Else
                    If IsNumeric(dR("tomo").ToString.Replace("BIS", "")) Then
                        item.Tomo = String.Format("{0:000}", CType(dR("tomo").ToString.Replace("BIS", ""), Integer)) & "BIS"
                    Else
                        item.Tomo = dR("tomo").ToString
                    End If
                End If
            End If
            If dR("fechaprincipal").ToString.Length >= 10 Then
                'ListaDoc(contador).fechaPrincipal = dR("fechaprincipal").ToString.Substring(0, 10)
                item.fechaPrincipal = FormatearFecha(dR("fechaprincipal"), "GERMAN")
            End If

            item.fechasModificaciones = dR("fechasmodificaciones").ToString
            item.Tipo = dR("Tipo").ToString
            item.CodTipo = dR("tipodoc_id").ToString
            For Each subItem As docCartoSEETipoDocu In tiposDocSIDCARTO
                If subItem.idTipodoc = dR("tipodoc_id").ToString Then
                    item.tipoDocumento = subItem
                    Exit For
                End If
            Next
            item.CodEstado = dR("estadodoc_id").ToString
            item.Estado = dR("Estado").ToString
            item.Escala = dR("Escala").ToString
            item.Vertical = dR("Vertical").ToString
            item.Horizontal = dR("Horizontal").ToString
            item.proceHoja = dR("Procehoja").ToString
            item.proceCarpeta = dR("Procecarpeta").ToString
            item.subTipoDoc = dR("subtipo").ToString
            item.NumDisco = "" 'dR("NumDisco").ToString
            item.Anejo = dR("Anejo").ToString
            item.Signatura = dR("Signatura").ToString
            item.Coleccion = dR("Coleccion").ToString
            item.Subdivision = dR("Subdivision").ToString
            item.cddProducto = dR("cdd_producto").ToString
            item.cddNombreFichero = dR("cdd_nomfich").ToString
            item.cddURL = dR("cdd_url").ToString
            item.titnABSYSdoc = dR("titn")
            If item.titnABSYSdoc > 0 Then
                item.cargaABSYS = True
                item.urlABSYSdoc = "https://www.ign.es/web/biblioteca_cartoteca/abnetcl.cgi?TITN=" & item.titnABSYSdoc
            Else
                item.cargaABSYS = False
                item.urlABSYSdoc = ""
            End If
            item.autorDocumento = dR("autor").ToString
            item.encabezadoABSYSdoc = dR("encabezado").ToString

            For Each elem As String In dR("listaMuniHisto").ToString.Split("#")
                item.listaMuniHistorico.Add(elem)
            Next
            For Each elem As String In dR("listaCodMuniHisto").ToString.Split("#")
                item.listaCodMuniHistorico.Add(elem)
            Next
            For Each elem As String In dR("listaMuniActual").ToString.Split("#")
                item.listaMuniActual.Add(elem)
            Next
            For Each elem As String In dR("listaCodMuniActual").ToString.Split("#")
                item.listaCodMuniActual.Add(elem)
            Next

            item.Provincias = dR("nombreprovincia").ToString
            item.ProvinciaRepo = CType(dR("numdoc").ToString.Substring(0, 2), Integer)
            item.FicheroJPG = DirRepoProvinciaByINE(item.ProvinciaRepo) & "\" & dR("numdoc").ToString & ".jpg"
            item.JuntaEstadistica = IIf(dR("JuntaEstadistica").ToString = "1", "Sí", "No")
            item.ObservacionesStandard = dR("observestandar").ToString
            item.Observaciones = dR("observaciones").ToString
            item.extraProps.propertyCode = dR("extraprops")

            If flag_CargarFicherosGEO Then
                item.getGeoFiles()
                item.getGeoFilesFromDatabase()
            End If

            resultados.Add(item)
            item = Nothing
        Next
        rcdDoc.Dispose()
        rcdDoc = Nothing



    End Sub

End Class
