<template>
  <m-form-dialog :model="model" :rules="rules" v-bind="bind" v-on="on">
    <el-form-item :label="$t('mod.admin.menu_group')" prop="menuGroupId">
      <m-select v-model="model.menuGroupId" :action="$mkh.api.admin.menuGroup.select" checked-first></m-select>
    </el-form-item>
    <el-form-item :label="$t('mkh.name')" prop="name">
      <el-input ref="nameRef" v-model="model.name" />
    </el-form-item>
    <el-form-item :label="$t('mkh.code')" prop="code">
      <el-input v-model="model.code" />
    </el-form-item>
    <el-form-item :label="$t('mkh.remarks')" prop="remarks">
      <el-input v-model="model.remarks" type="textarea" :rows="5" />
    </el-form-item>
  </m-form-dialog>
</template>
<script>
import { computed, reactive, ref } from 'vue'
import { useSave, withSaveProps as props } from 'mkh-ui'

export default {
  props,
  emits: ['success'],
  setup(props, { emit }) {
    const {
      $t,
      api: {
        admin: { role: api },
      },
    } = mkh
    const model = reactive({ menuGroupId: '', name: '', code: '', remarks: '' })
    const rules = computed(() => {
      return {
        menuGroupId: [{ required: true, message: $t('mod.admin.select_menu_group') }],
        name: [{ required: true, message: $t('mod.admin.input_role_name') }],
        code: [{ required: true, message: $t('mod.admin.input_role_code') }],
      }
    })
    const nameRef = ref(null)
    const { bind, on } = useSave({ props, api, model, emit })
    bind.autoFocusRef = nameRef
    bind.width = '500px'

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
