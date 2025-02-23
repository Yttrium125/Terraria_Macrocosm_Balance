using Macrocosm.Content.Items.Bars;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Macrocosm.Content.Items.Armor.Aluminum
{
    [AutoloadEquip(EquipType.Head)]
    public class AluminumHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 5);
            Item.rare = ItemRarityID.White;
            Item.defense = 2;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == ModContent.ItemType<AluminumHelmet>() && body.type == ModContent.ItemType<AluminumBreastplate>() && legs.type == ModContent.ItemType<AluminumGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.statDefense += 2;
            player.setBonus = "2 defense"; // TODO: localize
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient<AluminumBar>(16)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}