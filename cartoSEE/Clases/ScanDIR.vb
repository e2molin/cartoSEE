Public Class ScanDIR
    Public NumeroFicheros As String
    Public NumeroDirectorios As Integer
    Public DIRBase As String
    Dim mValorReDim As Integer
    Dim GAPArray As Integer
    Public ProcesarJPGcabeceras As Boolean
    Dim DBConexAccess As OleDb.OleDbConnection
    Dim ComandoSQL As OleDb.OleDbCommand
    Dim BasePreparada As Boolean

    Property ValorReDim() As Integer
        Get
            Return mValorReDim
        End Get
        Set(ByVal value As Integer)
            mValorReDim = value
        End Set
    End Property

    Property AnalizarJPGcabeceras() As Boolean
        Get
            Return ProcesarJPGcabeceras
        End Get
        Set(ByVal value As Boolean)
            ProcesarJPGcabeceras = value
        End Set
    End Property

    Function ListarArbolDirectorios(ByVal DirectorioIN As String, _
                                    ByRef ListaDir() As String) As Long

        Dim directoryPaths As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
        Dim directorySubPaths As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
        'Dim directoryPaths() As String
        Dim directoryPath As String
        Dim directorySubPath As String
        NumeroDirectorios = -1
        DIRBase = ""
        'Sacamos los ficheros del directorio de entrada
        'SacarListaArchivos(DirectorioIN, Extensiones)
        'Contenedor.Items.Add(DirectorioIN)
        NumeroDirectorios = NumeroDirectorios + 1
        If ListaDir.GetUpperBound(0) < NumeroDirectorios Then
            ReDim Preserve ListaDir(NumeroDirectorios + ValorReDim)
        End If
        ListaDir(NumeroDirectorios) = DirectorioIN
        Try
            directoryPaths = My.Computer.FileSystem.GetDirectories(DirectorioIN, FileIO.SearchOption.SearchTopLevelOnly)
        Catch e As Exception
            'No se puede acceder a la estructura de directorios
            ListarArbolDirectorios = 99
            Exit Function
        End Try

        'Recorremos los directorios de primer nivel del directorio de entrada
        For Each directoryPath In directoryPaths
            Try
                NumeroDirectorios = NumeroDirectorios + 1
                If ListaDir.GetUpperBound(0) < NumeroDirectorios Then
                    ReDim Preserve ListaDir(NumeroDirectorios + ValorReDim)
                End If
                ListaDir(NumeroDirectorios) = directoryPath
                'Recorremos los directorios y subdirectorios de cada directorio de primer nivel del directorio de entrada
                directorySubPaths = My.Computer.FileSystem.GetDirectories(directoryPath, FileIO.SearchOption.SearchAllSubDirectories)
                For Each directorySubPath In directorySubPaths
                    'SacarListaArchivos(directorySubPath, Extensiones)
                    'Contenedor.Items.Add(directorySubPath)
                    NumeroDirectorios = NumeroDirectorios + 1
                    If ListaDir.GetUpperBound(0) < NumeroDirectorios Then
                        ReDim Preserve ListaDir(NumeroDirectorios + ValorReDim)
                    End If
                    ListaDir(NumeroDirectorios) = directorySubPath
                Next
            Catch ex As Exception
                ListarArbolDirectorios = 99
                Exit Function
            End Try
        Next
        ReDim Preserve ListaDir(NumeroDirectorios)

        ListarArbolDirectorios = 0

    End Function

    Function RenombrarDirectorio(ByVal RutaOrigen As String, ByVal RutaDestino As String) As String

        RenombrarDirectorio = "NAK"
        Try
            System.IO.Directory.Move(RutaOrigen, RutaDestino)
            RenombrarDirectorio = "OK"
        Catch ex As Exception
            RenombrarDirectorio = ex.Message.ToString
        End Try

    End Function


    Function ListarFicherosDIREnBase(ByVal DirectorioIN As String, _
                                Optional ByRef Extensiones() As String = Nothing) As Long

        Dim directoryPaths As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
        Dim directorySubPaths As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
        'Dim directoryPaths() As String
        Dim directoryPath As String
        Dim directorySubPath As String
        Dim NombreBaseFinal As String
        If BasePreparada = False Then
            'No se puede localizar la base de datos de trabajo
            ListarFicherosDIREnBase = 99
            Exit Function
        End If
        If ConectarBD_Access(My.Application.Info.DirectoryPath & "\dirdisk.dat") = False Then
            'No se puede conectar a la base de datos
            ListarFicherosDIREnBase = 98
            Exit Function
        End If
        'Creamos los objetos comando
        ComandoSQL = New OleDb.OleDbCommand
        ComandoSQL.Connection = DBConexAccess
        'Iniciamos las variables
        NumeroFicheros = 0
        NumeroDirectorios = 0
        DIRBase = ""
        'Sacamos los ficheros del directorio de entrada
        SacarListaArchivosEnBase(DirectorioIN, Extensiones)
        Try
            directoryPaths = My.Computer.FileSystem.GetDirectories(DirectorioIN, FileIO.SearchOption.SearchTopLevelOnly)
        Catch e As Exception
            ListarFicherosDIREnBase = 97
            Exit Function
        End Try

        'Recorremos los directorios de primer nivel del directorio de entrada
        For Each directoryPath In directoryPaths
            Try
                SacarListaArchivosEnBase(directoryPath, Extensiones)
                'Recorremos los directorios y subdirectorios de cada directorio de primer nivel del directorio de entrada
                directorySubPaths = My.Computer.FileSystem.GetDirectories(directoryPath, FileIO.SearchOption.SearchAllSubDirectories)
                NumeroDirectorios = NumeroDirectorios + 1
                For Each directorySubPath In directorySubPaths
                    NumeroDirectorios = NumeroDirectorios + 1
                    SacarListaArchivosEnBase(directorySubPath, Extensiones)
                Next
            Catch ex As Exception
                ListarFicherosDIREnBase = 96
                Exit Function
            End Try
        Next
        ComandoSQL.Dispose()
        'Desconectamos la base de datos
        DesconectarBD_Access()

        If My.Computer.FileSystem.FileExists(My.Application.Info.DirectoryPath & "\dirdisk.dat") = True Then
            NombreBaseFinal = My.Application.Info.DirectoryPath & "\Scandir_" & Replace(Replace(CStr(Now), "/", "-"), ":", "_") & ".mdb"
            Rename(My.Application.Info.DirectoryPath & "\dirdisk.dat", _
                    NombreBaseFinal)
            DIRBase = NombreBaseFinal
        End If
        ListarFicherosDIREnBase = 0

    End Function

    Function ListarFicheros(ByVal DirectorioIN As String, ByRef ListaFicheros() As String, _
                                    Optional ByRef Extensiones() As String = Nothing) As Long

        Dim directoryPaths As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
        Dim directorySubPaths As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
        'Dim directoryPaths() As String
        Dim directoryPath As String
        Dim directorySubPath As String

        'Iniciamos las variables
        NumeroFicheros = -1
        NumeroDirectorios = 0
        'Sacamos los ficheros del directorio de entrada
        SacarListaArchivos(DirectorioIN, ListaFicheros, Extensiones)
        Try
            directoryPaths = My.Computer.FileSystem.GetDirectories(DirectorioIN, FileIO.SearchOption.SearchTopLevelOnly)
        Catch e As Exception
            ListarFicheros = 97
            Exit Function
        End Try

        'Recorremos los directorios de primer nivel del directorio de entrada
        For Each directoryPath In directoryPaths
            Try
                SacarListaArchivos(directoryPath, ListaFicheros, Extensiones)
                'Recorremos los directorios y subdirectorios de cada directorio de primer nivel del directorio de entrada
                directorySubPaths = My.Computer.FileSystem.GetDirectories(directoryPath, FileIO.SearchOption.SearchAllSubDirectories)
                NumeroDirectorios = NumeroDirectorios + 1
                For Each directorySubPath In directorySubPaths
                    NumeroDirectorios = NumeroDirectorios + 1
                    SacarListaArchivos(directorySubPath, ListaFicheros, Extensiones)
                Next
            Catch ex As Exception
                ListarFicheros = 96
                Exit Function
            End Try
        Next
        ReDim Preserve ListaFicheros(NumeroFicheros)
        ListarFicheros = 0

    End Function

    Sub SacarListaArchivosEnBase(ByVal Directorio As String, Optional ByRef Extensiones() As String = Nothing)

        Dim NombreFicheros As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
        Dim NombreFichero As String
        Dim DatosFichero As System.IO.FileInfo
        Dim CargarEnBase As Boolean
        'Variables para guardar los parametros del JPG
        Dim Anchura As Integer
        Dim Altura As Integer
        Dim ResX As Integer
        Dim ResY As Integer
        NombreFicheros = My.Computer.FileSystem.GetFiles(Directorio)
        For Each NombreFichero In NombreFicheros
            DatosFichero = My.Computer.FileSystem.GetFileInfo(NombreFichero)
            NumeroFicheros = NumeroFicheros + 1
            '-------------------------------------------------------------------------
            CargarEnBase = False
            If IsNothing(Extensiones) Then
                CargarEnBase = True
            Else
                CargarEnBase = IIf(Array.IndexOf(Extensiones, DatosFichero.Extension.ToLower) >= 0, True, False)
            End If
            If CargarEnBase = True Then
                If ProcesarJPGcabeceras = False Then
                    ComandoSQL.CommandText = "INSERT INTO rutasfiles " & _
                            "(Ruta,Nom_Fich,Nombre_Directorio,Numbytes,fecha_creacion,fecha_ultimaescritura,fecha_ultimoacceso,extension) VALUES " & _
                            "(""" & DatosFichero.FullName & """," & _
                            """" & DatosFichero.Name & """," & _
                            """" & DatosFichero.DirectoryName & """," & _
                            "" & DatosFichero.Length & "," & _
                            "'" & DatosFichero.CreationTime & "'," & _
                            "'" & DatosFichero.LastWriteTime & "'," & _
                            "'" & DatosFichero.LastAccessTime & "'," & _
                            """" & DatosFichero.Extension & """)"
                Else
                    Anchura = 0
                    Altura = 0
                    ResX = 0
                    ResY = 0
                    If DatosFichero.Extension.ToLower = ".jpg" Then _
                                    AnalizarJPGHeader(DatosFichero.FullName, Anchura, Altura, ResX, ResY)
                    ComandoSQL.CommandText = "INSERT INTO rutasfiles " & _
                            "(Ruta,Nom_Fich,Nombre_Directorio,Numbytes,fecha_creacion,fecha_ultimaescritura,fecha_ultimoacceso,extension," & _
                            "pixelwidth,pixelheight,reswidth,resheight) VALUES " & _
                            "(""" & DatosFichero.FullName & """," & _
                            """" & DatosFichero.Name & """," & _
                            """" & DatosFichero.DirectoryName & """," & _
                            "" & DatosFichero.Length & "," & _
                            "'" & DatosFichero.CreationTime & "'," & _
                            "'" & DatosFichero.LastWriteTime & "'," & _
                            "'" & DatosFichero.LastAccessTime & "'," & _
                            """" & DatosFichero.Extension & """," & _
                            CLng(Anchura) & "," & _
                            CLng(Altura) & "," & _
                            CLng(ResX) & "," & _
                            CLng(ResY) & "" & _
                            ")"
                    '------------------------------------------------------------------------------
                End If
                ComandoSQL.ExecuteNonQuery()
            End If
        Next

    End Sub

    Sub SacarListaArchivos(ByVal Directorio As String, ByRef ListaFicheros() As String, _
                                                        Optional ByRef Extensiones() As String = Nothing)

        Dim NombreFicheros As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
        Dim NombreFichero As String
        Dim DatosFichero As System.IO.FileInfo
        Dim CargarEnBase As Boolean
        NombreFicheros = My.Computer.FileSystem.GetFiles(Directorio)
        For Each NombreFichero In NombreFicheros
            DatosFichero = My.Computer.FileSystem.GetFileInfo(NombreFichero)

            '-------------------------------------------------------------------------
            CargarEnBase = False
            If IsNothing(Extensiones) Then
                CargarEnBase = True
            Else
                CargarEnBase = IIf(Array.IndexOf(Extensiones, DatosFichero.Extension.ToLower) >= 0, True, False)
            End If
            If CargarEnBase = True Then
                NumeroFicheros = NumeroFicheros + 1
                If ListaFicheros.GetUpperBound(0) < NumeroFicheros Then
                    ReDim Preserve ListaFicheros(NumeroFicheros + ValorReDim)
                End If
                ListaFicheros(NumeroFicheros) = DatosFichero.FullName
            End If
        Next

    End Sub

    Function ConectarBD_Access(ByVal RutaAccess As String) As Boolean

        Dim CadenaConexion As String
        CadenaConexion = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" & RutaAccess
        Try
            DBConexAccess = New OleDb.OleDbConnection
            DBConexAccess.ConnectionString = CadenaConexion
            DBConexAccess.Open()
            ConectarBD_Access = True
        Catch Fallo As Exception
            ConectarBD_Access = False
        End Try

    End Function
    Function DesconectarBD_Access() As Boolean

        If DBConexAccess.State = ConnectionState.Closed Then Exit Function
        Try
            DBConexAccess.Close()
            DBConexAccess.Dispose()
            DesconectarBD_Access = True
        Catch Fallo As Exception
            DesconectarBD_Access = False
        End Try
    End Function

    Public Sub New()

        BasePreparada = False
        If My.Computer.FileSystem.FileExists(My.Application.Info.DirectoryPath & "\semilla.dat") = True Then
            Try
                FileCopy(My.Application.Info.DirectoryPath & "\semilla.dat", My.Application.Info.DirectoryPath & "\dirdisk.dat")
                BasePreparada = True
            Catch E As Exception
                BasePreparada = False
            End Try
        End If
        ProcesarJPGcabeceras = False
        ValorReDim = 50

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="RutaFichero">Ruta de entrada del fichero JPG</param>
    ''' <param name="ImageWidth">Variable para almacenar anchura en pixel.(Número de columnas)</param>
    ''' <param name="ImageHeight">Variable para almacenar altura en pixel.(Numero de Lineas)</param>
    ''' <param name="XDensidad">Densidad Eje X</param>
    ''' <param name="YDensidad">Densidad Eje Y</param>
    ''' <returns>
    ''' 0:Proceso OK
    ''' 99: El fichero no existe
    ''' 98: El fichero no es un JPG
    ''' </returns>
    ''' <remarks></remarks>
    Function AnalizarJPGHeader(ByVal RutaFichero As String, _
                                    ByRef ImageWidth As Integer, _
                                    ByRef ImageHeight As Integer, _
                                    ByRef XDensidad As Long, _
                                    ByRef YDensidad As Long) As Long

        Dim FileCad As IO.FileStream
        Dim FileBin As IO.BinaryReader
        Dim Letra As Byte
        Dim okProc As Boolean
        Dim CabJFIF As String
        Dim CabMarcador As String
        Dim CabLongitud As Integer
        Dim iSubBucle As Long
        Dim Unidades As Byte
        Dim VersionJFIF As Integer
        Dim PosicionINICab As Long
        'Comprobamos existencia del fichero
        If System.IO.File.Exists(RutaFichero) = False Then
            AnalizarJPGHeader = 99  'El fichero no existe
            Exit Function
        End If
        'Abrimos el fichero
        Try
            FileCad = New IO.FileStream(RutaFichero, _
                                                IO.FileMode.Open, _
                                                IO.FileAccess.Read, _
                                                IO.FileShare.Read)
            FileBin = New IO.BinaryReader(FileCad)
        Catch Manage_err As Exception
            AnalizarJPGHeader = 98  'Error al acceder el fichero
            Exit Function
        End Try
        'Inicializamos las variables
        CabMarcador = ""
        CabJFIF = ""
        ImageWidth = 0
        ImageHeight = 0
        'SOI del JPG (Start Of Image)
        If Hex(FileBin.ReadByte) = "FF" Then okProc = True
        If Hex(FileBin.ReadByte) = "D8" Then okProc = True
        If okProc = False Then
            AnalizarJPGHeader = 97  'El fichero no es un JPG
        End If
        Do
            'Localizada una cabecera y calculo de su longitud en bytes
            If Hex(FileBin.ReadByte) = "FF" Then
                CabMarcador = Hex(FileBin.ReadByte)
                PosicionINICab = FileBin.BaseStream.Position - 1
            End If
            CabLongitud = 256 * FileBin.ReadByte
            CabLongitud = CabLongitud + FileBin.ReadByte
            'Procesamos en función de la cabecera
            Select Case CabMarcador
                Case Is = "E0"  'Marcador Application Marker APP0
                    For iSubBucle = 1 To 4
                        CabJFIF = CabJFIF & FileBin.ReadChar
                    Next iSubBucle
                    If CabJFIF = "JFIF" Then     'Identificador JFIF
                        Letra = FileBin.ReadByte
                        VersionJFIF = 256 * FileBin.ReadByte
                        VersionJFIF = VersionJFIF + FileBin.ReadByte
                        '257:Version 1.1
                        '258:Version 1.2
                        Unidades = FileBin.ReadByte
                        XDensidad = 256 * FileBin.ReadByte
                        XDensidad = XDensidad + FileBin.ReadByte
                        YDensidad = 256 * FileBin.ReadByte
                        YDensidad = YDensidad + FileBin.ReadByte
                        Application.DoEvents()
                    Else
                        AnalizarJPGHeader = 98
                        Exit Do
                    End If
                Case Is = "EC"  'Marcador APP12
                    'Application.DoEvents()
                Case Is = "EE"  'Marcador APP14
                    'Application.DoEvents()
                Case Is = "DB"  'Marcador DQT (Define a Quantization Table)
                    'Application.DoEvents()
                Case Is = "C0"  'Marcador S0F0 (Baseline DCT)
                    'Application.DoEvents()
                    Letra = FileBin.ReadByte
                    ImageHeight = 256 * FileBin.ReadByte
                    ImageHeight = ImageHeight + FileBin.ReadByte
                    ImageWidth = 256 * FileBin.ReadByte
                    ImageWidth = ImageWidth + FileBin.ReadByte
                    'Application.DoEvents()
                    AnalizarJPGHeader = 0
                    Exit Do
                Case Is = "C4"  'Marcador DHT (Define Huffman Table)
                    'Application.DoEvents()
                Case Is = "E1"
                    'TXT = ""
                    'For iSubBucle = 4 To CabLongitud
                    ' TXT = TXT & FileBin.ReadChar
                    'Next
                    'Application.DoEvents()
                Case Is = "DA"  'Marcador SOS (Start Of Scan)
                    'Application.DoEvents()
                Case Else
                    'Application.DoEvents()
            End Select
            'Application.DoEvents()
            CabMarcador = ""
            FileBin.BaseStream.Position = PosicionINICab + CabLongitud + 1
        Loop
        FileBin.Close()
        FileBin = Nothing
        FileCad.Close()
        FileCad.Dispose()
    End Function

    Public Function SacarFileDeRuta(ByVal PathCompleto As String) As String

        Dim i_path As Integer

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

End Class