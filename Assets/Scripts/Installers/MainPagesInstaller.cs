using Assets.Scripts.UIController.MenuWindows;
using City.MainPages.Army;
using Common.ScriptableObjects;
using MainPages.Campaign;
using MainPages.City;
using MainPages.Events;
using MainPages.MenuButtons;
using MainPages.MenuHud;
using MainPages.SecondCity;
using Ui.MainMenu;
using Ui.MainMenu.MenuButtons;
using UIController;
using UIController.Common;
using UIController.MenuWindows;
using UnityEngine;
using VContainer;
using VContainer.Extensions;
using VContainerUi;
using VContainerUi.Interfaces;

namespace Installers
{
    [CreateAssetMenu(menuName = "UI/Installer/" + nameof(MainPagesInstaller), fileName = nameof(MainPagesInstaller), order = 0)]
    public class MainPagesInstaller : ScriptableObjectInstaller
    {
        private const string MAIN_PAGES = "MainPages";
        private const int CANVAS_ORDER = 0;

        [SerializeField] private Canvas _canvas;
        [SerializeField] private SafeArea _safeArea;

        [SerializeField] private CityPageView _cityPageView;
        [SerializeField] private SecondCityPageView _secondCityPageView;
        [SerializeField] private EventsPageView _eventsPageView;
        [SerializeField] private CampaignPageView _campaignPageView;
        [SerializeField] private PageArmyView _pageArmyView;
        [SerializeField] private MenuButtonsView _menuButtonsView;
        [SerializeField] private MenuHudView _menuHudView;
        [SerializeField] private CityUiView _cityUiView;
        
        [Space(10)]
        [SerializeField] private MenuButtonsDataSo _menuButtonsDataSo;
        public override void Install(IContainerBuilder builder)
        {
            var canvas = Instantiate(_canvas);
            canvas.gameObject.name = MAIN_PAGES;
            canvas.GetComponent<Canvas>().sortingOrder = CANVAS_ORDER;

            var safeArea = Instantiate(_safeArea, canvas.transform);
            builder.RegisterInstance(safeArea);

            var rootCity = new GameObject();
            rootCity.name = nameof(rootCity);

            builder.Register<MainMenuController>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

            builder.RegisterUiView<CityPageController, CityPageView>(_cityPageView, rootCity.transform);
            builder.RegisterUiView<SecondCityPageController, SecondCityPageView>(_secondCityPageView, rootCity.transform);

            builder.RegisterUiView<EventsPageController, EventsPageView>(_eventsPageView, safeArea.RootTemporallyWindows);
            builder.RegisterUiView<CampaignPageController, CampaignPageView>(_campaignPageView, safeArea.RootTemporallyWindows);
            builder.RegisterUiView<PageArmyController, PageArmyView>(_pageArmyView, safeArea.RootTemporallyWindows);
            builder.RegisterUiView<MenuButtonsController, MenuButtonsView>(_menuButtonsView, safeArea.RootPermanentWindows);
            builder.RegisterUiView<MenuHudController, MenuHudView>(_menuHudView, safeArea.RootPermanentWindows);
            builder.RegisterUiView<CityUiController, CityUiView>(_cityUiView, safeArea.RootPermanentWindows);
            ConfigureScriptableObject(builder);
            ConfigureWindows(builder);
        }

        private void ConfigureWindows(IContainerBuilder builder)
        {
            builder.Register<IWindow, EventWindow>(Lifetime.Scoped)
                .AsImplementedInterfaces().AsSelf();

            builder.Register<IWindow, CityWindow>(Lifetime.Scoped)
                .AsImplementedInterfaces().AsSelf();

            builder.Register<IWindow, SecondCityWindow>(Lifetime.Scoped)
                .AsImplementedInterfaces().AsSelf();

            builder.Register<IWindow, CampaignWindow>(Lifetime.Scoped)
                .AsImplementedInterfaces().AsSelf();

            builder.Register<IWindow, ArmyWindow>(Lifetime.Scoped)
                .AsImplementedInterfaces().AsSelf();
        }

        private void ConfigureScriptableObject(IContainerBuilder builder)
        {
            builder.Register(resolver =>
                _menuButtonsDataSo as IMenuButtonsData, Lifetime.Singleton);
        }
    }
}
