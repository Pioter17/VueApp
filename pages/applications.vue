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
        <div class="text-h4 mb-10">Applications</div>
        <v-data-table :headers="headers" height="60vh" :items="apps">
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
                <add-new-app-form
                  ref="formComponent"
                  :editedItem="editedItem"
                ></add-new-app-form>
              </the-form-dialog>
              <the-delete-dialog
                @cancel-delete="closeDelete"
                @delete-confirm="deleteItemConfirm"
                :dialogDelete="dialogDelete"
                :itemName="editedItem.itemName"
              >
                {{ warningMessage }}
              </the-delete-dialog>
            </v-toolbar>
          </template>
          <template v-slot:item="{ item }">
            <tr @click="handleRowClick(item.id)" class="clickable__row">
              <td>{{ item.name }}</td>
              <td>{{ item.date }}</td>
              <td>{{ item.edition_date }}</td>
              <td>{{ item.server }}</td>
              <td>{{ tasks[item.count] }}</td>
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
import addNewAppFormVue from '@components/addNewAppForm.vue';

export default {
  components: { TheDeleteDialog, TheFormDialog, addNewAppFormVue },
  data() {
    return {
      dialog: false,
      dialogDelete: false,
      itemType: 'Application',
      warningMessage:
        'WARNING! This action will detach all tasks attached to this application!',
      headers: [
        { text: 'Name', value: 'name' },
        { text: 'Creation date', value: 'date' },
        { text: 'Edition date', value: 'edition_date' },
        { text: 'Server', value: 'server' },
        { text: 'Tasks' },
        { text: ' ' },
      ],
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
  computed: {
    apps() {
      let i = 0;
      const data = this.$store.getters.getApps;
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
    tasks() {
      return this.apps.map((app) => {
        return this.$store.getters.getTasks.filter(
          (task) => task.applicationId == app.id
        ).length;
      });
    },
    servers() {
      return this.$store.getters.getServers;
    },
    availableTasks() {
      if (this.editedItem.attachedServer != null) {
        return this.$store.getters.getApps.filter(
          (app) => app.serverId == this.editedItem.attachedServer.id
        );
      } else {
        return [];
      }
    },
  },
  methods: {
    handleRowClick(id) {
      this.$router.push('/details/applications/' + id);
    },
    openDialog() {
      this.dialog = true;
    },
    editItem(item) {
      this.editedIndex = this.apps.indexOf(item);
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
    deleteItem(item) {
      this.editedIndex = this.apps[this.apps.indexOf(item)].id;
      this.editedItem.itemName = this.apps[this.apps.indexOf(item)].name;
      this.dialogDelete = true;
    },
    deleteItemConfirm() {
      this.$store.dispatch('deleteApplication', this.editedIndex);
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
          const appToUpdate = {
            id: this.apps[this.editedIndex].id,
            name: this.editedItem.itemName,
            date: this.apps[this.editedIndex].date,
            edition_date: new Date().toISOString().split('T')[0],
            server: this.editedItem.attachedServer.name,
            serverId: this.editedItem.attachedServer.id,
            tasks: this.editedItem.attachedTasks,
          };
          this.$store.dispatch('updateApplication', {
            newItem: appToUpdate,
            index: appToUpdate.id,
          });
        } else {
          const newApp = {
            id: generateID(30),
            name: this.editedItem.itemName,
            date: new Date().toISOString().split('T')[0],
            edition_date: new Date().toISOString().split('T')[0],
            server: this.editedItem.attachedServer.name,
            serverId: this.editedItem.attachedServer.id,
            tasks: this.editedItem.attachedTasks,
          };
          this.$store.dispatch('saveApplication', newApp);
        }
        this.close();
      }
    },
  },
};
</script>
