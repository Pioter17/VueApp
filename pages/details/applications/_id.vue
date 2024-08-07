<template>
  <div class="wrapper">
    <the-form-dialog
      @cancel-close="close"
      @save-new-item="save"
      item-type="application"
      :dialog="dialog"
      :isNew="false"
    >
      <add-new-app-form
        ref="formComponent"
        :editedItem="editedItem"
      ></add-new-app-form>
    </the-form-dialog>
    <the-delete-dialog
      @cancel-delete="closeDelete"
      @delete-confirm="deleteConfirm"
      :dialogDelete="dialogDelete"
      :itemName="appDetails.name"
    >
      {{ $t(warningMessage) }}
    </the-delete-dialog>
    <the-details-display-card
      :itemDetails="appDetails"
      goToDest="/applications"
      :cameFrom="cameFromApplications"
      goToMessage="goToApplications"
      @delete-item="deleteApplication"
      @update-item="updateApplication"
      updateItemMessage="updateApplication"
      deleteItemMessage="deleteApplication"
    >
      <template #default>
        <v-card-text>
          <nuxt-link
            class="text-h6"
            :to="'/details/servers/' + appDetails.serverId"
          >
            {{ $t('server') }}: {{ appDetails.server }}
          </nuxt-link>
        </v-card-text>
      </template>
      <template #secondRow>
        <app-details :headers="headers" :appDetails="appDetails"></app-details>
      </template>
    </the-details-display-card>
  </div>
</template>

<script>
import TheDeleteDialog from '@UI/components/TheDeleteDialog.vue';
import TheFormDialog from '@UI/components/TheFormDialog.vue';
import addNewAppForm from '@components/addNewAppForm.vue';
import { getApplicationDetailsHeaders } from '@/core/constants/headers';
import TheDetailsDisplayCard from '@UI/components/TheDetailsDisplayCard.vue';
import AppDetails from '@components/appDetails.vue';

export default {
  components: {
    TheDeleteDialog,
    TheFormDialog,
    addNewAppForm,
    TheDetailsDisplayCard,
    AppDetails,
  },
  data() {
    return {
      warningMessage: 'applicationsPage.warning',
      tab: 0,
      cameFromApplications: false,
      dialogDelete: false,
      dialog: false,
      headers: getApplicationDetailsHeaders(this.$i18n),
      editedItem: {
        itemName: '',
        attachedServer: null,
        attachedTasks: [],
      },
    };
  },
  computed: {
    appDetails() {
      const application = this.$store.getters.getApps.find(
        (app) => app.id == this.$route.params.id
      );
      if (application) {
        return {
          id: application.id,
          name: application.name,
          date: application.date,
          edition_date: application.edition_date,
          server: this.$store.getters.getServers.find(
            (server) => server.id == application.serverId
          )?.name,
          serverId: application.serverId,
          tasks: this.getTasksList(application.id),
        };
      } else {
        return {};
      }
    },
  },
  watch: {
    appDetails(newValue) {
      this.updateEditedItem(newValue);
    },
    '$i18n.locale': 'localeChanged',
  },
  methods: {
    setTVar() {
      this.$i18n.locale = this.$store.getters.getLocale;
    },
    localeChanged() {
      this.headers = getApplicationDetailsHeaders(this.$i18n);
    },
    getTasksList(appId) {
      return this.$store.getters.getTasks
        .filter((task) => task.applicationId == appId)
        .map((task) => {
          return {
            ...task,
            server: this.$store.getters.getServers.find(
              (server) => server.id == task.serverId
            )?.name,
          };
        });
    },
    setCameFromApplications(value) {
      this.cameFromApplications = value;
    },
    deleteApplication() {
      this.dialogDelete = true;
    },
    deleteConfirm() {
      this.$store.dispatch('deleteApplication', this.appDetails.id);
      this.$router.back();
    },
    closeDelete() {
      this.dialogDelete = false;
    },
    close() {
      this.dialog = false;
    },
    updateApplication() {
      this.dialog = true;
    },
    updateEditedItem(newValue) {
      if (newValue != null) {
        this.editedItem = {
          itemName: newValue.name,
          attachedServer: this.$store.getters.getServers.find(
            (server) => server.id == newValue.serverId
          ),
          attachedTasks: this.$store.getters.getTasks.filter(
            (task) => task.applicationId == this.appDetails.id
          ),
        };
      } else {
        this.editedItem = {
          itemName: '',
          attachedServer: null,
          attachedTasks: [],
        };
      }
    },
    save() {
      const isValid = this.$refs.formComponent.validateForm();
      if (isValid) {
        const appToUpdate = {
          id: this.appDetails.id,
          name: this.editedItem.itemName,
          date: this.appDetails.date,
          edition: new Date().toISOString().split('T')[0],
          server: this.editedItem.attachedServer.name,
          serverId: this.editedItem.attachedServer.id,
          tasks: this.editedItem.attachedTasks,
        };
        this.$store.dispatch('updateApplication', {
          newItem: appToUpdate,
          index: this.$store.getters.getApps.indexOf(this.appDetails),
        });
        this.close();
      }
    },
  },
  beforeRouteEnter(_, from, next) {
    next((vm) => {
      vm.setTVar();
      if (from.fullPath == '/applications') {
        vm.setCameFromApplications(true);
      } else {
        vm.setCameFromApplications(false);
      }
    });
  },
  mounted() {
    this.updateEditedItem(this.appDetails);
  },
};
</script>
