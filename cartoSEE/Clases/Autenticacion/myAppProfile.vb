Public Class myAppProfile

    Property userApp As String
    Property usuarioISTARI As Boolean
    Property usuarioSystem As String
    Property nombrePermiso As String
    Property accesoMyApp As Boolean
    Property editarDocumentacion As Boolean = False
    Property asignarPermisosUsuarios As Boolean = False
    Property generarVersionCdD As Boolean = False
    Property asignarParamsWMS As Boolean = False
    Property disponible1 As Boolean = False
    Property disponible2 As Boolean = False
    Property disponible3 As Boolean = False
    Property disponible4 As Boolean = False
    Property disponible5 As Boolean = False
    Property disponible6 As Boolean = False

    Dim _CambiarPassw As Boolean


#Region "Definición de propiedades"

    Public ReadOnly Property getNombrePermiso() As String
        Get
            Return nombrePermiso
        End Get
    End Property

    Public ReadOnly Property hayAccesoMyApp() As String
        Get
            Return accesoMyApp
        End Get
    End Property

    Public ReadOnly Property getUserSystem() As String
        Get
            Return usuarioSystem
        End Get
    End Property
    Public ReadOnly Property getUserApp() As String
        Get
            Return userApp
        End Get
    End Property
    Public ReadOnly Property tienePermiso1() As Boolean
        Get
            Return asignarPermisosUsuarios
        End Get
    End Property
    Public ReadOnly Property tienePermiso2() As Boolean
        Get
            Return generarVersionCdD
        End Get
    End Property
    Public ReadOnly Property tienePermiso3() As Boolean
        Get
            Return asignarParamsWMS
        End Get
    End Property
    Public ReadOnly Property tienePermiso4() As Boolean
        Get
            Return disponible1
        End Get
    End Property
    Public ReadOnly Property permisoCargarDUMPfromABSYS() As Boolean
        Get
            Return disponible2
        End Get
    End Property
    Public ReadOnly Property permisoCargaMTN() As Boolean
        Get
            Return disponible3
        End Get
    End Property
    Public ReadOnly Property tienePermiso7() As Boolean
        Get
            Return disponible4
        End Get
    End Property
    Public ReadOnly Property tienePermiso8() As Boolean
        Get
            Return disponible5
        End Get
    End Property
    Public ReadOnly Property tienePermiso9() As Boolean
        Get
            Return disponible6
        End Get
    End Property

    Public ReadOnly Property cambiarPassw() As Boolean
        Get
            Return _CambiarPassw
        End Get
    End Property
    Public ReadOnly Property isUserISTARI() As Boolean
        Get
            Return usuarioISTARI
        End Get
    End Property
