<template>
  <v-row>
    <!-- <v-col cols="2">
      <v-sheet rounded="lg">
        <v-list :rounded="true">
          <v-list-item link>Everything</v-list-item>
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
        <h1>Servers</h1>
        <v-data-table :headers="headers" height="60vh" :items="servers">
          <template v-slot:item="{ item }">
            <tr @click="showServer(item.id)" class="clickable__row">
              <td>{{ item.name }}</td>
              <td>{{ item.date }}</td>
              <td>{{ item.applications }}</td>
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
      name: 'Servers Page',
      headers: [
        { text: 'Name', value: 'name' },
        { text: 'Date', value: 'date' },
        { text: 'Applications', value: 'applications' },
        { text: 'Tasks', value: 'tasks' },
      ],
    };
  },
  computed: {
    servers() {
      return this.$store.getters.getServers;
    },
  },
  methods: {
    showServer(id) {
      this.$router.push('/details/servers/' + id);
    },
  },
};
</script>
