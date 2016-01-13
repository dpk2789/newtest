namespace Accounts.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelationBtwItemNIngridients : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.tbl_CompoundItemIngredient", "ItemId");
            AddForeignKey("dbo.tbl_CompoundItemIngredient", "ItemId", "dbo.tbl_Item", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbl_CompoundItemIngredient", "ItemId", "dbo.tbl_Item");
            DropIndex("dbo.tbl_CompoundItemIngredient", new[] { "ItemId" });
        }
    }
}
