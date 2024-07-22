<template>
  <the-overview
    :items="items"
    itemType="Task"
    deleteActionName="deleteTask"
    :defaultItem="defaultItem"
    :itemToEdit="editedItem"
    :save="save"
    @edit-item="editItem"
    @close-dialog="close"
    :lastColumn="lastColumn"
    :secondLastColumn="secondLastColumn"
    :isNew="isNew"
  >
  </the-overview>
</template>

<script>
import { generateID } from './utils/functions/id-generator';
import TheOverview from '@UI/components/TheOverview.vue';
import AddNewTaskForm from '@components/addNewTaskForm.vue';

export default {
  components: { TheOverview, AddNewTaskForm },
  data() {
    return {
      isNew: true,
      headers: [
        { text: 'Name', value: 'name' },
        { text: 'Creation date', value: 'date' },
        { text: 'Edition date', value: 'edition_date' },
        { text: 'Server', value: 'server' },
        { text: 'Application', value: 'application' },
        { text: ' ' },
      ],
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
    };
  },
  provide() {
    return {
      backLink: '/details/tasks/',
      itemType: 'Task',
      headers: this.headers,
      filterFunction: this.filterTasks,
    };
  },
  computed: {
    items() {
      return this.$store.getters.getTasks.map((task) => {
        return {
          ...task,
          server: this.$store.getters.getServers.find(
            (server) => server.id == task.serverId
          ).name,
        };
      });
    },
  },
  methods: {
    lastColumn(item) {
      return item.application
        ? item.application
        : 'Nie przypisano do aplikacji';
    },
    secondLastColumn(item) {
      return item.server;
    },
    filterTasks(data, serverName, applicationName, taskName) {
      return data.filter((item) => {
        const searchApplicationNameMatch = String(item.application)
          .toLowerCase()
          .includes(applicationName.toLowerCase());
        const searchServerNameMatch = String(item.server)
          .toLowerCase()
          .includes(serverName.toLowerCase());
        const searchTaskNameMatch = item.name
          .toLowerCase()
          .includes(taskName.toLowerCase());
        return (
          searchApplicationNameMatch &&
          searchServerNameMatch &&
          searchTaskNameMatch
        );
      });
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
    save() {
      if (this.editedIndex != -1) {
        const taskToUpdate = {
          id: this.items[this.editedIndex].id,
          name: this.editedItem.itemName,
          date: this.items[this.editedIndex].date,
          edition_date: new Date().toISOString().split('T')[0],
          server: this.editedItem.attachedServer.name,
          application: this.editedItem.attachedApplication
            ? this.editedItem.attachedApplication.name
            : null,
          serverId: this.editedItem.attachedServer.id,
          applicationId: this.editedItem.attachedApplication
            ? this.editedItem.attachedApplication.id
            : null,
        };
        this.$store.dispatch('updateTask', {
          newItem: taskToUpdate,
          index: this.editedIndex,
        });
      } else {
        const newTask = {
          id: generateID(30),
          name: this.editedItem.itemName,
          date: new Date().toISOString().split('T')[0],
          edition_date: new Date().toISOString().split('T')[0],
          server: this.editedItem.attachedServer.name,
          application: this.editedItem.attachedApplication
            ? this.editedItem.attachedApplication.name
            : null,
          serverId: this.editedItem.attachedServer.id,
          applicationId: this.editedItem.attachedApplication
            ? this.editedItem.attachedApplication.id
            : null,
        };
        this.$store.dispatch('saveTask', newTask);
      }
    },
  },
};
</script>
