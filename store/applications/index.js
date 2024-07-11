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
};
