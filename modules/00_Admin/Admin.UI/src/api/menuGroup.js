const urls = {
  SELECT: 'MenuGroup/Select',
}
export default http => {
  return {
    select: () => http.get(urls.SELECT),
  }
}
