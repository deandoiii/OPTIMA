using OPTIMA;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using static System.Console;
namespace OPTIMA
{
    using static Input;
    interface IStatistics
    {
        public void GenerateReport();
    }
    public class Merchant : User, IStatistics
    {
        public List<Item> inventory { get; set; }
        List<Purchased> orders { get; set; }
        List<Item> searchlist = new List<Item>();
        List<string> searchcategories = new List<string>();

        public Merchant() { }

        public Merchant(List<string> userdetails) : base(userdetails)
        {
            inventory = allitems.Where(x => x.seller.Contains(storename)).OrderBy(x => x.name).ToList();
            orders = allorders.Where(x => x.item.seller.Contains(storename)).OrderBy(x => x.item.name).ToList();
        }
        public override void UI()
        {
            try
            {
                Clear();
                SetCursorPosition(5, WindowHeight / 2 - 2);
                List<string> merchantmenu = new List<string> { "Sales Analytics", "Manage Products",
                "View Orders", "Log-Out" };
                Menu<string> menu = new Menu<string>("", merchantmenu);
                string choice = menu.Browse_Options();
                switch (menu.active_index + 1)
                {
                    case 1:
                        SalesAnalytics();
                        break;
                    case 2:
                        ManageProducts();
                        break;
                    case 3:
                        ViewOrders();
                        break;
                    case 4:
                        Logout();
                        break;
                }
            }
            catch(Exception e)
            {
                WriteLine(e.Message);
                ReadKey(true);
                UI();
            }
        }

        //Start of Sales Analytics
        public void GenerateReport()
        {

        }
        public void SalesAnalytics()
        {
            try
            {
                Clear();
                SetCursorPosition(5, WindowHeight / 2 - 2);
                List<string> merchantmenu = new List<string> { "Revenue", "Sales Volume",
                "Average Transaction Value", "Back" };
                Menu<string> menu = new Menu<string>("Menu: ", merchantmenu);
                string choice = menu.Browse_Options();
                switch (menu.active_index + 1)
                {
                    case 1:
                        Revenue();
                        break;
                    case 2:
                        SalesVolume();
                        break;
                    case 3:
                        AverageTransaction();
                        break;
                    case 4:
                        UI();
                        break;
                }
            }
            catch(Exception e)
            {
                WriteLine(e.Message);
                ReadKey(true);
                SalesAnalytics();
            }
        }

