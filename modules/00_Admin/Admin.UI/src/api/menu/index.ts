import { crud } from 'mkh-ui'
import http from '../http'

const urls = {
  GROUP_SELECT: 'Menu/GroupSelect',
  TREE: 'Menu/Tree',
  UPDATE_SORT: 'Menu/UpdateSort',
}

export default {
  ...crud(http, 'Menu'),
  getGroupSelect: () => http.get(urls.GROUP_SELECT),
  getTree: (params) => http.get(urls.TREE, params),
  updateSort: (sorts) => http.post(urls.UPDATE_SORT, sorts),
}
