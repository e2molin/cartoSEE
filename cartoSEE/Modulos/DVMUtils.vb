Module DVMUtils

    '--------------------------------------------------------------------------------------------
    'Declaraciones de API
    '--------------------------------------------------------------------------------------------
    Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" _
                    (ByVal lpApplicationName As String, ByVal lpKeyName As String,
                    ByVal lpDefault As String, ByVal lpReturnedString As String,
                    ByVal nSize As Integer, ByVal lpFileName As String) As Integer
    Private Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" _
                    (ByVal lpApplicationName As String, ByVal lpKeyName As String,
                    ByVal lpString As String, ByVal lpFileName As String) As Integer

    Private Declare Sub InvalidateRect Lib "user32" (ByVal hwnd As Long, ByVal T As Long, ByVal bErase As Long)
    Private Declare Sub ValidateRect Lib "user32" (ByVal hwnd As Long, ByVal T As Long)

    Private Declare Auto Function SetProcessWorkingSetSize Lib "kernel32.dll" (ByVal procHandle As IntPtr,
                                    ByVal min As Int32, ByVal max As Int32) As Boolean



    Public Sub ClearMemory()

        Try
            Dim Mem As Process
            Mem = Process.GetCurrentProcess
            SetProcessWorkingSetSize(Mem.Handle, -1, -1)

        Catch ex As Exception

        End Try

    End Sub



    '**************************************************************************
    ' Funciones de gestión de archivos INI
    '**************************************************************************
#Region "Gestión de ficheros INI"

    ''' <summary>
    ''' Lectura de un archivo INI en el directorio del ejecutable
    ''' </summary>
    ''' <param name="Seccion">Seccion del archivo INI</param>
    ''' <param name="Item">Atributo de la seccion del archivo INI</param>
    ''' <returns>Devuelve una cadena con el valor del atributo dentro de la seccion</returns>
    ''' <remarks></remarks>
    Public Function LeeIni(ByVal Seccion As String, ByVal Item As String) As String
        Dim NumChar As Long
        Dim A As String
        Dim Ini
        A = Space(512)
        If Right(My.Application.Info.DirectoryPath, 1) = "\" Then
            Ini = My.Application.Info.DirectoryPath & My.Application.Info.AssemblyName & ".ini"
        Else
            Ini = My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".ini"
        End If
        NumChar = GetPrivateProfileString(Seccion, Item, "", A, 512, Ini)
        If NumChar = 0 Then
            LeeIni = ""
            Exit Function
        End If

        A = Left(A, NumChar)
        LeeIni = A
    End Function

    ''' <summary>
    ''' Escritura en un archivo INI en el directorio del ejecutable
    ''' </summary>
    ''' <param name="Seccion">Seccion del archivo INI</param>
    ''' <param name="Item">Atributo de la seccion del archivo INI</param>
    ''' <param name="Texto">Valor a escribir</param>
    ''' <remarks></remarks>
    Public Sub EscribeIni(ByVal Seccion As String, ByVal Item As String, ByVal Texto As String)
        Dim NumChar As Long
        Dim A
        Dim Ini
        A = Space(512)
        If Right(My.Application.Info.DirectoryPath, 1) = "\" Then
            Ini = My.Application.Info.DirectoryPath & My.Application.Info.AssemblyName & ".ini"
        Else
            Ini = My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".ini"
        End If
        NumChar = WritePrivateProfileString(Seccion, Item, Texto, Ini)
    End Sub

    ''' <summary>
    ''' Lectura de un archivo INI
    ''' </summary>
    ''' <param name="Seccion">Seccion del archivo INI</param>
    ''' <param name="Item">Atributo de la seccion del archivo INI</param>
    ''' <returns>Devuelve una cadena con el valor del atributo dentro de la seccion</returns>
    ''' <remarks></remarks>
    Public Function LeeIniLocal(ByVal Seccion As String, ByVal Item As String) As String
        Dim NumChar As Long
        Dim A As String
        Dim Ini
        A = Space(255)
        If Right(AppFolderSetting, 1) = "\" Then
            Ini = AppFolderSetting & My.Application.Info.AssemblyName & ".ini"
        Else
            Ini = AppFolderSetting & "\" & My.Application.Info.AssemblyName & ".ini"
        End If
        NumChar = GetPrivateProfileString(Seccion, Item, "", A, 255, Ini)
        If NumChar = 0 Then
            LeeIniLocal = ""
            Exit Function
        End If

        A = Left(A, NumChar)
        LeeIniLocal = A
    End Function

    ''' <summary>
    ''' Escritura en un archivo INI
    ''' </summary>
    ''' <param name="Seccion">Seccion del archivo INI</param>
    ''' <param name="Item">Atributo de la seccion del archivo INI</param>
    ''' <param name="Texto">Valor a escribir</param>
    ''' <remarks></remarks>
    Public Sub EscribeIniLocal(ByVal Seccion As String, ByVal Item As String, ByVal Texto As String)
        Dim NumChar As Long
        Dim A
        Dim Ini
        A = Space(255)
        If Right(AppFolderSetting, 1) = "\" Then
            Ini = AppFolderSetting & My.Application.Info.AssemblyName & ".ini"
        Else
            Ini = AppFolderSetting & "\" & My.Application.Info.AssemblyName & ".ini"
        End If
        NumChar = WritePrivateProfileString(Seccion, Item, Texto, Ini)
    End Sub

