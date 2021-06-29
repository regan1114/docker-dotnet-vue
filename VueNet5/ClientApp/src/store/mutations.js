import * as types from './mutations_type';

export const state = {
  loading: false,
  userInfo: {},
};

export const mutations = {
  [types.SET_LOADING](state, value) {
    state.loading = value;
  },
  [types.SET_USERINFO](state, value) {
    state.userInfo = value;
  },
};
