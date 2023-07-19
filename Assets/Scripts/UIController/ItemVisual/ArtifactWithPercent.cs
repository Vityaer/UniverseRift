using TMPro;
using UIController.GameSystems;
using UnityEngine;

namespace UIController.ItemVisual
{
    public class ArtifactWithPercent : MonoBehaviour
    {
        public SubjectCell artifactInfo;
        public TextMeshProUGUI textPercent;

        public void SetData(string ID, float percent)
        {
            //artifactInfo.SetItem(ArtifactSystem.Instance.GetArtifact(ID));
            textPercent.text = string.Concat(percent.ToString(), "%");
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}