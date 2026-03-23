using System;
using Common;
using Common.Db.CommonDictionaries;
using Common.Inventories.Splinters;
using Models;
using Models.Heroes;
using Models.Inventory.Splinters;
using TMPro;
using UIController.Animations;
using UIController.Cards;
using UIController.Inventory;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils.DataCheckUtils;
using VContainerUi.Abstraction;

namespace UIController.ItemVisual
{
    public class SubjectCell : UiView, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerExitHandler,
        IPointerEnterHandler
    {
        public Image Image;
        public Image Background;
        public Backlight Backlight;
        public TMP_Text Amount;
        public Button Button;
        public SimpleAnimations Animations;

        [SerializeField] private GameObject m_splinterIcon;
        [SerializeField] private RatingHero m_ratingHero;
        [SerializeField] private CardDetailImageView m_race;

        private ReactiveCommand<SubjectCell> m_onSelect = new();
        private CompositeDisposable m_disposables = new();

        private CommonDictionaries m_commonDictionaries;
        private ScrollRect m_scroll;

        [SerializeField] private string m_subjecyType; 
        [SerializeField] private BaseObject m_subject;
        public BaseObject Subject { get => m_subject; private set => m_subject = value; }
        public IObservable<SubjectCell> OnSelect => m_onSelect;

        protected override void Awake()
        {
            if (Backlight != null && Backlight.backlight != null)
            {
                Backlight.backlight.SetActive(false);
            }
        }

        protected override void Start()
        {
            Button.OnClickAsObservable().Subscribe(_ => OnClick()).AddTo(m_disposables);
        }

        public void Init(ScrollRect scrollRect, CommonDictionaries commonDictionaries)
        {
            m_scroll = scrollRect;
            m_commonDictionaries = commonDictionaries.ThrowIfArgumentNull();
        }

        public void SetData<T>(T baseObject) where T : BaseObject
        {
            Subject = baseObject;
            m_subjecyType = typeof(T).Name;
            Image.enabled = true;
            Image.sprite = baseObject.Image;
            Amount.text = baseObject.EqualsZero ? string.Empty : baseObject.ToString();
            SetInfoIcon(baseObject);
            SetRacingIcon(baseObject);
            SetRating(baseObject);
            gameObject.SetActive(true);
        }

        private void SetRating<T>(T baseObject) where T : BaseObject
        {
            switch (baseObject)
            {
                case GameItem gameItem:
                    if(m_commonDictionaries.ItemSets.TryGetValue(gameItem.Model.SetName, out ItemSet itemSet))
                    {
                        m_ratingHero.ShowRating(itemSet.Rating);
                    }
                    break;
                case GameSplinter splinter:
                    if (splinter.Model.ModelIds.Count == 1)
                    {
                        if(m_commonDictionaries.Heroes.TryGetValue(splinter.Model.ModelIds[0].ModelId, out HeroModel hero))
                        {
                            m_ratingHero.ShowRating(hero.General.Rating);
                        }
                    }
                    break;
            }
        }

        private void SetRacingIcon<T>(T baseObject) where T : BaseObject
        {
            m_race.gameObject.SetActive(false);

            switch (baseObject)
            {
                case GameSplinter splinter:
                    if (splinter.typeSplinter == SplinterType.Hero)
                    {
                        if (splinter.Model.ModelIds.Count == 1)
                        {
                            if(m_commonDictionaries.Heroes.TryGetValue(splinter.Model.ModelIds[0].ModelId, out HeroModel hero))
                            {
                                m_race.gameObject.SetActive(true);
                                m_race.SetData(m_commonDictionaries.Races[hero.General.Race].SpritePath);
                            }
                        }
                    }
                    break;
            }
        }

        private void SetInfoIcon<T>(T baseObject) where T : BaseObject
        {
            if (!m_splinterIcon)
            {
                Debug.LogError($"m_splinterIcon is null", gameObject);
                return;
            }

            switch (baseObject)
            {
                case GameSplinter:
                    m_splinterIcon.SetActive(true);
                    break;
                default:
                    m_splinterIcon.SetActive(false);
                    break;
            }
        }

        private void OnClick()
        {
            if (Subject != null)
                m_onSelect.Execute(this);
        }

        public void Clear()
        {
            Image.enabled = false;
            Amount.text = string.Empty;
            Subject = null;
            //Backlight?.backlight?.SetActive(false);
            gameObject.SetActive(false);
        }

        public void Disable()
        {
            Clear();
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            m_scroll?.OnBeginDrag(eventData);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            m_scroll?.OnDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            m_scroll?.OnEndDrag(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Animations?.Squezze();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Animations?.ToDefaultSize();
        }

        protected override void OnDestroy()
        {
            m_disposables.Dispose();
            base.OnDestroy();
        }
    }
}