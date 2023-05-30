import type { LoginDto, JwtCredential, VerifyCode, Profile } from 'mkh-ui'
import { encode } from 'js-base64'
import http from '../http'

export default {
  login(params: LoginDto) {
    let data = Object.assign({}, params)
    data.username = encode(data.username)
    data.password = encode(data.password)
    return http.post<JwtCredential>('Authorize/Login', data)
  },
  refreshToken: (params) => http.post<JwtCredential>('Authorize/RefreshToken', params),
  getVerifyCode: () => http.get<VerifyCode>('Authorize/VerifyCode'),
  getProfile: () => http.get<Profile>('Authorize/Profile'),
}
