import { bootstrap, useAppCreatedService, useModule } from 'mkh-ui'
import './options'
import mod_admin from './index'

useModule(mod_admin)

useAppCreatedService(({ config }) => {
  config.site.title = {
    'zh-cn': '通用统一认证平台',
    en: 'Common Authentication Platform',
  }

  // config.site.title = {
  //   'zh-cn': '17MKH',
  //   en: '17MKH English',
  // }

  /** 配置登录组件 */
  config.component.login = 'k'
  config.auth.enableVerifyCode = false
})

bootstrap()