#End Region

    '**************************************************************************
    ' Helpers para el manejo de ventanas modales de info, error y preguntas
    '**************************************************************************
#Region "Ventanas modales"
    ''' <summary>
    ''' Handle para preguntas modales
    ''' </summary>
    ''' <param name="question">Pregunta para incluir</param>
    ''' <param name="titleModal">Título caja modal</param>
    ''' <returns></returns>
    Public Function ModalQuestion(question As String, Optional titleModal As String = "") As DialogResult

        If titleModal = "" Then titleModal = AplicacionTitulo
        Return MessageBox.Show(question,
                        titleModal,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question)


    End Function

    ''' <summary>
    ''' Handle para ventanas de información
    ''' </summary>
    ''' <param name="message">Pregunta para incluir</param>
    ''' <param name="titleModal">Título caja modal</param>
    ''' <returns></returns>
    Public Function ModalInfo(message As String, Optional titleModal As String = "") As DialogResult

        If titleModal = "" Then titleModal = AplicacionTitulo
        Return MessageBox.Show(message,
                        titleModal,
                        MessageBoxButtons.OK, MessageBoxIcon.Information)


    End Function

    ''' <summary>
    ''' Handle para ventanas de advertencia
    ''' </summary>
    ''' <param name="message">Pregunta para incluir</param>
    ''' <param name="titleModal">Título caja modal</param>
    ''' <returns></returns>
    Public Function ModalExclamation(message As String, Optional titleModal As String = "") As DialogResult

        If titleModal = "" Then titleModal = AplicacionTitulo
        Return MessageBox.Show(message,
                        titleModal,
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation)


    End Function

    ''' <summary>
    ''' Handle para info de error
    ''' </summary>
    ''' <param name="message">Mensaje de error</param>
    ''' <param name="titleModal">Título caja modal</param>
    ''' <returns></returns>
    Public Function ModalError(message As String, Optional titleModal As String = "") As DialogResult

        If titleModal = "" Then titleModal = AplicacionTitulo
        Return MessageBox.Show(message,
                        titleModal,
                        MessageBoxButtons.OK, MessageBoxIcon.Error)


    End Function

#End Region

    Function GenerarLOG(ByVal Frase As String) As Boolean

        Dim sw As New System.IO.StreamWriter(ficheroLogger, True)
        Dim cadFechaInsert As String = Now.Year & "-" &
                                        String.Format("{0:00}", CInt(Now.Month.ToString)) & "-" &
                                        String.Format("{0:00}", CInt(Now.Day.ToString)) & " " &
                                        String.Format("{0:00}", CInt(Now.Hour.ToString)) & ":" &
                                        String.Format("{0:00}", CInt(Now.Minute.ToString))

        sw.WriteLine(cadFechaInsert & " # " & Frase)
        sw.Close()
        sw.Dispose()
        sw = Nothing

    End Function

    Function GenerarLOG(ByVal Frase As String, tacada As ArrayList) As Boolean

        Dim sw As New System.IO.StreamWriter(ficheroLogger, True)
        Dim cadFechaInsert As String = Now.Year & "-" &
                                        String.Format("{0:00}", CInt(Now.Month.ToString)) & "-" &
                                        String.Format("{0:00}", CInt(Now.Day.ToString)) & " " &
                                        String.Format("{0:00}", CInt(Now.Hour.ToString)) & ":" &
                                        String.Format("{0:00}", CInt(Now.Minute.ToString))

        sw.WriteLine(cadFechaInsert & " # " & Frase)
        For Each item As String In tacada
            sw.WriteLine(" # " & item)
        Next
        sw.Close()
        sw.Dispose()

    End Function

    '--------------------------------------------------------------------------
    ' Funciones de formateado de Fecha
    '--------------------------------------------------------------------------

