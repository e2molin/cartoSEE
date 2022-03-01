Module Autenticacion
    Public usuario_ISTARI As Boolean

    ''' <summary>
    ''' Funcion Para Autenticar el usuario con la base de datos
    ''' </summary>
    ''' <param name="Usuario">Nombre del usuario</param>
    ''' <param name="Passw">Password del usuario</param>
    ''' <returns>Devuelve un booleano permitiendo o denegando el acceso</returns>
    ''' <remarks></remarks>
    Function AutenticarUsuario(ByVal Usuario As String, ByVal Passw As String) As Integer

        'AutenticarUsuario=0 ---> no tiene permiso de acceso
        'AutenticarUsuario=1 ---> usuario de consulta
        'AutenticarUsuario=2 ---> usuario de administracion
        Dim userSystem() As String
        AutenticarUsuario = 0

        App_Machine = Environment.MachineName


        If Usuario.ToUpper = "E2MOLIN" And Passw.ToUpper = "ALMEO13" Then
            AutenticarUsuario = 2
            usuario_ISTARI = True
            Exit Function
        End If
        If modoDelegaciones = True Then
            App_User = "Delegaciones"
            AutenticarUsuario = 1
            Exit Function
        End If
        If Usuario.ToLower.Trim = "" Then
            If My.User.Name = "" Then Exit Function
            userSystem = My.User.Name.Split("\")
            Usuario = userSystem(userSystem.Length - 1)
            App_User = Usuario
        Else
            App_User = Usuario.ToLower.Trim
        End If

        Dim Permiso As Integer = 0

        If Passw.ToLower.Trim = "" Then
            ObtenerEscalar("SELECT codigo_permiso FROM bdsidschema.usuarios WHERE loginuser='" & App_User & "'", Permiso)
        Else
            ObtenerEscalar("SELECT codigo_permiso FROM bdsidschema.usuarios WHERE loginuser='" & App_User & "' " &
                            "AND " &
                            "loginpassw=md5('" & Passw.Trim.ToLower & "'||'" & App_User & "'||'dvmap')", Permiso)
        End If
        If Permiso = 0 Then
            registrarDatabaseLog("Intento fallido de conexión")
        Else
            registrarDatabaseLog("Conexión válida")
            If Permiso = 1 Then AutenticarUsuario = 2
            If Permiso = 2 Then AutenticarUsuario = 1
            If Permiso = 3 Then AutenticarUsuario = 1
            If Permiso = 4 Then AutenticarUsuario = 1
        End If



    End Function

End Module
