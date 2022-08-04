<template>
  <v-container fluid>
    <v-slide-y-transition mode="out-in">
      <v-row>
        <v-col>
          <h1>帳戶資料</h1>
          <p>身份需驗證才能讀取</p>
          <table-view :columnKeys="columnKeys" 
          :items="items">
          </table-view>
        </v-col>
      </v-row>
    </v-slide-y-transition>
  </v-container>
</template>

<script>
//保留vue2寫法
import { TableView } from '@/components';
import { useUserStore } from '@/stores';

export default {
  name: "UserDataView",
  components:{TableView},
  data: () => ({
    loading: true,
    showError: false,
    items: []
  }),
  computed: {
    columnKeys() {
      return [
        { text: "編號", value: "id" },
        { text: "帳號", value: "account"},
        { text: "帳號名稱", value: "username" }
      ];
    }
  },
  created() {
    this.loadData();
  },
  methods: {
    async loadData() {
      const userStore = useUserStore();
      this.loading = true;
      await userStore.getUserData();
      this.items = userStore.users;
      this.loading = false;
    }
  }
};
</script>
