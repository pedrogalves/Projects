using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Web.UI.WebControls;
using ZedGraph;

namespace Pedrina_Backtester
{
    public partial class Form1 : Form
    {
        public static int m_iDayChart = 0;
        public static FileInfo fiOpenFile;
        public static string [] szOpenFiles;
        public static DiaNegocio[] dia;
        public static TimeSpan tsInicioTrade = new TimeSpan(10, 00, 0);
        public static TimeSpan tsSaidaTrade = new TimeSpan(17, 00, 0);
        public static TimeSpan[] tsHorariosEntrada = new TimeSpan[] {new TimeSpan(10,0,0), new TimeSpan(10,30,0) };
        public static TimeSpan[] tsHorariosSaida = new TimeSpan[] { new TimeSpan(17, 0, 0), new TimeSpan(17, 30, 0) };
        public Form1()
        {
            InitializeComponent();
        }

        public class Ticker
        {
            public string sAtivo;
            public int iAbertura;
            public int iMaximo;
            public int iMinimo;
            public int iFechamento;
            public DateTime dtData;

        }
        public class MesNegocio
        {
            public DateTime dtMesAno;
            public int iTradeMes = 0;
            public int[] iAcumulado = new int[10];
            public int[] MaxDrawDown = new int[10];
            public DateTime []dtMaxDrawDown = new DateTime[10];
            public int[] MaxDrawUp = new int[10];
            public DateTime[] dtMaxDrawUp = new DateTime[10];
            public DateTime[] dtAtingiu1200 = new DateTime[10];
            public DateTime[] dtAtingiu2000 = new DateTime[10];
            public DateTime[] dtAtingiu4000 = new DateTime[10];
            public DateTime[] dtAtingiu1200neg = new DateTime[10];
            public DateTime[] dtAtingiu2000neg = new DateTime[10];
        }

        public class DiaNegocio
        {
            public DateTime dtData;
            public int iEntradaCompra = 0;
            public int iSaidaCompra = 0;
            public int iEntradaVenda = 100000;
            public int iSaidaVenda = 0;
            public int iMaximoChegou = 0;
            public int iMinChegou = 100000;
            public Ticker[] Ticker = new Ticker[1050];
            public Trade[] TradesDia = new Trade[20];
            public bool bNoMercado = false;
            public bool bHorariodeVerao = false;
            public TimeSpan tsInicioTrade = new TimeSpan(10, 0, 0);
            public TimeSpan tsSaidaTrade = new TimeSpan(17, 0, 0);
        }

        public class Trade
        {
            public DateTime dtHoraEntrada;
            public int iValorEntrada;
            public int iLucroAtual = 0;
            public int iAtingiu200 = 0;
            public int iAtingiu400 = 0;
            public int iAtingiu1000 = 0;
            public int iFechamento = 0;
            public bool bStop;
            public int iMaxTrade;
            public int iMinTrade;
            public tipoTrade tttipoTrade;
            public bool bNoMercado = false;
            public int iValorStop;
            public Alvo[] aAlvosSetados = new Alvo[10];
            public enum tipoTrade { compra, venda };
        }
        public class Alvo
        {
            public DateTime dtHoraSaida;
            public int iValorAlvo;
            public int iValorAtingido=0;
            public bool bStopado = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StreamReader readFile = new StreamReader(@"C:\Users\Pedro\Investimentos\winm12_0512.csv");
            string line;
            string[] lineCampos = new string[7];
            readFile.ReadLine();
            //Ticker [] dados = new Ticker[10000];
            DiaNegocio[] dia = new DiaNegocio[31];
            DateTime data = new DateTime(1, 1, 1, 0, 0, 0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (TimeSpan time = new TimeSpan(9, 0, 0); time < time.Add(TimeSpan.FromHours(2)); time = time.Add(TimeSpan.FromMinutes(15))) 
            {
                cbHorarioEntrada.Items.Add(time.ToString());
            }
        }

        private void bAbrirArquivo_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {
                //szOpenFiles = openFileDialog1.FileNames;
                fiOpenFile = new FileInfo(openFileDialog1.FileName);
                textBox1.Text = fiOpenFile.FullName;
            }
        }

