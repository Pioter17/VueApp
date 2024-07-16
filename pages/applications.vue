<template>
  <v-row>
    <v-col class="d-flex justify-center">
      <v-sheet
        outlined
        elevation="10"
        width="90vw"
        min-height="80vh"
        rounded="lg"
        class="pa-14 mt-16"
      >
        <div class="text-h4 mb-10">Applications</div>
        <v-data-table :headers="headers" height="60vh" :items="applications">
          <template v-slot:item="{ item, index }">
            <tr @click="handleRowClick(item.id)" class="clickable__row">
              <td>{{ item.name }}</td>
              <td>{{ item.date }}</td>
              <td>{{ item.edition_date }}</td>
              <td>{{ item.server }}</td>
              <td>{{ tasks[index] }}</td>
            </tr>
          </template>
        </v-data-table>
      </v-sheet>
    </v-col>
  </v-row>
</template>

<script>
export default {
  data() {
    return {
      name: 'Application Page',
      headers: [
        { text: 'Name', value: 'name' },
        { text: 'Creation date', value: 'date' },
        { text: 'Edition date', value: 'edition_date' },
        { text: 'Server', value: 'server' },
        { text: 'Tasks' },
      ],
    };
  },
  computed: {
    applications() {
      return this.$store.getters.getApps;
    },
    tasks() {
      return this.applications.map((app) => {
        return this.$store.getters.getTasks.filter(
          (task) => task.applicationId == app.id
        ).length;
      });
    },
  },
  methods: {
    handleRowClick(id) {
      this.$router.push('/details/applications/' + id);
    },
  },
};
</script>
