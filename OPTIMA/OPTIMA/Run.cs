using OPTIMA;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace OPTIMA
{
    using static Input;
    public class Run
    {
        static List<string> loggedacc;
        public static void OpenApp()
        {
            List<string> app_versions = new List<string> { "shOOPing Haven 3.0", "OPTIMA Dev. INC" };
            Menu<string> App_Choosing = new Menu<string>(" ", app_versions);
            Clear();
            string chosen_app = App_Choosing.Browse_Options();

            switch (App_Choosing.active_index + 1)
            {
                case 1:
                    MerchantorRegular();
                    break;
                case 2:
                    LoginorNewAcc("Admin");
                    break;
            }
        }

        public static void MerchantorRegular()
        {
            Clear();
            List<string> merchantorregular_list = new List<string> { "Merchant", "Regular" };
            Menu<string> merchantorregular = new Menu<string>(" ", merchantorregular_list);
            string merchantorregularchoice = merchantorregular.Browse_Options();
            LoginorNewAcc(merchantorregularchoice);
        }

        static void LoginorNewAcc(string acctype)
        {
            Clear();
            UserAccounts();
            List<string> loginornewacc_list = new List<string> { "Log-in here", "Create New Account" };
            Menu<string> loginornewacc_choice = new Menu<string>(" ", loginornewacc_list);
            string loginornewaccchoice = loginornewacc_choice.Browse_Options();
            switch (loginornewacc_choice.active_index + 1)
            {

                case 1:
                    Login(acctype);
                    break;
                case 2:
                    CreateAcc(acctype);
                    break;
            }
        }

        static void UserAccounts()
        {
            string basepath = @"C:\Users\Dawson\source\repos\OPTIMA\OPTIMA FILES";
            string filename = "UserAccounts.csv";
            string filepath = Path.Combine(basepath, filename);
            if (!File.Exists(filepath))
            {
                using (StreamWriter writer = File.CreateText(filepath))
                {
                    List<string> header = new List<string> { "Username", "Password", "Account Type",
                            "First Name", "Last Name", "Full Name", "Store Name", "Date Created" };
                    writer.WriteLine(string.Join(",", header));
                }
            }
        }

        //Start of Account Logging in
        static void Login(string acctype)
        {
            try
            {
                User user;
                Clear();

                SetCursorPosition((WindowWidth / 2) - 30, (WindowHeight / 2) - 3);
                Write(EnterUsername);
                int[] un_position = { CursorLeft, CursorTop };
                SetCursorPosition((WindowWidth / 2) - 30, (WindowHeight / 2) - 1);
                Write(EnterPassword);
                int[] pw_position = { CursorLeft, CursorTop };


                string username = String(un_position, EnterUsernameError);
                SetCursorPosition(pw_position[0], pw_position[1]);
                string password = GetMaskedPassword();
                Run verify = new Run();
                verify.AccountsFile(username, acctype);

                string filedirectory = @"C:\Users\Dawson\source\repos\OPTIMA\OPTIMA FILES";
                string filename = "LastLoggedIn";
                string filepath = Path.Combine(filedirectory, filename + ".txt");
                File.WriteAllLines(filepath, loggedacc);

                switch (acctype)
                {

                    case "Merchant":
                        user = new Merchant(loggedacc);
                        user.UI();
                        break;

                    case "Regular":
                        user = new Regular(loggedacc);
                        user.UI();
                        break;
                }


            }
            catch (Exception e)
            {
                WriteLine(e.Message);
                ReadKey(true);
                Login(acctype);
            }
        }

        public bool AccountsFile(string username, string acctype)
        {
            bool isExisting = false;
            try
            {
                string filedirectory = @"C:\Users\Dawson\source\repos\OPTIMA\OPTIMA FILES";
                string filename = "UserAccounts";
                string filepath = Path.Combine(filedirectory, filename + ".csv");


                if (!File.Exists(filepath))
                {
                    WriteLine("No accounts exist!");
                    return AccountsFile(username, acctype);
                }
                else
                {
                    string[] accounts = File.ReadAllLines(filepath);

                    foreach (string account in accounts)
                    {
                        string[] fields = account.Split(",");
                        if (fields[0].Trim() == username && fields[2].Trim() == acctype)
                        {

                            isExisting = true;
                            loggedacc = fields.ToList();
                            break;
                        }
                    }

                }
            }
            catch (IOException e)
            {

                WriteLine(e.Message);
                ReadKey(true);
                Login(acctype);
            }
            catch (Exception e)
            {

                WriteLine(e.Message);
                ReadKey(true);
                Login(acctype);
            }

            return isExisting;
        }

        //Start of Account Creation
        static void CreateAcc(string acctype)
        {
            Clear();
            SetCursorPosition((WindowWidth / 2) - 30, WindowHeight / 2 - 6);
            int starttop = CursorTop;
            int startleft = CursorLeft;
            int endtop = starttop;
            Write($"{EnterUsername} ");
            int username_top = starttop;
            endtop = username_top;
            SetCursorPosition(startleft, starttop + 3);
            Write($"{EnterPassword} ");
            int password_top = starttop + 3;
            endtop = password_top;
            SetCursorPosition(startleft, starttop + 6);
            Write($"Confirm Password: ");
            int confirmpassword_top = starttop + 6;
            endtop = confirmpassword_top;
            SetCursorPosition(startleft, starttop + 9);

            int firstname_top = 0;
            int lastname_top = 0;
            int storename_top = 0;
            if (acctype == "Regular")
            {

                Write($"Enter First Name: ");
                firstname_top = starttop + 9;
                endtop = firstname_top;
                SetCursorPosition(startleft, starttop + 12);
                Write($"Enter Last Name: ");
                lastname_top = starttop + 12;
                endtop = lastname_top;
            }
            else if (acctype == "Merchant")
            {

                Write("Enter Store Name: ");
                storename_top = starttop + 9;
            }

            try
            {
                string new_username;
                do
                {
                    SetCursorPosition(startleft + 25, username_top);
                    new_username = ReadLine();
                    List<string> user_details = ReadAccountsFile(new_username, acctype);
                    if (user_details == null || user_details.Count == 0)
                    {
                        break;
                    }
                    else if (new_username == ReadAccountsFile(new_username, acctype)[0])
                    {
                        SetCursorPosition(startleft + 25, username_top + 1);
                        WriteLine("Username already exists.");
                        ClearLine(startleft + 25, WindowWidth, username_top);
                        ReadLine();
                    }
                    else
                    {
                        break;
                    }
                } while (true);

                string new_password;
                do
                {
                    SetCursorPosition(startleft + 25, password_top);
                    new_password = GetMaskedPassword();
                    SetCursorPosition(startleft + 25, confirmpassword_top);
                    string confirmnew_password = GetMaskedPassword();
                    if (new_password != confirmnew_password)
                    {
                        SetCursorPosition(startleft + 25, confirmpassword_top + 1);
                        Write("Password does not match.");
                        ClearLine(startleft + 25, WindowWidth, password_top);
                        ClearLine(startleft + 25, WindowWidth, confirmpassword_top);

                    }
                    else
                    {
                        ClearLine(startleft + 25, WindowWidth, confirmpassword_top + 1);
                        break;
                    }
                } while (true);

                List<string> new_user_details = new List<string>() { "" };
                DateTime datecreated = DateTime.Now;
                string new_firstname;
                string new_lastname;
                string new_fullname = "";
                string new_storename;
                if (acctype == "Regular" || acctype == "Admin")
                {
                    do
                    {
                        SetCursorPosition(startleft + 25, firstname_top);
                        new_firstname = ReadLine();
                        SetCursorPosition(startleft + 25, lastname_top);
                        new_lastname = ReadLine();

                        bool ValidNames = true;
                        foreach (char letter in new_firstname + new_lastname)
                        {
                            if (!char.IsLetter(letter))
                            {
                                ValidNames = false;
                                SetCursorPosition(startleft + 25, lastname_top + 1);
                                Write("Names must only contain letters.");
                                ClearLine(startleft + 25, WindowWidth, firstname_top);
                                ReadLine();
                                ClearLine(startleft + 25, WindowWidth, lastname_top);
                                ReadLine();
                                break;

                            }
                        }
                        if (ValidNames)
                        {
                            new_firstname = char.ToUpper(new_firstname[0]) + new_firstname.Substring(1).ToLower();
                            new_lastname = char.ToUpper(new_lastname[0]) + new_lastname.Substring(1).ToLower();
                            new_fullname = new_firstname + " " + new_lastname;
                            break;
                        }
                        new_user_details = new List<string> { new_username, new_password, acctype, new_firstname, new_lastname, new_fullname, "-", datecreated.Date.ToString() };
                    } while (true);
                }
                else if (acctype == "Merchant")
                {

                    bool storenameExists = false;
                    do
                    {
                        SetCursorPosition(startleft + 25, storename_top);
                        new_storename = ReadLine();

                        string filedirectory = @"C:\Users\Dawson\source\repos\OPTIMA\OPTIMA FILES";
                        string filename = "UserAccounts.csv";
                        string filepath = Path.Combine(filedirectory, filename); ;
                        try
                        {
                            using (StreamReader reader = new StreamReader(filepath))
                            {
                                string line;
                                while ((line = reader.ReadLine()) != null)
                                {
                                    string[] fields = line.Split(',');
                                    if (fields[2] == new_storename)
                                    {
                                        SetCursorPosition(startleft + 25, storename_top + 1);
                                        WriteLine("Storename already exists.");
                                        ClearLine(startleft + 25, WindowWidth, storename_top);
                                        storenameExists = true;
                                        continue;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Failed to read the file: " + ex.Message);
                            CreateAcc(acctype);
                        }

                        new_user_details = new List<string> { new_username, new_password, acctype, "-", "-", "-", new_storename, datecreated.ToString() };
                    } while (storenameExists);
                }

                AddAccountsFile(new_user_details, acctype);

                string basepath = @"C:\Users\Dawson\source\repos\OPTIMA\OPTIMA FILES";
                string foldername = new_username;
                string folderpath = Path.Combine(basepath, foldername);
                Directory.CreateDirectory(folderpath);

                SetCursorPosition(WindowWidth / 2 - 13, endtop + 5);
                WriteLine("Account Successfully Created.");
                ReadKey(true);
                OpenApp();
            }
            catch (Exception ex)
            {
                WriteLine("Error: " + ex.Message);
                ReadKey(true);
                CreateAcc(acctype);
            }

        }

        public static List<string> ReadAccountsFile(string username, string acctype)
        {
            bool AccountExists = false;

            string basepath = @"C:\Users\Dawson\source\repos\OPTIMA\OPTIMA FILES";
            string filename = "UserAccounts.csv";
            string filepath = Path.Combine(basepath, filename);

            List<string> accounts = new List<string>();

            try
            {
                using (StreamReader reader = new StreamReader(filepath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] fields = line.Split(",");
                        if (fields[0] == username)
                        {
                            accounts = fields.ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to read the file: " + ex.Message);
                ReadKey(true);
                CreateAcc(acctype);
            }

            return accounts;
        }

        public static void AddAccountsFile(List<string> newuser_details, string acctype)
        {
            try
            {
                string basepath = @"C:\Users\Dawson\source\repos\OPTIMA\OPTIMA FILES";
                string filename = "UserAccounts.csv";
                string filepath = Path.Combine(basepath, filename);
                if (!File.Exists(filepath))
                {
                    using (StreamWriter writer = File.CreateText(filepath))
                    {
                        List<string> header = new List<string> { "Username", "Password", "Account Type",
                            "First Name", "Last Name", "Full Name", "Store Name", "Date Created" };
                        writer.WriteLine(string.Join(",", header));
                    }
                }
                else
                {
                    using (StreamWriter writer = File.AppendText(filepath))
                    {
                        writer.WriteLine(string.Join(",", newuser_details));
                    }
                }
            }
            catch (Exception e)
            {
                WriteLine("Error: " + e.Message);

                ReadKey(true);
                CreateAcc(acctype);
            }
        }

    }
    public class Input
    {
        public static string FormatText(int displaylength, string displaystr)
        {
            string shortstr;
            if (displaystr.Length >= displaylength - 3)
            {
                shortstr = displaystr.Substring(0, displaylength - 3) + "...";
                return shortstr;
            }
            else { return displaystr; }
        }
        public static string FormatLine(int displaylength, string displaystr)
        {
            string newstring = displaystr;
            for(int i = 0; i < displaystr.Length; i++)
            {
                if ((i+1) % displaylength == 0)
                {
                    newstring = displaystr.Substring(0, i) + "\n" + displaystr.Substring(i+1);
                }
            }
            return newstring;
        }
        public static string Header(string headerfor, string user, int displaylength)
        {
            string tabulated = "";
            if (headerfor == "Item" && user == "Merchant")
            {
                int space = displaylength * 1 / 4;
                tabulated = $"|{"Item Name".PadRight(space / 2).PadLeft(space)}|" +
                    $"{"Type".PadRight(space / 2).PadLeft(space)}|" +
                    $"{"Stock".PadRight(space / 2).PadLeft(space)}|" +
                    $"{"Price".PadRight(space / 2).PadLeft(space)}|";
            }
            else if (headerfor == "Item" && user == "Regular")
            {
                int space = displaylength * 1 / 5;
                tabulated = $"|{"Item Name".PadRight(space / 2).PadLeft(space)}|" +
                    $"{"Type".PadRight(space / 2).PadLeft(space)}|" +
                    $"{"Stock".PadRight(space / 2).PadLeft(space)}|" +
                    $"{"Price".PadRight(space / 2).PadLeft(space)}|" +
                    $"{"Store".PadRight(space / 2).PadLeft(space)}|";
            }
            else if (headerfor == "Purchased" && user == "Merchant")
            {
                int space = displaylength * 1 / 7;
                tabulated = $"|{"Item Name".PadRight(space / 2).PadLeft(space)}|" +
                    $"{"Type".PadRight(space / 2).PadLeft(space)}|" +
                    $"{"Price".PadRight(space / 2).PadLeft(space)}|" +
                    $"{"Quantity".PadRight(space/2).PadLeft(space)}|" +
                    $"{"Cost".PadRight(space / 2).PadLeft(space)}|" +
                    $"{"Date Purchased".PadRight(space / 2).PadLeft(space)}|" +
                    $"{"Buyer".PadRight(space / 2).PadLeft(space)}|";
            }
            else if(headerfor == "Purchased" &&  user == "Regular")
            {
                int space = displaylength * 1 / 7;
                tabulated = $"|{"Item Name".PadRight(space / 2).PadLeft(space)}|" +
                    $"{"Type".PadRight(space / 2).PadLeft(space)}|" +
                    $"{"Price".PadRight(space / 2).PadLeft(space)}|" +
                    $"{"Store".PadRight(space / 2).PadLeft(space)}|" +
                    $"{"Quantity".PadRight(space / 2).PadLeft(space)}|" +
                    $"{"Cost".PadRight(space / 2).PadLeft(space)}|" +
                    $"{"Date Purchased".PadRight(space / 2).PadLeft(space)}|";
            }

            return tabulated;
        }
        public static string ItemTable(Item item, int displaylength, string user)
        {
            string tabulated = item.itemstr;
            if(user == "Merchant")
            {
                int space = displaylength * 1 / 4;
                tabulated = $"|{FormatText(displaylength - 5, item.name).PadRight(space / 2).PadLeft(space)}|" +
                    $"{FormatText(displaylength - 5, item.type).PadRight(space / 2).PadLeft(space)}|" +
                    $"{item.stock.ToString().PadRight(space / 2).PadLeft(space)}|" +
                    $"{item.price.ToString().PadRight(space / 2).PadLeft(space)}|";
            }
            else if(user == "Regular")
            {
                int space = displaylength * 1 / 5;
                tabulated = $"|{FormatText(displaylength - 5, item.name).PadRight(space / 2).PadLeft(space)}|" +
                    $"{FormatText(displaylength - 5, item.type).PadRight(space / 2).PadLeft(space)}|" +
                    $"{item.stock.ToString().PadRight(space / 2).PadLeft(space)}|" +
                    $"{item.price.ToString().PadRight(space / 2).PadLeft(space)}|" +
                    $"{FormatText(displaylength - 5, item.seller).PadRight(space / 2).PadLeft(space)}|";
            }
            return tabulated;
        }
        public static string OrderTable(Purchased order, int displaylength, string user)
        {
            string tabulated = order.itemstr;
            if (user == "Merchant")
            {
                int space = displaylength * 1 / 7;
                tabulated = $"|{FormatText(displaylength - 5, order.item.name).PadRight(space / 2).PadLeft(space)}|" +
                    $"{FormatText(displaylength - 5, order.item.type).PadRight(space / 2).PadLeft(space)}|" +
                    $"{order.item.price.ToString().PadRight(space / 2).PadLeft(space)}|" +
                    $"{order.quantity.ToString().PadRight(space / 2).PadLeft(space)}|" +
                    $"{order.cost.ToString().PadRight(space / 2).PadLeft(space)}|" +
                    $"{order.purchasedon.ToString().PadRight(space / 2).PadLeft(space)}|" +
                    $"{FormatText(displaylength - 5, order.buyer).PadRight(space / 2).PadLeft(space)}|";
            }
            else if (user == "Regular")
            {
                int space = displaylength * 1 / 7;
                tabulated = $"|{FormatText(displaylength - 5, order.item.name).PadRight(space / 2).PadLeft(space)}|" +
                    $"{FormatText(displaylength - 5, order.item.type).PadRight(space / 2).PadLeft(space)}|" +
                    $"{order.item.price.ToString().PadRight(space / 2).PadLeft(space)}|" +
                    $"{order.quantity.ToString().PadRight(space / 2).PadLeft(space)}|" +
                    $"{FormatText(displaylength - 5, order.item.seller).PadRight(space / 2).PadLeft(space)}|" +
                    $"{order.cost.ToString().PadRight(space / 2).PadLeft(space)}|" +
                    $"{order.purchasedon.ToString().PadRight(space / 2).PadLeft(space)}|";
            }
            return tabulated;
        }

        public static void InputPrompt(int[] cursorposition, string inputPrompt, string errorPrompt)
        {
            try
            {
                SetCursorPosition(cursorposition[0], cursorposition[1]);
                Write(inputPrompt);
            }
            catch (Exception e)
            {
                SetCursorPosition(cursorposition[0], cursorposition[1] + 1);
                WriteLine(errorPrompt + " " + e.Message);
            }
        }

        public static string String(int[] cursorposition, string errorPrompt)
        {
            try
            {
                SetCursorPosition(cursorposition[0], cursorposition[1]);
                string input = Console.ReadLine();
                return input;
            }
            catch (ArgumentNullException e)
            {
                SetCursorPosition(cursorposition[0], cursorposition[1] + 1);
                WriteLine(errorPrompt + " " + e.Message);
                return String(cursorposition, errorPrompt);
            }
            catch (FormatException e)
            {
                SetCursorPosition(cursorposition[0], cursorposition[1] + 1);
                WriteLine(errorPrompt + " " + e.Message);
                return String(cursorposition, errorPrompt);
            }
            catch (Exception e)
            {
                SetCursorPosition(cursorposition[0], cursorposition[1] + 1);
                WriteLine(errorPrompt + " " + e.Message);
                return String(cursorposition, errorPrompt);
            }
        }
        public static int Int(int[] cursorposition, string errorPrompt)
        {
            try
            {
                SetCursorPosition(cursorposition[0], cursorposition[1]);
                int.TryParse(Console.ReadLine(), out int input);
                return input;
            }
            catch (ArgumentNullException e)
            {
                SetCursorPosition(cursorposition[0], cursorposition[1] + 1);
                WriteLine(errorPrompt + " " + e.Message);
                return Int(cursorposition, errorPrompt);
            }
            catch (FormatException e)
            {
                SetCursorPosition(cursorposition[0], cursorposition[1] + 1);
                WriteLine(errorPrompt + " " + e.Message);
                return Int(cursorposition, errorPrompt);
            }
            catch (Exception e)
            {
                SetCursorPosition(cursorposition[0], cursorposition[1]);
                WriteLine(errorPrompt + " " + e.Message);
                return Int(cursorposition, errorPrompt);
            }
        }
        public static double Double(int[] cursorposition, string errorPrompt)
        {
            try
            {
                SetCursorPosition(cursorposition[0], cursorposition[1]);
                double.TryParse(Console.ReadLine(), out double input);
                return input;
            }
            catch (ArgumentNullException e)
            {
                SetCursorPosition(cursorposition[0], cursorposition[1] + 1);
                WriteLine(errorPrompt + " " + e.Message);
                return Double(cursorposition, errorPrompt);
            }
            catch (FormatException e)
            {
                SetCursorPosition(cursorposition[0], cursorposition[1] + 1);
                WriteLine(errorPrompt + " " + e.Message);
                return Double(cursorposition, errorPrompt);
            }
            catch (Exception e)
            {
                SetCursorPosition(cursorposition[0], cursorposition[1] + 1);
                WriteLine(errorPrompt + " " + e.Message);
                return Double(cursorposition, errorPrompt);
            }
        }
        public static string GetMaskedPassword()
        {
            string masked_input = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true); // Read a key without displaying it on the screen

                // Check if the key is a printable character or a backspace
                if (char.IsLetterOrDigit(key.KeyChar) || char.IsSymbol(key.KeyChar) || char.IsPunctuation(key.KeyChar))
                {
                    masked_input += key.KeyChar;
                    Console.Write("*"); // Display an asterisk for each character
                }
                else if (key.Key == ConsoleKey.Backspace && masked_input.Length > 0)
                {
                    masked_input = masked_input.Substring(0, masked_input.Length - 1);
                    Console.Write("\b \b"); // Move the cursor back and overwrite the character with a space
                }

            } while (key.Key != ConsoleKey.Enter);

            return masked_input;
        }
        static public void ClearLine(int start_left, int end_right, int start_top)
        {
            // Set cursor position to the beginning of the line
            Console.SetCursorPosition(start_left, start_top);

            // Overwrite the line with spaces
            Console.Write(new string(' ', end_right));

            // Set the cursor position back to the beginning of the line
            Console.SetCursorPosition(start_left, start_top);
        }
        static public void ClearLine(int start_left, int end_right, int start_top, int end_bottom)
        {
            for (int i = start_top; i <= end_bottom; i++)
            {
                // Set cursor position to the beginning of the line
                Console.SetCursorPosition(start_left, i);

                // Overwrite the line with spaces
                Console.Write(new string(' ', end_right));
            }

            // Set the cursor position back to the beginning of the line
            Console.SetCursorPosition(start_left, start_top);
        }

        static public string LoginChoice = "Enter your choice for login: ";
        static public string LoginChoiceError = "Wrong choice. Please try again for login. ";
        static public string EnterUsername = "Enter username: ";
        static public string EnterUsernameError = "Invalid username. Please try again. ";
        static public string EnterPassword = "Enter password: ";
        static public string EnterPasswordError = "Invalid password. Please try again. ";
        static public string UIChoiceError = "Wrong choice! Try again. ";
        static public string UIChoice = "Enter your choice: ";
        static public string Quantity = "Enter quantity: ";
        static public string QuantityError = "Invalid quantity! Try again. ";
        static public string Price = "Enter price: ";
        static public string PriceError = "Invalid price! Try again. ";
        static public string ItemChoice = "What you want to buy: ";
        static public string ItemChoiceError = "Invalid item! Try again. ";
        static public string ViewCart = "Enter C to view cart: ";
        static public string ItemNotFound = "Item not found! Try again. ";
    }
    interface IScrollable
    {
        public string Browse_Options();
    }
    public class Menu<T> : IScrollable
    {
        //Input_Prompt input = new Input_Prompt();
        private int Active_Index;
        private int Active_Item;
        private int Active_Category;
        public List<string> OptionsList;
        public List<Item> ItemList;
        public List<Purchased> OrderList;
        public List<string> CategoryList;
        private string Prompt;

        public Menu(string prompt, List<string> options)
        {
            Prompt = prompt;
            OptionsList = options;
            Active_Index = 0;
        }
        public Menu(string prompt, List<Item> options1, List<string> options2)
        {
            Prompt = prompt;
            ItemList = options1;
            CategoryList = options2;
            Active_Item = 0;
            Active_Category = 0;
        }
        public Menu(string prompt, List<Purchased> options1, List<string> options2)
        {
            Prompt = prompt;
            OrderList = options1;
            CategoryList = options2;
            Active_Item = 0;
            Active_Category = 0;
        }

        public int active_index
        {
            get { return Active_Index; }
            set { Active_Index = value; }
        }
        public int active_item
        {
            get { return Active_Item; }
            set { Active_Item = value; }
        }
        public int active_category
        {
            get { return Active_Category; }
            set { Active_Category = value; }
        }

        //Main menu
        public void Display_Options()
        {
            Clear();
            CursorTop = (WindowHeight / 2) - (OptionsList.Count / 2);
            CursorLeft = (WindowWidth / 2) - (Prompt.Length / 2);
            WriteLine(Prompt);
            for (int i = 0; i < OptionsList.Count; i++)
            {
                CursorLeft = (WindowWidth / 2) - (OptionsList[i].Length / 2);
                object current_option = OptionsList[i];

                if (i == Active_Index)
                {
                    ForegroundColor = ConsoleColor.White;
                    BackgroundColor = ConsoleColor.Red;
                    Write(">");
                }
                else
                {
                    Write(" ");
                }
                WriteLine($"> {current_option} \n");
                ResetColor();
            }
        }

        public string Browse_Options()
        {

            CursorVisible = false;
            ConsoleKey keyPressed;
            int menu_left = CursorLeft;
            int menu_top = CursorTop;
            do
            {
                SetCursorPosition(menu_left, menu_top);
                Display_Options();
                ConsoleKeyInfo keyInfo = ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    Active_Index--;
                    if (Active_Index == -1)
                    {
                        Active_Index = OptionsList.Count - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    Active_Index++;
                    if (Active_Index == OptionsList.Count)
                    {
                        Active_Index = 0;
                    }
                }

            } while (keyPressed != ConsoleKey.Enter && keyPressed != ConsoleKey.Escape);

            CursorVisible = true;
            if (keyPressed == ConsoleKey.Enter)
            {
                return OptionsList[active_index];
            }
            else if (keyPressed == ConsoleKey.Escape)
            {
                return "Back";
            }
            else { return Browse_Options(); }
        }

        public void Display_Options(int cursortop, int cursorleft)
        {
            Clear();
            CursorTop = cursortop;

            CursorLeft = cursorleft;
            WriteLine(Prompt);
            for (int i = 0; i < OptionsList.Count; i++)
            {
                CursorLeft = cursorleft;
                object current_option = OptionsList[i];

                if (i == Active_Index)
                {
                    ForegroundColor = ConsoleColor.White;
                    BackgroundColor = ConsoleColor.Red;
                    Write(">");
                }
                else
                {
                    Write(" ");
                }
                WriteLine($"> {current_option} \n");
                ResetColor();
            }
        }

        public string Browse_Options(int cursortop, int cursorleft)
        {

            CursorVisible = false;
            ConsoleKey keyPressed;
            int menu_left = cursortop;
            int menu_top = cursorleft;
            do
            {
                SetCursorPosition(menu_left, menu_top);
                Display_Options(cursortop, cursorleft);
                ConsoleKeyInfo keyInfo = ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    Active_Index--;
                    if (Active_Index == -1)
                    {
                        Active_Index = OptionsList.Count - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    Active_Index++;
                    if (Active_Index == OptionsList.Count)
                    {
                        Active_Index = 0;
                    }
                }

            } while (keyPressed != ConsoleKey.Enter && keyPressed != ConsoleKey.Escape);

            CursorVisible = true;
            if (keyPressed == ConsoleKey.Enter)
            {
                return OptionsList[active_index];
            }
            else
            {
                return "Back";
            }
        }

        public void DisplayItems(int cursortop, int cursorleft, int linelength, string user)
        {
            CursorTop = cursortop;
            WriteLine(Prompt);
            string type = null;
            List<string> itemdisplay = new List<string>();
            itemdisplay = ItemList.Select(x => ItemTable(x, linelength, user)).ToList();
            type = "Item";
            Header(type, user, linelength);
            for (int i = 0; i < ItemList.Count; i++)
            {
                CursorLeft = cursorleft;
                object current_option = itemdisplay[i];

                if (i == Active_Item)
                {
                    ForegroundColor = ConsoleColor.White;
                    BackgroundColor = ConsoleColor.Red;
                    Write(">");
                }
                else
                {
                    Write(" ");
                }
                WriteLine($"> {current_option}");
                ResetColor();
            }
        }

        public Item BrowseItems(int cursortop, int cursorleft, int linelength, string user, Action ToFunction)
        {

            CursorVisible = false;
            ConsoleKey keyPressed;
            int menu_left = cursortop;
            int menu_top = cursorleft;
            do
            {
                SetCursorPosition(menu_left, menu_top);
                DisplayItems(cursortop, cursorleft, linelength, user);
                ConsoleKeyInfo keyInfo = ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    Active_Item--;
                    if (Active_Item == -1)
                    {
                        Active_Item = ItemList.Count - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    Active_Item++;
                    if (Active_Item == ItemList.Count)
                    {
                        Active_Item = 0;
                    }
                }

            } while (keyPressed != ConsoleKey.Enter && keyPressed != ConsoleKey.Escape);

            CursorVisible = true;
            if (keyPressed == ConsoleKey.Escape)
            {
                ToFunction();
            }
            return (keyPressed == ConsoleKey.Escape) ? null : ItemList[Active_Item];
        }
        public void DisplayOrders(int cursortop, int cursorleft, int linelength, string user)
        {
            CursorTop = cursortop;
            WriteLine(Prompt);
            string type = null;
            List<string> itemdisplay = new List<string>();
            itemdisplay = OrderList.Select(x => OrderTable(x, linelength, user)).ToList();
            type = "Purchased";

            Header(type, user, linelength);
            for (int i = 0; i < ItemList.Count; i++)
            {
                CursorLeft = cursorleft;
                object current_option = itemdisplay[i];

                if (i == Active_Item)
                {
                    ForegroundColor = ConsoleColor.White;
                    BackgroundColor = ConsoleColor.Red;
                    Write(">");
                }
                else
                {
                    Write(" ");
                }
                WriteLine($"> {current_option}");
                ResetColor();
            }
        }

        public Purchased BrowseOrders(int cursortop, int cursorleft, int linelength, string user, Action ToFunction)
        {

            CursorVisible = false;
            ConsoleKey keyPressed;
            int menu_left = cursortop;
            int menu_top = cursorleft;
            do
            {
                SetCursorPosition(menu_left, menu_top);
                DisplayItems(cursortop, cursorleft, linelength, user);
                ConsoleKeyInfo keyInfo = ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    Active_Item--;
                    if (Active_Item == -1)
                    {
                        Active_Item = OrderList.Count - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    Active_Item++;
                    if (Active_Item == OrderList.Count)
                    {
                        Active_Item = 0;
                    }
                }

            } while (keyPressed != ConsoleKey.Enter && keyPressed != ConsoleKey.Escape);

            CursorVisible = true;
            if (keyPressed == ConsoleKey.Escape)
            {
                ToFunction();
            }
            return (keyPressed == ConsoleKey.Escape) ? null : OrderList[Active_Item];
        }
        public void DisplayCategories(int cursortop, int cursorleft, int linelength)
        {
            Clear();
            CursorTop = cursortop;
            CursorLeft = cursorleft;
            WriteLine(Prompt);
            for (int i = 0; i < CategoryList.Count; i++)
            {
                string current_option = CategoryList[i];

                if (i == Active_Category)
                {
                    ForegroundColor = ConsoleColor.White;
                    BackgroundColor = ConsoleColor.Red;
                    Write(">");
                }
                else
                {
                    Write(" ");
                }
                int categoryspace = linelength / CategoryList.Count;
                Write($"{current_option.PadRight(categoryspace / 2).PadLeft(categoryspace)}");
                ResetColor();
            }
        }

        public string BrowseCategories(int cursortop, int cursorleft, int linelength)
        {

            CursorVisible = false;
            ConsoleKey keyPressed;
            int menu_left = CursorLeft;
            int menu_top = CursorTop;
            do
            {
                SetCursorPosition(menu_left, menu_top);
                DisplayCategories(cursortop, cursorleft, linelength);
                ConsoleKeyInfo keyInfo = ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.LeftArrow)
                {
                    Active_Category--;
                    if (Active_Category == -1)
                    {
                        Active_Category = CategoryList.Count - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.RightArrow)
                {
                    Active_Category++;
                    if (Active_Category == CategoryList.Count)
                    {
                        Active_Category = 0;
                    }
                }

            } while (keyPressed != ConsoleKey.Enter && keyPressed != ConsoleKey.Escape);

            CursorVisible = true;
            if (keyPressed == ConsoleKey.Enter)
            {
                return CategoryList[active_category];
            }
            else if (keyPressed == ConsoleKey.Escape)
            {
                return "Back";
            }
            else { return Browse_Options(); }
        }
    }
}
