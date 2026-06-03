<template>
    <el-button type="success" @click="dialogVisible = true; this.RequestUser = {}">添加用户</el-button>
    <el-button type="danger" @Click="batchDel">批量删除</el-button>

    <el-table
    :data="userList"
    style="width: 100%"
    @selection-change="handleSelectionChange"
   >
    <el-table-column type="selection" />
    <el-table-column type="index" label="序号"  />
    <el-table-column property="loginAct" label="账号"   width="120"/>
    <el-table-column property="name" label="姓名" width="120" show-overflow-tooltip />
    <el-table-column property="phone" label="手机" min-width show-overflow-tooltip />
    <el-table-column property="email" label="邮箱" show-overflow-tooltip />
    <el-table-column property="createTime" label="创建时间" show-overflow-tooltip />

    <el-table-column label="操作" show-overflow-tooltip>
        <template #default="scope">
            <el-button size="small" type="primary" @click="detail(scope.row.id)">查看</el-button>
            <el-button size="small" type="success" @click="edit(scope.row.id)">编辑</el-button>
            <el-button size="small" type="danger" @click="del(scope.row.id)">删除</el-button>
      </template>
    </el-table-column>
  </el-table>
  <!-- 分页条 -->
  <el-pagination background layout="prev, pager, next" 
    :total="total"
    :page-size="pageSize"
    v-on:current-change="toPage"
    v-on:prev-click="toPage"
    v-on:next-click="toPage"
    />
    
    <!-- 新增用户对话框 -->
    <el-dialog
    v-model="dialogVisible"
    :title="RequestUser.id > 0 ? '编辑用户' : '新增用户'" center
    align-center
    width="800"
    draggable
    >
    <!-- :before-close="handleClose" -->

    <template #footer>
        <!--添加验证 1.el-form 加个v-bind:rules="loginRules"属性绑定对应的规则对象 ref="loginRefForm"拿到form的属性-->
        <el-form ref="userRefForm" :model="RequestUser" 
            label-width="120px"  v-bind:rules="RequestUserRules">
            <!-- 2.在form item 中添加prop属性 指向要验证的字段 -->
            <el-form-item label="姓名" prop="name">
                <el-input v-model="RequestUser.name" />
            </el-form-item>
            <el-form-item label="账号" prop="loginAct">
                <el-input v-model="RequestUser.loginAct" />
            </el-form-item>
            <el-form-item label="密码" v-if="RequestUser.id > 0"><!--编辑-->
                <el-input type="password" v-model="RequestUser.loginPwd" />
            </el-form-item>

            <el-form-item label="密码" prop="loginPwd" v-else><!--新增-->
                <el-input type="password" v-model="RequestUser.loginPwd" />
            </el-form-item>
            <el-form-item label="邮箱" prop="email">
                <el-input v-model="RequestUser.email" />
            </el-form-item>
            <el-form-item label="手机号" prop="phone">
                <el-input v-model="RequestUser.phone" />
            </el-form-item>
            <!-- 下拉框由于只有是和否，全部绑定了一个对象 -->
            <el-form-item label="账号未过期" prop="accountNoExpired">
                <el-select v-model="RequestUser.accountNoExpired" placeholder="请选择" >
                    <el-option
                        v-for="item in options"
                        :key="item.value"
                        :label="item.label"
                        :value="item.value"
                    />
                </el-select>
            </el-form-item>
            <el-form-item label="密码未过期" prop="credentialsNoExpired">
                <el-select v-model="RequestUser.credentialsNoExpired" placeholder="请选择" >
                    <el-option
                        v-for="item in options"
                        :key="item.value"
                        :label="item.label"
                        :value="item.value"
                    />
                </el-select>
            </el-form-item>
            <el-form-item label="账号未锁定" prop="accountNoLocked">
                <el-select v-model="RequestUser.accountNoLocked" placeholder="请选择" >
                    <el-option
                        v-for="item in options"
                        :key="item.value"
                        :label="item.label"
                        :value="item.value"
                    />
                </el-select>
            </el-form-item>
            <el-form-item label="账号是否启用" prop="accountEnabled">
                <el-select v-model="RequestUser.accountEnabled" placeholder="请选择" >
                    <el-option
                        v-for="item in options"
                        :key="item.value"
                        :label="item.label"
                        :value="item.value"
                    />
                </el-select>
            </el-form-item>
        </el-form>  

        <div class="dialog-footer">
        <el-button @click="dialogVisible = false">关闭</el-button>
        <el-button type="primary" @click="submitUserInfo">提交</el-button>
        </div>
    </template>
  </el-dialog>
 
</template>

