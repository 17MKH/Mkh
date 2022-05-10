<template>
  <m-select :action="query" :clearable="clearable"></m-select>
</template>
<script>
export default {
  props: {
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
    clearable: {
      type: Boolean,
      default: true,
    },
  },
  setup(props) {
    const { queryEnumOptions } = mkh.api.admin.common
    const query = () => {
      return queryEnumOptions({ moduleCode: props.module, enumName: props.name })
    }

    return {
      query,
    }
  },
}
</script>
