import { Product } from '@/contexts/CartContext';
import productCar from '@/assets/product-car.jpg';
import productCharger from '@/assets/product-charger.jpg';
import productCap from '@/assets/product-cap.jpg';
import productTshirt from '@/assets/product-tshirt.jpg';
import productBottle from '@/assets/product-bottle.jpg';
import productJacket from '@/assets/product-jacket.jpg';

export const products: Product[] = [
  {
    id: '1',
    name: 'Model S',
    price: 89990,
    image: productCar,
    description: 'Vehículo eléctrico de alto rendimiento con autonomía extendida y aceleración instantánea.',
    category: 'Vehículos',
  },
  {
    id: '2',
    name: 'Cargador Inalámbrico',
    price: 150,
    image: productCharger,
    description: 'Cargador inalámbrico de alta velocidad con diseño minimalista y tecnología de última generación.',
    category: 'Accesorios',
  },
  {
    id: '3',
    name: 'Gorra Premium',
    price: 45,
    image: productCap,
    description: 'Gorra de diseño premium con logo bordado y ajuste perfecto. Material de alta calidad.',
    category: 'Ropa',
  },
  {
    id: '4',
    name: 'Camiseta Tech',
    price: 55,
    image: productTshirt,
    description: 'Camiseta de algodón premium con corte moderno y logo minimalista.',
    category: 'Ropa',
  },
  {
    id: '5',
    name: 'Botella de Agua',
    price: 35,
    image: productBottle,
    description: 'Botella térmica de acero inoxidable que mantiene las bebidas frías por 24 horas.',
    category: 'Accesorios',
  },
  {
    id: '6',
    name: 'Chaqueta Premium',
    price: 250,
    image: productJacket,
    description: 'Chaqueta de cuero premium con diseño elegante y acabados de lujo.',
    category: 'Ropa',
  },
];
