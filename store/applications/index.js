import data from '@static/data/applications.json';

export default {
  state() {
    return {
      applications: data,
    };
  },
  getters: {
    getApps(state) {
      return state.applications;
    },
  },
  mutations: {
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
