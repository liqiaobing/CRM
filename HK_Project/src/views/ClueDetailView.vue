<template>
      <el-form ref="ClueRefForm" :model="ClueModel" 
            label-width="120px"  v-bind:rules="ClueModelRules">
            <el-form-item label="负责人" >
                <div class="detail">&nbsp;{{ClueModel.owner}}</div>
            </el-form-item>
            <el-form-item label="所属活动" >
                <div class="detail">&nbsp;{{ClueModel.activity}}</div>
            </el-form-item>
            <el-form-item label="姓名" >
                <div class="detail">&nbsp;{{ClueModel.fullName}}</div>
            </el-form-item>
            <el-form-item label="称呼" >
                <div class="detail">&nbsp;{{ClueModel.appellation}}</div>
            </el-form-item>

            <el-form-item label="手机" >
                <div class="detail">&nbsp;{{ClueModel.phone}}</div>
            </el-form-item>
            
            <el-form-item label="微信" >
                <div class="detail">&nbsp;{{ClueModel.weixin}}</div>
            </el-form-item>
            <el-form-item label="QQ" >
                <div class="detail">&nbsp;{{ClueModel.qq}}</div>
            </el-form-item>
            <el-form-item label="邮箱" >
                <div class="detail">&nbsp;{{ClueModel.email}}</div>
            </el-form-item>  
            <el-form-item label="年龄" >
                <div class="detail">&nbsp;{{ClueModel.age}}</div>
            </el-form-item>
            <el-form-item label="职业" >
                <div class="detail">&nbsp;{{ClueModel.job}}</div>
            </el-form-item>
            <el-form-item label="年收入" >
                <div class="detail">&nbsp;{{ClueModel.yearIncome}}</div>
            </el-form-item>
            <el-form-item label="住址" >
                <div class="detail">&nbsp;{{ClueModel.address}}</div>
            </el-form-item>
            <el-form-item label="贷款" >
                <div class="detail">&nbsp;{{ClueModel.needLoan}}</div>
            </el-form-item>
            <el-form-item label="意向状态" >
                <div class="detail">&nbsp;{{ClueModel.intentionState}}</div>
            </el-form-item>
            <el-form-item label="意向产品" >
                <div class="detail">&nbsp;{{ClueModel.intentionProduct}}</div>
            </el-form-item>
            <el-form-item label="线索状态" >
                <div class="detail">&nbsp;{{ClueModel.state}}</div>
            </el-form-item>
            <el-form-item label="线索来源" >
                <div class="detail">&nbsp;{{ClueModel.source}}</div>
            </el-form-item>
            <el-form-item label="线索描述" >
                <div class="detail">&nbsp;{{ClueModel.description}}</div>
            </el-form-item>
            <el-form-item label="下次联系时间">
                <div class="detail">&nbsp;{{ClueModel.nextContactTime}}</div>
            </el-form-item>

            <el-form-item label="填写跟踪记录" prop="noteContent">
                <el-input
                    v-model="ClueModel.noteContent"
                    :rows="6"
                    type="textarea"
                    placeholder="请输入备注内容"
                />
            </el-form-item>
            <el-form-item label="跟踪方式" prop="noteWay">
                <el-select v-model="ClueModel.noteWay" placeholder="请选择"  @click="GetSelectedKeyValue('noteWay')">
                    <el-option
                        v-for="item in Options"
                        :key="item.id"
                        :label="item.typeValue"
                        :value="item.id"
                    />
                </el-select>
            </el-form-item>

            <el-form-item>
                <el-button type="primary" @click="submit">提 交</el-button>
                <el-button type="success" @click="dialogVisible = true" v-if="ClueModel.stateId != -1">转为客户</el-button>
                <el-button type="primary" @click="goBack">返 回</el-button>
            </el-form-item>
        </el-form>  

         <!-- 活动描述列表 -->
        <el-table
        :data="ClueContentList"
        style="width: 100%"
        >
            <el-table-column type="index" property="id" label="序号"  />
            <el-table-column property="noteWay" label="跟踪方式"   show-overflow-tooltip/>
            <el-table-column property="noteContent" label="跟踪内容"  show-overflow-tooltip />
            <el-table-column property="createTime" label="跟踪时间"  show-overflow-tooltip />
            <el-table-column property="createBy" label="跟踪人" show-overflow-tooltip />
            <el-table-column property="editTime" label="编辑时间"  show-overflow-tooltip />
            <el-table-column property="editBy" label="编辑人"  show-overflow-tooltip />
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

        <!-- 编辑对话框 -->
        <el-dialog
            v-model="dialogVisible_Edit"
            title="编辑跟踪内容" center
            align-center
            width="800"
            draggable>
            <template #footer>
                <el-form ref="ClueEditRefForm" :model="ClueEditModel" 
                    label-width="120px"  v-bind:rules="ClueModelRules">
                <el-form-item label="填写跟踪记录" prop="noteContent">
                    <el-input
                        v-model="ClueEditModel.noteContent"
                        :rows="6"
                        type="textarea"
                        placeholder="请输入备注内容"
                    />
                </el-form-item>
                <el-form-item label="跟踪方式" prop="noteWay">
                    <el-select v-model="ClueEditModel.noteWay" placeholder="请选择" >
                        <el-option
                            v-for="item in Options"
                            :key="item.id"
                            :label="item.typeValue"
                            :value="item.id"
                        />
                    </el-select>
                </el-form-item>
                </el-form>  
                <div class="dialog-footer">
                <el-button @click="dialogVisible_Edit = false">关闭</el-button>
                <el-button type="primary" @click="EditSubmit">提交</el-button>
                </div>
            </template>
        </el-dialog>

        <!-- 转为客户 提交信息对话框 -->
        <el-dialog
        v-model="dialogVisible"
        title="转为客户" center
        align-center
        width="800"
        draggable
        >
            <template #footer>
                <el-form ref="ToCustomReform" :model="ToCustomModel" 
                    label-width="120px"  v-bind:rules="ToCustomRules">

                    <el-form-item label="意向产品" prop="product">
                        <el-select v-model="ToCustomModel.product" placeholder="请选择" 
                            @click="GetSelectedKeyValue('Product')">
                            <el-option
                                v-for="item in productOptions"
                                :key="item.id"
                                :label="item.typeValue"
                                :value="item.id"
                            />
                        </el-select>
                    </el-form-item>
                    <el-form-item label="客户描述" prop="description">
                        <el-input
                            v-model="ToCustomModel.description"
                            :rows="6"
                            type="textarea"
                            placeholder="请输入描述内容"
                        />
                    </el-form-item>
                    <el-form-item label="下次联系时间" prop="nextContactTime">
                        <el-date-picker
                        v-model="ToCustomModel.nextContactTime "
                        type="datetime"
                        placeholder="请选择下次联系时间"
                        value-format="YYYY-MM-DD HH:mm:ss"
                        />
                    </el-form-item>
                </el-form>  

                <div class="dialog-footer">
                <el-button @click="dialogVisible = false">关闭</el-button>
                <el-button type="primary" @click="ToCustomSubmit">提交</el-button>
                </div>
            </template>
        </el-dialog>
