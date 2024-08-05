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
        totalItems: 0,
        totalPages: 0,
      };
    },
    getters: {
      getLocale(state) {
        return state.locale;
      },
      getTotalPages(state) {
        return state.totalPages;
      },
      getTotalItems(state) {
        return state.totalItems;
      },
    },
    mutations: {
      setTotalItems(state, payload) {
        state.totalItems = payload.totalItems;
      },
      setTotalPages(state, payload) {
        state.totalPages = payload.totalPages;
      },
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
