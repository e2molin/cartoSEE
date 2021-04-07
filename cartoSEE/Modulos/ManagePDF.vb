Module ManagePDF

    Function ObtenerInfoPDF(ByVal RutaPDF As String) As Integer

        Dim Reader As iTextSharp.text.pdf.PdfReader
        Dim NumPaginas As Integer


        Try
            Reader = New iTextSharp.text.pdf.PdfReader(RutaPDF)
            NumPaginas = Reader.NumberOfPages
            ObtenerInfoPDF = NumPaginas
            Application.DoEvents()
            Reader.Close()
        Catch ex As Exception
            ObtenerInfoPDF = 0
        End Try
        Reader = Nothing


    End Function



    ''' <summary>
    ''' Inserta una marca de agua de 100x100 unidades, 1.39x1.39 pulgadas, en la parte superior derecha como marca de agua
    ''' </summary>
    ''' <param name="RutaPDFin">Ruta PDF de entrada</param>
    ''' <param name="RutaPDFout">Ruta PDF de Salida</param>
    ''' <param name="RutaImagen">Ruta de la imagen. Si es mayor se reeescalará</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function IntroducirMarcaAgua(ByVal RutaPDFin As String, ByVal RutaPDFout As String, ByVal RutaImagen As String) As Boolean

        Dim NumPaginas As Integer
        Dim iBucle As Integer
        Dim Reader As iTextSharp.text.pdf.PdfReader
        Reader = New iTextSharp.text.pdf.PdfReader(RutaPDFin)



        Dim Stamper As iTextSharp.text.pdf.PdfStamper
        Stamper = New iTextSharp.text.pdf.PdfStamper(Reader, New System.IO.FileStream(RutaPDFout, IO.FileMode.Create))
        Application.DoEvents()


        Dim imagen As iTextSharp.text.Image
        imagen = iTextSharp.text.Image.GetInstance(RutaImagen)

        NumPaginas = Reader.NumberOfPages

        Dim under As iTextSharp.text.pdf.PdfContentByte
        Dim Marco As iTextSharp.text.Rectangle

        For iBucle = 1 To NumPaginas
            Marco = Reader.GetPageSize(iBucle)
            imagen.SetAbsolutePosition(0, Marco.Height - 100)
            imagen.ScaleAbsolute(100, 100)
            under = Stamper.GetOverContent(iBucle)
            under.AddImage(imagen)
        Next
        under = Nothing
        Stamper.Close()
        Stamper = Nothing
        Reader.Close()
        Reader = Nothing
        Application.DoEvents()

    End Function



    Function InsertarPagina(ByVal FicheroPDFin As String, ByVal FicheroPDFout As String, ByVal Posicion As Integer) As Boolean


        Dim pdfin As iTextSharp.text.pdf.PdfReader
        pdfin = New iTextSharp.text.pdf.PdfReader(FicheroPDFin)
        Application.DoEvents()
        Console.WriteLine("Numero de paginas:" & pdfin.NumberOfPages.ToString)
        Console.WriteLine("Version:" & pdfin.PdfVersion.ToString)
        Console.WriteLine(pdfin.Tampered)

        Dim PdfStamper As iTextSharp.text.pdf.PdfStamper
        PdfStamper = New iTextSharp.text.pdf.PdfStamper(pdfin, New System.IO.FileStream(FicheroPDFOUT, IO.FileMode.Create))
        Application.DoEvents()
        Dim tama As iTextSharp.text.Rectangle

        'Cojo de tamaño para la pagina nueva el tamaño de la portada
        'tama = pdfin.GetPageSize(1) 'Cojo de tamaño para la pagina nueva el tamaño de la portada
        'tama = iTextSharp.text.PageSize.A4
        'Hay que dividir entre 2.7 para pasar de pixels a puntos PDF
        'El tamaño de la página en pulgadas hay que dividirlo entre 72
        tama = New iTextSharp.text.Rectangle(0, 0, 634, 936) 'Abajo-Izquierda / Arriba-Derecha

        Console.WriteLine(tama.Height.ToString & "-" & tama.Width.ToString)
        Application.DoEvents()

        PdfStamper.InsertPage(Posicion, tama)


        tama = pdfin.GetPageSize(2)
        Console.WriteLine(tama.Height.ToString & "-" & tama.Width.ToString)
        Application.DoEvents()
        PdfStamper.Close()
        pdfin.Close()
        pdfin = Nothing
        Application.DoEvents()

    End Function



    'Function InsertarImagenPagina(ByVal FicheroPDFin As String, ByVal FicheroPDFout As String, _
    '                                ByVal RutaImagen As String, ByVal Posicion As Integer) As Boolean
    Function InsertarListaImagenesEnPDF(ByVal FicheroPDFin As String, ByVal FicheroPDFout As String, _
                                    ByVal ListaImagenes() As String) As Boolean


        Dim pdfin As iTextSharp.text.pdf.PdfReader
        Dim PdfStamper As iTextSharp.text.pdf.PdfStamper
        Dim ProcEscaneo As ScanDIR
        Dim PixelAncho As Integer
        Dim PixelAlto As Integer
        Dim ResAncho As Integer
        Dim ResAlto As Integer
        Dim TamaAlto As Single
        Dim TamaAncho As Single
        Dim Partes() As String
        Dim Posicion As Integer
        Dim RutaImagen As String


        pdfin = New iTextSharp.text.pdf.PdfReader(FicheroPDFin)
        Application.DoEvents()

        PdfStamper = New iTextSharp.text.pdf.PdfStamper(pdfin, New System.IO.FileStream(FicheroPDFout, IO.FileMode.Create))
        Application.DoEvents()
        Dim tama As iTextSharp.text.Rectangle

        '--------------------------------------------------
        ProcEscaneo = New ScanDIR

        For Each imagenInsert As String In ListaImagenes
            Partes = imagenInsert.Split("#")
            Posicion = CType(Partes(0), Integer)
            RutaImagen = Partes(1)
            PixelAncho = 0
            PixelAlto = 0
            ResAncho = 0
            ResAlto = 0
            ProcEscaneo.AnalizarJPGHeader(RutaImagen, PixelAncho, PixelAlto, ResAncho, ResAlto)
            TamaAlto = PixelAlto / ResAlto * 72
            TamaAncho = PixelAncho / ResAncho * 72
            Dim imagen As iTextSharp.text.Image
            imagen = iTextSharp.text.Image.GetInstance(RutaImagen)
            imagen.SetAbsolutePosition(0, 0)
            imagen.ScaleAbsolute(TamaAncho, TamaAlto)
            tama = New iTextSharp.text.Rectangle(0, 0, TamaAncho, TamaAlto) 'Abajo-Izquierda / Arriba-Derecha
            PdfStamper.InsertPage(Posicion, tama)
            Dim under As iTextSharp.text.pdf.PdfContentByte
            under = PdfStamper.GetOverContent(Posicion)
            under.AddImage(imagen)
            under = Nothing
            imagen = Nothing
        Next
        Application.DoEvents()
        ProcEscaneo = Nothing

        Application.DoEvents()
        PdfStamper.Close()
        pdfin.Close()
        pdfin = Nothing
        Application.DoEvents()
        InsertarListaImagenesEnPDF = True

    End Function


    Function DuplicarContinuacionPaginaN(ByVal FicheroPDFOrigen As String, ByVal FicheroPDFDestino As String, ByVal Pagina As Integer) As Boolean
        'Copia las paginas del PDFOrigen al PDFDestino creado de cero


        Dim pdfin As iTextSharp.text.pdf.PdfReader
        Dim Documento As iTextSharp.text.Document

        'Las paginas empiezan en cero
        Pagina = Pagina - 1

        pdfin = New iTextSharp.text.pdf.PdfReader(FicheroPDFOrigen)
        Documento = New iTextSharp.text.Document(pdfin.GetPageSizeWithRotation(1))

        Dim copia As New iTextSharp.text.pdf.PdfCopy(Documento, New System.IO.FileStream(FicheroPDFDestino, IO.FileMode.Create))
        Documento.Open()

        'Esto inserta tres veces la misma pagina
        copia.AddPage(copia.GetImportedPage(pdfin, Pagina))
        copia.AddPage(copia.GetImportedPage(pdfin, Pagina))
        copia.AddPage(copia.GetImportedPage(pdfin, Pagina))

        Documento.Close()
        Documento = Nothing
        copia.Close()
        copia = Nothing
        pdfin.Close()
        pdfin = Nothing

    End Function


    Function CollagePDF(ByVal FicheroPDFOrigen As String, ByVal FicheroPDFDestino As String, ByVal PaginasBorrar() As Integer) As Boolean
        'Copia las paginas del PDFOrigen al PDFDestino creado de cero


        Dim pdfin As iTextSharp.text.pdf.PdfReader
        Dim Documento As iTextSharp.text.Document

        'Las paginas empiezan en cero
        'Pagina = Pagina - 1

        pdfin = New iTextSharp.text.pdf.PdfReader(FicheroPDFOrigen)
        Documento = New iTextSharp.text.Document(pdfin.GetPageSizeWithRotation(1))

        Dim copia As New iTextSharp.text.pdf.PdfCopy(Documento, New System.IO.FileStream(FicheroPDFDestino, IO.FileMode.Create))
        Documento.Open()

        Dim NPaginas As Integer = pdfin.NumberOfPages

        For iBucle As Integer = 0 To NPaginas - 1
            If Array.IndexOf(PaginasBorrar, iBucle + 1) = -1 Then
                copia.AddPage(copia.GetImportedPage(pdfin, iBucle + 1))
            End If
        Next

        Documento.Close()
        Documento = Nothing
        copia.Close()
        copia = Nothing
        pdfin.Close()
        pdfin = Nothing
        CollagePDF = True

    End Function






    ''' <summary>
    ''' Crea un PDF compuesto por las páginas que hay dentro de un directorio
    ''' </summary>
    ''' <param name="CarpetaIN">Directorio en el que se encuentran las imagenes</param>
    ''' <param name="RutaPDFout">Ruta completa del fichero de salida</param>
    ''' <returns>Devuelve el número de páginas si se genera el fichero</returns>
    ''' <remarks>
    ''' Si insertamos más de una página en blanco seguidas con el metodo document.newPage, tenemos que tener en cuenta que si no insertamos
    ''' nada en el PDF, no se añafiran. El truco consiste en añadir entre página y página algo inficble como por ejemplo:
    ''' document.newPage
    ''' document.add(Chunk.NEWLINE)
    ''' document.newPage()
    '''</remarks>
    Function CrearDIR2PDF(ByVal CarpetaIN As String, ByVal RutaPDFout As String) As Integer

        Dim ListaImagenes() As String
        Dim Extensiones() As String
        Dim ProcEscaneo As ScanDIR
        Dim Aproc As Integer
        Dim PixelAncho As Integer
        Dim PixelAlto As Integer
        Dim ResAncho As Integer
        Dim ResAlto As Integer
        Dim TamaAlto As Single
        Dim TamaAncho As Single

        'Obtengo la lista de imagenes en el directorio
        ReDim Extensiones(0)
        ReDim ListaImagenes(50)
        Extensiones(0) = ".jpg"
        ProcEscaneo = New ScanDIR
        Aproc = ProcEscaneo.ListarFicheros(CarpetaIN, ListaImagenes, Extensiones)

        'Creamos la instancia al PDF
        Dim pdfdoc As iTextSharp.text.Document
        Dim pdfescritura As iTextSharp.text.pdf.PdfWriter
        Dim TamPagina As iTextSharp.text.Rectangle

        pdfdoc = New iTextSharp.text.Document()
        pdfescritura = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfdoc, New System.IO.FileStream(RutaPDFout, IO.FileMode.Create))
        pdfescritura.SetPdfVersion(New iTextSharp.text.pdf.PdfName(iTextSharp.text.pdf.PdfWriter.VERSION_1_6))
        pdfdoc.AddTitle("Documento SIDDAE")
        pdfdoc.AddAuthor("Instituto Geográfico Nacional")
        pdfdoc.AddKeywords("Deslindes, Línea Límite")
        pdfdoc.AddCreator("Libreria iTextSharp")
        pdfdoc.Open()
        For Each FichImagen As String In ListaImagenes
            'Obtengo el Tamaño de la Imagen
            PixelAncho = 0
            PixelAlto = 0
            ResAncho = 0
            ResAlto = 0
            Try
                ProcEscaneo.AnalizarJPGHeader(FichImagen, PixelAncho, PixelAlto, ResAncho, ResAlto)
                TamaAlto = PixelAlto / ResAlto * 72
                TamaAncho = PixelAncho / ResAncho * 72
                'TamaAncho = 634
                'TamaAlto = 936
                TamPagina = New iTextSharp.text.Rectangle(0, 0, TamaAncho, TamaAlto) 'Abajo-Izquierda / Arriba-Derecha
                'Abrimos el documento------------------------------------------------------------------------------------
                pdfdoc.SetPageSize(TamPagina)
                pdfdoc.NewPage()
                Dim imagen As iTextSharp.text.Image
                imagen = iTextSharp.text.Image.GetInstance(FichImagen)
                imagen.SetAbsolutePosition(0, 0)
                imagen.ScaleAbsolute(TamaAncho, TamaAlto)
                pdfdoc.Add(imagen)
                pdfescritura.Flush()
            Catch ex As Exception
                MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit For
            End Try
        Next
        CrearDIR2PDF = pdfescritura.PageNumber
        pdfdoc.Close()
        pdfdoc = Nothing
        pdfescritura = Nothing
        ProcEscaneo = Nothing
        If System.IO.File.Exists(RutaPDFout) = False Then
            CrearDIR2PDF = 0
        End If

    End Function


    Function ConvertirJPG2PDF(ByVal RutaJPGin As String, ByVal RutaPDFout As String) As Integer

        Dim PixelAncho As Integer
        Dim PixelAlto As Integer
        Dim ResAncho As Integer
        Dim ResAlto As Integer
        Dim TamaAlto As Single
        Dim TamaAncho As Single
        Dim FichImagen As String
        Dim ProcEscaneo As New ScanDIR


        'Creamos la instancia al PDF
        Dim pdfdoc As iTextSharp.text.Document
        Dim pdfescritura As iTextSharp.text.pdf.PdfWriter
        Dim TamPagina As iTextSharp.text.Rectangle

        pdfdoc = New iTextSharp.text.Document()
        pdfescritura = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfdoc, New System.IO.FileStream(RutaPDFout, IO.FileMode.Create))
        pdfescritura.SetPdfVersion(New iTextSharp.text.pdf.PdfName(iTextSharp.text.pdf.PdfWriter.VERSION_1_6))
        pdfdoc.AddTitle("Documento")
        pdfdoc.AddAuthor("ISIG SL")
        'pdfdoc.AddKeywords("Deslindes, Línea Límite")
        pdfdoc.AddCreator("Libreria iTextSharp")
        pdfdoc.Open()
        'Obtengo el Tamaño de la Imagen
        PixelAncho = 0
        PixelAlto = 0
        ResAncho = 0
        ResAlto = 0
        FichImagen = RutaJPGin
        Try
            ProcEscaneo.AnalizarJPGHeader(FichImagen, PixelAncho, PixelAlto, ResAncho, ResAlto)
            TamaAlto = PixelAlto / ResAlto * 72
            TamaAncho = PixelAncho / ResAncho * 72
            'TamaAncho = 634
            'TamaAlto = 936
            TamPagina = New iTextSharp.text.Rectangle(0, 0, TamaAncho, TamaAlto) 'Abajo-Izquierda / Arriba-Derecha
            'Abrimos el documento------------------------------------------------------------------------------------
            pdfdoc.SetPageSize(TamPagina)
            pdfdoc.NewPage()
            Dim imagen As iTextSharp.text.Image
            imagen = iTextSharp.text.Image.GetInstance(FichImagen)
            imagen.SetAbsolutePosition(0, 0)
            imagen.ScaleAbsolute(TamaAncho, TamaAlto)
            pdfdoc.Add(imagen)
            pdfescritura.Flush()
        Catch ex As Exception
            MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        ConvertirJPG2PDF = pdfescritura.PageNumber
        pdfdoc.Close()
        pdfdoc = Nothing
        pdfescritura = Nothing
        ProcEscaneo = Nothing
        If System.IO.File.Exists(RutaPDFout) = False Then
            ConvertirJPG2PDF = 0
        End If

    End Function

End Module
