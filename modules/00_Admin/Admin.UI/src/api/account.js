const urls = {
  /** 获取账户默认密码 */
  DEFAULT_PASSWORD: 'Account/DefaultPassword',
  /** 更新皮肤 */
  UPDATE_SKIN: 'Account/UpdateSkin',
}
export default http => {
  return {
    getDefaultPassword: () => http.get(urls.DEFAULT_PASSWORD),
    updateSkin: params => http.post(urls.UPDATE_SKIN, params),
  }
}
