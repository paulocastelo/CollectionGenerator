using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectionGenerator.Enums;

namespace CollectionGenerator
{
    class Collection
    {
        public int Size { get; set; }
        public double Days { get; set; }
        public Types Types { get; set; }
        public Themes Themes { get; set; }
        public Item[] ItemsSet { get; set; }
        public DateTime[] ReleaseDate { get; set; }
        public Collection(int size, Types types, Themes themes)
        {
            Size = size;
            Types = types;
            Themes = themes;
        }
        public Collection(int size, Types types, Themes themes, Item[] itemsSet) : this(size, types, themes)
        {
            ItemsSet = itemsSet;
        }

        public Collection(int size, Types types, Themes themes, Item[] itemsSet, DateTime releaseDate)
            : this(size, types, themes, itemsSet)
        {
            Days = 0.0;
            SetListRelease(releaseDate, Days);
        }
        public Collection(int size, Types types, Themes themes, Item[] itemsSet, DateTime releaseDate, double days)
            : this(size, types, themes, itemsSet)
        {
            SetListRelease(releaseDate, days);
        }
        private void SetListRelease(DateTime releaseDate, double days)
        {
            if (days == 0.0)
            {
                days = 7.0;
            }
            else
            {
                days = days;
            }
            DateTime[] listRelease = new DateTime[ItemsSet.Length];
            for (int i = 0; i < ItemsSet.Length; i++)
            {
                if (i == 0)
                {
                    listRelease[i] = releaseDate;
                }
                else
                {
                    listRelease[i] = listRelease[i - 1].AddDays(days);
                }
            }
            ReleaseDate = listRelease;
        }

        public override string ToString()
        {
            string dateRelease = string.Empty;
            string stringValue = string.Empty;
            string stringType = (Types).ToString();
            string stringTheme = (Themes).ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append($"Collection: Size: {Size} / Type: {stringType.Replace("_"," ")} / Theme: {stringTheme.Replace("_", " ")}\n");
            for (int i = 0; i < ItemsSet.Length; i++)
            {
                dateRelease = ReleaseDate[i].ToString();
                stringValue = (ItemsSet[i]).ToString();
                sb.Append($"{dateRelease}:[{stringValue}]");
                sb.Append("\n");
            }
            return sb.ToString();
        }
    }
}
