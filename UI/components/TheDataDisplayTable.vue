<template>
  <v-data-table :headers="headers" height="60vh" :items="data">
    <template v-slot:top>
      <v-toolbar flat>
        <v-spacer></v-spacer>
        <v-btn color="primary" dark class="mb-10" @click="openDialog">
          Add new {{ itemType }}
        </v-btn>
      </v-toolbar>
    </template>
    <template v-slot:item="{ item }">
      <tr @click="handleRowClick(item.id)" class="clickable__row">
        <td>{{ item.name }}</td>
        <td>{{ item.date }}</td>
        <td>{{ item.edition_date }}</td>
        <td>{{ secondLastColumn(item) }}</td>
        <td>
          {{ lastColumn(item) }}
        </td>
        <td>
          <v-icon small class="mr-2" @click.stop="openForm(item)">
            mdi-pencil
          </v-icon>
          <v-icon small @click.stop="openDelete(item)"> mdi-delete </v-icon>
        </td>
      </tr>
    </template>
  </v-data-table>
</template>

<script>
export default {
  inject: ['headers', 'itemType', 'backLink'],
  props: ['openDialog', 'data', 'lastColumn', 'secondLastColumn'],
  $emits: ['open-delete', 'open-form'],
  methods: {
    handleRowClick(id) {
      this.$router.push(this.backLink + id);
    },
    openForm(item) {
      this.$emit('open-form', item);
    },
    openDelete(item) {
      this.$emit('open-delete', item);
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
