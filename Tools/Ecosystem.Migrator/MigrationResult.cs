namespace Ecosystem.Migrator;

internal enum MigrationResult
{
    Success,
    Skipped,
    Retry,
    LockHeld
}

