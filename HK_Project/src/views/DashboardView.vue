<template>
    <el-container>
      <!-- 左侧 -->
      <el-aside v-bind:width="isCollapse ? '64px' : '200px'">
        <div class="MenuTitle">
            @海客管理系统
        </div>

        <!-- style="border-right: solid 0px" 解决左侧菜单大小问题 -->
        <el-menu
        active-text-color="#ffd04b"
        background-color="#334157"
        class="el-menu-vertical-demo"
        :default-active="currentRoute"
        text-color="#fff"
        v-bind:unique-opened="true"
        v-bind:collapse="isCollapse"
        style="border-right: solid 0px"
        v-bind:collapse-transition="false"
        :router="true" 
        >
        <!--  @open="handleOpen"
        @close="handleClose" -->
            <el-sub-menu index="1">
                <template #title>
                    <el-icon><OfficeBuilding /></el-icon>
                    <span>市场活动菜单</span>
                </template>
                <el-menu-item index="/dashboard/activity">市场活动</el-menu-item>
                <el-menu-item index="1-2">市场统计</el-menu-item>
            </el-sub-menu>
            <el-sub-menu index="2">
                <template #title>
                    <el-icon><Magnet /></el-icon>
                    <span>线索管理菜单</span>
                </template>
                <el-menu-item index="/dashboard/clue">线索管理</el-menu-item>
                <el-menu-item index="2-2">线索统计</el-menu-item>
            </el-sub-menu>

            <el-sub-menu index="3">
                <template #title>
                    <el-icon><Avatar /></el-icon>
                    <span>客户管理菜单</span>
                </template>
                <el-menu-item index="/dashboard/customer">客户管理</el-menu-item>
            </el-sub-menu>

            <el-sub-menu index="4">
                <template #title>
                    <el-icon><Wallet /></el-icon>
                    <span>交易管理菜单</span>
                </template>
                <el-menu-item index="4-1">交易管理</el-menu-item>
                <el-menu-item index="4-2">线索统计</el-menu-item>
            </el-sub-menu>

            <el-sub-menu index="5">
                <template #title>
                    <el-icon><Memo /></el-icon>
                    <span>产品管理菜单</span>
                </template>
                <el-menu-item index="5-1">产品管理</el-menu-item>
                <el-menu-item index="5-2">线索统计</el-menu-item>
            </el-sub-menu>

            <el-sub-menu index="6">
                <template #title>
                    <el-icon><Grid /></el-icon>
                    <span>字典管理菜单</span>
                </template>
                <el-menu-item index="6-1">字典管理</el-menu-item>
                <el-menu-item index="6-2">线索统计</el-menu-item>
            </el-sub-menu>

            <el-sub-menu index="7">
                <template #title>
                    <el-icon><User /></el-icon>
                    <span>用户管理菜单</span>
                </template>
                <el-menu-item index="/dashboard/user">用户管理</el-menu-item>
                <el-menu-item index="7-2">线索统计</el-menu-item>
            </el-sub-menu>
            
            <el-sub-menu index="8">
                <template #title>
                    <el-icon><Setting /></el-icon>
                    <span>系统管理菜单</span>
                </template>
                <el-menu-item index="8-1">系统管理</el-menu-item>
                <el-menu-item index="8-2">线索统计</el-menu-item>
            </el-sub-menu>
        </el-menu>
      </el-aside>

      <!-- 右侧 -->
      <el-container class="rightContent">
        <!-- 右上 -->
        <el-header>
            <el-icon class="Show" v-on:click="ShowMenu"><Fold /></el-icon>

            <!-- 登录信息下拉菜单 -->
            <el-dropdown :hide-on-click="false">
            <span class="el-dropdown-link">
                {{ this.user.name }}<el-icon class="el-icon--right"><arrow-down /></el-icon>
            </span>
            <template #dropdown>
                <el-dropdown-menu>
                    <el-dropdown-item>我的资料</el-dropdown-item>
                    <el-dropdown-item>修改密码</el-dropdown-item>
                    <el-dropdown-item divided @Click="OutLogin">退出登录</el-dropdown-item>
                </el-dropdown-menu>
            </template>
        </el-dropdown>
        </el-header>

        <!-- 右中 -->
        <el-main>
            <!--  isRouteAlive 控制路由页面的开启与关闭 -->
            <RouterView v-if="isRouteAlive"></RouterView>
        </el-main>
        <!-- 右下 -->
        <el-footer>
            2024 个人练手项目
        </el-footer>

      </el-container>
    </el-container>
