<template>
  <v-dialog v-model="dialogDelete" max-width="500px">
    <v-card>
      <v-card-title class="text-h5"
        >Are you sure you want to delete {{ itemName }}?</v-card-title
      >
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="blue darken-1" text @click="closeDelete">Cancel</v-btn>
        <v-btn color="blue darken-1" text @click="deleteConfirm">Delete</v-btn>
        <v-spacer></v-spacer>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  props: ['dialogDelete', 'itemName'],
  emits: ['cancel-delete', 'delete-confirm'],
  methods: {
    closeDelete() {
      this.$emit('cancel-delete');
    },
    deleteConfirm() {
      this.$emit('delete-confirm');
    },
  },
};
</script>

<style></style>
