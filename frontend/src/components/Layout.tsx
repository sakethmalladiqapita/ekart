import React, { ReactNode } from 'react';

// ✅ Define props interface for Layout component
interface LayoutProps {
  children: ReactNode; // 🧱 Any nested content passed to layout
}

// ✅ Layout component for consistent page structure
const Layout = ({ children }: LayoutProps) => {
  return (
    <div className="flex flex-col min-h-screen bg-shop-background">
      {/* 🔝 Header Section */}
      <header className="bg-white shadow-sm">
        <div className="container mx-auto px-4 py-4">
          <div className="flex justify-between items-center">
            <h1 className="text-xl font-bold text-shop-primary">ShopEase</h1>
            <nav>
              <ul className="flex space-x-6">
                <li>
                  <a href="/" className="text-shop-primary hover:text-shop-accent transition-colors">
                    Products
                  </a>
                </li>
                <li>
                  <a href="/checkout" className="text-shop-primary hover:text-shop-accent transition-colors">
                    Cart (0)
                  </a>
                </li>
              </ul>
            </nav>
          </div>
        </div>
      </header>

      {/* 📦 Main content goes here */}
      <main className="flex-grow">
        {children}
      </main>

      {/* 🔻 Footer Section */}
      <footer className="bg-white border-t">
        <div className="container mx-auto px-4 py-6">
          <p className="text-center text-gray-500 text-sm">
            &copy; {new Date().getFullYear()} ShopEase. All rights reserved.
          </p>
        </div>
      </footer>
    </div>
  );
};

export default Layout;
