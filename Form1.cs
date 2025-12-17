using System;
using System.Drawing;
using System.Windows.Forms;

namespace VendingMachinePlus
{
    public partial class Form1 : Form
    {
        bool[] kwStanKontoli = { true, false, false };
        const int kwMaxIlosc = 200;
        const int kwNajmnWartosc = 10;
        const int kwMargines = 20;
        float[] kwNominaly = { 200, 100, 50, 20, 10, 5, 2, 1, 0.50F, 0.20F, 0.10F };
        struct kwNominal
        {
            public ushort kwLicznosc;
            public float kwWartosc;
        }
        kwNominal[] kwPojemnik;

        TextBox kwtxtDolnaGranicaPrzedzialu = new TextBox();
        TextBox kwtxtGornaGranicaPrzedzialu = new TextBox();
        Button kwbtnPrzyciskAkecptacjiNominalow = new Button();
        Label kwlblEtykietaDolnejGranicyPrzedzialu = new Label();
        Label kwlblEtykietaGornejGranicyPrzedzialu = new Label();
        bool kwKontolkiDodane = false;



        int[] kwZakupy = { 0, 0, 0, 0, 0, 0 };
        int[] kwMiejsce = { 0, 0, 0, 0, 0, 0 };
        bool kwUsuwanie = false;
        int kwLicznikProd = 0;
        double kwSuma = 0;
        double kwRoznica = 0;
        double kwWplacono = 0;

        int kwx = 0;

        public Form1()
        {
            InitializeComponent();
            kwKontrola.SelectedTab = kwPagePulpit;            
            kwPojemnik = new kwNominal[kwNominaly.Length];
        }

