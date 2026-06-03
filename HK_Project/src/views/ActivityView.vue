<template>
    <!-- 行内表单 -->
    <el-form :inline="true" :model="activeModel" :rules="activityRules">
        <el-form-item label="负责人" >
            <el-select
                v-model="activeModel.ownerId"
                value-key="activeModel.ownerId"
                placeholder="请选择负责人"
                style="width:130px"
                @click="onGetOwners"
                clearable>
                <el-option
                    v-for="item in ownerOptions"
                    :key="item.ownerId"
                    :label="item.ownerName"
                    :value="item.ownerId"/>
            </el-select>
        </el-form-item>

        <el-form-item label="活动名称">
        <el-input v-model="activeModel.name" placeholder="请输入活动名称" clearable />
        </el-form-item>

        <el-form-item label="活动时间">
            <el-date-picker
                v-model="activeModel.activityTime"
                type="datetimerange"
                start-placeholder="开始时间"
                end-placeholder="结束时间"
                format="YYYY-MM-DD HH:mm:ss"
                value-format="YYYY-MM-DD HH:mm:ss"
            />
        </el-form-item>

        <el-form-item  prop="cost" label="活动预算">
            <el-input v-model="activeModel.cost" placeholder="请输入活动预算" clearable/>
        </el-form-item>

        <el-form-item label="创建时间">
            <el-date-picker
            v-model="activeModel.createTime"
            type="datetime"
            placeholder="请选择创建时间"
            value-format="YYYY-MM-DD HH:mm:ss"
            />
        </el-form-item>

        <el-form-item>
        <el-button type="primary" @click="getPageData(this.current_Page)">查询</el-button>
        <el-button type="primary" @click="onReset">重置</el-button>
        </el-form-item>
    </el-form>
    <el-button type="success" @click="dialogVisible = true; this.AddActivity = {}">录入市场活动</el-button>
    <el-button type="danger" @Click="batchDel">批量删除</el-button>
    <!-- 列表数据 -->
    <el-table
    :data="activeList"
    style="width: 100%"
    @selection-change="handleSelectionChange"
   >
    <el-table-column type="selection" />
    <el-table-column type="index" property="id" label="序号"  />
    <el-table-column property="ownerName" label="负责人"   width="80"/>
    <el-table-column property="name" label="活动名称" width="120" show-overflow-tooltip />
    <el-table-column property="startTime" label="开始时间" width="170" show-overflow-tooltip />
    <el-table-column property="endTime" label="结束时间" width="170" show-overflow-tooltip />
    <el-table-column property="cost" label="活动预算" width="170" show-overflow-tooltip />
    <el-table-column property="createTime" label="创建时间" width="170" show-overflow-tooltip />

    <el-table-column label="操作" style="width: 100px;">
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
    
    <!-- 新增活动对话框 -->
    <el-dialog
    v-model="dialogVisible"
    :title="AddActivity.id > 0 ? '编辑活动' : '新增活动'" center
    align-center
    width="800"
    draggable
    >
    <!-- :before-close="handleClose" -->

    <template #footer>
        <!--添加验证 1.el-form 加个v-bind:rules="loginRules"属性绑定对应的规则对象 ref="loginRefForm"拿到form的属性-->
        <el-form ref="ActivityReform" :model="AddActivity" 
            label-width="120px"  v-bind:rules="activityAddOrEditRules">
            <!-- 2.在form item 中添加prop属性 指向要验证的字段 -->
            <el-form-item label="负责人" prop="ownerId">
                <el-select
                    v-model="AddActivity.ownerId"
                    value-key="AddActivity.ownerId"
                    placeholder="请选择负责人"
                    @click="onGetOwners"
                    clearable>
                    <el-option
                        v-for="item in ownerOptions"
                        :key="item.ownerId"
                        :label="item.ownerName"
                        :value="item.ownerId"/>
                </el-select>
            </el-form-item>
            <el-form-item label="活动名称" prop="name">
                <el-input v-model="AddActivity.name" placeholder="请输入活动名称"/>
            </el-form-item>
            <el-form-item label="开始时间" prop="startTime">
                <el-date-picker
                v-model="AddActivity.startTime"
                type="datetime"
                placeholder="请选择活动开始时间"
                value-format="YYYY-MM-DD HH:mm:ss"
                />
            </el-form-item>
            <el-form-item label="结束时间" prop="endTime">
                <el-date-picker
                v-model="AddActivity.endTime"
                type="datetime"
                placeholder="请选择活动结束时间"
                value-format="YYYY-MM-DD HH:mm:ss"
                />
            </el-form-item>
           
            <el-form-item label="活动预算" prop="cost" >
                <el-input v-model="AddActivity.cost" placeholder="请输入活动预算"/>
            </el-form-item>
            <el-form-item label="活动描述" prop="description">
                <el-input
                    v-model="AddActivity.description"
                    :rows="6"
                    type="textarea"
                    placeholder="请输入描述内容"
                />
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
import {doGet, doPost, doPut, doDelete} from '../http/httpRequest';
import {messageTop, messageConfirm} from '../util/util';
export default{
    data(){
        return{
            //存储下拉列表负责人数据
            ownerOptions:[{}],
            //行内搜索对应model
            activeModel: {},
            //存放查询后的数据作为表格展示
            activeList: [{}],
            //一页多少条数据
            pageSize: 0,
            //总条数
            total: 0,
            //当前页
            current_Page: 1,
            //控制开关新增用户对话框
            dialogVisible: false,
            //存储提交对象
            AddActivity: {},
            //存储被勾选删除的数据
            delList: [],
            //添加或修改活动验证
            activityAddOrEditRules:{
                ownerId: [
                    {required:true,message:"负责人不能为空",trigger: 'blur' },
                ],
                name : [
                    { required: true, message: '活动名称不能为空', trigger: 'blur' }
                ],
                startTime : [
                    { required: true, message: '活动开始时间不能为空', trigger: 'blur' },
                ],
                endTime : [
                    { required: true, message: '活动结束时间不能为空', trigger: 'blur' },
                ],
                cost : [
                    { required: true, message: '活动预算不能为空', trigger: 'blur' },
                    {pattern:/^[0-9]+(\.[0-9]{2})?$/, message: '活动预算必须是整数或者两位小数', trigger: 'blur'},
                ],
                description : [
                    { required: true, message: '活动描述不能为空', trigger: 'blur' },
                    { min : 5, max : 255, message : '活动描述不能必须在5-255字符之间', trigger : 'blur'},
                ],
            },
            //行内搜索验证
            activityRules:{
                cost:[
                    {pattern:/^[0-9]+(\.[0-9]{2})?$/, message: '活动预算必须是整数或者两位小数', trigger: 'blur'}
                ]
            }
            
        }
    },
    //父组件提供的重新加载路由页面方法
    inject:['reload'],
    mounted(){
        //获取父组件的页数数据
        // console.log(this.pageIndex);
        // this.current_Page = this.pageIndex;
        //获取用户信息页
        this.getPageData(this.current_Page);
        this.onGetOwners();
    },
    methods:{
        //条件查询
        onReset:function(){
            this.activeModel = {};
        },
        //获取活动负责人列表
        onGetOwners:function(){
            doGet("/ActivityMag/Activity/GetOwners",{}).then((resp)=>{
                if(resp.data.code === 200){
                    this.ownerOptions = resp.data.data;
                }else{
                    messageTop("请先添加数据","warning");
                }
            });
        },
        //获取数据并分页 包括条件查询
        getPageData:function(pageIndex){
            let startTime = '';
            let endTime = '';
            for(let key in this.activeModel.activityTime){
                if(key == '0'){
                    startTime = this.activeModel.activityTime[key];
                }else if(key == '1'){
                    endTime = this.activeModel.activityTime[key];
                }
            }
            let request = {
                current: pageIndex,
                startTime: startTime,
                endTime: endTime,
                name: this.activeModel.name == null ? "" : this.activeModel.name,
                cost: this.activeModel.cost  == null ? "" : this.activeModel.cost,
                ownerId: this.activeModel.ownerId  == null ? "" : this.activeModel.ownerId,
                createTime: this.activeModel.createTime == null ? "" : this.activeModel.createTime,
            }
            doGet('/ActivityMag/Activity/ConditionQuery',request).then((resp)=>{
                //console.log(resp);
                if(resp.data.code === 200){
                    this.activeList = resp.data.data;
                    this.pageSize = resp.data.pageSize;
                    this.total = resp.data.total;
                }else{
                    this.activeList = {};
                    this.pageSize = resp.data.pageSize;
                    this.total = resp.data.total;
                    messageTop(resp.data.msg, "warning");
                }
            })
        },
        //选择框被勾选
        handleSelectionChange: function(e){
            //没次触发前清空 每次触发时会携带所有被勾选的数据
            this.delList = [];
            //将数据循环遍历存储到集合中  数组对象遍历
            e.forEach(data => {
                let userId = data.id;
                this.delList.push(userId);
            })
        },
        //点击分页栏调用
        toPage: function(currentPage){
            //每次点击后记录当前页
            this.current_Page = currentPage;
            //console.log(this.current_Page);
            //获取数据
            this.getPageData(currentPage);
        },
        //用户详情页
        detail: function(id){
            //跳转到/dashboard/user/ (id)路由上
            this.$router.push('/dashboard/activityDetail/' + id);
        },
        //提交活动信息表单
        submitUserInfo: function(){
            let formData = new FormData();
            for (let field in this.AddActivity) {
                if(field == "loginPwd" && this.AddActivity[field] == ""){
                    formData.append(field, "");
                }else{
                    formData.append(field, this.AddActivity[field]);
                }
            }
            //验证表单合法性   
            this.$refs.ActivityReform.validate((isValid)=>{
                if(isValid){
                    if(this.AddActivity.id > 0){ //编辑时
                        doPut('/ActivityMag/Activity', formData, 1).then( (resp) =>{
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
                        doPost('/ActivityMag/Activity', formData, 1).then( (resp) =>{
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
        //编辑活动
        edit: function(id){
            this.loadActivityInfo(id);
            //打开对话框
            this.dialogVisible = true;
        },
        //加载活动信息 用来编辑
        loadActivityInfo: function(id){
            doGet('/ActivityMag/Activity/ActInfo/' + id, {}).then((resp)=>{
                if(resp.data.code === 200){
                    this.AddActivity = resp.data.data;
                }else{
                    this.messageTop("获取信息失败", "error");
                }
            });
        },
        //删除用户
        del: function(id){
            messageConfirm("是否确定删除", "系统提醒").then(()=>{
                doDelete('ActivityMag/Activity/' + id, {}).then((resp)=>{
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