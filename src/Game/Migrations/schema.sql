-- gen_random_uuid()
-- or just uuid()

-- https://shell.duckdb.org/#queries=v0,%20%20-Create-table-from-Parquet-file%0ACREATE-TABLE-train_services-AS%0A----FROM-'s3%3A%2F%2Fduckdb%20blobs%2Ftrain_services.parquet'~,%20%20-Get-the-top%203-busiest-train-stations%0ASELECT-station_name%2C-count(*)-AS-num_services%0AFROM-train_services%0AGROUP-BY-ALL%0AORDER-BY-num_services-DESC%0ALIMIT-3~

CREATE TABLE IF NOT EXISTS manipulativeDef (
    id VARCHAR(36) PRIMARY KEY,
    name TEXT NOT NULL,
    imageRes TEXT NOT NULL,
    isWeapon BOOLEAN NOT NULL,
    isHelmet BOOLEAN NOT NULL,
    isArmor BOOLEAN NOT NULL,
    atk INTEGER NOT NULL,
    def INTEGER NOT NULL
);
