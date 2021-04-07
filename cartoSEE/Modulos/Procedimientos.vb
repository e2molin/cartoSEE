Module Procedimientos


    Function generaThumbImagen(ByVal rutaImagenIN As String, ByVal rutaImagenThumbOUT As String) As Boolean

        'Dim listaImages As List(Of Image)
        'Dim portada As Image
        Dim dimenX As Integer
        Dim dimenY As Integer
        Dim result As Boolean = False

        'Dim fs As System.IO.FileStream
        'Dim RutaDefault As String = My.Application.Info.DirectoryPath & "\default.png"

        Dim origen As New Bitmap(rutaImagenIN)

        If origen.Height > origen.Width Then
            'Vertical
            'dimenX = CType(origen.Width / origen.Height * 600, Integer)
            dimenX = 600
            dimenY = 800
        Else
            'Horizontal
            dimenX = 800
            dimenY = 600
            'dimenY = CType(origen.Height / origen.Width * 600, Integer)
        End If
        Dim thumb As New Bitmap(dimenX, dimenY)

        Dim g As Graphics = Graphics.FromImage(thumb)
        g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBilinear
        g.DrawImage(origen, New Rectangle(0, 0, origen.Width, origen.Height), _
                            New Rectangle(0, 0, dimenX, dimenY), GraphicsUnit.Pixel)
        g.DrawImage(origen, 0, 0, dimenX, dimenY)
        g.Dispose()

        Try
            thumb.Save(My.Application.Info.DirectoryPath & "\tmpthumb.jpg", System.Drawing.Imaging.ImageFormat.Jpeg)
            System.IO.File.Copy(My.Application.Info.DirectoryPath & "\tmpthumb.jpg", rutaImagenThumbOUT, True)
            result = True
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            origen.Dispose()
            origen = Nothing
        End Try

        Return result



    End Function

End Module
