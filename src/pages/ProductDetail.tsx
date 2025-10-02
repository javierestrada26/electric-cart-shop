import { useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { ArrowLeft } from 'lucide-react';
import { Button } from '@/components/ui/button';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { useCart } from '@/contexts/CartContext';
import { products } from '@/data/products';
import { toast } from '@/hooks/use-toast';
import { Navbar } from '@/components/Navbar';
import { CartSidebar } from '@/components/CartSidebar';

const ProductDetail = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const { addToCart } = useCart();
  const [selectedSize, setSelectedSize] = useState<string>('');
  const [isCartOpen, setIsCartOpen] = useState(false);
  
  const product = products.find((p) => p.id === id);

  if (!product) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-center">
          <h1 className="text-2xl font-bold mb-4">Producto no encontrado</h1>
          <Button onClick={() => navigate('/')}>Volver a la tienda</Button>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-background">
      <Navbar onCartClick={() => setIsCartOpen(true)} />
      <div className="pt-24 pb-16">
      <div className="container mx-auto px-4">
        <Button
          variant="ghost"
          onClick={() => navigate('/')}
          className="mb-8"
        >
          <ArrowLeft className="mr-2 h-4 w-4" />
          Volver
        </Button>

        <div className="grid md:grid-cols-2 gap-12 max-w-6xl mx-auto">
          {/* Image */}
          <div className="aspect-square bg-secondary rounded-sm overflow-hidden">
            <img
              src={product.image}
              alt={product.name}
              className="w-full h-full object-cover"
            />
          </div>

          {/* Product Info */}
          <div className="flex flex-col justify-center space-y-6">
            <div>
              <p className="text-sm text-muted-foreground uppercase tracking-wider mb-2">
                {product.category}
              </p>
              <h1 className="text-4xl font-bold mb-4">{product.name}</h1>
              <p className="text-3xl font-bold mb-6">
                ${product.price.toLocaleString()}
              </p>
            </div>

            <p className="text-lg text-muted-foreground leading-relaxed">
              {product.description}
            </p>

            <div className="space-y-4 pt-4">
              {product.sizes && product.sizes.length > 0 && (
                <div className="space-y-2">
                  <label className="text-sm font-medium">Talla</label>
                  <Select value={selectedSize} onValueChange={setSelectedSize}>
                    <SelectTrigger className="w-full">
                      <SelectValue placeholder="Selecciona una talla" />
                    </SelectTrigger>
                    <SelectContent>
                      {product.sizes.map((size) => (
                        <SelectItem key={size} value={size}>
                          {size}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </div>
              )}

              <Button
                variant="cart"
                size="lg"
                className="w-full"
                onClick={() => {
                  if (product.sizes && product.sizes.length > 0 && !selectedSize) {
                    toast({
                      title: "Selecciona una talla",
                      description: "Por favor selecciona una talla antes de agregar al carrito",
                      variant: "destructive",
                    });
                    return;
                  }
                  addToCart(product, selectedSize || undefined);
                }}
              >
                Agregar al Carrito
              </Button>
              
              <div className="grid grid-cols-3 gap-4 pt-6 border-t border-border">
                <div>
                  <p className="text-sm text-muted-foreground mb-1">Envío</p>
                  <p className="font-semibold">Gratis</p>
                </div>
                <div>
                  <p className="text-sm text-muted-foreground mb-1">Entrega</p>
                  <p className="font-semibold">2-4 días</p>
                </div>
                <div>
                  <p className="text-sm text-muted-foreground mb-1">Garantía</p>
                  <p className="font-semibold">2 años</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      </div>
      <CartSidebar isOpen={isCartOpen} onClose={() => setIsCartOpen(false)} />
    </div>
  );
};

export default ProductDetail;
