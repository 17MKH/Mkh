<template>
  <m-form-dialog :model="model" :rules="rules" v-bind="bind" v-on="on">
    <el-form-item :label="$t('mkh.name')" prop="name">
      <el-input ref="nameRef" v-model="model.name" clearable />
    </el-form-item>
    <el-form-item :label="$t('mkh.value')" prop="value">
      <el-input v-model="model.value" clearable />
    </el-form-item>
    <el-form-item :label="$t('mkh.icon')" prop="icon">
      <m-icon-picker v-model="model.icon" />
    </el-form-item>
    <el-form-item :label="$t('mod.admin.extend_data')" prop="extend">
      <div class="m-admin-dict-extend">
        <div class="m-admin-dict-extend_toolbar">
          <template v-for="toolbar in toolbars" :key="toolbar">
            <component :is="toolbar" v-model="model.extend"></component>
          </template>
        </div>
        <div class="m-admin-dict-extend_content">
          <el-input v-model="model.extend" type="textarea" :autosize="{ minRows: 5 }"> </el-input>
        </div>
      </div>
    </el-form-item>
    <el-form-item :label="$t('mkh.sort')" prop="sort">
      <el-input-number v-model="model.sort" />
    </el-form-item>
  </m-form-dialog>
</template>
<script>
import { computed, reactive, ref } from 'vue'
import { useSave, withSaveProps } from 'mkh-ui'

export default {
  props: {
    ...withSaveProps,
    parentId: {
      type: Number,
      default: 0,
    },
  },
  emits: ['success'],
  setup(props, { emit }) {
    const { store, $t } = mkh

    const api = mkh.api.admin.dictItem
    const model = reactive({ groupCode: '', dictCode: '', parentId: '', name: '', value: '', icon: '', extend: '', sort: 0 })
    const rules = computed(() => {
      return {
        name: [{ required: true, message: $t('mod.admin.input_dict_item_name') }],
        value: [{ required: true, message: $t('mod.admin.input_dict_item_value') }],
        sort: [{ required: true, message: $t('mod.admin.input_dict_item_sort') }],
      }
    })

    const nameRef = ref(null)
    const { bind, on } = useSave({ props, api, model, emit })
    bind.autoFocusRef = nameRef
    bind.width = '700px'
    bind.beforeSubmit = () => {
      const { groupCode, dictCode } = store.state.mod.admin.dict
      model.groupCode = groupCode
      model.dictCode = dictCode
      model.parentId = props.parentId
    }

    const toolbars = mkh.components.filter(m => m.includes('dict-toolbar-'))

    return {
      model,
      rules,
      bind,
      on,
      nameRef,
      toolbars,
    }
  },
}
</script>
<style lang="scss">
.m-admin-dict-extend {
  display: flex;
  flex-direction: column;
  align-items: stretch;

  &_toolbar {
    flex-shrink: 0;
    padding: 2px 5px;
    height: 36px;
    border: 1px solid #dcdfe6;
    border-bottom: 0;
    background-color: #f8f8f8;

    .m-button {
      padding: 0;
      min-height: 30px;
      height: 30px;
      width: 30px;
      font-size: 16px;
      color: rgb(51, 51, 51);

      &:hover {
        background-color: #e6e6e6;
      }
    }
  }

  &_content {
    flex-grow: 1;

    .el-textarea__inner {
      border-top-left-radius: 0;
      border-top-right-radius: 0;
    }
  }
}
</style>
