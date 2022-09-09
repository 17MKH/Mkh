<template>
  <m-container>
    <m-list ref="listRef" :title="t(`routes.${page.name}`)" :icon="page.icon" :cols="cols" :query-model="model" :query-method="api.query">
      <template #querybar>
        <el-form-item :label="t('mkh.login.username')" prop="username">
          <el-input v-model="model.username" clearable />
        </el-form-item>
        <el-form-item :label="t('mod.admin.name')" prop="name">
          <el-input v-model="model.name" clearable />
        </el-form-item>
        <el-form-item :label="t('mkh.phone')" prop="phone">
          <el-input v-model="model.phone" clearable />
        </el-form-item>
      </template>
      <template #buttons>
        <m-button-add :code="page.buttons!.add.code" @click="add" />
      </template>
      <template #col-status="{ row }">
        <el-tag v-if="row.status === 0" type="info" size="small" effect="dark">{{ t('mod.admin.account_inactive') }}</el-tag>
        <el-tag v-else-if="row.status === 1" type="success" size="small" effect="dark">{{ t('mod.admin.account_activated') }}</el-tag>
        <el-tag v-if="row.status === 2" type="warning" size="small" effect="dark">{{ t('mod.admin.account_disabled') }}</el-tag>
      </template>
      <template #operation="{ row }">
        <m-button-edit :code="page.buttons!.edit.code" @click="edit(row)" @success="refresh"></m-button-edit>
        <m-button-delete :code="page.buttons!.remove.code" :action="api.remove" :data="row.id" @success="refresh"></m-button-delete>
      </template>
    </m-list>
    <action v-model="actionProps.visible" :id="id" :mode="actionProps.mode" @success="refresh"></action>
  </m-container>
</template>
<script setup lang="ts">
  import { useEntityBaseCols } from 'mkh-ui'
  import { reactive } from 'vue'
  import { useList } from 'mkh-ui'
  import page from './page'
  import Action from '../action/index.vue'
  import api from '@/api/account'
  import { useI18n } from '@/locales'

  const { t } = useI18n()

  const model = reactive({ username: '', name: '', phone: '' })
  const cols = [
    { prop: 'id', label: 'mkh.id', width: '55', show: false },
    { prop: 'username', label: 'mkh.login.username' },
    { prop: 'name', label: 'mod.admin.name' },
    { prop: 'roleName', label: 'mkh.role' },
    { prop: 'phone', label: 'mkh.phone' },
    { prop: 'email', label: 'mkh.email' },
    { prop: 'status', label: 'mkh.status' },
    ...useEntityBaseCols(),
  ]

  const {
    listRef,
    id,
    actionProps,
    methods: { add, edit, refresh },
  } = useList<{ id: string }>()
</script>
