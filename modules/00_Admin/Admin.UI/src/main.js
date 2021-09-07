import { configure } from 'mkh-ui'
import 'mkh-ui/lib/style.css'
import 'mkh-skin-layui'
import 'mkh-skin-layui/lib/style.css'
import './index'

configure({
  http: {
    global: {
      baseURL: 'http://localhost:6220/api/',
    },
  },
  beforeMount({ config }) {
    config.component.login = 'k'
    config.site.title = '通用统一认证平台'
    config.auth.enableButtonPermissions = false
  },
})
