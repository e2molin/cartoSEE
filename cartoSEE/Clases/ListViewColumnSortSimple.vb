Public Class ListViewColumnSortSimple

    ' Esta clase implementa la interfaz IComparer
    Implements IComparer
    '
    ' La columna por la que queremos clasificar
    Public ColumnIndex As Integer = 0
    ' El tipo de clasificación a realizar
    Public Sorting As SortOrder = SortOrder.Ascending
    '
    ' Función que se usará para comparar los dos elementos
    Public Overridable Function Compare(ByVal a As Object, _
                                        ByVal b As Object) As Integer _
                                        Implements IComparer.Compare
        '
        ' Esta función devolverá:
        '   -1 si el primer elemento es menor que el segundo
        '    0 si los dos son iguales
        '    1 si el primero es mayor que el segundo
        '
        Dim menor, mayor As Integer
        Dim s1, s2 As String
        '
        ' Los objetos pasados a esta función serán del tipo ListViewItem.
        ' Convertir el texto en el formato adecuado
        ' y tomar el texto de la columna en la que se ha pulsado
        s1 = CType(a, ListViewItem).SubItems(ColumnIndex).Text
        s2 = CType(b, ListViewItem).SubItems(ColumnIndex).Text
        '
        ' Asignar cuando es menor o mayor, dependiendo del orden de clasificación
        Select Case Sorting
            Case SortOrder.Ascending
                ' Esta es la forma predeterminada
                menor = -1
                mayor = 1
            Case SortOrder.Descending
                ' invertimos los valores predeterminados
                menor = 1
                mayor = -1
            Case SortOrder.None
                ' Todos los elementos se considerarán iguales
                menor = 0
                mayor = 0
        End Select
        '
        ' Realizamos la comparación y devolvemos el valor esperado
        If s1 < s2 Then
            Return menor
        ElseIf s1 = s2 Then
            Return 0
        Else
            Return mayor
        End If
    End Function
End Class
