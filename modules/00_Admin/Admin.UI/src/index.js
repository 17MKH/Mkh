import mod from '$mkh-mod-admin'

mod.callback = ({ config }) => {
  const { login, refreshToken, getVerifyCode, getProfile } = mkh.api.admin.authorize
  const { updateSkin } = mkh.api.admin.account

  //设置登录方法
  config.actions.login = login
  //设置刷新令牌方法
  config.actions.refreshToken = refreshToken
  //设置获取验证码方法
  config.actions.getVerifyCode = getVerifyCode
  //设置获取账户信息方法
  config.actions.getProfile = getProfile
  //设置保存皮肤的方法
  config.actions.toggleSkin = updateSkin
}
