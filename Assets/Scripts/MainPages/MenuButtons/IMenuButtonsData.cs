using System.Collections.Generic;

namespace MainPages.MenuButtons
{
    public interface IMenuButtonsData
    {
        Dictionary<string, MenuButtonData> ButtonData { get; }
    }
}
