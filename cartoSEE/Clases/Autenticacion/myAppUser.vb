Public Class myAppUser

    Property Id As Integer
    Property Nombre As String
    Property Apellidos As String
    Property Telefono As String
    Property LoginUser As String
    Property LoginPass As String
    Property MachineName As String
    Property MachineSO As String
    Property MachineIP As String
    Property correo_electronico As String
    Property UserEnabled As Boolean
    Property Permisos As New myAppProfile

    Dim _CodPermisoMultiple As Integer

#Region "Definición de propiedades"

    Public Property codPermisoMultiple() As Integer
        Get
            Return _CodPermisoMultiple
        End Get
        Set(ByVal value As Integer)
            _CodPermisoMultiple = value
            _Permisos.AsignarPermisosBD(_CodPermisoMultiple)
        End Set
    End Property

    Public ReadOnly Property permisosLista() As myAppProfile
        Get
            Return _Permisos
        End Get

    End Property
#End Region

    Sub cambiarPassword(ByVal oldPassword As String, ByVal nuevaPassword As String)

        Dim cadUpdate As String
        Dim idUser As Integer = 0

        If oldPassword.IndexOf("'") > -1 Then
            ModalExclamation($"No se permite la utilización de apóstrofes.")
            Exit Sub
        End If

        If nuevaPassword.IndexOf("'") > -1 Then
            ModalExclamation($"No se permite la utilización de apóstrofes.")
            Exit Sub
        End If

        If oldPassword = "" Or nuevaPassword = "" Then
            ModalExclamation($"Credenciales erróneas para el usuario {_LoginUser}. La contraseña no se ha cambiado.")
            Exit Sub
        End If

        ObtenerEscalar($"SELECT iduser from bdsidschema.usuarios WHERE loginuser='{_LoginUser}' and loginpassw=md5('{oldPassword}{_LoginUser}dvmap')", idUser)

        If idUser = 0 Then
            ModalExclamation($"Credenciales erróneas para el usuario {_LoginUser}. La contraseña no se ha cambiado.")
            Exit Sub
        End If

        cadUpdate = $"UPDATE bdsidschema.usuarios SET loginpassw=md5('{nuevaPassword}{_LoginUser}dvmap') WHERE iduser={idUser}"

        If ExeSinTran(cadUpdate) = True Then
            ModalInfo("Contraseña modificada con éxito")
        Else
            ModalExclamation($"Credenciales erróneas para el usuario {_LoginUser}. La contraseña no se ha cambiado.")
        End If
    End Sub






End Class
