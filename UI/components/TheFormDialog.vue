<template>
  <v-dialog v-model="dialog" max-width="500px" persistent>
    <v-card>
      <v-card-title>
        <span class="text-h5"> New {{ itemType }} </span>
      </v-card-title>
      <v-card-text>
        <v-container>
          <slot></slot>
        </v-container>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="blue darken-1" text @click="close"> Cancel </v-btn>
        <v-btn color="blue darken-1" text @click="save"> Save </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  props: ['itemType', 'dialog'],
  emits: ['cancel-close', 'save-new-item'],
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
