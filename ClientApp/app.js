import Vue from 'vue'
import axios from 'axios'
import router from './router/index'
import store from './store'
import { sync } from 'vuex-router-sync'
import App from 'components/app-root'
import { FontAwesomeIcon } from './icons'
import vmodal from 'vue-js-modal'

// Registration of global components
Vue.component('icon', FontAwesomeIcon)
Vue.use(vmodal, { dialog: true })

Vue.prototype.axios = axios

sync(store, router)

const app = new Vue({
  store,
  router,
  ...App
})

export {
  app,
  router,
  store
}
