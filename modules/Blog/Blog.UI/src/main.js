import { configure } from 'mkh-ui'
import 'mkh-ui/lib/style.css'
import zhCN from '@mkh-locale/zh-cn'
import en from '@mkh-locale/en'
import 'mkh-mod-admin'
import 'mkh-mod-admin/lib/style.css'
import './index'

configure({ locale: { messages: [zhCN, en] } })

mkh.config.component.login = 'k'
mkh.config.site.title = '通用统一认证平台'
mkh.config.http.global.baseURL = 'http://localhost:6220/api/'
