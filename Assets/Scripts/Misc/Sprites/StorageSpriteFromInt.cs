using System.Collections.Generic;
using UnityEngine;

namespace Misc.Sprites
{
    [System.Serializable]
    public class StorageSpriteFromInt
    {
        [SerializeField] private List<SpiteFromCount> _listSprites = new List<SpiteFromCount>();

        public Sprite GetSprite(int tact)
        {
            Sprite result = null;
            int num = -1;
            for (int i = 0; i < _listSprites.Count; i++)
            {
                if (tact >= _listSprites[i].Count)
                {
                    num = i;
                }
            }

            if (num >= 0)
                result = _listSprites[num].Sprite;

            return result;
        }
    }
}