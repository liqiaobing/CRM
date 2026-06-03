import { ElMessage , ElMessageBox} from "element-plus"

//信息提示框
export function messageTop(msg, type){
    ElMessage({
        showClose: true,    //是否显示关闭按钮
        center: true ,        //文字是否提供
        duration: 3000,     //显示时间 默认3秒
        message: msg, //消息文字
        type: type,    //消息类型 枚举'success' | 'warning' | 'info' | 'error'
    })
}
//信息提示确认框
export function messageConfirm(msg, title = "Warning"){
    return ElMessageBox.confirm(
        msg,
        title,
        {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning',
        }
    )
}

//获取Token的key  可以用key拿value
export function  getTokenName(){
    return "dlyk token";
}

//删除localStorage和SessionStorage中的Token
export function removeToken(){
    window.sessionStorage.removeItem(getTokenName());
    window.localStorage.removeItem(getTokenName());
}

//返回上一个页面
export function goBack(){
    this.$router.go(-1);
}


