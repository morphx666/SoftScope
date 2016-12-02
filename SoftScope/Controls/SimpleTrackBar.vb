Public Class SimpleTrackBar
    Inherits Control

    Private mValue As Integer = 50
    Private mMax As Integer = 100
    Private mMin As Integer = 0
    Private mReadOnly As Boolean = False

    Private brushColor As Brush
    Private isLeftMouseButtonDown As Boolean

    Public Event ValueChanged(sender As Object, e As EventArgs)
    Public Event ValueChangedFromMouseClick(sender As Object, e As EventArgs)

    Public Sub New()
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.UserPaint, True)
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)

        AddHandler Me.ForeColorChanged, AddressOf SetPenColor
        AddHandler Me.MouseDown, Sub(s As Object, e As MouseEventArgs)
                                     isLeftMouseButtonDown = (Not mReadOnly) AndAlso ((e.Button And MouseButtons.Left) = MouseButtons.Left)
                                     If isLeftMouseButtonDown Then MouseX2Value(e.X)
                                 End Sub
        AddHandler Me.MouseUp, Sub(s As Object, e As MouseEventArgs) If isLeftMouseButtonDown Then isLeftMouseButtonDown = Not ((e.Button And MouseButtons.Left) = MouseButtons.Left)
        AddHandler Me.MouseMove, Sub(s As Object, e As MouseEventArgs) If isLeftMouseButtonDown Then MouseX2Value(e.X)

        SetPenColor()
    End Sub

    Public Property [ReadOnly] As Boolean
        Get
            Return mReadOnly
        End Get
        Set(value As Boolean)
            mReadOnly = value
        End Set
    End Property

    Public Property Value As Integer
        Get
            Return mValue
        End Get
        Set(value As Integer)
            mValue = value
            RaiseEvent ValueChanged(Me, New EventArgs())
            Me.Invalidate()
        End Set
    End Property

    Public Property Max As Integer
        Get
            Return mMax
        End Get
        Set(value As Integer)
            mMax = value
            Me.Invalidate()
        End Set
    End Property

    Public Property Min As Integer
        Get
            Return mMin
        End Get
        Set(value As Integer)
            mMin = value
            Me.Invalidate()
        End Set
    End Property

    Private Sub MouseX2Value(x As Integer)
        x = Math.Min(Me.DisplayRectangle.Width, Math.Max(0, x))
        Value = mMin + x / Me.DisplayRectangle.Width * (mMax - mMin)
        RaiseEvent ValueChangedFromMouseClick(Me, New EventArgs())
    End Sub

    Private Sub SetPenColor()
        brushColor?.Dispose()
        brushColor = New SolidBrush(Me.ForeColor)

        Me.Invalidate()
    End Sub

    Private Sub SimpleTrackBar_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        Dim g As Graphics = e.Graphics
        Dim r As Rectangle = Me.DisplayRectangle
        g.FillRectangle(brushColor, r.X, r.Y, CInt((mValue - mMin) / (mMax - mMin) * r.Width), r.Height)
    End Sub
End Class
