# Migración del sistema de branding

Última actualización: 2026-07-29

Estado general: recomendaciones implementadas localmente; pendiente de
configuración empresarial, commits y despliegue controlado

## Objetivo

Conseguir que una marca pueda configurarse desde `ConfigurationService` sin
tener que modificar, recompilar o desplegar código específico por `BrandId`.

Este documento es la referencia de continuidad para los siguientes pasos. Debe
actualizarse después de cada implementación, validación, commit o despliegue.

## Reglas de seguridad

- No eliminar bases de datos, esquemas ni tablas históricas.
- No añadir migraciones con `DROP`, `TRUNCATE` o borrados masivos como parte de
  esta consolidación.
- Antes de dejar de consumir una fuente antigua, comprobar que todas sus marcas
  activas existen en la fuente central.
- Separar los cambios de esquema de los cambios de aplicación y respetar el
  orden de despliegue entre productor y consumidores.
- No inventar configuración empresarial faltante. Los valores de cada marca
  deben ser confirmados o administrados mediante `ConfigurationService`.
- Conservar compatibilidad temporal de lectura cuando una web antigua todavía
  dependa de un endpoint previo.

## Estado de las recomendaciones

| Paso | Recomendación | Estado | Entrega |
|---|---|---|---|
| 1 | Ejecutar migraciones de producción de forma centralizada y segura | Completado | Commit `b96ec8f` |
| 2 | Hacer de `ConfigurationService` la única fuente de branding para notificaciones y PDFs | Implementado y validado localmente | Backend en commit `46369de`; pendiente de despliegue |
| 3 | Sustituir reglas y valores codificados por `BrandId` en AccountService y WalletService | Implementado y validado localmente | Backend en commit `46369de`; pendiente de despliegue |
| 4 | Retirar decisiones por marca restantes en InventoryService | Implementado y validado localmente | Backend en commit `477436e`; pendiente de despliegue |
| 5 | Verificar que todas las webs obtengan identidad visual y datos públicos en tiempo de ejecución | Implementado, validado y confirmado | Endpoint `5ed8af0`; webs `57dd242`, `20d123a`, `f4dc27f`, `028dec1` |
| 6 | Añadir pruebas de contrato, observabilidad y documentación operativa del flujo completo | Implementado, validado y confirmado | Backend `1cf605e`; instrumentación incluida en commits de las webs |
| 7 | Centralizar la entrega frontend en GitOps y verificarla sin acceso directo a pods | Implementado, validado y confirmado | GitOps `e42081a`; pipelines incluidos en commits frontend |

## Paso 1: migraciones de producción

Estado: completado.

Commit: `b96ec8f feat(db): add migration job and adopt existing schema for branding`

Implementado:

- Migrador central `Tools/Ecosystem.Migrator`.
- Job `PreSync` de Argo CD antes de desplegar los servicios.
- Construcción y publicación de la imagen `migrator` antes de las imágenes de
  aplicación.
- Migración inicial de `ConfigurationService` que adopta el esquema existente y
  crea la infraestructura de branding.
- Snapshot y factoría de diseño de EF Core.
- Reintentos y manejo del bloqueo de migraciones.

Precaución:

- La migración adopta estructuras existentes y no debe sustituirse por una
  recreación destructiva de la base.

## Paso 2: consolidación de NotificationService

Estado: implementado y confirmado en `46369de`; pendiente de despliegue.

Resultado:

- `ConfigurationService` expone por gRPC una configuración sanitizada de marca.
- Correos, consumidores y PDFs de `NotificationService` leen branding desde
  `ConfigurationService`.
- Se retiraron de `NotificationService` el repositorio, modelo, comandos,
  handlers, validador y `DbSet` duplicados de configuración de marca.
- El endpoint antiguo `GET /api/v1/email-sender-config` permanece como fachada
  de solo lectura para las webs existentes.
- Ya no existen endpoints de escritura de branding en `NotificationService`.
- `NotificationService` espera durante el arranque a que `ConfigurationService`
  publique el contrato compatible.
- El migrador compara en modo lectura las marcas activas antiguas con las
  configuraciones centrales y bloquea el cambio si falta alguna.

Protección de datos:

- La tabla física
  `notification_service.brand_configurations` no se elimina ni se modifica.
