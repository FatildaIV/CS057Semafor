using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS056Semafor
{
    public partial class SemaforForm : Form
    {
        public SemaforForm()
        {
            InitializeComponent();
        }
       

        class Semafor
        {
            public enum Stav
            {
                Vypnuto,
                Zapnuto
            }

            public enum Svetla
            {
                Stop,
                PripravitVolno,
                Volno,
                PripravitStop,
                OranzoveSviti,
                Zhasnute
            }

            Stav semafor = Stav.Vypnuto;
            Stav rizeni = Stav.Vypnuto;
            Svetla svetla = Svetla.Zhasnute;
            DateTime casPrechodu;                                       //cas prechodu do aktualniho stavu rizeni


            TimeSpan intervalStop = new TimeSpan(0, 0, 5);
            TimeSpan intervalVolno = new TimeSpan(0, 0, 10);
            TimeSpan intervalPripravitVolno = new TimeSpan(0, 0, 2);
            TimeSpan intervalPripravitStop = new TimeSpan(0, 0, 2);
            TimeSpan intervalBlikani = new TimeSpan(0, 0, 1);

            PictureBox svetloCervene;
            PictureBox svetloOranzove;
            PictureBox svetloZelene;

            public Semafor(PictureBox svetloCervene, PictureBox svetloOranzove, PictureBox svetloZelene)
            {
                this.svetloCervene = svetloCervene;
                this.svetloOranzove = svetloOranzove;
                this.svetloZelene = svetloZelene;

                casPrechodu = DateTime.Now;

            }

            //nastavi stav rizeni podle parametru
            private void ZmenitStav(Svetla novyStav)
            {
                this.svetla = novyStav;
                casPrechodu = DateTime.Now;                 //zapamatovat cas prechodu
                Vykreslit();                                //zobrazit svetla podle stavu
            }

            public void Zapnout()
            {
                semafor = Stav.Zapnuto;
                ZmenitStav(Svetla.OranzoveSviti);
                //if (stavPoZapnuti == RizeniStav.Volno)
                //{
                //    ZmenitStav(RizeniStav.PripravitVolno);
                //}
                //else
                //{
                //    ZmenitStav(RizeniStav.PripravitStop);
                //}

            }
            public void Vypnout()
            {
                semafor = Stav.Vypnuto;
                ZmenitStav(Svetla.Zhasnute);
           
            }

            public void RizeniZapnout()
            {
                rizeni = Stav.Zapnuto;
                ZmenitStav(Svetla.Stop);
            }

            public void RizeniVypnout()
            {
                rizeni = Stav.Vypnuto;
                ZmenitStav(Svetla.OranzoveSviti);
            }

            //vykresli aktualni stav semaforu (rozsviceni svetel)
            public void Aktualizovat()
            {
                TimeSpan casOdPrepnuti = DateTime.Now - casPrechodu;

                //stav se zmeni jen kdyz je potreba - vykresli se jen, kdyz je potreba
                bool zmenitStav = (semafor == Stav.Zapnuto);

                if (rizeni == Stav.Zapnuto)
                {
                    TimeSpan interval;
                    switch (svetla)
                    {
                        case Svetla.Stop: interval = intervalStop; break;
                        case Svetla.PripravitVolno: interval = intervalPripravitVolno; break;
                        case Svetla.Volno: interval = intervalVolno; break;
                        case Svetla.PripravitStop: interval = intervalPripravitStop; break;
                        default: throw new Exception();
                    }

                    zmenitStav = zmenitStav && (casOdPrepnuti > interval);
                    if (zmenitStav && svetla == Svetla.Stop)
                    {
                        ZmenitStav(Svetla.PripravitVolno);
                    }
                    else if (zmenitStav && svetla == Svetla.PripravitVolno)
                    {
                        ZmenitStav(Svetla.Volno);
                    }
                    else if (zmenitStav && svetla == Svetla.Volno)
                    {
                        ZmenitStav(Svetla.PripravitStop);
                    }
                    else if (zmenitStav && svetla == Svetla.PripravitStop)
                    {
                        ZmenitStav(Svetla.Stop);
                    }
                }
                else
                {
                    zmenitStav = zmenitStav && (casOdPrepnuti > intervalBlikani);
                    if (zmenitStav && svetla == Svetla.Zhasnute)
                    {
                        ZmenitStav(Svetla.OranzoveSviti);
                    }
                    else if (zmenitStav)
                    {
                        ZmenitStav(Svetla.Zhasnute);
                    }

                }
            }
         
            public void Vykreslit()
            {
                ZhasnoutSvetla();

                switch (svetla)
                {
                    case Svetla.Zhasnute:
                        
                        break;

                    case Svetla.OranzoveSviti:
                        svetloOranzove.Image = Properties.Resources.svetloZluta;
                        break;

                    case Svetla.Stop:
                        svetloCervene.Image = Properties.Resources.svetloCervena;
                        break;

                    case Svetla.PripravitVolno:
                        svetloCervene.Image = Properties.Resources.svetloCervena;
                        svetloOranzove.Image = Properties.Resources.svetloZluta;
                        break;

                    case Svetla.Volno:
                        svetloZelene.Image = Properties.Resources.svetloZelena;
                        break;

                    case Svetla.PripravitStop:
                        svetloOranzove.Image = Properties.Resources.svetloZluta;
                        break;
                }

            }
            private void ZhasnoutSvetla()
            {
                svetloCervene.Image = Properties.Resources.svetloVypnute;
                svetloOranzove.Image = Properties.Resources.svetloVypnute;
                svetloZelene.Image = Properties.Resources.svetloVypnute;

            }
        }
     


        private void semaforTimer_Tick(object sender, EventArgs e)
        {
            this.semafor.Aktualizovat();
        }
        Semafor semafor; 

        private void CS056Semafor_Load(object sender, EventArgs e)
        {
            this.semafor = new Semafor(pictureBox1, pictureBox2, pictureBox4);
        }

        private void zapnoutButton_Click(object sender, EventArgs e)
        {
            this.semafor.Zapnout();
        }

        private void vypnoutButton_Click(object sender, EventArgs e)
        {
            this.semafor.Vypnout();
        }

        private void rizeniVypnoutButton_Click(object sender, EventArgs e)
        {
            this.semafor.RizeniVypnout();
        }

        private void rizeniZapnoutButton_Click(object sender, EventArgs e)
        {
            this.semafor.RizeniZapnout();
        }
    }
}