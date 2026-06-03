<template>
    <el-button type="primary" @click="ImportExcelAll">批量导出(Excel)</el-button>
    <el-button type="success" @click="ImportExcelPartly">选择导出(Excel)</el-button>
    <!-- 客户列表 -->
    <el-table
        :data="CustomerList"
        style="width: 100%"
        @selection-change="handleSelectionChange"
    >
        <el-table-column type="selection" />
        <el-table-column type="index" property="id" label="序号" width="60" />
        <el-table-column property="ownerName" label="负责人"   width="80"/>
        <el-table-column property="fullName" label="姓名" width="80" show-overflow-tooltip />
        <el-table-column property="appellation" label="称呼" width="60" show-overflow-tooltip />
        <el-table-column property="phone" label="手机" width="80" show-overflow-tooltip />
        <el-table-column property="weixin" label="微信" width="80" show-overflow-tooltip />
        <el-table-column property="needLoan" label="是否贷款" width="80" show-overflow-tooltip />
        <el-table-column property="state" label="意向状态" width="110" show-overflow-tooltip />
        <el-table-column property="intentionState" label="线索状态" width="110" show-overflow-tooltip />
        <el-table-column property="source" label="线索来源" width="80" show-overflow-tooltip />
        <el-table-column property="productName" label="意向产品" width="110" show-overflow-tooltip />
        <el-table-column property="nextContactTime" label="下次联系时间" show-overflow-tooltip />

        <el-table-column label="操作" show-overflow-tooltip>
            <template #default="scope">
                <el-button size="small" type="primary" @click="">查看</el-button>
                <el-button size="small" type="success" @click="">编辑</el-button>
                <el-button size="small" type="danger" @click="">删除</el-button>
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
</template>

<script>
import { goBack, messageTop } from '@/util/util';

