<template>
  <v-form ref="form" class="mt3">
    <v-text-field
      v-model="editedItem.itemName"
      :label="$t('forms.serverName')"
      :rules="[(v) => !!v || $t('forms.serverName') + $t('forms.required')]"
    ></v-text-field>
    <br />
    <p>
      {{ $t('forms.serverInfo') }}
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
