import React from 'react';
import { HeroSectionProps } from '@/types';

export const HeroSection = ({ title, description }: HeroSectionProps) => {
  return (
    <div className="text-center mb-16">
      <h1 className="text-5xl md:text-6xl font-bold mb-4 tracking-tight">
        {title}
      </h1>
      <p className="text-xl text-muted-foreground max-w-2xl mx-auto">
        {description}
      </p>
    </div>
  );
};
