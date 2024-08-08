<template>
  <div class="full__width">
    <v-tabs v-model="tab" grow>
      <v-tab> {{ $t('applications') }} </v-tab>
      <v-tab> {{ $t('tasks') }} </v-tab>
    </v-tabs>
    <v-tabs-items class="full__width" v-model="tab">
      <v-tab-item>
        <v-data-table
          :headers="appHeaders"
          height="250px"
          :items="serverDetails.applications"
          :footer-props="footerProps"
        >
          <template v-slot:item="{ item }">
            <tr @click="showAppDetails(item.id)" class="clickable__row">
              <td>{{ item.name }}</td>
              <td>{{ item.date }}</td>
              <td>{{ item.edition }}</td>
              <td>{{ item.tasks }}</td>
            </tr>
          </template>
        </v-data-table>
      </v-tab-item>
      <v-tab-item>
        <v-data-table
          :headers="taskHeaders"
          height="250px"
          :items="serverDetails.tasks"
          :footer-props="footerProps"
        >
          <template v-slot:item="{ item }">
            <tr @click="showTaskDetails(item.id)" class="clickable__row">
              <td>{{ item.name }}</td>
              <td>{{ item.date }}</td>
              <td>{{ item.edition }}</td>
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
      </v-tab-item>
    </v-tabs-items>
  </div>
</template>

<script>
import { getFooter } from '@core/constants/footer';
export default {
  props: ['serverDetails', 'appHeaders', 'taskHeaders'],
  data() {
    return {
      tab: 0,
    };
  },
  computed: {
    footerProps() {
      return getFooter(this.$i18n);
    },
  },
  methods: {
    showAppDetails(appId) {
      this.$router.push('/details/applications/' + appId);
    },
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
