import { configure } from 'mkh-ui'
import 'mkh-ui/lib/style.css'
import zhCN from '@mkh-locale/zh-cn'
import en from '@mkh-locale/en'
import './mod.js'

configure({ locale: { messages: [zhCN, en] } })

mkh.config.site.logo = './logo.png'
mkh.config.site.title = '通用统一认证平台'
mkh.config.http.global.baseURL = 'http://localhost:6220/api/'
