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
        itemsPerPage: 10,
        currentPage: 1,
      };
    },
    getters: {
      getLocale(state) {
        return state.locale;
      },
      getItemsPerPage(state) {
        return state.itemsPerPage;
      },
      getCurrentPage(state) {
        return state.currentPage;
      },
    },
    mutations: {
      setLocale(state, payload) {
        state.locale = payload.newLocale;
      },
      setItemsPerPage(state, payload) {
        state.itemsPerPage = payload.itemsPerPage;
      },
      setCurrentPage(state, payload) {
        state.currentPage = payload.currentPage;
      },
    },
    actions: {
      changeLang(context, newLang) {
        context.commit('setLocale', { newLocale: newLang });
      },
      setItemsPerPage(context, newValue) {
        context.commit('setItemsPerPage', { itemsPerPage: newValue });
      },
      setCurrentPage(context, newValue) {
        context.commit('setCurrentPage', { currentPage: newValue });
      },
    },
  });
