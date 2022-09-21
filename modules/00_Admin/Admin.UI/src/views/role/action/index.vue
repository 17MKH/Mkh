<template>
  <m-form-dialog :model="model" :rules="rules" v-bind="form.props" v-on="form.on">
    <el-row>
      <el-col :span="24">
        <el-form-item :label="t('mod.admin.menu_group')" prop="menuGroupId">
          <m-select v-model="model.menuGroupId" :action="menuGroupApi.select" checked-first></m-select>
        </el-form-item>
        <el-form-item :label="t('mod.admin.role_name')" prop="name">
          <el-input ref="nameRef" v-model="model.name" />
        </el-form-item>
        <el-form-item :label="t('mod.admin.role_code')" prop="code">
          <el-input v-model="model.code" />
        </el-form-item>
        <el-form-item :label="t('mkh.remarks')" prop="remarks">
          <el-input v-model="model.remarks" type="textarea" :rows="5" />
        </el-form-item>
      </el-col>
    </el-row>
  </m-form-dialog>
</template>
<script setup lang="ts">
  import { computed, reactive, ref } from 'vue'
  import { ActionMode, useAction } from 'mkh-ui'
  import { useI18n } from '@/locales'
  import api from '@/api/role'
  import menuGroupApi from '@/api/menuGroup'

  const { t } = useI18n()

  const props = defineProps<{ id: string | undefined; mode: ActionMode }>()
  const emit = defineEmits()

  const model = reactive({ menuGroupId: '', name: '', code: '', remarks: '' })
  const rules = computed(() => {
    return {
      menuGroupId: [{ required: true, message: t('mod.admin.select_menu_group') }],
      name: [{ required: true, message: t('mod.admin.input_role_name') }],
      code: [{ required: true, message: t('mod.admin.input_role_code') }],
    }
  })

  const nameRef = ref(null)
  const { form } = useAction({ props, emit, api, model })
  form.props.autoFocusRef = nameRef
  form.props.width = '560px'
  form.props.labelWidth = '130px'
</script>
