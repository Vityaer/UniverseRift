using System;
using System.Collections.Generic;
using Common.Db.CommonDictionaries;
using Hero;
using UniRx;
using UnityEngine;
using UnityEngine.Pool;
using Utils;
using Utils.DataCheckUtils;
using VContainer;
using HeroController = Fight.HeroControllers.Generals.HeroController;

namespace City.TrainCamp.HeroInstances
{
    public class HeroInstancesController : MonoBehaviour
    {
        [Inject] private CommonDictionaries m_commonDictionaries;
        
        private ObjectPool<HeroRenderContainer> m_renderPool;
        private WrapperPool<HeroInstanceHelper> m_pool;
        private Transform m_root;
        private Transform m_rootHeroes;

        [SerializeField] private GameObject m_light;
        [SerializeField] private HeroInstanceHelper m_cameraPrefab;
        [SerializeField] private CustomRenderTexture m_renderTexture;
        [SerializeField] private Vector3 m_startPosition;
        [SerializeField] private Vector3 m_deltaPosition;
        
        private readonly Dictionary<HeroRenderContainer, HeroInstanceHelper> m_renders = new();
        private readonly Dictionary<string, HeroController> m_heroes = new();
        private readonly CompositeDisposable m_disposables = new();

        private int m_currentHeroIndex = -1;
        
        private void Awake()
        {
            m_root = new GameObject("HeroInstancesHelperParent").transform;
            m_root.SetParent(transform);
            
            m_rootHeroes = new GameObject("PoolHeroes").transform;
            m_rootHeroes.SetParent(transform);
            
            m_pool = new WrapperPool<HeroInstanceHelper>(m_cameraPrefab, null, m_root);
            m_renderPool =
                new ObjectPool<HeroRenderContainer>(CreateRenderContainer);
        }

        public HeroRenderContainer ShowHero(GameHero hero, int extaRating = 0)
        {
            HeroInstanceHelper helper = m_pool.Get();
            HeroRenderContainer renderContainer = m_renderPool.Get();

            m_currentHeroIndex = (m_currentHeroIndex + 1) % 100;
            
            helper.SetPosition(m_startPosition + m_deltaPosition * m_currentHeroIndex);

            HeroController heroController = CreateHero(hero, helper);
            int stage = (hero.HeroData.Rating + extaRating) / 5;
            stage = Math.Clamp(stage, 0, 2);
            heroController.SetStage(stage);
            
            helper.EnableCamera(renderContainer.RenderTexture);
            
            m_light.SetActive(true);
            renderContainer.SetHeroController(heroController);
            m_renders.Add(renderContainer, helper);
            return renderContainer;
        }

        public void ReleaseRender(HeroRenderContainer renderContainer)
        {
            renderContainer.ThrowIfArgumentNull();

            var helper = m_renders[renderContainer];
            helper.DisableCamera();
            
            renderContainer.HeroController.transform.SetParent(m_rootHeroes); 
            renderContainer.HeroController.gameObject.SetActive(false);
            
            m_pool.Release(helper);
            m_renderPool.Release(renderContainer);
            
            m_renders.Remove(renderContainer);
            if (m_renders.Count == 0)
            {
                m_light.SetActive(false);
            }
        }

        public HeroController GetHeroPrefab(string heroId)
        {
            string path = $"{Constants.ResourcesPath.HEROES_PATH}{heroId}";
            HeroController newHero = Resources.Load<HeroController>(path);
            return newHero;
        }

        private HeroController CreateHero(GameHero gameHero, HeroInstanceHelper helperCreatePoint)
        {
            if (m_heroes.TryGetValue(gameHero.Model.General.HeroId, out HeroController result))
            {
                helperCreatePoint.SetHeroController(result);
                return result;
            }
            
            HeroController heroPrefab = GetHeroPrefab(gameHero.Model.General.HeroId);
            heroPrefab.ThrowIfArgumentNull();
            
            result =
                Instantiate(heroPrefab, helperCreatePoint.CreatePoint.position, Quaternion.identity, helperCreatePoint.CreatePoint);
            
            helperCreatePoint.SetHeroController(result);
            m_heroes.Add(gameHero.Model.General.HeroId, result);

            return result;
        }

        private HeroRenderContainer CreateRenderContainer()
        {
            CustomRenderTexture renderTexture = CopyCustomRenderTexture(m_renderTexture);
            return new HeroRenderContainer(renderTexture);
        }
        
        private CustomRenderTexture CopyCustomRenderTexture(CustomRenderTexture source)
        {
            // Создаем новый CustomRenderTexture с такими же параметрами
            CustomRenderTexture copy = new CustomRenderTexture(source.width, source.height, source.format);
            copy.dimension = source.dimension;
            copy.enableRandomWrite = source.enableRandomWrite;
            copy.autoGenerateMips = source.autoGenerateMips;
            copy.useMipMap = source.useMipMap;
            copy.depthStencilFormat = source.depthStencilFormat;
            copy.useDynamicScale = source.useDynamicScale;
            copy.filterMode = source.filterMode;
            copy.initializationMode = source.initializationMode;

            // Создаем ресурс
            copy.Create();

            return copy;
        }
    }
}