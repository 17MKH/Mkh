const urls = {
  SELECT: 'MenuGroup/Select',
}
export default http => {
  return {
    select() {
      return http.get(urls.SELECT)
    },
  }
}
