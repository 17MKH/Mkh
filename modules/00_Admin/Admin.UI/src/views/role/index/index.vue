<template>
  <m-container class="m-admin-role">
    <m-flex-row>
      <m-flex-fixed width="250px" class="m-margin-r-10">
        <m-box page title="角色管理" icon="role" no-padding>
          <template #toolbar>
            <m-button :code="buttons.add.code" icon="plus" @click="add"></m-button>
          </template>
          <div v-for="role in roles" :key="role.id" :class="['m-admin-role_item', role.id === current.id ? 'active' : '']" @click="handleRoleChange(role)">
            <m-flex-row>
              <m-flex-auto>
                <span>{{ role.name }}</span>
              </m-flex-auto>
              <m-flex-fixed v-if="!role.locked">
                <m-button-edit :code="buttons.edit.code" @click.stop="edit(role)" @success="refresh"></m-button-edit>
                <m-button-delete :code="buttons.remove.code" :action="remove" :data="role.id" @success="refresh"></m-button-delete>
              </m-flex-fixed>
            </m-flex-row>
          </div>
        </m-box>
      </m-flex-fixed>
      <m-flex-auto>
        <m-tabs type="border-card">
          <el-tab-pane>
            <template #label>
              <span><m-icon name="preview" /> 基本信息</span>
            </template>
            <detail :role="current" />
          </el-tab-pane>
          <el-tab-pane>
            <template #label>
              <span><m-icon name="menu" /> 菜单绑定</span>
            </template>
            <bind-menu :role="current" />
          </el-tab-pane>
        </m-tabs>
      </m-flex-auto>
    </m-flex-row>
    <save :id="selection.id" v-model="saveVisible" :mode="mode" @success="refresh" />
  </m-container>
</template>
<script>
import { ref } from 'vue'
import { useList } from 'mkh-ui'
import page from './page'
import Save from '../save/index.vue'
import Detail from '../detail/index.vue'
import BindMenu from '../bind-menu/index.vue'
export default {
  components: { Save, Detail, BindMenu },
  setup() {
    const { buttons } = page
    const { query, remove } = mkh.api.admin.role
    const roles = ref([])
    const current = ref({ id: 0 })
    const { selection, mode, saveVisible, add, edit } = useList()

    const refresh = () => {
      selection.value = ''
      query().then(data => {
        roles.value = data
        if (data.length > 0) {
          handleRoleChange(data[0])
        }
      })
    }

    const handleRoleChange = role => {
      current.value = role
    }

    refresh()

    return {
      buttons,
      roles,
      current,
      selection,
      mode,
      saveVisible,
      add,
      edit,
      remove,
      refresh,
      handleRoleChange,
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
