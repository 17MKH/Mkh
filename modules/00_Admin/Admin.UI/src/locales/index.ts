import { useI18n as usei18n } from 'vue-i18n'
import { Locale, MkhMessagesSchema } from 'mkh-ui'
import { AdminMessagesSchema } from '@/locales/lang/zh-cn/index'

const useI18n = () => {
  return usei18n<
    {
      message: {
        mkh: MkhMessagesSchema
        mod: {
          admin: AdminMessagesSchema
        }
      }
    },
    Locale
  >()
}

export { useI18n }
