namespace Accounts.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCompoundItemNItemTypeInItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbl_CompoundItemIngredient",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ItemId = c.Guid(nullable: false),
                        UnitQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.tbl_Item", "TaxType", c => c.String());
            AddColumn("dbo.tbl_Item", "ItemType", c => c.String());
            AlterColumn("dbo.tbl_Item", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbl_Item", "Name", c => c.String(nullable: false, maxLength: 80));
            DropColumn("dbo.tbl_Item", "ItemType");
            DropColumn("dbo.tbl_Item", "TaxType");
            DropTable("dbo.tbl_CompoundItemIngredient");
        }
    }
}
