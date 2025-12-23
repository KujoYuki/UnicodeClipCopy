namespace Unicode
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            clipStartIndexTextBox = new TextBox();
            label1 = new Label();
            label3 = new Label();
            label2 = new Label();
            clipEndIndexTextBox = new TextBox();
            label4 = new Label();
            label5 = new Label();
            copyCharaCountNumericUpDown = new NumericUpDown();
            copySetDGV = new DataGridView();
            Index = new DataGridViewTextBoxColumn();
            TextColumn = new DataGridViewTextBoxColumn();
            CopyButton = new DataGridViewButtonColumn();
            enlargePictureBox = new PictureBox();
            enlargeTextBox = new TextBox();
            generateButton = new Button();
            selectedUnicodeTextBox = new TextBox();
            contextMenuStrip1 = new ContextMenuStrip(components);
            previousCodeButton = new Button();
            nextCodeButton = new Button();
            ((System.ComponentModel.ISupportInitialize)copyCharaCountNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)copySetDGV).BeginInit();
            ((System.ComponentModel.ISupportInitialize)enlargePictureBox).BeginInit();
            SuspendLayout();
            // 
            // clipStartIndexTextBox
            // 
            clipStartIndexTextBox.Location = new Point(44, 29);
            clipStartIndexTextBox.Name = "clipStartIndexTextBox";
            clipStartIndexTextBox.Size = new Size(100, 23);
            clipStartIndexTextBox.TabIndex = 0;
            clipStartIndexTextBox.Text = "24B0";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(55, 15);
            label1.TabIndex = 1;
            label1.Text = "探索範囲";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(15, 32);
            label3.Name = "label3";
            label3.Size = new Size(23, 15);
            label3.TabIndex = 2;
            label3.Text = "U+";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(185, 32);
            label2.Name = "label2";
            label2.Size = new Size(23, 15);
            label2.TabIndex = 4;
            label2.Text = "U+";
            // 
            // clipEndIndexTextBox
            // 
            clipEndIndexTextBox.Location = new Point(214, 29);
            clipEndIndexTextBox.Name = "clipEndIndexTextBox";
            clipEndIndexTextBox.ReadOnly = true;
            clipEndIndexTextBox.Size = new Size(100, 23);
            clipEndIndexTextBox.TabIndex = 3;
            clipEndIndexTextBox.Text = "27CF";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(150, 32);
            label4.Name = "label4";
            label4.Size = new Size(19, 15);
            label4.TabIndex = 5;
            label4.Text = "～";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(15, 70);
            label5.Name = "label5";
            label5.Size = new Size(129, 15);
            label5.TabIndex = 6;
            label5.Text = "一回あたりのコピー文字数";
            // 
            // copyCharaCountNumericUpDown
            // 
            copyCharaCountNumericUpDown.Location = new Point(150, 68);
            copyCharaCountNumericUpDown.Name = "copyCharaCountNumericUpDown";
            copyCharaCountNumericUpDown.Size = new Size(61, 23);
            copyCharaCountNumericUpDown.TabIndex = 8;
            copyCharaCountNumericUpDown.Value = new decimal(new int[] { 32, 0, 0, 0 });
            // 
            // copySetDGV
            // 
            copySetDGV.AllowUserToAddRows = false;
            copySetDGV.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            copySetDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            copySetDGV.Columns.AddRange(new DataGridViewColumn[] { Index, TextColumn, CopyButton });
            copySetDGV.Location = new Point(12, 97);
            copySetDGV.Name = "copySetDGV";
            copySetDGV.Size = new Size(431, 257);
            copySetDGV.TabIndex = 9;
            copySetDGV.CellContentClick += copySetDGV_CellContentClick;
            copySetDGV.RowPostPaint += copySetDGV_RowPostPaint;
            // 
            // Index
            // 
            Index.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            Index.HeaderText = "番地";
            Index.Name = "Index";
            Index.Width = 56;
            // 
            // TextColumn
            // 
            TextColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            TextColumn.HeaderText = "テキスト";
            TextColumn.Name = "TextColumn";
            // 
            // CopyButton
            // 
            CopyButton.HeaderText = "CopyButton";
            CopyButton.Name = "CopyButton";
            // 
            // enlargePictureBox
            // 
            enlargePictureBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            enlargePictureBox.Location = new Point(470, 136);
            enlargePictureBox.Name = "enlargePictureBox";
            enlargePictureBox.Size = new Size(214, 218);
            enlargePictureBox.TabIndex = 10;
            enlargePictureBox.TabStop = false;
            // 
            // enlargeTextBox
            // 
            enlargeTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            enlargeTextBox.Location = new Point(470, 97);
            enlargeTextBox.Name = "enlargeTextBox";
            enlargeTextBox.Size = new Size(100, 23);
            enlargeTextBox.TabIndex = 11;
            enlargeTextBox.TextChanged += enlargeTextBox_TextChanged;
            // 
            // generateButton
            // 
            generateButton.Location = new Point(354, 68);
            generateButton.Name = "generateButton";
            generateButton.Size = new Size(75, 23);
            generateButton.TabIndex = 12;
            generateButton.Text = "生成";
            generateButton.UseVisualStyleBackColor = true;
            generateButton.Click += generateAllCoyButton_Click;
            // 
            // selectedUnicodeTextBox
            // 
            selectedUnicodeTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            selectedUnicodeTextBox.Location = new Point(587, 97);
            selectedUnicodeTextBox.Name = "selectedUnicodeTextBox";
            selectedUnicodeTextBox.ReadOnly = true;
            selectedUnicodeTextBox.Size = new Size(100, 23);
            selectedUnicodeTextBox.TabIndex = 13;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // previousCodeButton
            // 
            previousCodeButton.Location = new Point(339, 28);
            previousCodeButton.Name = "previousCodeButton";
            previousCodeButton.Size = new Size(75, 23);
            previousCodeButton.TabIndex = 15;
            previousCodeButton.Text = "<< 前";
            previousCodeButton.UseVisualStyleBackColor = true;
            previousCodeButton.Click += previousCodeButton_Click;
            // 
            // nextCodeButton
            // 
            nextCodeButton.Location = new Point(420, 28);
            nextCodeButton.Name = "nextCodeButton";
            nextCodeButton.Size = new Size(75, 23);
            nextCodeButton.TabIndex = 16;
            nextCodeButton.Text = "次 >>";
            nextCodeButton.UseVisualStyleBackColor = true;
            nextCodeButton.Click += nextCodeButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(699, 369);
            Controls.Add(nextCodeButton);
            Controls.Add(previousCodeButton);
            Controls.Add(selectedUnicodeTextBox);
            Controls.Add(generateButton);
            Controls.Add(enlargeTextBox);
            Controls.Add(enlargePictureBox);
            Controls.Add(copySetDGV);
            Controls.Add(copyCharaCountNumericUpDown);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(clipEndIndexTextBox);
            Controls.Add(label3);
            Controls.Add(label1);
            Controls.Add(clipStartIndexTextBox);
            Name = "Form1";
            Text = "UnicodeClip";
            ((System.ComponentModel.ISupportInitialize)copyCharaCountNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)copySetDGV).EndInit();
            ((System.ComponentModel.ISupportInitialize)enlargePictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private Label label1;
        private Label label3;
        private Label label2;
        private TextBox clipEndIndexTextBox;
		private TextBox clipStartIndexTextBox;
		private Label label4;
        private Label label5;
        private NumericUpDown copyCharaCountNumericUpDown;
        private DataGridView copySetDGV;
        private PictureBox enlargePictureBox;
        private TextBox enlargeTextBox;
        private Button generateButton;
        private TextBox selectedUnicodeTextBox;
        private DataGridViewTextBoxColumn Index;
        private DataGridViewTextBoxColumn TextColumn;
        private DataGridViewButtonColumn CopyButton;
        private ContextMenuStrip contextMenuStrip1;
        private Button previousCodeButton;
        private Button nextCodeButton;
    }
}
