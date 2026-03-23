using Common.Db.CommonDictionaries;
using Editor.Common;
using Models.Guilds;
using Sirenix.OdinInspector;

namespace Assets.Editor.Pages.Bosses
{
    public class GuildBossModelEditor : BaseModelEditor<GuildBossContainer>
    {
        private CommonDictionaries _commonDictionaries;

        public GuildBossModelEditor(GuildBossContainer model, CommonDictionaries commonDictionaries)
        {
            _commonDictionaries = commonDictionaries;
            m_model = model;

            model.SetDictionary(_commonDictionaries);
            foreach (var mission in m_model.Missions)
            {
                mission.SetCommonDictionary(_commonDictionaries);
            }
        }

        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelText("Container")]
        [LabelWidth(250)]
        public GuildBossContainer Model
        {
            get => m_model;
            set => m_model = value;
        }
    }
}
