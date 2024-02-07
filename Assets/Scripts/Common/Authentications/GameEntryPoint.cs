using City.Panels.Registrations;
using Models.Common;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Common.Authentications
{
    public class GameEntryPoint : IInitializable
    {
        private const string PLAYER_ID_KEY = "PlayerId";

        [Inject] private readonly CommonGameData _commonGameData;
        [Inject] private readonly RegistrationPanelController _registrationPanelController;
        [Inject] private readonly RegistrationPanelView _registrationPanelView;

        public void Initialize()
        {
            if (!_registrationPanelView.OverrideId)
            {
                var playerId = GetPlayerId();
                if (playerId > 0)
                {
                    _commonGameData.Init(playerId).Forget();
                }
                else
                {
                    _registrationPanelController.OpenPanelRegistration();
                }
            }
            else
            {
                _commonGameData.Init(_registrationPanelView.PlayerId).Forget();
            }
        }

        private int GetPlayerId()
        {
            return PlayerPrefs.HasKey(PLAYER_ID_KEY) ? PlayerPrefs.GetInt(PLAYER_ID_KEY) : -1;
        }
    }
}
