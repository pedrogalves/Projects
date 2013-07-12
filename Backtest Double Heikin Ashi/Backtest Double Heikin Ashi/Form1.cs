using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Backtest_Double_Heikin_Ashi
{
    public partial class Form1 : Form
    {
        public static FileInfo fiOpenFile;
        public static Ticker [] dados = new Ticker[500000];
        public class Ticker
        {
            public string sAtivo;
            public int iAbertura;
            public int iMaximo;
            public int iMinimo;
            public int iFechamento;
            public DateTime dtData;

        }
        public enum tipo { COMPRA, VENDA };
        public class Trade
        {
            public DateTime dtData;
            public tipo tpTrade;
            public int iValorEntrada;
            public int iResultado;
            public int iMaxGanho;
            public int iMaxPerda;
            public bool bNoMercado;

        }
        public Form1()
        {
            InitializeComponent();
        }

        private void btOpenFile_Click(object sender, EventArgs e)
        {
            //openFileDialog1.Multiselect = true;
            openFileDialog1.InitialDirectory = @"C:\Users\Pedro\Investimentos";
            //openFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {
                //szOpenFiles = openFileDialog1.FileNames;
                fiOpenFile = new FileInfo(openFileDialog1.FileName);
                //textBox1.Text = fiOpenFile.FullName;
            }

            //carrega os dados
            StreamReader readFile = new StreamReader(fiOpenFile.FullName);
            string line;
            string[] lineCampos = new string[7];
            readFile.ReadLine();
            
            /*dia = new DiaNegocio[300*4];
            DateTime data = new DateTime(1, 1, 1, 0, 0, 0);
            tsInicioTrade = TimeSpan.Parse(cbHorarioEntrada.SelectedItem.ToString());
            tsSaidaTrade = TimeSpan.Parse(cbHorarioSaida.SelectedItem.ToString());
            */
            for (int i = 0, d = -1, t = 0; (line = readFile.ReadLine()) != null; i++)
            {
                lineCampos = line.Split(';');


                dados[i] = new Ticker();
                //dia[d].Ticker[i].sAtivo = lineCampos[0];
                dados[i].dtData = DateTime.Parse(lineCampos[0] + " " + lineCampos[1]);
                //dia[d].Ticker[i].dtData.Date = DateTime.Parse(lineCampos[0]);
                //dia[d].Ticker[i].dtData.TimeOfDay = DateTime.Parse(lineCampos[1]);
                dados[i].iAbertura = Convert.ToInt32(lineCampos[2]);
                dados[i].iMaximo = Convert.ToInt32(lineCampos[3]);
                dados[i].iMinimo = Convert.ToInt32(lineCampos[4]);
                dados[i].iFechamento = Convert.ToInt32(lineCampos[5]);
                //dia[d].Ticker[i].dtData = DateTime.Parse(lineCampos[5]);
            }
        }
        public class BlackFin
        {
            public int iMAXDA = 0;
            public int iMAXDC = 0;
            public int iMAXDO = 0;
            public int iMINDA = 0;
            public int iMINDC = 0;
            public int iMINDO = 0;
            public int iAMPDA = 0;
            public int iFDCPA = 0;
            public int iFDVDA = 0;
        }
        private void button1_Click(object sender, EventArgs e)
        { 
            int iMinimoEntrada = 0;
            int iMaximoEntrada = 0;
            //DateTime dtHoraEntrada;
            TimeSpan tsEntrada = new TimeSpan(1, 30, 0);
            DateTime dtInicioDia = new DateTime();
            DateTime dtSaidaDia = new DateTime();
            Trade[] hstTrade = new Trade[100000];
            bool bNewDay = true;
            bool bBegDay = true;
            int iStopCnt = 0;
            //rodar primeira hora e meia para criar ptos de entrada
            for (int i = 0, t = 0; dados[i] != null; i++)
            {

                if (bBegDay)
                {
                    dtInicioDia = dados[i].dtData;
                    //TimeSpan t1 = dtInicioDia.AddHours(1).TimeOfDay();
                    //dtInicioDia.AddMinutes(30).TimeOfDay();
                    //dtInicioDia = dtInicioDia.Date + tsEntrada;
                    for (int i2 = i; (dados[i2 + 1] != null && dados[i2].dtData.DayOfYear == dados[i2 + 1].dtData.DayOfYear); i2++)
                        dtSaidaDia = dados[i2+1].dtData;
                }
                
                //rodar dados para entrada /cria dados de entrada baseado na primeira hora e meia
                if (dados[i].dtData.TimeOfDay <= (dtInicioDia.TimeOfDay + tsEntrada) )
                {
                    if (bBegDay)
                    {
                        iMinimoEntrada = dados[i].iMinimo;
                        iMaximoEntrada = dados[i].iMaximo;
                        bBegDay = false;
                    }
                    if (dados[i].iMaximo > iMaximoEntrada)
                        iMaximoEntrada = dados[i].iMaximo;
                    if (dados[i].iMinimo < iMinimoEntrada)
                        iMinimoEntrada = dados[i].iMinimo;
                }
                // depois da primeira hora e meia testa o trade
                else
                {
                    // dá os pontos de entrada para o trade
                    if (iStopCnt < 2 && dados[i].iMaximo > iMaximoEntrada +50/* offset para entrada */&& (bNewDay || t == 0 || hstTrade[t - 1] != null && hstTrade[t - 1].tpTrade == tipo.VENDA))
                    {
                        bNewDay = false;
                        if (t > 0 && hstTrade[t - 1] != null && hstTrade[t - 1].bNoMercado)
                        {
                            hstTrade[t - 1].bNoMercado = false;
                            hstTrade[t - 1].iResultado = hstTrade[t - 1].iValorEntrada - iMaximoEntrada - 50;
                        }
                        hstTrade[t] = new Trade();
                        hstTrade[t].dtData = dados[i].dtData;
                        hstTrade[t].tpTrade = tipo.COMPRA;
                        hstTrade[t].bNoMercado = true;
                        hstTrade[t].iValorEntrada = iMaximoEntrada + 50;
                        hstTrade[t].iMaxGanho = hstTrade[t].iMaxPerda = 0;
                        t++;
                    }
                    else if (iStopCnt < 2 && dados[i].iMinimo < iMinimoEntrada - 50 && (bNewDay || t == 0 || hstTrade[t - 1] != null && hstTrade[t - 1].tpTrade == tipo.COMPRA)) 
                    {
                        bNewDay = false;
                        if (t > 0 && hstTrade[t - 1] != null && hstTrade[t - 1].bNoMercado)
                        {
                            hstTrade[t - 1].bNoMercado = false;
                            hstTrade[t - 1].iResultado = iMinimoEntrada -50 - hstTrade[t - 1].iValorEntrada;
                        }
                        hstTrade[t] = new Trade();
                        hstTrade[t].dtData = dados[i].dtData;
                        hstTrade[t].tpTrade = tipo.VENDA;
                        hstTrade[t].bNoMercado = true;
                        hstTrade[t].iValorEntrada = iMinimoEntrada - 50;
                        hstTrade[t].iMaxGanho = hstTrade[t].iMaxPerda = 0;
                        t++;
                    }

                    //atualiza o lucro/perda do trade
                    for (int i2 = 0; hstTrade[i2] != null; i2++)
                    {
                        if (hstTrade[i2].bNoMercado)
                        {
                            if (hstTrade[i2].tpTrade == tipo.COMPRA)
                            {
                                if (dados[i].iMaximo > hstTrade[i2].iValorEntrada)
                                    hstTrade[i2].iMaxGanho = ((dados[i].iMaximo - hstTrade[i2].iValorEntrada) > hstTrade[i2].iMaxGanho) ? (dados[i].iMaximo - hstTrade[i2].iValorEntrada) : hstTrade[i2].iMaxGanho;
                                else if (dados[i].iMinimo < hstTrade[i2].iValorEntrada)
                                    hstTrade[i2].iMaxPerda = ((dados[i].iMinimo - hstTrade[i2].iValorEntrada) < hstTrade[i2].iMaxPerda) ? (dados[i].iMinimo - hstTrade[i2].iValorEntrada) : hstTrade[i2].iMaxPerda;
                            }
                            else
                            {
                                if (dados[i].iMaximo > hstTrade[i2].iValorEntrada)
                                    hstTrade[i2].iMaxPerda = ((hstTrade[i2].iValorEntrada - dados[i].iMaximo) < hstTrade[i2].iMaxPerda) ? (hstTrade[i2].iValorEntrada - dados[i].iMaximo) : hstTrade[i2].iMaxPerda;
                                else if (dados[i].iMinimo < hstTrade[i2].iValorEntrada)
                                    hstTrade[i2].iMaxGanho = ((hstTrade[i2].iValorEntrada - dados[i].iMinimo) > hstTrade[i2].iMaxGanho) ? (hstTrade[i2].iValorEntrada - dados[i].iMinimo) : hstTrade[i2].iMaxGanho;
                            }
                        }
                    }

                    //sai do trade ao fim do dia
                    if (t > 0 && hstTrade[t - 1] != null && hstTrade[t - 1].bNoMercado)
                    {
                        if (hstTrade[t - 1].iMaxGanho >= 1000)
                        {
                            hstTrade[t - 1].iResultado = 1000;
                            hstTrade[t - 1].bNoMercado = false;
                            //t++;

                        }
                        else if (hstTrade[t - 1].iMaxPerda <= -400)
                        {
                            iStopCnt++;
                            hstTrade[t - 1].iResultado = -400;
                            hstTrade[t - 1].bNoMercado = false;
                            //t++;
                        }
                    }
                    //se acabou o dia, fecha todos trades 
                    if ( /*hstTrade[t-1] != null &&*/ (dados[i + 1] == null || dados[i].dtData.DayOfYear != dados[i + 1].dtData.DayOfYear))
                    {
                        bNewDay = bBegDay = true;
                        iStopCnt = 0;
                        if (hstTrade[t - 1].bNoMercado)
                        {
                            hstTrade[t - 1].bNoMercado = false;
                            if (hstTrade[t - 1].tpTrade == tipo.VENDA)
                                hstTrade[t - 1].iResultado = hstTrade[t - 1].iValorEntrada - dados[i].iAbertura;
                            else
                                hstTrade[t - 1].iResultado = dados[i].iAbertura - hstTrade[t - 1].iValorEntrada;
                            //t++;
                        }
                    }
                }
            }

            AnaliseMes[] analise = new AnaliseMes[50];
            analise[0] = new AnaliseMes();

            for (int i = 0, m = 0; hstTrade[i] != null; i++)
            {
                /*if (analise[m].iDrawUp + hstTrade[i].iResultado > analise[m].iDrawUp)
                {
                    analise[m].iDrawUp += hstTrade[i].iResultado;
                    analise[m].dtDrawUp = hstTrade[i].dtData;
                }
                else if (analise[m].iDrawDown + hstTrade[i].iResultado < analise[m].iDrawDown)
                {
                    analise[m].iDrawDown += hstTrade[i].iResultado;
                    analise[m].dtDrawDown = hstTrade[i].dtData;
                }*/
                analise[m].iResultFinal += hstTrade[i].iResultado;
                
                if (analise[m].iResultFinal < analise[m].iDrawDown)
                {
                    analise[m].iDrawDown = analise[m].iResultFinal;
                    analise[m].dtDrawDown = hstTrade[i].dtData;
                }
                else if (analise[m].iResultFinal > analise[m].iDrawUp)
                {
                    analise[m].iDrawUp = analise[m].iResultFinal;
                    analise[m].dtDrawUp = hstTrade[i].dtData;
                }
                if (!analise[m].bAtingiuAlvo && !analise[m].bAtingiuStop)
                {
                    if (analise[m].iResultFinal >= 2000)
                        analise[m].bAtingiuAlvo = true;
                    if (analise[m].iResultFinal <= -2000)
                        analise[m].bAtingiuStop = true;
                }
                if (hstTrade[i + 1] == null || hstTrade[i].dtData.Month != hstTrade[i + 1].dtData.Month)
                {
                    if (analise[m].bAtingiuStop)
                        analise[m].iResultFinal = -2000;
                    else if (analise[m].bAtingiuAlvo)
                        analise[m].iResultFinal = 2000;
                    analise[m++].iTradesMes = i + 1 - analise[m - 1].iTradesMes;
                    analise[m] = new AnaliseMes();
                }
            }
            StreamWriter writeFile = new StreamWriter(@"C:\Users\Pedro\Investimentos\analise_trades.csv");
            writeFile.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7}", "Alvo", "Stop", "Resultado", "# Trades", "DrawUp", "Data DrawUp", "DrawDown", "Data DrawDown");
            for (int i = 0; analise[i] != null; i++)
                writeFile.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7}", analise[i].bAtingiuAlvo ? "Alvo" : " ", analise[i].bAtingiuStop ? "Stop" : " ", analise[i].iResultFinal, analise[i].iTradesMes, analise[i].iDrawUp,
                                                            analise[i].dtDrawUp, analise[i].iDrawDown, analise[i].dtDrawDown);

            writeFile.Flush();
            writeFile.Close();

            //escrever array HK para conferencia num arquivo
            writeFile = new StreamWriter(@"C:\Users\Pedro\Investimentos\trades_pedrinho.csv");
            writeFile.WriteLine("{0};{1};{2};{3};{4};{5}", "DATA", "Entrada", "Resultado", "DrawUp", "DrawDown", "Tipo");
            for (int i = 0; hstTrade[i] != null; i++)
                writeFile.WriteLine("{0};{1};{2};{3};{4};{5}", hstTrade[i].dtData, hstTrade[i].iValorEntrada, hstTrade[i].iResultado,
                                                           hstTrade[i].iMaxGanho, hstTrade[i].iMaxPerda, hstTrade[i].tpTrade.ToString());

            writeFile.Flush();
            writeFile.Close();
            //atualizar lucro ou perda

            //sair no fim do dia 
        }
        public class AnaliseMes
        {
            public int iTradesMes = 0;
            public int iDrawUp = 0;
            public int iDrawDown = 0;
            public int iResultFinal = 0;
            public bool bAtingiuAlvo = false;
            public bool bAtingiuStop = false;
            public DateTime dtDrawUp = new DateTime();
            public DateTime dtDrawDown = new DateTime();
        }
        private void btTest_Click(object sender, EventArgs e)
        {
            //criar array dados 3m
            Ticker [] dados_3m = new Ticker[145000];
            for (int i = 0, m = 0, i2 = 0; dados[i] != null; i++)
            {
                if (m == 0)
                {
                    dados_3m[i2] = new Ticker();
                    //mais facil copiar a estrutura inteira
                    dados_3m[i2].dtData = dados[i].dtData;
                    dados_3m[i2].iAbertura = dados[i].iAbertura;
                    dados_3m[i2].iFechamento = dados[i].iFechamento;
                    dados_3m[i2].iMaximo = dados[i].iMaximo;
                    dados_3m[i2].iMinimo = dados[i].iMinimo;
                }
                else 
                {
                    dados_3m[i2].iFechamento = dados[i].iFechamento;
                    dados_3m[i2].iMaximo = (dados[i].iMaximo > dados_3m[i2].iMaximo) ? dados[i].iMaximo : dados_3m[i2].iMaximo;
                    dados_3m[i2].iMinimo = (dados[i].iMinimo < dados_3m[i2].iMinimo) ? dados[i].iMinimo : dados_3m[i2].iMinimo;
                }

                if (dados[i + 1] != null && dados[i].dtData.DayOfYear != dados[i + 1].dtData.DayOfYear || m == 2)
                {
                    m = 0;
                    i2++;
                }
                else// if (m < 2)
                    m++;

            }
            StreamWriter writeFile = new StreamWriter(@"C:\Users\Pedro\Investimentos\dados_3m.csv");
            writeFile.WriteLine("{0};{1};{2};{3};{4}", "DATA", "OPEN", "HIGH", "LOW", "CLOSE");
            for (int i = 0; dados_3m[i] != null; i++)
                writeFile.WriteLine("{0};{1};{2};{3};{4}", dados_3m[i].dtData, dados_3m[i].iAbertura, dados_3m[i].iMaximo,
                                                            dados_3m[i].iMinimo, dados_3m[i].iFechamento);

            writeFile.Flush();
            writeFile.Close();

            //criar o array heikin ashi 3m
            Ticker [] dados_heikinashi_3m = new Ticker[145000];
            for (int i = 0; dados_3m[i] != null; i++)
            {
                dados_heikinashi_3m[i] = new Ticker();
                dados_heikinashi_3m[i].dtData = dados_3m[i].dtData;
                if (i > 0)
                    dados_heikinashi_3m[i].iAbertura = (int)Math.Round(((dados_heikinashi_3m[i - 1].iAbertura + dados_heikinashi_3m[i - 1].iFechamento) / 2) / 5.0) * 5;
                else
                    dados_heikinashi_3m[i].iAbertura = (int)Math.Round(((dados_3m[i].iAbertura + dados_3m[i].iFechamento) / 2) / 5.0) * 5;
                dados_heikinashi_3m[i].iFechamento = (int)Math.Round(((dados_3m[i].iAbertura + dados_3m[i].iFechamento + dados_3m[i].iMaximo + dados_3m[i].iMinimo)/4) / 5.0) * 5;
                dados_heikinashi_3m[i].iMaximo = Math.Max(dados_3m[i].iMaximo,Math.Max(dados_heikinashi_3m[i].iAbertura,dados_heikinashi_3m[i].iFechamento));
                dados_heikinashi_3m[i].iMinimo = Math.Min(dados_3m[i].iMinimo,Math.Min(dados_heikinashi_3m[i].iAbertura,dados_heikinashi_3m[i].iFechamento));
            }

            //escrever array HK para conferencia num arquivo
            writeFile = new StreamWriter(@"C:\Users\Pedro\Investimentos\dados_heikinashi_3m.csv");
            writeFile.WriteLine("{0};{1};{2};{3};{4}", "DATA", "xOPEN", "xHIGH", "xLOW", "xCLOSE");
            for (int i = 0; dados_heikinashi_3m[i] != null; i++)
                writeFile.WriteLine("{0};{1};{2};{3};{4}", dados_heikinashi_3m[i].dtData, dados_heikinashi_3m[i].iAbertura, dados_heikinashi_3m[i].iMaximo,
                                                            dados_heikinashi_3m[i].iMinimo, dados_heikinashi_3m[i].iFechamento);            
            
            writeFile.Flush();
            writeFile.Close();
            /*
             * public class Trade
            {
                public DateTime dtData;
                enum tipo {COMPRA, VENDA};
                public int iValorEntrada;
                public int iMaxGanho;
                public int iMaxPerda;

            }*/
            Trade[] hstTrade = new Trade[100000];
            //testar a estrategia
            for (int i = 2, t = 0 ; dados_heikinashi_3m[i] != null; i++) 
            {
                //da a entrada
                if ( dados_heikinashi_3m[i - 1].iAbertura == dados_heikinashi_3m[i - 1].iMinimo &&
                    dados_heikinashi_3m[i - 2].iAbertura == dados_heikinashi_3m[i - 2].iMinimo && dados_heikinashi_3m[i].dtData.Hour >= 10 && ( t == 0 ||
                    ( hstTrade[t-1] != null && ( /*( hstTrade[t-1].tpTrade == tipo.VENDA && hstTrade[t-1].bNoMercado ) ||*/ !hstTrade[t-1].bNoMercado ) ) ) )
                { /*compra*/
                    /*if (t > 0)
                    {
                        hstTrade[t - 1].bNoMercado = false;
                        hstTrade[t - 1].iResultado = hstTrade[t - 1].iValorEntrada - dados_3m[i].iAbertura;
                    }*/
                    hstTrade[t] = new Trade();
                    hstTrade[t].dtData = dados_heikinashi_3m[i].dtData;
                    hstTrade[t].tpTrade = tipo.COMPRA;
                    hstTrade[t].bNoMercado = true;
                    hstTrade[t].iValorEntrada = dados_3m[i].iAbertura;
                    hstTrade[t].iMaxGanho = hstTrade[t].iMaxPerda = 0;
                    t++;
                }
                else if ( dados_heikinashi_3m[i - 1].iAbertura == dados_heikinashi_3m[i - 1].iMaximo &&
                        dados_heikinashi_3m[i - 2].iAbertura == dados_heikinashi_3m[i - 2].iMaximo && dados_heikinashi_3m[i].dtData.Hour >= 10 && (t == 0 ||
                        ( hstTrade[t-1] != null && ( /*( hstTrade[t-1].tpTrade == tipo.COMPRA && hstTrade[t-1].bNoMercado ) ||*/ !hstTrade[t-1].bNoMercado ) ) ) ) 
                { /*venda*/
                    /*if (t > 0)
                    {
                        hstTrade[t - 1].bNoMercado = false;
                        hstTrade[t - 1].iResultado = dados_3m[i].iAbertura - hstTrade[t - 1].iValorEntrada;
                    }*/
                    hstTrade[t] = new Trade();
                    hstTrade[t].dtData = dados_heikinashi_3m[i].dtData;
                    hstTrade[t].tpTrade = tipo.VENDA;
                    hstTrade[t].bNoMercado = true;
                    hstTrade[t].iValorEntrada = dados_3m[i].iAbertura;
                    hstTrade[t].iMaxGanho = hstTrade[t].iMaxPerda = 0;
                    t++;
                }
                
                //atualiiza lucro ou perda
                for (int i2 = 0; hstTrade[i2] != null; i2++)
                {
                    if ( hstTrade[i2].bNoMercado )
                    {
                        if (hstTrade[i2].tpTrade == tipo.COMPRA )
                        {
                            if (dados_3m[i].iMaximo > hstTrade[i2].iValorEntrada)
                                hstTrade[i2].iMaxGanho = ((dados_3m[i].iMaximo - hstTrade[i2].iValorEntrada) > hstTrade[i2].iMaxGanho) ? (dados_3m[i].iMaximo - hstTrade[i2].iValorEntrada) : hstTrade[i2].iMaxGanho;
                            else if (dados_3m[i].iMinimo < hstTrade[i2].iValorEntrada)
                                hstTrade[i2].iMaxPerda = ((dados_3m[i].iMinimo - hstTrade[i2].iValorEntrada) < hstTrade[i2].iMaxPerda) ? (dados_3m[i].iMinimo - hstTrade[i2].iValorEntrada) : hstTrade[i2].iMaxPerda;
                        }
                        else
                        {
                            if (dados_3m[i].iMaximo > hstTrade[i2].iValorEntrada)
                                hstTrade[i2].iMaxPerda = ((hstTrade[i2].iValorEntrada - dados_3m[i].iMaximo) < hstTrade[i2].iMaxPerda) ? (hstTrade[i2].iValorEntrada - dados_3m[i].iMaximo) : hstTrade[i2].iMaxPerda;
                            else if (dados_3m[i].iMinimo < hstTrade[i2].iValorEntrada)
                                hstTrade[i2].iMaxGanho = ((hstTrade[i2].iValorEntrada - dados_3m[i].iMinimo) > hstTrade[i2].iMaxGanho) ? (hstTrade[i2].iValorEntrada - dados_3m[i].iMinimo) : hstTrade[i2].iMaxGanho;
                        }
                    }
                }

                if (t > 0 && hstTrade[t - 1] != null && hstTrade[t - 1].bNoMercado) 
                {
                    if (hstTrade[t - 1].iMaxGanho >= 100)
                    {
                        hstTrade[t - 1].iResultado = 100;
                        hstTrade[t - 1].bNoMercado = false;
                        //t++;

                    }
                    else if (hstTrade[t - 1].iMaxPerda <= -50)
                    {
                        hstTrade[t - 1].iResultado = -50;
                        hstTrade[t - 1].bNoMercado = false;
                        //t++;
                    }
                }
                //se acabou o dia, fecha todos trades 
                if ( /*hstTrade[t-1] != null &&*/ (dados_heikinashi_3m[i + 1] != null && dados_heikinashi_3m[i].dtData.DayOfYear != dados_heikinashi_3m[i + 1].dtData.DayOfYear))
                //foreach ( Trade trade in hstTrade )
                //  trade.bNoMercado = false;
                {
                    if (hstTrade[t - 1].bNoMercado)
                    {
                        hstTrade[t - 1].bNoMercado = false;
                        if (hstTrade[t - 1].tpTrade == tipo.VENDA)
                            hstTrade[t - 1].iResultado = hstTrade[t - 1].iValorEntrada - dados_3m[i].iAbertura;
                        else
                            hstTrade[t - 1].iResultado = dados_3m[i].iAbertura - hstTrade[t - 1].iValorEntrada;
                        //t++;
                    }
                }

            }
            
            //escrever array HK para conferencia num arquivo
            writeFile = new StreamWriter(@"C:\Users\Pedro\Investimentos\trades_heikinashi_3m.csv");
            writeFile.WriteLine("{0};{1};{2};{3};{4};{5}", "DATA", "Entrada", "Resultado", "DrawUp", "DrawDown", "Tipo" );
            for (int i = 0; hstTrade[i] != null; i++)
                writeFile.WriteLine("{0};{1};{2};{3};{4};{5}", hstTrade[i].dtData, hstTrade[i].iValorEntrada, hstTrade[i].iResultado, 
                                                           hstTrade[i].iMaxGanho, hstTrade[i].iMaxPerda, hstTrade[i].tpTrade.ToString());

            writeFile.Flush();
            writeFile.Close();
        }

        
    }
}