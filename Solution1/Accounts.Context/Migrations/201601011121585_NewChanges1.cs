namespace Accounts.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewChanges1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.tbl_IssueItems", "AK_IssueItem_BillInvoice,2");
            CreateIndex("dbo.tbl_IssueItems", "IssueInvoice", name: "AK_IssueItem_BillInvoice,2");
        }
        
        public override void Down()
        {
            DropIndex("dbo.tbl_IssueItems", "AK_IssueItem_BillInvoice,2");
            CreateIndex("dbo.tbl_IssueItems", "IssueInvoice", unique: true, name: "AK_IssueItem_BillInvoice,2");
        }
    }
}
