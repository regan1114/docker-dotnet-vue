# 以 docker-compose 部署 Asp.net Vue mariadb 範例
此範例使用 docker-compose 來配置 mariadb + ASP.Net 6 + Vue.js 3的入門樣板。

* 前端除顯示vuetify介紹頁面、Vue向Web api取資料，更有登入時Jwt token認證機制。
* 資料庫使用docker建立，並賦予初始資料，登入帳密均為admin。

## 基本要求

* [.NET Core](https://www.microsoft.com/net/download/windows) >= 6.0
* [NodeJS](https://nodejs.org/) >= 16
* [Vue CLI](https://cli.vuejs.org/) >= 5.0
* 選擇自己喜歡的編輯器 ，此範例都是使用[VS Code](https://code.visualstudio.com/)

## 使用技術

**ASP.NET 6:**

* Web.API
* JWT認證

**Vue3 with CLI 5.0:**

* javascript
* Vue Router & Pinia
* 第三方套件(Vuetify)

**dokcer-compose**

* dockerfile
* docker-compose啟動順序 
* 在mariadb 建立初始 database 與 table

## 初始化

打開 terminal 輸入：

```
docker-compose up -d
```

## 參考範例

* vue與.net5的spa串接與使用。[here](https://github.com/SoftwareAteliers/asp-net-core-vue-starter)
* JWT與.net6的驗證。[here](https://jasonwatmore.com/post/2022/07/25/vue-3-pinia-user-registration-and-login-example-tutorial)

## License

[![MIT License](https://img.shields.io/badge/license-MIT-blue.svg?style=flat)](https://mit-license.org/)
