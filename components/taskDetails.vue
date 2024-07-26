<template>
  <v-card-text class="d-flex flex-column">
    <nuxt-link class="text-h6" :to="'/details/servers/' + taskDetails.serverId">
      {{ $t('server') }}: {{ taskDetails.server }}
    </nuxt-link>
    <nuxt-link
      class="text-h6"
      v-if="taskDetails.applicationId"
      :to="'/details/applications/' + taskDetails.applicationId"
    >
      {{ $t('application') }}: {{ taskDetails.application }}
    </nuxt-link>
    <div v-else>
      <p class="text-h6">
        {{ $t('tasksPage.taskNotAssigned') }}
      </p>
    </div>
  </v-card-text>
</template>

<script>
export default {
  props: ['taskDetails'],
};
</script>
