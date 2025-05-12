using Assets.Scripts.Fight.Common;
using Fight;
using Fight.AI;
using Fight.Factories;
using Fight.FightInterface;
using Fight.Grid;
using Fight.UI;
using Fight.WarTable;
using UIController.FightUI;
using UnityEngine;
using VContainer;
using VContainer.Extensions;
using VContainerUi;
using VContainerUi.Interfaces;

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
