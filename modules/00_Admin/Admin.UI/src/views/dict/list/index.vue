<template>
  <m-container>
    <m-list ref="listRef" :title="$t('mod.admin.dict_list')" icon="list" :cols="cols" :query-model="model" :query-method="query" :query-on-created="false">
      <template #querybar>
        <el-form-item :label="$t('mkh.name')" prop="name">
          <el-input v-model="model.name" clearable />
        </el-form-item>
        <el-form-item :label="$t('mkh.code')" prop="code">
          <el-input v-model="model.code" clearable />
        </el-form-item>
      </template>
      <template #buttons>
        <m-button-add :code="buttons.add.code" @click="add" />
      </template>
      <template #operation="{ row }">
        <m-button type="text" :text="$t('mod.admin.dict_item')" icon="cog" @click="openItemDialog(row)"></m-button>
        <m-button-edit :code="buttons.edit.code" @click="edit(row)" @success="refresh"></m-button-edit>
        <m-button-delete :code="buttons.remove.code" :action="remove" :data="row.id" @success="refresh"></m-button-delete>
      </template>
    </m-list>
    <save :id="selection?.id" v-model="actionDialogVisible" :group-code="groupCode" :mode="actionMode" @success="refresh" />
    <item-dialog v-model="showItemDialog" />
  </m-container>
</template>
<script setup lang="ts">
  import { reactive, ref, toRefs, watch } from 'vue'
  import { useEntityBaseCols, useList } from 'mkh-ui'
  import { useAdminStore } from '@/store'
  import { buttons } from '../index/page.json'
  import Save from '../save/index.vue'
  import ItemDialog from '../item/index/index.vue'
  import api from '@/api/dict'
  import { DictEntity } from '@/api/dict/dto'

  const props = defineProps({
    groupCode: {
      type: String,
      default: '',
    },
  })

  const store = useAdminStore()

  const { query, remove } = api
  const { groupCode } = toRefs(props)

  const { selection, actionDialogVisible, actionMode, refresh, reset } = useList<DictEntity>()

  const model = reactive({ groupCode, name: '', code: '' })
  const cols = [{ prop: 'id', label: 'mkh.id', width: '55', show: false }, { prop: 'name', label: 'mkh.name' }, { prop: 'code', label: 'mkh.code' }, ...useEntityBaseCols()]

  const showItemDialog = ref(false)

  const openItemDialog = (row) => {
    store.dict.groupCode = groupCode.value
    store.dict.dictCode = row.code

    selection.value = row
    showItemDialog.value = true
  }

  watch(groupCode, () => {
    reset()
  })
</script>
