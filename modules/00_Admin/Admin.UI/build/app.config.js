/** 应用模式 */

import { defineConfig } from 'vite'
import useBaseConfig from './base.config'

export default defineConfig(({ mode, command }) => {
  const config = useBaseConfig({ target: 'app', mode, command })

  if (mode === 'development') {
    config.resolve.alias['vue-i18n'] = 'vue-i18n/dist/vue-i18n.cjs.js'
  } else if (mode == 'production') {
    config.build = {
      outDir: '../../WebHost/wwwroot/web',
    }
  }
  return config
})
