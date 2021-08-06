<template>
  <m-list ref="listRef" class="m-border-none" :header="false" :cols="cols" :query-model="model" :query-method="query">
    <template #querybar>
      <el-form-item label="名称：" prop="name">
        <el-input v-model="model.name" clearable />
      </el-form-item>
      <el-form-item label="编码：" prop="code">
        <el-input v-model="model.code" clearable />
      </el-form-item>
    </template>
    <template #buttons>
      <m-button-add :code="buttons.itemAdd.code" @click="add" />
    </template>
    <template #expand="{ row }">
      <el-descriptions :column="4" size="small">
        <el-descriptions-item label="扩展数据：" :span="4">{{ row.extend }}</el-descriptions-item>
        <el-descriptions-item label="创建人：">{{ row.creator }}</el-descriptions-item>
        <el-descriptions-item label="创建时间：">{{ row.createdTime }}</el-descriptions-item>
        <el-descriptions-item label="修改人：">{{ row.modifier }}</el-descriptions-item>
        <el-descriptions-item label="修改时间：">{{ row.modifiedTime }}</el-descriptions-item>
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
import { useList, store } from 'mkh-ui'
import page from '../../index/page'
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
    const { query, remove } = mkh.api.admin.dictItem
    const parentId = toRef(props, 'parentId')

    const adminStore = store.state.mod.admin
    const groupCode = computed(() => adminStore.dict.groupCode)
    const dictCode = computed(() => adminStore.dict.dictCode)

    const model = reactive({ groupCode, dictCode, parentId, name: '', value: '' })
    const cols = [
      { prop: 'id', label: '编号', width: '55', show: false },
      { prop: 'name', label: '名称' },
      { prop: 'value', label: '值' },
      { prop: 'icon', label: '图标' },
      { prop: 'level', label: '级别' },
      { prop: 'sort', label: '排序' },
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
      buttons: page.buttons,
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
