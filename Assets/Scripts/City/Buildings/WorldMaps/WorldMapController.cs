using City.Buildings.Abstractions;
using VContainer;
using UniRx;
using Campaign;
using System.Linq;
using Common.Db.CommonDictionaries;
using Models.Common;
using LocalizationSystems;

namespace City.Buildings.WorldMaps
{
    public class WorldMapController : BaseBuilding<WorldMapView>
    {
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly CampaignController _campaignController;
        [Inject] private readonly CommonGameData _commonGameData;
        [Inject] private readonly ILocalizationSystem _localizationSystem;

        protected override void OnLoadGame()
        {
            var chapters = _commonDictionaries.CampaignChapters;
            for(var i = 0; i < chapters.Count; i++)
            {
                View.Chapters[i].SetData(_localizationSystem, chapters.ElementAt(i).Value);

                View.Chapters[i].OnSelect
                    .Subscribe(_ => OpenChapter(chapters.ElementAt(i).Value))
                    .AddTo(Disposables);
            }

            for (var i = chapters.Count; i < View.Chapters.Count; i++)
            {
                View.Chapters[i].Disable();
            }
        }

        private void OpenChapter(CampaignChapterModel chapter)
        {
            _campaignController.OpenChapter(chapter);
        }
    }
}