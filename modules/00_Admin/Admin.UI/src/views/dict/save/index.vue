<template>
  <m-form-dialog v-bind="bind" v-on="on">
    <el-form-item label="名称：" prop="name">
      <el-input ref="nameRef" v-model="model.name" />
    </el-form-item>
    <el-form-item label="编码：" prop="code">
      <el-input v-model="model.code" />
    </el-form-item>
  </m-form-dialog>
</template>
<script>
import { reactive, ref } from 'vue'
import { useSave, withSaveProps } from 'mkh-ui'

export default {
  props: {
    ...withSaveProps,
    groupCode: {
      type: String,
      default: '',
    },
  },
  emits: ['success'],
  setup(props, { emit }) {
    const api = mkh.api.admin.dict
    const model = reactive({ groupCode: '', name: '', code: '' })
    const rules = {
      name: [{ required: true, message: '请输入字典名称' }],
      code: [{ required: true, message: '请输入字典唯一编码' }],
    }

    const nameRef = ref(null)
    const { bind, on } = useSave({ title: '字典', props, api, model, rules, emit })
    bind.autoFocusRef = nameRef
    bind.width = '500px'
    bind.beforeSubmit = () => {
      model.groupCode = props.groupCode
    }

    return {
      model,
      bind,
      on,
      nameRef,
    }
  },
}
</script>
