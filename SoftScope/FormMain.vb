Imports System.Threading
Imports System.IO
Imports NAudio.Wave
Imports System.Xml.Linq

Public Class FormMain
    Private Enum AxisAssignments
        Off = 0
        LeftChannel = 1
        RightChannel = 2
        Standard = 3
    End Enum

    Private Const ToDeg As Double = 180.0 / Math.PI

    Private audioSource As WaveIn

    Private screenWidthHalf As Double
    Private widthFactor As Double

    Private screenHeightHalf As Double
    Private heightFactor As Double

    Private screenDiagonal As Double

    Private flipXY As Boolean
    Private flipX As Boolean
    Private flipY As Boolean

    Private pixel As New Pixel()
    Private pixels As New List(Of Pixel)
    Private pixelExactComparer As New PixelComparerExact()
    Private pixelSimilarComparer As New PixelComparerSimilar(4)

    Private sampleRate As Integer
    Private channels As Integer
    Private bitDepth As Integer
    Private dataSize As Integer
    Private Const bufferMilliseconds As Integer = 25
    Private stp As Integer
    Private maxNormValue As Integer
    Private tmpB() As Byte

    Private xAxis As AxisAssignments
    Private xAxisTick As Double
    Private xMspd As Integer ' Milliseconds per division
    Private xAxisDivision As Integer
    Private yAxis As AxisAssignments

    'Private filterL As New FilterButterworth(11000, sampleRate, FilterButterworth.PassType.Lowpass, Math.Sqrt(2))
    'Private filterR As New FilterButterworth(11000, sampleRate, FilterButterworth.PassType.Lowpass, Math.Sqrt(2))

    Private rayColor As Color
    Private rayGlowColor As Color
    Private rayAfterGlowColor As Color

    Private bufL() As Integer
    Private bufR() As Integer

    Private fftSize As FFTSizeConstants = FFTSizeConstants.FFTs2048
    Private fftSize2 As Integer = fftSize / 2
    Private fftL() As ComplexDouble
    Private fftR() As ComplexDouble
    Private fftHistSize As Integer = 4
    Private fftLHist(fftHistSize - 1)() As Double
    Private fftRHist(fftHistSize - 1)() As Double
    Private fftWavInBufL(fftSize - 1) As Double
    Private fftWavInBufR(fftSize - 1) As Double
    Private fftWavInIndex As Integer
    Private bufIndex As Integer
    Private fftWindowValues() As Double
    Private fftWindowSum As Double

    Private leftChannelColor As Color = Color.DarkSlateBlue
    Private rightChannelColor As Color = Color.OrangeRed

    Private renderAudioWaveForm As Boolean = False
    Private renderAudioFFT As Boolean = False

    Private lastPoints As New List(Of Pixel)

    Private abortThreads As Boolean

    Private mainWindowBounds As Rectangle

    Private Sub FormMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        SaveSettings()

        abortThreads = True

        StopAudioDevice()
    End Sub

    Private Sub FormMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.UserPaint, True)
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)

        LabelVersion.Text = My.Application.Info.Version.ToString()

        SetupEventHandlers()
        SetupComboBoxes()

        For i As Integer = 0 To fftHistSize - 1
            ReDim fftLHist(i)(fftSize2 - 1)
            ReDim fftRHist(i)(fftSize2 - 1)
        Next

        Dim renderer As New Thread(Sub()
                                       Do
                                           Thread.Sleep(1000 / 30)
                                           Me.Invalidate()
                                       Loop Until abortThreads
                                   End Sub)
        renderer.IsBackground = True
        renderer.Start()

        LoadSettings()
    End Sub

    Private Sub SetWindowParams()
        If Me.WindowState <> FormWindowState.Minimized Then
            Dim pixelRatio As Double = Me.DisplayRectangle.Width / Me.DisplayRectangle.Height

            screenWidthHalf = Me.DisplayRectangle.Width / 2
            widthFactor = (Me.DisplayRectangle.Width / pixelRatio) / (2 ^ bitDepth / 2)

            screenHeightHalf = Me.DisplayRectangle.Height / 2
            heightFactor = Me.DisplayRectangle.Height / (2 ^ bitDepth / 2)

            screenDiagonal = Math.Sqrt(Me.DisplayRectangle.Width ^ 2 + Me.DisplayRectangle.Height ^ 2)

            xAxisDivision = Me.DisplayRectangle.Width / 10
        End If
    End Sub

    Private Sub StopAudioDevice()
        If audioSource IsNot Nothing Then
            audioSource.StopRecording()
            audioSource.Dispose()
        End If
    End Sub

    Private Sub InitAudioSource()
        StopAudioDevice()

        audioSource = New WaveIn()
        audioSource.DeviceNumber = ComboBoxAudioDevices.SelectedIndex
        Dim wc As WaveInCapabilities = WaveIn.GetCapabilities(audioSource.DeviceNumber)
        LabelAudioSource.MaximumSize = New Size(PanelOptions.Width - LabelAudioSource.Left - 4, LabelAudioSource.Height)
        LabelAudioSource.Text = wc.ProductName

        Dim bestFormat As SupportedWaveFormat = SupportedWaveFormat.WAVE_FORMAT_1M08
        Dim names() As String = [Enum].GetNames(GetType(SupportedWaveFormat))
        Dim values() As Integer = [Enum].GetValues(GetType(SupportedWaveFormat))
        For i As Integer = 0 To values.Length - 1
            If wc.SupportsWaveFormat(values(i)) AndAlso values(i) > bestFormat Then
                bestFormat = [Enum].Parse(GetType(SupportedWaveFormat), names(i))
            End If
        Next

        Dim f As String = bestFormat.ToString().Split("_").Last()
        Dim v As String = ""
        Dim token As Integer = 1
        For i As Integer = 0 To f.Length - 1
            Select Case token
                Case 1 ' Sample Rate
                    If Char.IsDigit(f(i)) Then
                        v += f(i)
                    Else
                        Select Case v
                            Case "1" : sampleRate = 11025
                            Case "2" : sampleRate = 22050
                            Case "4" : sampleRate = 44100
                            Case "48" : sampleRate = 48000
                            Case "96" : sampleRate = 96000
                        End Select
                        i -= 1
                        token += 1
                    End If
                Case 2 ' Channels
                    If f(i) = "M" Then
                        channels = 1
                    Else
                        channels = 2
                    End If
                    token += 1
                Case 3
                    If f(i) = "0" Then
                        bitDepth = 8
                    Else
                        bitDepth = 16
                    End If
            End Select
        Next
        LabelAudioFormat.Text = $"{sampleRate / 1000:F3} KHz {If(channels = "1", "Mono", "Stereo")} {bitDepth} bits"
        LabelAudioFormat.ForeColor = If(channels = 1, Color.OrangeRed, Me.ForeColor)

        dataSize = bitDepth / 8
        stp = channels * dataSize
        maxNormValue = 2 ^ bitDepth

        ReDim tmpB(dataSize + (dataSize Mod 2) - 1)
        ReDim bufL(sampleRate * bufferMilliseconds / 1000 - 1)
        ReDim bufR(sampleRate * bufferMilliseconds / 1000 - 1)

        ReDim fftL(fftSize - 1)
        ReDim fftR(fftSize - 1)
        For i As Integer = 0 To fftSize - 1
            fftL(i) = New ComplexDouble()
            fftR(i) = New ComplexDouble()
        Next
        fftWindowValues = FFT.GetWindowValues(fftSize, FFTWindowConstants.Hanning)
        fftWindowSum = FFT.GetWindowSum(fftSize, FFTWindowConstants.Hanning)
        fftWavInIndex = 0
        bufIndex = 0

        audioSource.WaveFormat = New WaveFormat(sampleRate, bitDepth, channels)
        audioSource.BufferMilliseconds = bufferMilliseconds
        AddHandler audioSource.DataAvailable, AddressOf ProcessAudio
        audioSource.StartRecording()

        SetWindowParams()
        ComboBoxAudioDevices.Visible = False
    End Sub

    Private Sub SetupComboBoxes()
        For i As Integer = 0 To WaveIn.DeviceCount - 1
            ComboBoxAudioDevices.Items.Add(WaveIn.GetCapabilities(i).ProductName)
        Next

        For Each v In [Enum].GetValues(GetType(AxisAssignments))
            ComboBoxXAxis.Items.Add(v)
            If v <> AxisAssignments.Standard Then ComboBoxYAxis.Items.Add(v)
        Next
    End Sub

    Private Sub SetupEventHandlers()
        AddHandler Me.ResizeBegin, Sub() If Me.WindowState = FormWindowState.Normal Then mainWindowBounds = Me.Bounds
        AddHandler Me.ResizeEnd, Sub() If Me.WindowState = FormWindowState.Normal Then mainWindowBounds = Me.Bounds
        AddHandler Me.Resize, Sub() SetWindowParams()

        AddHandler PanelOptions.MouseLeave, Sub() PanelOptions.Visible = False
        AddHandler Me.MouseMove, Sub(s As Object, e1 As MouseEventArgs)
                                     If e1.X <= PanelOptions.Right Then
                                         PanelOptions.Visible = True
                                     End If
                                 End Sub

        AddHandler LabelAudioSource.MouseEnter, Sub() ComboBoxAudioDevices.Visible = True
        AddHandler ComboBoxAudioDevices.MouseLeave, Sub() ComboBoxAudioDevices.Visible = False
        AddHandler ComboBoxAudioDevices.SelectedIndexChanged, Sub() InitAudioSource()

        AddHandler LabelXAxis.MouseEnter, Sub() ComboBoxXAxis.Visible = True
        AddHandler ComboBoxXAxis.MouseLeave, Sub() ComboBoxXAxis.Visible = False
        AddHandler ComboBoxXAxis.SelectedIndexChanged, Sub()
                                                           xAxis = CType(ComboBoxXAxis.SelectedItem, AxisAssignments)
                                                           LabelXAxis.Text = xAxis.ToString()
                                                           ComboBoxXAxis.Visible = False
                                                       End Sub
        AddHandler TrackBarMsPerDiv.ValueChanged, Sub()
                                                      xMspd = TrackBarMsPerDiv.Value
                                                      LabelMsPerDiv.Text = TrackBarMsPerDiv.Value.ToString()
                                                  End Sub

        AddHandler LabelYAxis.MouseEnter, Sub() ComboBoxYAxis.Visible = True
        AddHandler ComboBoxYAxis.MouseLeave, Sub() ComboBoxYAxis.Visible = False
        AddHandler ComboBoxYAxis.SelectedIndexChanged, Sub()
                                                           yAxis = CType(ComboBoxYAxis.SelectedItem, AxisAssignments)
                                                           LabelYAxis.Text = yAxis.ToString()
                                                           ComboBoxYAxis.Visible = False
                                                       End Sub

        AddHandler CheckBoxFlipX.CheckedChanged, Sub() flipX = CheckBoxFlipX.Checked
        AddHandler CheckBoxFlipY.CheckedChanged, Sub() flipY = CheckBoxFlipY.Checked
        AddHandler CheckBoxFlipXY.CheckedChanged, Sub() flipXY = CheckBoxFlipXY.Checked

        AddHandler CheckBoxWaveForm.CheckedChanged, Sub() renderAudioWaveForm = CheckBoxWaveForm.Checked
        AddHandler CheckBoxFFT.CheckedChanged, Sub() renderAudioFFT = CheckBoxFFT.Checked

        Dim ChangeColor = Sub(p As Panel)
                              Using cDlg As New ColorDialog()
                                  cDlg.Color = p.BackColor
                                  cDlg.FullOpen = True
                                  If cDlg.ShowDialog(Me) = DialogResult.OK Then p.BackColor = cDlg.Color
                              End Using
                          End Sub

        AddHandler PanelBackColor.Click, Sub()
                                             ChangeColor(PanelBackColor)
                                             Me.BackColor = PanelBackColor.BackColor
                                         End Sub

        AddHandler PanelRayColor.Click, Sub()
                                            ChangeColor(PanelRayColor)
                                            SetRayColors()
                                        End Sub

        AddHandler PanelLeftChannel.Click, Sub()
                                               ChangeColor(PanelLeftChannel)
                                               leftChannelColor = PanelLeftChannel.BackColor
                                           End Sub

        AddHandler PanelRightChannel.Click, Sub()
                                                ChangeColor(PanelRightChannel)
                                                rightChannelColor = PanelRightChannel.BackColor
                                            End Sub
    End Sub

    Private Sub ProcessAudio(sender As Object, e As WaveInEventArgs)
        Dim tmp As Integer
        Dim i As Integer

        bufIndex = 0

        SyncLock pixels
            pixels.Clear()

            For i = i To e.Buffer.Length - 1 Step stp
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

                Select Case xAxis
                    Case AxisAssignments.Off : pixel.X = 0
                    Case AxisAssignments.LeftChannel : pixel.X = bufL(i / stp) * widthFactor
                    Case AxisAssignments.RightChannel : pixel.X = -bufR(i / stp) * heightFactor
                    Case AxisAssignments.Standard
                        pixel.X += xAxisDivision / xMspd
                        pixel.X = pixel.X Mod Me.DisplayRectangle.Width
                End Select
                Select Case yAxis
                    Case AxisAssignments.Off : pixel.Y = 0
                    Case AxisAssignments.LeftChannel : pixel.Y = bufL(i / stp) * widthFactor
                    Case AxisAssignments.RightChannel : pixel.Y = -bufR(i / stp) * heightFactor
                End Select

                If flipX Then pixel.X = -pixel.X
                If flipY Then pixel.Y = -pixel.Y

                If flipXY Then
                    tmp = pixel.X
                    pixel.X = pixel.Y
                    pixel.Y = tmp
                End If

                If xAxis <> AxisAssignments.Standard Then pixel.X += screenWidthHalf
                pixel.Y += screenHeightHalf

                If pixels.BinarySearch(pixel, pixelSimilarComparer) < 0 Then pixels.Add(pixel)
            Next

            If renderAudioFFT Then
                Do
                    FillFFTBuffer()
                Loop Until fftWavInIndex = 0 OrElse bufIndex = 0
            End If
        End SyncLock
    End Sub

    Protected Overrides Sub OnPaintBackground(e As PaintEventArgs)
        ' Do nothing...
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g As Graphics = e.Graphics

        Dim pixelsCopy As List(Of Pixel)
        SyncLock pixels
            pixelsCopy = pixels.ToList()
        End SyncLock

        Dim len As Integer = pixelsCopy.Count - 2
        Dim i As Integer
        'Dim j As Integer

        Dim pA1 As Pixel
        Dim pA2 As Pixel
        Dim A12 As Double

        'Dim pB1 As Pixel
        'Dim pB2 As Pixel
        'Dim B12 As Double

        'g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        'For i = 0 To lastPoints.Count - 2 Step 1
        '    RenderLine(g, lastPoints(i), lastPoints(i + 1))
        'Next

        If pixels.Count = 1 Then
            Using b As New SolidBrush(rayColor)
                g.FillEllipse(b, New Rectangle(pixels(0), New Size(2, 2)))
            End Using
        End If

        For i = 0 To len
            pA1 = pixelsCopy(i)
            pA2 = pixelsCopy(i + 1)

            If pA1 = pA2 AndAlso i < len Then Continue For

            A12 = Math.Atan2(pA1.Y - pA2.Y, pA1.X - pA2.X) * ToDeg Mod 180.0
            If A12 < 0 Then A12 = 360.0 - A12

            ' Unfortunately, this optimization causes too many issues
            '' Skip line segments with the same slope and treat them as a single line
            'For j = i + 1 To len
            '    pB1 = pixelsCopy(j)
            '    pB2 = pixelsCopy(j + 1)
            '    If pB1 = pB2 Then Continue For

            '    B12 = Math.Atan2(pB1.Y - pB2.Y, pB1.X - pB2.X) * ToDeg Mod 180.0
            '    If B12 < 0 Then B12 = 360.0 - B12

            '    If A12 <> B12 Then
            '        RenderLine(g, pA1, pB2)
            '        Exit For
            '    End If
            'Next
            'If j > len Then RenderLine(g, pA1, pB2)
            RenderLine(g, pA1, pA2)
        Next

        Using colorLeft As New Pen(leftChannelColor, 1)
            Using colorRight As New Pen(rightChannelColor, 1)
                If renderAudioWaveForm Then RenderWaveForm(g, colorLeft, colorRight)
                If renderAudioFFT Then RenderFFT(g, colorLeft, colorRight)
            End Using
        End Using

        lastPoints.Clear()
    End Sub

    Private Sub RenderLine(g As Graphics, p1 As Pixel, p2 As Pixel)
        Dim pixelIndex As Integer = lastPoints.BinarySearch(p1, pixelSimilarComparer)
        If pixelIndex < 0 Then
            p1.Alpha = 128
        Else
            p1.Alpha = lastPoints(pixelIndex).Alpha + 96
        End If
        p1.Alpha -= 30 * Distance(p1, p2) / screenDiagonal * 255
        p2.Alpha = p1.Alpha

        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        Using p As New Pen(Color.FromArgb(p1.Alpha / 1.5, rayGlowColor), 6)
            g.DrawLine(p, p1, p2)
        End Using

        g.SmoothingMode = Drawing2D.SmoothingMode.None
        Using p As New Pen(Color.FromArgb(p1.Alpha, rayColor), 1)
            g.DrawLine(p, p1, p2)
        End Using

        lastPoints.Add(p1)
        lastPoints.Add(p2)
    End Sub

    Private Function Distance(p1 As Pixel, p2 As Pixel) As Double
        Dim dx As Double = p1.X - p2.X
        Dim dy As Double = p1.Y - p2.Y
        Return Math.Sqrt(dx * dx + dy * dy)
    End Function

    Private Sub RenderFFT(g As Graphics, colorLeft As Pen, colorRight As Pen)
        If fftWavInIndex = 0 Then FFT.FourierTransform(fftSize, fftWavInBufL, fftL, fftWavInBufR, fftR, False)

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

    Private Sub FillFFTBuffer()
        Do
            If bufIndex >= bufL.Length Then
                If fftWavInIndex >= fftSize Then fftWavInIndex = 0
                bufIndex = 0
                Exit Do
            ElseIf fftWavInIndex >= fftSize Then
                fftWavInIndex = 0
                Exit Do
            End If

            fftWavInBufL(fftWavInIndex) = bufL(bufIndex) * fftWindowValues(fftWavInIndex)
            fftWavInBufR(fftWavInIndex) = bufR(bufIndex) * fftWindowValues(fftWavInIndex)

            fftWavInIndex += 1
            bufIndex += 1
        Loop
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

    Private Sub SaveSettings()
        Dim xml As XElement = <settings>
                                  <mainWindow>
                                      <left><%= mainWindowBounds.Left %></left>
                                      <top><%= mainWindowBounds.Top %></top>
                                      <width><%= mainWindowBounds.Width %></width>
                                      <height><%= mainWindowBounds.Height %></height>
                                      <state><%= Me.WindowState %></state>
                                  </mainWindow>
                                  <processOptions>
                                      <flipX><%= flipX %></flipX>
                                      <flipY><%= flipY %></flipY>
                                      <flipXY><%= flipXY %></flipXY>
                                  </processOptions>
                                  <apperance>
                                      <backColor><%= PanelBackColor.BackColor.ToArgb() %></backColor>
                                      <rayColor><%= PanelRayColor.BackColor.ToArgb() %></rayColor>
                                      <leftChannelColor><%= PanelLeftChannel.BackColor.ToArgb() %></leftChannelColor>
                                      <rightChannelColor><%= PanelRightChannel.BackColor.ToArgb() %></rightChannelColor>
                                  </apperance>
                                  <audioSource>
                                      <deviceIndex><%= ComboBoxAudioDevices.SelectedIndex %></deviceIndex>
                                  </audioSource>
                                  <axisAssignments>
                                      <x>
                                          <assignment><%= xAxis %></assignment>
                                          <xMspd><%= xMspd %></xMspd>
                                      </x>
                                      <y>
                                          <assignment><%= yAxis %></assignment>
                                      </y>
                                  </axisAssignments>
                              </settings>

        xml.Save(SettingsFile)
    End Sub

    Private Sub LoadSettings()
        ' Setup defaults in case some parsing fails -----------
        PanelBackColor.BackColor = Color.Black
        PanelRayColor.BackColor = Color.FromArgb(255, 90, 255, 90)
        PanelLeftChannel.BackColor = Color.SlateBlue
        PanelRightChannel.BackColor = Color.OrangeRed

        xAxis = AxisAssignments.LeftChannel
        yAxis = AxisAssignments.RightChannel
        xMspd = 100 ' 100ms per division
        ' -----------------------------------------------------

        If File.Exists(SettingsFile) Then
            Dim xml As XElement = XDocument.Load(SettingsFile).<settings>(0)

            Integer.TryParse(xml.<mainWindow>.<left>.Value, Me.Left)
            Integer.TryParse(xml.<mainWindow>.<top>.Value, Me.Top)
            Integer.TryParse(xml.<mainWindow>.<width>.Value, Me.Width)
            Integer.TryParse(xml.<mainWindow>.<height>.Value, Me.Height)
            mainWindowBounds = Me.Bounds
            [Enum].TryParse(Of FormWindowState)(xml.<mainWindow>.<state>.Value, Me.WindowState)

            Boolean.TryParse(xml.<processOptions>.<flipX>.Value, flipX)
            Boolean.TryParse(xml.<processOptions>.<flipY>.Value, flipY)
            Boolean.TryParse(xml.<processOptions>.<flipXY>.Value, flipXY)

            Dim SetPanelColor = Sub(panel As Panel, value As String)
                                    Dim argb As Integer
                                    If Integer.TryParse(value, argb) Then panel.BackColor = Color.FromArgb(argb)
                                End Sub

            SetPanelColor(PanelBackColor, xml.<apperance>.<backColor>.Value)
            SetPanelColor(PanelRayColor, xml.<apperance>.<rayColor>.Value)
            SetPanelColor(PanelLeftChannel, xml.<apperance>.<leftChannelColor>.Value)
            SetPanelColor(PanelRightChannel, xml.<apperance>.<rightChannelColor>.Value)

            Integer.TryParse(xml.<audioSource>.<deviceIndex>.Value, ComboBoxAudioDevices.SelectedIndex)

            Dim axis As AxisAssignments
            If [Enum].TryParse(Of AxisAssignments)(xml.<axisAssignments>.<x>.<assignment>.Value, axis) Then xAxis = axis
            If [Enum].TryParse(Of AxisAssignments)(xml.<axisAssignments>.<y>.<assignment>.Value, axis) Then yAxis = axis
        Else
            ComboBoxAudioDevices.SelectedIndex = 0

            Me.Size = New Size(1024, 768)
            Me.Location = New Point((Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2)
        End If

        ComboBoxXAxis.SelectedItem = xAxis
        ComboBoxYAxis.SelectedItem = yAxis
        TrackBarMsPerDiv.Value = xMspd
        LabelMsPerDiv.Text = xMspd.ToString()

        SetRayColors()
    End Sub

    Private Sub SetRayColors()
        rayColor = PanelRayColor.BackColor

        Dim hls As New HLSRGB(rayColor)
        hls.DarkenColor(0.3)
        rayGlowColor = hls.Color
        hls.DarkenColor(0.5)
        rayAfterGlowColor = hls.Color
    End Sub

    Private ReadOnly Property SettingsFile As String
        Get
            Dim fp As New IO.DirectoryInfo(My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData)
            Return IO.Path.Combine(fp.Parent.FullName, "settings.dat")
        End Get
    End Property
End Class