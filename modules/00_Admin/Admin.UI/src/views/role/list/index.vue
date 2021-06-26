<template>
  <m-container class="m-admin-role">
    <m-flex-row>
      <m-flex-fixed width="250px" class="m-margin-r-10">
        <m-box page title="角色管理" icon="role" no-padding>
          <template #toolbar>
            <m-button icon="plus" @click="add"></m-button>
          </template>
          <el-collapse v-model="selection" accordion class="role-list">
            <el-collapse-item v-for="role in rows" :key="role.id" :title="role.name" :name="role.id">
              <div class="m-text-right">
                <m-button type="text" text="编辑" icon="edit" @click="edit(role)"></m-button>
                <m-button-delete type="text" text="" :action="remove" :data="role.id" @success="refresh"></m-button-delete>
              </div>
              <el-descriptions :column="1">
                <el-descriptions-item label="编码：">{{ role.code }}</el-descriptions-item>
                <el-descriptions-item label="备注：">{{ role.remarks }}</el-descriptions-item>
                <el-descriptions-item label="创建人：">{{ role.creator }}</el-descriptions-item>
                <el-descriptions-item label="创建时间：">{{ role.createdTime }}</el-descriptions-item>
              </el-descriptions>
            </el-collapse-item>
          </el-collapse>
        </m-box>
      </m-flex-fixed>
      <m-flex-auto>
        <el-tabs class="page" type="border-card">
          <el-tab-pane>
            <template #label>
              <span><m-icon name="menu" /> 菜单绑定</span>
            </template>
          </el-tab-pane>
        </el-tabs>
      </m-flex-auto>
    </m-flex-row>
    <!-- <m-list ref="listRef" title="角色列表" :cols="cols" :query-model="model" :query-method="query">
      <template #querybar>
        <el-form-item label="名称：" prop="name">
          <el-input v-model="model.name" clearable></el-input>
        </el-form-item>
      </template>
      <template #querybar-buttons>
        <m-button type="success" icon="plus" text="添加" @click="add" />
      </template>

      <template #operation="{ row }">
        <m-button type="text" text="编辑" icon="edit" @click="edit(row)" />
      </template>
    </m-list> -->
    <save :id="selection" v-model="saveVisible" :mode="mode" @success="refresh" />
  </m-container>
</template>
<script>
import { ref } from 'vue'
import { useList } from 'mkh-ui'
import Save from '../save/index.vue'
export default {
  components: { Save },
  setup() {
    const { query, remove } = mkh.api.admin.role
    const rows = ref([])
    const selection = ref('')
    const { mode, saveVisible, add, edit } = useList()

    const refresh = () => {
      selection.value = ''
      query().then(data => {
        rows.value = data
      })
    }

    refresh()

    return {
      rows,
      selection,
      mode,
      saveVisible,
      add,
      edit,
      refresh,
      remove,
    }
  },
}
</script>
<style lang="scss">
.m-admin-role {
  .role-list {
    .el-collapse-item__header {
      font-weight: bold;
    }
    .el-collapse-item__header,
    .el-collapse-item__wrap {
      padding-left: 10px;
    }
    .el-collapse-item__content {
      padding-bottom: 0;
    }
  }
}
</style>
