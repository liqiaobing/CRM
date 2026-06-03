<template>
      <el-form ref="ClueRefForm" :model="ClueModel" 
            label-width="120px"  v-bind:rules="ClueModelRules">
            <el-form-item label="负责人" prop="ownerId">
                <el-select v-model="ClueModel.ownerId" placeholder="请选择" disabled>
                    <el-option
                        v-for="item in OwnerOptions"
                        :key="item.id"
                        :label="item.name"
                        :value="item.id"
                    />
                </el-select>
            </el-form-item>
            <el-form-item label="所属活动" prop="activityId">
                <el-select v-model="ClueModel.activityId" placeholder="请选择" >
                    <el-option
                        v-for="item in activityIdOptions"
                        :key="item.id"
                        :label="item.typeValue"
                        :value="item.id"
                        
                    />
                </el-select>
            </el-form-item>
            <el-form-item label="姓名" prop="fullName">
                <el-input v-model="ClueModel.fullName" />
            </el-form-item>
            <el-form-item label="称呼" prop="appellation">
                <el-select v-model="ClueModel.appellation" placeholder="请选择" >
                    <el-option
                        v-for="item in appellationOptions"
                        :key="item.id"
                        :label="item.typeValue"
                        :value="item.id"
                        
                    />
                </el-select>
            </el-form-item>

            <!-- 只读 -->
            <el-form-item label="手机" v-if="ClueModel.id > 0">
                <el-input v-model="ClueModel.phone" disabled/>
            </el-form-item>
            <el-form-item label="手机" prop="phone" v-else>
                <el-input v-model="ClueModel.phone" />
            </el-form-item>
            
            <el-form-item label="微信" prop="weixin">
                <el-input  v-model="ClueModel.weixin" />
            </el-form-item>
            <el-form-item label="qq" prop="qq">
                <el-input v-model="ClueModel.qq" />
            </el-form-item>
            <el-form-item label="邮箱" prop="email">
                <el-input v-model="ClueModel.email" />
            </el-form-item>  
            <el-form-item label="年龄" prop="age">
                <el-input v-model="ClueModel.age" />
            </el-form-item>
            <el-form-item label="职业" prop="job">
                <el-input v-model="ClueModel.job" />
            </el-form-item>
            <el-form-item label="年收入" prop="yearIncome">
                <el-input v-model="ClueModel.yearIncome" />
            </el-form-item>
            <el-form-item label="住址" prop="address">
                <el-input v-model="ClueModel.address" />
            </el-form-item>
            <el-form-item label="贷款" prop="needLoan">
                <el-select v-model="ClueModel.needLoan" placeholder="请选择" >
                    <el-option
                        v-for="item in needLoanOptions"
                        :key="item.id"
                        :label="item.typeValue"
                        :value="item.id"
                    />
                </el-select>
            </el-form-item>
            <el-form-item label="意向状态" prop="intentionState">
                <el-select v-model="ClueModel.intentionState" placeholder="请选择" >
                    <el-option
                        v-for="item in intentionstateOptions"
                        :key="item.id"
                        :label="item.typeValue"
                        :value="item.id"
                    />
                </el-select>
            </el-form-item>
            <el-form-item label="意向产品" prop="intentionProduct">
                <el-select v-model="ClueModel.intentionProduct" placeholder="请选择" >
                    <el-option
                        v-for="item in intentionProductOptions"
                        :key="item.id"
                        :label="item.typeValue"
                        :value="item.id"
                    />
                </el-select>
            </el-form-item>
            <el-form-item label="线索状态" prop="state">
                <el-select v-model="ClueModel.state" placeholder="请选择" >
                    <el-option
                        v-for="item in stateOptions"
                        :key="item.id"
                        :label="item.typeValue"
                        :value="item.id"
                    />
                </el-select>
            </el-form-item>
            <el-form-item label="线索来源" prop="source">
                <el-select v-model="ClueModel.source" placeholder="请选择" >
                    <el-option
                        v-for="item in sourceOptions"
                        :key="item.id"
                        :label="item.typeValue"
                        :value="item.id"
                    />
                </el-select>
            </el-form-item>
            <el-form-item label="线索描述" prop="description">
                <el-input
                    v-model="ClueModel.description"
                    :rows="6"
                    type="textarea"
                    placeholder="请输入备注内容"
                />
            </el-form-item>
            <el-form-item label="下次联系时间">
                <el-date-picker
                v-model="ClueModel.nextContactTime"
                type="datetime"
                placeholder="请选择下次联系时间"
                value-format="YYYY-MM-DD HH:mm:ss"
                />
            </el-form-item>
            <el-form-item>
                <el-button type="primary" @click="submit">提 交</el-button>
                <el-button type="primary" @click="goBack">返 回</el-button>
            </el-form-item>
        </el-form>  
</template>

