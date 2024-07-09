<template>
  <div class="wrapper">
    <button @click="goBack">Go back</button>
    <button v-if="!cameFromServers" @click="goToServers">Go to servers</button>
    <h1>
      {{ serverDetails.name }}
    </h1>
    <h2>Creation date: {{ serverDetails.date }}</h2>
    <v-tabs v-model="tab">
      <v-tab> Applications </v-tab>
      <v-tab> Tasks </v-tab>
    </v-tabs>

    <v-tabs-items v-model="tab">
      <v-tab-item>
        <v-data-table
          :headers="headers"
          height="60vh"
          :items="serverDetails.applications"
        >
          <template v-slot:item="{ item }">
            <tr @click="showAppDetails(item.id)" class="clickable__row">
              <td>{{ item.name }}</td>
              <td>{{ item.date }}</td>
              <td>{{ item.tasks }}</td>
            </tr>
          </template>
        </v-data-table>
      </v-tab-item>
      <v-tab-item>
        <v-data-table
          :headers="headers"
          height="60vh"
          :items="serverDetails.tasks"
        >
          <template v-slot:item="{ item }">
            <tr @click="showTaskDetails(item.id)" class="clickable__row">
              <td>{{ item.name }}</td>
              <td>{{ item.date }}</td>
              <td>{{ item.application }}</td>
            </tr>
          </template>
        </v-data-table>
      </v-tab-item>
    </v-tabs-items>
  </div>
</template>

<script>
export default {
  data() {
    return {
      tab: 0,
      cameFromServers: false,
      headers: [
        { text: 'Name', value: 'name' },
        { text: 'Date', value: 'date' },
        { text: 'Application', value: 'application' },
      ],
    };
  },
  methods: {
    getApplicationsList(serverId) {
      return this.$store.getters.getApps.filter(
        (app) => app.serverId == serverId
      );
    },
    getTasksList(serverId) {
      return this.$store.getters.getTasks.filter(
        (task) => task.serverId == serverId
      );
    },
    showAppDetails(appId) {
      this.$router.push('/details/applications/' + appId);
    },
    showTaskDetails(taskId) {
      this.$router.push('/details/tasks/' + taskId);
    },
    goBack() {
      this.$router.back();
    },
    goToServers() {
      this.$router.push('/servers');
    },
    setCameFromServers(value) {
      this.cameFromServers = value;
    },
  },
  computed: {
    serverDetails() {
      const server = this.$store.getters.getServers.find(
        (server) => server.id == this.$route.params.id
      );
      return {
        name: server.name,
        date: server.date,
        applications: this.getApplicationsList(server.id),
        tasks: this.getTasksList(server.id),
      };
    },
  },
  beforeRouteEnter(_, from, next) {
    next((vm) => {
      if (from.fullPath == '/servers') {
        vm.setCameFromServers(true);
      } else {
        vm.setCameFromServers(false);
      }
    });
  },
};
</script>

<style></style>
