# Electric Cart Shop API - Endpoints Documentation

## Base URL
```
http://localhost:5000/api
```

## Endpoints Disponibles

### 1. Productos

#### Obtener todos los productos
```http
GET /api/products
```

#### Obtener producto por ID
```http
GET /api/products/{id}
```

#### Obtener productos por categoría
```http
GET /api/products/category/{category}
```

### 2. Carrito

#### Obtener carrito por sessionId
```http
GET /api/cart/{sessionId}
```

#### Agregar producto al carrito
```http
POST /api/cart/add
Content-Type: application/json

{
  "productId": 1,
  "quantity": 2,
  "size": "M",
  "sessionId": "session-123"
}
```

#### Actualizar cantidad de item del carrito
```http
PUT /api/cart/{cartItemId}
Content-Type: application/json

{
  "quantity": 3
}
```

#### Eliminar item del carrito
```http
DELETE /api/cart/{cartItemId}
```

#### Limpiar todo el carrito
```http
DELETE /api/cart/clear/{sessionId}
```

#### Checkout (Finalizar compra)
```http
POST /api/cart/checkout
Content-Type: application/json

{
  "sessionId": "session-123",
  "customerName": "Juan Pérez",
  "customerEmail": "juan@email.com",
  "shippingAddress": "Calle 123, Ciudad",
  "paymentMethod": "Tarjeta de Crédito"
}
```

### 3. Logs de Compra

#### Obtener todos los logs de compra
```http
GET /api/purchaselogs
```

#### Obtener log por ID
```http
GET /api/purchaselogs/{id}
```

#### Obtener logs por número de orden
```http
GET /api/purchaselogs/order/{orderNumber}
```

## Casos de Uso Principales

### 1. Agregar artículo al carrito
1. **POST /api/cart/add** con productId, quantity y sessionId
2. Si el artículo ya existe con la misma talla, suma las cantidades
3. Si no existe, crea un nuevo item en el carrito

### 2. Aumentar cantidad desde el carrito
1. **PUT /api/cart/{cartItemId}** con nueva quantity
2. El precio total se actualiza automáticamente

### 3. Agregar el mismo artículo varias veces
1. Múltiples **POST /api/cart/add** con el mismo productId
2. Se acumula en un solo item con cantidad total
3. El precio refleja quantity × precio unitario

### 4. Agregar diferentes tipos de elementos
1. **POST /api/cart/add** con diferentes productId
2. Cada elemento aparece como item separado
3. Precio total = suma de todos los items

### 5. Eliminar algunos elementos
1. **DELETE /api/cart/{cartItemId}** para items específicos
2. **GET /api/cart/{sessionId}** para ver carrito actualizado
3. Precio total se recalcula automáticamente

### 6. Eliminar todos los artículos
1. **DELETE /api/cart/clear/{sessionId}**
2. Carrito queda vacío (saldo = 0)

### 7. Ver detalles del producto
1. **GET /api/products/{id}** para información completa
2. Incluye nombre, precio, imagen, descripción, categoría, tallas

### 8. Finalizar compra
1. **POST /api/cart/checkout** con datos del cliente
2. Se crea un PurchaseLog en la base de datos JSON
3. Se limpia el carrito automáticamente
4. Se genera número de orden único

## Estructura de la Base de Datos JSON

El archivo `Data/database.json` contiene:
- `products`: Array de productos disponibles
- `cartItems`: Array de items en carritos activos
- `orders`: Array de órdenes (reservado para futuro uso)
- `purchaseLogs`: Array de compras finalizadas
- `counters`: Contadores de IDs para cada entidad

## Validaciones

- **Productos**: Deben existir antes de agregarse al carrito
- **Carrito**: SessionId es requerido para todas las operaciones
- **Checkout**: CustomerName y CustomerEmail son obligatorios
- **Cantidades**: Deben ser números enteros positivos

## Persistencia

Todos los datos se guardan automáticamente en `Data/database.json` usando un sistema de sincronización con SemaphoreSlim para evitar conflictos de acceso concurrente.
