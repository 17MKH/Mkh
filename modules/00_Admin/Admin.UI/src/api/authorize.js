import { encode } from 'js-base64'

const urls = {
  /** 登录 */
  LOGIN: 'Authorize/Login',
  /** 刷新令牌 */
  REFRESH_TOKEN: 'Authorize/RefreshToken',
  /** 获取验证码 */
  VERIFY_CODE: 'Authorize/VerifyCode',
  /** 获取账户信息 */
  PROFILE: 'Authorize/Profile',
}

export default http => {
  return {
    login(params) {
      let data = Object.assign({}, params)
      data.username = encode(data.username)
      data.password = encode(data.password)
      return http.post(urls.LOGIN, data)
    },
    refreshToken: params => http.post(urls.REFRESH_TOKEN, params),
    getVerifyCode: () => http.get(urls.VERIFY_CODE),
    getProfile: () => http.get(urls.PROFILE),
  }
}
