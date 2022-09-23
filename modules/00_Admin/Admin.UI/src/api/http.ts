import { createHttp, bootstrapOptions } from 'mkh-ui'
import mod from 'virtual:mkh-mod-admin?base'

export default createHttp(bootstrapOptions, mod.code)
