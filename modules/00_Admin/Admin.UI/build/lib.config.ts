/** 打包库模式 */
import { resolve } from 'path'
import { defineConfig } from 'vite'
import useBaseConfig from './base.config'

export default defineConfig(({ mode, command }) => {
  let config = useBaseConfig({ target: 'lib', mode, command })

  //需要取消复制静态资源目录
  config.publicDir = false
  config.build = {
    outDir: 'lib',
    lib: {
      entry: resolve(__dirname, '../src/index.ts'),
      formats: ['es'],
      fileName: 'index',
    },
    rollupOptions: {
      /** 排除无需打包进去的依赖库 */
      external: ['vue', 'vue-router', 'pinia', 'mkh-ui'],
    },
  }

  return config
})
