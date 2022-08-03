import { createApp } from 'vue'
import App from './App.vue'
import vuetify from './plugins/vuetify'
import router from './router';
import {pinia} from './stores';
import { loadFonts } from './plugins/webfontloader';

loadFonts()

const app = createApp(App)
app.use(vuetify)
app.use(router)
app.use(pinia)
app.mount('#app')
