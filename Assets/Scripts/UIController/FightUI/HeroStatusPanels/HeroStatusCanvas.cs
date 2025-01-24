using UnityEngine;

namespace UIController.FightUI.HeroStatusPanels
{
    public class HeroStatusCanvas : MonoBehaviour
    {
        private Camera _camera;

        public void Start()
        {
            _camera = Camera.main;
        }
    }
}
