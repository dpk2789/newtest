namespace Accounts.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UnikeyKeyFalseInIssueType : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.tbl_IssueItems", "AK_IssueItem_BillInvoice,1");
            CreateIndex("dbo.tbl_IssueItems", "IssueType", name: "AK_IssueItem_BillInvoice,1");
        }
        
        public override void Down()
        {
            DropIndex("dbo.tbl_IssueItems", "AK_IssueItem_BillInvoice,1");
            CreateIndex("dbo.tbl_IssueItems", "IssueType", unique: true, name: "AK_IssueItem_BillInvoice,1");
        }
    }
}
