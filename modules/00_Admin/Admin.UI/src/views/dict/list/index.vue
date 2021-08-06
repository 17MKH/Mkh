<template>
  <m-container>
    <m-list ref="listRef" title="字典管理" con="dict" :cols="cols" :query-model="model" :query-method="query" :query-on-created="false">
      <template #querybar>
        <el-form-item label="名称：" prop="name">
          <el-input v-model="model.name" clearable />
        </el-form-item>
        <el-form-item label="编码：" prop="code">
          <el-input v-model="model.code" clearable />
        </el-form-item>
      </template>
      <template #buttons>
        <m-button-add :code="buttons.add.code" @click="add" />
      </template>
      <template #operation="{ row }">
        <m-button type="text" text="字典项" icon="cog" @click="openItemDialog(row)"></m-button>
        <m-button-edit :code="buttons.edit.code" @click="edit(row)" @success="refresh"></m-button-edit>
        <m-button-delete :code="buttons.remove.code" :action="remove" :data="row.id" @success="refresh"></m-button-delete>
      </template>
    </m-list>
    <save :id="selection.id" v-model="saveVisible" :group-code="groupCode" :mode="mode" @success="refresh" />
    <item-dialog v-model="showItemDialog" />
  </m-container>
</template>
<script>
import { reactive, ref, toRefs, watch } from 'vue'
import { useList, entityBaseCols, store } from 'mkh-ui'
import page from '../index/page'
import Save from '../save/index.vue'
import ItemDialog from '../item/index/index.vue'
export default {
  components: { Save, ItemDialog },
  props: {
    groupCode: {
      type: String,
      default: '',
    },
  },
  setup(props) {
    const { query, remove } = mkh.api.admin.dict
    const { groupCode } = toRefs(props)

    const model = reactive({ groupCode, name: '', code: '' })
    const cols = [{ prop: 'id', label: '编号', width: '55', show: false }, { prop: 'name', label: '名称' }, { prop: 'code', label: '编码' }, ...entityBaseCols]

    const list = useList()
    const showItemDialog = ref(false)

    const openItemDialog = row => {
      store.commit('mod/admin/setDict', { groupCode, dictCode: row.code })
      list.selection.value = row
      showItemDialog.value = true
    }

    watch(groupCode, () => {
      list.reset()
    })

    return {
      buttons: page.buttons,
      model,
      cols,
      query,
      remove,
      ...list,
      showItemDialog,
      openItemDialog,
    }
  },
}
</script>
