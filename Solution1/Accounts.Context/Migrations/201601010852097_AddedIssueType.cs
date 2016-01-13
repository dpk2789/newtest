namespace Accounts.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIssueType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_IssueItems", "ActionId", c => c.Int(nullable: false));
            AddColumn("dbo.tbl_IssueItems", "IssueType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_IssueItems", "IssueType");
            DropColumn("dbo.tbl_IssueItems", "ActionId");
        }
    }
}
