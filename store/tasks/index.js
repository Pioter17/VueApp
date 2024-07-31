import axios from 'axios';

export default {
  state() {
    return {
      tasks: [],
    };
  },
  getters: {
    getTasks(state) {
      return state.tasks;
    },
  },
  mutations: {
    setTasks(state, payload) {
      state.tasks = payload.tasks;
    },
    addTask(state, payload) {
      state.tasks.push(payload.newItem);
    },
    putTask(state, payload) {
      state.tasks.splice(payload.index, 1, payload.newItem);
    },
    removeTask(state, payload) {
      state.tasks = state.tasks.filter((task) => task.id != payload.taskId);
    },
    removeAllServerTasks(state, payload) {
      state.tasks = state.tasks.filter(
        (task) => task.serverId != payload.serverId
      );
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
    async fetchTasks(context) {
      try {
        const response = await axios.get('https://localhost:7092/api/Task');
        context.commit('setTasks', { tasks: response.data });
      } catch (error) {
        console.error('Error fetching servers:', error);
      }
    },
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
