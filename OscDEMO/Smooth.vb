Public Class Smooth
    ' https://www.codeproject.com/articles/18936/a-c-implementation-of-douglas-peucker-line-approxi
    ' http://stackoverflow.com/questions/17523240/algorithm-to-smooth-curve

    Public Shared Function DouglasPeuckerReduction(points As List(Of Point), tolerance As Double) As List(Of Point)
        If points Is Nothing OrElse points.Count < 3 Then Return points

        Dim firstPoint As Integer = 0
        Dim lastPoint As Integer = points.Count - 1
        Dim pointIndexsToKeep As New List(Of Integer)()

        'Add the first and last index to the keepers
        pointIndexsToKeep.Add(firstPoint)
        pointIndexsToKeep.Add(lastPoint)

        'The first and the last point cannot be the same
        While points(firstPoint).Equals(points(lastPoint))
            lastPoint -= 1
        End While

        DouglasPeuckerReduction(points, firstPoint, lastPoint, tolerance, pointIndexsToKeep)

        Dim returnPoints As New List(Of Point)()
        pointIndexsToKeep.Sort()
        For Each index As Integer In pointIndexsToKeep
            returnPoints.Add(points(index))
        Next

        Return returnPoints
    End Function

    ''' <summary>
    ''' Douglases the peucker reduction.
    ''' </summary>
    ''' <param name="points">The points.</param>
    ''' <param name="firstPoint">The first point.</param>
    ''' <param name="lastPoint">The last point.</param>
    ''' <param name="tolerance">The tolerance.</param>
    ''' <param name="pointIndexsToKeep">The point index to keep.</param>
    Private Shared Sub DouglasPeuckerReduction(points As List(Of Point), firstPoint As Integer, lastPoint As Integer, tolerance As Double, ByRef pointIndexsToKeep As List(Of Integer))
        Dim maxDistance As Double = 0
        Dim indexFarthest As Integer = 0

        For index As Integer = firstPoint To lastPoint - 1
            Dim distance As Double = PerpendicularDistance(points(firstPoint), points(lastPoint), points(index))
            If distance > maxDistance Then
                maxDistance = distance
                indexFarthest = index
            End If
        Next

        If maxDistance > tolerance AndAlso indexFarthest <> 0 Then
            'Add the largest point that exceeds the tolerance
            pointIndexsToKeep.Add(indexFarthest)

            DouglasPeuckerReduction(points, firstPoint, indexFarthest, tolerance, pointIndexsToKeep)
            DouglasPeuckerReduction(points, indexFarthest, lastPoint, tolerance, pointIndexsToKeep)
        End If
    End Sub

    ''' <summary>
    ''' The distance of a point from a line made from point1 and point2.
    ''' </summary>
    ''' <param name="pt1">The PT1.</param>
    ''' <param name="pt2">The PT2.</param>
    ''' <param name="p">The p.</param>
    ''' <returns></returns>
    Private Shared Function PerpendicularDistance(Point1 As Point, Point2 As Point, Point As Point) As Double
        'Area = |(1/2)(x1y2 + x2y3 + x3y1 - x2y1 - x3y2 - x1y3)|   *Area of triangle
        'Base = v((x1-x2)²+(x1-x2)²)                               *Base of Triangle*
        'Area = .5*Base*H                                          *Solve for height
        'Height = Area/.5/Base

        Dim area As Double = Math.Abs(0.5 * (Point1.X * Point2.Y + Point2.X * Point.Y + Point.X * Point1.Y - Point2.X * Point1.Y - Point.X * Point2.Y - Point1.X * Point.Y))
        Dim dx As Double = Point1.X - Point2.X
        Dim dy As Double = Point1.Y - Point2.Y
        Dim bottom As Double = Math.Sqrt(dx * dx + dy * dy)
        Dim height As Double = area / bottom * 2

        Return height
    End Function
End Class
