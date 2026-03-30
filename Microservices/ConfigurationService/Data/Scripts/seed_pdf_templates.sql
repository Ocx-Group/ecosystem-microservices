-- ============================================================
-- PDF Templates Table + Seed Data
-- Run against: configuration_service schema in PostgreSQL
-- ============================================================

CREATE SEQUENCE IF NOT EXISTS pdf_template_id_seq;

CREATE TABLE IF NOT EXISTS configuration_service.pdf_template (
    id                BIGINT PRIMARY KEY DEFAULT nextval('pdf_template_id_seq'::regclass),
    brand_id          BIGINT NOT NULL,
    template_key      VARCHAR(50) NOT NULL,
    html_content      TEXT NOT NULL,
    css_content       TEXT,
    is_active         BOOLEAN NOT NULL DEFAULT true,
    version           INT NOT NULL DEFAULT 1,
    created_at        TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at        TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at        TIMESTAMP,

    CONSTRAINT fk_pdf_template_brand FOREIGN KEY (brand_id)
        REFERENCES configuration_service.brand(id) ON DELETE RESTRICT
);

CREATE UNIQUE INDEX IF NOT EXISTS idx_pdf_template_brand_key
    ON configuration_service.pdf_template(brand_id, template_key)
    WHERE deleted_at IS NULL;

-- ============================================================
-- Invoice template (shared base, brand styling via placeholders)
-- Uses Liquid/Fluid syntax for dynamic content
-- ============================================================

