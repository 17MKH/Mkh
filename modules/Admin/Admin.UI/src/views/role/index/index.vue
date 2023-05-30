<template>
  <m-container class="m-admin-role">
    <m-flex-row class="m-fill-h">
      <m-flex-fixed width="300px" class="m-margin-r-10">
        <m-list-box ref="listBoxRef" v-model="roleId" :title="$t('mod.admin.role_list')" icon="role" :action="api.query" @change="handleRoleChange">
          <template #toolbar>
            <m-button :code="buttons.add.code" icon="plus" @click="add"></m-button>
          </template>
          <template #action="{ item }">
            <m-button-edit :code="buttons.edit.code" @click.stop="edit(item)" @success="refresh"></m-button-edit>
            <m-button-delete :code="buttons.remove.code" :action="api.remove" :data="item.id" @success="refresh"></m-button-delete>
          </template>
        </m-list-box>
      </m-flex-fixed>
      <m-flex-auto>
        <m-flex-col class="m-fill-h">
          <m-flex-fixed>
            <m-box :title="$t('mod.admin.role_info')" icon="preview" show-collapse>
              <el-descriptions :column="4" border>
                <el-descriptions-item :label="$t('mkh.name')">{{ current.name }}</el-descriptions-item>
                <el-descriptions-item :label="$t('mkh.code')">{{ current.code }}</el-descriptions-item>
                <el-descriptions-item :label="$t('mod.admin.bind_menu_group')">{{ current.menuGroupName }}</el-descriptions-item>
                <el-descriptions-item :label="$t('mod.admin.is_lock')">
                  <el-tag v-if="current.locked" type="danger" effect="dark" size="small">{{ $t('mod.admin.lock') }}</el-tag>
                  <el-tag v-else type="info" effect="dark" size="small">{{ $t('mod.admin.not_lock') }}</el-tag>
                </el-descriptions-item>
                <el-descriptions-item :label="$t('mkh.creator')">{{ current.creator }}</el-descriptions-item>
                <el-descriptions-item :label="$t('mkh.created_time')">{{ current.createdTime }}</el-descriptions-item>
                <el-descriptions-item :label="$t('mkh.remarks')" :span="2">{{ current.remarks }}</el-descriptions-item>
              </el-descriptions>
            </m-box>
          </m-flex-fixed>
          <m-flex-auto class="m-margin-t-10">
            <bind-menu :role="current" />
          </m-flex-auto>
        </m-flex-col>
      </m-flex-auto>
    </m-flex-row>
    <action v-model="actionProps.visible" :id="id" :mode="actionProps.mode" @success="refresh"></action>
  </m-container>
</template>
<script setup lang="ts">
  import { ref } from 'vue'
  import { useList } from 'mkh-ui'
  import Action from '../action/index.vue'
  import BindMenu from '../bind-menu/index.vue'
  import page from './page'
  import api from '@/api/role'

  const { buttons } = page
  const roleId = ref('')
  const current = ref({ id: 0, name: '', code: '', locked: false, menuGroupName: '', creator: '', createdTime: '', remarks: '' })
  const listBoxRef = ref()

  const {
    id,
    actionProps,
    methods: { add, edit },
  } = useList()

  const handleRoleChange = (val, role) => {
    current.value = role
  }

  const refresh = () => {
    console.log(listBoxRef.value)
    listBoxRef.value.refresh()
  }
</script>
<style lang="scss">
  .m-admin-role {
    &_item {
      padding: 0 20px;
      height: 45px;
      line-height: 45px;
      cursor: pointer;
      border-bottom: 1px solid #ebeef5;

      &:hover {
        background-color: #ecf5ff;
      }

      &.active {
        background-color: #ecf5ff;
        border-left: 3px solid #409eff;
      }
    }
  }
</style>
