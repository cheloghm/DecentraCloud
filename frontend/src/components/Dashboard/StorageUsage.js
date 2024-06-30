// src/components/Dashboard/StorageUsage.js

import React, { useEffect, useRef } from 'react';

const StorageUsage = () => {
  const canvasRef = useRef(null);

  useEffect(() => {
    const ctx = canvasRef.current.getContext('2d');
    ctx.clearRect(0, 0, canvasRef.current.width, canvasRef.current.height);
    ctx.beginPath();
    ctx.moveTo(50, 100);
    ctx.lineTo(150, 200);
    ctx.stroke();
  }, []);

  return (
    <div style={styles.container}>
      <h2>Storage Usage</h2>
      <canvas ref={canvasRef} width="400" height="400" style={styles.canvas}></canvas>
    </div>
  );
};

const styles = {
  container: {
    padding: '20px',
    marginLeft: '220px',
  },
  canvas: {
    border: '1px solid #000',
  },
};

export default StorageUsage;
