using Assets.Scripts.Fight.Common;
using City.Panels.BlackedPanels;
using City.Panels.DialogPanels;
using City.Panels.ScreenBlockers;
using City.Panels.TutorialTouchPanels;
using Fight.AI;
using Fight.Common;
using Fight.Common.Factories;
using Fight.Common.FightInterface;
using Fight.Common.Grid;
using Fight.Common.UI;
using Fight.Common.WarTable;
using Fight.Grid;
using Fight.Windows.WarTable;
using Plugins.VContainerUi.Runtime;
using UIController.FightUI;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Extensions;
using VContainerUi;
using VContainerUi.Interfaces;
using FightController = Fight.Common.FightController;

namespace Assets.Scripts.Installers
{
    [CreateAssetMenu(menuName = "UI/Installer/" + nameof(FightInstaller), fileName = nameof(FightInstaller), order = 0)]
    public class FightInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private FightView _fightView;
        [SerializeField] private FightPanelView _fightPanelView;
        [SerializeField] private WarTableView _warTableView;
        [SerializeField] private GridView _gridView;
        [SerializeField] private FightDirectionView _fightDirectionView;
        [SerializeField] private ScreenBlockerView m_screenBlockerView;
        [SerializeField] private BlackedPanelView m_blackedPanelView;
        [SerializeField] private TutorialTouchView m_tutorialTouchView;
        [SerializeField] private DialogPanelView m_dialogPanelView;
        [SerializeField] private int _canvasRenderOrder;
        
        public override void Install(IContainerBuilder builder)
        {
            var canvas = Instantiate(_canvas);
            canvas.name = nameof(FightInstaller);
            canvas.sortingOrder = _canvasRenderOrder;
            
            var rootGrid = new GameObject("GridRoot");
            builder.RegisterUiView<GridController, GridView>(_gridView, rootGrid.transform);

            builder.RegisterUiView<FightController, FightView>(_fightView, canvas.transform);
            builder.RegisterUiView<FightPanelController, FightPanelView>(_fightPanelView, canvas.transform);
            builder.RegisterUiView<WarTableController, WarTableView>(_warTableView, canvas.transform);
            builder.RegisterUiView<FightDirectionController, FightDirectionView>(_fightDirectionView, canvas.transform);
            builder.Register<BotProvider>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

            builder.RegisterUiView<BlackedPanelController, BlackedPanelView>(m_blackedPanelView, canvas.transform,
                false);
            builder.RegisterUiView<TutorialTouchController, TutorialTouchView>(m_tutorialTouchView, canvas.transform,
                false);
            builder.RegisterUiView<ScreenBlockerController, ScreenBlockerView>(m_screenBlockerView, canvas.transform,
                false);
            builder.RegisterUiView<DialogPanelController, DialogPanelView>(m_dialogPanelView, canvas.transform,
                false);
            
            
            ConfigureWindows(builder);
            ConfigureFactories(builder);
        }

        private void ConfigureWindows(IContainerBuilder builder)
        {
            builder.Register<IWindow, FightWindow>(Lifetime.Scoped)
                .AsImplementedInterfaces().AsSelf();
        }

        private void ConfigureFactories(IContainerBuilder builder)
        {
            builder.Register<HeroFactory>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<BotFactory>(Lifetime.Singleton);
            builder.Register<GridFactory>(Lifetime.Singleton);
        }
    }
}
