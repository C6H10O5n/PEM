using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PEM_VegClassification
{

    public struct psFecForonClass
    {
        public psFecForonClass(string cde, string nam)
        {
            code = cde;
            name = nam;
        }
        public string code;
        public string name;
    }
    public static class FecVegTypeLib
    {

        public static readonly Dictionary<int, psFecForonClass> FornonDict = new Dictionary<int, psFecForonClass>()
        {
             {0 ,new psFecForonClass("RCC"  ,"REGENERATING CLEARCUT")} // 0: Natural stand - any forested stand which has not been treated silviculturally and does not qualify under clear cut, partial cut, burn, old field , wind throw, alders, brush or dead categories.  The stand must contain trees that are capable of reaching at least 3 m in height at maturity.
            ,{1 ,new psFecForonClass("RCC"  ,"REGENERATING CLEARCUT")} // 1: Treated - treatment not classified, not Christmas trees. An area where silviculture activity has been identified from photos, but field data is not yet available.
            ,{2 ,new psFecForonClass("BRN"  ,"BURNS"                )} // 2: Burn - any stand that has been completely destroyed by fire leaving less than 25% crown closure.  In cases of partial burn the remaining live stand is to be categorized and not classed as burn.
            ,{3 ,new psFecForonClass("CHR"  ,"CHRISTMAS TREES"      )} // 3: Christmas trees - any stand being used for Christmas tree cultivation.
            ,{4 ,new psFecForonClass("SUG"  ,"SUGAR BUSH"           )} // 4: Sugar bush - any stand being used to produce maple sugar products. The stand may or may not have been silviculturally treated.
            ,{5 ,new psFecForonClass("OFD"  ,"OLD FIELD"            )} // 5: Old field - any field that has an indication of merchantable tree species growing in with less than 25% crown closure and less than 1.0 meters in height.
            ,{6 ,new psFecForonClass("WTH"  ,"WINDTHROW"            )} // 6: Wind throw - any stand where trees have been pushed over to more than 45 degrees from the vertical by wind action.
            ,{7 ,new psFecForonClass("MOR"  ,"DEAD"                 )} // 7: Dead - any stand that contains dead trees due to any cause and which contains less than 25% crown closure of live residual material and also contains  evidence of dead material either standing or laying on the ground with little or no evidence of regeneration.
            ,{8 ,new psFecForonClass("MOR"  ,"DEAD"                 )} // 8: Dead - 1 - any stand that contains dead trees due to any cause and has a 26-50% crown closure of live residual material and also contains evidence of dead material either standing or laying on the ground with little or no evidence of regeneration.
            ,{9 ,new psFecForonClass("MOR"  ,"DEAD"                 )} // 9: Dead - 2 - any stand that contains dead trees  due to any cause and has a  51-100% crown closure of live residual material and which contains evidence of dead material either standing or laying on the ground with little or no evidence of regeneration.
            ,{10,new psFecForonClass("RSRCH","RESEARCH"             )} //10: esearch stand - stands treated in some manner primarily to provide data on growth, etc. which contain sample plots for evaluation of response rather than intended as operational treatment.
            ,{11,new psFecForonClass("SOR"  ,"TREE SEED ORCHARDS"   )} //11: Seed orchard & seed production area - any stands designated by the Department as an area reserved for seed production.
            ,{12,new psFecForonClass("TRTD" ,"TREATED"              )} //12: Treated stand - treatment classified-an area where silviculture activity has occurred and the actual treatment has been identified primarily by field data, not including plantations, harvests, Christmas trees or sugarbush.
            ,{13,new psFecForonClass("MOR"  ,"DEAD"                 )} //13: Dead - 3 - any stand that contains 26-50% of equivalent crown closure of dead material and which contains regeneration which will be categorized in the stand classification section. Equivalent crown closure being an estimate of what the crown closure would be if the dead material was alive. 
            ,{14,new psFecForonClass("MOR"  ,"DEAD"                 )} //14: Dead - 4 - any stand that contains 51-75% of equivalent crown closure of dead material and which contains  regeneration which will be categorized in the stand classification section.  Equivalent crown closure being an estimate of what the crown closure would be if the dead material was alive.
            ,{15,new psFecForonClass("MOR"  ,"DEAD"                 )} //15: Dead - 5 - any stand that contains 75+% of equivalent crown closure of dead material and which contains regeneration which will be categorized in stand classification section.  Equivalent crown closure being an estimate of what the crown closure would be if the dead material was alive.  The live portion of the stand is to be classified as any forest stand as per the specifications.
            ,{16,new psFecForonClass("MMW"  ,"MOOSE MEADOWS"        )} //16: Moose Meadow - Any stand solely found in the Cape Breton highlands with the appearance of   old field returning to forest. Generally white spruce will be the only commercial species present with a crown closure less than 25%. All normal attributes are assigned to the existing commercial tree species as the main story. There can be no second story.
                                                                       //20: Plantation - a group of trees artificially established by direct seeding or setting out seedlings, transplants or cuttings.
            ,{33,new psFecForonClass("TSH"  ,"TALL SHRUBLANDS"      )} //33: Brush - any area containing less than 25% merchantable tree cover and contains non-merchantable woody plants consisting of at least 25% cover.  Replaces non-forested class, (FORNON 83), December, 1998
            ,{38,new psFecForonClass("TSH"  ,"TALL SHRUBLANDS"      )} //38: Alders less than 75% cover - any forested area containing alders that compose less than 75% crown closure.  Replaces non-forested class, (FORNON 88), December 1998
            ,{39,new psFecForonClass("TSH"  ,"TALL SHRUBLANDS"      )} //39: Alders 75% or greater cover - any forested area containing alders that compose 75% or more crown closure.  Replaces non-forested class, (FORNON 89), December 1998
            ,{60,new psFecForonClass("CC"   ,"RECENT CUT"           )} //60: Clear cut - any stand that has been completely cut and any residuals make up less than 25% crown closure and with little or no indication of regeneration.
                                                                       //61: Partial depletion verified - any stand that has been cut and Hardwood residuals make up 25% or more of the crown closure on the site, identified by photo Interpreters or field data.
                                                                       //62: Partial depletion not verified - a temporary code given to a stand identified from satellite imagery as a partial cut.  Further verification from photo interpretation or field data required for residuals
            ,{70,new psFecForonClass("UWL"  ,"UNCLASSIFIED WETLAND" )} //70: Wetlands general - any wet area, not identified as a lake, river or stream, excluding open and treed bogs, and beaver flowage. (In forestry data,  wetland complexes may include open and treed bogs)
            ,{71,new psFecForonClass("UWL"  ,"UNCLASSIFIED WETLAND" )} //71: Beaver flowage - an area that is or has been occupied by beavers
            ,{72,new psFecForonClass("OPL"  ,"OPEN PEATLAND"        )} //72: Open bogs - any area consisting primarily of ericaceous plants, sphagnum or other mosses with less than 25% live tree cover and poor drainage, (wet all year). Indicator plants: Bog Rosemary, Leather Leaf, Labrador Tea, Cranberry and Lambkill.  Ericaceous plants being plants in or related to the heather family (ericaceae). They are typically plants of acid soils, bogs and woodlands.
            ,{73,new psFecForonClass("WCU"  ,"WET CONIFEROUS UNCL"  )} //73: Treed bogs - any area consisting primarily of ericaceous plants, sphagnum or other mosses with stunted softwood or hardwood species having 25% or more live tree cover.
            ,{74,new psFecForonClass("UWL"  ,"UNCLASSIFIED WETLAND" )} //74: Coastal habitat areas - any area that has been defined as a wetland that lies in the ocean
            ,{75,new psFecForonClass("UWL"  ,"UNCLASSIFIED WETLAND" )} //75: Lake wetland -  any area that has been defined as a wetland that lies within freshwater (lake or river)
            ,{76,new psFecForonClass("CSTL" ,"COASTAL"              )} //76: Cliffs, dunes, coastal rocks – the area of land between the high tide mark and the forest or non-forest stand which consists of cliffs (a high steep face of a rocky or soil mass), dunes (a ridge or hill created by wind blown sand), or coastal rock (a toque shaped or lobate area of bedrock, may or may not extend into the water).
            ,{77,new psFecForonClass("WATER","INLAND WATERS"        )} //77: Inland water - inland water bodies which may include lakes, rivers, reservoirs, canals and ponds (STAND_ value: 9003)
            ,{78,new psFecForonClass("OCEAN","OPEN OCEAN"           )} //78: Ocean (STAND_ value of 9006)
            ,{83,new psFecForonClass("TSH"  ,"TALL SHRUBLANDS"      )} //83: Brush - any area containing less than 25% merchantable tree cover and contains non-merchantable woody plants consisting of at least 25% cover.  Being replaced with forested class, (FORNON 33) December, 1998
            ,{84,new psFecForonClass("LSH"  ,"LOW SHRUBLANDS"       )} //84: Rock barren - any area covered by at least 50% exposed rock outcrop and/or boulders with less than 25% live tree cover. (Boulders being rock fragments over 60cm in diameter.)
            ,{85,new psFecForonClass("LSH"  ,"BARREN"               )} //85: Barren - any area of less than 25% live tree cover containing "ericaceous" vegetation with less than 50% rock out crops and/or boulder cover and less than 50% other woody plant cover.  Area is dry and firm in summer. Indicator plants:  Bearberry, Rhodora, Blueberry, Huckleberry and Lambkill.
            ,{86,new psFecForonClass("AGR"  ,"ACTIVE AGRICULTURE"   )} //86: Agriculture - any hay field, pasture, tilled crop, or orchard which contains no merchantable species.
            ,{87,new psFecForonClass("UBN"  ,"URBAN"                )} //87: Urban - any area used primarily as residential or industrial and related structures such as streets, sidewalks, parking lots, etc.  Also includes house lots in wooded areas outside of towns and villages which are not adjacent to agricultural land and those lots surrounded by forest.  In cases of ribbon development along some roads, a strip may be delineated along the road and classed as urban. Obvious urban area within agricultural land will be delineated and coded accordingly. Categories that will be classified as urban are bunkers, golf courses, picnic parks, campgrounds, drive in theaters, auto salvage yards, power stations, water treatment areas, lagoons sewer/water, cemeteries, light houses, ball parks, etc.
            ,{88,new psFecForonClass("TSH"  ,"TALL SHRUBLANDS"      )} //88: Alders less than 75% cover - any forested area containing alders that compose less than 75% crown closure.  Dry land only. Being replaced with forested class, (FORNON 38), December, 1998)
            ,{89,new psFecForonClass("TSH"  ,"TALL SHRUBLANDS"      )} //89: Alders 75% or greater cover - any forested area containing alders that compose 75% or more crown closure.  Dry land only. Being replaced with forested class, (FORNON 39), December, 1998
            ,{91,new psFecForonClass("LBB"  ,"BLUEBERRIES"          )} //91: Blueberries - areas that appear to have been or are being used for blueberry production.
            ,{92,new psFecForonClass("MISC" ,"MISC"                 )} //92: Miscellaneous - any area of non-forest not covered by any of the other non-forest categories,  i.e. old mill site, rifle range, tower site, observation site, lake shore bottom (where unable to give forest/non-forest code), quarry, mining activity, wharf, pier, causeway, dams, unidentified objects, airstrips, etc.
            ,{93,new psFecForonClass("SANL" ,"LAND FILLS"           )} //93: Sanitary land fill - areas used by municipalities for disposal of garbage by means of burying the material.
            ,{94,new psFecForonClass("BEA"  ,"BEACH"                )} //94: Beach - that area of land between normal water line and the forest or non-forest category (i.e. bog, etc.). Areas exposed due to abnormally low water levels are not considered to be part of a beach.
            ,{95,new psFecForonClass("QRY"  ,"GRAVEL PITS"          )} //95: Gravel pit - any area either active or non-active used for the purpose of extracting gravel.
            ,{96,new psFecForonClass("PLS"  ,"PIPELINES"            )} //96: Pipeline corridor - A 25 meter buffer around a defined linear feature of a gas or oil pipeline route defining limited or restricted use lands.
            ,{97,new psFecForonClass("HYD"  ,"POWER LINES"          )} //97: Powerline corridor – A corridor of land with limited use due to powerlines, as defined from photography ( STAND_ value 9002)
            ,{98,new psFecForonClass("ROAD" ,"ROADS"                )} //98: Road corridor - Generated polygons of varying widths for paved roads, based on road classes. (STAND_ value 9000)
            ,{99,new psFecForonClass("RLY"  ,"RAILWAY CORRIDOR"     )} //99: Rail corridor - Generated 20 meter polygons around active and abandoned rail lines (STAND_ values 9001 & 9005)
            };

        public static readonly Dictionary<string, string> WetLandClassDict = new Dictionary<string, string>()
        {
             {"FRESHWATER MARSH","FWM"}
            ,{"OPEN PEATLAND","OPL"}
            ,{"SALT MARSH","SM"}
            ,{"SHALLOW WATER WETLAND","SWW"}
            ,{"SHRUB PEATLAND","SPL"}
            ,{"SHRUB SWAMP","SSW"}
            ,{"TREED PEATLAND","WCU"}
            ,{"TREED SWAMP","WDU"}
            ,{"UNCLASSIFIED WETLAND","UWL"}
        };

        public static readonly Dictionary<string, string> WetClassInvTypesDict = new Dictionary<string, string>()
        { 
                          //[nvi.wc_type]
             {"B","BEA"}  //Coastal Beach
            ,{"C","CLF"}  //Coastal Cliff Face
            ,{"D","CDN"}  //Coastal Dune
            ,{"R","COR"}  //Coastal/Exposed Rock
            ,{"RV","CRV"} // Coastal/Exposed Rock-Vegetated
            ,{"P","SLP"}  //Saline Pond
            ,{"S","SM"}   //Salt Marsh
            ,{"LW","SWW"} //Lake Wetland
            ,{"WG","UWL"} //Wetlands General
            ,{"U","UWL"}  //Undetermined
        };

        public static readonly List<string> SoilSeries_floodplain = new List<string> { "STEWIACKE", "CUMBERLAND", "MOSSMAN", "BRIDGEVILLE" };
        public static readonly List<string> SoilTypes_wetland     = new List<string> { "4", "7", "10", "13", "14" };
        public static readonly List<int> SoilTypes_wetland_Fornon = new List<int> { 16,33,38,39,76,84,85,92,94,96,97 };

        public static readonly List<int> ecodists_WD4                 = new List<int> { 730, 740, 750, 760, 840 };
        public static readonly List<int> ecodists_for_Highland        = new List<int> { 100, 210 };
        public static readonly List<int> ecodists_for_Coastal         = new List<int> { 810, 820, 830, 840, 910, 920 };
        public static readonly List<int> ecodists_for_OldFields       = new List<int> { 720, 730, 740, 750 };
        public static readonly List<int> ecodists_for_CoastalOverride = new List<int> { 840, 910 };


    }
}
