# Operación del branding dinámico

## Alcance

Este runbook cubre el alta, despliegue, verificación y reversión del branding
que las webs cargan desde `ConfigurationService` según su hostname. El flujo no
requiere cambios por `BrandId`, acceso directo a pods ni operaciones manuales
sobre la base de datos.

Contrato público:

```text
GET /api/v1/brandconfiguration/public/current?host={hostname}
X-Branding-Contract: public-branding-v1
Cache-Control: public, max-age=300
```

El cuerpo permitido contiene únicamente `brandId`, `clientId`, nombre, razón
social, URL de cliente, soporte, tipo de documento, logo y colores. `clientId`
identifica el tenant; no sustituye un token de autorización.

## Precondiciones para habilitar una marca

Antes de publicar DNS o tráfico:

1. La marca y su `BrandConfiguration` están activas y no eliminadas.
2. `ClientUrl` contiene el hostname correcto y no coincide con ninguna otra
   configuración activa. `www` y el dominio raíz se consideran equivalentes.
3. Están confirmados `Name`, `CompanyName`, `SupportEmail`, `LogoUrl`,
   `PrimaryColor`, `SecondaryColor` y `BackgroundColor`.
4. Los campos operativos de los pasos anteriores, incluidos grupos de pago,
   reglas de retiros y plantillas de notificación, fueron confirmados por
   negocio.
5. El dominio tiene TLS y enruta la web genérica correcta.
6. La consulta pública devuelve exactamente la marca esperada.

No se debe inventar una configuración faltante ni reutilizar el identificador
de otra marca como fallback.

## Validación antes de integrar

Desde la raíz de `ecosystem-microservices`:

```powershell
dotnet test Tests\Ecosystem.ConfigurationService.ContractTests\Ecosystem.ConfigurationService.ContractTests.csproj
dotnet build Microservices\ConfigurationService\Api\Ecosystem.ConfigurationService.Api.csproj
```

El pipeline `Deploy to Production` ejecuta las pruebas de contrato antes de
construir o publicar imágenes. Una falla detiene el despliegue antes de afectar
el registry o el clúster.

Las pruebas cubren:

- equivalencia entre dominio raíz, URL completa y prefijo `www`;
- rechazo de hostname inválido;
- exclusión de configuraciones inactivas;
- bloqueo cuando más de una marca activa coincide;
- lista exacta y casing JSON de los campos públicos;
- ausencia de campos administrativos, comisiones, retiros y blockchain.
- nombres y números de los 35 campos del contrato gRPC interno consumido por
  notificaciones, cuentas, wallet e inventario.

## Orden de despliegue

1. Integrar y publicar `ConfigurationService`.
2. Esperar a que Argo CD reporte la aplicación saludable y sincronizada.
3. Verificar el contrato público a través del gateway.
4. Publicar las webs.
5. Verificar cada hostname y después habilitar tráfico.

La operación se realiza por el pipeline y Argo CD. No usar `kubectl exec`,
`kubectl edit`, `kubectl delete`, `kubectl rollout` ni aplicar manifiestos
directamente a producción. Los contextos
`do-nyc3-ecosystem-prod-k8s` y
`do-nyc3-ecosystem-prod-k8s-admin` no son parte de este procedimiento.

Las Applications de los microservicios y de `ecosystem-web`, `recycoin-web` y
`recybot-web` pertenecen al `app-of-apps` central en
`Infrastructure/k8s/argocd/apps`. Los pipelines frontend solo publican imágenes;
no obtienen kubeconfig ni reconcilian recursos.

## Verificación posterior al despliegue

Sustituir `<dominio-marca>` por el hostname que se está habilitando:

```powershell
$response = Invoke-WebRequest `
  -Uri "https://api.ecosystemfx.net/api/v1/brandconfiguration/public/current?host=<dominio-marca>"
$response.StatusCode
$response.Headers['X-Branding-Contract']
$response.Content | ConvertFrom-Json | Select-Object -ExpandProperty data
```

Resultado esperado:

- HTTP `200`;
- cabecera `X-Branding-Contract: public-branding-v1`;
- `success: true`;
- `brandId`, `clientId`, nombre, dominio, logo, colores y soporte pertenecen a
  la misma marca;
- no aparecen campos internos.

La misma comprobación puede ejecutarse sin acceso al clúster:

```powershell
.\Tools\Test-BrandingDeployment.ps1 `
  -HostName @('<dominio-marca>') `
  -ExpectedBrandByHost @{ '<dominio-marca>' = <brand-id> }
