import { defineStore } from 'pinia';

import { api } from '@/helper/fetchWrapper';
import router from '@/router/';
import {useAlertStore} from '@/stores';

export const useAuthStore = defineStore({
    id: 'auth',
    state: () => ({
        // initialize state from local storage to enable user to stay logged in
        user: JSON.parse(localStorage.getItem('user')),
        returnUrl: null
    }),
    actions: {
        async login(data) {
            try {
                const user = await api.post(`users/login`, data);    
                console.log(`user ${JSON.stringify(user)}`)
                // update pinia state
                this.user = user;
                // store user details and jwt in local storage to keep user logged in between page refreshes
                localStorage.setItem('user', JSON.stringify(user));
                // redirect to previous url or default to home page
                router.push(this.returnUrl || '/');
            } catch (error) {
                const alertStore = useAlertStore();
                alertStore.error(error);    
            }
        },
        logout() {
            this.user = null;
            localStorage.removeItem("user");
            router.push('/login');
        }
    }
});
