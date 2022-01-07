using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionGenerator
{
    class Program
    {
        private static readonly Random randomValue = new Random();
        private static readonly object syncLock = new object();

        static void Main(string[] args)
        {
            //Set paths
            string pathFiles;
            Console.WriteLine("Set files directory: ");
            pathFiles = Console.ReadLine();
            if (pathFiles == "")
            {
                pathFiles = @"C:\Repos\projects\CollectionGenerator\CollectionGenerator\SourcesFiles\";
                //pathFiles = "";
            }

            //Check if directory exists
            var di = new DirectoryInfo(pathFiles);
            if (pathFiles != null && pathFiles != "" && di.Exists == true)
            {
                //Get list of files
                var filteredFiles = Directory
                        .GetFiles(pathFiles, "*.*")
                        .ToList();


                List<string>[] files = ListFromFiles(pathFiles, filteredFiles);

                //Get type item from list
                string listName = string.Empty;
                listName = "Types";
                string collectionType = GetRandomItem(files, listName);

                //Get theme item from list
                listName = "Themes";
                string collectionTheme = GetRandomItem(files, listName);

                //Select references to compose collection item
                listName = "References";
                //Parameters for randomic generating list
                int randomStart = 15;
                int randomEnd = 42;
                int qtItemAttributes = 9;
                int sizeCollection = SetRandom(randomStart, randomEnd);
                Item[] itemsSet = new Item[sizeCollection];
                DateTime releaseDate = DateTime.Now;
                double qtDays = 3.0;
                List<string> fileContent = GetFileContent(files, listName);

                //Set start and end to random items
                int startCollection = 1;
                int EndCollection = fileContent.Count;
                GenerateCollection(startCollection, EndCollection, sizeCollection, qtItemAttributes, itemsSet, fileContent);

                Collection generatedCollection = new Collection(sizeCollection, collectionType, collectionTheme, itemsSet, releaseDate, qtDays);

                //Create collection of files
                //Console.WriteLine("Input directory for output:");
                //string path = Console.ReadLine();
                var fileName = pathFiles + "CollectionFile.txt";
                ExportCollection(fileName, generatedCollection);
                Console.WriteLine();


                //Get items to selection
                //string[] options = GetOptions(pathFiles, filteredFiles);

                //Print and select options
                //int option = SelectOption(options);

                //Set parameter to print
                //listName = options[option];

                //Print list
                //PrintList(files, listName);

                //Generating arrays
            }
            else
            {
                Console.WriteLine("Invalid directory!");
            }

            Console.ReadKey();
        }

        private static List<string>[] ListFromFiles(string pathFiles, List<string> filteredFiles)
        {
            //Create arrayList from files
            List<string>[] newFiles = new List<string>[filteredFiles.Count];

            //Create list with file content
            for (int i = 0; i < filteredFiles.Count; i++)
            {
                //Creat string list for all items 
                List<string> fileLines = new List<string>();

                //Create string list for transformed items
                List<string> values = new List<string>();

                //Inserts first element with filename
                string arrayName = filteredFiles[i].Replace(pathFiles, "").Replace(".txt", "");

                //Create List to get item by tab number
                int maxTabOccurence = 10;
                List<string>[] itemByTabNumber = new List<string>[maxTabOccurence];

                //Open file method
                using (var fileStream = new FileStream(filteredFiles[i], FileMode.Open))
                using (var streamReader = new StreamReader(fileStream))
                {
                    //Incremental int to tab occurrences
                    int nTab = 0;

                    //Create all necessary list
                    for (int xy = 0; xy < itemByTabNumber.Length; xy++)
                    {
                        itemByTabNumber[xy] = new List<string>();
                    }

                    //ETL from file content
                    while (!streamReader.EndOfStream)
                    {
                        //Read line and replace special characters
                        string line = streamReader.ReadLine()
                                    .Replace("# ", "")
                                    .Replace("- ", "")
                                    .Replace("' ", "")
                                    .Replace("* ", "");

                        //Set number of tabs
                        nTab = line.Count(t => t == '\t');

                        //Read previous super types
                        string prevValue = "";
                        string value = "";
                        if (nTab != 0)
                        {
                            prevValue = itemByTabNumber[nTab - 1].Last();
                            value = $"{prevValue} : [{line}]".Replace("\t", "");
                        }
                        else
                        {
                            value = $"[{line}]".Replace("\t", "");
                        }
                        itemByTabNumber[nTab].Add(value);

                    }
                    for (int it = 0; it < itemByTabNumber.Length; it++)
                    {
                        foreach (var item in itemByTabNumber[it])
                        {
                            fileLines.Add(item);
                        }
                    }

                    //Console.WriteLine("Do you wish print all items? Y or N");
                    //string answer = Console.ReadLine();
                    //answer = answer.ToUpper();
                    //char opt = 'N';
                    //if (answer.ToUpper() != "Y" && answer.ToUpper() != "N" && answer.ToUpper() != "S")
                    //{
                    //    Console.WriteLine("Invalid option!");
                    //}
                    //else
                    //{
                    //    opt = char.Parse(answer);
                    //}
                    //Console.WriteLine(opt);
                    //if (opt == 'Y' || opt == 'S')
                    //{
                    //    Console.WriteLine("########## START ##########");

                    //    foreach (var item in fileLines)
                    //    {
                    //        Console.WriteLine(item);
                    //    }
                    //    Console.WriteLine();
                    //    Console.WriteLine("########## FINISH ##########");
                    //}
                    foreach (var item in fileLines)
                    {
                        values.Add(item);
                    }
                }
                ISet<string> temp = new SortedSet<string>();
                List<string> newtemp = new List<string>();
                foreach (var item in values)
                {
                    temp.Add(item);
                }
                //temp.Sort();
                newtemp.Add($"List: {arrayName}");
                foreach (var item in temp)
                {
                    newtemp.Add(item);
                }
                newFiles[i] = newtemp;
            }
            return newFiles;
        }

        private static string[] GetOptions(string pathFiles, List<string> filteredFiles)
        {
            //Set options to select
            string[] options = new string[filteredFiles.Count];
            for (int i = 0; i < options.Length; i++)
            {
                options[i] = filteredFiles[i].Replace(pathFiles, "").Replace(".txt", "");
            }

            return options;
        }

        private static int SelectOption(string[] options)
        {
            int option = 0;
            int value = 0;
            while (value == 0)
            {
                Console.WriteLine("Select list to print:");
                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine($"Input {i + 1} for {options[i]}");
                }

                //Input option
                string inputedOption = Console.ReadLine();
                if (int.TryParse(inputedOption, out value))
                {
                    option = value - 1;
                }
                if (value == 0)
                    Console.WriteLine("Invalid value!\n");
            }

            return option;
        }

        private static void PrintList(List<string>[] files, string listName)
        {
            //Print list
            for (int i = 0; i < files.Length; i++)
            {
                List<string> list = files[i];
                for (int j = 0; j < list.Count; j++)
                {
                    if (list.Contains($"List: {listName}"))
                    {
                        foreach (var item in files[i])
                        {
                            Console.WriteLine(item);
                        }
                        Console.WriteLine();
                        break;
                    }
                }
            }
        }

        private static string GetRandomItem(List<string>[] files, string listName)
        {
            //Get random item
            int indexRandom;
            string itemRandomValue = string.Empty;
            for (int i = 0; i < files.Length; i++)
            {
                List<string> list = files[i];
                for (int j = 0; j < list.Count; j++)
                {
                    if (list.Contains($"List: {listName}"))
                    {
                        indexRandom = randomValue.Next(list.Count);
                        itemRandomValue = list[indexRandom];
                        break;
                    }
                }
            }
            return itemRandomValue;
        }
        private static List<string> GetFileContent(List<string>[] files, string listName)
        {
            //Get random item
            List<string> resultList = new List<string>();
            for (int i = 0; i < files.Length; i++)
            {
                List<string> list = files[i];
                for (int j = 0; j < list.Count; j++)
                {
                    if (list.Contains($"List: {listName}"))
                    {
                        foreach (var item in list)
                        {
                            resultList.Add(item);
                        }
                        break;
                    }
                }
            }
            return resultList;
        }

        //private static List<string>[] ArrayListFromFiles(string pathFiles, List<string> filteredFiles)
        //{
        //    //Create arrayList from files
        //    List<string>[] files = new List<string>[filteredFiles.Count];

        //    //Create list with file content
        //    for (int i = 0; i < filteredFiles.Count; i++)
        //    {
        //        //Create string list
        //        List<string> values = new List<string>();

        //        //Inserts first element with filename
        //        string arrayName = filteredFiles[i].Replace(pathFiles, "").Replace(".txt", "");
        //        values.Add($"List: {arrayName}");

        //        //Open file method
        //        using (var fileStream = new FileStream(filteredFiles[i], FileMode.Open))
        //        using (var streamReader = new StreamReader(fileStream))
        //        {
        //            while (!streamReader.EndOfStream)
        //            {
        //                values.Add(streamReader.ReadLine());
        //            }
        //        }
        //        files[i] = values;
        //    }

        //    return files;
        //}

        private static void GenerateCollection(int start, int end, int sizeCollection, int qtItemAttributes, Item[] itemsSet, List<string> fileContent)
        {
            int index = 1;
            for (int i = 0; i < sizeCollection; i++)
            {
                //Creation of nested array: item and references
                List<string> refsInt = new List<string>();
                for (int j = 0; j < qtItemAttributes; j++)
                {
                    if (j == 0)
                    {
                        refsInt.Add(index++.ToString());
                    }
                    else
                    {
                        int randomNumber = SetRandom(start, end);
                        refsInt.Add(fileContent[randomNumber]);
                    }
                }
                //Set item object
                itemsSet[i] = new Item(refsInt);
            }
        }

        private static void ExportCollection(string path, Collection generatedCollection)
        {
            var newPath = path;
            using (var fileFill = new FileStream(newPath, FileMode.Create))
            {
                var encoding = Encoding.UTF8;
                var bytes = encoding.GetBytes(generatedCollection.ToString());
                fileFill.Write(bytes, 0, bytes.Length);
            }
            Console.WriteLine("Export finished!");
        }
        private static void ExportList(string path, Collection generatedCollection)
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
                result = randomValue.Next(initialValue, finalValue);
            }
            return result;
        }
    }
}
