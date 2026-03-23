using System;
using Models.City.Mines;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using VContainerUi.Abstraction;

namespace City.Buildings.Mines
{
    public class MineCard : UiView
    {
        public GameObject Panel;
        public Image MineImage;
        public Image OutLight;
        public TextMeshProUGUI CountRequirementText;
        public Button MainButton;

        private bool m_isOpen;
        private MineModel m_mineModel;
        private GameMineRestriction m_mineRestriction;
        private ReactiveCommand<MineCard> m_onSelect = new();

        public IObservable<MineCard> OnSelect => m_onSelect;

        public bool IsOpen => m_isOpen;
        public bool GetCanCreateFromCount => m_mineRestriction.CurrentCount < m_mineRestriction.MaxCount;

        public MineModel Model => m_mineModel;

        protected override void Awake()
        {
            MainButton.onClick.AddListener(Click);
            base.Awake();
        }

        public void SetData(MineModel mineModel, GameMineRestriction mineRestriction)
        {
            m_mineModel = mineModel;
            m_mineRestriction = mineRestriction;

            string path = m_mineModel.SpritePath.ReplaceForResources();
            path = path.Replace(".png", "");
            Sprite sprite = Resources.Load<Sprite>(path);
            MineImage.sprite = sprite;
            CountRequirementText.text =
                FunctionHelp.AmountFromRequireCount(m_mineRestriction.CurrentCount, m_mineRestriction.MaxCount);
            Panel.SetActive(true);

            m_isOpen = true;
        }

        private void Click()
        {
            if (m_mineRestriction.CurrentCount < m_mineRestriction.MaxCount) m_onSelect.Execute(this);
        }

        public void Select()
        {
            OutLight.enabled = true;
        }

        public void Diselect()
        {
            OutLight.enabled = false;
        }

        public void ShowPanel()
        {
            m_isOpen = true;
            Panel.SetActive(true);
        }
        public void HidePanel()
        {
            m_isOpen = false;
            Panel.SetActive(false);
        }
    }
}