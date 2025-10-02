import { useState, useCallback, useEffect } from 'react';
import { apiService } from '@/services/api';
import { Product } from '@/types';
import { useCartStore } from '@/stores/cartStore';

const useCart = () => {
  const cartStore = useCartStore();
  const [sessionId] = useState(() => {
    let sessionId = localStorage.getItem('cart-session-id');
    if (!sessionId) {
      sessionId = `session-${Date.now()}-${Math.random().toString(36).substr(2, 9)}`;
      localStorage.setItem('cart-session-id', sessionId);
    }
    return sessionId;
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // Sincroniza el carrito solo desde el backend y actualiza Zustand
  const syncCartWithBackend = useCallback(async () => {
    try {
      const backendCart = await apiService.getCart(sessionId);
      const syncedItems = backendCart.items.map(backendItem => ({
        id: backendItem.product.id.toString(),
        name: backendItem.product.name,
        price: backendItem.product.price,
        image: backendItem.product.image,
        description: backendItem.product.description,
        category: backendItem.product.category,
        sizes: backendItem.product.sizes,
        quantity: backendItem.quantity,
        size: backendItem.size,
        cartItemId: backendItem.id
      }));
      cartStore.items = syncedItems;
    } catch (err) {
      cartStore.items = [];
      console.warn('Failed to sync cart with backend:', err);
    }
  }, [sessionId, cartStore]);

  useEffect(() => {
    syncCartWithBackend();
  }, [syncCartWithBackend]);

  const addToCart = useCallback(async (product: Product, size?: string) => {
    setLoading(true);
    setError(null);
    try {
      await apiService.addToCart(parseInt(product.id), 1, size, sessionId);
      await syncCartWithBackend();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Error adding to cart');
    } finally {
      setLoading(false);
    }
  }, [sessionId, syncCartWithBackend]);

  const removeFromCart = useCallback(async (cartItemId: number) => {
    setLoading(true);
    setError(null);
    try {
      await apiService.removeFromCart(cartItemId);
      await syncCartWithBackend();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Error removing from cart');
    } finally {
      setLoading(false);
    }
  }, [syncCartWithBackend]);

  const updateQuantity = useCallback(async (cartItemId: number, quantity: number) => {
    if (quantity <= 0) {
      return removeFromCart(cartItemId);
    }
    setLoading(true);
    setError(null);
    try {
      await apiService.updateCartItem(cartItemId, quantity);
      await syncCartWithBackend();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Error updating quantity');
    } finally {
      setLoading(false);
    }
  }, [removeFromCart, syncCartWithBackend]);

  const clearCart = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      await apiService.clearCart(sessionId);
      await syncCartWithBackend();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Error clearing cart');
    } finally {
      setLoading(false);
    }
  }, [sessionId, syncCartWithBackend]);


  return {
    items: cartStore.items,
    addToCart,
    removeFromCart,
    updateQuantity,
    clearCart,
    syncCartWithBackend,
    getTotalItems: cartStore.getTotalItems,
    getTotalPrice: cartStore.getTotalPrice,
    isCartEmpty: cartStore.items.length === 0,
    cartItemCount: cartStore.getTotalItems(),
    cartTotal: cartStore.getTotalPrice(),
    loading,
    error,
  };
};

export default useCart;
