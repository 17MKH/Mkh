const urls = {
  ENUM_OPTIONS: 'Common/EnumOptions',
}
export default http => {
  return {
    //查询枚举选项列表
    queryEnumOptions(params) {
      return http.get(urls.ENUM_OPTIONS, params)
    },
  }
}