        public void Revenue()
        {
            //Clear();
            int displaystart_left = WindowWidth * 1/4;
            int displaystart_top = WindowHeight * 1/4;
            //ClearLine(displaystart_left + 5, WindowWidth - displaystart_left*2, 1, WindowHeight);
            SetCursorPosition(displaystart_left + 5, WindowHeight / 2 - 3);
            List<string> timeperiod = new List<string>() { "Day", "Week", "Month", "Quarter", "Year", "Back" };
            Menu<string> timemenu = new Menu<string>("Revenue by: ", timeperiod);
            string timechoice = timemenu.Browse_Options();
            //ClearLine(displaystart_left * 2, WindowWidth, 1, BufferHeight);

            List<Tuple<int, DateTime, double>> datalist = new List<Tuple<int, DateTime, double>>();
            DateTime datetoday = DateTime.Now;
            Clear();
            switch (timechoice)
            {
                case "Day":
                    InputPrompt(new int[] { displaystart_left + 10, displaystart_top}, "Revenue for the last 7 days: ", "");
                    for(int i = 0; i < 7; i++)
                    {
                        int daynumber = i;
                        DateTime currentdate = datetoday.AddDays(-i);
                        double revenue = orders.Where(x => x.purchasedon == currentdate).Select(x => x.cost).Sum();
                        datalist.Add(Tuple.Create(daynumber,  currentdate, revenue));
                        
                        InputPrompt(new int[] { (displaystart_left) + 10, displaystart_top + (2*i) }, datetoday.ToLongDateString() + ": " + revenue, "");
                    }
                    break;
                case "Week":
                    InputPrompt(new int[] { displaystart_left + 10, displaystart_top }, "Revenue for the last 7 days: ", "");
                    for (int i = 0; i < 7; i++)
                    {
                        int daynumber = i;
                        DateTime currentdate = datetoday.AddDays(-i);
                        double revenue = orders.Where(x => x.purchasedon == currentdate).Select(x => x.cost).Sum();
                        datalist.Add(Tuple.Create(daynumber, currentdate, revenue));

                        InputPrompt(new int[] { (displaystart_left) + 10, displaystart_top + (2 * i) }, datetoday.ToLongDateString() + ": " + revenue, "");
                    }
                    break;
                case "Month":
                    InputPrompt(new int[] { displaystart_left + 10, displaystart_top }, "Revenue for the last 7 days: ", "");
                    for (int i = 0; i < 7; i++)
                    {
                        int daynumber = i;
                        DateTime currentdate = datetoday.AddDays(-i);
                        double revenue = orders.Where(x => x.purchasedon == currentdate).Select(x => x.cost).Sum();
                        datalist.Add(Tuple.Create(daynumber, currentdate, revenue));

                        InputPrompt(new int[] { (displaystart_left) + 10, displaystart_top + (2 * i) }, datetoday.ToLongDateString() + ": " + revenue, "");
                    }
                    break;
                case "Quarter":
                    InputPrompt(new int[] { displaystart_left + 10, displaystart_top }, "Revenue for the last 7 days: ", "");
                    for (int i = 0; i < 7; i++)
                    {
                        int daynumber = i;
                        DateTime currentdate = datetoday.AddDays(-i);
                        double revenue = orders.Where(x => x.purchasedon == currentdate).Select(x => x.cost).Sum();
                        datalist.Add(Tuple.Create(daynumber, currentdate, revenue));

                        InputPrompt(new int[] { (displaystart_left) + 10, displaystart_top + (2 * i) }, datetoday.ToLongDateString() + ": " + revenue, "");
                    }
                    break;
                case "Back":
                    SalesAnalytics();
                    break;
            }
            SalesAnalytics();
        }
        public void SalesVolume()
        {
            List<string> timeperiod = new List<string>() { "Day", "Week", "Month", "Quarter", "Year" };
            Menu<string> timemenu = new Menu<string>("Items sold by: ", timeperiod);
        }
        public void AverageTransaction()
        {
            List<string> timeperiod = new List<string>() { "Day", "Week", "Month", "Quarter", "Year" };
            Menu<string> timemenu = new Menu<string>("Average transaction by: ", timeperiod);
        }
        //Start of Inventory Management
        public void ManageProducts()
        {
            Clear();
            int displaylength = (WindowWidth * 1 / 5);
            List<string> merchantmenu = new List<string> { "Browse", "Search", "Sort/Filter", "Add", "Back" };
            Menu<string> menu = new Menu<string>("Menu: ", merchantmenu);
            string choice = menu.Browse_Options();
            switch (menu.active_index + 1)
            {
                case 1:
                    BrowseInventory();
                    break;
                case 2:
                    SearchItem();
                    break;
                case 3:
                    SortFilter();
                    break;
                case 4:
                    AddItem();
                    break;
                case 5:
                    UI();
                    break;
            }
        }



