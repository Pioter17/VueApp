<template>
  <v-dialog v-model="dialogDelete" max-width="700px" max-height="400px">
    <v-card>
      <v-card-title class="text-h5">
        {{ $t('deleteConfirm') }} {{ itemName }}?
      </v-card-title>
      <v-card-subtitle class="mt-2">
        <slot></slot>
      </v-card-subtitle>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="blue darken-1" text @click="closeDelete">{{
          $t('cancel')
        }}</v-btn>
        <v-btn color="blue darken-1" text @click="deleteConfirm">{{
          $t('delete')
        }}</v-btn>
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
