const urls = {
  /** 获取账户默认密码 */
  DEFAULT_PASSWORD: 'Account/DefaultPassword',
}
export default http => {
  const getDefaultPassword = () => {
    return http.get(urls.DEFAULT_PASSWORD)
  }

  return { getDefaultPassword }
}
