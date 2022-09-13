import { createHttp } from 'mkh-ui'
import options from '@/options'
import mod from 'virtual:mkh-mod-admin?base'

export default createHttp(options, mod.code)
