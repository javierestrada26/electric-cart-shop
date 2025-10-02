import { useProducts } from '@/hooks';
import { PageLayout, HeroSection, ProductGrid, LoadingSpinner } from '@/components';
import { APP_CONFIG } from '@/constants';

const Index = () => {
  const { products, currentCategory, loading, error } = useProducts();

  const title = currentCategory || APP_CONFIG.name;
  const description = currentCategory
    ? `Explora nuestra colección de ${currentCategory.toLowerCase()}`
    : APP_CONFIG.description;

  return (
    <PageLayout>
      <div className="container mx-auto px-4">
        <HeroSection title={title} description={description} />
        
        {loading && <LoadingSpinner message="Cargando productos..." />}
        
        {error && (
          <div className="text-center py-12">
            <p className="text-red-500 mb-4">Error: {error}</p>
            <p className="text-muted-foreground">Mostrando productos locales como respaldo</p>
          </div>
        )}
        
        {!loading && <ProductGrid products={products} />}
      </div>
    </PageLayout>
  );
};

export default Index;
