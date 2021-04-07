Module GestionDocumentacion

    Function GenerarMetadatosNEM(ByVal docu As docSIDCARTO, ByVal xmlPathFile As String) As Boolean

        Dim Contenido As String
        Dim ComboObservaciones As String
        Dim cont As Integer
        Dim RutaPlantilla As String
        Dim PrefijoNom As String = ""
        Dim NomXMLFecha As String = ""
        Dim ComentarioMETA As String = ""
        Dim FechaCreacionMetadato As String
        Dim cadFechaPrincipal As String = ""
        Dim Rutas() As String
        Dim procTilde As New Destildator

        Dim Xmin As Double
        Dim Ymin As Double
        Dim Xmax As Double
        Dim Ymax As Double

        RutaPlantilla = ""
        If docu.CodTipo > 0 Then
            For Each tipo As TiposDocumento In RutasPlantillasMetadatos
                If docu.CodTipo = tipo.idTipodoc Then
                    RutaPlantilla = tipo.RutaMetadatosTipo
                    PrefijoNom = tipo.PrefijoNom
                End If
            Next
        End If

        'Chapuza para las plantillas de Junta estadística
        If docu.CodTipo = 7 And docu.JuntaEstadistica = "Sí" Then
            RutaPlantilla = RutaPlantilla.Replace("Plantilla_PlanoPoblacion.xml", "Plantilla_PlanoPoblacionJuntaSI.xml")
            PrefijoNom = "ESIGNPARCELARIOURBANO_"
        End If

        If RutaPlantilla = "" Then
            MessageBox.Show("No se ha definido una plantilla de metadatos para " & docu.Tipo & ".", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Function
        End If
        If System.IO.File.Exists(RutaPlantilla) = False Then
            MessageBox.Show("Plantilla de metadatos no localizada.", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Function
        End If

        Dim sr As New System.IO.StreamReader(RutaPlantilla)
        Contenido = sr.ReadToEnd
        sr.Close()
        sr.Dispose()
        sr = Nothing

        '-----------------------------------------------------------------------------------------------
        'Modificación de la información variable para cada documento
        '-----------------------------------------------------------------------------------------------
        FechaCreacionMetadato = Now.Year.ToString & "-" & _
                                                   String.Format("{0:00}", Now.Month) & "-" & _
                                                   String.Format("{0:00}", Now.Day)
        Contenido = Contenido.Replace("_FechaCreacionMetadato_", Now.Year.ToString & "-" & _
                                                   String.Format("{0:00}", Now.Month) & "-" & _
                                                   String.Format("{0:00}", Now.Day))

        If docu.Anejo.Trim <> "" Then
            Contenido = Contenido.Replace("_Anejo_", docu.Anejo)
            Contenido = Contenido.Replace("_Anejos1_", "Anejos:" & docu.Anejo & ".")
        Else
            Contenido = Contenido.Replace("_Anejo_", "")
            Contenido = Contenido.Replace("_Anejos1_", "")
        End If
        Contenido = Contenido.Replace("_NDocumento_", docu.Sellado)
        Contenido = Contenido.Replace("_Subdivision_", docu.Subdivision)
        If docu.Coleccion.Trim <> "" Then
            Contenido = Contenido.Replace("_Coleccion_", "Colección: " & docu.Coleccion & ".")
        Else
            Contenido = Contenido.Replace("_Coleccion_", "")
        End If

        If docu.Escala.Trim <> "" Then
            Contenido = Contenido.Replace("_Escala_", "1:" & docu.Escala)
            Contenido = Contenido.Replace("_EscalaV_", docu.Escala)
        Else
            Contenido = Contenido.Replace("_Escala_", "desconocida")
            Contenido = Contenido.Replace("_EscalaV_", "0")
        End If

        Contenido = Contenido.Replace("_Provincia1_", DameProvinciaByINE(docu.ProvinciaRepo))
        If docu.ObservacionesStandard.Trim = "" And docu.Observaciones.Trim = "" Then
            Contenido = Contenido.Replace("_Observaciones_", "")
        Else
            ComboObservaciones = ""
            If docu.ObservacionesStandard.Trim <> "" Then
                ComboObservaciones = docu.ObservacionesStandard.Trim & ". "
            End If
            If docu.Observaciones.Trim <> "" Then
                ComboObservaciones = ComboObservaciones & docu.Observaciones.Trim & "."
            End If
            ComboObservaciones = ComboObservaciones.Replace("..", ".")
            Contenido = Contenido.Replace("_Observaciones_", ComboObservaciones.Trim)
        End If
        If Not docu.fechaPrincipal Is Nothing Then
            If docu.fechaPrincipal.Length >= 10 Then
                cadFechaPrincipal = docu.fechaPrincipal.ToString
                Contenido = Contenido.Replace("_FechaPrincipal_", cadFechaPrincipal)
            End If
        End If

        'Procesamos los municipios a los que pertenece el documento
        '-------------------------------------------------------------------------------------------------------------------
        Dim Munis() As String = docu.Municipios.Split("#")
        Dim MunisINE() As String = docu.MunicipiosINE.Split("#")
        Dim MunisHisto As String = ""
        Dim MunisActual As String = ""
        Dim cadenaMunis As String = ""
        Application.DoEvents()
        If Munis.Length = 1 Then
            Contenido = Contenido.Replace("_MunicipioH_", docu.Municipios)
        Else
            Contenido = Contenido.Replace("_MunicipioH_", Munis(0))
        End If
        cont = -1
        For Each Muni As String In Munis
            cont = cont + 1
            If MunisINE(cont).EndsWith("00") = False Then
                MunisHisto = MunisHisto & Muni & ","
                cadenaMunis = cadenaMunis & DameMunicipioByINE(MunisINE(cont).Substring(0, 5) & "00") & _
                                " antes " & Muni & " (" & DameProvinciaByINE(MunisINE(cont).Substring(0, 2)) & _
                                ":" & MunisINE(cont).Substring(0, 5) & "), "
            Else
                MunisActual = MunisActual & Muni & ","
                cadenaMunis = cadenaMunis & Muni & _
                                " (" & DameProvinciaByINE(MunisINE(cont).Substring(0, 2)) & _
                                ":" & MunisINE(cont).Substring(0, 5) & "), "
            End If
        Next
        MunisActual = MunisActual.Trim
        If MunisActual <> "" Then MunisActual = MunisActual.Substring(0, MunisActual.Length - 1)
        MunisHisto = MunisHisto.Trim
        If MunisHisto <> "" Then MunisHisto = MunisHisto.Substring(0, MunisHisto.Length - 1)
        cadenaMunis = cadenaMunis.Trim
        If cadenaMunis <> "" Then cadenaMunis = cadenaMunis.Substring(0, cadenaMunis.Length - 1)
        Contenido = Contenido.Replace("_ActualHProv_", cadenaMunis)
        Contenido = Contenido.Replace("_MunicipioHistóricos_", MunisHisto)
        Contenido = Contenido.Replace("_MunicipioActuales_", MunisActual)


        If rutaRepoThumbs = "" Then
            Contenido = Contenido.Replace("_Miniatura_", rutaRepo & "\_Miniaturas\" & docu.FicheroJPG)
        Else
            Contenido = Contenido.Replace("_Miniatura_", rutaRepoThumbs & "/" & _
                                          String.Format("{0:00}", docu.ProvinciaRepo) & _
                                           "/" & docu.Sellado & ".jpg")
        End If

        If rutaCentroDescargas = "" Then
            Contenido = Contenido.Replace("_RutaJPG_", rutaRepo & "\_Scan250\" & docu.FicheroJPG)

        Else
            Contenido = Contenido.Replace("_RutaJPG_", rutaCentroDescargas)
            'Contenido = Contenido.Replace("_RutaJPG_", rutaCentroDescargas & _
            '                                "/GEODOCAT/_Scan250/" & String.Format("{0:00}", docu.ProvinciaRepo) & _
            '                                "/" & docu.Sellado & ".jpg")
        End If


        Application.DoEvents()

        'Procesamos las fechas de modificación,que pueden ser más de una.
        'Además de valor, insertamos estructura.#FECHA_MODIFICACION#
        '-------------------------------------------------------------------------------------------------------------------
        '                    <gmd:date>
        '                        <gmd:CI_Date>
        '                            <gmd:date>
        '                                <gco:Date>1002-01-01</gco:Date>
        '                            </gmd:date>
        '                            <gmd:dateType>
        '                                <gmd:CI_DateTypeCode
        '                                    codeList="./resources/codeList.xml#CI_DateTypeCode" codeListValue="revision">revision</gmd:CI_DateTypeCode>
        '                            </gmd:dateType>
        '                        </gmd:CI_Date>
        '                    </gmd:date>
        '                    <gmd:date>
        '                        <gmd:CI_Date>
        '                            <gmd:date>
        '                                <gco:Date>_FechaModificacion_</gco:Date>
        '                            </gmd:date>
        '                            <gmd:dateType>
        '                                <gmd:CI_DateTypeCode
        '                                    codeList="./resources/codeList.xml#CI_DateTypeCode" codeListValue="revision">revision</gmd:CI_DateTypeCode>
        '                            </gmd:dateType>
        '                        </gmd:CI_Date>
        '                   </gmd:date>
        '-------------------------------------------------------------------------------------------------------------------
        Dim cadModificaciones As String = ""
        If docu.fechasModificaciones.Trim <> "" Then
            Dim ListaModificaciones() As String = docu.fechasModificaciones.Split("/")
            Dim cadModificacionesRoot As String
            cont = 0
            cadModificaciones = ""
            cadModificacionesRoot = _
                    Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & "<gmd:date>" & System.Environment.NewLine & _
                    Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & "<gmd:CI_Date>" & System.Environment.NewLine & _
                    Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & "<gmd:date>" & System.Environment.NewLine & _
                    Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & "<gco:Date>_FechaModificacion_</gco:Date>" & System.Environment.NewLine & _
                    Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & "</gmd:date>" & System.Environment.NewLine & _
                    Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & "<gmd:dateType>" & System.Environment.NewLine & _
                    Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & "<gmd:CI_DateTypeCode" & System.Environment.NewLine & _
                    Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & "codeList=""./resources/codeList.xml#CI_DateTypeCode"" codeListValue=""revision"">revision</gmd:CI_DateTypeCode>" & System.Environment.NewLine & _
                    Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & "</gmd:dateType>" & System.Environment.NewLine & _
                    Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & "</gmd:CI_Date>" & System.Environment.NewLine & _
                    Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & "</gmd:date>" & System.Environment.NewLine
            For Each Modificacion As String In ListaModificaciones
                cont = cont + 1
                cadModificaciones = cadModificaciones & cadModificacionesRoot.Replace("_FechaModificacion_", Modificacion & "-01-01")
            Next
            Contenido = Contenido.Replace("#FECHA_MODIFICACION#" & System.Environment.NewLine, cadModificaciones)
        Else
            Contenido = Contenido.Replace("#FECHA_MODIFICACION#" & System.Environment.NewLine, cadModificaciones)
        End If
        Application.DoEvents()


        'Procesamos los ficheros ECW asociados al documento.
        'Además de valor, insertamos estructura.#FICHEROS_ECW#
        '-------------------------------------------------------------------------------------------------------------------
        '            <gmd:onLine>
        '                <gmd:CI_OnlineResource>
        '                    <gmd:linkage>
        '                        <gmd:URL>http://www.cnig.es/.../_Ruta_</gmd:URL>
        '                    </gmd:linkage>
        '                </gmd:CI_OnlineResource>
        '            </gmd:onLine>
        '-------------------------------------------------------------------------------------------------------------------
        If rutaCentroDescargas = "" Then
            cadModificaciones = ""
            DameRutasFicherosECW(docu, Rutas)
            If IsNothing(Rutas) = False Then
                If Rutas.Length > 0 And IsNothing(Rutas(0)) = False Then
                    Dim ListaModificaciones() As String = docu.fechasModificaciones.Split("/")
                    Dim cadModificacionesRoot As String
                    cont = 0
                    cadModificaciones = ""
                    cadModificacionesRoot = _
                            Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & "<gmd:onLine>" & System.Environment.NewLine & _
                            Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & "<gmd:CI_OnlineResource>" & System.Environment.NewLine & _
                            Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & "<gmd:linkage>" & System.Environment.NewLine & _
                            Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & "<gmd:URL>_Ruta_</gmd:URL>" & System.Environment.NewLine & _
                            Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & "</gmd:linkage>" & System.Environment.NewLine & _
                            Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & "</gmd:CI_OnlineResource>" & System.Environment.NewLine & _
                            Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & "</gmd:onLine>" & System.Environment.NewLine
                    For Each Ruta As String In Rutas
                        cont = cont + 1
                        If rutaCentroDescargas <> "" Then
                            Ruta = Ruta.Replace(rutaRepoGeorref, rutaCentroDescargas & "/GEODOCAT_II")
                            cadModificaciones = cadModificaciones & _
                                                cadModificacionesRoot.Replace("_Ruta_", Ruta.Replace("\", "/"))
                        Else
                            cadModificaciones = cadModificaciones & _
                                                cadModificacionesRoot.Replace("_Ruta_", Ruta)
                        End If
                    Next
                    Contenido = Contenido.Replace("#RUTAS_ECW#" & System.Environment.NewLine, cadModificaciones)
                Else
                    Contenido = Contenido.Replace("#RUTAS_ECW#" & System.Environment.NewLine, cadModificaciones)
                End If
            Else
                Contenido = Contenido.Replace("#RUTAS_ECW#" & System.Environment.NewLine, cadModificaciones)
            End If
        Else
            'Meter ruta centro de descargas
            Dim cadModificacionesRoot As String
            cadModificacionesRoot = _
                    Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & "<gmd:onLine>" & System.Environment.NewLine & _
                    Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & "<gmd:CI_OnlineResource>" & System.Environment.NewLine & _
                    Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & "<gmd:linkage>" & System.Environment.NewLine & _
                    Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & "<gmd:URL>" & rutaCentroDescargas & "</gmd:URL>" & System.Environment.NewLine & _
                    Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & "</gmd:linkage>" & System.Environment.NewLine & _
                    Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & "</gmd:CI_OnlineResource>" & System.Environment.NewLine & _
                    Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & Convert.ToChar(Keys.Tab) & "</gmd:onLine>" & System.Environment.NewLine
            Contenido = Contenido.Replace("#RUTAS_ECW#" & System.Environment.NewLine, cadModificacionesRoot)
        End If
        Application.DoEvents()

        'Procesamos el ámbito geográfico
        '-------------------------------------------------------------------------------------------------------------------
        DameMBR_Documento(docu.Sellado, Xmin, Ymin, Xmax, Ymax)

        If Ymin = 0 Or Ymax = 0 Then
            'Si no obtenemos coordenadas, ponemos el BB de España
            Contenido = Contenido.Replace("_LongOeste_", "-9.29777")
            Contenido = Contenido.Replace("_LongEste_", "4.32704")
            Contenido = Contenido.Replace("_LatNorte_", "43.79295")
            Contenido = Contenido.Replace("_LatSur_", "35.99957")
        Else
            'REVISAR   ConversionUTMED50_to_LLWGS84_GSB(Xmin, Ymin, 30)
            'REVISAR   ConversionUTMED50_to_LLWGS84_GSB(Xmax, Ymax, 30)
            Contenido = Contenido.Replace("_LongOeste_", Math.Round(Xmin, 6).ToString.Replace(",", "."))
            Contenido = Contenido.Replace("_LongEste_", Math.Round(Xmax, 6).ToString.Replace(",", "."))
            Contenido = Contenido.Replace("_LatNorte_", Math.Round(Ymax, 6).ToString.Replace(",", "."))
            Contenido = Contenido.Replace("_LatSur_", Math.Round(Ymin, 6).ToString.Replace(",", "."))
        End If


        'Ponemos nombre al fichero
        '-------------------------------------------------------------------------------------------------------------------
        Application.DoEvents()
        Dim nombreFicheroNEM As String
        If Munis.Length > 0 Then
            nombreFicheroNEM = PrefijoNom & cadFechaPrincipal & "_" & _
                            Munis(0) & "_" & _
                            FechaCreacionMetadato & "_" & docu.Sellado
        Else
            nombreFicheroNEM = PrefijoNom & cadFechaPrincipal & "_" & _
                docu.Municipios & "_" & _
                FechaCreacionMetadato & "_" & docu.Sellado
        End If
        nombreFicheroNEM = procTilde.destildar(nombreFicheroNEM)
        procTilde = Nothing
        nombreFicheroNEM = nombreFicheroNEM.Replace(" ", "_")
        Contenido = Contenido.Replace("_IdentificadorFichero_", nombreFicheroNEM)

        Dim sw As New System.IO.StreamWriter(xmlPathFile & "\" & nombreFicheroNEM & ".xml", False, System.Text.Encoding.UTF8)
        sw.WriteLine(Contenido)
        sw.Close()
        sw.Dispose()
        sw = Nothing
        GenerarMetadatosNEM = True

    End Function


    Function DameRutasFicherosECW(ByVal documento As docSIDCARTO, ByRef Rutas() As String) As Boolean


        Dim RutaDOC As String
        Dim contador As Integer

        ReDim Rutas(0)

        'Si no hay acceso al repositorio Base, directamente salgo
        If System.IO.Directory.Exists(rutaRepoGeorref) = False Then Exit Function

        'Comprobamos el más simple de todos
        Dim Munis() As String = documento.MunicipiosINE.Split("#")
        For Each Muni As String In Munis
            RutaDOC = rutaRepoGeorref & _
                             "\" & DirRepoProvinciaByTipodoc(documento.CodTipo) & _
                             "\" & Muni.Substring(0, 2) & _
                             "\" & Muni & _
                            "\" & documento.Sellado & ".ecw"
            If System.IO.File.Exists(RutaDOC) = True Then
                Rutas(0) = RutaDOC
                Return True
            End If
        Next
        'El nombre del documento es compuesto
        ReDim Rutas(20)
        For Each Muni As String In Munis
            RutaDOC = rutaRepoGeorref & _
                             "\" & DirRepoProvinciaByTipodoc(documento.CodTipo) & _
                             "\" & Muni.Substring(0, 2) & _
                             "\" & Muni & _
                            "\" & documento.Sellado & "_01.ecw"
            If System.IO.File.Exists(RutaDOC) = True Then
                Rutas(0) = RutaDOC
                contador = 0
                Do Until contador = 20
                    contador = contador + 1
                    RutaDOC = rutaRepoGeorref & _
                                     "\" & DirRepoProvinciaByTipodoc(documento.CodTipo) & _
                                     "\" & Muni.Substring(0, 2) & _
                                     "\" & Muni & _
                                    "\" & documento.Sellado & "_" & String.Format("{0:00}", contador + 1) & ".ecw"

                    If System.IO.File.Exists(RutaDOC) = True Then
                        Rutas(contador) = RutaDOC
                    Else
                        Exit Do
                    End If
                Loop
                ReDim Preserve Rutas(contador - 1)
                Exit For
            End If
        Next

        Application.DoEvents()
        If Rutas(0) Is Nothing Then
            Return False
        Else
            Return True
        End If


    End Function

    Function GenerarScriptRenombreFicheros(ByVal documento As docSIDCARTO, _
                                       ByVal RutaEntrada As String, _
                                       ByVal NuevoTipo As Integer, _
                                        ByVal NuevoMuni As String, _
                                        ByVal NuevoNumSellado As String) As String

        Dim RutaDoc As String
        Dim NombreFich As String
        If NuevoMuni = "" Then
            Dim Munis() As String = documento.MunicipiosINE.Split("#")
            NuevoMuni = Munis(0)
        Else
            NuevoMuni = String.Format("{0:0000000}", CType(NuevoMuni, Integer))
        End If
        If NuevoTipo = -1 Then
            NuevoTipo = documento.CodTipo
        End If
        NombreFich = SacarFileDeRuta(RutaEntrada)
        If NuevoNumSellado <> "" Then
            NombreFich = SacarFileDeRuta(RutaEntrada)
            NombreFich = NuevoNumSellado & NombreFich.Substring(6, NombreFich.Length - 6)
        End If
        If RutaEntrada.Contains("_Scan250") Then
            RutaDoc = rutaRepo & "\_Scan250\" & _
                    DirRepoProvinciaByINE(NuevoMuni.Substring(0, 2)) & "250\" & NombreFich
        ElseIf RutaEntrada.Contains("_Scan400") Then
            RutaDoc = rutaRepo & "\_Scan400\" & _
                    DirRepoProvinciaByINE(NuevoMuni.Substring(0, 2)) & "\" & NombreFich
        Else
            RutaDoc = rutaRepoGeorref & _
                             "\" & DirRepoProvinciaByTipodoc(NuevoTipo) & _
                             "\" & NuevoMuni.Substring(0, 2) & _
                             "\" & NuevoMuni & "\" & NombreFich
        End If

        Return RutaDoc
        'If RutaEntrada <> RutaDoc Then
        '    GenerarScriptRenombreFicheros = RutaDoc
        'Else

        'End If

    End Function


    Function GenerarProyectoGM(ByVal ListaRutas As ArrayList, ByVal RecortarContornos As Boolean) As Boolean

        Dim RutaDoc As String
        Dim sw As System.IO.StreamWriter

        If PlantillaGIS <> "" Then
            Try
                System.IO.File.Copy(PlantillaGIS, My.Application.Info.DirectoryPath & "\LaunchGM.gmw", True)
            Catch ex As Exception
                MessageBox.Show("Problema: " & ex.Message, AplicacionTitulo, _
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Function
            End Try
        End If

        If PlantillaGIS <> "" Then
            sw = New System.IO.StreamWriter(My.Application.Info.DirectoryPath & "\LaunchGM.gmw", _
                                    True, System.Text.Encoding.Default)
        Else
            sw = New System.IO.StreamWriter(My.Application.Info.DirectoryPath & "\LaunchGM.gmw", _
                                    False, System.Text.Encoding.Default)
            sw.WriteLine("GLOBAL_MAPPER_SCRIPT VERSION=""1.00"" FILENAME=""" & My.Application.Info.DirectoryPath & "\LaunchGM.gmw" & """")
            sw.WriteLine("UNLOAD_ALL")
        End If

        Dim ListaPuntos As New ArrayList
        Dim Sello As String
        For Each Ruta As String In ListaRutas
            RutaDoc = Ruta
            If RecortarContornos = True Then
                Sello = SacarFileDeRuta(Ruta).ToLower.Replace(".ecw", "")
                DamePuntosContorno(Sello, ListaPuntos)
                sw.WriteLine("DEFINE_SHAPE SHAPE_NAME=""" & Sello & """")
                For Each vert As Point In ListaPuntos
                    sw.WriteLine(vert.X & "," & vert.Y)
                Next
                ListaPuntos.Clear()
                sw.WriteLine("END_DEFINE_SHAPE")
            End If
            sw.WriteLine("IMPORT FILENAME=""" & RutaDoc & """ TYPE=""ECW"" \")
            sw.WriteLine("LABEL_FIELD="""" ANTI_ALIAS=""NO"" AUTO_CONTRAST=""NO"" CONTRAST_SHARED=""YES"" CONTRAST_MODE=""NONE"" \")
            If RecortarContornos = True Then
                sw.WriteLine("CLIP_COLLAR=""POLY"" CLIP_COLLAR_POLY=""" & Sello & """ TEXTURE_MAP=""NO""")
            Else
                sw.WriteLine("CLIP_COLLAR=""NONE"" TEXTURE_MAP=""NO""")
            End If

        Next
        ListaPuntos = Nothing
        sw.Close()
        sw.Dispose()
        sw = Nothing
        GenerarProyectoGM = True

    End Function

    Function GenerarProyectoGM(ByVal ListaRutas As ArrayList, ByVal RecortarContornos As Boolean, rutaScript As String) As Boolean

        Dim RutaDoc As String
        Dim sw As System.IO.StreamWriter

        'If PlantillaGIS <> "" Then
        '    Try
        '        System.IO.File.Copy(PlantillaGIS, My.Application.Info.DirectoryPath & "\LaunchGM.gmw", True)
        '    Catch ex As Exception
        '        MessageBox.Show("Problema: " & ex.Message, AplicacionTitulo, _
        '                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '        Exit Function
        '    End Try
        'End If

        'If PlantillaGIS <> "" Then
        '    sw = New System.IO.StreamWriter(My.Application.Info.DirectoryPath & "\LaunchGM.gmw", _
        '                            True, System.Text.Encoding.Default)
        'Else
        '    sw = New System.IO.StreamWriter(My.Application.Info.DirectoryPath & "\LaunchGM.gmw", _
        '                            False, System.Text.Encoding.Default)
        '    sw.WriteLine("GLOBAL_MAPPER_SCRIPT VERSION=""1.00"" FILENAME=""" & My.Application.Info.DirectoryPath & "\LaunchGM.gmw" & """")
        '    sw.WriteLine("UNLOAD_ALL")
        'End If

        sw = New System.IO.StreamWriter(rutaScript, False, System.Text.Encoding.Default)
        sw.WriteLine("GLOBAL_MAPPER_SCRIPT VERSION=""1.00"" FILENAME=""" & rutaScript & """")
        sw.WriteLine("UNLOAD_ALL")


        Dim ListaPuntos As New ArrayList
        Dim Sello As String
        For Each Ruta As String In ListaRutas
            RutaDoc = Ruta
            If RecortarContornos = True Then
                Sello = SacarFileDeRuta(Ruta).ToLower.Replace(".ecw", "")
                DamePuntosContorno(Sello, ListaPuntos)
                sw.WriteLine("DEFINE_SHAPE SHAPE_NAME=""" & Sello & """")
                For Each vert As Point In ListaPuntos
                    sw.WriteLine(vert.X & "," & vert.Y)
                Next
                ListaPuntos.Clear()
                sw.WriteLine("END_DEFINE_SHAPE")
            End If
            sw.WriteLine("IMPORT FILENAME=""" & RutaDoc & """ TYPE=""ECW"" \")
            sw.WriteLine("LABEL_FIELD="""" ANTI_ALIAS=""NO"" AUTO_CONTRAST=""NO"" CONTRAST_SHARED=""YES"" CONTRAST_MODE=""NONE"" \")
            If RecortarContornos = True Then
                sw.WriteLine("CLIP_COLLAR=""POLY"" CLIP_COLLAR_POLY=""" & Sello & """ TEXTURE_MAP=""NO""")
            Else
                sw.WriteLine("CLIP_COLLAR=""NONE"" TEXTURE_MAP=""NO""")
            End If

        Next
        ListaPuntos = Nothing
        sw.Close()
        sw.Dispose()
        sw = Nothing
        GenerarProyectoGM = True

    End Function

End Module
