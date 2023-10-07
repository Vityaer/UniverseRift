using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace MainPages.MenuButtons
{
    [Serializable]
    public class MenuButtonData
    {
        public string Id;

        [HorizontalGroup("Group 1", LabelWidth = 40)]
        public string Text;

        [HorizontalGroup("Group 1", LabelWidth = 40)]
        [PreviewField]
        public Sprite Icon;

    }
}
