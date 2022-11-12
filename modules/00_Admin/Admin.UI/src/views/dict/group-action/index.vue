<template>
  <m-form-dialog :model="model" :rules="rules" v-bind="form.props" v-on="form.on">
    <el-row>
      <el-col>
        <el-form-item :label="t('mkh.name')" prop="name">
          <el-input ref="nameRef" v-model="model.name" />
        </el-form-item>
        <el-form-item :label="t('mkh.code')" prop="code">
          <el-input v-model="model.code" />
        </el-form-item>
        <el-form-item :label="t('mkh.icon')" prop="icon">
          <m-icon-picker v-model="model.icon" />
        </el-form-item>
        <el-form-item :label="t('mkh.remarks')" prop="remarks">
          <el-input v-model="model.remarks" type="textarea" :rows="5" />
        </el-form-item>
      </el-col>
    </el-row>
  </m-form-dialog>
</template>
<script setup lang="ts">
  import { computed, ref } from 'vue'
  import { ActionMode, useAction } from 'mkh-ui'
  import { useI18n } from '@/locales'
  import api from '@/api/dictGroup'

  const { t } = useI18n()

  const props = defineProps<{ id?: string; mode: ActionMode }>()
  const emit = defineEmits()

  const model = ref({ name: '', code: '', icon: '', remarks: '' })
  const rules = computed(() => {
    return { name: [{ required: true, message: t('mod.admin.input_dict_group_name') }], code: [{ required: true, message: t('mod.admin.input_dict_group_code') }] }
  })

  const nameRef = ref(null)
  const { form } = useAction({ props, api, model, emit })
  form.props.autoFocusRef = nameRef
  form.props.width = '700px'
</script>
