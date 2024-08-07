import axios from 'axios';

export default {
  state() {
    return {
      servers: [],
      totalServerItems: 0,
    };
  },
  getters: {
    getServers(state) {
      return state.servers;
    },
    getTotalServerItems(state) {
      return state.totalServerItems;
    },
  },
  mutations: {
    setTotalServerItems(state, payload) {
      state.totalServerItems = payload.totalItems;
    },
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
    async fetchAllServers(context) {
      try {
        const response = await axios.get('https://localhost:7092/api/Server');
        context.commit('setServers', { servers: response.data });
        context.commit('setTotalServerItems', {
          totalItems: response.data.length,
        });
      } catch (error) {
        console.error('Error fetching servers:', error);
      }
    },
    async fetchServers(context, data) {
      try {
        const pagination = data.pagination;
        const search = data.search;
        const response = await axios.get(
          'https://localhost:7092/api/Server/paginated-servers',
          {
            params: {
              pageNumber: pagination[0],
              pageSize: pagination[1],
              serverName: search[0],
            },
          }
        );
        context.commit('setServers', { servers: response.data.servers });
        context.commit('setTotalServerItems', {
          totalItems: response.data.totalItems,
        });
      } catch (error) {
        console.error('Error fetching servers:', error);
      }
    },
    saveServer(context, newItem) {
      axios
        .post('https://localhost:7092/api/Server', newItem)
        .then(function (response) {
          console.log(response);
          context.dispatch('fetchServers', {
            pagination: [
              context.getters.getCurrentPage,
              context.getters.getItemsPerPage,
            ],
            search: ['', '', ''],
          });
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
          context.dispatch('fetchServers', {
            pagination: [
              context.getters.getCurrentPage,
              context.getters.getItemsPerPage,
            ],
            search: ['', '', ''],
          });
        })
        .catch(function (error) {
          console.log(error);
        });
    },
  },
};
