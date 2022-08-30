Public Class myAppUser

    Property id As Integer
    Property nombre As String
    Property apellidos As String
    Property telefono As String
    Property loginUser As String
    Property loginPass As String
    Property machineName As String
    Property machineSO As String
    Property machineIP As String
    Property correoElectronico As String
    Property enabled As Boolean

    Dim _CodPermisoMultiple As Integer
    Dim _Permisos As New myAppProfile


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



    Sub cambiarPassword(ByVal nuevaPassword As String)

        Dim cadUpdate As String

        cadUpdate = "UPDATE bdsidschema.usuarios SET loginpassw=md5('" & nuevaPassword & "' || '" & _loginUser & "' || 'dvmap') WHERE loginuser='" & _loginUser & "'"

        Dim okProc As Boolean = ExeSinTran(cadUpdate)
        If okProc = True Then
            MessageBox.Show("Contraseña modificada", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("No se han realizado modificaciones", AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub





End Class