        //Inventory Case 1
        public void BrowseInventory()
        {
            Clear();
            int displaystart_left = 0;
            int displaystart_top = 0;
            int displaylength = (WindowWidth - 5) - displaystart_left;

            List<Item> inventorylist = inventory;
            List<string> inventorycategories = new List<string> { "Item", "Type" };
            Menu<Item> browsemenu = new Menu<Item>("", inventorylist, inventorycategories);

            string searchchoice = browsemenu.BrowseCategories(displaystart_top + 3, displaystart_left, displaylength);
            Item itemchoice = new Item();
            switch (searchchoice)
            {
                case "Item":
                    searchlist = inventory.OrderBy(x => x.name).ToList();
                    browsemenu.ItemList = searchlist;
                    itemchoice = browsemenu.BrowseItems(displaystart_top + 6, displaystart_left, displaylength, "Merchant", BrowseInventory);
                    break;
                case "Type":
                    searchlist = inventory.OrderBy(x => x.type).ToList();
                    browsemenu.ItemList = searchlist;
                    itemchoice = browsemenu.BrowseItems(displaystart_top + 6, displaystart_left, displaylength, "Merchant", BrowseInventory);
                    break;
                case "Back":
                    ManageProducts();
                    break;
                default:
                    break;

            }
            Clear();
            if (itemchoice != null)
            {
                ItemOptions(itemchoice);
            }
            else
            {
                ManageProducts();
            }
        }
        public void SearchItem()
        {
            try
            {
                int displaystart_left = 0;
                int displaystart_top = 0;
                int displaylength = (WindowWidth - 5) - displaystart_left;

                string searchfor = "";

                searchlist = inventory;
                searchcategories = new List<string> { "All", "Keyword", "Item", "Type" };
                Menu<Item> searchbrowse = new Menu<Item>("", searchlist, searchcategories);
                searchbrowse.Display_Options();
                searchbrowse.DisplayItems(displaystart_top + 6, displaystart_left, displaylength, "Merchant");

                ConsoleKey keyPressed;
                InputPrompt(new int[] { displaystart_left, displaystart_top }, "Search here: ", "");
                do
                {
                    ClearLine(displaystart_left, displaystart_left + displaylength,
                        displaystart_top, searchlist.Count + displaystart_top);
                    ConsoleKeyInfo keyInfo = ReadKey(false);
                    keyPressed = keyInfo.Key;

                    //Verify pleaseee
                    searchfor += keyPressed;

                    searchlist = inventory.Where(x => x.itemstr.Contains(searchfor)).
                        OrderBy(x => x.name.Contains(searchfor)).
                        ThenBy(x => x.type.Contains(searchfor)).ToList();

                } while (keyPressed != ConsoleKey.Enter && keyPressed != ConsoleKey.Escape);

                if (keyPressed == ConsoleKey.Escape)
                {
                    ManageProducts();
                }
                else if (keyPressed == ConsoleKey.Enter)
                {
                    var searchchoice = searchbrowse.Browse_Options();
                    Item itemchoice = null;
                    switch (searchchoice)
                    {
                        case "All":
                            searchlist = inventory.OrderBy(x => x.name).ToList();
                            searchbrowse.ItemList = searchlist;
                            itemchoice = searchbrowse.BrowseItems(displaystart_top + 6, displaystart_left, displaylength, "Merchant", SearchItem);
                            break;
                        case "Keyword":
                            searchlist = inventory.Where(x => x.itemstr.Contains(searchfor)).
                                OrderBy(x => x.name.Contains(searchfor)).ThenBy(x => x.type.Contains(searchfor)).
                                ThenBy(x => x.seller.Contains(searchfor)).ToList();
                            searchbrowse.ItemList = searchlist;
                            itemchoice = searchbrowse.BrowseItems(displaystart_top + 6, displaystart_left, displaylength, "Merchant", SearchItem);
                            break;
                        case "Item":
                            searchlist = inventory.Where(x => x.name.Contains(searchfor)).
                             OrderBy(x => x.name).ToList();
                            searchbrowse.ItemList = searchlist;
                            itemchoice = searchbrowse.BrowseItems(displaystart_top + 6, displaystart_left, displaylength, "Merchant", SearchItem);
                            break;
                        case "Type":
                            searchlist = inventory.Where(x => x.type.Contains(searchfor)).
                             OrderBy(x => x.type).ToList();
                            searchbrowse.ItemList = searchlist;
                            itemchoice = searchbrowse.BrowseItems(displaystart_top + 6, displaystart_left, displaylength, "Merchant", SearchItem);
                            break;
                        case "Back":
                            ManageProducts();
                            break;
                        default:
                            break;
                    }
                    Clear(); 
                    if (itemchoice != (Item)(object)"Back")
                    {
                        ItemOptions(itemchoice);
                    }
                    else
                    {
                        ManageProducts();
                    }

                }
            }
            catch (Exception e)
            {
                WriteLine("Error: " + e.Message);
                SearchItem();
            }

        }

