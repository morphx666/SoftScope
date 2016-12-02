<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormMain))
        Me.PanelOptions = New System.Windows.Forms.Panel()
        Me.ComboBoxYAxis = New System.Windows.Forms.ComboBox()
        Me.ComboBoxXAxis = New System.Windows.Forms.ComboBox()
        Me.ComboBoxAudioDevices = New System.Windows.Forms.ComboBox()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.LabelVersion = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.PanelRightChannel = New System.Windows.Forms.Panel()
        Me.PanelLeftChannel = New System.Windows.Forms.Panel()
        Me.PanelRayColor = New System.Windows.Forms.Panel()
        Me.PanelBackColor = New System.Windows.Forms.Panel()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.LabelAudioFormat = New System.Windows.Forms.Label()
        Me.LabelAudioSource = New System.Windows.Forms.Label()
        Me.LabelYAxis = New System.Windows.Forms.Label()
        Me.LabelMsPerDiv = New System.Windows.Forms.Label()
        Me.LabelXAxis = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CheckBoxFlipY = New System.Windows.Forms.CheckBox()
        Me.CheckBoxFlipXY = New System.Windows.Forms.CheckBox()
        Me.CheckBoxFFT = New System.Windows.Forms.CheckBox()
        Me.CheckBoxWaveForm = New System.Windows.Forms.CheckBox()
        Me.CheckBoxFlipX = New System.Windows.Forms.CheckBox()
        Me.ButtonPlayFile = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PanelOptions.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PanelOptions
        '
        Me.PanelOptions.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PanelOptions.BackColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(33, Byte), Integer), CType(CType(33, Byte), Integer))
        Me.PanelOptions.Controls.Add(Me.ButtonPlayFile)
        Me.PanelOptions.Controls.Add(Me.ComboBoxYAxis)
        Me.PanelOptions.Controls.Add(Me.ComboBoxXAxis)
        Me.PanelOptions.Controls.Add(Me.ComboBoxAudioDevices)
        Me.PanelOptions.Controls.Add(Me.Panel4)
        Me.PanelOptions.Controls.Add(Me.Panel3)
        Me.PanelOptions.Controls.Add(Me.Panel2)
        Me.PanelOptions.Controls.Add(Me.Panel1)
        Me.PanelOptions.Controls.Add(Me.LabelVersion)
        Me.PanelOptions.Controls.Add(Me.Label8)
        Me.PanelOptions.Controls.Add(Me.PictureBox1)
        Me.PanelOptions.Controls.Add(Me.PanelRightChannel)
        Me.PanelOptions.Controls.Add(Me.PanelLeftChannel)
        Me.PanelOptions.Controls.Add(Me.PanelRayColor)
        Me.PanelOptions.Controls.Add(Me.PanelBackColor)
        Me.PanelOptions.Controls.Add(Me.Label6)
        Me.PanelOptions.Controls.Add(Me.Label5)
        Me.PanelOptions.Controls.Add(Me.Label4)
        Me.PanelOptions.Controls.Add(Me.LabelAudioFormat)
        Me.PanelOptions.Controls.Add(Me.LabelAudioSource)
        Me.PanelOptions.Controls.Add(Me.LabelYAxis)
        Me.PanelOptions.Controls.Add(Me.LabelMsPerDiv)
        Me.PanelOptions.Controls.Add(Me.LabelXAxis)
        Me.PanelOptions.Controls.Add(Me.Label14)
        Me.PanelOptions.Controls.Add(Me.Label12)
        Me.PanelOptions.Controls.Add(Me.Label11)
        Me.PanelOptions.Controls.Add(Me.Label3)
        Me.PanelOptions.Controls.Add(Me.Label7)
        Me.PanelOptions.Controls.Add(Me.Label2)
        Me.PanelOptions.Controls.Add(Me.Label10)
        Me.PanelOptions.Controls.Add(Me.Label9)
        Me.PanelOptions.Controls.Add(Me.Label1)
        Me.PanelOptions.Controls.Add(Me.CheckBoxFlipY)
        Me.PanelOptions.Controls.Add(Me.CheckBoxFlipXY)
        Me.PanelOptions.Controls.Add(Me.CheckBoxFFT)
        Me.PanelOptions.Controls.Add(Me.CheckBoxWaveForm)
        Me.PanelOptions.Controls.Add(Me.CheckBoxFlipX)
        Me.PanelOptions.Location = New System.Drawing.Point(0, 0)
        Me.PanelOptions.Name = "PanelOptions"
        Me.PanelOptions.Size = New System.Drawing.Size(168, 729)
        Me.PanelOptions.TabIndex = 0
        Me.PanelOptions.Visible = False
        '
        'ComboBoxYAxis
        '
        Me.ComboBoxYAxis.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBoxYAxis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxYAxis.FormattingEnabled = True
        Me.ComboBoxYAxis.Location = New System.Drawing.Point(76, 553)
        Me.ComboBoxYAxis.Name = "ComboBoxYAxis"
        Me.ComboBoxYAxis.Size = New System.Drawing.Size(87, 23)
        Me.ComboBoxYAxis.TabIndex = 8
        Me.ComboBoxYAxis.Visible = False
        '
        'ComboBoxXAxis
        '
        Me.ComboBoxXAxis.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBoxXAxis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxXAxis.FormattingEnabled = True
        Me.ComboBoxXAxis.Location = New System.Drawing.Point(76, 532)
        Me.ComboBoxXAxis.Name = "ComboBoxXAxis"
        Me.ComboBoxXAxis.Size = New System.Drawing.Size(87, 23)
        Me.ComboBoxXAxis.TabIndex = 8
        Me.ComboBoxXAxis.Visible = False
        '
        'ComboBoxAudioDevices
        '
        Me.ComboBoxAudioDevices.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBoxAudioDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxAudioDevices.FormattingEnabled = True
        Me.ComboBoxAudioDevices.Location = New System.Drawing.Point(24, 432)
        Me.ComboBoxAudioDevices.Name = "ComboBoxAudioDevices"
        Me.ComboBoxAudioDevices.Size = New System.Drawing.Size(139, 23)
        Me.ComboBoxAudioDevices.TabIndex = 8
        Me.ComboBoxAudioDevices.Visible = False
        '
        'Panel4
        '
        Me.Panel4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel4.BackColor = System.Drawing.Color.DimGray
        Me.Panel4.Location = New System.Drawing.Point(3, 613)
        Me.Panel4.Margin = New System.Windows.Forms.Padding(3, 18, 3, 18)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(160, 1)
        Me.Panel4.TabIndex = 7
        '
        'Panel3
        '
        Me.Panel3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel3.BackColor = System.Drawing.Color.DimGray
        Me.Panel3.Location = New System.Drawing.Point(3, 492)
        Me.Panel3.Margin = New System.Windows.Forms.Padding(3, 18, 3, 18)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(160, 1)
        Me.Panel3.TabIndex = 7
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.BackColor = System.Drawing.Color.DimGray
        Me.Panel2.Location = New System.Drawing.Point(3, 392)
        Me.Panel2.Margin = New System.Windows.Forms.Padding(3, 18, 3, 18)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(160, 1)
        Me.Panel2.TabIndex = 7
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.DimGray
        Me.Panel1.Location = New System.Drawing.Point(3, 83)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(3, 18, 3, 18)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(160, 1)
        Me.Panel1.TabIndex = 7
        '
        'LabelVersion
        '
        Me.LabelVersion.AutoSize = True
        Me.LabelVersion.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelVersion.ForeColor = System.Drawing.Color.DimGray
        Me.LabelVersion.Location = New System.Drawing.Point(74, 40)
        Me.LabelVersion.Name = "LabelVersion"
        Me.LabelVersion.Size = New System.Drawing.Size(61, 13)
        Me.LabelVersion.TabIndex = 6
        Me.LabelVersion.Text = "0000.00.00"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Segoe UI Light", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.White
        Me.Label8.Location = New System.Drawing.Point(71, 16)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(92, 25)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "SoftScope"
        '
        'PanelRightChannel
        '
        Me.PanelRightChannel.BackColor = System.Drawing.Color.OrangeRed
        Me.PanelRightChannel.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PanelRightChannel.Location = New System.Drawing.Point(126, 286)
        Me.PanelRightChannel.Name = "PanelRightChannel"
        Me.PanelRightChannel.Size = New System.Drawing.Size(13, 13)
        Me.PanelRightChannel.TabIndex = 3
        '
        'PanelLeftChannel
        '
        Me.PanelLeftChannel.BackColor = System.Drawing.Color.SlateBlue
        Me.PanelLeftChannel.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PanelLeftChannel.Location = New System.Drawing.Point(126, 265)
        Me.PanelLeftChannel.Name = "PanelLeftChannel"
        Me.PanelLeftChannel.Size = New System.Drawing.Size(13, 13)
        Me.PanelLeftChannel.TabIndex = 3
        '
        'PanelRayColor
        '
        Me.PanelRayColor.BackColor = System.Drawing.Color.FromArgb(CType(CType(90, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.PanelRayColor.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PanelRayColor.Location = New System.Drawing.Point(126, 244)
        Me.PanelRayColor.Name = "PanelRayColor"
        Me.PanelRayColor.Size = New System.Drawing.Size(13, 13)
        Me.PanelRayColor.TabIndex = 3
        '
        'PanelBackColor
        '
        Me.PanelBackColor.BackColor = System.Drawing.Color.Black
        Me.PanelBackColor.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PanelBackColor.Location = New System.Drawing.Point(126, 223)
        Me.PanelBackColor.Name = "PanelBackColor"
        Me.PanelBackColor.Size = New System.Drawing.Size(13, 13)
        Me.PanelBackColor.TabIndex = 3
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(21, 285)
        Me.Label6.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(82, 15)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Right Channel"
        Me.Label6.UseMnemonic = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(21, 264)
        Me.Label5.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(74, 15)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Left Channel"
        Me.Label5.UseMnemonic = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(21, 243)
        Me.Label4.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(26, 15)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Ray"
        Me.Label4.UseMnemonic = False
        '
        'LabelAudioFormat
        '
        Me.LabelAudioFormat.AutoEllipsis = True
        Me.LabelAudioFormat.AutoSize = True
        Me.LabelAudioFormat.Location = New System.Drawing.Point(21, 456)
        Me.LabelAudioFormat.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.LabelAudioFormat.Name = "LabelAudioFormat"
        Me.LabelAudioFormat.Size = New System.Drawing.Size(80, 15)
        Me.LabelAudioFormat.TabIndex = 2
        Me.LabelAudioFormat.Text = "Audio Format"
        Me.LabelAudioFormat.UseMnemonic = False
        '
        'LabelAudioSource
        '
        Me.LabelAudioSource.AutoEllipsis = True
        Me.LabelAudioSource.AutoSize = True
        Me.LabelAudioSource.Location = New System.Drawing.Point(21, 435)
        Me.LabelAudioSource.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.LabelAudioSource.Name = "LabelAudioSource"
        Me.LabelAudioSource.Size = New System.Drawing.Size(104, 15)
        Me.LabelAudioSource.TabIndex = 2
        Me.LabelAudioSource.Text = "Sound Card Name"
        Me.LabelAudioSource.UseMnemonic = False
        '
        'LabelYAxis
        '
        Me.LabelYAxis.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelYAxis.Location = New System.Drawing.Point(74, 556)
        Me.LabelYAxis.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.LabelYAxis.Name = "LabelYAxis"
        Me.LabelYAxis.Size = New System.Drawing.Size(89, 15)
        Me.LabelYAxis.TabIndex = 2
        Me.LabelYAxis.Text = "Right Channel"
        Me.LabelYAxis.UseMnemonic = False
        '
        'LabelMsPerDiv
        '
        Me.LabelMsPerDiv.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelMsPerDiv.Location = New System.Drawing.Point(75, 577)
        Me.LabelMsPerDiv.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.LabelMsPerDiv.Name = "LabelMsPerDiv"
        Me.LabelMsPerDiv.Size = New System.Drawing.Size(88, 15)
        Me.LabelMsPerDiv.TabIndex = 2
        Me.LabelMsPerDiv.Text = "1000"
        Me.LabelMsPerDiv.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.LabelMsPerDiv.UseMnemonic = False
        '
        'LabelXAxis
        '
        Me.LabelXAxis.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelXAxis.Location = New System.Drawing.Point(74, 535)
        Me.LabelXAxis.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.LabelXAxis.Name = "LabelXAxis"
        Me.LabelXAxis.Size = New System.Drawing.Size(89, 15)
        Me.LabelXAxis.TabIndex = 2
        Me.LabelXAxis.Text = "Left Channel"
        Me.LabelXAxis.UseMnemonic = False
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(18, 577)
        Me.Label14.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(45, 15)
        Me.Label14.TabIndex = 2
        Me.Label14.Text = "Ms/Div"
        Me.Label14.UseMnemonic = False
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(18, 556)
        Me.Label12.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(38, 15)
        Me.Label12.TabIndex = 2
        Me.Label12.Text = "Y Axis"
        Me.Label12.UseMnemonic = False
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(18, 535)
        Me.Label11.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(38, 15)
        Me.Label11.TabIndex = 2
        Me.Label11.Text = "X Axis"
        Me.Label11.UseMnemonic = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(21, 222)
        Me.Label3.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(71, 15)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Background"
        Me.Label3.UseMnemonic = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.Label7.Location = New System.Drawing.Point(12, 306)
        Me.Label7.Margin = New System.Windows.Forms.Padding(3)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(42, 15)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "Panels"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.Label2.Location = New System.Drawing.Point(12, 201)
        Me.Label2.Margin = New System.Windows.Forms.Padding(3)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(73, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Appearance"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.Label10.Location = New System.Drawing.Point(9, 514)
        Me.Label10.Margin = New System.Windows.Forms.Padding(3)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(121, 15)
        Me.Label10.TabIndex = 1
        Me.Label10.Text = "Oscilloscope Options"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.Label9.Location = New System.Drawing.Point(9, 414)
        Me.Label9.Margin = New System.Windows.Forms.Padding(3)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(81, 15)
        Me.Label9.TabIndex = 1
        Me.Label9.Text = "Audio Source"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.Label1.Location = New System.Drawing.Point(12, 105)
        Me.Label1.Margin = New System.Windows.Forms.Padding(3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(95, 15)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Process Options"
        '
        'CheckBoxFlipY
        '
        Me.CheckBoxFlipY.AutoSize = True
        Me.CheckBoxFlipY.Location = New System.Drawing.Point(21, 151)
        Me.CheckBoxFlipY.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.CheckBoxFlipY.Name = "CheckBoxFlipY"
        Me.CheckBoxFlipY.Size = New System.Drawing.Size(55, 19)
        Me.CheckBoxFlipY.TabIndex = 0
        Me.CheckBoxFlipY.Text = "Flip Y"
        Me.CheckBoxFlipY.UseMnemonic = False
        Me.CheckBoxFlipY.UseVisualStyleBackColor = True
        '
        'CheckBoxFlipXY
        '
        Me.CheckBoxFlipXY.AutoSize = True
        Me.CheckBoxFlipXY.Location = New System.Drawing.Point(21, 176)
        Me.CheckBoxFlipXY.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.CheckBoxFlipXY.Name = "CheckBoxFlipXY"
        Me.CheckBoxFlipXY.Size = New System.Drawing.Size(62, 19)
        Me.CheckBoxFlipXY.TabIndex = 0
        Me.CheckBoxFlipXY.Text = "Flip XY"
        Me.CheckBoxFlipXY.UseMnemonic = False
        Me.CheckBoxFlipXY.UseVisualStyleBackColor = True
        '
        'CheckBoxFFT
        '
        Me.CheckBoxFFT.AutoSize = True
        Me.CheckBoxFFT.Location = New System.Drawing.Point(21, 352)
        Me.CheckBoxFFT.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.CheckBoxFFT.Name = "CheckBoxFFT"
        Me.CheckBoxFFT.Size = New System.Drawing.Size(77, 19)
        Me.CheckBoxFFT.TabIndex = 0
        Me.CheckBoxFFT.Text = "Spectrum"
        Me.CheckBoxFFT.UseMnemonic = False
        Me.CheckBoxFFT.UseVisualStyleBackColor = True
        '
        'CheckBoxWaveForm
        '
        Me.CheckBoxWaveForm.AutoSize = True
        Me.CheckBoxWaveForm.Location = New System.Drawing.Point(21, 327)
        Me.CheckBoxWaveForm.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.CheckBoxWaveForm.Name = "CheckBoxWaveForm"
        Me.CheckBoxWaveForm.Size = New System.Drawing.Size(86, 19)
        Me.CheckBoxWaveForm.TabIndex = 0
        Me.CheckBoxWaveForm.Text = "Wave Form"
        Me.CheckBoxWaveForm.UseMnemonic = False
        Me.CheckBoxWaveForm.UseVisualStyleBackColor = True
        '
        'CheckBoxFlipX
        '
        Me.CheckBoxFlipX.AutoSize = True
        Me.CheckBoxFlipX.Location = New System.Drawing.Point(21, 126)
        Me.CheckBoxFlipX.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.CheckBoxFlipX.Name = "CheckBoxFlipX"
        Me.CheckBoxFlipX.Size = New System.Drawing.Size(55, 19)
        Me.CheckBoxFlipX.TabIndex = 0
        Me.CheckBoxFlipX.Text = "Flip X"
        Me.CheckBoxFlipX.UseMnemonic = False
        Me.CheckBoxFlipX.UseVisualStyleBackColor = True
        '
        'ButtonPlayFile
        '
        Me.ButtonPlayFile.FlatAppearance.BorderColor = System.Drawing.Color.DimGray
        Me.ButtonPlayFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonPlayFile.Image = CType(resources.GetObject("ButtonPlayFile.Image"), System.Drawing.Image)
        Me.ButtonPlayFile.Location = New System.Drawing.Point(51, 635)
        Me.ButtonPlayFile.Name = "ButtonPlayFile"
        Me.ButtonPlayFile.Size = New System.Drawing.Size(66, 66)
        Me.ButtonPlayFile.TabIndex = 9
        Me.ButtonPlayFile.UseVisualStyleBackColor = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.SoftScope.My.Resources.Resources.icon
        Me.PictureBox1.Location = New System.Drawing.Point(6, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(60, 50)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 5
        Me.PictureBox1.TabStop = False
        '
        'FormMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(1008, 729)
        Me.Controls.Add(Me.PanelOptions)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.LightGray
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FormMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "SoftScope"
        Me.PanelOptions.ResumeLayout(False)
        Me.PanelOptions.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PanelOptions As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents CheckBoxFlipY As CheckBox
    Friend WithEvents CheckBoxFlipXY As CheckBox
    Friend WithEvents CheckBoxFlipX As CheckBox
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents PanelRightChannel As Panel
    Friend WithEvents PanelLeftChannel As Panel
    Friend WithEvents PanelRayColor As Panel
    Friend WithEvents PanelBackColor As Panel
    Friend WithEvents Label7 As Label
    Friend WithEvents CheckBoxFFT As CheckBox
    Friend WithEvents CheckBoxWaveForm As CheckBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Label8 As Label
    Friend WithEvents LabelVersion As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents LabelAudioSource As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents LabelAudioFormat As Label
    Friend WithEvents ComboBoxAudioDevices As ComboBox
    Friend WithEvents ComboBoxYAxis As ComboBox
    Friend WithEvents ComboBoxXAxis As ComboBox
    Friend WithEvents Panel3 As Panel
    Friend WithEvents LabelYAxis As Label
    Friend WithEvents LabelXAxis As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents LabelMsPerDiv As Label
    Friend WithEvents Panel4 As Panel
    Friend WithEvents ButtonPlayFile As Button
End Class
