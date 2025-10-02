import React from 'react';
import { ShoppingCart } from 'lucide-react';
import { Button } from '../ui/button';
import { useCart } from '@/hooks';
import { Link, useLocation } from 'react-router-dom';
import { NavbarProps } from '@/types';
import { PRODUCT_CATEGORIES } from '@/constants';

export const Navbar = ({ onCartClick }: NavbarProps) => {
  const { cartItemCount } = useCart();
  const location = useLocation();
  const searchParams = new URLSearchParams(location.search);
  const currentCategory = searchParams.get('category');

  return (
    <nav className="fixed top-0 left-0 right-0 z-50 bg-background/95 backdrop-blur-sm border-b border-border">
      <div className="container mx-auto px-4">
        <div className="h-16 flex items-center justify-between">
          <Link to="/" className="text-2xl font-bold tracking-tight hover:opacity-80 transition-opacity">
            TECH SHOP
          </Link>
          
          <div className="flex items-center gap-6">
            {PRODUCT_CATEGORIES.map((category) => (
              <Link
                key={category.key}
                to={`/?category=${category.key}`}
                className={`text-sm font-medium transition-colors hover:text-accent ${
                  currentCategory === category.key ? 'text-accent' : 'text-foreground'
                }`}
              >
                {category.label}
              </Link>
            ))}
            
            <Button
              variant="ghost"
              size="icon"
              onClick={onCartClick}
              className="relative"
            >
              <ShoppingCart className="h-5 w-5" />
              {cartItemCount > 0 && (
                <span className="absolute -top-1 -right-1 bg-accent text-accent-foreground text-xs font-bold rounded-full h-5 w-5 flex items-center justify-center">
                  {cartItemCount}
                </span>
              )}
            </Button>
          </div>
        </div>
      </div>
    </nav>
  );
};
