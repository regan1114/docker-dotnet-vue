<template>
  <v-container fluid>
    <v-slide-y-transition mode="out-in">
      <v-row>
        <v-col>
          <h1>帳戶資料</h1>
          <p>資料需驗證才能從後端讀取</p>
          <v-data-table
            :headers="headers"
            :items="itemList"
            hide-default-footer
            :loading="loading"
            class="elevation-1"
          >
          </v-data-table>
        </v-col>
      </v-row>
    </v-slide-y-transition>

    <v-alert class="mt-10" v-if="showError" type="error" v-text="errorMessage">
      錯誤訊息框
    </v-alert>
  </v-container>
</template>

<script>
import { mapActions } from "vuex";

export default {
  name: "UserDataView",
  data: () => ({
    loading: true,
    showError: false,
    errorMessage: "驗證失敗",
    itemList: []
  }),
  computed: {
    headers() {
      return [
        { text: "編號", value: "id" },
        { text: "帳號", value: "username" }
      ];
    }
  },
  created() {
    this.fetchUserData();
  },
  methods: {
    ...mapActions(["getUserData"]),

    async fetchUserData() {
      this.loading = true;
      const result = await this.getUserData();
      this.itemList = result.data;
      this.showError = result.status !== 200;
      this.loading = false;
    }
  }
};
</script>
