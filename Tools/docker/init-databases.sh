#!/bin/bash
# =============================================================================
# init-databases.sh — Creates one database per microservice on first startup.
# PostgreSQL runs this automatically when the data volume is empty.
# =============================================================================
set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    CREATE DATABASE account_service_db;
    CREATE DATABASE configuration_service_db;
    CREATE DATABASE inventory_service_db;
    CREATE DATABASE wallet_service_db;
    CREATE DATABASE notification_service_db;
EOSQL

