<template>
  <el-select clearable placeholder="请选择..." @change="handleChange">
    <el-option v-for="item in modules" :key="item.code" :label="item.label" :value="item.code"> </el-option>
  </el-select>
</template>
<script>
export default {
  emits: ['change'],
  setup(props, { emit }) {
    const handleChange = code => {
      const mod = mkh.modules.find(m => m.code === code)
      emit('change', mod)
    }
    return {
      modules: mkh.modules,
      handleChange,
    }
  },
}
</script>
