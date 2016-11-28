Public Class FilterButterworth
    Public Enum PassType
        Highpass
        Lowpass
    End Enum

    ''' <summary>
    ''' rez amount, from sqrt(2) to ~ 0.1
    ''' </summary>
    Private ReadOnly mResonance As Single

    Private ReadOnly mFrequency As Single
    Private ReadOnly mSampleRate As Integer
    Private ReadOnly mPassType As PassType

    Private ReadOnly c As Single, a1 As Single, a2 As Single, a3 As Single, b1 As Single, b2 As Single

    ''' <summary>
    ''' Array of input values, latest are in front
    ''' </summary>
    Private inputHistory As Single() = New Single(1) {}

    ''' <summary>
    ''' Array of output values, latest are in front
    ''' </summary>
    Private outputHistory As Single() = New Single(2) {}

    Public Sub New(frequency As Single, sampleRate As Integer, filter As PassType, resonance As Single)
        mResonance = resonance
        mFrequency = frequency
        mSampleRate = sampleRate
        mPassType = filter

        Select Case mPassType
            Case PassType.Lowpass
                c = 1.0F / CSng(Math.Tan(Math.PI * frequency / sampleRate))
                a1 = 1.0F / (1.0F + resonance * c + c * c)
                a2 = 2.0F * a1
                a3 = a1
                b1 = 2.0F * (1.0F - c * c) * a1
                b2 = (1.0F - resonance * c + c * c) * a1
                Exit Select
            Case PassType.Highpass
                c = CSng(Math.Tan(Math.PI * frequency / sampleRate))
                a1 = 1.0F / (1.0F + resonance * c + c * c)
                a2 = -2.0F * a1
                a3 = a1
                b1 = 2.0F * (c * c - 1.0F) * a1
                b2 = (1.0F - resonance * c + c * c) * a1
                Exit Select
        End Select
    End Sub

    Public Sub Update(newInput As Single)
        Dim newOutput As Single = a1 * newInput + a2 * Me.inputHistory(0) + a3 * Me.inputHistory(1) - b1 * Me.outputHistory(0) - b2 * Me.outputHistory(1)

        Me.inputHistory(1) = Me.inputHistory(0)
        Me.inputHistory(0) = newInput

        Me.outputHistory(2) = Me.outputHistory(1)
        Me.outputHistory(1) = Me.outputHistory(0)
        Me.outputHistory(0) = newOutput
    End Sub

    Public ReadOnly Property Value() As Single
        Get
            Return Me.outputHistory(0)
        End Get
    End Property
End Class