#Region "Funciones de formateado de Fecha"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="tipo">{SPAIN (default),GERMAN,ISO8601}</param>
    ''' <returns></returns>
    Function FormatearFecha(ByVal fecha As Date, Optional ByVal tipo As String = "") As String

        Dim result As String = ""
        If fecha = Nothing Then Return result

        If tipo = "SPAIN" Then
            result = String.Format("{0:00}", fecha.Day) & "-" & String.Format("{0:00}", fecha.Month) & "-" & fecha.Year
        ElseIf tipo = "GERMAN" Or tipo = "ISO8601" Then
            result = fecha.Year & "-" & String.Format("{0:00}", fecha.Month) & "-" & String.Format("{0:00}", fecha.Day)
        Else
            'Por defecto devuelve el formato español
            result = String.Format("{0:00}", fecha.Day) & "-" & String.Format("{0:00}", fecha.Month) & "-" & fecha.Year
        End If

        Return result

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="tipo">{SPAIN (default), GERMAN, ISO8601}</param>
    ''' <returns></returns>
    Function FormatearFechaHora(ByVal fecha As Date, Optional ByVal tipo As String = "") As String

        Dim result As String = ""
        If fecha = Nothing Then Return result

        If tipo = "SPAIN" Then
            result = String.Format("{0:00}", fecha.Day) & "-" & String.Format("{0:00}", fecha.Month) & "-" & fecha.Year & " " &
                String.Format("{0:00}", fecha.Hour) & ":" & String.Format("{0:00}", fecha.Minute) & ":" & String.Format("{0:00}", fecha.Second)
        ElseIf tipo = "GERMAN" Or tipo = "ISO8601" Then
            result = fecha.Year & "-" & String.Format("{0:00}", fecha.Month) & "-" & String.Format("{0:00}", fecha.Day) & " " &
                String.Format("{0:00}", fecha.Hour) & ":" & String.Format("{0:00}", fecha.Minute) & ":" & String.Format("{0:00}", fecha.Second)
        Else
            'Por defecto devuelve el formato español
            result = String.Format("{0:00}", fecha.Day) & "-" & String.Format("{0:00}", fecha.Month) & "-" & fecha.Year & " " &
                String.Format("{0:00}", fecha.Hour) & ":" & String.Format("{0:00}", fecha.Minute) & ":" & String.Format("{0:00}", fecha.Second)
        End If

        Return result

    End Function

    Function RenameFecha_SPAIN_to_ISO8601(ByVal fecha As String) As String

        If fecha = Nothing Then Return ""
        If fecha = "" Then Return ""
        Dim dateSeparator As String = ""
        Try
            If fecha.Contains("/") Then dateSeparator = "/"
            If fecha.Contains("-") Then dateSeparator = "-"
            Dim fechaPart() As String = fecha.Split(dateSeparator)
            If fechaPart.Length <> 3 Then Return ""
            Return fechaPart(2) & "-" & String.Format("{0:00}", fechaPart(1)) & "-" & String.Format("{0:00}", fechaPart(0))
        Catch ex As Exception
            ModalError(ex.Message)
            Return ""
        End Try


    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="tipo">{SPAIN (default),GERMAN, ISO8601}</param>
    ''' <returns></returns>
    Function formatFechaActual(Optional ByVal tipo As String = "") As String

        Dim result As String = ""

        If tipo = "SPAIN" Then
            result = String.Format("{0:00}", Now.Day) & "-" & String.Format("{0:00}", Now.Month) & "-" & Now.Year &
                        " " & String.Format("{0:00}", Now.Hour) & ":" & String.Format("{0:00}", Now.Minute) &
                        ":" & String.Format("{0:00}", Now.Second)
        ElseIf tipo = "GERMAN" Or tipo = "ISO8601" Then
            result = Now.Year & "-" & String.Format("{0:00}", Now.Month) & "-" & String.Format("{0:00}", Now.Day) &
                        " " & String.Format("{0:00}", Now.Hour) & ":" & String.Format("{0:00}", Now.Minute) &
                        ":" & String.Format("{0:00}", Now.Second)

        Else
            result = String.Format("{0:00}", Now.Day) & "-" & String.Format("{0:00}", Now.Month) & "-" & Now.Year &
                                " " & String.Format("{0:00}", Now.Hour) & ":" & String.Format("{0:00}", Now.Minute) &
                                ":" & String.Format("{0:00}", Now.Second)
        End If

        Return result

    End Function

#End Region


    '--------------------------------------------------------------------------------------------------------
    'Utils nomenManager
    '--------------------------------------------------------------------------------------------------------

    Function getNGBECodeFromINE(codigoINE As String) As String

        'PPMMMCCSSNN
        'PP - Provincia
        'MMM - Municipio
        'CC - Entidad Colectiva
        'SS - EntidadSingular
        'NN - Núcleo de población (Si es 99 es diseminado) 

        If codigoINE.Length <> 11 Then Return ""

        If codigoINE.Substring(9, 2) <> "00" Then Return "2.1.10"   'Núcleo
        If codigoINE.Substring(7, 2) <> "00" Then Return "2.1.9"    'Singular
        If codigoINE.Substring(5, 2) <> "00" Then Return "2.1.5"    'Colectiva



    End Function


End Module
