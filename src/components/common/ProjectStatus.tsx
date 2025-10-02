import React, { useState, useEffect } from 'react';

export const ProjectStatus = () => {
  const [apiStatus, setApiStatus] = useState<'checking' | 'connected' | 'disconnected'>('checking');
  const [showMessage, setShowMessage] = useState(false);

  useEffect(() => {
    const checkApiStatus = async () => {
      try {
        const response = await fetch('http://localhost:5000/api/products');
        if (response.ok) {
          setApiStatus('connected');
        } else {
          setApiStatus('disconnected');
        }
      } catch (error) {
        setApiStatus('disconnected');
      }
    };

    checkApiStatus();
    const interval = setInterval(checkApiStatus, 10000); // Check every 10 seconds

    return () => clearInterval(interval);
  }, []);
    // Mostrar mensaje por 3 segundos cuando cambia el estado
    useEffect(() => {
      if (apiStatus !== 'checking') {
        setShowMessage(true);
        const timer = setTimeout(() => setShowMessage(false), 3000); // 3 segundos
        return () => clearTimeout(timer);
      }
    }, [apiStatus]);
  
    if (!showMessage) return null; // No renderizar nada

  const getStatusConfig = () => {
    switch (apiStatus) {
      case 'connected':
        return {
          bgColor: 'bg-green-100',
          borderColor: 'border-green-400',
          textColor: 'text-green-700',
          dotColor: 'bg-green-500',
          message: 'API Conectada - Backend funcionando'
        };
      case 'disconnected':
        return {
          bgColor: 'bg-yellow-100',
          borderColor: 'border-yellow-400',
          textColor: 'text-yellow-700',
          dotColor: 'bg-yellow-500',
          message: 'Modo Local - Backend desconectado'
        };
      default:
        return {
          bgColor: 'bg-blue-100',
          borderColor: 'border-blue-400',
          textColor: 'text-blue-700',
          dotColor: 'bg-blue-500',
          message: 'Verificando conexión...'
        };
    }
  };

  const config = getStatusConfig();

  return (
    <div className={`fixed bottom-4 right-4 ${config.bgColor} ${config.borderColor} ${config.textColor} px-4 py-2 rounded-lg shadow-lg z-50`}>
      <div className="flex items-center gap-2">
        <div className={`w-2 h-2 ${config.dotColor} rounded-full ${apiStatus === 'checking' ? 'animate-pulse' : ''}`}></div>
        <span className="text-sm font-medium">
          {config.message}
        </span>
      </div>
    </div>
  );
};
