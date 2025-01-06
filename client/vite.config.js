import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

export default defineConfig({
  server: {
    open: true,
    proxy: {
      "/api": {
        target: "https://localhost:5001", // Replace with your backend URL
        changeOrigin: true,
        secure: false,
      },
    },
  },
  build: {
    outDir: "build",
  },
  plugins: [react()],
});
