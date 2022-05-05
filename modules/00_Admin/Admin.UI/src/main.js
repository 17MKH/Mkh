import { configure } from 'mkh-ui'
import './index'

configure({
  http: {
    global: {
      baseURL: import.meta.env.MKH_API_URL,
    },
  },
  beforeMount({ config }) {
    config.component.login = 'k'
    config.site.title = {
      'zh-cn': '通用统一认证平台',
      en: 'Common Authentication Platform',
    }
    config.auth.enableButtonPermissions = true
  },
})
