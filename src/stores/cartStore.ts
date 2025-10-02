import { create } from 'zustand';
import { persist } from 'zustand/middleware';
import { toast } from '@/hooks/use-toast';
import { Product, CartItem, CartState } from '@/types';

export const useCartStore = create<CartState>()(
  persist(
    (set, get) => ({
      items: [],

      addToCart: (product: Product, size?: string) => {
        const { items } = get();
        const existingItem = items.find(
          (item) => item.id === product.id && item.size === size
        );
        
        if (existingItem) {
          toast({
            title: "Cantidad actualizada",
            description: `${product.name}${size ? ` - Talla ${size}` : ''} - Cantidad: ${existingItem.quantity + 1}`,
          });
          set({
            items: items.map((item) =>
              item.id === product.id && item.size === size
                ? { ...item, quantity: item.quantity + 1 }
                : item
            ),
          });
        } else {
          toast({
            title: "Agregado al carrito",
            description: `${product.name}${size ? ` - Talla ${size}` : ''} ha sido agregado`,
          });
          // Incluir cartItemId si está disponible en el producto
          const cartItem: CartItem = { 
            ...product, 
            quantity: 1, 
            size,
            cartItemId: (product as any).cartItemId
          };
          set({
            items: [...items, cartItem],
          });
        }
      },

      removeFromCart: (productId: string, size?: string) => {
        const { items } = get();
        const item = items.find(
          (item) => item.id === productId && item.size === size
        );
        
        if (item) {
          toast({
            title: "Producto eliminado",
            description: `${item.name}${size ? ` - Talla ${size}` : ''} ha sido eliminado del carrito`,
          });
        }
        
        set({
          items: items.filter(
            (item) => !(item.id === productId && item.size === size)
          ),
        });
      },

      updateQuantity: (productId: string, quantity: number, size?: string) => {
        if (quantity <= 0) {
          get().removeFromCart(productId, size);
          return;
        }
        
        const { items } = get();
        set({
          items: items.map((item) =>
            item.id === productId && item.size === size ? { ...item, quantity } : item
          ),
        });
      },

      clearCart: () => {
        toast({
          title: "Carrito vaciado",
          description: "Todos los productos han sido eliminados",
        });
        set({ items: [] });
      },

      getTotalItems: () => {
        const { items } = get();
        return items.reduce((total, item) => total + item.quantity, 0);
      },

      getTotalPrice: () => {
        const { items } = get();
        return items.reduce((total, item) => total + item.price * item.quantity, 0);
      },
    }),
    {
      name: 'cart-storage',
      version: 1,
    }
  )
);
