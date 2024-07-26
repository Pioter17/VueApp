<template>
  <v-data-table
    :headers="headers"
    height="250px"
    :items="appDetails.tasks"
    class="full__width"
    :footer-props="footerProps"
  >
    <template v-slot:item="{ item }">
      <tr @click="showTaskDetails(item.id)" class="clickable__row">
        <td>{{ item.name }}</td>
        <td>{{ item.date }}</td>
        <td>{{ item.edition_date }}</td>
        <td>{{ item.server }}</td>
      </tr>
    </template>
  </v-data-table>
</template>

<script>
import { getFooter } from '@core/constants/footer';
export default {
  props: ['headers', 'appDetails'],
  computed: {
    footerProps() {
      return getFooter(this.$i18n);
    },
  },
  methods: {
    showTaskDetails(taskId) {
      this.$router.push('/details/tasks/' + taskId);
    },
  },
};
</script>

<style scoped>
.full__width {
  width: 100%;
}
</style>
