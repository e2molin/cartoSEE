Public Class myAppProfile

    Property userApp As String
    Property usuarioISTARI As Boolean
    Property usuarioSystem As String
    Property nombrePermiso As String


    Property numPermisosApp As Integer = 5
    Property AccesoMyApp As Boolean                             '0
    Property EditarDocumentacion As Boolean = False             '1
    Property AsignarPermisosUsuarios As Boolean = False         '2
    Property GenerarVersionCdD As Boolean = False               '3
    Property AsignarParamsWMS As Boolean = False                '4
    Property puedeCambiarSuPassword As Boolean = False


    Property ListaPermisosApp() As String() = {"Acceso a aplicación", "Edición de documentación", "Edición de usuarios", "Generar Versión CdD", "Asignar parámetros WMS"}

    Property FieldGrants As String = "permisocartosee"


    Dim _CambiarPassw As Boolean


#Region "Definición de propiedades"
    Public ReadOnly Property cambiarPassw() As Boolean
        Get
            Return puedeCambiarSuPassword
        End Get
    End Property

    Public ReadOnly Property isUserISTARI() As Boolean
        Get
            Return usuarioISTARI
        End Get
    End Property
    Public ReadOnly Property getNombrePermiso() As String
        Get
            Return nombrePermiso
        End Get
    End Property

    Public ReadOnly Property userSystem() As String
        Get
            Return usuarioSystem
        End Get
    End Property

