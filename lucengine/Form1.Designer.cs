namespace lucengine
{
    partial class lucengine
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.logo = new System.Windows.Forms.PictureBox();
            this.indexPanel = new System.Windows.Forms.Panel();
            this.BM25Toggle = new System.Windows.Forms.ComboBox();
            this.indexButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.searchBox = new System.Windows.Forms.TextBox();
            this.searchPanel = new System.Windows.Forms.Panel();
            this.searchButton = new System.Windows.Forms.Button();
            this.exportButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).BeginInit();
            this.indexPanel.SuspendLayout();
            this.searchPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // logo
            // 
            this.logo.Image = global::lucengine.Properties.Resources.logo;
            this.logo.Location = new System.Drawing.Point(12, 12);
            this.logo.Name = "logo";
            this.logo.Size = new System.Drawing.Size(325, 95);
            this.logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logo.TabIndex = 0;
            this.logo.TabStop = false;
            // 
            // indexPanel
            // 
            this.indexPanel.Controls.Add(this.indexButton);
            this.indexPanel.Controls.Add(this.BM25Toggle);
            this.indexPanel.Location = new System.Drawing.Point(842, 31);
            this.indexPanel.Name = "indexPanel";
            this.indexPanel.Size = new System.Drawing.Size(210, 76);
            this.indexPanel.TabIndex = 1;
            // 
            // BM25Toggle
            // 
            this.BM25Toggle.BackColor = System.Drawing.Color.LightCyan;
            this.BM25Toggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BM25Toggle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BM25Toggle.FormattingEnabled = true;
            this.BM25Toggle.Items.AddRange(new object[] {
            "Standard",
            "BM25"});
            this.BM25Toggle.Location = new System.Drawing.Point(17, 19);
            this.BM25Toggle.Name = "BM25Toggle";
            this.BM25Toggle.Size = new System.Drawing.Size(187, 24);
            this.BM25Toggle.TabIndex = 2;
            this.BM25Toggle.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // indexButton
            // 
            this.indexButton.BackColor = System.Drawing.Color.CadetBlue;
            this.indexButton.Enabled = false;
            this.indexButton.FlatAppearance.BorderSize = 0;
            this.indexButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.indexButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.indexButton.ForeColor = System.Drawing.Color.White;
            this.indexButton.Location = new System.Drawing.Point(17, 49);
            this.indexButton.Name = "indexButton";
            this.indexButton.Size = new System.Drawing.Size(187, 24);
            this.indexButton.TabIndex = 3;
            this.indexButton.Text = "Index";
            this.indexButton.UseVisualStyleBackColor = false;
            this.indexButton.Click += new System.EventHandler(this.IndexButton_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Enabled = false;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 178);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1040, 691);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // searchBox
            // 
            this.searchBox.BackColor = System.Drawing.Color.LightCyan;
            this.searchBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchBox.Location = new System.Drawing.Point(3, 3);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(272, 22);
            this.searchBox.TabIndex = 3;
            // 
            // searchPanel
            // 
            this.searchPanel.Controls.Add(this.exportButton);
            this.searchPanel.Controls.Add(this.searchButton);
            this.searchPanel.Controls.Add(this.searchBox);
            this.searchPanel.Enabled = false;
            this.searchPanel.Location = new System.Drawing.Point(12, 131);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.Size = new System.Drawing.Size(1040, 41);
            this.searchPanel.TabIndex = 5;
            // 
            // searchButton
            // 
            this.searchButton.BackColor = System.Drawing.Color.CadetBlue;
            this.searchButton.FlatAppearance.BorderSize = 0;
            this.searchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchButton.ForeColor = System.Drawing.Color.White;
            this.searchButton.Location = new System.Drawing.Point(281, 3);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(97, 22);
            this.searchButton.TabIndex = 4;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = false;
            // 
            // exportButton
            // 
            this.exportButton.BackColor = System.Drawing.Color.CadetBlue;
            this.exportButton.Enabled = false;
            this.exportButton.FlatAppearance.BorderSize = 0;
            this.exportButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exportButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exportButton.ForeColor = System.Drawing.Color.White;
            this.exportButton.Location = new System.Drawing.Point(847, 3);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(187, 22);
            this.exportButton.TabIndex = 6;
            this.exportButton.Text = "Export";
            this.exportButton.UseVisualStyleBackColor = false;
            // 
            // lucengine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.ClientSize = new System.Drawing.Size(1064, 881);
            this.Controls.Add(this.searchPanel);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.indexPanel);
            this.Controls.Add(this.logo);
            this.Name = "lucengine";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "lucengine";
            ((System.ComponentModel.ISupportInitialize)(this.logo)).EndInit();
            this.indexPanel.ResumeLayout(false);
            this.searchPanel.ResumeLayout(false);
            this.searchPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox logo;
        private System.Windows.Forms.Panel indexPanel;
        private System.Windows.Forms.ComboBox BM25Toggle;
        private System.Windows.Forms.Button indexButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TextBox searchBox;
        private System.Windows.Forms.Panel searchPanel;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.Button exportButton;
    }
}

