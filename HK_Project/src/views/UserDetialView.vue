<template class="pageBackColor">
     <el-form :model="userDetail" label-width="120px" style="max-width: 600px" >
                <!-- 2.在form item 中添加prop属性 指向要验证的字段 -->
        <el-form-item label="ID">
        <div class="detail">&nbsp;{{userDetail.id}}</div>
        </el-form-item>

        <el-form-item label="账号">
        <div class="detail">&nbsp;{{userDetail.loginAct}}</div>
        </el-form-item>
<!-- 
        <el-form-item label="密码">
        <div class="detail">&nbsp;******</div>
        </el-form-item> -->

        <el-form-item label="姓名">
        <div class="detail">&nbsp;{{userDetail.name}}</div>
        </el-form-item>

        <el-form-item label="手机">
        <div class="detail">&nbsp;{{userDetail.phone}}</div>
        </el-form-item>

        <el-form-item label="邮箱">
        <div class="detail">&nbsp;{{userDetail.email}}</div>
        </el-form-item>

        <el-form-item label="账号未过期">
        <div class="detail">&nbsp;{{userDetail.accountNoExpired === 1 ? '是' : '否'}}</div>
        </el-form-item>

        <el-form-item label="密码未过期">
        <div class="detail">&nbsp;{{userDetail.credentialsNoExpired === 1 ? '是' : '否'}}</div>
        </el-form-item>

        <el-form-item label="账号未锁定">
        <div class="detail">&nbsp;{{userDetail.accountNoLocked === 1 ? '是' : '否'}}</div>
        </el-form-item>

        <el-form-item label="账号是否启用">
        <div class="detail">&nbsp;{{userDetail.accountEnabled  === 1 ? '是' : '否'}}</div>
        </el-form-item>

        <el-form-item label="创建时间">
        <div class="detail">&nbsp;{{userDetail.createTime}}</div>
        </el-form-item>

        <el-form-item label="创建人">
        <div class="detail">&nbsp;{{userDetail.createByInfo.userName}}</div>
        </el-form-item>

        <el-form-item label="编辑时间">
        <div class="detail">&nbsp;{{userDetail.editTime}}</div>
        </el-form-item>

        <el-form-item label="编辑人">
        <div class="detail">&nbsp;{{userDetail.editByInfo.userName}}</div>
        </el-form-item>

        <el-form-item label="最近登录时间">
        <div class="detail">&nbsp;{{userDetail.lastLoginTime}}</div>
        </el-form-item>

        <el-form-item>
        <el-button type="success" @click="goBack">返 回</el-button>
        </el-form-item>
    </el-form>  

</template>

<script>
import {doGet} from '../http/httpRequest'
import {goBack} from '../util/util'
export default{ 
    data(){
        return{
            userDetail: {
                createByInfo:{},
                editByInfo:{},
            },
        }
    },
    mounted(){
        this.getUserDetail();
    }
    ,
    methods :{
        getUserDetail: function(){
            let id = this.$route.params.id;
            doGet('UserMag/User/' + id, {}).then((resp)=>{
                if(resp.data.code === 200){
                    //console.log(resp);
                    this.userDetail = resp.data.data;
                    if (!this.userDetail.createByInfo) {
                        this.userDetail.createByInfo = {};
                    }
                    if (!this.userDetail.editByInfo) {
                        this.userDetail.editByInfo = {};
                    }
                }
            });
        },
        // //返回上一页 历史记录
        // goBack: function(){
        //     this.$router.go(-1);
        // }
    },
}
</script>

<style scoped>
.detail {
  background-color: #edf5f5;
  width: 100%;
  padding-left: 15px;
}
/* .pageBackColor{
    background-color: white;
} */
</style>