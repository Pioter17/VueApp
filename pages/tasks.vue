<template>
  <the-overview
    :items="items"
    itemType="task"
    deleteActionName="deleteTask"
    :defaultItem="defaultItem"
    :itemToEdit="editedItem"
    :save="save"
    @edit-item="editItem"
    @close-dialog="close"
    :lastColumn="lastColumn"
    :secondLastColumn="secondLastColumn"
    :isNew="isNew"
    :headers="headers"
    :snackbar="snackbar"
    :snackMessage="snackbarMessage"
  >
  </the-overview>
</template>

<script>
import { generateID } from '@pages/utils/functions/id-generator';
import TheOverview from '@UI/components/TheOverview.vue';
import { getTasksHeaders } from '@core/constants/headers';

export default {
  components: { TheOverview },
  data() {
    return {
      isNew: true,
      headers: getTasksHeaders(this.$i18n),
      editedIndex: -1,
      editedItem: {
        itemName: '',
        attachedServer: null,
        attachedApplication: null,
      },
      defaultItem: {
        itemName: '',
        attachedServer: null,
        attachedApplication: null,
      },
      snackbar: false,
      snackbarMessage: '',
    };
  },
  provide() {
    return {
      backLink: '/details/tasks/',
      itemType: 'task',
      headers: this.headers,
      fetchFunction: 'fetchTasks',
    };
  },
  computed: {
    items() {
      return this.$store.getters.getTasks.map((task) => {
        return {
          ...task,
          server: this.$store.getters.getServers.find(
            (server) => server.id == task.serverId
          )?.name,
        };
      });
    },
  },
  watch: {
    '$i18n.locale': 'localeChanged',
  },
  methods: {
    setTVar() {
      this.$i18n.locale = this.$store.getters.getLocale;
    },
    localeChanged() {
      this.headers = getTasksHeaders(this.$i18n);
    },
    lastColumn(item) {
      return item.application
        ? item.application
        : this.$t('tasksPage.taskNotAssigned');
    },
    secondLastColumn(item) {
      return item.server;
    },
    editItem(item) {
      this.editedIndex = this.items.indexOf(item);
      if (this.editedIndex != -1) {
        this.isNew = false;
      }
      const itemToBeEdited = this.items[this.editedIndex];
      this.editedItem.itemName = itemToBeEdited.name;
      this.editedItem.attachedServer = this.$store.getters.getServers.find(
        (server) => server.id == itemToBeEdited.serverId
      );
      this.editedItem.attachedApplication = this.$store.getters.getApps.find(
        (application) => application.id == itemToBeEdited.applicationId
      );
    },
    close() {
      this.isNew = true;
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem);
        this.editedIndex = -1;
      });
    },
    async save() {
      if (this.editedIndex != -1) {
        const taskToUpdate = {
          id: this.items[this.editedIndex].id,
          name: this.editedItem.itemName.trim(),
          date: this.items[this.editedIndex].date,
          edition: new Date().toISOString().split('T')[0],
          server: this.editedItem.attachedServer.name,
          application: this.editedItem.attachedApplication
            ? this.editedItem.attachedApplication.name
            : '',
          serverId: this.editedItem.attachedServer.id,
          applicationId: this.editedItem.attachedApplication
            ? this.editedItem.attachedApplication.id
            : '',
        };
        this.$store.dispatch('updateTask', {
          newItem: taskToUpdate,
          index: this.editedIndex,
        });
      } else {
        const newTask = {
          id: generateID(30),
          name: this.editedItem.itemName.trim(),
          date: new Date().toISOString().split('T')[0],
          edition: new Date().toISOString().split('T')[0],
          server: this.editedItem.attachedServer.name,
          application: this.editedItem.attachedApplication
            ? this.editedItem.attachedApplication.name
            : '',
          serverId: this.editedItem.attachedServer.id,
          applicationId: this.editedItem.attachedApplication
            ? this.editedItem.attachedApplication.id
            : '',
        };
        try {
          await this.$store.dispatch('saveTask', newTask);
        } catch (error) {
          this.snackbar = true;
          this.snackbarMessage = error.message;
        }
      }
    },
    fetchData() {
      this.$store.dispatch('fetchAllServers');
      this.$store.dispatch('fetchAllApps');
    },
  },
  beforeRouteEnter(_, from, next) {
    next((vm) => {
      vm.setTVar();
      vm.fetchData();
    });
  },
};
</script>
