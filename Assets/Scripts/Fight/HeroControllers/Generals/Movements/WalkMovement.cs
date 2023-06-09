using Cysharp.Threading.Tasks;
using Fight.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace Fight.HeroControllers.Generals.Movements
{
    //public class WalkMovement : AbstractMovement
    //{
    //    public override async UniTask Move(HeroController hero, Stack<HexagonCell> way)
    //    {
    //        Vector3 targetPos, startPos;
    //        float t = 0f;
    //        HexagonCell currentCell = way.Pop();
    //        while (way.Count > 0)
    //        {
    //            hero.Cell.ClearSublject();
    //            currentCell = way.Pop();
    //            startPos = hero.transform.position;
    //            targetPos = currentCell.Position;

    //            if (hero.IsFacingRight ^ ((targetPos.x - startPos.x) > 0))
    //                FlipX();

    //            t = 0f;

    //            while (t <= 1f)
    //            {
    //                t += Time.deltaTime * speedMove;
    //                tr.position = Vector2.Lerp(startPos, targetPos, t);
    //                await UniTask.Yield();
    //            }

    //            tr.position = targetPos;
    //            myPlace = currentCell;
    //            myPlace.SetHero(this);
    //        }
    //    }
    //}
}
