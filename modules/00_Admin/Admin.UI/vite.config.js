const { resolve } = require("path");
import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";
import mkh from "mkh-ui/lib/plugins";

export default defineConfig({
  plugins: [
    mkh({
      htmlTransform: {
        custom: resolve(__dirname, "./node_modules/mkh-ui/entries/index.html"),
        render: {
          /** 版权信息 */
          copyright: "版权所有：OLDLI",
          /** Logo */
          logo: "./logo.png",
        },
      },
    }),
    vue(),
  ],
  server: {
    port: 5220,
  },
  resolve: {
    alias: {
      "@": resolve(__dirname, "package"),
    },
  },
});
