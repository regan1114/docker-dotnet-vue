<template>
  <v-app>
    <v-main>
      <AlertView/>
      <NavigationView/>
    </v-main>
  </v-app>
</template>

<script setup>
import NavigationView from './NavigationView'
import { AlertView } from '@/components';
</script>
