using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace WindowsApplication1
{
    public partial class Form1 : Form
    {
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
        public class DiaNegocio
        {
            public DateTime dtData;
            public int iEntradaCompra=0;
            public int iSaidaCompra=0;
            public int iEntradaVenda=100000;
            public int iSaidaVenda=0;
            public int iMaximoChegou=0;
            public int iMinChegou=100000;
            public Ticker[] Ticker = new Ticker[1050];
            public Trade[] TradesDia = new Trade[10];
            public bool bNoMercado = false;
                       
        }

        public class Trade
        {
            public DateTime dtHoraEntrada;
            public int iValorEntrada;
            public int iAtingiu200=0;
            public int iAtingiu400 = 0;
            public int iAtingiu1000 = 0;
            public int iFechamento = 0;
            public bool bStop;
            public int iMaxTrade;
            public int iMinTrade;
            public tipoTrade tttipoTrade;
            public bool bNoMercado=false;
            public int iValorStop;

            public enum tipoTrade { compra, venda };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StreamReader readFile = new StreamReader(@"C:\Users\Pedro\Investimentos\winm12_original.csv");
            string line;
            string [] lineCampos = new string[7];
            readFile.ReadLine();
            //Ticker [] dados = new Ticker[10000];
            DiaNegocio[] dia = new DiaNegocio[31];
            DateTime data = new DateTime(1, 1, 1, 0, 0, 0);

            for (int i = 0, d=0, t=0; (line = readFile.ReadLine()) != null; i++ )
            {
                lineCampos = line.Split(';');

                if (data.DayOfYear != DateTime.Parse(lineCampos[5]).DayOfYear)
                {
                    data = DateTime.Parse(lineCampos[5]);
                    dia[d++] = new DiaNegocio();
                    i = 0;
                    t = 0;
                    dia[d-1].dtData = data;

                }

                dia[d-1].Ticker[i] = new Ticker();
                dia[d-1].Ticker[i].sAtivo  = lineCampos[0];
                dia[d-1].Ticker[i].iAbertura = Convert.ToInt32(lineCampos[1]);
                dia[d-1].Ticker[i].iMaximo = Convert.ToInt32(lineCampos[2]);
                dia[d-1].Ticker[i].iMinimo = Convert.ToInt32(lineCampos[3]);
                dia[d-1].Ticker[i].iFechamento = Convert.ToInt32(lineCampos[4]);
                dia[d-1].Ticker[i].dtData = DateTime.Parse(lineCampos[5]);
                if((dia[d-1].Ticker[i].dtData.Hour == 9) || (dia[d-1].Ticker[i].dtData.Hour == 10 && dia[d-1].Ticker[i].dtData.Minute <= 30))
                //if ((dia[d - 1].Ticker[i].dtData.Hour == 9) || (dia[d - 1].Ticker[i].dtData.Hour == 10 && dia[d - 1].Ticker[i].dtData.Minute == 00))
                {
                    if (dia[d - 1].Ticker[i].dtData.Day == 31)
                        i = i;

                    if(dia[d-1].Ticker[i].iMinimo < dia[d-1].iMinChegou)
                    {
                        dia[d-1].iMinChegou = dia[d-1].Ticker[i].iMinimo;
                        //dia[d-1].iEntradaVenda = dia[d-1].Ticker[i].iMinimo - 5;
                        dia[d - 1].iEntradaVenda = dia[d - 1].Ticker[i].iMinimo - 20;
                    }
                    if(dia[d-1].Ticker[i].iMaximo > dia[d-1].iMaximoChegou)
                    {
                        dia[d-1].iMaximoChegou = dia[d-1].Ticker[i].iMaximo;
                        //dia[d - 1].iEntradaCompra = dia[d - 1].Ticker[i].iMaximo + 5;
                        dia[d - 1].iEntradaCompra = dia[d - 1].Ticker[i].iMaximo + 20;
                    }
                    

                }
                if (((dia[d - 1].Ticker[i].dtData.Hour >= 10 && dia[d - 1].Ticker[i].dtData.Minute > 30) || dia[d - 1].Ticker[i].dtData.Hour >= 10)&&
                
                //if ((dia[d - 1].Ticker[i].dtData.Hour >= 10 && dia[d - 1].Ticker[i].dtData.Minute >= 0) &&
                    (dia[d - 1].Ticker[i].dtData.Hour < 17))/*||
                    (dia[d - 1].Ticker[i].dtData.Hour == 17 && dia[d - 1].Ticker[i].dtData.Minute < 29)) */
                {
                    //verifica stop/gain
                    if (dia[d - 1].bNoMercado)
                    {
                        //gain 200 ptos compra
                        if (dia[d - 1].TradesDia[t].tttipoTrade == Trade.tipoTrade.compra)
                        {
                            //gain 200 ptos compra
                            if (dia[d - 1].TradesDia[t].iAtingiu200 == 0 && (dia[d - 1].Ticker[i].iMinimo <= dia[d - 1].TradesDia[t].iValorEntrada + 200 && dia[d - 1].Ticker[i].iMaximo >= dia[d - 1].TradesDia[t].iValorEntrada + 200))
                                dia[d - 1].TradesDia[t].iAtingiu200 = 200;

                            //gain 400 ptos compra
                            if (dia[d - 1].TradesDia[t].iAtingiu400 == 0 && (dia[d - 1].Ticker[i].iMinimo <= dia[d - 1].TradesDia[t].iValorEntrada + 400 && dia[d - 1].Ticker[i].iMaximo >= dia[d - 1].TradesDia[t].iValorEntrada + 400))
                                dia[d - 1].TradesDia[t].iAtingiu400 = 400;

                            //gain 1000 ptos compra
                            if (dia[d - 1].TradesDia[t].iAtingiu1000 == 0 && (dia[d - 1].Ticker[i].iMinimo <= dia[d - 1].TradesDia[t].iValorEntrada + 1000 && dia[d - 1].Ticker[i].iMaximo >= dia[d - 1].TradesDia[t].iValorEntrada + 1000))
                                dia[d - 1].TradesDia[t].iAtingiu1000 = 1000;

                            //stop compra
                            if (dia[d - 1].Ticker[i].iMaximo >= dia[d - 1].TradesDia[t].iValorStop && dia[d - 1].Ticker[i].iMinimo <= dia[d - 1].TradesDia[t].iValorStop)
                            {
                                dia[d - 1].TradesDia[t].iAtingiu200 = dia[d - 1].TradesDia[t].iAtingiu200 != 0 ? dia[d - 1].TradesDia[t].iAtingiu200 : dia[d - 1].iSaidaCompra - dia[d - 1].TradesDia[t].iValorEntrada;
                                dia[d - 1].TradesDia[t].iAtingiu400 = dia[d - 1].TradesDia[t].iAtingiu400 != 0 ? dia[d - 1].TradesDia[t].iAtingiu400 : dia[d - 1].iSaidaCompra - dia[d - 1].TradesDia[t].iValorEntrada;
                                dia[d - 1].TradesDia[t].iAtingiu1000 = dia[d - 1].TradesDia[t].iAtingiu1000 != 0 ? dia[d - 1].TradesDia[t].iAtingiu1000 : dia[d - 1].iSaidaCompra - dia[d - 1].TradesDia[t].iValorEntrada;
                                dia[d - 1].TradesDia[t].iFechamento = dia[d - 1].TradesDia[t].iFechamento != 0 ? dia[d - 1].TradesDia[t].iFechamento : dia[d - 1].iSaidaCompra - dia[d - 1].TradesDia[t].iValorEntrada;
                                dia[d - 1].TradesDia[t++].bStop = true;
                                dia[d - 1].bNoMercado = false;
                            }
                        }
                        if (dia[d - 1].bNoMercado && dia[d - 1].TradesDia[t].tttipoTrade == Trade.tipoTrade.venda)
                        {
                            //gain 200 ptos venda
                            if (dia[d - 1].TradesDia[t].iAtingiu200 == 0 && (dia[d - 1].Ticker[i].iMaximo >= dia[d - 1].TradesDia[t].iValorEntrada - 200 && dia[d - 1].Ticker[i].iMinimo <= dia[d - 1].TradesDia[t].iValorEntrada - 200))
                                dia[d - 1].TradesDia[t].iAtingiu200 = 200;

                            //gain 400 ptos venda
                            if (dia[d - 1].TradesDia[t].iAtingiu400 == 0 && (dia[d - 1].Ticker[i].iMaximo >= dia[d - 1].TradesDia[t].iValorEntrada - 400 && dia[d - 1].Ticker[i].iMinimo <= dia[d - 1].TradesDia[t].iValorEntrada - 400))
                                dia[d - 1].TradesDia[t].iAtingiu400 = 400;

                            //gain 1000 ptos venda
                            if (dia[d - 1].TradesDia[t].iAtingiu1000 == 0 && (dia[d - 1].Ticker[i].iMaximo >= dia[d - 1].TradesDia[t].iValorEntrada - 1000 && dia[d - 1].Ticker[i].iMinimo <= dia[d - 1].TradesDia[t].iValorEntrada - 1000))
                                dia[d - 1].TradesDia[t].iAtingiu1000 = 1000;

                            //stop venda
                            if (dia[d - 1].Ticker[i].iMinimo <= dia[d - 1].TradesDia[t].iValorStop && dia[d - 1].Ticker[i].iMaximo >= dia[d - 1].TradesDia[t].iValorStop)
                            {
                                dia[d - 1].TradesDia[t].iAtingiu200 = dia[d - 1].TradesDia[t].iAtingiu200 != 0 ? dia[d - 1].TradesDia[t].iAtingiu200 : dia[d - 1].TradesDia[t].iValorEntrada - dia[d - 1].iSaidaVenda;
                                dia[d - 1].TradesDia[t].iAtingiu400 = dia[d - 1].TradesDia[t].iAtingiu400 != 0 ? dia[d - 1].TradesDia[t].iAtingiu400 : dia[d - 1].TradesDia[t].iValorEntrada - dia[d - 1].iSaidaVenda;
                                dia[d - 1].TradesDia[t].iAtingiu1000 = dia[d - 1].TradesDia[t].iAtingiu1000 != 0 ? dia[d - 1].TradesDia[t].iAtingiu1000 : dia[d - 1].TradesDia[t].iValorEntrada - dia[d - 1].iSaidaVenda;
                                dia[d - 1].TradesDia[t].iFechamento = dia[d - 1].TradesDia[t].iFechamento != 0 ? dia[d - 1].TradesDia[t].iFechamento : dia[d - 1].TradesDia[t].iValorEntrada - dia[d - 1].iSaidaVenda;
                                dia[d - 1].TradesDia[t++].bStop = true;
                                dia[d - 1].bNoMercado = false;
                            }
                        }
                    }
                    if (dia[d - 1].TradesDia[t] == null)
                        dia[d - 1].TradesDia[t] = new Trade();
                    //entrada compra
                    if (!dia[d - 1].bNoMercado && (t==0 || (t>0 && dia[d - 1].TradesDia[t-1].tttipoTrade == Trade.tipoTrade.venda)) && ((dia[d - 1].Ticker[i].iMinimo <= dia[d - 1].iEntradaCompra && dia[d - 1].Ticker[i].iMaximo >= dia[d - 1].iEntradaCompra)))
                    {
                        if (dia[d - 1].TradesDia[t] == null) 
                            dia[d - 1].TradesDia[t] = new Trade();
                        dia[d - 1].TradesDia[t].dtHoraEntrada = dia[d - 1].Ticker[i].dtData;
                        dia[d - 1].TradesDia[t].iValorEntrada = dia[d - 1].iEntradaCompra;
                        dia[d - 1].bNoMercado = true;
                        dia[d - 1].iSaidaCompra = (dia[d - 1].iEntradaCompra - dia[d - 1].iEntradaVenda) < 420 ? dia[d - 1].iEntradaVenda : dia[d - 1].iEntradaCompra - 420;
                        //dia[d - 1].iSaidaCompra = (dia[d - 1].iEntradaCompra - dia[d - 1].iEntradaVenda) < 400 ? dia[d - 1].iEntradaVenda : dia[d - 1].iEntradaCompra - 400;
                        dia[d - 1].TradesDia[t].iValorStop = dia[d - 1].iSaidaCompra;
                        dia[d - 1].TradesDia[t].tttipoTrade = Trade.tipoTrade.compra;
                    }
                    //entrada venda
                    if (!dia[d - 1].bNoMercado && (t==0 || (t > 0 && dia[d - 1].TradesDia[t - 1].tttipoTrade == Trade.tipoTrade.compra)) && ((dia[d - 1].Ticker[i].iMaximo >= dia[d - 1].iEntradaVenda && dia[d - 1].Ticker[i].iMinimo <= dia[d - 1].iEntradaVenda)))
                    {
                        if (dia[d - 1].TradesDia[t] == null)
                            dia[d - 1].TradesDia[t] = new Trade();
                        dia[d - 1].TradesDia[t].dtHoraEntrada = dia[d - 1].Ticker[i].dtData;
                        dia[d - 1].TradesDia[t].iValorEntrada = dia[d - 1].iEntradaVenda;
                        dia[d - 1].bNoMercado = true;
                        dia[d - 1].iSaidaVenda = (dia[d - 1].iEntradaCompra - dia[d - 1].iEntradaVenda) < 420 ? dia[d - 1].iEntradaCompra : dia[d - 1].iEntradaVenda + 420;
                        //dia[d - 1].iSaidaVenda = (dia[d - 1].iEntradaCompra - dia[d - 1].iEntradaVenda) < 400 ? dia[d - 1].iEntradaCompra : dia[d - 1].iEntradaVenda + 400;
                        dia[d - 1].TradesDia[t].iValorStop = dia[d - 1].iSaidaVenda;
                        dia[d - 1].TradesDia[t].tttipoTrade = Trade.tipoTrade.venda;
                    }
                }
                else if (dia[d - 1].bNoMercado)
                {
                    dia[d - 1].bNoMercado = false;
                    if (dia[d - 1].TradesDia[t].tttipoTrade == Trade.tipoTrade.compra)
                    {
                        dia[d - 1].TradesDia[t].iAtingiu200 = dia[d - 1].TradesDia[t].iAtingiu200 != 0 ? dia[d - 1].TradesDia[t].iAtingiu200 : dia[d - 1].Ticker[i].iFechamento - dia[d - 1].TradesDia[t].iValorEntrada;
                        dia[d - 1].TradesDia[t].iAtingiu400 = dia[d - 1].TradesDia[t].iAtingiu400 != 0 ? dia[d - 1].TradesDia[t].iAtingiu400 : dia[d - 1].Ticker[i].iFechamento - dia[d - 1].TradesDia[t].iValorEntrada;
                        dia[d - 1].TradesDia[t].iAtingiu1000 = dia[d - 1].TradesDia[t].iAtingiu1000 != 0 ? dia[d - 1].TradesDia[t].iAtingiu1000 : dia[d - 1].Ticker[i].iFechamento - dia[d - 1].TradesDia[t].iValorEntrada;
                        dia[d - 1].TradesDia[t].iFechamento = dia[d - 1].Ticker[i].iFechamento - dia[d - 1].TradesDia[t].iValorEntrada;
                    }
                    if (dia[d - 1].TradesDia[t].tttipoTrade == Trade.tipoTrade.venda)
                    {
                        dia[d - 1].TradesDia[t].iAtingiu200 = dia[d - 1].TradesDia[t].iAtingiu200 != 0 ? dia[d - 1].TradesDia[t].iAtingiu200 : dia[d - 1].TradesDia[t].iValorEntrada - dia[d - 1].Ticker[i].iFechamento;
                        dia[d - 1].TradesDia[t].iAtingiu400 = dia[d - 1].TradesDia[t].iAtingiu400 != 0 ? dia[d - 1].TradesDia[t].iAtingiu400 : dia[d - 1].TradesDia[t].iValorEntrada - dia[d - 1].Ticker[i].iFechamento;
                        dia[d - 1].TradesDia[t].iAtingiu1000 = dia[d - 1].TradesDia[t].iAtingiu1000 != 0 ? dia[d - 1].TradesDia[t].iAtingiu1000 : dia[d - 1].TradesDia[t].iValorEntrada - dia[d - 1].Ticker[i].iFechamento;
                        dia[d - 1].TradesDia[t].iFechamento = dia[d - 1].TradesDia[t].iValorEntrada - dia[d - 1].Ticker[i].iFechamento;
                    }


                }
            }

            StreamWriter writeFile = new StreamWriter(@"C:\Users\Pedro\Investimentos\teste_result_correto.csv");
            writeFile.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7};{8}","DATA","TIPO","ENTRADA","STOP","CHEGOU","ALVO A","ALVO B","ALVO C","FECHAMENTO");
            for (int d = 0; dia[d] != null; d++)
                for (int t = 0; dia[d].TradesDia[t] != null; t++)
                    writeFile.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7};{8}", dia[d].TradesDia[t].dtHoraEntrada, dia[d].TradesDia[t].tttipoTrade, dia[d].TradesDia[t].iValorEntrada,
                        dia[d].TradesDia[t].bStop, dia[d].TradesDia[t].iMaxTrade, dia[d].TradesDia[t].iAtingiu200, dia[d].TradesDia[t].iAtingiu400, dia[d].TradesDia[t].iAtingiu1000,
                        dia[d].TradesDia[t].iFechamento);
            writeFile.Flush();
            writeFile.Close();

            MessageBox.Show("Backtest calculado");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
