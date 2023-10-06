using EBookLib;

namespace Program
{
    internal class Program
    {
        /// <summary>
        /// Используется для ввода длины библиотеки.
        /// </summary>
        /// <param name="myLibrary">Библиотека для инициализации.</param>
        static void EnterCount(out MyLibrary<PrintEdition> myLibrary)
        {
            while (true)
                try
                {
                    int n = int.Parse(Console.ReadLine()!);
                    if (n <= 0)
                        throw new Exception();
                    myLibrary = new MyLibrary<PrintEdition>(n);
                    break;
                }
                catch (Exception)
                {
                    continue;
                }
        }

        /// <summary>
        /// Информирует, когда выводят информацию о книге на экран.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void PrintHandler(object? sender, EventArgs e)
        {
            Console.WriteLine("PRINTED!");
            Console.WriteLine(sender);
        }

        /// <summary>
        /// Информирует, когда забирают из библиотеки книги, начинающиеся на определенную букву.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void TakeHandler(object? sender, LibraryEventArgs e)
        {
            var myLibrary = (MyLibrary<PrintEdition>)sender;
            Console.WriteLine($"ATTENTION! Books starts with {e.Start} were taken!");

            var newLibrary = new PrintEdition[myLibrary.Length];
            int count = 0;

            for (int i = 0; i < myLibrary.Count; ++i)
            {
                if (myLibrary[i].name[0] != e.Start)
                {
                    newLibrary[count] = myLibrary[i];
                    count++;
                }
                else
                    Console.WriteLine(myLibrary[i]);
            }

            sender = newLibrary;
            myLibrary.Count = count;
        }

        static void Main(string[] args)
        {
            do
            {
                MyLibrary<PrintEdition> myLibrary;
                try
                {
                    Console.WriteLine("Введите количество печатных изданий:");
                    EnterCount(out myLibrary);
                    WorkWithLibrary.ToFillInLibrary(ref myLibrary);
                    WorkWithLibrary.PrintAllBooks(myLibrary);
                    WorkWithLibrary.PrintMyLibrary(myLibrary);
                    WorkWithLibrary.PrintBooksFromLetter(myLibrary);
                    WorkWithLibrary.PrintMyLibrary(myLibrary);
                    WorkWithLibrary.SerelizationLibrary(myLibrary);
                    MyLibrary<PrintEdition> deserLibrary = WorkWithLibrary.DeserelizationLibrary();
                    Console.WriteLine("Библиотека после сериализации:");
                    Console.WriteLine(deserLibrary);
                    Console.WriteLine($"Среднее количество страниц в книгах: {deserLibrary.AveragePagesBook:F2}");
                    Console.WriteLine($"Среднее количество страниц в журналах: {deserLibrary.AveragePagesMagazine:F2}");

                    Console.WriteLine("Для выхода нажмите Escape. Если желаете продолжить, нажмите любую кнопку.");
                }
                catch (Exception)
                {
                    Console.WriteLine("К сожалению, произошла неизвестная ошибка(");
                    Console.WriteLine("Для выхода нажмите Escape. Если желаете продолжить, нажмите любую кнопку.");
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }
    }
}
