<template>
  <m-form-dialog :model="model" :rules="rules" v-bind="form.props" v-on="form.on">
    <el-form-item :label="t('mkh.name')" prop="name">
      <el-input ref="nameRef" v-model="model.name" />
    </el-form-item>
    <el-form-item :label="t('mkh.code')" prop="code">
      <el-input v-model="model.code" />
    </el-form-item>
  </m-form-dialog>
</template>
<script setup lang="ts">
  import { computed, reactive, ref } from 'vue'
  import { useAction } from 'mkh-ui'
  import { useI18n } from '@/locales'
  import api from '@/api/dict'

  const { t } = useI18n()

  const props = defineProps({
    id: {
      type: String,
    },
    mode: {
      type: String,
    },
    groupCode: {
      type: String,
      default: '',
    },
  })

  const emit = defineEmits()

  const model = reactive({ groupCode: '', name: '', code: '' })
  const rules = computed(() => {
    return {
      name: [{ required: true, message: t('mod.admin.input_dict_name') }],
      code: [{ required: true, message: t('mod.admin.input_dict_code') }],
    }
  })

  const nameRef = ref(null)
  const { form } = useAction({ props, api, model, emit })
  form.props.autoFocusRef = nameRef
  form.props.width = '500px'
  form.props.beforeSubmit = () => {
    model.groupCode = props.groupCode
  }
</script>
