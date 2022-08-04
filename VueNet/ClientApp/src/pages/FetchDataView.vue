<template>
  <v-container fluid>
    <v-slide-y-transition mode="out-in">
      <v-row>
        <v-col>
          <h1>天氣預報</h1>
          <p>資料不需驗證就能從後端讀取</p>

          <table-view :columnKeys="columnKeys" :items="items"> </table-view>
        </v-col>
      </v-row>
    </v-slide-y-transition>
  </v-container>
</template>

<script setup>
//採用vue3寫法
import { ref, onMounted } from "vue";
import { TableView } from "@/components";
import { useWeatherStore } from "@/stores";
import { reactive } from "@vue/reactivity";

const loading = ref(true);

const items = ref([]);

const columnKeys = reactive([
  { value: "datetime", text: "日期", key: "date" },
  { value: "temperatureC", text: "攝氏溫度.(C)" },
  { value: "temperatureF", text: "華氏溫度.(F)" },
  { value: "summary", text: "體感" },
]);
async function fetchWeatherForecasts() {
  loading.value = true;
  const weatherStore = useWeatherStore();
  await weatherStore.weatherForecast();
  // this.items = result ? result.data : [];
  items.value = weatherStore.weathers;
  console.log(`${JSON.stringify(items)}`);
  loading.value = false;
}
const loadData = async () => {
  fetchWeatherForecasts();
};
onMounted(loadData);
</script>
