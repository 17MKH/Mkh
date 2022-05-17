const urls = {
  //查询枚举选项列表
  ENUM_OPTIONS: 'Common/EnumOptions',
  //查询平台选项列表
  PLATFORM_OPTIONS: 'Common/PlatformOptions',
}
export default http => {
  return {
    queryEnumOptions: params => http.get(urls.ENUM_OPTIONS, params),
    queryPlatformOptions: () => http.get(urls.PLATFORM_OPTIONS),
  }
}
