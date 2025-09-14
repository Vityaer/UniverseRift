using Models.City.Mines;
using System;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;
using Utils;
using VContainer;
using VContainerUi.Abstraction;
using Vector3 = UnityEngine.Vector3;

namespace City.Buildings.Mines
{
    public class PlaceForMine : MonoBehaviour
    {
        public string Id;
        public List<MineType> Types = new();
        public Button PlaceButton;
        public Image MineIcon;
        public RectTransform PlaceRectTransform;
        public RectTransform MineBuildRectTransform;
        public LocalizeStringEvent LevelText;

        private MineModel _mineModel;
        private MineData _mineData;
        private CompositeDisposable _disposables = new();
        private ReactiveCommand<PlaceForMine> _onClick = new();

        [Header("Animations")]
        [SerializeField] private int m_startBuildHeight;
        [SerializeField] private float m_buildTime;
        [SerializeField] private RectTransform m_buildingPoint;
        [SerializeField] private Ease m_buildingEase;
        
        private Tween _tween;
        
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
        
        public void CreateMine(MineModel model, MineData data)
        {
            SetData(model, data);
            
            MineBuildRectTransform.position += new Vector3(0, m_startBuildHeight, 0);
            
            _tween.Kill();
            _tween = MineBuildRectTransform
                .DOMove(m_buildingPoint.position, m_buildTime)
                .SetEase(m_buildingEase);
        }

        public void SetCanBuild(bool canBuilding)
        {
            gameObject.SetActive(canBuilding);
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
            _tween.Kill();
            MineBuildRectTransform.position = m_buildingPoint.position;
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
            _tween.Kill();
            _disposables.Dispose();
        }
    }
}