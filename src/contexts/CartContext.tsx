import React, { createContext, useContext, useState, useEffect } from 'react';
import { toast } from '@/hooks/use-toast';

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
}

interface CartContextType {
  items: CartItem[];
  addToCart: (product: Product, size?: string) => void;
  removeFromCart: (productId: string, size?: string) => void;
  updateQuantity: (productId: string, quantity: number, size?: string) => void;
  clearCart: () => void;
  getTotalItems: () => number;
  getTotalPrice: () => number;
}

const CartContext = createContext<CartContextType | undefined>(undefined);

export const CartProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [items, setItems] = useState<CartItem[]>(() => {
    const savedCart = localStorage.getItem('cart');
    return savedCart ? JSON.parse(savedCart) : [];
  });

  useEffect(() => {
    localStorage.setItem('cart', JSON.stringify(items));
  }, [items]);

  const addToCart = (product: Product, size?: string) => {
    setItems((prevItems) => {
      const existingItem = prevItems.find(
        (item) => item.id === product.id && item.size === size
      );
      
      if (existingItem) {
        toast({
          title: "Cantidad actualizada",
          description: `${product.name}${size ? ` - Talla ${size}` : ''} - Cantidad: ${existingItem.quantity + 1}`,
        });
        return prevItems.map((item) =>
          item.id === product.id && item.size === size
            ? { ...item, quantity: item.quantity + 1 }
            : item
        );
      }
      
      toast({
        title: "Agregado al carrito",
        description: `${product.name}${size ? ` - Talla ${size}` : ''} ha sido agregado`,
      });
      return [...prevItems, { ...product, quantity: 1, size }];
    });
  };

  const removeFromCart = (productId: string, size?: string) => {
    setItems((prevItems) => {
      const item = prevItems.find(
        (item) => item.id === productId && item.size === size
      );
      if (item) {
        toast({
          title: "Producto eliminado",
          description: `${item.name}${size ? ` - Talla ${size}` : ''} ha sido eliminado del carrito`,
        });
      }
      return prevItems.filter(
        (item) => !(item.id === productId && item.size === size)
      );
    });
  };

  const updateQuantity = (productId: string, quantity: number, size?: string) => {
    if (quantity <= 0) {
      removeFromCart(productId, size);
      return;
    }
    
    setItems((prevItems) =>
      prevItems.map((item) =>
        item.id === productId && item.size === size ? { ...item, quantity } : item
      )
    );
  };

  const clearCart = () => {
    toast({
      title: "Carrito vaciado",
      description: "Todos los productos han sido eliminados",
    });
    setItems([]);
  };

  const getTotalItems = () => {
    return items.reduce((total, item) => total + item.quantity, 0);
  };

  const getTotalPrice = () => {
    return items.reduce((total, item) => total + item.price * item.quantity, 0);
  };

  return (
    <CartContext.Provider
      value={{
        items,
        addToCart,
        removeFromCart,
        updateQuantity,
        clearCart,
        getTotalItems,
        getTotalPrice,
      }}
    >
      {children}
    </CartContext.Provider>
  );
};

export const useCart = () => {
  const context = useContext(CartContext);
  if (context === undefined) {
    throw new Error('useCart must be used within a CartProvider');
  }
  return context;
};
