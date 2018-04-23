#Region "#reference"
Imports System.Drawing
Imports System.Windows.Forms
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraPrinting.Native
' ...
#End Region

Public Class Form1

#Region "#code"
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Create a new Printing System.
        Dim ps As New PrintingSystem()

        ' Create a new link and add a handler to its CreateDetailArea event.
        Dim link As New Link(ps)
        AddHandler link.CreateDetailArea, AddressOf OnDocumentCreated

        ' Create a document for the link.
        link.CreateDocument()

        ' Create a document's map.
        CreateDocumentMap(ps.Document)

        ' Show the report's preview.
        link.ShowPreview()
    End Sub

    Private Sub OnDocumentCreated(ByVal sender As Object, ByVal e As CreateAreaEventArgs)
        ' Draw 5 bricks on a document.
        For i As Integer = 0 To 4
            Dim brick As New TextBrick()
            brick.Text = "Item #" & (i + 1).ToString()
            brick.BackColor = Color.Yellow
            brick.Rect = New Rectangle(10, i * 100, 100, 50)
            e.Graph.DrawBrick(brick, brick.Rect)
        Next i
    End Sub

    Private Sub CreateDocumentMap(ByVal document As Document)
        ' Create a collection of 'Page and Brick' pairs.
        Dim Pairs As New BrickPagePairCollection()

        ' Create an enumerator of all document bricks.
        Dim en As New DocumentBrickEnumerator(document)

        ' Add the 'Brick-Page' pairs for all document bricks.
        While en.MoveNext()
            Pairs.Add(BrickPagePair.Create(en.Brick, en.Page))
        End While

        ' Add bookmarks for all pairs to the document's map.
        Dim Pair As BrickPagePair
        For Each Pair In Pairs
            Dim page As Page = document.Pages(Pair.PageIndex)
            Dim brick As Brick = CType(page.GetBrickByIndices(Pair.BrickIndices), Brick)
            Dim BrickText As String = CType(brick, TextBrick).Text
            Dim mapNode As New BookmarkNode(BrickText, brick, page)
            document.BookmarkNodes.Add(mapNode)
        Next
    End Sub

#End Region
End Class
