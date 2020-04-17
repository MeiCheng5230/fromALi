<template>
    <div class="invitelist">
        <div class="list">
            <div class="item" v-for="item in inviteList" :key="item.index">
                <div class="info">
                    <div class="name">{{ item.nodename  +'('+ item.nodecode + ')' }}</div>
                    <div class="time">{{ item.regtime }}</div>
                </div>
                <div class="code">{{ item.invitationcode }}</div>
            </div>
        </div>
        <noData v-if="!inviteList.length" text="咦！您目前还木有邀请好友哟"/>
    </div>
</template>

<script>
const noData = () => import("@/components/noData.vue");
export default {
    data() {
        return {
            inviteList: []
        }
    },
    created() {
        this.inviteList = this.$route.params.inviteList;
    },
    components: {
        noData
    }
}
</script>

<style lang="scss" scoped>
.invitelist {
    min-height: 100%;
    background: #f7f7fc;
    font-size: 0.3rem;
    .list {
        .item {
            display: flex;
            align-items: center;
            background: #fff;
            border-bottom: 1px solid #f7f7fc;
            padding: 0.36rem 0.3rem;
            .info {
                flex: auto;
                .name {
                    margin-bottom: 0.2rem;
                }
                .time {
                    font-size: 0.24rem;
                    color: #999;
                }
            }
        }
    }
}
</style>
