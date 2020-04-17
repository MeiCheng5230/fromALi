export default new Vuex.Store({
    state: {
        isLoading: true,
        pageTotal: 0
    },
    mutations: {
        setLoading(state, isLoad) {
            state.isLoading = isLoad;
            if (state.isLoading) {
                vant.Toast.loading({
                    duration: 0,
                    forbidClick: true,
                    mask: true
                });
                return
            }

        },
        setPageTotal(){

        }
    },
    actions: {},
    modules: {}
})
