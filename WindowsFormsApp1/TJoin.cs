using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class TJoin
    {
        private TSMO EntrySmo, ExitSmo;
        bool State;
        int NumUnServ;
        public TJoin(TSMO aSmoEntry, TSMO aSmoExit)
        {
            EntrySmo = aSmoEntry;
            ExitSmo = aSmoExit;
            State = true;
            NumUnServ = 0;
        }
        //у конструкторі об’єкту «маршрут» задаються СМО EntrySmo, з якої вимога виходить,
        // та СМО ExitSmo, до якої вимога надходить
        public void send()
        {
            if (GetState() == true)
            {
                EntrySmo.SetExit(false);
                ExitSmo.SetEntry(true);
            }
            else NumUnServ = NumUnServ + 1;
        }
        public bool GetState()
        {
            if (ExitSmo.GetBlock() == true) return false; else return true;
        }
        public int GetNumUnServ()
        {
            return NumUnServ;
        }

    }
    class TJoinIN
    { //об’єкт «маршрут входу», що передає вимогу від вхідного потоку до СМО
        private TSystEntry Entry;
        TSMO ExitSmo;
        bool State;
        int NumUnServ;
        public TJoinIN(TSystEntry aEntry, TSMO aSmoExit)
        {
            Entry = aEntry;
            ExitSmo = aSmoExit;
            State = true;
            NumUnServ = 0;
        }
        //у конструкторі задається вхідний потік, з якого вимога виходить,
        // та СМО, до якої вимога надходить
        public void send()
        {
            if (GetState() == true)
            {
                Entry.SetExit(false);
                ExitSmo.SetEntry(true);
            }
            else
                NumUnServ = NumUnServ + 1;
        }
        public bool GetState()
        {
            if (ExitSmo.GetBlock() == true) return false; else return true;
        }
        public int GetNumUnServ()
        {
            return NumUnServ;
        }
    }
    class TJoinOUT
    { //об’єкт «маршрут виходу», що передає вимогу із СМО на вихід із мережі МО
        private
        TSMO EntrySmo;
        bool State;
        int NumService;
        public TJoinOUT(TSMO aSmoEntry)
        {
            EntrySmo = aSmoEntry;
            NumService = 0;
            State = true;
        }
        //у конструкторі задається СМО, з якої вимога виходить
        public void send()
        {
            if (State == true)
            {
                EntrySmo.SetExit(false);
                NumService = NumService + 1;
            }
        }
        public int GetNumUnServ()
        {
            return NumService;
        }
    }
}
