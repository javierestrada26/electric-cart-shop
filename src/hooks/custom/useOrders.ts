import { useState, useCallback } from 'react';
import { apiService, CreateOrderRequest, OrderResponse } from '@/services/api';
import useCart from './useCart';

export const useOrders = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const { items, clearCart } = useCart();

  const createOrder = useCallback(async (customerData: { name: string; email: string }): Promise<OrderResponse | null> => {
    if (items.length === 0) {
      setError('El carrito está vacío');
      return null;
    }

    setLoading(true);
    setError(null);

    try {
      const orderRequest: CreateOrderRequest = {
        customerName: customerData.name,
        customerEmail: customerData.email,
        orderItems: items.map(item => ({
          product: {
            id: parseInt(item.id),
            name: item.name,
            price: item.price,
            image: item.image,
            description: item.description,
            category: item.category,
            sizes: item.sizes,
          },
          quantity: item.quantity,
          size: item.size,
        })),
      };

      const orderResponse = await apiService.createOrder(orderRequest);
      
      // Clear cart after successful order
      await clearCart();
      
      return orderResponse;
    } catch (err) {
      console.error('Error creating order:', err);
      setError(err instanceof Error ? err.message : 'Error creating order');
      return null;
    } finally {
      setLoading(false);
    }
  }, [items, clearCart]);

  return {
    createOrder,
    loading,
    error,
  };
};


