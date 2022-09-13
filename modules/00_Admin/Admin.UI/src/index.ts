import { useAppCreatedService } from 'mkh-ui'
import { useAdminStore } from './store'
import api from './api'
import mod from 'virtual:mkh-mod-admin'

//注册服务
useAppCreatedService(({ config }) => {
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

export { api, useAdminStore }
