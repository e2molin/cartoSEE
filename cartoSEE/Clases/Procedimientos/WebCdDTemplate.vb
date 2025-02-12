Public Class WebCdDTemplate

    Private _contenidoHTML As String
    Private _sr As System.IO.StreamReader
    Private _sw As System.IO.StreamWriter

#Region "Class Properties"

    Property templateHTML As String
    Property docuAT As docCartoSEE
    Property folderHTML As String
    Property imgThumb As String


#End Region
    Public Sub New()

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Sub prepareTemplate()

        _sr = New System.IO.StreamReader(templateHTML)
        _contenidoHTML = _sr.ReadToEnd
        _sr.Close()
        _sr.Dispose()
        _sr = Nothing

    End Sub


    Sub generateHTMLreport()

        Dim subParteHTML As String = ""
        Dim contElem As Integer
        Dim nombreFicheroIndexMH As String
        Dim lastMuni As String = ""
        _contenidoHTML = _contenidoHTML.Replace("_#NUMERODOCUMENTO#_", docuAT.Sellado)
        _contenidoHTML = _contenidoHTML.Replace("_#TIPODOC#_", docuAT.tipoDocumento.NombreTipo)
        _contenidoHTML = _contenidoHTML.Replace("_#SUBTIPODOC#_", docuAT.subTipoDoc)
        _contenidoHTML = _contenidoHTML.Replace("_#SUBDIVISION#_", docuAT.Subdivision)
        _contenidoHTML = _contenidoHTML.Replace("_#ESCALADOC#_", "1:" & docuAT.Escala)
        _contenidoHTML = _contenidoHTML.Replace("_#SIZEDOC#_", docuAT.Horizontal.ToString.Replace(",", ".") & " x " & docuAT.Vertical.ToString.Replace(",", ".") & " cm.")
        _contenidoHTML = _contenidoHTML.Replace("_#CALIDADDOC#_", docuAT.Estado)
        _contenidoHTML = _contenidoHTML.Replace("_#SIGNATURA#_", docuAT.Signatura)
        _contenidoHTML = _contenidoHTML.Replace("_#FECHAPRINCIPAL#_", docuAT.FechaPrincipal)
        _contenidoHTML = _contenidoHTML.Replace("_#FECHAMODIFICACION#_", docuAT.fechasModificaciones)
        _contenidoHTML = _contenidoHTML.Replace("_#PROVINCIADOCUMENTO#_", docuAT.Provincias)
        _contenidoHTML = _contenidoHTML.Replace("_#TOMODOC#_", docuAT.Tomo)
        subParteHTML = ""
        contElem = -1
        For Each elem As String In docuAT.listaMuniHistorico
            Application.DoEvents()
            contElem = contElem + 1
            If elem = "" Then Continue For
            If elem.Length > 50 Then elem = elem.Substring(0, 50) & "..."
            subParteHTML = subParteHTML & "<li class=""list-group-item"">" & elem
            Dim procTilde As New Destildator
            nombreFicheroIndexMH = docuAT.listaCodMuniHistorico(contElem) & "_Resumen_documentos_Archivo-" & procTilde.destildar(elem).Replace(" ", "_") & ".html"
            procTilde = Nothing
            subParteHTML = subParteHTML & "<span class=""badge alert-info"">" & _
                            "<a href=""" & nombreFicheroIndexMH & """ title=""Ver documentos de " & elem & """>Ver más documentos</a></span></li>"
        Next
        _contenidoHTML = _contenidoHTML.Replace("_#MUNICIPIOSHISTORICOS#_", subParteHTML)
        subParteHTML = ""
        contElem = -1
        For Each elem As String In docuAT.listaMuniActual
            If elem = lastMuni Then Continue For 'Así evitamos duplicar municipios en la lista de municipios actuales
            Application.DoEvents()
            contElem = contElem + 1
            If elem = "" Then Continue For
            If elem.Length > 50 Then elem = elem.Substring(0, 50) & "..."
            Dim procTilde As New Destildator
            nombreFicheroIndexMH = docuAT.listaCodMuniActual(contElem) & "_Resumen_documentos_Archivo-" & procTilde.destildar(elem).Replace(" ", "_") & ".html"
            procTilde = Nothing
            subParteHTML = subParteHTML & "<li class=""list-group-item"">" & elem
            subParteHTML = subParteHTML & "<span class=""badge alert-info"">" & _
                            "<a href=""" & nombreFicheroIndexMH & """ title=""Ver documentos de " & elem & """>Ver más documentos</a></span></li>"
            lastMuni = elem
        Next
        _contenidoHTML = _contenidoHTML.Replace("_#MUNICIPIOSACTUALES#_", subParteHTML)


        If docuAT.Anejo <> "" Then
            _contenidoHTML = _contenidoHTML.Replace("_#ANEJOS#_", "<li class=""list-group-item"">" & docuAT.Anejo & "</li>")
        Else
            _contenidoHTML = _contenidoHTML.Replace("_#ANEJOS#_", "<li class=""list-group-item"">No se citan</li>")
        End If
        _contenidoHTML = _contenidoHTML.Replace("_#ANEJOS#_", "<li class=""list-group-item"">" & docuAT.Anejo & "</li>")
        _contenidoHTML = _contenidoHTML.Replace("_#RUTAZIPCDD#_", "../zip/" & docuAT.nameFile4CDD.Replace(".jpg", ".zip"))
        _contenidoHTML = _contenidoHTML.Replace("_#PATHTHUMBNAIL#_", "../thumb/" & docuAT.nameFile4CDD)

        'SEO customize
        _contenidoHTML = _contenidoHTML.Replace("_#SEOTITLE#_", docuAT.SEOTitle)
        _contenidoHTML = _contenidoHTML.Replace("_#SEOKEYWORDS#_", "," & docuAT.SEOKeywords)
        _contenidoHTML = _contenidoHTML.Replace("_#SEODESCRIPTION#_", docuAT.SEODescription)

        Application.DoEvents()
        _contenidoHTML = _contenidoHTML.Replace("_#CENTERBBOX#_", docuAT.BBOXCenter4OL3_ByExtent)
        _contenidoHTML = _contenidoHTML.Replace("_#BBOX#_", docuAT.BBOX4OL3)

    End Sub


    Sub saveHTMLreport()
        _sw = New System.IO.StreamWriter(folderHTML & "\" & docuAT.nameFileHTML)
        _sw.Write(_contenidoHTML)
        _sw.Close()
        _sw.Dispose()
        _sw = Nothing
    End Sub

End Class
