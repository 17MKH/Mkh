/** 应用模式 */

import { defineConfig } from 'vite'
import useBaseConfig from './base.config'

export default defineConfig(({ mode, command }) => {
  const config = useBaseConfig({ target: 'app', mode, command })

  if (mode == 'production') {
    config.build = {
      outDir: '../../WebHost/wwwroot/web',
    }
  }
  return config
})
