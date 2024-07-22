<template>
  <div class="wrapper">
    <the-form-dialog
      @cancel-close="close"
      @save-new-item="save"
      item-type="Server"
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
      {{ warningMessage }}
    </the-delete-dialog>
    <v-card
      outlined
      elevation="10"
      width="1200px"
      height="750px"
      class="mx-auto mt-16 pa-10"
    >
      <v-row class="d-flex justify-space-between mb-12">
        <div class="d-flex flex-column">
          <v-card-actions class="d-flex align-center" style="gap: 20px">
            <v-btn @click="goBack">Go back</v-btn>
            <v-btn v-if="!cameFromServers" @click="goToServers">
              Go to servers
            </v-btn>
          </v-card-actions>
          <v-card-title primary-title class="text-h2 mb-4">
            {{ serverDetails.name }}
          </v-card-title>
          <v-card-subtitle class="text-h5 mt-2">
            Creation date: {{ serverDetails.date }}
          </v-card-subtitle>
          <v-card-subtitle class="text-h5">
            Edition date: {{ serverDetails.edition_date }}
          </v-card-subtitle>
        </div>
        <div height="200" class="d-flex align-center">
          <v-card-actions
            class="action__buttons d-flex flex-column align-center justify-center"
          >
            <v-btn color="info" block large @click="updateServer">
              UPDATE SERVER
            </v-btn>
            <v-btn color="error" class="ml-0" block large @click="deleteServer">
              DELETE SERVER
            </v-btn>
          </v-card-actions>
        </div>
      </v-row>
      <v-row>
        <v-tabs v-model="tab" grow>
          <v-tab> Applications </v-tab>
          <v-tab> Tasks </v-tab>
        </v-tabs>
        <v-tabs-items class="full__width" v-model="tab">
          <v-tab-item>
            <v-data-table
              :headers="appHeaders"
              height="250px"
              :items="serverDetails.applications"
            >
              <template v-slot:item="{ item }">
                <tr @click="showAppDetails(item.id)" class="clickable__row">
                  <td>{{ item.name }}</td>
                  <td>{{ item.date }}</td>
                  <td>{{ item.edition_date }}</td>
                  <td>{{ item.tasks }}</td>
                </tr>
              </template>
            </v-data-table>
          </v-tab-item>
          <v-tab-item>
            <v-data-table
              :headers="taskHeaders"
              height="250px"
              :items="serverDetails.tasks"
            >
              <template v-slot:item="{ item }">
                <tr @click="showTaskDetails(item.id)" class="clickable__row">
                  <td>{{ item.name }}</td>
                  <td>{{ item.date }}</td>
                  <td>{{ item.edition_date }}</td>
                  <td>
                    {{
                      item.application
                        ? item.application
                        : 'Nie przypisano do aplikacji'
                    }}
                  </td>
                </tr>
              </template>
            </v-data-table>
          </v-tab-item>
        </v-tabs-items>
      </v-row>
    </v-card>
  </div>
</template>

<script>
import TheDeleteDialog from '@UI/components/TheDeleteDialog.vue';
import TheFormDialog from '@UI/components/TheFormDialog.vue';
import addNewServerForm from '@components/addNewServerForm.vue';
import {
  serverDetailsTasksHeaders,
  serverDetailsApplicationsHeaders,
} from '@core/constants/headers';

export default {
  components: { TheDeleteDialog, TheFormDialog, addNewServerForm },
  data() {
    return {
      tab: 0,
      warningMessage: `WARNING! This action will DELETE all applications and tasks attached to this server!\n
        Consider reattaching them first.
        Are you sure to delete this server?`,
      cameFromServers: false,
      dialogDelete: false,
      dialog: false,
      taskHeaders: serverDetailsTasksHeaders,
      appHeaders: serverDetailsApplicationsHeaders,
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
      return {
        id: server.id,
        name: server.name,
        date: server.date,
        edition_date: server.edition_date,
        applications: this.getApplicationsList(server.id),
        tasks: this.getTasksList(server.id),
      };
    },
  },
  watch: {
    serverDetails(newValue) {
      this.updateEditedItem(newValue);
    },
  },
  methods: {
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
    showAppDetails(appId) {
      this.$router.push('/details/applications/' + appId);
    },
    showTaskDetails(taskId) {
      this.$router.push('/details/tasks/' + taskId);
    },
    goBack() {
      this.$router.back();
    },
    goToServers() {
      this.$router.push('/servers');
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

<style scoped>
.action__buttons {
  gap: 20px;
  width: 300px;
}

.full__width {
  width: 100%;
}
</style>
