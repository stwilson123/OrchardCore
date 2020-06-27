const menu = {
    state: {
        sysMenus: [],
        firstGetMenu: true
    },
    mutations: {
        SET_MENUS: (state, menus) => {
            state.sysMenus = menus
        },
        SET_FIRSTGETMENU:(state) => {
            state.firstGetMenu = false
        }
    },
    actions: {
        setMenus({ commit }, menus) {
            commit('SET_MENUS', menus)
        },
        setFristGetMenu({ commit }) {
            commit('SET_FIRSTGETMENU')
        }
    }
}
export default menu
