<template>
  <m-form-dialog v-bind="bind" v-on="on">
    <el-form-item label="名称：" prop="name">
      <el-input ref="autoFocusRef" v-model="model.name" />
    </el-form-item>
    <el-form-item label="编码：" prop="code">
      <el-input v-model="model.code" />
    </el-form-item>
    <el-form-item label="备注：" prop="remarks">
      <el-input v-model="model.remarks" type="textarea" :rows="5" />
    </el-form-item>
  </m-form-dialog>
</template>
<script>
import { reactive, ref } from 'vue'
import { useSave, withSaveProps as props } from 'mkh-ui'

export default {
  props,
  emits: ['success', 'error'],
  setup(props, { emit }) {
    const api = mkh.api.admin.role
    const model = reactive({ name: '', code: '', remarks: '' })
    const rules = {
      name: [{ required: true, message: '请输入名称' }],
      code: [{ required: true, message: '请输入唯一编码' }],
    }
    const autoFocusRef = ref(null)
    const { bind, on } = useSave({ title: '角色', api, model, rules, props, emit })
    bind.width = '500px'
    bind.autoFocusRef = autoFocusRef

    return {
      model,
      bind,
      on,
      autoFocusRef,
    }
  },
}
</script>
