namespace Accounts.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingAutomaticInvoice : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbl_AutomaticInvoiceForm",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Prefix = c.String(),
                        Suffix = c.String(),
                        Numbering = c.Double(nullable: false),
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
            DropTable("dbo.tbl_AutomaticInvoiceForm");
        }
    }
}
