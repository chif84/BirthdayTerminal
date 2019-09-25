namespace ELMA.BirthdayTerminal.Services
{
    /// <summary>
    /// Сервис сериализации
    /// </summary>
    public interface ISerializationService
    {
        /// <summary>
        /// Десериализации
        /// </summary>
        T Deserialize<T>(string json) where T : class;

        /// <summary>
        /// Сериализации
        /// </summary>
        string Serialize<T>(T obj) where T : class;
    }
}
