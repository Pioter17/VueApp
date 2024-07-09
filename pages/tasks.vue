<template>
  <v-row>
    <v-col cols="2">
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
    </v-col>

    <v-col style="border: 1px solid black">
      <v-sheet min-height="70vh" rounded="lg" style="border: 5px solid red">
        <h1>Tasks</h1>
        <cos></cos>
        <v-data-table :headers="headers" height="60vh" :items="items">
          <template v-slot:item="{ item }">
            <tr @click="handleRowClick(item)" class="clickable__row">
              <td>{{ item.name }}</td>
              <td>{{ item.date }}</td>
              <td>{{ item.server }}</td>
              <td>{{ item.application }}</td>
            </tr>
          </template>
        </v-data-table>
      </v-sheet>
    </v-col>
  </v-row>
</template>

<script>
import cos from '@components/cos';
export default {
  components: {
    cos,
  },
  data() {
    return {
      name: 'Servers Page',
      headers: [
        { text: 'Name', value: 'name' },
        { text: 'Date', value: 'date' },
        { text: 'Server', value: 'server' },
        { text: 'Application', value: 'application' },
      ],
    };
  },
  computed: {
    items() {
      return this.$store.getters.getTasks;
    },
  },
  methods: {
    handleRowClick(item) {
      console.log('Clicked row:', item.id);
      // dodaj tu dowolną logikę, np. nawigacja do nowej strony, wyświetlenie szczegółów itp.
    },
  },
};
</script>