        public int ItemIndex(Item toget)
        {
            foreach (Item item in allitems)
            {
                if (toget.itemcode == item.itemcode)
                    return allitems.IndexOf(item);
            }
            return 0;
        }
        public void ItemDetails(Item choiceinfo)
        {
            int displaystart_top = (WindowHeight / 2) - 5;
            int displaystart_left = (WindowWidth/2) - 20;
            int displaylength = WindowWidth *2/3;

            InputPrompt(new int[] { displaystart_left, displaystart_top }, 
                "Item Name: " + FormatLine(displaylength, choiceinfo.name) + "\n", "");
            InputPrompt(new int[] { displaystart_left, displaystart_top+3 },
                "Item Type: " + FormatLine(displaylength, choiceinfo.type) + "\n", "");
            InputPrompt(new int[] { displaystart_left, displaystart_top+6 },
                "Item Stock: " + choiceinfo.stock + "\n", "");
            InputPrompt(new int[] { displaystart_left, displaystart_top+9 },
                "Item Price: " + choiceinfo.price + "\n", "");
        }
        public void ItemDetails(Item choiceinfo, int displaystart_left, int displaystart_top)
        {
            
            int displaylength = WindowWidth * 2 / 3;

            InputPrompt(new int[] { displaystart_left, displaystart_top },
                "Item Name: " + FormatLine(displaylength, choiceinfo.name) + "\n", "");
            InputPrompt(new int[] { displaystart_left, displaystart_top + 3 },
                "Item Type: " + FormatLine(displaylength, choiceinfo.type) + "\n", "");
            InputPrompt(new int[] { displaystart_left, displaystart_top + 6 },
                "Item Stock: " + choiceinfo.stock + "\n", "");
            InputPrompt(new int[] { displaystart_left, displaystart_top + 9 },
                "Item Price: " + choiceinfo.price + "\n", "");
        }

        public void ItemOptions(Item itemchoice)
        {
            
            Clear();
            ItemDetails(itemchoice);                
            ReadKey(true);
            List<string> merchantmenu = new List<string> { "Edit", "Delete", "Back" };
            Menu<string> menu = new Menu<string>("Menu: ", merchantmenu);
            string choice = menu.Browse_Options();
            switch (menu.active_index + 1)
            {
                case 1:
                    EditItem(itemchoice);
                    break;
                case 2:
                    DeleteItem(itemchoice);
                    break;
                case 3:
                    ManageProducts();
                    break;
            }
        }

