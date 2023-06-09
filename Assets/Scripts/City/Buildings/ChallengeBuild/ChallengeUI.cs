using Fight;
using Models.Fights.Challenge;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.ChallengeBuild
{
    public class ChallengeUI : MonoBehaviour
    {
        [SerializeField] private Text textName;
        [SerializeField] private Image backgroundChallenge;
        [SerializeField] private GameObject btnOpen;
        [SerializeField] private GameObject imageIsDone;
        [SerializeField] private ChallengeModel challenge;
        [SerializeField] private ChallengeBuild challengeBuild;
        public void SetData(ChallengeModel challenge, ChallengeBuild challengeBuild)
        {
            this.challengeBuild = challengeBuild;
            this.challenge = challenge;
            UpdateUI();
        }
        public void UpdateUI()
        {
            textName.text = challenge.Name;
            backgroundChallenge.sprite = LocationController.Instance.GetBackgroundForMission(challenge.Mission.Location);
            UpdateControllersUI();
        }
        public void UpdateControllersUI()
        {
            imageIsDone.SetActive(challenge.IsDone);
            btnOpen.SetActive(!challenge.IsDone);
        }
        public void OpenChallenge()
        {
            challengeBuild.OpenChallenge(challenge);
        }
    }
}