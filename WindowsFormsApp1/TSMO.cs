using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class TSMO
    {
        int NumChannel, MaxQueue; //кількість пристроїв обслуговування
        List<double> TimeFinServ = new List<double>(); //
        List<int> StateChannel = new List<int>(); //поточний стан пристроїв
        int StateQueue; //поточний стан черги
        double Serv;
        public bool Entry, Exit, block;
        double tmin;
        int minChannel;
        public TSMO(int aNum, int aMax, double aTimeMod, double aServ)
        {
            int i;
            StateQueue = 0; //початковий стан черги
            Entry = false;
            Exit = false;
            block = false;
            MaxQueue = aMax;
            NumChannel = aNum;
            Serv = aServ;
            for (i = 0; i <= NumChannel - 1; i++)
            {
                TimeFinServ.Add(aTimeMod);
                StateChannel.Add(0); //початковий стан пристроїв - вільний
            }
            NextTime(); // формувати найближчу подію в СМО
        }
        public void NextTime()
        {
            int i;
            tmin = TimeFinServ[0];
            minChannel = 1;
            if (NumChannel > 1)
            {
                for (i = 1; i <= NumChannel; i++)
                {
                    if (TimeFinServ[i - 1] < tmin)
                    {
                        tmin = TimeFinServ[i - 1];
                        minChannel = i;
                    }
                }
            }
        }
        public void Seize(double at)
        {
            bool j = true;
            int i;
            if (Entry)
            {
                j = false;
                i = 1;
                while (i <= NumChannel && j == false)
                    if (StateChannel[i - 1] == 0)
                    {
                        j = true;
                        SeizeChannel(i, at);
                        NextTime();
                        Entry = false;
                    }
                    else
                        i++;
            }
            if (!j && MaxQueue > 0)
            {
                StateQueue += 1;
                Entry = false;
                if (StateQueue == MaxQueue)
                    block = true;
            }
        }
        public void Releize(int aChannel, double aTimeMod, double at)
        {
            if (StateQueue > 0) {
                StateQueue = StateQueue - 1;

                if (block)
                    block = false;
                SeizeChannel(aChannel, at);
                NextTime();
            }
            else {
                ReleizeChannel(aChannel, aTimeMod);
                NextTime();
            }
            Exit = true;
        }
        public int GetStateQue()
        {
            return StateQueue;
        }
        Random a = new Random();
        public double GenExp(double parametr) {
            return -parametr * Math.Log(a.NextDouble());
        }
        public void SeizeChannel(int aChannel, double at)
        {
            StateChannel[aChannel - 1] = 1;
            TimeFinServ[aChannel - 1] = at + GenExp(Serv);
        }
        public void ReleizeChannel(int aChannel, double aTimeMod)
        {
            StateChannel[aChannel - 1] = 0;
            TimeFinServ[aChannel - 1] = aTimeMod;
        }
        public int GetStateChannel(int aNum)
        {
            return StateChannel[aNum - 1];
        }
        public double GetAverLoadChannel()
        {
            int sum, i;

            sum = 0;
            for (i = 1; i <= GetNumChannel(); i++)
            {
                sum = sum + StateChannel[i - 1];
            }
            return sum;
        }

        public int GetNumChannel()
        {
            return NumChannel;
        }
        public bool GetBlock()
        {
            return block;
        }

        public void SetExit(bool aEnt)
        {
            Exit = aEnt;
        }
        public double GetMinTime()
        {
            return tmin;
        }
        public int GetMinChannel()
        {
            return minChannel;
        }
        public void SetEntry(bool aEx)
        {
            Entry = aEx;
        }
    }
}
