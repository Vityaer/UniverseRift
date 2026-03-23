using System;
using System.Threading;
using City.Panels.BlackedPanels;
using City.Panels.DialogPanels;
using City.Panels.ScreenBlockers;
using City.Panels.TutorialTouchPanels;
using City.SliderCity;
using City.TrainCamp.HeroPanels;
using Cysharp.Threading.Tasks;
using MainPages.Army;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using VContainer;

namespace Models.Tutorials
{
    public class HeroesTrainingTutorial : ITutorialStage
    {
        [Inject] private PageArmyView m_pageArmyView;
        [Inject] private HeroPanelController m_heroPanelController;
        [Inject] private HeroPanelView m_heroPanelView;
        
        [Inject] private readonly ScreenBlockerController m_screenBlockerController;
        [Inject] private BlackedPanelController m_blackedPanelController;
        [Inject] private TutorialTouchController m_tutorialTouchController;
        [Inject] private DialogPanelController m_dialogPanelController;
        
        [LabelWidth(200)]
        public int LevelUpFirstHero;
        
        [LabelWidth(200)]
        public int LevelUpSecondHero;
        
        public async UniTask PlayAsync(CancellationToken token)
        {
            await UniTask.DelayFrame(3, cancellationToken: token);

            var card = m_pageArmyView.CardsContainer.Content.GetChild(0).GetComponent<RectTransform>();
            await m_blackedPanelController.SelectView(card);
            m_tutorialTouchController.ShowTouch(card);

            await UniTask.WaitUntil(() => m_heroPanelController.IsActive, cancellationToken: token);

            var levelUpButton = m_heroPanelView.LevelUpButton.GetComponent<RectTransform>();
            await m_blackedPanelController.SelectView(levelUpButton);
            m_tutorialTouchController.ShowTouch(levelUpButton);

            int countLevelUp = 0;
            IDisposable disposable = m_heroPanelController.OnLevelUp
                    .Subscribe(gameHero => countLevelUp = gameHero.HeroData.Level);
            
            await UniTask.WaitUntil(() => countLevelUp >= LevelUpFirstHero, cancellationToken: token);
            m_heroPanelView.LevelUpButton.interactable = false;

            m_screenBlockerController.Open();

            var nextHeroButtonRect = m_heroPanelView.ToRightButton.GetComponent<RectTransform>();
            
            await m_blackedPanelController.SelectView(nextHeroButtonRect);
            m_tutorialTouchController.ShowTouch(nextHeroButtonRect);
            m_screenBlockerController.Close();

            await m_heroPanelController.OnSwipe
                .Where(swipe => swipe == SwipeType.Right)
                .First()
                .ToUniTask(cancellationToken: token);
            m_heroPanelView.LevelUpButton.interactable = true;

            
            await m_blackedPanelController.SelectView(levelUpButton);
            m_tutorialTouchController.ShowTouch(levelUpButton);
            
            countLevelUp = 0;
            await UniTask.WaitUntil(() => countLevelUp >= LevelUpSecondHero, cancellationToken: token);
            m_heroPanelView.LevelUpButton.interactable = false;
            m_screenBlockerController.Open();

            await m_blackedPanelController.SelectView(m_heroPanelView.CloseButton.GetComponent<RectTransform>());
            m_tutorialTouchController.ShowTouch(m_heroPanelView.CloseButton.GetComponent<RectTransform>());
            
            m_screenBlockerController.Close();
            await m_heroPanelView.CloseButton.OnClickAsObservable()
                .First()
                .ToUniTask(cancellationToken: token);
            
            m_heroPanelView.LevelUpButton.interactable = true;
        }
    }
}