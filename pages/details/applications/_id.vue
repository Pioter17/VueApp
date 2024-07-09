<template>
  <div class="wrapper">
    <button @click="goBack">Go back</button>
    <button v-if="!cameFromApplications" @click="goToApplications">
      Go to applications
    </button>
    <h1>
      {{ appDetails.name }}
    </h1>
    <h2>Creation date: {{ appDetails.date }}</h2>
    <nuxt-link :to="'/details/servers/' + appDetails.serverId">
      <h2>Server: {{ appDetails.server }}</h2>
    </nuxt-link>
    <v-data-table :headers="headers" height="60vh" :items="appDetails.tasks">
      <template v-slot:item="{ item }">
        <tr @click="showTaskDetails(item.id)" class="clickable__row">
          <td>{{ item.name }}</td>
          <td>{{ item.date }}</td>
          <td>{{ item.server }}</td>
        </tr>
      </template>
    </v-data-table>
  </div>
</template>

<script>
export default {
  data() {
    return {
      tab: 0,
      cameFromApplications: false,
      headers: [
        { text: 'Name', value: 'name' },
        { text: 'Date', value: 'date' },
        { text: 'Server', value: 'server' },
      ],
    };
  },
  methods: {
    getTasksList(appId) {
      return this.$store.getters.getTasks.filter(
        (task) => task.applicationId == appId
      );
    },
    showTaskDetails(taskId) {
      this.$router.push('/details/tasks/' + taskId);
    },
    goBack() {
      this.$router.back();
    },
    goToApplications() {
      this.$router.push('/applications');
    },
    setCameFromApplications(value) {
      this.cameFromApplications = value;
    },
  },
  computed: {
    appDetails() {
      console.log(this.$route.params);
      const application = this.$store.getters.getApps.find(
        (app) => app.id == this.$route.params.id
      );
      console.log(this.getTasksList(application.id));
      return {
        name: application.name,
        date: application.date,
        server: application.server,
        serverId: application.serverId,
        tasks: this.getTasksList(application.id),
      };
    },
  },
  beforeRouteEnter(_, from, next) {
    next((vm) => {
      if (from.fullPath == '/applications') {
        vm.setCameFromApplications(true);
      } else {
        vm.setCameFromApplications(false);
      }
    });
  },
};
</script>

<style></style>
