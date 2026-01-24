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

            // Clear past results
            foreach (Control c  in resultsPanel.Controls) c.Dispose();
            resultsPanel.Controls.Clear();

            // Get results from Searcher class
            IEnumerable<Document> results = Searcher.SearchIndex(query, topNo);

            if ((results == null) || !results.Any())
            {
                MessageBox.Show("No articles found.");
                resultsPanel.ResumeLayout();
                return;
            }

            // Place panels with each article inside the main results panel
            foreach (Document doc in results) {
                Panel articlePanel = DrawArticlePanel(doc);
                resultsPanel.Controls.Add(articlePanel);
            }

            resultsPanel.ResumeLayout();
        }
        private Panel DrawArticlePanel(Document doc)
        {
            //// Get article info
            //string title = doc.Get("title");
            //string authors = doc.Get("authors");
            

            // Construct panel
            Panel panel = new Panel();
            panel.Width = resultsPanel.Width - 25; 
            panel.Height = 150; 
            panel.BorderStyle = BorderStyle.None;
            panel.Padding = new Padding(5);
            panel.BackColor = Color.Transparent;

            // Title label
            Label labelTitle = new Label();
            labelTitle.Text = doc.Get("title");
            labelTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold); 
            labelTitle.AutoSize = true; 
            labelTitle.MaximumSize = new Size(panel.Width - 10, 0); 
            labelTitle.Location = new Point(5, 5);
            labelTitle.ForeColor = Color.White;
            panel.Controls.Add(labelTitle);

            // Authors Label 
            Label labelAuthors = new Label();
            labelAuthors.Text = string.IsNullOrEmpty(doc.Get("authors")) ? "Unknown Authors" : doc.Get("authors");
            labelAuthors.Font = new Font("Segoe UI", 10);
            labelAuthors.ForeColor = Color.LightGray;
            labelAuthors.AutoSize = true;
            labelAuthors.Location = new Point(5, labelTitle.Bottom + 5);
            panel.Controls.Add(labelAuthors);

            // Abstract text
            RichTextBox textboxAbstract = new RichTextBox();

            string abstractText = doc.Get("abstract");

            // Trim abstract
            if (abstractText.Length > 200) abstractText = abstractText.Substring(0, 200) + "...";
            textboxAbstract.Text = abstractText;
            textboxAbstract.ForeColor = Color.White;
            textboxAbstract.ReadOnly = true;
            textboxAbstract.BackColor = Color.DarkSlateGray; 
            textboxAbstract.BorderStyle = BorderStyle.None;
            textboxAbstract.Location = new Point(5, labelAuthors.Bottom + 10); 
            textboxAbstract.Width = panel.Width - 15;
            // textboxAbstract.Height = panel.Height - textboxAbstract.Top - 5; 
            textboxAbstract.ScrollBars = RichTextBoxScrollBars.None; 
            textboxAbstract.Font = new Font("Segoe UI", 12);
            panel.Controls.Add(textboxAbstract);

            //// Add controls to panel and return it
            //panel.Controls.Add(labelTitle);
            //panel.Controls.Add(labelAuthors);
            //panel.Controls.Add(textboxAbstract);

            return panel;
        }
    }
}
