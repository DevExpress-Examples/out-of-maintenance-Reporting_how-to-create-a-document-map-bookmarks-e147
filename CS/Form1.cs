using System;
using System.Windows.Forms;

using System.Drawing;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Native;
// ...

namespace docBookmarkNodes {
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class Form1 : System.Windows.Forms.Form {
        private System.Windows.Forms.Button button1;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public Form1() {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(88, 88);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 40);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                      this.button1});
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.Run(new Form1());
        }

        private void button1_Click(object sender, System.EventArgs e) {
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

            // Add the 'Brick-Page' pairs for all document bricks
            while (en.MoveNext()) {
                pairs.Add(BrickPagePair.Create(en.Brick, en.Page));
            }

            // Sort the collection of pairs.
            pairs.Sort(BrickPagePairCollection.BrickComparer);

            // Add bookmarks for all pairs to the document's map.
            foreach (BrickPagePair pair in pairs) {
                string brickText = ((TextBrick)pair.Brick).Text;
                BookmarkNode mapNode = new BookmarkNode(brickText, pair.Brick, pair.Page);
                document.BookmarkNodes.Add(mapNode);
            }
        }




    }
}

