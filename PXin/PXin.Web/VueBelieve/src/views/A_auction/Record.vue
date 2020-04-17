<template>
    <div class="Record">
        <table>
            <thead>
                <tr>
                    <th>排名区间(名)</th>
                    <th>竞拍价(单价)</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in 20" :key="item">
                    <td>1-5</td>
                    <td>1999P点</td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
export default {
    created() {
        document.title = "拍卖记录";
    }
}
</script>

<style lang="scss" scoped>
.Record {
    font-size: 0.28rem;
    table {
        width: 100%;
        border-collapse: collapse;
        tr {
            th {
                background: #eaf6ff;
                &:first-child {
                    text-align: left;
                    padding: 0.2rem 0 0.2rem 0.6rem;
                }
                &:last-child {
                    text-align: right;
                    padding: 0.2rem 0.6rem 0.2rem 0;
                }
            }
            td {
                &:nth-child(odd) {
                    padding: 0.2rem 0 0.2rem 0.6rem;
                }
                &:last-child {
                    padding: 0.2rem 0.6rem 0.2rem 0;
                    text-align: right;
                }
            }
            &:nth-child(odd) {
                td {
                    background: #fff;
                }
            }
            &:nth-child(even) {
                td {
                    background: #f7f7fc;
                }
            }
        }
    }
}
</style>
