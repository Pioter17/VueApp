<template>
  <v-form ref="form" class="mt3">
    <v-text-field
      v-model="editedItem.itemName"
      label="Server name"
      :rules="[(v) => !!v || 'Server name is required']"
    ></v-text-field>
    <br />
    <p>
      You can only set the server name here. To attach applications and/or task
      to it, go to applications and/or tasks pages. This solution is to prevent
      some unintentional actions, like detaching all tasks from another server,
      because of reattaching the application they are connected to.
    </p>
  </v-form>
</template>

<script>
export default {
  props: ['editedItem'],
  methods: {
    validateForm() {
      return this.$refs.form.validate();
    },
  },
};
</script>
