<template>
  <v-card
    outlined
    elevation="10"
    width="1200"
    height="750"
    class="mx-auto mt-16 pa-10"
  >
    <v-row class="d-flex justify-space-between mb-12">
      <div class="d-flex flex-column">
        <v-card-actions class="d-flex align-center" style="gap: 20px">
          <v-btn @click="goBack">{{ $t('goBack') }}</v-btn>
          <v-btn v-if="!cameFrom" @click="goTo">{{ $t(goToMessage) }}</v-btn>
        </v-card-actions>
        <v-card-title primary-title class="text-h2 mb-4">
          {{ itemDetails.name }}
        </v-card-title>
        <v-card-subtitle class="text-h5 mt-2">
          {{ $t('creationDate') }}: {{ itemDetails.date }}
        </v-card-subtitle>
        <v-card-subtitle class="text-h5">
          {{ $t('editionDate') }}: {{ itemDetails.edition_date }}
        </v-card-subtitle>
        <slot> </slot>
      </div>
      <div height="200" class="d-flex align-center">
        <v-card-actions
          class="action__buttons d-flex flex-column"
          style="gap: 20px"
        >
          <v-btn color="info" block large @click="updateItem">
            {{ $t(updateItemMessage) }}
          </v-btn>
          <v-btn color="error" class="ml-0" block large @click="deleteItem">
            {{ $t(deleteItemMessage) }}
          </v-btn>
        </v-card-actions>
      </div>
    </v-row>
    <v-row>
      <slot name="secondRow"></slot>
    </v-row>
  </v-card>
</template>

<script>
export default {
  props: [
    'cameFrom',
    'goToDest',
    'itemDetails',
    'goToMessage',
    'updateItemMessage',
    'deleteItemMessage',
  ],
  emits: ['delete-item', 'update-item'],
  methods: {
    goBack() {
      this.$router.back();
    },
    goTo() {
      this.$router.push(this.goToDest);
    },
    updateItem() {
      this.$emit('update-item');
    },
    deleteItem() {
      this.$emit('delete-item');
    },
  },
};
</script>

<style scoped>
.action__buttons {
  gap: 20px;
  width: 300px;
}

.full__width {
  width: 100%;
}
</style>
