using System.Linq;
using City.Panels.WhereGetSubjectPanels;
using Common.Db.CommonDictionaries;
using Common.Resourses;
using LocalizationSystems;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainer;
using VContainerUi.Model;
using VContainerUi.Messages;

namespace City.Panels.SubjectPanels.Resources
{
    public class ResourcePanelController : UiPanelController<ResourcePanelView>
    {
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly WhereGetSubjectPanelController _whereGetSubjectPanelController;
        
        private readonly ILocalizationSystem _localizationSystem;

        private GameResource _gameResource;
        
        public ResourcePanelController(ILocalizationSystem localizationSystem)
        {
            _localizationSystem = localizationSystem;
        }

        public override void Start()
        {
            View.HowGetButton.OnClickAsObservable()
                .Subscribe(_ => OpenWhereGetResource())
                .AddTo(Disposables);
            base.Start();
        }

        private void OpenWhereGetResource()
        {
            _whereGetSubjectPanelController.ShowWhereGetResourcePanel(_gameResource.Type);
        }

        public void ShowData(GameResource resource)
        {
            View.MainImage.SetData(resource);
            View.Name.StringReference = _localizationSystem.LocalizationUiContainer
                .GetLocalizedContainer($"{resource.Type}Name");

            View.Desctiption.StringReference = _localizationSystem.LocalizationUiContainer
                .GetLocalizedContainer($"{resource.Type}Desctiption");

            _gameResource = resource;
            MessagesPublisher.OpenWindowPublisher.OpenWindow<ResourcePanelController>(openType: OpenType.Additive);

            var flag = _commonDictionaries.HelpResourceModels.Values
                .FirstOrDefault(help => help.TargetType == resource.Type) == null;
            
            View.HowGetButton.gameObject.SetActive(flag);
        }
    }
}