```

También existe el workflow manual `Verify Production Branding`, que recibe una
lista de hostnames y un mapa JSON opcional de `BrandId`. No ejecutarlo hasta que
los valores hayan sido confirmados por negocio.

Después abrir la web y verificar:

- título, favicon, logo, nombre y correo de soporte;
- colores primario, secundario y de fondo;
- las solicitudes a APIs internas llevan el `X-Client-ID` resuelto;
- autenticación y una operación de lectura normal siguen funcionando.

En DevTools, la medición más reciente queda disponible con:

```javascript
performance.getEntriesByName('ecosystem.branding.bootstrap').at(-1)
```

Cada carga también emite el evento `ecosystem:branding-bootstrap` con
`outcome`, `durationMs` y `brandId`. Un proveedor RUM puede suscribirse sin
modificar `BrandingService`:

```javascript
window.addEventListener('ecosystem:branding-bootstrap', ({ detail }) => {
  // Enviar detail al proveedor RUM aprobado.
});
```

El outcome `fallback` significa que la web inició con recursos locales y debe
investigarse, aunque la interfaz siga disponible.

## Observabilidad del backend

Meter de .NET:

```text
Ecosystem.ConfigurationService.Branding
```

Instrumentos:

```text
ecosystem.branding.bootstrap.requests{outcome}
ecosystem.branding.bootstrap.duration{outcome}
```

Outcomes: `resolved`, `invalid_host`, `not_found`, `ambiguous` y `error`. Las
etiquetas no incluyen el hostname para evitar cardinalidad no controlada. El
meter queda listo para el collector OpenTelemetry de la plataforma; mientras
no exista exporter, los logs estructurados son la fuente operativa.

Logs relevantes:

- `Public branding resolved for {Host} as BrandId {BrandId}...`
- `Public branding was not found for {Host}`
- `Public branding is ambiguous for {Host}...`
- `Public branding resolution failed...`

Alertas recomendadas cuando el collector esté conectado:

- cualquier `ambiguous`: crítica e inmediata;
- tasa combinada de `error` y `fallback` mayor a 1 % durante 5 minutos;
- p95 de duración mayor a 1 segundo durante 10 minutos;
- aumento de `not_found` después de un cambio de DNS o configuración.

La capacidad del contrato puede comprobarse internamente con
`/health/contracts/public-branding-v1`. La disponibilidad externa se valida por
el endpoint real a través del gateway, no entrando al pod.

## Diagnóstico

| Síntoma | Causa probable | Acción segura |
|---|---|---|
| HTTP 404 | No existe coincidencia activa | Revisar `ClientUrl`, estado y dominio solicitado desde la administración central |
| Log `ambiguous` | Dos configuraciones activas normalizan al mismo dominio | Corregir la duplicidad en ConfigurationService antes de reintentar |
| Web usa fallback | Timeout, respuesta inválida o 404 | Revisar medición del navegador, gateway y logs centrales |
| Logo anterior con colores nuevos | `LogoUrl` vacío/inaccesible o caché | Confirmar URL HTTPS, CORS/CDN y esperar los 300 segundos de caché |
| Tenant incorrecto en llamadas | Web antigua o bootstrap fallido | Confirmar que la versión desplegada incluye el interceptor de tenant |
| Notificaciones/PDF distintos a la web | Configuración central incompleta o consumidor no actualizado | Comparar la misma marca en ConfigurationService y la versión Argo CD |

## Reversión

1. Detener el tráfico nuevo o revertir DNS si la marca se estaba habilitando.
2. Revertir la versión de la web mediante Git/pipeline/Argo CD.
3. Si el problema pertenece al contrato, revertir la versión de
   `ConfigurationService` de la misma manera.
4. Verificar nuevamente el endpoint y el fallback de la web.

No eliminar configuraciones, tablas, esquemas ni bases de datos para hacer
rollback. Este contrato es aditivo y no requiere una reversión de esquema. Una
configuración solo debe desactivarse desde el flujo administrativo después de
confirmar qué registro es incorrecto.
