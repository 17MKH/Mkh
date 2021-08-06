<template>
  <m-cascader :action="query"></m-cascader>
</template>
<script>
export default {
  props: {
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
  },
  setup(props) {
    const query = () => {
      const { group, dict } = props
      return mkh.api.admin.dict.cascader({ groupCode: group, dictCode: dict })
    }

    return {
      query,
    }
  },
}
</script>