<script>
import { goBack, messageTop } from '@/util/util';
import {doGet, doPost, doPut} from '../http/httpRequest'
export default{
    data(){
        return{
            ClueModel:{},
            OwnerOptions: {},
            activityIdOptions: {},
            intentionProductOptions: {},
            appellationOptions: {},
            needLoanOptions: {},
            intentionstateOptions: {},
            stateOptions: {},
            sourceOptions: {},
            isCreateOrEdit: false,
            ClueModelRules: {
                activityId:[
                    
                ],
                fullName:[
                    {required:true,message:"姓名不能为空",trigger: 'blur' },
                    {pattern: /^[\u4E00-\u9FA5]{1,5}$/,  message: '姓名必须是中文', trigger: 'blur'}
                ],
                appellation:[
                    
                ],
                phone:[
                    { required: true, message: '请输入手机号码', trigger: 'blur' },
                    { pattern : /^1[3-9]\d{9}$/, message: '手机号码格式有误', trigger: 'blur'},
                    { validator : this.checkphone, trigger: 'blur'}
                ],
                weixin:[
                    
                ],
                qq:[
                    { min : 5, message: 'qq号最低为五位', trigger: 'blur'},
                    { pattern : /^\d+$/, message: 'qq号必须为数字', trigger: 'blur'},
                ],
                email:[
                    { pattern : /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/, message: '邮箱格式有误', trigger: 'blur'}
                ],
                age:[
                    { pattern : /^\d+$/, message: '年龄必须为数字', trigger: 'blur'},
                ],
                job:[
                    
                ],
                yearIncome:[
                    {pattern:/^[0-9]+(\.[0-9]{2})?$/, message: '年收入整数或者两位小数', trigger: 'blur'}
                ],
                address:[
                    
                ],
                needLoan:[
                    
                ],
                intentionstate:[
                    
                ],
                intentionProduct:[
                    
                ],
                state:[
                    
                ],
                source:[
                    
                ],
                state:[
                    
                ],
                description:[
                    { min : 5, max : 255, message : '活动描述不能必须在5-255字符之间', trigger : 'blur'},
                ]
            }
        }
    },
    inject:['reload'],
    mounted(){
        this.GetSelectedKeyValue("appellation");
        this.GetSelectedKeyValue("needLoan");
        this.GetSelectedKeyValue("intentionstate");
        this.GetSelectedKeyValue("userstate");
        this.GetSelectedKeyValue("source");
        this.GetSelectedKeyValue("Activity");
        this.GetSelectedKeyValue("Product");

        this.LoadLoginUser();

        this.getClueData();
    },
    methods:{
        goBack,
        //获取dicValue中的数据 type_code对应 typecode表
        GetSelectedKeyValue: function(type_code){
            doGet('/Clue/Detail/GetSelected/' + type_code,{}).then((resp) => {
                if("appellation" === type_code){
                    this.appellationOptions = resp.data.data;
                }else if("needLoan" === type_code){
                    this.needLoanOptions = resp.data.data;
                }else if("intentionstate" === type_code){
                    this.intentionstateOptions = resp.data.data;
                }else if("userstate" === type_code){
                    this.stateOptions = resp.data.data;
                }else if("source" === type_code){
                    this.sourceOptions = resp.data.data;
                }else if("Activity" === type_code){
                    this.activityIdOptions = resp.data.data;
                }if("Product" === type_code){
                    this.intentionProductOptions = resp.data.data;
                }
            });
        },
        //加载登录用户信息方法
        LoadLoginUser:function(){
            doGet('/UserMag/Login/userInfo',{}).then( (resp)=>{
                let opt = [{
                    'id': resp.data.data.id,
                    'name': resp.data.data.name
                }]
                this.OwnerOptions = opt;
                this.ClueModel.ownerId = resp.data.data.id;
            });
        },
        //检查手机号是否被注册
        checkphone:function(rule, value, calback){
            let phone = value;
            if(phone){
                doGet('/Clue/Detail/Checkphone/' + phone,{}).then((resp)=>{
                    if(resp.data.code === 500){
                        //手机号已被注册
                        return calback(new Error('该手机号已被注册'));
                    }else{
                        return calback();//验证通过
                    }
                });
            }
            
        },
        //提交表单数据
        submit: function(){
            this.$refs.ClueRefForm.validate((isValid)=>{
                if(isValid){
                    //console.log("1111");
                    let formdata = new FormData();
                    for(let field in this.ClueModel){
                        formdata.append(field, this.ClueModel[field]);
                    }
                    if(this.ClueModel.id > 0){//编辑
                        doPut('/Clue/Clue', formdata).then((resp)=>{
                            if(resp.data.code === 200){
                                messageTop("提交成功", 'success');
                                this.reload();
                            }
                        });
                    }else{ //添加
                        doPost('/Clue/AddClue', formdata).then((resp)=>{
                            if(resp.data.code === 200){
                                messageTop("提交成功", 'success');
                                this.reload();
                            }
                        });
                    }
                    
                }else{
                    //console.log("验证失败");
                }
            });
        },
        //加载数据编辑
        getClueData:function(){
            if(this.ClueModel.id > 0){
                let id = this.$route.params.id;
                doGet('/Clue/Edit/' + id,{}).then((resp)=>{
                    if(resp.data.code === 200){
                        //console.log(resp.data);
                        this.activityIdOptions = resp.data.data.keyValuePairs;
                        this.ClueModel = resp.data.data;
                        this.reload();
                    }
                });
            }
        }
    },
}
</script>