using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectionGenerator.Enums;

namespace CollectionGenerator
{
    class Program
    {
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        static void Main(string[] args)
        {
            DateTime release;
            Console.WriteLine("Input first release date:");
            string inputLine = Console.ReadLine();
            if (inputLine == "")
            {
                release = DateTime.Now;
            }
            else
            {
                release = DateTime.Parse(inputLine);
            }
            //Setting item name
            var nameItem = "Collection";
            int start = 15;
            int end = 42;
            // Case start = 0 or end = 0, it prompt shows
            int sizeCollection = SetRandom(start, end);
            Console.WriteLine($"Valores selecionados para {nameItem}: \nValor: {sizeCollection}.");
            Console.WriteLine();

            //Initial parameters
            start = 1;

            //Types
            nameItem = "Types";
            end = (int)Enum.GetValues(typeof(Types)).Cast<Types>().Last();
            int typeInt = SetRandom(start, end);
            Types typeEnum = (Types)typeInt;
            Console.WriteLine($"Valores selecionados para {nameItem}: \nValor: {typeInt}.");
            Console.WriteLine();

            //Themes
            nameItem = "Themes";
            end = (int)Enum.GetValues(typeof(Themes)).Cast<Themes>().Last(); ;
            int themesInt = SetRandom(start, end);
            Themes themeEnum = (Themes)themesInt;
            Console.WriteLine($"Valores selecionados para {nameItem}: \nValor: {themesInt}.");
            Console.WriteLine();

            //Setting Collection
            int index = 1;
            int itemRef = 9;
            end = (int)Enum.GetValues(typeof(References)).Cast<References>().Last();


            //Set items collection
            Item[] itemsSet = new Item[sizeCollection];

            for (int i = 0; i < sizeCollection; i++)
            {
                //Creation of nested array: item and references
                int[] refs = new int[itemRef];
                for (int j = 0; j < itemRef; j++)
                {
                    if (j == 0)
                    {
                        refs[j] = index++;
                    }
                    else
                    {
                        refs[j] = SetRandom(start, end);
                    }
                }
                //Set item object
                itemsSet[i] = new Item(refs, release);
            }

            //Set collection
            Collection generatedCollection = new Collection(sizeCollection, typeEnum, themeEnum, itemsSet, release);

            var fileName = "CollectionFile.txt";
            CreateFile(fileName, generatedCollection);
            Console.WriteLine("Generating file:");
            Console.WriteLine(generatedCollection);

            Console.WriteLine();

            Console.ReadKey();
        }

        private static void CreateFile(string path, Collection generatedCollection)
        {
            var newPath = path;
            using (var fileFill = new FileStream(newPath, FileMode.Create))
            {
                var encoding = Encoding.UTF8;
                var bytes = encoding.GetBytes(generatedCollection.ToString());
                fileFill.Write(bytes, 0, bytes.Length);
            }
        }
        private static int SetRandom(int start, int end)
        {

            //Setting variables
            int initialValue = start, finalValue = end;
            if (initialValue == 0 || finalValue == 0)
            {
                int value;
                var typeRange = string.Empty;
                for (int ini = 0; ini < 2; ini++)
                {
                    //Setting type of range's value
                    if (ini == 0)
                        typeRange = "initial";
                    if (ini == 1)
                        typeRange = "final";

                    int setValue = 0;
                    while (setValue < 2)
                    {
                        Console.WriteLine($"Input {typeRange} value to range:");
                        var line = Console.ReadLine();
                        if (int.TryParse(line, out value))
                        {
                            setValue = value;
                            if (ini == 0)
                            {
                                initialValue = setValue;
                            }
                            if (ini == 1)
                            {
                                finalValue = setValue;
                            }
                            setValue++;
                        }
                        if (setValue == 0)
                            Console.WriteLine("Invalid value!\n");
                    }
                }
            }
            //Setting initial range valueLine();
            var result = 0;
            //Lock is used to prevent occurance of same number
            lock (syncLock)
            { // synchronize
                result = random.Next(initialValue, finalValue);
            }
            return result;
        }
    }
}
