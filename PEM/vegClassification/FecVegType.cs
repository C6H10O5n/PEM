using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PEM_VegClassification
{
    /// <summary>
    /// FEC VegType Classification
    /// Conversion of the [VegMap 1.33.0_final.py] script
    /// origionall coded by Courtney; based on origional 2014
    /// logic Eugene and I worked on
    /// </summary>
    public class FecVegType : VegBase
    {
        //Constructor
        public FecVegType(int iLndclass, int iFornon, string iSpecies, int iWetClass, string iWCType, int iPltFlag, int iEcodistrict, string iEcosection, string iSoilType, string iSoilSeries, string iSoilDrainClass, int iKarstCode, string iWetlandClass, int iCoastalFlag=-1, bool iConvertXS2RSBS = false, int iSiteClass = 5, int iPercentBase = 10, int iSppPctDigits = 2)
            : base(iLndclass, iFornon, iSpecies, iWetClass, iConvertXS2RSBS, iSiteClass, iPercentBase, iSppPctDigits)
        {
            pltFlag = iPltFlag;
            ecodistrict = iEcodistrict;
            ecosection = iEcosection;
            soilSeries = iSoilSeries.Trim().ToUpper();
            soilType = iSoilType.Trim().ToUpper();
            soilDrainClass = iSoilDrainClass.Trim().ToUpper();
            karstCode = iKarstCode;
            wetlandClass = iWetlandClass.Trim().ToUpper();
            wetClassInvType = iWCType.Trim().ToUpper();
            coastalFlag = iCoastalFlag;

        }

        //Private Properties for Required Inputs
        protected int pltFlag { get; }
        protected int ecodistrict { get; } //from elc layer
        protected string ecosection { get; } //from elc layer
        protected string soilSeries { get; } // from cansis soil series maps
        protected string soilType { get; } // from fec soiltype raster [stgroup]
        protected string soilDrainClass { get; } //from fec soiltype raster [stdrain]: IMPERFECT, POOR, WELL: grouping of soiltypes [stgroup]
        protected int karstCode { get; }  //from seperate GIS Raster: as provided by Eugene based on 2021 lidar assessment??
        protected string wetlandClass { get; }  //from seperate GIS Raster as provided by Wildlife Division??
        protected string wetClassInvType { get; }  //[wc_type] from forest interp data
        protected int coastalFlag { get; } //form GIS Proxmitity


        //Private Properties
        protected string vegTypeBaseRule { get; set; }
        protected string vegTypeFinalRule { get; set; }
        protected string vegRuleTrace { get; set; } = "";
        protected string vegRuleTraceInsert(string vt) { return (vegRuleTrace=="" || !vt.Contains("(")) ? vt : vt.Replace("(", "("+vegRuleTrace); }
        protected string vegTypeBaseWithRuleTrace { get=> vegRuleTraceInsert(vegTypeBaseRule); }
        protected string vegTypeFinalWithRuleTrace { get => vegRuleTraceInsert(vegTypeFinalRule); }

        //Public Properties
        public string GetVegTypeBaseWithRule { get { if (vegTypeBaseRule is null) vegTypeBaseRule = getVegTypeBase(); return vegTypeBaseWithRuleTrace; } }
        public string GetVegTypeWithRule { get { if (vegTypeFinalRule is null) vegTypeFinalRule = getVegType(); return vegTypeFinalWithRuleTrace; } }
        public string GetVegType { 
            get 
            {
                if (vegTypeFinalRule is null)
                {
                    vegTypeFinalRule = getVegType();
                }
                
                //Split of the rule part of string: "VT(rule)" => "VT"
                if (vegTypeFinalRule.Contains("("))
                {
                    return vegTypeFinalWithRuleTrace.Split('(')[0];
                }
                else
                {
                    return vegTypeFinalWithRuleTrace;
                }
                 
            }
        }
        public string GetForestGroup 
        {
            get
            {

                if (getVegType() == "WCU" || getVegType() == "WDU")
                {
                    return getVegType().Substring(0, 2);
                }
                else if (lndclass == 0 || isNonForested || isForestedDisturbed)
                {
                    return "xNF";
                }
                else if (isPlantation)
                {
                    return "xPL";
                }
                else if (isUnclRegen)
                {
                    return "xRF";
                }
                else
                {
                    if (vegTypeFinalRule is null) vegTypeFinalRule = getVegType();
                    return vegTypeFinalRule.Substring(0, 2);
                }
            }
        }


        protected string ecosDrng { get => ecosection.Substring(0, 1); } //Ecosection Soil Drainage code
        protected string ecosText { get => ecosection.Substring(1, 1); } //Ecosection Soil Testure code
        protected string ecosTopo { get => ecosection.Substring(2, 2); } //Ecosection Topography code

        protected bool isOldFieldEcod { get => FecVegTypeLib.ecodists_for_OldFields.Contains(ecodistrict); }

        protected bool isForestedWetlandSoilType { get => FecVegTypeLib.SoilTypes_wetland.Contains(soilType); }
        protected bool isWD4Ecod { get => FecVegTypeLib.ecodists_WD4.Contains(ecodistrict); }
        protected bool isForestedFloodplainSoilSeries { get => FecVegTypeLib.SoilSeries_floodplain.Contains(soilSeries); }
        protected bool isHighlandEcod { get => FecVegTypeLib.ecodists_for_Highland.Contains(ecodistrict); }
        protected bool isCoastalEcod { get => FecVegTypeLib.ecodists_for_Coastal.Contains(ecodistrict); }
        protected bool isCoastalOverrideEcod { get => FecVegTypeLib.ecodists_for_CoastalOverride.Contains(ecodistrict); }


        protected bool isCoastal
        {
            get
            {
                if (coastalFlag==1)
                {
                    if (GetVegTypeBaseWithRule.StartsWith("WD")) return false;
                    else if (rs >= 4 && (ecodistrict == 840 || ecodistrict == 910)) return true;
                    else if (wp >= 3 || jp >= 5) return false;
                    else if (nc > 0) return false;
                    else return true;
                }
                else
                {
                    return false;
                }
            }
        }
        protected bool isKarst 
        {
            get
            {
                if (karstCode == 1)
                    return true;
                else
                    return false;
            }
        }
        protected bool isPlantation
        {
            get
            {
                if (fornon == 20)
                    return true;
                else if (pltFlag == 1)
                    return true;
                else if (ex >= 3)
                    return true;
                else
                    return false;
            }
        }
        protected bool isHighland
        {
            get
            {
                if (hl >= 8  && isHighlandEcod && rs+wp+eh+be+wa==0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        protected bool isFloodPlain
        {
            get
            {
                if (isForestedFloodplainSoilSeries)
                {
                    if (bs >= 4) return false;
                    else if (eh >= 3) return false;
                    else if (jp >= 4) return false;
                    else if (rp >= 4) return false;
                    else if (rs >= 4) return false;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        protected bool isForestedWetland
        {
            get
            {
                if (isForestedWetlandSoilType)
                {
                    if (wi >= 4) return false;
                    else if (sp1 == "GB") return false;
                    else if (sp1 == "RO" || ro >= 2) return false;
                    else if (sp1 == "WE" || we >= 3) return false;
                    else if (sp1 == "BC" || bc >= 3) return false;
                    else if (sm+be > 0) return false;
                    else if (wb > 1) return false;
                    else if (wp > 1) return false;
                    else if (wb > 0 && wb + rm >= 5) return false;
                    else if (wb > 0 && wb + rm + yb >= 5 && yb >= 3) return false;
                    else if (ws + tl >= 7 && tl > 3 && ws > 0) return false;
                    else if (wp + rp >= 5) return false;

                    //stanley area review:
                    //else if(ecodistrict==630 && soilType== "7 [14]" && [height, cc, site, species]) return false;


                    else return true;
                }
                else
                {
                    if(isHighland && tl >= 3) return true;

                    else return false;
                }
            }
        }

        protected bool isForestGroup_WF { get => "WD,WM,WC".Contains(GetVegTypeBaseWithRule.Substring(0, 2)); }
        protected bool notForestGroup_WF { get => !isForestGroup_WF; }



        //Main Classification Logic
        protected string getVegType()
        {
            
            //initalize base vegtype
            if (vegTypeBaseRule is null)
                vegTypeBaseRule = getVegTypeBase();
            
            
            //Main Classification Logic
            if (lndclass == 0)
            {
                return "OCEAN";
            }
            else if (isNonForested)
            {
                return getNonForClass();
            }
            else if (isForestedDisturbed)
            {
                return getNonForClass();
            }
            else if (isPlantation)
            {
                return getVT_Plantation();
            }
            else if (isUnclRegen)
            {
                return getUnclRegenClass();
            }
            else if (ec >= 1)
            {
                return getVT_Cedar();
            }
            else if (isKarst)
            {
                return getVT_Karst();
            }
            else if (isCoastal && notForestGroup_WF)
            {
                return getVT_Coastal();
            }
            else
            {
                return vegTypeBaseRule;
            }
        }


        //Base VT Classification Logic
        protected string getVegTypeBase(bool pWF = true, bool pFP = true, bool pHL = true)
        {

            if (isForestedWetland && pWF)
            {
                return getVT_ForestedWetland();
            }
            else if (isFloodPlain && pFP)
            {
                return getVT_ForestedFloodplain();
            }
            else if (isHighland && pHL)
            {
                return getVT_Highlands();
            }
            else
            {
                if (isHardwood) //Hardwood Dominant
                {
                    return getVT_hw();
                }
                else if (isMixedwood) //Mixedwood
                {
                    return getVt_mw();
                }
                else if (isSoftwood)//Softwood Dominant
                {
                    return getVt_sw();
                }
                else
                {
                    return "vtErr:Swpct=" + sw.ToString();
                }
            }
        }


        //Non-Forest
        protected string getNonForClass()
        {
            if (FecVegTypeLib.WetLandClassDict.ContainsKey(wetlandClass) && FecVegTypeLib.WetLandClassDict[wetlandClass]!="UWL")
                return FecVegTypeLib.WetLandClassDict[wetlandClass];
            else if (FecVegTypeLib.WetClassInvTypesDict.ContainsKey(wetClassInvType) && FecVegTypeLib.WetClassInvTypesDict[wetClassInvType] != "UWL")
                return FecVegTypeLib.WetClassInvTypesDict[wetClassInvType];
            else
            {
                if (FecVegTypeLib.FornonDict.ContainsKey(fornon)) 
                {
                    if (isForestedWetlandSoilType && FecVegTypeLib.SoilTypes_wetland_Fornon.Contains(fornon)) return "UWL";
                    else if ((fornon == 84 || fornon == 85) && soilDrainClass == "POOR") return "UWL";
                    else return FecVegTypeLib.FornonDict[fornon].code;
                }
                else 
                {
                    return "?fornon:" + fornon.ToString();
                }
            }
        }


        //Unclassified Regen
        protected string getUnclRegenClass()
        {
            if (isPlantation) return "RF Plt";
            else return "RF";
        }

        //Unit Specific Classification Logic
        protected string getVT_Cedar()
        {
            if (isForestedWetland) return "WC10(ce:r1)";
            else return "MW13(ce:r1)";
        }
        protected string getVT_ForestedWetland()
        {
            if (isHardwood) //Wet Hardwood Dominant
            {
                if (bp >= 3) return "FP4(wf:r1)";
                else if (wa >= 3) return "WD1(wf:r1)";
                else if (ta >= 3) return "WD5(wf:r1)";
                else if (rm >= 3 && wa == 0) return "WD2(wf:r1)";
                else if ((rm >= 3 && wa >= 1) || sp1 == "YB") return "WD3(wf:r1)";
                else if (isWD4Ecod && sp1 == "RM") return "WD4/4a(wf:r1)";
                else return "WDU(wf:r1)";
            }
            else if (isMixedwood) //Wet Mixedwood
            {
                if (eh >= 3) return "WC8(wf:r2)";
                else if (jp >= 3) return "WC3(wf:r2)";
                else if (ta >= 5) return "WD5(wf:r2)";
                else if (bs + rm >= 5 && bs > 0 && rm > 0 && bs > sp - bs) return "WM3/3a(wf:r1)";
                else if (rs >= 3 && rs >= sp - rs) return "WM2(wf:r1)";
                else if (ws + yb >= 5 && ws > 0 && yb > 0) return "WM4(wf:r1)";
                else if (wa > 0) return "WM2(wf:r2)";
                else if (bf > swn - bf || bf + rm >= 5) return "WM1(wf:r1)";
                else if (ws >= 2 || (ws >= 0 && ws + bf >= 3) || (ws >= 0 && ws + eh >= 3) || yb >= 3) return "WM4(wf:r2)";
                else if (rm >= 2 || tl + bs >= 3) return "WM3/3a(wf:r2)";
                else return "WMU(wf:r1)";
            }
            else if (isSoftwood)//Wet Softwood Dominant
            {
                if (jp > 0 && jp > rp) return "WC3(wf:r1)";
                else if (rp > 0) return "WC4(wf:r1)";
                else if (isHighlandEcod && bs >= swn - bs) return "WB3(wf:r1)";
                else if (isHighlandEcod && ws >= swn - ws) return "WB2(wf:r1)";
                else if (eh >= 3) return "WC8(wf:r1)";
                else if (tl >= 3) return "WC7/7a(wf:r1)";
                else if (sp1 == "WS" || (ws + bf >= 5 && ws >= 3)) return "WC9(wf:r1)";
                else if (bf > swn - bf && bs < 3 && bf + bs >= 5) return "WC6(wf:r1)";
                else if (rs + bf >= 5 && rs >= 3) return "WC5(wf:r1)";
                else if (sp1 == "XS") return "WC1/WC2(wf:r1)";
                else if (bf >= 3 && bf > bs) return "WC6(wf:r2)";
                else if (bs >= 3) return "WC1/WC2(wf:r2)";
                else return "WCU(wf:r1)";
            }
            else
            {
                return "vtErr:Swpct=" + sw.ToString();
            }
        }
        protected string getVT_ForestedFloodplain()
        {

            if (isHardwood) //Hardwood Dominant
            {
                if (we >= 3 && rm > sm && rm > 0) return "FP3(fp:r1)";
                else if (wa >= 4) return "FP7(fp:r1)";
                else if (sm + yb + wa + be + th >= 5 && ro == 0) return "FP1(fp:r1)";
                else if (ro + wa + rm + sm >= 5 && ro > 0) return "FP2(fp:r1)";
                else if (rm >= 4 && ro == 0 && rm > wa) return "FP3(fp:r2)";
                else if (bc >= 4) return "FP5(fp:r1)";
                else //Otherwise revert to base rules
                {
                    vegRuleTrace += "fpx1->";
                    return getVegTypeBase(true, false, true);
                }

            }
            else if (isMixedwood || isSoftwood)//Mixedwood-Softwood Dominant
            {
                if (ws >= 3) return "FP6(fp:r1)";
                else if (tl >= 5 && soilDrainClass == "Well") return "SP9(fp:r1)";
                else if (tl >= 5 && soilDrainClass != "Well") return "WC7(fp:r1)";
                else if (rm >= 5) return "FP3(fp:r3)";
                else if (th >= 5 && ws >= 2) return "FP1(fp:r2)";
                else if (wa >= 5) return "FP7(fp:r1)";
                else //Otherwise revert to base rules
                {        
                    vegRuleTrace += "fpx2->";
                    return getVegTypeBase(true, false, true);
                }
            }
            else
            {
                return "vtErr:Swpct=" + sw.ToString();
            }
        }

        protected string getVT_Highlands()
        {
            if (isHardwood) //Hardwood Dominant
            {
                if (wb + yb >= 5 && wb >= rm && yb >= rm) return "HL4(hl:r1)";
                else if (rm + yb >= 5 && yb >= 2) return "TH8(hl:r1)";
                else if (rm >= 5 && yb < 2) return "IH7(hl:r1)";
                else if (rm + wb >= 5 && wb < 6) return "IH6/8(hl:r1)"; // identify stands with high huckleberry cover (IH8) in PSPs.
                else if (rm + wb >= 5 && wb >= 6) return "IH6b(hl:r1)";
                else return "HL4(hl:r2)";
            }
            else if (isMixedwood) //Mixedwood
            {
                if (yb >= wb) return "HL3(hl:r1)";
                else if (wb > yb) return "HL5(hl:r1)";
                else return "HLU(hl:r1)";
            }
            else //Softwood
            {
                if (bf >= 7 || (bf >= swn - bf && bf > ws && ws >= bs)) return "HL1(hl:r1)";
                else if (tl >= 5) return "WC7(hl:r1)";
                else if (bf >= 3 && (bf + bs) >= (swn - (bf + bs)) && ws < bs) return "HL6(hl:r2)";
                else if (ws >= 5 || (ws >= bf)) return "HL2(hl:r1)";
                else
                {
                    if (bs >= swn - bs) return "SP5/7(hl:r1)";
                    else if (bf + bs >= 6 && ws <= 3) return "HL6(hl:r3)";
                    else return "HLU(hl:r2)";
                }

            }
        }
        protected string getVT_Coastal()
        {
            if (isHardwood) //Hardwood Dominant
            {
                vegRuleTrace += "cox1->";
                return getVegTypeBase();
            }
            else if (isMixedwood) //Mixedwood
            {
                if (rs + th == 0) return "CB4(co:r1)";
                else if (ws >= 5) return "CB2/OF1(co:r1)";
                else if ((rs > 3 || rs >= sp - rs) && isCoastalOverrideEcod) return "CA1(co:r1)";
                else return "CBU(co:r2)";
            }
            else if (isSoftwood)//Softwood Dominant
            {
                if (bs + bf + tl >= 5 && bs > bf && bs > tl) return "CB1(co:r1)";
                else if (ws + bf + tl >= 5 && ws > bf + tl) return "CB2/OF1(co:r2)";
                else if (bf > 0 && bf > bs && bf > rs && bf > ws) return "CB3(co:r1)";
                else if ((rs > 5 || rs >= sp - rs) && isCoastalOverrideEcod) return "CA1(co:r1)";
                else if (jp >= 5) return "SP1(co:r1)";
                else if (tl >= 5 && soilDrainClass != "Poor") return "SP9(co:r1)";
                else if (tl >= 5 && soilDrainClass == "Poor") return "WC7(co:r1)";
                else if (bs >= 3 && bs >= ws) return "CB1(co:r2)";
                else if (ws >= 3 && ws > bs) return "CB2/OF1(co:r3)";

                else return "CBU(co:r3)";
            }
            else
            {
                return "vtErr:Swpct=" + sw.ToString();
            }
        }
        protected string getVT_Karst()
        {
            if (isHardwood) //Hardwood Dominant
            {
                if(th>=5) return "KA2(ka:r1)";
                else return "KA4(ka:r1)";
            }
            else if (isMixedwood) //Mixedwood
            {
                if (cs >= 5) return "KA3(ka:r1)";
                else return "KA4(ka:r2)";
            }
            else if (isSoftwood)//Softwood Dominant
            {
                if (eh >= 2) return "KA1(ka:r1)";
                else return "KA4(ka:r3)";
            }
            else
            {
                return "vtErr:Swpct=" + sw.ToString();
            }
        }
        protected string getVT_Plantation()
        {
            if (ns >= 3) return "NS plt";
            else if (ex >= 3) return "EX plt";
            else if (bs >= 5) return "BS Plt";
            else if (rs >= 5) return "RS Plt";
            else if (ws >= 5) return "WS Plt";
            else if (wp >= 5) return "WP Plt";
            else if (jp >= 5) return "JP Plt";
            else if (rp >= 5) return "RP Plt";
            else if (tl >= 5) return "TL Plt";
            else return "Unk Plt";
        }


        //Base Forest Logic by CoverType
        protected string getVT_hw()
        {
            if (th <= 2) //Intolerant Hardwood
            {
                if (bp >= 5 && sp1 == "BP") return "FP4(ih:r1)";
                else if (we >= 3) return "FP3(ih:r1)";
                else if (bc + rm >= 5 && bc >= 3) return "FP5(ih:r1)";
                else if (bp >= 4) return "FP4(ih:r1)";
                else if (wi >= 4) return "FPU(ih:r1,wi)";
                else if (bc + ws >= 4) return "FP6(ih:r1,bc)";


                else if ((gb >= 5 || (gb + ws > 5 && gb >= ws) || (gb + ta > 5 && gb >= ta)) && (gb > rm && rm + ta < 5)) return "IH9(ih:r1)";


                else if (ro + ta >= 6 && ro >= 2 && ta >= 2) return "IH1a(ih:r1)";


                else if (ta >= 5 || (ta + rm >= 5 && ta >= 3))
                {
                    if (rm >= 3) return "IH4a(ih:r1)";
                    else if (wa >= 1) return "IH3/5(ih:r1)";  // split in psps based on ltA/tA
                    else if (rm + wb >= 5) return "IH6a(ih:r1)";
                    else if (soilDrainClass == "WELL") return "IH1(ih:r1)";  // target ltA in psps
                    else return "IH4(ih:r1)"; // target tA in psps
                }


                else if (ro >= 5) return "IH2(ih1)";
                else if (ro + rm >= 5 && ro >= 2 && rm > 0) return "IH2a(ih:r1)";
                else if (sp1 == "RO") return "IH2a(ih:r2)";


                else if (rm >= 5 || (rm + ta >= 5 && rm > 0 && ta > 0))
                {
                    if (rm >= 6 && wb == 0 && yb <= 1 && sm == 0) return "IH7(ih:r1)";
                    else if (rm + yb >= 7 && yb == 2)
                    {
                        if (wa == 0) return "TH8(ih:r1)";
                        else return "TH8a(ih:r1)";
                    }
                    else if (ta == 2 && rm >= 3 && wb + gb == 0) return "IH4a(ih:r2)";
                    else if (ta == 2 && rm >= 3 && wb + gb > 0) return "IH6a(ih:r2)";
                    else if (rm + sm >= 5 && sm >= 2) return "TH9(ih:r1)";
                    else if (rm + wb >= 5) return "IH6/8(ih:r1)"; // identify stands with high huckleberry cover (IH8) in PSPs.
                    else return "IHU(ih:r1)"; 
                }


                else if (wb >= 5 || (rm + wb >= 5 && rm > 0 && wb > 0) || (ta + wb >= 5 && ta > 0 && wb > 0) || (ro + wb >= 5 && ro > 0 && wb > 0))
                {
                    if (wb + yb >= 7 && yb == 2) return "TH7(ih:r1)";
                    else if (wb > 5 && ta <= 1 && yb <= 1) return "IH6b(ih:r1)";
                    else if (ta > 1) return "IH6a(ih:r3)";
                    else if (rm + yb == 6) return "TH8(ih:r2)";
                    else return "IH6/8(ih:r2)"; // identify stands with high huckleberry cover (IH8) in PSPs.
                }


                //Final clean Up rules to classify odd mixes that don't fit into main rule set...i.e:GB04RM04BF02,TA04IH04XS02
                else if (sp1 == "GB") return "IH9(ih:r9)";
                else if (sp1 == "WB") return "IH6b(ih:r9)";
                else if (sp1 == "RM") return "IH6/8(ih:r9)"; // identify stands with high huckleberry cover (IH8) in PSPs.
                else if (sp1 == "TA") return "IH4(ih:r9)";

                else if (sp1 == "IH" | sp2 == "IH") return "IHU(ih:r9,ih)";  //old inventory 'IH' codes

                else return "IHU(ih:r2)";
            }
            else //Tolerant Hardwood
            {
                if (wa > 0 && wi > 0 && wa + wi >= 6) return "FP7(th:r1)";
                if (we >= 3) return "FP3(th:r1)";

                else if (wa + ws >= 5 && cs <= 1) return "OF6(th:r1)";

                else if (ta >= 5) return "IH3/5(th:r1)";

                else if (th >= 5)
                {
                    if (ro >= 2) return "TH6(th:r1)";

                    else if (be >= 5) return "TH5(th:r1)";

                    else if (wa + iw >= 1 && sm >= rm) return "TH3/4(th:r1)";
                    else if (wa + iw >= 1 && sm < rm && yb > 0) return "TH8a(th:r1)";
                    else if (wa + iw == 0)
                    {
                        if (yb >= be && yb > sm && sm >= 1 && rm < 3 && wb < 3) return "TH1a/2a(th:r1)";
                        else if (rm >= 2 && sm >= 2 && rm + sm >= 5 && be + yb < 5) return "TH9(th:r1)";
                        else if (yb + rm >= 5 && rm >= 1 && wb < 3 && be + sm < 3) return "TH8(th:r1)";
                        else if (th >= 5 && sm >= be && sm >= yb && rm < 3) return "TH1/2(th:r1)";
                        else if (yb + rm >= 5 && rm > sm && yb > 0 && rm > wb && wa == 0) return "TH8(th:r2)";
                        else if (yb + rm >= 5 && rm > sm && yb > 0 && rm > wb && wa > 0) return "TH8a(th:r2)";
                        else if (yb + wb >= 5 && wb > 0) return "TH7(th:r1)";
                        else if (yb >= 5) return "TH1a/2a(th:r2)";
                        else if (we > 0) return "FPU(th:r1)";
                        else if (sp1 == "BE") return "TH1/2(th:r2)";
                        else return "THU(th:r1)";
                    }
                    else if (sm + rm >= 5 && rm >= 3) return "TH9(th:r2)";
                    else if (wa >= 2 && wa + be + yb >= 6) return "TH3/4(th:r2)";
                    else return "THU(th:r2)";
                }

                else if (ro > 0) return "TH6(th:r1)";
                else if (wb > 0 && yb > 0 && wb > rm) return "TH7(th:r2)";
                else if (sm + rm >= 5 && rm >= 3 && sm >= th - sm) return "TH9(th:r3)";

                else if (yb >= th - yb && rm >= ih - rm && rm + yb >= 5 && wa == 0) return "TH8(th:r3)";
                else if (yb >= th - yb && rm >= ih - rm && rm + yb >= 5 && wa > 0) return "TH8a(th:r3)";

                else if (rm + ta > 0 && rm + ta >= wb && wa == 0) return "TH8(th:r4)";
                else if (rm + ta > 0 && rm + ta >= wb && yb > 0 && rm > 0 && wa > 0) return "TH8a(th:r4)";

                else if (rm + wb >= 5)
                {
                    if (rm >= wb && ta > 1) return "IH6a(th:r1)";
                    else if (rm >= wb && ta <= 1) return "IH6/8(th:r1)"; // identify stands with high huckleberry cover (IH8) in PSPs.
                    else if (wb > rm && rm > 0) return "IH6b(th:r1)";
                    else return "IHU(th:r1)";
                }

                else if (gb >= 5) return "IH9(th:r1)";

                else return "THU(th:r3)";
            }

        }
        protected string getVt_mw()
        {
            if (we >= 3) return "FP3(mw:r1)";
            else if (bc >= 4) return "FP5(mw:r1)";

            else if (uih + uth + xs >= 5) return "MWU(mw:r1)";

            else if (sp >= sw - sp) //Spruces are the most common softwoods
            {
                if (rs >= sp - rs && rs > eh && ro + wp < 5) // rS is the most common spruce
                {
                    if (sm + yb + be >= ih) return "MW1(mw:r1)";  // rS-TH
                    else if (ta == 0) return "MW2(mw:r1)";  // rS-IH
                    else return "MW2a(mw:r1)";  // rS-IH (Aspen variant)
                }
                else if (ws >= sp - ws && ws >= bf && ws > eh) // wS is the most common spruce
                {
                    if (ws + bc >= 7 & bc > 0 && ws > 5) return "FP6(mw:r1)";
                    else if (ws + bc >= 7 & bc > 0 && ws <= 5) return "FP5(mw:r2)";
                    else if (ws >= 7) return "OF1(mw:r1)";
                    else if (wa >= 5) return "OF6(mw:r1)";
                    else if (gb + ws >= 7 && gb >= 4) return "IH9(mw:r1)";
                    else if (sm + yb + be >= ih) return "MW5(mw:r1)";  // wS-TH
                    else return "MW6(mw:r1)";  // wS-IH
                }
                else if (bs >= sp - bs && bs >= bf) //bS is the most common spruce
                {
                    if (rm >= hw - rm && ta <= 1)
                    {
                        if (soilDrainClass == "Poor") return "WM3";
                        else return "MW9(mw:r1)"; // bS-rM/wB
                    }
                    else return "MW10(mw:r1)";  // bS-aspen
                }
                else if (eh + sp >= 5)
                {
                    if (sm + yb + be >= ih) return "MW3(mw:r1)";  // eH-TH
                    else if (sm + yb + be < ih && ta >= ih - ta) return "MW4a(mw:r1)";  // eH-IH (Aspen variant)
                    else if (sm + yb + be < ih && ta < ih - ta) return "MW4(mw:r1)";  // eH-IH
                    else return "MWU(mw:r2)";
                }
                else return "MWU(mw:r3)";

            }
            else //Spruces are NOT the most common softwoods
            {
                if (eh >= swn - eh || eh >=3)
                {
                    if (sm + yb + be >= ih) return "MW3(mw:r1)";  // eH-TH
                    else if (sm + yb + be < ih && ta >= ih - ta) return "MW4a(mw:r1)";  // eH-IH (Aspen variant)
                    else if (sm + yb + be < ih && ta < ih - ta) return "MW4(mw:r1)";  // eH-IH
                    else return "MWU(mw:r4)";
                }
                else if (bf > swn - bf)
                {
                    if (sm + yb + be >= ih) return "MW5(mw:r2)";  // wS-TH         
                    else if (rm >= wb && rm > yb && rm > 0) return "MW7(mw:r1)";  // bF-rM
                    else if (wb >= rm && wb > 0) return "MW8(mw:r1)";  // bF-wB
                    else return "MWU(mw:r5)";
                }
                else if (wp >= swn - wp)
                {
                    if(ta >= 7) return "IH1(mw:r1)";
                    else if (ro >= rm && ro > 0) return "MW11(mw:r1)";  // rO-wP
                    else return "MW12(mw:r1)";   // rM-wP
                }
                else
                {
                    if (ws >= bf && sm + yb + be >= ih && ws > 0) return "MW5(mw:r9)";
                    else if (ws >= bf && ih > th && ws > 0) return "MW6(mw:r9)";
                    else if (bs >= bf && ta <= 1 && bs > 0) return "MW9(mw:r9)";
                    else if (bs >= bf && ta > 1 && bs > 0) return "MW10(mw:r9)";
                    else if (rs >= bf && th >= ih && rs > 0 && rs > eh) return "MW1(mw:r9)";
                    else if (rs >= bf && ih > th && rs > 0 && rs > eh) return "MW2(mw:r9)";
                    else if (eh >= swn - eh && sm + yb + be >= ih) return "MW3(mw:r9)";
                    else if (eh >= swn - eh && ih > sm + yb + be) return "MW4(mw:r9)";
                    else if (jp >= 4) return "SP1(mw:r9)";
                    else return "MWU(mw:r9)";
                }

            }

        }
        protected string getVt_sw()
        {

            //Old field
            if (ws >= 7 || (ws + tl >= 7 && tl >= 2))
            {
                if (ws >= 7) return "OF1(of:r1)";
                else return "OF2(of:r1)";
            }
            else if (wp >= 5 && isOldFieldEcod && ecosTopo == "DM") return "OF3(of:r1)";
            else if (wp + ws >= 5 && ws >= wp && isOldFieldEcod && ecosTopo == "DM" && eh + rs <= 1) return "OF1(of:r2)";

            //Spruce-Pine
            else if (bs + pin >= 5 || tl >= 5 || bs + tl >= 5)
            {
                if (bs + jp >= 5 && jp > 0 && jp >= bs) return "SP1(sp:r1)";
                else if (bs + jp >= 5 && jp > 0 && jp < bs) return "SP1a(sp:r1)";

                else if (wp + rp >= 5 && rp > 0 && wp > 0) return "SP3(sp:r1)";

                else if (bs + rp >= 5 && rp > 0 && rp >= bs) return "SP2(sp:r1)";
                else if (bs + rp >= 5 && rp > 0 && rp < bs) return "SP2a(sp:r1)";

                else if (bs + wp >= 5 && wp > 0 && bs < 5) return "SP4(sp:r1)";
                else if (bs + wp >= 5 && wp > 0 && bs >= 5) return "SP4a(sp:r1)";

                else if (bs + ws >= 7 && ws > 0 && ws >= swn - ws) return "SH6(sp:r1)";

                else if (tl >= 5) return "SP9(sp:r1)";

                else if (bs >= 5 && tl > 0) return "SP7(sp:r1)";

                else if (bs + eh >= 5 && eh > 0) return "SP8(sp:r1)"; //Gets the black spruce dominated stands of SH11

                else if (bs + bf >= 5 && bf >= bs) return "SP6(sp:r1)";

                else if (bs >= 5 && bs >= swn - bs) return "SP5/7(sp:r1)"; //If well drained = SP5 otherwise sp7

                else if (bs + tl >= 5 && (tl > 0 && tl < 5)) return "SP5/7(sp:r3)"; //If well drained = SP5 otherwise sp7

                else if (rp + rs >= 5) return "SP2a(sp:r2)";

                else if (wp + rs >= 5) return "SP4(sp:r2)";

                else if (bs + ws >= 7) return "SH6(sp:r1)";

                else return "SPU(sp:r1)";

            }

            //Spruce Hemlock - with eH
            else if (eh > 0)
            {
                if (eh + wp + rs >= 5)
                {
                    if (eh >= 7 && wp == 0) return "SH1(sh:r1)";
                    else if (wp >= sx && rs < 2) return "SH2(sh:r1)";
                    else return "SH3(sh:r2)";
                }
                else if (eh + bs >= 5 && bs > 0) return "SP8(sh:r1)";
                else if (ws + bf >= 5 && ws > 0 && eh < 3) return "SH6(sh:r1)";
                else return "SH3(sh:r3)";
            }

            //Spruce Hemlock - eH absent
            else
            {
                if (tl >= 3) return "SP9(sh:r1)";
                else if (sx + wp >= 5 && (wp >= 2 && wp <= 4)) return "SH4(sh:r1)";
                else if (sx + ws >= 5 && ws > 0 && rs > 0) return "SH7(sh:r1)";
                else if (ws + bf >= 5 && ws > 0 && bf > 0 && ws > bs && ws > rs) return "SH6(sh:r2)";
                else if (ws + bs >= 5 & ws > 0 && bs > 0) return "SP10(sh:r1)";
                else if (rs + bf >= 5 && rs > 0 && rs > ws && rs >= bs) return "SH5(sh:r1)";
                else if (rs + ws >= 4 && rs > 0 && ws > 0) return "SH7(sh:r2)";
                else if (bf >= swn - bf)
                {
                    if (bs <= 1) return "SH8(sh:r1)";
                    else if (bs >= 2) return "SP6(sh:r1)";
                    else return "SHU(sh:r1)";
                }
                else if (bs + bf >= 5 && bs > 0 && bf > 0) return "SP6(sh:r2)";
                else if (xs >= 5 || xs + bf >= 5) return "SHU(sh:r2)";
                else if (bs + ws >= 7) return "SH12(sh:r2)";
                else if (wp + rm >= 7) return "MW11(sh:r1)";
                else return "SHU(sh:r3)";
            }
        }
    }
}
