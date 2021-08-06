<template>
  <m-select :action="query" clearable> </m-select>
</template>
<script>
export default {
  setup() {
    const modules = mkh.modules.map(m => {
      return {
        label: m.label,
        value: m.code,
        data: m,
      }
    })

    const query = () => {
      return new Promise(resolve => {
        resolve(modules)
      })
    }

    return {
      query,
    }
  },
}
</script>
