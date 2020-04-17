;
var PXin = {
    conversation: {},
    chat: {},
    RongIMEmoji: {},
    RongIMVoice: {},
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
                let msgBox = document.getElementById("message-box");
                switch (message.messageType) {
                    case RongIMClient.MessageType.TextMessage:
                        let toEmojiMsg = RongIMLib.RongIMEmoji.symbolToEmoji(message.content.content);
                        msgBox.innerHTML += "</br>用户" + message.targetId + "向你发送了一条消息：</br>" + toEmojiMsg;
                        // message.content.content => 文字内容
                        break;
                    case RongIMClient.MessageType.VoiceMessage:
                        console.log();
                        let voiceMsg = message.content.content;
                        //let msgBox = document.getElementById("receiveMsg");
                        msgBox.innerHTML += "</br>用户" + message.targetId + "向你发送了一条语音：</br> <button class='voiceInfo' data-message=" + voiceMsg + " onclick='PXin.playVoice(this)'>播放测试语音 base64 </button>";
                        // message.content.content => 格式为 AMR 的音频 base64
                        break;
                    case RongIMClient.MessageType.ImageMessage:
                        let imageMsg = message.content.imageUri;
                        msgBox.innerHTML += "</br>用户" + message.targetId + "向你发送了一张图片： </br><image src=" + imageMsg + " />";
                        console.log(message.content.imageUri);
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
    ImageUpload: function (event) {
        var thisTarget = event.target || event.srcElement;
        var _file = thisTarget.files;
        var file = _file[0];

        var fileReader = new FileReader();
        fileReader.readAsDataURL(file);
        fileReader.onabort = function (e) {
            console.log("文件读取异常" + file.name);
        };
        fileReader.onerror = function (e) {
            console.log("文件读取出现错误" + file.name);
        };

        fileReader.onload = function (e) {
            if (e.target.result) {
                let imageBase64Str;
                console.log("完成" + file.name);
                imageBase64Str = e.target.result;
                // console.log(imageBase64Str);
                // if (fileLength < file.length) {
                //     reader.readAsDataURL(file);
                // } else {
                // }
                // console.log(imageBase64Str);
                let imageBase64Array = imageBase64Str.split(',');
                let suffixStr = imageBase64Array[0];
                let index = suffixStr.indexOf('/') + 1;
                let suffix = suffixStr.substr(index, suffixStr.length - 7 - index);
                let imageBase64Content = imageBase64Array[1];

                $.ajax({
                    type: "post",
                    url: "http://localhost:4778/api/Sys/UploadFile",
                    data: {
                        "content": imageBase64Content,
                        "typeid": suffix,
                        "nodeid": 3434909,
                        "sid": 81123,
                    },
                    dataType: "json",
                    success: function (res) {
                        if (res.result > 0) {
                            var imageMsg = new RongIMLib.ImageMessage({
                                "content": "",
                                "imageUri": res.data.fullurl,
                                "isFull": false,
                                "extra": ""
                            });
                            PXin.sendMessage(imageMsg);
                        }

                    }
                });
            }
        };

    },
    sendMessage: function (message) {
        let toEmojiMsg = RongIMLib.RongIMEmoji.symbolToEmoji(message.content);
        console.log("toEmojiMsg:" + toEmojiMsg);
        // var msg = new RongIMLib.TextMessage({
        //     content: message,
        //     extra: '附加信息'
        // });
        var conversationType = RongIMLib.ConversationType.PRIVATE; // 单聊, 其他会话选择相应的消息类型即可
        var targetId = this.conversation.id; // 目标 Id
        RongIMClient.getInstance().sendMessage(conversationType, targetId, message, {
            onSuccess: function (message) {
                console.log($(message)[0]);
                let showMsg;
                if ("TextMessage" == $(message)[0].messageType) {
                    showMsg = toEmojiMsg;
                } else if ("VoiceMessage" == $(message)[0].messageType) {
                    showMsg = "<button class='voiceInfo' data-message=" + toEmojiMsg + " onclick='PXin.playVoice(this)'>播放测试语音 base64 </button>";
                } else if ("ImageMessage" == $(message)[0].messageType) {
                    showMsg = "<image style='width:120px;height:60px' src=" + message.content.imageUri + " />";
                }
                // message 为发送的消息对象并且包含服务器返回的消息唯一 Id 和发送消息时间戳
                console.log('Send successfully');

                var newMsg = '<div class="rongcloud-Message rongcloud-clearfix  rongcloud-Message-send " id="rcs-templte-message-text">';
                newMsg += '<div>';
                newMsg += ' <div class="rongcloud-Message-header"><img class="rongcloud-img rongcloud-Message-avatar rongcloud-avatar" src="http://7xo1cb.com1.z0.glb.clouddn.com/rongcloudkefu2.png" alt="">';
                newMsg += ' <div class="rongcloud-Message-author rongcloud-clearfix"> <a class="rongcloud-author"> 我';
                newMsg += '  </a> </div>';
                newMsg += ' </div>';
                newMsg += '</div>';
                newMsg += '<div class="rongcloud-Message-body">';
                newMsg += ' <div class="rongcloud-Message-text">';
                newMsg += '<pre class="rongcloud-Message-entry">' + showMsg + ' </pre>';
                newMsg += ' </div>';
                newMsg += ' </div>';
                newMsg += '  </div>';

                var hisMsg = document.getElementById("rcs-templte-message-text");
                let msgBox = document.getElementById("message-box");
                msgBox.innerHTML = msgBox.innerHTML + newMsg;
                var inputMsgBox = document.getElementById("chatbox-message");
                inputMsgBox.value = "";
                inputMsgBox.focus();

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
    showEmojiBox: function () { //显示隐藏表情的box
        var dis = this.chat.emojiBox.parentNode.style.display;
        if (dis == "none") {
            this.chat.emojiBox.parentNode.style.display = "block";
        } else {
            this.chat.emojiBox.parentNode.style.display = "none";
        }
    },
    getEmojiDetailList: function () { //获取全部表情
        var shadowDomList = [];
        for (var i = 0; i < this.RongIMEmoji.list.length; i++) {
            var value = this.RongIMEmoji.list[i];
            shadowDomList.push(value.node);
        }
        return shadowDomList;
    },
    bindClickEmoji: function (emojis) { //为每个表情绑定click事件
        for (var i = 0; i < emojis.length; i++) {
            var emojiHtml = emojis[i];
            this.chat.emojiBox.appendChild(emojiHtml);
            emojiHtml.onclick = this.clickEmoji;
        }
    },
    clickEmoji: function (event) { //click表情事件
        var e = event || window.event;
        var target = e.target || e.srcElement;
        if (document.all && !document.addEventListener === false) {
            console.log(target);
        }
        PXin.chat.chatbox.value = PXin.chat.chatbox.value + target.getAttribute("name");
        PXin.showEmojiBox();
    },
    playVoice: function (t) {
        let voice = t.getAttribute('data-message');
        if (voice) {
            var duration = voice.length / 1024; // 音频持续大概时间(秒)
            PXin.RongIMVoice.preLoaded(voice, function () {
                PXin.RongIMVoice.play(voice, duration);
            });
        } else {
            console.error('请传入 amr 格式的 base64 音频文件');
        }
    },
    keyboard: function (event) {
        var thisTarget = event.target || event.srcElement;
        setTimeout(function () {
            thisTarget.scrollIntoView(true);
        }, 500)
    },
    keySend: function (event) {
        if (event.keyCode == '13' && !event.shiftKey) {
            event.preventDefault()
            let msg = $("#chatbox-message").val();
            if (!msg) {
                $("#chatbox-message").focus();
                return;
            }
            let message = new RongIMLib.TextMessage({
                content: msg,
                extra: '附加信息'
            });
            PXin.sendMessage(message);
        } else {
            //inputChange();
        }
    },
    //inputChange: function () {
    //    var timespan = new Date().getTime() - conversation.lastSendTime;
    //    if (timespan > 1000 * 6) {
    //        conversation.lastSendTime += timespan;
    //        sendTyping();
    //    }
    //},
    startConversation: function (id) { //开始会话
        this.conversation.id = id;
    },
    openConversation: function (conversation) { //打开会话
        RongIMLib.RongIMEmoji.init();
    },
    init: function (config) { //初始化
        this.sdkInit(config);
        this.chat.emojiBox = config.emojiBox;
        this.chat.chatbox = config.chatbox;

        let RongIMEmoji = RongIMLib.RongIMEmoji;
        RongIMEmoji.init();
        this.RongIMEmoji = RongIMEmoji;

        var RongIMVoice = RongIMLib.RongIMVoice;
        RongIMVoice.init();
        this.RongIMVoice = RongIMVoice;
    }
};