using MainPages.MenuButtons;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Common.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(MenuButtonsDataSo), menuName = "Settings/" + nameof(MenuButtonsDataSo))]
    public class MenuButtonsDataSo : SerializedScriptableObject, IMenuButtonsData
    {
        [SerializeField] private Dictionary<string, MenuButtonData> _buttonData;

        public Dictionary<string, MenuButtonData> ButtonData => _buttonData;
    }
}
