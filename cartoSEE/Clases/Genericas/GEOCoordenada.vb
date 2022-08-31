' Esta clase necesita una conexión a una base de datos con POSTGIS instalado

Public Class GEOCoordenada

    Const Equatorial_EARTH_circunference_meters As Double = 40075016.686 'Equatorial circumference of the Earth for the reference geoid used by OpenStreetMap

    Enum srs As Integer
        GEO_ETRS89 = 4258
        GEO_WGS84 = 4326
        GEO_PicoNieves = 4728
        GOOGLE_SphericalMercator = 3857
        REGCAN_UTM27 = 4082
        REGCAN_UTM28 = 4083
        UTM28_SobreETRS89 = 25828
        UTM29_SobreETRS89 = 25829
        UTM30_SobreETRS89 = 25830
        UTM31_SobreETRS89 = 25831
        UTM28_SobreED50 = 23028
        UTM29_SobreED50 = 23029
        UTM30_SobreED50 = 23030
        UTM31_SobreED50 = 23031
    End Enum



    Property EastingCoord As Double = 0
    Property NorthingCoord As Double = 0
    Property EPSGcode As Integer = 4326

    ''' <summary>
    ''' Devuelve en metros la longitud de un Tile en un determinado nivel de zoom
    ''' </summary>
    ''' <returns></returns>
    Function getTileSize(nZoom As Integer) As Double

        Dim sizeTile As Double

        Try
            sizeTile = Equatorial_EARTH_circunference_meters * Math.Cos(Math.PI / 180 * NorthingCoord) / Math.Pow(2, nZoom)
        Catch ex As Exception
            MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return sizeTile

    End Function

    Function getPixelSize(nZoom As Integer, pixelsInTile As Integer) As Double

        Dim sizePixel As Double = 0

        Try
            Dim sizeTile As Double = getTileSize(nZoom)
            sizePixel = sizeTile / pixelsInTile
        Catch ex As Exception
            MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return sizePixel

    End Function


    Sub New()
        Application.DoEvents()
    End Sub

    Sub New(eastingIN As Double, northingIN As Double, epsgCodeIN As srs)
        Application.DoEvents()
        _EastingCoord = eastingIN
        _NorthingCoord = northingIN
        _EPSGcode = epsgCodeIN
    End Sub

    Sub New(eastingIN As String, northingIN As String, epsgCodeIN As srs)
        Application.DoEvents()
        _EastingCoord = CType(eastingIN.Replace(".", Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator), Double)
        _NorthingCoord = CType(northingIN.Replace(".", Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator), Double)
        _EPSGcode = epsgCodeIN
    End Sub
    Sub New(eastingIN As String, northingIN As String, epsgCodeIN As String)

        Try
            _EastingCoord = CType(eastingIN.Replace(".", Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator), Double)
            _NorthingCoord = CType(northingIN.Replace(".", Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator), Double)
            For Each item In System.Enum.GetValues(GetType(srs))
                If item.GetHashCode.ToString = epsgCodeIN Then
                    Application.DoEvents()
                    _EPSGcode = item
                End If
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Public Function convertTo(newEPSGCode As srs) As GEOCoordenada

        Dim rcddGEO As New DataTable
        Dim cooOUT As GEOCoordenada
        Application.DoEvents()
        Dim geoSQL As String = "SELECT " &
                    "ST_X(ST_Transform(ST_GeomFromText('POINT(" & EastingCoord.ToString.Replace(",", ".") & " " & NorthingCoord.ToString.Replace(",", ".") & ")'," & EPSGcode.GetHashCode & ")," & newEPSGCode.GetHashCode & ")) as Lon, " &
                    "ST_Y(ST_Transform(ST_GeomFromText('POINT(" & EastingCoord.ToString.Replace(",", ".") & " " & NorthingCoord.ToString.Replace(",", ".") & ")'," & EPSGcode.GetHashCode & ")," & newEPSGCode.GetHashCode & ")) as Lat "

        Try
            rcddGEO = New DataTable
            If CargarRecordset(geoSQL, rcddGEO) = False Then
                rcddGEO = Nothing
            End If
            For Each dR As DataRow In rcddGEO.Select
                cooOUT = New GEOCoordenada(dR.Item("Lon").ToString, dR.Item("Lat").ToString, newEPSGCode)
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            rcddGEO.Dispose()
            rcddGEO = Nothing
        End Try


        Return cooOUT


    End Function




End Class
