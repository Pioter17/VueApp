<template>
  <div class="wrapper">
    <v-snackbar v-model="snackbar" :timeout="3000" color="error">
      {{ snackbarMessage }}
    </v-snackbar>
    <the-form-dialog
      @cancel-close="close"
      @save-new-item="save"
      item-type="task"
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
    <the-details-display-card
      :itemDetails="taskDetails"
      goToDest="/tasks"
      :cameFrom="cameFromTasks"
      goToMessage="goToTasks"
      @delete-item="deleteTask"
      @update-item="updateTask"
      updateItemMessage="updateTask"
      deleteItemMessage="deleteTask"
    >
      <task-details :taskDetails="taskDetails"></task-details>
    </the-details-display-card>
  </div>
</template>

<script>
import TheDeleteDialog from '@UI/components/TheDeleteDialog.vue';
import TheFormDialog from '@UI/components/TheFormDialog.vue';
import addNewTaskForm from '@components/addNewTaskForm.vue';
import TheDetailsDisplayCard from '@UI/components/TheDetailsDisplayCard.vue';
import TaskDetails from '@components/taskDetails.vue';

export default {
  components: {
    TheDeleteDialog,
    TheFormDialog,
    addNewTaskForm,
    TheDetailsDisplayCard,
    TaskDetails,
  },
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
      snackbar: false,
      snackbarMessage: '',
    };
  },
  computed: {
    taskDetails() {
      const task = this.$store.getters.getTasks.find(
        (task) => task.id == this.$route.params.id
      );
      task.server = this.$store.getters.getServers.find(
        (server) => server.id == task.serverId
      )?.name;
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
    setTVar() {
      this.$i18n.locale = this.$store.getters.getLocale;
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
    async save() {
      const isValid = this.$refs.formComponent.validateForm();
      if (isValid) {
        const taskToUpdate = {
          id: this.taskDetails.id,
          name: this.editedItem.itemName,
          date: this.taskDetails.date,
          edition: new Date().toISOString().split('T')[0],
          server: this.editedItem.attachedServer.name,
          application: this.editedItem.attachedApplication
            ? this.editedItem.attachedApplication.name
            : '',
          serverId: this.editedItem.attachedServer.id,
          applicationId: this.editedItem.attachedApplication
            ? this.editedItem.attachedApplication.id
            : '',
        };
        try {
          await this.$store.dispatch('updateTask', {
            newItem: taskToUpdate,
            index: this.$store.getters.getTasks.indexOf(this.taskDetails),
          });
        } catch (error) {
          this.snackbar = true;
          this.snackbarMessage = error.message;
        }
        this.close();
      }
    },
  },
  beforeRouteEnter(_, from, next) {
    next((vm) => {
      vm.setTVar();
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
