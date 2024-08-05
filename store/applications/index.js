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
    async fetchApps(context) {
      try {
        const response = await axios.get('https://localhost:7092/api/App');
        context.commit('setApps', { apps: response.data });
      } catch (error) {
        console.error('Error fetching servers:', error);
      }
    },
    saveApplication(context, newItem) {
      context.commit('reattachTasksToNewApplication', {
        newItem: newItem,
      });
      context.commit('addApplication', { newItem: newItem });
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
