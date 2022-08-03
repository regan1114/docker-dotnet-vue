import {defineStore} from 'pinia';
import { api } from '@/helper/fetchWrapper';
import { useAlertStore } from '@/stores';

export const useWeatherStore = defineStore({
  id: 'weathers',
  state: () => ({
    weathers: null
  }),
  actions: {
    async weatherForecast() {
      try {
        this.weathers = await api.get(`WeatherForecast`, null);
      } catch (error) {
        const alertStore = useAlertStore();
        alertStore.error(error);      
      }
    }
  }
})
