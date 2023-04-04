import "bootstrap/dist/css/bootstrap.css"
import 'bootstrap/dist/js/bootstrap.bundle.min.js';

import { createApp } from 'vue'
import App from './App.vue'
import apiService from '@/services/apiService.js'
import dateformatter from "@/mixins/dateformatter"

const app = createApp(App)

app.provide("retrospectiveApi", apiService)
app.mixin(dateformatter)

app.mount('#app')

