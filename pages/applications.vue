<template>
  <v-row>
    <!-- <v-col cols="2">
      <v-sheet rounded="lg">
        <v-list :rounded="true">
          <v-list-item>Everything</v-list-item>
          <v-list-item v-for="n in 5" :key="n" link>
            {{ 'List item ' + n }}
          </v-list-item>

          <v-divider class="my-2"></v-divider>

          <v-list-item
            color="grey-lighten-4"
            title="Refresh"
            link
          ></v-list-item>
        </v-list>
      </v-sheet>
    </v-col> -->

    <v-col style="border: 1px solid black">
      <v-sheet min-height="70vh" rounded="lg" style="border: 5px solid red">
        <h1>Applications</h1>
        <v-data-table :headers="headers" height="60vh" :items="applications">
          <template v-slot:item="{ item }">
            <tr @click="handleRowClick(item.id)" class="clickable__row">
              <td>{{ item.name }}</td>
              <td>{{ item.date }}</td>
              <td>{{ item.server }}</td>
              <td>{{ item.tasks }}</td>
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
        { text: 'Date', value: 'date' },
        { text: 'Server', value: 'server' },
        { text: 'Tasks', value: 'tasks' },
      ],
    };
  },
  computed: {
    applications() {
      return this.$store.getters.getApps;
    },
  },
  methods: {
    handleRowClick(id) {
      this.$router.push('/details/applications/' + id);
    },
  },
};
</script>
