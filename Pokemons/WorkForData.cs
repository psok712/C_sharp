using CsvHelper;
using PokemonTable;
using System.Text;

namespace PokemonDataHelper
{
    /// <summary>
    /// Работа с данными из таблицы покемонов
    /// </summary>
    internal class StatPokemon
    {
        /// <summary>
        /// Преобразует строку таблицы с данными в string с разделителями в виде запятой.
        /// </summary>
        /// <param name="inf">Строка таблицы данных о покемоне.</param>
        /// <returns>Отформатированную строку таблицы.</returns>
        private string ListToString(InformationPokemon inf) => $"{inf.id},{inf.name},{inf.type1},{inf.type2},{inf.total},{inf.hp},{inf.attack},{inf.defense},{inf.spAtk},{inf.spDef},{inf.speed},{inf.generation},{inf.legendary}";

        /// <summary>
        /// Собирает статистику по легендраным и нелегендарным покемонам, выводит ее на экран и сохраняет списки в файлы.
        /// </summary>
        /// <param name="listPokemon">Таблица с данными.</param>
        public void Legendary(List<InformationPokemon> listPokemon)
        {
            int countLegend = 0; // счетчик легендарных покемонов
            int countNotLegend = 0; // счетчик нелегендарных покемонов
            foreach (var el in listPokemon)
            {
                if (el.legendary == "True")
                    using (StreamWriter sw = new StreamWriter("Pokemon-Legendary.csv", true, Encoding.UTF8))
                    {
                        sw.WriteLine(ListToString(el));
                        countLegend++;
                    }
                else
                    using (StreamWriter sw = new StreamWriter("Pokemon-Usual.csv", true, Encoding.UTF8))
                    {
                        sw.WriteLine(ListToString(el));
                        countNotLegend++;
                    }
            }

            Console.WriteLine($"Легендарных покемонов в таблице - {countLegend}\nДанные о легендарных сохранены по пути: {Path.GetFullPath("Pokemon-Legendary.csv")}");
            Console.WriteLine($"Нелегендарных покемонов в таблице - {countNotLegend}\nДанные о нелегендарных сохранены по пути: {Path.GetFullPath("Pokemon-Usual.csv")}");
        }

        /// <summary>
        /// Статистика покемонов по поколениям. Выводит ее на экран и записывает каждое поколение в свой файл.
        /// </summary>
        /// <param name="listPokemon">Таблица с данными.</param>
        public void Generation(List<InformationPokemon> listPokemon)
        {
            var orderedGen = from g in listPokemon // сортировка коллекции по generation по возрастанию 
                             orderby g.generation
                             select g;

            int? minGen = orderedGen.ElementAt(0).generation; // минимальное поколение
            List<InformationPokemon> list = new List<InformationPokemon>(); // коллекция для покемонов одного поколения
            int? maxAttack = int.MinValue;
            InformationPokemon maxAttackPok = new InformationPokemon();

            foreach (var elGen in orderedGen)
            {
                if (elGen.generation == minGen)
                {
                    list.Add(elGen); //заполнение коллекции элементами одного поколения
                    if (elGen.attack > maxAttack) // поиск покемона с максимальной атакой из поколения
                    {
                        maxAttack = elGen.attack;
                        maxAttackPok = elGen;
                    }
                }
                else
                {
                    Console.WriteLine($"Покемон с максимальной атакой из поколения {minGen}: {ListToString(maxAttackPok)}");
                    foreach (var el in list)
                    {
                        Console.WriteLine(ListToString(el));
                        using (StreamWriter sw = new StreamWriter($"Pokemon-Gen-{minGen}.csv", true, Encoding.UTF8))
                        {
                            sw.WriteLine(ListToString(el));
                        }
                    }
                    Console.WriteLine();
                    minGen++;
                    maxAttack = int.MinValue;
                    list = new List<InformationPokemon>();
                }
            }
            if (list.Count > 0) //вывод для последнего поколения
            {
                Console.WriteLine($"Покемон с максимальной атакой из поколения {minGen}: {ListToString(maxAttackPok)}");
                foreach (var el in list)
                {
                    Console.WriteLine(ListToString(el));
                    using (StreamWriter sw = new StreamWriter($"Pokemon-Gen-{minGen}.csv", true, Encoding.UTF8))
                    {
                        sw.WriteLine(ListToString(el));
                    }
                }
            }
        }

