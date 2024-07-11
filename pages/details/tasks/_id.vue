<template>
  <div class="wrapper">
    <v-card
      outlined
      elevation="10"
      width="800"
      height="400"
      class="mx-auto mt-16 pa-6"
    >
      <v-row>
        <v-col cols="8">
          <v-card-actions class="d-flex align-center" style="gap: 20px">
            <v-btn @click="goBack">Go back</v-btn>
            <v-btn v-if="!cameFromTasks" @click="goToTasks">Go to tasks</v-btn>
          </v-card-actions>
          <v-card-title primary-title class="text-h2 mb-4">
            {{ taskDetails.name }}
          </v-card-title>
          <v-card-subtitle class="text-h5">
            Creation date: {{ taskDetails.date }}
          </v-card-subtitle>
          <v-card-text class="d-flex flex-column">
            <nuxt-link
              class="text-h6"
              :to="'/details/servers/' + taskDetails.serverId"
            >
              Server: {{ taskDetails.server }}
            </nuxt-link>
            <nuxt-link
              class="text-h6"
              v-if="taskDetails.applicationId"
              :to="'/details/applications/' + taskDetails.applicationId"
            >
              Application: {{ taskDetails.application }}
            </nuxt-link>
            <div v-else>
              <p>This task is not attached to any application</p>
              <v-btn color="success">Attach to application</v-btn>
            </div>
          </v-card-text>
        </v-col>
        <v-col>
          <v-card-actions
            class="d-flex flex-column align-center justify-center"
            style="gap: 20px"
          >
            <v-btn color="info" block large>UPDATE TASK</v-btn>
            <v-btn color="error" class="ml-0" block large>DELETE TASK</v-btn>
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
      cameFromTasks: false,
      headers: [
        { text: 'Name', value: 'name' },
        { text: 'Date', value: 'date' },
        { text: 'Application', value: 'application' },
      ],
    };
  },
  methods: {
    goBack() {
      this.$router.back();
    },
    goToTasks() {
      this.$router.push('/tasks');
    },
    setCameFromTasks(value) {
      this.cameFromTasks = value;
    },
  },
  computed: {
    taskDetails() {
      console.log(this.$route.params);
      const task = this.$store.getters.getTasks.find(
        (task) => task.id == this.$route.params.id
      );
      console.log(task);
      return task;
    },
  },
  beforeRouteEnter(_, from, next) {
    next((vm) => {
      if (from.fullPath == '/servers') {
        vm.setCameFromTasks(true);
      } else {
        vm.setCameFromTasks(false);
      }
    });
  },
};
</script>

<style></style>
