using Campaign;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UIController.Campaign
{
    public class CampaignChapterController : MonoBehaviour, IDisposable
    {
        [SerializeField] private Image Image;
        [SerializeField] private TMP_Text Name;
        [SerializeField] private CampaignChapterModel _chapter;
        [SerializeField] private bool IsOpen = false;
        [SerializeField] private Button Button;

        private CompositeDisposable _disposables = new CompositeDisposable();    
        private ReactiveCommand<CampaignChapterModel> _onSelect = new ReactiveCommand<CampaignChapterModel>();

        public IObservable<CampaignChapterModel> OnSelect => _onSelect;

        void Start()
        {
            Button.OnClickAsObservable().Subscribe(_ => Select()).AddTo(_disposables);
        }

        public void SetData(CampaignChapterModel chapter)
        {
            _chapter = chapter;
            Name.text = string.Concat(chapter.numChapter.ToString(), ". ", chapter.Name);
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

        public void Dispose()
        {
            _disposables.Dispose();
        }

    }
}