using UnityEngine;
using UnityEngine.UI;

public class ChallengeUI : MonoBehaviour
{
    [SerializeField] private Text textName;
    [SerializeField] private Image backgroundChallenge;
    [SerializeField] private GameObject btnOpen;
    [SerializeField] private GameObject imageIsDone;
    [SerializeField] private Challenge challenge;
    [SerializeField] private ChallengeBuild challengeBuild;
    public void SetData(Challenge challenge, ChallengeBuild challengeBuild)
    {
        this.challengeBuild = challengeBuild;
        this.challenge = challenge;
        UpdateUI();
    }
    public void UpdateUI()
    {
        textName.text = challenge.Name;
        backgroundChallenge.sprite = LocationController.Instance.GetBackgroundForMission(challenge.mission.Location);
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
