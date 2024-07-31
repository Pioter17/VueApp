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
    },
    updateApplication(context, payload) {
      context.commit('detachTasksFromApplication', {
        appId: payload.newItem.id,
      });
      context.commit('reattachTasksToNewApplication', {
        newItem: payload.newItem,
      });
      context.commit('putApplication', {
        newItem: payload.newItem,
        index: payload.index,
      });
    },
    deleteApplication(context, appId) {
      context.commit('removeApplication', { appId: appId });
      context.commit('detachTasksFromApplication', { appId: appId });
    },
  },
};
