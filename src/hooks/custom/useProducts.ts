import { useState, useEffect, useMemo, useCallback } from 'react';
import { useSearchParams } from 'react-router-dom';
import { apiService, ApiProduct } from '@/services/api';
import { products as localProducts } from '@/data/products';
import { Product } from '@/types';

export const useProducts = () => {
  const [searchParams] = useSearchParams();
  const categoryFilter = searchParams.get('category');
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const convertApiProductToProduct = useCallback((apiProduct: ApiProduct): Product => ({
    id: apiProduct.id.toString(),
    name: apiProduct.name,
    price: apiProduct.price,
    image: apiProduct.image,
    description: apiProduct.description,
    category: apiProduct.category,
    sizes: apiProduct.sizes,
  }), []);

  const fetchProducts = useCallback(async () => {
    setLoading(true);
    setError(null);
    
    try {
      let apiProducts: ApiProduct[];
      
      if (categoryFilter) {
        apiProducts = await apiService.getProductsByCategory(categoryFilter);
      } else {
        apiProducts = await apiService.getProducts();
      }

      const convertedProducts = apiProducts.map(convertApiProductToProduct);
      setProducts(convertedProducts);
    } catch (err) {
      console.error('Error fetching products from API:', err);
      setError(err instanceof Error ? err.message : 'Error fetching products');
      
      // Fallback to local data
      const filteredLocalProducts = categoryFilter
        ? localProducts.filter((product) => product.category === categoryFilter)
        : localProducts;
      setProducts(filteredLocalProducts);
    } finally {
      setLoading(false);
    }
  }, [categoryFilter, convertApiProductToProduct]);

  useEffect(() => {
    fetchProducts();
  }, [fetchProducts]);

  const categories = useMemo(() => {
    return [...new Set(products.map(product => product.category))];
  }, [products]);

  return {
    products,
    categories,
    currentCategory: categoryFilter,
    loading,
    error,
    refetch: fetchProducts,
  };
};
