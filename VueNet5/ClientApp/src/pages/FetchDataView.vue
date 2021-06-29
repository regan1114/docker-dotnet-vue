<template>
  <v-container fluid>
    <v-slide-y-transition mode="out-in">
      <v-row>
        <v-col>
          <h1>天氣預報</h1>
          <p>資料不需驗證就能從後端讀取</p>

          <v-data-table
            :headers="headers"
            :items="forecasts"
            hide-default-footer
            :loading="loading"
            class="elevation-1"
          >
            <template v-slot:progress>
              <v-progress-linear color="blue" indeterminate></v-progress-linear>
            </template>
            <template v-slot:[`item.date`]="{ item }">
              <td>{{ convertDate(item.date) }}</td>
            </template>
            <template v-slot:[`item.temperatureC`]="{ item }">
              <v-chip :color="getColor(item.temperatureC)" dark>{{ item.temperatureC }}</v-chip>
            </template>
          </v-data-table>
        </v-col>
      </v-row>
    </v-slide-y-transition>    
  </v-container>
</template>

<script>
import { mapActions } from "vuex";
import { formatDateTime } from "../utils/tools";

export default {
  name: 'FetchDataView',
  
  data: () => ({
      loading: true,
      showError: false,
      errorMessage: "Error while loading weather forecast.",
      forecasts: [],
  }),
  computed: {
    headers() {
      return [
        { text: "日期", value: "date" },
        { text: "攝氏溫度.(C)", value: "temperatureC" },
        { text: "華氏溫度.(F)", value: "temperatureF" },
        { text: "體感", value: "summary" },
      ];
    },
  },
  created(){
    this.fetchWeatherForecasts();
  },
  methods: {
    ...mapActions(["weatherForecast"]),
    
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
      const result = await this.weatherForecast()
      this.forecasts = result.data;
      
      this.loading = false;
    },
  },
};
</script>
