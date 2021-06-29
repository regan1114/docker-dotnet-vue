/* eslint-disable no-unused-vars */

import axios from "axios";
import * as types from "./mutations_type";
import router from '../router';

const API_URL = "";

const api = {
  get: async function(url, params) {
    let headers = {};
    let user = JSON.parse(localStorage.getItem("user"));
    if (user) headers = { Authorization: "Bearer " + user.jwtToken };
    return await axios.get(`${API_URL}/api/${url}`, {
      headers: headers,
      params: params
    });
  },
  post: async function(url, params) {
    let headers = {};
    let user = JSON.parse(localStorage.getItem("user"));
    if (user) headers = { Authorization: "Bearer " + user.jwtToken };
    return await axios.post(`${API_URL}/api/${url}`, params, {
      headers: headers
    });
  }
};

export async function login({ commit }, data) {
  try {
    const results = await api.post("users/login", data);
    console.log(`login e ${JSON.stringify(results)}`)
    if (results.status === 200) {
      localStorage.setItem("user", JSON.stringify(results.data));
      commit(types.SET_USERINFO, results.data);
      router.push({path:"/"})
    }
    return results;
  } catch (e) {
    console.log(`login e ${JSON.stringify(e)}`)
    return e;
  }
}

export async function logout({ commit }) {
  try {
    localStorage.removeItem("user");
    commit(types.SET_USERINFO, null);
  } catch (e) {
    return e;
  }
}

export function clearUser({ commit , dispatch}) {
  dispatch('logout');
  router.push({path:"/login"})
}

export async function getUserData({ commit , dispatch}) {
  try {
    return await api.get("users", null);
  } catch (e) {
    dispatch('clearUser');
    return e;
  }
}
export async function weatherForecast() {
  try {
    return await api.get("WeatherForecast", null);
  } catch (e) {
    return e;
  }
}
