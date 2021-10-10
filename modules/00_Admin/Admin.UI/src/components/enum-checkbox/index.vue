<template>
  <el-checkbox-group>
    <el-checkbox v-for="opt in options" :key="opt.value" :label="opt.value">{{ opt.label }}</el-checkbox>
  </el-checkbox-group>
</template>
<script>
import { ref } from 'vue'

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
  },
  setup(props) {
    const { queryEnumOptions } = mkh.api.admin.common
    const options = ref([])

    queryEnumOptions({ moduleCode: props.module, enumName: props.name }).then(data => {
      options.value = data
    })

    return {
      options,
    }
  },
}
</script>