import {doDelete, doGet, doPost, doPut, doGetFile} from '../http/httpRequest'
export default{
    data(){
        return{
            CustomerList:[],
            pageSize:0,
            total:0,
            currentPage:1,
            //存储被选中的对象
            selectedList:[],
        }
    },
    inject:['reload'],
    mounted(){
        this.getPageData(this.currentPage);
    },
    methods:{
        goBack,
        //点击分页栏调用
        toPage: function(current_Page){
            //每次点击后记录当前页
            this.currentPage = current_Page;
            console.log( this.currentPage);
            //获取数据
            this.getPageData(current_Page);
        },
        //获取列表数据
        getPageData: function(current_Page){
            doGet('/Customer/GetCustomerList/' + current_Page,{}).then((resp) =>{
                if(resp.data.code === 200){
                    this.CustomerList = resp.data.data;
                    this.pageSize = resp.data.pageSize;
                    this.total = resp.data.total;
                }
            });
        },
        //导出Excel文件
        ImportExcelAll: function(){
            doGetFile('/Customer/ImportExcelAll',{}).then((resp, fileName) =>{
                //--------------1-------------------
                /*
                fileName = decodeURI(resp.headers['content-disposition'].split("filename=").pop().split(";")[0]);
                // 处理返回的文件流
                //主要是将返回的data数据通过blob保存成文件
                var content = resp.data;
                var blob = new Blob([content]);
                //var fileName = "wyy.xlsx"; //要保存的文件名称
                if ("download" in document.createElement("a")) {
                    // 非IE下载
                    // 如果不支持，则创建一个新的a元素并隐藏它  
                    var elink = document.createElement("a");
                    // 设置a元素的download属性为文件名，这使得点击a元素时开始下载文件
                    elink.download = fileName;
                    // 隐藏a元素，使其在页面上不可见 
                    elink.style.display = "none";
                    // 创建一个指向Blob对象的URL，并设置为a元素的href属性，这样a元素就可以下载该Blob对象表示的文件了
                    elink.href = URL.createObjectURL(blob);
                    // 将a元素添加到文档的body中，使其可见并可以被点击
                    document.body.appendChild(elink);
                    // 模拟点击a元素，开始下载文件  
                    elink.click();
                    // 释放之前为Blob对象创建的URL，以释放内存 
                    URL.revokeObjectURL(elink.href); // 释放URL 对象
                    // 从文档的body中移除a元素，清理内存 
                    document.body.removeChild(elink);
                } else {
                    // IE10+下载
                    navigator.msSaveBlob(blob, fileName);
                }
                //console.log(resp);
                */
                //--------------2--------------
                
                // 使用后台返回的数据创建一个新的Blob对象  
                var content = resp.data;
                var blob = new Blob([content]);
                //如果fileName参数未定义或为空，则从res的headers中获取'content-disposition'字段，并从中提取文件名  
                if (!fileName) {
                    fileName = decodeURI(resp.headers['content-disposition'].split("filename=").pop().split(";")[0]);
                    //.log(fileName);
                    //fileName = "客户信息.xlsx"
                    // let fileName1 = fileName.split('filename=');
                    // console.log(fileName1);
                    // let fileName2 = fileName1.pop();
                    // console.log(fileName2);
                    // 通过 URLEncoder.encode(pFileName, StandardCharsets.UTF_8.name()) 加密编码的, 使用decodeURI(fileName) 解密
                    //let fileName3 = decodeURI(fileName2)
                    // 通过 new String(pFileName.getBytes(), StandardCharsets.ISO_8859_1) 加密编码的, 使用decodeURI(escape(fileName)) 解密
                    // fileName = decodeURI(escape(fileName))
                    //console.log(fileName3);

                }
            
                // 检查当前浏览器是否支持msSaveOrOpenBlob方法（这是旧版IE浏览器特有的API）
                if ('msSaveOrOpenBlob' in navigator) {
                // 如果支持，使用该方法下载文件，参数为Blob对象和文件名  
                    window.navigator.msSaveOrOpenBlob(blob, fileName);
                } else {
                    // 如果不支持，则创建一个新的a元素并隐藏它  
                    const elink = document.createElement('a');
                    // 设置a元素的download属性为文件名，这使得点击a元素时开始下载文件
                    elink.download = fileName;
                    // 隐藏a元素，使其在页面上不可见 
                    elink.style.display = 'block';
                    // 创建一个指向Blob对象的URL，并设置为a元素的href属性，这样a元素就可以下载该Blob对象表示的文件了
                    elink.href = URL.createObjectURL(blob);
                    // 将a元素添加到文档的body中，使其可见并可以被点击
                    document.body.appendChild(elink);
                    // 模拟点击a元素，开始下载文件  
                    elink.click();
                    // 释放之前为Blob对象创建的URL，以释放内存 
                    URL.revokeObjectURL(elink.href);
                    // 从文档的body中移除a元素，清理内存 
                    document.body.removeChild(elink);
                }
                
            });
        },
         //导出Excel文件
         ImportExcelPartly: function(){
            if(this.selectedList.length == 0){
                messageTop("请先选中需要导出的数据", 'warning');
                return;
            }
            let queryStr = this.selectedList.join(',');
            doGetFile('/Customer/ImportExcel/' + queryStr,{}).then((resp, fileName) =>{
                // 使用后台返回的数据创建一个新的Blob对象  
                var content = resp.data;
                var blob = new Blob([content]);
                //如果fileName参数未定义或为空，则从res的headers中获取'content-disposition'字段，并从中提取文件名  
                if (!fileName) {
                    fileName = decodeURI(resp.headers['content-disposition'].split("filename=").pop().split(";")[0]);
                }
                // 检查当前浏览器是否支持msSaveOrOpenBlob方法（这是旧版IE浏览器特有的API）
                if ('msSaveOrOpenBlob' in navigator) {
                // 如果支持，使用该方法下载文件，参数为Blob对象和文件名  
                    window.navigator.msSaveOrOpenBlob(blob, fileName);
                } else {
                    // 如果不支持，则创建一个新的a元素并隐藏它  
                    const elink = document.createElement('a');
                    // 设置a元素的download属性为文件名，这使得点击a元素时开始下载文件
                    elink.download = fileName;
                    // 隐藏a元素，使其在页面上不可见 
                    elink.style.display = 'block';
                    // 创建一个指向Blob对象的URL，并设置为a元素的href属性，这样a元素就可以下载该Blob对象表示的文件了
                    elink.href = URL.createObjectURL(blob);
                    // 将a元素添加到文档的body中，使其可见并可以被点击
                    document.body.appendChild(elink);
                    // 模拟点击a元素，开始下载文件  
                    elink.click();
                    // 释放之前为Blob对象创建的URL，以释放内存 
                    URL.revokeObjectURL(elink.href);
                    // 从文档的body中移除a元素，清理内存 
                    document.body.removeChild(elink);
                }
            });
        },
        //勾选时触发函数
        handleSelectionChange:function(selecteds){
            this.selectedList=[];
            selecteds.forEach(item =>{
                let id = item.id;
                this.selectedList.push(id);
            });
        }
    },
}
</script>

<style scoped>
.el-table{
    margin-top: 12px;
}
.el-pagination{
    padding-top: 12px;
}
</style>