<template>
  <m-form-dialog :model="model" :rules="rules" v-bind="bind" v-on="on">
    <el-form-item :label="$t('mkh.name')" prop="name">
      <el-input ref="nameRef" v-model="model.name" />
    </el-form-item>
    <el-form-item :label="$t('mkh.remarks')" prop="remarks">
      <el-input v-model="model.remarks" type="textarea" :rows="5" />
    </el-form-item>
  </m-form-dialog>
</template>
<script>
import { computed, reactive, ref } from 'vue'
import { useSave, withSaveProps } from 'mkh-ui'

export default {
  props: {
    ...withSaveProps,
  },
  emits: ['success'],
  setup(props, { emit }) {
    const {
      $t,
      api: {
        admin: { menuGroup: api },
      },
    } = mkh

    const model = reactive({ name: '', remarks: '' })
    const rules = computed(() => {
      return { name: [{ required: true, message: $t('mod.admin.input_menu_group_name') }] }
    })

    const nameRef = ref(null)
    const { bind, on } = useSave({ props, api, model, rules, emit })
    bind.autoFocusRef = nameRef
    bind.width = '700px'

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
