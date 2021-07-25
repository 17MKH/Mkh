const { resolve } = require('path')
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import mkh from 'mkh-ui/lib/plugins'

export default defineConfig({
  plugins: [
    mkh({
      htmlTransform: {
        render: {
          /** 版权信息 */
          copyright: '版权所有：OLDLI',
          /** Logo */
          logo: './logo.png',
        },
      },
    }),
    vue(),
  ],
  server: {
    port: 5220,
    fs: {
      strict: false,
    },
  },
  resolve: {
    alias: {
      '@': resolve(__dirname, 'package'),
    },
  },
})
