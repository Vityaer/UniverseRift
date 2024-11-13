namespace UI.Utils.Localizations.Containers
{
    public class LocalizationUiContainer : LocalizationContainer
    {
        protected override string TableName => Constants.Localization.UI_TABLE_NAME;

        public LocalizationUiContainer()
        {
            WaitLoadLocation().Forget();
        }
    }
}