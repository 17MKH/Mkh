<template>
  <m-form-dialog :model="model" :rules="rules" v-bind="form.props" v-on="form.on">
    <el-row>
      <el-col :span="24">
        <el-form-item :label="t('mkh.name')" prop="name">
          <el-input ref="nameRef" v-model="model.name" />
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
  import api from '@/api/menuGroup'

  const { t } = useI18n()

  const props = defineProps<{ id: string | undefined; mode: ActionMode }>()
  const emit = defineEmits()

  const model = ref({ name: '', remarks: '' })
  const rules = computed(() => {
    return { name: [{ required: true, message: t('mod.admin.input_menu_group_name') }] }
  })

  const nameRef = ref(null)
  const { form } = useAction({ props, emit, api, model })
  form.props.autoFocusRef = nameRef
  form.props.width = '700px'
</script>
