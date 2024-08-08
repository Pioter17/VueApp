import axios from 'axios';

export default {
  state() {
    return {
      tasks: [],
      totalTaskItems: 0,
    };
  },
  getters: {
    getTasks(state) {
      return state.tasks;
    },
    getTotalTaskItems(state) {
      return state.totalTaskItems;
    },
  },
  mutations: {
    setTotalTaskItems(state, payload) {
      state.totalTaskItems = payload.totalItems;
    },
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
    async fetchAllTasks(context) {
      try {
        const response = await axios.get('https://localhost:7092/api/Task');
        context.commit('setTasks', { tasks: response.data });
        context.commit('setTotalTaskItems', {
          totalItems: response.data.length,
        });
      } catch (error) {
        console.error('Error fetching Tasks:', error);
      }
    },
    async fetchTasks(context, data) {
      try {
        const pagination = data.pagination;
        const search = data.search;
        const response = await axios.get(
          'https://localhost:7092/api/Task/paginated-tasks',
          {
            params: {
              pageNumber: pagination[0],
              pageSize: context.getters.getItemsPerPage,
              serverName: search[0],
              applicationName: search[1],
              taskName: search[2],
              sortBy: data.sortBy,
              sortDesc: data.sortDesc,
            },
          }
        );
        context.commit('setTasks', { tasks: response.data.tasks });
        context.commit('setTotalTaskItems', {
          totalItems: response.data.totalItems,
        });
      } catch (error) {
        console.error('Error fetching Tasks:', error);
      }
    },
    async saveTask(context, newItem) {
      try {
        await axios
          .post('https://localhost:7092/api/Task', newItem)
          .then(function (response) {
            console.log(response);
            context.dispatch('fetchTasks', {
              pagination: [
                context.getters.getCurrentPage,
                context.getters.getItemsPerPage,
              ],
              search: ['', '', ''],
              sortBy: [],
              sortDesc: [],
            });
            context.commit('setItemsPerPage', { itemsPerPage: 10 });
          });
      } catch (error) {
        if (error.response.status == 409) {
          throw new Error(error.response.data.message);
        } else {
          throw new Error('An unexpected error occurred');
        }
      }
    },
    async updateTask(context, payload) {
      try {
        await axios
          .put('https://localhost:7092/api/Task', payload.newItem)
          .then(function (response) {
            console.log(response);
            context.commit('putTask', {
              newItem: payload.newItem,
              index: payload.index,
            });
          });
      } catch (error) {
        if (error.response.status == 409) {
          throw new Error(error.response.data.message);
        } else {
          throw new Error('An unexpected error occurred');
        }
      }
    },
    deleteTask(context, taskId) {
      context.commit('removeTask', { taskId: taskId });
      axios
        .delete('https://localhost:7092/api/Task?id=' + taskId)
        .then(function (response) {
          console.log(response);
          context.dispatch('fetchTasks', {
            pagination: [
              context.getters.getCurrentPage,
              context.getters.getItemsPerPage,
            ],
            search: ['', '', ''],
            sortBy: [],
            sortDesc: [],
          });
        })
        .catch(function (error) {
          console.log(error);
        });
    },
  },
};
