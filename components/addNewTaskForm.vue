<template>
  <v-form ref="form">
    <v-text-field
      v-model="editedItem.itemName"
      label="Task name"
      :rules="[(v) => !!v || 'Task name is required']"
    ></v-text-field>
    <v-select
      v-model="editedItem.attachedServer"
      :items="servers"
      item-text="name"
      label="Attach to server"
      return-object
      :rules="[(v) => !!v || 'Server selection is required']"
      @change="changeServer"
    ></v-select>
    <v-select
      v-model="editedItem.attachedApplication"
      :items="applications"
      item-text="name"
      label="Attach to application"
      return-object
      no-data-text="This server doesn't have any applications attached"
    ></v-select>
  </v-form>
</template>

<script>
export default {
  props: ['editedItem', 'applications', 'servers'],
  emits: ['server-changed'],
  methods: {
    validateForm() {
      return this.$refs.form.validate();
    },
    changeServer() {
      this.$emit('server-changed');
    },
  },
};
</script>
