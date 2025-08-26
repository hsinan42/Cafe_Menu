namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_image_length : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Categories", "CategoryImage", c => c.String(maxLength: 300));
            AlterColumn("dbo.Products", "ProductImage", c => c.String(maxLength: 300));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "ProductImage", c => c.String(maxLength: 150));
            AlterColumn("dbo.Categories", "CategoryImage", c => c.String(maxLength: 150));
        }
    }
}
