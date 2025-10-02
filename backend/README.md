# 🚀 Electric Cart Shop - Backend API

## 📋 Descripción
API REST desarrollada en .NET 8 para el sistema de e-commerce Electric Cart Shop. Implementa arquitectura limpia con inyección de dependencias, logging de compras y diseño orientado a interfaces.

## 🏗️ Arquitectura

### Estructura del Proyecto
```
ElectricCartShop.API/
├── Controllers/          # Controladores API REST
├── Services/            # Lógica de negocio
├── Repositories/        # Acceso a datos
├── Interfaces/          # Contratos y abstracciones
├── DTOs/               # Objetos de transferencia de datos
├── Models/             # Entidades del dominio
└── Program.cs          # Configuración y DI
```

### Patrones Implementados
- **Repository Pattern**: Abstracción de acceso a datos
- **Service Layer**: Lógica de negocio encapsulada
- **DTO Pattern**: Transferencia de datos optimizada
- **Dependency Injection**: Inyección de dependencias nativa
- **Interface Segregation**: Principio SOLID aplicado

## 🛠️ Tecnologías

- **.NET 8**: Framework principal
- **ASP.NET Core Web API**: API REST
- **Swagger/OpenAPI**: Documentación automática
- **CORS**: Configuración para frontend
- **Logging**: Sistema de logs integrado

## 📡 Endpoints API

### Products
- `GET /api/products` - Obtener todos los productos
- `GET /api/products/{id}` - Obtener producto por ID
- `GET /api/products/category/{category}` - Productos por categoría
- `POST /api/products` - Crear producto
- `PUT /api/products/{id}` - Actualizar producto
- `DELETE /api/products/{id}` - Eliminar producto

### Cart
- `GET /api/cart/{sessionId}` - Obtener carrito
- `POST /api/cart/add` - Agregar al carrito
- `PUT /api/cart/{id}` - Actualizar item del carrito
- `DELETE /api/cart/{id}` - Eliminar del carrito
- `DELETE /api/cart/clear/{sessionId}` - Vaciar carrito

### Orders
- `GET /api/orders` - Obtener todas las órdenes
- `GET /api/orders/{id}` - Obtener orden por ID
- `GET /api/orders/order-number/{orderNumber}` - Orden por número
- `POST /api/orders` - Crear orden

### Purchase Logs
- `GET /api/purchaselogs` - Obtener logs de compras
- `GET /api/purchaselogs/{id}` - Log por ID
- `GET /api/purchaselogs/order/{orderNumber}` - Logs por orden

## 🔧 Configuración

### Requisitos
- .NET 8 SDK
- Visual Studio 2022 o VS Code
- Postman (para testing)

### Instalación
```bash
# Clonar el repositorio
git clone <repository-url>

# Navegar al directorio del backend
cd backend/ElectricCartShop.API

# Restaurar dependencias
dotnet restore

# Ejecutar la aplicación
dotnet run
```

### Configuración CORS
El backend está configurado para aceptar requests desde:
- `http://localhost:3000` (React)
- `http://localhost:5173` (Vite)
- `http://localhost:8081` (Vite alternativo)

## 📊 Características

### ✅ Funcionalidades Implementadas
- **CRUD completo** para productos
- **Gestión de carrito** con sesiones
- **Procesamiento de órdenes** con validación
- **Logging automático** de compras exitosas
- **Inyección de dependencias** configurada
- **CORS habilitado** para frontend
- **Swagger UI** para documentación

### 🔒 Seguridad
- Validación de datos de entrada
- Manejo de errores centralizado
- Logging de operaciones críticas

### 📈 Escalabilidad
- Arquitectura modular
- Interfaces bien definidas
- Fácil extensión de funcionalidades

## 🚀 Uso

### Ejecutar en Desarrollo
```bash
dotnet run --environment Development
```

### Acceder a Swagger
```
https://localhost:5001/swagger
```

### Ejemplo de Request
```json
POST /api/cart/add
{
  "productId": 1,
  "quantity": 2,
  "size": "M",
  "sessionId": "session-123"
}
```

## 📝 Logging de Compras

Cada compra exitosa se registra automáticamente con:
- Número de orden
- Información del cliente
- Monto total
- Fecha y hora
- Datos adicionales

## 🔄 Integración con Frontend

El backend está diseñado para integrarse perfectamente con el frontend React/Vite, proporcionando:
- Endpoints RESTful optimizados
- CORS configurado
- Respuestas JSON consistentes
- Manejo de errores estandarizado

## 📚 Documentación

- Swagger UI disponible en `/swagger`
- Código documentado con comentarios XML
- Interfaces bien definidas para extensibilidad

## 🧪 Ejemplos de uso con Postman

### 1. Agregar producto al carrito
- **POST** `http://localhost:5000/api/cart/add`
- **Body (JSON):**
```json
{
  "productId": 1,
  "quantity": 2,
  "size": "M",
  "sessionId": "session-123"
}
```
- **Headers:** `Content-Type: application/json`

### 2. Obtener el carrito
- **GET** `http://localhost:5000/api/cart/session-123`

### 3. Crear una orden
- **POST** `http://localhost:5000/api/orders`
- **Body (JSON):**
```json
{
  "customerEmail": "cliente@ejemplo.com",
  "customerName": "Cliente Ejemplo",
  "orderItems": [
    {
      "product": { "id": 1, "name": "Producto 1", "price": 10000, "image": "url", "description": "desc", "category": "cat" },
      "quantity": 2,
      "size": "M"
    }
  ]
}
```
- **Headers:** `Content-Type: application/json`

### 4. Ver logs de compras
- **GET** `http://localhost:5000/api/purchaselogs`


