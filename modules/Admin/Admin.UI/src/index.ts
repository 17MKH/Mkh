import { useAppCreatedService } from 'mkh-ui'
import useStore from './store'
import mod from 'virtual:mkh-mod-admin'
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

export default mod

export { api, useStore }
