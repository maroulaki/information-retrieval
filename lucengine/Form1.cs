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
            Indexer.CreateIndex(useBM25);

            searchPanel.Enabled = true;
            searchBox.Clear();
            indexPanel.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
