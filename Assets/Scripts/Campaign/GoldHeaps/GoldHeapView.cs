﻿using Misc.Sprites;
using UnityEngine;
using UnityEngine.UI;
using VContainerUi.Abstraction;

namespace Campaign.GoldHeaps
{
    public class GoldHeapView : UiView
    {
        [field: SerializeField] public RectTransform Rect { get; private set; }
        [field: SerializeField] public Image Image { get; private set; }
        [field: SerializeField] public Button HeapButton { get; private set; }
        [field: SerializeField] public StorageSpriteFromInt ListSprite { get; private set; }
    }
}