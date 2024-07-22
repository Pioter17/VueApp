<template>
  <v-row>
    <the-form-dialog
      @cancel-close="close"
      @save-new-item="checkAndSave"
      :item-type="itemType"
      :dialog="dialog"
      :isNew="isNew"
    >
      <add-new-task-form
        v-if="itemType == 'Task'"
        ref="formComponent"
        :editedItem="editedItem"
      ></add-new-task-form>
      <add-new-app-form
        v-else-if="itemType == 'Application'"
        ref="formComponent"
        :editedItem="editedItem"
      ></add-new-app-form>
      <add-new-server-form
        v-else
        ref="formComponent"
        :editedItem="editedItem"
      ></add-new-server-form>
    </the-form-dialog>
    <the-delete-dialog
      @cancel-delete="closeDelete"
      @delete-confirm="deleteItemConfirm"
      :dialogDelete="dialogDelete"
      :itemName="editedItem.itemName"
    >
      {{ warning }}
    </the-delete-dialog>
    <v-col class="d-flex justify-center">
      <v-sheet
        outlined
        elevation="10"
        width="90vw"
        min-height="80vh"
        rounded="lg"
        class="pa-14 mt-16"
      >
        <div class="text-h4 mb-10">{{ itemType + 's' }}</div>
        <the-data-display-table
          @open-delete="deleteItem"
          @open-form="openEditForm"
          :openDialog="openDialog"
          :data="items"
          :lastColumn="lastColumn"
          :secondLastColumn="secondLastColumn"
        ></the-data-display-table>
      </v-sheet>
    </v-col>
  </v-row>
</template>

<script>
import TheDeleteDialog from '@UI/components/TheDeleteDialog.vue';
import TheFormDialog from '@UI/components/TheFormDialog.vue';
import AddNewTaskForm from '@components/addNewTaskForm.vue';
import TheOverview from '@UI/components/TheOverview.vue';
import TheDataDisplayTable from '@UI/components/TheDataDisplayTable.vue';
import AddNewAppForm from '@components/addNewAppForm.vue';
import AddNewServerForm from '@components/addNewServerForm.vue';

export default {
  components: {
    TheDeleteDialog,
    TheFormDialog,
    AddNewTaskForm,
    TheOverview,
    TheDataDisplayTable,
    AddNewAppForm,
    AddNewServerForm,
  },
  props: [
    'items',
    'itemType',
    'deleteActionName',
    'defaultItem',
    'itemToEdit',
    'save',
    'lastColumn',
    'secondLastColumn',
    'warning',
    'isNew',
  ],
  emits: ['edit-item', 'save-data', 'close-dialog'],
  data() {
    return {
      dialog: false,
      dialogDelete: false,
      editedIndex: -1,
    };
  },
  computed: {
    editedItem() {
      return this.itemToEdit;
    },
  },
  methods: {
    openDialog() {
      this.dialog = true;
    },
    deleteItem(item) {
      this.editedIndex = this.items[this.items.indexOf(item)].id;
      this.editedItem.itemName = this.items[this.items.indexOf(item)].name;
      this.dialogDelete = true;
    },
    deleteItemConfirm() {
      this.$store.dispatch(this.deleteActionName, this.editedIndex);
      this.closeDelete();
    },
    close() {
      this.dialog = false;
      this.editedIndex = -1;
      this.$emit('close-dialog');
    },
    closeDelete() {
      this.dialogDelete = false;
      this.$nextTick(() => {
        this.editedIndex = -1;
      });
    },
    openEditForm(item) {
      this.openDialog();
      this.$emit('edit-item', item);
    },
    checkAndSave() {
      const isValid = this.$refs.formComponent.validateForm();
      if (isValid) {
        this.save();
      }
      this.close();
    },
  },
};
</script>
