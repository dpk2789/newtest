namespace Accounts.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbl_ItemCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentCategoryId = c.Int(),
                        CategoryName = c.String(nullable: false, maxLength: 80),
                        Parent_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tbl_ItemCategory", t => t.Parent_Id)
                .Index(t => t.CategoryName, unique: true, name: "AK_ItemCategory_CategoryName")
                .Index(t => t.Parent_Id);

            CreateTable(
                "dbo.tbl_Item",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(nullable: false, maxLength: 80),
                        Name = c.String(nullable: false, maxLength: 80),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemCategoryId = c.Int(nullable: false),
                        UnitId = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tbl_ItemCategory", t => t.ItemCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.tbl_Unit", t => t.UnitId, cascadeDelete: true)
                .Index(t => t.Code, unique: true, name: "AK_Item_ItemCode")
                .Index(t => t.ItemCategoryId)
                .Index(t => t.UnitId);

            CreateTable(
                "dbo.tbl_Unit",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 80),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "AK_Unit_UnitName");

            CreateTable(
                "dbo.tbl_PurchasBill",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        BillInvoice = c.String(nullable: false, maxLength: 80),
                        SupplierInvoice = c.String(),
                        BillDate = c.DateTime(),
                        //  TaxWithExtendedPrice = c.Decimal(precision: 18, scale: 2),
                        Freight = c.Decimal(precision: 18, scale: 2),
                        Packaging = c.Decimal(precision: 18, scale: 2),
                        OtherExpenses = c.Decimal(precision: 18, scale: 2),
                        // BillTotal = c.Decimal(precision: 18, scale: 2),
                        SupplierId = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tbl_Supplier", t => t.SupplierId, cascadeDelete: true)
                .Index(t => t.BillInvoice, unique: true, name: "AK_PurchaseBill_BillInvoice")
                .Index(t => t.SupplierId);

            CreateTable(
                "dbo.tbl_PurchaseItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(),
                        Name = c.String(),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        //  ExtendedPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitId = c.Guid(nullable: false),
                        PurchaseBillId = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tbl_PurchasBill", t => t.PurchaseBillId, cascadeDelete: true)
                .ForeignKey("dbo.tbl_Unit", t => t.UnitId, cascadeDelete: true)
                .Index(t => t.UnitId)
                .Index(t => t.PurchaseBillId);

            Sql("ALTER TABLE dbo.tbl_PurchaseItems ADD ExtendedPrice AS CAST(Quantity * UnitPrice AS Decimal(18, 2))");

            CreateTable(
                "dbo.tbl_PurchaseTaxes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TaxCode = c.String(),
                        Name = c.String(),
                        Percent = c.Decimal(precision: 18, scale: 2),
                        PurchaseBillId = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tbl_PurchasBill", t => t.PurchaseBillId, cascadeDelete: true)
                .Index(t => t.PurchaseBillId);

            Sql(@"Create FUNCTION [dbo].[GetTaxWithExtendedPrice](@purchaseBillId uniqueidentifier)
                RETURNS DECIMAL(18, 2)
                AS
                BEGIN

                DECLARE @extendedPriceSum Decimal(18, 2);
                DECLARE @taxSum Decimal(18, 2);

                SELECT @taxSum = Sum([Percent])
                FROM tbl_PurchaseTaxes
                WHERE PurchaseBillId = @purchaseBillId;

                SELECT @extendedPriceSum = Sum(ExtendedPrice)
                FROM tbl_PurchaseItems
                WHERE PurchaseBillId = @purchaseBillId;

                RETURN  ISNULL(@extendedPriceSum *@taxSum/100, 0);
                END");

            Sql("ALTER TABLE dbo.tbl_PurchasBill ADD TaxWithExtendedPrice AS dbo.GetTaxWithExtendedPrice(Id)");


            Sql(@"Create FUNCTION [dbo].[GetSumOfItemsAndTaxes](@purchaseBillId uniqueidentifier)
                RETURNS DECIMAL(18, 2)
                AS
                BEGIN

                DECLARE @extendedPriceSum Decimal(18, 2);
                DECLARE @taxSum Decimal(18, 2);

                SELECT @taxSum = Sum([Percent])
                FROM tbl_PurchaseTaxes
                WHERE PurchaseBillId = @purchaseBillId;

                SELECT @extendedPriceSum = Sum(ExtendedPrice)
                FROM tbl_PurchaseItems
                WHERE PurchaseBillId = @purchaseBillId;

                RETURN ISNULL(@extendedPriceSum, 0) + ISNULL(@extendedPriceSum *@taxSum/100, 0);
                END");

            // Sql("ALTER TABLE dbo.tbl_PurchasBill ADD BillTotal AS SUM(Freight) dbo.GetSumOfItemsAndTaxes(Id)");
            Sql("ALTER TABLE dbo.tbl_PurchasBill ADD BillTotal AS ISNULL(Freight,0) +ISNULL(Packaging,0)+ISNULL(OtherExpenses,0) + dbo.GetSumOfItemsAndTaxes(Id) + dbo.GetTaxWithExtendedPrice(Id)");


            CreateTable(
                "dbo.tbl_Supplier",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccountName = c.String(nullable: false, maxLength: 80),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Age = c.Int(),
                        Gender = c.String(),
                        DateOfBirth = c.DateTime(),
                        FathersName = c.String(),
                        CustomerType = c.String(),
                        OfficePlotNumber = c.String(),
                        OfficeStreetName = c.String(),
                        OfficeLandMark = c.String(),
                        OfficeColony = c.String(),
                        OfficeCity = c.String(),
                        OfficeState = c.String(),
                        OfficeZipCode = c.String(),
                        ShippingPlotNumber = c.String(),
                        ShippingStreetName = c.String(),
                        ShippingLandMark = c.String(),
                        ShippingColony = c.String(),
                        ShippingCity = c.String(),
                        ShippingState = c.String(),
                        ShippingZipCode = c.String(),
                        Email = c.String(),
                        Mobile = c.String(),
                        EmpergencyContact = c.String(),
                        MainImageName = c.String(),
                        ImageExtention = c.String(),
                        SalesTaxNumber = c.String(),
                        PANNumber = c.String(),
                        Status = c.Boolean(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.AccountName, unique: true, name: "AK_Supplier_AccountName");

            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");

            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);

            CreateTable(
                "dbo.tbl_Store",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 80),
                        WareHouseId = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tbl_WareHouse", t => t.WareHouseId, cascadeDelete: true)
                .Index(t => t.Name, unique: true, name: "AK_Store_Name")
                .Index(t => t.WareHouseId);

            CreateTable(
                "dbo.tbl_StoreItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(),
                        Name = c.String(),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        //  ExtendedPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitId = c.Guid(nullable: false),
                        StoreId = c.Guid(nullable: false),
                        PurchaseBillId = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tbl_Store", t => t.StoreId, cascadeDelete: true)
                .ForeignKey("dbo.tbl_Unit", t => t.UnitId, cascadeDelete: true)
                .Index(t => t.UnitId)
                .Index(t => t.StoreId);

            Sql("ALTER TABLE dbo.tbl_StoreItems ADD ExtendedPrice AS CAST(Quantity * UnitPrice AS Decimal(18, 2))");

            CreateTable(
                "dbo.tbl_WareHouse",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 80),
                        PlotNumber = c.String(),
                        StreetName = c.String(),
                        LandMark = c.String(),
                        Colony = c.String(),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "AK_WareHouse_Name");

            CreateTable(
                "dbo.tbl_StoreCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentCategoryId = c.Int(),
                        CategoryName = c.String(nullable: false, maxLength: 80),
                        Parent_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tbl_StoreCategory", t => t.Parent_Id)
                .Index(t => t.CategoryName, unique: true, name: "AK_StoreCategory_CategoryName")
                .Index(t => t.Parent_Id);

            CreateTable(
                "dbo.tbl_TaxCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentCategoryId = c.Int(),
                        CategoryName = c.String(nullable: false, maxLength: 80),
                        Parent_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tbl_TaxCategory", t => t.Parent_Id)
                .Index(t => t.CategoryName, unique: true, name: "AK_TaxCategory_CategoryName")
                .Index(t => t.Parent_Id);

            CreateTable(
                "dbo.tbl_Tax",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TaxCode = c.String(),
                        Name = c.String(nullable: false, maxLength: 80),
                        Percent = c.Decimal(precision: 18, scale: 2),
                        TaxCategoryId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tbl_TaxCategory", t => t.TaxCategoryId, cascadeDelete: true)
                .Index(t => t.Name, unique: true, name: "AK_Tax_Name")
                .Index(t => t.TaxCategoryId);

            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");

            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.tbl_Tax", "TaxCategoryId", "dbo.tbl_TaxCategory");
            DropForeignKey("dbo.tbl_TaxCategory", "Parent_Id", "dbo.tbl_TaxCategory");
            DropForeignKey("dbo.tbl_StoreCategory", "Parent_Id", "dbo.tbl_StoreCategory");
            DropForeignKey("dbo.tbl_Store", "WareHouseId", "dbo.tbl_WareHouse");
            DropForeignKey("dbo.tbl_StoreItems", "UnitId", "dbo.tbl_Unit");
            DropForeignKey("dbo.tbl_StoreItems", "StoreId", "dbo.tbl_Store");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.tbl_PurchasBill", "SupplierId", "dbo.tbl_Supplier");
            DropForeignKey("dbo.tbl_PurchaseTaxes", "PurchaseBillId", "dbo.tbl_PurchasBill");
            DropForeignKey("dbo.tbl_PurchaseItems", "UnitId", "dbo.tbl_Unit");
            DropForeignKey("dbo.tbl_PurchaseItems", "PurchaseBillId", "dbo.tbl_PurchasBill");
            DropForeignKey("dbo.tbl_Item", "UnitId", "dbo.tbl_Unit");
            DropForeignKey("dbo.tbl_Item", "ItemCategoryId", "dbo.tbl_ItemCategory");
            DropForeignKey("dbo.tbl_ItemCategory", "Parent_Id", "dbo.tbl_ItemCategory");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.tbl_Tax", new[] { "TaxCategoryId" });
            DropIndex("dbo.tbl_Tax", "AK_Tax_Name");
            DropIndex("dbo.tbl_TaxCategory", new[] { "Parent_Id" });
            DropIndex("dbo.tbl_TaxCategory", "AK_TaxCategory_CategoryName");
            DropIndex("dbo.tbl_StoreCategory", new[] { "Parent_Id" });
            DropIndex("dbo.tbl_StoreCategory", "AK_StoreCategory_CategoryName");
            DropIndex("dbo.tbl_WareHouse", "AK_WareHouse_Name");
            DropIndex("dbo.tbl_StoreItems", new[] { "StoreId" });
            DropIndex("dbo.tbl_StoreItems", new[] { "UnitId" });
            DropIndex("dbo.tbl_Store", new[] { "WareHouseId" });
            DropIndex("dbo.tbl_Store", "AK_Store_Name");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.tbl_Supplier", "AK_Supplier_AccountName");
            DropIndex("dbo.tbl_PurchaseTaxes", new[] { "PurchaseBillId" });
            DropIndex("dbo.tbl_PurchaseItems", new[] { "PurchaseBillId" });
            DropIndex("dbo.tbl_PurchaseItems", new[] { "UnitId" });
            DropIndex("dbo.tbl_PurchasBill", new[] { "SupplierId" });
            DropIndex("dbo.tbl_PurchasBill", "AK_PurchaseBill_BillInvoice");
            DropIndex("dbo.tbl_Unit", "AK_Unit_UnitName");
            DropIndex("dbo.tbl_Item", new[] { "UnitId" });
            DropIndex("dbo.tbl_Item", new[] { "ItemCategoryId" });
            DropIndex("dbo.tbl_Item", "AK_Item_ItemCode");
            DropIndex("dbo.tbl_ItemCategory", new[] { "Parent_Id" });
            DropIndex("dbo.tbl_ItemCategory", "AK_ItemCategory_CategoryName");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.tbl_Tax");
            DropTable("dbo.tbl_TaxCategory");
            DropTable("dbo.tbl_StoreCategory");
            DropTable("dbo.tbl_WareHouse");
            DropTable("dbo.tbl_StoreItems");
            DropTable("dbo.tbl_Store");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.tbl_Supplier");
            DropTable("dbo.tbl_PurchaseTaxes");
            DropTable("dbo.tbl_PurchaseItems");
            DropTable("dbo.tbl_PurchasBill");
            DropTable("dbo.tbl_Unit");
            DropTable("dbo.tbl_Item");
            DropTable("dbo.tbl_ItemCategory");
        }
    }
}
