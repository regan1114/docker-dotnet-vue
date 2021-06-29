import Vue from 'vue';
import Vuex from 'vuex';
import { state, mutations } from './mutations';
import * as actions from './actions';
import * as getters from './getters';

Vue.use(Vuex);

export default new Vuex.Store({
  state,
  mutations,
  actions,
  getters,
});
