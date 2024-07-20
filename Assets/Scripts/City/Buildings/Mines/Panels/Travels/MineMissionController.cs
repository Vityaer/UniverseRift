using AssetKits.ParticleImage;
using Models.Fights.Campaign;
using System;
using System.Globalization;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

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
            StatusMission statusMission
            )
        {
            _status = statusMission;
            _missionData = mineMissionData;
            _missionModel = missionModel;
            if (_status != StatusMission.NotOpen)
            {
                DateTime startDateTime;
                try
                {
                    startDateTime = DateTime.ParseExact(
                    mineMissionData.DateTimeCreate,
                    Constants.Common.DateTimeFormat,
                    CultureInfo.InvariantCulture
                    );
                }
                catch
                {
                    startDateTime = DateTime.Parse(mineMissionData.DateTimeCreate);
                }

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
            _onSelect.Execute(this);
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}