</template>

<script>
import { goBack, messageTop } from '@/util/util';
import {doDelete, doGet, doPost, doPut} from '../http/httpRequest'
export default{
    data(){
        return{
            //表单
            ClueModel:{},
            Options: [],
            ClueModelRules: {
                noteContent:[
                    { min : 5, max : 255, message : '活动描述不能必须在5-255字符之间', trigger : 'blur'},
                ],
                noteWay:[
                    
                ],
            },
            dialogVisible: false,
            //表单
            ToCustomModel: {},
            productOptions: [],
            ClueContentList:[],
            ToCustomRules: {
                product:[
                    {required: true, message: "意向产品必须选择", trigger : 'blur'},
                ],
                nextContactTime: [
                    {required: true, message: "下次联系时间必须填写", trigger : 'blur'},
                ],
                description: [
                    {required: true, message: "客户描述不能为空", trigger : 'blur'},
                    { min : 5, max : 255, message : '客户描述不能必须在5-255字符之间', trigger : 'blur'},
                ]
            },
            pageSize:0,
            total:0,
            currentPage:1,
            dialogVisible_Edit:false,
            ClueEditModel:{},
            
        }
    },
    inject:['reload'],
    mounted(){
        this.getClueData();
        this.getPageData(this.currentPage);
    },
    methods:{
        goBack,
        //提交表单数据
        submit: function(){
            this.$refs.ClueRefForm.validate((isValid)=>{
                if(isValid){
                    let id = this.$route.params.id;
                    //console.log(id);
                    doPost('/Clue/ClueRemark', {
                        "noteContent" : this.ClueModel.noteContent,
                        "noteWay" : this.ClueModel.noteWay,
                        "clueId" : id,
                    }).then((resp)=>{
                        if(resp.data.code === 200){
                            messageTop("提交成功", 'success');
                            this.reload();
                        }
                    });
                }
            });
        },
        //获取noteWay下拉框数据
        GetSelectedKeyValue: function(type_code){
            doGet('/Clue/Detail/GetSelected/' + type_code,{}).then((resp) => {
                if("noteWay" === type_code){
                    this.Options = resp.data.data;
                }else if('Product' == type_code){
                    this.productOptions = resp.data.data;
                }
            });
        },
        //获取详情
        getClueData: function(){
            let id = this.$route.params.id;
            doGet('Clue/Detail/' + id,{}).then((resp) => {
                if(resp.data.code === 200){
                    let data = resp.data.data;
                    //console.log(data[0]);
                    this.ClueModel = data[0];
                }
            });
        },
        //转为客户提交表单
        ToCustomSubmit: function(){
            // this.$refs.ClueRefForm.validate((isValid)=>{
            //     if(isValid){
            this.$refs.ToCustomReform.validate((isValid) => {
                if(isValid){
                    let id = this.$route.params.id;
                    doPost('/Customer/Custom', {
                        "clueId" : id,
                        "product" : this.ToCustomModel.product,
                        "description" : this.ToCustomModel.description,
                        "nextContactTime" : this.ToCustomModel.nextContactTime,
                    }).then((resp) => {
                        if(resp.data.code === 200){
                            messageTop('转客户成功', 'success')
                            this.reload();
                        }
                    });
                }
            });
        },
        //点击分页栏调用
        toPage: function(current_Page){
            //每次点击后记录当前页
            this.currentPage = current_Page;
            console.log( this.currentPage);
            //获取数据
            this.getPageData(current_Page);
        },
        //获取clueRemark列表数据
        getPageData: function(current_Page){
            let id = this.$route.params.id;
            doGet('/Clue/ClueRemark/' + id + "/" + current_Page,{}).then((resp) =>{
                if(resp.data.code === 200){
                    this.ClueContentList = resp.data.data;
                    this.pageSize = resp.data.pageSize;
                    this.total = resp.data.total;
                }
            });
        },
        //编辑
        EditSubmit:function(){
            doPut('/Clue/ClueRemark/',{
                "id" : this.ClueEditModel.id,
                "noteContent" : this.ClueEditModel.noteContent,
                "noteWay" : this.ClueEditModel.noteWay,
            }).then((resp) =>{
                if(resp.data.code === 200){
                    messageTop('编辑成功', 'success');
                    this.reload();
                    this.dialogVisible_Edit = false;
                }
            });
        },
        edit:function(id){
            console.log(id);
            this.GetSelectedKeyValue('noteWay');
            this.dialogVisible_Edit = true;
            doGet('/Clue/ClueRemarkEdit/' + id ,{}).then((resp) =>{
                if(resp.data.code === 200){
                    this.ClueEditModel = resp.data.data;
                    //console.log(this.ClueEditModel);
                }
            });
        },
        //删除
        del:function(id){
            doPut('/Clue/ClueRemark/' + id,{}).then((resp) =>{
                if(resp.data.code === 200){
                    messageTop('删除成功', 'success');
                    this.reload();
                }
            });
        }
    },
}
</script>

<style scoped>
.el-pagination{
    padding-top: 12px;
}
</style>