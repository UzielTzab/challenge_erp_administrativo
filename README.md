# Challenge ERP Administrativo

API para registrar recepciones contra una orden de compra. Incluye backend .NET, pruebas automatizadas y un frontend en React.

## Requisitos

- .NET SDK 8 o superior.
- Node.js y npm.
- `dotnet-ef` instalado globalmente:

```powershell
dotnet tool install --global dotnet-ef
```

## Entregables incluidos

- Código completo de la API en `ChallengeErp.Api/`.
- Pruebas automatizadas en `ChallengeErp.Tests/`.
- Frontend en `ChallengeERP.Frontend/`.
- Migraciones, configuración y solución .NET incluidos en el repositorio.

## Estructura

```text
ChallengeErp.Api/          # API, SQLite y migraciones
ChallengeErp.Tests/        # Pruebas xUnit
ChallengeERP.Frontend/     # React + TypeScript
ChallengeErp.slnx          # Solucion .NET
```

## Ejecutar el proyecto

Ejecuta los siguientes comandos desde la raiz del repositorio.

### 1. Restaurar dependencias

```powershell
dotnet restore .\ChallengeErp.slnx
cd .\ChallengeERP.Frontend
npm install
cd ..
```

### 2. Crear la base SQLite

```powershell
cd .\ChallengeErp.Api
dotnet ef database update --context AppDbContext
```

Esto crea `app.db`, aplica las migraciones e inserta la orden de ejemplo `OC-1001`.

Para reiniciar los datos sin borrar migraciones:

```powershell
Remove-Item .\app.db -Force
dotnet ef database update --context AppDbContext
```

### 3. Levantar la API

Desde `ChallengeErp.Api`:

```powershell
dotnet run --launch-profile http
```

Puerto de la API:

```text
http://localhost:5013
```

Visualizar los endpoints en Swagger:

```text
http://localhost:5013/swagger
```

### 4. Levantar el frontend

En otra terminal, desde la raiz:

```powershell
cd .\ChallengeERP.Frontend
npm run dev
```

Vite mostrara la URL, normalmente:

```text
http://localhost:5173
```

El frontend usa el proxy de Vite para comunicarse con la API en `http://localhost:5013`.

## Probar la API

### 1. Consultar una orden

```http
GET http://localhost:5013/ordenes/OC-1001
```

PowerShell:

```powershell
Invoke-RestMethod `
	-Method Get `
	-Uri http://localhost:5013/ordenes/OC-1001
```

Respuesta `200 OK` (resumida; incluye todas las líneas de la orden):

```json
{
	"id": "OC-1001",
	"provider": "Vidrios del Sureste",
	"status": "Open",
	"lines": [
		{
			"id": 1,
			"article": "Vidrio flotado 6 mm",
			"unitOfMeasure": "m2",
			"orderedQuantity": 100,
			"receivedQuantity": 0,
			"pendingQuantity": 100,
			"maximumAcceptable": 102
		}
	]
}
```

### 2. Registrar una recepción

URL:

```http
POST http://localhost:5013/ordenes/OC-1001/recepciones
Content-Type: application/json
```

JSON de entrada:

```json
{
	"lines": [
		{
			"lineId": 1,
			"quantity": 20
		}
	]
}
```

Escribe este comando en PowerShell para realizar una Recepción; petición POST:

```powershell
$body = @{
	lines = @(
		@{ lineId = 1; quantity = 20 }
	)
} | ConvertTo-Json -Depth 3

Invoke-RestMethod `
	-Method Post `
	-Uri http://localhost:5013/ordenes/OC-1001/recepciones `
	-ContentType 'application/json' `
	-Body $body
```

Respuesta `200 OK` (resumida; incluye todas las líneas de la orden):

