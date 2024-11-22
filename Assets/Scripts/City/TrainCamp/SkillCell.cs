using Models.Heroes.Skills;
using System;
using UIController;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace City.TrainCamp
{
    public class SkillCell : MonoBehaviour
    {
        [SerializeField] private Image Icon;
        [SerializeField] private Button Button;

        private ReactiveCommand<SkillCell> _onSelect = new();
        private Skill _data;
        private IDisposable _disposable;

        public Skill Data => _data;
        public IObservable<SkillCell> OnSelect => _onSelect;

        private void Awake()
        {
            _disposable = Button.OnClickAsObservable().Subscribe(_ => _onSelect.Execute(this));
        }

        public void SetData(Skill skill)
        {
            _data = skill;
            Icon.sprite = _data.SpritePath;
            gameObject.SetActive(true);
        }

        public void OffObject()
        {
            _data = null;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}