Imports System.Drawing.Printing


Module Basics
    '--------------------------------------------------------------------------------------------
    'Variables Globales
    '--------------------------------------------------------------------------------------------
    Public PlantillaGIS As String
    Public rutaRepo As String
    Public rutaRepoGeorref As String
    Public rutaRepoGeorrefBase As String
    Public rutaRepoInventarioInfo As String
    Public CalidadFavorita As String
    Public rutaRepoWeb As String
    Public MapaBase As String
    Public RutaRejillaNTV2 As String

    Public DB_Servidor As String
    Public DB_Port As Long
    Public DB_Instancia As String
    Public DB_User As String
    Public DB_Pass As String

    Public accessUser As String
    Public accessPass As String
    Public usuarioMyApp As myAppUser



    'Public App_User As String = ""
    'Public App_Pass As String
    'Public App_Permiso As Integer
    'Public App_Machine As String = ""

    Public urlAbsysLink As String
    Public canonicalURLCatalogoNew As String
    Public path7zUtility As String
    Public VisorECW As String
    Public VisorJPG As String
    Public VisorPrint As String
    Public AplicacionTitulo As String = "CartoSEE"
    Public subVersion As String = "2.5.1" '2.5 liberado el 31-Octubre-2012
    Public ActivarMiniaturas As Boolean
    Public AppFolderSetting As String
    Public ficheroLogger As String
    Public modoDelegaciones As Boolean = False

    'Carrito de la compra
    Public CarritoCompra As New ArrayList
    Public CarritoCompraOld() As docSIDCARTO

    'Plantillas metadatos
    Public RutasPlantillasMetadatos() As TiposDocumento
    Public rutaCentroDescargas As String
    Public rutaCentroDescargasScript As String
    Public rutaRepoThumbs As String

    Public Const visorCartociudad As String = "https://www.cartociudad.es/visor/"
    Public urlCDDSearchEngine As String
    Public urlGazetteerNGBESearchEngine As String


    'Encabezados para las consultas
    Public Encabezados(20) As EncabezadosConsulta
    Public TipoTerritoriosEditables() As String = {"Municipio histórico", "Territorio histórico"}

    Public Sub LeerConfiguracionINI()

        'Pasos previos a leer por primera vez el INI
        AppFolderSetting = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\Develmap\cartoSEE"
        Application.DoEvents()
        Try
            If Not System.IO.Directory.Exists(AppFolderSetting) Then
                System.IO.Directory.CreateDirectory(AppFolderSetting)
                System.IO.File.Copy(My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".ini", AppFolderSetting & "\" & My.Application.Info.AssemblyName & ".ini")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & ". La aplicación no puede iniciarse", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End
        End Try

        ficheroLogger = AppFolderSetting & "\logger.log"
        rutaRepo = LeeIni("Repositorio", "rutaRepo")
        rutaRepoWeb = LeeIni("Repositorio", "rutaRepoWeb")
        rutaRepoGeorref = LeeIni("Repositorio", "rutaRepoGeorref")
        rutaRepoGeorrefBase = LeeIni("Repositorio", "rutaRepoGeorrefBase")
        rutaRepoInventarioInfo = LeeIni("Repositorio", "rutaRepoInventarioInfo").Trim
        rutaCentroDescargas = LeeIni("Metadatos", "rutaCentroDescargas").Trim
        rutaRepoThumbs = LeeIni("Metadatos", "rutaRepoThumbs").Trim
        urlAbsysLink = LeeIni("Configuracion", "urlAbsysLink")
        urlCDDSearchEngine = LeeIni("Configuracion", "urlCDDSearchEngine")
        urlGazetteerNGBESearchEngine = LeeIni("Configuracion", "urlGazetteerNGBESearchEngine")


        canonicalURLCatalogoNew = LeeIni("Configuracion", "canonicalURLCatalogoNew")

        CalidadFavorita = LeeIni("Repositorio", "CalidadFavorita")
        RutaRejillaNTV2 = LeeIni("Configuracion", "RutaRejillaNTV2")
        path7zUtility = LeeIni("Configuracion", "path7zUtility")

        'Utiliza la tabla tbusuarios para validar el tipo de usuario
        If LeeIni("Database", "ModoDelegaciones").ToUpper = "SI" Then
            modoDelegaciones = True
        Else
            modoDelegaciones = False
        End If

        DB_Servidor = LeeIni("Database", "Servidor")
        DB_Port = Val(LeeIni("Database", "Port"))
        DB_Instancia = LeeIni("Database", "Instancia")
        DB_User = LeeIni("Database", "User")
        DB_Pass = LeeIni("Database", "Pass")
        'Establezco los valores por defecto
        If modoDelegaciones = True Then
            If DB_User.Trim = "" Then DB_User = "badasid"
            If DB_Pass.Trim = "" Then DB_Pass = ".delegaciones"
        Else
            If DB_User.Trim = "" Then DB_User = "badasid"
            If DB_Pass.Trim = "" Then DB_Pass = "sidbada"
        End If

        VisorECW = LeeIniLocal("Visores", "VisorECW")
        VisorJPG = LeeIniLocal("Visores", "VisorJPG")
        VisorPrint = LeeIniLocal("Visores", "VisorPrint")
        MapaBase = LeeIni("Visores", "MapaBase")
        PlantillaGIS = LeeIni("Visores", "PlantillaGIS")
        If LeeIni("Repositorio", "ActivarMiniaturas") = "SI" Then
            ActivarMiniaturas = True
        Else
            ActivarMiniaturas = False
        End If


        'Lectura de los encabezados del archivo INI
        'Si no hay configuraci´ñon de encabezados en el INI Local, los leemos del común en Remoto
        Application.DoEvents()

        If LeeIniLocal("QueryFields", "Visible1") <> "" Then
            For iBucle As Integer = 1 To Encabezados.Length - 1
                Encabezados(iBucle).Visible = IIf(LeeIniLocal("QueryFields", "Visible" & iBucle.ToString) = "SI", True, False)
                Encabezados(iBucle).NombreEncabezado = LeeIniLocal("QueryFields", "Nombre" & iBucle.ToString)
                Encabezados(iBucle).Anchura = 90
            Next iBucle
        Else
            For iBucle As Integer = 1 To Encabezados.Length - 1
                Encabezados(iBucle).Visible = IIf(LeeIni("QueryFields", "Visible" & iBucle.ToString) = "SI", True, False)
                Encabezados(iBucle).NombreEncabezado = LeeIni("QueryFields", "Nombre" & iBucle.ToString)
                Encabezados(iBucle).Anchura = 90
            Next iBucle
            ' Y ahora los guardamos en el local para que la próxima vez se lean del Local y no del remoto
            Application.DoEvents()
            For iBucle As Integer = 1 To Encabezados.Length - 1
                EscribeIniLocal("QueryFields", "Visible" & iBucle.ToString, IIf(Encabezados(iBucle).Visible = True, "SI", "NO"))
            Next
        End If





    End Sub

    Public Function SacarFileDeRuta(ByVal PathCompleto As String) As String

        Dim i_path As Integer
        SacarFileDeRuta = ""
        If String.IsNullOrEmpty(PathCompleto) Then
            Exit Function
        End If
        If PathCompleto.Trim = "" Then
            Exit Function
        End If


        For i_path = PathCompleto.Length To 1 Step -1
            If PathCompleto.Substring(i_path - 1, 1) = "\" Then Exit For
        Next i_path
        If i_path = 1 Then
            SacarFileDeRuta = ""
            Exit Function
        End If
        SacarFileDeRuta = Right(PathCompleto, Len(PathCompleto) - i_path)
        SacarFileDeRuta = PathCompleto.Substring(i_path, PathCompleto.Length - i_path)

    End Function

    Public Function SacarDirDeRuta(ByVal PathCompleto As String) As String

        Dim i_path As Integer
        SacarDirDeRuta = ""
        If String.IsNullOrEmpty(PathCompleto) Then
            Exit Function
        End If
        For i_path = Len(PathCompleto) To 1 Step -1
            If Mid(PathCompleto, i_path, 1) = "\" Then Exit For
        Next i_path
        If i_path = 1 Then
            SacarDirDeRuta = ""
            Exit Function
        End If

        SacarDirDeRuta = Left(PathCompleto, i_path)

    End Function

    Function obtenerIntervalo(cadenaIn As String, separador As String, ByRef valor1 As String, ByRef valor2 As String) As Boolean

        Dim partes() As String = cadenaIn.Trim.Split(separador)
        If partes.Length <> 2 Then Return False

        If IsNumeric(partes(0)) And IsNumeric(partes(1)) Then
            valor1 = partes(0)
            valor2 = partes(1)
            Return True
        End If
        Return False

    End Function

    Function obtenerIntervalo(cadenaIn As String, separador As String, ByRef listaSellos As ArrayList) As Boolean

        Dim partes() As String = cadenaIn.Trim.Split(separador)
        For Each parte In partes
            listaSellos.Add(parte)
        Next
        Return True
        If listaSellos.Count > 0 Then
            Return True
        Else
            Return False
        End If

    End Function

    Sub cargarImagenFromWeb(ByVal Lienzo As PictureBox, ByVal RutaImagen As String, ByVal RutaDefault As String, Optional stretchMode As Boolean = True, Optional pathInTag As String = "")

        Dim imageLoaded As String
        Try
            If IO.File.Exists(RutaImagen) Then
                imageLoaded = RutaImagen
            Else
                imageLoaded = RutaDefault
            End If
            Lienzo.Load(imageLoaded)
            Lienzo.SizeMode = IIf(stretchMode = True, PictureBoxSizeMode.StretchImage, PictureBoxSizeMode.Zoom)
            If pathInTag = "" Then
                Lienzo.Tag = imageLoaded
            Else
                Lienzo.Tag = pathInTag
            End If

        Catch ex As Exception
            GenerarLOG(ex.Message)
            ModalError(ex.Message)
        End Try

    End Sub


    Public Function ImprimirPDF(ByVal RutaPDF As String, ByVal Impresora As String) As Boolean

        Dim Proceso As Process
        Dim si As New ProcessStartInfo
        Dim RutaFoxit As String = "c:\Archivos de Programa\Foxit Software\Foxit Reader\Foxit Reader.exe"
        Dim CancelarProcesoImpresion As Boolean
        ImprimirPDF = False
        If System.IO.File.Exists(RutaFoxit) = False Then
            MessageBox.Show("No se localiza el Módulo de impresión de PDF", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Function
        End If

        si.FileName = RutaFoxit
        si.Arguments = "/p """ & RutaPDF & """ """ & Impresora & """"
        'si.WindowStyle = ProcessWindowStyle.Normal
        'Lanzamos el proceso para ejecutar el script con global mapper
        CancelarProcesoImpresion = False
        Proceso = New Process
        Proceso = Process.Start(si)
        'Controlo informando de que le proceso está en marcha
        Do
            Application.DoEvents()
            If CancelarProcesoImpresion = True Then
                Proceso.Kill()
            End If
        Loop While Proceso.HasExited = False
        ImprimirPDF = True
        Proceso.CloseMainWindow()
        Proceso.Kill()
        Proceso.Dispose()
        Proceso = Nothing
        si = Nothing

    End Function


    Function ImprimirPDFAcrobat(ByVal RutaFichero As String) As Boolean
        'Dim argumento As String = "/t ""mytest.pdf"" ""My Windows PrinterName"""
        Dim argumento As String = "/t """ & RutaFichero & """"
        '        Dim starter As New ProcessStartInfo("C:\Archivos de programa\Adobe\Reader 9.0\Reader\AcroRd32.exe", argumento)

        Dim Proceso As Process
        Dim si As New ProcessStartInfo

        si.FileName = "C:\Archivos de programa\Adobe\Reader 9.0\Reader\AcroRd32.exe"
        si.Arguments = argumento

        'starter.CreateNoWindow = True
        'starter.RedirectStandardOutput = True
        Proceso = New Process
        Proceso = Process.Start(si)
        'Controlo informando de que le proceso está en marcha
        Do
            Application.DoEvents()
            'If CancelarProceso = True Then
            ' Proceso.Kill()
            'End If
        Loop While Proceso.HasExited = False
        ImprimirPDFAcrobat = True
        Proceso.CloseMainWindow()
        Proceso.Kill()
        Proceso.Dispose()
        Proceso = Nothing
        si = Nothing
    End Function

    Function LanzarVisorExterno(ByVal RutaFichero As String) As Boolean

        LanzarVisorExterno = False

        If System.IO.File.Exists(RutaFichero) = False Then
            If rutaRepoWeb <> "" Then
                RutaFichero = RutaFichero.Replace(rutaRepo, rutaRepoWeb)
                RutaFichero = RutaFichero.Replace("\", "/")
                If IsConnectionAvailable(RutaFichero) = True Then
                    LanzarVisorExterno = LanzarPDFWeb(RutaFichero)
                    Exit Function
                Else
                    MessageBox.Show("No se encuentra el fichero", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Function
                End If
            Else
                MessageBox.Show("No se encuentra el fichero" & System.Environment.NewLine & _
                                RutaFichero, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Function
            End If
        End If

        Try
            If VisorECW <> "" And RutaFichero.ToLower.EndsWith(".ecw") Then
                Process.Start(VisorECW, RutaFichero)
            ElseIf VisorJPG <> "" And RutaFichero.ToLower.EndsWith(".jpg") Then
                Process.Start(VisorJPG, RutaFichero)
            Else
                Process.Start(RutaFichero)
            End If
            LanzarVisorExterno = True
        Catch ex As Exception
            MessageBox.Show("Error:" & ex.Message & System.Environment.NewLine & _
                            RutaFichero, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try

    End Function

    Function LanzarPDFWeb(ByVal URLPDF As String) As Boolean

        LanzarPDFWeb = False
        If IsConnectionAvailable(URLPDF) = False Then
            MessageBox.Show("El recurso online no se encuentra disponible", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Function
        End If

        Try
            Process.Start(URLPDF)
            LanzarPDFWeb = True
        Catch ex As Exception
            'No tengo claro porque lanza una excepcion
            'MessageBox.Show("Error:" & ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try

    End Function


    Function DameAutoNomFichero(Optional ByVal Extension As String = ".txt") As String

        Dim fechaFichero As Date = Now
        DameAutoNomFichero = fechaFichero.Year & _
                        String.Format("{0:00}", CInt(fechaFichero.Month.ToString)) & _
                        String.Format("{0:00}", CInt(fechaFichero.Month.ToString)) & _
                        String.Format("{0:00}", CInt(fechaFichero.Month.ToString)) & _
                        String.Format("{0:00}", CInt(fechaFichero.Hour.ToString)) & _
                        String.Format("{0:00}", CInt(fechaFichero.Minute.ToString)) & _
                        String.Format("{0:00}", CInt(fechaFichero.Second.ToString)) & Extension



    End Function

    Function ExisteLaImpresora(ByVal NombreImpresora As String) As Boolean

        Dim pd As New PrintDocument

        ExisteLaImpresora = False
        For Each impresoras As String In PrinterSettings.InstalledPrinters
            If impresoras.ToString = NombreImpresora Then
                ExisteLaImpresora = True
                Exit For
            End If
        Next
        pd.Dispose()
        pd = Nothing

    End Function
    Public Function IsConnectionAvailable(ByVal NombreURL As String) As Boolean

        Dim objUrl As New System.Uri(NombreURL)
        Dim objWebReq As System.Net.WebRequest
        objWebReq = System.Net.WebRequest.Create(objUrl)
        Dim objResp As System.Net.WebResponse
        Try
            objResp = objWebReq.GetResponse
            objResp.Close()
            objWebReq = Nothing
            Return True
        Catch ex As Exception
            MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            objWebReq = Nothing
            Return False
        End Try

    End Function

    Function ArrayEliminarDuplicados(ByVal items() As docSIDCARTO) As docSIDCARTO()
        Dim noDupsArrList As New ArrayList
        For i As Integer = 0 To items.Length - 1
            If Not noDupsArrList.Contains(items(i)) Then
                noDupsArrList.Add(items(i))
            End If
        Next

        Dim uniqueItems() As docSIDCARTO
        ReDim uniqueItems(noDupsArrList.Count - 1)
        noDupsArrList.CopyTo(uniqueItems)
        Return uniqueItems

    End Function

    Sub LanzarSpinner()

        dlgSpinner.MdiParent = MDIPrincipal
        dlgSpinner.Show()

    End Sub

    Sub LanzarSpinner(ByVal Texto As String)

        dlgSpinner.MdiParent = MDIPrincipal
        dlgSpinner.Label1.Text = Texto
        dlgSpinner.Show()

    End Sub

    Sub CerrarSpinner()
        dlgSpinner.Close()
    End Sub

    Function testWriteInPath(ByVal rutaRemota As String) As Boolean

        If rutaRemota.Trim = "" Then
            MessageBox.Show("Ruta no definida", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        End If
        Try
            Dim file As System.IO.FileStream
            file = System.IO.File.Create(rutaRemota & "\testdummy1234.txt")
            file.Close()
            System.IO.File.Delete(rutaRemota & "\testdummy1234.txt")
            Return True
        Catch ex As Exception
            MessageBox.Show("No dispone de permiso de escritura en el repositorio.en " & rutaRemota & Environment.NewLine() & _
                            "Consulte con su administrador de sistemas.", _
                            AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        End Try
    End Function

End Module
