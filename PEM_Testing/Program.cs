using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using PEM_VegClassification;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Testing PEM: Enter parameters:\n");

        var vi = "1,0,BS04TL03BF03,0,,0,210,WHMO,2,Bridgewater,1,0,0,0,true,5";
        Console.WriteLine($"InputString: {vi}\n  VT = {getVT(vi.Split(','))}");

        while (true)
        {
            Console.WriteLine("\nEnter parameters:\n");
            vi = Console.ReadLine();
            string vr = "";
            try
            {
                vr = getVT(vi.Split(','));
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nERR: { e.Message}");
                break;
            }
            Console.WriteLine($"\nInputString: {vi}\n  VT = {vr}");

        }



    }

    private static string getVT(string[] iVars, bool iIncludeRule = false)
    {
        try
        {
            FecVegType vt =
                new FecVegType(
                    int.Parse(iVars[0]),   //iLndclass
                    int.Parse(iVars[1]),   //iFornon
                    iVars[2],              //iSpecies
                    int.Parse(iVars[3]),   //iWetClass
                    iVars[4],              //iWCType
                    int.Parse(iVars[5]),   //iPltFlag
                    int.Parse(iVars[6]),   //iEcodistrict
                    iVars[7],              //iEcosection
                    iVars[8],              //iSoilType
                    iVars[9],              //iSoilSreies
                    iVars[10],             //iSoilDrainClass
                    int.Parse(iVars[11]),  //iKarstCode
                    iVars[12],             //iWetlandClass
                    int.Parse(iVars[13]),  //iCoastalFlag
                    bool.Parse(iVars[14]), //iConvertXs2RSBS
                    int.Parse(iVars[15])   //iSiteClass
                    );

            if (iIncludeRule)
            {
                return vt.GetVegTypeWithRule;
            }
            else
            {
                return vt.GetVegType;
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

    }

}