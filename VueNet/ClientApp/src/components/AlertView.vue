<template>
    <v-alert v-if="alert" :type="alert.type" >
      {{alert.message}}
    </v-alert>
</template>
<script setup>
//採用vue3寫法
import { watch } from 'vue'
import { storeToRefs } from 'pinia';
import { useAlertStore } from '@/stores';

const alertStore = useAlertStore();
const { alert } = storeToRefs(alertStore);
watch(alert, (newVal, oldVal) => {
  console.log({ newVal, oldVal })
  if(newVal){
    setTimeout(() => {
      alertStore.clear()
    }, 2000)
  }
})
</script>
