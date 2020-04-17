/**  
 * 身份证15位编码规则：dddddd yymmdd xx p   
 * dddddd：地区码   
 * yymmdd: 出生年月日   
 * xx: 顺序类编码，无法确定   
 * p: 性别，奇数为男，偶数为女  
 * <p />  
 * 身份证18位编码规则：dddddd yyyymmdd xxx y   
 * dddddd：地区码   
 * yyyymmdd: 出生年月日   
 * xxx:顺序类编码，无法确定，奇数为男，偶数为女   
 * y: 校验码，该位数值可通过前17位计算获得  
 * <p />  
 * 18位号码加权因子为(从右到左) Wi = [ 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2,1 ]  
 * 验证位 Y = [ 1, 0, 10, 9, 8, 7, 6, 5, 4, 3, 2 ]   
 * 校验位计算公式：Y_P = mod( ∑(Ai×Wi),11 )   
 * i为身份证号码从右往左数的 2...18 位; Y_P为脚丫校验码所在校验码数组位置  
 *   
 */

var Wi = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1];// 加权因子   
var ValideCode = [1, 0, 10, 9, 8, 7, 6, 5, 4, 3, 2];// 身份证验证位值.10代表X   
function IdCardValidate(idCard) {
    idCard = trim(idCard.replace(/ /g, ""));
    if (idCard.length == 10 || idCard.length == 11) {
        return ValidHKID(idCard);
    } else if (idCard.length == 15) {
        return isValidityBrithBy15IdCard(idCard);
    } else if (idCard.length == 18) {
        var a_idCard = idCard.split("");// 得到身份证数组   
        if (isValidityBrithBy18IdCard(idCard) && isTrueValidateCodeBy18IdCard(a_idCard)) {
            return true;
        } else {
            return false;
        }
    } else {
        return false;
    }
}
/**  
 * 判断身份证号码为18位时最后的验证位是否正确  
 * @param a_idCard 身份证号码数组  
 * @return  
 */
function isTrueValidateCodeBy18IdCard(a_idCard) {
    var sum = 0; // 声明加权求和变量   
    if (a_idCard[17].toLowerCase() == 'x') {
        a_idCard[17] = 10;// 将最后位为x的验证码替换为10方便后续操作   
    }
    for (var i = 0; i < 17; i++) {
        sum += Wi[i] * a_idCard[i];// 加权求和   
    }
    valCodePosition = sum % 11;// 得到验证码所位置   
    if (a_idCard[17] == ValideCode[valCodePosition]) {
        return true;
    } else {
        return false;
    }
}
/**  
 * 通过身份证判断是男是女  
 * @param idCard 15/18位身份证号码   
 * @return 'female'-女、'male'-男  
 */
function maleOrFemalByIdCard(idCard) {
    idCard = trim(idCard.replace(/ /g, ""));// 对身份证号码做处理。包括字符间有空格。   
    if (idCard.length == 15) {
        if (idCard.substring(14, 15) % 2 == 0) {
            return 'female';
        } else {
            return 'male';
        }
    } else if (idCard.length == 18) {
        if (idCard.substring(14, 17) % 2 == 0) {
            return 'female';
        } else {
            return 'male';
        }
    } else {
        return null;
    }
}
/**  
 * 验证18位数身份证号码中的生日是否是有效生日  
 * @param idCard 18位书身份证字符串  
 * @return  
 */
function isValidityBrithBy18IdCard(idCard18) {
    var year = idCard18.substring(6, 10);
    var month = idCard18.substring(10, 12);
    var day = idCard18.substring(12, 14);
    var temp_date = new Date(year, parseFloat(month) - 1, parseFloat(day));
    // 这里用getFullYear()获取年份，避免千年虫问题   
    if (temp_date.getFullYear() != parseFloat(year)
          || temp_date.getMonth() != parseFloat(month) - 1
          || temp_date.getDate() != parseFloat(day)) {
        return false;
    } else {
        return true;
    }
}
/**  
 * 验证15位数身份证号码中的生日是否是有效生日  
 * @param idCard15 15位书身份证字符串  
 * @return  
 */
