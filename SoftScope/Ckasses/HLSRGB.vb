<Serializable()>
Public Class HLSRGB
    Private mRed As Byte = 0
    Private mGreen As Byte = 0
    Private mBlue As Byte = 0
    Private mAlpha As Byte = 255

    Private mHue As Double = 0.0
    Private mLuminance As Double = 0.0
    Private mSaturation As Double = 0.0

    Public Structure HueLumSat
        Private mH As Double
        Private mL As Double
        Private mS As Double

        Public Sub New(hue As Double, lum As Double, sat As Double)
            mH = hue
            mL = lum
            mS = sat
        End Sub

        Public Property Hue() As Double
            Get
                Return mH
            End Get
            Set(value As Double)
                mH = value
            End Set
        End Property

        Public Property Lum() As Double
            Get
                Return mL
            End Get
            Set(value As Double)
                mL = value
            End Set
        End Property

        Public Property Sat() As Double
            Get
                Return mS
            End Get
            Set(value As Double)
                mS = value
            End Set
        End Property
    End Structure

    Public Sub New(c As Color)
        mRed = c.R
        mGreen = c.G
        mBlue = c.B
        mAlpha = c.A
        ToHLS()
    End Sub

    Public Sub New(hue As Double, luminance As Double, saturation As Double)
        mHue = hue
        mLuminance = luminance
        mSaturation = saturation
        ToRGB()
    End Sub

    Public Sub New(red As Byte, green As Byte, blue As Byte)
        mRed = red
        mGreen = green
        mBlue = blue
        mAlpha = 255
    End Sub

    Public Sub New(alpha As Byte, red As Byte, green As Byte, blue As Byte)
        mRed = red
        mGreen = green
        mBlue = blue
        mAlpha = alpha
    End Sub

    Public Sub New(hlsrgb As HLSRGB)
        mRed = hlsrgb.Red
        mBlue = hlsrgb.Blue
        mGreen = hlsrgb.Green
        mLuminance = hlsrgb.Luminance
        mHue = hlsrgb.Hue
        mSaturation = hlsrgb.Saturation
    End Sub

    Public Sub New()
    End Sub

    Public Property Red() As Byte
        Get
            Return mRed
        End Get
        Set(value As Byte)
            mRed = value
            ToHLS()
        End Set
    End Property

    Public Property Green() As Byte
        Get
            Return mGreen
        End Get
        Set(value As Byte)
            mGreen = value
            ToHLS()
        End Set
    End Property

    Public Property Blue() As Byte
        Get
            Return mBlue
        End Get
        Set(value As Byte)
            mBlue = value
            ToHLS()
        End Set
    End Property

    Public Property Luminance() As Double
        Get
            Return mLuminance
        End Get
        Set(value As Double)
            mLuminance = chkLum(value)
            ToRGB()
        End Set
    End Property

    Public Property Hue() As Double
        Get
            Return mHue
        End Get
        Set(value As Double)
            mHue = chkHue(value)
            ToRGB()
        End Set
    End Property

    Public Property Saturation() As Double
        Get
            Return mSaturation
        End Get
        Set(value As Double)
            mSaturation = chkSat(value)
            ToRGB()
        End Set
    End Property

    Public Property Alpha() As Byte
        Get
            Return mAlpha
        End Get
        Set(value As Byte)
            mAlpha = value
        End Set
    End Property

    Public Property HLS() As HueLumSat
        Get
            Return New HueLumSat(mHue, mLuminance, mSaturation)
        End Get
        Set(value As HueLumSat)
            mHue = chkHue(value.Hue)
            mLuminance = chkLum(value.Lum)
            mSaturation = chkSat(value.Sat)
            ToRGB()
        End Set
    End Property

    Public Property Color() As Color
        Get
            Return Color.FromArgb(mAlpha, mRed, mGreen, mBlue)
        End Get
        Set(value As Color)
            mRed = value.R
            mGreen = value.G
            mBlue = value.B
            mAlpha = value.A
            ToHLS()
        End Set
    End Property

    Public Sub LightenColor(lightenBy As Double)
        mLuminance *= (1.0 + lightenBy)
        If mLuminance > 1.0 Then mLuminance = 1.0
        ToRGB()
    End Sub

    Public Sub DarkenColor(darkenBy As Double)
        Luminance *= darkenBy
    End Sub

    Private Sub ToHLS()
        Dim minVal As Double = Math.Min(mRed, Math.Min(mGreen, mBlue))
        Dim maxVal As Double = Math.Max(mRed, Math.Max(mGreen, mBlue))

        Dim mDif As Double = maxVal - minVal
        Dim mSum As Double = maxVal + minVal

        mLuminance = mSum / 510.0

        If maxVal = minVal Then
            mSaturation = 0.0
            mHue = 0.0
        Else
            Dim rNorm As Double = (maxVal - mRed) / mDif
            Dim gNorm As Double = (maxVal - mGreen) / mDif
            Dim bNorm As Double = (maxVal - mBlue) / mDif

            mSaturation = If(mLuminance <= 0.5, (mDif / mSum), mDif / (510.0 - mSum))

            If mRed = maxVal Then mHue = 60.0 * (6.0 + bNorm - gNorm)
            If mGreen = maxVal Then mHue = 60.0 * (2.0 + rNorm - bNorm)
            If mBlue = maxVal Then mHue = 60.0 * (4.0 + gNorm - rNorm)
            If mHue > 360.0 Then mHue = Hue - 360.0
        End If
    End Sub

    Private Sub ToRGB()
        If mSaturation = 0.0 Then
            Red = CByte(mLuminance * 255.0)
            mGreen = mRed
            mBlue = mRed
        Else
            Dim rm1 As Double
            Dim rm2 As Double

            If mLuminance <= 0.5 Then
                rm2 = mLuminance + mLuminance * mSaturation
            Else
                rm2 = mLuminance + mSaturation - mLuminance * mSaturation
            End If
            rm1 = 2.0 * mLuminance - rm2
            mRed = ToRGB1(rm1, rm2, mHue + 120.0)
            mGreen = ToRGB1(rm1, rm2, mHue)
            mBlue = ToRGB1(rm1, rm2, mHue - 120.0)
        End If
    End Sub

    Private Function ToRGB1(rm1 As Double, rm2 As Double, rh As Double) As Byte
        If rh > 360.0 Then
            rh -= 360.0
        ElseIf rh < 0.0 Then
            rh += 360.0
        End If

        If (rh < 60.0) Then
            rm1 = rm1 + (rm2 - rm1) * rh / 60.0
        ElseIf (rh < 180.0) Then
            rm1 = rm2
        ElseIf (rh < 240.0) Then
            rm1 = rm1 + (rm2 - rm1) * (240.0 - rh) / 60.0
        End If

        Return CByte(If(rm1 * 255 > 255, 255, rm1 * 255))
    End Function

    Private Function chkHue(value As Double) As Double
        If value < 0.0 Then value = Math.Abs((360.0 + value) Mod 360.0)
        If value > 360.0 Then value = value Mod 360.0

        Return value
    End Function

    Private Function chkLum(value As Double) As Double
        If (value < 0.0) Or (value > 1.0) Then
            If value < 0.0 Then value = Math.Abs(value)
            If value > 1.0 Then value = 1.0
        End If

        Return value
    End Function

    Private Function chkSat(value As Double) As Double
        If value < 0.0 Then value = Math.Abs(value)
        If value > 1.0 Then value = 1.0

        Return value
    End Function

    Public Shared Widening Operator CType(value As Color) As HLSRGB
        Return New HLSRGB(value)
    End Operator

    Public Shared Narrowing Operator CType(value As HLSRGB) As Color
        Return value.Color
    End Operator

    Public Function ToHTML() As String
        Return String.Format("#{0}{1}{2}", mRed.ToString("X:00"),
                                            mGreen.ToString("X:00"),
                                            mBlue.ToString("X:00"))
    End Function
End Class
