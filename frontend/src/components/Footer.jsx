import React from 'react';

// ✅ Footer component for all pages (sticky bottom on long pages)
const Footer = () => {
  return (
    <footer style={{
      backgroundColor: '#1f2937',  // Dark slate color
      color: '#d1d5db',           // Light gray text
      textAlign: 'center',
      padding: '20px',
      marginTop: 'auto'           // Ensures footer sits at bottom in flex layout
    }}>
      {/* 🔖 Copyright */}
      <div style={{ fontSize: '0.95rem' }}>
        © {new Date().getFullYear()} eKart. All rights reserved.
      </div>

      {/* 💡 Tech stack hint */}
      <div style={{ fontSize: '0.85rem', marginTop: '8px' }}>
        Built using React, .NET & MongoDB.
      </div>
    </footer>
  );
};

export default Footer;
