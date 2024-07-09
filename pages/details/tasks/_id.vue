<template>
  <div class="wrapper">
    <button @click="goBack">Go back</button>
    <button v-if="!cameFromTasks" @click="goToTasks">Go to tasks</button>
    <h1>
      {{ taskDetails.name }}
    </h1>
    <h2>Creation date: {{ taskDetails.date }}</h2>
    <nuxt-link :to="'/details/servers/' + taskDetails.serverId">
      <h2>Server: {{ taskDetails.server }}</h2>
    </nuxt-link>
    <nuxt-link :to="'/details/applications/' + taskDetails.applicationId">
      <h2>Application: {{ taskDetails.application }}</h2>
    </nuxt-link>
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
