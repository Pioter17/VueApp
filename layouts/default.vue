<template>
  <v-app id="inspire">
    <v-app-bar dense class="d-flex align-center justify-center">
      <v-container class="d-flex align-center justify-center" style="gap: 20px">
        <v-avatar class="me-4" color="grey-darken-1" size="32">
          <img src="@static/v.png" alt="vue" />
        </v-avatar>

        <router-link v-for="link in links" :key="link.title" :to="link.link">
          <v-btn variant="text" class="m-10">
            {{ link.title }}
          </v-btn>
        </router-link>

        <v-spacer></v-spacer>

        <!-- <v-responsive max-width="160">
          <v-text-field
            density="compact"
            label="Search"
            :rounded="true"
            variant="solo-filled"
            flat
            hide-details
            single-line
          ></v-text-field>
        </v-responsive> -->
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
    title: 'Strona główna',
    link: '/',
  },
  {
    title: 'Serwery',
    link: '/servers',
  },
  {
    title: 'Aplikacje',
    link: '/applications',
  },
  {
    title: 'Taski',
    link: '/tasks',
  },
];
</script>

<script>
export default {
  name: 'DefaultLayout',
  data() {
    return {
      links: ['Dashboard', 'Messages', 'Profile', 'Updates'],
      clipped: false,
      drawer: false,
      fixed: false,
      items: [
        {
          icon: 'mdi-apps',
          title: 'Welcome',
          to: '/',
        },
        {
          icon: 'mdi-chart-bubble',
          title: 'Inspire',
          to: '/inspire',
        },
      ],
      miniVariant: false,
      right: true,
      rightDrawer: false,
      title: 'Vuetify.js',
    };
  },
};
</script>

<style>
.clickable__row {
  cursor: pointer;
}
</style>
