using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class TSystEntry
    {
        bool Exit, Entry;
        double Interval, TimeExit;//момент часу, в який вимога має вийти з вхідного потоку
        int NumArrival;//загальна кількість вимог, що створена вхідним потоком вимог
        public TSystEntry(double aInterval)
        {
            Entry = true;
            Exit = false;
            Interval = aInterval;
        }
        Random a = new Random();
        public double GenExp(double parametr)
        {
            return -parametr * Math.Log(a.NextDouble());
        }
        public void Arrival(double at)
        {
            TimeExit = at + GenExp(Interval);
            //інтервал часу між надходження вимог заданий за експоненціальним законом розподілу
            Exit = true;
            NumArrival = NumArrival + 1;
        }
        public void EntryInSyst()
        {
            Exit = true;
            Entry = true;
        }
        public double GetMinTime()
        {
            return TimeExit;
        }
        public int GetNumArrival()
        {
            return NumArrival;
        }
        public void SetExit(bool aEnt)
        {
            Exit = aEnt;
        }
        public void SetEntry(bool aEx)
        {
            Entry = aEx;
        }
    }
}
