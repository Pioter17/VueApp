import data from '@static/data/servers.json';

export default {
  state() {
    return {
      servers: data,
    };
  },
  getters: {
    getServers(state) {
      return state.servers;
    },
  },
  mutations: {
    // addServer(state, payload) {
    //   state.tasks.push(payload.newItem);
    // },
  },
  actions: {
    // saveServer(context, newItem) {
    //   context.commit('addTask', { newItem: newItem });
    // },
  },
};
