<template>
  <m-container>
    <m-list ref="listRef" :title="$t(`mkh.routes.${page.name}`)" :icon="page.icon" :cols="cols" :query-model="model" :query-method="query">
      <template #querybar>
        <el-form-item :label="$t('mkh.login.username')" prop="username">
          <el-input v-model="model.username" clearable />
        </el-form-item>
        <el-form-item :label="$t('mod.admin.name')" prop="name">
          <el-input v-model="model.name" clearable />
        </el-form-item>
        <el-form-item :label="$t('mkh.phone')" prop="phone">
          <el-input v-model="model.phone" clearable />
        </el-form-item>
      </template>
      <template #buttons>
        <m-button-add :code="page.buttons.add.code" @click="add" />
      </template>
      <template #col-status="{ row }">
        <el-tag v-if="row.status === 0" type="info" size="small" effect="dark">{{ $t('mod.admin.account_inactive') }}</el-tag>
        <el-tag v-else-if="row.status === 1" type="success" size="small" effect="dark">{{ $t('mod.admin.account_activated') }}</el-tag>
        <el-tag v-if="row.status === 2" type="warning" size="small" effect="dark">{{ $t('mod.admin.account_disabled') }}</el-tag>
      </template>
      <template #operation="{ row }">
        <m-button-edit :code="page.buttons.edit.code" @click="edit(row)" @success="refresh"></m-button-edit>
        <m-button-delete :code="page.buttons.remove.code" :action="remove" :data="row.id" @success="refresh"></m-button-delete>
      </template>
    </m-list>
    <save :id="selection.id" v-model="saveVisible" :mode="mode" @success="refresh" />
  </m-container>
</template>
<script>
import { useList, entityBaseCols } from 'mkh-ui'
import { reactive } from 'vue'
import page from './page.json'
import Save from '../save/index.vue'
export default {
  components: { Save },
  setup() {
    const { query, remove } = mkh.api.admin.account
    const model = reactive({ username: '', name: '', phone: '' })
    const cols = [
      { prop: 'id', label: 'mkh.id', width: '55', show: false },
      { prop: 'username', label: 'mkh.login.username' },
      { prop: 'name', label: 'mod.admin.name' },
      { prop: 'roleName', label: 'mkh.role' },
      { prop: 'phone', label: 'mkh.phone' },
      { prop: 'email', label: 'mkh.email' },
      { prop: 'status', label: 'mkh.status' },
      ...entityBaseCols(),
    ]

    const list = useList()

    return {
      page,
      model,
      cols,
      query,
      remove,
      ...list,
    }
  },
}
</script>
