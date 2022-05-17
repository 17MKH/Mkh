<template>
  <m-select :action="query" :clearable="clearable"></m-select>
</template>
<script>
export default {
  props: {
    clearable: {
      type: Boolean,
      default: true,
    },
  },
  setup() {
    const { queryPlatformOptions } = mkh.api.admin.common
    const query = () => {
      return queryPlatformOptions()
    }

    return {
      query,
    }
  },
}
</script>
