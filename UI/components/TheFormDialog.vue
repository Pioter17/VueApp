<template>
  <v-dialog v-model="dialog" max-width="600px" persistent>
    <v-card>
      <v-card-title>
        <span class="text-h5 mt-2">
          {{ $t(isNewAction) }} {{ $t('itemType.' + itemType) }}
        </span>
      </v-card-title>
      <v-card-text>
        <v-container>
          <slot></slot>
        </v-container>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="blue darken-1" class="mb-2" text @click="close">
          {{ $t('cancel') }}
        </v-btn>
        <v-btn color="blue darken-1" class="mb-2" text @click="save">
          {{ $t('save') }}
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  props: ['itemType', 'dialog', 'isNew'],
  emits: ['cancel-close', 'save-new-item'],
  computed: {
    isNewAction() {
      return this.isNew ? 'addNew' : 'edit';
    },
  },
  methods: {
    close() {
      this.$emit('cancel-close');
    },
    save() {
      this.$emit('save-new-item');
    },
  },
};
</script>
