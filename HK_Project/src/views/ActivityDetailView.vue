<template>
    <!-- 活动详情表单 -->
    <el-form :model="activityDetail" label-width="120px"  ref="RemakeRfrom"
        v-bind:rules="RemakeRules">
        <el-form-item label="ID">
        <div class="detail">&nbsp;{{activityDetail.id}}</div>
        </el-form-item>

        <el-form-item label="负责人">
        <div class="detail">&nbsp;{{activityDetail.ownerName}}</div>
        </el-form-item>

        <el-form-item label="活动名称">
        <div class="detail">&nbsp;{{activityDetail.name}}</div>
        </el-form-item>

        <el-form-item label="开始时间">
        <div class="detail">&nbsp;{{activityDetail.startTime}}</div>
        </el-form-item>

        <el-form-item label="结束时间">
        <div class="detail">&nbsp;{{activityDetail.endTime}}</div>
        </el-form-item>

        <el-form-item label="活动预算">
        <div class="detail">&nbsp;{{activityDetail.cost}}</div>
        </el-form-item>

        <el-form-item label="活动描述">
        <div class="detail">&nbsp;{{activityDetail.description}}</div>
        </el-form-item>

        <el-form-item label="创建时间">
        <div class="detail">&nbsp;{{activityDetail.createTime}}</div>
        </el-form-item>

        <el-form-item label="创建人">
        <div class="detail">&nbsp;{{activityDetail.createName}}</div>
        </el-form-item>

        <el-form-item label="编辑时间">
        <div class="detail">&nbsp;{{activityDetail.editTime}}</div>
        </el-form-item>

        <el-form-item label="编辑人">
        <div class="detail">&nbsp;{{activityDetail.editName}}</div>
        </el-form-item>

        <el-form-item label="填写备注" prop="noteContent">
            <el-input
                v-model="activityDetail.noteContent"
                :rows="6"
                type="textarea"
                placeholder="请输入描述内容"
            />
        </el-form-item>
        <el-form-item>
            <el-button  @click="goBack">返 回</el-button>
            <el-button type="primary" @click="onSubmit">提 交</el-button>
        </el-form-item>
    </el-form>  

    <!-- 活动描述列表 -->
    <el-table
    :data="RemarkList"
    style="width: 100%"
   >
    <el-table-column type="index" property="id" label="序号"  />
    <el-table-column property="noteContent" label="备注内容"   show-overflow-tooltip/>
    <el-table-column property="createTime" label="备注时间"  show-overflow-tooltip />
    <el-table-column property="createByName" label="备注人"  show-overflow-tooltip />
    <el-table-column property="editTime" label="编辑时间" show-overflow-tooltip />
    <el-table-column property="editByName" label="编辑人"  show-overflow-tooltip />
    <el-table-column label="操作" style="width: 80px;">
        <template #default="scope">
            <el-button size="small" type="success" @click="edit(scope.row.id)">编辑</el-button>
            <el-button size="small" type="danger" @click="del(scope.row.id)">删除</el-button>
             <!-- href="javascript:使a标签失效   -->
             <!-- <a href="javascript:" @click="某个事件">编 辑</a>
             &nbsp;
             <a href="javascript:" @click="某个事件">删 除</a> -->
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

    <!-- 修改备注对话框 -->
    <el-dialog
    v-model="dialogVisible"
    title="编辑备注内容" center
    align-center
    width="800"
    draggable
    >
    <template #footer>
        <el-form ref="RemakeEditRfrom" :model="EditNoteContent" 
            label-width="120px"  v-bind:rules="RemakeRules">
            <el-form-item label="填写备注信息" prop="noteContent">
            <el-input
                v-model="EditNoteContent.noteContent"
                :rows="6"
                type="textarea"
                placeholder="请输入备注内容"
            />
        </el-form-item>
        </el-form>  
        <div class="dialog-footer">
            <el-button @click="dialogVisible = false">关闭</el-button>
            <el-button type="primary" @click="onEditSubmit">提交</el-button>
        </div>
    </template>
  </el-dialog>
</template>

<script>
import {goBack, messageTop} from '../util/util'
import {doGet, doPut, doPost} from '../http/httpRequest'
export default{
    data(){
        return{
            activityDetail: {
            },
            //表单校验 备注
            RemakeRules: {
                noteContent: [
                    {required: true, message: "备注不能为空", trigger: 'blur'},
                    {min : 5, max : 255, message : '活动描述不能必须在5-255字符之间', trigger : 'blur'},
                ],
            },
            //存储活动备注
            RemarkList:[],
            //当前页 默认1
            pageSize: 10,
            //总页数
            total: 0,
            //当前页
            currentPage: 1,
            //存放获取用来修改的备注
            EditNoteContent: {},
            //对话框 默认关闭 false
            dialogVisible: false,
        }
    },
    inject:['reload'],
    mounted(){
        this.getDetail();
        this.getPageData(this.currentPage);
    },
    methods:{
        //声明导入的goBack函数
        goBack,
        //获取详情信息
        getDetail: function(){
            let id = this.$route.params.id;
            doGet('/ActivityMag/Activity/Detail/' + id,{}).then((resp)=>{
                if(resp.data.code === 200){
                    this.activityDetail = resp.data.data;
                }else{
                    messageTop('获取数据失败', 'error');
                }
            });
        },
        //提交表单 (备注)
        onSubmit:function(){
            //验证表单合法性   
            this.$refs.RemakeRfrom.validate((isValid)=>{
                if(isValid){
                    doPost('/ActivityRemake/Remake', {
                        "id": this.activityDetail.id,
                        "noteContent": this.activityDetail.noteContent
                    }).then((resp)=>{
                        if(resp.data.code === 200){
                            messageTop("提交备注成功", "success");
                            this.dialogVisible = false;
                            this.reload();
                        }
                    });
                }
            });
        },
        //提交表单 修改备注
        onEditSubmit:function(){
            this.$refs.RemakeEditRfrom.validate((isValid) =>{
                doPut('/ActivityRemake/Remake', {
                    "id": this.EditNoteContent.id,
                    "noteContent" : this.EditNoteContent.noteContent,
                }).then((resp) => {
                    if(resp.data.code === 200){
                        messageTop("修改成功", 'success');
                        this.dialogVisible = false;
                        this.reload();
                    }
                });
            });
        },
         //点击分页栏调用
        toPage: function(current_Page){
            //每次点击后记录当前页
            this.currentPage = current_Page;
            //console.log(this.current_Page);
            //获取数据
            this.getPageData();
        },
        //编辑
        edit:function(id){
            this.dialogVisible = true;
            doGet('ActivityRemake/Remake/' + id,{}).then((resp)=>{
                if(resp.data.code === 200){
                    this.EditNoteContent = resp.data.data;
                }
            });
        },
        //删除
        del:function(id){
            doPut('ActivityRemake/Remake/' + id,{}).then((resp) => {
                if(resp.data.code === 200){
                    messageTop("删除成功", "success");
                    this.reload();
                }
            });
        },
        //获取备注信息列表分页
        getPageData: function(){
            let id = this.$route.params.id;
            doGet('/ActivityRemake/Remake/' + id + "/" + this.currentPage,{}).then((resp) =>{
                if(resp.data.code === 200){
                    this.RemarkList = resp.data.data;
                    this.pageSize = resp.pageSize;
                    this.total = resp.data.total;
                }
            });
        }
    }
}
    
</script>
<style scoped>
.detail {
  background-color: #edf5f5;
  width: 100%;
  padding-left: 15px;
}
</style>