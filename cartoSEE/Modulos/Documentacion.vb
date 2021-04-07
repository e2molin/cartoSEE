Module Documentacion



    Public Structure Punto
        Dim X As Double
        Dim Y As Double
    End Structure

    Public Structure Notificacion
        Dim id As Integer
        Dim referencia As String
        Dim fecha As Date
        Dim usuario As String
        Dim estado As String
        Dim descripcion As String
    End Structure

    Public Structure usuario
        Dim id As Integer
        Dim nombre As String
        Dim apellidos As String
        Dim telefono As String
        Dim loginUser As String
        Dim loginPass As String
        Dim codPermiso As Integer
    End Structure


    Public Structure TiposDocumento

        Dim idTipodoc As Integer
        Dim NombreTipo As String
        Dim RepoTipo As String
        Dim RutaMetadatosTipo As String
        Dim PrefijoNom As String

    End Structure
    Public Structure EncabezadosConsulta

        Dim NombreEncabezado As String
        Dim Anchura As Integer
        Dim Visible As Boolean

    End Structure

    Public Structure RenFich

        Dim NombreAntiguo As String
        Dim NombreNuevo As String

    End Structure

    Public Structure Documento

        Dim Indice As Long
        Dim Tipo As String
        Dim Sello As String
        Dim FicheroPDF As String

    End Structure

    Public Structure Territorio

        Dim Indice As Long
        Dim Nombre As String
        Dim TipoEntidad As String
        Dim INE As String

    End Structure

    Public Structure docSIDCARTO
        Dim Indice As Integer
        Dim Sellado As String                              'Sellado
        Dim Tipo As String
        Dim CodTipo As Integer
        Dim CodEstado As String
        Dim Estado As String
        Dim Escala As String
        Dim Tomo As String
        Dim proceHoja As String
        Dim proceCarpeta As String
        Dim NumDisco As String
        Dim fechaPrincipal As String
        Dim fechasModificaciones As String
        Dim Anejo As String
        Dim Signatura As String
        Dim Coleccion As String
        Dim Subdivision As String
        Dim Vertical As String
        Dim Horizontal As String
        Dim Observaciones As String
        Dim ObservacionesStandard As String
        Dim Municipios As String
        Dim MunicipiosINE As String
        Dim MunicipiosLiteral As String
        Dim Provincias As String
        Dim JuntaEstadistica As String
        Dim FicheroJPG As String
        Dim Alto As String
        Dim Ancho As String
        Dim ProvinciaRepo As Integer
        Dim subTipoDoc as string
    End Structure


    Sub DameDocumentacionSIDCARTO_ByConsulta(ByVal ConsultaSQL As String, _
                                             ByRef ListaDoc() As docSIDCARTO)

        CargarListaDocumentosSIDCARTO(ConsultaSQL, ListaDoc)

    End Sub


    ''' <summary>
    ''' Obtener los documentos del SIDCARTO dado un filtro independiente de la vinculación geográfica
    ''' </summary>
    ''' <param name="Filtro">Filtro SQL que se aplica</param>
    ''' <param name="ListaDoc">Array para almacenar los datos del SIDCARTO</param>
    ''' <remarks></remarks>
    Sub DameDocumentacionSIDCARTO_ByFiltro(ByVal Filtro As String, _
                                ByRef ListaDoc() As docSIDCARTO, ByRef CadenaSQL As String)

        CadenaSQL = "SELECT archivo.idarchivo,archivo.numdoc,archivo.escala,archivo.tomo,archivo.coleccion," & _
                    "archivo.subdivision,archivo.fechaprincipal,archivo.fechasmodificaciones,archivo.anejo,archivo.vertical, " & _
                    "archivo.horizontal,archivo.tipodoc_id,archivo.estadodoc_id,archivo.procecarpeta,archivo.procehoja,archivo.subtipo," & _
                    "archivo.juntaestadistica,archivo.signatura,archivo.observestandar_id,archivo.observaciones," & _
                    "tbtipodocumento.tipodoc as Tipo," & _
                    "tbestadodocumento.estadodoc as Estado," & _
                    "tbobservaciones.observestandar," & _
                    "munihisto.cod_munihisto,munihisto.nombremunicipiohistorico,munihisto.provincia_id as CodProv,provincias.nombreprovincia " & _
                    "FROM archivo " & _
                    "INNER JOIN tbtipodocumento ON tbtipodocumento.idtipodoc=archivo.tipodoc_id " & _
                    "INNER JOIN tbestadodocumento ON tbestadodocumento.idestadodoc=archivo.estadodoc_id " & _
                    "INNER JOIN  archivo2munihisto  ON archivo2munihisto.archivo_id=archivo.idarchivo " & _
                    "LEFT JOIN  tbobservaciones  ON tbobservaciones.idobservestandar=archivo.observestandar_id " & _
                    "INNER JOIN munihisto on munihisto.idmunihisto= archivo2munihisto.munihisto_id " & _
                    "INNER JOIN provincias on munihisto.provincia_id= provincias.idprovincia " & _
                    "WHERE archivo.idarchivo>0 " & Filtro & " order by archivo.idarchivo"
        CargarListaDocumentosSIDCARTO(CadenaSQL, ListaDoc)

    End Sub

    Sub DameDocumentacionSIDCARTO_ByINEActual(ByVal CodigoINEMuni As Integer, _
                                ByRef ListaDoc() As docSIDCARTO, ByRef CadenaSQL As String, _
                                Optional ByVal Filtro As String = "")

        CadenaSQL = "SELECT archivo.idarchivo,archivo.numdoc,archivo.escala,archivo.tomo,archivo.coleccion," & _
                        "archivo.subdivision,archivo.fechaprincipal,archivo.fechasmodificaciones,archivo.anejo,archivo.vertical, " & _
                        "archivo.horizontal,archivo.tipodoc_id,archivo.estadodoc_id,archivo.procecarpeta,archivo.procehoja,archivo.subtipo," & _
                        "archivo.juntaestadistica,archivo.signatura,archivo.observestandar_id,archivo.observaciones," & _
                        "tbtipodocumento.tipodoc as Tipo," & _
                        "tbestadodocumento.estadodoc as Estado," & _
                        "tbobservaciones.observestandar," & _
                        "munihisto.cod_munihisto,munihisto.nombremunicipiohistorico,munihisto.provincia_id as CodProv,provincias.nombreprovincia " & _
                        "FROM archivo " & _
                        "INNER JOIN tbtipodocumento ON tbtipodocumento.idtipodoc=archivo.tipodoc_id " & _
                        "INNER JOIN tbestadodocumento ON tbestadodocumento.idestadodoc=archivo.estadodoc_id " & _
                        "INNER JOIN archivo2munihisto  ON archivo2munihisto.archivo_id=archivo.idarchivo " & _
                        "LEFT JOIN  tbobservaciones  ON tbobservaciones.idobservestandar=archivo.observestandar_id " & _
                        "INNER JOIN munihisto ON munihisto.idmunihisto= archivo2munihisto.munihisto_id " & _
                        "INNER JOIN provincias on munihisto.provincia_id= provincias.idprovincia " & _
                        "WHERE archivo.idarchivo IN " & _
                        "(SELECT archivo_id FROM archivo2munihisto " & _
                        "INNER JOIN munihisto ON munihisto.idmunihisto= archivo2munihisto.munihisto_id " & _
                        "WHERE munihisto.cod_Muni=" & CodigoINEMuni.ToString & ") " & _
                        IIf(Filtro = "", "", " " & Filtro & "") & " " & _
                        "order by archivo.idarchivo"

        CargarListaDocumentosSIDCARTO(CadenaSQL, ListaDoc)


    End Sub

    ''' <summary>
    ''' Obtener los documentos del SIDCARTO dado el Codigo INE Histórico del Municipio
    ''' </summary>
    ''' <param name="CodigoMuni">Código Historico del municipio a consultar</param>
    ''' <param name="ListaDoc">Array para almacenar los datos del SIDCARTO</param>
    ''' <param name="Filtro">Cadena SQL opcional para limitar la consulta</param>
    ''' <remarks></remarks>
    Sub DameDocumentacionSIDCARTO(ByVal CodigoMuni As Integer, _
                                ByRef ListaDoc() As docSIDCARTO, ByRef CadenaSQL As String, _
                                Optional ByVal Filtro As String = "")

        CadenaSQL = "SELECT archivo.idarchivo,archivo.numdoc,archivo.escala,archivo.tomo,archivo.coleccion," & _
                        "archivo.subdivision,archivo.fechaprincipal,archivo.fechasmodificaciones,archivo.anejo,archivo.vertical, " & _
                        "archivo.horizontal,archivo.tipodoc_id,archivo.estadodoc_id,archivo.procecarpeta,archivo.procehoja,archivo.subtipo," & _
                        "archivo.juntaestadistica,archivo.signatura,archivo.observestandar_id,archivo.observaciones," & _
                        "tbtipodocumento.tipodoc as Tipo," & _
                        "tbestadodocumento.estadodoc as Estado," & _
                        "tbobservaciones.observestandar," & _
                        "munihisto.cod_munihisto,munihisto.nombremunicipiohistorico,munihisto.provincia_id as CodProv,provincias.nombreprovincia " & _
                        "FROM archivo " & _
                        "INNER JOIN tbtipodocumento on tbtipodocumento.idtipodoc=archivo.tipodoc_id " & _
                        "INNER JOIN tbestadodocumento on tbestadodocumento.idestadodoc=archivo.estadodoc_id " & _
                        "INNER JOIN  archivo2munihisto  on archivo2munihisto.archivo_id=archivo.idarchivo " & _
                        "LEFT JOIN  tbobservaciones  on tbobservaciones.idobservestandar=archivo.observestandar_id " & _
                        "INNER JOIN munihisto on munihisto.idmunihisto= archivo2munihisto.munihisto_id " & _
                        "INNER JOIN provincias on munihisto.provincia_id = provincias.idprovincia " & _
                        "WHERE archivo.idarchivo IN " & _
                        "(SELECT archivo_id from archivo2munihisto " & _
                        "INNER JOIN munihisto on munihisto.idmunihisto= archivo2munihisto.munihisto_id " & _
                        "WHERE munihisto.idmunihisto=" & CodigoMuni.ToString & ") " & _
                        IIf(Filtro = "", "", " " & Filtro & "") & " " & _
                        "order by archivo.idarchivo"

        CargarListaDocumentosSIDCARTO(CadenaSQL, ListaDoc)

    End Sub

    ''' <summary>
    ''' Obtener los documentos del SIDCARTO de una provincia dado su Codigo INE
    ''' </summary>
    ''' <param name="CodigoProvincia">Código INE de la provincia a consultar</param>
    ''' <param name="ListaDoc">Array para almacenar los datos del SIDCARTO</param>
    ''' <param name="Filtro">Cadena SQL opcional para limitar la consulta</param>
    ''' <remarks></remarks>
    Sub DameDocumentacionSIDCARTO_ByProvincia(ByVal CodigoProvincia As Integer, _
                                ByRef ListaDoc() As docSIDCARTO, ByRef CadenaSQL As String, _
                                Optional ByVal Filtro As String = "")

        CadenaSQL = "SELECT archivo.idarchivo,archivo.numdoc,archivo.escala,archivo.tomo,archivo.coleccion," & _
                        "archivo.subdivision,archivo.fechaprincipal,archivo.fechasmodificaciones,archivo.anejo,archivo.vertical, " & _
                        "archivo.horizontal,archivo.tipodoc_id,archivo.estadodoc_id,archivo.procecarpeta,archivo.procehoja,archivo.subtipo," & _
                        "archivo.juntaestadistica,archivo.signatura,archivo.observestandar_id,archivo.observaciones," & _
                        "tbtipodocumento.tipodoc as Tipo," & _
                        "tbestadodocumento.estadodoc as Estado," & _
                        "tbobservaciones.observestandar," & _
                        "munihisto.cod_munihisto,munihisto.nombremunicipiohistorico,munihisto.provincia_id as CodProv,provincias.nombreprovincia " & _
                        "FROM archivo " & _
                        "INNER JOIN tbtipodocumento on tbtipodocumento.idtipodoc=archivo.tipodoc_id " & _
                        "INNER JOIN tbestadodocumento on tbestadodocumento.idestadodoc=archivo.estadodoc_id " & _
                        "INNER JOIN  archivo2munihisto  on archivo2munihisto.archivo_id=archivo.idarchivo " & _
                        "LEFT JOIN  tbobservaciones  on tbobservaciones.idobservestandar=archivo.observestandar_id " & _
                        "INNER JOIN munihisto on munihisto.idmunihisto= archivo2munihisto.munihisto_id " & _
                        "INNER JOIN provincias on munihisto.provincia_id = provincias.idprovincia " & _
                        "WHERE archivo.provincia_id=" & CodigoProvincia & " " & _
                        IIf(Filtro = "", "", " " & Filtro & "") & " " & _
                        "order by archivo.idarchivo"
        CargarListaDocumentosSIDCARTO(CadenaSQL, ListaDoc)

    End Sub

    ''' <summary>
    ''' Obtener el documento del SIDCARTO dado su iddoc
    ''' </summary>
    ''' <param name="nIndice">Identificador del documento</param>
    ''' <param name="Docu">Variable para cargar el resultado</param>
    ''' <remarks></remarks>
    Sub DameDocumentacionSIDCARTO_byIndice(ByVal nIndice As Integer, _
                                ByRef Docu() As docSIDCARTO, ByRef CadenaSQL As String)

        CadenaSQL = "SELECT archivo.idarchivo,archivo.numdoc,archivo.escala,archivo.tomo,archivo.coleccion," & _
                        "archivo.subdivision,archivo.fechaprincipal,archivo.fechasmodificaciones,archivo.anejo,archivo.vertical, " & _
                        "archivo.horizontal,archivo.tipodoc_id,archivo.estadodoc_id,archivo.procecarpeta,archivo.procehoja,archivo.subtipo," & _
                        "archivo.juntaestadistica,archivo.signatura,archivo.observestandar_id,archivo.observaciones," & _
                        "tbtipodocumento.tipodoc as Tipo," & _
                        "tbestadodocumento.estadodoc as Estado," & _
                        "tbobservaciones.observestandar," & _
                        "munihisto.cod_munihisto,munihisto.nombremunicipiohistorico,munihisto.provincia_id as CodProv,provincias.nombreprovincia " & _
                        "FROM archivo " & _
                        "INNER JOIN tbtipodocumento on tbtipodocumento.idtipodoc=archivo.tipodoc_id " & _
                        "INNER JOIN tbestadodocumento on tbestadodocumento.idestadodoc=archivo.estadodoc_id " & _
                        "INNER JOIN  archivo2munihisto  on archivo2munihisto.archivo_id=archivo.idarchivo " & _
                        "LEFT JOIN  tbobservaciones  on tbobservaciones.idobservestandar=archivo.observestandar_id " & _
                        "INNER JOIN munihisto on munihisto.idmunihisto= archivo2munihisto.munihisto_id " & _
                        "INNER JOIN provincias on munihisto.provincia_id = provincias.idprovincia " & _
                        "WHERE archivo.idarchivo =" & nIndice

        CargarListaDocumentosSIDCARTO(CadenaSQL, Docu)

    End Sub

    ''' <summary>
    ''' Obtener el documento del SIDCARTO dado su número de sellado
    ''' </summary>
    ''' <param name="nSello">Número de sellado del documento</param>
    ''' <param name="ListaDoc">Array para cargar el resultado</param>
    ''' <remarks></remarks>
    Sub DameDocumentacionSIDCARTO_bySellado(ByVal nSello As Integer, _
                                ByRef ListaDoc() As docSIDCARTO, ByRef CadenaSQL As String)
        Dim SelloFormato As String
        SelloFormato = String.Format("{0:000000}", nSello)

        CadenaSQL = "SELECT archivo.idarchivo,archivo.numdoc,archivo.escala,archivo.tomo,archivo.coleccion," & _
                    "archivo.subdivision,archivo.fechaprincipal,archivo.fechasmodificaciones,archivo.anejo,archivo.vertical, " & _
                    "archivo.horizontal,archivo.tipodoc_id,archivo.estadodoc_id,archivo.procecarpeta,archivo.procehoja,archivo.subtipo," & _
                    "archivo.juntaestadistica,archivo.signatura,archivo.observestandar_id,archivo.observaciones," & _
                    "tbtipodocumento.tipodoc as Tipo," & _
                    "tbestadodocumento.estadodoc as Estado," & _
                    "tbobservaciones.observestandar," & _
                    "munihisto.cod_munihisto,munihisto.nombremunicipiohistorico,munihisto.provincia_id as CodProv,provincias.nombreprovincia " & _
                    "FROM archivo " & _
                    "INNER JOIN tbtipodocumento on tbtipodocumento.idtipodoc=archivo.tipodoc_id " & _
                    "INNER JOIN tbestadodocumento on tbestadodocumento.idestadodoc=archivo.estadodoc_id " & _
                    "INNER JOIN  archivo2munihisto  on archivo2munihisto.archivo_id=archivo.idarchivo " & _
                    "LEFT JOIN  tbobservaciones  on tbobservaciones.idobservestandar=archivo.observestandar_id " & _
                    "INNER JOIN munihisto on munihisto.idmunihisto= archivo2munihisto.munihisto_id " & _
                    "INNER JOIN provincias on munihisto.provincia_id = provincias.idprovincia " & _
                    "WHERE archivo.numdoc='" & SelloFormato & "' order by archivo.idarchivo"
        CargarListaDocumentosSIDCARTO(CadenaSQL, ListaDoc)

    End Sub

    ''' <summary>
    ''' Obtener el documento del SIDCARTO dado su número de sellado
    ''' </summary>
    ''' <param name="nSello1">Número de sellado inicial del intervalo del documento</param>
    ''' <param name="nSello2">Número de sellado final del intervalo del documento</param>
    ''' <param name="ListaDoc">Array para cargar el resultado</param>
    ''' <remarks></remarks>
    Sub DameDocumentacionSIDCARTO_byIntervaloSellado(ByVal nSello1 As Integer, ByVal nSello2 As Integer, _
                                ByRef ListaDoc() As docSIDCARTO, ByRef CadenaSQL As String)
        Dim SelloFormato1 As String
        Dim SelloFormato2 As String

        SelloFormato1 = String.Format("{0:000000}", nSello1)
        SelloFormato2 = String.Format("{0:000000}", nSello2)

        CadenaSQL = "SELECT archivo.idarchivo,archivo.numdoc,archivo.escala,archivo.tomo,archivo.coleccion," & _
                    "archivo.subdivision,archivo.fechaprincipal,archivo.fechasmodificaciones,archivo.anejo,archivo.vertical, " & _
                    "archivo.horizontal,archivo.tipodoc_id,archivo.estadodoc_id,archivo.procecarpeta,archivo.procehoja,archivo.subtipo," & _
                    "archivo.juntaestadistica,archivo.signatura,archivo.observestandar_id,archivo.observaciones," & _
                    "tbtipodocumento.tipodoc as Tipo," & _
                    "tbestadodocumento.estadodoc as Estado," & _
                    "tbobservaciones.observestandar," & _
                    "munihisto.cod_munihisto,munihisto.nombremunicipiohistorico,munihisto.provincia_id as CodProv,provincias.nombreprovincia " & _
                    "FROM archivo " & _
                    "INNER JOIN tbtipodocumento on tbtipodocumento.idtipodoc=archivo.tipodoc_id " & _
                    "INNER JOIN tbestadodocumento on tbestadodocumento.idestadodoc=archivo.estadodoc_id " & _
                    "INNER JOIN  archivo2munihisto  on archivo2munihisto.archivo_id=archivo.idarchivo " & _
                    "LEFT JOIN  tbobservaciones  on tbobservaciones.idobservestandar=archivo.observestandar_id " & _
                    "INNER JOIN munihisto on munihisto.idmunihisto= archivo2munihisto.munihisto_id " & _
                    "INNER JOIN provincias on munihisto.provincia_id = provincias.idprovincia " & _
                    "WHERE archivo.numdoc>='" & SelloFormato1 & "' and archivo.numdoc<='" & SelloFormato2 & "' order by archivo.idarchivo"
        CargarListaDocumentosSIDCARTO(CadenaSQL, ListaDoc)

    End Sub


    Function DameDocumentacionSIDCARTO_bySellado(ByVal nSello As Integer) As docSIDCARTO
        Dim SelloFormato As String
        Dim listadoc() As docSIDCARTO
        Dim cadenaSQL As String
        SelloFormato = String.Format("{0:000000}", nSello)
        cadenaSQL = "SELECT archivo.idarchivo,archivo.numdoc,archivo.escala,archivo.tomo,archivo.coleccion," & _
                    "archivo.subdivision,archivo.fechaprincipal,archivo.fechasmodificaciones,archivo.anejo,archivo.vertical, " & _
                    "archivo.horizontal,archivo.tipodoc_id,archivo.estadodoc_id,archivo.procecarpeta,archivo.procehoja,archivo.subtipo," & _
                    "archivo.juntaestadistica,archivo.signatura,archivo.observestandar_id,archivo.observaciones," & _
                    "tbtipodocumento.tipodoc as Tipo," & _
                    "tbestadodocumento.estadodoc as Estado," & _
                    "tbobservaciones.observestandar," & _
                    "munihisto.cod_munihisto,munihisto.nombremunicipiohistorico,munihisto.provincia_id as CodProv,provincias.nombreprovincia " & _
                    "FROM archivo " & _
                    "INNER JOIN tbtipodocumento on tbtipodocumento.idtipodoc=archivo.tipodoc_id " & _
                    "INNER JOIN tbestadodocumento on tbestadodocumento.idestadodoc=archivo.estadodoc_id " & _
                    "INNER JOIN  archivo2munihisto  on archivo2munihisto.archivo_id=archivo.idarchivo " & _
                    "LEFT JOIN  tbobservaciones  on tbobservaciones.idobservestandar=archivo.observestandar_id " & _
                    "INNER JOIN munihisto on munihisto.idmunihisto= archivo2munihisto.munihisto_id " & _
                    "INNER JOIN provincias on munihisto.provincia_id = provincias.idprovincia " & _
                    "WHERE archivo.numdoc='" & SelloFormato & "' order by archivo.idarchivo"
        CargarListaDocumentosSIDCARTO(CadenaSQL, listadoc)
        If listadoc.Length = 1 Then
            Return listadoc(0)
        Else
            MessageBox.Show("Error e2m:456. Consulte con su administrador", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Function

    Sub DameDocumentacionSIDCARTO_byListaSellado(ByVal listaSellos As ArrayList, ByRef ListaDoc() As docSIDCARTO, ByRef CadenaSQL As String)

        Dim selloFormato As String = ""

        For Each elemSello In listaSellos
            If selloFormato = "" Then
                selloFormato = "'" & String.Format("{0:000000}", elemSello) & "'"
                Continue For
            End If
            selloFormato = selloFormato & ",'" & String.Format("{0:000000}", elemSello) & "'"
        Next
        If selloFormato = "" Then Exit Sub
        CadenaSQL = "SELECT archivo.idarchivo,archivo.numdoc,archivo.escala,archivo.tomo,archivo.coleccion," & _
                    "archivo.subdivision,archivo.fechaprincipal,archivo.fechasmodificaciones,archivo.anejo,archivo.vertical, " & _
                    "archivo.horizontal,archivo.tipodoc_id,archivo.estadodoc_id,archivo.procecarpeta,archivo.procehoja,archivo.subtipo," & _
                    "archivo.juntaestadistica,archivo.signatura,archivo.observestandar_id,archivo.observaciones," & _
                    "tbtipodocumento.tipodoc as Tipo," & _
                    "tbestadodocumento.estadodoc as Estado," & _
                    "tbobservaciones.observestandar," & _
                    "munihisto.cod_munihisto,munihisto.nombremunicipiohistorico,munihisto.provincia_id as CodProv,provincias.nombreprovincia " & _
                    "FROM archivo " & _
                    "INNER JOIN tbtipodocumento on tbtipodocumento.idtipodoc=archivo.tipodoc_id " & _
                    "INNER JOIN tbestadodocumento on tbestadodocumento.idestadodoc=archivo.estadodoc_id " & _
                    "INNER JOIN  archivo2munihisto  on archivo2munihisto.archivo_id=archivo.idarchivo " & _
                    "LEFT JOIN  tbobservaciones  on tbobservaciones.idobservestandar=archivo.observestandar_id " & _
                    "INNER JOIN munihisto on munihisto.idmunihisto= archivo2munihisto.munihisto_id " & _
                    "INNER JOIN provincias on munihisto.provincia_id = provincias.idprovincia " & _
                    "WHERE archivo.numdoc in (" & selloFormato & ") order by archivo.idarchivo"
        CargarListaDocumentosSIDCARTO(CadenaSQL, ListaDoc)
    End Sub

    Sub DameDocumentacionSIDCARTO_byEntorno(ByVal Xmax As Integer, ByVal Ymax As Integer, _
                                            ByVal Xmin As Integer, ByVal Ymin As Integer, _
                                            ByRef ListaDoc() As docSIDCARTO, ByRef CadenaSQL As String)
        Dim CadenaSQLSpacial As String
        'Obtenemos las líneas límite dentro del entorno
        CadenaSQLSpacial = "SELECT sellado FROM contornos WHERE ST_Intersects(" & _
                            "contornos.geom," & _
                            "ST_GeomFromText('POLYGON((" & _
                            Xmin & " " & Ymax & "," & _
                            Xmax & " " & Ymax & "," & _
                            Xmax & " " & Ymin & "," & _
                            Xmin & " " & Ymin & "," & _
                            Xmin & " " & Ymax & "))',23030))"

        CadenaSQL = "SELECT archivo.idarchivo,archivo.numdoc,archivo.escala,archivo.tomo,archivo.coleccion," & _
                    "archivo.subdivision,archivo.fechaprincipal,archivo.fechasmodificaciones,archivo.anejo,archivo.vertical, " & _
                    "archivo.horizontal,archivo.tipodoc_id,archivo.estadodoc_id,archivo.procecarpeta,archivo.procehoja,archivo.subtipo," & _
                    "archivo.juntaestadistica,archivo.signatura,archivo.observestandar_id,archivo.observaciones," & _
                    "tbtipodocumento.tipodoc as Tipo," & _
                    "tbestadodocumento.estadodoc as Estado," & _
                    "tbobservaciones.observestandar," & _
                    "munihisto.cod_munihisto,munihisto.nombremunicipiohistorico,munihisto.provincia_id as CodProv,provincias.nombreprovincia " & _
                    "FROM archivo " & _
                    "INNER JOIN tbtipodocumento on tbtipodocumento.idtipodoc=archivo.tipodoc_id " & _
                    "INNER JOIN tbestadodocumento on tbestadodocumento.idestadodoc=archivo.estadodoc_id " & _
                    "INNER JOIN  archivo2munihisto  on archivo2munihisto.archivo_id=archivo.idarchivo " & _
                    "LEFT JOIN  tbobservaciones  on tbobservaciones.idobservestandar=archivo.observestandar_id " & _
                    "INNER JOIN munihisto on munihisto.idmunihisto= archivo2munihisto.munihisto_id " & _
                    "INNER JOIN provincias on munihisto.provincia_id = provincias.idprovincia " & _
                    "WHERE archivo.numdoc in (" & CadenaSQLSpacial & ") order by archivo.idarchivo LIMIT 1000"
        CargarListaDocumentosSIDCARTO(CadenaSQL, ListaDoc)
        Application.DoEvents()

    End Sub

    'Obtener los últimos 100 documentos introducidos
    Sub DameDocumentacionSIDCARTO_UltimosRegistros(ByRef ListaDoc() As docSIDCARTO, ByVal NumeroReg As Integer, ByRef cadenaSQL As String)

        cadenaSQL = "SELECT archivo.idarchivo,archivo.numdoc,archivo.escala,archivo.tomo,archivo.coleccion," & _
                    "archivo.subdivision,archivo.fechaprincipal,archivo.fechasmodificaciones,archivo.anejo,archivo.vertical, " & _
                    "archivo.horizontal,archivo.tipodoc_id,archivo.estadodoc_id,archivo.procecarpeta,archivo.procehoja,archivo.subtipo," & _
                    "archivo.juntaestadistica,archivo.signatura,archivo.observestandar_id,archivo.observaciones," & _
                    "tbtipodocumento.tipodoc as Tipo," & _
                    "tbestadodocumento.estadodoc as Estado," & _
                    "tbobservaciones.observestandar," & _
                    "munihisto.cod_munihisto,munihisto.nombremunicipiohistorico,munihisto.provincia_id as CodProv,provincias.nombreprovincia " & _
                    "FROM archivo " & _
                    "INNER JOIN tbtipodocumento on tbtipodocumento.idtipodoc=archivo.tipodoc_id " & _
                    "INNER JOIN tbestadodocumento on tbestadodocumento.idestadodoc=archivo.estadodoc_id " & _
                    "INNER JOIN  archivo2munihisto  on archivo2munihisto.archivo_id=archivo.idarchivo " & _
                    "LEFT JOIN  tbobservaciones  on tbobservaciones.idobservestandar=archivo.observestandar_id " & _
                    "INNER JOIN munihisto on munihisto.idmunihisto= archivo2munihisto.munihisto_id " & _
                    "INNER JOIN provincias on munihisto.provincia_id = provincias.idprovincia " & _
                    "ORDER BY archivo.idarchivo DESC LIMIT " & NumeroReg
        CargarListaDocumentosSIDCARTO(CadenaSQL, ListaDoc)

    End Sub


    ''' <summary>
    ''' Obtener los documentos correspondientes a documentación del SIDDAE a partir de una sql
    ''' </summary>
    ''' <param name="CadenaSQL">SQL Generada</param>
    ''' <param name="ListaDoc">Array para almacenar los resultados</param>
    ''' <remarks></remarks>
    Sub CargarListaDocumentosSIDCARTO(ByVal CadenaSQL As String, ByRef ListaDoc() As docSIDCARTO)

        Dim rcdDoc As DataTable
        Dim filas() As DataRow
        Dim contador As Long
        Dim Anterior As Integer

        rcdDoc = New DataTable
        If CargarRecordset(CadenaSQL, rcdDoc) = True Then
            filas = rcdDoc.Select
            ReDim ListaDoc(rcdDoc.Rows.Count - 1)
            contador = -1
            Anterior = 0
            For Each dR As DataRow In filas
                If CType(dR("idarchivo"), Integer) <> Anterior Then
                    contador = contador + 1
                    Anterior = Val(dR("idarchivo").ToString)
                    ListaDoc(contador).Indice = dR("idarchivo").ToString
                    ListaDoc(contador).Sellado = dR("numdoc").ToString
                    'Maquillamos el tomo
                    If dR("tomo").ToString = "" Then
                        ListaDoc(contador).Tomo = dR("tomo").ToString
                    Else
                        If IsNumeric(dR("tomo")) Then
                            ListaDoc(contador).Tomo = String.Format("{0:000}", CType(dR("tomo").ToString, Integer))
                        Else
                            If IsNumeric(dR("tomo").ToString.Replace("BIS", "")) Then
                                ListaDoc(contador).Tomo = String.Format("{0:000}", CType(dR("tomo").ToString.Replace("BIS", ""), Integer)) & "BIS"
                            Else
                                ListaDoc(contador).Tomo = dR("tomo").ToString
                            End If
                        End If
                    End If
                    If dR("fechaprincipal").ToString.Length >= 10 Then
                        'ListaDoc(contador).fechaPrincipal = dR("fechaprincipal").ToString.Substring(0, 10)
                        ListaDoc(contador).fechaPrincipal = FormatearFecha(dR("fechaprincipal"), "GERMAN")
                    End If

                    ListaDoc(contador).fechasModificaciones = dR("fechasmodificaciones").ToString
                    ListaDoc(contador).Tipo = dR("Tipo").ToString
                    ListaDoc(contador).CodTipo = dR("tipodoc_id").ToString
                    ListaDoc(contador).CodEstado = dR("estadodoc_id").ToString
                    ListaDoc(contador).Estado = dR("Estado").ToString
                    ListaDoc(contador).Escala = dR("Escala").ToString
                    ListaDoc(contador).Vertical = dR("Vertical").ToString
                    ListaDoc(contador).Horizontal = dR("Horizontal").ToString
                    ListaDoc(contador).proceHoja = dR("Procehoja").ToString
                    ListaDoc(contador).proceCarpeta = dR("Procecarpeta").ToString
                    ListaDoc(contador).subTipoDoc = dR("subtipo").ToString
                    ListaDoc(contador).NumDisco = "" 'dR("NumDisco").ToString
                    ListaDoc(contador).Anejo = dR("Anejo").ToString
                    ListaDoc(contador).Signatura = dR("Signatura").ToString
                    ListaDoc(contador).Coleccion = dR("Coleccion").ToString
                    ListaDoc(contador).Subdivision = dR("Subdivision").ToString
                    ListaDoc(contador).Municipios = dR("nombremunicipiohistorico").ToString
                    ListaDoc(contador).MunicipiosINE = String.Format("{0:0000000}", dR("cod_munihisto"))
                    ListaDoc(contador).MunicipiosLiteral = dR("nombremunicipiohistorico").ToString & _
                                                        " (" & String.Format("{0:0000000}", dR("cod_munihisto")) & ")"
                    ListaDoc(contador).Provincias = dR("nombreprovincia").ToString
                    ListaDoc(contador).ProvinciaRepo = CType(dR("numdoc").ToString.Substring(0, 2), Integer)
                    ListaDoc(contador).FicheroJPG = DirRepoProvinciaByINE(ListaDoc(contador).ProvinciaRepo) & _
                                                    "\" & dR("numdoc").ToString & ".jpg"
                    ListaDoc(contador).JuntaEstadistica = IIf(dR("JuntaEstadistica").ToString = "1", "Sí", "No")
                    ListaDoc(contador).ObservacionesStandard = dR("observestandar").ToString
                    ListaDoc(contador).Observaciones = dR("observaciones").ToString

                Else
                    ListaDoc(contador).Municipios = ListaDoc(contador).Municipios & _
                                            "#" & dR("nombremunicipiohistorico").ToString
                    ListaDoc(contador).MunicipiosINE = ListaDoc(contador).MunicipiosINE & _
                                            "#" & String.Format("{0:0000000}", dR("cod_munihisto"))
                    ListaDoc(contador).MunicipiosLiteral = ListaDoc(contador).MunicipiosLiteral & ", " & _
                                                    dR("nombremunicipiohistorico").ToString & _
                                                    " (" & String.Format("{0:0000000}", dR("cod_munihisto")) & ")"
                    If ListaDoc(contador).Provincias.Contains(dR("nombreprovincia").ToString) = False Then
                        ListaDoc(contador).Provincias = ListaDoc(contador).Provincias & ", " & dR("nombreprovincia").ToString
                    End If
                End If
            Next
            rcdDoc.Dispose()
            ReDim Preserve ListaDoc(contador)
        End If

    End Sub

End Module
