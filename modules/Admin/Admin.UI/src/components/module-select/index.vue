<template>
  <m-select :action="query" clearable> </m-select>
</template>
<script setup lang="ts">
  import { mkh } from 'mkh-ui'

  const modules = mkh.modules.map((m) => {
    return {
      label: m.label,
      value: m.code,
      data: m,
    }
  })

  const query = () => {
    return new Promise((resolve) => {
      resolve(modules)
    })
  }
</script>
