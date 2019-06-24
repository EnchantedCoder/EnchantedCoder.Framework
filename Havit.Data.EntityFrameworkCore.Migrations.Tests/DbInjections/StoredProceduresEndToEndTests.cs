﻿using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Havit.Data.EntityFrameworkCore.Migrations.Tests.DbInjections
{
    public class StoredProceduresEndToEndTests
    {
        [TestClass]
        public class AddingStoredProcedure
        {
            [Table("Dummy")]
            private class DummySource
            {
                public int Id { get; set; }
            }

            [Table("Dummy")]
            private class DummyTarget
            {
                public int Id { get; set; }
            }

            [TestMethod]
            public void StoredProceduresDbInjections_EndToEnd_AddingStoredProcedure()
            {
                var procedure = "CREATE OR ALTER PROCEDURE [dbo].[GetTables]() AS BEGIN SELECT * FROM [sys].[tables] END";

                var source = new EndToEndTestDbContext<DummySource>();
                var target = new EndToEndTestDbContext<DummyTarget>(builder => builder.HasAnnotation("StoredProcedure:GetTables", procedure));
                var migrations = source.Migrate(target);

                Assert.AreEqual(1, migrations.Count);
                Assert.AreEqual(
                    procedure,
                    migrations[0].CommandText);
            }
        }

        [TestClass]
        public class ModifyingStoredProcedure
        {
            [Table("Dummy")]
            private class DummySource
            {
                public int Id { get; set; }
            }

            [Table("Dummy")]
            private class DummyTarget
            {
                public int Id { get; set; }
            }

            [TestMethod]
            public void StoredProceduresDbInjections_EndToEnd_ModifyingStoredProcedure()
            {
                var source = new EndToEndTestDbContext<DummySource>(builder => builder.HasAnnotation("StoredProcedure:GetTables", "CREATE PROCEDURE [dbo].[GetTables]() AS BEGIN SELECT * FROM [sys].[tables] END"));
                var newProcedure = "CREATE PROCEDURE [dbo].[GetTables]() AS BEGIN SELECT * FROM [sys].[tables] WHERE schema_id = 1 END";
	            var newProcedureAlter = "ALTER PROCEDURE [dbo].[GetTables]() AS BEGIN SELECT * FROM [sys].[tables] WHERE schema_id = 1 END";
				var target = new EndToEndTestDbContext<DummyTarget>(builder => builder.HasAnnotation("StoredProcedure:GetTables", newProcedure));
                var migrations = source.Migrate(target);

                Assert.AreEqual(1, migrations.Count);
                Assert.AreEqual(
                    newProcedureAlter,
                    migrations[0].CommandText);
            }
        }

        [TestClass]
        public class DeletingStoredProcedure
        {
            [Table("Dummy")]
            private class DummySource
            {
                public int Id { get; set; }
            }

            [Table("Dummy")]
            private class DummyTarget
            {
                public int Id { get; set; }
            }

            [TestMethod]
            public void StoredProceduresDbInjections_EndToEnd_DeletingStoredProcedure()
            {
                var source = new EndToEndTestDbContext<DummySource>(builder => builder.HasAnnotation("StoredProcedure:GetTables", "CREATE OR ALTER PROCEDURE [dbo].[GetTables]() AS BEGIN SELECT * FROM [sys].[tables] END"));
                var target = new EndToEndTestDbContext<DummyTarget>();
                var migrations = source.Migrate(target);

                Assert.AreEqual(1, migrations.Count);
                Assert.AreEqual(
                    "DROP PROCEDURE [GetTables]",
                    migrations[0].CommandText);
            }
        }
    }
}