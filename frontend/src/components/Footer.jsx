import React from 'react';

const Footer = () => {
  return (
    <footer style={{
      backgroundColor: '#1f2937',
      color: '#d1d5db',
      textAlign: 'center',
      padding: '20px',
      marginTop: 'auto'
    }}>
      <div style={{ fontSize: '0.95rem' }}>
        © {new Date().getFullYear()} eKart. All rights reserved.
      </div>
      <div style={{ fontSize: '0.85rem', marginTop: '8px' }}>
        Built with ❤️ using React, .NET & MongoDB.
      </div>
    </footer>
  );
};

export default Footer;
