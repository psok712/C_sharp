using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;
using PokemonTable;
using PokemonDataHelper;

namespace IndependentWork1
{
    internal class Program
    {
        static void Main()
        {
            List<InformationPokemon> listPokemon;
            StatPokemon pokemon = new StatPokemon();
            string? path = "";
            int numb;
            do
            {
                ShowMenu();
                try
                {
                    if (int.TryParse(Console.ReadLine(), out numb) && numb > 0)
                        switch (numb)
                        {
                            case 1:
                                if (!FileAddress(out path))
                                {
                                    CheckEntry(out numb);
                                }
                                break;
                            case 2:
                                if (ReadFile(path, out listPokemon))
                                    pokemon.Legendary(listPokemon);
                                else
                                {
                                    CheckEntry(out numb);
                                }
                                break;
                            case 3:
                                if (ReadFile(path, out listPokemon))
                                    pokemon.Generation(listPokemon);
                                else
                                {
                                    CheckEntry(out numb);
                                }
                                break;
                            case 4:
                                if (ReadFile(path, out listPokemon))
                                    pokemon.MaxSpeed(listPokemon);
                                else
                                {
                                    CheckEntry(out numb);
                                }
                                break;
                            case 5:
                                if (ReadFile(path, out listPokemon))
                                    OutPutStatistics(listPokemon);
                                else
                                {
                                    CheckEntry(out numb);
                                }
                                break;
                            default: break;
                        }
                    else Console.WriteLine("Извините, некорректный ввод, попробуйте снова.");
                }
                catch (Exception) 
                {
                    Console.WriteLine("К сожалению произошла неизвестная ошибка. Повторите запрос.")
                }
            } while (numb != 6);
        }

        /// <summary>
        /// Проверка введенного пути на существование
        /// </summary>
        /// <returns></returns>
        static bool FileAddress(out string path)
        {
            Console.Write("Введите путь к файлу: ");
            path = Console.ReadLine();
            if (File.Exists(path))
                return true;
            Console.WriteLine("Извините, такого файла не существует.");
            return false;
        }

        /// <summary>
        /// Статистика по покемонам.
        /// </summary>
        /// <param name="listPokemon">Таблица данных.</param>
        static void OutPutStatistics(List<InformationPokemon> listPokemon)
        {
            StatPokemon statsPok = new StatPokemon();

            Console.WriteLine("a.");
            int[] countGen;
            statsPok.Generation(listPokemon, out countGen);
            for (int i = 1; i < countGen.Length; i++)
                Console.WriteLine($"{i} поколение содержит {countGen[i]} покемонов.");

            Console.WriteLine("b.");
            var valueAttack = statsPok.AttackMaxAndMinPokemon(listPokemon);
            Console.WriteLine($"Самый мощный покемон из таблицы: {valueAttack.maxAttack}.");
            Console.WriteLine($"Самый слабый покемон из таблицы: {valueAttack.minAttack}.");


            Console.WriteLine("c.");
            Console.WriteLine($"Всего {statsPok.CountPoisonBug(listPokemon)} ядовитых жуков в базе данных.");

            Console.WriteLine("d.");
            Console.WriteLine($"Всего {statsPok.CountGeneration(listPokemon)} покемонов второго поколения, у которых защита меньше 50.");
        }

        /// <summary>
        /// Чтение базы данных о покемонах.
        /// </summary>
        /// <param name="path">Путь расположения файла.</param>
        /// <returns>Данные в виде таблицы.</returns>
        static bool ReadFile(string path, out List<InformationPokemon> listPokemon)
        {
            listPokemon = new List<InformationPokemon>();
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8,
                Delimiter = ",",
            };
            try
            {
                using (var fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var textReader = new StreamReader(fs, Encoding.UTF8))
                    using (var csv = new CsvReader(textReader, configuration))
                    {
                        var data = csv.GetRecords<InformationPokemon>();

                        foreach (var person in data)
                        {
                            listPokemon.Add(person);
                        }
                    }
                }
            }
            catch (HeaderValidationException)
            {
                Console.WriteLine("Извините, но вы ввели некорректно структурированный файл.");
                return false;
            }
            catch(ArgumentException)
            {
                Console.WriteLine("Ой, похоже вы забыли ввести путь к файлу(");
                return false;
            }
            catch (FileNotFoundException) 
            {
                Console.WriteLine("Ой, похоже вы забыли ввести путь к файлу(");
                return false;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Ввывод пунктов меню на экран
        /// </summary>
        static void ShowMenu()
        {
            Console.WriteLine("Меню:");
            Console.WriteLine("1.Ввести адрес файла.");
            Console.WriteLine("2.Вывести группы легендарных и нелегендарных покемонов.");
            Console.WriteLine("3.Вывести список покемонов относящихся к одному поколению.");
            Console.WriteLine("4.Вывести покемонов с максимальной скоростью.");
            Console.WriteLine("5.Вывести статистику по покемонам.");
            Console.WriteLine("6.Выйти из меню.");

        }

        /// <summary>
        /// Проверка введенных данных
        /// </summary>
        /// <returns></returns>
        static void CheckEntry(out int numb)
        {
            Console.WriteLine("Если желаете вернуться в главное меню нажмите 0, иначе для выхода нажмите 6.");
            bool flag = false;
            do
            {
                if (flag)
                    Console.WriteLine("К сожалению, вы опять ввели некорректное значение(\nЕсли желаете вернуться в главное меню нажмите 0, иначе для выхода нажмите 6.");
                flag = true;
            } while (!int.TryParse(Console.ReadLine(), out numb) || (numb != 0 && numb != 6));
        }
    }
}
