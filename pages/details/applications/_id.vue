<template>
  <div class="wrapper">
    <the-form-dialog
      @cancel-close="close"
      @save-new-item="save"
      item-type="Application"
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
      {{ warningMessage }}
    </the-delete-dialog>
    <v-card
      outlined
      elevation="10"
      width="1200"
      height="750"
      class="mx-auto mt-16 pa-10"
    >
      <v-row class="d-flex justify-space-between mb-12">
        <div class="d-flex flex-column">
          <v-card-actions class="d-flex align-center" style="gap: 20px">
            <v-btn @click="goBack">Go back</v-btn>
            <v-btn v-if="!cameFromApplications" @click="goToApplications"
              >Go to applications</v-btn
            >
          </v-card-actions>
          <v-card-title primary-title class="text-h2 mb-4">
            {{ appDetails.name }}
          </v-card-title>
          <v-card-subtitle class="text-h5 mt-2">
            Creation date: {{ appDetails.date }}
          </v-card-subtitle>
          <v-card-subtitle class="text-h5">
            Edition date: {{ appDetails.edition_date }}
          </v-card-subtitle>
          <v-card-text>
            <nuxt-link
              class="text-h6"
              :to="'/details/servers/' + appDetails.serverId"
            >
              Server: {{ appDetails.server }}
            </nuxt-link>
          </v-card-text>
        </div>
        <div height="200" class="d-flex align-center">
          <v-card-actions
            class="action__buttons d-flex flex-column"
            style="gap: 20px"
          >
            <v-btn color="info" block large @click="updateApplication">
              UPDATE APPLICATION
            </v-btn>
            <v-btn
              color="error"
              class="ml-0"
              block
              large
              @click="deleteApplication"
            >
              DELETE APPLICATION
            </v-btn>
          </v-card-actions>
        </div>
      </v-row>
      <v-row>
        <v-data-table
          :headers="headers"
          height="250px"
          :items="appDetails.tasks"
          class="full__width"
        >
          <template v-slot:item="{ item }">
            <tr @click="showTaskDetails(item.id)" class="clickable__row">
              <td>{{ item.name }}</td>
              <td>{{ item.date }}</td>
              <td>{{ item.edition_date }}</td>
              <td>{{ item.server }}</td>
            </tr>
          </template>
        </v-data-table>
      </v-row>
    </v-card>
  </div>
</template>

<script>
import TheDeleteDialog from '@UI/components/TheDeleteDialog.vue';
import TheFormDialog from '@UI/components/TheFormDialog.vue';
import addNewAppForm from '@components/addNewAppForm.vue';
import { applicationDetailsHeaders } from '@core/constants/headers';

export default {
  components: { TheDeleteDialog, TheFormDialog, addNewAppForm },
  data() {
    return {
      warningMessage:
        'WARNING! This action will detach all tasks attached to this application!',
      tab: 0,
      cameFromApplications: false,
      dialogDelete: false,
      dialog: false,
      headers: applicationDetailsHeaders,
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
      return {
        id: application.id,
        name: application.name,
        date: application.date,
        edition_date: application.edition_date,
        server: this.$store.getters.getServers.find(
          (server) => server.id == application.serverId
        ).name,
        serverId: application.serverId,
        tasks: this.getTasksList(application.id),
      };
    },
  },
  watch: {
    appDetails(newValue) {
      this.updateEditedItem(newValue);
    },
  },
  methods: {
    getTasksList(appId) {
      return this.$store.getters.getTasks
        .filter((task) => task.applicationId == appId)
        .map((task) => {
          return {
            ...task,
            server: this.$store.getters.getServers.find(
              (server) => server.id == task.serverId
            ).name,
          };
        });
    },
    showTaskDetails(taskId) {
      this.$router.push('/details/tasks/' + taskId);
    },
    goBack() {
      this.$router.back();
    },
    goToApplications() {
      this.$router.push('/applications');
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
          edition_date: new Date().toISOString().split('T')[0],
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

<style scoped>
.action__buttons {
  gap: 20px;
  width: 300px;
}

.full__width {
  width: 100%;
}
</style>
