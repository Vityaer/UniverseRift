using Common.Inventories.Resourses;
using Common.Resourses;
using Models.Items;
using UIController.Inventory;

namespace City.Buildings.Forge
{
    public class GameItemRelation
    {
        public ItemRelationModel Model;
        public GameItem Ingredient;
        public GameItem Result;
        public GameResource Cost;

        public GameItemRelation(ItemRelationModel model, GameItem ingredient, GameItem result)
        {
            Model = model;
            Ingredient = ingredient;
            Result = result;
            Cost = new GameResource(Model.Cost);
        }
    }
}
