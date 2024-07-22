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
    addServer(state, payload) {
      state.servers.push(payload.newItem);
    },
    putServer(state, payload) {
      state.servers.splice(payload.index, 1, payload.newItem);
    },
    removeServer(state, payload) {
      state.servers = state.servers.filter(
        (server) => server.id != payload.serverId
      );
    },
  },
  actions: {
    saveServer(context, newItem) {
      context.commit('addServer', { newItem: newItem });
    },
    updateServer(context, payload) {
      context.commit('putServer', {
        newItem: payload.newItem,
        index: payload.index,
      });
    },
    deleteServer(context, serverId) {
      context.commit('removeAllServerTasks', { serverId: serverId });
      context.commit('removeAllServerApplications', { serverId: serverId });
      context.commit('removeServer', { serverId: serverId });
    },
  },
};
