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
  mutations: {
    addTask(state, payload) {
      state.tasks.push(payload.newItem);
    },
    putTask(state, payload) {
      state.tasks.splice(payload.index, 1, payload.newItem);
    },
    removeTask(state, payload) {
      state.tasks = state.tasks.filter((task) => task.id != payload.taskId);
    },
    detachTasksFromApplication(state, payload) {
      state.tasks.forEach((task) => {
        if (task.applicationId == payload.appId) {
          task.application = null;
          task.applicationId = null;
        }
      });
    },
    reattachTasksToNewApplication(state, payload) {
      payload.newItem.tasks.forEach((newTask) => {
        state.tasks.forEach((task) => {
          if (task.id == newTask.id) {
            task.application = payload.newItem.name;
            task.applicationId = payload.newItem.id;
            return;
          }
        });
      });
    },
  },
  actions: {
    saveTask(context, newItem) {
      context.commit('addTask', { newItem: newItem });
    },
    updateTask(context, payload) {
      context.commit('putTask', {
        newItem: payload.newItem,
        index: payload.index,
      });
    },
    deleteTask(context, taskId) {
      context.commit('removeTask', { taskId: taskId });
    },
  },
};
