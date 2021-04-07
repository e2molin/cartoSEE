Public Class MetadataISO19115
    Private _contenidoXML As String
    Private _sr As System.IO.StreamReader
    Private _sw As System.IO.StreamWriter

#Region "Class Properties"

    Property templateXML As String
    Property docuAT As docCartoSEE
    Property folderXML As String
    Property imgThumb As String

#End Region

    Public Sub New()

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Function prepareTemplateMetadato() As Boolean

        If templateXML = "" Then Return False
        _sr = New System.IO.StreamReader(templateXML)
        _contenidoXML = _sr.ReadToEnd
        _sr.Close()
        _sr.Dispose()
        _sr = Nothing
        Return True

    End Function

    Function fillMetadataTemplate() As Boolean

        Dim Contenido As String
        Dim ComboObservaciones As String
        Dim cont As Integer
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


        'Dim sr As New System.IO.StreamReader(templateXML)
        'Contenido = sr.ReadToEnd
        'sr.Close()
        'sr.Dispose()
        'sr = Nothing

        '-----------------------------------------------------------------------------------------------
        'Modificación de la información variable para cada documento
        '-----------------------------------------------------------------------------------------------
        Contenido = _contenidoXML

        FechaCreacionMetadato = Now.Year.ToString & "-" & _
                                                   String.Format("{0:00}", Now.Month) & "-" & _
                                                   String.Format("{0:00}", Now.Day)
        Contenido = Contenido.Replace("_FechaCreacionMetadato_", Now.Year.ToString & "-" & _
                                                   String.Format("{0:00}", Now.Month) & "-" & _
                                                   String.Format("{0:00}", Now.Day))

        If docuAT.Anejo.Trim <> "" Then
            Contenido = Contenido.Replace("_Anejo_", docuAT.Anejo)
            Contenido = Contenido.Replace("_Anejos1_", "Anejos:" & docuAT.Anejo & ".")
        Else
            Contenido = Contenido.Replace("_Anejo_", "")
            Contenido = Contenido.Replace("_Anejos1_", "")
        End If
        Contenido = Contenido.Replace("_NDocumento_", docuAT.Sellado)
        Contenido = Contenido.Replace("_Subdivision_", docuAT.Subdivision)
        If docuAT.Coleccion.Trim <> "" Then
            Contenido = Contenido.Replace("_Coleccion_", "Colección: " & docuAT.Coleccion & ".")
        Else
            Contenido = Contenido.Replace("_Coleccion_", "")
        End If

        If docuAT.Escala.Trim <> "" Then
            Contenido = Contenido.Replace("_Escala_", "1:" & docuAT.Escala)
            Contenido = Contenido.Replace("_EscalaV_", docuAT.Escala)
        Else
            Contenido = Contenido.Replace("_Escala_", "desconocida")
            Contenido = Contenido.Replace("_EscalaV_", "0")
        End If

        Contenido = Contenido.Replace("_Provincia1_", DameProvinciaByINE(docuAT.ProvinciaRepo))
        If docuAT.ObservacionesStandard.Trim = "" And docuAT.Observaciones.Trim = "" Then
            Contenido = Contenido.Replace("_Observaciones_", "")
        Else
            ComboObservaciones = ""
            If docuAT.ObservacionesStandard.Trim <> "" Then
                ComboObservaciones = docuAT.ObservacionesStandard.Trim & ". "
            End If
            If docuAT.Observaciones.Trim <> "" Then
                ComboObservaciones = ComboObservaciones & docuAT.Observaciones.Trim & "."
            End If
            ComboObservaciones = ComboObservaciones.Replace("..", ".")
            Contenido = Contenido.Replace("_Observaciones_", ComboObservaciones.Trim)
        End If
        If Not docuAT.fechaPrincipal Is Nothing Then
            If docuAT.fechaPrincipal.Length >= 10 Then
                cadFechaPrincipal = docuAT.fechaPrincipal.ToString
                Contenido = Contenido.Replace("_FechaPrincipal_", cadFechaPrincipal)
            End If
        End If

        'Procesamos los municipios a los que pertenece el documento
        '-------------------------------------------------------------------------------------------------------------------
        'Dim Munis() As String = docuAT.Municipios.Split("#")
        'Dim MunisINE() As String = docuAT.MunicipiosINE.Split("#")
        'Dim MunisHisto As String = ""
        'Dim MunisActual As String = ""
        'Dim cadenaMunis As String = ""
        'Application.DoEvents()
        'cont = -1
        'For Each Muni As String In docuAT.listaCodMuniActual
        '    cont = cont + 1
        '    If MunisINE(cont).EndsWith("00") = False Then
        '        MunisHisto = MunisHisto & Muni & ","
        '        cadenaMunis = cadenaMunis & DameMunicipioByINE(MunisINE(cont).Substring(0, 5) & "00") & _
        '                        " antes " & Muni & " (" & DameProvinciaByINE(MunisINE(cont).Substring(0, 2)) & _
        '                        ":" & MunisINE(cont).Substring(0, 5) & "), "
        '    Else
        '        MunisActual = MunisActual & Muni & ","
        '        cadenaMunis = cadenaMunis & Muni & _
        '                        " (" & DameProvinciaByINE(MunisINE(cont).Substring(0, 2)) & _
        '                        ":" & MunisINE(cont).Substring(0, 5) & "), "
        '    End If
        'Next
        'MunisActual = MunisActual.Trim
        'If MunisActual <> "" Then MunisActual = MunisActual.Substring(0, MunisActual.Length - 1)
        'MunisHisto = MunisHisto.Trim
        'If MunisHisto <> "" Then MunisHisto = MunisHisto.Substring(0, MunisHisto.Length - 1)
        'cadenaMunis = cadenaMunis.Trim
        'If cadenaMunis <> "" Then cadenaMunis = cadenaMunis.Substring(0, cadenaMunis.Length - 1)
        Contenido = Contenido.Replace("_ActualHProv_", docuAT.municipiosActualLiteral)
        Contenido = Contenido.Replace("_MunicipioHistóricos_", docuAT.municipiosHistoLiteral)
        Contenido = Contenido.Replace("_MunicipioActuales_", docuAT.municipiosActualLiteral)


        If rutaRepoThumbs = "" Then
            Contenido = Contenido.Replace("_Miniatura_", rutaRepo & "\_Miniaturas\" & docuAT.FicheroJPG)
        Else
            Contenido = Contenido.Replace("_Miniatura_", rutaRepoThumbs & "/" & String.Format("{0:00}", docuAT.ProvinciaRepo) & "/" & docuAT.Sellado & ".jpg")
        End If

        If rutaCentroDescargas = "" Then
            Contenido = Contenido.Replace("_RutaJPG_", rutaRepo & "\_Scan250\" & docuAT.FicheroJPG)

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
        If docuAT.fechasModificaciones.Trim <> "" Then
            Dim ListaModificaciones() As String = docuAT.fechasModificaciones.Split("/")
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
            'e2m DameRutasFicherosECW(docu, Rutas)
            If IsNothing(Rutas) = False Then
                If Rutas.Length > 0 And IsNothing(Rutas(0)) = False Then
                    Dim ListaModificaciones() As String 'e2m = docu.fechasModificaciones.Split("/")
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
        DameMBR_Documento(docuAT.Sellado, Xmin, Ymin, Xmax, Ymax)

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
        Contenido = Contenido.Replace("_IdentificadorFichero_", docuAT.nameFileNEMXML)
        _contenidoXML = Contenido
        Return True

    End Function


    Function saveMetadataFile() As Boolean
        Try
            _sw = New System.IO.StreamWriter(folderXML & docuAT.nameFileNEMXML)
            _sw.Write(_contenidoXML)
            _sw.Close()
            _sw.Dispose()
            _sw = Nothing
            Return True
        Catch ex As Exception
            MessageBox.Show("Error:" & ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        End Try
    End Function
End Class
