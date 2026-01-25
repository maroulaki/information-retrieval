using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Search.Highlight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheArtOfDev.HtmlRenderer.WinForms;

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
            searchPanel.Visible = true;
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
                MessageBox.Show("No articles found.", "Error");
                resultsPanel.ResumeLayout();
                return;
            }

            // Place panels with each article inside the main results panel
            foreach (Document doc in results) {
                // I parse the query again because it will be needed for highlighting
                QueryParser parser = new QueryParser(Lucene.Net.Util.LuceneVersion.LUCENE_48, "content", Globals.Analyzer);
                Query parsedQuery = parser.Parse(query); 
                Panel articlePanel = DrawArticlePanel(doc, parsedQuery);
                resultsPanel.Controls.Add(articlePanel);
            }

            resultsPanel.ResumeLayout();
        }
        private Panel DrawArticlePanel(Document doc, Query query)
        {
            string titleText = string.IsNullOrEmpty(doc.Get("title")) ? "No Title" : doc.Get("title");
            string authorsText = string.IsNullOrEmpty(doc.Get("authors")) ? "Unknown Authors" : doc.Get("authors");
            string abstractText = string.IsNullOrEmpty(doc.Get("abstract")) ? "No abstract found." : doc.Get("abstract");

           
            titleText = $"<div style='font-family: Segoe UI; font-size: 20px; font-weight: bold; color: white;'>" + HighlightKeywords(titleText, query, false) + "</div>";
            authorsText = $"<div style='font-family: Segoe UI; font-size: 14px; color: #D3D3D3;'>" + HighlightKeywords(authorsText, query, false) + "</div>";
            abstractText = $"<div style='font-family: Segoe UI; font-size: 14px; font-style: italic; color: white;'>" + HighlightKeywords(abstractText, query, true) + "</div>";

            // Construct panel
            Panel panel = new Panel();
            panel.Width = resultsPanel.Width - 25; 
            panel.Height = 50; 
            panel.BorderStyle = BorderStyle.None;
            panel.Padding = new Padding(5);
            panel.BackColor = Color.Transparent;

            // Title label
            HtmlLabel labelTitle = new HtmlLabel();
            labelTitle.Text = titleText;
            labelTitle.AutoSize = true; 
            labelTitle.MaximumSize = new Size(panel.Width - 10, 0); 
            labelTitle.Location = new Point(5, 5);
            labelTitle.Width = panel.Width - 10;
            labelTitle.BackColor = Color.Transparent;
            panel.Controls.Add(labelTitle);

            // Authors Label 
            HtmlLabel labelAuthors = new HtmlLabel();
            labelAuthors.Text = authorsText;
            labelAuthors.BackColor = Color.Transparent;
            labelAuthors.AutoSize = false;
            labelAuthors.Location = new Point(5, labelTitle.Bottom + 5);
            labelAuthors.Width = panel.Width - 10;
            panel.Controls.Add(labelAuthors);

            // Adding a "More like this" button
            Button similarButton = new Button();
            similarButton.FlatStyle = FlatStyle.Flat;
            similarButton.BackColor = Color.CadetBlue;
            similarButton.ForeColor = Color.White;
            similarButton.FlatAppearance.BorderSize = 0;
            similarButton.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            similarButton.Location = new Point(panel.Width - 120, labelAuthors.Bottom + 5);
            similarButton.Text = "More like this";
            similarButton.AutoSize = true;
            // similarButton.Click += DisplaySimilarResults;
            similarButton.BringToFront();
            
            panel.Controls.Add(similarButton);

            // Lucene Highlighter works with HTML output, so I will make HTML label for the abstract
            HtmlLabel labelAbstract = new HtmlLabel();
            labelAbstract.Text = abstractText;
            labelAbstract.AutoSize = false;
            labelAbstract.Height = 100;
            labelAbstract.Width = panel.Width - 15;
            labelAbstract.BackColor = Color.Transparent;
            labelAbstract.Location = new Point(5, labelAuthors.Bottom + 5);
            panel.Controls.Add(labelAbstract);

            panel.Height = labelAbstract.Bottom + 10;
            return panel;
        }

        private string HighlightKeywords(string rawText, Query query, bool isAbstract)
        {
            SimpleHTMLFormatter formatter = new SimpleHTMLFormatter("|||", "///");
            QueryScorer scorer = new QueryScorer(query);
            Highlighter highlighter = new Highlighter(formatter, scorer);
            highlighter.TextFragmenter = new NullFragmenter();

            TokenStream stream = Globals.Analyzer.GetTokenStream("", new StringReader(rawText));
            string highlightedText = highlighter.GetBestFragment(stream, rawText);

            if (string.IsNullOrEmpty(highlightedText)) {
                rawText = rawText.Length > 200 ? rawText.Substring(0, 200) + "..." : rawText;
                return rawText;
            }

            // Cropping the text
            if (isAbstract)
            {
                int keywordEndPosition = highlightedText.IndexOf("|||");
                int start = Math.Max(0, keywordEndPosition - 50);
                int end = Math.Min(highlightedText.Length, keywordEndPosition + 53);
                highlightedText = "..." + highlightedText.Substring(start, end - start) + "...";
            }

            highlightedText = highlightedText.Replace("|||", "<span style='background-color:yellow; font-weight:bold; color:black;'>").Replace("///", "</span>");

            return highlightedText;
        }

        //private void DisplaySimilarResults(string ID)
        //{
        //    resultsPanel.SuspendLayout();

        //    // Clear past results
        //    foreach (Control c in resultsPanel.Controls) c.Dispose();
        //    resultsPanel.Controls.Clear();

        //    // Get results from Searcher class
        //    IEnumerable<Document> similarResults = Searcher.MoreLikeThis(ID);

        //    if ((similarResults == null) || !similarResults.Any())
        //    {
        //        MessageBox.Show("No articles found.", "Error");
        //        resultsPanel.ResumeLayout();
        //        return;
        //    }

        //    // Place panels with each article inside the main results panel
        //    foreach (Document doc in similarResults)
        //    {
        //        // I parse the query again because it will be needed for highlighting
        //        Panel articlePanel = DrawArticlePanel(doc, null);
        //        resultsPanel.Controls.Add(articlePanel);
        //    }

        //    resultsPanel.ResumeLayout();
        //}
    }
}
