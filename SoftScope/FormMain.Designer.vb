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
        Me.PanelRightChannel = New System.Windows.Forms.Panel()
        Me.PanelLeftChannel = New System.Windows.Forms.Panel()
        Me.PanelRayColor = New System.Windows.Forms.Panel()
        Me.PanelBackColor = New System.Windows.Forms.Panel()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CheckBoxFlipY = New System.Windows.Forms.CheckBox()
        Me.CheckBoxFlipXY = New System.Windows.Forms.CheckBox()
        Me.CheckBoxFFT = New System.Windows.Forms.CheckBox()
        Me.CheckBoxWaveForm = New System.Windows.Forms.CheckBox()
        Me.CheckBoxFlipX = New System.Windows.Forms.CheckBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.LabelVersion = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.PanelOptions.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PanelOptions
        '
        Me.PanelOptions.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PanelOptions.BackColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(33, Byte), Integer), CType(CType(33, Byte), Integer))
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
        Me.PanelOptions.Controls.Add(Me.Label3)
        Me.PanelOptions.Controls.Add(Me.Label7)
        Me.PanelOptions.Controls.Add(Me.Label2)
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
        'PanelRightChannel
        '
        Me.PanelRightChannel.BackColor = System.Drawing.Color.OrangeRed
        Me.PanelRightChannel.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PanelRightChannel.Location = New System.Drawing.Point(126, 285)
        Me.PanelRightChannel.Name = "PanelRightChannel"
        Me.PanelRightChannel.Size = New System.Drawing.Size(13, 13)
        Me.PanelRightChannel.TabIndex = 3
        '
        'PanelLeftChannel
        '
        Me.PanelLeftChannel.BackColor = System.Drawing.Color.SlateBlue
        Me.PanelLeftChannel.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PanelLeftChannel.Location = New System.Drawing.Point(126, 264)
        Me.PanelLeftChannel.Name = "PanelLeftChannel"
        Me.PanelLeftChannel.Size = New System.Drawing.Size(13, 13)
        Me.PanelLeftChannel.TabIndex = 3
        '
        'PanelRayColor
        '
        Me.PanelRayColor.BackColor = System.Drawing.Color.FromArgb(CType(CType(90, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.PanelRayColor.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PanelRayColor.Location = New System.Drawing.Point(126, 243)
        Me.PanelRayColor.Name = "PanelRayColor"
        Me.PanelRayColor.Size = New System.Drawing.Size(13, 13)
        Me.PanelRayColor.TabIndex = 3
        '
        'PanelBackColor
        '
        Me.PanelBackColor.BackColor = System.Drawing.Color.Black
        Me.PanelBackColor.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PanelBackColor.Location = New System.Drawing.Point(126, 222)
        Me.PanelBackColor.Name = "PanelBackColor"
        Me.PanelBackColor.Size = New System.Drawing.Size(13, 13)
        Me.PanelBackColor.TabIndex = 3
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(21, 284)
        Me.Label6.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(82, 15)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Right Channel"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(21, 263)
        Me.Label5.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(74, 15)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Left Channel"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(21, 242)
        Me.Label4.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(26, 15)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Ray"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(21, 221)
        Me.Label3.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(71, 15)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Background"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.Label7.Location = New System.Drawing.Point(12, 305)
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
        Me.Label2.Location = New System.Drawing.Point(12, 200)
        Me.Label2.Margin = New System.Windows.Forms.Padding(3)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(73, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Appearance"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.Label1.Location = New System.Drawing.Point(12, 104)
        Me.Label1.Margin = New System.Windows.Forms.Padding(3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(95, 15)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Process Options"
        '
        'CheckBoxFlipY
        '
        Me.CheckBoxFlipY.AutoSize = True
        Me.CheckBoxFlipY.Location = New System.Drawing.Point(21, 150)
        Me.CheckBoxFlipY.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.CheckBoxFlipY.Name = "CheckBoxFlipY"
        Me.CheckBoxFlipY.Size = New System.Drawing.Size(55, 19)
        Me.CheckBoxFlipY.TabIndex = 0
        Me.CheckBoxFlipY.Text = "Flip Y"
        Me.CheckBoxFlipY.UseVisualStyleBackColor = True
        '
        'CheckBoxFlipXY
        '
        Me.CheckBoxFlipXY.AutoSize = True
        Me.CheckBoxFlipXY.Location = New System.Drawing.Point(21, 175)
        Me.CheckBoxFlipXY.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.CheckBoxFlipXY.Name = "CheckBoxFlipXY"
        Me.CheckBoxFlipXY.Size = New System.Drawing.Size(62, 19)
        Me.CheckBoxFlipXY.TabIndex = 0
        Me.CheckBoxFlipXY.Text = "Flip XY"
        Me.CheckBoxFlipXY.UseVisualStyleBackColor = True
        '
        'CheckBoxFFT
        '
        Me.CheckBoxFFT.AutoSize = True
        Me.CheckBoxFFT.Location = New System.Drawing.Point(21, 351)
        Me.CheckBoxFFT.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.CheckBoxFFT.Name = "CheckBoxFFT"
        Me.CheckBoxFFT.Size = New System.Drawing.Size(77, 19)
        Me.CheckBoxFFT.TabIndex = 0
        Me.CheckBoxFFT.Text = "Spectrum"
        Me.CheckBoxFFT.UseVisualStyleBackColor = True
        '
        'CheckBoxWaveForm
        '
        Me.CheckBoxWaveForm.AutoSize = True
        Me.CheckBoxWaveForm.Location = New System.Drawing.Point(21, 326)
        Me.CheckBoxWaveForm.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.CheckBoxWaveForm.Name = "CheckBoxWaveForm"
        Me.CheckBoxWaveForm.Size = New System.Drawing.Size(86, 19)
        Me.CheckBoxWaveForm.TabIndex = 0
        Me.CheckBoxWaveForm.Text = "Wave Form"
        Me.CheckBoxWaveForm.UseVisualStyleBackColor = True
        '
        'CheckBoxFlipX
        '
        Me.CheckBoxFlipX.AutoSize = True
        Me.CheckBoxFlipX.Location = New System.Drawing.Point(21, 125)
        Me.CheckBoxFlipX.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.CheckBoxFlipX.Name = "CheckBoxFlipX"
        Me.CheckBoxFlipX.Size = New System.Drawing.Size(55, 19)
        Me.CheckBoxFlipX.TabIndex = 0
        Me.CheckBoxFlipX.Text = "Flip X"
        Me.CheckBoxFlipX.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Segoe UI Light", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.White
        Me.Label8.Location = New System.Drawing.Point(71, 10)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(92, 25)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "SoftScope"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.SoftScope.My.Resources.Resources.icon
        Me.PictureBox1.Location = New System.Drawing.Point(12, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(60, 50)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 5
        Me.PictureBox1.TabStop = False
        '
        'LabelVersion
        '
        Me.LabelVersion.AutoSize = True
        Me.LabelVersion.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelVersion.ForeColor = System.Drawing.Color.DimGray
        Me.LabelVersion.Location = New System.Drawing.Point(74, 34)
        Me.LabelVersion.Name = "LabelVersion"
        Me.LabelVersion.Size = New System.Drawing.Size(61, 13)
        Me.LabelVersion.TabIndex = 6
        Me.LabelVersion.Text = "0000.00.00"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.DimGray
        Me.Panel1.Location = New System.Drawing.Point(12, 82)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(151, 1)
        Me.Panel1.TabIndex = 7
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
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
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
End Class
