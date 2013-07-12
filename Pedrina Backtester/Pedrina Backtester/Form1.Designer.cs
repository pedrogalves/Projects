namespace Pedrina_Backtester
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.bAbrirArquivo = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.bLoadHistorico = new System.Windows.Forms.Button();
            this.bTestar = new System.Windows.Forms.Button();
            this.bGravaBacktest = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbHorarioEntrada = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.cbHorarioSaida = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.zedGraphControlz = new ZedGraph.ZedGraphControl();
            this.button2 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btPreviousChart = new System.Windows.Forms.Button();
            this.btNextChart = new System.Windows.Forms.Button();
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.buttonBF = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // bAbrirArquivo
            // 
            this.bAbrirArquivo.Location = new System.Drawing.Point(581, 210);
            this.bAbrirArquivo.Name = "bAbrirArquivo";
            this.bAbrirArquivo.Size = new System.Drawing.Size(82, 23);
            this.bAbrirArquivo.TabIndex = 0;
            this.bAbrirArquivo.Text = "Abrir";
            this.bAbrirArquivo.UseVisualStyleBackColor = true;
            this.bAbrirArquivo.Click += new System.EventHandler(this.bAbrirArquivo_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 212);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(569, 20);
            this.textBox1.TabIndex = 1;
            // 
            // bLoadHistorico
            // 
            this.bLoadHistorico.Location = new System.Drawing.Point(581, 181);
            this.bLoadHistorico.Name = "bLoadHistorico";
            this.bLoadHistorico.Size = new System.Drawing.Size(82, 23);
            this.bLoadHistorico.TabIndex = 2;
            this.bLoadHistorico.Text = "Load";
            this.bLoadHistorico.UseVisualStyleBackColor = true;
            this.bLoadHistorico.Click += new System.EventHandler(this.bLoadHistorico_Click);
            // 
            // bTestar
            // 
            this.bTestar.Location = new System.Drawing.Point(581, 152);
            this.bTestar.Name = "bTestar";
            this.bTestar.Size = new System.Drawing.Size(82, 23);
            this.bTestar.TabIndex = 3;
            this.bTestar.Text = "Testar";
            this.bTestar.UseVisualStyleBackColor = true;
            this.bTestar.Click += new System.EventHandler(this.bTestar_Click);
            // 
            // bGravaBacktest
            // 
            this.bGravaBacktest.Location = new System.Drawing.Point(581, 123);
            this.bGravaBacktest.Name = "bGravaBacktest";
            this.bGravaBacktest.Size = new System.Drawing.Size(82, 23);
            this.bGravaBacktest.TabIndex = 4;
            this.bGravaBacktest.Text = "Gravar";
            this.bGravaBacktest.UseVisualStyleBackColor = true;
            this.bGravaBacktest.Click += new System.EventHandler(this.bGravaBacktest_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Regras Backtest";
            // 
            // cbHorarioEntrada
            // 
            this.cbHorarioEntrada.FormattingEnabled = true;
            this.cbHorarioEntrada.Location = new System.Drawing.Point(9, 55);
            this.cbHorarioEntrada.Name = "cbHorarioEntrada";
            this.cbHorarioEntrada.Size = new System.Drawing.Size(121, 21);
            this.cbHorarioEntrada.TabIndex = 6;
            this.cbHorarioEntrada.Text = "Horário de Entrada";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Horário de Entrada";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(227, 56);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker1.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Horário de Saída";
            // 
            // cbHorarioSaida
            // 
            this.cbHorarioSaida.FormattingEnabled = true;
            this.cbHorarioSaida.Location = new System.Drawing.Point(9, 95);
            this.cbHorarioSaida.Name = "cbHorarioSaida";
            this.cbHorarioSaida.Size = new System.Drawing.Size(121, 21);
            this.cbHorarioSaida.TabIndex = 9;
            this.cbHorarioSaida.Text = "Horário de Saída";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(239, 123);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 38);
            this.button1.TabIndex = 11;
            this.button1.Text = "Resultado Estratégia";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // zedGraphControlz
            // 
            this.zedGraphControlz.Location = new System.Drawing.Point(0, 0);
            this.zedGraphControlz.Name = "zedGraphControlz";
            this.zedGraphControlz.ScrollGrace = 0;
            this.zedGraphControlz.ScrollMaxX = 0;
            this.zedGraphControlz.ScrollMaxY = 0;
            this.zedGraphControlz.ScrollMaxY2 = 0;
            this.zedGraphControlz.ScrollMinX = 0;
            this.zedGraphControlz.ScrollMinY = 0;
            this.zedGraphControlz.ScrollMinY2 = 0;
            this.zedGraphControlz.Size = new System.Drawing.Size(150, 150);
            this.zedGraphControlz.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(351, 123);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 12;
            this.button2.Text = "Gráfico";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(730, 487);
            this.tabControl1.TabIndex = 13;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.buttonBF);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.bAbrirArquivo);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.textBox1);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.bLoadHistorico);
            this.tabPage1.Controls.Add(this.cbHorarioSaida);
            this.tabPage1.Controls.Add(this.bTestar);
            this.tabPage1.Controls.Add(this.dateTimePicker1);
            this.tabPage1.Controls.Add(this.bGravaBacktest);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.cbHorarioEntrada);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(722, 461);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btPreviousChart);
            this.tabPage2.Controls.Add(this.btNextChart);
            this.tabPage2.Controls.Add(this.zedGraphControl1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(722, 461);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btPreviousChart
            // 
            this.btPreviousChart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btPreviousChart.Location = new System.Drawing.Point(681, 3);
            this.btPreviousChart.Name = "btPreviousChart";
            this.btPreviousChart.Size = new System.Drawing.Size(20, 23);
            this.btPreviousChart.TabIndex = 2;
            this.btPreviousChart.Text = "<";
            this.btPreviousChart.UseVisualStyleBackColor = true;
            this.btPreviousChart.Click += new System.EventHandler(this.btPreviousChart_Click);
            // 
            // btNextChart
            // 
            this.btNextChart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btNextChart.Location = new System.Drawing.Point(702, 3);
            this.btNextChart.Name = "btNextChart";
            this.btNextChart.Size = new System.Drawing.Size(20, 23);
            this.btNextChart.TabIndex = 1;
            this.btNextChart.Text = ">";
            this.btNextChart.UseVisualStyleBackColor = true;
            this.btNextChart.Click += new System.EventHandler(this.btNextChart_Click);
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphControl1.Location = new System.Drawing.Point(3, 3);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0;
            this.zedGraphControl1.ScrollMaxX = 0;
            this.zedGraphControl1.ScrollMaxY = 0;
            this.zedGraphControl1.ScrollMaxY2 = 0;
            this.zedGraphControl1.ScrollMinX = 0;
            this.zedGraphControl1.ScrollMinY = 0;
            this.zedGraphControl1.ScrollMinY2 = 0;
            this.zedGraphControl1.Size = new System.Drawing.Size(716, 455);
            this.zedGraphControl1.TabIndex = 0;
            // 
            // buttonBF
            // 
            this.buttonBF.Location = new System.Drawing.Point(581, 93);
            this.buttonBF.Name = "buttonBF";
            this.buttonBF.Size = new System.Drawing.Size(82, 23);
            this.buttonBF.TabIndex = 13;
            this.buttonBF.Text = "Testar BF";
            this.buttonBF.UseVisualStyleBackColor = true;
            this.buttonBF.Click += new System.EventHandler(this.bTestarBF_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 487);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button bAbrirArquivo;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button bLoadHistorico;
        private System.Windows.Forms.Button bTestar;
        private System.Windows.Forms.Button bGravaBacktest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbHorarioEntrada;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbHorarioSaida;
        private System.Windows.Forms.Button button1;
        private ZedGraph.ZedGraphControl zedGraphControlz;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private ZedGraph.ZedGraphControl zedGraphControl1;
        private System.Windows.Forms.Button btPreviousChart;
        private System.Windows.Forms.Button btNextChart;
        private System.Windows.Forms.Button buttonBF;
    }
}

