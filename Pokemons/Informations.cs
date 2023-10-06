using CsvHelper.Configuration.Attributes;

namespace PokemonTable
{
    /// <summary>
    /// Сведения о покемонах в стуктурированном ввиде
    /// </summary>
    public class InformationPokemon
    {
        [Name("#")]
        public int id { get; set; } // ID для каждого покемона

        [Name("Name")]
        public string? name { get; set; } // Имя каждого покемона

        [Name("Type 1")]
        public string? type1 { get; set; } // Тип, который определяет слабость/сопротивление атакам.

        [Name("Type 2")]
        public string? type2 { get; set; } // Некоторые покемоны двойного типа и имеют 2

        [Name("Total")]
        public int total { get; set; } // Сумма всех характеристик, которые идут после этого, общее руководство о том, насколько силен покемон.

        [Name("HP")]
        public int hp { get; set; } // Очки жизни или здоровье определяют, какой урон покемон может выдержать, прежде чем упадет в обморок.

        [Name("Attack")]
        public int attack { get; set; } // Базовый модификатор для обычных атак

        [Name("Defense")]
        public int defense { get; set; } // Базовое сопротивление урону от обычных атак.

        [Name("Sp. Atk")]
        public int spAtk { get; set; } // Специальная атака, базовый модификатор для специальных атак.

        [Name("Sp. Def")]
        public int spDef { get; set; } // Базовое сопротивление урону от специальных атак.

        [Name("Speed")]
        public int speed { get; set; } // Определяет, какой покемон атакует первым в каждом раунде.

        [Name("Generation")]
        public int generation { get; set; } // Поколение покемона.

        [Name("Legendary")]
        public string? legendary { get; set; } // Легендарный или нелегендарный покемон.
    }
}
