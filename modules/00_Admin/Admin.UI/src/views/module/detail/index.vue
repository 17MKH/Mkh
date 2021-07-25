<template>
  <m-dialog title="模块详情" custom-class="m-admin-module-detail" icon="component" width="80%" height="70%" no-padding no-scrollbar>
    <el-tabs tab-position="left" class="m-fill-h" type="border-card">
      <el-tab-pane>
        <template #label>
          <m-icon name="form" />
          <span class="m-margin-l-5">页面信息</span>
        </template>
        <el-table :data="pages" border style="width: 100%" height="100%">
          <el-table-column type="expand">
            <template #default="props">
              <h2 class="m-margin-b-15">关联按钮</h2>
              <el-table :data="Object.values(props.row.buttons)" border size="mini">
                <el-table-column label="名称" prop="text" width="150" align="center"> </el-table-column>
                <el-table-column label="编码" prop="code" width="300" align="center"> </el-table-column>
                <el-table-column label="绑定权限" prop="permissions">
                  <template #default="{ row }">
                    <p v-for="p in row.permissions" :key="p">{{ p }}</p>
                  </template>
                </el-table-column>
              </el-table>
            </template>
          </el-table-column>
          <el-table-column label="标题" prop="title"> </el-table-column>
          <el-table-column label="图标" prop="name">
            <template #default="{ row }">
              <m-icon v-if="row.icon" :name="row.icon" />
              <span class="m-margin-l-10">{{ row.icon }}</span>
            </template>
          </el-table-column>
          <el-table-column label="路由名称" prop="name"> </el-table-column>
          <el-table-column label="路由地址" prop="path"> </el-table-column>
          <el-table-column label="绑定权限" prop="permissions">
            <template #default="{ row }">
              <p v-for="p in row.permissions" :key="p">{{ p }}</p>
            </template>
          </el-table-column>
        </el-table>
      </el-tab-pane>
      <el-tab-pane>
        <template #label>
          <m-icon name="lock" />
          <span class="m-margin-l-5">权限信息</span>
        </template>
        <el-table :data="permissions" border style="width: 100%" height="100%">
          <el-table-column label="控制器" prop="controller"> </el-table-column>
          <el-table-column label="操作" prop="action"> </el-table-column>
          <el-table-column label="请求方式" prop="httpMethodName"> </el-table-column>
          <el-table-column label="权限编码" prop="code"> </el-table-column>
          <el-table-column label="访问模式" prop="mode">
            <template #default="{ row }">
              <el-tag v-if="row.mode === 0" type="warning">匿名可访问</el-tag>
              <el-tag v-else-if="row.mode === 1" type="primary">登录可访问</el-tag>
              <el-tag v-if="row.mode === 2" type="success">授权可访问</el-tag>
            </template>
          </el-table-column>
        </el-table>
      </el-tab-pane>
    </el-tabs>
  </m-dialog>
</template>
<script>
import { computed, ref, watchEffect } from '@vue/runtime-core'
export default {
  props: {
    mod: {
      type: Object,
      required: true,
    },
  },
  setup(props) {
    const { getPermissions } = mkh.api.admin.module
    const pages = computed(() => {
      return props.mod.pages
    })
    const permissions = ref([])

    watchEffect(() => {
      if (props.mod.code)
        getPermissions({ moduleCode: props.mod.code }).then(data => {
          permissions.value = data
        })
    })

    return {
      pages,
      permissions,
    }
  },
}
</script>
<style lang="scss">
.m-admin-module-detail {
  .el-tabs__content {
    padding: 10px 10px 10px 0;
    height: 100%;
  }
  .el-tab-pane {
    height: 100%;
  }
}
</style>
