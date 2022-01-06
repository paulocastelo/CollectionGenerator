using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionGenerator
{
    class Item
    {
        public Object[] ItemAtrib = new Object[9];

        public Item(List<string> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                ItemAtrib[i] = list[i];
            }
        }
        public override string ToString()
        {
            string stringValue;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ItemAtrib.Length; i++)
            {
                if (i == 0)
                {
                    sb.Append($"[{ItemAtrib[i]}]");
                }
                else
                {
                    stringValue = ItemAtrib[i].ToString();
                    sb.Append($"\n\t{stringValue}");
                }
            }
            return sb.ToString();
        }
    }
}
