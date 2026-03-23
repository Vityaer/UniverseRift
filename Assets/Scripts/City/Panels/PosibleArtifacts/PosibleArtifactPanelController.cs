using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using Models.Inventory.Splinters;
using UIController.ItemVisual;
using UiExtensions.Misc;
using UiExtensions.Panels;
using VContainer;

namespace City.Panels.PosibleArtifacts
{
    public class PosibleArtifactPanelController : UiPanelController<PosibleArtifactPanelView>
    {
        [Inject] private readonly CommonDictionaries m_commonDictionaries;

        private DynamicUiList<ArtifactWithPercent, GameRandomSplinterData> m_artifactViews;

        public override void Start()
        {
            base.Start();
            m_artifactViews = new DynamicUiList<ArtifactWithPercent, GameRandomSplinterData>(View.Prefab,
                View.ScrollRect.content,
                View.ScrollRect,
                m_commonDictionaries,
                onCreate: CreateArtifactView
            );
        }

        public void SetData(List<RandomSplinterModel> splinterDatas)
        {
            List<GameRandomSplinterData> preparedDatas = new(splinterDatas.Count);
            float sum = splinterDatas.Sum(artifact => artifact.Valuation);
            preparedDatas.AddRange(from model
                    in splinterDatas
                let percent = model.Valuation / sum
                select new GameRandomSplinterData(model, percent));

            m_artifactViews.ShowDatas(preparedDatas);
        }

        private void CreateArtifactView(ArtifactWithPercent obj)
        {
            obj.Init(View.ScrollRect, m_commonDictionaries);
        }
    }
}