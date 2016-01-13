namespace Accounts.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCompoundItemId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_CompoundItemIngredient", "CompoundItemId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_CompoundItemIngredient", "CompoundItemId");
        }
    }
}
