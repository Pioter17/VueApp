<template>
  <v-row>
    <v-col class="d-flex justify-center">
      <v-sheet
        outlined
        elevation="10"
        width="90vw"
        min-height="80vh"
        rounded="lg"
        class="pa-14 mt-16"
      >
        <div class="text-h4 mb-10">Tasks</div>
        <v-data-table :headers="headers" height="60vh" :items="items">
          <template v-slot:top>
            <v-toolbar flat>
              <v-spacer></v-spacer>
              <v-btn color="primary" dark class="mb-10" @click="openDialog">
                Add new {{ itemType }}
              </v-btn>
              <the-form-dialog
                @cancel-close="close"
                @save-new-item="save"
                :item-type="itemType"
                :dialog="dialog"
              >
                <add-new-task-form
                  ref="formComponent"
                  :editedItem="editedItem"
                ></add-new-task-form>
              </the-form-dialog>
              <the-delete-dialog
                @cancel-delete="closeDelete"
                @delete-confirm="deleteItemConfirm"
                :dialogDelete="dialogDelete"
                :itemName="editedItem.itemName"
              ></the-delete-dialog>
            </v-toolbar>
          </template>
          <template v-slot:item="{ item }">
            <tr @click="handleRowClick(item.id)" class="clickable__row">
              <td>{{ item.name }}</td>
              <td>{{ item.date }}</td>
              <td>{{ item.edition_date }}</td>
              <td>{{ item.server }}</td>
              <td>
                {{
                  item.application
                    ? item.application
                    : 'Nie przypisano do aplikacji'
                }}
              </td>
              <td>
                <v-icon small class="mr-2" @click.stop="editItem(item)">
                  mdi-pencil
                </v-icon>
                <v-icon small @click.stop="deleteItem(item)">
                  mdi-delete
                </v-icon>
              </td>
            </tr>
          </template>
        </v-data-table>
      </v-sheet>
    </v-col>
  </v-row>
</template>

<script>
import TheDeleteDialog from '@UI/components/TheDeleteDialog.vue';
import { generateID } from './utils/functions/id-generator';
import TheFormDialog from '@UI/components/TheFormDialog.vue';
import AddNewTaskForm from '@components/addNewTaskForm.vue';

export default {
  components: { TheDeleteDialog, TheFormDialog, AddNewTaskForm },
  data() {
    return {
      dialog: false,
      dialogDelete: false,
      itemType: 'Task',
      headers: [
        { text: 'Name', value: 'name' },
        { text: 'Date', value: 'date' },
        { text: 'Edition date', value: 'edition_date' },
        { text: 'Server', value: 'server' },
        { text: 'Application', value: 'application' },
        { text: ' ' },
      ],
      editedIndex: -1,
      editedItem: {
        itemName: '',
        attachedServer: null,
        attachedApplication: null,
      },
      defaultItem: {
        itemName: '',
        attachedServer: null,
        attachedApplication: null,
      },
    };
  },
  computed: {
    items() {
      return this.$store.getters.getTasks;
    },
  },
  methods: {
    handleRowClick(id) {
      this.$router.push('/details/tasks/' + id);
    },
    openDialog() {
      this.dialog = true;
    },
    editItem(item) {
      this.editedIndex = this.items.indexOf(item);
      const itemToEdit = this.items[this.editedIndex];
      this.editedItem.itemName = itemToEdit.name;
      this.editedItem.attachedServer = this.$store.getters.getServers.find(
        (server) => server.id == itemToEdit.serverId
      );
      this.editedItem.attachedApplication = this.$store.getters.getApps.find(
        (application) => application.id == itemToEdit.applicationId
      );
      this.dialog = true;
    },
    deleteItem(item) {
      this.editedIndex = this.items[this.items.indexOf(item)].id;
      this.editedItem.itemName = this.items[this.items.indexOf(item)].name;
      this.dialogDelete = true;
    },
    deleteItemConfirm() {
      this.$store.dispatch('deleteTask', this.editedIndex);
      this.closeDelete();
    },
    close() {
      this.dialog = false;
      this.editedIndex = -1;
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem);
        this.editedIndex = -1;
      });
    },
    closeDelete() {
      this.dialogDelete = false;
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem);
        this.editedIndex = -1;
      });
    },
    save() {
      const isValid = this.$refs.formComponent.validateForm();
      if (isValid) {
        if (this.editedIndex != -1) {
          const taskToUpdate = {
            id: this.items[this.editedIndex].id,
            name: this.editedItem.itemName,
            date: this.items[this.editedIndex].date,
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
            index: this.editedIndex,
          });
        } else {
          const newTask = {
            id: generateID(30),
            name: this.editedItem.itemName,
            date: new Date().toISOString().split('T')[0],
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
          this.$store.dispatch('saveTask', newTask);
        }
        this.close();
      }
    },
  },
};
</script>

<style lang="scss" scoped>
.v-data-table > .v-data-table__wrapper > table > tbody > tr > th,
.v-data-table > .v-data-table__wrapper > table > thead > tr > th,
.v-data-table > .v-data-table__wrapper > table > tfoot > tr > th {
  font-size: 20px !important;
}
</style>