</template>

<script>
import { RouterView } from 'vue-router';
import {doGet} from '../http/httpRequest'
import { getTokenName, messageTop, removeToken , messageConfirm} from "@/util/util";
export default{
    data(){
        return{
            //控制左侧菜单折叠
            isCollapse : false,
            user:{
                name:"",
            },
            //控制路由页面的开启与关闭
            isRouteAlive: true,
            //用来记录当前路由
            currentRoute: "/",
        }
    },
    //提供给子组件使用
    provide()
    {
      return{
        //重新加载页面方法
        reload:()=>{
            this.isRouteAlive = false;  //右侧内容隐藏
            this.$nextTick(function(){ // $nextTick()，当数据更新了(关闭也算)，在dom中渲染后，自动执行该函数，
                this.isRouteAlive = true;
                // this.pageIndex = page;
                // console.log(this.pageIndex + "----page" + page);
            });
        },
        //用来指定重新加载的页码 默认第一页
        //pageIndex: null,
      }
    }
    ,
    //vue中的生命钩子函数mounted页面渲染后执行
    mounted(){
        //加载当前登录用户信息
        this.LoadLoginUser();
        //刷新时获取路由路径，给下拉菜单显示对应子菜单使用
        this.getCurrentRoute();
    },
    methods:{
        //控制左侧菜单折叠方法
        ShowMenu:function(){
            this.isCollapse = !this.isCollapse;
            //console.log(this.isCollapse);
        },
        //加载登录用户信息方法
        LoadLoginUser:function(){
            doGet('/UserMag/Login/userInfo',{}).then( (resp)=>{
                //console.log(resp);
                this.user = resp.data.data
            });
        },
        //退出登录
        OutLogin:function(){
            doGet('/UserMag/OutLogin', {}).then((resp)=>{
                if(resp.data.code === 200){
                    removeToken();
                    //跳到登录页
                    window.location.href = "/";
                    messageTop("成功退出系统", "success");
                }else{
                    messageConfirm("退出系统异常，是否强制退出")
                    .then(()=>{
                        removeToken();
                        window.location.ref="/";
                    }).catch(()=>{
                        console.log("取消退出");
                    });
                }
            });

        },
        //获取当前路由路径 给子菜单
        getCurrentRoute: function(){
            let path = this.$route.path;
            // /dashboard/activityDetail/undefined 多级目录需要截取前面的
            let pathArray = path.split('/');
            if(pathArray.length > 3){
                path = "/" + pathArray[1] + "/" + pathArray[2];
                this.currentRoute = path;
            }else{
                this.currentRoute = path;
            }
        }
    },
}
</script>

<style scoped>
.el-aside{
    background-color: #545C64;
}
.el-header{
    height: 35px;
    line-height: 35px;
    background-color: azure;
    text-align: left;
}
.el-main{
    background-color: #f9f9f9;
}
.el-footer{
    background-color: aliceblue;
    height: 35px;
    /* 左右居中 */
    text-align: center;
    /* 上下居中 和行高保持一致 */
    line-height: 35px;
}
/* 右容器设置高度 计算屏幕百分百 */
.rightContent{
    height: calc(100vh); 
}
.MenuTitle{
    height: 35px;
    color: #f9f9f9;
    text-align: center;
    line-height: 35px;
}
.Show{
    cursor: pointer;
}
/* 登录信息下拉框漂移到右边 */
.el-dropdown{
    float: right;
    text-align: center;
    line-height: 35px;
}
</style>