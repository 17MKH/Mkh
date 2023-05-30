<template>
  <m-container>
    <m-list ref="listRef" :title="t('mod.admin.dict_list')" icon="list" :cols="cols" :query-model="model" :query-method="query" :query-on-created="false">
      <template #querybar>
        <el-form-item :label="t('mkh.name')" prop="name">
          <el-input v-model="model.name" clearable />
        </el-form-item>
        <el-form-item :label="t('mkh.code')" prop="code">
          <el-input v-model="model.code" clearable />
        </el-form-item>
      </template>
      <template #buttons>
        <m-button-add :code="page.buttons.add.code" @click="add" />
      </template>
      <template #operation="{ row }">
        <m-button type="primary" text icon="cog" @click="openItemDialog(row)">{{ t('mod.admin.dict_item') }}</m-button>
        <m-button-edit :code="page.buttons.edit.code" @click="edit(row)" @success="refresh"></m-button-edit>
        <m-button-delete :code="page.buttons.remove.code" :action="remove" :data="row.id" @success="refresh"></m-button-delete>
      </template>
    </m-list>
    <action v-model="actionProps.visible" :id="id" :mode="actionProps.mode" :group-code="groupCode" @success="refresh" />
    <item-dialog v-model="showItemDialog" />
  </m-container>
</template>
<script setup lang="ts">
  import { reactive, ref, toRefs, watch } from 'vue'
  import { useEntityBaseCols, useList } from 'mkh-ui'
  import { useI18n } from '@/locales'
  import useStore from '@/store'
  import page from '../index/page'
  import Action from '../action/index.vue'
  import ItemDialog from '../item/index/index.vue'
  import api from '@/api/dict'

  const { t } = useI18n()
  const store = useStore()

  const props = defineProps({
    groupCode: {
      type: String,
      default: '',
    },
  })

  const { query, remove } = api
  const { groupCode } = toRefs(props)

  const model = reactive({ groupCode, name: '', code: '' })
  const cols = [{ prop: 'id', label: 'mkh.id', width: '55', show: false }, { prop: 'name', label: 'mkh.name' }, { prop: 'code', label: 'mkh.code' }, ...useEntityBaseCols()]

  const {
    listRef,
    selection,
    id,
    actionProps,
    methods: { add, edit, refresh, reset },
  } = useList()

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
