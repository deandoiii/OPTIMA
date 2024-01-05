using OPTIMA;
using System;
using static System.Console;
namespace OPTIMA
{
    using static Input;
    interface IReceipt
    {
        public void GenerateReceipt();
    }
    public class Regular : User, IReceipt
    {
        List<Item> inventory {  get; set; }
        List<Purchased> orders { get; set; } 
        public Regular(List<string> userdetails) : base(userdetails)
        {
            inventory = allitems.OrderBy(x => x.name).ToList();
            orders = allorders.Where(x => x.buyer.Contains(username)).OrderByDescending(x => x.purchasedon).ToList();
        }

        public override void UI()
        {
            try
            {
                Clear();
                SetCursorPosition(WindowWidth / 2 - 7, WindowHeight / 2 - 3);
                List<string> regularmenu = new List<string> { "Display Items", "Open Cart", "View Purchases", "Log-Out" };
                Menu<string> menu = new Menu<string>("Menu: ", regularmenu);
                string choice = menu.Browse_Options();
                switch (menu.active_index + 1)
                {
                    case 1:
                        BrowseItems();
                        break;
                    case 2:
                        OpenCart();
                        break;
                    case 3:
                        ViewPurchases();
                        break;
                    case 4:
                        Logout();
                        break;

                }
            }
            catch (Exception e)
            {
                WriteLine(e.Message);
                ReadKey(true);
                UI();
            }
        }

        public void BrowseItems()
        {

        }

        public void OpenCart()
        {

        }

        public void ViewPurchases()
        {

        }

        override public void Profile()
        {

        }

        public void GenerateReceipt()
        {

        }
    }
}

