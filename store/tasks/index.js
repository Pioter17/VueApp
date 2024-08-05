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
      state.tasks.forEach((task) => {
        if (task.serverId == payload.serverId) {
          axios
            .delete('https://localhost:7092/api/Task?id=' + task.id)
            .then(function (response) {
              console.log(response);
            })
            .catch(function (error) {
              console.log(error);
            });
        }
      });
      state.tasks = state.tasks.filter(
        (task) => task.serverId != payload.serverId
      );
    },
    detachTasksFromApplication(state, payload) {
      state.tasks.forEach((task) => {
        if (task.applicationId == payload.appId) {
          task.application = '';
          task.applicationId = '';
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
    async fetchTasks(context, data) {
      try {
        const pagination = data.pagination;
        const search = data.search;
        const response = await axios.get('https://localhost:7092/api/Task', {
          params: {
            pageNumber: pagination[0],
            pageSize: pagination[1],
            serverName: search[0],
            applicationName: search[1],
            taskName: search[2],
          },
        });
        context.commit('setTasks', { tasks: response.data.tasks });
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
    saveTask(newItem) {
      axios
        .post('https://localhost:7092/api/Task', newItem)
        .then(function (response) {
          console.log(response);
        })
        .catch(function (error) {
          console.log(error);
        });
    },
    updateTask(context, payload) {
      context.commit('putTask', {
        newItem: payload.newItem,
        index: payload.index,
      });
      axios
        .put('https://localhost:7092/api/Task', payload.newItem)
        .then(function (response) {
          console.log(response);
        })
        .catch(function (error) {
          console.log(error);
        });
    },
    deleteTask(context, taskId) {
      context.commit('removeTask', { taskId: taskId });
      axios
        .delete('https://localhost:7092/api/Task?id=' + taskId)
        .then(function (response) {
          console.log(response);
        })
        .catch(function (error) {
          console.log(error);
        });
    },
  },
};
