import { defineStore } from 'pinia'

export const useAdminStore = defineStore('mod.admin', {
  state: () => {
    return {
      dict: {
        groupCode: '',
        dictCode: '',
      },
    }
  },
})
