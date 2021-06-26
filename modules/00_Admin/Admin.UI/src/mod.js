import mod from '@mkh-mod-admin'

mod.callback = () => {
  const { login, refreshToken, getVerifyCode, getProfile } = mkh.api.admin.authorize
  //设置登录方法
  mkh.config.actions.login = login
  //设置刷新令牌方法
  mkh.config.actions.refreshToken = refreshToken
  //设置获取验证码方法
  mkh.config.actions.getVerifyCode = getVerifyCode
  //设置获取账户信息方法
  mkh.config.actions.getProfile = getProfile
}
