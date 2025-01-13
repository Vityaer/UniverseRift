using Hero;
using LocalizationSystems;
using Models.Heroes.Skills;
using System.Collections.Generic;
using UI.Utils.Localizations.Extensions;
using UiExtensions.Scroll.Interfaces;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace UIController.SkillPanels
{
    public class SkillPanelController : UiPanelController<SkillPanelView>
    {
        private readonly ILocalizationSystem _localizationSystem;

        private SkillModel _skill;

        public SkillPanelController(ILocalizationSystem localizationSystem)
        {
            _localizationSystem = localizationSystem;
        }

        public void ShowSkillData(GameHero gameHero, SkillModel skill)
        {
            _skill = skill;
            View.NameSkillText.StringReference = _localizationSystem.LocalizationUiContainer
                .GetLocalizedContainer($"{skill.ID}Name");

            View.DescriptionSkillText.StringReference = _localizationSystem.LocalizationUiContainer
                .GetLocalizedContainer($"{skill.ID}Description");

            skill.GetSkill(gameHero.HeroData.CurrentBreakthrough, out _, out var level);
            View.LevelSkillText.StringReference = _localizationSystem.LocalizationUiContainer
                .GetLocalizedContainer("SkillLevel")
                .WithArguments(new List<object> { level + 1 });

            View.ImageSkill.sprite = _skill.SpritePath;
            MessagesPublisher.OpenWindowPublisher.OpenWindow<SkillPanelController>(openType: OpenType.Additive);
        }
    }
}
