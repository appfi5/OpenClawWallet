import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'
import { fileURLToPath, URL } from 'node:url'

// https://vite.dev/config/
export default defineConfig({
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url)),
    },
  },
  plugins: [react()],

  server: {
    proxy: {
      '/api': {
        target: 'https://OpenClawWalletServer1.rivtower.cc/',
        // target: 'http://127.0.0.1:8080',
        changeOrigin: true,
        // rewrite: path => path.replace(/^\/api/, ''),
      }
    }
  }
})
