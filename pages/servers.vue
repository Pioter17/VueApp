<template>
  <the-overview
    :items="servers"
    itemType="server"
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
    :headers="headers"
    :snackbar="snackbar"
    :snackMessage="snackbarMessage"
  ></the-overview>
</template>

<script>
import { generateID } from '@pages/utils/functions/id-generator';
import TheOverview from '@UI/components/TheOverview.vue';
import { getServersHeaders } from '@core/constants/headers';

export default {
  components: { TheOverview },
  data() {
    return {
      isNew: true,
      warningMessage: 'serversPage.warning',
      headers: getServersHeaders(this.$i18n),
      editedIndex: -1,
      editedItem: {
        itemName: '',
      },
      defaultItem: {
        itemName: '',
      },
      snackbar: false,
      snackbarMessage: '',
    };
  },
  provide() {
    return {
      backLink: '/details/servers/',
      itemType: 'server',
      fetchFunction: 'fetchServers',
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
  watch: {
    '$i18n.locale': 'localeChanged',
  },
  methods: {
    setTVar() {
      this.$i18n.locale = this.$store.getters.getLocale;
    },
    localeChanged() {
      this.headers = getServersHeaders(this.$i18n);
    },
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
    async save() {
      if (this.editedIndex != -1) {
        const serverToUpdate = {
          id: this.servers[this.editedIndex].id,
          name: this.editedItem.itemName.trim(),
          date: this.servers[this.editedIndex].date,
          edition: new Date().toISOString().split('T')[0],
        };
        this.$store.dispatch('updateServer', {
          newItem: serverToUpdate,
          index: this.editedIndex,
        });
      } else {
        const newServer = {
          id: generateID(30),
          name: this.editedItem.itemName.trim(),
          date: new Date().toISOString().split('T')[0],
          edition: new Date().toISOString().split('T')[0],
        };
        try {
          await this.$store.dispatch('saveServer', newServer);
        } catch (error) {
          this.snackbar = true;
          this.snackbarMessage = error.message;
        }
      }
      this.close();
    },
    fetchData() {
      this.$store.dispatch('fetchAllTasks');
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
