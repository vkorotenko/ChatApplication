import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

export default new Vuex.Store({
    state: {
        // Максимизирован ли
        appstatemax: true,
        // Закрыт ли чат
        closed: false,
        // Список чатов
        topics: [],
        // Токен 
        token: null,
        // Идентификатор
        id: null,
        // Имя пользователя
        username: ''
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
        },
        // Устанавливаем сессию после логона
        setLoginUser(state, data) {
            // access_token: "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiNzkyMTA4NDMwODAiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJ1c2VyIiwibmJmIjoxNTYyMzIyNjQ5LCJleHAiOjE1NjIzMjI5NDksImlzcyI6IlZLT1JPVEVOS08iLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjUxODg0LyJ9.QN54gGY7XNDANoLUMzXO4Mag1CF1dHNyiJZ8xM3NZ4c"
            // id: 52
            // username: "79210843080"            
            state.id = data.id;
            state.username = data.username;
            state.token = data.access_token;
        }

    },
    actions: {
        async getData({ commit }) {
            commit('gotData', await getData())
        }
    }
})
