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
};
