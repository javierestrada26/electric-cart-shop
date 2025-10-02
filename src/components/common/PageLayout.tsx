import React, { ReactNode } from 'react';
import { Navbar } from '../layout/Navbar';
import { useCartSidebar } from '@/hooks';
import { CartSidebar } from '../cart/CartSidebar';
import { ProjectStatus } from './ProjectStatus';

interface PageLayoutProps {
  children: ReactNode;
  showNavbar?: boolean;
}

export const PageLayout = ({ children, showNavbar = true }: PageLayoutProps) => {
  const { isCartOpen, openCart, closeCart } = useCartSidebar();

  return (
    <div className="min-h-screen bg-background">
      {showNavbar && <Navbar onCartClick={openCart} />}
      
      <main className="pt-24 pb-16">
        {children}
      </main>

      <CartSidebar isOpen={isCartOpen} onClose={closeCart} />
      <ProjectStatus />
    </div>
  );
};
