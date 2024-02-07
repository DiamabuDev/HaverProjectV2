using Microsoft.EntityFrameworkCore.Migrations;

namespace HaverDevProject.Data
{
    public static class ExtraMigration
    {
        public static void Steps(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
                    CREATE TRIGGER SetNcrQaTimestampOnUpdate
                    AFTER UPDATE ON NcrQas
                    BEGIN
                        UPDATE NcrQas
                        SET RowVersion = randomblob(8)
                        WHERE rowid = NEW.rowid;
                    END
                ");
            migrationBuilder.Sql(
                @"
                    CREATE TRIGGER SetNcrQaTimestampOnInsert
                    AFTER INSERT ON NcrQas
                    BEGIN
                        UPDATE NcrQas
                        SET RowVersion = randomblob(8)
                        WHERE rowid = NEW.rowid;
                    END
                ");            
        }
    }
}
