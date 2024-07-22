<template>
  <the-overview
    :items="servers"
    itemType="Server"
    deleteActionName="deleteServer"
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
      warningMessage: `WARNING! This action will DELETE all applications and tasks attached to this server!\n
        Consider reattaching them first.
        Are you sure to delete this server?`,
      headers: [
        { text: 'Name', value: 'name' },
        { text: 'Creation date', value: 'date' },
        { text: 'Edition date', value: 'edition_date' },
        { text: 'Applications' },
        { text: 'Tasks' },
        { text: ' ', value: 'actions', sortable: false },
      ],
      editedIndex: -1,
      editedItem: {
        itemName: '',
      },
      defaultItem: {
        itemName: '',
      },
    };
  },
  provide() {
    return {
      backLink: '/details/servers/',
      itemType: 'Server',
      headers: this.headers,
    };
  },
  computed: {
    servers() {
      let i = 0;
      const data = this.$store.getters.getServers;
      const newData = [];
      data.forEach((element) => {
        element = {
          ...element,
          count: i,
        };
        newData.push(element);
        i++;
      });
      return newData;
    },
    applications() {
      return this.servers.map((server) => {
        return this.$store.getters.getApps.filter(
          (app) => app.serverId == server.id
        ).length;
      });
    },
    tasks() {
      return this.servers.map((server) => {
        return this.$store.getters.getTasks.filter(
          (task) => task.serverId == server.id
        ).length;
      });
    },
  },
  methods: {
    lastColumn(item) {
      return this.tasks[item.count];
    },
    secondLastColumn(item) {
      return this.applications[item.count];
    },
    editItem(item) {
      this.editedIndex = this.servers.indexOf(item);
      if (this.editedIndex != -1) {
        this.isNew = false;
      }
      const itemToEdit = this.servers[this.editedIndex];
      this.editedItem.itemName = itemToEdit.name;
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
        const serverToUpdate = {
          id: this.servers[this.editedIndex].id,
          name: this.editedItem.itemName,
          date: this.servers[this.editedIndex].date,
          edition_date: new Date().toISOString().split('T')[0],
        };
        this.$store.dispatch('updateServer', {
          newItem: serverToUpdate,
          index: this.editedIndex,
        });
      } else {
        const newServer = {
          id: generateID(30),
          name: this.editedItem.itemName,
          date: new Date().toISOString().split('T')[0],
          edition_date: new Date().toISOString().split('T')[0],
        };
        this.$store.dispatch('saveServer', newServer);
      }
      this.close();
    },
  },
};
</script>
