
import { createApp } from 'vue'
import App from './App.vue'

//从router.js中导入router组件
import router from './router/router'

//import...from...语句导入，从element-plus框架导入ElementPlus组件
import ElementPlus from 'element-plus'

//导入element-plus的css样式，不需要from子句
import 'element-plus/dist/index.css'

//import...from...语句导入，从element-plus框架导入icon图标
import * as ElementPlusIconsVue from '@element-plus/icons-vue'

//导入element-plus国际化的中文包组件
import zhCn from 'element-plus/dist/locale/zh-cn.mjs'

import LoginView from './views/LoginView.vue'//导入loginview

const app = createApp(App)

//element icon注册
for (const [key, component] of Object.entries(ElementPlusIconsVue)) {
    app.component(key, component)
  }

app.use(router)
app.use(ElementPlus, {locale: zhCn})
//利用上面所导入的createApp()函数，创建一个vue应用，mount是挂载到#app地方
app.mount('#app')



