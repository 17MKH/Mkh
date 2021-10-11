import { configure } from 'mkh-ui'
import './index'

configure({
  http: {
    global: {
      baseURL: 'http://localhost:6221/api/',
    },
  },
  beforeMount({ config }) {
    config.component.login = 'k'
    config.site.title = '通用统一认证平台'
    config.auth.enableButtonPermissions = false
  },
})
