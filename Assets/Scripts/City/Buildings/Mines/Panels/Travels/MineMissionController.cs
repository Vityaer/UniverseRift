using AssetKits.ParticleImage;
using Models.Fights.Campaign;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace City.Buildings.Mines.Panels.Travels
{
    public class MineMissionController : MonoBehaviour
    {
        [SerializeField] private Button _selectButton;
        [SerializeField] private TMP_Text _missionLabel;
        [SerializeField] private GameObject _completePanel;
        [SerializeField] private ParticleImage _particle;

        private CompositeDisposable _disposables = new();
        private ReactiveCommand<MineMissionController> _onSelect = new();
        private StatusMission _status = StatusMission.NotOpen;
        private MineMissionData _missionData;
        private MissionModel _missionModel;

        public IObservable<MineMissionController> OnSelect => _onSelect;
        public StatusMission Status => _status;
        public MineMissionData MissionData => _missionData;
        public MissionModel MissionModel => _missionModel;

        private void Awake()
        {
            _selectButton.OnClickAsObservable().Subscribe(_ => OnClick()).AddTo(_disposables);
        }

        public void SetData(
            MineMissionData mineMissionData,
            MissionModel missionModel,
            StatusMission statusMission,
            string dateTimeCreate
            )
        {
            _status = statusMission;
            _missionData = mineMissionData;
            _missionModel = missionModel;
            if (_status != StatusMission.NotOpen)
            {
                DateTime startDateTime = TimeUtils.ParseTime(dateTimeCreate);
                var dateTimeRefresh = startDateTime.AddHours(Constants.Game.MINE_MISSION_REFRESH_HOURS);
                var deltaTime = dateTimeRefresh - DateTime.UtcNow;
                _missionLabel.text = $"{deltaTime.Hours}h. {deltaTime.Minutes}m.";
            }

            UpdateUI();
        }

        public void SetLabel(string text)
        {
            _missionLabel.text = text;
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
                    _missionLabel.text = string.Empty;
                    gameObject.SetActive(false);
                    break;
                case StatusMission.Open:
                    _particle.Play();
                    gameObject.SetActive(true);
                    break;
                case StatusMission.Complete:
                    _completePanel.SetActive(true);
                    gameObject.SetActive(true);
                    _particle.Stop();
                    break;
            }
        }

        private void OnClick()
        {
            _onSelect.Execute(this);
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}
