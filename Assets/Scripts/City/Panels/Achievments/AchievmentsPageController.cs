using City.Achievements;
using Common;
using Db.CommonDictionaries;
using Models.Achievments;
using Models.Common;
using Models.Data;
using System.Collections.Generic;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace City.Buildings.Requirement
{
    public class AchievmentsPageController : UiPanelController<AchievmentsPanelView>
    {
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly IObjectResolver _objectResolver;

        [SerializeField] protected List<GameAchievment> _achievments;

        private List<AchievmentData> RequirementSaves = new List<AchievmentData>();

        protected override void OnLoadGame()
        {
            //LoadData(GameController.GetPlayerGame.saveMainRequirements);

            //_achievments = new List<GameAchievment>(_commonDictionaries.Achievments.Count);

            //foreach (var achievmentModel in _commonDictionaries.Achievments)
            //{
            //    var gameAchievment = new GameAchievment(achievmentModel.Value, new AchievmentData());
            //    var achievmentUi = Object.Instantiate(View.Prefab, View.Content);
            //    achievmentUi.SetData(gameAchievment, View.Scroll);
            //    _objectResolver.Inject(achievmentUi);
            //    _achievments.Add(gameAchievment);

            //    achievmentUi.ObserverOnChange.Subscribe(_ => SaveData()).AddTo(Disposables);
            //}
        }

        public void LoadData(List<AchievmentData> achievmentSaves)
        {
            for (int i = 0; i < achievmentSaves.Count; i++)
            {
                var achievment = _achievments.Find(achiev => achiev.Id == achievmentSaves[i].ModelId);
                if (achievment != null)
                {
                    achievment.SetProgress(achievmentSaves[i].CurrentStage, achievmentSaves[i].Progress);
                }
                else
                {
                    achievmentSaves.Remove(achievmentSaves[i]);
                }
            }
            CreateRequrements();
            OnAfterLoadData();
        }

        protected virtual void SaveData()
        {
            //_commonGameData.allRequirement
            //GameController.GetPlayerGame.SaveMainRequirements(_achievments);
        }

        protected void CreateRequrements()
        {
            //AchievmentView currentTask = null;
            //foreach (var task in _achievments)
            //{
            //    currentTask = CreateRequirementUI();
            //    currentTask.SetData(task);
            //    currentTask.RegisterOnChange(SaveData);
            //    currentTask.SetScroll(scrollRectController);
            //    listTaskUI.Add(currentTask);
            //}
        }

        protected virtual void OnAfterLoadData() { }

        private AchievmentView CreateRequirementUI()
        {
            var result = Object.Instantiate(View.Prefab, View.Content);
            //_achievmentsUi.Add(result);
            return result;
        }

        [ContextMenu("Clear all task")]
        public void ClearAllTask()
        {
            //for (int i = 0; i < listTaskUI.Count; i++)
            //{
            //    listTaskUI[i].Restart();
            //}
        }

        //Test and check
        [ContextMenu("Check all")]
        public void CheckAll()
        {
            for (int i = 0; i < _achievments.Count - 1; i++)
            {
                for (int j = i + 1; j < _achievments.Count; j++)
                {
                    if (_achievments[i].Id == _achievments[j].Id)
                    {
                        Debug.Log(string.Concat("two Requirement with equals ID: ", _achievments[i].Id.ToString(), " ,i: ", i.ToString(), " ,j: ", j.ToString()));
                    }
                }
            }
        }


    }
}