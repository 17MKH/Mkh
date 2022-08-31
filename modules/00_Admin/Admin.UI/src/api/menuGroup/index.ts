import { crud } from 'mkh-ui'
import http from '../http'

const urls = {
  SELECT: 'MenuGroup/Select',
}
export default {
  ...crud(http, 'MenuGroup'),
  select: () => http.get(urls.SELECT),
}
