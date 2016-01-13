namespace Accounts.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIngredientNameNIdInCompound : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_CompoundItemIngredient", "IngridentId", c => c.Guid(nullable: false));
            AddColumn("dbo.tbl_CompoundItemIngredient", "IngridentName", c => c.String());
            DropColumn("dbo.tbl_CompoundItemIngredient", "CompoundItemId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbl_CompoundItemIngredient", "CompoundItemId", c => c.Guid(nullable: false));
            DropColumn("dbo.tbl_CompoundItemIngredient", "IngridentName");
            DropColumn("dbo.tbl_CompoundItemIngredient", "IngridentId");
        }
    }
}
