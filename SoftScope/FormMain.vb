Imports System.Threading
Imports System.IO
Imports NAudio.Wave
Imports System.Xml.Linq

' <div>Icons made by <a href="http://www.flaticon.com/authors/madebyoliver" title="Madebyoliver">Madebyoliver</a> from <a href="http://www.flaticon.com" title="Flaticon">www.flaticon.com</a> is licensed by <a href="http://creativecommons.org/licenses/by/3.0/" title="Creative Commons BY 3.0" target="_blank">CC 3.0 BY</a></div>

Public Class FormMain
    Private Const ToRad As Double = Math.PI / 180.0
    Private Const ToDeg As Double = 180.0 / Math.PI

    Private Enum AxisAssignments
        Off = 0
        LeftChannel = 1
        RightChannel = 2
        Standard = 3
    End Enum

    Private audioSource As WaveIn
    Private audioOut As WaveOut
    Private waveReader As WaveFileReader

    Private screenWidthHalf As Double
    Private widthFactor As Double

    Private screenHeightHalf As Double
    Private heightFactor As Double

    Private screenDiagonal As Double

    Private flipXY As Boolean
    Private flipX As Boolean
    Private flipY As Boolean

    Private pixel As Pixel
    Private pixels As New List(Of Pixel)
    Private pixelsCopy As New List(Of Pixel)
    Private lastPixels As New List(Of Pixel)
    Private pixelSimilarComparer As New PixelComparerSimilar(4)

    Private sampleRate As Integer
    Private channels As Integer
    Private bitDepth As Integer
    Private dataSize As Integer
    Private bufferMilliseconds As Integer
    Private stp As Integer
    Private maxNormValue As Integer
    Private tmpB() As Byte

    Private xAxis As AxisAssignments
    Private xAxisTick As Double
    Private xMspd As Integer ' Milliseconds per division for xAxis = Standard
    Private xAxisDivision As Integer
    Private yAxis As AxisAssignments
    Private drawGrid As Boolean
    Private gridColor As Color

    'Private filterL As New FilterButterworth(11000, sampleRate, FilterButterworth.PassType.Lowpass, Math.Sqrt(2))
    'Private filterR As New FilterButterworth(11000, sampleRate, FilterButterworth.PassType.Lowpass, Math.Sqrt(2))

    Private rayColor As Color
    Private rayGlowColor As Color
    Private rayAfterGlowColor As Color
    Private rayBrightness As Double  ' FIXME: This should be user customizable
    Private gain As Double = 1.0 ' FIXME: This should be user customizable

    Private bufL() As Integer
    Private bufR() As Integer
    Private bufferLength As Integer

    Private leftChannelColor As Color = Color.DarkSlateBlue
    Private rightChannelColor As Color = Color.OrangeRed

    Private renderAudioWaveForm As Boolean = False
    Private renderAudioFFT As Boolean = False

    Private abortThreads As Boolean

    Private mainWindowBounds As Rectangle

    Private Sub FormMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.UserPaint, True)
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)

        LabelVersion.Text = My.Application.Info.Version.ToString()

        SetupEventHandlers()
        SetupComboBoxes()

        If ComboBoxAudioDevices.Items.Count = 0 Then
            MsgBox("No Input audio devices found", MsgBoxStyle.Exclamation)
            Application.Exit()
        Else
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
        End If

        AddHandler Me.FormClosing, Sub()
                                       abortThreads = True

                                       StopAudioDevice()
                                       SaveSettings()

                                       Application.DoEvents()
                                   End Sub

        'PlayFromFile("youscope.wav")
        'PlayFromFile("kickstarter192khz.wav")
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
            audioSource = Nothing
        End If

        If audioOut IsNot Nothing Then
            audioOut.Stop()
            audioOut.Dispose()
            audioOut = Nothing
        End If

        If waveReader IsNot Nothing Then waveReader.Dispose()

        SimpleTrackBarPlayProgress.ReadOnly = True
    End Sub

    Private Sub InitAudioSource()
        If ComboBoxAudioDevices.SelectedIndex = -1 Then Exit Sub

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

        SetupBuffers()

        audioSource.WaveFormat = New WaveFormat(sampleRate, bitDepth, channels)
        audioSource.BufferMilliseconds = bufferMilliseconds
        AddHandler audioSource.DataAvailable, AddressOf ProcessAudio
        audioSource.StartRecording()

        SetWindowParams()
        ComboBoxAudioDevices.Visible = False
    End Sub

    Private Sub SetupBuffers()
        dataSize = bitDepth / 8
        stp = channels * dataSize
        maxNormValue = 2 ^ bitDepth

        bufferMilliseconds = 25 ' Math.Max(20, (sampleRate / 2) / 1000)

        ReDim tmpB(dataSize + (dataSize Mod 2) - 1)

        bufferLength = sampleRate * bufferMilliseconds / 1000
        ReDim bufL(bufferLength - 1)
        ReDim bufR(bufferLength - 1)

        ReDim fftL(fftSize - 1)
        ReDim fftR(fftSize - 1)
        For i As Integer = 0 To fftSize - 1
            fftL(i) = New ComplexDouble()
            fftR(i) = New ComplexDouble()
        Next
        fftWindowValues = FFT.GetWindowValues(fftSize, FFTWindowConstants.Hanning)
        fftWindowSum = FFT.GetWindowSum(fftSize, FFTWindowConstants.Hanning)
        fftWavDstIndex = 0
        ffWavSrcBufIndex = 0
    End Sub

    Private Sub SetupComboBoxes()
        For i As Integer = 0 To WaveIn.DeviceCount - 1
            ComboBoxAudioDevices.Items.Add(WaveIn.GetCapabilities(i).ProductName)
        Next
        If ComboBoxAudioDevices.Items.Count = 0 Then
            LabelAudioSource.Text = "No Devices Available"
            LabelAudioSource.ForeColor = Color.DimGray
            LabelAudioFormat.Visible = False
        End If

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

        AddHandler LabelAudioSource.MouseEnter, Sub() If audioOut Is Nothing AndAlso ComboBoxAudioDevices.Items.Count > 0 Then ComboBoxAudioDevices.Visible = True
        AddHandler ComboBoxAudioDevices.MouseLeave, Sub() ComboBoxAudioDevices.Visible = False
        AddHandler ComboBoxAudioDevices.SelectedIndexChanged, Sub() InitAudioSource()

        AddHandler LabelXAxis.MouseEnter, Sub() ComboBoxXAxis.Visible = True
        AddHandler ComboBoxXAxis.MouseLeave, Sub() ComboBoxXAxis.Visible = False
        AddHandler ComboBoxXAxis.SelectedIndexChanged, Sub()
                                                           xAxis = CType(ComboBoxXAxis.SelectedItem, AxisAssignments)
                                                           LabelXAxis.Text = xAxis.ToString()
                                                           ComboBoxXAxis.Visible = False
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

        ' FIXME: Cheap trick to "fix" panel rendering issues, likely caused by the AllPaintingInWmPaint flag
        AddHandler PanelOptions.MouseMove, Sub() If PanelOptions.Visible Then PanelOptions.Refresh()

        AddHandler ButtonPlayFile.Click, Sub()
                                             If audioOut IsNot Nothing Then
                                                 StopAudioDevice()
                                                 InitAudioSource()
                                             Else
                                                 Using dlg As New OpenFileDialog()
                                                     dlg.Filter = "WAV Files|*.wav"
                                                     If dlg.ShowDialog(Me) = DialogResult.OK Then
                                                         PanelOptions.Visible = False
                                                         PlayFromFile(dlg.FileName)
                                                     End If
                                                 End Using
                                             End If
                                         End Sub

        AddHandler SimpleTrackBarPlayProgress.ValueChangedFromMouseClick, Sub() If audioOut IsNot Nothing Then waveReader.Position = waveReader.Length * SimpleTrackBarPlayProgress.Value / SimpleTrackBarPlayProgress.Max
    End Sub

    Private Sub ProcessAudio(sender As Object, e As WaveInEventArgs)
        Dim i As Integer
        Dim bufferIndex As Integer

        ffWavSrcBufIndex = 0

        SyncLock pixels
            pixels.Clear()

            For i = i To e.Buffer.Length - 1 Step stp
                bufferIndex = i / stp
                Select Case bitDepth
                    Case 8
                        bufL(bufferIndex) = (128 - e.Buffer(i)) * gain
                        bufR(bufferIndex) = (128 - e.Buffer(i + dataSize)) * gain
                    Case 16
                        Array.Copy(e.Buffer, i, tmpB, 0, dataSize)
                        bufL(bufferIndex) = BitConverter.ToInt16(tmpB, 0) * gain

                        Array.Copy(e.Buffer, i + dataSize, tmpB, 0, dataSize)
                        bufR(bufferIndex) = BitConverter.ToInt16(tmpB, 0) * gain
                    Case 24
                        Array.Copy(e.Buffer, i, tmpB, 1, dataSize)
                        bufL(bufferIndex) = BitConverter.ToInt32(tmpB, 0) * gain

                        Array.Copy(e.Buffer, i + dataSize, tmpB, 1, dataSize)
                        bufR(bufferIndex) = BitConverter.ToInt32(tmpB, 0) * gain
                End Select

                ProcessBuffer(bufferIndex)
            Next

            If renderAudioFFT Then FillFFTBuffer()
        End SyncLock
    End Sub

    Private Sub ProcessBuffer(bufferIndex As Integer)
        Dim tmp As Integer

        'filterL.Update(normL)
        'normL = filterL.Value

        'filterR.Update(normR)
        'normR = filterR.Value

        Select Case xAxis
            Case AxisAssignments.Off : pixel.X = 0
            Case AxisAssignments.LeftChannel : pixel.X = bufL(bufferIndex) * widthFactor
            Case AxisAssignments.RightChannel : pixel.X = -bufR(bufferIndex) * heightFactor
            Case AxisAssignments.Standard
                pixel.X += xAxisDivision / xMspd
                pixel.X = pixel.X Mod Me.DisplayRectangle.Width
        End Select
        Select Case yAxis
            Case AxisAssignments.Off : pixel.Y = 0
            Case AxisAssignments.LeftChannel : pixel.Y = bufL(bufferIndex) * widthFactor
            Case AxisAssignments.RightChannel : pixel.Y = -bufR(bufferIndex) * heightFactor
        End Select

        ' FIXME: The FlipX and FlipXY options don't work correctly when X axis is set to "Standard"
        If flipX Then pixel.X = -pixel.X
        If flipY Then pixel.Y = -pixel.Y

        If flipXY Then
            tmp = pixel.X
            pixel.X = pixel.Y
            pixel.Y = tmp
        End If

        If xAxis <> AxisAssignments.Standard Then pixel.X += screenWidthHalf
        pixel.Y += screenHeightHalf

        Dim index As Integer = pixels.BinarySearch(pixel, pixelSimilarComparer)
        If index < 0 Then
            pixels.Add(pixel.Clone())
        Else
            pixels(index).Alpha += 96
        End If
    End Sub

    Protected Overrides Sub OnPaintBackground(e As PaintEventArgs)
        e.Graphics.Clear(PanelBackColor.BackColor)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g As Graphics = e.Graphics
        'Dim i As Integer

        '' Render after glow
        'g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        'For i = 0 To pixelsCopy.Count - 2
        '    RenderLine(g, pixelsCopy(i), pixelsCopy(i + 1), 0.2, 12.0, 0.0)
        'Next

        SyncLock pixels
            pixelsCopy = pixels.ToList()
        End SyncLock

        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        If drawGrid Then RenderGrid(g)

        ' FIXME: There should be a better solution to draw the ray when it's not moving
        If pixels.Count = 1 Then
            Using b As New SolidBrush(rayColor)
                g.FillEllipse(b, New Rectangle(pixels(0), New Size(2, 2)))
            End Using
        Else
            For i = 0 To pixelsCopy.Count - 2
                RenderLine(g, pixelsCopy(i), pixelsCopy(i + 1))
            Next
        End If

        Using colorLeft As New Pen(leftChannelColor, 1)
            Using colorRight As New Pen(rightChannelColor, 1)
                If renderAudioWaveForm Then RenderWaveForm(g, colorLeft, colorRight)
                If renderAudioFFT Then RenderFFT(g, colorLeft, colorRight)
            End Using
        End Using

        lastPixels.Clear()
    End Sub

    Private Sub RenderGrid(g As Graphics)
        Using p As New Pen(gridColor)
            If xAxis = AxisAssignments.Standard Then
                For x As Integer = 0 To Me.DisplayRectangle.Width Step Me.DisplayRectangle.Width / 10
                    g.DrawLine(p, x, 0, x, Me.DisplayRectangle.Height)
                Next

                For y As Integer = 0 To Me.DisplayRectangle.Height Step Me.DisplayRectangle.Height / 10
                    g.DrawLine(p, 0, y, Me.DisplayRectangle.Width, y)
                Next
            Else
                Dim center As Point = New Point(Me.DisplayRectangle.Width / 2, Me.DisplayRectangle.Height / 2)
                Dim radius As Integer = Math.Min(Me.DisplayRectangle.Width, Me.DisplayRectangle.Height) / 2

                Dim s As Integer = 360 / 24
                For a As Integer = 0 To 360 - s Step s
                    g.DrawLine(p, center, New Point(center.X + radius * Math.Cos(-a * ToRad), center.Y + radius * Math.Sin(a * ToRad)))
                Next
                g.DrawEllipse(p, center.X - radius, center.Y - radius, radius * 2, radius * 2)
            End If
        End Using
    End Sub

    Private Sub RenderLine(g As Graphics, p1 As Pixel, p2 As Pixel, Optional alphaMultiplier As Double = 1.0, Optional glowWidth As Single = 6.0, Optional rayWidth As Single = 1.0)
        'Dim pixelIndex As Integer = lastPixels.BinarySearch(p1, pixelSimilarComparer)
        'If pixelIndex >= 0 Then
        '    p1.Alpha += 32
        'Else
        '    p1.Alpha -= 8
        'End If
        p1.Alpha *= alphaMultiplier
        p1.Alpha -= (1.0 - rayBrightness) * 150 * Distance(p1, p2) / screenDiagonal * 255
        'p2.Alpha = p1.Alpha

        Using p As New Pen(Color.FromArgb(p1.Alpha / 1.5, rayGlowColor), glowWidth)
            g.DrawLine(p, p1, p2)
        End Using

        If rayWidth > 0 Then
            g.SmoothingMode = Drawing2D.SmoothingMode.None
            Using p As New Pen(Color.FromArgb(p1.Alpha, rayColor), rayWidth)
                g.DrawLine(p, p1, p2)
            End Using
        End If

        'lastPixels.Add(p1)
        'lastPixels.Add(p2)
    End Sub

    Private Function Distance(p1 As Pixel, p2 As Pixel) As Double
        Dim dx As Double = p1.X - p2.X
        Dim dy As Double = p1.Y - p2.Y
        Return Math.Sqrt(dx * dx + dy * dy)
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
                                  <panels>
                                      <wave><%= renderAudioWaveForm %></wave>
                                      <fft><%= renderAudioFFT %></fft>
                                  </panels>
                                  <apperance>
                                      <backColor><%= PanelBackColor.BackColor.ToArgb() %></backColor>
                                      <rayColor><%= PanelRayColor.BackColor.ToArgb() %></rayColor>
                                      <leftChannelColor><%= PanelLeftChannel.BackColor.ToArgb() %></leftChannelColor>
                                      <rightChannelColor><%= PanelRightChannel.BackColor.ToArgb() %></rightChannelColor>
                                      <drawGrid><%= drawGrid %></drawGrid>
                                      <rayBrightness><%= rayBrightness %></rayBrightness>
                                      <gain><%= gain %></gain>
                                      <gridColor><%= gridColor.ToArgb() %></gridColor>
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

        rayBrightness = 0.7
        gain = 1.0
        drawGrid = False
        gridColor = Color.FromArgb(128, Color.DimGray)

        sampleRate = 44100
        channels = 2
        bitDepth = 16
        ' -----------------------------------------------------

        If File.Exists(SettingsFile) Then
            Dim i As Integer
            Dim d As Double
            Dim b As Boolean
            Dim axis As AxisAssignments
            Dim ws As FormWindowState

            Dim xml As XElement = XDocument.Load(SettingsFile).<settings>(0)

            If Integer.TryParse(xml.<mainWindow>.<left>.Value, i) Then Me.Left = i
            If Integer.TryParse(xml.<mainWindow>.<top>.Value, i) Then Me.Top = i
            If Integer.TryParse(xml.<mainWindow>.<width>.Value, i) Then Me.Width = i
            If Integer.TryParse(xml.<mainWindow>.<height>.Value, i) Then Me.Height = i
            mainWindowBounds = Me.Bounds
            If [Enum].TryParse(Of FormWindowState)(xml.<mainWindow>.<state>.Value, ws) Then Me.WindowState = ws

            If Boolean.TryParse(xml.<processOptions>.<flipX>.Value, b) Then CheckBoxFlipX.Checked = b
            If Boolean.TryParse(xml.<processOptions>.<flipY>.Value, b) Then CheckBoxFlipY.Checked = b
            If Boolean.TryParse(xml.<processOptions>.<flipXY>.Value, b) Then CheckBoxFlipXY.Checked = b

            Dim SetPanelColor = Sub(panel As Panel, value As String)
                                    Dim argb As Integer
                                    If Integer.TryParse(value, argb) Then panel.BackColor = Color.FromArgb(argb)
                                End Sub

            SetPanelColor(PanelBackColor, xml.<apperance>.<backColor>.Value)
            SetPanelColor(PanelRayColor, xml.<apperance>.<rayColor>.Value)
            SetPanelColor(PanelLeftChannel, xml.<apperance>.<leftChannelColor>.Value)
            SetPanelColor(PanelRightChannel, xml.<apperance>.<rightChannelColor>.Value)
            Boolean.TryParse(xml.<apperance>.<drawGrid>.Value, drawGrid)
            If Double.TryParse(xml.<apperance>.<rayBrightness>.Value, d) Then rayBrightness = d
            If Double.TryParse(xml.<apperance>.<gain>.Value, d) Then gain = d
            If Integer.TryParse(xml.<apperance>.<gridColor>.Value, i) Then gridColor = Color.FromArgb(i)

            If Integer.TryParse(xml.<audioSource>.<deviceIndex>.Value, i) AndAlso i >= 0 AndAlso i < ComboBoxAudioDevices.Items.Count Then
                ComboBoxAudioDevices.SelectedIndex = i
            Else
                If ComboBoxAudioDevices.Items.Count > 0 Then
                    ComboBoxAudioDevices.SelectedIndex = 0
                Else
                    SetupBuffers()
                End If
            End If

            If [Enum].TryParse(Of AxisAssignments)(xml.<axisAssignments>.<x>.<assignment>.Value, axis) Then xAxis = axis
            If [Enum].TryParse(Of AxisAssignments)(xml.<axisAssignments>.<y>.<assignment>.Value, axis) Then yAxis = axis

            If Boolean.TryParse(xml.<panels>.<wave>.Value, b) Then CheckBoxWaveForm.Checked = b
            If Boolean.TryParse(xml.<panels>.<fft>.Value, b) Then CheckBoxFFT.Checked = b
        Else
            ComboBoxAudioDevices.SelectedIndex = 0

            Me.Size = New Size(1024, 768)
            Me.Location = New Point((Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2)
        End If

        ComboBoxXAxis.SelectedItem = xAxis
        ComboBoxYAxis.SelectedItem = yAxis
        LabelMsPerDiv.Text = xMspd.ToString()
        SimpleTrackBarPlayProgress.Value = 0
        SimpleTrackBarPlayProgress.ReadOnly = True

        SetRayColors()
    End Sub

    Private Sub SetRayColors()
        rayColor = PanelRayColor.BackColor

        Dim hls As New HLSRGB(rayColor)
        hls.DarkenColor(0.3)
        rayGlowColor = hls.Color
        hls.DarkenColor(0.5)
        rayAfterGlowColor = hls.Color

        pixel = New Pixel(0, 0, 128)
    End Sub

    Private ReadOnly Property SettingsFile As String
        Get
            Dim fp As New IO.DirectoryInfo(My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData)
            Return IO.Path.Combine(fp.Parent.FullName, "settings.dat")
        End Get
    End Property

    Private Sub PlayFromFile(fileName As String)
        StopAudioDevice()

        waveReader = New WaveFileReader(fileName)
        sampleRate = waveReader.WaveFormat.SampleRate
        channels = waveReader.WaveFormat.Channels
        bitDepth = waveReader.WaveFormat.BitsPerSample

        SetupBuffers()

        Dim lastPosWidth As Integer = 0

        gain = 0.5

        Dim wp As New CustomWaveProvider(waveReader, bufferLength)
        AddHandler wp.DataAvailable, Sub(b() As Byte)
                                         ProcessAudio(Me, New WaveInEventArgs(b, b.Length))
                                         Dim posWidth As Integer = waveReader.Position / waveReader.Length * SimpleTrackBarPlayProgress.Max
                                         If posWidth <> lastPosWidth Then
                                             lastPosWidth = posWidth
                                             Me.Invoke(New MethodInvoker(Sub() SimpleTrackBarPlayProgress.Value = posWidth))
                                         End If
                                     End Sub

        ' FIXME: There has to be a better solution
        ' Find the number of buffers for the waveOutDevice, so that internal WaveOut buffer is as close to bufferLength * stp as possible
        Dim desiredLatency As Integer = 300 ' WaveOut default. See https://github.com/naudio/NAudio/blob/master/NAudio/Wave/WaveOutputs/WaveOut.cs#L102
        Dim outBufLen As Integer
        Dim numBuf As Integer = 0
        Do
            numBuf += 1
            outBufLen = waveReader.WaveFormat.ConvertLatencyToByteSize(desiredLatency + numBuf - 1) / numBuf
        Loop While bufferLength * stp < outBufLen

        audioOut = New WaveOut(WaveCallbackInfo.FunctionCallback())
        AddHandler audioOut.PlaybackStopped, Sub()
                                                 gain = 1.0
                                                 StopAudioDevice()
                                                 Me.Invoke(New MethodInvoker(Sub() SimpleTrackBarPlayProgress.Value = 0))
                                                 If Not abortThreads Then InitAudioSource()
                                             End Sub
        audioOut.DesiredLatency = desiredLatency
        audioOut.NumberOfBuffers = numBuf
        audioOut.Init(wp)
        audioOut.Play()

        ' FIXME: Need to find out why playback freezes the whole program. The DoEvents statements seem to help.
        Application.DoEvents()
        Application.DoEvents()
        Application.DoEvents()

        SimpleTrackBarPlayProgress.ReadOnly = False
    End Sub
End Class