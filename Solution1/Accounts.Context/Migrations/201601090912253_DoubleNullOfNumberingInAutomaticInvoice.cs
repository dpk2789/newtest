namespace Accounts.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DoubleNullOfNumberingInAutomaticInvoice : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbl_AutomaticInvoiceForm", "Numbering", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbl_AutomaticInvoiceForm", "Numbering", c => c.Double(nullable: false));
        }
    }
}
