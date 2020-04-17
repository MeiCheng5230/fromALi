<template>
    <div class="knowledge">
        <div class="top-fixed">
            <div class="tab">
                <div :class="item.select?'sel':''" v-for="(item, index) in headTab" :key="index" @click="SetTab(item)">{{ item.tit }}</div>
            </div>
            <div class="remark">共发布{{ num }}个知识问答</div>
        </div>
        <!-- 我的知识库 -->
        <div class="datalist" v-if="typeid==1">
            <van-list
                v-model="knowledgeLoad"
                :finished="knowledgeFinished"
                finished-text="没有更多了"
                @load="knowledgeonLoad"
                :offset="20"
            >
                <router-link :to="{path: '/detail', query: {id: item.id, status: typeid}}" tag="div" class="item" v-for="(item, index) in knowledgeList" :key="index">
                    <div class="top">
                        <div class="ico">Q</div>
                        <div class="tit">{{ item.title }}</div>
                    </div>
                    <div class="bot">
                        <div class="price">{{ item.price }} {{ item.paytype==0?'V':'UV' }}</div>
                        <div class="pernum">{{ item.num }}人已看过</div>
                    </div>
                </router-link>
            </van-list>
        </div>
        <!-- 我的草稿 -->
        <div class="datalist" v-if="typeid==0">   
            <van-list
                v-model="draftLoad"
                :finished="draftFinished"
                finished-text="没有更多了"
                @load="draftonLoad"
                :offset="20"
            >
                <router-link :to="{path: '/detail', query: {id: item.id, status: typeid}}" tag="div" class="item" v-for="(item, index) in draftList" :key="index">
                    <div class="top">
                        <div class="ico">Q</div>
                        <div class="tit">{{ item.title }}</div>
                    </div>
                    <div class="bot">
                        <div class="price">{{ item.price }} {{ item.paytype==0?'V':'UV' }}</div>
                        <div class="pernum">{{ item.num }}人已看过</div>
                    </div>
                </router-link>
            </van-list>
        </div>
        <!-- <nodata /> -->
        <router-link to="/additional" tag="div" class="addImg">
            <img src="@/assets/images/icon_add_knowledgebase.png" alt=""/>
        </router-link>
    </div>
</template>

<script>
import { GetMyKnowledges } from '@/api/getData';
const nodata = () => import("@/components/nodata");
export default {
    data() {
        return {
            headTab: [{tit: '我的知识库', select: true, num: ''}, {tit: '我的草稿', num: ''}],
            pagesize: 6,    // 每页请求数量
            typeid: 1,  // 请求类型： 1-知识库，0-草稿
            num: '',   // 总数   

            draftList: [],  // 草稿数据
            draftPage: 0,   // 草稿页码
            draftLoad: false,    // 草稿加载状态
            draftFinished: false,    // 草稿加载完成

            knowledgeList: [],  // 知识库数据
            knowlePage: 0,      // 知识库页码
            knowledgeLoad: false,    // 草稿加载状态
            knowledgeFinished: false,    // 草稿加载完成
        }
    },
    updated() {
        for (const itm of this.headTab) {
            if(itm.select) {
                this.num = itm.num
            }
        }
    },
    methods: {
        draftonLoad() {
        // 加载草稿数据
            this.draftPage += 1;
            this.GetMyKnowledges(this.draftPage, 0);
        },
        knowledgeonLoad() {
        // 加载知识库数据
            this.knowlePage += 1;
            this.GetMyKnowledges(this.knowlePage, 1);
        },
        async GetMyKnowledges(pagenum, typeid) {
        // 获取知识库信息 typeid 1-知识库， 0-草稿； pagenum-页码
            let result = await GetMyKnowledges({...this.$global.userInfo, typeid: typeid, pagesize: this.pagesize, pagenum: pagenum});
            if (result.result > 0) {
                if (typeid == 0) {
                    this.headTab[1].num = result.data.num;
                    this.draftLoad = false;
                    if (!result.data) {
                        this.Toast('暂无数据');
                        this.draftFinished = true;
                        return;
                    };
                    this.draftList = this.draftList.concat(result.data.list);
                    if(result.data.list.length < this.pagesize) {
                        this.draftFinished = true;
                    }
                } else if (typeid == 1) {
                    this.headTab[0].num = result.data.num;
                    this.knowledgeLoad = false;
                    if (!result.data) {
                        this.Toast('暂无数据');
                        this.knowledgeFinished = true;
                        return;
                    };
                    this.knowledgeList = this.knowledgeList.concat(result.data.list);
                    if (result.data.list.length < this.pagesize) {
                        this.knowledgeFinished = true;
                    }
                }
            } else {
                this.Toast.fail(result.message);
            }
        },
        SetTab(data) {
        // 切换选项
            if (data.tit == '我的草稿') {
                if (!this.draftFinished) {
                    this.draftonLoad();
                };
                this.typeid = 0;
            } else {
                this.typeid = 1;
            };
            for (const item of this.headTab) {
                this.$set(item, 'select', false);
            };
            this.$set(data, 'select', true);
        },
    },
    components: {
        nodata
    }
}
</script>

<style lang="scss" scoped>
.knowledge {
    height: 100%;
    display: flex;
    flex-direction: column;
    .van-list {
        padding-top: 2rem;
        // min-height: 100%;
        box-sizing: border-box;
    }
    .top-fixed {
        position: fixed;
        top: 0;
        width: 100%;
        background: #fff;
    }
    .remark {
        color: #999;
        font-size: 0.3rem;
        background: #f7f7fc;
        padding: 0.2rem 0.3rem;
    }
    .tab {
        display: flex;
        font-size: 0.3rem;
        color: #666;
        padding: 0.3rem;
        padding-bottom: 0.4rem;
        div {
            height: 0.4rem;
            margin-right: 0.3rem;
            line-height: 0.4rem;
            &.sel {
                font-weight: bold;
                color: #333;
                position: relative;
                &::after {
                    content: '';
                    position: absolute;
                    bottom: 0;
                    width: 100%;
                    height: 0.12rem;
                    display: block;
                    background-color: #2ea2fa;
	                opacity: 0.5;
                }
            }
        }
    }
    .datalist {
        flex: auto;
        overflow-y: auto;
        padding: 0 0.3rem;
        .item {
            padding: 0.6rem 0;
            border-bottom: 1px solid #eaeaea;
            .top {
                display: flex;
                .ico {
                    flex-shrink: 0;
                    width: 0.36rem;
                    height: 0.36rem;
                    background-color: #ff9232;
                    text-align: center;
                    line-height: 0.36rem;
                    border-radius: 50%;
                    font-size: 0.24rem;
                    color: #fff;
                    margin-right: 0.17rem;
                }
                .tit {
                    font-size: 0.3rem;
                    line-height: 0.36rem;
                    font-weight: bold;
                    text-overflow: -o-ellipsis-lastline;
                    overflow: hidden;
                    text-overflow: ellipsis;
                    display: -webkit-box;
                    -webkit-line-clamp: 2;
                    line-clamp: 2;
                    -webkit-box-orient: vertical;
                }
            }
            .bot {
                display: flex;
                margin-top: 0.3rem;
                .price {
                    color: #ff1541;
                    font-weight: bold;
                    margin-left: 0.53rem;              
                }
                .pernum {
                    flex: auto;
                    text-align: right;
                    color: #999;
                    font-size: 0.24rem;
                }
            }
        }
    }
    .addImg {
        width: 1.2rem;
        height: 1.2rem;
        position: fixed;
        bottom: 1rem;
        right: 0.5rem;
        img {
            width: 100%;
            height: 100%;
        }
    }
}
</style>
