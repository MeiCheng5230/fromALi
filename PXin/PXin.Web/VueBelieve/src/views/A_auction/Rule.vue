<template>
    <div class="AauctionRule">
        <div class="text">
            <div class="title">竞拍规则：</div>
            <p>1.每个月会放出固定数量的A点进行竞拍，排名在数量内的竞拍为有效竞拍；</p>
            <p>2.支付成功后，将扣除您钱包内支付的P点；</p>
            <p>3.系统每日更新竞拍排名，竞拍失败退回支付的P点<span style='color: red; font-weight: bold;'>（如果每月最后一天竞拍失败出局的，将只退回竞拍P点一半的P点）</span>；</p>
            <p>4.竞拍结果月底统一结算，有效竞拍的每一个数量可获得1个权益点；</p>
            <p>5.排名按竞拍单价和支付时间倒序排序，如一次竞拍多个A点，单价为有效竞拍的最低价，会出现竞拍失败，望知悉；</p>
        </div>
    </div>
</template>
<style lang="scss" scoped>
.AauctionRule {
    min-height: 100%;
    background: #f2f2f2;
    .text {
        background: #fff;
        padding: 0.6rem 0.4rem 0.8rem 0.35rem;
        .title {
            letter-spacing: 1px;
        }
        p {
            margin: 0;
            margin-top: 0.24rem;
            font-size: 0.24rem;
            color: #999;
        }
    }
}
</style>
