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
        <div class="text-h4 mb-10">Tasks</div>
        <v-data-table :headers="headers" height="60vh" :items="items">
          <template v-slot:item="{ item }">
            <tr @click="handleRowClick(item.id)" class="clickable__row">
              <td>{{ item.name }}</td>
              <td>{{ item.date }}</td>
              <td>{{ item.server }}</td>
              <td>
                {{
                  item.application
                    ? item.application
                    : 'Nie przypisano do aplikacji'
                }}
              </td>
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
    handleRowClick(id) {
      this.$router.push('/details/tasks/' + id);
    },
  },
};
</script>

<style lang="scss" scoped>
.v-data-table > .v-data-table__wrapper > table > tbody > tr > th,
.v-data-table > .v-data-table__wrapper > table > thead > tr > th,
.v-data-table > .v-data-table__wrapper > table > tfoot > tr > th {
  font-size: 20px !important;
}
</style>
