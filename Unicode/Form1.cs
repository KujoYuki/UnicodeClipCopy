using System.Text;
using System.Unicode;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace Unicode
{
    public partial class Form1 : Form
    {
        const int ClipBoardCount = 25;
        public Form1()
        {
            InitializeComponent();
            displayPictureBox.AllowDrop = true;
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
            string displayChar = enlargeTextBox.Text[..1];
            int codePoint = char.ConvertToUtf32(displayChar, 0);
            string name = UnicodeInfo.GetName(codePoint);
            selectedUnicodeTextBox.Text = $"U+{codePoint:X4}";
            charaNameTextBox.Text = name;

            // 新しいビットマップを作成
            var bmp = new Bitmap(enlargePictureBox.Width, enlargePictureBox.Width);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(enlargePictureBox.BackColor);

                if (!string.IsNullOrEmpty(enlargeTextBox.Text))
                {
                    // 描画品質の設定
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                    // 余白を確保（10%）
                    float margin = 0.1f;
                    float availableWidth = enlargePictureBox.Width * (1 - margin * 2);
                    float availableHeight = availableWidth;

                    // 初期フォントサイズ
                    float fontSize = Math.Min(enlargePictureBox.Width, enlargePictureBox.Width) * 1.0f;

                    using var format = new StringFormat(System.Drawing.StringFormat.GenericTypographic);
                    format.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;

                    Font? font = null;
                    RectangleF textBounds;

                    // 文字が収まるまでフォントサイズを縮小
                    do
                    {
                        font?.Dispose();
                        font = new Font(enlargeTextBox.Font.FontFamily, fontSize, FontStyle.Regular);

                        // 実際の描画領域を測定
                        CharacterRange[] characterRanges = { new(0, displayChar.Length) };
                        format.SetMeasurableCharacterRanges(characterRanges);
                        Region[] regions = g.MeasureCharacterRanges(displayChar, font,
                            new RectangleF(0, 0, enlargePictureBox.Width * 3, enlargePictureBox.Width * 3), format);
                        textBounds = regions[0].GetBounds(g);
                        regions[0].Dispose();

                        // 文字が収まるかチェック
                        if (textBounds.Width <= availableWidth && textBounds.Height <= availableHeight)
                        {
                            break;
                        }

                        // フォントサイズを5%縮小
                        fontSize *= 0.95f;

                    } while (fontSize > 10); // 最小フォントサイズの制限

                    // 中央に配置するための座標を計算
                    float x = (enlargePictureBox.Width - textBounds.Width) / 2 - textBounds.X;
                    float y = (enlargePictureBox.Width - textBounds.Height) / 2 - textBounds.Y;

                    // 文字を描画
                    using (Brush brush = new SolidBrush(enlargeTextBox.ForeColor))
                    {
                        g.DrawString(displayChar, font, brush, x, y, format);
                    }

                    font?.Dispose();
                }
            }

            enlargePictureBox.Image = bmp;
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
                    catch
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
                        //string text = $"{index}: {textCell.Value.ToString()!}";
                        string text = $"{textCell.Value.ToString()!}";
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

        private void displayPictureBox_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                // 既存の画像を破棄
                if (enlargePictureBox.Image != null)
                {
                    enlargePictureBox.Image.Dispose();
                }

                // ファイルがドロップされた場合
                if (e.Data!.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop)!;
                    if (files.Length > 0)
                    {
                        string filePath = files[0];

                        // 画像ファイルかチェック
                        string ext = Path.GetExtension(filePath).ToLower();
                        if (ext == ".png" || ext == ".jpg")
                        {
                            // ファイルから画像を読み込み
                            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                            {
                                displayPictureBox.Image = Image.FromStream(fs);
                            }

                            // ファイル名がUnicode番地の場合はその文字を自動入力
                            string fileName = Path.GetFileNameWithoutExtension(filePath);
                            fileNameSuffixTextBox.Text = fileName;
                            Clipboard.SetText(fileName);

                        }
                        else
                        {
                            MessageBox.Show("サポートされていない画像形式です。", "エラー",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                // Bitmapが直接ドロップされた場合
                else if (e.Data.GetDataPresent(DataFormats.Bitmap))
                {
                    displayPictureBox.Image = (Image)e.Data.GetData(DataFormats.Bitmap)!;

                    // TextBoxをクリア
                    enlargeTextBox.Text = string.Empty;
                    selectedUnicodeTextBox.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"画像の読み込みに失敗しました: {ex.Message}", "エラー",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void displayPictureBox_DragEnter(object sender, DragEventArgs e)
        {
            // ファイルがドラッグされているか、画像データがドラッグされているかチェック
            if (e.Data!.GetDataPresent(DataFormats.FileDrop) ||
                e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void captureImagesButton_Click(object sender, EventArgs e)
        {
            int minX = displayPictureBox.Left;
            int minY = displayPictureBox.Top;
            int maxX = selectedUnicodeTextBox.Right;
            int maxY = selectedUnicodeTextBox.Bottom;

            int captureWidth = maxX - minX;
            int captureHeight = maxY - minY;

            var bmp = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            //Bitmap bmp = new Bitmap(captureWidth, captureHeight);
            //this.DrawToBitmap(bmp, new Rectangle(minX, minY, captureWidth, captureHeight));
            this.DrawToBitmap(bmp, new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height));
            string unicodeText = selectedUnicodeTextBox.Text;
            string suffix = fileNameSuffixTextBox.Text;
            bmp.Save($"{unicodeText}_{suffix}.png");
        }

        private void fileNameSuffixTextBox_TextChanged(object sender, EventArgs e)
        {
            string input = fileNameSuffixTextBox.Text;
            if (!input.StartsWith("U+")) return;

            // Unicode文字が入力されたら該当の文字を表示する
            string hexPart = input[2..];
            if (int.TryParse(hexPart, System.Globalization.NumberStyles.HexNumber, null, out int codePoint))
            {
                try
                {
                    // コードポイントから文字を取得
                    string character = char.ConvertFromUtf32(codePoint);

                    // enlargeTextBoxに文字を設定（これによりenlargeTextBox_TextChangedが呼ばれる）
                    enlargeTextBox.Text = character;
                }
                catch
                {
                    return; // 何もしない
                }
            }
        }
    }
}