function isValidityBrithBy15IdCard(idCard15) {
    var year = idCard15.substring(6, 8);
    var month = idCard15.substring(8, 10);
    var day = idCard15.substring(10, 12);
    var temp_date = new Date(year, parseFloat(month) - 1, parseFloat(day));
    // 对于老身份证中的你年龄则不需考虑千年虫问题而使用getYear()方法   
    if (temp_date.getYear() != parseFloat(year)
            || temp_date.getMonth() != parseFloat(month) - 1
            || temp_date.getDate() != parseFloat(day)) {
        return false;
    } else {
        return true;
    }
}
//去掉字符串头尾空格   
function trim(str) {
    return str.replace(/(^\s*)|(\s*$)/g, "");
}

function ValidHKID(CardNumber) {
    
    if (CardNumber == "")
        return true;
    var regex = /^[a-zA-Z]{1,2}\d{6}\([0-9a-zAZ-Z]\)$/;

    if (regex.test(CardNumber)) {
        if (CardNumber.length == 10) {
            var sum = 9 * GetAppNumber("@") + 8 * GetAppNumber(CardNumber.substr(0, 1)) + 7 * parseInt(CardNumber.substr(1, 1)) + 6 * parseInt(CardNumber.substr(2, 1)) + 5 * parseInt(CardNumber.substr(3, 1)) + 4 * parseInt(CardNumber.substr(4, 1)) + 3 * parseInt(CardNumber.substr(5, 1)) + 2 * parseInt(CardNumber.substr(6, 1));

            var checkdigit = 11 - sum % 11;

            if (checkdigit == 10)
                checkdigit = "A";
            if (checkdigit == 11)
                checkdigit = "0";

            if (checkdigit == CardNumber.substr(8, 1)) {

                return true;
            }
        }
        if (CardNumber.length == 11)
            var sum = 9 * GetAppNumber(CardNumber.substr(0, 1)) + 8 * GetAppNumber(CardNumber.substr(1, 1)) + 7 * parseInt(CardNumber.substr(2, 1)) + 6 * parseInt(CardNumber.substr(3, 1)) + 5 * parseInt(CardNumber.substr(4, 1)) + 4 * parseInt(CardNumber.substr(5, 1)) + 3 * parseInt(CardNumber.substr(6, 1)) + 2 * parseInt(CardNumber.substr(7, 1));
        var checkdigit = 11 - sum % 11;

        if (checkdigit == 10)
            checkdigit = "A";
        if (checkdigit == 11)
            checkdigit = "0";

        if (checkdigit == CardNumber.substr(9, 1)) {

            return true;
        }
    }
    

    return false;

}

function GetAppNumber(str)
{
   
    switch(str)
    {
        case "@":
            return 58;
            break; 
        case "A":
            return 10;
            break;
        case "B":
            return 11;
            break;
        case "C":
            return 12;
            break;
        case "D":
            return 13;
            break;
        case "E":
            return 14;
            break;
        case "F":
            return 15;
            break;
        case "G":
            return 16;
            break;
        case "H":
            return 17;
            break;
        case "I":
            return 18;
            break;
        case "J":
            return 19;
            break;
        case "K":
            return 20;
            break;
        case "L":
            return 21;
            break;
        case "M":
            return 22;
            break;
        case "N":
            return 23;
            break;
        case "O":
            return 24;
            break;
        case "P":
            return 25;
            break;
        case "Q":
            return 26;
            break;
        case "R":
            return 27;
            break;
        case "S":
            return 28;
            break;
        case "T":
            return 29;
            break;
        case "U ":
            return 30;
            break;
        case "V":
            return 31;
            break;
        case "W":
            return 32;
            break;
        case "X":
            return 33;
            break;
        case "Y":
            return 34;
            break;
        case "Z":
            return 35;
            break;
        
        default:
            return 0;
    }

}