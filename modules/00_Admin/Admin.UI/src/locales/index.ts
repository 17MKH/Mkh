import { useI18n as usei18n } from 'vue-i18n'
import { MessagesSchema } from '@/locales/lang/zh-cn/index'

const useI18n = () => {
  return usei18n<{ message: MessagesSchema }>()
}

export { useI18n }
