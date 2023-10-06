using System.Runtime.Serialization;

namespace EBookLib
{
    [DataContract, KnownType(typeof(Book)), KnownType(typeof(Magazine))]
    public abstract class PrintEdition : IPrinting
    {
        [DataMember]
        public string? name; // Название книги.

        [DataMember]
        internal int pages; // Количество страниц в книге.

        public PrintEdition()
        {
            name = null;
            pages = 0;
        } 

        public PrintEdition(string name, int pages)
        {
            this.name = name;
            Pages = pages;
        }

        /// <summary>
        /// Свойство для проверки верного ввода страниц печатного издании.
        /// </summary>
        /// <exception cref="ArgumentException">При отрицательном количестве страниц.</exception>
        private int Pages
        {
            set
            {
                if (value <= 0)
                    throw new ArgumentException();
                else
                    pages = value;
            }
        }

        /// <summary>
        /// Событие, оповещающее, что книги были выведены на экран.
        /// </summary>
        public event EventHandler OnPrint;

        /// <summary>
        /// Метод для вызова события OnPrint.
        /// </summary>
        public void Print() 
            => OnPrint?.Invoke(this, new EventArgs());

        /// <summary>
        /// Вывод информации о печатных изданиях.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"name = {name}; pages = {pages}";
        }
    }
}
