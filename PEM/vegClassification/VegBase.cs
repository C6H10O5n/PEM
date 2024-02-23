using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PEM_VegClassification
{

    public class VegBase
    {
        //Constructor
        public VegBase(int iLndclass, int iFornon, string iSpecies, int iWetClass = 0, bool iConvertXS2RSBS = false, int iSiteClass = 5, int iPercentBase = 10, int iSppPctDigits = 2)
        {
            //Set Base-Level Information
            fornon = iFornon;
            species = iSpecies.TrimEnd();
            lndclass = iLndclass;
            wetclass = iWetClass;

            // Parse input species string
            try
            {
                spp = ParseSpeciesString(species,iConvertXS2RSBS,iSiteClass, iPercentBase, iSppPctDigits);
                sppCodesOrdered = spp.OrderByDescending(x => x.Value).Select(x => x.Key).ToList();
                isInitalized = true;
            }
            catch (Exception e)
            {
                isInitalized = false;
                inilizationErr = "FcInputErr";
            }
        }


        //Main Input Data Fields
        public string species { get; }
        protected int fornon { get; set; }
        protected int lndclass { get; set; }
        protected int wetclass { get; set; } //*Forest Interp: Wet forest stand indicator - a value of 1 indicates wet land/poor soil drainage beneath at
                                             // least part of a forest stand for a significant portion of the growing season
        protected Dictionary<string, float> spp { get; set; }
        protected List<string> sppCodesOrdered { get; set; }
        protected bool isInitalized { get; set; }
        protected string inilizationErr { get; set; }

        
        //Public Static Methods
        public static Dictionary<string, float> ParseSpeciesString(string species, bool iConvertXS2RSBS = false, int iSiteClass = 5, int iPercentBase = 10, int iSppPctDigits = 2)
        {
            Dictionary<string, float> spp = new Dictionary<string, float>();
            try
            {


                // Set Base Percent Adjustment
                float pctadj = (float)(iPercentBase / 10d);

                int iSppChars = 2;
                int singleSppLen = iSppChars + iSppPctDigits;
                
                for (int idx = 1; idx <= (int)(species.Length / singleSppLen); idx++)
                {
                    string sp = species.Substring((idx - 1) * singleSppLen, iSppChars).Trim();
                    int pc = int.Parse(species.Substring((idx - 1) * singleSppLen + iSppChars, iSppPctDigits));

                    // update outdated species codes to current standard:
                    if (VegBaseLib.sppOldCodes.ContainsKey(sp))
                        sp = VegBaseLib.sppOldCodes[sp];


                    // Added on Nov 22, 2012 to handel converting XS ... to be implemented need to add site/lc as input to routine
                    if (iConvertXS2RSBS && sp == "XS")
                        sp = iSiteClass <= 3 ? "BS" : "RS";


                    //Create Main Species Dictionary 
                    if (spp.Keys.Contains(sp))
                        spp[sp] += pc / (float)pctadj;
                    else
                        spp.Add(sp, pc / (float)pctadj);
                }
                return spp;
            }
            catch
            {
                throw new Exception($"Species Parser Failed on inputs: species=[{species}]"); ;
            }

        }
        public static bool isNonForestedDef(int fornon, string species)
        {
            return (fornon > 62
                || new int[] { 3, 11, 16, 33, 38, 39 }.Contains(fornon)
                || (new int[] { 5 }.Contains(fornon) && species == ""));
        }
        public static bool iForestedDisturbedDef(int fornon)
        {
            return new int[] {2,6,7,8,13,14,15}.Contains(fornon);
        }
        public static string ConvertFECBasalAreaSummartToSpeciesLabel(string iBaSum)
        {
            string spp = "";
            string[] sl;
            string s, p;

            foreach ( string sb in iBaSum.Split(','))
            {
                sl = sb.Trim().Split(' ');
                if (sl.Length == 2)
                {
                    s = sl[0];
                    p = sl[1];
                    if (VegBaseLib.fecBaSppLut.ContainsKey(s))
                    {
                        s = VegBaseLib.fecBaSppLut[s];
                    }
                    spp += s.ToUpper() + p.Trim().PadLeft(3,'0');
                }
            }

            return spp;
        }


        //Private Protected Properties
        protected bool isNonForested
        {
            get => isNonForestedDef(fornon, species);
        }
        protected bool isForestedDisturbed
        {
            get => iForestedDisturbedDef(fornon);
        }
        protected bool isForested { get => !isNonForested; }
        protected bool isUnclRegen { get => species == "" || uc >= 5; }

        #region Spp Group Precents
        protected virtual float hw   { get => spp.Where(s => VegBaseLib.sppGrps["hw"  ].Contains(s.Key)).Sum(p => p.Value); } //hardwood
        protected virtual float th   { get => spp.Where(s => VegBaseLib.sppGrps["th"  ].Contains(s.Key)).Sum(p => p.Value); } //tolerant hardwood
        protected virtual float ih   { get => spp.Where(s => VegBaseLib.sppGrps["ih"  ].Contains(s.Key)).Sum(p => p.Value); } //intolerant hardwood
        protected virtual float robc { get => spp.Where(s => VegBaseLib.sppGrps["robc"].Contains(s.Key)).Sum(p => p.Value); } //red oak + black cherry
        protected virtual float rm   { get => spp.Where(s => VegBaseLib.sppGrps["rm"  ].Contains(s.Key)).Sum(p => p.Value); }
        protected virtual float ta   { get => spp.Where(s => VegBaseLib.sppGrps["ta"  ].Contains(s.Key)).Sum(p => p.Value); }
        protected virtual float wi   { get => spp.Where(s => VegBaseLib.sppGrps["wi"  ].Contains(s.Key)).Sum(p => p.Value); }
        protected virtual float we   { get => spp.Where(s => VegBaseLib.sppGrps["we"  ].Contains(s.Key)).Sum(p => p.Value); }
        protected virtual float wb   { get => spp.Where(s => VegBaseLib.sppGrps["wb"  ].Contains(s.Key)).Sum(p => p.Value); }
        protected virtual float gb   { get => spp.Where(s => VegBaseLib.sppGrps["gb"  ].Contains(s.Key)).Sum(p => p.Value); }
        protected virtual float bc   { get => spp.Where(s => VegBaseLib.sppGrps["bc"  ].Contains(s.Key)).Sum(p => p.Value); }
        protected virtual float bp   { get => spp.Where(s => VegBaseLib.sppGrps["bp"  ].Contains(s.Key)).Sum(p => p.Value); }
        protected virtual float ro   { get => spp.Where(s => VegBaseLib.sppGrps["ro"  ].Contains(s.Key)).Sum(p => p.Value); }
        protected virtual float sm   { get => spp.Where(s => VegBaseLib.sppGrps["sm"  ].Contains(s.Key)).Sum(p => p.Value); }
        protected virtual float be   { get => spp.Where(s => VegBaseLib.sppGrps["be"  ].Contains(s.Key)).Sum(p => p.Value); }
        protected virtual float iw   { get => spp.Where(s => VegBaseLib.sppGrps["iw"  ].Contains(s.Key)).Sum(p => p.Value); }
        protected virtual float yb   { get => spp.Where(s => VegBaseLib.sppGrps["yb"  ].Contains(s.Key)).Sum(p => p.Value); }
        protected virtual float wa   { get => spp.Where(s => VegBaseLib.sppGrps["wa"  ].Contains(s.Key)).Sum(p => p.Value); }
        protected virtual float sw   { get => spp.Where(s => VegBaseLib.sppGrps["sw"  ].Contains(s.Key)).Sum(p => p.Value); } //softwood
        protected virtual float sp   { get => spp.Where(s => VegBaseLib.sppGrps["sp"  ].Contains(s.Key)).Sum(p => p.Value); } //spruce
        protected virtual float sx   { get => spp.Where(s => VegBaseLib.sppGrps["sx"  ].Contains(s.Key)).Sum(p => p.Value); } //rs,bs,xs
        protected virtual float bs   { get => spp.Where(s => VegBaseLib.sppGrps["bs"  ].Contains(s.Key)).Sum(p => p.Value); }
        protected virtual float ws   { get => spp.Where(s => VegBaseLib.sppGrps["ws"  ].Contains(s.Key)).Sum(p => p.Value); }
        protected virtual float rs   { get => spp.Where(s => VegBaseLib.sppGrps["rs"  ].Contains(s.Key)).Sum(p => p.Value); }
        protected virtual float xs   { get => spp.Where(s => VegBaseLib.sppGrps["xs"  ].Contains(s.Key)).Sum(p => p.Value); }
        protected virtual float ns   { get => spp.Where(s => VegBaseLib.sppGrps["ns"  ].Contains(s.Key)).Sum(p => p.Value); }
        protected virtual float bf   { get => spp.Where(s => VegBaseLib.sppGrps["bf"  ].Contains(s.Key)).Sum(p => p.Value); }
        protected virtual float pi   { get => spp.Where(s => VegBaseLib.sppGrps["pi"  ].Contains(s.Key)).Sum(p => p.Value); } //pine
        protected virtual float wp   { get => spp.Where(s => VegBaseLib.sppGrps["wp"  ].Contains(s.Key)).Sum(p => p.Value); }
        protected virtual float jp   { get => spp.Where(s => VegBaseLib.sppGrps["jp"  ].Contains(s.Key)).Sum(p => p.Value); }
        protected virtual float rp   { get => spp.Where(s => VegBaseLib.sppGrps["rp"  ].Contains(s.Key)).Sum(p => p.Value); }
        protected virtual float tl   { get => spp.Where(s => VegBaseLib.sppGrps["tl"  ].Contains(s.Key)).Sum(p => p.Value); }
        protected virtual float xl   { get => spp.Where(s => VegBaseLib.sppGrps["xl"  ].Contains(s.Key)).Sum(p => p.Value); }
        protected virtual float eh   { get => spp.Where(s => VegBaseLib.sppGrps["eh"  ].Contains(s.Key)).Sum(p => p.Value); }
        protected virtual float ec   { get => spp.Where(s => VegBaseLib.sppGrps["ec"  ].Contains(s.Key)).Sum(p => p.Value); }
        protected virtual float uc   { get => spp.Where(s => VegBaseLib.sppGrps["uc"  ].Contains(s.Key)).Sum(p => p.Value); } //unclassified
        protected virtual float ex   { get => spp.Where(s => VegBaseLib.sppGrps["ex"  ].Contains(s.Key)).Sum(p => p.Value); } //exotic
        protected virtual float swn  { get => spp.Where(s => VegBaseLib.sppGrps["swn" ].Contains(s.Key)).Sum(p => p.Value); } //softwood-native
        protected virtual float pin  { get => spp.Where(s => VegBaseLib.sppGrps["pin" ].Contains(s.Key)).Sum(p => p.Value); } //pine-native
        protected virtual float nc   { get => spp.Where(s => VegBaseLib.sppGrps["nc"  ].Contains(s.Key)).Sum(p => p.Value); } //non-coastal
        protected virtual float hl   { get => spp.Where(s => VegBaseLib.sppGrps["hl"  ].Contains(s.Key)).Sum(p => p.Value); } //Highland spp
        protected virtual float cs   { get => spp.Where(s => VegBaseLib.sppGrps["cs"  ].Contains(s.Key)).Sum(p => p.Value); } //climax species
        protected virtual float uih  { get => spp.Where(s => VegBaseLib.sppGrps["uih"].Contains(s.Key)).Sum(p => p.Value);  } //old aggregare IH codes
        protected virtual float uth  { get => spp.Where(s => VegBaseLib.sppGrps["uth"].Contains(s.Key)).Sum(p => p.Value);  } //old aggregare TH codes
        #endregion

        //Base Covertype Defintion
        protected virtual bool isHardwood { get => sw<=2.5?true:false; }
        protected virtual bool isMixedwood { get => sw > 2.5 && sw < 7.5 ? true : false; }
        protected virtual bool isSoftwood { get => sw >= 7.5 ? true : false; }


        //Public Properties
        public string sp1 { get => sppCodesOrdered.Count < 1 ? "" : sppCodesOrdered[0]; }
        public string sp2 { get => sppCodesOrdered.Count < 2 ? "" : sppCodesOrdered[1]; }
        public string sp3 { get => sppCodesOrdered.Count < 3 ? "" : sppCodesOrdered[2]; }
        public string sp4 { get => sppCodesOrdered.Count < 4 ? "" : sppCodesOrdered[3]; }
        public float getSppPercents(List<string> ls) { return spp.Where(s => ls.Contains(s.Key)).Sum(p => p.Value); }

      

        //ELA Indicators
        public int GetSeralScore(int iEcodistrict)
        {
            if (!isInitalized) return -9;

            if (isForested)
            {
                int ss = 0;
                foreach (string s in spp.Keys)
                {
                    if (spp[s] > 0)
                    {
                        ss += (int)(VegBaseLib.sppEcodSerVals[s].get_GetSeralVal(iEcodistrict) * spp[s]);
                    }
                }
                if (ss > 10)
                    return ss;
                else
                    return 10; //default to lowest score
            }
            else
            {
                return 0;
            }
        }
        public string GetSeralClass(int iEcodistrict)
        {
            if (!isInitalized) return inilizationErr;

            if (!isForested) return "NonFor";
            else if (isUnclRegen) return "UnclRegen";
            else
            {
                if (VegBaseLib.sppEcodSerVals[sp1].get_HasEcodistrictKey(iEcodistrict))
                {
                    int ss = GetSeralScore(iEcodistrict);
                    if (ss < 10) return "ErrLow";
                    else if (ss <= 23) return "Early";
                    else if (ss <= 37) return "Mid";
                    else if (ss <= 50) return "Late";
                    else return "ErrHigh";
                }
                else
                {
                    return "UnknownEcoDistrict";
                }
            }
        }
        public string GetDevClass(int iHeight, string iAllHt)
        {
            if (!isInitalized) return inilizationErr;

            if (isNonForested) return "NonFor";
            else if (iAllHt == "*") return "Multi";
            else
            {
                if (iHeight <= 6) return "Estab";
                else if (iHeight <= 11) return "Young";
                else if (iHeight <= 37) return "Mature";
                else return "ErrHigh";
            }
        }
    }
}
