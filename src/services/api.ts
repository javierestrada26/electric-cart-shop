const API_BASE_URL = 'http://localhost:5000/api';
const API_ENABLED = true; // API habilitada

export interface ApiProduct {
  id: number;
  name: string;
  price: number;
  image: string;
  description: string;
  category: string;
  sizes?: string[];
}

export interface ApiCartItem {
  id: number;
  product: ApiProduct;
  quantity: number;
  size?: string;
}

export interface ApiCart {
  items: ApiCartItem[];
  totalAmount: number;
  totalItems: number;
}

export interface ApiOrder {
  id: number;
  orderNumber: string;
  customerEmail: string;
  customerName: string;
  totalAmount: number;
  status: string;
  orderDate: string;
  orderItems: ApiOrderItem[];
}

export interface ApiOrderItem {
  id: number;
  product: ApiProduct;
  quantity: number;
  size?: string;
  unitPrice: number;
  totalPrice: number;
}

export interface CreateOrderRequest {
  customerEmail: string;
  customerName: string;
  orderItems: {
    product: ApiProduct;
    quantity: number;
    size?: string;
  }[];
}

export interface OrderResponse {
  orderNumber: string;
  status: string;
  totalAmount: number;
  orderDate: string;
  message: string;
}

class ApiService {
  private async request<T>(endpoint: string, options: RequestInit = {}): Promise<T> {
    if (!API_ENABLED) {
      throw new Error('API is disabled');
    }
    const url = `${API_BASE_URL}${endpoint}`;
    const config: RequestInit = {
      headers: {
        'Content-Type': 'application/json',
        ...options.headers,
      },
      ...options,
    };
    try {
      const response = await fetch(url, config);
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      // Si la respuesta es 204 o no tiene contenido, retorna undefined
      if (response.status === 204) {
        return undefined as T;
      }
      const text = await response.text();
      if (!text) {
        return undefined as T;
      }
      return JSON.parse(text) as T;
    } catch (error) {
      console.error('API request failed:', error);
      throw error;
    }
  }

  // Products
  async getProducts(): Promise<ApiProduct[]> {
    return this.request<ApiProduct[]>('/products');
  }

  async getProduct(id: number): Promise<ApiProduct> {
    return this.request<ApiProduct>(`/products/${id}`);
  }

  async getProductsByCategory(category: string): Promise<ApiProduct[]> {
    return this.request<ApiProduct[]>(`/products/category/${encodeURIComponent(category)}`);
  }

  // Cart
  async getCart(sessionId: string): Promise<ApiCart> {
    return this.request<ApiCart>(`/cart/${sessionId}`);
  }

  async addToCart(productId: number, quantity: number, size: string | undefined, sessionId: string): Promise<ApiCartItem> {
    return this.request<ApiCartItem>('/cart/add', {
      method: 'POST',
      body: JSON.stringify({
        productId,
        quantity,
        size,
        sessionId,
      }),
    });
  }

  async updateCartItem(id: number, quantity: number): Promise<ApiCartItem> {
    return this.request<ApiCartItem>(`/cart/${id}`, {
      method: 'PUT',
      body: JSON.stringify({ quantity }),
    });
  }

  async removeFromCart(id: number): Promise<void> {
    await this.request(`/cart/${id}`, {
      method: 'DELETE',
    });
  }

  async clearCart(sessionId: string): Promise<void> {
    await this.request(`/cart/clear/${sessionId}`, {
      method: 'DELETE',
    });
  }

  // Orders
  async createOrder(orderData: CreateOrderRequest): Promise<OrderResponse> {
    return this.request<OrderResponse>('/orders', {
      method: 'POST',
      body: JSON.stringify(orderData),
    });
  }

  async getOrders(): Promise<ApiOrder[]> {
    return this.request<ApiOrder[]>('/orders');
  }

  async getOrder(id: number): Promise<ApiOrder> {
    return this.request<ApiOrder>(`/orders/${id}`);
  }

  async getOrderByNumber(orderNumber: string): Promise<ApiOrder> {
    return this.request<ApiOrder>(`/orders/order-number/${orderNumber}`);
  }

  // Purchase Logs
  async getPurchaseLogs(): Promise<any[]> {
    return this.request<any[]>('/purchaselogs');
  }

  async getPurchaseLogsByOrder(orderNumber: string): Promise<any[]> {
    return this.request<any[]>(`/purchaselogs/order/${orderNumber}`);
  }
}

export const apiService = new ApiService();
