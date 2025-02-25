using Models.City.Mines;
using System;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;
using Utils;
using VContainer;
using VContainerUi.Abstraction;

namespace City.Buildings.Mines
{
    public class PlaceForMine : MonoBehaviour
    {
        public string Id;
        public List<MineType> Types = new();
        public Button PlaceButton;
        public Image MineIcon;
        public LocalizeStringEvent LevelText;

        private MineModel _mineModel;
        private MineData _mineData;
        private CompositeDisposable _disposables = new();
        private ReactiveCommand<PlaceForMine> _onClick = new();

        public MineModel MineModel => _mineModel;
        public MineData MineData => _mineData;
        public IObservable<PlaceForMine> OnClick => _onClick;

        private void Awake()
        {
            Clear();
        }

        protected void Start()
        {
            PlaceButton.OnClickAsObservable().Subscribe(_ => OpenPanelForCreateMine()).AddTo(_disposables);
        }

        public void SetData(MineModel mineModel, MineData mineData)
        {
            _mineModel = mineModel;
            _mineData = mineData;
            UpdateUi();
        }

        public void LevelUp()
        {
            _mineData.Level += 1;
            UpdateUi();
        }

        public void Clear()
        {
            MineIcon.sprite = null;
            MineIcon.enabled = false;
            LevelText.gameObject.SetActive(false);
            _mineModel = null;
            _mineData = null;
        }

        private void UpdateUi()
        {
            MineIcon.sprite = _mineModel.SpritePath.LoadSpriteFromResources();
            MineIcon.enabled = true;
            LevelText.gameObject.SetActive(true);
            if(LevelText.StringReference.TryGetValue("Level", out var variable))
            {
                var stringVariable = variable as IntVariable;
                stringVariable.Value = _mineData.Level;
            }
        }

        public void OpenPanelForCreateMine()
        {
            _onClick.Execute(this);
        }

        public void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}