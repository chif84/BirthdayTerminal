namespace ELMA.BirthdayTerminal.Services
{
    /// <summary>
    /// Сервис локального хранения данных
    /// </summary>
    public interface ILocalStorageService
    {
        /// <summary>
        /// Поместить сериализованный объект
        /// </summary>
        void Set(string key, byte[] obj);

        /// <summary>
        /// Поместить объект
        /// </summary>
        void Set<T>(string key, T obj) where T : class;

        /// <summary>
        /// Получить объект
        /// </summary>
        T Get<T>(string key) where T : class;
    }
}
