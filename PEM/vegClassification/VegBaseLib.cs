using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PEM_VegClassification
{
    public static class VegBaseLib
    {

        //Dictionary of Species Grouping Used in Forest Community Assignement Logic
        public readonly static Dictionary<string, List<string>> sppGrps = new Dictionary<string, List<string>>() {
        //Hardwood
         {"hw"  , new List<string>() { "SM", "YB", "AS", "BA", "WA", "BE", "WE", "TH", "IW", "AL", "WI", "PI", "RM", "WB", "GB", "TA", "LA", "RO", "IH", "OH", "UH", "BP", "BC" }} // 23 - 3 OldCodes: IH,TH,OH
        ,{"th"  , new List<string>() { "SM", "YB", "AS", "BA", "WA", "BE", "WE", "TH", "IW" }} // 9
        ,{"ih"  , new List<string>() { "AL", "WI", "PI", "RM", "WB", "GB", "TA", "IH", "OH", "UH", "BP", "RO", "BC", "LA" }} // 14
        ,{"robc", new List<string>() { "RO", "BC" }} // 11
        ,{"rm"  , new List<string>() { "RM" }}
        ,{"ta"  , new List<string>() { "TA", "LA" }}
        ,{"wi"  , new List<string>() { "WI" }}
        ,{"we"  , new List<string>() { "WE" }}
        ,{"wb"  , new List<string>() { "WB" }}
        ,{"gb"  , new List<string>() { "GB" }}
        ,{"bc"  , new List<string>() { "BC" }}
        ,{"bp"  , new List<string>() { "BP" }}
        ,{"ro"  , new List<string>() { "RO" }}
        ,{"sm"  , new List<string>() { "SM" }}
        ,{"be"  , new List<string>() { "BE" }}
        ,{"iw"  , new List<string>() { "IW" }}
        ,{"yb"  , new List<string>() { "YB" }}
        ,{"wa"  , new List<string>() { "AS", "BA", "WA" }}
        ,{"uih" , new List<string>() { "IH" }}
        ,{"uth" , new List<string>() { "TH" }}

        //Softwood
        ,{"sw"  , new List<string>() { "WS", "RS", "BS", "XS", "BF", "EH", "WP", "RP", "JP", "TL", "EC", "OS", "SP", "AP", "WL", "EL", "JL", "XL", "SS", "NS", "DF", "US" }} // 22 - 2 OldCodes: XS,OS
        ,{"sp"  , new List<string>() { "WS", "RS", "BS", "XS", "SS", "NS" }}
        ,{"sx"  , new List<string>() { "RS", "BS", "XS" }}
        ,{"bs"  , new List<string>() { "BS" }}
        ,{"ws"  , new List<string>() { "WS" }}
        ,{"rs"  , new List<string>() { "RS" }}
        ,{"xs"  , new List<string>() { "XS" }}
        ,{"ns"  , new List<string>() { "NS" }}
        ,{"bf"  , new List<string>() { "BF" }}
        ,{"pi"  , new List<string>() { "WP", "RP", "JP", "SP", "AP" }}
        ,{"wp"  , new List<string>() { "WP" }}
        ,{"jp"  , new List<string>() { "JP" }}
        ,{"rp"  , new List<string>() { "RP" }}
        ,{"tl"  , new List<string>() { "TL" }}
        ,{"xl"  , new List<string>() { "EL", "WL", "JL", "XL" }}
        ,{"eh"  , new List<string>() { "EH" }}
        ,{"ec"  , new List<string>() { "EC" }}
        ,{"uc"  , new List<string>() { "UC", "US", "UH" }}
        ,{"ex"  , new List<string>() { "NS", "EL", "WL", "JL", "XL", "AP", "SP", "SS", "DF" }}

        //Softwood-Native
        ,{"swn" , new List<string>() { "BF", "BS", "EC", "EH", "JP", "OS", "RP", "RS", "TL", "US", "WP", "WS", "XS" }} // 13 - 2 OldCodes: XS,OS
        ,{"pin" , new List<string>() { "WP", "RP", "JP" }}

        //Non-Coastal
        ,{"nc" , new List<string>() { "AS","BA","BE","EC","EH","IW","RP","RS","SM","TH","WA","WE","YB" }} // 13 **need to verify if RS should be included???

        //Highlands Spp
        ,{"hl" , new List<string>() { "BF","WS","BS","TL","WB","YB" }} //

        //Climax Species
        ,{"cs" , new List<string>() { "BE","EH","RS","SM","WP","YB","IW" }} // 7
        };

        //Dictionary of Outdated Species Codes with Current Naming
        public readonly static Dictionary<string, string> sppOldCodes = new Dictionary<string, string>()
        {
             {"S", "XS"}
            ,{"F", "BF"}
            ,{"A", "TA"}
            ,{"H", "OH"}
            ,{"E", "EH"}
            ,{"O", "RO"}
            ,{"L", "TL"}
        };

        //List of Provincial Ecodistricts
        public readonly static List<int> stdEcodList = new List<int>() { 100, 210, 220, 310, 320, 330, 340, 350, 360, 370, 380, 410, 420, 430, 440, 450, 510, 520, 530, 540, 550, 560, 610, 620, 630, 710, 720, 730, 740, 750, 760, 770, 780, 810, 820, 830, 840, 910, 920 };

        //List of Species-Level Seral Scores by Ecodistrict
        public readonly static Dictionary<string, sppSeral> sppEcodSerVals = new Dictionary<string, sppSeral>()
        {
             {"AL", new sppSeral("AL", "alder"                , "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1")}
            ,{"AP", new sppSeral("AP", "austrian pine"        , "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1")}
            ,{"AS", new sppSeral("AS", "ash"                  , "4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4")}
            ,{"BA", new sppSeral("BA", "black ash"            , "2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2")}
            ,{"BC", new sppSeral("BC", "black cherry"         , "2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2")}
            ,{"BE", new sppSeral("BE", "beech"                , "5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5")}
            ,{"BF", new sppSeral("BF", "balsam fir"           , "5,5,5,5,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,5,5,5,5,1,1")}
            ,{"BP", new sppSeral("BP", "balsam poplar"        , "1,3,3,3,3,1,1,1,1,1,1,1,1,1,1,1,3,3,3,3,1,1,1,1,3,1,1,1,1,1,1,1,1,1,1,1,1,3,1")}
            ,{"BS", new sppSeral("BS", "black spruce"         , "5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5")}
            ,{"BW", new sppSeral("BW", "basswood"             , "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1")}
            ,{"DF", new sppSeral("DF", "douglas fir"          , "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1")}
            ,{"EC", new sppSeral("EC", "eastern cedar"        , "5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5")}
            ,{"EH", new sppSeral("EH", "eastern hemlock"      , "5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5")}
            ,{"EL", new sppSeral("EL", "european larch"       , "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1")}
            ,{"EM", new sppSeral("EM", "white elm"            , "2,2,4,2,4,2,2,2,2,2,2,2,2,2,2,2,4,4,4,2,2,2,4,4,4,2,2,2,2,2,2,2,2,2,2,2,2,2,2")}
            ,{"EO", new sppSeral("EO", "english oak"          , "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1")}
            ,{"GB", new sppSeral("GB", "grey birch"           , "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1")}
            ,{"HL", new sppSeral("HL", "hybrid larch"         , "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1")}
            ,{"IH", new sppSeral("IH", "intolerant hardwood"  , "3,2,4,2,2,2,2,2,4,2,2,2,2,2,2,2,2,2,2,2,4,3,2,2,2,2,2,2,2,2,2,3,2,2,2,2,2,2,2")}
            ,{"IW", new sppSeral("IW", "ironwood"             , "4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4")}
            ,{"JL", new sppSeral("JL", "japanese larch"       , "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1")}
            ,{"JP", new sppSeral("JP", "jack pine"            , "2,3,2,2,2,2,2,2,2,2,2,2,2,3,2,2,2,2,3,3,3,3,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2")}
            ,{"LA", new sppSeral("LA", "largetooth aspen"     , "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1")}
            ,{"NS", new sppSeral("NS", "norway spruce"        , "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1")}
            ,{"OH", new sppSeral("OH", "other hardwood"       , "3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3")}
            ,{"OS", new sppSeral("OS", "other softwood"       , "3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3")}
            ,{"PC", new sppSeral("PC", "pin cherry"           , "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1")}
            ,{"RM", new sppSeral("RM", "red maple"            , "3,2,4,2,2,2,2,2,4,2,5,2,2,2,2,2,2,2,2,2,5,3,2,2,2,2,2,2,2,2,2,3,2,3,3,2,2,2,2")}
            ,{"RO", new sppSeral("RO", "oak"                  , "4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4")}
            ,{"RP", new sppSeral("RP", "red pine"             , "3,3,3,3,3,3,3,3,3,4,3,3,3,4,3,3,3,3,4,4,4,4,4,4,4,3,4,3,3,3,4,4,3,4,4,3,3,3,3")}
            ,{"RS", new sppSeral("RS", "red spruce"           , "5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5")}
            ,{"SM", new sppSeral("SM", "sugar maple"          , "5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5")}
            ,{"SP", new sppSeral("SP", "scots pine"           , "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1")}
            ,{"SS", new sppSeral("SS", "sitka spruce"         , "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1")}
            ,{"ST", new sppSeral("ST", "striped maple"        , "2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2")}
            ,{"TA", new sppSeral("TA", "aspen"                , "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1")}
            ,{"TH", new sppSeral("TH", "tolerant hardwood"    , "5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5")}
            ,{"TL", new sppSeral("TL", "eastern larch"        , "3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3")}
            ,{"UC", new sppSeral("UC", "unclassified species" , "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1")}
            ,{"UH", new sppSeral("UH", "unclassified hardwood", "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1")}
            ,{"US", new sppSeral("US", "unclassified softwood", "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1")}
            ,{"WA", new sppSeral("WA", "white ash"            , "4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4")}
            ,{"WB", new sppSeral("WB", "white birch"          , "3,4,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2")}
            ,{"WE", new sppSeral("WE", "white elm"            , "2,2,4,2,4,2,2,2,2,2,2,2,2,2,2,2,4,4,4,2,2,2,4,4,4,2,2,2,2,2,2,2,2,2,2,2,2,2,2")}
            ,{"WI", new sppSeral("WI", "willow"               , "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1")}
            ,{"WL", new sppSeral("WL", "western larch"        , "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1")}
            ,{"WP", new sppSeral("WP", "white pine"           , "5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5")}
            ,{"WS", new sppSeral("WS", "white spruce"         , "4,4,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,5,5,5,4,1,1")}
            ,{"XL", new sppSeral("XL", "hybrid larch"         , "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1")}
            ,{"XS", new sppSeral("XS", "red&black spruce"     , "5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5")}
            ,{"YB", new sppSeral("YB", "yellow birch"         , "5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5")}
        };

        //List of FEC Plot data Scecies Standardization Mapping
        public readonly static Dictionary<string, string> fecBaSppLut = new Dictionary<string, string>
        {
            {"bCh","BC"},
            {"Ir","IW"},
            {"Ald","OH"},
            {"App","OH"},
            {"Ser","OH"},
            {"cCh","OH"},
            {"mtA","OH"},
            {"mtM","OH"},
            {"pCh","OH"},
            {"stM","OH"},
            {"wiH","OH"},
            {"hS","RS"},
            {"eL","TL"},
            {"AE","WE"},
            {"Wil","WI"}
            };

    }
}
