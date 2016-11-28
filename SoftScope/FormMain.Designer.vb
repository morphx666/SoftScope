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
        Me.PanelOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'PanelOptions
        '
        Me.PanelOptions.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PanelOptions.BackColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(33, Byte), Integer), CType(CType(33, Byte), Integer))
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
        Me.PanelRightChannel.Location = New System.Drawing.Point(126, 193)
        Me.PanelRightChannel.Name = "PanelRightChannel"
        Me.PanelRightChannel.Size = New System.Drawing.Size(13, 13)
        Me.PanelRightChannel.TabIndex = 3
        '
        'PanelLeftChannel
        '
        Me.PanelLeftChannel.BackColor = System.Drawing.Color.SlateBlue
        Me.PanelLeftChannel.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PanelLeftChannel.Location = New System.Drawing.Point(126, 172)
        Me.PanelLeftChannel.Name = "PanelLeftChannel"
        Me.PanelLeftChannel.Size = New System.Drawing.Size(13, 13)
        Me.PanelLeftChannel.TabIndex = 3
        '
        'PanelRayColor
        '
        Me.PanelRayColor.BackColor = System.Drawing.Color.FromArgb(CType(CType(90, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.PanelRayColor.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PanelRayColor.Location = New System.Drawing.Point(126, 151)
        Me.PanelRayColor.Name = "PanelRayColor"
        Me.PanelRayColor.Size = New System.Drawing.Size(13, 13)
        Me.PanelRayColor.TabIndex = 3
        '
        'PanelBackColor
        '
        Me.PanelBackColor.BackColor = System.Drawing.Color.Black
        Me.PanelBackColor.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PanelBackColor.Location = New System.Drawing.Point(126, 130)
        Me.PanelBackColor.Name = "PanelBackColor"
        Me.PanelBackColor.Size = New System.Drawing.Size(13, 13)
        Me.PanelBackColor.TabIndex = 3
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(21, 192)
        Me.Label6.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(82, 15)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Right Channel"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(21, 171)
        Me.Label5.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(74, 15)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Left Channel"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(21, 150)
        Me.Label4.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(26, 15)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Ray"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(21, 129)
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
        Me.Label7.Location = New System.Drawing.Point(12, 213)
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
        Me.Label2.Location = New System.Drawing.Point(12, 108)
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
        Me.Label1.Location = New System.Drawing.Point(12, 12)
        Me.Label1.Margin = New System.Windows.Forms.Padding(3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(95, 15)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Process Options"
        '
        'CheckBoxFlipY
        '
        Me.CheckBoxFlipY.AutoSize = True
        Me.CheckBoxFlipY.Location = New System.Drawing.Point(21, 58)
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
        Me.CheckBoxFlipXY.Location = New System.Drawing.Point(21, 83)
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
        Me.CheckBoxFFT.Location = New System.Drawing.Point(21, 259)
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
        Me.CheckBoxWaveForm.Location = New System.Drawing.Point(21, 234)
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
        Me.CheckBoxFlipX.Location = New System.Drawing.Point(21, 33)
        Me.CheckBoxFlipX.Margin = New System.Windows.Forms.Padding(12, 3, 3, 3)
        Me.CheckBoxFlipX.Name = "CheckBoxFlipX"
        Me.CheckBoxFlipX.Size = New System.Drawing.Size(55, 19)
        Me.CheckBoxFlipX.TabIndex = 0
        Me.CheckBoxFlipX.Text = "Flip X"
        Me.CheckBoxFlipX.UseVisualStyleBackColor = True
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
        Me.Name = "FormMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SoftScope"
        Me.PanelOptions.ResumeLayout(False)
        Me.PanelOptions.PerformLayout()
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
End Class
