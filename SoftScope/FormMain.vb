Imports System.Threading
Imports System.IO
Imports NAudio.Wave

Public Class FormMain
    Private Const ToDeg As Double = 180.0 / Math.PI

    Private audioSource As WaveIn

    Private screenWidthHalf As Double
    Private widthFactor As Double

    Private screenHeightHalf As Double
    Private heightFactor As Double

    Private distanceFactor As Double

    Private flipXY As Boolean = True
    Private flipX As Boolean = True
    Private flipY As Boolean = False

    Private pixels As New List(Of Point)

    Private Const sampleRate As Integer = 96000
    Private Const channels As Integer = 2
    Private Const bitDepth As Integer = 16
    Private Const dataSize As Integer = bitDepth \ 8
    Private Const bufferMilliseconds As Integer = 25
    'Private Const bitrate As Integer = (sampleRate * bitDepth * channels * bufferMilliseconds / 1000) / 8
    Private Const stp As Integer = channels * dataSize
    Private Const maxNormValue As Integer = 2 ^ bitDepth
    Private tmpB(dataSize + (dataSize Mod 2) - 1) As Byte

    'Private filterL As New FilterButterworth(11000, sampleRate, FilterButterworth.PassType.Lowpass, Math.Sqrt(2))
    'Private filterR As New FilterButterworth(11000, sampleRate, FilterButterworth.PassType.Lowpass, Math.Sqrt(2))

    Private beamColor As Color = Color.FromArgb(255, 90, 255, 90)
    'Private beamColor As Color = Color.FromArgb(255, 90, 90, 255)

    Private bufL(sampleRate * channels / 2 * bufferMilliseconds / 1000 - 1) As Integer
    Private bufR(sampleRate * channels / 2 * bufferMilliseconds / 1000 - 1) As Integer

    Private fftSize As FFTSizeConstants = FFTSizeConstants.FFTs2048
    Private fftSize2 As Integer = fftSize / 2
    Private fftL(fftSize - 1) As ComplexDouble
    Private fftR(fftSize - 1) As ComplexDouble
    Private fftHistSize As Integer = 4
    Private fftLHist(fftHistSize - 1)() As Double
    Private fftRHist(fftHistSize - 1)() As Double
    Private fftWindowSum As Double = FFT.GetWindowSum(fftSize, FFTWindowConstants.Hanning)

    Private renderAudioWaveForm As Boolean = True
    Private renderAudioFFT As Boolean = True

    Private lastPoints As New List(Of Point)

    Private abortThreads As Boolean

    Private Sub FormMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        audioSource.StopRecording()
        audioSource.Dispose()
    End Sub

    Private Sub FormMain_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.UserPaint, True)
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)

        audioSource = New WaveIn()
        audioSource.WaveFormat = New WaveFormat(sampleRate, bitDepth, channels)
        audioSource.BufferMilliseconds = bufferMilliseconds
        AddHandler audioSource.DataAvailable, AddressOf ProcessAudio
        audioSource.StartRecording()

        ' http://oscilloscopemusic.com/
        flipXY = False
        flipX = False
        flipY = False

        ' youscope
        'flipXY = True
        'flipX = True
        'flipY = True

        For i As Integer = 0 To fftHistSize - 1
            ReDim fftLHist(i)(fftSize2 - 1)
            ReDim fftRHist(i)(fftSize2 - 1)
        Next

        Dim renderer As New Thread(Sub()
                                       Do
                                           Thread.Sleep(1000 / 60)
                                           Me.Invalidate()
                                       Loop Until abortThreads
                                   End Sub)
        renderer.IsBackground = True
        renderer.Start()
    End Sub

    Private Sub ProcessAudio(sender As Object, e As WaveInEventArgs)
        Dim x As Integer
        Dim y As Integer
        Dim tmp As Integer

        SyncLock pixels
            pixels.Clear()

            For i As Integer = 0 To e.Buffer.Length - 1 Step stp
                Select Case bitDepth
                    Case 8
                        bufL(i / stp) = (128 - e.Buffer(i))
                        bufR(i / stp) = (128 - e.Buffer(i + dataSize))
                    Case 16
                        Array.Copy(e.Buffer, i, tmpB, 0, dataSize)
                        bufL(i / stp) = BitConverter.ToInt16(tmpB, 0)

                        Array.Copy(e.Buffer, i + dataSize, tmpB, 0, dataSize)
                        bufR(i / stp) = BitConverter.ToInt16(tmpB, 0)
                    Case 24
                        Array.Copy(e.Buffer, i, tmpB, 1, dataSize)
                        bufL(i / stp) = BitConverter.ToInt32(tmpB, 0)

                        Array.Copy(e.Buffer, i + dataSize, tmpB, 1, dataSize)
                        bufR(i / stp) = BitConverter.ToInt32(tmpB, 0)
                End Select

                'filterL.Update(normL)
                'normL = filterL.Value

                'filterR.Update(normR)
                'normR = filterR.Value

                x = bufL(i / stp) * widthFactor
                y = -bufR(i / stp) * heightFactor

                If flipX Then x = -x
                If flipY Then y = -y

                If flipXY Then
                    tmp = x
                    x = y
                    y = tmp
                End If

                x += screenWidthHalf
                y += screenHeightHalf

                pixels.Add(New Point(x, y))
            Next
        End SyncLock
    End Sub

    Private Sub RunTest()
        Dim l() As Single
        Dim r() As Single
        ReadWav("youscope.wav", l, r)

        Dim ln(l.Length - 1) As Integer
        Dim rn(r.Length - 1) As Integer
        For i As Integer = 0 To l.Length - 1
            ln(i) = l(i) * 32767
            rn(i) = r(i) * 32767
        Next

        Dim t As New Thread(Sub()
                                Dim i As Integer = 0
                                Dim n(8192 - 1) As Integer
                                Dim f As Boolean = True

                                Dim oneSecondInBytes = 48000 * 2 * 2
                                Dim delay = (n.Length * 1000) / oneSecondInBytes

                                Do
                                    If f Then
                                        For j = 0 To n.Length - 1 Step 2
                                            n(j) = ln(i) / 5
                                            n(j + 1) = rn(i + 1) / 5
                                            i += 2
                                        Next
                                    End If
                                    f = Not f

                                    'DxvuCtrl_PeakValues(Nothing, n, Nothing)

                                    Thread.Sleep(delay)
                                Loop
                            End Sub)
        t.IsBackground = True
        t.Start()
    End Sub

    Private Shared Function ReadWav(filename As String, ByRef L As Single(), ByRef R As Single()) As Boolean
        L = Nothing
        R = Nothing

        Try
            Using fs As FileStream = File.Open(filename, FileMode.Open)
                Dim reader As New BinaryReader(fs)

                ' chunk 0
                Dim chunkID As Integer = reader.ReadInt32()
                Dim fileSize As Integer = reader.ReadInt32()
                Dim riffType As Integer = reader.ReadInt32()


                ' chunk 1
                Dim fmtID As Integer = reader.ReadInt32()
                Dim fmtSize As Integer = reader.ReadInt32()
                ' bytes for this chunk
                Dim fmtCode As Integer = reader.ReadInt16()
                Dim channels As Integer = reader.ReadInt16()
                Dim sampleRate As Integer = reader.ReadInt32()
                Dim byteRate As Integer = reader.ReadInt32()
                Dim fmtBlockAlign As Integer = reader.ReadInt16()
                Dim bitDepth As Integer = reader.ReadInt16()

                If fmtSize = 18 Then
                    ' Read any extra values
                    Dim fmtExtraSize As Integer = reader.ReadInt16()
                    reader.ReadBytes(fmtExtraSize)
                End If

                ' chunk 2
                Dim dataID As Integer = reader.ReadInt32()
                Dim bytes As Integer = reader.ReadInt32()

                ' DATA!
                Dim byteArray As Byte() = reader.ReadBytes(bytes)

                Dim bytesForSamp As Integer = bitDepth / 8
                Dim samps As Integer = bytes \ bytesForSamp


                Dim asFloat As Single() = Nothing
                Select Case bitDepth
                    Case 64
                        Dim asDouble As Double() = New Double(samps - 1) {}
                        Buffer.BlockCopy(byteArray, 0, asDouble, 0, bytes)
                        asFloat = Array.ConvertAll(asDouble, Function(e) CSng(e))
                        Exit Select
                    Case 32
                        asFloat = New Single(samps - 1) {}
                        Buffer.BlockCopy(byteArray, 0, asFloat, 0, bytes)
                        Exit Select
                    Case 16
                        Dim asInt16 As Int16() = New Int16(samps - 1) {}
                        Buffer.BlockCopy(byteArray, 0, asInt16, 0, bytes)
                        asFloat = Array.ConvertAll(asInt16, Function(e) e / CSng(Int16.MaxValue))
                        Exit Select
                    Case Else
                        Return False
                End Select

                Select Case channels
                    Case 1
                        L = asFloat
                        R = Nothing
                        Return True
                    Case 2
                        samps /= 2
                        L = New Single(samps - 1) {}
                        R = New Single(samps - 1) {}
                        Dim s As Integer = 0
                        For i As Integer = 0 To samps - 1
                            L(i) = asFloat(s)
                            R(i) = asFloat(s + 1)
                            s += 2
                        Next
                        Return True
                    Case Else
                        Return False
                End Select
            End Using
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Return False
        End Try

        Return False
    End Function

    Protected Overrides Sub OnPaintBackground(e As PaintEventArgs)
        ' Do nothing...
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g As Graphics = e.Graphics

        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        Dim pixelsCopy As List(Of Point)
        SyncLock pixels
            pixelsCopy = pixels.ToList()
        End SyncLock

        Dim a As Double
        Dim len As Integer = pixelsCopy.Count - 2

        Dim pA1 As Point
        Dim pA2 As Point
        Dim A12 As Double

        Dim pB1 As Point
        Dim pB2 As Point
        Dim B12 As Double

        For i As Integer = 0 To lastPoints.Count - 1 Step 2
            pA1 = lastPoints(i)
            pB2 = lastPoints(i + 1)
            Using p As New Pen(Color.FromArgb(Math.Max(0, 96 - Distance(pA1, pB2) * distanceFactor), beamColor), 6)
                g.DrawLine(p, pA1, pB2)
            End Using
        Next
        lastPoints.Clear()

        For i As Integer = 0 To len
            pA1 = pixelsCopy(i)
            pA2 = pixelsCopy(i + 1)

            If i = 0 Then
                Using b As New SolidBrush(beamColor)
                    g.FillEllipse(b, New Rectangle(pA1, New Size(2, 2)))
                End Using
            End If

            If pA1 = pA2 Then Continue For

            A12 = Math.Atan2(pA1.Y - pA2.Y, pA1.X - pA2.X) * ToDeg

            ' Skip line segments with the same slope and treat them as a lone single line
            For j = i + 1 To len
                pB1 = pixelsCopy(j)
                pB2 = pixelsCopy(j + 1)
                If pB1 = pB2 Then Continue For

                B12 = Math.Atan2(pB1.Y - pB2.Y, pB1.X - pB2.X) * ToDeg
                If A12 <> B12 Then
                    a = Distance(pA1, pB2) * distanceFactor
                    Using p As New Pen(Color.FromArgb(Math.Max(0, 128 - a), beamColor), 3)
                        g.DrawLine(p, pA1, pB2)
                    End Using
                    Using p As New Pen(Color.FromArgb(Math.Max(8, 255 - a), beamColor), 1)
                        g.DrawLine(p, pA1, pB2)
                    End Using

                    lastPoints.Add(pA1)
                    lastPoints.Add(pB2)

                    i = j
                    Exit For
                End If
            Next
        Next

        Using colorLeft As New Pen(Color.FromArgb(255, Color.SlateBlue), 1)
            Using colorRight As New Pen(Color.FromArgb(255, Color.OrangeRed), 1)
                If renderAudioWaveForm Then RenderWaveForm(g, colorLeft, colorRight)
                If renderAudioFFT Then RenderFFT(g, colorLeft, colorRight)
            End Using
        End Using
    End Sub

    Private Sub RenderFFT(g As Graphics, colorLeft As Pen, colorRight As Pen)
        Dim bufDblL(bufL.Length - 1) As Double
        Dim bufDblR(bufR.Length - 1) As Double

        For i As Integer = 0 To bufL.Length - 1
            bufDblL(i) = bufL(i) * FFT.ApplyWindow(i, fftSize, FFTWindowConstants.Hanning)
            bufDblR(i) = bufR(i) * FFT.ApplyWindow(i, fftSize, FFTWindowConstants.Hanning)
        Next

        FFT.FourierTransform(fftSize, bufDblL, fftL, bufDblR, fftR, False)

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
            For x As Integer = 0 To fftSize2 - 1 Step 100
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

    Private Sub RenderWaveForm(g As Graphics, colorLeft As Pen, colorRight As Pen)
        Dim r As New Rectangle(0, 0, screenWidthHalf / 2, screenHeightHalf / 2)
        r.X = Me.DisplayRectangle.Width - r.Width - 8
        r.Y = Me.DisplayRectangle.Height - r.Height - 8

        Using sb As New SolidBrush(Color.FromArgb(128, 33, 33, 33))
            g.FillRectangle(sb, r)
        End Using

        Dim pts() As Point
        For x As Integer = colorLeft.Width To bufL.Length - 2
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
        Dim y As Integer

        If channel = 1 Then ' Left
            v1 = bufL(x) / maxNormValue * hh
            v2 = bufL(x + 1) / maxNormValue * hh
            y = r.Y + hh / 2
        Else                ' Right
            v1 = bufR(x) / maxNormValue * hh
            v2 = bufR(x + 1) / maxNormValue * hh
            y = r.Bottom - hh / 2
        End If

        ps(0) = New Point(r.X + x / bufL.Length * r.Width, v1 + y)
        ps(1) = New Point(r.X + (x + 1) / bufL.Length * r.Width, v2 + y)

        Return ps
    End Function

    Public Shared Function Distance(p1 As Point, p2 As Point) As Double
        Dim dx As Double = p1.X - p2.X
        Dim dy As Double = p1.Y - p2.Y
        Return Math.Sqrt(dx * dx + dy * dy)
    End Function

    Private Sub FormMain_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        Dim pixelRatio As Double = Me.DisplayRectangle.Width / Me.DisplayRectangle.Height

        screenWidthHalf = Me.DisplayRectangle.Width / 2
        widthFactor = (Me.DisplayRectangle.Width / pixelRatio) / (2 ^ bitDepth / 2)

        screenHeightHalf = Me.DisplayRectangle.Height / 2
        heightFactor = Me.DisplayRectangle.Height / (2 ^ bitDepth / 2)

        Dim diagonal As Double = Math.Sqrt(Me.DisplayRectangle.Width ^ 2 + Me.DisplayRectangle.Height ^ 2)
        Dim w As Integer = Screen.FromControl(Me).Bounds.Width - Me.DisplayRectangle.Width
        Dim h As Integer = Screen.FromControl(Me).Bounds.Height - Me.DisplayRectangle.Height
        Dim m As Integer = Math.Sqrt(Math.Max(w, h))
        distanceFactor = m / diagonal * 256
    End Sub
End Class
