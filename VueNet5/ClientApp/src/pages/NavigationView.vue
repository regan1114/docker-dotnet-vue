<template>
  <v-app>
    <v-navigation-drawer
      persistent
      :mini-variant="miniVariant"
      :clipped="clipped"
      v-model="drawer"
      enable-resize-watcher
      fixed
      app
    >
      <v-list>
        <v-list-item
          value="true"
          v-for="(item, i) in menuList"
          :key="i"
          @click="menuClick(item.path)"
        >
          <v-list-item-action>
            <v-icon v-html="item.icon"></v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title v-text="item.title"></v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>

    <v-app-bar app :clipped-left="clipped" color="info" dark>
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>
      <v-btn class="d-none d-lg-flex" icon @click="miniVariant = !miniVariant">
        <v-icon
          v-html="miniVariant ? 'mdi-chevron-right' : 'mdi-chevron-left'"
        ></v-icon>
      </v-btn>
      <v-btn class="d-none d-lg-flex" icon @click="clipped = !clipped">
        <v-icon>mdi-web</v-icon>
      </v-btn>
      <v-toolbar-title v-text="title"></v-toolbar-title>
      <v-spacer></v-spacer>
    </v-app-bar>

    <v-main>
      <router-view />
    </v-main>
    <v-footer> </v-footer>
  </v-app>
</template>

<script>
// import HelloWorld from './components/HelloWorld';
import { mapGetters, mapActions } from "vuex";

export default {
  name: "NavigationView",
  data: () => ({
    miniVariant: false,
    clipped: true,
    drawer: true,
    right: true,
    title: ".NET 5 Vue 整合",
    items: []
  }),
  created() {
    this.$router.options.routes.forEach(route => {
      this.items.push({
        title: route.title,
        icon: route.icon,
        path: route.path,
        verification: route.verification
      });
    });
  },
  computed: {
    ...mapGetters(["getUserInfo"]),
    menuList() {
      let list = JSON.parse(JSON.stringify(this.items))
      if (
        this.getUserInfo === null ||
        JSON.stringify(this.getUserInfo) === JSON.stringify({})
      ) {
        return list.filter(m => !m.verification)
      } else {
        list.push({ title: "登出", icon: "mdi-logout", path: "logout"});
        
        return list.filter(m => m.path !== "/login");
      }
    }
  },
  methods: {
    ...mapActions(["logout"]),

    async menuClick(urlPath) {
      if (urlPath === "logout") {
        await this.logout();
      } else {
        this.$router.push(urlPath);
      }
    }
  }
};
</script>
