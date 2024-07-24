<template>
  <v-form ref="form">
    <v-text-field
      v-model="editedItem.itemName"
      :label="$t('forms.taskName')"
      :rules="[
        (v) => !!v || $t('forms.taskName') + ' ' + $t('forms.required'),
        (v) => (v && v.trim() !== '') || $t('forms.noWhitespace'),
        (v) => checkName(v) || $t('forms.nameMustBeUnique'),
        (v) => v.length <= 15 || $t('forms.tooLong'),
      ]"
    ></v-text-field>
    <v-select
      v-model="editedItem.attachedServer"
      :items="servers"
      item-text="name"
      :label="$t('forms.attachToServer')"
      return-object
      :rules="[(v) => !!v || $t('forms.serverRequired')]"
    ></v-select>
    <v-select
      v-model="editedItem.attachedApplication"
      :items="applications"
      item-text="name"
      :label="$t('forms.attachToApplication')"
      return-object
      :no-data-text="$t('forms.emptyServer')"
    ></v-select>
  </v-form>
</template>

<script>
import { isUniqueName } from '@pages/utils/functions/check-unique-name';

export default {
  props: ['editedItem'],
  data() {
    return {
      oldName: this.editedItem.itemName,
    };
  },
  computed: {
    tasks() {
      return this.$store.getters.getTasks;
    },
    servers() {
      return this.$store.getters.getServers;
    },
    applications() {
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
    validateForm() {
      return this.$refs.form.validate();
    },
    checkName(value) {
      return isUniqueName(value, this.tasks, this.oldName);
    },
  },
};
</script>
