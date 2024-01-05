using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OPTIMA
{
    public class Item
    {
        public int itemcode { get;set; }
        public string name { get; set; }
        public string type { get; set; }
        public int stock { get; set; }
        public double price { get; set; }
        public string seller { get; set; }
        public string[] details { get; set; }
        public string itemstr { get; set; }

        public Item() { }
        public Item(string[] itemstr)
        {
            itemcode = int.Parse(itemstr[0]);
            name = itemstr[1];
            type = itemstr[2];
            stock = int.Parse(itemstr[3]);
            price = Double.Parse(itemstr[4]);
            seller = itemstr[5];
            this.details = itemstr;
            this.itemstr = string.Join(",", details);
        }

        public void UpdateDetails()
        {
            this.details = new string[] { itemcode.ToString(), 
                name, type, stock.ToString(), 
                price.ToString(), seller };
            this.itemstr = string.Join(",", details);
        }

    }

    public class Purchased
    {
        public Item item { get; set; }
        public int quantity { get; set; }
        public double cost { get; set; }
        public DateTime purchasedon { get; set; }
        public string buyer {get; set;}
        public int ordernumber { get; set; }
        public string[] details { get; set; }
        public string itemstr { get; set; }

        public Purchased() { }
        public Purchased(string[] itemstr)
        {
            item = new Item
            {
                itemcode = int.Parse(itemstr[0]),
                name = itemstr[1],
                type = itemstr[2],
                seller = itemstr[3],
                stock = int.Parse(itemstr[4]),
                price = Double.Parse(itemstr[5])
            };
            quantity = int.Parse(itemstr[6]);
            cost = Double.Parse(itemstr[7]);
            purchasedon = DateTime.Parse(itemstr[8]);
            buyer = itemstr[9];
            ordernumber = int.Parse(itemstr[10]);
            this.details = itemstr;
            this.itemstr = string.Join(",", details);

        }

        public void UpdateDetails()
        {
            this.details = new string[] { item.itemcode.ToString(),
                item.name, item.type, item.stock.ToString(),
                item.price.ToString(), item.seller,
                quantity.ToString(), cost.ToString(),
                purchasedon.ToString(), buyer, ordernumber.ToString()};
            this.itemstr = string.Join(",", details);
        }
    }
}
