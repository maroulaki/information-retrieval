using Lucene.Net.Documents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lucengine
{
    public partial class lucengine : Form
    {
        public lucengine()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            indexButton.Enabled = true;
        }

        private void IndexButton_Click(object sender, EventArgs e)
        {
            bool useBM25 = false;
            if (BM25Toggle.Text.Equals("BM25"))
            {
                useBM25 = true;
            }
            Globals.useBM25 = useBM25;
            Indexer.CreateIndex();

            searchPanel.Enabled = true;
            searchBox.Clear();
            indexPanel.Enabled = false;
        }


        private void searchButton_Click(object sender, EventArgs e)
        {
            DisplayResults(searchBox.Text, (int)articleNo.Value);
            resultsPanel.Enabled = true;
        }

        private void articleNo_ValueChanged(object sender, EventArgs e)
        {

        }

        private void DisplayResults(string query, int topNo)
        {
            resultsPanel.SuspendLayout();

            foreach (Control c  in resultsPanel.Controls) c.Dispose();
            resultsPanel.Controls.Clear();

            IEnumerable<Document> results = Searcher.SearchIndex(query, topNo);

            if ((results == null) || !results.Any())
            {
                MessageBox.Show("No articles found.");
                resultsPanel.ResumeLayout();
                return;
            }

            foreach (Document doc in results) {
                Panel articlePanel = DrawArticlePanel(doc);
                resultsPanel.Controls.Add(articlePanel);
            }

            resultsPanel.ResumeLayout();
        }
        private Panel DrawArticlePanel(Document doc)
        {
            string title = doc.Get("title");
            string authors = doc.Get("authors");
            string abstractText = doc.Get("abstract");

            if (abstractText.Length > 200) abstractText = abstractText.Substring(0, 200) + "...";

            // --- Container Panel ---
            Panel panel = new Panel();
            panel.Width = resultsPanel.Width - 25; // Account for scrollbar
            panel.Height = 150; // Fixed height as requested
            panel.BorderStyle = BorderStyle.None;
            panel.Padding = new Padding(5);
            panel.BackColor = Color.Transparent;

            // --- Title Label ---
            Label labelTitle = new Label();
            labelTitle.Text = title;
            labelTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold); // Bold Title
            labelTitle.AutoSize = true; // Let it grow if title is long
            labelTitle.MaximumSize = new Size(panel.Width - 10, 0); // Wrap text
            labelTitle.Location = new Point(5, 5);
            labelTitle.ForeColor = Color.White;

            // --- Author Label ---
            Label lblAuthor = new Label();
            lblAuthor.Text = "By: " + authors;
            lblAuthor.Font = new Font("Segoe UI", 9, FontStyle.Italic);
            lblAuthor.ForeColor = Color.White;
            lblAuthor.AutoSize = true;
            lblAuthor.Location = new Point(5, labelTitle.Bottom + 5); // Place below title

            // --- Abstract RichTextBox ---
            RichTextBox rtbAbstract = new RichTextBox();
            rtbAbstract.Text = abstractText;
            rtbAbstract.ReadOnly = true;
            rtbAbstract.BackColor = Color.DarkSlateGray; // Make it look like a label
            rtbAbstract.BorderStyle = BorderStyle.None;
            rtbAbstract.Location = new Point(5, lblAuthor.Bottom + 10); // Place below author
            rtbAbstract.Width = panel.Width - 15;
            rtbAbstract.Height = panel.Height - rtbAbstract.Top - 5; // Fill remaining space
            rtbAbstract.ScrollBars = RichTextBoxScrollBars.None; // Hide scrolls for cleaner look

            // --- Add Controls to Panel ---
            panel.Controls.Add(labelTitle);
            panel.Controls.Add(lblAuthor);
            panel.Controls.Add(rtbAbstract);

            return panel;
        }
    }
}
