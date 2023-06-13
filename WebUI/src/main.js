import { createApp } from 'vue'
import App from './App.vue'
import router from './router/router.js'
import { createVuetify } from 'vuetify'
import 'vuetify/styles'
import 'material-design-icons-iconfont/dist/material-design-icons.css'
import '@mdi/font/css/materialdesignicons.css'
import * as components from 'vuetify/components'
import * as labsComponents from 'vuetify/labs/components'
import * as directives from 'vuetify/directives'


const vuetify = createVuetify({
    components,
    directives,
    labsComponents,
    iconfont: 'md'
})

createApp(App).use(router).use(vuetify).mount('#app')
