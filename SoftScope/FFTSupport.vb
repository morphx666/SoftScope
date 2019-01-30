Partial Public Class FormMain
    Private fftSize As FFTSizeConstants = FFTSizeConstants.FFTs2048
    Private fftSize2 As Integer = fftSize / 2
    Private fftL() As ComplexDouble
    Private fftR() As ComplexDouble
    Private fftHistSize As Integer = 4
    Private fftLHist(fftHistSize - 1)() As Double
    Private fftRHist(fftHistSize - 1)() As Double
    Private fftWavDstBufL(fftSize - 1) As Double
    Private fftWavDstBufR(fftSize - 1) As Double
    Private fftWavDstIndex As Integer
    Private ffWavSrcBufIndex As Integer
    Private fftWindowValues() As Double
    Private fftWindowSum As Double

    Private Sub RenderFFT(g As Graphics, colorLeft As Pen, colorRight As Pen)
        RunFFT()

        Dim r As New Rectangle(0, 0, screenWidthHalf / 2, screenHeightHalf / 2)
        r.X = Me.DisplayRectangle.Width - r.Width - 8
        r.Y = Me.DisplayRectangle.Height - r.Height - 8

        If renderAudioWaveForm Then r.Y -= r.Height + 8

        Dim s As Size
        Dim penOffset As Integer = colorLeft.Width / 2
        Dim lastPL As Point = New Point(r.X + penOffset, r.Bottom)
        Dim lastPR As Point = lastPL
        Dim newDivX As Integer

        Using p As New Pen(Color.FromArgb(128, Color.DarkSlateGray))
            For x As Integer = 0 To fftSize2 - 1 Step fftSize2 / 5
                For x1 As Integer = 0 To 10
                    newDivX = r.X + x + Math.Min(Math.Log10(x1 + 1) / Math.Log10(fftSize2 - 1) * r.Width, r.Width) + s.Width
                    g.DrawLine(p, newDivX, r.Y, newDivX, r.Bottom)
                Next
            Next
        End Using

        Dim lastW As Integer = FFT2Pts(fftSize2 - 1, r, 1, fftSize).Width + colorLeft.Width
        For x As Integer = 0 To fftSize2 - 1
            s = FFT2Pts(x, r, 1, fftSize)
            newDivX = r.X + x / fftSize2 * (r.Width - lastW) + s.Width + penOffset
            g.DrawLine(colorLeft, lastPL.X, lastPL.Y, newDivX, r.Bottom - s.Height - colorLeft.Width)
            lastPL = New Point(newDivX, r.Bottom - s.Height)

            s = FFT2Pts(x, r, 2, fftSize)
            g.DrawLine(colorRight, lastPR.X, lastPR.Y, newDivX, r.Bottom - s.Height - colorLeft.Width)
            lastPR = New Point(newDivX, r.Bottom - s.Height)
        Next

        g.DrawRectangle(Pens.DimGray, r)
        Using sb As New SolidBrush(Color.FromArgb(128, 33, 33, 33))
            g.FillRectangle(sb, r)
        End Using
    End Sub

    Public Sub RunFFT()
        If fftWavDstIndex = 0 Then FFT.FourierTransform(fftSize, fftWavDstBufL, fftL, fftWavDstBufR, fftR, False)

        For i As Integer = 0 To fftHistSize - 2
            For j As Integer = 0 To fftSize2 - 1
                fftLHist(i)(j) = fftLHist(i + 1)(j)
                fftRHist(i)(j) = fftRHist(i + 1)(j)
            Next
        Next

        For i As Integer = 0 To fftSize2 - 1
            fftLHist(fftHistSize - 1)(i) = fftL(i).Power()
            fftRHist(fftHistSize - 1)(i) = fftR(i).Power()
        Next
    End Sub

    Private Sub FillFFTBuffer()
        If renderAudioFFT Then
            Do
                Do
                    If ffWavSrcBufIndex >= bufferLength Then
                        If fftWavDstIndex >= fftSize Then fftWavDstIndex = 0
                        ffWavSrcBufIndex = 0
                        Exit Do
                    ElseIf fftWavDstIndex >= fftSize Then
                        fftWavDstIndex = 0
                        Exit Do
                    End If

                    fftWavDstBufL(fftWavDstIndex) = bufL(ffWavSrcBufIndex) * fftWindowValues(fftWavDstIndex)
                    fftWavDstBufR(fftWavDstIndex) = bufR(ffWavSrcBufIndex) * fftWindowValues(fftWavDstIndex)

                    fftWavDstIndex += 1
                    ffWavSrcBufIndex += 1
                Loop
            Loop Until fftWavDstIndex = 0 OrElse ffWavSrcBufIndex = 0
        End If
    End Sub

    Private Function FFT2Pts(x As Integer, r As Rectangle, channel As Integer, fftSize As FFTSizeConstants) As Size
        Dim v As Double

        v = (FFTAvg(x, channel) / fftWindowSum * 2) / 20
        v = Math.Min(Math.Log10(v + 1) / 10 * r.Height, r.Height)
        x = Math.Min(Math.Log10(x + 1) / Math.Log10(fftSize2 - 1) * r.Width, r.Width)

        Return New Size(x, v)
    End Function

    Private Function FFTAvg(x As Integer, channel As Integer) As Double
        Dim v As Double
        For i As Integer = 0 To fftHistSize - 1
            If channel = 1 Then
                v += fftLHist(i)(x)
            Else
                v += fftRHist(i)(x)
            End If
        Next
        Return v / fftHistSize
    End Function
End Class
