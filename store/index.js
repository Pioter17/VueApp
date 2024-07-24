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
    state() {
      return {
        locale: 'en',
      };
    },
    getters: {
      getLocale(state) {
        return state.locale;
      },
    },
    mutations: {
      setLocale(state, payload) {
        state.locale = payload.newLocale;
      },
    },
    actions: {
      changeLang(context, newLang) {
        context.commit('setLocale', { newLocale: newLang });
      },
    },
  });
