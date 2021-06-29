<template>
  <v-app>
    <NavigationView/> 
  </v-app>
</template>

<script>
import NavigationView from './pages/NavigationView';

export default {
  name: 'App',
  components:{
    NavigationView
  },
};
</script>
