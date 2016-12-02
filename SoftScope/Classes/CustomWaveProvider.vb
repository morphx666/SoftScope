Imports NAudio.Wave

Public Class CustomWaveProvider
    Implements IWaveProvider

    Private waveReader As WaveFileReader
    Private bufferSize As Integer

    Public Event DataAvailable(b() As Byte)

    Public Sub New(wfr As WaveFileReader, maxBufferSize As Integer)
        waveReader = wfr
        bufferSize = maxBufferSize
    End Sub

    Public ReadOnly Property WaveFormat As WaveFormat Implements IWaveProvider.WaveFormat
        Get
            Return waveReader.WaveFormat
        End Get
    End Property

    Public Function Read(buffer() As Byte, offset As Integer, count As Integer) As Integer Implements IWaveProvider.Read
        Dim n As Integer = waveReader.Read(buffer, offset, count)
        RaiseEvent DataAvailable(buffer)
        Return n
    End Function
End Class