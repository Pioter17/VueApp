import data from '@static/data/tasks.json';

export default {
  state() {
    return {
      tasks: data,
    };
  },
  getters: {
    getTasks(state) {
      return state.tasks;
    },
  },
};
