import Vue from 'vue';
import Vuex from 'vuex';

import tasks from './tasks';
import applications from './applications';
import servers from './servers';

Vue.use(Vuex);

export default () =>
  new Vuex.Store({
    modules: {
      servers,
      applications,
      tasks,
    },
  });
