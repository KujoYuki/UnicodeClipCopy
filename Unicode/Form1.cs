using System.Text;
using System.Windows.Forms;

namespace Unicode
{
    public partial class Form1 : Form
    {
        const int ClipBoardCount = 25;
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 選択した文字を大きく表示する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enlargeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (enlargeTextBox.Text.Length < 1)
            {
                // 表示文字をクリア
                enlargePictureBox.Image = null;
                selectedUnicodeTextBox.Text = string.Empty;
                return;
            }

            if (enlargePictureBox.Image != null)
            {
                enlargePictureBox.Image.Dispose();
            }

            // 最初の1文字を取得
            string displayChar = enlargeTextBox.Text.Substring(0, 1);
            // 新しいビットマップを作成
            Bitmap bmp = new Bitmap(enlargePictureBox.Width, enlargePictureBox.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(enlargePictureBox.BackColor);

                if (!string.IsNullOrEmpty(enlargeTextBox.Text))
                {
                    // 描画品質の設定
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                    // 大きなフォントを作成（PictureBoxのサイズに基づく）
                    float fontSize = Math.Min(enlargePictureBox.Width, enlargePictureBox.Height) * 0.7f;
                    using (Font font = new Font(enlargeTextBox.Font.FontFamily, fontSize, FontStyle.Regular))
                    {
                        // 文字のサイズを測定
                        SizeF textSize = g.MeasureString(displayChar, font);

                        // 中央に配置するための座標を計算
                        float x = (enlargePictureBox.Width - textSize.Width) / 2;
                        float y = (enlargePictureBox.Height - textSize.Height) / 2;

                        // 文字を描画
                        using (Brush brush = new SolidBrush(enlargeTextBox.ForeColor))
                        {
                            g.DrawString(displayChar, font, brush, x, y);
                        }
                    }
                }
            }

            enlargePictureBox.Image = bmp;

            int codePoint = char.ConvertToUtf32(displayChar, 0);
            selectedUnicodeTextBox.Text = $"U+{codePoint:X4}";
        }

        /// <summary>
        /// 指定した文字からclipEndIndexTextBoxで指定した文字までをクリップボード履歴にコピーする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void generateAllCoyButton_Click(object sender, EventArgs e)
        {
            copySetDGV.Rows.Clear();

            // 開始・終了コードポイントを取得
            if (!int.TryParse(clipStartIndexTextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out int startCode))
            {
                MessageBox.Show("開始位置が正しい16進数ではありません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 1行あたりのコピー文字数を取得
            int charaCountPerLine = (int)copyCharaCountNumericUpDown.Value;
            if (charaCountPerLine <= 0)
            {
                MessageBox.Show("コピー文字数は1以上を指定してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 指定範囲のUnicode文字を生成
            int endCode = startCode + (charaCountPerLine * ClipBoardCount);
            clipEndIndexTextBox.Text = $"{(endCode - 1):X4}";
            var lines = new List<StringBuilder>();
            for (int i = 0; i < ClipBoardCount; i++)
            {
                lines.Add(new StringBuilder());
            }

            for (int lineCount = 0; lineCount < ClipBoardCount; lineCount++)
            {
                for (int codeCount = 0; codeCount < charaCountPerLine; codeCount++)
                {
                    int codePoint = startCode + charaCountPerLine * lineCount + codeCount;

                    // サロゲートペア範囲をスキップ
                    if (codePoint >= 0xD800 && codePoint <= 0xDFFF)
                        continue;

                    try
                    {
                        string character = char.ConvertFromUtf32(codePoint);
                        lines[lineCount].Append(character);
                    }
                    catch (Exception ex)
                    {
                        // 無効なコードポイントはスキップ
                        continue;
                    }
                }
                copySetDGV.Rows.Add(
                        $"U+{(startCode + charaCountPerLine * lineCount):X4}",
                        lines[lineCount].ToString(),
                        "コピー"
                    );
            }
        }

        private void copySetDGV_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid!.RowHeadersWidth, e.RowBounds.Height);

            e.Graphics.DrawString(rowIdx, grid.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        private void copySetDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // ヘッダーをスキップ
            if (e.RowIndex < 0)
                return;

            // CopyButton列のインデックスを確認
            if (copySetDGV.Columns[e.ColumnIndex].Name == "CopyButton")
            {
                // テキスト列の値を取得
                var textCell = copySetDGV.Rows[e.RowIndex].Cells["TextColumn"];
                if (textCell.Value is not null)
                {
                    try
                    {
                        string index = (e.RowIndex + 1).ToString();
                        string text = $"{index}: {textCell.Value.ToString()!}";
                        Clipboard.SetText(text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"コピーに失敗しました: {ex.Message}", "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void previousCodeButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(clipStartIndexTextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out int startCode))
            {
                MessageBox.Show("開始位置が正しい16進数ではありません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 1行あたりのコピー文字数を取得
            int interval = ClipBoardCount * (int)copyCharaCountNumericUpDown.Value;
            int newStartCode = startCode - interval;
            int endCode = startCode - 1;
            if (newStartCode < 0) return;

            clipStartIndexTextBox.Text = $"{newStartCode:X4}";
            clipEndIndexTextBox.Text = $"{(startCode - 1):X4}";
            generateAllCoyButton_Click(sender, e);
        }

        private void nextCodeButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(clipStartIndexTextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out int startCode))
            {
                MessageBox.Show("開始位置が正しい16進数ではありません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 1行あたりのコピー文字数を取得
            int interval = ClipBoardCount * (int)copyCharaCountNumericUpDown.Value;
            int newStartCode = startCode + interval;
            int endCode = newStartCode + interval;
            if (endCode > int.Parse("FFFF", System.Globalization.NumberStyles.HexNumber)) return;

            clipStartIndexTextBox.Text = $"{newStartCode:X4}";
            clipEndIndexTextBox.Text = $"{(endCode - 1):X4}";
            generateAllCoyButton_Click(sender, e);
        }
    }
}
