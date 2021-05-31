import { configure } from "mkh-ui/package/index";
import "mkh-ui/lib/style.css";
import zhCN from "@mkh-locale/zh-cn";
import en from "@mkh-locale/en";
import "./mod.js";

configure({ locale: { messages: [zhCN, en] } });

MkhUI.config.site.logo = "./logo.png";
MkhUI.config.site.title = "通用统一认证平台";
MkhUI.http.global.baseURL = "http://localhost:6220/api/";
