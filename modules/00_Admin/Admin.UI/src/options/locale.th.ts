import type { MkhLocaleMessages } from 'mkh-ui'
import el from 'element-plus/lib/locale/lang/th'
import mkh from 'mkh-ui/lib/locales/th'
// import mkh from 'mkh-ui/lib/locales/en'
import mod_admin from '@/locales/lang/th'
import mod_admin_routes from '@/locales/lang/th/routes'

const options: MkhLocaleMessages = {
  name: 'th',
  el: el,
  mkh: mkh,
  routes: { ...mod_admin_routes },
  mod: {
    admin: mod_admin,
  },
  skin: {},
}

export default options
