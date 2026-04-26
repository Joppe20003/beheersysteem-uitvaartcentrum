import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import basicSsl from '@vitejs/plugin-basic-ssl'

// https://vite.dev/config/
export default defineConfig({
    plugins: [react(), basicSsl()],
    server: {
        https: {}, // Forceert HTTPS
        port: 5173,
        open: true,  // Dit opent automatisch je browser op de juiste https URL
        strictPort: true,
    }
})