<script>
import {doGet, doPost, doPut, doDelete} from '../http/httpRequest'
import {messageTop, messageConfirm} from '../util/util'
export default{
    data(){
        return{
            userList: [{}],
            //一页多少条数据
            pageSize: 0,
            //总条数
            total: 0,
            //当前页
            current_Page: 1,
            //控制开关新增用户对话框
            dialogVisible: false,
            //存储提交对象
            RequestUser: {},
            //下拉框选择项
            options: [
                {
                    label: "是",
                    value: 1,
                },{
                    label: "否",
                    value: 0,
                },
            ],
            //存储被勾选删除的数据
            delList: [],
            //表单验证
            RequestUserRules:{
                name: [
                    {required:true,message:"姓名不能为空",trigger: 'blur' },
                    {pattern: /^[\u4E00-\u9FA5]{1,5}$/,  message: '姓名必须是中文', trigger: 'blur'}
                ],
                loginAct : [
                    { required: true, message: '请输入登录账号', trigger: 'blur' }
                ],
                loginPwd : [
                    { required: true, message: '请输入登录密码', trigger: 'blur' },
                    { min: 6, max: 16, message: '登录密码长度为6-16位', trigger: 'blur' }
                ],
                phone : [
                    { required: true, message: '请输入手机号码', trigger: 'blur' },
                    { pattern : /^1[3-9]\d{9}$/, message: '手机号码格式有误', trigger: 'blur'}
                ],
                email : [
                    { required: true, message: '请输入邮箱', trigger: 'blur' },
                    { pattern : /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/, message: '邮箱格式有误', trigger: 'blur'}
                ],
                accountNoExpired : [
                    { required: true, message: '请选择账号是否未过期', trigger: 'blur' },
                ],
                credentialsNoExpired : [
                    { required: true, message: '请选择密码是否未过期', trigger: 'blur' },
                ],
                accountNoLocked : [
                    { required: true, message: '请选择账号是否未未锁定', trigger: 'blur' },
                ],
                accountEnabled : [
                    { required: true, message: '请选择账号是否可用', trigger: 'blur' },
                ],
            },
        }
    },
    //父组件提供的重新加载路由页面方法
    inject:['reload'],
    mounted(){
        //获取父组件的页数数据
        // console.log(this.pageIndex);
        // this.current_Page = this.pageIndex;
        //获取用户信息页
        this.getUsersData(this.current_Page);
    },
    methods:{
        //选择框被勾选
        handleSelectionChange: function(e){
            this.delList = [];
            e.forEach(data => {
                let userId = data.id;
                this.delList.push(userId);
            })
        },
        getUsersData:function(pageIndex){
            //获取用户分页信息 第一次加载
            // let data = {
            //     page:pageIndex,
            // }
            doGet('/UserMag/Users/' + pageIndex, {}).then((resp)=>{
                if(resp.data.code === 200){
                    this.userList = resp.data.data;
                    this.pageSize = resp.data.pageSize;
                    this.total = resp.data.total;
                }
            }); 
        },
        //点击分页栏调用
        toPage: function(currentPage){
            //每次点击后记录当前页
            this.current_Page = currentPage;
            //console.log(this.current_Page);
            //获取数据
            this.getUsersData(currentPage);
        },
        //用户详情页
        detail: function(id){
            //跳转到/dashboard/user/ (id)路由上
            this.$router.push('/dashboard/user/' + id);
        },
        //提交用户信息表单
        submitUserInfo: function(){
            let formData = new FormData();
            for (let field in this.RequestUser) {
                if(field == "loginPwd" && this.RequestUser[field] == ""){
                    formData.append(field, "");
                }else{
                    formData.append(field, this.RequestUser[field]);
                }
            }
            //验证表单合法性   
            this.$refs.userRefForm.validate((isValid)=>{
                if(isValid){
                    if(this.RequestUser.id > 0){ //编辑时
                        doPut('/UserMag/User', formData, 1).then( (resp) =>{
                            if(resp.data.code === 200){
                                messageTop("编辑成功", "success");
                                //调用父组件提供的重新加载路由页面方法
                                this.reload();
                                this.dialogVisible = false
                            }else{
                                messageTop("提交失败:"+resp.data.msg, "error");
                            }
                        })
                    }else{
                        doPost('/UserMag/User', formData, 1).then( (resp) =>{
                            if(resp.data.code === 200){
                                messageTop("提交成功", "success");
                                //调用父组件提供的重新加载路由页面方法
                                this.reload();
                                this.dialogVisible = false
                            }else{
                                messageTop("提交失败:"+resp.data.msg, "error");
                            }
                        })
                    }
                }
                
            })
        },
        //编辑用户
        edit: function(id){
            this.loadUserInfo(id);
            //打开对话框
            this.dialogVisible = true;
        },
        //加载用户信息
        loadUserInfo: function(id){
            doGet('UserMag/User/' + id, {}).then((resp)=>{
                if(resp.data.code === 200){
                    this.RequestUser = resp.data.data;
                }
            });
        },
        //删除用户
        del: function(id){
            messageConfirm("是否确定删除", "系统提醒").then(()=>{
                doDelete('UserMag/User/' + id).then((resp)=>{
                    if(resp.data.code === 200){
                        messageTop("删除用户成功", "success");
                        //console.log(this.current_Page + "relaod(x)");
                        this.reload();
                    }else{
                        messageTop("删除用户失败", "error");
                    }
                })
            }).catch(()=>{
                messageTop("用户取消删除", "warning");
            })
        },
        //批量删除用户
        batchDel: function(){
            if(this.delList.length <= 0) {
                messageTop("请先勾选要删除的数据", "warning")
                return;
            }
            //字符串
            let delStr = this.delList.join(',')
            messageConfirm("是否确定删除", "系统提醒").then(()=>{
                doDelete('UserMag/UserBatchDel/' + delStr).then((resp)=>{
                    if(resp.data.code === 200){
                        messageTop("删除用户成功", "success");
                        //console.log(this.current_Page + "relaod(x)");
                        this.reload();
                    }else{
                        messageTop("删除用户失败", "error");
                    }
                })
            }).catch(()=>{
                messageTop("用户取消删除", "warning");
            })
        }
    }
}
</script>

<style scoped>
.el-pagination{
    margin-top: 12px;
}
.el-table{
    margin-top: 12px;
}
</style>