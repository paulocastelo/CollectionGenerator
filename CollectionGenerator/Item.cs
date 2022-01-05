using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectionGenerator.Enums;

namespace CollectionGenerator
{
    class Item
    {
        public Object[] References = new Object[9];

        public Item(int[] list, DateTime dateTime)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (i == 0)
                {
                    References[i] = list[i];
                }
                else
                {
                    References[i] = (References)list[i];
                }
            }
        }
        public override string ToString()
        {
            string stringValue;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < References.Length; i++)
            {
                if (i == 0)
                {
                    sb.Append($"[{References[i]}]");
                }
                else
                {
                    stringValue = (References[i]).ToString();
                    sb.Append($"\n\t[{stringValue.Replace("__Sub__",": ").Replace("_"," ")}]");
                }
            }
            return sb.ToString();
        }
    }
}
