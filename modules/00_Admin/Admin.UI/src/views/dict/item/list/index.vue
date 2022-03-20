<template>
  <m-list ref="listRef" class="m-border-none" :header="false" :cols="cols" :query-model="model" :query-method="query">
    <template #querybar>
      <el-form-item :label="$t('mkh.name')" prop="name">
        <el-input v-model="model.name" clearable />
      </el-form-item>
      <el-form-item :label="$t('mkh.code')" prop="code">
        <el-input v-model="model.code" clearable />
      </el-form-item>
    </template>
    <template #buttons>
      <m-button-add :code="buttons.itemAdd.code" @click="add" />
    </template>
    <template #expand="{ row }">
      <el-descriptions :column="4" size="small">
        <el-descriptions-item :label="$t('mod.admin.extend_data')" :span="4">{{ row.extend }}</el-descriptions-item>
        <el-descriptions-item :label="$t('mkh.creator')">{{ row.creator }}</el-descriptions-item>
        <el-descriptions-item :label="$t('mkh.created_time')">{{ row.createdTime }}</el-descriptions-item>
        <el-descriptions-item :label="$t('mkh.modifier')">{{ row.modifier }}</el-descriptions-item>
        <el-descriptions-item :label="$t('mkh.modified_time')">{{ row.modifiedTime }}</el-descriptions-item>
      </el-descriptions>
    </template>
    <template #col-icon="{ row }">
      <m-icon v-if="row.icon" :name="row.icon" />
    </template>
    <template #operation="{ row }">
      <m-button-edit :code="buttons.itemEdit.code" @click="edit(row)" @success="handleChange"></m-button-edit>
      <m-button-delete :code="buttons.itemRemove.code" :action="remove" :data="row.id" @success="handleChange"></m-button-delete>
    </template>
  </m-list>
  <save :id="selection.id" v-model="saveVisible" :parent-id="parentId" :mode="mode" @success="handleChange" />
</template>
<script>
import { computed, reactive, toRef, watch } from 'vue'
import { useList } from 'mkh-ui'
import { buttons } from '../../index/page.json'
import Save from '../save/index.vue'
export default {
  components: { Save },
  props: {
    parentId: {
      type: Number,
      default: 0,
    },
  },
  emits: ['change'],
  setup(props, { emit }) {
    const { store } = mkh

    const { query, remove } = mkh.api.admin.dictItem
    const parentId = toRef(props, 'parentId')

    const adminStore = store.state.mod.admin
    const groupCode = computed(() => adminStore.dict.groupCode)
    const dictCode = computed(() => adminStore.dict.dictCode)

    const model = reactive({ groupCode, dictCode, parentId, name: '', value: '' })
    const cols = [
      { prop: 'id', label: 'mkh.id', width: '55', show: false },
      { prop: 'name', label: 'mkh.name' },
      { prop: 'value', label: 'mkh.value' },
      { prop: 'icon', label: 'mkh.icon' },
      { prop: 'level', label: 'mkh.level' },
      { prop: 'sort', label: 'mkh.sort' },
    ]

    const list = useList()

    watch([parentId, groupCode, dictCode], () => {
      list.refresh()
    })

    const handleChange = () => {
      list.refresh()
      emit('change')
    }

    return {
      buttons,
      model,
      cols,
      query,
      remove,
      ...list,
      handleChange,
    }
  },
}
</script>
