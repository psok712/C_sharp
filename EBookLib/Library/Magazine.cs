using System.Runtime.Serialization;

namespace EBookLib
{
    [DataContract]
    public class Magazine : PrintEdition
    {
        [DataMember]
        private int _period; // Период, на который оформляют подписку на журнал.
        public Magazine(string name, int pages, int period) : base(name, pages)
        {
            _period = period; 
        }
       
        /// <summary>
        /// Для вывода информации о журнале.
        /// </summary>
        /// <returns>Информацию о журнале.</returns>
        public override string ToString()
        {
            return $"{base.ToString()}; period = {_period}";
        }
    }

}