        private void kwKontrola_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == kwKontrola.TabPages[0])
            {
                if (kwStanKontoli[0])
                {
                    e.Cancel = false;
                    kwKontrola.SelectedTab = kwPagePulpit;
                }
                else
                    e.Cancel = true;
            }
            else
                if (e.TabPage == kwKontrola.TabPages[1])
            {
                if (kwStanKontoli[1])
                {
                    e.Cancel = false;
                    kwKontrola.SelectedTab = kwPageBankomat;
                }
                else
                    e.Cancel = true;
            }
            else
                if (e.TabPage == kwKontrola.TabPages[2])
            {
                if (kwStanKontoli[2])
                {
                    e.Cancel = false;
                    kwKontrola.SelectedTab = kwPageAutomat;
                }
                else
                    e.Cancel = true;
            }
        }

        private void kwbtnWyplata_Click(object sender, EventArgs e)
        {
            kwStanKontoli[0] = false;
            kwStanKontoli[1] = true;
            kwKontrola.SelectedTab = kwPageBankomat;
            kwlbltabopis.Visible = false;
            kwdgvTabelaNom.Visible = false;
            kwbtnAkceptacja.Enabled = true;
            kwdgvResztaA.Visible = false;
            kwdgvProdukty.Visible = true;
        }

        private void kwbtnAutomat_Click(object sender, EventArgs e)
        {
            kwStanKontoli[0] = false;
            kwStanKontoli[2] = true;
            kwKontrola.SelectedTab = kwPageAutomat;
            
        }

        private void kwbtnPowrot1_Click(object sender, EventArgs e)
        {
            kwStanKontoli[1] = false;
            kwStanKontoli[0] = true;
            kwKontrola.SelectedTab = kwPagePulpit;
        }

        private void kwbtnPowrot2_Click(object sender, EventArgs e)
        {
            kwStanKontoli[2] = false;
            kwStanKontoli[0] = true;
            kwKontrola.SelectedTab = kwPagePulpit;
        }

        private void kwbtnAkceptacja_Click(object sender, EventArgs e)
        {
            errorProvider1.Dispose();
            if (kwcmbWalutaBankomat.SelectedIndex < 0)
            {
                errorProvider1.SetError(kwcmbWalutaBankomat, "Musisz wybrać walutę wypłaty");
                return;
            }
            if (!(kwrbtLosowo.Checked || kwrbtPrzedzialowo.Checked))
            {
                errorProvider1.SetError(kwBox, "Wybierz sposób wypłaty nominałów");
                return;
            }
            if (kwrbtLosowo.Checked)
            {
                Random kwlos = new Random();
                for (ushort kwi = 0; kwi < kwPojemnik.Length; kwi++)
                {
                    kwPojemnik[kwi].kwWartosc = kwNominaly[kwi];
                    kwPojemnik[kwi].kwLicznosc = (ushort)(kwlos.Next(kwMaxIlosc));
                }
                kwlbltabopis.Visible = true;
                kwdgvTabelaNom.Visible = true;
                kwdgvTabelaNom.Rows.Clear();
                for (int kwi = 0; kwi < kwPojemnik.Length; kwi++)
                {
                    kwdgvTabelaNom.Rows.Add();
                    kwdgvTabelaNom.Rows[kwi].Cells[0].Value = kwPojemnik[kwi].kwLicznosc;
                    kwdgvTabelaNom.Rows[kwi].Cells[1].Value = kwPojemnik[kwi].kwWartosc;
                    if (kwPojemnik[kwi].kwWartosc >= kwNajmnWartosc)
                    {
                        kwdgvTabelaNom.Rows[kwi].Cells[2].Value = "banknot";
                    }
                    else
                        kwdgvTabelaNom.Rows[kwi].Cells[2].Value = "moneta";

                    kwdgvTabelaNom.Rows[kwi].Cells[3].Value = kwcmbWalutaBankomat.Text;

                    for (ushort kwk = 0; kwk < 4; kwk++)
                    {
                        kwdgvTabelaNom.Rows[kwi].Cells[kwk].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    kwbtnAkceptacja.Enabled = false;
                }



            }
            else
            {
                if (kwKontolkiDodane)
                {
                    kwlblEtykietaDolnejGranicyPrzedzialu.Visible = true;
                    kwtxtDolnaGranicaPrzedzialu.Visible = true;
                    kwtxtDolnaGranicaPrzedzialu.Enabled = true;
                    kwlblEtykietaGornejGranicyPrzedzialu.Visible = true;
                    kwtxtGornaGranicaPrzedzialu.Visible = true;
                    kwtxtGornaGranicaPrzedzialu.Enabled = true;
                    kwbtnPrzyciskAkecptacjiNominalow.Visible = true;
                    kwbtnPrzyciskAkecptacjiNominalow.Enabled = true;
                }
                else
                {
                    kwKontolkiDodane = true;
                    kwlblEtykietaDolnejGranicyPrzedzialu.Text = "Dolna granica przedziału liczebności";
                    kwlblEtykietaDolnejGranicyPrzedzialu.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Italic);
                    kwlblEtykietaDolnejGranicyPrzedzialu.TextAlign = ContentAlignment.MiddleCenter;
                    kwlblEtykietaDolnejGranicyPrzedzialu.Visible = true;
                    kwlblEtykietaDolnejGranicyPrzedzialu.Location = new Point(kwcmbWalutaBankomat.Left + kwcmbWalutaBankomat.Width,
                        groupBox1.Top + groupBox1.Height + kwMargines / 2);
                    kwlblEtykietaDolnejGranicyPrzedzialu.Height = 60;
                    kwlblEtykietaDolnejGranicyPrzedzialu.Width = 150;
                    kwlblEtykietaDolnejGranicyPrzedzialu.BackColor = kwKontrola.TabPages[1].BackColor;
                    kwlblEtykietaDolnejGranicyPrzedzialu.ForeColor = Color.Black;
                    kwKontrola.TabPages[1].Controls.Add(kwlblEtykietaDolnejGranicyPrzedzialu);

                    kwtxtDolnaGranicaPrzedzialu.BackColor = Color.White;
                    kwtxtDolnaGranicaPrzedzialu.ForeColor = Color.Black;
                    kwtxtDolnaGranicaPrzedzialu.Visible = true;
                    kwtxtDolnaGranicaPrzedzialu.Enabled = true;
                    kwtxtDolnaGranicaPrzedzialu.Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold);
                    kwtxtDolnaGranicaPrzedzialu.Text = "";
                    kwtxtDolnaGranicaPrzedzialu.TextAlign = HorizontalAlignment.Center;
                    kwtxtDolnaGranicaPrzedzialu.Location = new Point(kwlblEtykietaDolnejGranicyPrzedzialu.Left +
                        kwlblEtykietaDolnejGranicyPrzedzialu.Width + kwMargines / 2,
                        kwlblEtykietaDolnejGranicyPrzedzialu.Top);
                    kwtxtDolnaGranicaPrzedzialu.Size = new Size(50, 50);
                    kwtxtDolnaGranicaPrzedzialu.BorderStyle = BorderStyle.FixedSingle;
                    kwKontrola.TabPages[1].Controls.Add(kwtxtDolnaGranicaPrzedzialu);

                    kwlblEtykietaGornejGranicyPrzedzialu.Text = "Górna granica przedziału liczebności";
                    kwlblEtykietaGornejGranicyPrzedzialu.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Italic);
                    kwlblEtykietaGornejGranicyPrzedzialu.TextAlign = ContentAlignment.MiddleCenter;
                    kwlblEtykietaGornejGranicyPrzedzialu.Location = new Point(kwtxtDolnaGranicaPrzedzialu.Left
                        + kwtxtDolnaGranicaPrzedzialu.Width + kwMargines / 2, kwtxtDolnaGranicaPrzedzialu.Top);
                    kwlblEtykietaGornejGranicyPrzedzialu.Height = 60;
                    kwlblEtykietaGornejGranicyPrzedzialu.Width = 150;
                    kwlblEtykietaGornejGranicyPrzedzialu.BackColor = kwKontrola.TabPages[1].BackColor;
                    kwlblEtykietaGornejGranicyPrzedzialu.ForeColor = kwKontrola.TabPages[1].ForeColor;
                    kwlblEtykietaGornejGranicyPrzedzialu.Visible = true;
                    kwKontrola.TabPages[1].Controls.Add(kwlblEtykietaGornejGranicyPrzedzialu);

                    kwtxtGornaGranicaPrzedzialu.BackColor = Color.White;
                    kwtxtGornaGranicaPrzedzialu.ForeColor = Color.Black;
                    kwtxtGornaGranicaPrzedzialu.Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold);
                    kwtxtGornaGranicaPrzedzialu.Text = "";
                    kwtxtGornaGranicaPrzedzialu.Location = new Point(kwlblEtykietaGornejGranicyPrzedzialu.Left +
                        kwlblEtykietaGornejGranicyPrzedzialu.Width + kwMargines / 2, kwlblEtykietaGornejGranicyPrzedzialu.Top);
                    kwtxtGornaGranicaPrzedzialu.Size = new Size(50, 30);
                    kwtxtGornaGranicaPrzedzialu.Visible = true;
                    kwtxtGornaGranicaPrzedzialu.Enabled = true;
                    kwtxtGornaGranicaPrzedzialu.BorderStyle = BorderStyle.FixedSingle;
                    kwKontrola.TabPages[1].Controls.Add(kwtxtGornaGranicaPrzedzialu);

                    kwbtnPrzyciskAkecptacjiNominalow.Text = "Akceptacja przedziału liczności nominałów";
                    kwbtnPrzyciskAkecptacjiNominalow.BackColor = this.BackColor;
                    kwbtnPrzyciskAkecptacjiNominalow.ForeColor = Color.Black;
                    kwbtnPrzyciskAkecptacjiNominalow.Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold);
                    kwbtnPrzyciskAkecptacjiNominalow.Location = new Point(kwtxtGornaGranicaPrzedzialu.Left +
                        kwtxtGornaGranicaPrzedzialu.Width + kwMargines / 2, kwtxtGornaGranicaPrzedzialu.Top);
                    kwbtnPrzyciskAkecptacjiNominalow.Size = new Size(170, 60);
                    kwbtnPrzyciskAkecptacjiNominalow.TextAlign = ContentAlignment.MiddleCenter;
                    kwbtnPrzyciskAkecptacjiNominalow.Visible = true;
                    kwbtnPrzyciskAkecptacjiNominalow.Enabled = true;
                    kwKontrola.TabPages[1].Controls.Add(kwbtnPrzyciskAkecptacjiNominalow);
                    kwbtnPrzyciskAkecptacjiNominalow.Click += new EventHandler(kwbtnPrzyciskAkecptacjiNominalow_Click);

                    kwlblPodajKwote.Enabled = true;
                    kwtxtKwota.Enabled = true;
                    kwbtnAkceptacja.Enabled = true;

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (kwUsuwanie)
            {
                errorProvider1.Dispose();
                if (kwZakupy[0] == 0)
                {
                    errorProvider1.SetError(kwbtnCzekolada, "W koszyku nie znajduje sie ten przedmiot");
                    return;

                }
                else
                {
                    kwZakupy[0]--;

                    kwdgvProdukty.Rows[kwMiejsce[0]].Cells[1].Value = kwZakupy[0];
                    kwdgvProdukty.Rows[kwMiejsce[0]].Cells[2].Value = (kwZakupy[0] * 3).ToString("F");
                    if (kwZakupy[0] == 0)
                    {
                        kwdgvProdukty.Rows.RemoveAt(kwMiejsce[0]);
                        kwLicznikProd--;
                        for (int kwi = 0; kwi < kwMiejsce.Length; kwi++)
                        {
                            if (kwMiejsce[kwi] > kwMiejsce[0])
                            {
                                kwMiejsce[kwi]--;
                            }
                        }
                    }
                    kwSuma -= 3;
                    kwtxtbSuma.Text = kwSuma.ToString("F");
                    kwRoznica = kwSuma - kwWplacono;
                    if (kwRoznica > 0)
                    {
                        kwtxtbDoZap.Text = kwRoznica.ToString("F");
                    }
                    else
                    {
                        kwtxtbDoZap.Text = "0.00";
                        kwtxtReszta.Text = (kwRoznica * (-1)).ToString("F");
                    }
                }

            }
            else
            {

                if (kwZakupy[0] == 0)
                {
                    kwdgvProdukty.Rows.Add();
                    kwMiejsce[0] = kwLicznikProd;
                    kwLicznikProd++;
                    kwdgvProdukty.Rows[kwMiejsce[0]].Cells[0].Value = "Czekolada";
                }
                kwZakupy[0]++;


                kwdgvProdukty.Rows[kwMiejsce[0]].Cells[1].Value = kwZakupy[0];
                kwdgvProdukty.Rows[kwMiejsce[0]].Cells[2].Value = (kwZakupy[0] * 3).ToString("F");
                kwSuma += 3;
                kwtxtbSuma.Text = kwSuma.ToString("F");
                kwRoznica = kwSuma - kwWplacono;
                if (kwRoznica > 0)
                {
                    kwtxtbDoZap.Text = kwRoznica.ToString("F");
                }
                else
                {
                    kwtxtbDoZap.Text = "0.00";
                    kwtxtReszta.Text = (kwRoznica * (-1)).ToString("F");
                }


            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            errorProvider1.Dispose();
            if (kwUsuwanie)
            {
                if (kwZakupy[1] == 0)
                {
                    errorProvider1.SetError(kwbtnCiastka, "W koszyku nie znajduje sie ten przedmiot");
                    return;

                }
                else
                {
                    kwZakupy[1]--;

                    kwdgvProdukty.Rows[kwMiejsce[1]].Cells[1].Value = kwZakupy[1];
                    kwdgvProdukty.Rows[kwMiejsce[1]].Cells[2].Value = (kwZakupy[0] * 2.60).ToString("F");
                    if (kwZakupy[1] == 0)
                    {
                        kwdgvProdukty.Rows.RemoveAt(kwMiejsce[1]);
                        kwLicznikProd--;
                        for (int kwi = 0; kwi < kwMiejsce.Length; kwi++)
                        {
                            if (kwMiejsce[kwi] > kwMiejsce[1])
                            {
                                kwMiejsce[kwi]--;
                            }
                        }
                    }
                    kwSuma -= 2.60;
                    kwtxtbSuma.Text = kwSuma.ToString("F");
                    kwRoznica = kwSuma - kwWplacono;
                    if (kwRoznica > 0)
                    {
                        kwtxtbDoZap.Text = kwRoznica.ToString("F");
                    }
                    else
                    {
                        kwtxtbDoZap.Text = "0.00";
                        kwtxtReszta.Text = (kwRoznica * (-1)).ToString("F");
                    }
                }


            }
            else
            {

                if (kwZakupy[1] == 0)
                {
                    kwdgvProdukty.Rows.Add();
                    kwMiejsce[1] = kwLicznikProd;
                    kwLicznikProd++;
                    kwdgvProdukty.Rows[kwMiejsce[1]].Cells[0].Value = "Ciastka";
                }
                kwZakupy[1]++;


                kwdgvProdukty.Rows[kwMiejsce[1]].Cells[1].Value = kwZakupy[1];
                kwdgvProdukty.Rows[kwMiejsce[1]].Cells[2].Value = (kwZakupy[1] * 2.60).ToString("F");
                kwSuma += 2.60;
                kwtxtbSuma.Text = kwSuma.ToString("F");
                kwRoznica = kwSuma - kwWplacono;
                if (kwRoznica > 0)
                {
                    kwtxtbDoZap.Text = kwRoznica.ToString("F");
                }
                else
                {
                    kwtxtbDoZap.Text = "0.00";
                    kwtxtReszta.Text = (kwRoznica * (-1)).ToString("F");
                }




            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            errorProvider1.Dispose();
            if (kwUsuwanie)
            {
                if (kwZakupy[2] == 0)
                {
                    errorProvider1.SetError(kwbtnChrupki, "W koszyku nie znajduje sie ten przedmiot");
                    return;

                }
                else
                {
                    kwZakupy[2]--;

                    kwdgvProdukty.Rows[kwMiejsce[2]].Cells[1].Value = kwZakupy[2];
                    kwdgvProdukty.Rows[kwMiejsce[2]].Cells[2].Value = (kwZakupy[2] * 4.20).ToString("F");

                    if (kwZakupy[2] == 0)
                    {
                        kwdgvProdukty.Rows.RemoveAt(kwMiejsce[2]);
                        kwLicznikProd--;
                        for (int kwi = 0; kwi < kwMiejsce.Length; kwi++)
                        {
                            if (kwMiejsce[kwi] > kwMiejsce[2])
                            {
                                kwMiejsce[kwi]--;
                            }
                        }
                    }
                    kwSuma -= 4.20;
                    kwtxtbSuma.Text = kwSuma.ToString("F");
                    kwRoznica = kwSuma - kwWplacono;
                    if (kwRoznica > 0)
                    {
                        kwtxtbDoZap.Text = kwRoznica.ToString("F");
                    }
                    else
                    {
                        kwtxtbDoZap.Text = "0.00";
                        kwtxtReszta.Text = (kwRoznica * (-1)).ToString("F");
                    }
                }

            }
            else
            {

                if (kwZakupy[2] == 0)
                {
                    kwdgvProdukty.Rows.Add();
                    kwMiejsce[2] = kwLicznikProd;
                    kwLicznikProd++;
                    kwdgvProdukty.Rows[kwMiejsce[2]].Cells[0].Value = "Chrupki";
                }
                kwZakupy[2]++;


                kwdgvProdukty.Rows[kwMiejsce[2]].Cells[1].Value = kwZakupy[2];
                kwdgvProdukty.Rows[kwMiejsce[2]].Cells[2].Value = (kwZakupy[2] * 4.20).ToString("F");
                kwSuma += 4.20;
                kwtxtbSuma.Text = kwSuma.ToString("F");
                kwRoznica = kwSuma - kwWplacono;
                if (kwRoznica > 0)
                {
                    kwtxtbDoZap.Text = kwRoznica.ToString("F");
                }
                else
                {
                    kwtxtbDoZap.Text = "0.00";
                    kwtxtReszta.Text = (kwRoznica * (-1)).ToString("F");
                }

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            errorProvider1.Dispose();
            if (kwUsuwanie)
            {
                if (kwZakupy[3] == 0)
                {
                    errorProvider1.SetError(kwbtnPaluszki, "W koszyku nie znajduje sie ten przedmiot");
                    return;

                }
                else
                {
                    kwZakupy[3]--;

                    kwdgvProdukty.Rows[kwMiejsce[3]].Cells[1].Value = kwZakupy[3];
                    kwdgvProdukty.Rows[kwMiejsce[3]].Cells[2].Value = (kwZakupy[3] * 2.30).ToString("F");

                    if (kwZakupy[3] == 0)
                    {
                        kwdgvProdukty.Rows.RemoveAt(kwMiejsce[3]);
                        kwLicznikProd--;
                        for (int kwi = 0; kwi < kwMiejsce.Length; kwi++)
                        {
                            if (kwMiejsce[kwi] > kwMiejsce[3])
                            {
                                kwMiejsce[kwi]--;
                            }
                        }
                    }
                    kwSuma -= 2.30;
                    kwtxtbSuma.Text = kwSuma.ToString("F");
                    kwRoznica = kwSuma - kwWplacono;
                    if (kwRoznica > 0)
                    {
                        kwtxtbDoZap.Text = kwRoznica.ToString("F");
                    }
                    else
                    {
                        kwtxtbDoZap.Text = "0.00";
                        kwtxtReszta.Text = (kwRoznica * (-1)).ToString("F");
                    }
                }

            }
            else
            {

                if (kwZakupy[3] == 0)
                {
                    kwdgvProdukty.Rows.Add();
                    kwMiejsce[3] = kwLicznikProd;
                    kwLicznikProd++;
                    kwdgvProdukty.Rows[kwMiejsce[3]].Cells[0].Value = "Paluszki";
                }
                kwZakupy[3]++;


                kwdgvProdukty.Rows[kwMiejsce[3]].Cells[1].Value = kwZakupy[3];
                kwdgvProdukty.Rows[kwMiejsce[3]].Cells[2].Value = (kwZakupy[3] * 2.30).ToString("F");
                kwSuma += 2.30;
                kwtxtbSuma.Text = kwSuma.ToString("F");
                kwRoznica = kwSuma - kwWplacono;
                if (kwRoznica > 0)
                {
                    kwtxtbDoZap.Text = kwRoznica.ToString("F");
                }
                else
                {
                    kwtxtbDoZap.Text = "0.00";
                    kwtxtReszta.Text = (kwRoznica * (-1)).ToString("F");
                }

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            errorProvider1.Dispose();
            if (kwUsuwanie)
            {
                if (kwZakupy[4] == 0)
                {
                    errorProvider1.SetError(kwbtnWoda, "W koszyku nie znajduje sie ten przedmiot");
                    return;

                }
                else
                {
                    kwZakupy[4]--;

                    kwdgvProdukty.Rows[kwMiejsce[4]].Cells[1].Value = kwZakupy[4];
                    kwdgvProdukty.Rows[kwMiejsce[4]].Cells[2].Value = (kwZakupy[4] * 1.20).ToString("F");

                    if (kwZakupy[4] == 0)
                    {
                        kwdgvProdukty.Rows.RemoveAt(kwMiejsce[4]);
                        kwLicznikProd--;
                        for (int kwi = 0; kwi < kwMiejsce.Length; kwi++)
                        {
                            if (kwMiejsce[kwi] > kwMiejsce[4])
                            {
                                kwMiejsce[kwi]--;
                            }
                        }
                    }
                    kwSuma -= 1.20;
                    kwtxtbSuma.Text = kwSuma.ToString("F");
                    kwRoznica = kwSuma - kwWplacono;
                    if (kwRoznica > 0)
                    {
                        kwtxtbDoZap.Text = kwRoznica.ToString("F");
                    }
                    else
                    {
                        kwtxtbDoZap.Text = "0.00";
                        kwtxtReszta.Text = (kwRoznica * (-1)).ToString("F");
                    }
                }

            }
            else
            {

                if (kwZakupy[4] == 0)
                {
                    kwdgvProdukty.Rows.Add();
                    kwMiejsce[4] = kwLicznikProd;
                    kwLicznikProd++;
                    kwdgvProdukty.Rows[kwMiejsce[4]].Cells[0].Value = "Woda";
                }
                kwZakupy[4]++;


                kwdgvProdukty.Rows[kwMiejsce[4]].Cells[1].Value = kwZakupy[4];
                kwdgvProdukty.Rows[kwMiejsce[4]].Cells[2].Value = (kwZakupy[4] * 1.20).ToString("F");
                kwSuma += 1.20;
                kwtxtbSuma.Text = kwSuma.ToString("F");
                kwRoznica = kwSuma - kwWplacono;
                if (kwRoznica > 0)
                {
                    kwtxtbDoZap.Text = kwRoznica.ToString("F");
                }
                else
                {
                    kwtxtbDoZap.Text = "0.00";
                    kwtxtReszta.Text = (kwRoznica * (-1)).ToString("F");
                }

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            errorProvider1.Dispose();
            if (kwUsuwanie)
            {
                if (kwZakupy[5] == 0)
                {
                    errorProvider1.SetError(kwbtnOranzada, "W koszyku nie znajduje sie ten przedmiot");
                    return;

                }
                else
                {
                    kwZakupy[5]--;

                    kwdgvProdukty.Rows[kwMiejsce[5]].Cells[1].Value = kwZakupy[5];
                    kwdgvProdukty.Rows[kwMiejsce[5]].Cells[2].Value = (kwZakupy[5] * 1.80).ToString("F");

                    if (kwZakupy[5] == 0)
                    {
                        kwdgvProdukty.Rows.RemoveAt(kwMiejsce[5]);
                        kwLicznikProd--;
                        for (int kwi = 0; kwi < kwMiejsce.Length; kwi++)
                        {
                            if (kwMiejsce[kwi] > kwMiejsce[5])
                            {
                                kwMiejsce[kwi]--;
                            }
                        }
                    }
                    kwSuma -= 1.80;
                    kwtxtbSuma.Text = kwSuma.ToString("F");
                    kwRoznica = kwSuma - kwWplacono;
                    if (kwRoznica > 0)
                    {
                        kwtxtbDoZap.Text = kwRoznica.ToString("F");
                    }
                    else
                    {
                        kwtxtbDoZap.Text = "0.00";
                        kwtxtReszta.Text = (kwRoznica * (-1)).ToString("F");
                    }
                }

            }
            else
            {

                if (kwZakupy[5] == 0)
                {
                    kwdgvProdukty.Rows.Add();
                    kwMiejsce[5] = kwLicznikProd;
                    kwLicznikProd++;
                    kwdgvProdukty.Rows[kwMiejsce[5]].Cells[0].Value = "Oranżada";
                }
                kwZakupy[5]++;


                kwdgvProdukty.Rows[kwMiejsce[5]].Cells[1].Value = kwZakupy[5];
                kwdgvProdukty.Rows[kwMiejsce[5]].Cells[2].Value = (kwZakupy[5] * 1.80).ToString("F");
                kwSuma += 1.80;
                kwtxtbSuma.Text = kwSuma.ToString("F");
                kwRoznica = kwSuma - kwWplacono;
                if (kwRoznica > 0)
                {
                    kwtxtbDoZap.Text = kwRoznica.ToString("F");
                }
                else
                {
                    kwtxtbDoZap.Text = "0.00";
                    kwtxtReszta.Text = (kwRoznica * (-1)).ToString("F");
                }

            }
        }

        private void kwcbUsuwanie_CheckedChanged(object sender, EventArgs e)
        {
            if (kwcbUsuwanie.Checked == true)
                kwUsuwanie = true;
            else
                kwUsuwanie = false;
        }

        private void kwbtn200_Click(object sender, EventArgs e)
        {
            errorProvider1.Dispose();
            if(kwLicznikProd == 0)
            {
                errorProvider1.SetError(kwbtn200, "Najpierw wybierz produkty");
                return;
            }
            kwWplacono += 200;
            kwtxtbWplacono.Text = kwWplacono.ToString("F");
            kwRoznica = kwSuma - kwWplacono;
            kwcbUsuwanie.Enabled = false;
            if (kwRoznica > 0)
            {
                kwtxtbDoZap.Text = kwRoznica.ToString("F");
                
            }
            else
            {
                kwtxtbDoZap.Text = "0.00";
                kwtxtReszta.Text = (kwRoznica * (-1)).ToString("F");
                kwgbLiczbyDoZap.Enabled = false;
            }
        }

        private void kwbtn100_Click(object sender, EventArgs e)
        {
            errorProvider1.Dispose();
            if (kwLicznikProd == 0)
            {
                errorProvider1.SetError(kwbtn100, "Najpierw wybierz produkty");
                return;
            }
            kwWplacono += 100;
            kwtxtbWplacono.Text = kwWplacono.ToString("F");
            kwRoznica = kwSuma - kwWplacono;
            if (kwRoznica > 0)
            {
                kwtxtbDoZap.Text = kwRoznica.ToString("F");
            }
            else
            {
                kwtxtbDoZap.Text = "0.00";
                kwtxtReszta.Text = (kwRoznica * (-1)).ToString("F");
                kwgbLiczbyDoZap.Enabled = false;
            }
        }

        private void kwbtnAnuluj_Click(object sender, EventArgs e)
        {
            kwdgvProdukty.Rows.Clear();
            kwgbLiczbyDoZap.Enabled = true;
            kwSuma = 0;
            kwRoznica = 0;
            kwWplacono = 0;
            kwUsuwanie = false;
            kwLicznikProd = 0;
            for (int kwreset = 0; kwreset < kwMiejsce.Length; kwreset++)
            {
                kwMiejsce[kwreset] = 0;
                kwZakupy[kwreset] = 0;
            }
            kwtxtbWplacono.Text = "0.00";
            kwtxtbSuma.Text = "0.00";
            kwtxtReszta.Text = "0.00";
            kwcbUsuwanie.Checked = false;
        }

        private void kwbtnPotwierdz_Click(object sender, EventArgs e)
        {
            errorProvider1.Dispose();
            int kwNumerWTabeli=0;


            if (kwcmbWalutaAutomat.Text != "PLN" && kwcmbWalutaAutomat.Text != "EURO" && kwcmbWalutaAutomat.Text != "USD" )
            {
                errorProvider1.SetError(kwcmbWalutaAutomat, "Proszę wybrać walutę z listy");
                return;
            }

            if (kwcmbSposobWyplatyAutomat.Text != "Losowo" && kwcmbSposobWyplatyAutomat.Text != "Najmniejsza ilość")
            {
                errorProvider1.SetError(kwcmbSposobWyplatyAutomat, "Proszę wybrać sposób wypłaty reszty z listy ");
                return;
            }
            if(kwLicznikProd == 0)
            {
                errorProvider1.SetError(kwbtnPotwierdz, "Nie wybrano żadnego produktu");
                return;
            }

            if(kwcmbsposob.Text != "Płatność kartą" && kwcmbsposob.Text != "Płatność gotówką")
            {
                errorProvider1.SetError(kwcmbsposob, "Proszę wybrać sposób zapłaty z listy");
                return;
            }

            if (kwcmbsposob.Text == "Płatność kartą")
            {
                if (string.IsNullOrEmpty(kwtxtNumerKonta.Text))
                {
                    errorProvider1.SetError(kwtxtNumerKonta, "Proszę podać numer konta");
                    return;
                }
                if (!int.TryParse(kwtxtNumerKonta.Text, out int x))
                {
                    errorProvider1.SetError(kwtxtNumerKonta, "Błędnie podany numer konta");
                    return;
                }
                kwlblDokonane.Visible = true;



            }


            else
            {
                float kwIloscDoOddania = (float)(kwRoznica  * (-1));
                if (kwRoznica > 0)
                {
                    errorProvider1.SetError(kwtxtbDoZap, "Proszę wpłacić pozostałą kwotę");
                    return;
                }

                kwdgvProdukty.Rows.Clear();
                kwgbProdukty.Enabled = false;
                kwlblDokonane.Visible = true;
                
                kwSuma = 0;
                kwRoznica = 0;
                kwWplacono = 0;
                kwUsuwanie = false;
                kwLicznikProd = 0;
                for (int kwreset = 0; kwreset < kwMiejsce.Length; kwreset++)
                {
                    kwMiejsce[kwreset] = 0;
                    kwZakupy[kwreset] = 0;
                }


                kwcbUsuwanie.Checked = false;
                kwdgvProdukty.Visible = false;

                
                if (kwcmbSposobWyplatyAutomat.Text == "Losowo")
                {
                    Random kwlos = new Random();
                    while (kwIloscDoOddania > 0)
                    {
                        for (ushort kwi = 0; kwi < kwPojemnik.Length; kwi++)
                        {
                            if (kwNominaly[kwi] <= kwIloscDoOddania)
                            {
                                kwPojemnik[kwi].kwWartosc = kwNominaly[kwi];
                                do
                                {
                                    ushort kwWylosowanaLiczba = (ushort)kwlos.Next(kwMaxIlosc);
                                    kwPojemnik[kwi].kwLicznosc += kwWylosowanaLiczba;
                                } while (kwPojemnik[kwi].kwLicznosc * kwPojemnik[kwi].kwWartosc > kwIloscDoOddania);

                            }
                            kwIloscDoOddania = (float)(kwIloscDoOddania - (kwPojemnik[kwi].kwLicznosc * kwPojemnik[kwi].kwWartosc));
                            
                        }
                        
                        if (kwIloscDoOddania>0)
                        {
                            kwPojemnik[kwPojemnik.Length-1].kwLicznosc += (ushort)(kwIloscDoOddania / 0.1F);

                        }
                       
                        
                    }
                    


                }
                else
                {
                    
                    while(kwIloscDoOddania > 0)
                    {
                        
                        for (ushort kwi = 0; kwi < kwPojemnik.GetLength(0); kwi++)
                        {
                            if (kwNominaly[kwi] <= kwIloscDoOddania)
                            {
                                kwPojemnik[kwi].kwWartosc = kwNominaly[kwi];
                                kwPojemnik[kwi].kwLicznosc++;
                                
                                while ((kwPojemnik[kwi].kwLicznosc+1) * kwPojemnik[kwi].kwWartosc <= kwIloscDoOddania) 
                                {
                                    
                                    kwPojemnik[kwi].kwLicznosc++;
                                    
                                }
                                
                            }
                            kwIloscDoOddania = (kwIloscDoOddania - (kwPojemnik[kwi].kwLicznosc * kwPojemnik[kwi].kwWartosc));
                            
                        }
                    }
                }
                kwdgvResztaA.Visible = true;
                kwdgvResztaA.Rows.Clear();
                
                
                for (int kwi = 0; kwi < kwPojemnik.Length; kwi++)
                {
                    if (kwPojemnik[kwi].kwLicznosc != 0)
                    {
                        kwdgvResztaA.Rows.Add();
                        kwdgvResztaA.Rows[kwNumerWTabeli].Cells[0].Value = kwPojemnik[kwi].kwLicznosc;
                        kwdgvResztaA.Rows[kwNumerWTabeli].Cells[1].Value = kwPojemnik[kwi].kwWartosc;
                        if (kwPojemnik[kwi].kwWartosc >= kwNajmnWartosc)
                        {
                            kwdgvResztaA.Rows[kwNumerWTabeli].Cells[2].Value = "banknot";
                        }
                        else if (kwPojemnik[kwi].kwWartosc == 0)
                        {
                            kwdgvResztaA.Rows[kwNumerWTabeli].Cells[2].Value = "brak";
                        }
                        else
                            kwdgvResztaA.Rows[kwNumerWTabeli].Cells[2].Value = "moneta";
                        kwdgvResztaA.Rows[kwNumerWTabeli].Cells[3].Value = kwcmbWalutaAutomat.Text;
                        kwNumerWTabeli++;
                    }

                }


            }
            kwbtnPotwierdz.Enabled = false;
        }
        void kwbtnPrzyciskAkecptacjiNominalow_Click(object sender, EventArgs e)
        {
            ushort kwDolnaGranica, kwGornaGranica;
            errorProvider1.Dispose();
            if (string.IsNullOrEmpty(kwtxtDolnaGranicaPrzedzialu.Text))
            {
                errorProvider1.SetError(kwtxtDolnaGranicaPrzedzialu, "Musisz podać dolną granicę przedziału liczności nominałów");
                return;
            }
            while (!ushort.TryParse(kwtxtDolnaGranicaPrzedzialu.Text, out kwDolnaGranica))
            {
                errorProvider1.SetError(kwtxtDolnaGranicaPrzedzialu, "Błąd w zapisie dolnej granicy przedziału liczebności nominałów");
                return;
            }
            if (kwDolnaGranica <= 0)
            {
                errorProvider1.SetError(kwtxtDolnaGranicaPrzedzialu, "Dolna granica liczności nominałów musi być > 0");
                return;
            }
            if (string.IsNullOrEmpty(kwtxtGornaGranicaPrzedzialu.Text))
            {
                errorProvider1.SetError(kwtxtGornaGranicaPrzedzialu, "Musisz podać górną granicę liczebności nominałów");
                return;
            }
            while (!ushort.TryParse(kwtxtGornaGranicaPrzedzialu.Text, out kwGornaGranica))
            {
                errorProvider1.SetError(kwtxtGornaGranicaPrzedzialu, "Błąd w zapisise górnej graniccy liczebności nominałów");
                return;
            }
            if (kwGornaGranica <= kwDolnaGranica)
            {
                errorProvider1.SetError(kwtxtGornaGranicaPrzedzialu, "Górna granica przedziału liczebności musi być" +
                    "większa lub równa dolnej granicy liczebności");
                return;
            }
            Random kwrnd = new Random();
            for (int kwk = 0; kwk < kwPojemnik.GetLength(0); kwk++)
            {
                kwPojemnik[kwk].kwLicznosc = (ushort)(kwrnd.NextDouble() * (kwGornaGranica - kwDolnaGranica) + kwDolnaGranica);
                kwPojemnik[kwk].kwWartosc = kwNominaly[kwk];
            }
            kwdgvTabelaNom.Visible = true;
            for (int kwi = 0; kwi < kwPojemnik.GetLength(0); kwi++)
            {
                kwdgvTabelaNom.Rows.Add();
                kwdgvTabelaNom.Rows[kwi].Cells[0].Value = kwPojemnik[kwi].kwLicznosc;
                kwdgvTabelaNom.Rows[kwi].Cells[1].Value = kwPojemnik[kwi].kwWartosc;
                if (kwPojemnik[kwi].kwWartosc >= kwNajmnWartosc)
                    kwdgvTabelaNom.Rows[kwi].Cells[2].Value = "banknot";
                else
                    kwdgvTabelaNom.Rows[kwi].Cells[2].Value = "moneta";
                kwdgvTabelaNom.Rows[kwi].Cells[3].Value = kwcmbWalutaBankomat.Text;

            }
            kwtxtDolnaGranicaPrzedzialu.Enabled = false;
            kwtxtGornaGranicaPrzedzialu.Enabled = false;
            kwcmbWalutaBankomat.Enabled = false;
            kwbtnPrzyciskAkecptacjiNominalow.Enabled = false;

        }

        private void kwbtn50_Click(object sender, EventArgs e)
        {
            errorProvider1.Dispose();
            if (kwLicznikProd == 0)
            {
                errorProvider1.SetError(kwbtn50, "Najpierw wybierz produkty");
                return;
            }
            kwWplacono += 50;
            kwtxtbWplacono.Text = kwWplacono.ToString("F");
            kwRoznica = kwSuma - kwWplacono;
            if (kwRoznica > 0)
            {
                kwtxtbDoZap.Text = kwRoznica.ToString("F");
            }
            else
            {
                kwtxtbDoZap.Text = "0.00";
                kwtxtReszta.Text = (kwRoznica * (-1)).ToString("F");
                kwgbLiczbyDoZap.Enabled = false;
            }
        }

        private void kwbtn20_Click(object sender, EventArgs e)
        {
            errorProvider1.Dispose();
            if (kwLicznikProd == 0)
            {
                errorProvider1.SetError(kwbtn20, "Najpierw wybierz produkty");
                return;
            }
            kwWplacono += 20;
            kwtxtbWplacono.Text = kwWplacono.ToString("F");
            kwRoznica = kwSuma - kwWplacono;
            if (kwRoznica > 0)
            {
                kwtxtbDoZap.Text = kwRoznica.ToString("F");
            }
            else
            {
                kwtxtbDoZap.Text = "0.00";
                kwtxtReszta.Text = (kwRoznica * (-1)).ToString("F");
                kwgbLiczbyDoZap.Enabled = false;
            }
        }

        private void kwbtn10_Click(object sender, EventArgs e)
        {
            errorProvider1.Dispose();
            if (kwLicznikProd == 0)
            {
                errorProvider1.SetError(kwbtn10, "Najpierw wybierz produkty");
                return;
            }
            kwWplacono += 10;
            kwtxtbWplacono.Text = kwWplacono.ToString("F");
            kwRoznica = kwSuma - kwWplacono;
            if (kwRoznica > 0)
            {
                kwtxtbDoZap.Text = kwRoznica.ToString("F");
            }
            else
            {
                kwtxtbDoZap.Text = "0.00";
                kwtxtReszta.Text = (kwRoznica * (-1)).ToString("F");
                kwgbLiczbyDoZap.Enabled = false;
            }
        }

        private void kwbtn5_Click(object sender, EventArgs e)
        {
            errorProvider1.Dispose();
            if (kwLicznikProd == 0)
            {
                errorProvider1.SetError(kwbtn5, "Najpierw wybierz produkty");
                return;
            }
            kwWplacono += 5;
            kwtxtbWplacono.Text = kwWplacono.ToString("F");
            kwRoznica = kwSuma - kwWplacono;
            if (kwRoznica > 0)
            {
                kwtxtbDoZap.Text = kwRoznica.ToString("F");
            }
            else
            {
                kwtxtbDoZap.Text = "0.00";
                kwtxtReszta.Text = (kwRoznica * (-1)).ToString("F");
                kwgbLiczbyDoZap.Enabled = false;
            }
        }

        private void kwbtn2_Click(object sender, EventArgs e)
        {
            errorProvider1.Dispose();
            if (kwLicznikProd == 0)
            {
                errorProvider1.SetError(kwbtn2, "Najpierw wybierz produkty");
                return;
            }
            kwWplacono += 2;
            kwtxtbWplacono.Text = kwWplacono.ToString("F");
            kwRoznica = kwSuma - kwWplacono;
            if (kwRoznica > 0)
            {
                kwtxtbDoZap.Text = kwRoznica.ToString("F");
            }
            else
            {
                kwtxtbDoZap.Text = "0.00";
                kwtxtReszta.Text = (kwRoznica * (-1)).ToString("F");
                kwgbLiczbyDoZap.Enabled = false;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            errorProvider1.Dispose();
            if (kwLicznikProd == 0)
            {
                errorProvider1.SetError(kwbtn1, "Najpierw wybierz produkty");
                return;
            }
            kwWplacono += 1;
            kwtxtbWplacono.Text = kwWplacono.ToString("F");
            kwRoznica = kwSuma - kwWplacono;
            if (kwRoznica > 0)
            {
                kwtxtbDoZap.Text = kwRoznica.ToString("F");
            }
            else
            {
                kwtxtbDoZap.Text = "0.00";
                kwtxtReszta.Text = (kwRoznica * (-1)).ToString("F");
                kwgbLiczbyDoZap.Enabled = false;
            }
        }

        private void kwbtn05_Click(object sender, EventArgs e)
        {
            errorProvider1.Dispose();
            if (kwLicznikProd == 0)
            {
                errorProvider1.SetError(kwbtn05, "Najpierw wybierz produkty");
                return;
            }
            kwWplacono += 0.5F;
            kwtxtbWplacono.Text = kwWplacono.ToString("F");
            kwRoznica = kwSuma - kwWplacono;
            if (kwRoznica > 0)
            {
                kwtxtbDoZap.Text = kwRoznica.ToString("F");
            }
            else
            {
                kwtxtbDoZap.Text = "0.00";
                kwtxtReszta.Text = (kwRoznica * (-1)).ToString("F");
                kwgbLiczbyDoZap.Enabled = false;
            }
        }

        private void kwbtn02_Click(object sender, EventArgs e)
        {
            errorProvider1.Dispose();
            if (kwLicznikProd == 0)
            {
                errorProvider1.SetError(kwbtn02, "Najpierw wybierz produkty");
                return;
            }
            kwWplacono += 0.2F;
            kwtxtbWplacono.Text = kwWplacono.ToString("F");
            kwRoznica = kwSuma - kwWplacono;
            if (kwRoznica > 0)
            {
                kwtxtbDoZap.Text = kwRoznica.ToString("F");
            }
            else
            {
                kwtxtbDoZap.Text = "0.00";
                kwtxtReszta.Text = (kwRoznica * (-1)).ToString("F");
                kwgbLiczbyDoZap.Enabled = false;
            }
        }

        private void kwbtn01_Click(object sender, EventArgs e)
        {
            errorProvider1.Dispose();
            if (kwLicznikProd == 0)
            {
                errorProvider1.SetError(kwbtn01, "Najpierw wybierz produkty");
                return;
            }
            kwWplacono += 0.1F;
            kwtxtbWplacono.Text = kwWplacono.ToString("F");
            kwRoznica = kwSuma - kwWplacono;
            if (kwRoznica > 0)
            {
                kwtxtbDoZap.Text = kwRoznica.ToString("F");
            }
            else
            {
                kwtxtbDoZap.Text = "0.00";
                kwtxtReszta.Text = (kwRoznica * (-1)).ToString("F");
                kwgbLiczbyDoZap.Enabled = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kwcmbsposob.Text == "Płatność gotówką")
            {
                kwgbLiczbyDoZap.Visible = true;
                kwtxtNumerKonta.Visible = false;
                kwlblPodajNumer.Visible = false;
                kwbtnNoweKonto.Visible = false;


            }
            else
            {
                kwgbLiczbyDoZap.Visible = false;
                kwtxtNumerKonta.Visible = true;
                kwlblPodajNumer.Visible = true;
                kwbtnNoweKonto.Visible = true;

            }
        }

        private void kwbtnNoweKonto_Click(object sender, EventArgs e)
        {
            kwgbFormularz.Visible = true;
            kwtxtNazwisko.Visible = true;
            kwlblImie.Text = "Podaj imię";
            kwtxtNazwisko.Text = "";
            kwtxtImie.Text = "";
            kwlblNazwisko.Text = "Podaj nazwisko";
        }

        private void kwbtnPotwierdzKonto_Click(object sender, EventArgs e)
        {
            if (kwx == 0)
            {
                Random kwRandom = new Random();
                kwtxtNazwisko.Visible = false;
                kwlblNazwisko.Text = "Dziękujemy za utworzenie";
                kwlblImie.Text = "Twój numer konta: ";
                kwtxtImie.Text = kwRandom.Next(10000).ToString();
                kwx = 1;
            }
            else if (kwx == 1)
            {
                kwgbFormularz.Visible = false;
                kwx = 0;
            }
        }

        private void kwbtnAnuluj_Click_1(object sender, EventArgs e)
        {
            errorProvider1.Dispose();
            kwdgvResztaA.Visible = false;
            kwdgvProdukty.Visible = true;
            kwtxtbWplacono.Text = "0.00";
            kwtxtReszta.Text = "0.00";
            kwgbLiczbyDoZap.Enabled = true;
            kwSuma = 0;
            kwRoznica = 0;
            kwWplacono = 0;
            kwUsuwanie = false;
            kwLicznikProd = 0;
            for (int kwreset = 0; kwreset < kwMiejsce.Length; kwreset++)
            {
                kwMiejsce[kwreset] = 0;
                kwZakupy[kwreset] = 0;
                
            }
            for (int kwi = 0; kwi < kwPojemnik.Length; kwi++)
            {
                kwPojemnik[kwi].kwLicznosc = 0;
                kwPojemnik[kwi].kwWartosc = 0;
            }


            kwcbUsuwanie.Checked = false;
            kwdgvResztaA.Rows.Clear();
            kwdgvProdukty.Rows.Clear();
            kwtxtbDoZap.Text = "0.00";
            kwtxtbSuma.Text = "0.00";
            kwcbUsuwanie.Enabled = true;
            kwbtnPotwierdz.Enabled = true;
            kwgbProdukty.Enabled = true;
            kwlblDokonane.Visible = false;
            kwgbFormularz.Visible = false;
            kwtxtNumerKonta.Text = "";
            


        }
    }
}
