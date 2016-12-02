Imports System.Math
Imports System.ComponentModel

' The FourierTransform function is based on the original work by Murphy McCauley

' TODO: Very interesting functions here: http://rapidq.phatcode.net/samples/fftlib.html
' C algorithms: http://paulbourke.net/miscellaneous/dft/

''' <exclude/>
<Browsable(False), EditorBrowsable(EditorBrowsableState.Never)>
Public Module FFT
    Public Enum FFTSizeConstants
        ''' <summary>32 bands</summary>
        FFTs32 = 32
        ''' <summary>64 bands</summary>
        FFTs64 = 64
        ''' <summary>128 bands</summary>
        FFTs128 = 128
        ''' <summary>256 bands</summary>
        FFTs256 = 256
        ''' <summary>512 bands</summary>
        FFTs512 = 512
        ''' <summary>1024 bands</summary>
        FFTs1024 = 1024
        ''' <summary>2048 bands</summary>
        FFTs2048 = 2048
        ''' <summary>4096 bands</summary>
        FFTs4096 = 4096
        ''' <summary>8192 bands</summary>
        FFTs8192 = 8192
        ''' <summary>16384 bands</summary>
        FFTs16384 = 16384
        ''' <summary>32768 bands</summary>
        FFTs32768 = 32768
    End Enum

    Public Enum FFTWindowConstants
        ''' <summary>No windowing is applied</summary>
        None = 0
        ''' <summary>Apply a basic triangle window</summary>
        Triangle = 1
        ''' <summary>Apply the Julius von Hann window</summary>
        Hanning = 2
        ''' <summary>Apply the Richard Hamming window</summary>
        Hamming = 3
        ''' <summary>Apply the Welch window</summary>
        Welch = 4
        ''' <summary>Apply the Gaussian window</summary>
        Gaussian = 5
        ''' <summary>Apply the Blackman window</summary>
        Blackman = 6
        ''' <summary>Apply the Parzen window</summary>
        Parzen = 7
        ''' <summary>Apply the Bartlett window</summary>
        Bartlett = 8
        ''' <summary>Apply the Cones window</summary>
        Connes = 9
        ''' <summary>Apply the KaiserBessel window</summary>
        KaiserBessel = 10
        ''' <summary>Apply the BlackmanHarris window</summary>
        BlackmanHarris = 11
        ''' <summary>Apply the Nuttall window</summary>
        Nuttall = 12
        ''' <summary>Apply the BlackmanNuttall window</summary>
        BlackmanNuttall = 13
        ''' <summary>Apply the FlatTop window</summary>
        FlatTop = 14
    End Enum

    Public Class ComplexDouble
        Public R As Double
        Public I As Double

        Public Sub New()
        End Sub

        Public Sub New(r As Double)
            Me.R = r
        End Sub

        Public Sub New(r As Double, i As Double)
            Me.R = r
            Me.I = i
        End Sub

        Public Function Abs() As Double
            Return Magnitude()
        End Function

        Public Function Power() As Double
            'Return R ^ 2 + I ^ 2
            Return R * R + I * I
        End Function

        Public Function Power2() As Double
            Return Math.Abs(R) + Math.Abs(I)
        End Function

        Public Function Power2Root() As Double
            Return Sqrt(Power2())
        End Function

        Public Function Conjugate() As ComplexDouble
            Return New ComplexDouble(R, -I)
        End Function

        Public Function Magnitude() As Double
            Return Sqrt(Power())
        End Function

        Public Shared Operator +(n1 As ComplexDouble, n2 As ComplexDouble) As ComplexDouble
            Return New ComplexDouble(n1.R + n2.R, n1.I + n2.I)
        End Operator

        Public Shared Operator +(n1 As ComplexDouble, n2 As Double) As ComplexDouble
            Return New ComplexDouble(n1.R + n2, n1.I)
        End Operator

        Public Shared Operator +(n1 As Double, n2 As ComplexDouble) As ComplexDouble
            Return New ComplexDouble(n1 + n2.R, n2.I)
        End Operator

        Public Shared Operator -(n1 As ComplexDouble, n2 As ComplexDouble) As ComplexDouble
            Return New ComplexDouble(n1.R - n2.R, n1.I - n2.I)
        End Operator

        Public Shared Operator -(n1 As ComplexDouble, n2 As Double) As ComplexDouble
            Return New ComplexDouble(n1.R - n2, n1.I)
        End Operator

        Public Shared Operator -(n1 As Double, n2 As ComplexDouble) As ComplexDouble
            Return New ComplexDouble(n1 - n2.R, -n2.I)
        End Operator

        Public Shared Operator *(n1 As ComplexDouble, n2 As ComplexDouble) As ComplexDouble
            Return New ComplexDouble(n1.R * n2.R - n1.I * n2.I, n1.I * n2.R + n2.I * n1.R)
        End Operator

        Public Shared Operator *(n1 As ComplexDouble, n2 As Double) As ComplexDouble
            Return New ComplexDouble(n1.R * n2, n1.I * n2)
        End Operator

        Public Shared Operator *(n1 As Double, n2 As ComplexDouble) As ComplexDouble
            Return New ComplexDouble(n1 * n2.R, n1 * n2.I)
        End Operator

        Public Shared Operator /(n1 As ComplexDouble, n2 As Double) As ComplexDouble
            Return New ComplexDouble(n1.R / n2, n1.I / n2)
        End Operator

        Public Shared Operator ^(n1 As ComplexDouble, n2 As Integer) As ComplexDouble
            Dim r As ComplexDouble = n1
            For i As Integer = 0 To n2 - 1
                r *= n1
            Next
            Return r
        End Operator

        Public Shared Function FromDouble(value As Double) As ComplexDouble
            Return New ComplexDouble(value, value)
        End Function

        Public Shared Function FromDouble(array() As Double) As ComplexDouble()
            Return (From d In array Select ComplexDouble.FromDouble(d)).ToArray()
        End Function

        Public Overrides Function ToString() As String
            Return String.Format("{0}+{1}i", R, I)
        End Function
    End Class

    Private Const PI2 As Double = 2.0 * PI
    Private Const LHHALF As Integer = 30 ' half-length of Hilbert transform filter
    Private Const LH As Integer = 2 * LHHALF + 1     ' filter length must be odd

    Private Function NumberOfBitsNeeded(powerOfTwo As Integer) As Integer
        For i As Integer = 0 To 32
            If (powerOfTwo And CInt(2 ^ i)) <> 0 Then Return i
        Next
    End Function

    Public Function ToNearestPowerOfTwo(x As Integer) As Integer
        Return Math.Pow(2, Math.Ceiling(Math.Log(x) / Math.Log(2)))
    End Function

    Public Function IsPowerOfTwo(x As Integer) As Boolean
        Return Not ((x And (x - 1)) <> 0) AndAlso (x >= 2)
    End Function

    Private Function ReverseBits(index As Integer, numBits As Integer) As Integer
        Dim rev As Integer
        For i As Integer = 0 To numBits - 1
            rev = (rev * 2) Or (index And 1)
            index \= 2
        Next i

        Return rev
    End Function

    ' http://groovit.disjunkt.com/analog/time-domain/fft.html
    ' http://www.relisoft.com/science/Physics/sound.html
    ' Decimation-in-time in-place FFT algorithm
    Public Sub FourierTransform(fftSize As Integer,
                                waveInL() As Double, fftOutL() As ComplexDouble,
                                waveInR() As Double, fftOutR() As ComplexDouble,
                                doInverse As Boolean)
        Dim numBits As Integer
        Dim k As Integer

        Dim deltaAngle As Double
        Dim alpha As Double
        Dim beta As Double
        Dim tmp As New ComplexDouble
        Dim initialAngle = New ComplexDouble(1)
        Dim angle As ComplexDouble

        Static rBits() As Integer
        Static lastFFTSize As Integer

        Dim blockSize As Integer = 2
        Dim blockEnd As Integer = 1
        Dim inverter As Double = If(doInverse, -1.0, 1.0)

        If lastFFTSize <> fftSize Then
            lastFFTSize = fftSize
            ReDim rBits(fftSize - 1)
            numBits = NumberOfBitsNeeded(fftSize)

            For i = 0 To fftSize - 1
                rBits(i) = ReverseBits(i, numBits)

                fftOutL(rBits(i)) = New ComplexDouble(waveInL(i))
                fftOutR(rBits(i)) = New ComplexDouble(waveInR(i))
            Next i
        Else
            For i = 0 To fftSize - 1
                fftOutL(rBits(i)) = New ComplexDouble(waveInL(i))
                fftOutR(rBits(i)) = New ComplexDouble(waveInR(i))
            Next i
        End If

        Do
            deltaAngle = PI2 / blockSize * inverter
            alpha = 2.0 * Sin(0.5 * deltaAngle) ^ 2.0
            beta = Sin(deltaAngle)

            For i = 0 To fftSize - 1 Step blockSize
                angle = initialAngle

                For j = i To blockEnd + i - 1
                    k = j + blockEnd

                    tmp.R = angle.R * fftOutL(k).R - angle.I * fftOutL(k).I
                    tmp.I = angle.I * fftOutL(k).R + angle.R * fftOutL(k).I
                    fftOutL(k) = fftOutL(j) - tmp
                    fftOutL(j) += tmp

                    tmp.R = angle.R * fftOutR(k).R - angle.I * fftOutR(k).I
                    tmp.I = angle.I * fftOutR(k).R + angle.R * fftOutR(k).I
                    fftOutR(k) = fftOutR(j) - tmp
                    fftOutR(j) += tmp

                    angle -= New ComplexDouble(alpha * angle.R + beta * angle.I,
                                               alpha * angle.I - beta * angle.R)
                Next
            Next

            blockEnd = blockSize
            blockSize *= 2
        Loop While blockSize <= fftSize

        If doInverse Then
            For i = 0 To fftSize - 1
                fftOutL(i).R /= fftSize
                fftOutR(i).R /= fftSize
            Next
        End If
    End Sub

#Region "Other FFT Algorithms"
    ' http://www.nicholson.com/dsp.fft1.html
    Public Function FFT1r(doInverse As Boolean, x() As Double, m As Integer) As ComplexDouble()
        Dim xc(x.Length - 1) As ComplexDouble

        For i As Integer = 0 To x.Length - 1
            xc(i) = New ComplexDouble(x(i))
        Next

        Return FFT1r(doInverse, xc, m)
    End Function

    Public Function FFT1r(doInverse As Boolean, x() As ComplexDouble, m As Integer) As ComplexDouble()
        Dim n As Integer = CInt(2 ^ m)
        Dim y(n - 1) As ComplexDouble

        For i As Integer = 0 To y.Length - 1
            y(i) = New ComplexDouble()
        Next

        Rec_FFT(doInverse, n, x, 0, y, 0, 1, 1)

        Return y
    End Function

    ' *** recursive out-of-place radix-2 FFT ***
    Private Sub Rec_FFT(doInverse As Boolean, n As Integer, x() As ComplexDouble, kx As Integer, y() As ComplexDouble, ky As Integer, ks As Integer, os As Double)
        Dim n2, i As Integer
        Dim c, s As Double
        Dim k1, k2 As Integer
        Dim ar, ai, br, bi As Double
        Dim flag As Integer = If(doInverse, -1, 1)

        If n = 1 Then
            ' ** this does a bit-reversed-index copy and scale **
            If doInverse = -1 Then
                s = 1 / ks
            Else
                s = 1
            End If
            y(ky).R = x(kx).R * s
            y(ky).I = x(kx).I * s
        Else
            n2 = n \ 2
            Rec_FFT(flag, n2, x, kx, y, ky, ks * 2, os)
            Rec_FFT(flag, n2, x, kx + ks, y, CInt(ky + os * n2), ks * 2, os)
            For i = 0 To n2 - 1
                c = Cos(i * 2 * PI / n)
                s = Sin(i * flag * 2 * PI / n)
                k1 = CInt(ky + os * (i))
                k2 = CInt(ky + os * (i + n2))
                ar = y(k1).R
                ai = y(k1).I
                br = c * y(k2).R - s * y(k2).I
                bi = c * y(k2).I + s * y(k2).R
                y(k1).R = ar + br
                y(k1).I = ai + bi
                y(k2).R = ar - br
                y(k2).I = ai - bi
            Next i
        End If
    End Sub

    ' *** Discrete Cosine Transform ***
    Private Sub DiscreteCosineTransform(n As Integer, isgn As Integer, a() As Double, ip() As Double, w() As Double)

    End Sub
#End Region

    Public Function FrequencyOfIndex(numberOfSamples As Integer, index As Integer) As Double
        If index >= numberOfSamples Then
            Return 0.0
        ElseIf index <= numberOfSamples / 2 Then
            Return CDbl(index) / CDbl(numberOfSamples)
        Else
            Return -CDbl(numberOfSamples - index) / CDbl(numberOfSamples)
        End If
    End Function

    Public Sub CalcFrequency(numberOfSamples As Integer, frequencyIndex As Integer, dataIn() As ComplexDouble, dataOut As ComplexDouble)
        Dim cos1 As Double, cos2 As Double, cos3 As Double
        Dim sin1 As Double, sin2 As Double, sin3 As Double
        Dim beta As Double
        Dim theta As Double = 2.0 * PI * frequencyIndex / CDbl(numberOfSamples)

        sin1 = Sin(-2.0 * theta)
        sin2 = Sin(-theta)
        cos1 = Cos(-2.0 * theta)
        cos2 = Cos(-theta)
        beta = 2 * cos2

        For k As Integer = 0 To numberOfSamples - 2
            'Update trig values
            sin3 = beta * sin2 - sin1
            sin1 = sin2
            sin2 = sin3

            cos3 = beta * cos2 - cos1
            cos1 = cos2
            cos2 = cos3

            'dataOut = dataOut + dataIn(k) * cos3 - imagIn(k) * sin3
            'imagOut = imagOut + imagIn(k) * cos3 + dataIn(k) * sin3

            dataOut.R += dataIn(k).R * cos3 - dataIn(k).I * sin3
            dataOut.I += dataIn(k).I * cos3 + dataIn(k).R * sin3
        Next
    End Sub

    ' http://mathworld.wolfram.com/ApodizationFunction.html
    ' http://www.hulinks.co.jp/support/flexpro/v7/dataanalysis_tapering.html
    ' http://www.phon.ucl.ac.uk/home/mark/basicdsp/
    Public Function ApplyWindow(i As Integer, windowSize As FFTSizeConstants, windowType As FFTWindowConstants) As Double
        Dim w As Integer = CInt(windowSize) - 1

        Select Case windowType
            Case FFTWindowConstants.None
                Return 1.0
            Case FFTWindowConstants.Triangle
                Return 1.0 - Abs(1.0 - ((2 * i) / w))
            Case FFTWindowConstants.Hanning
                Return (0.5 * (1.0 - Cos(PI2 * i / w)))
            Case FFTWindowConstants.Hamming
                Return 0.54 - 0.46 * Cos(PI2 * i / w)
            Case FFTWindowConstants.Welch
                Return 1.0 - (i - 0.5 * (w - 1)) / (0.5 * (w + 1) ^ 2)
            Case FFTWindowConstants.Gaussian
                'Return Math.E ^ (-6.25 * PI * i ^ 2 / w ^ 2)
                Return Math.E ^ (-6.25 * PI * i * i / (w * w))
            Case FFTWindowConstants.Blackman
                Return 0.42 - 0.5 * Cos(PI2 * i / w) + 0.08 * Cos(2 * PI2 * i / w)
            Case FFTWindowConstants.Parzen
                Return 1.0 - Abs((i - 0.5 * w) / (0.5 * (w + 1)))
            Case FFTWindowConstants.Bartlett
                Return 1.0 - Abs(i) / w
            Case FFTWindowConstants.Connes
                'Return (1.0 - i ^ 2 / w ^ 2) ^ 2
                Return (1.0 - i * i / (w * w)) ^ 2
            Case FFTWindowConstants.KaiserBessel
                If i >= 0 And i <= w / 2 Then
                    Return Bessel((w / 2) * (Sqrt(1 - 2 * i / w) ^ 2)) / Bessel(w / 2)
                Else
                    Return 0.0
                End If
            Case FFTWindowConstants.BlackmanHarris
                Return 0.35875 - 0.48829 * Cos(PI2 * i / w) + 0.14128 * Cos(2 * PI2 * i / w) - 0.01168 * Cos(3 * PI * i / w)
            Case FFTWindowConstants.Nuttall
                Return 0.355768 - 0.487396 * Cos(PI2 * i) / w + 0.144232 * Cos(2 * PI2 * i) / w - 0.012604 * Cos(3 * PI2 * i) / w
            Case FFTWindowConstants.BlackmanNuttall
                Return 0.3635819 - 0.4891775 * Cos(PI2 * i) / w + 0.1365995 * Cos(2 * PI2 * i) / w - 0.0106411 * Cos(3 * PI2 * i) / w
            Case FFTWindowConstants.FlatTop
                Return 1.0 - 1.93 * Cos(PI2 * i) / w + 1.29 * Cos(2 * PI2 * i) / w - 0.388 * Cos(3 * PI2 * i) / w + 0.032 * Cos(4 * PI2 * i) / w
        End Select
    End Function

    Public Function GetWindowValues(windowSize As FFTSizeConstants, windowType As FFTWindowConstants) As Double()
        Dim values(windowSize - 1) As Double
        For i As Integer = 0 To windowSize - 1
            values(i) = ApplyWindow(i, windowSize, windowType)
        Next
        Return values
    End Function

    Public Function GetWindowSum(windowSize As FFTSizeConstants, windowType As FFTWindowConstants) As Double
        Dim sum As Double = 0
        For i As Integer = 0 To windowSize - 1
            sum += ApplyWindow(i, windowSize, windowType)
        Next
        Return sum
    End Function

    Public Function AWeighting(freq As Double) As Double
        If freq > 0 Then
            'Dim f2 = freq ^ 2
            'Dim f4 = freq ^ 4
            Dim f2 = freq * freq
            Dim f4 = freq * freq * freq * freq
            Return 10 * Log(1.562339 * f4 / ((f2 + 107.65265 ^ 2) * (f2 + 737.86223 ^ 2))) / Log(10) + 10 * Log(2.242881E+16 * f4 / ((f2 + 20.598997 ^ 2) ^ 2 * (f2 + 12194.22 ^ 2) ^ 2)) / Log(10)
        Else
            Return Double.MinValue
        End If
    End Function

    Private Function Bessel(x As Double) As Double
        Dim r As Double = 1
        For l As Integer = 0 To 2
            r += ((x / 2) ^ (2 * l)) / (Fact(l)) ^ 2
        Next l
        Return r
    End Function

    Private Function Fact(x As Integer) As Integer
        Dim n As Integer = 1
        For i As Integer = 1 To x
            n *= i
        Next
        Return n
    End Function

    'http://w3.msi.vxu.se/exarb/mj_ex.pdf
    Public Function HilbertTransform() As Double()
        Static isInit As Boolean
        Static h(LH) As Double

        If Not isInit Then
            Dim taper As Double

            h(LHHALF) = 0.0
            For i As Integer = 1 To LHHALF - 1
                taper = 0.54 + 0.46 * Cos(PI * i / LHHALF)
                h(LHHALF + i) = taper * (-(i Mod 2) * 2.0 / (PI * (i)))
                h(LHHALF - i) = -h(LHHALF + i)
            Next

            isInit = True
        End If

        Return h
    End Function

    Public Function HilbertTransform(x() As Double)
        Return HilbertTransform(ComplexDouble.FromDouble(x))
    End Function

    Public Function HilbertTransform(x() As ComplexDouble)
        Dim n As Integer = x.Length
        Dim z(n - 1) As ComplexDouble
        Dim temp As Double
        Dim i As Integer

        For i = 0 To n - 1
            z(i) = New ComplexDouble(x(i).R, 0)
        Next

        z = FFT1r(False, z, n)
        Dim k As Integer = n / 2
        z(0).R = 0
        z(0).I = 0
        z(k).R = 0
        z(k).I = 0

        For i = 1 To k - 1
            temp = z(i).R
            z(i).R = -z(i).I
            z(i).I = temp
        Next

        For i = k + 1 To n - 1
            temp = -z(i).R
            z(i).R = -z(i).I
            z(i).I = temp
        Next

        'z = FFT1r(True, z, n)
        'For i = 0 To n - 1
        '    x(i) = z(i).R
        'Next

        Return FFT1r(True, z, n)
    End Function

    'Public Sub Convolute(lx As Integer, ifx As Integer, x() As Double, ly As Integer, ify As Integer, y() As Double, lz As Integer, ifz As Integer, z() As Double)
    '    ' Very Simple Convolution
    '    Dim ilx As Integer = ifx + lx - 1
    '    Dim ily As Integer = ify + ly - 1
    '    Dim ilz As Integer = ifz + lz - 1
    '    Dim jlow As Integer
    '    Dim jhigh As Integer
    '    Dim sum As Double = 0

    '    For i As Integer = ifz To ilz - 1
    '        jlow = (i + 1) - ily : If jlow < ifx Then jlow = ifx
    '        jhigh = (i + 1) - ify : If jhigh > ilx Then jhigh = ilx
    '        For j As Integer = jlow To jhigh - 1
    '            sum += x(j + 1 - ifx) * y((i + 1) - (j + 1) - ify)
    '        Next
    '        z(i + 1 - ifz) = sum
    '    Next
    'End Sub

    ' http://www.mpi-hd.mpg.de/astrophysik/HEA/internal/Numerical_Recipes/f13-1.pdf
    Public Sub Convolute(data() As Double, n As Integer, respns() As Double, m As Integer, isign As Integer, ans() As Double)
        Dim i As Integer
        Dim no2 As Integer

        Dim dum As Double
        Dim mag2 As Double
        Dim fft() As Double = Vector(Of Double)(1, n << 1)

        For i = 1 To (m - 1) \ 2 : respns(n + 1 - i) = respns(m + 1 - i) : Next
        For i = (m + 3) \ 2 To n - (m - 1) \ 2 : respns(i) = 0.0 : Next
        TwoFFT(data, respns, fft, ans, n)
        no2 = n \ 2
        For i = 2 To n + 2 Step 2
            If isign = 1 Then
                dum = ans(i - 1)
                ans(i - 1) = (fft(i - 1) * dum - fft(i) * ans(i)) / no2
                ans(i) = (fft(i) * dum + fft(i - 1) * ans(i)) / no2
            ElseIf isign = -1 Then
                mag2 = Sqrt(ans(i - 1)) + Sqrt(ans(i))
                If mag2 = 0.0 Then Throw New Exception("Deconvolving at response zero in Convolute")

                dum = ans(i - 1)
                ans(i - 1) = (fft(i - 1) * dum + fft(i) * ans(i)) / mag2 / no2
                ans(i) = (fft(i) * dum - fft(i - 1) * ans(i)) / mag2 / no2
            Else
                Throw New Exception("No meaning for isign in Convolute")
            End If
        Next

        ans(2) = ans(n + 1)
        RealFT(ans, n, -1)
    End Sub

    Public Function TriangularSmooth(y() As Double) As Double()
        Dim s(y.Length - 1) As Double

        For i As Integer = 3 To y.Length - 1 - 2
            s(i) = (y(i - 2) + 2 * y(i - 1) + 3 * y(i) + 2 * y(i + 1) + y(i + 2)) / 9
        Next

        Return s
    End Function

    'http://www.fizyka.umk.pl/nrbook/c14-8.pdf (dead link)
    'http://www.vias.org/tmdatanaleng/cc_filter_savgolay.html
    'http://ib.cnea.gov.ar/~fiscom/Libreria/NumRec/C/savgol.c

    ' c()   = DataPoints
    ' np    = Size?
    ' nl    = Number of leftward (past) data points
    ' np    = Number of leftward (past) data points
    ' nr    = Number of rightward (future) data points
    ' ld    = Order of the derivative desired (e.g., ld = 0 for smoothed function)
    ' m     = order of the smoothing polynomial, also equal to the highest conserved moment; usual values are m = 2 or m = 4.
    Public Sub SavitzkyGolay(c() As Double, np As Integer, nl As Integer, nr As Integer, ld As Integer, m As Integer)
        Dim imj As Integer
        Dim ipj As Integer
        Dim j As Integer
        Dim k As Integer
        Dim kk As Integer
        Dim mm As Integer
        Dim indx() As Integer

        Dim d As Double
        Dim fac As Double
        Dim sum As Double
        Dim a()() As Double
        Dim b() As Double

        If np < nl + nr + 1 OrElse nl < 0 OrElse nr < 0 OrElse ld > m OrElse nl + nr < m Then
            Throw New Exception("Invalid Arguments")
        End If

        indx = Vector(Of Integer)(1, m + 1)
        a = Matrix(1, m + 1, 1, m + 1)
        b = Vector(Of Double)(1, m + 1)

        For ipj = 0 To 2 * m
            sum = If(ipj <> 0, 0.0, 1.0)
            For k = 1 To nr : sum += k ^ ipj : Next
            For k = 1 To nl : sum += (-k) ^ ipj : Next

            mm = Min(ipj, 2 * m - ipj)
            For imj = -mm To mm Step 2 : a(1 + (ipj + imj) / 2)(1 + (ipj - imj) / 2) = sum : Next
        Next

        d = Ludcmp(a, m + 1, indx)
        For j = 1 To m + 1 : b(j) = 0.0 : Next
        b(ld + 1) = 1.0
        Lubksb(a, m + 1, indx, b)
        'For kk = 1 To np : c(kk) = 0.0 : Next
        For k = -nl To nr
            sum = b(1)
            fac = 1.0
            For mm = 1 To m
                fac *= k
                sum += b(mm + 1) * fac
            Next
            kk = ((np - k) Mod np) + 1
            c(kk) = sum
        Next
    End Sub

#Region "Helper Functions"
    Private Const NR_END As Integer = 1
    Private Const TINY As Double = Double.MinValue

    Private Function Matrix(nrl As Integer, nrh As Integer, ncl As Integer, nch As Integer) As Double()()
        Dim nrow As Integer = nrh - nrl + 1
        Dim ncol As Integer = nch - ncl + 1

        Dim m(nrow + NR_END)() As Double
        For r As Integer = 0 To ncol
            ReDim m(r)(nrow * ncol + NR_END)
        Next

        Return m
    End Function

    Private Function Vector(Of T)(nl As Integer, nh As Integer) As T()
        Dim v(nh - nl + 1 + NR_END) As T
        Return v
    End Function

    Private Sub Lubksb(a()() As Double, n As Integer, indx() As Integer, b() As Double)
        Dim i As Integer
        Dim ii As Integer = 0
        Dim ip As Integer
        Dim j As Integer

        Dim sum As Double

        For i = 1 To n
            ip = indx(i)
            sum = b(ip)
            b(ip) = b(i)
            If ii <> 0 Then
                For j = ii To i - 1 : sum -= a(i)(j) * b(j) : Next
            Else
                If sum <> 0 Then ii = i
            End If
            b(i) = sum
        Next

        For i = n To 1 Step -1
            sum = b(i)
            For j = i + 1 To n : sum -= a(i)(j) * b(j) : Next
            b(i) = sum / a(i)(i)
        Next
    End Sub

    Private Function Ludcmp(a As Double()(), n As Integer, indx() As Integer) As Double
        Dim i As Integer
        Dim imax As Integer
        Dim j As Integer
        Dim k As Integer

        Dim big As Double
        Dim dum As Double
        Dim sum As Double
        Dim temp As Double
        Dim vv() As Double = Vector(Of Double)(1, n)

        Dim d As Double = 1

        For i = 1 To n
            big = 0.0
            For j = 1 To n
                temp = Abs(a(i)(j))
                If temp > big Then big = temp
            Next j
            If big = 0.0 Then Throw New Exception("Singular matrix in routine ludcmp")
            vv(i) = 1.0 / big
        Next

        For j = 1 To n
            For i = 1 To j - 1
                sum = a(i)(j)
                For k = 1 To i - 1 : sum -= a(i)(k) * a(k)(j) : Next
                a(i)(j) = sum
            Next
            big = 0.0
            For i = j To n
                sum = a(i)(j)
                For k = 1 To j - 1 : sum -= a(i)(k) * a(k)(j) : Next
                a(i)(j) = sum
                dum = vv(i) * Abs(sum)
                If dum >= big Then
                    big = dum
                    imax = i
                End If
            Next

            If j <> imax Then
                For k = 1 To n
                    dum = a(imax)(k)
                    a(imax)(k) = a(j)(k)
                    a(j)(k) = dum
                Next
                d = -d
                vv(imax) = vv(j)
            End If

            indx(j) = imax
            If a(j)(j) = 0.0 Then a(j)(j) = TINY
            If j <> n Then
                dum = 1.0 / a(j)(j)
                For i = j + 1 To n : a(i)(j) *= dum : Next
            End If
        Next

        Return d
    End Function

    Private Sub TwoFFT(data1() As Double, data2() As Double, fft1() As Double, fft2() As Double, n As Integer)
        Dim nn2 As Integer = 2 + 2 * n
        Dim nn3 As Integer = 1 + nn2
        Dim jj As Integer = 2
        Dim j As Integer

        Dim rep As Double
        Dim [rem] As Double
        Dim aip As Double
        Dim aim As Double

        For j = 1 To n
            fft1(jj - 1) = data1(j)
            fft1(jj) = data2(j)
            jj += 2
        Next
        Four1(fft1, n, 1)

        fft2(1) = fft1(2)
        fft1(2) = 0.0 : fft2(2) = 0.0
        For j = 3 To n + 1 Step 2
            rep = 0.5 * (fft1(j) + fft1(nn2 - j))
            [rem] = 0.5 * (fft1(j) - fft1(nn2 - j))
            aip = 0.5 * (fft1(j + 1) + fft1(nn3 - j))
            aim = 0.5 * (fft1(j + 1) - fft1(nn3 - j))
            fft1(j) = rep
            fft1(j + 1) = aim
            fft1(nn2 - j) = rep
            fft1(nn3 - j) = -aim
            fft2(j) = aip
            fft2(j + 1) = -[rem]
            fft2(nn2 - j) = aip
            fft2(nn3 - j) = [rem]
        Next
    End Sub

    Private Sub RealFT(data() As Double, n As Integer, isign As Integer)
        Dim i, i1, i2, i3, i4, np3 As Integer
        Const c1 As Double = 0.5
        Dim c2, h1r, h1i, h2r, h2i As Double
        Dim wr, wi, wpr, wpi, wtemp As Double
        Dim theta As Double = PI / (n / 2)

        If isign = 1 Then
            c2 = -0.5
            Four1(data, n \ 2, 1)
        Else
            c2 = 0.5
            theta = -theta
        End If

        wtemp = Sin(0.5 * theta)
        wpr = -2.0 * wtemp * wtemp
        wpi = Sin(theta)
        wr = 1.0 + wpr
        wi = wpi
        np3 = n + 3

        For i = 2 To n \ 4
            i1 = 2 * i - 1
            i2 = 1 + i1
            i3 = np3 - i2
            i4 = 1 + i3
            h1r = c1 * (data(i1) + data(i3))
            h1i = c1 * (data(i2) - data(i4))
            h2r = -c2 * (data(i2) + data(i4))
            h2i = c2 * (data(i1) - data(i3))

            data(i1) = h1r + wr * h2r - wi * h2i
            data(i2) = h1i + wr * h2i + wi * h2r
            data(i3) = h1r - wr * h2r + wi * h2i
            data(i4) = -h1i + wr * h2i + wi * h2r

            wtemp = wr
            wr = wtemp * wpr - wi * wpi + wr
            wi = wi * wpr + wtemp * wpi + wi
        Next

        If isign = 1 Then
            h1r = data(1)
            data(1) = h1r + data(2)
            data(2) = h1r - data(2)
        Else
            h1r = data(1)
            data(1) = c1 * (h1r + data(2))
            data(2) = c1 * (h1r - data(2))
            Four1(data, n >> 1, -1)
        End If
    End Sub

    Private Sub Four1(data() As Double, nn As Integer, isign As Integer)
        Dim n As Integer = nn * 2
        Dim mmax As Integer = 2
        Dim m As Integer
        Dim j As Integer = 1
        Dim istep As Integer
        Dim i As Integer

        Dim wtemp As Double
        Dim wr As Double
        Dim wpr As Double
        Dim wpi As Double
        Dim wi As Double
        Dim theta As Double
        Dim tempr As Double
        Dim tempi As Double

        For i = 1 To n - 1 Step 2
            If j > i Then
                Swap(data(j), data(i))
                Swap(data(j + 1), data(i + 1))
            End If
            m = n \ 2
            While m >= 2 AndAlso j > m
                j -= m
                m \= 2
            End While
            j += m
        Next

        While n > mmax
            istep = mmax * 2
            theta = isign * (2 * PI / mmax)
            wtemp = Sin(0.5 * theta)
            wpr = -2.0 * wtemp * wtemp
            wpi = Sin(theta)
            wr = 1.0
            wi = 0.0

            For m = 1 To mmax - 2 Step 2
                For i = m To n - istep Step istep
                    j = i + mmax
                    tempr = wr * data(j) - wi * data(j + 1)
                    tempi = wr * data(j + 1) + wi * data(j)
                    data(j) = data(i) - tempr
                    data(j + 1) = data(i + 1) - tempi
                    data(i) += tempr
                    data(i + 1) += tempi
                Next
                wtemp = wr
                wr = wtemp * wpr - wi * wpi + wr
                wi = wi * wpr + wtemp * wpi + wi
            Next
            mmax = istep
        End While
    End Sub

    Private Sub Swap(Of T)(ByRef v1 As T, ByRef v2 As T)
        Dim tmp As T = v1
        v1 = v2
        v2 = tmp
    End Sub
#End Region
End Module
