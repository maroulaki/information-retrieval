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
            this.indexPanel = new System.Windows.Forms.Panel();
            this.indexButton = new System.Windows.Forms.Button();
            this.BM25Toggle = new System.Windows.Forms.ComboBox();
            this.resultsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.searchBox = new System.Windows.Forms.TextBox();
            this.searchPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.articleNo = new System.Windows.Forms.NumericUpDown();
            this.exportButton = new System.Windows.Forms.Button();
            this.searchButton = new System.Windows.Forms.Button();
            this.logo = new System.Windows.Forms.PictureBox();
            this.indexPanel.SuspendLayout();
            this.searchPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.articleNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).BeginInit();
            this.SuspendLayout();
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
            // indexButton
            // 
            this.indexButton.BackColor = System.Drawing.Color.CadetBlue;
            this.indexButton.Enabled = false;
            this.indexButton.FlatAppearance.BorderSize = 0;
            this.indexButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.indexButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.indexButton.ForeColor = System.Drawing.Color.White;
            this.indexButton.Location = new System.Drawing.Point(17, 49);
            this.indexButton.Name = "indexButton";
            this.indexButton.Size = new System.Drawing.Size(187, 24);
            this.indexButton.TabIndex = 3;
            this.indexButton.Text = "Index";
            this.indexButton.UseVisualStyleBackColor = false;
            this.indexButton.Click += new System.EventHandler(this.IndexButton_Click);
            // 
            // BM25Toggle
            // 
            this.BM25Toggle.BackColor = System.Drawing.Color.PowderBlue;
            this.BM25Toggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BM25Toggle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BM25Toggle.FormattingEnabled = true;
            this.BM25Toggle.Items.AddRange(new object[] {
            "Standard",
            "BM25"});
            this.BM25Toggle.Location = new System.Drawing.Point(17, 19);
            this.BM25Toggle.Name = "BM25Toggle";
            this.BM25Toggle.Size = new System.Drawing.Size(187, 25);
            this.BM25Toggle.TabIndex = 2;
            this.BM25Toggle.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // resultsPanel
            // 
            this.resultsPanel.AutoScroll = true;
            this.resultsPanel.Enabled = false;
            this.resultsPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.resultsPanel.Location = new System.Drawing.Point(12, 178);
            this.resultsPanel.Name = "resultsPanel";
            this.resultsPanel.Size = new System.Drawing.Size(1040, 691);
            this.resultsPanel.TabIndex = 2;
            this.resultsPanel.WrapContents = false;
            // 
            // searchBox
            // 
            this.searchBox.BackColor = System.Drawing.Color.PowderBlue;
            this.searchBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchBox.Location = new System.Drawing.Point(3, 5);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(272, 25);
            this.searchBox.TabIndex = 3;
            // 
            // searchPanel
            // 
            this.searchPanel.Controls.Add(this.label1);
            this.searchPanel.Controls.Add(this.articleNo);
            this.searchPanel.Controls.Add(this.exportButton);
            this.searchPanel.Controls.Add(this.searchButton);
            this.searchPanel.Controls.Add(this.searchBox);
            this.searchPanel.Enabled = false;
            this.searchPanel.Location = new System.Drawing.Point(12, 131);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.Size = new System.Drawing.Size(1040, 41);
            this.searchPanel.TabIndex = 5;
            this.searchPanel.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(290, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "Top:";
            // 
            // articleNo
            // 
            this.articleNo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.articleNo.Location = new System.Drawing.Point(326, 6);
            this.articleNo.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.articleNo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.articleNo.Name = "articleNo";
            this.articleNo.Size = new System.Drawing.Size(43, 25);
            this.articleNo.TabIndex = 7;
            this.articleNo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.articleNo.ValueChanged += new System.EventHandler(this.articleNo_ValueChanged);
            // 
            // exportButton
            // 
            this.exportButton.BackColor = System.Drawing.Color.CadetBlue;
            this.exportButton.Enabled = false;
            this.exportButton.FlatAppearance.BorderSize = 0;
            this.exportButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exportButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exportButton.ForeColor = System.Drawing.Color.White;
            this.exportButton.Location = new System.Drawing.Point(847, 5);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(187, 24);
            this.exportButton.TabIndex = 6;
            this.exportButton.Text = "Export";
            this.exportButton.UseVisualStyleBackColor = false;
            // 
            // searchButton
            // 
            this.searchButton.BackColor = System.Drawing.Color.CadetBlue;
            this.searchButton.FlatAppearance.BorderSize = 0;
            this.searchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchButton.ForeColor = System.Drawing.Color.White;
            this.searchButton.Location = new System.Drawing.Point(375, 6);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(97, 24);
            this.searchButton.TabIndex = 4;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = false;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // logo
            // 
            this.logo.Image = global::lucengine.Properties.Resources.imageedit_1_4010139320;
            this.logo.Location = new System.Drawing.Point(12, 12);
            this.logo.Name = "logo";
            this.logo.Size = new System.Drawing.Size(325, 95);
            this.logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logo.TabIndex = 0;
            this.logo.TabStop = false;
            // 
            // lucengine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSlateGray;
            this.ClientSize = new System.Drawing.Size(1064, 881);
            this.Controls.Add(this.searchPanel);
            this.Controls.Add(this.resultsPanel);
            this.Controls.Add(this.indexPanel);
            this.Controls.Add(this.logo);
            this.Name = "lucengine";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "lucengine";
            this.indexPanel.ResumeLayout(false);
            this.searchPanel.ResumeLayout(false);
            this.searchPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.articleNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox logo;
        private System.Windows.Forms.Panel indexPanel;
        private System.Windows.Forms.ComboBox BM25Toggle;
        private System.Windows.Forms.Button indexButton;
        private System.Windows.Forms.FlowLayoutPanel resultsPanel;
        private System.Windows.Forms.TextBox searchBox;
        private System.Windows.Forms.Panel searchPanel;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.NumericUpDown articleNo;
        private System.Windows.Forms.Label label1;
    }
}

