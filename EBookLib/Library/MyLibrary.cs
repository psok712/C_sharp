using System.Collections;
using System.Runtime.Serialization;
using System.Text;

namespace EBookLib
{
    /// <summary>
    /// Класс для реализации события OnTake.
    /// </summary>
    public class LibraryEventArgs : EventArgs
    {
        public char Start { get; set; } // Первая буква названия книги.

        public LibraryEventArgs(char start)
        {
            Start = start;
        }
    }

    [DataContract]
    public class MyLibrary<T> : IEnumerable<T>
        where T : PrintEdition
    {
        [DataMember]
        public T[] library; // Массив для хранения печатных изданий.
        public int Length { get { return library.Length; } } // Длина массива.
        public int Count { get; set; } = 0; // Количество заполненных элементов массива.

        public MyLibrary(int capacity)
        {
            library = new T[capacity];
        }

        /// <summary>
        /// Событие, оповещающее, что книги были взяты.
        /// </summary>
        public event EventHandler<LibraryEventArgs> OnTake;

        /// <summary>
        /// Выводит на экран книги, начинающиеся с определенной буквы, и удаляет их из библиотеки.
        /// </summary>
        /// <param name="start">Буква, с которой производится выборка книг.</param>
        public void TakeBooks(char start)
            => OnTake?.Invoke(this, new LibraryEventArgs(start));

        /// <summary>
        /// Индексатор для обращения или возврата элемента библиотеки.
        /// </summary>
        /// <param name="i">Номер элемента библиотеки.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public T this[int i]
        {
            get
            {
                if (library == null)
                    throw new ArgumentNullException();
                else if(library.Length <= i || i < 0)
                    throw new IndexOutOfRangeException();
                return library[i];
            }

            set
            {
                if (library == null)
                    throw new ArgumentNullException();
                else if (library.Length <= i || i < 0)
                    throw new IndexOutOfRangeException();
                library[i] = value;
            }
        }

        /// <summary>
        /// Добавляет печатное издание в библиотечный массив.
        /// </summary>
        /// <param name="printed">Печатное издание, которое нужно добавить в список.</param>
        public void Add(T printed)
        {
            library[Count] = printed;
            Count++;
        }

        /// <summary>
        /// Считает среднее количество страниц в книгах.
        /// </summary>
        public double AveragePagesBook
        {
            get
            {
                int countPages = 0;
                int countBook = 0;
                for (int i = 0; i < library.Length; ++i)
                    if (library[i] is Book)
                    {
                        countBook++;
                        countPages += library[i].pages;
                    }

                return countBook != 0? (double)countPages / countBook : 0;
            }
        }

        /// <summary>
        /// Считает среднее количество страниц в журналах.
        /// </summary>
        public double AveragePagesMagazine
        {
            get
            {
                int countPages = 0;
                int countMagazine = 0;
                for (int i = 0; i < library.Length; ++i)
                    if (library[i] is Magazine)
                    {
                        countMagazine++;
                        countPages += library[i].pages;
                    }

                return countMagazine != 0? (double)countPages / countMagazine : 0;
            }
        }

        /// <summary>
        /// Метод для итерации коллекции.
        /// </summary>
        /// <returns>Элементы коллекции.</returns>
        public IEnumerator<T> GetEnumerator()
            => new MyLibraryEnumerator<T>(library);

        /// <summary>
        /// Метод для итерации коллекции.
        /// </summary>
        /// <returns>Элементы коллекции.</returns>
        IEnumerator IEnumerable.GetEnumerator()
            => new MyLibraryEnumerator<T>(library);

        /// <summary>
        /// Осуществляет итерацию коллекции library.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        class MyLibraryEnumerator<U> : IEnumerator<U>, IEnumerable
        {
            U[] library;
            int position = -1;

            public MyLibraryEnumerator(U[] library)
            {
                this.library = library;
            }

            public U Current 
                => library[position];

            object IEnumerator.Current
                => Current!;

            public void Dispose() { }

            public IEnumerator GetEnumerator() 
                => library.GetEnumerator();

            public bool MoveNext()
            {
                if (position < library.Length - 1)
                {
                    position++;
                    return true;
                }
                return false;
            }

            public void Reset()
               => position = -1;
        }

        /// <summary>
        /// Для вывода всей информации о библиотеке на экран.
        /// </summary>
        /// <returns>Элементы библиотеки.</returns>
        public override string ToString()
        {
            var strRes = new StringBuilder();
            int pagesTotal = 0;

            for (int i = 0; i < library.Length; ++i)
            {
                pagesTotal += library[i].pages;
                strRes.AppendFormat($"{library[i]}\r\n");
            }
            strRes.Insert(0, $"Общее число страниц во всех печатных изданиях:{pagesTotal}\r\n");

            return strRes.ToString();
        }
    }
}
