using Fight.HeroControllers.Generals;
using UnityEngine;
using Utils.DataCheckUtils;

namespace City.TrainCamp.HeroInstances
{
    public class HeroRenderContainer
    {
        private HeroController m_heroController;
        
        public HeroController HeroController => m_heroController;
        public CustomRenderTexture RenderTexture { get; }

        public HeroRenderContainer(CustomRenderTexture renderTexture)
        {
            RenderTexture = renderTexture.ThrowIfArgumentNull();
        }
        
        public void SetHeroController(HeroController heroController)
        {
            m_heroController = heroController.ThrowIfArgumentNull();
        }
        
        public void ShowAnimation()
        {
            m_heroController.ViewAnimator.Play(Constants.Visual.ANIMATOR_ATTACK_NAME_HASH);
        }
    }
}