```json
{
	"id": "OC-1001",
	"provider": "Vidrios del Sureste",
	"status": "Open",
	"lines": [
		{
			"id": 1,
			"article": "Vidrio flotado 6 mm",
			"unitOfMeasure": "m2",
			"orderedQuantity": 100,
			"receivedQuantity": 20,
			"pendingQuantity": 80,
			"maximumAcceptable": 102
		}
	]
}
```

Errores principales del endpoint:

- `400`: cantidad inválida o línea que no pertenece a la orden.
- `404`: la orden no existe.
- `409`: la orden ya está cerrada.
- `422`: la recepción supera la tolerancia del 2%; se rechaza toda la operación.

## Tabla e información inicial en la base de datos

```text
OC-1001 | Vidrios del Sureste | Open
Linea 1: Vidrio flotado 6 mm | 100 m2 | 180.00
Linea 2: Silicon estructural   | 40 pza | 95.00
Linea 3: Perfil de aluminio 3 m| 25 pza | 310.00
```

## Ejecutar pruebas

Desde la raiz del repositorio ejecuta:

```powershell
dotnet test .\ChallengeErp.Tests\ChallengeErp.Tests.csproj
```

Las pruebas cubren recepcion parcial, exceso de tolerancia y cierre automatico. Usan una base InMemory, por lo que no modifican la base de datos principal.

Resultado esperado:

```text
Test summary: total: 3, failed: 0, succeeded: 3, skipped: 0
Build succeeded
```

Las pruebas ejecutadas son:

- `RegisterReception_RecepcionParcialValida_ReturnsOkAndUpdatesPending`: verifica que una recepción parcial actualice lo recibido y lo pendiente.
- `RegisterReception_ExcedeTolerancia_Returns422`: verifica que se rechace una recepción que supera el 2% de tolerancia.
- `RegisterReception_CierreAutomatico_CompletaTodasLasLineas`: verifica que la orden cambie a `Close` cuando todas sus líneas se completan.

Los tests no dependen de mensajes escritos en consola; xUnit determina el resultado mediante aserciones. Si alguna aserción falla, el comando muestra el nombre de la prueba, el valor esperado, el valor recibido y la línea donde ocurrió el fallo.

## Decisiones Técnicas

- **API:** Se utilizó Controllers para mantener un enrutamiento HTTP claro y estructurado, facilitando la lectura y futura escalabilidad del código.
- **Persistencia:** Se integró SQLite con Entity Framework Core. El uso de migraciones garantiza que la base de datos inicial, con la orden de compra requerida, sea 100% reproducible sin necesidad de levantar infraestructura adicional.
- **Pruebas:** Se configuró EF Core InMemory para el proyecto de testing, asegurando que las pruebas de las reglas de negocio estén completamente aisladas, sean rápidas y no dependan de la base de datos física.
- **Frontend:** Se implementó una arquitectura modular que separa claramente la capa de red, los tipos, el tema y los componentes visuales.

### Qué mejoraría con más tiempo (Limitaciones)

- **Servicio de Dominio:** Extraería las reglas a un servicio de dominio para mejorar la separación de responsabilidades, facilitar las pruebas unitarias y mantener el controlador enfocado únicamente en la capa HTTP.

	La responsabilidad de cada parte quedaría distribuida así:

	```text
	OrdersController
	├── Recibe la petición
	├── Llama al servicio de dominio
	└── Convierte el resultado en una respuesta HTTP

	ReceptionDomainService
	├── Valida cantidades
	├── Valida líneas
	├── Calcula la tolerancia
	├── Aplica todas las recepciones
	└── Cierra la orden si corresponde
	```

- **Cobertura de Casos:** Agregaría pruebas para otros códigos de error y casos de redondeo vital para garantizar la robustez del sistema ante entradas inesperadas o límites técnicos.
- **Escalabilidad:** Agregaría un endpoint `GET /ordenes` con paginación para soportar la gestión de múltiples órdenes de compra, también agregaría un endpoint que devuelva el historial de recepciones registradas tanto para la orden como por línea.
