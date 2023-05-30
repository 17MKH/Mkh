import type { BootstrapOptions } from 'mkh-ui'
import { useBootstrapOptions } from 'mkh-ui'
import messages_zh_cn from './locale.zh-cn'
import messages_en from './locale.en'
import messages_th from './locale.th'

const options: BootstrapOptions = {
  locale: {
    messages: {
      'zh-cn': messages_zh_cn,
      en: messages_en,
      th: messages_th,
    },
  },
  http: {
    global: {
      baseURL: import.meta.env.MKH_API_URL,
    },
  },
}

useBootstrapOptions(options)

export type MessageSchema = typeof messages_zh_cn
