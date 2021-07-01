Module ConsultasGEO

    Public Structure Territorios
        Dim Nombre As String
        Dim Municipio As String
        Dim Provincia As String
        Dim CodigoINE As Long
        Dim CodigoINEProvincia As Long
        Dim Tipo As String
        Dim NPoligono As Integer
    End Structure



    Function DameMBR_Documento(ByVal Sellado As String, ByRef X1 As Double, ByRef Y1 As Double, ByRef X2 As Double, ByRef Y2 As Double) As Boolean

        Dim cadSQL As String
        Dim rcdTMP As DataTable
        Dim filas() As DataRow
        Dim x1_tmp As Double
        Dim y1_tmp As Double
        Dim x2_tmp As Double
        Dim y2_tmp As Double
        Dim contador As Integer = -1

        cadSQL = "SELECT " & _
                    "st_xmin(ST_box2d(ST_Transform(geom,4230))) as xmin," & _
                    "st_xmax(ST_box2d(ST_Transform(geom,4230))) as xmax," & _
                    "st_ymin(ST_box2d(ST_Transform(geom,4230))) as ymin," & _
                    "st_ymax(ST_box2d(ST_Transform(geom,4230))) as ymax " & _
                    "FROM contornos WHERE " & _
                    "Sellado='" & Sellado & "'"
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
        X1 = x1_tmp
        X2 = x2_tmp
        Y1 = y1_tmp
        Y2 = y2_tmp
        DameMBR_Documento = True


    End Function



    Function DameColindantes(ByVal CodigoINE As String, ByRef ListaColindantes() As Territorios) As Boolean
        Dim CadenaSQL As String
        Dim rcdColindantes As DataTable
        Dim contador As Long
        Dim filas() As DataRow


        CadenaSQL = "Select distinct " & _
                    "if(r1.municipio<>" & Val(CodigoINE) & ",r1.municipio,r2.municipio) AS INE," & _
                    "if(r1.municipio<>" & Val(CodigoINE) & ",r1.nombre,r2.nombre) AS Nombre," & _
                    "if(r1.municipio<>" & Val(CodigoINE) & ",r1.tipoentidad,r2.tipoentidad) AS TIPO " & _
                    "FROM(limites) " & _
                    "INNER JOIN territorios r1 on r1.poligono = limites.rpoly " & _
                    "INNER JOIN territorios r2 on r2.poligono = limites.lpoly " & _
                    "WHERE r1.municipio=" & Val(CodigoINE) & " or r2.municipio=" & Val(CodigoINE) & " order by r1.municipio"
        rcdColindantes = New DataTable
        If CargarRecordset(CadenaSQL, rcdColindantes) = True Then
            filas = rcdColindantes.Select
            ReDim ListaColindantes(rcdColindantes.Rows.Count - 1)
            contador = -1
            For Each dR As DataRow In filas
                contador = contador + 1
                ListaColindantes(contador).Nombre = dR("Nombre").ToString
                ListaColindantes(contador).Municipio = ""
                ListaColindantes(contador).CodigoINE = dR("INE")
                ListaColindantes(contador).Tipo = dR("Tipo").ToString
                ListaColindantes(contador).CodigoINEProvincia = 0
                'Dim fila() As DataRow = ListaProvincias.Select("INE=" & dR("Provincia2"))
                'If fila.Length > 0 Then
                '    ListaColindantes(contador).Provincia = fila(0).ItemArray(1).ToString
                'End If
                'fila = Nothing
                dR = Nothing
            Next
            DameColindantes = True
        Else
            DameColindantes = False
        End If
        rcdColindantes.Dispose()

    End Function

    Function DameColindantes(ByVal NPoligono As Long, ByRef ListaColindantes() As Territorios) As Boolean
        Dim CadenaSQL As String
        Dim rcdColindantes As DataTable
        Dim contador As Long
        Dim filas() As DataRow

        CadenaSQL = "Select distinct " & _
                    "if(r1.poligono<>" & NPoligono & ",r1.municipio,r2.municipio) AS INE," & _
                    "if(r1.poligono<>" & NPoligono & ",r1.nombre,r2.nombre) AS Nombre," & _
                    "if(r1.poligono<>" & NPoligono & ",r1.tipoentidad,r2.tipoentidad) AS Tipo " & _
                    "FROM limites " & _
                    "INNER JOIN territorios r1 on r1.poligono = limites.rpoly " & _
                    "INNER JOIN territorios r2 on r2.poligono = limites.Lpoly " & _
                    "where rpoly = " & NPoligono & " Or lpoly = " & NPoligono & " " & _
                    "order by r1.municipio"
        rcdColindantes = New DataTable
        If CargarRecordset(CadenaSQL, rcdColindantes) = True Then
            filas = rcdColindantes.Select
            ReDim ListaColindantes(rcdColindantes.Rows.Count - 1)
            contador = -1
            For Each dR As DataRow In filas
                contador = contador + 1
                ListaColindantes(contador).Nombre = dR("Nombre").ToString
                ListaColindantes(contador).Municipio = ""
                ListaColindantes(contador).CodigoINE = dR("INE")
                ListaColindantes(contador).Tipo = dR("Tipo").ToString
                ListaColindantes(contador).CodigoINEProvincia = 0
                'Dim fila() As DataRow = ListaProvincias.Select("INE=" & dR("Provincia2"))
                'If fila.Length > 0 Then
                '    ListaColindantes(contador).Provincia = fila(0).ItemArray(1).ToString
                'End If
                'fila = Nothing
                dR = Nothing
            Next
            DameColindantes = True
        Else
            DameColindantes = False
        End If
        rcdColindantes.Dispose()

    End Function


    Function DameColindantes(ByVal NumeroPoligono As Integer, ByRef ListaColindantes As DataTable) As Boolean
        Dim CadenaSQL As String

        'CadenaSQL = "(Select " & _
        '            "CASE WHEN r1.poligono_carto<>" & NumeroPoligono & " then r1.municipio ELSE r2.municipio END as MUNICIPIO," & _
        '            "CASE WHEN r1.poligono_carto<>" & NumeroPoligono & " then r1.nombre ELSE r2.nombre END as NOMBRE," & _
        '            "CASE WHEN r1.poligono_carto<>" & NumeroPoligono & " then r1.poligono_carto ELSE r2.poligono_carto END as POLIGONO," & _
        '            "CASE WHEN r1.poligono_carto<>" & NumeroPoligono & " then r1.tipo ELSE r2.tipo END as TIPO " & _
        '            "FROM  limites " & _
        '            "INNER JOIN territorios r1 on r1.poligono_carto = limites.rpoly " & _
        '            "INNER JOIN territorios r2 on r2.poligono_carto = limites.Lpoly " & _
        '            "WHERE r1.tipo='Municipio' and r2.tipo='Municipio' and " & _
        '            "(rpoly = " & NumeroPoligono & " Or lpoly = " & NumeroPoligono & ")) UNION (" & _
        '            "SELECT municipio,nombre,poligono_carto,tipo FROM territorios WHERE poligono_carto=" & NumeroPoligono & _
        '            ")"
        CadenaSQL = "(Select " & _
                    "CASE WHEN r1.poligono_carto<>" & NumeroPoligono & " then r1.municipio ELSE r2.municipio END as MUNICIPIO," & _
                    "CASE WHEN r1.poligono_carto<>" & NumeroPoligono & " then r1.nombre ELSE r2.nombre END as NOMBRE," & _
                    "CASE WHEN r1.poligono_carto<>" & NumeroPoligono & " then r1.nombremostrado ELSE r2.nombremostrado END as NOMBREMOSTRADO," & _
                    "CASE WHEN r1.poligono_carto<>" & NumeroPoligono & " then r1.poligono_carto ELSE r2.poligono_carto END as POLIGONO," & _
                    "CASE WHEN r1.poligono_carto<>" & NumeroPoligono & " then r1.tipo ELSE r2.tipo END as TIPO " & _
                    "FROM  limites " & _
                    "INNER JOIN territorios r1 on r1.poligono_carto = limites.rpoly " & _
                    "INNER JOIN territorios r2 on r2.poligono_carto = limites.Lpoly " & _
                    "WHERE r1.tipo='Municipio' and r2.tipo='Municipio' and " & _
                    "(rpoly = " & NumeroPoligono & " Or lpoly = " & NumeroPoligono & ")) UNION (" & _
                    "SELECT municipio,nombre,nombremostrado,poligono_carto,tipo FROM territorios WHERE poligono_carto=" & NumeroPoligono & _
                    ")"


        If CargarDatatable(CadenaSQL, ListaColindantes) = True Then
            DameColindantes = True
        Else
            DameColindantes = False
        End If

    End Function

    Function DameMunicipioByINE(ByVal CodigoINE As Integer) As String

        Dim Muni() As DataRow
        DameMunicipioByINE = "Desconocido"
        Muni = ListaMunicipiosHisto.Select("cod_munihisto=" & CodigoINE)
        For Each dR As DataRow In Muni
            DameMunicipioByINE = dR("nombre").ToString
        Next
        Muni = Nothing
    End Function

    Function DameTipoEntidadByCodigoPoligono(ByVal NPoligono As Long) As String

        Dim Muni() As DataRow
        DameTipoEntidadByCodigoPoligono = "Desconocido"
        Muni = ListaMunicipiosHisto.Select("poligono=" & NPoligono & "")
        For Each dR As DataRow In Muni
            DameTipoEntidadByCodigoPoligono = dR("TipoEntidad").ToString
        Next
        Muni = Nothing
    End Function
    Function DameProvinciaByINE(ByVal CodigoINE As String) As String

        Dim fila() As DataRow = ListaProvincias.Select("INE=" & CodigoINE)
        DameProvinciaByINE = "Desconocida"
        If fila.Length > 0 Then
            DameProvinciaByINE = fila(0).ItemArray(1).ToString
        End If
        fila = Nothing

    End Function

    Function DameAutonomiaByINEProvincia(ByVal CodigoINE As String) As String

        Dim cadOUT As String = ""
        Try
            Dim fila() As DataRow = ListaProvincias.Select("INE=" & CodigoINE)
            If fila.Length > 0 Then
                cadOUT = String.Format("{0:00}", fila(0).ItemArray(2))
            End If
            fila = Nothing
        Catch ex As Exception
            GenerarLOG("e2m: No se pudo obtener el código de autonomia: " & CodigoINE)
        End Try

        Return cadOUT


    End Function

    Function DirRepoProvinciaByINE(ByVal CodigoINE As String) As String

        'Solucionamos el caso del Bug de Madrid
        If CodigoINE = "82" Then CodigoINE = "28"
        If CodigoINE = "88" Then CodigoINE = "28"
        Dim fila() As DataRow = ListaProvincias.Select("INE=" & CodigoINE)
        DirRepoProvinciaByINE = "Desconocida"
        If fila.Length > 0 Then
            DirRepoProvinciaByINE = fila(0).ItemArray(3).ToString
        End If
        fila = Nothing

    End Function

    Function DirRepoProvinciaByTipodoc(ByVal Tipodoc As Integer) As String

        DirRepoProvinciaByTipodoc = ""
        'If Tipodoc = 0 Then Exit Function
        Dim fila() As DataRow = ListaTiposDocumento.Select("idtipodoc=" & Tipodoc)

        If fila.Length > 0 Then
            DirRepoProvinciaByTipodoc = fila(0).ItemArray(2).ToString
        End If
        fila = Nothing

    End Function


    Function InsertarProvinciaMetadato(ByVal cadena As String) As String

        Dim cadwork As String
        cadwork = cadena
        For iBucle As Integer = 1 To 50
            cadwork = cadwork.Replace("(" & String.Format("{0:00}", iBucle), _
                                        "(" & DameProvinciaByINE(iBucle) & ":" & String.Format("{0:00}", iBucle))
        Next
        InsertarProvinciaMetadato = cadwork

    End Function

    Function ExisteLimiteEntreAyB_ByINE(ByVal INEA As Integer, ByVal INEB As Integer) As Boolean

        Dim cadSQL As String
        Dim reportTMP As DataTable
        Dim filas() As DataRow
        Dim cad_INEA As String
        Dim cad_INEB As String

        If INEA = INEB Then
            ExisteLimiteEntreAyB_ByINE = True
            Exit Function
        End If

        cad_INEA = String.Format("{0:00000}", INEA)
        cad_INEB = String.Format("{0:00000}", INEB)

        ExisteLimiteEntreAyB_ByINE = False

        cadSQL = "SELECT limites.id_limite,limites.LPOLY,limites.RPOLY,TL.municipio as INE_L,TR.municipio as INE_R " & _
                "from limites " & _
                "inner join territorios TL on TL.poligono_carto=limites.LPOLY " & _
                "inner join territorios TR on TR.poligono_carto=limites.RPOLY WHERE " & _
                "(TL.municipio=" & cad_INEA & " AND TR.municipio=" & cad_INEB & ") OR " & _
                "(TR.municipio=" & cad_INEA & " AND TL.municipio=" & cad_INEB & ")"
        reportTMP = New DataTable
        If CargarRecordset(cadSQL, reportTMP) = True Then
            filas = reportTMP.Select
            If filas.Length = 0 Then
                Application.DoEvents()
            Else
                ExisteLimiteEntreAyB_ByINE = True
            End If
        Else
            Application.DoEvents()
        End If

        If ExisteLimiteEntreAyB_ByINE = True Then
            reportTMP.Dispose()
            Exit Function
        End If


        'cadSQL = "SELECT DISTINCT TL.municipio as INE_L,TR.municipio as INE_R ,TL.pertenencia as INEs_L,TR.pertenencia as INEs_R," & _
        '            "tl.nombre as Nombre1,tr.nombre as Nombre2 " & _
        '            "from(limites) " & _
        '            "inner join territorios TL on TL.poligono_carto=limites.LPOLY " & _
        '            "inner join territorios TR on TR.poligono_carto=limites.RPOLY " & _
        '            "where tl.municipio=" & INEA & " or tr.municipio=" & INEA & " " & _
        '            "or tl.pertenencia like '%" & INEA & "%' or tr.pertenencia like '%" & INEA & "%'"
        cadSQL = "SELECT DISTINCT TL.municipio as INE_L,TR.municipio as INE_R ,TL.pertenencia as INEs_L,TR.pertenencia as INEs_R," & _
                    "tl.nombre as Nombre1,tr.nombre as Nombre2 " & _
                    "from(limites) " & _
                    "inner join territorios TL on TL.poligono_carto=limites.LPOLY " & _
                    "inner join territorios TR on TR.poligono_carto=limites.RPOLY " & _
                    "where (tl.pertenencia like '%" & cad_INEB & "%' and tr.pertenencia like '%" & cad_INEA & "%') OR " & _
                    "(tr.pertenencia like '%" & cad_INEB & "%' and tl.pertenencia like '%" & cad_INEA & "%')"
        reportTMP = New DataTable
        If CargarRecordset(cadSQL, reportTMP) = True Then
            filas = reportTMP.Select
            If filas.Length = 0 Then
                Application.DoEvents()
            Else
                ExisteLimiteEntreAyB_ByINE = True
            End If
        Else
            Application.DoEvents()
        End If
        reportTMP.Dispose()
        reportTMP = Nothing

    End Function

    Function ExportarCentroidesDocumento2XYZ(ByVal docu As docSIDCARTO, _
                                    ByVal RutaFich As String, Optional ByVal Appending As Boolean = False, _
                                    Optional ByVal listaCampos As String = "") As Boolean

        Dim reportTMP As DataTable
        Dim CadSQL As String
        Dim filas() As DataRow
        Dim cadCoord As String
        Dim nombre As String
        Dim anteriorNom As String
        Dim cadCentroid As String

        CadSQL = "SELECT nombre, dv_dumppoints(ST_AsText(ST_Centroid(geom))) as centroide " & _
                    "FROM contornos WHERE sellado='" & docu.Sellado & "'"

        reportTMP = New DataTable
        If CargarRecordset(CadSQL, reportTMP) = True Then
            filas = reportTMP.Select
            If filas.Length = 0 Then
                Application.DoEvents()
            Else
                Dim sw As New System.IO.StreamWriter(RutaFich, Appending, System.Text.Encoding.Unicode)
                anteriorNom = ""
                For Each fila As DataRow In filas
                    nombre = fila.Item("nombre").ToString
                    If nombre <> anteriorNom Then
                        sw.WriteLine("DESCRIPTION=Unknown Area Type")
                        sw.WriteLine("NAME=" & nombre)
                        'Exportamos las columnas seleccionadas
                        If listaCampos.IndexOf("Tipo,") > -1 Then sw.WriteLine("TIPO=" & docu.Tipo.ToString)
                        If listaCampos.IndexOf("Tomo") > -1 Then sw.WriteLine("TOMO=" & docu.Tomo.ToString)
                        If listaCampos.IndexOf("Estado") > -1 Then sw.WriteLine("ESTADO=" & docu.Estado.ToString)
                        If listaCampos.IndexOf("Escala") > -1 Then sw.WriteLine("ESCALA=" & docu.Escala.ToString)
                        If listaCampos.IndexOf("Fecha") > -1 Then sw.WriteLine("FECHA=" & docu.fechaPrincipal.ToString)
                        If listaCampos.IndexOf("Municipios") > -1 Then sw.WriteLine("MUNICIPIOS=" & docu.MunicipiosLiteral.ToString)
                        If listaCampos.IndexOf("Signatura") > -1 Then sw.WriteLine("SIGNATURA=" & docu.Signatura.ToString)
                        If listaCampos.IndexOf("Colección") > -1 Then sw.WriteLine("COLECCION=" & docu.Coleccion.ToString)
                        If listaCampos.IndexOf("Subdivisión") > -1 Then sw.WriteLine("SUBDIVISION=" & docu.Subdivision.ToString)
                        If listaCampos.IndexOf("Anejo") > -1 Then sw.WriteLine("ANEJO=" & docu.Anejo.ToString)
                        If listaCampos.IndexOf("Observaciones") > -1 Then sw.WriteLine("OBSERVACIONES=" & docu.ObservacionesStandard.ToString)
                        If listaCampos.IndexOf("Carpeta") > -1 Then sw.WriteLine("CARPETA=" & docu.proceCarpeta.ToString)
                        If listaCampos.IndexOf("Hoja") > -1 Then sw.WriteLine("HOJA=" & docu.proceHoja.ToString)
                        If listaCampos.IndexOf("Fecha Modificación") > -1 Then sw.WriteLine("MODIFICACION=" & docu.fechasModificaciones.ToString)
                        If listaCampos.IndexOf("Dimensiones") > -1 Then sw.WriteLine("DIMENSIONES=" & docu.Horizontal & " x " & docu.Vertical)
                        If listaCampos.IndexOf("Junta") > -1 Then sw.WriteLine("JUNTA=" & docu.JuntaEstadistica.ToString)
                        If listaCampos.IndexOf("Provincia") > -1 Then sw.WriteLine("PROVINCIA=" & docu.Provincias.ToString)
                        If listaCampos.IndexOf("Comentario") > -1 Then sw.WriteLine("COMENTARIO=" & docu.Observaciones.ToString)
                        anteriorNom = nombre
                    End If
                    cadCentroid = fila.Item("centroide").ToString.Replace("(", "").Replace(")", "")
                    Dim vertexList() As String = cadCentroid.Split(",")
                    If vertexList.Length > 1 Then
                        cadCoord = vertexList(0).ToString.Replace(",", ".") & "," & _
                                    vertexList(1).ToString.Replace(",", ".") & "," & "-999999"
                        sw.WriteLine(cadCoord)
                    End If
                Next
                sw.Close() : sw.Dispose() : sw = Nothing
            End If
        Else
            Application.DoEvents()
        End If
        reportTMP.Dispose()
        reportTMP = Nothing
        filas = Nothing

    End Function

    Function FunctionDameContornoMosaico(ByVal idMosaico As Integer) As String

        Dim reportTMP As DataTable
        Dim CadSQL As String = ""
        Dim filas() As DataRow
        Dim cadOUT As String = ""
        CadSQL = "SELECT ST_AsText(ST_UNION(geom)) as contorno FROM contornos WHERE sellado IN " & _
                    "(select archivo.numdoc from docdigital2archivo inner join archivo on archivo.idarchivo=" & _
                    "docdigital2archivo.archivo_id where docdigital_id=" & idMosaico & ")"
        reportTMP = New DataTable
        If CargarRecordset(CadSQL, reportTMP) = True Then
            filas = reportTMP.Select
            If filas.Length = 0 Then
                Application.DoEvents()
            Else
                For Each fila As DataRow In filas
                    cadOUT = fila.Item("contorno").ToString
                Next
                reportTMP.Dispose()
                reportTMP = Nothing
                filas = Nothing
            End If
        End If
        Return cadOUT


    End Function

    Sub FunctionDameContornoHojasMosaico(ByVal idMosaico As Integer, ByVal RutaFich As String)

        Dim reportTMP As DataTable
        Dim CadSQL As String = ""
        Dim filas() As DataRow
        Dim cadgeom As String = ""

        CadSQL = "SELECT archivo.numdoc AS sellado,archivo.subdivision AS subdiv,ST_AsText(contornos.geom) as contorno " & _
                    "FROM docdigital " & _
                    "INNER JOIN docdigital2archivo ON docdigital2archivo.docdigital_id=docdigital.iddocdigital " & _
                    "INNER JOIN archivo ON docdigital2archivo.archivo_id=archivo.idarchivo " & _
                    "INNER JOIN contornos ON archivo.idarchivo=contornos.archivo_id " & _
                    "WHERE docdigital_id=" & idMosaico
        reportTMP = New DataTable
        If CargarRecordset(CadSQL, reportTMP) = True Then
            Dim sw As New System.IO.StreamWriter(RutaFich, True, System.Text.Encoding.Unicode)

            filas = reportTMP.Select
            If filas.Length = 0 Then
                Application.DoEvents()
            Else
                For Each fila As DataRow In filas
                    cadgeom = fila.Item("contorno").ToString
                    cadgeom = cadgeom.Replace("MULTIPOLYGON((", "")
                    cadgeom = cadgeom.Replace("POLYGON((", "")
                    cadgeom = cadgeom.Replace("))", "")
                    cadgeom = cadgeom.Replace("(", "")
                    cadgeom = cadgeom.Replace(")", "")
                    Dim vertexList() As String = cadgeom.Split(",")
                    If vertexList.Length > 1 Then
                        sw.WriteLine("DESCRIPTION=Unknown Area Type")
                        sw.WriteLine("NAME=" & fila.Item("sellado").ToString & " - " & fila.Item("subdiv").ToString)
                        sw.WriteLine("CLOSED=YES")
                        For Each vertex As String In vertexList
                            sw.WriteLine(vertex & " -99999999")
                        Next
                    End If
                Next
                sw.Close() : sw.Dispose() : sw = Nothing
                reportTMP.Dispose()
                reportTMP = Nothing
                filas = Nothing
            End If
        End If



    End Sub


    Function ExportarContornosDocumento2XYZ(ByVal docu As docSIDCARTO, _
                                        ByVal RutaFich As String, Optional ByVal Appending As Boolean = False, _
                                        Optional ByVal listaCampos As String = "") As Boolean


        Dim reportTMP As DataTable
        Dim CadSQL As String
        Dim filas() As DataRow
        Dim cadCoord As String
        Dim cadVertex As String
        Dim nombre As String
        Dim anteriorNom As String
        Dim columna As String

        CadSQL = "SELECT nombre,dv_dumppoints(ST_AsText(geom)) as vertice FROM contornos WHERE sellado='" & docu.Sellado & "'"
        reportTMP = New DataTable
        If CargarRecordset(CadSQL, reportTMP) = True Then
            filas = reportTMP.Select
            If filas.Length = 0 Then
                Application.DoEvents()
            Else
                Dim sw As New System.IO.StreamWriter(RutaFich, Appending, System.Text.Encoding.Unicode)
                anteriorNom = ""
                For Each fila As DataRow In filas
                    nombre = fila.Item("nombre").ToString
                    If nombre <> anteriorNom Then
                        sw.WriteLine("DESCRIPTION=Unknown Area Type")
                        sw.WriteLine("NAME=" & nombre)
                        'Exportamos las columnas seleccionadas
                        If listaCampos.IndexOf("Tipo,") > -1 Then sw.WriteLine("TIPO=" & docu.Tipo.ToString)
                        If listaCampos.IndexOf("Tomo") > -1 Then sw.WriteLine("TOMO=" & docu.Tomo.ToString)
                        If listaCampos.IndexOf("Estado") > -1 Then sw.WriteLine("ESTADO=" & docu.Estado.ToString)
                        If listaCampos.IndexOf("Escala") > -1 Then sw.WriteLine("ESCALA=" & docu.Escala.ToString)
                        If listaCampos.IndexOf("Fecha") > -1 Then
                            If docu.fechaPrincipal Is Nothing Then
                                sw.WriteLine("FECHA=Sin fecha")
                            Else
                                sw.WriteLine("FECHA=" & docu.fechaPrincipal.ToString)
                            End If
                        End If
                        If listaCampos.IndexOf("Municipios") > -1 Then sw.WriteLine("MUNICIPIOS=" & docu.MunicipiosLiteral.ToString)
                        If listaCampos.IndexOf("Signatura") > -1 Then sw.WriteLine("SIGNATURA=" & docu.Signatura.ToString)
                        If listaCampos.IndexOf("Colección") > -1 Then sw.WriteLine("COLECCION=" & docu.Coleccion.ToString)
                        If listaCampos.IndexOf("Subdivisión") > -1 Then sw.WriteLine("SUBDIVISION=" & docu.Subdivision.ToString)
                        If listaCampos.IndexOf("Anejo") > -1 Then sw.WriteLine("ANEJO=" & docu.Anejo.ToString)
                        If listaCampos.IndexOf("Observaciones") > -1 Then sw.WriteLine("OBSERVACIONES=" & docu.ObservacionesStandard.ToString)
                        If listaCampos.IndexOf("Carpeta") > -1 Then sw.WriteLine("CARPETA=" & docu.proceCarpeta.ToString)
                        If listaCampos.IndexOf("Hoja") > -1 Then sw.WriteLine("HOJA=" & docu.proceHoja.ToString)
                        If listaCampos.IndexOf("Fecha Modificación") > -1 Then sw.WriteLine("MODIFICACION=" & docu.fechasModificaciones.ToString)
                        If listaCampos.IndexOf("Dimensiones") > -1 Then sw.WriteLine("DIMENSIONES=" & docu.Horizontal & " x " & docu.Vertical)
                        If listaCampos.IndexOf("Junta") > -1 Then sw.WriteLine("JUNTA=" & docu.JuntaEstadistica.ToString)
                        If listaCampos.IndexOf("Provincia") > -1 Then sw.WriteLine("PROVINCIA=" & docu.Provincias.ToString)
                        If listaCampos.IndexOf("Comentario") > -1 Then sw.WriteLine("COMENTARIO=" & docu.Observaciones.ToString)
                        sw.WriteLine("CLOSED=YES")
                        anteriorNom = nombre
                    End If
                    cadVertex = fila.Item("vertice").ToString.Replace("(", "").Replace(")", "")
                    Dim vertexList() As String = cadVertex.Split(",")
                    If vertexList.Length > 1 Then
                        cadCoord = vertexList(0).ToString.Replace(",", ".") & "," & _
                                    vertexList(1).ToString.Replace(",", ".") & "," & "-999999"
                        sw.WriteLine(cadCoord)
                    End If
                Next
                sw.Close() : sw.Dispose() : sw = Nothing
            End If
        Else
            Application.DoEvents()
        End If
        reportTMP.Dispose()
        reportTMP = Nothing
        filas = Nothing


    End Function

    Function DamePuntosContorno(ByVal Sellado As String, ByRef ListaP As ArrayList)

        Dim reportTMP As DataTable
        Dim CadSQL As String
        Dim filas() As DataRow
        Dim cadVertex As String

        Dim Vertice As Point

        CadSQL = "SELECT nombre,dv_dumppoints(ST_AsText(ST_Buffer(geom,1))) as vertice FROM contornos WHERE nombre like '" & Sellado & "%'"
        reportTMP = New DataTable
        If CargarRecordset(CadSQL, reportTMP) = True Then
            filas = reportTMP.Select
            If filas.Length = 0 Then
                Application.DoEvents()
            Else
                For Each fila As DataRow In filas
                    cadVertex = fila.Item("vertice").ToString.Replace("(", "").Replace(")", "")
                    Dim vertexList() As String = cadVertex.Split(",")
                    If vertexList.Length > 1 Then
                        Vertice.X = CType(vertexList(0).ToString.Replace(".", ","), Integer)
                        Vertice.Y = CType(vertexList(1).ToString.Replace(".", ","), Integer)
                        ListaP.Add(Vertice)
                    End If
                Next
            End If
        Else
            Application.DoEvents()
        End If
        reportTMP.Dispose()
        reportTMP = Nothing
        filas = Nothing

    End Function

End Module
