using System.Runtime.Serialization;

namespace EBookLib
{
    [DataContract]
    public class Book : PrintEdition
    {
        [DataMember]
        private string _author; // Автор книги.

        public Book()
        {
            _author = null;
        }

        public Book(string name, int pages, string author) : base(name, pages)
        {
            _author = author;
        }

        /// <summary>
        /// Для вывода информации о книге.
        /// </summary>
        /// <returns>Информацию о книге.</returns>
        public override string ToString()
        {
            return $"{base.ToString()}; author = {_author}";
        }
    }
}
