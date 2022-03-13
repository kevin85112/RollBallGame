namespace RollBallGame
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button_start = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label_score = new System.Windows.Forms.Label();
            this.textBox_PlayArea = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_start
            // 
            this.button_start.Font = new System.Drawing.Font("Microsoft YaHei UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_start.Location = new System.Drawing.Point(34, 30);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(100, 40);
            this.button_start.TabIndex = 0;
            this.button_start.Text = "Start";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox2.Location = new System.Drawing.Point(32, 100);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(540, 12);
            this.textBox2.TabIndex = 2;
            // 
            // label_score
            // 
            this.label_score.AutoSize = true;
            this.label_score.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_score.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label_score.Location = new System.Drawing.Point(160, 34);
            this.label_score.Name = "label_score";
            this.label_score.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label_score.Size = new System.Drawing.Size(111, 30);
            this.label_score.TabIndex = 3;
            this.label_score.Text = "Score : 0";
            this.label_score.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox_PlayArea
            // 
            this.textBox_PlayArea.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_PlayArea.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textBox_PlayArea.Font = new System.Drawing.Font("新細明體", 1.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox_PlayArea.Location = new System.Drawing.Point(32, 130);
            this.textBox_PlayArea.Name = "textBox_PlayArea";
            this.textBox_PlayArea.ReadOnly = true;
            this.textBox_PlayArea.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.textBox_PlayArea.Size = new System.Drawing.Size(540, 400);
            this.textBox_PlayArea.TabIndex = 4;
            this.textBox_PlayArea.Text = "";
            this.textBox_PlayArea.WordWrap = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(32, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(540, 100);
            this.label1.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(604, 562);
            this.Controls.Add(this.label_score);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.textBox_PlayArea);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Roll Ball Game";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.TextBox textBox2;
        public System.Windows.Forms.Label label_score;
        public System.Windows.Forms.RichTextBox textBox_PlayArea;
        private System.Windows.Forms.Label label1;
    }
}

