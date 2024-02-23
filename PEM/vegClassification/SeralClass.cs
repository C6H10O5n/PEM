using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PEM_VegClassification
{
    public class ecodSeralValue
    {
        private Dictionary<int, int> pSerVals;

        public ecodSeralValue(string iSerValEcodList)
        {
            pSerVals = new Dictionary<int, int>();
            var vals = iSerValEcodList.Split(',');
            for (int i = 0; i < VegBaseLib.stdEcodList.Count; i++)
                pSerVals.Add(VegBaseLib.stdEcodList[i], int.Parse(vals[i]));
        }

        public int get_GetSeralVal(int iEcoDistrict) { return pSerVals[iEcoDistrict]; }

        public bool get_HasEcodistrictKey(int iEcoDistrict) { return pSerVals.ContainsKey(iEcoDistrict); }
    }
    public class sppSeral
    {
        public sppSeral(string iSppCode, string iSppName, string iSerValEcodList)
        {
            pSppCode = iSppCode;
            pSppName = iSppName;
            pEcodSerVals = new ecodSeralValue(iSerValEcodList);
        }

        private string pSppCode;
        private string pSppName;
        private ecodSeralValue pEcodSerVals;

        public string sppCode { get => pSppCode; }
        public string sppName { get => pSppName; }

        public int get_GetSeralVal(int iEcoDistrict) { return pEcodSerVals.get_GetSeralVal(iEcoDistrict); }
        public bool get_HasEcodistrictKey(int iEcoDistrict) { return pEcodSerVals.get_HasEcodistrictKey(iEcoDistrict); }
    }

}