        //Inventory Case 2
        public void SortFilter()
        {
            try
            {
                int displaystart_left = (WindowWidth * 1 / 5);
                int displaystart_top = 10;
                int displaylength = (WindowWidth - 5) - displaystart_left;

                ClearLine((WindowWidth * 1 / 5), WindowWidth, displaystart_top, inventory.Count + displaystart_top);

                List<string> plusorminus = new List<string>() { "Ascending ", "Descending"};
                Menu<string> plusminus = new Menu<string>("", plusorminus);
                string plusminuschoice = plusminus.Browse_Options(displaystart_top, displaystart_left * 2);


                List<string> statuslist = new List<string>() { "Adequate", "Critical" };
                Menu<string> status = new Menu<string>("", statuslist);
                string statuschoice = status.Browse_Options(displaystart_top, displaystart_left * 4);

                List<Item> sortlist = inventory;
                if(plusminuschoice == "Ascending" && statuschoice == "Adequate")
                    sortlist = inventory.Where(x => x.stock >= 3).OrderBy(x => x.stock).ToList();
                else if (plusminuschoice == "Ascending" && statuschoice == "Critical")
                    sortlist = inventory.Where(x => x.stock <= 3).OrderBy(x => x.stock).ToList();
                else if (plusminuschoice == "Descending" && statuschoice == "Adequate")
                    sortlist = inventory.Where(x => x.stock >= 3).OrderByDescending(x => x.stock).ToList();
                else if (plusminuschoice == "Descending" && statuschoice == "Critical")
                    sortlist = inventory.Where(x => x.stock <= 3).OrderByDescending(x => x.stock).ToList();

                Menu<Item> sortmenu = new Menu<Item>("", sortlist, statuslist);
                Item itemchoice = sortmenu.BrowseItems(displaystart_top + 5, displaystart_left, displaylength, "Merchant", SortFilter);
                Clear();
                if (itemchoice != (Item)(object)"Back")
                    {
                    ItemOptions(itemchoice);
                }
                else
                {
                    ManageProducts();
                }

            }
            catch (Exception e)
            {
                WriteLine("Error: ", e.Message);
                SortFilter();
            }
        }
        //Inventory Case 2
        public void AddItem()
        {
            Clear();
            //Enter new item details
            int displaystart_left = (WindowWidth * 2 / 5) ;
            int displaystart_top = 10;
            int displaylength = (WindowWidth - 5) - displaystart_left;
            int[] cursorposition = new int[] { displaystart_left, displaystart_top };
            ClearLine((WindowWidth * 1 / 5), WindowWidth, displaystart_top, inventory.Count + displaystart_top);

            InputPrompt(cursorposition, "New Item", "");
            InputPrompt(new int[] { displaystart_left, displaystart_top + 2 }, "Name: ", "");
            InputPrompt(new int[] { displaystart_left, displaystart_top + 4 }, "Type: ", "");
            InputPrompt(new int[] { displaystart_left, displaystart_top + 6 }, "Stock: ", "");
            InputPrompt(new int[] { displaystart_left, displaystart_top + 8 }, "Price: ", "");

            displaystart_left += 10;
            string newname = String(new int[] { displaystart_left, displaystart_top + 2 }, "");
            string itemtype = String(new int[] { displaystart_left, displaystart_top + 4 }, "");
            int itemstock = Int(new int[] { displaystart_left, displaystart_top + 6 }, "");
            double itemprice = Double(new int[] { displaystart_left, displaystart_top + 8 }, "");
            int itemcode = (inventory.Any()) ? inventory.Max(x => x.itemcode) + 1 : 1;
            string itemseller = storename;

            string[] itemdetails = new string[] {itemcode.ToString(), newname, itemtype,
                                    itemstock.ToString(), itemprice.ToString(), itemseller};

            foreach (string item in itemdetails)
                WriteLine(item);

            //Add to inventory list
            Item newitem = new Item(itemdetails);
            WriteLine(newitem);
            UpdateFile("ProductInventory.csv", newitem.itemstr);
            inventory = GetContent("ProductInventory.csv").Select(x => new Item(x)).ToList();
            foreach (Item item in inventory)
                WriteLine(item.name);
            WriteLine(inventory.Count);
            ReadKey(true);
            ManageProducts();
        }
         //Inventory Case 3
        public void EditItem(Item itemchoice)
        {
            Clear();
            try
            {
                
                ItemDetails(itemchoice, 5, (WindowHeight/2) - 5);
                ReadKey(true);
                int displaylength = WindowWidth - (WindowWidth * 2 / 3);
                int displaystart_top = (WindowHeight / 2) - 3;
                int displaystart_left = (WindowWidth * 2 / 3) + (displaylength * 1/3) ;
                List<string> editlist = new List<string>() { "Name", "Type", "Price", "Stock", "Save" };
                Menu<string> editmenu = new Menu<string>("Which detail to update?", editlist);
                string editchoice = editmenu.Browse_Options();

                ItemDetails(itemchoice, 5, (WindowHeight / 2) - 5);

                switch (editchoice)
                {
                    case "Name":
                        do
                        {
                            InputPrompt(new int[] { displaystart_left, displaystart_top }, "Enter item name: ", "Invalid item name");
                            string newname = String(new int[] { displaystart_left, displaystart_top + 2 }, "Invalid item name.");
                            bool nameExists = false;
                            foreach (Item item in inventory)
                            {
                                if (item.name == newname)
                                    nameExists = true;
                            }
                            if (!nameExists)
                            {
                                itemchoice.name = newname;
                                break;
                            }
                            else
                            {
                                WriteLine("Item name already exists: ");
                                Clear();
                            }
                        } while (true);
                        break;
                    case "Type":
                        InputPrompt(new int[] { displaystart_left, displaystart_top }, "Enter item type: ", "Invalid item type");
                        string itemtype = String(new int[] { displaystart_left, displaystart_top + 2 }, "");
                        itemchoice.type = itemtype;
                        break;
                    case "Price":
                        do
                        {
                            InputPrompt(new int[] { displaystart_left, displaystart_top }, Price, PriceError);
                            double newprice = Double(new int[] { displaystart_left, displaystart_top + 2}, "Invalid amount.");
                            if(newprice > 0)
                            {
                                itemchoice.price = newprice;
                                break;
                            }
                            else
                            {
                                WriteLine(PriceError);
                                Clear();
                            }
                        } while (true);

                        break;
                    case "Stock":
                        List<string> stockoptions = new List<string>() { "Override", "Add", "Subtract", "Back" };
                        Menu<string> stockmenu = new Menu<string>("Choose option: ", editlist);
                        string stockchoice = stockmenu.Browse_Options(displaystart_top, displaystart_left);
                        InputPrompt(new int[] { displaystart_left, displaystart_top }, Quantity, QuantityError);
                        int newquantity = Int(new int[] { displaystart_left, displaystart_top + 2}, QuantityError);
                        switch (stockchoice)
                        {
                            case "Override":
                                if (newquantity > 0)
                                    if (inventory != null) itemchoice.stock = newquantity;
                                break;
                            case "Add":
                                if (newquantity > 0)
                                    if (inventory != null) itemchoice.stock += newquantity;
                                break;
                            case "Subtract":
                                if (newquantity > 0)
                                    if (inventory != null) itemchoice.stock -= newquantity;
                                break;
                            default:
                                Console.WriteLine("Invalid action.");
                                break;

                        }
                        break;
                    default:
                        Console.WriteLine("Invalid!");
                        EditItem(itemchoice);
                        break;
                }

                itemchoice.UpdateDetails();
                ItemDetails(itemchoice, 5, (WindowHeight / 2) - 5);
                int index = ItemIndex(itemchoice);
                allitems[index] = itemchoice;
                
                List<string> updatelist = allitems.OrderBy(x => x.itemcode).Select(x => x.itemstr).ToList();
                UpdateFile("ProductInventory.csv", updatelist);
                ReadKey(true);
                Clear();
                ManageProducts();
                
            }
            catch(Exception e)
            {
                WriteLine("Error: " +  e.Message);
            }
        }
        //Inventory Case 4
        public void DeleteItem(Item itemchoice)
        {
            try
            {
                foreach (Item item in allitems)
                {
                    if (itemchoice.itemcode == item.itemcode)
                    {
                        inventory.Remove(item);
                        Clear();
                        WriteLine("Item successfully removed.");
                        ReadKey(true);
                        Clear();
                        ManageProducts();
                    }
                }
            }
            catch (Exception e)
            {
                WriteLine("Error: " + e.Message);

            }
        }

