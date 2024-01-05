using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace OPTIMA
{
    using static Input;
    public abstract class User
    {
        public string username { get; set; }
        public string password { get; set; }
        public string accounttype { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string fullname { get; set; }
        public string storename { get; set; }
        public DateTime createdon { get; set; }

        public List<Item> allitems;
        public List<Purchased> allorders;

        public User() { }

        public User(List<string> userdetails)
        {
            try
            {
                username = userdetails[0];
                password = userdetails[1];
                accounttype = userdetails[2];
                firstname = userdetails[3];
                lastname = userdetails[4];
                fullname = userdetails[5];
                storename = userdetails[6];
                createdon = DateTime.Parse(userdetails[7]);

                allitems = GetContent("ProductInventory.csv").Select(x => new Item(x)).ToList();
                allorders = GetContent("PurchasedItems.csv").Select(x => new Purchased(x)).ToList();
            }
            catch (Exception e)
            {
                WriteLine(e.Message);
            }
        }

        public void Logout()
        {

            string filename = "LastLoggedIn";
            string filepath = Path.Combine(basedirectory, filename + ".txt");
            File.Delete(filepath);

            Run.MerchantorRegular();
            
        }

        public abstract void UI();


        public virtual void Profile() { }

        //filehandling
        string basedirectory = @"C:\Users\Dawson\source\repos\OPTIMA\OPTIMA FILES";

        public List<string[]> GetContent(string filename)
        {
            List<string[]> content = new List<string[]>();
            try
            {
                string filepath = Path.Combine(basedirectory, filename);
                if (File.Exists(filepath))
                {
                    using (StreamReader reader = new StreamReader(filepath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] fields = line.Split(",");
                            content.Add(fields);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                WriteLine(e.Message);
                ReadKey(true);
                return GetContent(filename);
            }
            return content;
        }

        public void UpdateFile(string filename, string toadd)
        {
            try
            {
                string filepath = Path.Combine(basedirectory, filename);
                List<string> header = new List<string>();
                if (!File.Exists(filepath))
                {
                    using (StreamWriter writer = File.CreateText(filepath))
                    {
                    }
                    UpdateFile(filename, toadd);
                }
                else
                {
                    using (StreamWriter writer = File.AppendText(filepath))
                    {
                        writer.WriteLine(toadd);
                    }
                }
            }
            catch (Exception e)
            {
                WriteLine("Error: ", e.Message);

            }
        }
        public void UpdateFile(string filename, List<string> toadd)
        {
            try
            {
                
                string filepath = Path.Combine(basedirectory, filename);
                if (File.Exists(filepath))
                {

                    File.Delete(filepath);
                }
                if (!File.Exists(filepath))
                {
                    using (StreamWriter writer = File.CreateText(filepath))
                    {
                    }
                    UpdateFile(filename, toadd);
                }
                else
                {
                    using (StreamWriter writer = File.AppendText(filepath))
                    {
                        foreach(string item in toadd)
                        {
                            writer.WriteLine(toadd);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                WriteLine("Error: ", e.Message);

            }
        }
    }

}