- El guard consulta por separado la conexión de NotificationService y la de
  ConfigurationService, por lo que también funciona si usan bases distintas.
- Durante la implementación y las validaciones no se abrió ninguna conexión a
  una base de datos.

Archivos principales:

- `Domain/Protos/configuration.proto`
- `Microservices/ConfigurationService/Api/GrpcServices/ConfigurationGrpcService.cs`
- `Microservices/NotificationService/Application/Adapters/IBrandConfigurationReader.cs`
- `Microservices/NotificationService/Application/Adapters/ConfigurationServicePdfAdapter.cs`
- `Microservices/NotificationService/Api/Controllers/BrandConfigurationController.cs`
- `Tools/Ecosystem.Migrator/Program.cs`
- `Infrastructure/k8s/services/configuration-service/deployment.yaml`
- `Infrastructure/k8s/services/notification-service/deployment.yaml`

Validaciones realizadas:

- `Ecosystem.Migrator`: compilación correcta, 0 errores.
- `ConfigurationService.Api`: compilación correcta, 0 errores.
- `NotificationService.Api`: compilación correcta, 0 errores.
- Manifiestos Kustomize de ConfigurationService y NotificationService válidos.
- `git diff --check` correcto.
- Sin referencias residuales al repositorio o `DbSet` local de branding.

Precondición operativa:

- Las semillas centrales conocidas incluyen `BrandId` 1 a 4.
- Recybotia utiliza `BrandId 5`.
- Si existe una configuración antigua activa para la marca 5 y todavía no hay
  una configuración central activa, el migrador bloqueará deliberadamente el
  despliegue.
- La configuración de la marca 5 debe completarse en `ConfigurationService` con
  valores empresariales confirmados antes del despliegue. No debe inventarse ni
  copiarse automáticamente sin revisión.

## Paso 3: eliminar reglas codificadas por marca

Estado: implementación terminada y confirmada en `46369de`; pendiente de
pruebas automatizadas de políticas y despliegue.

Resultado:

- El contrato gRPC de `BrandConfiguration` incluye ahora las políticas internas
  necesarias para AccountService y WalletService.
- AccountService consume la configuración central para:
  - padre, patrocinador y patrocinador binario predeterminados;
  - activación automática de registros normales y registros con Google;
  - red blockchain configurada para la dirección de cada marca.
- WalletService consume la configuración central para:
  - usuario administrador de movimientos, créditos, reversiones y matrices;
  - obligación de tener un pool activo;
  - grupo de pago utilizado para compras y balances;
  - cálculo por total de factura cuando una marca no tiene grupo de pago ni
    detalles de factura migrados;
  - reglas de fecha y horario de retiro;
  - regla de compras del 10 %;
  - límite de retiro para afiliados sin directos;
  - comisiones, PDFs y construcción de débitos que antes dependían de una
    interfaz no registrada.
- Se eliminaron los fallbacks que convertían un tenant inválido en `BrandId 2`.
- Se retiraron las constantes de nombres de administrador, remitentes, marcas y
  padres que quedaron sin uso.
- Una configuración central ausente produce un fallo explícito; ya no utiliza
  silenciosamente la configuración de Ecosystem o RecyCoin.

Corrección adicional:

- WalletService ya utilizaba `IBrandConfigurationProvider` en varios servicios,
  pero esa interfaz no estaba registrada en su contenedor. Compilaba, aunque los
  flujos podían fallar al resolverse en ejecución.
- Esos consumidores usan ahora `IConfigurationAdapter`, que sí está registrado
  y obtiene la configuración por gRPC.
- WalletService anunciaba el puerto gRPC `50051` en Kubernetes, pero no lo
  abría en Kestrel. El programa configura ahora HTTP en `8080`, gRPC HTTP/2 en
  `50051` y excluye la ruta interna `/wallet.WalletGrpc/` de la resolución de
  tenant basada en cabeceras.

Despliegue:

- AccountService y WalletService reciben
  `GrpcServices__ConfigurationService=http://configuration-service:50051`.
- Ambos esperan el contrato `brand-configuration-v2` antes de iniciar.
- NotificationService conserva la señal `brand-configuration-v1`, porque solo
  necesita el subconjunto de branding del paso 2.
- Esto impide que los consumidores de políticas arranquen contra una versión
  de ConfigurationService que aún no incluya los campos del paso 3.

