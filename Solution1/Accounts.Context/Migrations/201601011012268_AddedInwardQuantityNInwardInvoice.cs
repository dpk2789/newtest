namespace Accounts.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedInwardQuantityNInwardInvoice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_IssueItems", "InwardInvoice", c => c.String());
            AddColumn("dbo.tbl_IssueItems", "InwardQuantity", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_IssueItems", "InwardQuantity");
            DropColumn("dbo.tbl_IssueItems", "InwardInvoice");
        }
    }
}
