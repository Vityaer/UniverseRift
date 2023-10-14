using MainPages.MenuButtons;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Common.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(MenuButtonsDataSo), menuName = "Settings/" + nameof(MenuButtonsDataSo))]
    public class MenuButtonsDataSo : SerializedScriptableObject, IMenuButtonsData
    {
        [SerializeField] private List<MenuButtonData> _buttonData;

        public List<MenuButtonData> ButtonData => _buttonData;
    }
}