Validaciones realizadas:

- `ConfigurationService.Api`: compilación correcta, 0 errores.
- `AccountService.Application`: compilación correcta, 0 errores.
- `AccountService.Infra.IoC`: compilación correcta, 0 errores.
- `WalletService.Api`: compilación correcta, 0 errores.
- `NotificationService.Api`: recompilado correctamente con el contrato
  ampliado.
- Manifiestos Kustomize de AccountService y WalletService válidos.
- `appsettings.json` de AccountService válido.
- Búsqueda estática sin decisiones `brandId == número`, `brandId switch`,
  fallbacks de tenant a la marca 2 ni nombres de administradores codificados en
  AccountService y WalletService.
- `git diff --check` correcto.

Limitaciones de validación:

- El repositorio no contiene proyectos de pruebas automatizadas. La cobertura
  por marcas actuales y por una marca nueva sigue pendiente.
- La compilación completa de `AccountService.Api` quedó impedida localmente por
  un archivo de caché Razor bloqueado en `Api/obj`. Las capas Application,
  Infra.IoC y todas sus dependencias sí compilaron. No se borró el directorio
  `obj` ni se detuvieron procesos del usuario.
- Un intento de salida alternativa no pudo restaurar paquetes por el acceso de
  red restringido; no representa un error de compilación del código.

Protección de datos:

- Este paso no añade migraciones ni cambios de esquema.
- No se conectó a ninguna base de datos.
- No se ejecutaron operaciones de escritura, copia o eliminación de datos.

Pendiente para cerrar completamente el paso:

- Añadir una suite de pruebas de políticas de marca.
- Ejecutar la compilación completa de AccountService.Api cuando el archivo de
  caché deje de estar bloqueado.
- Confirmar la configuración central de `BrandId 5` antes del despliegue.

## Paso 4: retirar decisiones por marca en InventoryService

Estado: implementado y confirmado en `477436e`; pendiente de despliegue.

Resultado:

- InventoryService consume por gRPC la configuración de marca publicada por
  ConfigurationService.
- `GetAllTradingAcademyHandler` obtiene
  `TradingAcademyPaymentGroupId` de la configuración central; ya no decide
  entre los grupos 6 y 14 mediante `TenantId`.
- El endpoint heredado `GET /api/v1/product/get_all_recycoin` se conserva para
  no romper las webs antiguas, pero funciona como fachada del grupo
  `DefaultPaymentGroupId` de la marca activa.
- Ambos flujos filtran los productos por el `BrandId` resuelto, además del
  grupo de pago configurado.
- Se eliminaron los métodos especiales del repositorio y las constantes 6, 11
  y 14 que codificaban las marcas anteriores.
- Una marca sin configuración activa o sin el grupo requerido produce un fallo
  explícito; no hereda silenciosamente los valores de otra marca.

Comunicación y despliegue:

- InventoryService abre HTTP en `8080` y gRPC HTTP/2 en `50051`.
- La ruta interna `/inventory.InventoryGrpc/` queda fuera de la resolución de
  tenant por cabeceras para permitir las llamadas gRPC de WalletService.
- InventoryService recibe
  `GrpcServices__ConfigurationService=http://configuration-service:50051` y
  espera el contrato `brand-configuration-v2` antes de iniciar.
- WalletService recibe
  `GrpcServices__InventoryService=http://inventory-service:50051`.
- El Service de Kubernetes de InventoryService ya exponía ambos puertos; no fue
  necesario cambiarlo.

Compatibilidad:

- Se conservaron los nombres públicos
  `get_all_trading_academy` y `get_all_recycoin`.
- El nombre RecyCoin permanece únicamente como compatibilidad de API. Ya no
  selecciona una marca ni un grupo de pago específico en el código.
- Las semillas centrales existentes ya contienen los valores equivalentes:
  marca 1 usa grupos 2/6 y marca 2 usa grupos 11/14. No se modificaron ni se
  ejecutaron esas semillas durante este paso.

Validaciones realizadas:

- `InventoryService.Api`: compilación correcta, 0 errores.
- Manifiestos Kustomize de InventoryService y WalletService válidos.
- `appsettings.json` de InventoryService válido.
- Búsqueda estática sin comparaciones de tenant o marca contra números ni
  referencias a las constantes retiradas.
