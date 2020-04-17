;var PXin = {
    conversation:{},
    //官方初始化
    sdkInit: function (params) {
        var appKey = params.appKey;
        var token = params.token;
        RongIMLib.RongIMClient.init(appKey);
        // 连接状态监听器
        RongIMClient.setConnectionStatusListener({
            onChanged: function (status) {
                // status 标识当前连接状态
                switch (status) {
                    case RongIMLib.ConnectionStatus.CONNECTED:
                        console.log('链接成功');
                        break;
                    case RongIMLib.ConnectionStatus.CONNECTING:
                        console.log('正在链接');
                        break;
                    case RongIMLib.ConnectionStatus.DISCONNECTED:
                        console.log('断开连接');
                        break;
                    case RongIMLib.ConnectionStatus.KICKED_OFFLINE_BY_OTHER_CLIENT:
                        console.log('其他设备登录');
                        break;
                    case RongIMLib.ConnectionStatus.DOMAIN_INCORRECT:
                        console.log('域名不正确');
                        break;
                    case RongIMLib.ConnectionStatus.NETWORK_UNAVAILABLE:
                        console.log('网络不可用');
                        break;
                }
            }
        });
        // 消息监听器
        RongIMClient.setOnReceiveMessageListener({
            // 接收到的消息
            onReceived: function (message) {
                // 判断消息类型
                switch (message.messageType) {
                    case RongIMClient.MessageType.TextMessage:
                        var msgBox = document.getElementById("receiveMsg");
                        msgBox.innerHTML += "</br>用户" + message.targetId + ":向你发送了一条消息：" + message
                            .content.content;
                        // message.content.content => 文字内容
                        break;
                    case RongIMClient.MessageType.VoiceMessage:
                        // message.content.content => 格式为 AMR 的音频 base64
                        break;
                    case RongIMClient.MessageType.ImageMessage:
                        // message.content.content => 图片缩略图 base64
                        // message.content.imageUri => 原图 URL
                        break;
                    case RongIMClient.MessageType.LocationMessage:
                        // message.content.latiude => 纬度
                        // message.content.longitude => 经度
                        // message.content.content => 位置图片 base64
                        break;
                    case RongIMClient.MessageType.RichContentMessage:
                        // message.content.content => 文本消息内容
                        // message.content.imageUri => 图片 base64
                        // message.content.url => 原图 URL
                        break;
                    case RongIMClient.MessageType.InformationNotificationMessage:
                        // do something
                        break;
                    case RongIMClient.MessageType.ContactNotificationMessage:
                        // do something
                        break;
                    case RongIMClient.MessageType.ProfileNotificationMessage:
                        // do something
                        break;
                    case RongIMClient.MessageType.CommandNotificationMessage:
                        // do something
                        break;
                    case RongIMClient.MessageType.CommandMessage:
                        // do something
                        break;
                    case RongIMClient.MessageType.UnknownMessage:
                        // do something
                        break;
                    default:
                        // do something
                }
            }
        });
        //开始连接
        RongIMClient.connect(token, {
            onSuccess: function (userId) {
                console.log('Connect successfully. ' + userId);
            },
            onTokenIncorrect: function () {
                console.log('token 无效');
            },
            onError: function (errorCode) {
                var info = '';
                switch (errorCode) {
                    case RongIMLib.ErrorCode.TIMEOUT:
                        info = '超时';
                        break;
                    case RongIMLib.ConnectionState.UNACCEPTABLE_PAROTOCOL_VERSION:
                        info = '不可接受的协议版本';
                        break;
                    case RongIMLib.ConnectionState.IDENTIFIER_REJECTED:
                        info = 'appkey不正确';
                        break;
                    case RongIMLib.ConnectionState.SERVER_UNAVAILABLE:
                        info = '服务器不可用';
                        break;
                }
                console.log(info);
            }
        });
    },
    sendMessage: function (msg) {
        var msg = new RongIMLib.TextMessage({
            content: msg,
            extra: '附加信息'
        });
        var conversationType = RongIMLib.ConversationType.PRIVATE; // 单聊, 其他会话选择相应的消息类型即可
        var targetId = this.conversation.id; // 目标 Id
        RongIMClient.getInstance().sendMessage(conversationType, targetId, msg, {
            onSuccess: function (message) {
                // message 为发送的消息对象并且包含服务器返回的消息唯一 Id 和发送消息时间戳
                console.log('Send successfully');
            },
            onError: function (errorCode, message) {
                var info = '';
                switch (errorCode) {
                    case RongIMLib.ErrorCode.TIMEOUT:
                        info = '超时';
                        break;
                    case RongIMLib.ErrorCode.UNKNOWN:
                        info = '未知错误';
                        break;
                    case RongIMLib.ErrorCode.REJECTED_BY_BLACKLIST:
                        info = '在黑名单中，无法向对方发送消息';
                        break;
                    case RongIMLib.ErrorCode.NOT_IN_DISCUSSION:
                        info = '不在讨论组中';
                        break;
                    case RongIMLib.ErrorCode.NOT_IN_GROUP:
                        info = '不在群组中';
                        break;
                    case RongIMLib.ErrorCode.NOT_IN_CHATROOM:
                        info = '不在聊天室中';
                        break;
                }
                console.log('发送失败: ' + info + errorCode);
            }
        });
    },
    startConversation:function (id) { 
        this.conversation.id=id;
    },
    init: function (config) {
        this.sdkInit(config);
    }
};