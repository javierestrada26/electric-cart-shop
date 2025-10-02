import { Product } from '@/contexts/CartContext';
import { Button } from './ui/button';
import { useCart } from '@/contexts/CartContext';
import { Link } from 'react-router-dom';

interface ProductCardProps {
  product: Product;
}

export const ProductCard = ({ product }: ProductCardProps) => {
  const { addToCart } = useCart();

  return (
    <div className="group relative bg-card rounded-sm overflow-hidden transition-all duration-300 hover:shadow-lg">
      <Link to={`/product/${product.id}`}>
        <div className="aspect-square overflow-hidden bg-secondary">
          <img
            src={product.image}
            alt={product.name}
            className="w-full h-full object-cover transition-transform duration-500 group-hover:scale-105"
          />
        </div>
      </Link>
      
      <div className="p-6">
        <Link to={`/product/${product.id}`}>
          <h3 className="font-semibold text-lg mb-2 group-hover:text-accent transition-colors">
            {product.name}
          </h3>
        </Link>
        <p className="text-muted-foreground text-sm mb-4 line-clamp-2">
          {product.description}
        </p>
        
        <div className="flex items-center justify-between">
          <span className="text-xl font-bold">${product.price.toLocaleString()}</span>
        </div>
      </div>
    </div>
  );
};
