//导入
import { createRouter, createWebHistory } from 'vue-router'

//定义一个变量
const router = createRouter({
  //路由历史
  history: createWebHistory(import.meta.env.BASE_URL),
  //配置路由
  routes: [
    {
      path: '/',
      //对应页面
      component: ()=>import('../views/LoginView.vue')
    },
    {
      path: '/dashboard',
      component: ()=>import('../views/DashboardView.vue'),
      //子路由
      children : [
        {
            //路由路径，子路由路径不能以斜杠开头
            path: 'user',
            //路由路径所对应的页面
            component : () => import('../views/UserView.vue'),
        },{
          //动态路由  :id是变量
          path: 'user/:id',
          component: ()=> import('../views/UserDetialView.vue'),
        },{
          path: 'activity',
          component: ()=> import('../views/ActivityView.vue'),
        },{
          path: 'activityDetail/:id',
          component: ()=> import('../views/ActivityDetailView.vue')
        },{
          path: 'clue',
          component: ()=> import('../views/ClueView.vue')
        },{
          path: 'clue/:id',
          component: ()=> import('../views/ClueRecordView.vue'),
        },{
          path: 'detail/:id',
          component: ()=> import('../views/ClueDetailView.vue')
        },{
          path: 'customer',
          component: ()=> import('../views/CustomerView.vue'),
        }
      ],
    }
  ]
})
//导出路由对象
export default router