- Los únicos nombres específicos restantes son los endpoints heredados
  conservados por compatibilidad.
- `git diff --check` correcto; solo se reportaron avisos de normalización
  LF/CRLF del entorno.

Limitaciones de validación:

- La compilación conserva advertencias preexistentes de nulabilidad.
- El aviso de seguridad de AutoMapper queda fuera de esta migración por decisión
  expresa; no bloquea la compilación del paso 4.
- No se ejecutaron pruebas contra una base de datos ni contra el clúster.

Protección de datos:

- Este paso no añade ni ejecuta migraciones.
- No se abrió ninguna conexión a una base de datos.
- No se escribieron, copiaron ni eliminaron datos.

Precondición operativa:

- Cada marca que utilice estos endpoints debe tener configurado
  `DefaultPaymentGroupId` y, cuando corresponda,
  `TradingAcademyPaymentGroupId` en ConfigurationService.
- La configuración empresarial pendiente de `BrandId 5` sigue bloqueando un
  despliegue seguro y no debe inventarse.

## Paso 5: branding de las webs en tiempo de ejecución

Estado: endpoint confirmado en `5ed8af0` y webs confirmadas en `57dd242`
(`web`), `20d123a` (`recycoin`), `f4dc27f` (`housecoin`) y `028dec1`
(`recybot`); pendiente de configuración central completa y despliegue.

Webs auditadas:

- `web` — Ecosystem, Angular 15.
- `recycoin` — RecyCoin, Angular 20.
- `housecoin` — HouseCoin, Angular 15.
- `recybot` — Recybotia, Angular 20.

Contrato público:

- ConfigurationService publica
  `GET /api/v1/brandconfiguration/public/current?host={hostname}`.
- El hostname se normaliza, incluyendo la equivalencia con `www`, y debe
  coincidir con exactamente una configuración activa.
- El endpoint solo devuelve los datos necesarios para iniciar la web:
  `BrandId`, identificador de tenant, nombre, razón social, URL del cliente,
  soporte, tipo de documento, logo y colores.
- El identificador de tenant se utiliza como `X-Client-ID`, pero no autoriza
  una solicitud. Los endpoints protegidos siguen requiriendo sus tokens de
  autorización y esos tokens no forman parte del contrato público.
- No se publican políticas de retiros, comisiones, usuarios administradores,
  direcciones blockchain ni otros campos internos.

Inicialización de las webs:

- Las cuatro aplicaciones cargan el branding por `window.location.hostname`
  antes de iniciar la interfaz.
- La consulta tiene un límite de cinco segundos. Si ConfigurationService no
  está disponible, la aplicación inicia con sus recursos visuales anteriores
  como fallback.
- El nombre actualiza el título del documento.
- `LogoUrl` actualiza favicon, componentes de logo y las imágenes públicas
  marcadas como identidad de marca.
- Nombre y correo de soporte se sustituyen en las vistas públicas marcadas.
- Los colores crean variables CSS de marca y actualizan las variables comunes
  de Bootstrap y los estilos primarios.
- Un interceptor sobrescribe `X-Client-ID` para las APIs internas con el tenant
  resuelto por dominio. Los valores compilados anteriores quedan únicamente
  como fallback de compatibilidad.

Hardcodes retirados:

- `TicketHubService` ya no fija los `BrandId` 1, 2 o 3.
- Se corrigió el caso de Recybotia que enviaba incorrectamente `BrandId 2`.
- El administrador de plantillas de correo de Recybotia ya no lee
  `environment.brand`.
- Se eliminaron `BrandId 5` y el nombre Recybotia de los archivos de entorno.
- Una identidad ausente produce un error explícito en operaciones que requieren
  tenant; no se sustituye por otra marca.

Validaciones realizadas:

- `ConfigurationService.Api`: compilación correcta, 0 errores.
- Build de producción de `web`: correcta.
- Build de producción de `recycoin`: correcta.
- Build de producción de `housecoin`: correcta.
- Build de producción de `recybot`: correcta.
- TypeScript `--noEmit` correcto en las cuatro webs.
- Búsqueda estática sin `environment.brand` ni asignaciones numéricas de
  `BrandId` en los servicios de tickets.
- `git diff --check` correcto en los cinco repositorios; solo se reportaron
  avisos de normalización LF/CRLF.

