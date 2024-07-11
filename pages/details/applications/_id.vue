<template>
  <div class="wrapper">
    <v-card
      outlined
      elevation="10"
      width="1200"
      height="700"
      class="mx-auto mt-16 pa-6"
    >
      <v-row>
        <v-col cols="9">
          <v-card-actions class="d-flex align-center" style="gap: 20px">
            <v-btn @click="goBack">Go back</v-btn>
            <v-btn v-if="!cameFromApplications" @click="goToApplications"
              >Go to applications</v-btn
            >
          </v-card-actions>
          <v-card-title primary-title class="text-h2 mb-4">
            {{ appDetails.name }}
          </v-card-title>
          <v-card-subtitle class="text-h5">
            Creation date: {{ appDetails.date }}
          </v-card-subtitle>
          <v-card-text>
            <nuxt-link
              class="text-h6"
              :to="'/details/servers/' + appDetails.serverId"
            >
              Server: {{ appDetails.server }}
            </nuxt-link>
          </v-card-text>
          <v-data-table
            :headers="headers"
            height="350px"
            :items="appDetails.tasks"
          >
            <template v-slot:item="{ item }">
              <tr @click="showTaskDetails(item.id)" class="clickable__row">
                <td>{{ item.name }}</td>
                <td>{{ item.date }}</td>
                <td>{{ item.server }}</td>
              </tr>
            </template>
          </v-data-table>
        </v-col>
        <v-col width="200px">
          <v-card-actions
            class="d-flex flex-column align-center justify-center"
            style="gap: 20px"
          >
            <v-btn color="info" block large>UPDATE APPLICATION</v-btn>
            <v-btn color="error" class="ml-0" block large
              >DELETE APPLICATION</v-btn
            >
          </v-card-actions>
        </v-col>
      </v-row>
    </v-card>
  </div>
</template>

<script>
export default {
  data() {
    return {
      tab: 0,
      cameFromApplications: false,
      headers: [
        { text: 'Name', value: 'name', width: '25px' },
        { text: 'Creation date', value: 'date', width: '75px' },
        { text: 'Server', value: 'server', width: '25px' },
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
