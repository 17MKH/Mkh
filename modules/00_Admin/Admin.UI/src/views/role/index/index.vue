<template>
  <m-container class="m-admin-role">
    <m-flex-row>
      <m-flex-fixed width="300px" class="m-margin-r-10">
        <m-list-box ref="listBoxRef" v-model="roleId" title="角色列表" icon="role" :action="query" @change="handleRoleChange">
          <template #toolbar>
            <m-button :code="buttons.add.code" icon="plus" @click="add"></m-button>
          </template>
          <template #action="{ item }">
            <m-button-edit text="" :code="buttons.edit.code" @click.stop="edit(item)" @success="refresh"></m-button-edit>
            <m-button-delete text="" :code="buttons.remove.code" :action="remove" :data="item.id" @success="refresh"></m-button-delete>
          </template>
        </m-list-box>
      </m-flex-fixed>
      <m-flex-auto>
        <m-flex-col class="m-fill-h">
          <m-flex-fixed>
            <m-box title="角色信息" icon="preview" show-collapse>
              <el-descriptions :column="4" border>
                <el-descriptions-item label="名称">{{ current.name }}</el-descriptions-item>
                <el-descriptions-item label="唯一编码">{{ current.code }}</el-descriptions-item>
                <el-descriptions-item label="绑定菜单组">{{ current.menuGroupName }}</el-descriptions-item>
                <el-descriptions-item label="是否锁定">
                  <el-tag v-if="current.locked" type="danger" effect="dark" size="small">锁定</el-tag>
                  <el-tag v-else type="info" effect="dark" size="small">未锁定</el-tag>
                </el-descriptions-item>
                <el-descriptions-item label="创建时间">{{ current.createdTime }}</el-descriptions-item>
                <el-descriptions-item label="创建人">{{ current.creator }}</el-descriptions-item>
                <el-descriptions-item label="备注" :span="2">{{ current.remarks }}</el-descriptions-item>
              </el-descriptions>
            </m-box>
          </m-flex-fixed>
          <m-flex-auto class="m-margin-t-10">
            <bind-menu :role="current" />
          </m-flex-auto>
        </m-flex-col>
      </m-flex-auto>
    </m-flex-row>
    <save :id="selection.id" v-model="saveVisible" :mode="mode" @success="refresh" />
  </m-container>
</template>
<script>
import { ref } from 'vue'
import { useList } from 'mkh-ui'
import { buttons } from './page.json'
import Save from '../save/index.vue'
import BindMenu from '../bind-menu/index.vue'
export default {
  components: { Save, BindMenu },
  setup() {
    const { query, remove } = mkh.api.admin.role
    const roles = ref([])
    const roleId = ref('')
    const current = ref({ id: 0 })
    const listBoxRef = ref()
    const { selection, mode, saveVisible, add, edit } = useList()

    const handleRoleChange = (val, role) => {
      console.log(role)
      current.value = role
    }

    const refresh = () => {
      console.log(listBoxRef.value)
      listBoxRef.value.refresh()
    }

    return {
      buttons,
      roles,
      roleId,
      current,
      listBoxRef,
      selection,
      mode,
      saveVisible,
      add,
      edit,
      remove,
      query,
      handleRoleChange,
      refresh,
    }
  },
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
