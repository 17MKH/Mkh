import { defineStore } from 'pinia'

export const useComponentStore = defineStore('app.skin', {
  state: () => {
    return {
      dict: {
        groupCode: '',
        dictCode: '',
      },
    }
  },
})
