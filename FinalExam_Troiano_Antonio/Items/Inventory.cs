using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Troiano_Antonio
{
    static class Inventory
    {
        public static List<Items> items = new List<Items>();
        public static int InventoryCount;
        //public static void AddKey(Key key)
        //{
        //    items.Add(key);
        //}
        public static bool Contains(Items obj)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] == obj) return true;
            }
            return false;
        }
        public static bool ContainsAll()
        {
            if (InventoryCount == 6) return true;
            else return false;
        }
    }
}
