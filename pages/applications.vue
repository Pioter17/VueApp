<template>
  <the-overview
    :items="apps"
    itemType="application"
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
    :headers="headers"
  ></the-overview>
</template>

<script>
import { generateID } from '@pages/utils/functions/id-generator';
import TheOverview from '@UI/components/TheOverview.vue';
import { getApplicationsHeaders } from '@/core/constants/headers';

export default {
  components: { TheOverview },
  data() {
    return {
      isNew: true,
      warningMessage: 'applicationsPage.warning',
      headers: getApplicationsHeaders(this.$i18n),
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
      itemType: 'application',
      headers: this.headers,
      fetchFunction: 'fetchApps',
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
          )?.name,
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
  watch: {
    '$i18n.locale': 'localeChanged',
    '$store.getters.getLocale': 'setTVar',
  },
  methods: {
    setTVar() {
      this.$i18n.locale = this.$store.getters.getLocale;
    },
    localeChanged() {
      this.headers = getApplicationsHeaders(this.$i18n);
    },
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
          name: this.editedItem.itemName.trim(),
          date: this.apps[this.editedIndex].date,
          edition: new Date().toISOString().split('T')[0],
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
          name: this.editedItem.itemName.trim(),
          date: new Date().toISOString().split('T')[0],
          edition: new Date().toISOString().split('T')[0],
          server: this.editedItem.attachedServer.name,
          serverId: this.editedItem.attachedServer.id,
          tasks: this.editedItem.attachedTasks,
        };
        this.$store.dispatch('saveApplication', newApp);
      }
      this.close();
    },
  },
  beforeRouteEnter(_, from, next) {
    next((vm) => {
      vm.setTVar();
    });
  },
};
</script>
