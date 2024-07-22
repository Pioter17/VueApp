<template>
  <the-overview
    :items="apps"
    itemType="Application"
    deleteActionName="deleteApplication"
    :defaultItem="defaultItem"
    :itemToEdit="editedItem"
    :save="save"
    @edit-item="editItem"
    @close-dialog="close"
    :lastColumn="lastColumn"
    :secondLastColumn="secondLastColumn"
    :warning="warningMessage"
    :isNew="isNew"
  ></the-overview>
</template>

<script>
import { generateID } from './utils/functions/id-generator';
import TheOverview from '@UI/components/TheOverview.vue';

export default {
  components: { TheOverview },
  data() {
    return {
      isNew: true,
      warningMessage:
        'WARNING! This action will detach all tasks attached to this application!',
      headers: [
        { text: 'Name', value: 'name' },
        { text: 'Creation date', value: 'date' },
        { text: 'Edition date', value: 'edition_date' },
        { text: 'Server', value: 'server' },
        { text: 'Tasks' },
        { text: ' ' },
      ],
      editedIndex: -1,
      editedItem: {
        itemName: '',
        attachedServer: null,
        attachedTasks: [],
      },
      defaultItem: {
        itemName: '',
        attachedServer: null,
        attachedTasks: [],
      },
    };
  },
  provide() {
    return {
      backLink: '/details/applications/',
      itemType: 'Application',
      headers: this.headers,
    };
  },
  computed: {
    apps() {
      let i = 0;
      const data = this.$store.getters.getApps;
      const newData = [];
      data.forEach((element) => {
        element = {
          ...element,
          server: this.$store.getters.getServers.find(
            (server) => server.id == element.serverId
          ).name,
          count: i,
        };
        newData.push(element);
        i++;
      });
      return newData;
    },
    tasks() {
      return this.apps.map((app) => {
        return this.$store.getters.getTasks.filter(
          (task) => task.applicationId == app.id
        ).length;
      });
    },
  },
  methods: {
    lastColumn(item) {
      return this.tasks[item.count];
    },
    secondLastColumn(item) {
      return item.server;
    },
    editItem(item) {
      this.editedIndex = this.apps.indexOf(item);
      if (this.editedIndex != -1) {
        this.isNew = false;
      }
      const itemToEdit = this.apps[this.editedIndex];
      this.editedItem.itemName = itemToEdit.name;
      this.editedItem.attachedServer = this.$store.getters.getServers.find(
        (server) => server.id == itemToEdit.serverId
      );
      this.editedItem.attachedTasks = this.$store.getters.getTasks.filter(
        (task) => task.applicationId == item.id
      );
      this.dialog = true;
    },
    close() {
      this.dialog = false;
      this.isNew = true;
      this.editedIndex = -1;
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem);
        this.editedIndex = -1;
      });
    },
    save() {
      if (this.editedIndex != -1) {
        const appToUpdate = {
          id: this.apps[this.editedIndex].id,
          name: this.editedItem.itemName,
          date: this.apps[this.editedIndex].date,
          edition_date: new Date().toISOString().split('T')[0],
          server: this.editedItem.attachedServer.name,
          serverId: this.editedItem.attachedServer.id,
          tasks: this.editedItem.attachedTasks,
        };
        this.$store.dispatch('updateApplication', {
          newItem: appToUpdate,
          index: this.editedIndex,
        });
      } else {
        const newApp = {
          id: generateID(30),
          name: this.editedItem.itemName,
          date: new Date().toISOString().split('T')[0],
          edition_date: new Date().toISOString().split('T')[0],
          server: this.editedItem.attachedServer.name,
          serverId: this.editedItem.attachedServer.id,
          tasks: this.editedItem.attachedTasks,
        };
        this.$store.dispatch('saveApplication', newApp);
      }
      this.close();
    },
  },
};
</script>
