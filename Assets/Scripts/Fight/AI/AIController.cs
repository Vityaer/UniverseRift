using Fight.Misc;
using UnityEngine;

namespace Fight.AI
{
    public class AIController : MonoBehaviour
    {
        void Awake()
        {
            instance = this;
        }
        public BaseAI skynet;
        public void StartAIFight()
        {
            skynet.StartAI();
        }
        public bool CheckMeOnSubmission(Side side) { return skynet.CheckMeOnSubmission(side); }
        private static AIController instance;
        public static AIController Instance { get => instance; }
    }
}