import { getTokenName, messageTop, removeToken , messageConfirm} from "@/util/util";
import axios from "axios";

//后端接口地址的前缀
// axios.defaults.baseURL="https://localhost:7257/api"
//axios.defaults.headers['X-Requested-With']="XMLHttpRequest"
//axios.defaults.headers.post['Content-Type']='application/json'
export function judgeUrl(urlType){
    if(urlType === 0) {axios.defaults.baseURL="https://localhost:7257/api";}
    if(urlType === 1) {axios.defaults.baseURL="https://localhost:7075/api";}
}


export function doGet(url, params, urlType=1){
    judgeUrl(urlType);
    return axios({
        method:"get",
        url:url,
        params:params,
        dataType:"json"
    })
}

export function doGetFile(url, params, urlType=1){
    judgeUrl(urlType);
    return axios({
        method:"get",
        url:url,
        params:params,
        dataType:"json",
        responseType: "blob" // 表明返回服务器返回的数据类型
    })
}

// export function doPost(url, data){
//     return axios({
//         method:"post",
//         url:url,
//         params:data,
//         dataType:"json"
//     })
// }

export function doPost(url, data, urlType=1){
    judgeUrl(urlType);
    return axios({
        method:"post",
        url:url,
        data:data,
        dataType:"json"
    })
}

export function doPut(url, data, urlType=1){
    judgeUrl(urlType);
    return axios({
        method:"put",
        url:url,
        data:data,
        dataType:"json"
    })
}

export function doDelete(url, params, urlType=1){
    judgeUrl(urlType);
    return axios({
        method:"delete",
        url:url,
        params:params,
        dataType:"json"
    })
}

// 添加请求拦截器
axios.interceptors.request.use( (config) => {
    // 在发送请求之前做些什么
    let token = window.sessionStorage.getItem(getTokenName());
    if (!token) {
        token = window.localStorage.getItem(getTokenName());
        if (token) { //说明用户选择了记住我
            config.headers['rememberMe'] = true;
        }
    }
    if (token) {
        //let tokenObject = JSON.parse(token);
        //在请求头中放了一个token，后端就可以从请求头中接收到该token
        //config.headers['Authorization'] = tokenObject.jwt;
        config.headers['Authorization'] = 'Bearer ' + token;
    }
    return config;
},  (error) => {
    // 对请求错误做些什么
    return Promise.reject(error);
});



// 添加响应拦截器
axios.interceptors.response.use( (response) => {
    
    if (response.data.code == 400 || response.data.code > 900) { //code码大于900都是token问题 （这边按400来返回token异常 401是没权限）
        // 提示一下token不合法的原因
        messageConfirm('是否重新去登录？')
        .then(() => { //当点击“确定”按钮就执行该then函数
            //去重新登录，把浏览器的token清理一下
            removeToken();
            //跳到登录页
            window.location.href = "/";
        })
        .catch(() => { //当点击“取消”按钮就执行该catch函数
            messageTop("取消去登录", "warning");
        });
    }
    return response;
}, function (error) {
    // 对响应错误做点什么
    if(error.request != null){
        if(error.request.status === 401){
            messageConfirm('是否重新去登录?', "登录过期")
            .then(() => { //当点击“确定”按钮就执行该then函数
                //去重新登录，把浏览器的token清理一下
                window.location.href = "/";
                removeToken();
            }).catch(() => { //当点击“取消”按钮就执行该catch函数
                messageTop("取消跳转登录", "warning");
            })
        }else if(error.request.status === 400){
            messageTop("请求错误", "error");
        }else if(error.request.status === 500){
            messageTop(error.response.data.msg,"error")
        }
    }
    
    return Promise.reject(error);
});

