namespace Accounts.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIssueInvoiceUnique : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_IssueItems", "IssueInvoice", c => c.String(nullable: false, maxLength: 80));
            CreateIndex("dbo.tbl_IssueItems", "IssueInvoice", unique: true, name: "AK_IssueItem_BillInvoice");
        }
        
        public override void Down()
        {
            DropIndex("dbo.tbl_IssueItems", "AK_IssueItem_BillInvoice");
            DropColumn("dbo.tbl_IssueItems", "IssueInvoice");
        }
    }
}