        /// <summary>
        /// Подсчет покемонов по поколениям.
        /// </summary>
        /// <param name="listPokemon">Таблица с данными.</param>
        /// <param name="countGen">Массив, куда занесутся результаты.</param>
        /// <returns>Вовзвращает массив с количеством покемонов в каждом поколении.</returns>
        public void Generation(List<InformationPokemon> listPokemon, out int[] countGen)
        {
            var orderedGen = from g in listPokemon // сортировка коллекции по generation по возрастанию 
                             orderby g.generation
                             select g;
            countGen = new int[orderedGen.ElementAt(orderedGen.Count() - 1).generation + 1]; // определение массива максимальным количеством поколений.

            try
            {
                int minGen = orderedGen.ElementAt(0).generation; // минимальное поколение
                foreach (var elGen in orderedGen)
                {
                    if (elGen.generation == minGen)
                        countGen[minGen]++;
                    else
                    {
                        minGen++;
                        countGen[minGen]++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Статистика покемонов с максимальной скоростью. Выводит ее на экран и запысывает в файл.
        /// </summary>
        /// <param name="listPokemon"></param>
        public void MaxSpeed(List<InformationPokemon> listPokemon)
        {
            int? maxSpeed = int.MinValue; // максимальная скорость у покемонов
            var pokemon = new InformationPokemon();
            foreach (var el in listPokemon)
                if (el.speed > maxSpeed) maxSpeed = el.speed;

            Console.WriteLine("Покемоны, чья скорость отличается от максимума не более, чем на 10 единиц:");
            foreach (var el in listPokemon)
            {
                if (el.speed >= maxSpeed - 10)
                {
                    using (StreamWriter sw = new StreamWriter("Speed-Pokemon.csv", true, Encoding.UTF8))
                    {
                        sw.WriteLine(ListToString(el));
                    }
                    Console.WriteLine(ListToString(el));
                }
            }
        }

        /// <summary>
        /// Поиск максимального и минимального по силе покемонов.
        /// </summary>
        /// <param name="listPokemon">Таблица с данными.</param>
        /// <returns>Кортеж значений максимального и минимального покемонов.</returns>
        public (string maxAttack, string minAttack) AttackMaxAndMinPokemon(List<InformationPokemon> listPokemon)
        {
            int maxAttack = int.MinValue;
            int minAttack = int.MaxValue;
            InformationPokemon maxPokemon = new InformationPokemon();
            InformationPokemon minPokemon = new InformationPokemon();

            foreach (var el in listPokemon)
            {
                if (maxAttack < el.attack)
                {
                    maxAttack = el.attack;
                    maxPokemon = el;
                }

                if (minAttack > el.attack)
                {
                    minAttack = el.attack;
                    minPokemon = el;
                }
            }

            return (ListToString(maxPokemon), ListToString(minPokemon));
        }

        /// <summary>
        /// Считает ядовитых жуков в таблице.
        /// </summary>
        /// <param name="listPokemon">Таблица данных.</param>
        /// <returns>Количество таких жуков.</returns>
        public int CountPoisonBug(List<InformationPokemon> listPokemon)
        {
            int count = 0;

            foreach (var el in listPokemon)
            {
                if ((el.type1 == "Poison" || el.type1 == "Bug") && (el.type2 == "Poison" || el.type2 == "Bug") && el.type1 != el.type2)
                    count++;
            }

            return count;
        }

        /// <summary>
        /// Считает покемонов 2 поколения, защита которых меньше 50.
        /// </summary>
        /// <param name="listPokemon">Таблица данных.</param>
        /// <returns>Количество подходящих покемонов.</returns>
        public int CountGeneration(List<InformationPokemon> listPokemon)
        {
            int count = 0;
            int numbGen = 2; // поколение
            int defence = 50; // защита 

            foreach (var el in listPokemon)
            {
                if (el.defense < defence && el.generation == numbGen)
                    count++;
            }

            return count;
        }
    }
}
