<template>
  <v-form ref="form" class="mt3">
    <v-text-field
      v-model="editedItem.itemName"
      :label="$t('forms.serverName')"
      :rules="[
        (v) => !!v || $t('forms.serverName') + ' ' + $t('forms.required'),
        (v) => (v && v.trim() !== '') || $t('forms.noWhitespace'),
        (v) => checkName(v) || $t('forms.nameMustBeUnique'),
        (v) => v.length <= 15 || $t('forms.tooLong'),
      ]"
    ></v-text-field>
    <br />
    <p>
      {{ $t('forms.serverInfo') }}
    </p>
  </v-form>
</template>

<script>
import { isUniqueName } from '@pages/utils/functions/check-unique-name';

export default {
  props: ['editedItem'],
  computed: {
    servers() {
      return this.$store.getters.getServers;
    },
  },
  methods: {
    validateForm() {
      return this.$refs.form.validate();
    },
    checkName(value) {
      return isUniqueName(value, this.servers);
    },
  },
};
</script>
