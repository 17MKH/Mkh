const urls = {
  /** 登录 */
  LOGIN: "Authorize/Login",
  /** 获取验证码 */
  VERIFY_CODE: "Authorize/VerifyCode",
  /** 获取账户信息 */
  PROFILE: "Authorize/Profile",
};

export default (http) => {
  console.log(http);
  return {
    login(params) {
      return http.post(urls.LOGIN, params);
    },
    getVerifyCode() {
      return http.get(urls.VERIFY_CODE);
    },
    getProfile() {
      return http.get(urls.PROFILE);
    },
  };
};
