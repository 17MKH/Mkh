const state = {
  dict: {
    groupCode: '',
    dictCode: '',
  },
}

const mutations = {
  setDict(state, dict) {
    state.dict = dict
  },
}

export default {
  namespaced: true,
  state,
  mutations,
}
