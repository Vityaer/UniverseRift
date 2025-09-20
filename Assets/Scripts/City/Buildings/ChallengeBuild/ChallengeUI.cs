using Fight.Common;
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
    }
}