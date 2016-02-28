using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSVSearcher
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (CheckArguments(args) == false)
                {
                    Console.WriteLine("Ошибка! Невеоное число аргументов!");
                    ShowHelp();
                    return;
                }

                string fileName = GetFileName(args);
                string phrase = GetPhrase(args);
                if (String.IsNullOrEmpty(fileName) || String.IsNullOrEmpty(phrase))
                {
                    Console.WriteLine("Ошибка! указан файл или фраза!");
                    ShowHelp();
                    return;
                }

                Console.WriteLine("Файл для поиска: " + fileName);
                Console.WriteLine("Фраза: " + phrase);
                Console.WriteLine();
                SearchStrings(fileName, phrase);
                Console.Write("\nРабота завершена");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("OMG! Что-то пошло не так. А именно:");
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }
        }

        static void SearchStrings(string file, string phrase)
        {
            List<string> result = new List<string>();
            string[] lines = File.ReadAllLines(file, Encoding.Default);
            phrase = phrase.ToLower();
            foreach (string item in lines)
            {
                if(item.ToLower().Contains(phrase))
                {
                    Console.WriteLine("Найдено: " + item);
                    result.Add(item);
                }
            }

            Console.WriteLine("\nВсего найдено: " + result.Count);

            if (result.Count > 0)
            {
                string searchResultName = Path.GetFileNameWithoutExtension(file) + "_result" + Path.GetExtension(file);
                File.WriteAllLines(searchResultName, result.ToArray(), Encoding.Default);
                Console.WriteLine("Результат: " + searchResultName);
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine(new String('-', 35));
            Console.WriteLine("\tПоиск строк в csv файле");
            Console.WriteLine(new String('-' , 35));
            Console.WriteLine("Параметры запуска: { путь к программе } файл фраза");
            Console.WriteLine("\t'файл' - имя файла в котором надо найти строки (файл должен находится рядом с exe файлом)");
            Console.WriteLine("\t'фраза' - слово которое должна сожержать строка (поиск без учёта регистра)");
            Console.WriteLine("Например: СSVSearcher.exe file1.csv Москва");
            Console.Write("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static bool CheckArguments(string[] args)
        {
            return args.Length >= 2;
        }

        static string GetFileName(string[] args)
        {
            string fileName = args[0].Trim();
            bool isExists = File.Exists(fileName);
            string ext = Path.GetExtension(fileName).ToLower();
            bool success = (isExists && ext == ".csv");
            if(success)
            {
                return fileName;
            }
            return null;
        }

        static string GetPhrase(string[] args)
        {
            string phr = null;
            string[] arg = new string[args.Length - 1];
            Array.ConstrainedCopy(args, 1, arg, 0, args.Length - 1);
            phr = String.Join(" ", arg);
            
            return phr;
        }
    }
}