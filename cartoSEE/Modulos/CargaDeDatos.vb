Imports System.Data
Imports System.Data.Common

Module CargaDeDatos

    Public ListaMunicipiosHisto As DataTable
    Public ListaProvincias As DataTable
    Public ListaTiposDocumento As DataTable
    Public ListaMunicipiosActual As DataTable
    Public tiposDocSIDCARTO As ArrayList

    Dim dA As DbDataAdapter
    Dim cmdSQL As DbCommand

    Public docHRTypes As New Dictionary(Of Integer, String) From {
                                    {0, "El documento no aparece citado en la Hoja registral"},
                                    {1, "El documento aparece como TÌtulo jurÌdico"},
                                    {2, "El documento aparece como Documento TÈcnico"},
                                    {3, "El documento aparece como Otros Documentos"}
                                }

    Function CargarListaTiposDocumento() As Boolean

        ListaTiposDocumento = New DataTable
        tiposDocSIDCARTO = Nothing
        tiposDocSIDCARTO = New ArrayList
        If CargarDatatable("SELECT idtipodoc,tipodoc,dirrepo,nombrecdd,prefijometadato FROM bdsidschema.tbtipodocumento", ListaTiposDocumento) = False Then
            MessageBox.Show("No se puede acceder a la tabla de tipos de documentos", My.Application.Info.AssemblyName, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        ReDim RutasPlantillasMetadatos(ListaTiposDocumento.Rows.Count - 1)
        Dim item As docCartoSEETipoDocu

        For iBucle As Integer = 0 To ListaTiposDocumento.Rows.Count - 1
            RutasPlantillasMetadatos(iBucle).idTipodoc = ListaTiposDocumento.Rows(iBucle).Item(0).ToString
            RutasPlantillasMetadatos(iBucle).NombreTipo = ListaTiposDocumento.Rows(iBucle).Item(1).ToString
            RutasPlantillasMetadatos(iBucle).RepoTipo = ListaTiposDocumento.Rows(iBucle).Item(2).ToString
            RutasPlantillasMetadatos(iBucle).RutaMetadatosTipo = ""

            RutasPlantillasMetadatos(iBucle).PrefijoNom = ListaTiposDocumento.Rows(iBucle).Item(4).ToString
            item = New docCartoSEETipoDocu
            item.idTipodoc = ListaTiposDocumento.Rows(iBucle).Item(0).ToString
            item.NombreTipo = ListaTiposDocumento.Rows(iBucle).Item(1).ToString
            item.RepoTipo = ListaTiposDocumento.Rows(iBucle).Item(2).ToString
            item.rutaMetadatosTipo = ""
            item.prefijoNombreCDD = ListaTiposDocumento.Rows(iBucle).Item(3).ToString
            item.prefijoMetadatos = ListaTiposDocumento.Rows(iBucle).Item(4).ToString
            tiposDocSIDCARTO.Add(item)
            item = Nothing
        Next

    End Function


    Function CargarListasProvincias() As Boolean

        ListaProvincias = New DataTable

        If CargarDatatable("SELECT idprovincia as INE,nombreprovincia as Nombre,comautonoma_id as autonomia, dirrepo " &
                            "FROM bdsidschema.provincias order by idprovincia", ListaProvincias) = False Then
            'CargarListasMunicipios = False
            MessageBox.Show("No se puede acceder a la tabla de provincias",
                            My.Application.Info.AssemblyName, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Function

    ''' <summary>
    ''' Crea un datatable con la lista de territorios
    ''' </summary>
    Sub CargarListasMunicipios()

        Try
            ListaMunicipiosHisto = New DataTable
            If Not CargarDatatableMuni("SELECT idterritorio as idmunihisto, nombre,provincia as provincia_id,munihisto as cod_munihisto,
                                        translate(nombre,'·ÈÌÛ˙‡ËÏÚ˘¡…Õ”⁄¿»Ã“Ÿ','aeiouaeiouAEIOUAEIOU') as nombreSearch,municipio as inecortoActual 
                                        FROM bdsidschema.territorios WHERE tipo IN ('Municipio','Municipio histÛrico','Condominio histÛrico','Territorio histÛrico','Exclave')", ListaMunicipiosHisto) Then
                ModalExclamation("No se puede acceder a la tabla de territorios")
            End If

        Catch ex As Exception
            ModalError(ex.Message)
        End Try





    End Sub

    ''' <summary>
    ''' Crea un datatable con la lista de municipios actuales
    ''' </summary>
    Sub CargarListasMunicipiosActual()
        Dim cadSQL As String

        Try
            ListaMunicipiosActual = New DataTable
            cadSQL = "SELECT identidad as idmunihisto,nombre ,codigoprov as provincia_id,inecorto || '00' as cod_munihisto," &
                    "translate(listamunicipios.nombre,'·ÈÌÛ˙‡ËÏÚ˘¡…Õ”⁄¿»Ã“Ÿ','aeiouaeiouAEIOUAEIOU') as nombreSearch,inecorto as inecortoActual " &
                    "FROM ngmepschema.listamunicipios"
            If Not CargarDatatableMuni(cadSQL, ListaMunicipiosActual) Then
                ModalExclamation("No se puede acceder a la tabla de municipios actuales")
            End If
        Catch ex As Exception
            ModalError(ex.Message)
        End Try



    End Sub

    Sub registrarDatabaseLog(mensaje As String, Optional statementSQL As String = "")

        If usuarioMyApp.permisosLista.isUserISTARI Then Exit Sub
        If statementSQL <> "" Then
            ExeSinTran("INSERT INTO bdsidschema.logactivity (usuario, maquina, fecha,aplicacion,descrip,consulta) VALUES ('" & usuarioMyApp.loginUser & "','" & usuarioMyApp.machineIP & "','" &
                                FormatearFechaHora(Now, "GERMAN") & "','" & AplicacionTitulo & "',E'" & mensaje.Replace("'", "\'") & "',E'" & statementSQL.Replace("'", "\'") & "')")
        Else
            ExeSinTran("INSERT INTO bdsidschema.logactivity (usuario, maquina, fecha,aplicacion,descrip) VALUES ('" & usuarioMyApp.loginUser & "','" & usuarioMyApp.machineName & "','" & FormatearFechaHora(Now, "GERMAN") & "','" & AplicacionTitulo & "','" & mensaje & "')")
        End If

    End Sub


    Function TratarCadenaExportarCSV(ByVal CadenaIN As String)

        Dim cadWork As String
        cadWork = CadenaIN
        cadWork.Replace("""", """""")
        TratarCadenaExportarCSV = """" & cadWork & """"

    End Function


    Function CargarDataView(ByVal cadenaSQL As String, ByRef Contenedor As DataView) As Boolean

        Dim DsTmp As New DataSet
        cmdSQL = ProData.CreateCommand()
        cmdSQL.Connection = MainConex
        cmdSQL.CommandType = CommandType.Text
        cmdSQL.CommandText = cadenaSQL
        dA = ProData.CreateDataAdapter()
        dA.SelectCommand = cmdSQL
        Try
            dA.Fill(DsTmp)
            Contenedor.Table = DsTmp.Tables(0)
            dA.Dispose()
            DsTmp.Dispose()
            cmdSQL.Dispose()
            Application.DoEvents()
            CargarDataView = True
        Catch Fallo As Exception
            MessageBox.Show(Fallo.Message, My.Application.Info.AssemblyName, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Function


    ''' <summary>
    ''' Funcion para crear un recordset utilizando el modelo de objetos ADO.NET
    ''' </summary>
    ''' <param name="CadenaSQL">Cadena SQL a ejecutar</param>
    ''' <param name="Contenedor">Objeto Datatable para guardar la informaciÛn</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function CargarRecordset(ByVal CadenaSQL As String, ByRef Contenedor As DataTable) As Boolean

        cmdSQL = ProData.CreateCommand()
        cmdSQL.Connection = MainConex
        cmdSQL.CommandType = CommandType.Text
        cmdSQL.CommandText = CadenaSQL
        dA = ProData.CreateDataAdapter()
        'Contenedor = New DataTable
        dA.SelectCommand = cmdSQL
        Try
            dA.Fill(Contenedor)
            dA.Dispose()
            cmdSQL.Dispose()
            CargarRecordset = True
        Catch Fallo As Exception
            CargarRecordset = False
            GenerarLOG(Fallo.Message)
            ModalError(Fallo.Message)
        End Try

    End Function

    Function ExeSinTran(ByVal cadSQL As String) As Boolean

        Dim myComando As DbCommand
        If cadSQL = "" Then Exit Function
        ExeSinTran = False
        Try
            myComando = ProData.CreateCommand
            myComando.Connection = MainConex
            myComando.CommandType = CommandType.Text
            myComando.CommandText = cadSQL
            myComando.ExecuteNonQuery()
            ExeSinTran = True
        Catch ex As Exception
            MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
            GenerarLOG(cadSQL)
        End Try


    End Function



    Function ExeTran(ByRef ListaSQL() As String) As Boolean

        Dim myTrans As DbTransaction
        Dim myComando As DbCommand
        Dim SQLfrase As String = ""
        ExeTran = False
        myTrans = MainConex.BeginTransaction
        Try
            myComando = ProData.CreateCommand
            myComando.Connection = MainConex
            myComando.CommandType = CommandType.Text
            For Each SQLfrase In ListaSQL
                If SQLfrase = "" Then Continue For
                myComando.CommandText = SQLfrase
                myComando.Transaction = myTrans
                myComando.ExecuteNonQuery()
            Next
            myTrans.Commit()
            ExeTran = True
        Catch ex As Exception
            myTrans.Rollback()
            MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
            GenerarLOG(SQLfrase)
        End Try


    End Function


    Function ExeTran(ByRef ListaSQL As ArrayList) As Boolean

        Dim myTrans As DbTransaction
        Dim myComando As DbCommand
        Dim SQLfrase As String = ""
        ExeTran = False
        myTrans = MainConex.BeginTransaction
        Try
            myComando = ProData.CreateCommand
            myComando.Connection = MainConex
            myComando.CommandType = CommandType.Text
            For Each SQLfrase In ListaSQL
                If SQLfrase = "" Then Continue For
                myComando.CommandText = SQLfrase
                myComando.Transaction = myTrans
                myComando.ExecuteNonQuery()
            Next
            'myTrans.Rollback()
            Application.DoEvents()
            myTrans.Commit()
            ExeTran = True
        Catch ex As Exception
            myTrans.Rollback()
            MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
            GenerarLOG(SQLfrase)
        End Try


    End Function

    Function CargarDatatable(ByVal CadenaSQL As String, ByRef Contenedor As DataTable) As Boolean

        Dim dataR As DbDataReader


        cmdSQL = ProData.CreateCommand()
        cmdSQL.Connection = MainConex
        cmdSQL.CommandType = CommandType.Text
        cmdSQL.CommandText = CadenaSQL
        Try
            dataR = cmdSQL.ExecuteReader()
            Contenedor.Load(dataR)
            dataR.Close()
            dataR = Nothing
            cmdSQL.Dispose()
            Application.DoEvents()
            CargarDatatable = True
        Catch Fallo As Exception
            CargarDatatable = False
            MessageBox.Show(Fallo.Message, My.Application.Info.AssemblyName, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Function

    Function CargarDatatableMuni(ByVal CadenaSQL As String, ByRef Contenedor As DataTable) As Boolean

        Dim dataMuni As DbDataReader


        cmdSQL = ProData.CreateCommand()
        cmdSQL.Connection = MainConex
        cmdSQL.CommandType = CommandType.Text
        cmdSQL.CommandText = CadenaSQL
        Try
            dataMuni = cmdSQL.ExecuteReader()
            Contenedor.Load(dataMuni)
            dataMuni.Close()
            dataMuni = Nothing
            cmdSQL.Dispose()
            Application.DoEvents()
            CargarDatatableMuni = True
        Catch Fallo As Exception
            CargarDatatableMuni = False
            MessageBox.Show(Fallo.Message, My.Application.Info.AssemblyName, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Function


    Function ObtenerEscalar(ByVal cadenaSQL As String, ByRef Resultado As String) As Boolean

        cmdSQL = ProData.CreateCommand()
        cmdSQL.Connection = MainConex
        cmdSQL.CommandType = CommandType.Text
        cmdSQL.CommandText = cadenaSQL
        Try
            Resultado = cmdSQL.ExecuteScalar
            ObtenerEscalar = True
        Catch Fallo As Exception
            ObtenerEscalar = False
            MessageBox.Show(Fallo.Message, My.Application.Info.AssemblyName, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        cmdSQL.Dispose()
        cmdSQL = Nothing

    End Function



End Module
