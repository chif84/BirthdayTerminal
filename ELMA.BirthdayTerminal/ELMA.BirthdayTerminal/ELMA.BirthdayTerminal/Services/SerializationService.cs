using ELMA.BirthdayTerminal.Services;
using Newtonsoft.Json;

[assembly: Xamarin.Forms.Dependency(typeof(SerializationService))]
namespace ELMA.BirthdayTerminal.Services
{
    /// <summary>
    /// Сервис сериализации
    /// </summary>
    public class SerializationService : ISerializationService
    {
        /// <summary>
        /// Десериализации
        /// </summary>
        public T Deserialize<T>(string json) where T : class
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Сериализации
        /// </summary>
        public string Serialize<T>(T obj) where T : class
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
