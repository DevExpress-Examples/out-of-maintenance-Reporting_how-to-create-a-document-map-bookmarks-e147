Imports System.Drawing
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraPrinting.Native
' ...


Public Class Form1
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents Button1 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(88, 104)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(88, 40)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Button1"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(292, 266)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.Button1})
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles Button1.Click
        ' Create a new Printing System.
        Dim Ps As New PrintingSystem()

        ' Create a new link and add a handler to its CreateDetailArea event.
        Dim Link As New Link(Ps)
        AddHandler Link.CreateDetailArea, AddressOf OnDocumentCreated

        ' Create a document for the link.
        Link.CreateDocument()

        ' Create a document's map.
        CreateDocumentMap(Ps.Document)

        ' Show the report's preview.
        Link.ShowPreview()

        ' Remove the handler for the CreateDetailArea event.
        RemoveHandler Link.CreateDetailArea, AddressOf OnDocumentCreated
    End Sub

    Private Sub OnDocumentCreated(ByVal Sender As Object, ByVal e As CreateAreaEventArgs)
        ' Draw 5 bricks on a document.
        Dim i As Integer
        For i = 0 To 4
            Dim Brick As New TextBrick()
            Brick.Text = "Item #" + (i + 1).ToString()
            Brick.BackColor = Color.Yellow
            Brick.Rect = New RectangleF(10, i * 100, 100, 50)
            e.Graph.DrawBrick(Brick, Brick.Rect)
        Next
    End Sub

    Private Sub CreateDocumentMap(ByVal document As Document)
        ' Create a collection of 'Page and Brick' pairs.
        Dim Pairs As New BrickPagePairCollection()

        ' Create an enumerator of all document bricks.
        Dim en As New DocumentBrickEnumerator(document)

        ' Add the 'Brick-Page' pairs for all document bricks
        While en.MoveNext()
            Pairs.Add(BrickPagePair.Create(en.Brick, en.Page))
        End While

        ' Sort the collection of pairs.
        Pairs.Sort(BrickPagePairCollection.BrickComparer)

        ' Add bookmarks for all pairs to the document's map.
        Dim Pair As BrickPagePair
        For Each Pair In Pairs
            Dim BrickText As String = CType(Pair.Brick, TextBrick).Text
            Dim mapNode As New BookmarkNode(BrickText, Pair.Brick, Pair.Page)
            document.BookmarkNodes.Add(mapNode)
        Next
    End Sub


End Class
