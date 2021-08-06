<template>
  <m-form-dialog v-bind="bind" v-on="on">
    <el-form-item label="名称：" prop="name">
      <el-input ref="nameRef" v-model="model.name" />
    </el-form-item>
    <el-form-item label="编码：" prop="code">
      <el-input v-model="model.code" />
    </el-form-item>
    <el-form-item label="图标：" prop="icon">
      <m-icon-picker v-model="model.icon" />
    </el-form-item>
    <el-form-item label="备注：" prop="remarks">
      <el-input v-model="model.remarks" type="textarea" :rows="5" />
    </el-form-item>
  </m-form-dialog>
</template>
<script>
import { reactive, ref } from 'vue'
import { useSave, withSaveProps } from 'mkh-ui'

export default {
  props: {
    ...withSaveProps,
  },
  setup(props, { emit }) {
    const api = mkh.api.admin.dictGroup
    const model = reactive({ name: '', code: '', icon: '', remarks: '' })
    const rules = { name: [{ required: true, message: '请输入字典分组名称' }], code: [{ required: true, message: '请输入字典分组的唯一编码' }] }

    const nameRef = ref(null)
    const { bind, on } = useSave({ title: '字典分组', props, api, model, rules, emit })
    bind.autoFocusRef = nameRef
    bind.width = '700px'

    return {
      model,
      bind,
      on,
      nameRef,
    }
  },
}
</script>
