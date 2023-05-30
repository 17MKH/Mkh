import type { MkhLocaleMessages } from 'mkh-ui'
import el from 'element-plus/lib/locale/lang/en'
import mkh from 'mkh-ui/lib/locales/en'
import mod_admin from '@/locales/lang/en'
import mod_admin_routes from '@/locales/lang/en/routes'

const options: MkhLocaleMessages = {
  name: 'en',
  el: el,
  mkh: mkh,
  routes: { ...mod_admin_routes },
  mod: {
    admin: mod_admin,
  },
  skin: {},
}

export default options
