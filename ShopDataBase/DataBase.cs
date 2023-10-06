using NewVariant.Interfaces;
using NewVariant.Exceptions;
using NewVariant.Models;
using System.Runtime.Serialization.Json;

namespace EkRLib
{
    /// <summary>
    /// Класс таблиц классов Sale, Buyer, Shop, Good.
    /// </summary>
    public class DataBase : IDataBase
    {
        /// <summary>
        /// Таблица для продаж.
        /// </summary>
        public List<Sale> BaseSale { get; set; }

        /// <summary>
        /// Таблица для покупателей.
        /// </summary>
        public List<Buyer> BaseBuyer { get; set; }

        /// <summary>
        /// Таблица для магазинов.
        /// </summary>
        public List<Shop> BaseShop { get; set; }

        /// <summary>
        /// Таблица для товаров.
        /// </summary>
        public List<Good> BaseGood { get; set; }

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public DataBase() { }

        /// <summary>
        /// Создает новую таблицу типа Т.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="DataBaseException">Если такая таблица уже существует,
        /// выбрасывает исключение такого типа.
        /// </exception>
        public void CreateTable<T>() where T : IEntity
        {
            var ListT = new List<T>();

            if (ListT is List<Sale>)
            {
                if (BaseSale != null)
                    throw new DataBaseException();

                BaseSale = new List<Sale>();
            }
            else if (ListT is List<Shop>)
            {
                if (BaseShop != null)
                    throw new DataBaseException();

                BaseShop = new List<Shop>();
            }
            else if (ListT is List<Good>)
            {
                if (BaseGood != null)
                    throw new DataBaseException();

                BaseGood = new List<Good>();
            }
            else if (ListT is List<Buyer>)
            {
                if (BaseBuyer != null)
                    throw new DataBaseException();

                BaseBuyer = new List<Buyer>();
            }
        }

        /// <summary>
        /// Десериализует и сохраняет таблицу типа Т из файла, расположенного по
        /// переданному пути(path).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        public void Deserialize<T>(string path) where T : IEntity
        {
            var deserializer = new DataContractJsonSerializer(typeof(List<T>));
            var ListT = new List<T>();

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {

                if (ListT is List<Sale>)
                {
                    BaseSale = deserializer.ReadObject(fs) as List<Sale>;
                }
                else if (ListT is List<Shop>)
                {
                    BaseShop = deserializer.ReadObject(fs) as List<Shop>;
                }
                else if (ListT is List<Buyer>)
                {
                    BaseBuyer = deserializer.ReadObject(fs) as List<Buyer>;
                }
                else if (ListT is List<Good>)
                {
                    BaseGood = deserializer.ReadObject(fs) as List<Good>;
                }
            }
        }

        /// <summary>
        /// Возвращает ссылку на таблицу типа Т. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="DataBaseException">Если такой таблицы не существует,
        /// выбрасывает исключение типа DataBaseException.
        /// </exception>
        public IEnumerable<T> GetTable<T>() where T : IEntity
        {
            var ListT = new List<T>();

            if (ListT is List<Sale>)
            {
                if (BaseSale == null)
                    throw new DataBaseException();

                IEnumerable<T> EnumBase = (IEnumerable<T>)BaseSale;
                return EnumBase;
            }
            else if (ListT is List<Shop>)
            {
                if (BaseShop == null)
                    throw new DataBaseException();

                IEnumerable<T> EnumBase = (IEnumerable<T>)BaseShop;
                return EnumBase;
            }
            else if (ListT is List<Good>)
            {
                if (BaseGood == null)
                    throw new DataBaseException();

                IEnumerable<T> EnumBase = (IEnumerable<T>)BaseGood;
                return EnumBase;
            }
            else if (ListT is List<Buyer>)
            {
                if (BaseBuyer == null)
                    throw new DataBaseException();

                IEnumerable<T> EnumBase = (IEnumerable<T>)BaseBuyer;
                return EnumBase;
            }

            throw new DataBaseException();
        }

        /// <summary>
        /// Добавляет новую строку в таблицу типа Т. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getEntity"></param>
        /// <exception cref="DataBaseException">В случае попытки добавления строки
        /// в несуществующую таблицу, выбрасывает исключение такого типа.</exception>
        public void InsertInto<T>(Func<T> getEntity) where T : IEntity
        {
            var ListT = new List<T>();

            if (ListT is List<Sale>)
            {
                if (BaseSale == null)
                    throw new DataBaseException();

                BaseSale.Add(getEntity.Invoke() as Sale);
            }
            else if (ListT is List<Shop>)
            {
                if (BaseShop == null)
                    throw new DataBaseException();

                BaseShop.Add(getEntity.Invoke() as Shop);
            }
            else if (ListT is List<Good>)
            {
                if (BaseGood == null)
                    throw new DataBaseException();

                BaseGood.Add(getEntity.Invoke() as Good);
            }
            else if (ListT is List<Buyer>)
            {
                if (BaseBuyer == null)
                    throw new DataBaseException();

                BaseBuyer.Add(getEntity.Invoke() as Buyer);
            }
        }

        /// <summary>
        /// Сериализует таблицу типа Т в файл, расположенный по переданному пути(path).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <exception cref="DataBaseException">Если такой таблицы не
        /// существует, выбрасывает исключение такого типа.
        /// </exception>
        public void Serialize<T>(string path) where T : IEntity
        {
            var serializer = new DataContractJsonSerializer(typeof(List<T>));

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                var ListT = new List<T>();

                if (ListT is List<Sale>)
                {
                    if (BaseSale == null)
                        throw new DataBaseException();

                    serializer.WriteObject(fs, BaseSale);
                }
                else if (ListT is List<Shop>)
                {
                    if (BaseShop == null)
                        throw new DataBaseException();

                    serializer.WriteObject(fs, BaseShop);
                }
                else if (ListT is List<Buyer>)
                {
                    if (BaseBuyer == null)
                        throw new DataBaseException();

                    serializer.WriteObject(fs, BaseBuyer);
                }
                else if (ListT is List<Good>)
                {
                    if (BaseGood == null)
                        throw new DataBaseException();

                    serializer.WriteObject(fs, BaseGood);
                }

                throw new DataBaseException();
            }
        }
    }
}
