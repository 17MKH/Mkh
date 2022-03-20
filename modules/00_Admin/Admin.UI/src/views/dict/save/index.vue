<template>
  <m-form-dialog :model="model" :rules="rules" v-bind="bind" v-on="on">
    <el-form-item :label="$t('mkh.name')" prop="name">
      <el-input ref="nameRef" v-model="model.name" />
    </el-form-item>
    <el-form-item :label="$t('mkh.code')" prop="code">
      <el-input v-model="model.code" />
    </el-form-item>
  </m-form-dialog>
</template>
<script>
import { computed, reactive, ref } from 'vue'
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
    const { $t } = mkh
    const api = mkh.api.admin.dict
    const model = reactive({ groupCode: '', name: '', code: '' })
    const rules = computed(() => {
      return {
        name: [{ required: true, message: $t('mod.admin.input_dict_name') }],
        code: [{ required: true, message: $t('mod.admin.input_dict_code') }],
      }
    })

    const nameRef = ref(null)
    const { bind, on } = useSave({ props, api, model, emit })
    bind.autoFocusRef = nameRef
    bind.width = '500px'
    bind.beforeSubmit = () => {
      model.groupCode = props.groupCode
    }

    return {
      model,
      rules,
      bind,
      on,
      nameRef,
    }
  },
}
</script>
