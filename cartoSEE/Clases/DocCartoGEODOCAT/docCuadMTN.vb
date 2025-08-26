Public Class docCuadMTN
    Property IdarchivodocMTN As Integer
    Property Sellado As Integer                             'Sellado
    Property Tipo As String
    Property Subtipo As String
    Property Tomo As String
    Property ProceHoja As String
    Property ProceCarpeta As String
    Property Signatura As String
    Property FechaDoc As String
    Property FechaDocType As String
    Property Encabezado As String
    Property AutorEntidad As String
    Property Observador As String
    Property Instrumentos As String
    Property NumPag As Integer
    Property ProvinciaINE As Integer
    Property ProvinciaNombre As String
    Property DivZona As String
    Property SubDivType As String
    Property SubDivNum As String
    Property ItinType As String
    Property ItinNum As String
    Property Cuaderno As String
    Property CuadernoType As String
    Property OldName As String
    Property NewName As String
    Property Create_at As String
    Property Create_By As String
    Property FechaModificacion As String
    Property Ambito As String
    Property Observaciones As String
    Property ficheroPDF As String
    Property listaTerritorios As New ArrayList
    Property Anejos As String
    Property NombreOLD As String
    Property NombreNEW As String

    Property extraProps As New FlagsPropertiesCuaderno

    ' Referencias al CdD
    Property ProductoCDD As String
    Property NameFileCDD As String ' Si el documento ya está en el CdD, esta propiedad almacena el nombre con el que se ha pasado. Si nunca ha estado, es nulo
    Property FechaFileCDD As String
    Property cddURL As String
    Property esDistribuiblePorCDD As Boolean = True

    Property TaxonomiaWebSemanticaCode As String = "2.4.1.1.2"
    Property TaxonomiaWebSemanticaName As String = "Actas, cuadernos, reseñas y gráficos de líneas límite"

    Property historialCambios As New ArrayList
    Dim historialConsultado As Boolean = False



    'Propiedades temporales que uso en Runtime
    ReadOnly Property nameFile4CDD() As String

        Get
            Return $"CMTN{String.Format("{0:00000000}", Sellado)}.pdf"
        End Get
    End Property

    ReadOnly Property SelladoIdProductor() As String

        Get
            Return $"CMTN{String.Format("{0:00000000}", Sellado)}"
        End Get
    End Property

    ReadOnly Property Contenido() As String

        Get
            Dim cadReturn As String = ""
            cadReturn &= IIf(DivZona <> "", $"Zona {DivZona}. ", "Zona no definida. ")
            cadReturn &= IIf(SubDivType <> "", $"{SubDivType}: {SubDivNum}. ", "")
            cadReturn &= IIf(Cuaderno <> "", $"Cuaderno {Cuaderno}. ", "Cuaderno no definido. ")
            cadReturn &= IIf(CuadernoType <> "", $"{CuadernoType}. ", "")
            cadReturn &= IIf(ItinType <> "" And ItinNum <> "", $"{ItinType}: {ItinNum}. ", "")
            Return cadReturn
        End Get
    End Property

    ReadOnly Property rutaFicheroPDF() As String

        Get
            Return $"{rutaRepoCI}\_pdf\{String.Format("{0:00}", ProvinciaINE)}\{SelladoIdProductor}.pdf"
        End Get
    End Property

    ReadOnly Property rutaFicheroThumb() As String

        Get
            Return $"{rutaRepoCI}_Miniaturas\{String.Format("{0:00}", ProvinciaINE)}\{SelladoIdProductor}.jpg"
        End Get
    End Property

    ReadOnly Property rutaFicheroAltaRes() As String

        Get
            Return $""
        End Get
    End Property

    ReadOnly Property rutaFicheroBajaRes() As String

        Get
            Return $""
        End Get
    End Property





    ReadOnly Property Alias4CDD() As String


        Get
            'Opción 1
            'cadOUT = $"C. interior planimetría {sellado}. {getListaNombresTerritoriosCDD()}. {firmas}"
            'If cadOUT.Length > 200 Then
            '    cadOUT = $"C. interior planimetría {sellado}. Varios municipios. {firmas}"
            'End If

            'Opción 2
            Dim comentarioAbreviado As String = ""
            Dim cadOUT As String


            If Ambito = "" Then
                If DivZona <> "" Then comentarioAbreviado = $"Zona {DivZona}"
                If SubDivType <> "" Then comentarioAbreviado = $". {SubDivType} {SubDivNum}"
                If Cuaderno <> "" And Cuaderno <> "Sin número" Then comentarioAbreviado &= $". Cuad.{Cuaderno}"
                If ItinNum <> "" Then comentarioAbreviado &= $". Itin {ItinNum}"
                If Observaciones <> "" Then comentarioAbreviado &= $". {Observaciones}"
                comentarioAbreviado = Trim(comentarioAbreviado)
            Else
                comentarioAbreviado = Ambito.Replace("Itinerarios", "Itin.").Replace("Itinerario", "Itin.").Replace("número ", "").Replace("Cuaderno ", "Cuad.").Replace("cuaderno", "cuad.").Replace("  ", "").Replace("..", ".").Trim
            End If
            'comentarioAbreviado = Ambito.Replace("Itinerarios", "Itiner.").Replace("Itinerario", "Itiner.").Replace("número", "nº").Replace("Cuaderno", "Cuad.").Replace("cuaderno", "cuad.").Replace("  ", "").Replace("..", ".").Trim
            cadOUT = $"Itin. {Sellado}. {getListaNombresTerritoriosCDDCuadernosInteriores(3)}{IIf(comentarioAbreviado.StartsWith("."), $"{comentarioAbreviado}", $". {comentarioAbreviado}")}"
            If cadOUT.Length > 200 Then cadOUT = cadOUT.Substring(0, 199)
            Return cadOUT
        End Get

    End Property





    Function getListaNombresTerritoriosCDD() As String

        Dim cadResult As String = ""
        For Each item As TerritorioBSID In listaTerritorios
            If cadResult <> "" Then cadResult &= $", {item.getNombreFull}" : Continue For
            cadResult = item.getNombreFull
        Next
        Return cadResult

    End Function

    Function getListaNombresTerritoriosCDDCuadernosInteriores(Optional separador As String = ", ", Optional maxNumberOfTerritories As Integer = 10) As String

        Dim cadResult As String = ""
        If listaTerritorios.Count > maxNumberOfTerritories Then Return "Varios territorios"
        For Each item As TerritorioBSID In listaTerritorios
            If cadResult <> "" Then cadResult &= $"{separador}{item.getNombreSencillo}" : Continue For
            cadResult = item.getNombreSencillo
        Next
        Return cadResult

    End Function

    Function getListaNombresTerritorios(Optional separador As String = ", ", Optional maxNumberOfTerritories As Integer = 10) As String

        Dim cadResult As String = ""
        If listaTerritorios.Count > maxNumberOfTerritories Then Return "Varios territorios"
        For Each item As TerritorioBSID In listaTerritorios
            If cadResult <> "" Then cadResult &= $"{separador}{item.nombre}" : Continue For
            cadResult = item.nombre
        Next
        Return cadResult

    End Function



    Sub New()
        'territoriosPrincipal = New ArrayList
        'territoriosSecundarios = New ArrayList
    End Sub

    Sub New(idCuadernoMTN As Integer)

        'Dim consultaSQL As String = $"SELECT archivodocmtn.idarchivodocmtn,archivodocmtn.create_at,archivodocmtn.tipo,archivodocmtn.subtipo,archivodocmtn.tomo,archivodocmtn.sellado,
        '        archivodocmtn.codprov,archivodocmtn.fecha,archivodocmtn.nota_fecha,archivodocmtn.pag,archivodocmtn.zona_num,archivodocmtn.subdivision_tipo,archivodocmtn.extraprops,archivodocmtn.encabezado,archivodocmtn.autor_entidad,
        '        archivodocmtn.subdivision_num,archivodocmtn.itin_tipo,archivodocmtn.itin_num,archivodocmtn.cuaderno,archivodocmtn.cuad_tipo, archivodocmtn.anejos, archivodocmtn.nombre_old, archivodocmtn.nombre_new,archivodocmtn.signatura,
        '        archivodocmtn.observaciones,archivodocmtn.create_by,archivodocmtn.ambito,archivodocmtn.namefilecdd,archivodocmtn.fechafilecdd,archivodocmtn.observador,provincias.nombreprovincia,
        '        string_agg(territorios.idterritorio::text,'|') as idTerris,
        '        string_agg(Territorios.Nombre,'|') as nombreTerris,
        '        string_agg(Territorios.Tipo,'|') as tipoTerris,
        '        string_agg(Territorios.poligono_carto::text,'|') as poligonocarto,
        '        string_agg(Territorios.Municipio::text,'|') as muniTerris 
        '        FROM bdsidschema.archivodocmtn 
        '        LEFT JOIN bdsidschema.provincias on archivodocmtn.codprov= provincias.idprovincia 
        '        LEFT JOIN bdsidschema.archivodocmtn2terris ON archivodocmtn.idarchivodocmtn=archivodocmtn2terris.archivodocmtn_id 
        '        LEFT JOIN bdsidschema.territorios ON archivodocmtn2terris.territorio_id=territorios.idterritorio  
        '        WHERE archivodocmtn.idarchivodocmtn={idCuadernoMTN} 
        '        group by archivodocmtn.idarchivodocmtn,archivodocmtn.create_at,archivodocmtn.tipo,archivodocmtn.subtipo,archivodocmtn.tomo,archivodocmtn.sellado,
        '        archivodocmtn.codprov,archivodocmtn.fecha,archivodocmtn.nota_fecha,archivodocmtn.pag,archivodocmtn.zona_num,archivodocmtn.subdivision_tipo,
        '        archivodocmtn.subdivision_num,archivodocmtn.cuaderno,archivodocmtn.itin_tipo, archivodocmtn.itin_num, archivodocmtn.cuad_tipo,archivodocmtn.extraprops,
        '        archivodocmtn.encabezado,archivodocmtn.autor_entidad,
        '        archivodocmtn.anejos, archivodocmtn.nombre_old, archivodocmtn.nombre_new,archivodocmtn.signatura,archivodocmtn.observaciones,archivodocmtn.create_by,
        '        archivodocmtn.ambito,archivodocmtn.namefilecdd,archivodocmtn.fechafilecdd,provincias.nombreprovincia"

        Dim consultaSQL As String = $"SELECT archivodocmtn.idarchivodocmtn,archivodocmtn.tipo,archivodocmtn.subtipo,archivodocmtn.tomo,archivodocmtn.sellado,
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
                                               WHERE archivodocmtn.idarchivodocmtn={idCuadernoMTN} 
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
        rellenarDataset(consultaSQL)

    End Sub

    Private Sub rellenarDataset(consultaSQL As String)

        Dim rcdDoc As DataTable
        Dim filas() As DataRow
        Dim terriDeslin As TerritorioBSID

        rcdDoc = New DataTable
        If Not CargarRecordset(consultaSQL, rcdDoc) Then
            rcdDoc = Nothing
            Exit Sub
        End If

        filas = rcdDoc.Select
        For Each dR As DataRow In filas
            'Esto es necesario mientras haya documentos con el documento Principal sin especificar

            IdarchivodocMTN = dR("idarchivodocMTN")
            Sellado = dR("sellado")
            Tipo = dR("tipo").ToString
            Subtipo = dR("subtipo").ToString
            Tomo = dR("tomo").ToString
            FechaDoc = dR("fecha").ToString
            FechaDocType = dR("nota_fecha").ToString
            'item.FechaModificacion = dR("fechamodificacion").ToString

            'item.docType4HR = dR("doctypehr").ToString

            Try
                ProvinciaINE = dR("codprov")
                'item.ficheroPDF = rutaRepoCI & String.Format("{0:00}", item.ProvinciaINE).ToString & "\CMTN" & String.Format("{0:00000000}", dR("sellado")) & ".pdf"
                ficheroPDF = $"{rutaRepoCI}{String.Format("{0:00}", ProvinciaINE)}\CMTN{String.Format("{0:00000000}", dR("sellado"))}.pdf"

            Catch ex As Exception
                GenerarLOG("El documento no tiene provincia asignada:" & dR("sellado").ToString)
            End Try


            Observaciones = dR("observaciones").ToString

            Create_at = dR("create_at").ToString
            Create_By = dR("create_by").ToString

            'InfoCDD
            ProductoCDD = "Cuadernos topográficos (interiores)"
            NameFileCDD = dR("namefilecdd").ToString
            FechaFileCDD = dR("fechafilecdd").ToString
            cddURL = $"https://centrodedescargas.cnig.es/CentroDescargas/busquedaIdProductor.do?idProductor={Sellado}&Serie=CCINT"

            'Contenido
            Cuaderno = dR("cuaderno").ToString
            CuadernoType = dR("cuad_tipo").ToString
            DivZona = dR("zona_num").ToString
            SubDivType = dR("subdivision_tipo").ToString
            SubDivNum = dR("subdivision_num").ToString
            ItinType = dR("itin_tipo").ToString
            ItinNum = dR("itin_num").ToString
            Ambito = dR("ambito").ToString
            extraProps.propertyCode = dR("extraprops")


            Observador = dR("observador").ToString
            Instrumentos = dR("instrumentos").ToString
            AutorEntidad = dR("autor_entidad").ToString '"Instituto Geográfico y Estadístico"
            Encabezado = dR("encabezado").ToString '"Trabajos Topográficos"
            Signatura = dR("signatura").ToString '"Archivo Compacto"

            Dim idTerris() As String = dR("idTerris").ToString.Split("|")
            Dim nombreTerris() As String = dR("nombreTerris").ToString.Split("|")
            Dim tipoTerris() As String = dR("tipoTerris").ToString.Split("|")
            Dim muniTerris() As String = dR("muniTerris").ToString.Split("|")
            Dim poligosCarto() As String = dR("poligonocarto").ToString.Split("|")


            For Each elem As String In dR("idTerris").ToString.Split("|")
                listaTerritorios.Add(New TerritorioBSID(CType(elem, Integer)))
            Next

            Anejos = dR("anejos").ToString
            NombreOLD = dR("nombre_old").ToString
            NombreNEW = dR("nombre_new").ToString
            ProvinciaNombre = dR("nombreprovincia").ToString

        Next
        rcdDoc.Dispose()
        rcdDoc = Nothing



    End Sub

    Sub cargarHistorial()

        If historialConsultado = True Then Exit Sub

        Dim rcdHistorial As New DataTable
        Dim filas As DataRow()
        Dim edicion As docCartoSEEVariacion
        Try
            If Not CargarRecordset($"SELECT * from bdsidschema.archivodocmtnlog WHERE archivodocmtn_id={IdarchivodocMTN}", rcdHistorial) Then
                Exit Try
            End If
            filas = rcdHistorial.Select
            historialCambios.Clear()
            For Each fila As DataRow In filas
                edicion = New docCartoSEEVariacion
                edicion.usuario = fila("usuario_update").ToString
                edicion.fechaVariacion = fila("fecha_update")
                edicion.tipoVariacion = fila("tipo_variacion").ToString
                edicion.valorOld = fila("valor_old").ToString
                edicion.ValorNew = fila("valor_new").ToString
                historialCambios.Add(edicion)
                edicion = Nothing
            Next
        Catch ex As Exception
            Application.DoEvents()
        Finally
            Erase filas
            filas = Nothing
            rcdHistorial.Dispose()
            rcdHistorial = Nothing
            historialConsultado = True
        End Try

    End Sub

End Class
