using OPTIMA;
using System;
using System.Security.Cryptography.X509Certificates;
using static System.Console;

namespace OPTIMA
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                
                string filedirectory = @"C:\Users\Dawson\source\repos\OPTIMA\OPTIMA FILES";
                string filename = @"LastLoggedIn";
                string filepath = Path.Combine(filedirectory, filename + ".txt");
                //Run.OpenApp();

                if (!File.Exists(filepath))
                {
                    Run.OpenApp();
                }
                else
                {

                    List<string> loggedacc = new List<string>();
                    using (StreamReader sr = new StreamReader(filepath))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            loggedacc.Add(line);
                        }
                        
                    }
                    switch (loggedacc[2])
                    {

                        case "Merchant":
                            Merchant merchant = new Merchant(loggedacc);
                            foreach (Item item in merchant.allitems)
                                WriteLine(item.seller);
                            foreach (Item item in merchant.inventory)
                                WriteLine(item.name);
                            ReadKey(true);
                            merchant.UI();
                            break;

                        case "Regular":
                            Regular regular = new Regular(loggedacc);
                            regular.UI();
                            break;
                    }
                }
            }
            catch(Exception e)
            {
                WriteLine("Error: " + e.Message);
            }
        }

    }
}