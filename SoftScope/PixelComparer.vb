Public Class PixelComparerExact
    Implements IComparer(Of Pixel)

    Public Function Compare(p1 As Pixel, p2 As Pixel) As Integer Implements IComparer(Of Pixel).Compare
        Dim x As Integer = p1.X.CompareTo(p2.X)
        If x = 0 Then
            Return p1.Y.CompareTo(p2.Y)
        Else
            Return x
        End If
    End Function
End Class

Public Class PixelComparerSimilar
    Implements IComparer(Of Pixel)

    Public Property Threshold As Integer = 2

    Public Sub New()
    End Sub

    Public Sub New(threshold As Integer)
        Me.Threshold = threshold
    End Sub

    Public Function Compare(p1 As Pixel, p2 As Pixel) As Integer Implements IComparer(Of Pixel).Compare
        If Math.Abs(p1.X - p2.X) > Threshold OrElse Math.Abs(p1.Y - p2.Y) > Threshold Then
            Return ExactCompare(p1, p2)
        Else
            Return 0
        End If
    End Function

    Private Function ExactCompare(p1 As Pixel, p2 As Pixel) As Integer
        Dim x As Integer = p1.X.CompareTo(p2.X)
        If x = 0 Then
            Return p1.Y.CompareTo(p2.Y)
        Else
            Return x
        End If
    End Function
End Class


