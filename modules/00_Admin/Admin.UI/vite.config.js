const { resolve } = require('path')
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import mkh from 'mkh-ui/lib/plugins'

export default defineConfig(({ mode, command }) => {
  let config = {
    base: './',
    server: {
      port: 5220,
    },
    plugins: [
      mkh({
        mode,
        command,
        /** 依赖模块 */
        dependencyModules: [],
        /** 皮肤 */
        skins: [],
        /** 语言包 */
        locales: ['zh-cn', 'en'],
        /** index.html文件转换 */
        htmlTransform: {
          /** 模板渲染数据，如果使用自己的模板，则自己定义渲染数据 */
          render: {
            /** 版权信息 */
            copyright: '版权所有：OLDLI',
            /** Logo */
            logo: './assets/mkh/logo.png',
          },
          /** 压缩配置 */
          htmlMinify: {},
        },
      }),
      vue(),
    ],
    css: {
      preprocessorOptions: {
        scss: {
          charset: false,
        },
      },
    },
    resolve: {
      alias: {
        '@': resolve(__dirname),
      },
    },
  }

  //打包成库
  if (mode == 'lib') {
    //库模式需要取消复制静态资源目录
    config.publicDir = false

    config.build = {
      lib: {
        entry: resolve(__dirname, 'src/index.js'),
        formats: ['es'],
        fileName: 'index',
      },
      outDir: 'lib',
      rollupOptions: {
        /** 排除无需打包进去的依赖库 */
        external: ['vue', 'vue-router', 'vuex', 'mkh-ui'],
      },
    }
  } else if (mode == 'production') {
    config.build = {
      outDir: '../../WebHost/wwwroot/web',
    }
  }
  return config
})
