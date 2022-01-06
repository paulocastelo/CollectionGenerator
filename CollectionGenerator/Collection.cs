using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionGenerator
{
    class Collection
    {
        public int Size { get; set; }
        public double Days { get; set; }
        public string Types { get; set; }
        public string Themes { get; set; }
        public Item[] ItemsSet { get; set; }
        public DateTime[] ReleaseDate { get; set; }
        public Collection(int size, string types, string themes)
        {
            Size = size;
            Types = types;
            Themes = themes;
        }
        public Collection(int size, string types, string themes, Item[] itemsSet) : this(size, types, themes)
        {
            ItemsSet = itemsSet;
        }

        public Collection(int size, string types, string themes, Item[] itemsSet, DateTime releaseDate)
            : this(size, types, themes, itemsSet)
        {
            Days = 0.0;
            SetListRelease(releaseDate, Days);
        }
        public Collection(int size, string types, string themes, Item[] itemsSet, DateTime releaseDate, double days)
            : this(size, types, themes, itemsSet)
        {
            SetListRelease(releaseDate, days);
        }
        private void SetListRelease(DateTime releaseDate, double day)
        {
            double varDay = day;
            if (day == 0.0)
            {
                day = 7.0;
            }
            else
            {
                day = varDay;
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
                    listRelease[i] = listRelease[i - 1].AddDays(day);
                }
            }
            ReleaseDate = listRelease;
        }

        public override string ToString()
        {
            string dateRelease = string.Empty;
            string stringValue = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append($"Collection: Size: {Size} / Type: {Types} / Theme: {Themes}\n");
            for (int i = 0; i < ItemsSet.Length; i++)
            {
                dateRelease = ReleaseDate[i].ToString();
                stringValue = ItemsSet[i].ToString();
                sb.Append($"{dateRelease}:{stringValue}");
                sb.Append("\n");
            }
            return sb.ToString();
        }
    }
}
