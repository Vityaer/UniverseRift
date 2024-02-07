using AssetKits.ParticleImage;
using Models.Fights.Campaign;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.Voyage
{
    public class VoyageMissionController : MonoBehaviour, IDisposable
    {
        [SerializeField] private Button _selectButton;
        [SerializeField] private TMP_Text _missionIndexText;
        [SerializeField] private StatusMission _status = StatusMission.NotOpen;
        [SerializeField] private GameObject _completePanel;
        [SerializeField] private ParticleImage _particle;

        private int _numMission = 0;
        private CompositeDisposable _disposables = new CompositeDisposable();
        private ReactiveCommand<int> _onSelect = new ReactiveCommand<int>();

        public IObservable<int> OnSelect => _onSelect;
        public StatusMission Status => _status;

        private void Awake()
        {
            _selectButton.OnClickAsObservable().Subscribe(_ => OnClick()).AddTo(_disposables);   
        }

        public void SetData(int numMission, StatusMission newStatus)
        {
            _numMission = numMission;
            _status = newStatus;
            _missionIndexText.text = $"{numMission}";
        }

        public void SetLabel(string text)
        {
            _missionIndexText.text = text;
        }

        public void SetStatus(StatusMission newStatus)
        {
            _status = newStatus;
            UpdateUI();
        }

        private void UpdateUI()
        {
            switch (_status)
            {
                case StatusMission.NotOpen:
                    _completePanel.SetActive(false);
                    _particle.Stop();
                    break;
                case StatusMission.Open:
                    _particle.Play();
                    break;
                case StatusMission.Complete:
                    _completePanel.SetActive(true);
                    _particle.Stop();
                    break;
            }
        }

        private void OnClick()
        {
            _onSelect.Execute(_numMission - 1);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

    }
}