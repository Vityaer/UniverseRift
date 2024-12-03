namespace Fight.HeroControllers.Generals.Movements
{
    //public class WalkMovement : AbstractMovement
    //{
    //public override IEnumerator Move(HeroController hero, Stack<HexagonCell> way)
    //{
    //Animator.SetBool("Speed", true);
    //var way = _gridController.FindWay(myPlace, targetCell, typeMovement: hero.GetBaseCharacteristic.MovementType);
    //Vector3 targetPos, startPos;
    //float t = 0f;

    //var currentCell = way.Pop();
    //while (way.Count > 0)
    //{
    //    myPlace.ClearSublject();
    //    currentCell = way.Pop();

    //    startPos = Self.position;
    //    targetPos = currentCell.Position;

    //    if (isFacingRight ^ ((targetPos.x - startPos.x) > 0))
    //        FlipX();

    //    t = 0f;

    //    while (t <= 1f)
    //    {
    //        t += Time.deltaTime * speedMove;
    //        Self.position = Vector3.Lerp(startPos, targetPos, t);
    //        yield return null;
    //    }

    //    Self.position = targetPos;
    //    myPlace = currentCell;
    //    myPlace.SetHero(this);
    //}

    //Animator.SetBool("Speed", false);
    //SetMyPlaceColor();
    //}
    //}
}