INSERT INTO configuration_service.pdf_template (brand_id, template_key, html_content, css_content)
SELECT b.id, 'invoice', E'<!DOCTYPE html>
<html lang="es">
<head>
  <meta charset="UTF-8">
  <style>
    * { margin: 0; padding: 0; box-sizing: border-box; }
    body { font-family: Arial, Helvetica, sans-serif; background: {{brand.BackgroundColor}}; color: #333; font-size: 12px; }
    .header { background: {{brand.PrimaryColor}}; color: {{brand.SecondaryColor}}; padding: 20px 30px; display: flex; justify-content: space-between; align-items: center; }
    .header img { max-width: 170px; height: auto; }
    .receipt-box { background: {{brand.SecondaryColor}}; color: {{brand.PrimaryColor}}; padding: 8px 16px; border-radius: 4px; font-weight: bold; font-size: 16px; }
    .company-info { padding: 15px 30px; border-bottom: 2px solid {{brand.PrimaryColor}}; }
    .company-info h2 { color: {{brand.PrimaryColor}}; margin-bottom: 5px; }
    .company-info p { color: #666; line-height: 1.6; }
    .section { padding: 15px 30px; }
    .section-title { color: {{brand.PrimaryColor}}; font-size: 14px; font-weight: bold; margin-bottom: 10px; border-bottom: 1px solid {{brand.PrimaryColor}}; padding-bottom: 5px; }
    .customer-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 8px; }
    .customer-grid .label { font-weight: bold; color: #555; }
    .customer-grid .value { color: #333; }
    table { width: 100%; border-collapse: collapse; margin-top: 10px; }
    thead th { background: {{brand.PrimaryColor}}; color: {{brand.SecondaryColor}}; padding: 10px 8px; text-align: left; font-size: 11px; text-transform: uppercase; }
    tbody td { padding: 8px; border-bottom: 1px solid #e0e0e0; }
    tbody tr:nth-child(even) { background: #f9f9f9; }
    .totals { text-align: right; padding: 15px 30px; }
    .totals .total-row { display: flex; justify-content: flex-end; gap: 20px; margin-bottom: 5px; }
    .totals .grand-total { font-size: 16px; font-weight: bold; color: {{brand.PrimaryColor}}; border-top: 2px solid {{brand.PrimaryColor}}; padding-top: 8px; }
    .footer { text-align: center; padding: 10px; color: #999; font-size: 10px; border-top: 1px solid #e0e0e0; margin-top: 20px; }
  </style>
</head>
<body>
  <div class="header">
    {% if brand.LogoUrl %}<img src="{{brand.LogoUrl}}" alt="{{brand.CompanyName}}" />{% endif %}
    <div class="receipt-box">{{brand.DocumentType}} #{{invoice.ReceiptNumber}}</div>
  </div>

  <div class="company-info">
    <h2>{{brand.CompanyName}}</h2>
    <p>
      {% if brand.CompanyIdentifier %}ID: {{brand.CompanyIdentifier}} | {% endif %}
      {{brand.SupportEmail}}
      {% if brand.SupportPhone %} | {{brand.SupportPhone}}{% endif %}
    </p>
    <p>Fecha: {{invoice.Date | date: "%d/%m/%Y"}}</p>
  </div>

  <div class="section">
    <div class="section-title">Datos del Cliente</div>
    <div class="customer-grid">
      <span class="label">Nombre:</span><span class="value">{{customer.Name}} {{customer.LastName}}</span>
      <span class="label">Usuario:</span><span class="value">{{customer.UserName}}</span>
      <span class="label">Email:</span><span class="value">{{customer.Email}}</span>
      <span class="label">Teléfono:</span><span class="value">{{customer.Phone}}</span>
      <span class="label">País:</span><span class="value">{{customer.Country}}</span>
      <span class="label">Ciudad:</span><span class="value">{{customer.City}}</span>
    </div>
  </div>

  <div class="section">
    <div class="section-title">Detalle de Productos</div>
    <table>
      <thead>
        <tr>
          <th>Concepto</th>
          <th>Cantidad</th>
          <th>Precio</th>
          <th>Descuento</th>
          <th>Total</th>
        </tr>
      </thead>
      <tbody>
        {% for item in invoice.Items %}
        <tr>
          <td>{{item.ProductName}}</td>
          <td>{{item.Quantity}}</td>
          <td>${{item.Price | round: 2}}</td>
          <td>${{item.Discount | round: 2}}</td>
          <td>${{item.Total | round: 2}}</td>
        </tr>
        {% endfor %}
      </tbody>
    </table>
  </div>

  <div class="totals">
    <div class="total-row"><span>Subtotal:</span><span>${{invoice.Subtotal | round: 2}}</span></div>
    {% if invoice.TaxTotal > 0 %}<div class="total-row"><span>Impuesto:</span><span>${{invoice.TaxTotal | round: 2}}</span></div>{% endif %}
    <div class="total-row grand-total"><span>Total:</span><span>${{invoice.Total | round: 2}}</span></div>
  </div>

  <div class="footer">
    <p>{{brand.CompanyName}} — Generado el {{invoice.Date | date: "%d/%m/%Y %H:%M"}}</p>
  </div>
</body>
</html>', NULL
FROM configuration_service.brand b
WHERE NOT EXISTS (
    SELECT 1 FROM configuration_service.pdf_template pt
    WHERE pt.brand_id = b.id AND pt.template_key = 'invoice' AND pt.deleted_at IS NULL
);

-- Membership template
INSERT INTO configuration_service.pdf_template (brand_id, template_key, html_content, css_content)
SELECT b.id, 'membership', E'<!DOCTYPE html>
<html lang="es">
<head>
  <meta charset="UTF-8">
  <style>
    * { margin: 0; padding: 0; box-sizing: border-box; }
    body { font-family: Arial, Helvetica, sans-serif; background: {{brand.BackgroundColor}}; color: #333; font-size: 12px; }
    .header { background: {{brand.PrimaryColor}}; color: {{brand.SecondaryColor}}; padding: 20px 30px; display: flex; justify-content: space-between; align-items: center; }
    .header img { max-width: 170px; height: auto; }
    .receipt-box { background: {{brand.SecondaryColor}}; color: {{brand.PrimaryColor}}; padding: 8px 16px; border-radius: 4px; font-weight: bold; font-size: 16px; }
    .company-info { padding: 15px 30px; border-bottom: 2px solid {{brand.PrimaryColor}}; }
    .company-info h2 { color: {{brand.PrimaryColor}}; margin-bottom: 5px; }
    .welcome { padding: 20px 30px; text-align: center; }
    .welcome h1 { color: {{brand.PrimaryColor}}; font-size: 22px; margin-bottom: 10px; }
    .welcome p { color: #666; font-size: 14px; }
    .details { padding: 15px 30px; }
    .details .label { font-weight: bold; color: #555; }
    .amount { font-size: 24px; font-weight: bold; color: {{brand.PrimaryColor}}; text-align: center; padding: 20px; }
    .footer { text-align: center; padding: 10px; color: #999; font-size: 10px; border-top: 1px solid #e0e0e0; margin-top: 20px; }
  </style>
</head>
<body>
  <div class="header">
    {% if brand.LogoUrl %}<img src="{{brand.LogoUrl}}" alt="{{brand.CompanyName}}" />{% endif %}
    <div class="receipt-box">Membresía #{{invoice.ReceiptNumber}}</div>
  </div>

  <div class="company-info">
    <h2>{{brand.CompanyName}}</h2>
    <p>{% if brand.CompanyIdentifier %}ID: {{brand.CompanyIdentifier}} | {% endif %}{{brand.SupportEmail}}</p>
  </div>

  <div class="welcome">
    <h1>¡Bienvenido a {{brand.CompanyName}}!</h1>
    <p>Tu membresía ha sido activada exitosamente.</p>
  </div>

  <div class="details">
    <p><span class="label">Nombre:</span> {{customer.Name}} {{customer.LastName}}</p>
    <p><span class="label">Usuario:</span> {{customer.UserName}}</p>
    <p><span class="label">Fecha:</span> {{invoice.Date | date: "%d/%m/%Y"}}</p>
  </div>

  <div class="amount">${{invoice.Total | round: 2}}</div>

  <div class="footer">
    <p>{{brand.CompanyName}} — Generado el {{invoice.Date | date: "%d/%m/%Y %H:%M"}}</p>
  </div>
</body>
</html>', NULL
FROM configuration_service.brand b
WHERE NOT EXISTS (
    SELECT 1 FROM configuration_service.pdf_template pt
    WHERE pt.brand_id = b.id AND pt.template_key = 'membership' AND pt.deleted_at IS NULL
);
