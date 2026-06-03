<template>
    <el-container>
        <!--左侧-->
        <el-aside width="200px">
            <img src="../assets/logo.svg" alt=".">
            <p class="imgTitle">欢迎使用海客管理系统</p>
        </el-aside>
         <!--右侧-->
        <el-main>
            <div class="loginTitle">欢迎登录</div>

            <!--添加验证 1.el-form 加个v-bind:rules="loginRules"属性绑定对应的规则对象 ref="loginRefForm"拿到form的属性-->
            <el-form ref="loginRefForm" :model="user" label-width="120px" style="max-width: 600px" v-bind:rules="loginRules">
                <!-- 2.在form item 中添加prop属性 指向要验证的字段 -->
                <el-form-item label="账号" prop="loginAct">
                    <el-input v-model="user.loginAct" />
                </el-form-item>
                <el-form-item label="密码" prop="loginPwd">
                    <el-input type="password" v-model="user.loginPwd" />
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" v-on:click="login">登 录</el-button>
                </el-form-item>
                <el-form-item>
                    <el-checkbox label="记住我" v-model="user.rememberMe" />
                </el-form-item>
            </el-form>  
        </el-main>
    </el-container>
  </template>
  
  

<script>
    import {defineComponent} from 'vue';
    import {doGet, doPost} from "../http/httpRequest.js";
    import { messageTop, getTokenName, removeToken } from '@/util/util.js';
    export default defineComponent({
        name : "LoginView",
        data(){
            return{
                user :{
                  
                },
                //登录表单的验证规则
                loginRules:{
                    //定义账号的验证规则，规则可以有多个，所以是数组
                    loginAct:[
                        { required: true, message: '账号不能为空', trigger: 'blur' },
                    ],
                    //登录密码的规则
                    loginPwd:[
                        { required: true, message: '密码不能为空', trigger: 'blur' },
                        { min: 6, max: 16, message: '密码长度必须在6-16之间', trigger: 'blur' },
                    ]
                }
            }
        },
        mounted(){
            //页面渲染后调用免登录方法
            this.freeLogin();
        },
        methods:{
            //登录函数
            login(){
               //提交前验证输入框的合法性
               this.$refs.loginRefForm.validate((isValid)=>{

                //isValid true 为验证通过 false未通过
                    if(isValid){
                        //可以提交登录
                        // let formData = new FormData(); //这种不是json类型，需要指定请求头中的类型
                        // formData.append("loginAct", this.user.loginAct);
                        // formData.append("loginPwd", this.user.loginPwd);
                        
                         let data = {
                            loginAct : this.user.loginAct,
                            loginPwd : this.user.loginPwd,
                            rememberMe : this.user.rememberMe
                        }

                        //console.log(this.user.rememberMe);
                        doPost("/Authorize/login", data, 0).then( (resp) => {
                            console.log(resp);
                            if(resp.data.code === 200){
                                //提示
                                messageTop("登录成功", "success");
                                //登陆成功 删除之前存储的token
                                removeToken();

                                //前端存储JWT
                                if(this.user.rememberMe===true){
                                    window.localStorage.setItem(getTokenName(), resp.data.data);
                                }else{
                                    window.sessionStorage.setItem(getTokenName(), resp.data.data);
                                }
                                //跳转到系统的主页面
                                window.location.href = "/dashboard";
                            }else{
                                //失败提示 success' | 'warning' | 'info' | 'error'
                                messageTop("登录失败", "error");
                            }
                        });
                    }
               });
            },
            //免登录函数
            freeLogin: function(){
                //判断token是否存在localStorage里面 在里面证明上次登录勾选了记住我
                let token = window.localStorage.getItem(getTokenName());
                if(token){
                    doGet("/User/login/free").then(resp=>{
                        if(resp.data.code === 200){
                            //操作成功，token有效
                            window.location.href = "/dashboard";
                        }
                    });
                }
            }

        }
    })
</script>

<style scoped>
.el-aside {
    background: #1a1a1a;
    width: 40%;
    text-align: center;
}
.el-main {
    height: calc(100vh);
}
img {
    height: 413px;
}
.imgTitle {
    color: #f9f9f9;
    font-size: 28px;
}
.el-form {
    width: 60%;
    margin: auto;
}
.loginTitle {
    text-align: center;
    margin-top: 100px;
    margin-bottom: 25px;
    font-weight: bold;
}
.el-button {
    width: 100%;
}

</style>