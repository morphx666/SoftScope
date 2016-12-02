Imports System.IO

Public Class WaveHelper
    Public Shared Function Read(filename As String, ByRef L As Single(), ByRef R As Single(), ByRef sampleRate As Integer, ByRef channels As Integer, ByRef bitDepth As Integer) As Boolean
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
                channels = reader.ReadInt16()
                sampleRate = reader.ReadInt32()
                Dim byteRate As Integer = reader.ReadInt32()
                Dim fmtBlockAlign As Integer = reader.ReadInt16()
                bitDepth = reader.ReadInt16()

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
                    Case 32
                        asFloat = New Single(samps - 1) {}
                        Buffer.BlockCopy(byteArray, 0, asFloat, 0, bytes)
                    Case 16
                        Dim asInt16 As Int16() = New Int16(samps - 1) {}
                        Buffer.BlockCopy(byteArray, 0, asInt16, 0, bytes)
                        asFloat = Array.ConvertAll(asInt16, Function(e) e / CSng(Int16.MaxValue))
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
        End Try

        Return False
    End Function
End Class