        //Start of Order Management
        
        public void ViewOrders()
        {
            try
            {
                Clear();
                int displaystart_left = 5;
                int displaystart_top = 10;
                int displaylength = (WindowWidth - 5) - displaystart_left;

                ClearLine((WindowWidth * 1 / 5), WindowWidth, displaystart_top, orders.Count + displaystart_top);

                string searchfor = "";

                List<string> sortby = new List<string> { "All", "Item", "Type", "Date Purchased", "Buyer" };
                List<Purchased> storeorders = orders;
                Menu<Purchased> menu = new Menu<Purchased>("", storeorders, sortby);

                menu.Display_Options();
                menu.DisplayItems(displaystart_top + 6, displaystart_left, displaylength, "Merchant");

                ConsoleKey keyPressed;
                InputPrompt(new int[] { displaystart_left, displaystart_top }, "Search here: ", "");
                do
                {
                    ClearLine(displaystart_left, displaystart_left + displaylength,
                        displaystart_top, storeorders.Count + displaystart_top);
                    ConsoleKeyInfo keyInfo = ReadKey(false);
                    keyPressed = keyInfo.Key;

                    //Verify pleaseee
                    searchfor += keyPressed;

                    searchlist = inventory.Where(x => x.itemstr.Contains(searchfor)).
                        OrderBy(x => x.name.Contains(searchfor)).
                        ThenBy(x => x.type.Contains(searchfor)).ToList();

                } while (keyPressed != ConsoleKey.Enter && keyPressed != ConsoleKey.Escape);

                if (keyPressed == ConsoleKey.Escape)
                {
                    UI();
                }
                else if (keyPressed == ConsoleKey.Enter)
                {
                    var sortchoice = menu.Browse_Options();
                    Purchased itemchoice = null;
                    switch (sortchoice)
                    {
                        case "All":
                            storeorders = orders.OrderBy(x => x.item.name).ToList();
                            menu.OrderList = storeorders;
                            itemchoice = menu.BrowseOrders(displaystart_top + 6, displaystart_left, displaylength, "Merchant", SearchItem);
                            break;
                        case "Keyword":
                            storeorders = orders.Where(x => x.itemstr.Contains(searchfor)).
                                OrderBy(x => x.item.name.Contains(searchfor)).ThenBy(x => x.item.type.Contains(searchfor)).
                                ThenBy(x => x.item.seller.Contains(searchfor)).ToList();
                            menu.OrderList = storeorders;
                            itemchoice = menu.BrowseOrders(displaystart_top + 6, displaystart_left, displaylength, "Merchant", SearchItem);
                            break;
                        case "Item":
                            storeorders = orders.Where(x => x.item.name.Contains(searchfor)).
                             OrderBy(x => x.item.name).ToList();
                            menu.OrderList = storeorders;
                            itemchoice = menu.BrowseOrders(displaystart_top + 6, displaystart_left, displaylength, "Merchant", SearchItem);
                            break;
                        case "Type":
                            storeorders = orders.Where(x => x.item.type.Contains(searchfor)).
                             OrderBy(x => x.item.type).ThenBy(x => x.item.name).ToList();
                            menu.OrderList = storeorders;
                            itemchoice = menu.BrowseOrders(displaystart_top + 6, displaystart_left, displaylength, "Merchant", SearchItem);
                            break;
                        case "Date Purchased":
                            storeorders = orders.Where(x => x.purchasedon.ToString().Contains(searchfor)).
                             OrderBy(x => x.purchasedon).ThenBy(x => x.item.name).ToList();
                            menu.OrderList = storeorders;
                            itemchoice = menu.BrowseOrders(displaystart_top + 6, displaystart_left, displaylength, "Merchant", SearchItem);
                            break;
                        case "Buyer":
                            storeorders = orders.Where(x => x.buyer.Contains(searchfor)).
                             OrderBy(x => x.buyer).ThenBy(x => x.item.name).ToList();
                            menu.OrderList = storeorders;
                            itemchoice = menu.BrowseOrders(displaystart_top + 6, displaystart_left, displaylength, "Merchant", SearchItem);
                            break;
                        case "Back":
                            UI();
                            break;
                        default:
                            break;
                    }
                    Clear();
                    if (itemchoice != (Purchased)(object)"Back")
                    {
                        OrderDetails(itemchoice);
                    }
                    else
                    {
                        UI();
                    }

                }
            }
            catch (Exception e)
            {
                WriteLine("Error: " + e.Message);
                SearchItem();
            }

        }
        public void OrderDetails(Purchased choiceinfo)
        {
            int displaystart_left = WindowWidth + 5;
            int displaystart_top = (WindowWidth / 2) - 6;
            int displaylength = WindowWidth * 2 / 3;

            InputPrompt(new int[] { displaystart_left, displaystart_top },
                "Item Name: " + FormatLine(displaylength, choiceinfo.item.name) + "\n", "");
            InputPrompt(new int[] { displaystart_left, displaystart_top + 3 },
                "Item Type: " + FormatLine(displaylength, choiceinfo.item.type) + "\n", "");
            InputPrompt(new int[] { displaystart_left, displaystart_top + 6 },
                "Item Price: " + choiceinfo.item.price + "\n", "");
            InputPrompt(new int[] { displaystart_left, displaystart_top + 9 },
                "Item Stock: " + choiceinfo.quantity + "\n", "");
            InputPrompt(new int[] { displaystart_left, displaystart_top + 12 },
                "Item Stock: " + choiceinfo.cost + "\n", "");
            InputPrompt(new int[] { displaystart_left, displaystart_top + 15 },
                "Item Stock: " + choiceinfo.purchasedon + "\n", "");
            InputPrompt(new int[] { displaystart_left, displaystart_top + 18 },
                "Item Type: " + FormatLine(displaylength, choiceinfo.buyer) + "\n", "");
        }

    }
}

