using FluentMigrator;

namespace NancySample.Migrations
{
    [Migration(201512232052)]
    public class _201512232052_CreateUserTable : Migration
    {
        public override void Up()
        {
            Create.Table("users")
                .WithColumn("id").AsInt64().NotNullable().PrimaryKey("users_pk").Identity()
                .WithColumn("username").AsString(40).Unique("users_username")
                .WithColumn("uuid").AsGuid().Unique("users_uuid")
                .WithColumn("passhash").AsString()
                .WithColumn("secret").AsString(20)
                .WithColumn("email").AsString(80).Unique("users_email");
        }

        public override void Down()
        {
            Delete.Table("users");
        }
    }
}
