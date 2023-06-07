namespace Fight.WarTable
{
    public interface IWorkWithWarTable
    {
        void RegisterOnOpenCloseWarTable();

        void UnregisterOnOpenCloseWarTable();

        void Change(bool isOpen);
    }
}