        private void CarregarDadosHistorico()
        {
            StreamReader readFile = new StreamReader(fiOpenFile.FullName);
            string line;
            string[] lineCampos = new string[7];
            readFile.ReadLine();
            //Ticker [] dados = new Ticker[10000];
            dia = new DiaNegocio[300*4];
            DateTime data = new DateTime(1, 1, 1, 0, 0, 0);
            tsInicioTrade = TimeSpan.Parse(cbHorarioEntrada.SelectedItem.ToString());
            tsSaidaTrade = TimeSpan.Parse(cbHorarioSaida.SelectedItem.ToString());

            for (int i = 0, d = -1, t = 0; (line = readFile.ReadLine()) != null; i++)
            {
                lineCampos = line.Split(';');

                if (data.DayOfYear != DateTime.Parse(lineCampos[0]).DayOfYear)
                {
                    data = DateTime.Parse(lineCampos[0] + " " + lineCampos[1]);
                    dia[++d] = new DiaNegocio();
                    i = 0;
                    t = 0;
                    dia[d].dtData = data;
                    if (dia[d].dtData.Hour == 10)
                    {
                        dia[d].bHorariodeVerao = true;
                        dia[d].tsInicioTrade = tsInicioTrade.Add(TimeSpan.FromHours(1));
                        dia[d].tsSaidaTrade = tsSaidaTrade.Add(TimeSpan.FromHours(1));
                    }
                    else
                    {
                        dia[d].tsInicioTrade = tsInicioTrade;
                        dia[d].tsSaidaTrade = tsSaidaTrade;
                    }

                }

                dia[d].Ticker[i] = new Ticker();
                //dia[d].Ticker[i].sAtivo = lineCampos[0];
                dia[d].Ticker[i].dtData = DateTime.Parse(lineCampos[0] + " " + lineCampos[1]);
                //dia[d].Ticker[i].dtData.Date = DateTime.Parse(lineCampos[0]);
                //dia[d].Ticker[i].dtData.TimeOfDay = DateTime.Parse(lineCampos[1]);
                dia[d].Ticker[i].iAbertura = Convert.ToInt32(lineCampos[2]);
                dia[d].Ticker[i].iMaximo = Convert.ToInt32(lineCampos[3]);
                dia[d].Ticker[i].iMinimo = Convert.ToInt32(lineCampos[4]);
                dia[d].Ticker[i].iFechamento = Convert.ToInt32(lineCampos[5]);
                //dia[d].Ticker[i].dtData = DateTime.Parse(lineCampos[5]);
            }
        }

        private void bLoadHistorico_Click(object sender, EventArgs e)
        {
            CarregarDadosHistorico();
        }

        /*private void Page_Load(object sender, EventArgs e)
        {
            // declare a new GridView
            GridView csvGrid = new GridView();

            // add GridView to page
            Form1.Controls.Add(csvGrid);

            // declare csv parser passing in path to file
            using (CsvReader csvData = new CsvReader(Server.MapPath("result.csv")))
            {
                // set GridView to use DataTable returned from parser as the source
                // for it's data. True is passed in to signify that file contains a
                // header row which will name returned DataColumn's based on values
                // read in from header row.
                csvGrid.DataSource = csvData.ReadToEnd(true);

            } // dispose of parser

            // tell GridView to create display based on values from DataSource
            csvGrid.DataBind();
        }*/
        private void resultadoEstrategia()
        {
            int iLucro = 0, iValorEntrada = 0;
            bool NoMercado = false;
            foreach (string fileName in szOpenFiles)
            {
                StreamReader sr = new StreamReader(fileName);
                string szWholeFile = sr.ReadToEnd();
                //char [] strArray = szWholeFile.ToCharArray();
                //Array.Reverse(strArray);
                //string newStr = new string(strArray);
                char [] split = new char[2];
                split[0] = '\n';
                split[1] = '\r';
                string[] fileLines = szWholeFile.Split(split);
                Array.Reverse(fileLines);
                foreach (string linha in fileLines)
                {
                    string[] campos = linha.Split(';');
                    if (campos.Length == 9 && campos[3] == "100")
                    {
                        if (campos[7] == "COMPRADO")
                            iValorEntrada = -Convert.ToInt32(campos[4]);
                        else if (campos[7] == "VENDIDO")
                            iValorEntrada = Convert.ToInt32(campos[4]);
                        else if (campos[7] == "NO DINHEIRO")
                            iLucro += (iValorEntrada < 0) ? (Convert.ToInt32(campos[4]) + iValorEntrada - 10) : (iValorEntrada - Convert.ToInt32(campos[4]) - 10);
                    }

                }
            }

        }

