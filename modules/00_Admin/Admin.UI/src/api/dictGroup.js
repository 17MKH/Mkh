const urls = {
  SELECT: 'DictGroup/Select',
}
export default http => {
  return {
    select: () => http.get(urls.SELECT),
  }
}
