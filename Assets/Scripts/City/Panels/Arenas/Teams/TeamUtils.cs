using Models.Arenas;
using Newtonsoft.Json;
using UnityEngine;

namespace City.Panels.Arenas.Teams
{
    public static class TeamUtils
    {
        private static JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        public static TeamContainer LoadTeam(string teamKey)
        {
            TeamContainer result = null;
            if (PlayerPrefs.HasKey(teamKey))
            {
                var teamJson = PlayerPrefs.GetString(teamKey);
                result = JsonConvert.DeserializeObject<TeamContainer>(teamJson, settings);
            }

            if (result == null)
                result = new TeamContainer(teamKey);

            return result;
        }

        public static void SaveTeam(this TeamContainer teamContainer)
        {
            var json = JsonConvert.SerializeObject(teamContainer, Constants.Common.SerializerSettings);
            PlayerPrefs.SetString(teamContainer.Id, json);
        }
    }
}