Limitaciones de validación:

- Las builds conservan advertencias preexistentes de Sass, CommonJS,
  compatibilidad de navegadores y hojas de estilo antiguas.
- No se hizo una prueba visual contra datos reales porque la configuración no
  está desplegada.
- Las páginas conservan contenido empresarial propio de cada producto, como
  términos legales, white papers, contratos y rutas de compra. Ese contenido
  no es identidad visual y requeriría un CMS o un modelo público adicional si
  se desea una única web completamente white-label.

Precondiciones operativas:

- `ClientUrl` debe contener el dominio correcto y ser único entre las
  configuraciones activas.
- `LogoUrl`, colores, nombre y soporte deben completarse en
  ConfigurationService. Las semillas actuales no asignan `LogoUrl`.
- `BrandId 5` continúa sin configuración central confirmada; Recybotia usará
  sus recursos de fallback y no debe desplegarse como migración completa hasta
  registrar esos datos.
- El orden de despliegue debe ser ConfigurationService primero y las webs
  después.

Protección de infraestructura y datos:

- No se añadieron ni ejecutaron migraciones de base de datos.
- No se abrió ninguna conexión a una base de datos.
- No se consultaron ni manipularon pods.
- No se ejecutaron comandos contra los contextos
  `do-nyc3-ecosystem-prod-k8s` ni
  `do-nyc3-ecosystem-prod-k8s-admin`.

## Paso 6: contrato, observabilidad y operación

Estado: código confirmado en `1cf605e`, instrumentación web incluida en sus
commits y documentación confirmada en `e42081a`; pendiente de despliegue.

Pruebas automatizadas:

- Se añadió el proyecto
  `Tests/Ecosystem.ConfigurationService.ContractTests`.
- Valida normalización de host, equivalencia con `www`, rechazo de entradas
  inválidas, exclusión de configuraciones inactivas y cierre seguro ante
  dominios ambiguos.
- Fija la lista exacta y el casing web de `PublicBrandingDto`.
- Comprueba que el contrato no expone campos administrativos, correo interno,
  comisiones, retiros ni blockchain.
- Fija nombres y números de los 35 campos del contrato gRPC interno para
  impedir cambios incompatibles con NotificationService, AccountService,
  WalletService e InventoryService.
- El workflow de producción ejecuta estas pruebas antes de construir o publicar
  imágenes.

Observabilidad:

- La resolución quedó separada en `PublicBrandingResolver`, por lo que puede
  verificarse sin una base de datos.
- `ConfigurationService` produce logs estructurados para los resultados
  `resolved`, `invalid_host`, `not_found`, `ambiguous` y `error`.
- El meter `Ecosystem.ConfigurationService.Branding` publica contadores y
  duración sin usar hostnames como etiquetas de alta cardinalidad.
- El contrato exitoso devuelve `X-Branding-Contract: public-branding-v1` y
  caché pública de cinco minutos; errores usan `no-store`.
- Se añadió `/health/contracts/public-branding-v1`.
- Las cuatro webs registran
  `performance.getEntriesByName('ecosystem.branding.bootstrap')` y emiten
  `ecosystem:branding-bootstrap` con outcome, duración y marca resuelta.

Documentación:

- `docs/BRANDING_OPERATIONS_RUNBOOK.md` contiene precondiciones, orden GitOps,
  validación externa, métricas, logs, alertas, diagnóstico y rollback.
- El procedimiento evita acceso directo a pods y excluye explícitamente los
  contextos productivos indicados.
- El rollback nunca elimina configuraciones ni modifica el esquema.

Validaciones realizadas:

- 12 pruebas de contrato correctas.
- TypeScript `--noEmit` correcto en `web`, `recycoin`, `housecoin` y
  `recybot`.
- `ConfigurationService.Api` compila correctamente.
- El aviso conocido de AutoMapper se mantiene fuera de alcance por decisión
  expresa.
- No se conectó a una base de datos y no se ejecutaron migraciones.
- No se consultaron ni manipularon pods o contextos Kubernetes.

Pendiente operativo:

- Conectar el meter al collector OpenTelemetry elegido por la plataforma. Hasta
  entonces, los logs estructurados son la fuente central disponible.
- Completar `LogoUrl` y la configuración empresarial pendiente, especialmente
  la marca 5, antes del despliegue.
