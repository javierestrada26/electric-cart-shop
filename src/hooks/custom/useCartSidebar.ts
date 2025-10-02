import { useState } from 'react';

export const useCartSidebar = () => {
  const [isCartOpen, setIsCartOpen] = useState(false);

  const openCart = () => setIsCartOpen(true);
  const closeCart = () => setIsCartOpen(false);
  const toggleCart = () => setIsCartOpen(prev => !prev);

  return {
    isCartOpen,
    openCart,
    closeCart,
    toggleCart,
  };
};



