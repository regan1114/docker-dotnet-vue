import Vue from "vue";
import store from '../store';
import * as types from '../store/mutations_type';
import Router from "vue-router";
import HelloWorld from "../components/HelloWorld";
import LoginView from "../LoginView";
import FetchDataView from "../pages/FetchDataView";
import UserDataView from "../pages/UserDataView";
// import UserInfoView from "../pages/UserInfoView";

Vue.use(Router);

const router = new Router({
  routes: [
    {
      path: "/",
      name: "HelloWorld",
      title: "首頁", 
      icon: "mdi-home",
      verification:false,
      component: HelloWorld
    },
    {
      path: "/fetch-data",
      name: "fetch-data",
      title: "天氣預報", 
      icon: "mdi-download",
      verification:false,
      component: FetchDataView
    },
    {
      path: "/user-data",
      name: "user-data",
      title: "用戶列表", 
      icon: "mdi-format-list-bulleted",
      verification:true,
      component: UserDataView
    },
    // {
    //   path: "/userInfo",
    //   name: "userInfo",
    //   title: "個人頁面", 
    //   icon: "mdi-information",
    //   verification:true,
    //   component: UserInfoView
    // },
    {
      path: "/login",
      name: "LoginView",
      title: "登入", 
      icon: "mdi-login",
      verification:false,
      component: LoginView
    }
  ]
});

router.beforeEach((to, from, next) => {
  const user = JSON.parse(localStorage.getItem("user"))
  const storeInfo = store.getters.getUserInfo;
  if (user && JSON.stringify(storeInfo) === JSON.stringify({})) {
    store.commit(types.SET_USERINFO, user) 
  }

  if (user && to.path === "/login") {
    next({ path: "/" });
  }
  
  next();
});

export default router;
