﻿using Models.Data.Inventories;

namespace Models.Items
{
    public class ItemRelationModel : BaseModel
    {
        public string ResultItemName;
        public string ItemIngredientName;
        public int RequireCount;
        public ResourceData Cost;
    }
}
