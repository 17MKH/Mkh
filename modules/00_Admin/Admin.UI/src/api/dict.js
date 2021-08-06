const urls = {
  SELECT: 'Dict/Select',
  TREE: 'Dict/Tree',
  CASCADER: 'Dict/Cascader',
}
export default http => {
  const select = params => {
    return http.get(urls.SELECT, params)
  }
  const tree = params => {
    return http.get(urls.TREE, params)
  }
  const cascader = params => {
    return http.get(urls.CASCADER, params)
  }

  return { select, tree, cascader }
}
