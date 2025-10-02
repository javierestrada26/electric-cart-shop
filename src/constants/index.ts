export const APP_CONFIG = {
  name: 'TECH SHOP',
  description: 'Descubre nuestra colección de productos innovadores con diseño minimalista',
} as const;

export const CATEGORIES = {
  ACCESSORIES: 'Accesorios',
  CLOTHING: 'Ropa',
} as const;

export const PRODUCT_CATEGORIES = [
  { key: 'Accesorios', label: 'Accesorios' },
  { key: 'Ropa', label: 'Ropa' },
] as const;

export const SHIPPING_INFO = {
  free: 'Gratis',
  delivery: '2-4 días',
  warranty: '2 años',
} as const;

export const CART_MESSAGES = {
  added: 'Agregado al carrito',
  updated: 'Cantidad actualizada',
  removed: 'Producto eliminado',
  cleared: 'Carrito vaciado',
  selectSize: 'Selecciona una talla',
  selectSizeDescription: 'Por favor selecciona una talla antes de agregar al carrito',
} as const;



