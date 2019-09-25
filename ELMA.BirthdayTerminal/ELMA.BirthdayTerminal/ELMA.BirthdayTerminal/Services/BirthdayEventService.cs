using ELMA.BirthdayTerminal.Models;
using ELMA.BirthdayTerminal.Models.Elma;
using ELMA.BirthdayTerminal.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(BirthdayEventService))]
namespace ELMA.BirthdayTerminal.Services
{
    /// <summary>
    /// Сервис работы с поздравлениями
    /// </summary>
    public class BirthdayEventService : IBirthdayEventService
    {
        private Guid userTypeUid = new Guid("cfdeb03c-35e9-45e7-aad8-037d83888f73");

        private IConnectionService connectionService;
        private ISerializationService serializationService;
        private ISecurityService securityService;

        private List<BirthdayEvent> todayEvents = new List<BirthdayEvent>();

        private List<BirthdayEvent> nextEvents = new List<BirthdayEvent>();

        /// <summary>
        /// Ctor
        /// </summary>
        public BirthdayEventService()
        {
            connectionService = DependencyService.Get<IConnectionService>();
            serializationService = DependencyService.Get<ISerializationService>();
            securityService = DependencyService.Get<ISecurityService>();
        }

        /// <summary>
        /// Запросить события из ELMA
        /// </summary>
        public bool SyncronizeEvents(GlobalSettings settings)
        {
            var path = settings.Host + @"/API/REST/Entity/Query?type=" + userTypeUid.ToString();

            try
            {
                var response = connectionService.SendGet(path, securityService.AuthToken);

                var users = serializationService.Deserialize<User[]>(response).ToList();

                #region Искуственные даты рождения

                users.ForEach(user =>
                {
                    if (user == users.Last() || user == users.First())
                    {
                        user.BirthDate = DateTime.Now.AddDays(1);
                    }
                    else
                    {
                        user.BirthDate = DateTime.Now;
                    }

                });

                #endregion

                todayEvents.Clear();
                nextEvents.Clear();

                var todayPairs = new List<Tuple<BirthdayEvent, User>>();
                var nextPairs = new List<Tuple<BirthdayEvent, User>>();

                users.ForEach(user =>
                {
                    if (user.BirthDate.Value.Date == DateTime.Today)
                    {
                        var bEvent = new BirthdayEvent(user);
                        todayEvents.Add(bEvent);
                        todayPairs.Add(new Tuple<BirthdayEvent, User>(bEvent, user));
                    }
                    else
                    {
                        var bEvent = new BirthdayEvent(user);
                        nextEvents.Add(bEvent);
                        nextPairs.Add(new Tuple<BirthdayEvent, User>(bEvent, user));
                    }

                });

                //загрузка фотографий
                SyncronizePhoto(settings, todayPairs);
                SyncronizePhoto(settings, nextPairs);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Получить сегодняшние события
        /// </summary>
        public List<BirthdayEvent> GetTodayEvents()
        {
            return todayEvents;
        }

        /// <summary>
        /// Получить будующие события
        /// </summary>
        public List<BirthdayEvent> GetNexEvents()
        {
            return nextEvents;
        }

        /// <summary>
        /// Загрузить фотографии
        /// </summary>
        private async void SyncronizePhoto(GlobalSettings settings, List<Tuple<BirthdayEvent, User>> pairs)
        {
            pairs.ForEach(pair =>
            {
                if (pair != null && pair.Item1 != null && pair.Item2 != null && pair.Item2.Photo != null && pair.Item2.Photo.Uid != null)
                {
                    var photoUid = pair.Item2.Photo.Uid.Value;

                    var path = settings.Host + @"/API/REST/Files/Download?uid=" + pair.Item2.Photo.Uid.ToString();

                    try
                    {
                        pair.Item1.Image = ImageSource.FromStream(() => new MemoryStream(connectionService.SendGet2(path, securityService.AuthToken)));
                    }
                    catch (Exception ex)
                    {
                    }
                }
            });
        }
    }
}
