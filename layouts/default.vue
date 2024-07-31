<template>
  <v-app id="inspire">
    <v-app-bar dense class="d-flex align-center justify-center">
      <v-container class="d-flex align-center justify-center" style="gap: 20px">
        <v-avatar class="me-4" color="grey-darken-1" size="32">
          <img src="@static/v.png" alt="vue" />
        </v-avatar>

        <router-link v-for="link in links" :key="link.title" :to="link.link">
          <v-btn variant="text" class="m-10">
            {{ $t(link.title) }}
          </v-btn>
        </router-link>

        <v-spacer></v-spacer>

        <v-container class="">
          <v-btn small elevation="" @click="changeLang('pl')"> PL </v-btn>
          <v-btn small elevation="" @click="changeLang('en')"> EN </v-btn>
        </v-container>
      </v-container>
    </v-app-bar>

    <v-main class="bg-grey-lighten-3" style="height: 90vh">
      <v-container>
        <Nuxt />
      </v-container>
    </v-main>
  </v-app>
</template>

<script setup>
const links = [
  {
    title: 'mainPage',
    link: '/',
  },
  {
    title: 'servers',
    link: '/servers',
  },
  {
    title: 'applications',
    link: '/applications',
  },
  {
    title: 'tasks',
    link: '/tasks',
  },
];
</script>
<script>
export default {
  methods: {
    changeLang(lang) {
      this.$i18n.locale = lang;
      this.$store.dispatch('changeLang', lang);
    },
  },
  mounted() {
    console.log('dupa');
    this.$store.dispatch('fetchServers');
    this.$store.dispatch('fetchTasks');
    this.$store.dispatch('fetchApps');
  },
};
</script>

<style>
.clickable__row {
  cursor: pointer;
}
</style>
