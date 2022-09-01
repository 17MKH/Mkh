<template>
  <m-cascader :action="query"></m-cascader>
</template>
<script setup lang="ts">
  import api from '@/api'

  const props = defineProps({
    //分组编码
    group: {
      type: String,
      required: true,
    },
    //字典编码
    dict: {
      type: String,
      required: true,
    },
  })

  const query = () => {
    const { group, dict } = props
    return api.dict.cascader({ groupCode: group, dictCode: dict })
  }
</script>
