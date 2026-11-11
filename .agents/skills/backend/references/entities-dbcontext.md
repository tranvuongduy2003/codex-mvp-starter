# EF Core Model And Repository Workflow

Use this for `src/Infrastructure/Persistence`, EF configurations, repositories, and database-facing integration tests. `src/Infrastructure/AGENTS.md` owns persistence rules.

## Scout

```powershell
rg -n "DbSet<|ApplyConfigurationsFromAssembly|HasDefaultSchema" src/Infrastructure/Persistence
rg -n "IEntityTypeConfiguration|ToTable|HasKey|Property\(" src/Infrastructure/Persistence/Configurations
rg -n "interface I.*Repository|class .*Repository" src/Application src/Infrastructure/Persistence/Repositories
rg -n "ApplicationDatabaseContext|IntegrationTestFixture" tests
```

## Change Flow

1. Read the domain/application model that owns the behavior before editing EF.
2. Confirm identifier shape before mapping: internal PK/FK columns are `integer`/`bigint` (`int`/`long`);
   `uuid` is only a separately named, documented external identifier. Do not add GUID keys.
3. Update Application ports first when persistence is accessed through a new capability.
4. Implement or adjust Infrastructure adapters after the port shape is clear.
5. Keep EF configuration near existing configurations for the same aggregate/context.
6. Add integration coverage when SQL translation, transactions, mappings, concurrency, or provider behavior matter.
7. Use `references/migrations.md` when schema changes require a new migration.
