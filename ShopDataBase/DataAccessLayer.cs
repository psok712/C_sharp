using NewVariant.Interfaces;
using NewVariant.Models;
using System.Collections.Immutable;

namespace EkRLib
{
    /// <summary>
    /// Класс для работы с базами данных класса DataBase.
    /// </summary>
    public class DataAccessLayer : IDataAccessLayer
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public DataAccessLayer() { }

        /// <summary>
        /// Возвращает список всех товаров, купленных покупателем с самым
        /// длинным именем.
        /// </summary>
        /// <param name="dataBase"></param>
        /// <returns></returns>
        public IEnumerable<Good> GetAllGoodsOfLongestNameBuyer(IDataBase dataBase)
        {
            var nameBuyer = (dataBase as DataBase).BaseBuyer.ElementAt(0); // Покупатель с самым длинным именем.
            int max = int.MinValue;                                        // Максимальная длина имени.
            var maxNameLength = new Dictionary<string, Buyer>();           // Словарь самых длинных имен
            var listIdGoods = new List<int>();                             // Лист с номерами товаров, приобретенных покупателем с самым длинным именем.

            // Поиск максимальной длины имени.
            foreach (var el in (dataBase as DataBase).BaseBuyer)
                if (max < el.Name.Length)
                    max = el.Name.Length;

            // Поиск покупателей с самыми длинными именами.
            foreach (var el in (dataBase as DataBase).BaseBuyer)
                if (max == el.Name.Length)
                    maxNameLength.Add(el.Name, el);

            maxNameLength.ToImmutableSortedSet();                            // Сортировка по имени
            nameBuyer = maxNameLength.ElementAt(0).Value;                    // Взятие последнего в лексикографическом порядке имени.

            foreach (var el in (dataBase as DataBase).BaseSale)
                if (el.BuyerId == nameBuyer.Id)
                    listIdGoods.Add(el.GoodId);

            // Поиск имен подходящих товаров по номеру.
            var listGoods = from elGood in (dataBase as DataBase).BaseGood
                            from elIdGood in listIdGoods
                            where elGood.Id == elIdGood
                            select elGood;

            return listGoods;
        }

        /// <summary>
        /// Для каждой страны определяет количество ее магазинов.
        /// </summary>
        /// <param name="dataBase"></param>
        /// <returns>Страну с минимальным количеством магазинов.</returns>
        public int GetMinimumNumberOfShopsInCountry(IDataBase dataBase)
        {
            // Группировка магазинов по странам.
            var country = from shop in (dataBase as DataBase).BaseShop
                          group shop.Id by shop.Country;

            int min = int.MaxValue;
            foreach (var el in country)
                if (min > el.Count())
                    min = el.Count();

            return min;
        }

        /// <summary>
        /// Для каждой страны определяет количество потраченных денег.
        /// </summary>
        /// <param name="dataBase"></param>
        /// <returns>Возвращает наименьшее из полученных значений.</returns>
        public string? GetMinimumSalesCity(IDataBase dataBase)
        {
            // Составляет пары цена за товар - номер магазина. 
            var priceAndShop = from saleGood in (dataBase as DataBase).BaseSale
                               from good in (dataBase as DataBase).BaseGood
                               where saleGood.GoodId == good.Id
                               select (Price: good.Price * saleGood.GoodCount, ShopId: saleGood.ShopId);

            // Группировка цен по странам, где была произведена покупка.
            var priceAndCity = from el in priceAndShop
                               from shop in (dataBase as DataBase).BaseShop
                               where el.ShopId == shop.Id
                               group el.Price by shop.City;

            var dictionary = new Dictionary<string, long>(); // Словарь страна - ключ, сумма потрач. денег - значение.

            // Подсчет суммы денег по странам.
            foreach (var el in priceAndCity)
                dictionary.Add(el.Key, el.Sum());

            (long Price, string City) minCity = (int.MaxValue, string.Empty); // Значение минимума с городом.

            // Поиск минимального значения.
            foreach (var el in dictionary)
                if (el.Value < minCity.Price)
                    minCity = (el.Value, el.Key);

            return minCity.City;
        }

        /// <summary>
        /// Возвращает название категории самого дорогого товара
        /// </summary>
        /// <param name="dataBase"></param>
        /// <returns></returns>
        public string? GetMostExpensiveGoodCategory(IDataBase dataBase)
        {
            // Сортировка по цене.(по убыванию)
            var MostCategory = from el in (dataBase as DataBase).BaseGood
                               orderby el.Price descending
                               select el.Category;

            // Вывод первого товара, так как значения отсортированы по убыванию.
            return MostCategory.ElementAt(0).ToString();
        }

        /// <summary>
        /// Возвращает список покупателей, которые купили самый популярный
        /// товар – такой товар, чьих единиц приобретено максимальное число.
        /// </summary>
        /// <param name="dataBase"></param>
        /// <returns></returns>
        public IEnumerable<Buyer> GetMostPopularGoodBuyers(IDataBase dataBase)
        {
            // Группировка количества купленного товара по номеру.
            var goodAndCount = from good in (dataBase as DataBase).BaseGood
                               from sale in (dataBase as DataBase).BaseSale
                               where sale.GoodId == good.Id
                               group sale.GoodCount by good.Id;

            var dictionaryGoodCount = new Dictionary<long, long>(); // Словарь с номером - ключом, суммой купленного товара - значением.

            // Подсчет суммы по номеру товара.
            foreach (var el in goodAndCount)
                dictionaryGoodCount.Add(el.Key, el.Sum());

            (long, long Count) mostPopularGood = (0, int.MinValue);

            // Поиск минимального значения.
            foreach (var el in dictionaryGoodCount)
                if (el.Value > mostPopularGood.Count)
                    mostPopularGood = (el.Key, el.Value);

            // Перебор покупателей для сравнения с покупками, чтобы определить тех, кто приобрели самый популярный товар.
            return from buyer in (dataBase as DataBase).BaseBuyer
                   where (from sale in (dataBase as DataBase).BaseSale
                          where sale.GoodId == mostPopularGood.Item1
                          select sale.BuyerId).Contains(buyer.Id)
                   select buyer;
        }

        /// <summary>
        /// Возвращает список покупок, совершенных покупателями во всех городах,
        /// отличных от города их проживания.
        /// </summary>
        /// <param name="dataBase"></param>
        /// <returns></returns>
        public IEnumerable<Sale> GetOtherCitySales(IDataBase dataBase)
        {
            // Сортировка по покупкам и покупателям с номером магазина, где была совершена покупка.
            var saleAndShop = from sale in (dataBase as DataBase).BaseSale
                              from buyer in (dataBase as DataBase).BaseBuyer
                              where sale.BuyerId == buyer.Id
                              select (Sale: sale, Buyer: buyer, Shop: sale.ShopId);

            var saleBuyer = new List<Sale>(); // Лист для записи покупок.

            // Поиск покупателей, совершивших покупку в другом городе.
            foreach (var el in saleAndShop)
                foreach (var item in (dataBase as DataBase).BaseShop)
                    if (item.Id == el.Shop && el.Buyer.City != item.City)
                        saleBuyer.Add(el.Sale);

            return saleBuyer;
        }

        /// <summary>
        /// Возвращает общую стоимость покупок, совершенных всеми
        /// покупателями
        /// </summary>
        /// <param name="dataBase"></param>
        /// <returns></returns>
        public long GetTotalSalesValue(IDataBase dataBase)
            => (from sale in (dataBase as DataBase).BaseSale
                from good in (dataBase as DataBase).BaseGood
                where sale.GoodId == good.Id
                select sale.GoodCount * good.Price).Sum();
    }
}
