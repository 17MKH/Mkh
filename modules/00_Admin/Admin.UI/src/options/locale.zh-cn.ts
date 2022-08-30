import type { MkhLocaleMessages } from 'mkh-ui'
import el from 'element-plus/lib/locale/lang/zh-cn'
import mkh from 'mkh-ui/lib/locales/zh-cn'
import mod_admin from '@/locales/zh-cn'
import mod_admin_routes from '@/locales/zh-cn/routes'

const options: MkhLocaleMessages = {
  name: 'zh-cn',
  el: el,
  mkh: mkh,
  routes: { ...mod_admin_routes },
  mod: {
    doc: mod_admin,
  },
  skin: {},
}

export default options
