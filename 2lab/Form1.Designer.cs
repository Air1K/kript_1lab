namespace _2lab
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
            button1 = new Button();
            button2 = new Button();
            textBox1 = new TextBox();
            label1 = new Label();
            saveFileDialog1 = new SaveFileDialog();
            openFileDialog1 = new OpenFileDialog();
            comboBox1 = new ComboBox();
            button3 = new Button();
            button4 = new Button();
            label2 = new Label();
            button5 = new Button();
            button6 = new Button();
            label3 = new Label();
            label4 = new Label();
            button7 = new Button();
            button8 = new Button();
            comboBox2 = new ComboBox();
            label5 = new Label();
            comboBox3 = new ComboBox();
            comboBox4 = new ComboBox();
            button9 = new Button();
            button10 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(57, 119);
            button1.Name = "button1";
            button1.Size = new Size(125, 29);
            button1.TabIndex = 0;
            button1.Text = "Шифровать";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(57, 153);
            button2.Name = "button2";
            button2.Size = new Size(125, 29);
            button2.TabIndex = 1;
            button2.Text = "Расшифровать";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(57, 52);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(125, 27);
            textBox1.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(57, 29);
            label1.Name = "label1";
            label1.Size = new Size(33, 20);
            label1.TabIndex = 3;
            label1.Text = "Key";
            label1.Click += label1_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "DES-64", "AES" });
            comboBox1.Location = new Point(57, 85);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(125, 28);
            comboBox1.TabIndex = 4;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // button3
            // 
            button3.Location = new Point(223, 120);
            button3.Name = "button3";
            button3.Size = new Size(125, 29);
            button3.TabIndex = 6;
            button3.Text = "Расшифровать";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(223, 86);
            button4.Name = "button4";
            button4.Size = new Size(125, 29);
            button4.TabIndex = 5;
            button4.Text = "Шифровать";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(223, 29);
            label2.Name = "label2";
            label2.Size = new Size(95, 20);
            label2.TabIndex = 7;
            label2.Text = "Generate key";
            label2.Click += label2_Click;
            // 
            // button5
            // 
            button5.Location = new Point(41, 379);
            button5.Name = "button5";
            button5.Size = new Size(121, 29);
            button5.TabIndex = 8;
            button5.Text = "Генерация";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.Location = new Point(41, 446);
            button6.Name = "button6";
            button6.Size = new Size(121, 29);
            button6.TabIndex = 9;
            button6.Text = "Создать";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(41, 356);
            label3.Name = "label3";
            label3.Size = new Size(320, 20);
            label3.TabIndex = 10;
            label3.Text = "Генерация матрицы симметричных ключей ";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(41, 423);
            label4.Name = "label4";
            label4.Size = new Size(390, 20);
            label4.TabIndex = 11;
            label4.Text = "Создание ключевых носителей для  каждого абонента";
            // 
            // button7
            // 
            button7.Location = new Point(650, 379);
            button7.Name = "button7";
            button7.Size = new Size(94, 29);
            button7.TabIndex = 12;
            button7.Text = "button7";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // button8
            // 
            button8.Location = new Point(490, 370);
            button8.Name = "button8";
            button8.Size = new Size(94, 29);
            button8.TabIndex = 13;
            button8.Text = "button8";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Items.AddRange(new object[] { "DES-64", "AES" });
            comboBox2.Location = new Point(223, 52);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(125, 28);
            comboBox2.TabIndex = 14;
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(413, 29);
            label5.Name = "label5";
            label5.Size = new Size(139, 20);
            label5.TabIndex = 15;
            label5.Text = "Шифрование пикч";
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Items.AddRange(new object[] { "DES-64", "AES" });
            comboBox3.Location = new Point(413, 52);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(125, 28);
            comboBox3.TabIndex = 16;
            comboBox3.SelectedIndexChanged += comboBox3_SelectedIndexChanged;
            // 
            // comboBox4
            // 
            comboBox4.FormattingEnabled = true;
            comboBox4.Items.AddRange(new object[] { "ECB", "CBC" });
            comboBox4.Location = new Point(413, 87);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new Size(125, 28);
            comboBox4.TabIndex = 17;
            comboBox4.SelectedIndexChanged += comboBox4_SelectedIndexChanged;
            // 
            // button9
            // 
            button9.Location = new Point(413, 155);
            button9.Name = "button9";
            button9.Size = new Size(125, 29);
            button9.TabIndex = 19;
            button9.Text = "Расшифровать";
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;
            // 
            // button10
            // 
            button10.Location = new Point(413, 121);
            button10.Name = "button10";
            button10.Size = new Size(125, 29);
            button10.TabIndex = 18;
            button10.Text = "Шифровать";
            button10.UseVisualStyleBackColor = true;
            button10.Click += button10_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1444, 334);
            Controls.Add(button9);
            Controls.Add(button10);
            Controls.Add(comboBox4);
            Controls.Add(comboBox3);
            Controls.Add(label5);
            Controls.Add(comboBox2);
            Controls.Add(button8);
            Controls.Add(button7);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(label2);
            Controls.Add(button3);
            Controls.Add(button4);
            Controls.Add(comboBox1);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private TextBox textBox1;
        private Label label1;
        private SaveFileDialog saveFileDialog1;
        private OpenFileDialog openFileDialog1;
        private ComboBox comboBox1;
        private Button button3;
        private Button button4;
        private Label label2;
        private Button button5;
        private Button button6;
        private Label label3;
        private Label label4;
        private Button button7;
        private Button button8;
        private ComboBox comboBox2;
        private Label label5;
        private ComboBox comboBox3;
        private ComboBox comboBox4;
        private Button button9;
        private Button button10;
    }
}