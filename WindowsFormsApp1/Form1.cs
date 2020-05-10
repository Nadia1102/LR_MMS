using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private double TimeMod, t;
        public Form1()
        {
            InitializeComponent();
        }
        public double GetTimeMod()
        {
            return TimeMod;
        }
        public double GetTimeNow()
        {
            return t;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TimeMod = Convert.ToDouble(textBox1.Text);
            t = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<TSMO> smo = new List<TSMO>();
            int NumSMO;
            double tmin;
            TSystEntry systEntry;
            List<TJoin> join = new List<TJoin>();
            TJoinIN joinIn;
            TJoinOUT joinOut;
            double prob;
            int _event, i;
            List<double> AverQue = new List<double>();
            List<double> AverDevices = new List<double>();

            NumSMO = 3; //кількість СМО
            prob = 0;
            for (i = 0; i <= NumSMO - 1; i++)
            {
                AverQue.Add(0);
                AverDevices.Add(0);
            }
            //структура мережі масового обслуговування
            systEntry = new TSystEntry(1.0/0.2); //створення вхідного потоку вимог
            smo.Add(new TSMO(1, 1, GetTimeMod(), 3)); //створення СМО1
            joinIn = new TJoinIN(systEntry, smo[0]);// створення маршруту до СМО1
            smo.Add(new TSMO(1, 5, GetTimeMod(), 1.0/0.25)); //створення СМО2
            join.Add(new TJoin(smo[0], smo[1])); //створення маршруту від СМО1 до СМО2
            smo.Add(new TSMO(1, 1, GetTimeMod(), 5)); //створення СМО3
            join.Add(new TJoin(smo[1], smo[2])); //створення маршруту від СМО2 до СМО3
            joinOut = new TJoinOUT(smo[NumSMO - 1]); //створення маршруту на вихідсистеми
            t = 0; // початкове значення модельного часу
            Random a = new Random();
            while (t < GetTimeMod())
            {
                tmin = systEntry.GetMinTime();
                _event = 0;
                for (i = 0; i <= NumSMO - 1; i++)
                    if (smo[i].GetMinTime() < tmin)
                    {
                        tmin = smo[i].GetMinTime();
                        _event = i + 1;
                    }
                for (i = 0; i <= NumSMO - 1; i++)
                {
                    AverQue[i] = AverQue[i] + ((tmin - t) / GetTimeMod()) * smo[i].GetStateQue();
                    AverDevices[i] = AverDevices[i] + ((tmin) / GetTimeMod()) * smo[i].GetAverLoadChannel();
                }
                t = tmin; //просування часу в момент найближчої події
                switch (_event)
                {
                    case 0:
                        {
                            systEntry.Arrival(GetTimeNow());
                            joinIn.send();
                            smo[0].Seize(GetTimeNow());
                        }
                        break;
                    case 1:
                        {
                            smo[0].Releize(smo[0].GetMinChannel(), GetTimeMod(), GetTimeNow());
                            join[0].send();
                            smo[1].Seize(GetTimeNow());
                        }
                        break;
                    case 2:
                        {
                            smo[1].Releize(smo[1].GetMinChannel(), GetTimeMod(), GetTimeNow());
                            join[1].send();
                            smo[2].Seize(GetTimeNow());
                        }
                        break;
                    case 3:
                        {
                            smo[2].Releize(smo[2].GetMinChannel(), GetTimeMod(), GetTimeNow());
                            joinOut.send();
                        }
                        break;
                }
            }
            prob = joinIn.GetNumUnServ();
            for (i = 0; i <= NumSMO - 2; i++)
                prob = prob + join[i].GetNumUnServ();
            prob = prob / systEntry.GetNumArrival();

            textBox2.Text = GetTimeNow().ToString();
            textBox3.Text = prob.ToString();
            textBox4.Text = joinOut.GetNumUnServ().ToString();
            textBox5.Text = systEntry.GetNumArrival().ToString();
            textBox6.Text = AverQue[0].ToString();
            textBox7.Text = AverDevices[0].ToString();
            textBox8.Text = joinIn.GetNumUnServ().ToString();
            textBox9.Text = AverQue[1].ToString();
            textBox10.Text = AverDevices[1].ToString();
            textBox11.Text = join[0].GetNumUnServ().ToString();
            textBox12.Text = AverQue[2].ToString();
            textBox13.Text = AverDevices[2].ToString();
            textBox14.Text = join[1].GetNumUnServ().ToString();
        }
    }
}
