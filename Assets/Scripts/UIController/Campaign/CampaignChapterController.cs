using Campaign;
using LocalizationSystems;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace UIController.Campaign
{
    public class CampaignChapterController : MonoBehaviour
    {
        [SerializeField] private Image Image;
        [SerializeField] private LocalizeStringEvent Name;
        [SerializeField] private CampaignChapterModel _chapter;
        [SerializeField] private bool IsOpen = false;
        [SerializeField] private Button Button;

        private CompositeDisposable _disposables = new();    
        private ReactiveCommand<CampaignChapterModel> _onSelect = new();

        public IObservable<CampaignChapterModel> OnSelect => _onSelect;

        void Start()
        {
            Button.OnClickAsObservable().Subscribe(_ => Select()).AddTo(_disposables);
        }

        public void SetData(ILocalizationSystem localizationSystem, CampaignChapterModel chapter)
        {
            _chapter = chapter;
            Name.StringReference = localizationSystem
                .GetLocalizedContainer($"CampaignChapter{chapter.Name}Name");
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void Select()
        {
            if (IsOpen)
            {
                _onSelect.Execute(_chapter);
            }
        }

        public void Open()
        {
            Image.color = Color.white;
            IsOpen = true;
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }

    }
}