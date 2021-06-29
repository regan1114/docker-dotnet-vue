<template>
  <v-container fluid>
    <v-row>
      <v-col>
        <h1>個人首頁</h1>
        <p>置換</p>
      </v-col>
    </v-row>
    <v-row justify="center">
      <v-col md="4">
        <v-btn color="info" @click="refresh">替換token</v-btn>
      </v-col>
      <v-col md="4">
        <v-btn color="primary">撤銷token(等同登出)</v-btn>
      </v-col>
    </v-row>
    <v-alert class="mt-10" v-if="showError" type="error" v-text="errorMessage">
      錯誤訊息框
    </v-alert>
  </v-container>
</template>

<script>
export default {
  name: "UserInfoView",
  data: () => ({
    loading: true,
    showError: false,
    errorMessage: ""
  }),
  computed: {},
  created() {},
  methods: {
  }
};
</script>