        private void GravarBacktest(string fileName)
        {
            //StreamWriter writeFile = new StreamWriter(@"C:\Users\Pedro\Investimentos\result_" + fiOpenFile);
            StreamWriter writeFile = new StreamWriter(fileName);

      /*      writeFile.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}", "DATA", "Max DrawDown", "Data MxDrawDown", "Max DrawUp", "Data MxDrawUp", "Data 1200", "Data 2000", "Data 4000", "Data -1200", "Data -2000");
            /*
             * public DateTime dtMesAno;
            public int iTradeMes;
            public int MaxDrawDown;
            public DateTime dtMaxDrawDown;
            public int MaxDrawUp;
            public DateTime dtMaxDrawUp;
            DateTime dtAtingiu1200;
            DateTime dtAtingiu2000;
            DateTime dtAtingiu4000;
            DateTime dtAtingiu1200neg;
            DateTime dtAtingiu2000neg;
             */
            DateTime data = new DateTime(1, 1, 1);
            MesNegocio [] resultMes = new MesNegocio[100];

            for(int i = 0, h = -1; dia[i] != null; i++)
            {
                if (/*dia[i].dtData.DayOfYear != data.DayOfYear ||*/ dia[i].dtData.Month != data.Month || dia[i].dtData.Year != data.Year)
                {
                    resultMes[++h] = new MesNegocio();
                    data = dia[i].dtData;
                    resultMes[h].dtMesAno = data;
                }

                for (int j = 0; dia[i].TradesDia[j] != null; j++)
                {
                    resultMes[h].iTradeMes = i + 1;
                    for (int k = 0; dia[i].TradesDia[j].aAlvosSetados[k] != null; k++)
                    {
                        resultMes[h].iAcumulado[k] += dia[i].TradesDia[j].aAlvosSetados[k].iValorAtingido;
                        if (resultMes[h].iAcumulado[k] < resultMes[h].MaxDrawDown[k])
                        {
                            resultMes[h].MaxDrawDown[k] = resultMes[h].iAcumulado[k];
                            resultMes[h].dtMaxDrawDown[k] = dia[i].TradesDia[j].dtHoraEntrada;
                        }
                        if (resultMes[h].iAcumulado[k] > resultMes[h].MaxDrawUp[k])
                        {
                            resultMes[h].MaxDrawUp[k] = resultMes[h].iAcumulado[k];
                            if (resultMes[h].dtMaxDrawUp[k] == null)
                                resultMes[h].dtMaxDrawUp[k] = new DateTime();
                            resultMes[h].dtMaxDrawUp[k] = dia[i].TradesDia[j].dtHoraEntrada;
                        }
                        if (resultMes[h].iAcumulado[k] <= -1200)
                        {
                            if (resultMes[h].dtAtingiu1200neg[k] == null)
                                resultMes[h].dtAtingiu1200neg[k] = new DateTime();
                            resultMes[h].dtAtingiu1200neg[k] = dia[i].TradesDia[j].dtHoraEntrada;
                        }
                        if (resultMes[h].iAcumulado[k] <= -2000)
                        {
                            if (resultMes[h].dtAtingiu2000neg[k] == null)
                                resultMes[h].dtAtingiu2000neg[k] = new DateTime();
                            resultMes[h].dtAtingiu2000neg[k] = dia[i].TradesDia[j].dtHoraEntrada;
                        }
                        if (resultMes[h].iAcumulado[k] >= 1200)
                        {
                            if (resultMes[h].dtAtingiu1200[k] == null)
                                resultMes[h].dtAtingiu1200[k] = new DateTime();
                            resultMes[h].dtAtingiu1200[k] = dia[i].TradesDia[j].dtHoraEntrada;
                        }
                        if (resultMes[h].iAcumulado[k] >= 2000)
                        {
                            if (resultMes[h].dtAtingiu2000[k] == null)
                                resultMes[h].dtAtingiu2000[k] = new DateTime();
                            resultMes[h].dtAtingiu2000[k] = dia[i].TradesDia[j].dtHoraEntrada;
                        }
                        if (resultMes[h].iAcumulado[k] >= 4000)
                        {
                            if (resultMes[h].dtAtingiu4000[k] == null)
                                resultMes[h].dtAtingiu4000[k] = new DateTime();
                            resultMes[h].dtAtingiu4000[k] = dia[i].TradesDia[j].dtHoraEntrada;
                        }
                    }
                }
            }
            
            
            writeFile.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7};{8}", "DATA", "TIPO", "ENTRADA", "STOP", "CHEGOU", "ALVO A", "ALVO B", "ALVO C", "FECHAMENTO");
            for (int d = 0; dia[d] != null; d++)
                for (int t = 0; dia[d].TradesDia[t] != null; t++)
                    writeFile.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7};{8}", dia[d].TradesDia[t].dtHoraEntrada, dia[d].TradesDia[t].tttipoTrade, dia[d].TradesDia[t].iValorEntrada,
                        dia[d].TradesDia[t].bStop, dia[d].TradesDia[t].iMaxTrade, dia[d].TradesDia[t].aAlvosSetados[0].iValorAtingido, dia[d].TradesDia[t].aAlvosSetados[1].iValorAtingido, dia[d].TradesDia[t].aAlvosSetados[2].iValorAtingido,
                        dia[d].TradesDia[t].aAlvosSetados[3].iValorAtingido);
            writeFile.Flush();
            writeFile.Close();
            
            MessageBox.Show("Backtest calculado");
        }

