export interface Product {
  id: string;
  name: string;
  price: number;
  image: string;
  description: string;
  category: string;
  sizes?: string[];
}

export interface CartItem extends Product {
  quantity: number;
  size?: string;
  cartItemId?: number; // ID del item en el carrito del backend
}

export interface CartState {
  items: CartItem[];
  addToCart: (product: Product, size?: string) => void;
  removeFromCart: (productId: string, size?: string) => void;
  updateQuantity: (productId: string, quantity: number, size?: string) => void;
  clearCart: () => void;
  getTotalItems: () => number;
  getTotalPrice: () => number;
}

export interface ProductCardProps {
  product: Product;
}

export interface CartSidebarProps {
  isOpen: boolean;
  onClose: () => void;
}

export interface NavbarProps {
  onCartClick: () => void;
}

export interface HeroSectionProps {
  title: string;
  description: string;
}

export interface ProductGridProps {
  products: Product[];
}



