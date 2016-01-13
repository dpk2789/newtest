namespace Accounts.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSettingsNAutomaticInvoiceForm : DbMigration
    {
        public override void Up()
        {
          
            CreateTable(
                "dbo.tbl_Settings",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AutomaticPurchaseInvoice = c.Boolean(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tbl_Settings");
          
        }
    }
}
