<template>
  <v-app>
    <v-app-bar color="info" app dark>
        <v-app-bar-nav-icon variant="text" @click.stop="drawer = !drawer"></v-app-bar-nav-icon>
      <v-toolbar-title >{{title}}</v-toolbar-title>
      <v-spacer></v-spacer>
    </v-app-bar>

    <v-navigation-drawer
        v-model="drawer"
        bottom
        temporary
      >
        <v-list
          :items="items"
        >
          <v-list-item
            v-for="(item, i) in menuList"
            :key="i"
            :value="item"
            active-color="primary"
            @click="menuClick(item.path)"
          >
              <v-list-item-avatar start>
                <v-icon :icon="item.icon"></v-icon>
              </v-list-item-avatar>
              <v-list-item-title v-text="item.title"></v-list-item-title>
          </v-list-item>

        </v-list>
      </v-navigation-drawer>


    

    <v-main>
      <router-view />
    </v-main>
    <v-footer> </v-footer>
  </v-app>
</template>

<script>
import { useAuthStore } from '@/stores';

export default {
  name: "NavigationView",
  data: () => ({
    miniVariant: false,
    clipped: true,
    drawer: true,
    right: true,
    title: ".NET Vue 整合",
    items: [],
  }),
  created() {
    this.$router.options.routes.forEach(route => {
      this.items.push({
        title: route.title,
        icon: route.icon,
        path: route.path,
        // verification: route.verification
      });
    });
  },
  computed: {
    menuList() {
      const authStore = useAuthStore();
      const isNotExist = JSON.stringify(authStore.user) === JSON.stringify({})
      let list = JSON.parse(JSON.stringify(this.items))
      if (authStore.user === null || isNotExist) {
        return list.filter(m => !m.verification)
      } else {
        list.push({ title: "登出", icon: "mdi-logout", path: "logout"});
        return list.filter(m => m.path !== "/login");
      }
    }
  },
  methods: {
    async menuClick(urlPath) {
      if (urlPath === "logout") {
        const authStore = useAuthStore();
        authStore.logout();
      } else {
        this.$router.push(urlPath);
      }
    }
  }
};
</script>
