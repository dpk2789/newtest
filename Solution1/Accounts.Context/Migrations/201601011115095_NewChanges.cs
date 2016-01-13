namespace Accounts.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewChanges : DbMigration
    {
        public override void Up()
        {
            RenameIndex(table: "dbo.tbl_IssueItems", name: "AK_IssueItem_BillInvoice", newName: "AK_IssueItem_BillInvoice,2");
            AlterColumn("dbo.tbl_IssueItems", "IssueType", c => c.String(nullable: false, maxLength: 80));
            CreateIndex("dbo.tbl_IssueItems", "IssueType", unique: true, name: "AK_IssueItem_BillInvoice,1");
            DropColumn("dbo.tbl_IssueItems", "ActionId");
            DropColumn("dbo.tbl_IssueItems", "InwardInvoice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbl_IssueItems", "InwardInvoice", c => c.String());
            AddColumn("dbo.tbl_IssueItems", "ActionId", c => c.Int(nullable: false));
            DropIndex("dbo.tbl_IssueItems", "AK_IssueItem_BillInvoice,1");
            AlterColumn("dbo.tbl_IssueItems", "IssueType", c => c.String());
            RenameIndex(table: "dbo.tbl_IssueItems", name: "AK_IssueItem_BillInvoice,2", newName: "AK_IssueItem_BillInvoice");
        }
    }
}
