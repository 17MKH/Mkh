const urls = {
  SELECT: 'Dict/Select',
  TREE: 'Dict/Tree',
  CASCADER: 'Dict/Cascader',
}
export default http => {
  return {
    select: params => http.get(urls.SELECT, params),
    tree: params => http.get(urls.TREE, params),
    cascader: params => http.get(urls.CASCADER, params),
  }
}
