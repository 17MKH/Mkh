<template>
  <m-form-dialog v-bind="bind" v-on="on">
    <el-form-item label="菜单组：" prop="menuGroupId">
      <m-admin-menu-group-select v-model="model.menuGroupId" />
    </el-form-item>
    <el-form-item label="名称：" prop="name">
      <el-input ref="nameRef" v-model="model.name" />
    </el-form-item>
    <el-form-item label="唯一编码：" prop="code">
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
    const model = reactive({ menuGroupId: '', name: '', code: '', remarks: '' })
    const rules = {
      menuGroupId: [{ required: true, message: '请选择菜单组' }],
      name: [{ required: true, message: '请输入名称' }],
      code: [{ required: true, message: '请输入唯一编码' }],
    }
    const nameRef = ref(null)
    const { bind, on } = useSave({ title: '角色', props, api, model, rules, emit })
    bind.autoFocusRef = nameRef
    bind.width = '500px'

    return {
      model,
      bind,
      on,
      nameRef,
    }
  },
}
</script>