#End Region


    Function DameResumenProps() As ArrayList

        Dim resumenPermisos As New ArrayList

        resumenPermisos.Add(IIf(AccesoMyApp = True, "SI: ", "NO: ") & ListaPermisosApp(0))
        resumenPermisos.Add(IIf(EditarDocumentacion = True, "SI: ", "NO: ") & ListaPermisosApp(1))
        resumenPermisos.Add(IIf(AsignarPermisosUsuarios = True, "SI: ", "NO: ") & ListaPermisosApp(2))
        resumenPermisos.Add(IIf(GenerarVersionCdD = True, "SI: ", "NO: ") & ListaPermisosApp(3))
        resumenPermisos.Add(IIf(AsignarParamsWMS = True, "SI: ", "NO: ") & ListaPermisosApp(4))
        Return resumenPermisos

    End Function


    Public Sub New()

        permisoConsulta()

    End Sub

    Public Sub permisoEdicion()

        editarDocumentacion = True
        nombrePermiso = "Consulta y edición"

    End Sub

    Public Sub PermisoConsulta()

        EditarDocumentacion = False
        AsignarPermisosUsuarios = False
        GenerarVersionCdD = False
        AsignarParamsWMS = False
        puedeCambiarSuPassword = False
        nombrePermiso = "Consulta"

    End Sub

    Public Sub PermisoISTARI()

        AccesoMyApp = True
        EditarDocumentacion = True
        AsignarPermisosUsuarios = True
        GenerarVersionCdD = True
        AsignarParamsWMS = True
        puedeCambiarSuPassword = True
        usuarioISTARI = True

    End Sub


    Public Sub AsignarPermisosBD(ByVal accessLevel As Integer)

        '00.Acceder aplicación
        '01.Edición de documentación
        '02.Edición de usuarios
        '03.Generar Versión CdD
        '04.Asignar parámetros WMS


        Dim cadAccess As String = ""
        For Each permiso As String In ListaPermisosApp
            cadAccess &= "0"
        Next
        If develmap.develcode.BaseConversor.ToNumBase(accessLevel, 2) <> "" Then
            cadAccess = develmap.develcode.BaseConversor.ToNumBase(accessLevel, 2)
        End If
        If cadAccess.Length < numPermisosApp Then
            For iBit As Integer = cadAccess.Length + 1 To numPermisosApp
                cadAccess &= "0"
            Next iBit
            Application.DoEvents()
        End If
        AccesoMyApp = False
        EditarDocumentacion = False
        AsignarPermisosUsuarios = False
        GenerarVersionCdD = False
        AsignarParamsWMS = False


        Try
            AccesoMyApp = IIf(cadAccess.Substring(0, 1) = "1", True, False)
            EditarDocumentacion = IIf(cadAccess.Substring(1, 1) = "1", True, False)
            AsignarPermisosUsuarios = IIf(cadAccess.Substring(2, 1) = "1", True, False)
            GenerarVersionCdD = IIf(cadAccess.Substring(3, 1) = "1", True, False)
            AsignarParamsWMS = IIf(cadAccess.Substring(4, 1) = "1", True, False)
        Catch ex As Exception
            ModalError("Problemas al autenticar: " & ex.Message)
        End Try


    End Sub

    ''' <summary>
    ''' Funcion Para Autenticar el usuario con la base de datos
    ''' </summary>
    ''' <param name="usuarioKeyboard">Nombre del usuario</param>
    ''' <param name="passwKeyboard">Password del usuario</param>
    ''' <returns>Devuelve un booleano permitiendo o denegando el acceso</returns>
    ''' <remarks></remarks>
    Function AutenticarUsuario(ByVal usuarioKeyboard As String, ByVal passwKeyboard As String) As Integer

        'AutenticarUsuario=0 ---> Hay permiso de acceso a la aplicación
        'AutenticarUsuario=1 ---> No hay permiso de acceso a la aplicación
        AutenticarUsuario = 1

        'Si el usuario es superadministrador, paso sin validar ni siquiera el usuario de sistema
        If usuarioKeyboard.ToUpper = "E2MOLIN" And passwKeyboard.ToUpper = "ALMEO13" Then
            usuarioSystem = "ISTARI"
            usuarioMyApp.loginUser = "DEVELMAP"
            PermisoISTARI()
            AutenticarUsuario = 0
            Return AutenticarUsuario
        End If

        'Si no soy istari, primero obtengo el usuario del sistema
        Dim userSystem() As String
        If My.User.Name = "" Then
            registrarDatabaseLog("Usuario del sistema no identificado")
            Return 1
        End If
        userSystem = My.User.Name.Split("\")
        usuarioSystem = userSystem(userSystem.Length - 1)
        usuarioMyApp.machineName = Environment.MachineName

        'Si se entra como sólo lectura, se valida sólo que el usuario de la máquina esté autorizado en la base de datos
        'Si se entra como administrador para editar, se valida usuario de máquina y password
        'Si se entra como usuario desde otra máquina, se valida el usuario escrito
        Dim nivelAcceso As Integer = 0

        If usuarioKeyboard = "" Then
            ' Si no meto usuario, utilizo el del sistema
            ObtenerEscalar($"SELECT {FieldGrants} FROM bdsidschema.usuarios WHERE enable=1 And loginuser='{usuarioSystem.Trim.ToLower}'", nivelAcceso)
            usuarioMyApp.loginUser = usuarioSystem.Trim.ToLower
        Else
            If usuarioKeyboard <> "" And passwKeyboard <> "" Then
                ObtenerEscalar($"SELECT {FieldGrants} FROM bdsidschema.usuarios WHERE enable=1 And loginuser='{usuarioKeyboard}' AND loginpassw=md5('{passwKeyboard.Trim}'||'{usuarioKeyboard.ToLower}'||'dvmap')", nivelAcceso)
                usuarioMyApp.loginUser = usuarioKeyboard
            Else
                registrarDatabaseLog("Intento fallido de conexión")
                Return 1
            End If
        End If
        If nivelAcceso = 0 Then
            registrarDatabaseLog("Intento fallido de conexión")
            Return 1
        End If

        AsignarPermisosBD(nivelAcceso)
        puedeCambiarSuPassword = True
        ' Si no mete password, rebajo sus permisos a los de sólo consulta
        If passwKeyboard = "" Then PermisoConsulta()

        registrarDatabaseLog("Conexión válida")
        AutenticarUsuario = 0

    End Function






End Class
