import mod from 'virtual:mkh-mod-admin'
import { useAppService } from 'mkh-ui'
import api from './api'

//注册服务
useAppService(({ config }) => {
  const { updateSkin } = api.account
  const { login, getVerifyCode, getProfile, refreshToken } = api.authorize

  config.systemActions = {
    login,
    getVerifyCode,
    getProfile,
    toggleSkin: updateSkin,
    refreshToken,
  }
})

export default mod
