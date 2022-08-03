
import { createRouter,  createWebHistory } from 'vue-router';
import { useAuthStore, useAlertStore } from '@/stores';
import LoginView from "../LoginView";
import HelloWorld from '../pages/HelloWorld'
import FetchDataView from "../pages/FetchDataView";
import UserDataView from "../pages/UserDataView";

const routes = [
    {
                        path: "/",
                        name: "HelloWorld",
                        title: "首頁", 
                        icon: "mdi-home",
                        verification:false,
                        component: HelloWorld
                    },{
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
                    },
                    // catch all redirect to home page
]

const router = createRouter({
  history: createWebHistory(),
  routes
})
router.beforeEach(async (to) => {
  const alertStore = useAlertStore();
  alertStore.clear();
  // redirect to login page if not logged in and trying to access a restricted page 
  const publicPages = ['/login'];
  const authRequired = !publicPages.includes(to.path);
  const authStore = useAuthStore();
  if (authRequired && !authStore.user) {
      authStore.returnUrl = to.fullPath;
      return '/login';
  }
});

export default router;

