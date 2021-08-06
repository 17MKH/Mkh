const urls = {
  SELECT: 'DictGroup/Select',
}
export default http => {
  return {
    select() {
      return http.get(urls.SELECT)
    },
  }
}
