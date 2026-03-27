-- ============================================================
-- Brand Configuration Table + Seed Data
-- Run against: configuration_service schema in PostgreSQL
-- ============================================================

-- Create sequence
CREATE SEQUENCE IF NOT EXISTS brand_configuration_id_seq;

-- Create table
CREATE TABLE IF NOT EXISTS configuration_service.brand_configuration (
    id                              BIGINT PRIMARY KEY DEFAULT nextval('brand_configuration_id_seq'::regclass),
    brand_id                        BIGINT NOT NULL,
    admin_user_name                 VARCHAR(100) NOT NULL,

    -- Email / notifications
    sender_name                     VARCHAR(255) NOT NULL,
    sender_email                    VARCHAR(255) NOT NULL,
    email_template_folder           VARCHAR(100) NOT NULL,

    -- Frontend
    client_url                      VARCHAR(500) NOT NULL,

    -- Commission distribution
    commission_enabled              BOOLEAN NOT NULL DEFAULT false,
    commission_levels               JSONB NOT NULL DEFAULT '[]'::jsonb,
    bonus_percentage                DECIMAL(10,2) NOT NULL DEFAULT 0,

    -- PDF / Invoice branding
    pdf_template_name               VARCHAR(100) NOT NULL,
    company_name                    VARCHAR(255) NOT NULL,
    company_identifier              VARCHAR(100),
    support_email                   VARCHAR(255) NOT NULL,
    support_phone                   VARCHAR(50),
    document_type                   VARCHAR(100),
    logo_url                        VARCHAR(500),
    primary_color                   VARCHAR(20) NOT NULL DEFAULT '#000000',
    secondary_color                 VARCHAR(20) NOT NULL DEFAULT '#FFFFFF',
    background_color                VARCHAR(20) NOT NULL DEFAULT '#FFFFFF',

    -- Affiliate tree
    default_father_affiliate_id     INT,
    activate_on_registration        BOOLEAN NOT NULL DEFAULT true,

    -- Payment groups
    default_payment_group_id        INT,
    trading_academy_payment_group_id INT,

    -- Withdrawal rules
    withdrawal_validation_type      VARCHAR(50) NOT NULL DEFAULT 'None',
    withdrawal_time_zone            VARCHAR(100),
    withdrawal_start_hour           INT,
    withdrawal_end_hour             INT,
    withdrawal_cap_no_directs       DECIMAL(18,2),
    requires_10_percent_purchase_rule BOOLEAN NOT NULL DEFAULT false,
    pool_validation_required        BOOLEAN NOT NULL DEFAULT false,

    -- Crypto / ConPayment
    con_payment_enabled             BOOLEAN NOT NULL DEFAULT false,
    con_payment_address             VARCHAR(255),
    blockchain_network_id           INT,

    -- Status & audit
    is_active                       BOOLEAN NOT NULL DEFAULT true,
    created_at                      TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                      TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at                      TIMESTAMP,

    CONSTRAINT fk_brand_configuration_brand FOREIGN KEY (brand_id)
        REFERENCES configuration_service.brand(id) ON DELETE RESTRICT
);

CREATE UNIQUE INDEX IF NOT EXISTS idx_brand_configuration_brand_id
    ON configuration_service.brand_configuration(brand_id)
    WHERE deleted_at IS NULL;

-- ============================================================
-- Seed data for 4 existing brands
-- Values extracted from old microservices hardcoded constants
-- ============================================================

INSERT INTO configuration_service.brand_configuration
    (brand_id, admin_user_name,
     sender_name, sender_email, email_template_folder, client_url,
     commission_enabled, commission_levels, bonus_percentage,
     pdf_template_name, company_name, company_identifier, support_email, support_phone,
     document_type, primary_color, secondary_color, background_color,
     default_father_affiliate_id, activate_on_registration,
     default_payment_group_id, trading_academy_payment_group_id,
     withdrawal_validation_type, withdrawal_time_zone, withdrawal_start_hour, withdrawal_end_hour,
     withdrawal_cap_no_directs, requires_10_percent_purchase_rule, pool_validation_required,
     con_payment_enabled, con_payment_address, blockchain_network_id)
VALUES
-- Brand 1: Ecosystem
(1, 'adminecosystem',
 'Ecosystem Sharing Evolution', 'support@ecosystemfx.net', 'Ecosystem', 'https://ecosystemfx.net/',
 false, '[]'::jsonb, 0,
 'ecosystem', 'Ecosystem OCX Sharing Evolution S.A.', '3-101-844938',
 'facturacion@ecosystemfx.com', NULL,
 'Factura de servicios', '#257272', '#FFFFFF', '#FFFFFF',
 NULL, false,
 2, 6,
 'DatabaseDriven', 'Central America Standard Time', 8, 18,
 NULL, false, true,
 false, NULL, 99),

-- Brand 2: RecyCoin
(2, 'adminrecycoin',
 'Recycoin', 'support@recycoin.net', 'RecyCoin', 'https://recycoin.net/',
 true, '[15.0, 5.0]'::jsonb, 50,
 'recycoin', 'OCX GROUP SOCIEDAD ANONIMA', '3-101-865783',
 'support@recycoin.net', '+9802510215',
 'Recibo', '#1E90FF', '#39FF14', '#1A1A2E',
 12557, true,
 11, 14,
 'None', NULL, NULL, NULL,
 75.00, true, false,
 false, NULL, 56),

-- Brand 3: HouseCoin
(3, 'adminhousecoin',
 'Housecoin', 'support@thehousecoin.net', 'HouseCoin', 'https://thehousecoin.net/',
 true, '[8.0, 6.0, 5.0, 4.0, 2.0]'::jsonb, 0,
 'housecoin', 'SMART INTERNATIONAL INVESTMENTS LLC', 'L24000157429',
 'support@thehousecoin.net', '(561) 694-8107',
 'Recibo', '#64FFDA', '#CCD6F6', '#0A192F',
 12586, true,
 12, NULL,
 'FridayUtc', 'UTC', 0, 24,
 NULL, false, false,
 true, 'THiJ78d6DHm1575GfFyfe1K6k2Fp6Bq5pP', 202),

-- Brand 4: Éxito Juntos
(4, 'adminexitojuntos',
 'Éxito Juntos', 'support@exitojuntos.com', 'ExitoJuntos', 'https://exitojuntos.com/',
 false, '[]'::jsonb, 0,
 'exitojuntos', 'Éxito Juntos', NULL,
 'support@exitojuntos.com', NULL,
 'Recibo', '#DAA520', '#FFFFFF', '#000000',
 12673, true,
 13, NULL,
 'None', NULL, NULL, NULL,
 NULL, false, false,
 false, NULL, NULL)

ON CONFLICT DO NOTHING;
