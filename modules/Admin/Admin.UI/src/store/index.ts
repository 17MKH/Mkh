import { defineStore } from 'pinia'

export default defineStore('mod.admin', {
  state: () => {
    return {
      dict: {
        groupCode: '',
        dictCode: '',
      },
    }
  },
})
