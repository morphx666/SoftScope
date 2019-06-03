Partial Public Class FormMain
    Private Sub RenderWaveForm(g As Graphics, colorLeft As Pen, colorRight As Pen)
        Dim r As New Rectangle(0, 0, screenWidthHalf / 2, screenHeightHalf / 2)
        r.X = Me.DisplayRectangle.Width - r.Width - 8
        r.Y = Me.DisplayRectangle.Height - r.Height - 8

        Using sb As New SolidBrush(Color.FromArgb(128, 33, 33, 33))
            g.FillRectangle(sb, r)
        End Using

        Dim pts() As Point
        For x As Integer = colorLeft.Width To bufferLength - 2
            pts = Buf2Pts(x, r, 1)
            g.DrawLine(colorLeft, pts(0), pts(1))
            pts = Buf2Pts(x, r, 2)
            g.DrawLine(colorRight, pts(0), pts(1))
        Next

        g.DrawRectangle(Pens.DimGray, r)
        Using p As New Pen(Color.FromArgb(128, 66, 66, 66))
            g.DrawLine(p, r.X, r.Y + r.Height \ 2, r.Right, r.Y + r.Height \ 2)
        End Using
    End Sub

    Private Function Buf2Pts(x As Integer, r As Rectangle, channel As Integer) As Point()
        Dim ps(2 - 1) As Point

        Dim v1 As Integer
        Dim v2 As Integer
        Dim hh As Integer = r.Height / 2
        Dim hh2 As Integer = hh / 2
        Dim y As Integer

        If channel = 1 Then ' Left
            v1 = bufL(x) / maxNormValue * hh
            v2 = bufL(x + 1) / maxNormValue * hh
            y = r.Y + hh2
        Else                ' Right
            v1 = bufR(x) / maxNormValue * hh
            v2 = bufR(x + 1) / maxNormValue * hh
            y = r.Bottom - hh2
        End If

        ps(0) = New Point(r.X + x / bufferLength * r.Width, v1 + y)
        ps(1) = New Point(r.X + (x + 1) / bufferLength * r.Width, v2 + y)

        Return ps
    End Function
End Class
