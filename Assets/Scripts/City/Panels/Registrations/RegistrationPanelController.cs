using Cysharp.Threading.Tasks;
using Models.Common;
using Network.DataServer;
using Network.DataServer.Messages;
using UiExtensions.Panels;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace City.Panels.Registrations
{
    public class RegistrationPanelController : UiPanelController<RegistrationPanelView>, IInitializable
    {
        private const int NAME_LENGTH_MIN = 5;
        private const string DEFAULT_AVATAR_PATH = "Assets/Resources/UI/HeroIcons/1129.png";

        [Inject] private readonly IUiMessagesPublisherService m_uiMessagesPublisher;
        [Inject] private readonly CommonGameData m_commonGameData;

        private bool m_isDone = false;
        private string m_playerInfo;

        public new void Initialize()
        {
            View.InputFieldNewNamePlayer.onValueChanged.AddListener(OnChangeNewName);
            View.StartRegistrationButton.OnClickAsObservable().Subscribe(_ => StartRegistration())
                .AddTo(Disposables);
            base.Initialize();
        }

        public async UniTask RegistrationAsync()
        {
            m_uiMessagesPublisher.OpenWindowPublisher.OpenWindow<RegistrationPanelController>(openType: OpenType.Additive);
            await UniTask.WaitUntil(() => m_isDone);
        }

        private void OnChangeNewName(string newName)
        {
            if (!newName.Equals(m_playerInfo))
            {
                bool enoughLength = newName.Length > NAME_LENGTH_MIN;
                View.StartRegistrationButton.interactable = enoughLength;
            }
            else
            {
                View.StartRegistrationButton.interactable = false;
            }
        }

        private void StartRegistration()
        {
            PlayerPrefs.DeleteAll();
            var newName = View.InputFieldNewNamePlayer.text;
            View.InputFieldNewNamePlayer.interactable = false;
            View.StartRegistrationButton.interactable = false;
            CreateAccount(newName).Forget();
        }

        private async UniTaskVoid CreateAccount(string name)
        {
            PlayerRegistration message = new PlayerRegistration { Name = name, AvatarPath = DEFAULT_AVATAR_PATH };
            string result = await DataServer.PostData(message);

            if (int.TryParse(result, out int id))
            {
                PlayerPrefs.DeleteAll();
                m_commonGameData.Init(id).Forget();
                m_isDone = true;
                Close();
            }
            else
            {
                View.InputFieldNewNamePlayer.interactable = true;
                View.StartRegistrationButton.interactable = true;
            }
        }

        public override void Dispose()
        {
            View.InputFieldNewNamePlayer.onValueChanged.RemoveAllListeners();
            base.Dispose();
        }

    }
}