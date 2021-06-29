<template>
  <v-app>
    <v-container fluid>
      <v-row align="center" justify="center">
        <v-col sm="8" md="4">
          <v-card>
            <v-toolbar color="info" dark flat>
              <v-toolbar-title>登入頁面</v-toolbar-title>
            </v-toolbar>
            <v-card-text>
              <v-form ref="form" v-model="valid" :lazy-validation="lazy">
                <v-row>
                  <v-col>
                    <v-text-field
                      v-model="loginInfo.username"
                      :rules="usernameRules"
                      label="帳號"
                      required
                    ></v-text-field>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col>
                    <v-text-field
                      v-model="loginInfo.password"
                      type="password"
                      :rules="passwordRules"
                      label="密碼"
                      required
                    ></v-text-field>
                  </v-col>
                </v-row>
              </v-form>
            </v-card-text>
            <v-card-actions>
              <v-row>
                <v-col>
                  <v-btn
                  style="width: 100%; margin: 10 auto"
                  color="info"
                  :loading="loading"
                  @click="loginEvent"
                  >登入</v-btn>
                </v-col>
              </v-row>
            </v-card-actions>
          </v-card>
        </v-col>
      </v-row>
    </v-container>
    <v-alert :value="showError" type="error" v-text="errorMessage">
      錯誤訊息
    </v-alert>
  </v-app>
</template>

<script>
import { mapActions } from "vuex";

export default {
  name: "LoginView",
  data() {
    return {
      valid: true,
      lazy: false,
      loading: false,
      loginInfo: {
        username: "",
        password: ""
      },
      usernameRules: [v => !!v || "帳號是必填欄位"],
      passwordRules: [
        v => !!v || "密碼是必填欄位",
        v => v.length >= 4 || "密碼最少4位數"
      ],
      showError: false,
      errorMessage: ""
    };
  },
  methods: {
    ...mapActions(["login"]),

    async loginEvent() {
      if (this.valid) {
        const results = await this.login(this.loginInfo);
        if (results.status !== 200) {
          console.log(JSON.stringify(results))
          this.showError = true;
          this.errorMessage = results.message;
        } else {
          this.$router.push('/').catch(()=>{});  
        }
      }
    }
  }
};
</script>
