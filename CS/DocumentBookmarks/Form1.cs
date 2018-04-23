#region #reference
using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Native;
// ...
#endregion #reference

namespace DocumentBookmarks {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        #region #code
        private void button1_Click(object sender, EventArgs e) {
            // Create a new Printing System.
            PrintingSystem ps = new PrintingSystem();

            // Create a new link and add a handler to its CreateDetailArea event.
            Link link = new Link(ps);
            link.CreateDetailArea += new CreateAreaEventHandler(OnDocumentCreated);

            // Create a document for the link.
            link.CreateDocument();

            // Create a document's map.
            CreateDocumentMap(ps.Document);

            // Show the report's preview.
            link.ShowPreview();

            // Remove the handler for the CreateDetailArea event.
            link.CreateDetailArea -= new CreateAreaEventHandler(OnDocumentCreated);
        }

        private void OnDocumentCreated(object sender, CreateAreaEventArgs e) {
            // Draw 5 bricks on a document.
            for (int i = 0; i < 5; i++) {
                TextBrick brick = new TextBrick();
                brick.Text = "Item #" + (i + 1).ToString();
                brick.BackColor = Color.Yellow;
                brick.Rect = new Rectangle(10, i * 100, 100, 50);
                e.Graph.DrawBrick(brick, brick.Rect);
            }
        }

        private void CreateDocumentMap(Document document) {
            // Create a collection of 'Page and Brick' pairs.
            BrickPagePairCollection pairs = new BrickPagePairCollection();

            // Create an enumerator of all document bricks.
            DocumentBrickEnumerator en = new DocumentBrickEnumerator(document);

            // Add the 'Brick-Page' pairs for all document bricks.
            while (en.MoveNext()) {
                pairs.Add(BrickPagePair.Create(en.Brick, en.Page));
            }

            // Add bookmarks for all pairs to the document's map.
            foreach (BrickPagePair pair in pairs) {
                Page page = document.Pages[pair.PageIndex];

                Brick brick = page.GetBrickByIndices(pair.BrickIndices) as Brick;
                string BrickText = ((TextBrick)brick).Text;
                BookmarkNode mapNode = new BookmarkNode(BrickText, brick, page);
                document.BookmarkNodes.Add(mapNode);
            }
        }
        #endregion #code

    }
}
