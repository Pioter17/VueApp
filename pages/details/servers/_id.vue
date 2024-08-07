<template>
  <div class="wrapper">
    <the-form-dialog
      @cancel-close="close"
      @save-new-item="save"
      item-type="server"
      :dialog="dialog"
      :isNew="false"
    >
      <add-new-server-form
        ref="formComponent"
        :editedItem="editedItem"
      ></add-new-server-form>
    </the-form-dialog>
    <the-delete-dialog
      @cancel-delete="closeDelete"
      @delete-confirm="deleteConfirm"
      :dialogDelete="dialogDelete"
      :itemName="serverDetails.name"
    >
      {{ $t(warningMessage) }}
    </the-delete-dialog>
    <the-details-display-card
      :itemDetails="serverDetails"
      goToDest="/servers"
      :cameFrom="cameFromServers"
      goToMessage="goToServers"
      @delete-item="deleteServer"
      @update-item="updateServer"
      updateItemMessage="updateServer"
      deleteItemMessage="deleteServer"
    >
      <template #secondRow>
        <server-details
          :appHeaders="appHeaders"
          :taskHeaders="taskHeaders"
          :serverDetails="serverDetails"
        ></server-details>
      </template>
    </the-details-display-card>
  </div>
</template>

<script>
import TheDeleteDialog from '@UI/components/TheDeleteDialog.vue';
import TheFormDialog from '@UI/components/TheFormDialog.vue';
import addNewServerForm from '@components/addNewServerForm.vue';
import {
  getServerDetailsApplicationsHeaders,
  getServerDetailsTasksHeaders,
} from '@/core/constants/headers';
import TheDetailsDisplayCard from '@UI/components/TheDetailsDisplayCard.vue';
import ServerDetails from '@components/serverDetails.vue';

export default {
  components: {
    TheDeleteDialog,
    TheFormDialog,
    addNewServerForm,
    TheDetailsDisplayCard,
    ServerDetails,
  },
  data() {
    return {
      warningMessage: 'serversPage.warning',
      cameFromServers: false,
      dialogDelete: false,
      dialog: false,
      taskHeaders: getServerDetailsTasksHeaders(this.$i18n),
      appHeaders: getServerDetailsApplicationsHeaders(this.$i18n),
      editedItem: {
        itemName: '',
      },
    };
  },
  computed: {
    serverDetails() {
      const server = this.$store.getters.getServers.find(
        (server) => server.id == this.$route.params.id
      );
      if (server) {
        return {
          id: server.id,
          name: server.name,
          date: server.date,
          edition: server.edition_date,
          applications: this.getApplicationsList(server.id),
          tasks: this.getTasksList(server.id),
        };
      } else {
        return {};
      }
    },
  },
  watch: {
    serverDetails(newValue) {
      this.updateEditedItem(newValue);
    },
    '$i18n.locale': 'localeChanged',
    '$store.getters.getLocale': 'setTVar',
  },
  methods: {
    setTVar() {
      this.$i18n.locale = this.$store.getters.getLocale;
    },
    localeChanged() {
      this.taskHeaders = getServerDetailsTasksHeaders(this.$i18n);
      this.appHeaders = getServerDetailsApplicationsHeaders(this.$i18n);
    },
    getApplicationsList(serverId) {
      const apps = this.$store.getters.getApps.filter(
        (app) => app.serverId == serverId
      );
      apps.forEach((app) => {
        app.tasks = this.$store.getters.getTasks.filter(
          (task) => task.applicationId == app.id
        ).length;
      });
      return apps;
    },
    getTasksList(serverId) {
      return this.$store.getters.getTasks.filter(
        (task) => task.serverId == serverId
      );
    },
    setCameFromServers(value) {
      this.cameFromServers = value;
    },
    deleteServer() {
      this.dialogDelete = true;
    },
    deleteConfirm() {
      this.$store.dispatch('deleteServer', this.serverDetails.id);
      this.$router.back();
    },
    closeDelete() {
      this.dialogDelete = false;
    },
    close() {
      this.dialog = false;
    },
    updateServer() {
      this.dialog = true;
    },
    updateEditedItem(newValue) {
      if (newValue != null) {
        this.editedItem = {
          itemName: newValue.name,
        };
      } else {
        this.editedItem = {
          itemName: '',
        };
      }
    },
    save() {
      const isValid = this.$refs.formComponent.validateForm();
      if (isValid) {
        const serverToUpdate = {
          id: this.serverDetails.id,
          name: this.editedItem.itemName,
          date: this.serverDetails.date,
          edition_date: new Date().toISOString().split('T')[0],
        };
        this.$store.dispatch('updateServer', {
          newItem: serverToUpdate,
          index: this.$store.getters.getServers.findIndex(
            (server) => server.id == this.serverDetails.id
          ),
        });
        this.close();
      }
    },
  },
  beforeRouteEnter(_, from, next) {
    next((vm) => {
      vm.setTVar();
      if (from.fullPath == '/servers') {
        vm.setCameFromServers(true);
      } else {
        vm.setCameFromServers(false);
      }
    });
  },
  mounted() {
    this.updateEditedItem(this.serverDetails);
  },
};
</script>
