using UIController.Common;
using UnityEngine;
using VContainer;
using VContainer.Extensions;

namespace Installers
{
    [CreateAssetMenu(menuName = "UI/Installer/" + nameof(GuildPagesInstaller), fileName = nameof(GuildPagesInstaller),
        order = 0)]
    public class GuildPagesInstaller : ScriptableObjectInstaller
    {
        private const string MAIN_PAGES = "GuildPages";
        private const int CANVAS_ORDER = 0;

        [SerializeField] private Canvas _canvas;
        [SerializeField] private SafeArea _safeArea;


        public override void Install(IContainerBuilder builder)
        {
            Canvas canvas = Instantiate(_canvas);
            canvas.gameObject.name = MAIN_PAGES;
            canvas.GetComponent<Canvas>().sortingOrder = CANVAS_ORDER;

            SafeArea safeArea = Instantiate(_safeArea, canvas.transform);
            builder.RegisterInstance(safeArea);

            GameObject rootCity = new GameObject();
            rootCity.name = nameof(rootCity);

            //builder.RegisterUiView<NewGuildPanelController, NewGuildPanelView>(_newGuildPanelView, safeArea.RootTemporallyWindows);
            //builder.RegisterUiView<AvailableGuildsPanelController, AvailableGuildsPanelView>(_availableGuildsPanelView, safeArea.RootTemporallyWindows);
            //builder.RegisterUiView<GuildBossRaidPanelController, GuildBossRaidPanelView>(_guildBossRaidPanelController, safeArea.RootTemporallyWindows);
            //builder.RegisterUiView<GuildAdministrationPanelController, GuildAdministrationPanelView>(_guildAdministrationPanelView, safeArea.RootTemporallyWindows);
        }
    }
}