#End Region


    Function dameResumenProps() As ArrayList

        Dim resumenPermisos As New ArrayList

        resumenPermisos.Add(IIf(accesoMyApp = True, "SI: ", "NO: ") & "Acceso a la App")
        resumenPermisos.Add(IIf(editarDocumentacion = True, "SI: ", "NO: ") & "Edición de documentación")
        resumenPermisos.Add(IIf(asignarPermisosUsuarios = True, "SI: ", "NO: ") & "Asignar permisos de usuarios")
        resumenPermisos.Add(IIf(generarVersionCdD = True, "SI: ", "NO: ") & "Generar versión para el CdD")
        resumenPermisos.Add(IIf(asignarParamsWMS = True, "SI: ", "NO: ") & "Asignar parámetros de WMS")
        resumenPermisos.Add(IIf(disponible1 = True, "SI: ", "NO: ") & "No asignado")
        resumenPermisos.Add(IIf(disponible2 = True, "SI: ", "NO: ") & "No asignado")
        resumenPermisos.Add(IIf(disponible3 = True, "SI: ", "NO: ") & "No asignado")
        resumenPermisos.Add(IIf(disponible4 = True, "SI: ", "NO: ") & "No asignado")
        resumenPermisos.Add(IIf(disponible5 = True, "SI: ", "NO: ") & "No asignado")
        resumenPermisos.Add(IIf(disponible6 = True, "SI: ", "NO: ") & "No asignado")

        Return resumenPermisos

    End Function


    Public Sub New()

        permisoConsulta()

    End Sub

    Public Sub permisoEdicion()

        editarDocumentacion = True
        nombrePermiso = "Consulta y edición"

    End Sub

    Public Sub permisoConsulta()

        editarDocumentacion = False
        nombrePermiso = "Consulta"

    End Sub

    Public Sub permisoISTARI()

        accesoMyApp = True
        editarDocumentacion = True
        asignarPermisosUsuarios = True
        generarVersionCdD = True
        asignarParamsWMS = True
        disponible1 = True
        disponible2 = True
        disponible3 = True
        disponible4 = True
        disponible5 = True
        disponible6 = True
        usuarioISTARI = True

    End Sub


    Public Sub AsignarPermisosBD(ByVal accessLevel As Integer)

        '00.Acceder aplicación
        '01.Editar documentos
        '02.Asignar permisos a users
        '03.Generar versión CdD
        '04.Asignar parámetros del WMS
        '05.No asignado
        '06.No asignado
        '07.No asignado
        '08.No asignado
        '09.No asignado
        '10.No asignado


        Dim cadAccess As String
        cadAccess = develmap.develcode.BaseConversor.ToNumBase(accessLevel, 2)
        If cadAccess.Length < 11 Then
            'No hay acceso
            accesoMyApp = False
            editarDocumentacion = False
            asignarPermisosUsuarios = False
            generarVersionCdD = False
            asignarParamsWMS = False
            disponible1 = False
            disponible2 = False
            disponible3 = False
            disponible4 = False
            disponible5 = False
            disponible6 = False
        Else
            'Hay acceso, veamos con qué permisos
            accesoMyApp = True
            editarDocumentacion = IIf(cadAccess.Substring(1, 1) = "1", True, False)
            asignarPermisosUsuarios = IIf(cadAccess.Substring(2, 1) = "1", True, False)
            generarVersionCdD = IIf(cadAccess.Substring(3, 1) = "1", True, False)
            asignarParamsWMS = IIf(cadAccess.Substring(4, 1) = "1", True, False)
            disponible1 = IIf(cadAccess.Substring(5, 1) = "1", True, False)
            disponible2 = IIf(cadAccess.Substring(6, 1) = "1", True, False)
            disponible3 = IIf(cadAccess.Substring(7, 1) = "1", True, False)
            disponible4 = IIf(cadAccess.Substring(8, 1) = "1", True, False)
            disponible5 = IIf(cadAccess.Substring(9, 1) = "1", True, False)
            disponible6 = IIf(cadAccess.Substring(10, 1) = "1", True, False)
        End If

    End Sub

    ''' <summary>
    ''' Funcion Para Autenticar el usuario con la base de datos
    ''' </summary>
    ''' <param name="usuarioKeyboard">Nombre del usuario</param>
    ''' <param name="passwKeyboard">Password del usuario</param>
    ''' <returns>Devuelve un booleano permitiendo o denegando el acceso</returns>
    ''' <remarks></remarks>
    Function AutenticarUsuario(ByVal usuarioKeyboard As String, ByVal passwKeyboard As String) As Integer

        'AutenticarUsuario=0 ---> No hay permiso de acceso a la aplicación
        'AutenticarUsuario=1 ---> usuario de consulta
        'AutenticarUsuario=2 ---> usuario de administracion
        AutenticarUsuario = 0

        'Si el usuario es superadministrador, paso sin validar ni siquiera el usuario de sistema
        If usuarioKeyboard.ToUpper = "E2MOLIN" And passwKeyboard.ToUpper = "ALMEO13" Then
            usuarioSystem = "ISTARI"
            permisoISTARI()
            Return 1
        End If

        'Obtengo el usuario del Sistema
        Dim userSystem() As String
        If My.User.Name = "" Then Exit Function
        userSystem = My.User.Name.Split("\")
        usuarioSystem = userSystem(userSystem.Length - 1)
        usuarioMyApp.machineName = Environment.MachineName
        'Si se entra como sólo lectura, se valida sólo que el usuario de la máquina esté autorizado en la base de datos
        'Si se entra como administrador para editar, se valida usuario de máquina y password
        'Si se entra como usuario desde otra máquina, se valida el usuario escrito
        Dim nivelAcceso As Integer = 0

        If usuarioKeyboard = "" And passwKeyboard = "" Then
            ObtenerEscalar("SELECT permisocartosee FROM bdsidschema.usuarios WHERE loginuser='" & usuarioSystem.Trim.ToLower & "'", nivelAcceso)
            usuarioMyApp.loginUser = usuarioSystem.Trim.ToLower
        ElseIf usuarioKeyboard <> "" And passwKeyboard <> "" Then
            ObtenerEscalar("SELECT permisocartosee FROM bdsidschema.usuarios WHERE loginuser='" & usuarioKeyboard & "' " &
                                "AND loginpassw=md5('" & passwKeyboard.Trim.ToLower & "'||'" & usuarioKeyboard.ToLower & "'||'dvmap')", nivelAcceso)
            usuarioMyApp.loginUser = usuarioKeyboard
        End If

        'Con esto 
        If nivelAcceso = 0 Then
            registrarDatabaseLog("Intento fallido de conexión")
            Return 0
        End If

        AsignarPermisosBD(nivelAcceso)
        If passwKeyboard = "" Then
            _CambiarPassw = False
        Else
            _CambiarPassw = True
        End If
        registrarDatabaseLog("Conexión válida")
        Return 2



        If passwKeyboard = "" Then
            ObtenerEscalar("SELECT permisocartosee FROM bdsidschema.usuarios WHERE loginuser='" & usuarioSystem.Trim.ToLower & "'", nivelAcceso)
            Application.DoEvents()
            usuarioMyApp.loginUser = usuarioSystem.Trim.ToLower

            'Con las siguientes líneas no validamos si el usuario está registrado
            AsignarPermisosBD(nivelAcceso)
            'permisoConsulta()
            _CambiarPassw = False
            registrarDatabaseLog("Conexión válida")
            Return 2
        Else
            If usuarioKeyboard.ToLower = "administrador" Then
                'Accede como administrador desde su ordenador
                ObtenerEscalar("SELECT permisocartosee FROM bdsidschema.usuarios WHERE loginuser='" & usuarioSystem.Trim.ToLower & "' " &
                                "AND loginpassw=md5('" & passwKeyboard.Trim.ToLower & "'||'" & usuarioSystem.Trim.ToLower & "'||'dvmap')", nivelAcceso)
                usuarioMyApp.loginUser = usuarioSystem.Trim.ToLower
            Else
                'Accede como administrador desde otro ordenador.
                ObtenerEscalar("SELECT permisocartosee FROM bdsidschema.usuarios WHERE loginuser='" & usuarioKeyboard & "' " &
                                "AND loginpassw=md5('" & passwKeyboard.Trim.ToLower & "'||'" & usuarioKeyboard.ToLower & "'||'dvmap')", nivelAcceso)
                usuarioMyApp.loginUser = usuarioKeyboard
            End If
            Application.DoEvents()
        End If


    End Function






End Class
