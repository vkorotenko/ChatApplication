import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

export default new Vuex.Store({
    state: {
        appstatemax: true,
        closed: false,
        topics: {},
        userid: null
    },
    mutations: {
        maximize(state) {
            state.appstatemax = true;
        },
        minimize(state) {
            state.appstatemax = false;
        },
        close(state) {
            state.closed = true;
        },
        open(state) {
            state.closed = false;
        }

    },
    actions: {
        async actionA({ commit }) {
            commit('gotData', await getData())
        }
    }
})
