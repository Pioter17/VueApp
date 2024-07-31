import axios from 'axios';

export default {
  state() {
    return {
      servers: [],
    };
  },
  getters: {
    getServers(state) {
      return state.servers;
    },
  },
  mutations: {
    setServers(state, payload) {
      state.servers = payload.servers;
    },
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
    async fetchServers(context) {
      try {
        const response = await axios.get('https://localhost:7092/api/Server');
        context.commit('setServers', { servers: response.data });
      } catch (error) {
        console.error('Error fetching servers:', error);
      }
    },
    saveServer(context, newItem) {
      context.commit('addServer', { newItem: newItem });
      axios
        .post('https://localhost:7092/api/Server', newItem)
        .then(function (response) {
          console.log(response);
        })
        .catch(function (error) {
          console.log(error);
        });
    },
    updateServer(context, payload) {
      context.commit('putServer', {
        newItem: payload.newItem,
        index: payload.index,
      });
      axios
        .put('https://localhost:7092/api/Server', payload.newItem)
        .then(function (response) {
          console.log(response);
        })
        .catch(function (error) {
          console.log(error);
        });
    },
    deleteServer(context, serverId) {
      context.commit('removeAllServerTasks', { serverId: serverId });
      context.commit('removeAllServerApplications', { serverId: serverId });
      context.commit('removeServer', { serverId: serverId });
      axios
        .delete('https://localhost:7092/api/Server?id=' + serverId)
        .then(function (response) {
          console.log(response);
        })
        .catch(function (error) {
          console.log(error);
        });
    },
  },
};