/* 0       private bool ComparaHora(Ticker tTickerAtual, DateTime time)
        {
            TimeSpan timee;
            timee.

            time.

        }*/
        private void CriarPontodeEntradaDia(int d)
        {
            tsInicioTrade = TimeSpan.Parse(cbHorarioEntrada.SelectedItem.ToString());
            for (int i = 0; dia[d] != null && TimeSpan.Compare(dia[d].Ticker[i].dtData.TimeOfDay, dia[d].tsInicioTrade) <= 0; i++)
            {
                if (dia[d].Ticker[i].dtData.Day == 31)
                    i = i;

                if (dia[d].Ticker[i].iMinimo < dia[d].iMinChegou)
                {
                    dia[d].iMinChegou = dia[d].Ticker[i].iMinimo;
                    //dia[d-1].iEntradaVenda = dia[d-1].Ticker[i].iMinimo - 5;
                    dia[d].iEntradaVenda = dia[d].Ticker[i].iMinimo - 5;
                }
                if (dia[d].Ticker[i].iMaximo > dia[d].iMaximoChegou)
                {
                    dia[d].iMaximoChegou = dia[d].Ticker[i].iMaximo;
                    //dia[d].iEntradaCompra = dia[d].Ticker[i].iMaximo + 5;
                    dia[d].iEntradaCompra = dia[d].Ticker[i].iMaximo + 5;
                }
            }
        }
        private void bTestar_Click(object sender, EventArgs e)
        {
            for (int d = 0; dia[d] != null; d++)
            {
                CriarPontodeEntradaDia(d);
                for (int i = 0, t = 0; dia[d].Ticker[i] != null; i++)
                {
                    if (TimeSpan.Compare(dia[d].Ticker[i].dtData.TimeOfDay, dia[d].tsInicioTrade) > 0 && 
                        TimeSpan.Compare(dia[d].Ticker[i].dtData.TimeOfDay, dia[d].tsSaidaTrade) <= 0)
                     {
                        //função VerificaTradeTicker
                        /*if (dia[d].TradesDia[t] == null)
                            dia[d].TradesDia[t] = new Trade();*/
                        //entrada compra
                        if (!dia[d].bNoMercado && (t == 0 || (t > 0 && dia[d].TradesDia[t - 1].tttipoTrade == Trade.tipoTrade.venda)) && ((dia[d].Ticker[i].iMinimo <= dia[d].iEntradaCompra && dia[d].Ticker[i].iMaximo >= dia[d].iEntradaCompra)))
                        {
                            if (dia[d].TradesDia[t] == null)
                            {
                                dia[d].TradesDia[t] = new Trade();
                                dia[d].TradesDia[t].aAlvosSetados[0] = new Alvo();
                                dia[d].TradesDia[t].aAlvosSetados[0].iValorAlvo = 200;
                                dia[d].TradesDia[t].aAlvosSetados[1] = new Alvo();
                                dia[d].TradesDia[t].aAlvosSetados[1].iValorAlvo = 700;
                                dia[d].TradesDia[t].aAlvosSetados[2] = new Alvo();
                                dia[d].TradesDia[t].aAlvosSetados[2].iValorAlvo = 1000;
                                dia[d].TradesDia[t].aAlvosSetados[3] = new Alvo();
                                dia[d].TradesDia[t].aAlvosSetados[3].iValorAlvo = 99999;
                            }
                            dia[d].TradesDia[t].dtHoraEntrada = dia[d].Ticker[i].dtData;
                            dia[d].TradesDia[t].iValorEntrada = dia[d].iEntradaCompra;
                            dia[d].bNoMercado = true;
                            dia[d].iSaidaCompra = (dia[d].iEntradaCompra - dia[d].iEntradaVenda) < 400 ? dia[d].iEntradaVenda : dia[d].iEntradaCompra - 400;
                            //dia[d].iSaidaCompra = (dia[d].iEntradaCompra - dia[d].iEntradaVenda) < 400 ? dia[d].iEntradaVenda : dia[d].iEntradaCompra - 400;
                            dia[d].TradesDia[t].iValorStop = dia[d].iSaidaCompra;
                            dia[d].TradesDia[t].tttipoTrade = Trade.tipoTrade.compra;
                        }
                        //entrada venda
                        if (!dia[d].bNoMercado && (t == 0 || (t > 0 && dia[d].TradesDia[t - 1].tttipoTrade == Trade.tipoTrade.compra)) && ((dia[d].Ticker[i].iMaximo >= dia[d].iEntradaVenda && dia[d].Ticker[i].iMinimo <= dia[d].iEntradaVenda)))
                        {
                            if (dia[d].TradesDia[t] == null)
                            {
                                dia[d].TradesDia[t] = new Trade();
                                dia[d].TradesDia[t].aAlvosSetados[0] = new Alvo();
                                dia[d].TradesDia[t].aAlvosSetados[0].iValorAlvo = 200;
                                dia[d].TradesDia[t].aAlvosSetados[1] = new Alvo();
                                dia[d].TradesDia[t].aAlvosSetados[1].iValorAlvo = 400;
                                dia[d].TradesDia[t].aAlvosSetados[2] = new Alvo();
                                dia[d].TradesDia[t].aAlvosSetados[2].iValorAlvo = 1000;
                                dia[d].TradesDia[t].aAlvosSetados[3] = new Alvo();
                                dia[d].TradesDia[t].aAlvosSetados[3].iValorAlvo = 999999;
                            }
                            dia[d].TradesDia[t].dtHoraEntrada = dia[d].Ticker[i].dtData;
                            dia[d].TradesDia[t].iValorEntrada = dia[d].iEntradaVenda;
                            dia[d].bNoMercado = true;
                            dia[d].iSaidaVenda = (dia[d].iEntradaCompra - dia[d].iEntradaVenda) < 400 ? dia[d].iEntradaCompra : dia[d].iEntradaVenda + 400;
                            //dia[d].iSaidaVenda = (dia[d].iEntradaCompra - dia[d].iEntradaVenda) < 400 ? dia[d].iEntradaCompra : dia[d].iEntradaVenda + 400;
                            dia[d].TradesDia[t].iValorStop = dia[d].iSaidaVenda;
                            dia[d].TradesDia[t].tttipoTrade = Trade.tipoTrade.venda;
                        }
                        //verifica stop/gain
                        if (dia[d].bNoMercado)
                        {
                            //gain 200 ptos compra
                            if (dia[d].TradesDia[t].tttipoTrade == Trade.tipoTrade.compra)
                            {
                                //dia[d].TradesDia[t].aAlvosSetados[a].iValorAtingido = dia[d].Ticker[i].iMaximo - dia[d].TradesDia[t].iValorEntrada;

                                for (int a = 0; dia[d].TradesDia[t].aAlvosSetados[a] != null && !dia[d].TradesDia[t].aAlvosSetados[a].bStopado /*&& dia[d].TradesDia[t].aAlvosSetados[a].iValorAlvo > dia[d].TradesDia[t].aAlvosSetados[a].iValorAtingido*/; a++)
                                {
                                    if (dia[d].TradesDia[t].aAlvosSetados[a].iValorAlvo <= dia[d].TradesDia[t].aAlvosSetados[a].iValorAtingido) continue;
                                    //problema depois que dá o stop gain do primeiro, acertar as condiçoes
                                    //if(t<2 || (t>=2 && !(dia[d].TradesDia[t-1].aAlvosSetados[a].bStopado && dia[d].TradesDia[t-1].aAlvosSetados[a].bStopado)))
                                    //{ 
                                    if (/*dia[d].TradesDia[t].iAtingiu200 == 0 && */(dia[d].Ticker[i].iMinimo <= dia[d].TradesDia[t].iValorStop && dia[d].Ticker[i].iMaximo >= dia[d].TradesDia[t].iValorStop)) 
                                        //if(dia[d].TradesDia[t].iLucroAtual < dia[d].TradesDia[t].iValorStop)
                                        {
                                            dia[d].TradesDia[t].bStop = true;
                                            dia[d].bNoMercado = false;
                                            dia[d].TradesDia[t].aAlvosSetados[a].bStopado = true;
                                            dia[d].TradesDia[t].aAlvosSetados[a].iValorAtingido = dia[d].TradesDia[t].iValorStop - dia[d].TradesDia[t].iValorEntrada;
                                        }
                                        else if (/*dia[d].TradesDia[t].iAtingiu200 == 0 && */(dia[d].Ticker[i].iMinimo <= dia[d].TradesDia[t].iValorEntrada + dia[d].TradesDia[t].aAlvosSetados[a].iValorAlvo && dia[d].Ticker[i].iMaximo >= dia[d].TradesDia[t].iValorEntrada + dia[d].TradesDia[t].aAlvosSetados[a].iValorAlvo))
                                            dia[d].TradesDia[t].aAlvosSetados[a].iValorAtingido = dia[d].TradesDia[t].aAlvosSetados[a].iValorAlvo;
                                        else //if(dia[d].TradesDia[t].iLucroAtual >= dia[d].TradesDia[t].aAlvosSetados[a].iValorAlvo)
                                        {
                                            dia[d].TradesDia[t].aAlvosSetados[a].iValorAtingido = dia[d].Ticker[i].iMaximo - dia[d].TradesDia[t].iValorEntrada;
                                        }
                                    //}
                                }
                                if (dia[d].TradesDia[t].bStop)
                                {
                                    dia[d].bNoMercado = false;
                                    if (dia[d].TradesDia[t].iValorStop - dia[d].TradesDia[t].iValorEntrada >= -400)
                                        i--;
                                    t++;
                                }



                                    //gain 200 ptos compra
                               /* if (dia[d].TradesDia[t].iAtingiu200 == 0 && (dia[d].Ticker[i].iMinimo <= dia[d].TradesDia[t].iValorEntrada + 200 && dia[d].Ticker[i].iMaximo >= dia[d].TradesDia[t].iValorEntrada + 200))
                                    dia[d].TradesDia[t].iAtingiu200 = 200;

                                //gain 400 ptos compra
                                if (dia[d].TradesDia[t].iAtingiu400 == 0 && (dia[d].Ticker[i].iMinimo <= dia[d].TradesDia[t].iValorEntrada + 400 && dia[d].Ticker[i].iMaximo >= dia[d].TradesDia[t].iValorEntrada + 400))
                                    dia[d].TradesDia[t].iAtingiu400 = 400;

                                //gain 1000 ptos compra
                                if (dia[d].TradesDia[t].iAtingiu1000 == 0 && (dia[d].Ticker[i].iMinimo <= dia[d].TradesDia[t].iValorEntrada + 1000 && dia[d].Ticker[i].iMaximo >= dia[d].TradesDia[t].iValorEntrada + 1000))
                                    dia[d].TradesDia[t].iAtingiu1000 = 1000;

                                //stop compra
                                if (dia[d].Ticker[i].iMaximo >= dia[d].TradesDia[t].iValorStop && dia[d].Ticker[i].iMinimo <= dia[d].TradesDia[t].iValorStop)
                                {
                                    dia[d].TradesDia[t].iAtingiu200 = dia[d].TradesDia[t].iAtingiu200 != 0 ? dia[d].TradesDia[t].iAtingiu200 : dia[d].iSaidaCompra - dia[d].TradesDia[t].iValorEntrada;
                                    dia[d].TradesDia[t].iAtingiu400 = dia[d].TradesDia[t].iAtingiu400 != 0 ? dia[d].TradesDia[t].iAtingiu400 : dia[d].iSaidaCompra - dia[d].TradesDia[t].iValorEntrada;
                                    dia[d].TradesDia[t].iAtingiu1000 = dia[d].TradesDia[t].iAtingiu1000 != 0 ? dia[d].TradesDia[t].iAtingiu1000 : dia[d].iSaidaCompra - dia[d].TradesDia[t].iValorEntrada;
                                    dia[d].TradesDia[t].iFechamento = dia[d].TradesDia[t].iFechamento != 0 ? dia[d].TradesDia[t].iFechamento : dia[d].iSaidaCompra - dia[d].TradesDia[t].iValorEntrada;
                                    dia[d].TradesDia[t++].bStop = true;
                                    dia[d].bNoMercado = false;
                                }*/
                            }

                            if (dia[d].bNoMercado && dia[d].TradesDia[t].tttipoTrade == Trade.tipoTrade.venda)
                            {
                                dia[d].TradesDia[t].iLucroAtual = dia[d].TradesDia[t].iValorEntrada - dia[d].Ticker[i].iMinimo;


                                for (int a = 0; dia[d].TradesDia[t].aAlvosSetados[a] != null && !dia[d].TradesDia[t].aAlvosSetados[a].bStopado /*&& dia[d].TradesDia[t].aAlvosSetados[a].iValorAlvo > dia[d].TradesDia[t].aAlvosSetados[a].iValorAtingido*/; a++)
                                {
                                    if (dia[d].TradesDia[t].aAlvosSetados[a].iValorAlvo <= dia[d].TradesDia[t].aAlvosSetados[a].iValorAtingido) continue;
                                    //if(t<2 || (t>=2 && !(dia[d].TradesDia[t-1].aAlvosSetados[a].bStopado && dia[d].TradesDia[t-1].aAlvosSetados[a].bStopado)))
                                    //{   
                                    if ((dia[d].Ticker[i].iMaximo >= dia[d].TradesDia[t].iValorStop && dia[d].Ticker[i].iMinimo <= dia[d].TradesDia[t].iValorStop))
                                    //if(dia[d].TradesDia[t].iLucroAtual < dia[d].TradesDia[t].iValorStop)
                                    {
                                        dia[d].TradesDia[t].bStop = true;
                                        dia[d].bNoMercado = false;
                                        dia[d].TradesDia[t].aAlvosSetados[a].bStopado = true;
                                        dia[d].TradesDia[t].aAlvosSetados[a].iValorAtingido = dia[d].TradesDia[t].iValorEntrada - dia[d].TradesDia[t].iValorStop;
                                    }
                                    else if ((dia[d].Ticker[i].iMaximo >= dia[d].TradesDia[t].iValorEntrada - dia[d].TradesDia[t].aAlvosSetados[a].iValorAlvo && dia[d].Ticker[i].iMinimo <= dia[d].TradesDia[t].iValorEntrada - dia[d].TradesDia[t].aAlvosSetados[a].iValorAlvo))
                                        dia[d].TradesDia[t].aAlvosSetados[a].iValorAtingido = dia[d].TradesDia[t].aAlvosSetados[a].iValorAlvo;
                                    else //if(dia[d].TradesDia[t].iLucroAtual >= dia[d].TradesDia[t].aAlvosSetados[a].iValorAlvo)
                                    {
                                        dia[d].TradesDia[t].aAlvosSetados[a].iValorAtingido = dia[d].TradesDia[t].iValorEntrada - dia[d].Ticker[i].iMinimo;
                                    }
                                    //}
                                }
                                if (dia[d].TradesDia[t].bStop)
                                {
                                    dia[d].bNoMercado = false;
                                    if (dia[d].TradesDia[t].iValorEntrada - dia[d].TradesDia[t].iValorStop >= -400)
                                        i--;
                                    t++;
                                }

                                /*
                                //gain 200 ptos venda
                                if (dia[d].TradesDia[t].iAtingiu200 == 0 && (dia[d].Ticker[i].iMaximo >= dia[d].TradesDia[t].iValorEntrada - 200 && dia[d].Ticker[i].iMinimo <= dia[d].TradesDia[t].iValorEntrada - 200))
                                    dia[d].TradesDia[t].iAtingiu200 = 200;

                                //gain 400 ptos venda
                                if (dia[d].TradesDia[t].iAtingiu400 == 0 && (dia[d].Ticker[i].iMaximo >= dia[d].TradesDia[t].iValorEntrada - 400 && dia[d].Ticker[i].iMinimo <= dia[d].TradesDia[t].iValorEntrada - 400))
                                    dia[d].TradesDia[t].iAtingiu400 = 400;

                                //gain 1000 ptos venda
                                if (dia[d].TradesDia[t].iAtingiu1000 == 0 && (dia[d].Ticker[i].iMaximo >= dia[d].TradesDia[t].iValorEntrada - 1000 && dia[d].Ticker[i].iMinimo <= dia[d].TradesDia[t].iValorEntrada - 1000))
                                    dia[d].TradesDia[t].iAtingiu1000 = 1000;

                                //stop venda
                                if (dia[d].Ticker[i].iMinimo <= dia[d].TradesDia[t].iValorStop && dia[d].Ticker[i].iMaximo >= dia[d].TradesDia[t].iValorStop)
                                {
                                    dia[d].TradesDia[t].iAtingiu200 = dia[d].TradesDia[t].iAtingiu200 != 0 ? dia[d].TradesDia[t].iAtingiu200 : dia[d].TradesDia[t].iValorEntrada - dia[d].iSaidaVenda;
                                    dia[d].TradesDia[t].iAtingiu400 = dia[d].TradesDia[t].iAtingiu400 != 0 ? dia[d].TradesDia[t].iAtingiu400 : dia[d].TradesDia[t].iValorEntrada - dia[d].iSaidaVenda;
                                    dia[d].TradesDia[t].iAtingiu1000 = dia[d].TradesDia[t].iAtingiu1000 != 0 ? dia[d].TradesDia[t].iAtingiu1000 : dia[d].TradesDia[t].iValorEntrada - dia[d].iSaidaVenda;
                                    dia[d].TradesDia[t].iFechamento = dia[d].TradesDia[t].iFechamento != 0 ? dia[d].TradesDia[t].iFechamento : dia[d].TradesDia[t].iValorEntrada - dia[d].iSaidaVenda;
                                    dia[d].TradesDia[t++].bStop = true;
                                    dia[d].bNoMercado = false;
                                }*/
                            }
                        }
                        
                    }
                    /*else if (dia[d].bNoMercado)
                    {
                        dia[d].bNoMercado = false;
                        if (dia[d].TradesDia[t].tttipoTrade == Trade.tipoTrade.compra)
                        {
                            dia[d].TradesDia[t].iAtingiu200 = dia[d].TradesDia[t].iAtingiu200 != 0 ? dia[d].TradesDia[t].iAtingiu200 : dia[d].Ticker[i].iFechamento - dia[d].TradesDia[t].iValorEntrada;
                            dia[d].TradesDia[t].iAtingiu400 = dia[d].TradesDia[t].iAtingiu400 != 0 ? dia[d].TradesDia[t].iAtingiu400 : dia[d].Ticker[i].iFechamento - dia[d].TradesDia[t].iValorEntrada;
                            dia[d].TradesDia[t].iAtingiu1000 = dia[d].TradesDia[t].iAtingiu1000 != 0 ? dia[d].TradesDia[t].iAtingiu1000 : dia[d].Ticker[i].iFechamento - dia[d].TradesDia[t].iValorEntrada;
                            dia[d].TradesDia[t].iFechamento = dia[d].Ticker[i].iFechamento - dia[d].TradesDia[t].iValorEntrada;
                        }
                        if (dia[d].TradesDia[t].tttipoTrade == Trade.tipoTrade.venda)
                        {
                            dia[d].TradesDia[t].iAtingiu200 = dia[d].TradesDia[t].iAtingiu200 != 0 ? dia[d].TradesDia[t].iAtingiu200 : dia[d].TradesDia[t].iValorEntrada - dia[d].Ticker[i].iFechamento;
                            dia[d].TradesDia[t].iAtingiu400 = dia[d].TradesDia[t].iAtingiu400 != 0 ? dia[d].TradesDia[t].iAtingiu400 : dia[d].TradesDia[t].iValorEntrada - dia[d].Ticker[i].iFechamento;
                            dia[d].TradesDia[t].iAtingiu1000 = dia[d].TradesDia[t].iAtingiu1000 != 0 ? dia[d].TradesDia[t].iAtingiu1000 : dia[d].TradesDia[t].iValorEntrada - dia[d].Ticker[i].iFechamento;
                            dia[d].TradesDia[t].iFechamento = dia[d].TradesDia[t].iValorEntrada - dia[d].Ticker[i].iFechamento;
                        }


                    }*/
                }
            }
        }

        private void bGravaBacktest_Click(object sender, EventArgs e)
        {
            //saveFileDialog1.AddExtension = true;
            saveFileDialog1.DefaultExt = "csv";
            //saveFileDialog1.CheckFileExists = true;
            if (DialogResult.OK == saveFileDialog1.ShowDialog())
            {
                GravarBacktest(saveFileDialog1.FileName);
            }
            
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            for (TimeSpan time = new TimeSpan(9, 0, 0); time < TimeSpan.Parse("11:01:00"); time = time.Add(TimeSpan.FromMinutes(15)))
            {
                cbHorarioEntrada.Items.Add(time.ToString());
            }
            cbHorarioEntrada.SelectedIndex = 4;

            for (TimeSpan time = new TimeSpan(17, 0, 0); time < TimeSpan.Parse("17:30:00"); time = time.Add(TimeSpan.FromMinutes(5)))
            {
                cbHorarioSaida.Items.Add(time.ToString());
            }
            cbHorarioSaida.SelectedIndex = 0;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            resultadoEstrategia();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Setup the graph
            CreateGraph(zedGraphControl1, m_iDayChart);
            // Size the control to fill the form with a margin
            SetSize();
            tabControl1.SelectedIndex = 1;
        }

        private void zedGraphControl1_Resize(object sender, EventArgs e)
        {
            SetSize();
        }

        private void SetSize()
        {
            zedGraphControl1.Location = new Point(10, 10);
            // Leave a small margin around the outside of the control
            zedGraphControl1.Size = new Size(ClientRectangle.Width - 20,
                                    ClientRectangle.Height - 20);
        }

        private void CreateGraph(ZedGraphControl zgc, int iDayChart)
        {
            // get a reference to the GraphPane
            GraphPane myPane = zgc.GraphPane;
            myPane.CurveList.Clear();
            DiaNegocio diaChart = new DiaNegocio();

            myPane.Title.Text = "Japanese Candlestick Chart Demo";
            myPane.XAxis.Title.Text = "Trading Date";
            myPane.YAxis.Title.Text = "Share Price, $US";
           
            StockPointList spl = new StockPointList();
            Random rand = new Random();

            // First day is jan 1st
            XDate xDate = new XDate(2006, 1, 1);


            for (int i = 0; dia[iDayChart].Ticker[i] != null; i++)
            {
                CriarPontodeEntradaDia(iDayChart);

                diaChart = dia[iDayChart];

                myPane.Title.Text = "WINFUT " + diaChart.dtData.ToString();
                
                double x = diaChart.Ticker[i].dtData.ToOADate();//xDate.XLDate;
                double open = diaChart.Ticker[i].iAbertura;
                double close = diaChart.Ticker[i].iFechamento;//open + rand.NextDouble() * 10.0 - 5.0;
                double hi = diaChart.Ticker[i].iMaximo;// Math.Max(open, close) + rand.NextDouble() * 5.0;
                double low = diaChart.Ticker[i].iMinimo;// Math.Min(open, close) - rand.NextDouble() * 5.0;

                StockPt pt = new StockPt(x, hi, low, open, close, 100000);
                spl.Add(pt);

                //open = close;
                // Advance one day
                //xDate.AddDays(1.0);
                // but skip the weekends
                //if (XDate.XLDateToDayOfWeek(xDate.XLDate) == 6)
                //  xDate.AddDays(2.0);
            }

            JapaneseCandleStickItem myCurve = myPane.AddJapaneseCandleStick("trades", spl);
            myCurve.Stick.IsAutoSize = true;
            myCurve.Stick.Color = Color.Blue;
            
            double[] dbentradavenda = new double[500];
            for( int i = 0; i < 500; i++)
                dbentradavenda[i] = (double)diaChart.iEntradaVenda;

            LineItem entradaVenda = myPane.AddCurve("Venda", null, dbentradavenda, Color.Red,SymbolType.None);

            double[] dbentradacompra = new double[500];
            for (int i = 0; i < 500; i++)
                dbentradacompra[i] = (double)diaChart.iEntradaCompra;

            LineItem entradaCompra = myPane.AddCurve("Compra", null, dbentradacompra, Color.Green, SymbolType.None);

            // Use DateAsOrdinal to skip weekend gaps
            myPane.XAxis.Type = AxisType.DateAsOrdinal;

            // pretty it up a little
            myPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45.0f);
            myPane.Fill = new Fill(Color.White, Color.FromArgb(220, 220, 255), 45.0f);

            // Tell ZedGraph to refigure the
            // axes since the data have changed
            zgc.AxisChange();
            zgc.Invalidate();

            //ATENÇÃO, OBSERVAR QUE QUANDO RETORNA À PRIMEIRA ENTRADA COMUMENTE EMBARCA NO SENTIDO CONTRÁRIO!!! VERIFICAR SE COMPORTAMENTO SE 
            //REPETE SEGUIDAS VEZES PRO ÍNDICE, PODE SER UMA FORMA DE GANHO COM UM ALVO MENOR DE ~300, 400 PTOS
        }

        private void btNextChart_Click(object sender, EventArgs e)
        {
            m_iDayChart++;
            // Setup the graph
            CreateGraph(zedGraphControl1, m_iDayChart);
            // Size the control to fill the form with a margin
            SetSize();
        }

        private void btPreviousChart_Click(object sender, EventArgs e)
        {
            m_iDayChart--;
            if (m_iDayChart < 0)
                m_iDayChart++;
            // Setup the graph
            CreateGraph(zedGraphControl1, m_iDayChart);
            // Size the control to fill the form with a margin
            SetSize();
        }
        /*
        private void button1_Click_1(object sender, EventArgs e)
        {
            Page_Load();
        }*/
    }
}
