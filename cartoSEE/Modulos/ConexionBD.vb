Imports System.Data
Imports System.Data.Common

Module ConexionBD
    Public ProData As DbProviderFactory
    Public MainConex As DbConnection
    Enum TiposBase
        SQLServer = 1
        Oracle = 2
        MySQL = 3
        Access2003 = 4
        Access2007 = 5
        PostgreSQL = 6
    End Enum

    ''' <summary>
    ''' Funcion para conectar mediante ADO.NET a bases de datos
    ''' </summary>
    ''' <param name="Tipo">Tipo de la base de datos</param>
    ''' <param name="IPServer">IP del Servidor de Bases de Datos</param>
    ''' <param name="Puerto">Puerto de Conexión a la base de datos</param>
    ''' <param name="Usuario">Usuario de acceso a la base de datos</param>
    ''' <param name="Passw">Password del Usuario</param>
    ''' <param name="Servicio">Servicio (Oracle), Catalogo (SQLServer) o Instancia (MySQL)</param>
    ''' <param name="RutaAccess">Ruta del fichero Access</param>
    ''' <returns>Devuelve un booleano indicando si hay o no conexion y crea un objeto conexion MainConex</returns>
    ''' <remarks></remarks>
    Function ConectarBD(ByVal Tipo As TiposBase, ByVal IPServer As String, ByVal Puerto As Long, ByVal Usuario As String, _
                       ByVal Passw As String, ByVal Servicio As String, ByVal RutaAccess As String) As Boolean

        Dim sProveedor As String
        Dim CadenaConexion As String

        ConectarBD = False
        CadenaConexion = ""
        sProveedor = ""

        If Tipo = 1 Then
            sProveedor = "System.Data.SqlClient"
            CadenaConexion = "server=" & IPServer & "," & Puerto & ";" & _
                             "uid=" & Usuario & ";pwd=" & Passw & ";Initial Catalog=" & Servicio
        ElseIf Tipo = 2 Then
            sProveedor = "System.Data.OracleClient"
            CadenaConexion = "User Id=" & Usuario & "; Password=" & Passw & ";" & _
                            "Data Source=(DESCRIPTION = (" & _
                            "ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)" & _
                            "(HOST = " & IPServer & ")(PORT = " & Puerto & ")) )" & _
                            "(CONNECT_DATA = (SERVER = DEDICATED) " & _
                            "(SERVICE_NAME = " & Servicio & ")));"
        ElseIf Tipo = 3 Then
            sProveedor = "MySql.Data.MySqlClient"
            CadenaConexion = "server=" & IPServer & ";user id=" & Usuario & ";port=" & Puerto & ";" & _
                                "password=" & Passw & ";database=" & Servicio & ";pooling=false"
        ElseIf Tipo = 4 Then
            sProveedor = "OleDb.OleDbConnection"
            If Usuario = "" And Passw = "" Then
                CadenaConexion = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" & RutaAccess
            ElseIf Usuario = "" And Passw <> "" Then
                CadenaConexion = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & RutaAccess & ";" & _
                                    "Jet OLEDB:Database Password=" & Passw & ";"
            ElseIf Usuario <> "" And Passw <> "" Then
                CadenaConexion = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
                                "Data Source=" & RutaAccess & ";" & _
                                "Jet OLEDB:System Database=system.mdw;" & _
                                "User ID=" & Usuario & ";Password=" & Passw & ";"
            End If
        ElseIf Tipo = 5 Then
            sProveedor = "OleDb.OleDbConnection"
            If Usuario = "" And Passw = "" Then
                CadenaConexion = "Provider=Microsoft.ACE.OLEDB.12.0;" & _
                                "Data Source=" & RutaAccess & ";Persist Security Info=False"
            ElseIf Usuario = "" And Passw <> "" Then
                CadenaConexion = "Provider=Microsoft.ACE.OLEDB.12.0;" & _
                                "Data Source" & RutaAccess & ";Jet OLEDB:Database Password=" & Passw & ";"
            End If
        ElseIf Tipo = 6 Then
            sProveedor = "Npgsql"
            CadenaConexion = "Server=" & IPServer & ";" & _
                            "Port=" & Puerto & ";" & _
                            "User Id=" & Usuario & ";" & _
                            "Password=" & Passw & ";" & _
                            "Database=" & Servicio & ";CommandTimeout=120;"
        Else
            Exit Function
        End If

        ProData = DbProviderFactories.GetFactory(sProveedor)
        Try
            MainConex = ProData.CreateConnection
            MainConex.ConnectionString = CadenaConexion
            MainConex.Open()
            ConectarBD = True
        Catch Fallo As Exception
            MessageBox.Show(Fallo.Message, My.Application.Info.AssemblyName, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Function


    Function DesconectarBD() As Boolean

        If MainConex.State = ConnectionState.Closed Then Exit Function
        Try
            MainConex.Close()
            MainConex.Dispose()
            DesconectarBD = True
        Catch Fallo As Exception
            MessageBox.Show(Fallo.Message)
        End Try
    End Function

 


End Module
