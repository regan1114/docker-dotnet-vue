<template>
  <div>
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
                      v-model="loginInfo.account"
                      :rules="accountRules"
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
  </div>
</template>

<script setup>
import {ref, reactive } from 'vue'
import { useAuthStore } from '@/stores';

const authStore = useAuthStore();
const valid= ref(true);
const lazy= ref(false);
const loading = ref(false);
const loginInfo = reactive({
        account: "",
        password: ""
      })
const accountRules = reactive([v => !!v || "帳號是必填欄位"]);
const passwordRules = reactive([
        v => !!v || "密碼是必填欄位",
        v => v.length >= 4 || "密碼最少4位數"
      ])
async function loginEvent() {
    if (valid.value) {
      await authStore.login(loginInfo);
    }
  }
</script>
