const urls = {
  //查询枚举选项列表
  ENUM_OPTIONS: 'Common/EnumOptions',
}
export default http => {
  return {
    queryEnumOptions: params => http.get(urls.ENUM_OPTIONS, params),
  }
}
