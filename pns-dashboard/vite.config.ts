import path from "path"
import { defineConfig } from 'vite'
import tailwindcss from '@tailwindcss/vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [tailwindcss(), react()],
  resolve: {
    alias: {
      "@": path.resolve(__dirname, "./src"),
    },
  },
  server: {
    proxy: {
      '/api': {
        target: 'https://localhost:7198',
        changeOrigin: true,
        secure: false,
      },
      '/hubs': {
        target: 'https://localhost:7198',
        ws: true,
        changeOrigin: true,
        secure: false,
      }
    }
  }
})