- Confirmar commits por cada repositorio y ejecutar la secuencia del runbook.

## Paso 7: entrega frontend completamente GitOps

Estado: GitOps y verificación confirmados en `e42081a`; pipelines frontend
confirmados en `57dd242`, `20d123a` y `028dec1`; pendiente de despliegue.

Centralización:

- El `app-of-apps` de `ecosystem-microservices` administra ahora las
  Applications de `ecosystem-web`, `recycoin-web` y `recybot-web`.
- El AppProject `ecosystem` se encuentra dentro del directorio administrado por
  el `app-of-apps`, con sync wave `-1`, y autoriza únicamente los cuatro
  repositorios conocidos.
- Cada frontend conserva promoción por tags inmutables mediante Argo CD Image
  Updater y sincronización automática.

Pipelines frontend:

- Se retiraron la obtención de kubeconfig, `kubectl apply`, anotaciones
  manuales, consultas de Deployment y `kubectl rollout status`.
- Los pipelines solo construyen y publican la imagen. El release describe una
  imagen publicada, no afirma que el despliegue ya fue verificado.
- Argo CD es el único responsable de reconciliar el estado del clúster.

Verificación externa:

- `Tools/Test-BrandingDeployment.ps1` comprueba HTTP 200, la cabecera
  `public-branding-v1`, el envelope, la lista exacta de campos públicos, campos
  obligatorios, coincidencia de `ClientUrl` y opcionalmente el `BrandId`.
- El workflow manual `Verify Production Branding` recibe hostnames y ejecuta
  esa validación únicamente a través de
  `https://api.ecosystemfx.net`.
- La prueba no usa credenciales de Kubernetes, conexiones de base de datos ni
  acceso a servicios internos.

HouseCoin:

- `housecoin` no contiene manifiestos ni pipeline GitOps y su remoto actual es
  `atorress91/house-coin`, fuera de `Ocx-Group`.
- No se inventó un dominio, nombre de imagen o Application. Su incorporación
  requiere confirmar primero repositorio canónico, hostname e imagen de
  producción.
- El branding dinámico de HouseCoin permanece implementado y compilable, pero
  su entrega no forma parte del `app-of-apps` hasta resolver esos datos.

Validaciones realizadas:

- YAML correcto en el AppProject, las tres Applications y los cuatro workflows
  modificados o añadidos.
- Referencias de la solución válidas después de mover el AppProject.
- Sintaxis PowerShell correcta para el smoke test.
- Búsqueda estática sin `kubectl`, kubeconfig, rollouts ni consultas de
  deployments en los tres pipelines frontend.
- No se ejecutó el smoke test contra producción porque todavía faltan los
  hostnames y `BrandId` empresariales confirmados.
- No se accedió a bases de datos, pods ni contextos Kubernetes.

Precondiciones de despliegue:

- La Application raíz `ecosystem-apps` debe existir y seguir
  `ecosystem-microservices/main`.
- Argo CD debe poder leer los tres repositorios frontend.
- Image Updater y `pullsecret:ecosystem/ocx-registry` deben estar operativos.
- Deben completarse los datos centrales de cada marca antes de publicar las
  imágenes frontend.

## Hallazgos transversales

- El backend de los pasos 2 y 3 está confirmado en `46369de`, el paso 4 en
  `477436e` y el endpoint del paso 5 en `5ed8af0`; ninguno está desplegado.
- Los cambios de las cuatro webs del paso 5 y la instrumentación del paso 6
  permanecen en sus respectivos árboles de trabajo.
- Los pasos técnicos 1 a 7 están confirmados localmente en sus respectivos
  repositorios; no se ha hecho push ni despliegue.
- El siguiente frente es resolver la incorporación GitOps de HouseCoin,
  completar la configuración empresarial y ejecutar el despliegue controlado
  con sus verificaciones públicas.

## Cómo continuar

1. Leer este documento antes de reauditar el repositorio completo.
2. Revisar el estado de Git para distinguir trabajo implementado de trabajo ya
   confirmado.
3. Ejecutar el paso pendiente más próximo.
4. Registrar aquí las decisiones, archivos modificados, validaciones y
   precondiciones de despliegue.
5. No marcar un paso como desplegado hasta verificar el estado real del clúster.
