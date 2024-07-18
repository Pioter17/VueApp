<template>
  <v-form ref="form">
    <v-text-field
      v-model="editedItem.itemName"
      label="Application name"
      :rules="[(v) => !!v || 'Application name is required']"
    ></v-text-field>
    <v-select
      v-model="editedItem.attachedServer"
      :items="servers"
      item-text="name"
      label="Attach to server"
      return-object
      :rules="[(v) => !!v || 'Server selection is required']"
    ></v-select>
    <v-select
      v-model="editedItem.attachedTasks"
      :items="tasks"
      item-text="name"
      label="Attach tasks"
      multiple
      return-object
      no-data-text="This server doesn't have any tasks attached"
    ></v-select>
  </v-form>
</template>

<script>
export default {
  props: ['editedItem'],
  computed: {
    servers() {
      return this.$store.getters.getServers;
    },
    tasks() {
      if (this.editedItem.attachedServer != null) {
        return this.$store.getters.getTasks.filter(
          (task) => task.serverId == this.editedItem.attachedServer.id
        );
      } else {
        return [];
      }
    },
  },
  methods: {
    validateForm() {
      return this.$refs.form.validate();
    },
  },
};
</script>
