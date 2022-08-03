<template>
  <v-container fluid>
    <v-slide-y-transition mode="out-in">
      <v-row>
        <v-col>
          <h1>天氣預報</h1>
          <p>資料不需驗證就能從後端讀取</p>

          <table-view :columnKeys="columnKeys" 
          :items="items">
          </table-view>
        </v-col>
      </v-row>
    </v-slide-y-transition>    
  </v-container>
</template>

<script>
import { formatDateTime } from "@/helper/tools";
import { TableView } from "@/components";
import {useWeatherStore} from "@/stores";

export default {
  name: 'FetchDataView',
  components:{TableView},
  data: () => ({
      loading: true,
      showError: false,
      errorMessage: "Error while loading weather forecast.",
      items: [],
  }),
  computed: {
    columnKeys() {
      return [
        { value: "datetime", text: "日期", key: "date" },
        { value: "temperatureC", text: "攝氏溫度.(C)"},
        { value: "temperatureF", text: "華氏溫度.(F)"},
        { value: "summary", text: "體感"},
      ];
    },
  },
  created(){
    this.fetchWeatherForecasts();
  },
  methods: {
    getColor(temperature) {
      if (temperature < 0) {
        return "blue";
      } else if (temperature >= 0 && temperature < 30) {
        return "green";
      } else {
        return "red";
      }
    },

    convertDate(dateString){
      return formatDateTime(dateString)
    },
    async fetchWeatherForecasts() {
      this.loading = true;
      const weatherStore = useWeatherStore();
      await weatherStore.weatherForecast()
      // this.items = result ? result.data : [];
      this.items = weatherStore.weathers;
      console.log(`${JSON.stringify(this.items)}`)
      this.loading = false;
    },
  },
};
</script>
