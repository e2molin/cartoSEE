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
    Property fechaPrincipal As String
    Property fechasModificaciones As String
    Property Anejo As String
    Property Signatura As String
    Property Coleccion As String
    Property Subdivision As String
    Property Vertical As String
    Property Horizontal As String
    Property Observaciones As String
    Property ObservacionesStandard As String
    Property listaMuniHistorico As New ArrayList
    Property listaMuniActual As New ArrayList
    Property listaCodMuniHistorico As New ArrayList
    Property listaCodMuniActual As New ArrayList
    Property listaFicherosGeo As New ArrayList
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

    'Namespace ABSYS
    Property autorDocumento As String
    Property titnDocumento As Integer

    Property BBOX4OL3 As String = "POLYGON((-3 42,3 42,0 39,-3 42))"
    Property BBOXCenter4OL3_ByExtent As String = "[-3, 39, 3, 42]"
    Property BBOX_Xmin As String = "-10"
    Property BBOX_Xmax As String = "-5"
    Property BBOX_Ymin As String = "39"
    Property BBOX_Ymax As String = "43"

    ReadOnly Property yearFechaPrincipal As String
        Get
            Dim cadOut As String = ""
            Try
                If Not fechaPrincipal Is Nothing Then
                    If fechaPrincipal.Length >= 3 And (fechaPrincipal.Substring(4, 1) = "-" Or fechaPrincipal.Substring(4, 1) = "/") Then
                        cadOut = fechaPrincipal.Substring(0, 4)
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
            If filas.Length = 0 And listaFicherosGeo.Count = 0 Then
                cadOUT = "Georreferenciación no disponible"
            ElseIf filas.Length = 0 And listaFicherosGeo.Count > 0 Then
                cadOUT = "Existe fichero ECW sin contorno"
            ElseIf filas.Length > 0 And listaFicherosGeo.Count = 0 Then
                Application.DoEvents()
            ElseIf filas.Length > 0 And listaFicherosGeo.Count > 0 Then
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
            If filas.Length = 0 And listaFicherosGeo.Count = 0 Then
                cadOUT = "Georreferenciación no disponible"
            ElseIf filas.Length = 0 And listaFicherosGeo.Count > 0 Then
                cadOUT = "Existe fichero ECW sin contorno"
            ElseIf filas.Length > 0 And listaFicherosGeo.Count = 0 Then
                Application.DoEvents()
            ElseIf filas.Length > 0 And listaFicherosGeo.Count > 0 Then
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


    ReadOnly Property nameFile4CDD() As String

        Get
            Return _tipoDocumento.prefijoNombreCDD & "_" & String.Format("{0:000000}", Sellado) & ".jpg"
        End Get
    End Property

    'Nambre de la carpeta para guardar los documentos y después zippearlos para el centro de descargas
    ReadOnly Property nameFolder4CDD() As String
        Get
            Return _tipoDocumento.NombreTipo.Substring(0, 5).Replace("ñ", "n").ToUpper & "_" & String.Format("{0:000000}", Sellado) & "_" & _fechaPrincipal.Substring(0, 4)
        End Get
    End Property



    ReadOnly Property nameFileNEMXML() As String
        Get
            For Each tipo As docCartoSEETipoDocu In tiposDocSIDCARTO
                If tipoDocumento.idTipodoc = tipo.idTipodoc Then
                    Return tipo.prefijoMetadatos & "_" & String.Format("{0:000000}", Sellado) & "_" & _fechaPrincipal.Substring(0, 4) & ".xml"
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
            Return _tipoDocumento.NombreTipo.Substring(0, 5).Replace("ñ", "n").ToUpper & "_" & String.Format("{0:000000}", Sellado) & "_" & _fechaPrincipal.Substring(0, 4) & ".zip"
        End Get
    End Property
    ReadOnly Property nameFileHTML() As String

        Get
            Return _tipoDocumento.NombreTipo.Substring(0, 5).Replace("ñ", "n").ToUpper & "_" & String.Format("{0:000000}", Sellado) & "_" & _fechaPrincipal.Substring(0, 4) & "-info.html"
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

    Sub getGeoFilesFromDatabase()

        rcdgeoFiles.Clear()
        rcdgeoFiles.Dispose()
        CargarDatatable("Select * from bdsidschema.contornos where archivo_id=" & docIndex, rcdgeoFiles)


    End Sub
    Sub getGeoFiles()

        Dim RutaDOC As String
        Dim contador As Integer

        listaFicherosGeo.Clear()
        'Si no hay acceso al repositorio Base, directamente salgo
        If System.IO.Directory.Exists(rutaRepoGeorref) = False Then Exit Sub

        'Comprobamos el más simple de todos

        For Each Muni As String In listaCodMuniHistorico
            RutaDOC = rutaRepoGeorref &
                             "\" & DirRepoProvinciaByTipodoc(tipoDocumento.idTipodoc) &
                             "\" & Muni.Substring(0, 2) &
                             "\" & Muni &
                            "\" & Sellado & ".ecw"
            If System.IO.File.Exists(RutaDOC) = True Then
                listaFicherosGeo.Add(RutaDOC)
                Exit Sub
            End If
        Next
        'Si llegamos aquí, es porque el nombre del documento es compuesto

        For Each Muni As String In listaCodMuniHistorico
            RutaDOC = rutaRepoGeorref &
                             "\" & DirRepoProvinciaByTipodoc(tipoDocumento.idTipodoc) &
                             "\" & Muni.Substring(0, 2) &
                             "\" & Muni &
                            "\" & Sellado & "_01.ecw"
            If System.IO.File.Exists(RutaDOC) = True Then
                listaFicherosGeo.Add(RutaDOC)
                contador = 0
                Do Until contador = 20
                    contador = contador + 1
                    RutaDOC = rutaRepoGeorref & _
                                     "\" & DirRepoProvinciaByTipodoc(tipoDocumento.idTipodoc) & _
                                     "\" & Muni.Substring(0, 2) & _
                                     "\" & Muni & _
                                    "\" & Sellado & "_" & String.Format("{0:00}", contador + 1) & ".ecw"

                    If System.IO.File.Exists(RutaDOC) = True Then
                        listaFicherosGeo.Add(RutaDOC)
                    Else
                        Exit Do
                    End If
                Loop
                Exit For
            End If
        Next

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

        cadSQL = "SELECT st_xmin(ST_box2d(ST_Transform(geom,4230))) as xmin," &
                        "st_xmax(ST_box2d(ST_Transform(geom,4230))) as xmax," &
                        "st_ymin(ST_box2d(ST_Transform(geom,4230))) as ymin," &
                        "st_ymax(ST_box2d(ST_Transform(geom,4230))) as ymax " &
                        "FROM contornos WHERE not geom is null and archivo_id=" & docIndex
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
        BBOX4OL3 = "POLYGON((" & BBOX_Xmin & " " & BBOX_Ymin & "," & _
                           "" & BBOX_Xmin & " " & BBOX_Ymax & "," & _
                           "" & BBOX_Xmax & " " & BBOX_Ymax & "," & _
                           "" & BBOX_Xmax & " " & BBOX_Ymin & "," & _
                           "" & BBOX_Xmin & " " & BBOX_Ymin & "))"
        BBOXCenter4OL3_ByExtent = "[" & BBOX_Xmin & "," & BBOX_Ymin & "," & BBOX_Xmax & "," & BBOX_Ymax & "]"

    End Sub


End Class
