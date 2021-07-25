const urls = {
  GROUP_SELECT: 'Menu/GroupSelect',
  TREE: 'Menu/Tree',
  UPDATE_SORT: 'Menu/UpdateSort',
}
export default http => {
  return {
    getGroupSelect() {
      return http.get(urls.GROUP_SELECT)
    },
    getTree(params) {
      return http.get(urls.TREE, params)
    },
    updateSort(sorts) {
      return http.post(urls.UPDATE_SORT, sorts)
    },
  }
}
