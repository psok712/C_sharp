using EBookLib;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Program
{
    static class WorkWithLibrary
    {
        /// <summary>
        /// Генерация случайно сгенерированного названия книги или журнала.(от 3 до 10 символов)
        /// </summary>
        /// <returns>Случайное название печатного изделия.</returns>
        private static string ChooseRandomName()
        {
            var rnd = new Random();
            int n = rnd.Next(3, 11);
            StringBuilder str = new StringBuilder(n);

            for (int i = 0; i < n; ++i)
            {
                if (i == 0)
                    str.Append((char)rnd.Next('A', 'Z' + 1));
                else
                    str.Append((char)rnd.Next('a', 'z' + 1));
            }

            return str.ToString();
        }

        /// <summary>
        /// Генерация случайного автора книги из списка.
        /// </summary>
        /// <returns>Случайно сгенерированный автор.</returns>
        private static string ChooseRandomAuthor()
        {
            var rnd = new Random();
            int rand = rnd.Next(0, 7);

            switch (rand)
            {
                case 0: return "Pushkin";
                case 1: return "Gogol";
                case 2: return "Dostoyevsky";
                case 3: return "Bulgakov";
                case 4: return "Remarque";
                case 5: return "Gorky";
                case 6: return "Fet";
                default: return "Dashkov";
            }
        }

        /// <summary>
        /// Заполненяет библиотеку случайно сгенерированными книгами и журналами.
        /// </summary>
        /// <param name="myLibrary">Пустая библиотека для заполнения.</param>
        public static void ToFillInLibrary(ref MyLibrary<PrintEdition> myLibrary)
        {
            int i = 0;

            // Заполнение библиотеки рандомными книгами и журналами.
            while (i < myLibrary.Length)
            {
                try
                {
                    Random r = new Random();
                    if (r.Next(0, 2) == 0)
                        myLibrary.Add(new Book(ChooseRandomName(), r.Next(-11, 101), ChooseRandomAuthor()));
                    else
                        myLibrary.Add(new Magazine(ChooseRandomName(), r.Next(-11, 101), r.Next(1, 100)));
                    i++;
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Страниц не может быть отрицательное количество! Генерируется новая книга.");
                    continue;
                }
                catch (Exception) { continue; }
            }
        }

        /// <summary>
        /// Вызывает для всех книг метод Print().
        /// </summary>
        /// <param name="myLibrary">Библиотека с печатными изданиями.</param>
        public static void PrintAllBooks(MyLibrary<PrintEdition> myLibrary)
        {
            Console.WriteLine("Книги из библиотеки:");
            foreach(var el in myLibrary)
            {
                if (el is Magazine)
                    continue;
                el.OnPrint += Program.PrintHandler;
                el.Print();
            }
        }

        /// <summary>
        /// Выводит на экран и убирает из библиотеки книги, начинающиеся на случайно сгенерированную букву.
        /// </summary>
        /// <param name="myLibrary">Библиотека печатных изданий.</param>
        public static void PrintBooksFromLetter(MyLibrary<PrintEdition> myLibrary)
        {
            Random rnd = new Random();
            char startLetter = (char)rnd.Next('A', 'Z' + 1);
            myLibrary.OnTake += Program.TakeHandler;
            myLibrary.TakeBooks(startLetter);
        }

        /// <summary>
        /// Выводит содержимое библиотеки на экран.
        /// </summary>
        /// <param name="myLibrary">Библиотека печатных изданий.</param>
        public static void PrintMyLibrary(MyLibrary<PrintEdition> myLibrary)
        {
            Console.WriteLine("Содержимое библиотеки:");
            foreach (var el in myLibrary)
                Console.WriteLine(el);
        }

        /// <summary>
        /// Производит сериализацию библиотека в файл "mylibrary.txt".
        /// </summary>
        /// <param name="myLibrary">Библиотека печатных изданий.</param>
        public static void SerelizationLibrary(MyLibrary<PrintEdition> myLibrary)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MyLibrary<PrintEdition>));

            using (FileStream fs = new FileStream("mylibrary.txt", FileMode.Create))
            {
                using var write = JsonReaderWriterFactory.CreateJsonWriter(fs, System.Text.Encoding.Unicode, false, true); 
                serializer.WriteObject(write, myLibrary);
            }
            Console.WriteLine("Библиотека успешно сериализована.");
        }

        /// <summary>
        /// Производит десериализацию файла в библиотеку печатных изданий.
        /// </summary>
        /// <returns>Библиотеку печатных изданий.</returns>
        public static MyLibrary<PrintEdition> DeserelizationLibrary()
        {
            var deser = new DataContractJsonSerializer(typeof(MyLibrary<PrintEdition>));
            MyLibrary<PrintEdition> library;
            using (FileStream fs = new FileStream("mylibrary.txt", FileMode.Open))
                library = deser.ReadObject(fs) as MyLibrary<PrintEdition>;

            Console.WriteLine("Библиотека успешно десериализована.");
            return library!;
        }
    }
}
