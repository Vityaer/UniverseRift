using Models.Heroes.Skills;
using System;
using UIController;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace City.TrainCamp
{
    public class SkillCell : MonoBehaviour
    {
        [SerializeField] private Image Icon;

        private ReactiveCommand<SkillCell> _onSelect = new();
        private Skill _data;

        public Skill Data => _data;
        public IObservable<SkillCell> OnSelect => _onSelect;

        public void SetData(Skill skill)
        {
            _data = skill;
            Icon.sprite = _data.Icon;
            gameObject.SetActive(true);
        }

        public void OpenDetail()
        {
        }

        public void OffObject()
        {
            _data = null;
            gameObject.SetActive(false);
        }
    }
}