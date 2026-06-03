<template>
    <!-- 按钮 -->
    <el-button  type="primary" @click="InputClue">录入线索</el-button>
    <el-button  type="success" @click="dialogVisible = true">导入线索(Excel)</el-button>
    <el-button  type="danger" @click="">批量删除</el-button>

    <!-- 列表数据 -->
    <el-table
    :data="clueList"
    style="width: 100%"
    @selection-change="handleSelectionChange"
   >
    <el-table-column type="selection" />
    <el-table-column type="index" property="id" label="序号" width="56px"  />
    <el-table-column property="owner" label="负责人"   width="75px"/>
    <el-table-column property="activity" label="所属活动" width="86px" show-overflow-tooltip />
    <el-table-column property="fullName" label="姓名" width="75px" show-overflow-tooltip />
    <el-table-column property="appellation" label="称呼" width="56px" show-overflow-tooltip />
    <el-table-column property="phone" label="手机" width="118px" show-overflow-tooltip />
    <el-table-column property="weixin" label="微信" width="118px" show-overflow-tooltip />
    <el-table-column property="needLoan" label="是否贷款" width="88px" show-overflow-tooltip />
    <el-table-column property="intentionState" label="意向状态" width="86px" show-overflow-tooltip />
    <el-table-column property="intentionProduct" label="意向产品" width="80px" show-overflow-tooltip />
    <el-table-column property="state" label="线索状态" width="86px" show-overflow-tooltip />
    <el-table-column property="source" label="线索来源" width="86px" show-overflow-tooltip />
    <el-table-column property="nextContactTime" label="下次联系时间" width="120px" show-overflow-tooltip />

    <el-table-column label="操作" style="width: 140px;" >
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

     <!-- 录入线索（Excel）对话框 -->
    <el-dialog
    v-model="dialogVisible"
    title="录入线索(Excel)" center
    align-center
    width="800"
    draggable
    >
    <!-- 文件上传    :http-request是对应上传的方法，会注入上传的参数进去，多个文件注入多个（不是数组，是一个一个排队上传，
                        之前学生管理系统是html加javascript是将获取上传文件返回的数组后一起上传的）
                    :auto-upload=false是关闭自动上传，才用手动
                    :limit=5是限制一次最多上传五个文件
                    :multiple=true是一次可以选择多个文件-->
    <el-upload
            ref="uploadRef"
            methods="post"
            :auto-upload="false" 
            :http-request="uploadFile"
            :limit = "5"
            :multiple="true"
        >
            <!-- <template #trigger> 插槽  -->
            <template #trigger>
                <el-button type="primary">选择Excel文件</el-button>
            </template>
            仅支持后缀名为.xls或.xlsx的文件

            <template #tip>
                <div class="el-upload__tip" >
                    <h3>重要提示：</h3>
                    <ul style="font-size: 14px;">
                        <li>上传仅支持后缀名为.xls或.xlsx的文件；</li>
                        <li>给定Excel文件的第一行将视为字段名；</li>
                        <li>请确认您的文件大小不超过50MB；</li>
                        <li>日期值以文本形式保存，必须符合yyyy-MM-dd格式；</li>
                        <li>日期时间以文本形式保存，必须符合yyyy-MM-dd HH:mm:ss的格式；</li>
                    </ul>
                </div>
            </template>
        </el-upload>
    <!-- <template #footer> 插槽  -->
    <template #footer>
        <div class="fileTip">
            <el-button @click="dialogVisible = false">关 闭</el-button>
            <el-button type="primary" @click="submitExcel">导 入</el-button>
        </div>
    </template>
  </el-dialog>
</template>

<script>
import {doGet, doPost} from '../http/httpRequest'
import {messageTop} from '../util/util'
export default{
    data(){
        return{
            currentPage: 1,
            pageSize: 0,
            clueList: [],
            total: 0,
            dialogVisible: false,
            isCreateOrEdit: false,
            
        }
    },
    mounted(){
        this.GetClueList(this.currentPage);
    },
    methods:{
        //跳转分页
        toPage:function(current){
            this.currentPage = current;
            this.GetClueList(current);
        },
        //选择框选中
        handleSelectionChange: function(){

        },
        //线索列表
        GetClueList: function(crtPage){
            doGet('/Clue/Clue/' + crtPage, {}).then((resp) => {
                if(resp.data.code === 200){
                    this.clueList = resp.data.data;
                    this.total = resp.data.total;
                    this.pageSize = resp.data.pageSize;
                }else{
                    messageTop("获取数据失败",'error');
                }
            });
        },
        //提交上传Excel文件
        submitExcel: function(){
            this.$refs.uploadRef.submit();
        },
        //uploadFile 处理上传 上传文件的请求
        uploadFile: function(parameter){
            console.log(parameter);
            
            let fileOjb = parameter.file; //相当于input里取得的files
            console.log(fileOjb);
            let formData = new FormData(); // new一个FormData对象
            formData.append('formFile', fileOjb); //文件对象，前面file是参数名，后面fileObj是参数值
            doPost('/Clue/Clue', formData).then((resp) =>{
                messageTop(resp.data.msg, 'success');
                this.$refs.uploadRef.clearFiles();
            });
        },
        //详情
        InputClue: function(id){
            this.$router.push('/dashboard/clue/' + id);
        },
        //编辑
        edit: function(id){
            this.$router.push('/dashboard/clue/' + id);
        },
        del:function(id){
            
        },
        detail:function(id){
            this.$router.push('/dashboard/detail/' + id);
        }

    },
}
</script>

<style>
.el-table{
    margin-top: 12px;
}
.el-pagination{
    padding-top: 12px;
}
.fileTip {
  padding-top: 15px;
}
</style>
