Public Structure Pixel
    Public Property X As Integer
    Public Property Y As Integer
    Private mAlpha As Integer

    Public Sub New(x As Integer, y As Integer)
        Me.New()
        Me.X = x
        Me.Y = y
    End Sub

    Public Sub New(x As Integer, y As Integer, alpha As Integer)
        Me.X = x
        Me.Y = y
        Me.Alpha = alpha
    End Sub

    Public Sub New(p As Point)
        X = p.X
        Y = p.Y
    End Sub

    Public Sub New(p As Point, alpha As Integer)
        Me.New(p)
        Me.Alpha = alpha
    End Sub

    Public Property Alpha As Integer
        Get
            Return mAlpha
        End Get
        Set(value As Integer)
            mAlpha = Math.Max(0, Math.Min(255, value))
        End Set
    End Property

    Public Shadows Function ToString() As String
        Return $"X={X}, Y={Y}, Alpha={Alpha}"
    End Function

    Public Shared Operator =(p1 As Pixel, p2 As Pixel) As Boolean
        Return p1.X = p2.X AndAlso p1.Y = p2.Y
    End Operator

    Public Shared Operator <>(p1 As Pixel, p2 As Pixel) As Boolean
        Return Not p1 = p2
    End Operator

    Public Shared Narrowing Operator CType(p As Pixel) As Point
        Return New Point(p.X, p.Y)
    End Operator

    Public Shared Widening Operator CType(p As Point) As Pixel
        Return New Pixel(p.X, p.Y)
    End Operator
End Structure
