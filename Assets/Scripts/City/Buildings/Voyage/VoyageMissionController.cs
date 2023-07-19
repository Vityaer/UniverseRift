using Models.Fights.Campaign;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.Voyage
{
    public class VoyageMissionController : MonoBehaviour, IDisposable
    {
        [SerializeField] private Button _selectButton;
        [SerializeField] private StatusMission _status = StatusMission.NotOpen;

        private int _numMission = 0;
        private CompositeDisposable _disposables = new CompositeDisposable();
        private ReactiveCommand<int> _onSelect = new ReactiveCommand<int>();

        public IObservable<int> OnSelect => _onSelect;

        private void Awake()
        {
            //_selectButton.OnClickAsObservable().Subscribe(_ => OnClick()).AddTo(_disposables);   
        }

        public void OpenMission()
        {
            throw new NotImplementedException();
        }

        public void SetData(int numMission, StatusMission newStatus)
        {
            _numMission = numMission;
            _status = newStatus;
        }

        public void SetStatus(StatusMission newStatus)
        {
            _status = newStatus;
        }

        private void OnClick()
        {
            _onSelect.Execute(_numMission);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

    }
}