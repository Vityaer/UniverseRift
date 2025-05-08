using Fight.Grid;
using Models.Heroes;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Fight.HeroControllers.Generals.Movements
{
    public class WalkMovement : AbstractMovement
    {
        public override async UniTask Move(HexagonCell targetCell)
        {
            if (!Hero.IsFastFight)
            {
                Animator.SetBool("Speed", true);
                var way = GridController.FindWay(MyPlace, targetCell, typeMovement: TypeMovement.Walk);
                Vector3 targetPos, startPos;
                var t = 0f;

                var currentCell = way.Pop();
                while (way.Count > 0)
                {
                    MyPlace.ClearSublject();
                    currentCell = way.Pop();

                    startPos = Hero.transform.position;
                    targetPos = currentCell.Position;

                    if (Hero.IsFacingRight ^ ((targetPos.x - startPos.x) > 0))
                        Hero.FlipX();

                    t = 0f;

                    while (t <= 1f)
                    {
                        t += Time.deltaTime * Hero.SpeedMove;
                        Hero.transform.position = Vector3.Lerp(startPos, targetPos, t);
                        await UniTask.Yield();
                    }
                    Hero.transform.position = targetPos;

                    MyPlace = currentCell;
                    MyPlace.SetHero(Hero);
                    _onChangePlace.Execute(MyPlace);
                }

                Animator.SetBool("Speed", false);
                SetMyPlaceColor();
            }
            else
            {
                Hero.transform.position = targetCell.Position;
                MyPlace = targetCell;
                MyPlace.SetHero(Hero);
                SetMyPlaceColor();
            }
        }
    }
}
