<template>
  <div class="wrapper d-flex flex-column align-center justify-center">
    <div class="ma-10 text-h4 secondary--text text--lighten-2">
      {{ $t('welcome') }}
    </div>
    <v-avatar class="mb-10" color="grey-darken-1" size="90">
      <img src="@static/nuxt.png" alt="vue" />
    </v-avatar>
    <v-card width="400" height="400" class="pa-4" outlined elevation="15">
      <v-card-title primary-title> {{ $t('systems') }} </v-card-title>
      <v-card-actions
        class="d-flex flex-column align-center justify-center"
        style="gap: 20px"
      >
        <nuxt-link to="/servers">
          <v-btn
            color="primary"
            height="45"
            width="360"
            class="ml-0 text-h6"
            block
            large
          >
            {{ $t('servers') }}
          </v-btn>
        </nuxt-link>
        <nuxt-link to="/applications">
          <v-btn
            color="primary"
            height="45"
            width="360"
            class="ml-0 text-h6"
            block
            large
          >
            {{ $t('applications') }}
          </v-btn>
        </nuxt-link>
        <nuxt-link to="/tasks">
          <v-btn
            color="primary"
            height="45"
            width="360"
            class="ml-0 text-h6"
            block
            large
          >
            {{ $t('tasks') }}
          </v-btn>
        </nuxt-link>
      </v-card-actions>
    </v-card>
  </div>
</template>

<script>
export default {
  methods: {
    setTVar() {
      this.$i18n.locale = this.$store.getters.getLocale;
    },
  },
  beforeRouteEnter(_, from, next) {
    next((vm) => {
      vm.setTVar();
    });
  },
};
</script>
