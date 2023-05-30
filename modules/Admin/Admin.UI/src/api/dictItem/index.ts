import { crud } from 'mkh-ui'
import http from '../http'

export default {
  ...crud(http, 'DictItem'),
}
