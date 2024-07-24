<template>
  <v-data-table :headers="headers" height="60vh" :items="filteredData">
    <template v-slot:top>
      <v-toolbar flat class="mb-10">
        <v-col cols="10" sm="3" class="pa-4 pl-0">
          <v-text-field
            v-model="searchServer"
            append-icon="mdi-magnify"
            :label="$t('search') + ' ' + $t('itemType.server')"
            hide-details
            style="width: 200px"
          ></v-text-field>
        </v-col>
        <v-col cols="10" sm="3" class="pa-4">
          <v-text-field
            v-if="itemType != 'server'"
            v-model="searchApplication"
            append-icon="mdi-magnify"
            :label="$t('search') + ' ' + $t('itemType.application')"
            hide-details
            style="width: 200px"
          ></v-text-field>
        </v-col>
        <v-col cols="10" sm="3" class="pa-4">
          <v-text-field
            v-if="itemType == 'task'"
            v-model="searchTask"
            append-icon="mdi-magnify"
            :label="$t('search') + ' ' + $t('itemType.task')"
            hide-details
            style="width: 200px"
          ></v-text-field>
        </v-col>
        <v-spacer></v-spacer>
        <v-btn color="primary" dark @click="openDialog">
          {{ $t('addNew') }} {{ $t('itemType.' + itemType) }}
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
import { filterData } from '@pages/utils/functions/data-filter';

export default {
  inject: ['itemType', 'backLink'],
  props: ['openDialog', 'data', 'lastColumn', 'secondLastColumn', 'headers'],
  $emits: ['open-delete', 'open-form'],
  data() {
    return {
      searchServer: '',
      searchApplication: '',
      searchTask: '',
    };
  },
  computed: {
    filteredData() {
      return filterData(
        this.data,
        this.searchServer,
        this.searchApplication,
        this.searchTask,
        this.itemType
      );
    },
  },
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

<style scoped>
.search {
  width: 100px;
}
</style>
