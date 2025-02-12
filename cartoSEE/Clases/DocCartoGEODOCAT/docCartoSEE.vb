Public Class docCartoSEE
    Property docIndex As Integer
    Property Sellado As String                              'Sellado
    Property Tipo As String
    Property tipoDocumento As docCartoSEETipoDocu
    Property CodTipo As Integer
    Property CodEstado As String
    Property Estado As String
    Property Escala As String
    Property Tomo As String
    Property proceHoja As String
    Property proceCarpeta As String
    Property NumDisco As String
    Property FechaPrincipal As String
    Property TipoFechaPrincipal As String
    Property fechasModificaciones As String
    Property Anejo As String
    Property Signatura As String
    Property Proyecto As String
    Property Coleccion As String
    Property Subdivision As String
    Property Vertical As String
    Property Horizontal As String
    Property Observaciones As String
    Property Comentarios As String
    Property EdificiosCitados As String
    Property ObservacionesStandard As String
    Property extraProps As New FlagsProperties
    Property listaMuniHistorico As New ArrayList
    Property listaMuniActual As New ArrayList
    Property listaCodMuniHistorico As New ArrayList
    Property listaCodMuniActual As New ArrayList
    Property listaTerritorios As New ArrayList
    Property listaFicherosGeo As New ArrayList
    Property listaFicherosGeo23030 As New ArrayList
    Property listaFicherosGeo25830 As New ArrayList

    Property rcdgeoFiles As New DataTable
    '    Property MunicipiosINE As String
    '    Property MunicipiosLiteral As String
    Property Provincias As String
    Property JuntaEstadistica As String
    Property FicheroJPG As String
    Property Alto As String
    Property Ancho As String
    Property ProvinciaRepo As Integer
    Property subTipoDoc As String
    Property cddProducto As String
    Property cddURL As String
    Property cddNombreFichero As String
    Property cddFechaSubida As String
    Property cddGeometria As String

    'Namespace ABSYS
    Property autorEntidad As String
    Property autorPersona As String
    Property cargaABSYS As Boolean
    Property titnABSYSdoc As Integer
    Property encabezadoABSYSdoc As String
    Property urlABSYSdoc As String

    ' Historial de cambios
    Property createAt As String
    Property updateAt As String
    Property historialCambios As New ArrayList
    Dim historialConsultado As Boolean = False
    Dim getGeoFilesFromDatabaseConsultado As Boolean = False
    Dim getGeoFilesConsultado As Boolean = False

    Property BBOX4OL3 As String = "POLYGON((-3 42,3 42,0 39,-3 42))"
    Property BBOXCenter4OL3_ByExtent As String = "[-3, 39, 3, 42]"
    Property BBOX_Xmin As String = "-10"
    Property BBOX_Xmax As String = "-5"
    Property BBOX_Ymin As String = "39"
    Property BBOX_Ymax As String = "43"


    ReadOnly Property GetURIMetadatoMARC21 As String
        Get
            If titnABSYSdoc > 0 Then Return $"{canonicalURLCatalogoNew}/catalogoweb/xml/{String.Format("{0:0000000}", titnABSYSdoc)}-marc21.xml"
            Return ""
        End Get
    End Property

    ReadOnly Property GetURIFichaCatalogo As String
        Get
            If titnABSYSdoc > 0 Then Return $"{canonicalURLCatalogoNew}/catalogoweb/html/{IIf(titnABSYSdoc > 100000, String.Format("{0:0000000}", titnABSYSdoc), String.Format("{0:000000}", titnABSYSdoc))}.html"
            Return ""
        End Get
    End Property


    ReadOnly Property yearFechaPrincipal As String
        Get
            Dim cadOut As String = ""
            Try
                If Not FechaPrincipal Is Nothing Then
                    If FechaPrincipal.Length >= 3 And (FechaPrincipal.Substring(4, 1) = "-" Or FechaPrincipal.Substring(4, 1) = "/") Then
                        cadOut = FechaPrincipal.Substring(0, 4)
                    Else
                        cadOut = "Sin fecha"
                    End If
                Else
                    cadOut = "Sin fecha"
                End If

                Return cadOut
            Catch ex As Exception
                GenerarLOG("e2m: Error en procGenerateHTMLReport -> yearFechaPrincipal -> " & ex.Message)
            End Try
            Return ""
        End Get
    End Property


    ReadOnly Property getInfoWMS As String
        Get
            Dim filas() As DataRow = rcdgeoFiles.Select
            Dim cadOUT As String = ""
            For Each fila As DataRow In filas
                If cadOUT = "" Then cadOUT = fila.Item("tipowms").ToString & " (" & IIf(fila.Item("mostrarwms") = 1, "Sí", "No") & ")" : Continue For
                cadOUT = cadOUT & ", " & fila.Item("tipowms").ToString & " (" & IIf(fila.Item("mostrarwms") = 1, "Sí", "No") & ")"
            Next
            If filas.Length = 0 And listaFicherosGeo23030.Count = 0 Then
                cadOUT = "Georreferenciación no disponible"
            ElseIf filas.Length = 0 And listaFicherosGeo23030.Count > 0 Then
                cadOUT = "Existe fichero ECW sin contorno"
            ElseIf filas.Length > 0 And listaFicherosGeo23030.Count = 0 Then
                Application.DoEvents()
            ElseIf filas.Length > 0 And listaFicherosGeo23030.Count > 0 Then
                Application.DoEvents()
            Else
                Application.DoEvents()
            End If

            Return cadOUT
        End Get
    End Property

    ReadOnly Property getZIndex As String
        Get
            Dim filas() As DataRow = rcdgeoFiles.Select
            Dim cadOUT As String = ""
            For Each fila As DataRow In filas
                If cadOUT = "" Then cadOUT = fila.Item("zindex").ToString : Continue For
                cadOUT = cadOUT & "#" & fila.Item("zindex").ToString
            Next
            If filas.Length = 0 And listaFicherosGeo23030.Count = 0 Then
                cadOUT = "Georreferenciación no disponible"
            ElseIf filas.Length = 0 And listaFicherosGeo23030.Count > 0 Then
                cadOUT = "Existe fichero ECW sin contorno"
            ElseIf filas.Length > 0 And listaFicherosGeo23030.Count = 0 Then
                Application.DoEvents()
            ElseIf filas.Length > 0 And listaFicherosGeo23030.Count > 0 Then
                Application.DoEvents()
            Else
                Application.DoEvents()
            End If

            Return cadOUT
        End Get
    End Property

    ReadOnly Property municipiosHistoLiteral() As String
        Get
            Dim cadOut As String = ""
            For Each elem As String In listaMuniHistorico
                If cadOut = "" Then cadOut = elem : Continue For
                cadOut = cadOut & ", " & elem
            Next
            Return cadOut
        End Get
    End Property

    ReadOnly Property muniHistoLiteralConINEHistorico() As String
        Get
            Dim cadOut As String = ""
            For iterador As Integer = 0 To listaMuniHistorico.Count - 1
                If cadOut = "" Then cadOut = listaMuniHistorico.Item(iterador).ToString & " (" & listaCodMuniHistorico.Item(iterador).ToString & ")" : Continue For
                cadOut = cadOut & ", " & listaMuniHistorico.Item(iterador).ToString & " (" & listaCodMuniHistorico.Item(iterador).ToString & ")"
            Next
            Return cadOut
        End Get
    End Property

    ReadOnly Property muniHistoLiteralConMuniActual(Optional soloMuniActualSiIguales As Boolean = False) As String
        Get
            Dim cadOut As String = ""
            Dim elem As String
            For iterador As Integer = 0 To listaMuniHistorico.Count - 1
                elem = listaMuniHistorico.Item(iterador).ToString & " (" & listaMuniActual.Item(iterador).ToString & ")"
                If soloMuniActualSiIguales Then
                    If listaMuniHistorico.Item(iterador).ToString = listaMuniActual.Item(iterador).ToString Then
                        elem = listaMuniActual.Item(iterador).ToString
                    End If
                End If
                If cadOut = "" Then cadOut = elem : Continue For
                cadOut = cadOut & ", " & elem
            Next
            Return cadOut
        End Get
    End Property

    ReadOnly Property municipiosHistoLiteralHTML() As String
        Get
            Dim cadOut As String = ""
            Dim idMuni As Integer = -1
            Dim procTilde As New Destildator
            For Each elem As String In listaMuniHistorico
                idMuni = idMuni + 1
                If cadOut = "" Then
                    cadOut = "<a href=""" & listaCodMuniHistorico.Item(idMuni) & "_Resumen_documentos_Archivo-" & procTilde.destildar(elem).Replace(" ", "_") & ".html" & """>" & elem & "</a>"
                    Continue For
                End If
                cadOut = cadOut & ", " & "<a href=""" & listaCodMuniHistorico.Item(idMuni) & "_Resumen_documentos_Archivo-" & procTilde.destildar(elem).Replace(" ", "_") & ".html" & """>" & elem & "</a>"
            Next
            procTilde = Nothing
            Return cadOut
        End Get
    End Property

    ReadOnly Property getListaProvincias() As String
        Get
            Dim cadProv As String = ""
            For Each provElem In Provincias.Split("#")
                If cadProv.IndexOf(provElem) = -1 Then cadProv &= $"{IIf(cadProv = "", "", ", ")}{provElem}"
            Next
            Return cadProv
        End Get
    End Property


    ReadOnly Property municipiosActualLiteral() As String
        Get
            Dim cadOut As String = ""
            For Each elem As String In listaMuniActual
                If cadOut = "" Then cadOut = elem : Continue For
                cadOut = cadOut & ", " & elem
            Next
            Return cadOut
        End Get
    End Property


    ReadOnly Property rutaFicheroAltaRes() As String
        Get
            Return rutaRepo & "\_Scan400\" & _FicheroJPG
        End Get
    End Property

    ReadOnly Property rutaFicheroBajaRes() As String
        Get
            Return rutaRepo & "\_Scan250\" & _FicheroJPG.Replace("\", "250\")
        End Get
    End Property

    ReadOnly Property rutaFicheroThumb() As String

        Get
            Return rutaRepo & "\_Miniaturas\" & _FicheroJPG
        End Get
    End Property

    ReadOnly Property rutaFicheroPDF() As String
        Get 'Opcional. No todos los documentos tienen recurso en PDF
            Dim selladoFormat As String = String.Format("{0:00000000}", CType(Sellado, Integer))
            Return $"{rutaRepo}\_pdf\{String.Format("{0:00}", ProvinciaRepo)}\{tipoDocumento.prefijoNombreCDD}{selladoFormat}.pdf"
        End Get
    End Property

    ReadOnly Property nameFilePropuestoParaCDD(Optional extension As String = "") As String

        Get
            Dim procTilde As New Destildator
            Return _tipoDocumento.prefijoNombreCDD &
                String.Format("{0:000000}", Sellado) & "_" &
                _FechaPrincipal.Substring(0, 4) & "_" &
                procTilde.destildar(municipiosHistoLiteral()).Replace(",", "-").ToUpper.Replace(" ", "_") &
                IIf(extension <> "", "." & extension.ToLower, "")
        End Get

    End Property

    ReadOnly Property nameFile4CDD() As String

        Get
            Return _tipoDocumento.prefijoNombreCDD & "_" & String.Format("{0:000000}", Sellado) & ".jpg"
        End Get
    End Property

    'Nambre de la carpeta para guardar los documentos y después zippearlos para el centro de descargas
    ReadOnly Property nameFolder4CDD() As String
        Get
            Return _tipoDocumento.NombreTipo.Substring(0, 5).Replace("ñ", "n").ToUpper & "_" & String.Format("{0:000000}", Sellado) & "_" & _FechaPrincipal.Substring(0, 4)
        End Get
    End Property



    ReadOnly Property nameFileNEMXML() As String
        Get
            For Each tipo As docCartoSEETipoDocu In tiposDocSIDCARTO
                If tipoDocumento.idTipodoc = tipo.idTipodoc Then
                    Return tipo.prefijoMetadatos & "_" & String.Format("{0:000000}", Sellado) & "_" & _FechaPrincipal.Substring(0, 4) & ".xml"
                End If
            Next
            Return ""
        End Get
    End Property

    ReadOnly Property rutaPlantillaMetadato As String
        Get
            For Each tipo As TiposDocumento In RutasPlantillasMetadatos
                If tipoDocumento.idTipodoc = tipo.idTipodoc Then
                    If tipoDocumento.idTipodoc = 7 Then
                        'Chapuza para las plantillas de Junta estadística
                        Return tipo.RutaMetadatosTipo.Replace("Plantilla_PlanoPoblacion.xml", "Plantilla_PlanoPoblacionJuntaSI.xml")
                    Else
                        Return tipo.RutaMetadatosTipo
                    End If
                End If
            Next
            Return ""
        End Get
    End Property

    ReadOnly Property nameFileECWJPG() As String

        Get
            Return _tipoDocumento.NombreTipo.Substring(0, 5).Replace("ñ", "n").ToUpper & "_" & String.Format("{0:000000}", Sellado) & "_" & _FechaPrincipal.Substring(0, 4) & ".zip"
        End Get
    End Property
    ReadOnly Property nameFileHTML() As String

        Get
            Return _tipoDocumento.NombreTipo.Substring(0, 5).Replace("ñ", "n").ToUpper & "_" & String.Format("{0:000000}", Sellado) & "_" & _FechaPrincipal.Substring(0, 4) & "-info.html"
        End Get
    End Property

    ReadOnly Property SEOTitle() As String
        Get
            Return tipoDocumento.NombreTipo & " nº " & Sellado & " de " & municipiosActualLiteral
        End Get
    End Property

    ReadOnly Property SEOKeywords() As String
        Get
            Return tipoDocumento.NombreTipo & " , Archivo Topográfico, Instituto Geográfico Nacional," & municipiosActualLiteral
        End Get
    End Property

    ReadOnly Property getCdDAlias() As String
        Get
            If tipoDocumento.NombreTipo = "Directorio" Then
                Return Trim("nº " & Sellado & ". " &
                        muniHistoLiteralConMuniActual(True) &
                        IIf(subTipoDoc <> "", ". " & subTipoDoc, "") &
                        IIf(Coleccion <> "", ". Colección " & Coleccion, ""))
            ElseIf tipoDocumento.NombreTipo = "Hoja kilométrica" Then
                Return Trim("nº " & Sellado & ". " &
                        muniHistoLiteralConMuniActual(True) &
                        IIf(subTipoDoc <> "", ". " & subTipoDoc, "") &
                        IIf(Coleccion <> "", ". Colección " & Coleccion, "") &
                        IIf(Subdivision <> "", ". " & Subdivision, ""))
            ElseIf tipoDocumento.NombreTipo = "Parcelario urbano JE" Then
                Return Trim("nº " & Sellado & ". " &
                        muniHistoLiteralConMuniActual(True) &
                        IIf(subTipoDoc <> "", ". " & subTipoDoc, "") &
                        IIf(Coleccion <> "", ". Colección " & Coleccion, "") &
                        IIf(Subdivision <> "", ". " & Subdivision, ""))
            ElseIf tipoDocumento.NombreTipo = "Planimetría" Then
                Return nameFilePropuestoParaCDD
            ElseIf tipoDocumento.NombreTipo = "Altimetría" Then
                Return nameFilePropuestoParaCDD
            ElseIf tipoDocumento.NombreTipo = "Conjunta" Then
                Return nameFilePropuestoParaCDD
            Else
                Return "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " &
                       "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
            End If

        End Get
    End Property

    ''' <summary>
    ''' Formatea el número de sellado con seis caracteres
    ''' </summary>
    ''' <returns></returns>
    ReadOnly Property getIdProductor4CdD() As String

        Get
            Return String.Format("{0:000000}", Sellado)
        End Get

    End Property



    ReadOnly Property SEODescription() As String
        Get
            If tipoDocumento.NombreTipo = "Planimetría" Then
                Return "Mapa manuscrito a escala 1:25000 que recoge la información planimétrica necesaria para la realización del M.T.N. 1/50.000 " &
                        "(vías de comunicación, núcleos de población, hidrografía, mojones de delimitación municipal...). " &
                        "Se crearon para la formación del Mapa Topográfico Nacional a escala 1/50.000 y fueron base fundamental para el conocimiento de la riqueza catastral de España."
            ElseIf tipoDocumento.NombreTipo = "Altimetría" Then
                Return "Mapa manuscrito que recoge la información altimétrica, especialmente las curvas de nivel a una equidistancia normalmente de 10 metros, necesaria para la realización " &
                        "del M.T.N. 1/50.000. Se crearon para la formación del Mapa Topográfico Nacional a escala 1/50.000 y fueron base fundamental para el conocimiento de la riqueza catastral de España."
            Else
                Return "Mapas manuscritos que recoge la información altimétrica y planimétrica, necesaria para la realización del M.T.N. 1/50.000. " &
                    "Se crearon para la formación del Mapa Topográfico Nacional a escala 1/50.000 y fueron base fundamental para el conocimiento de la riqueza catastral de España."
            End If

        End Get
    End Property

    Sub New()

    End Sub

    Sub New(idArchivo As Integer)

        Dim consultaSQL As String
        If idArchivo > 0 Then
            'Si recibimos un iddocsiddae>0, cargamos datos de la database

            consultaSQL = $"SELECT archivo.idarchivo,archivo.numdoc,archivo.escala,archivo.tomo,archivo.coleccion,archivo.subdivision,archivo.fechaprincipal,archivo.tipo_fechaprincipal,
		                        archivo.fechasmodificaciones,archivo.anejo,archivo.vertical,archivo.horizontal,archivo.tipodoc_id,archivo.estadodoc_id,archivo.procecarpeta,archivo.procehoja,
		                        archivo.subtipo,archivo.juntaestadistica,archivo.signatura,archivo.observestandar_id,archivo.extraprops,archivo.observaciones,archivo.observ,archivo.proyecto,
		                        archivo.cdd_nomfich,archivo.cdd_url,archivo.cdd_producto,archivo.cdd_geometria,archivo.cdd_fecha,archivo.titn,archivo.autor,archivo.autor_persona,archivo.encabezado,archivo.nombreedificio,
		                        tbtipodocumento.tipodoc as Tipo,tbestadodocumento.estadodoc as Estado, tbobservaciones.observestandar,
                                archivo.provincia_id as repoprov,archivo.fechacreacion,archivo.fechamodificacion,
                                string_agg(territorios.idterritorio::character varying,'#') as listaIdTerris,
		                        string_agg(territorios.nombre,'#') as listaMuniHisto,string_agg(to_char(territorios.munihisto, 'FM0000009'::text),'#') as listaCodMuniHisto,
		                        string_agg(listamunicipios.nombre,'#') as listaMuniActual, string_agg(listamunicipios.inecorto,'#') as listaCodMuniActual, 
		                        string_agg(provincias.nombreprovincia,'#') as nombreprovincia 
                        FROM bdsidschema.archivo 
	                        LEFT JOIN bdsidschema.tbtipodocumento ON tbtipodocumento.idtipodoc=archivo.tipodoc_id 
	                        LEFT JOIN bdsidschema.tbestadodocumento ON tbestadodocumento.idestadodoc=archivo.estadodoc_id 
	                        LEFT JOIN bdsidschema.archivo2territorios  ON archivo2territorios.archivo_id=archivo.idarchivo 
	                        LEFT JOIN bdsidschema.tbobservaciones  ON tbobservaciones.idobservestandar=archivo.observestandar_id 
	                        LEFT JOIN bdsidschema.territorios on territorios.idterritorio= archivo2territorios.territorio_id 
	                        LEFT JOIN ngmepschema.listamunicipios on territorios.nomen_id= listamunicipios.identidad 
	                        LEFT JOIN bdsidschema.provincias on territorios.provincia= provincias.idprovincia 
                        WHERE archivo.idarchivo={idArchivo}
                          group by archivo.idarchivo,archivo.numdoc,archivo.escala,archivo.tomo,archivo.coleccion,archivo.subdivision,archivo.fechaprincipal,archivo.tipo_fechaprincipal,
  	                        archivo.fechasmodificaciones,archivo.anejo,archivo.vertical, archivo.horizontal, archivo.tipodoc_id, archivo.estadodoc_id, archivo.procecarpeta, 
  	                        archivo.procehoja, archivo.subtipo,archivo.juntaestadistica, archivo.signatura, archivo.observestandar_id,archivo.extraprops, archivo.observaciones,archivo.observ,
                            archivo.proyecto,tbtipodocumento.tipodoc,archivo.cdd_nomfich,archivo.cdd_url,archivo.cdd_producto,archivo.titn,archivo.autor,archivo.autor_persona,archivo.encabezado,archivo.nombreedificio,
                            tbestadodocumento.estadodoc,tbobservaciones.observestandar "
            loadDocAttributesFromDatabase(consultaSQL)
        End If

    End Sub

    Private Sub loadDocAttributesFromDatabase(consultaSQL)

        Dim rcdDoc As DataTable
        Dim filas() As DataRow
        Dim contador As Long
        Dim Anterior As Integer
        Dim item As docCartoSEE

        rcdDoc = New DataTable
        If CargarRecordset(consultaSQL, rcdDoc) = False Then
            rcdDoc = Nothing
            Exit Sub
        End If
        filas = rcdDoc.Select
        contador = -1
        Anterior = 0
        For Each dR As DataRow In filas
            item = New docCartoSEE
            docIndex = dR("idarchivo").ToString
            Sellado = dR("numdoc").ToString
            'If flag_ActualizarInfoGeom = True Then item.getFootprint()
            'Maquillamos el tomo
            If dR("tomo").ToString = "" Then
                Tomo = dR("tomo").ToString
            Else
                If IsNumeric(dR("tomo")) Then
                    Tomo = String.Format("{0:000}", CType(dR("tomo").ToString, Integer))
                Else
                    If IsNumeric(dR("tomo").ToString.Replace("BIS", "")) Then
                        Tomo = String.Format("{0:000}", CType(dR("tomo").ToString.Replace("BIS", ""), Integer)) & "BIS"
                    Else
                        Tomo = dR("tomo").ToString
                    End If
                End If
            End If
            If dR("fechaprincipal").ToString.Length >= 10 Then
                'ListaDoc(contador).fechaPrincipal = dR("fechaprincipal").ToString.Substring(0, 10)
                FechaPrincipal = FormatearFecha(dR("fechaprincipal"), "GERMAN")
            End If
            TipoFechaPrincipal = dR("tipo_fechaprincipal").ToString
            fechasModificaciones = dR("fechasmodificaciones").ToString
            For Each subItem As docCartoSEETipoDocu In tiposDocSIDCARTO
                If subItem.idTipodoc = dR("tipodoc_id").ToString Then
                    tipoDocumento = subItem
                    Exit For
                End If
            Next
            Tipo = dR("Tipo").ToString
            CodTipo = dR("tipodoc_id").ToString
            For Each subItem As docCartoSEETipoDocu In tiposDocSIDCARTO
                If subItem.idTipodoc = dR("tipodoc_id").ToString Then
                    tipoDocumento = subItem
                    Exit For
                End If
            Next
            CodEstado = dR("estadodoc_id").ToString
            Estado = dR("Estado").ToString
            Escala = dR("Escala").ToString
            Vertical = dR("Vertical").ToString
            Horizontal = dR("Horizontal").ToString
            proceHoja = dR("Procehoja").ToString
            proceCarpeta = dR("Procecarpeta").ToString
            subTipoDoc = dR("subtipo").ToString
            NumDisco = "" 'dR("NumDisco").ToString
            Anejo = dR("Anejo").ToString
            Signatura = dR("Signatura").ToString
            Proyecto = dR("proyecto").ToString
            Coleccion = dR("Coleccion").ToString
            Subdivision = dR("Subdivision").ToString

            cddProducto = dR("cdd_producto").ToString
            cddNombreFichero = dR("cdd_nomfich").ToString
            cddURL = dR("cdd_url").ToString
            cddFechaSubida = dR("cdd_fecha").ToString
            cddGeometria = IIf(dR("cdd_geometria") = 1, "Sí", "No")
            titnABSYSdoc = dR("titn")
            If titnABSYSdoc > 0 Then
                cargaABSYS = True
                urlABSYSdoc = $"{urlAbsysLink}{titnABSYSdoc}"
            Else
                cargaABSYS = False
                urlABSYSdoc = ""
            End If
            autorEntidad = dR("autor").ToString
            autorPersona= dR("autor_persona").ToString
            encabezadoABSYSdoc = dR("encabezado").ToString

            For Each elem As String In dR("listaIdTerris").ToString.Split("#")
                listaTerritorios.Add(New TerritorioBSID(CType(elem, Integer)))
            Next

            For Each elem As String In dR("listaMuniHisto").ToString.Split("#")
                If elem = "" Then Continue For
                listaMuniHistorico.Add(elem)
            Next
            For Each elem As String In dR("listaCodMuniHisto").ToString.Split("#")
                If elem = "" Then Continue For
                listaCodMuniHistorico.Add(elem)
            Next
            For Each elem As String In dR("listaMuniActual").ToString.Split("#")
                If elem = "" Then Continue For
                listaMuniActual.Add(elem)
            Next
            For Each elem As String In dR("listaCodMuniActual").ToString.Split("#")
                If elem = "" Then Continue For
                listaCodMuniActual.Add(elem)
            Next

            Provincias = dR("nombreprovincia").ToString
            ProvinciaRepo = dR("repoprov")
            FicheroJPG = DirRepoProvinciaByINE(ProvinciaRepo) & "\" & dR("numdoc").ToString & ".jpg"
            JuntaEstadistica = IIf(dR("JuntaEstadistica").ToString = "1", "Sí", "No")
            ObservacionesStandard = dR("observestandar").ToString
            Observaciones = dR("observaciones").ToString
            Comentarios = dR("observ").ToString
            EdificiosCitados = dR("nombreedificio").ToString
            extraProps.propertyCode = dR("extraprops")

            createAt = IIf(dR("fechacreacion").ToString = "", "No registrada", dR("fechacreacion").ToString)
            updateAt = IIf(dR("fechamodificacion").ToString = "", "No hay ediciones", dR("fechamodificacion").ToString)


            'If flag_CargarFicherosGEO Then
            '    getGeoFiles()
            '    getGeoFilesFromDatabase()
            'End If


        Next
        rcdDoc.Dispose()
        rcdDoc = Nothing



    End Sub





    Sub getGeoFilesFromDatabase()

        If getGeoFilesFromDatabaseConsultado = True Then Exit Sub

        rcdgeoFiles.Clear()
        rcdgeoFiles.Dispose()
        CargarDatatable($"SELECT * FROM bdsidschema.contornos WHERE archivo_id={docIndex}", rcdgeoFiles)

        getGeoFilesFromDatabaseConsultado = True

    End Sub


    'Busca los ficheros georreferenciados escaneando el directorio
    Sub getGeoFiles()

        Dim RutaDoc As String
        Dim contador As Integer
        If getGeoFilesConsultado = True Then Exit Sub
        listaFicherosGeo.Clear()

        'Si no hay acceso al repositorio Base, directamente salgo
        If Not IO.Directory.Exists(rutaRepoGeorrefBase) Then Exit Sub

        'Vamos a analizar los siguientes EPSGs
        Dim lstEPSG As New ArrayList
        lstEPSG.Add("epsg23030")
        lstEPSG.Add("epsg25830")

        For Each Muni As String In listaCodMuniHistorico
            For Each epsgCode As String In lstEPSG
                RutaDoc = $"{rutaRepoGeorrefBase}\{epsgCode}\{DirRepoProvinciaByTipodoc(tipoDocumento.idTipodoc)}\{Muni.Substring(0, 2)}\{Muni}\{Sellado}.ecw"
                If IO.File.Exists(RutaDoc) Then listaFicherosGeo.Add(New FileGeorref With {
                    .NameFile = $"{Sellado}.ecw",
                    .PathFile = RutaDoc,
                    .EPSCode = epsgCode
                })

            Next
            If listaFicherosGeo.Count > 0 Then Exit Sub
        Next

        For Each epsgCode As String In lstEPSG
            For Each Muni As String In listaCodMuniHistorico
                RutaDoc = $"{rutaRepoGeorrefBase}\{epsgCode}\{DirRepoProvinciaByTipodoc(tipoDocumento.idTipodoc)}\{Muni.Substring(0, 2)}\{Muni}\{Sellado}_01.ecw"
                If IO.File.Exists(RutaDoc) Then
                    listaFicherosGeo.Add(New FileGeorref With {
                        .NameFile = $"{Sellado}_01.ecw",
                        .PathFile = RutaDoc,
                        .EPSCode = epsgCode
                    })
                    contador = 1
                    Do Until contador = 20
                        contador += 1
                        RutaDoc = $"{rutaRepoGeorrefBase}\{epsgCode}\{DirRepoProvinciaByTipodoc(tipoDocumento.idTipodoc)}\{Muni.Substring(0, 2)}\{Muni}\{Sellado}_{String.Format("{0:00}", contador)}.ecw"
                        If IO.File.Exists(RutaDoc) Then
                            listaFicherosGeo.Add(New FileGeorref With {
                                .NameFile = $"{Sellado}_{String.Format("{0:00}", contador)}.ecw",
                                .PathFile = RutaDoc,
                                .EPSCode = epsgCode
                            })
                        Else
                            Exit Do
                        End If
                    Loop
                    Exit For
                End If
            Next
        Next

        getGeoFilesConsultado = True

    End Sub


    Sub getFootprint()

        Dim cadSQL As String
        Dim rcdTMP As DataTable
        Dim filas() As DataRow
        Dim x1_tmp As Double
        Dim y1_tmp As Double
        Dim x2_tmp As Double
        Dim y2_tmp As Double
        Dim contador As Integer = -1

        cadSQL = "SELECT st_xmin(Box2D(ST_Transform(geom,4230))) as xmin," &
                        "st_xmax(Box2D(ST_Transform(geom,4230))) as xmax," &
                        "st_ymin(Box2D(ST_Transform(geom,4230))) as ymin," &
                        "st_ymax(Box2D(ST_Transform(geom,4230))) as ymax " &
                        "FROM bdsidschema.contornos WHERE not geom is null and archivo_id=" & docIndex
        rcdTMP = New DataTable
        If CargarRecordset(cadSQL, rcdTMP) = True Then
            filas = rcdTMP.Select
            If filas.Length > 0 Then
                For Each fila As DataRow In filas
                    x1_tmp = CType(fila("xmin"), Double)
                    y1_tmp = CType(fila("ymin"), Double)
                    x2_tmp = CType(fila("xmax"), Double)
                    y2_tmp = CType(fila("ymax"), Double)
                Next
            End If
            filas = Nothing
        End If
        rcdTMP.Dispose()
        rcdTMP = Nothing
        BBOX_Xmin = CType(x1_tmp, String).Replace(",", ".")
        BBOX_Xmax = CType(x2_tmp, String).Replace(",", ".")
        BBOX_Ymin = CType(y1_tmp, String).Replace(",", ".")
        BBOX_Ymax = CType(y2_tmp, String).Replace(",", ".")
        BBOX4OL3 = "POLYGON((" & BBOX_Xmin & " " & BBOX_Ymin & "," &
                           "" & BBOX_Xmin & " " & BBOX_Ymax & "," &
                           "" & BBOX_Xmax & " " & BBOX_Ymax & "," &
                           "" & BBOX_Xmax & " " & BBOX_Ymin & "," &
                           "" & BBOX_Xmin & " " & BBOX_Ymin & "))"
        BBOXCenter4OL3_ByExtent = "[" & BBOX_Xmin & "," & BBOX_Ymin & "," & BBOX_Xmax & "," & BBOX_Ymax & "]"

    End Sub

    Sub cargarHistorial()

        If historialConsultado = True Then Exit Sub

        Dim rcdHistorial As New DataTable
        Dim filas As DataRow()
        Dim edicion As docCartoSEEVariacion
        Try
            If CargarRecordset("SELECT * from bdsidschema.archivolog WHERE archivo_id=" & docIndex, rcdHistorial) = False Then
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
