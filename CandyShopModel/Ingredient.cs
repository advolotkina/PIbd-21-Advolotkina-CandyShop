using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CandyShopModel
{
    public class Ingredient
    {
        public int Id { get; set; }

        [Required]
        public string IngredientName { get; set; }

        [ForeignKey("IngredientId")]
        public virtual List<CandyIngredient> CandyIngredients { get; set; }

        [ForeignKey("IngredientId")]
        public virtual List<WarehouseIngredient> WarehouseIngredients { get; set; }
    }
}
