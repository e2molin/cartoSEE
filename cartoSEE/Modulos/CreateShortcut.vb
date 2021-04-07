Imports IWshRuntimeLibrary
Module CreateShortcut

    Public Function GetTargetFromShortCut(ByVal rutaLink As String) As String

        Dim wshShell
        Dim shortCut
        Dim target As String
        wshShell = CreateObject("WScript.Shell")
        shortCut = wshShell.createshortcut(rutaLink)
        target = shortCut.targetpath
        shortCut = Nothing
        wshShell = Nothing
        Return target

    End Function

    Public Function CreateLink(ByVal rutaLink As String, ByVal rutaFichero As String)


        MessageBox.Show("Herramienta no disponible")

        'Dim shortCut As IWshRuntimeLibrary.IWshShortcut
        'Dim wshShell As WshShellClass
        'Try
        '    wshShell = New WshShellClass
        '    shortCut = CType(wshShell.CreateShortcut(rutaLink), IWshRuntimeLibrary.IWshShortcut)
        '    With shortCut
        '        .TargetPath = rutaFichero
        '        .WindowStyle = 1
        '        .WorkingDirectory = SacarDirDeRuta(rutaFichero)
        '        '.Arguments
        '        '.iconlocation
        '        .Save()
        '    End With

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message)
        '    GenerarLOG(ex.Message)
        '    Return False
        'End Try
        'wshShell = Nothing
        'Return True


    End Function



End Module
