<template>
  <v-data-table
    :headers="headers"
    height="60vh"
    :items="filteredData"
    :no-data-text="$t('noData')"
    :footer-props="footerProps"
    :options.sync="options"
    :server-items-length="
      itemType == 'server'
        ? totalServers
        : itemType == 'application'
        ? totalApps
        : totalTasks
    "
    :loading="loading"
    @update:options="fetchData"
  >
    <template v-slot:top>
      <v-toolbar flat class="mb-10">
        <v-col cols="10" sm="3" class="pa-4 pl-0">
          <v-text-field
            v-model="searchParams.searchServer"
            append-icon="mdi-magnify"
            :label="$t('search') + ' ' + $t('itemType.server')"
            hide-details
            style="width: 200px"
          ></v-text-field>
        </v-col>
        <v-col cols="10" sm="3" class="pa-4">
          <v-text-field
            v-if="itemType != 'server'"
            v-model="searchParams.searchApplication"
            append-icon="mdi-magnify"
            :label="$t('search') + ' ' + $t('itemType.application')"
            hide-details
            style="width: 200px"
          ></v-text-field>
        </v-col>
        <v-col cols="10" sm="3" class="pa-4">
          <v-text-field
            v-if="itemType == 'task'"
            v-model="searchParams.searchTask"
            append-icon="mdi-magnify"
            :label="$t('search') + ' ' + $t('itemType.task')"
            hide-details
            style="width: 200px"
          ></v-text-field>
        </v-col>
        <v-spacer></v-spacer>
        <div class="buttons">
          <v-btn color="success" width="210" @click="exportPage">
            {{ $t('exportPage') }}
          </v-btn>
          <v-btn color="success" width="210" @click="exportAll">
            {{ $t('exportAll') }}
          </v-btn>
          <v-btn color="primary" dark width="210" @click="openDialog">
            {{ $t('addNew') }} {{ $t('itemType.' + itemType) }}
          </v-btn>
        </div>
      </v-toolbar>
    </template>
    <template v-slot:item="{ item }">
      <tr @click="handleRowClick(item.id)" class="clickable__row">
        <td>{{ item.name }}</td>
        <td>{{ item.date }}</td>
        <td>{{ item.edition }}</td>
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
import { getFooter } from '@core/constants/footer';
import axios from 'axios';

export default {
  inject: ['itemType', 'backLink', 'fetchFunction'],
  props: [
    'openDialog',
    'data',
    'lastColumn',
    'secondLastColumn',
    'headers',
    'searchParams',
  ],
  $emits: ['open-delete', 'open-form'],
  data() {
    return {
      options: {
        page: 1,
        itemsPerPage: 10,
        sortBy: [],
        sortDesc: [],
      },
      loading: false,
    };
  },
  computed: {
    filteredData() {
      return filterData(
        this.data,
        this.searchParams.searchServer,
        this.searchParams.searchApplication,
        this.searchParams.searchTask,
        this.itemType
      );
    },
    totalTasks() {
      return this.$store.getters.getTotalTaskItems;
    },
    totalApps() {
      return this.$store.getters.getTotalAppItems;
    },
    totalServers() {
      return this.$store.getters.getTotalServerItems;
    },
    footerProps() {
      return getFooter(this.$i18n);
    },
  },
  watch: {
    searchParams: {
      handler: 'fetchData',
      deep: true,
    },
    '$store.getters.getItemsPerPage'(newItemsPerPage) {
      this.options.itemsPerPage = newItemsPerPage;
    },
  },
  methods: {
    fetchData() {
      setTimeout(() => {}, 1000);
      this.loading = true;
      this.$store.dispatch('setItemsPerPage', this.options.itemsPerPage);
      this.$store.dispatch('setCurrentPage', this.options.page);
      this.$store
        .dispatch(this.fetchFunction, {
          pagination: [this.options.page, this.options.itemsPerPage],
          search: [
            this.searchParams.searchServer,
            this.searchParams.searchApplication,
            this.searchParams.searchTask,
          ],
          sortBy: this.options.sortBy ? this.options.sortBy[0] : '',
          sortDesc: this.options.sortDesc ? this.options.sortDesc[0] : false,
        })
        .finally(() => {
          this.loading = false;
        });
    },
    async exportAll() {
      const itemName = this.itemType == 'application' ? 'App' : this.item;
      const enpointURL =
        'https://localhost:7092/api/' +
        itemName[0].toUpperCase() +
        itemName.slice(1) +
        '/export-' +
        this.itemType +
        's';
      try {
        const response = await axios.get(enpointURL, {
          responseType: 'blob',
        });
        const url = window.URL.createObjectURL(new Blob([response.data]));
        const link = document.createElement('a');
        link.href = url;
        const date = new Date().toDateString();
        link.setAttribute(
          'download',
          this.itemType[0].toUpperCase() +
            this.itemType.slice(1) +
            's -' +
            date +
            '.xlsx'
        );
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
      } catch (error) {
        console.error(error);
      }
    },
    async exportPage() {
      const itemName = this.itemType == 'application' ? 'App' : this.itemType;
      const enpointURL =
        'https://localhost:7092/api/' +
        itemName[0].toUpperCase() +
        itemName.slice(1) +
        '/export-page';
      try {
        const response = await axios.get(enpointURL, {
          params: {
            pageNumber: this.options.page,
            pageSize: this.options.itemsPerPage,
            serverName: this.searchParams.searchServer,
            applicationName: this.searchParams.searchApplication,
            taskName: this.searchParams.searchTask,
            sortBy: this.options.sortBy ? this.options.sortBy[0] : '',
            sortDesc: this.options.sortDesc ? this.options.sortDesc[0] : false,
          },
          responseType: 'blob',
        });

        const url = window.URL.createObjectURL(new Blob([response.data]));
        const link = document.createElement('a');
        link.href = url;
        link.setAttribute(
          'download',
          `${this.itemType}_Page_${this.options.page}.xlsx`
        );

        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
      } catch (error) {
        console.error(error);
      }
    },
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

.buttons {
  display: flex;
  flex-direction: column;
  gap: 5px;
}
</style>
