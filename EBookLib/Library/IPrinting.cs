namespace EBookLib
{
    /// <summary>
    /// Используется для отслеживания вывода информации.
    /// </summary>
    public interface IPrinting
    {
        /// <summary>
        /// Событие для оповещения вывода информации о книге на экран.
        /// </summary>
        event EventHandler OnPrint; 

        /// <summary>
        /// Метод для вызова события OnPrint.
        /// </summary>
        public void Print();
    }
}
