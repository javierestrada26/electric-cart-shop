import { useState, useEffect } from 'react';
import { apiService, ApiProduct, ApiCart, ApiOrder, CreateOrderRequest, OrderResponse } from '@/services/api';

export const useApi = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleRequest = async <T>(request: () => Promise<T>): Promise<T | null> => {
    setLoading(true);
    setError(null);
    
    try {
      const result = await request();
      return result;
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'An error occurred';
      setError(errorMessage);
      console.error('API Error:', err);
      return null;
    } finally {
      setLoading(false);
    }
  };

  return {
    loading,
    error,
    handleRequest,
    // Products
    getProducts: () => handleRequest(() => apiService.getProducts()),
    getProduct: (id: number) => handleRequest(() => apiService.getProduct(id)),
    getProductsByCategory: (category: string) => handleRequest(() => apiService.getProductsByCategory(category)),
    
    // Cart
    getCart: (sessionId: string) => handleRequest(() => apiService.getCart(sessionId)),
    addToCart: (productId: number, quantity: number, size: string | undefined, sessionId: string) => 
      handleRequest(() => apiService.addToCart(productId, quantity, size, sessionId)),
    updateCartItem: (id: number, quantity: number) => handleRequest(() => apiService.updateCartItem(id, quantity)),
    removeFromCart: (id: number) => handleRequest(() => apiService.removeFromCart(id)),
    clearCart: (sessionId: string) => handleRequest(() => apiService.clearCart(sessionId)),
    
    // Orders
    createOrder: (orderData: CreateOrderRequest) => handleRequest(() => apiService.createOrder(orderData)),
    getOrders: () => handleRequest(() => apiService.getOrders()),
    getOrder: (id: number) => handleRequest(() => apiService.getOrder(id)),
    getOrderByNumber: (orderNumber: string) => handleRequest(() => apiService.getOrderByNumber(orderNumber)),
  };
};


