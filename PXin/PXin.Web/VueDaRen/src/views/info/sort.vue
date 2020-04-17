<template>
    <div class="sort">
        <div class="tit">请点击下方选项选择你的专业领域(单选）</div>
        <div class="lst">
            <div class="item" :class="item.select?'select':''" :style="item.name.length>=6?'width: 4.2rem':''" @click="SetSelect(item, 'lst1')" v-for="item in sortlist" :key="item.index"  :data-num="item.id">
                {{ item.name }}
            </div>
        </div>
        <div class="tit">请选择对应标签(最多可选九个）</div>
        <div class="lst">
            <div class="item" :class="item.select?'select':''" :style="item.name.length>=6?'width: 4.2rem':''" @click="SetSelect(item, 'lst2')" v-for="item in lst" :key="item.index" :data-num="item.id">
                {{ item.name }}
            </div>
        </div>
        <button class="btn" @click="SetAdd" v-if="showBtn">保存</button>
        <div class="remark">
            <div class="title">温馨提示</div>
            <p>1.首页名片仅展示前4个话题标签，超出部分话题标签只有在用户选择该话题时分区时，才予以显示。</p>
            <p>2.上传相关资格证书可加快审核</p>
        </div>
    </div>
</template>

<script>
import { GetClassifications, CreateMajors } from '@/api/getData';
export default {
    data() {
        return {
            sortlist: [],   // 一级大分类
            lst: [],    // 二级小分类
            majorid: [],    // 选中的二级分类
            showBtn: false,
            majors: [],     // 返回要显示的分类
            status: '',
			hasNum: false,	// 最多选择九个
        }
    },
    created() {
        this.GetClassifications();
        var userinfo = JSON.parse(this.getStore('info'));
        console.log(userinfo);
        if(userinfo.status == 0 || userinfo.status == 2) {
            this.showBtn = true;
        }
    },
    methods: {
        SetSelect(data, type) {
            var userinfo = JSON.parse(this.getStore('info'));
            if(userinfo.status == 1 || userinfo.status == 3) {
                return false;
            }
			
            // 选择专业领域
            if(type == 'lst1') {
                this.lst = data.list;
                this.majorid = [];
                for (const itm of this.sortlist) {
                    this.$set(itm, 'select', false);
                }
                for (const item of this.lst) {
                    this.$set(item, 'select', false);
                }
				this.$set(data, 'select', !data.select);
				this.hasNum = false;
            } else if(type == 'lst2') {
				if(data.select) {
					this.hasNum = false;
				}
				if(this.hasNum) {
					this.Toast('你最多可以选择九个！');
					return false;
				}
				this.$set(data, 'select', !data.select);
				var arr = [];
				for(const itm of this.lst) {
					if(itm.select) {
						arr.push(itm)
					}
				}
				if(arr.length >= 9) {
					this.hasNum = true;
				}
			}
        },
        async GetClassifications() {
        // 获取专业领域列表
            let result = await GetClassifications({type: 2, ...this.$global.userInfo});
            var userinfo = JSON.parse(this.getStore('info')); 
            this.majors = userinfo.majors || [];
            if(result.result > 0) {
                this.sortlist = result.data;
                if(this.sortlist.length) {
                    if (this.majors == []) {
                        this.$set(this.sortlist[0], 'select', true);
                        this.lst = this.sortlist[0].list;
                    } else {
                        this.ShowSort(result.data);
                    }
                }
            } else {
                this.Toast(result.message)
            }
        },
        async CreateMajors(majorid) {
        // 添加专业领域
            let result = await CreateMajors({majorid: majorid, ...this.$global.userInfo});
            if(result.result > 0) {
                this.Toast.success(result.message);
                setTimeout(() => {
                    this.$router.go(-1);
                }, 1000)
            } else {
                this.Toast(result.message);
            }
        },
        SetAdd() {
        // 点击保存
            this.majorid = [];
            for (const itm of this.lst) {
                if(itm.select) {
                    this.majorid.push(itm.id);
                }
            }
            var str = this.majorid.join(',');
            this.CreateMajors(str)
        },
        ShowSort(arr) {
			var newarr = [];
            for (const item of this.majors) {
                var st1 = item.split(',')[0];
                var st2 = item.split(',')[1];
                for (const itm of arr) {
                    if(itm.id == Number(st1)) {
                        this.$set(itm, 'select', true);
                        this.lst = itm.list;
                    }
                };
                for (const itm of this.lst) {
                    if (itm.id == st2) {
                        this.$set(itm, 'select', true);
                    }
					
                };
            };
			for (const itm of this.lst) {
                if(itm.select) {
					newarr.push(itm);
				}			
            };
			if(newarr.length >= 9) {
				this.hasNum = true;
			} else {
				this.hasNum = false;
			}
		}
    }
}
</script>

<style lang="scss" scoped>
.sort {
    padding: 0.3rem;
    font-size: 0.3rem;
    .tit {
        font-weight: bold;
        margin-bottom: 0.3rem;
    }
    .lst {
        margin-bottom: 0.3rem;
        .item {
            color: #999;
            display: inline-block;
            border-radius: 0.04rem;
            padding: 0.2rem;
            margin-right: 0.2rem;
            margin-bottom: 0.3rem;
            background: #f7f7fc;
            width: 2rem;
            text-align: center;
            box-sizing: border-box;
            &.select {
                background: #2ea2fa;
                color: #fff;
            }
        }
    }
    .btn {
        background: #2ea2fa;
        color: #fff;
        padding: 0.28rem 0;
        border: none;
        border-radius: 0.04rem;
        width: 100%;
        margin-top: 0.6rem;
    }
    .remark {
        padding-top: 0.6rem;
        .title {
            padding-bottom: 0.2rem;
        }
        p {
            margin: 0;
            padding-bottom: 0.1rem;
            font-size: 0.24rem;
            line-height: 0.36rem;
        }
    }
}
</style>
