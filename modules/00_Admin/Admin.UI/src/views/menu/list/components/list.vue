<template>
  <m-list title="菜单列表" icon="list"></m-list>
</template>
<script>
import { useList } from 'mkh-ui'
import { reactive } from '@vue/reactivity'
export default {
  props: {
    parentId: {
      type: Number,
      default: 0,
    },
  },
  setup(props) {
    const model = reactive({ name: '' })
    const cols = []
  },
}
</script>
