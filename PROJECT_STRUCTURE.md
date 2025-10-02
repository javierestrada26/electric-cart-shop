# 🏗️ Estructura del Proyecto

## Estructura de Carpetas (Frontend)

```
src/
├── components/           # Componentes organizados por dominio
│   ├── layout/           # Navbar, Footer, etc.
│   ├── product/          # ProductCard, etc.
│   ├── cart/             # CartSidebar, etc.
│   ├── common/           # Componentes reutilizables
│   └── ui/               # Componentes UI genéricos
├── hooks/                # Hooks personalizados
│   └── custom/           # Hooks de negocio
├── stores/               # Estado global (Zustand)
├── types/                # Tipos TypeScript
├── constants/            # Constantes del proyecto
├── utils/                # Utilidades
├── data/                 # Datos estáticos
├── pages/                # Páginas principales
└── assets/               # Imágenes y recursos
```

## Estructura de Carpetas (Backend)

```
backend/ElectricCartShop.API/
├── Controllers/          # Endpoints API REST
├── Services/             # Lógica de negocio
├── Repositories/         # Acceso a datos
├── Interfaces/           # Contratos y abstracciones
├── DTOs/                 # Data Transfer Objects
├── Models/               # Entidades del dominio
└── Program.cs            # Configuración principal
```

## Resumen
- Separación clara de frontend y backend.
- Código modular, escalable y fácil de mantener.
- Estado global sincronizado con backend.
- Componentes y hooks reutilizables.
- Documentación y endpoints claros para pruebas.



