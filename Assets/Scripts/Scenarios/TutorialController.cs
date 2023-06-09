using Common;
using Common.Resourses;
using Models;
using UnityEngine;

namespace Scenarios
{
    public class TutorialController : MonoBehaviour
    {
        SimpleBuildingModel tutorial = null;
        private const string NAME_RECORD_STAGE = "Stage";
        private int stage = 0;

        void Start()
        {
            GameController.Instance.RegisterOnLoadGame(OnLoadGame);
        }

        void OnLoadGame()
        {
            tutorial = GameController.GetCitySave.tutorial;
            stage = tutorial.GetRecordInt(NAME_RECORD_STAGE);
            switch (stage)
            {
                case 0:
                    StartTutorial();
                    break;
            }
        }

        private void StartTutorial()
        {
            stage += 1;
            GameController.Instance.AddResource(new Resource(TypeResource.SimpleHireCard, 50, 0));
            GameController.Instance.AddResource(new Resource(TypeResource.SimpleTask, 50, 0));
            GameController.Instance.AddResource(new Resource(TypeResource.SpecialTask, 50, 0));
            GameController.Instance.AddResource(new Resource(TypeResource.SpecialHireCard, 50, 0));
            SaveStage();
        }

        private void SaveStage()
        {
            tutorial.SetRecordInt(NAME_RECORD_STAGE, stage);
        }
    }
}