<template>
  <div class="wrapper">
    <the-form-dialog
      @cancel-close="close"
      @save-new-item="save"
      item-type="Task"
      :dialog="dialog"
      :isNew="false"
    >
      <add-new-task-form
        ref="formComponent"
        :editedItem="editedItem"
      ></add-new-task-form>
    </the-form-dialog>
    <the-delete-dialog
      @cancel-delete="closeDelete"
      @delete-confirm="deleteConfirm"
      :dialogDelete="dialogDelete"
      :itemName="taskDetails.name"
    ></the-delete-dialog>
    <v-card
      outlined
      elevation="10"
      width="800"
      height="500"
      class="mx-auto mt-16 pa-6"
    >
      <v-row>
        <v-col cols="8">
          <v-card-actions class="d-flex align-center" style="gap: 20px">
            <v-btn @click="goBack">Go back</v-btn>
            <v-btn v-if="!cameFromTasks" @click="goToTasks">Go to tasks</v-btn>
          </v-card-actions>
          <v-card-title primary-title class="text-h2 mb-4">
            {{ taskDetails.name }}
          </v-card-title>
          <v-card-subtitle class="text-h5 mt-6">
            Creation date: {{ taskDetails.date }}
          </v-card-subtitle>
          <v-card-subtitle class="text-h5">
            Edition date: {{ taskDetails.edition_date }}
          </v-card-subtitle>
          <v-card-text class="d-flex flex-column">
            <nuxt-link
              class="text-h6"
              :to="'/details/servers/' + taskDetails.serverId"
            >
              Server: {{ taskDetails.server }}
            </nuxt-link>
            <nuxt-link
              class="text-h6"
              v-if="taskDetails.applicationId"
              :to="'/details/applications/' + taskDetails.applicationId"
            >
              Application: {{ taskDetails.application }}
            </nuxt-link>
            <div v-else>
              <p class="text-h6">
                This task is not attached to any application
              </p>
              <v-btn color="success">Attach to application</v-btn>
            </div>
          </v-card-text>
        </v-col>
        <v-col>
          <v-card-actions
            class="d-flex flex-column align-center justify-center"
            style="gap: 20px"
          >
            <v-btn color="info" @click="updateTask" block large>
              UPDATE TASK
            </v-btn>
            <v-btn color="error" class="ml-0" @click="deleteTask" block large>
              DELETE TASK
            </v-btn>
          </v-card-actions>
        </v-col>
      </v-row>
    </v-card>
  </div>
</template>

<script>
import TheDeleteDialog from '@UI/components/TheDeleteDialog.vue';
import TheFormDialog from '@UI/components/TheFormDialog.vue';
import addNewTaskForm from '@components/addNewTaskForm.vue';

export default {
  components: { TheDeleteDialog, TheFormDialog, addNewTaskForm },
  data() {
    return {
      tab: 0,
      cameFromTasks: false,
      dialogDelete: false,
      dialog: false,
      headers: [
        { text: 'Name', value: 'name' },
        { text: 'Date', value: 'date' },
        { text: 'Application', value: 'application' },
      ],
      editedItem: {
        itemName: '',
        attachedServer: null,
        attachedApplication: null,
      },
    };
  },
  computed: {
    taskDetails() {
      const task = this.$store.getters.getTasks.find(
        (task) => task.id == this.$route.params.id
      );
      task.server = this.$store.getters.getServers.find(
        (server) => server.id == task.serverId
      ).name;
      return task;
    },
    servers() {
      return this.$store.getters.getServers;
    },
  },
  watch: {
    taskDetails(newValue) {
      this.updateEditedItem(newValue);
    },
  },
  methods: {
    goBack() {
      this.$router.back();
    },
    goToTasks() {
      this.$router.push('/tasks');
    },
    setCameFromTasks(value) {
      this.cameFromTasks = value;
    },
    deleteTask() {
      this.dialogDelete = true;
    },
    deleteConfirm() {
      this.$store.dispatch('deleteTask', this.taskDetails.id);
      this.$router.back();
    },
    closeDelete() {
      this.dialogDelete = false;
    },
    close() {
      this.dialog = false;
    },
    updateTask() {
      this.dialog = true;
    },
    updateEditedItem(newValue) {
      if (newValue != null) {
        this.editedItem = {
          itemName: newValue.name,
          attachedServer: this.$store.getters.getServers.find(
            (server) => server.id == newValue.serverId
          ),
          attachedApplication: this.$store.getters.getApps.find(
            (application) => application.id == newValue.applicationId
          ),
        };
      } else {
        this.editedItem = {
          itemName: '',
          attachedServer: null,
          attachedApplication: null,
        };
      }
    },
    save() {
      const isValid = this.$refs.formComponent.validateForm();
      if (isValid) {
        const taskToUpdate = {
          id: this.taskDetails.id,
          name: this.editedItem.itemName,
          date: this.taskDetails.date,
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
          index: this.$store.getters.getTasks.indexOf(this.taskDetails),
        });
        this.close();
      }
    },
  },
  beforeRouteEnter(_, from, next) {
    next((vm) => {
      if (from.fullPath == '/tasks') {
        vm.setCameFromTasks(true);
      } else {
        vm.setCameFromTasks(false);
      }
    });
  },
  mounted() {
    this.updateEditedItem(this.taskDetails);
  },
};
</script>
