# 🚀 Guía para Probar la API en Postman

## 1. Ejecutar el Servidor

Primero ejecuta el servidor API:
```bash
cd backend\ElectricCartShop.API
dotnet run
```

El servidor iniciará en: `http://localhost:5000` o `https://localhost:5001`

## 2. Configuración Base en Postman

- **Base URL**: `http://localhost:5000/api`
- **Headers**: `Content-Type: application/json` (para requests POST/PUT)

---

## 🛍️ **PRUEBAS PASO A PASO**

### **PASO 1: Ver Productos Disponibles**

**Request:**
```
GET http://localhost:5000/api/products
```

**Respuesta esperada:**
```json
[
  {
    "id": 1,
    "name": "Cargador Inalámbrico",
    "price": 150.0,
    "image": "/src/assets/product-charger.jpg",
    "description": "Cargador inalámbrico de alta velocidad...",
    "category": "Accesorios",
    "sizes": null
  },
  // ... más productos
]
```

### **PASO 2: Ver Producto Específico**

**Request:**
```
GET http://localhost:5000/api/products/1
```

### **PASO 3: Ver Carrito Vacío**

**Request:**
```
GET http://localhost:5000/api/cart/session-123
```

**Respuesta esperada:**
```json
{
  "items": [],
  "totalAmount": 0,
  "totalItems": 0
}
```

### **PASO 4: Agregar Primer Producto al Carrito**

**Request:**
```
POST http://localhost:5000/api/cart/add
Content-Type: application/json

{
  "productId": 1,
  "quantity": 2,
  "sessionId": "session-123"
}
```

**Respuesta esperada:**
```json
{
  "id": 1,
  "product": {
    "id": 1,
    "name": "Cargador Inalámbrico",
    "price": 150.0,
    "image": "/src/assets/product-charger.jpg",
    "description": "...",
    "category": "Accesorios",
    "sizes": null
  },
  "quantity": 2,
  "size": null
}
```

### **PASO 5: Agregar Producto con Talla**

**Request:**
```
POST http://localhost:5000/api/cart/add
Content-Type: application/json

{
  "productId": 2,
  "quantity": 1,
  "size": "M",
  "sessionId": "session-123"
}
```

### **PASO 6: Agregar Mismo Producto (debe sumar cantidad)**

**Request:**
```
POST http://localhost:5000/api/cart/add
Content-Type: application/json

{
  "productId": 1,
  "quantity": 1,
  "sessionId": "session-123"
}
```

### **PASO 7: Ver Carrito Actualizado**

**Request:**
```
GET http://localhost:5000/api/cart/session-123
```

**Respuesta esperada:**
```json
{
  "items": [
    {
      "id": 1,
      "product": {
        "id": 1,
        "name": "Cargador Inalámbrico",
        "price": 150.0,
        // ...
      },
      "quantity": 3,
      "size": null
    },
    {
      "id": 2,
      "product": {
        "id": 2,
        "name": "Gorra Premium",
        "price": 45.0,
        // ...
      },
      "quantity": 1,
      "size": "M"
    }
  ],
  "totalAmount": 495.0,
  "totalItems": 4
}
```

### **PASO 8: Actualizar Cantidad de un Item**

**Request:**
```
PUT http://localhost:5000/api/cart/1
Content-Type: application/json

{
  "quantity": 5
}
```

### **PASO 9: Agregar Más Productos Diferentes**

**Request:**
```
POST http://localhost:5000/api/cart/add
Content-Type: application/json

{
  "productId": 3,
  "quantity": 2,
  "size": "L",
  "sessionId": "session-123"
}
```

**Request:**
```
POST http://localhost:5000/api/cart/add
Content-Type: application/json

{
  "productId": 4,
  "quantity": 1,
  "sessionId": "session-123"
}
```

### **PASO 10: Ver Carrito con Múltiples Items**

**Request:**
```
GET http://localhost:5000/api/cart/session-123
```

### **PASO 11: Eliminar un Item Específico**

**Request:**
```
DELETE http://localhost:5000/api/cart/2
```

### **PASO 12: Finalizar Compra (Checkout)**

**Request:**
```
POST http://localhost:5000/api/cart/checkout
Content-Type: application/json

{
  "sessionId": "session-123",
  "customerName": "Juan Pérez",
  "customerEmail": "juan@email.com",
  "shippingAddress": "Calle 123, Ciudad",
  "paymentMethod": "Tarjeta de Crédito"
}
```

**Respuesta esperada:**
```json
{
  "orderNumber": "ORD-20241002-A1B2C3D4",
  "totalAmount": 645.0,
  "status": "Completed",
  "purchaseDate": "2024-10-02T...",
  "items": [...]
}
```

### **PASO 13: Verificar Carrito Vacío Después del Checkout**

**Request:**
```
GET http://localhost:5000/api/cart/session-123
```

**Respuesta esperada:**
```json
{
  "items": [],
  "totalAmount": 0,
  "totalItems": 0
}
```

### **PASO 14: Ver Purchase Logs**

**Request:**
```
GET http://localhost:5000/api/purchaselogs
```

### **PASO 15: Limpiar Carrito Manualmente (opcional)**

Si quieres limpiar un carrito sin hacer checkout:

**Request:**
```
DELETE http://localhost:5000/api/cart/clear/session-123
```

---

## 📋 **COLECCIÓN POSTMAN**

### Variables de Entorno
Crea estas variables en Postman:
- `base_url`: `http://localhost:5000/api`
- `session_id`: `session-123`

### Requests Organizados
1. **Products** 📦
   - GET Products List
   - GET Product by ID
   - GET Products by Category

2. **Cart Management** 🛒
   - GET Cart
   - POST Add to Cart
   - PUT Update Cart Item
   - DELETE Remove Cart Item
   - DELETE Clear Cart

3. **Checkout** 💳
   - POST Checkout

4. **Purchase Logs** 📋
   - GET All Purchase Logs
   - GET Purchase Log by ID

---

## ⚠️ **Puntos Importantes**

1. **SessionId**: Usa el mismo `sessionId` para mantener items en el mismo carrito
2. **ProductId**: Usa IDs que existen (1-6 según los productos iniciales)
3. **Sizes**: Solo productos de ropa tienen tallas (productos 2, 3, 5)
4. **Validaciones**: El API valida que los productos existan antes de agregarlos

## 🐛 **Casos de Error para Probar**

1. **Producto inexistente:**
```json
{
  "productId": 999,
  "quantity": 1,
  "sessionId": "session-123"
}
```

2. **Checkout con carrito vacío:**
```json
{
  "sessionId": "empty-session",
  "customerName": "Test",
  "customerEmail": "test@email.com"
}
```

3. **Checkout sin datos requeridos:**
```json
{
  "sessionId": "session-123"
}
```

¡Ahora puedes probar todos los escenarios completos del carrito de compras! 🎉
