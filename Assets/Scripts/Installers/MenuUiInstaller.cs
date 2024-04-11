using Campaign;
using City.Buildings.Friends;
using City.Buildings.Mails;
using City.Buildings.WorldMaps;
using UIController.LoadingUI;
using UnityEngine;
using VContainer;
using VContainer.Extensions;
using VContainerUi;

namespace Installers
{
    [CreateAssetMenu(menuName = "UI/Installer/" + nameof(MenuUiInstaller), fileName = nameof(MenuUiInstaller), order = 0)]
    public class MenuUiInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private Canvas _canvas;


        [SerializeField] private CampaignView _campaignView;
        [SerializeField] private WorldMapView _worldMapView;

        public override void Install(IContainerBuilder builder)
        {
            var canvas = Instantiate(_canvas);
            builder.RegisterUiView<CampaignController, CampaignView>(_campaignView, canvas.transform);
            builder.RegisterUiView<WorldMapController, WorldMapView>(_worldMapView, canvas.transform);

        }
    }
}
