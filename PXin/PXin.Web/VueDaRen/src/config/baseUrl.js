import global from "./global";

let baseUrl ;
let imgUrl;
let apiUrl;
// if (process.env.NODE_ENV !== 'production') {
//   apiUrl='';
//   baseUrl='';
//   imgUrl='http://images2.pcn.sulink.cn/web';
// } else {
//   if(process.env.type=="test"){

//     apiUrl='http://client2.pcn.sulink.cn';
//     baseUrl='http://client2.pcn.sulink.cn';
//     imgUrl='http://images2.pcn.sulink.cn/web';

//     console.log('现在使用的是测试服地址：http://client2.pcn.sulink.cn');
//   } else {

//     apiUrl='';
//     baseUrl='';
//     imgUrl='http://images2.p.cn/web';

//     console.log('现在使用的是正式服地址：http://client2.p.cn');
//   }
// }
export {
  baseUrl,
  imgUrl,
  apiUrl
};
