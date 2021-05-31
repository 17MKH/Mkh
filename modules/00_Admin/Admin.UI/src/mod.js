import mod from "@mkh-mod-admin";

mod.callback = ({ api }) => {
  const { login, getVerifyCode, getProfile } = api.authorize;
  //设置登录方法
  MkhUI.config.actions.login = login;
  //设置获取验证码方法
  MkhUI.config.actions.getVerifyCode = getVerifyCode;
  //设置获取账户信息方法
  MkhUI.config.actions.getProfile = getProfile;
};
