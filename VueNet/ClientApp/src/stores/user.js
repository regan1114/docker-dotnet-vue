import {defineStore} from 'pinia';
import { api } from '@/helper/fetchWrapper';
import { useAlertStore } from '@/stores';

export const useUserStore = defineStore({
  id:"user",
  state: () => ({
    users: null,
    user: {}
  }),
  actions: {
    async getUserData() {
      try {
        this.users = await api.get("users", null);
      } catch (error) {
        const alertStore = useAlertStore();
        alertStore.error(error);          
      }
    }
  }
})