Public Class docCuadMTN
    Property IdarchivodocMTN As Integer
    Property Sellado As Integer                             'Sellado
    Property Tipo As String
    Property Subtipo As String
    Property Tomo As String
    Property FechaDoc As String
    Property FechaDocType As String
    Property NumPag As Integer
    Property ProvinciaINE As Integer
    Property DivZona As String
    Property SubDivType As String
    Property SubDivNum As String
    Property ItinType As String
    Property ItinNum As String
    Property Cuaderno As String
    Property CuadernoType As String
    Property OldName As String
    Property NewName As String
    Property Anejos As String
    Property Create_at As String
    Property UserAlta As String
    Property FechaModificacion As String
    Property Ambito As String
    Property Observaciones As String
    Property ficheroPDF As String
    Property listaTerritorios As New ArrayList

    Property NameFileCDD As String ' Si el documento ya está en el CdD, esta propiedad almacena el nombre con el que se ha pasado. Si nunca ha estado, es nulo
    Property FechaFileCDD As String

    Property TaxonomiaWebSemanticaCode As String = "2.4.1.1.2"
    Property TaxonomiaWebSemanticaName As String = "Actas, cuadernos, reseñas y gráficos de líneas límite "

    'Propiedades temporales que uso en Runtime
    Property esDistribuiblePorCDD As Boolean = True


    ReadOnly Property ProvinciaNombre() As String
        Get
            Return ListaProvincias.Rows(ProvinciaINE - 1).Item(1).ToString
        End Get
    End Property

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

    Function getListaNombresTerritoriosCDDCuadernosInteriores(maxNumberOfTerritories) As String

        Dim cadResult As String = ""
        If listaTerritorios.Count > maxNumberOfTerritories Then Return "Varios territorios"
        For Each item As TerritorioBSID In listaTerritorios
            If cadResult <> "" Then cadResult &= $", {item.getNombreSencillo}" : Continue For
            cadResult = item.getNombreSencillo
        Next
        Return cadResult

    End Function

    Sub New()
        'territoriosPrincipal = New ArrayList
        'territoriosSecundarios = New ArrayList
    End Sub

    Sub New(idCuadernoMTN As Integer)

        Dim consultaSQL As String = $"SELECT archivodocmtn.idarchivodocmtn,archivodocmtn.create_at,archivodocmtn.tipo,archivodocmtn.subtipo,archivodocmtn.tomo,archivodocmtn.sellado,
                archivodocmtn.codprov,archivodocmtn.fecha,archivodocmtn.nota_fecha,archivodocmtn.pag,archivodocmtn.zona_num,archivodocmtn.subdivision_tipo,
                archivodocmtn.subdivision_num,archivodocmtn.itin_tipo,archivodocmtn.itin_num,archivodocmtn.cuaderno,archivodocmtn.cuad_tipo, archivodocmtn.anejos, archivodocmtn.nombre_old, archivodocmtn.nombre_new,
                archivodocmtn.observaciones,archivodocmtn.create_by,archivodocmtn.ambito,archivodocmtn.namefilecdd,archivodocmtn.fechafilecdd,
                string_agg(territorios.idterritorio::text,'|') as idTerris,
                string_agg(Territorios.Nombre,'|') as nombreTerris,
                string_agg(Territorios.Tipo,'|') as tipoTerris,
                string_agg(Territorios.poligono_carto::text,'|') as poligonocarto,
                string_agg(Territorios.Municipio::text,'|') as muniTerris 
                FROM bdsidschema.archivodocmtn 
                INNER JOIN bdsidschema.archivodocmtn2terris ON archivodocmtn.idarchivodocmtn=archivodocmtn2terris.archivodocmtn_id 
                INNER JOIN bdsidschema.territorios ON archivodocmtn2terris.territorio_id=territorios.idterritorio  
                WHERE {idCuadernoMTN} 
                group by archivodocmtn.idarchivodocmtn,archivodocmtn.create_at,archivodocmtn.tipo,archivodocmtn.subtipo,archivodocmtn.tomo,archivodocmtn.sellado,
                archivodocmtn.codprov,archivodocmtn.fecha,archivodocmtn.nota_fecha,archivodocmtn.pag,archivodocmtn.zona_num,archivodocmtn.subdivision_tipo,
                archivodocmtn.subdivision_num,archivodocmtn.cuaderno,archivodocmtn.itin_tipo, archivodocmtn.itin_num, archivodocmtn.cuad_tipo, 
                archivodocmtn.anejos, archivodocmtn.nombre_old, archivodocmtn.nombre_new,archivodocmtn.observaciones,archivodocmtn.create_by,archivodocmtn.ambito,archivodocmtn.namefilecdd,archivodocmtn.fechafilecdd"

        rellenarDataset(consultaSQL)

    End Sub

    Private Sub rellenarDataset(consultaSQL As String)

        Dim rcdDoc As DataTable
        Dim filas() As DataRow
        Dim contador As Long
        Dim Anterior As Integer
        Dim terriDeslin As TerritorioBSID

        rcdDoc = New DataTable
        If Not CargarRecordset(consultaSQL, rcdDoc) Then
            rcdDoc = Nothing
            Exit Sub
        End If

        filas = rcdDoc.Select
        contador = -1
        Anterior = 0
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

            NumPag = dR("pag")
            Observaciones = dR("observaciones").ToString

            Anejos = dR("anejos").ToString

            'InfoCDD
            NameFileCDD = dR("namefilecdd").ToString
            FechaFileCDD = dR("fechafilecdd").ToString

            'Carga de la información sobre las provisionalidades
            DivZona = dR("zona_num").ToString
            SubDivType = dR("subdivision_tipo").ToString
            SubDivNum = dR("subdivision_num").ToString
            ItinType = dR("itin_tipo").ToString
            ItinNum = dR("itin_num").ToString
            Ambito = dR("ambito").ToString

            Dim idTerris() As String = dR("idTerris").ToString.Split("|")
            Dim nombreTerris() As String = dR("nombreTerris").ToString.Split("|")
            Dim tipoTerris() As String = dR("tipoTerris").ToString.Split("|")
            Dim muniTerris() As String = dR("muniTerris").ToString.Split("|")
            Dim poligosCarto() As String = dR("poligonocarto").ToString.Split("|")


            For Each elem As String In dR("idTerris").ToString.Split("|")
                listaTerritorios.Add(New TerritorioBSID(CType(elem, Integer)))
            Next



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
        rcdDoc.Dispose()
        rcdDoc = Nothing



    End Sub

End Class
