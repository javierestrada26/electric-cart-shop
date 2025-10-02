# 🛒 Electric Cart Shop - Proyecto Final

## Resumen
Proyecto e-commerce completo con frontend en React + TypeScript + Zustand y backend en .NET 8 API REST.

- Frontend moderno, responsive y modular
- Backend robusto, seguro y documentado
- Estado global sincronizado con backend
- Componentes y hooks reutilizables
- Endpoints claros y fáciles de probar

## Estructura General

```
electric-cart-shop/
├── src/ (Frontend)
│   ├── components/
│   ├── hooks/
│   ├── stores/
│   ├── types/
│   ├── constants/
│   ├── utils/
│   ├── data/
│   ├── pages/
│   └── assets/
└── backend/
    └── ElectricCartShop.API/
        ├── Controllers/
        ├── Services/
        ├── Repositories/
        ├── Interfaces/
        ├── DTOs/
        ├── Models/
        └── Program.cs
```

## ¿Cómo ejecutar?

### Frontend
```bash
npm install
npm run dev
# http://localhost:5173
```

### Backend
```bash
cd backend/ElectricCartShop.API
dotnet restore
dotnet run
# https://localhost:5001/swagger
```

## Funcionalidades principales
- Catálogo de productos y detalles
- Carrito de compras sincronizado
- Procesamiento de órdenes y logging
- API RESTful con Swagger
- UI moderna y adaptable

## Estado actual
- 100% funcional y listo para producción
- Código limpio, modular y documentado


