import axios from 'axios';

export default {
  state() {
    return {
      applications: [],
    };
  },
  getters: {
    getApps(state) {
      return state.applications;
    },
  },
  mutations: {
    setApps(state, payload) {
      state.applications = payload.apps;
    },
    addApplication(state, payload) {
      payload.newItem.tasks = payload.newItem.tasks.length;
      state.applications.push(payload.newItem);
    },
    putApplication(state, payload) {
      payload.newItem.tasks = payload.newItem.tasks.length;
      state.applications.splice(payload.index, 1, payload.newItem);
    },
    removeApplication(state, payload) {
      state.applications = state.applications.filter(
        (app) => app.id != payload.appId
      );
    },
    removeAllServerApplications(state, payload) {
      state.applications.forEach((app) => {
        if (app.serverId == payload.serverId) {
          axios
            .delete('https://localhost:7092/api/App?id=' + app.id)
            .then(function (response) {
              console.log(response);
            })
            .catch(function (error) {
              console.log(error);
            });
        }
      });
      state.applications = state.applications.filter(
        (app) => app.serverId != payload.serverId
      );
    },
  },
  actions: {
    async fetchApps(context, data) {
      try {
        const pagination = data.pagination;
        const search = data.search;
        const response = await axios.get('https://localhost:7092/api/App', {
          params: {
            pageNumber: pagination[0],
            pageSize: pagination[1],
            serverName: search[0],
            applicationName: search[1],
          },
        });
        context.commit('setApps', { apps: response.data.applications });
        context.commit('setTotalItems', {
          totalItems: response.data.totalItems,
        });
        context.commit('setTotalPages', {
          totalPages: response.data.totalPages,
        });
      } catch (error) {
        console.error('Error fetching servers:', error);
      }
    },
    saveApplication(newItem) {
      axios
        .post('https://localhost:7092/api/App', newItem)
        .then(function (response) {
          console.log(response);
        })
        .catch(function (error) {
          console.log(error);
        });
    },
    updateApplication(context, payload) {
      context.commit('detachTasksFromApplication', {
        appId: payload.newItem.id,
      });
      context.commit('reattachTasksToNewApplication', {
        newItem: payload.newItem,
      });
      axios
        .put('https://localhost:7092/api/App', payload.newItem)
        .then(function (response) {
          console.log(response);
        })
        .catch(function (error) {
          console.log(error);
        });
      context.commit('putApplication', {
        newItem: payload.newItem,
        index: payload.index,
      });
    },
    deleteApplication(context, appId) {
      context.commit('removeApplication', { appId: appId });
      context.commit('detachTasksFromApplication', { appId: appId });
      axios
        .delete('https://localhost:7092/api/App?id=' + appId)
        .then(function (response) {
          console.log(response);
        })
        .catch(function (error) {
          console.log(error);
        });
    },
  },
};
