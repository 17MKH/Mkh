<template>
  <m-drawer :title="$t('mod.admin.manage_group')" icon="list" width="900px" no-scrollbar>
    <m-list ref="listRef" :header="false" :cols="cols" :query-model="model" :query-method="query">
      <template #querybar>
        <el-form-item :label="$t('mkh.name')" prop="name">
          <el-input v-model="model.name" clearable />
        </el-form-item>
      </template>
      <template #buttons>
        <m-button-add :code="buttons.groupAdd.code" @click="add" />
      </template>
      <template #operation="{ row }">
        <m-button-edit :code="buttons.groupEdit.code" @click="edit(row)" @success="handleChange"></m-button-edit>
        <m-button-delete :code="buttons.groupRemove.code" :action="remove" :data="row.id" @success="handleChange"></m-button-delete>
      </template>
    </m-list>
    <action :id="id" v-model="actionProps.visible" :mode="actionProps.mode" @success="handleChange" />
  </m-drawer>
</template>
<script setup lang="ts">
  import { useList, useEntityBaseCols } from 'mkh-ui'
  import { reactive } from 'vue'
  import Action from '../action/index.vue'
  import page from '../../index/page'
  import api from '@/api/menuGroup'

  const emit = defineEmits(['change'])

  const { buttons } = page

  const { query, remove } = api
  const model = reactive({ name: '' })
  const cols = [{ prop: 'id', label: 'mkh.id', width: '55', show: false }, { prop: 'name', label: 'mkh.name' }, { prop: 'remarks', label: 'mkh.remarks' }, ...useEntityBaseCols()]

  const {
    listRef,
    id,
    actionProps,
    methods: { add, edit, refresh },
  } = useList()

  const handleChange = () => {
    refresh()
    emit('change')
  }
</script>
