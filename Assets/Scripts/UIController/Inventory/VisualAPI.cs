namespace UIController.Inventory
{
    public interface VisualAPI
    {
        VisualAPI GetVisual();
        void ClearUI();
        void SetUI(ThingUI UI);
        void UpdateUI();
        void ClickOnItem();
    }
}