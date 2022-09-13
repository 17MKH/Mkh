import { useAppCreatedService, useAppMountService } from 'mkh-ui'
import { useAdminStore } from './store'
import mod from './mod'
import { useHttp } from './api/http'
import api from './api'

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

useAppMountService(() => {
  useHttp(mod)
})

export default mod

export { api, useAdminStore }
