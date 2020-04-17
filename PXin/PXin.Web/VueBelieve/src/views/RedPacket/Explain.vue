<template>
    <div class="explain" :style="{backgroundImage: 'url('+require('@/assets/images/red_packet_explain_bg_01.png')+')'}" v-title data-title="活动说明">
        <div class="content">
            <div class="top" :style="{backgroundImage: 'url('+require('@/assets/images/red_packet_explain_bg_02.png')+')'}">活动说明</div>
            <div class="text">
                <p>红包当日有效，当日未领取视为放弃；</p>
                <p>账号有效期内，当月在充值码首页购买充值码(SVC)，次月即可参与领取红包的活动；</p>
                <p>每个权益点可领取100天的红包，领取的奖励在权益点到期日统一结算；</p>
                <p>专户DOS、SV等奖励结算后转至你对应的钱包；</p>
                <p>充值码(SVC)，超过1000的部分，以1000为倍数生成充值码转至您的钱包，不足1000的部分转至你的SV钱包；</p>
                <p>通过不正当手段获得奖励的用户，一经查实，广州相信科技有限公司有权撤销其所有奖励；</p>
                <p>本次活动最终解释权归广州相信科技有限公司所有；</p>
            </div>
        </div>
    </div>
</template>


<style lang="scss" scoped>
.explain {
    width: 100%;
    min-height: 100%;
    background-size: 100% 100%;
    box-sizing: border-box;
    padding: 0.6rem 0;
    .content {
      margin: 0 0.3rem 0 0.3rem;
      background-color: rgba(255, 227, 206, 0.26);
	    border-radius: 0.1rem;
        .top {
            height: 0.8rem;
            line-height: 0.8rem;
            transform: translateY(-0.3rem);
            background-size: 2.6rem auto;
            background-repeat: no-repeat;
            background-position: top center;
            text-align: center;
            color: #ff6f44;
        }
        .text {
            color: #fff;
            padding: 0.4rem;
            font-size: 0.24rem;
            p {
                margin: 0;
                padding-bottom: 0.6rem;
            }
            ::before {
                content: "";
                width: 0.2rem;
                height: 0.2rem;
                background: #fff;
                display: inline-block;
                margin-right: 0.3rem;
                transform:rotate(45deg);
            }
        }
    }
}
</style>
