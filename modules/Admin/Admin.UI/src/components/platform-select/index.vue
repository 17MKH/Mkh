<template>
  <m-select :action="query" :clearable="clearable"></m-select>
</template>
<script setup lang="ts">
  import api from '@/api/common'
  defineProps({
    clearable: {
      type: Boolean,
      default: true,
    },
  })

  const query = () => {
    return api.queryPlatformOptions()
  }
</script>
