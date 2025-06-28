
import Vue from 'vue'
import Vuetify, { 
  VApp,
  VNavigationDrawer,
  VFooter,
  VList,
  VBtn,
  VIcon,
  VToolbar,
  VCard,
  VDivider,
  VDialog,
  VTextField,
  VDataTable,
  VSelect,
 } from 'vuetify/lib'
import 'vuetify/src/stylus/app.styl'
import VGrid from 'vuetify/lib/components/VGrid/'
import transitions from 'vuetify/lib/components/transitions/'

Vue.use(Vuetify, {
  components: {
    VApp,
    VNavigationDrawer,
    VFooter,
    VList,
    VBtn,
    VIcon,
    VGrid,
    VToolbar,
    VCard,
    VDivider,
    VDialog,
    VTextField,
    VDataTable,
    VSelect,
    transitions
  },
  theme: {
    primary: '#1976D2',
    secondary: '#424242',
    accent: '#82B1FF',
    error: '#FF5252',
    info: '#2196F3',
    success: '#4CAF50',
    warning: '#FFC107'
  },
  customProperties: true,
  iconfont: 'md',
})
