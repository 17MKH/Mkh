<template>
  <m-radio :action="query"></m-radio>
</template>
<script setup lang="ts">
  import api from '@/api/common'
  const props = defineProps({
    //模块编码
    module: {
      type: String,
      required: true,
    },
    //枚举名称
    name: {
      type: String,
      required: true,
    },
  })
  const query = () => {
    return api.queryEnumOptions({ moduleCode: props.module, enumName: props.name })
  }
</script>
