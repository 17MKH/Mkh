import { crud } from 'mkh-ui'
import http from '../http'

export default {
  ...crud(http, 'DictGroup'),
  select: () => http.get('DictGroup/Select'),
}
