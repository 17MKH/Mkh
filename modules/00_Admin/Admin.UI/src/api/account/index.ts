import type { UpdateSkinDto } from 'mkh-ui'
import { crud } from 'mkh-ui'
import http from '../http'

export default {
  /**增删改查 */
  ...crud(http, 'Account'),
  /**
   * 获取账户默认密码
   */
  getDefaultPassword: () => http.get('Account/DefaultPassword'),
  /**
   * 更新皮肤
   */
  updateSkin: (params: UpdateSkinDto) => http.post<void>('Account/UpdateSkin', params),
}
