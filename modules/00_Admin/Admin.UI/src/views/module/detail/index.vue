<template>
  <m-dialog :title="$t('mod.admin.module_details')" custom-class="m-admin-module-detail" icon="component" width="80%" height="70%" no-padding no-scrollbar>
    <el-tabs tab-position="left" class="m-fill-h" type="border-card">
      <el-tab-pane>
        <template #label>
          <m-icon name="form" />
          <span class="m-margin-l-5">{{ $t('mod.admin.page_info') }}</span>
        </template>
        <el-table :data="pages" border style="width: 100%" height="100%">
          <el-table-column type="expand">
            <template #default="props">
              <h2 class="m-margin-b-15">{{ $t('mod.admin.relation_button') }}</h2>
              <el-table :data="Object.values(props.row.buttons)" border size="mini">
                <el-table-column :label="$t('mkh.name')" prop="text" width="150" align="center"> </el-table-column>
                <el-table-column :label="$t('mkh.code')" prop="code" width="300" align="center"> </el-table-column>
                <el-table-column :label="$t('mod.admin.bind_permission')" prop="permissions">
                  <template #default="{ row }">
                    <p v-for="p in row.permissions" :key="p">{{ p }}</p>
                  </template>
                </el-table-column>
              </el-table>
            </template>
          </el-table-column>
          <el-table-column :label="$t('mkh.title')" prop="title">
            <template #default="{ row }">
              {{ $t('mkh.routes.' + row.name) }}
            </template>
          </el-table-column>
          <el-table-column :label="$t('mkh.icon')" prop="icon">
            <template #default="{ row }">
              <m-icon v-if="row.icon" :name="row.icon" />
              <span class="m-margin-l-10">{{ row.icon }}</span>
            </template>
          </el-table-column>
          <el-table-column :label="$t('mod.admin.route_name')" prop="name"> </el-table-column>
          <el-table-column :label="$t('mod.admin.route_path')" prop="path"> </el-table-column>
          <el-table-column :label="$t('mod.admin.bind_permission')" prop="permissions">
            <template #default="{ row }">
              <p v-for="p in row.permissions" :key="p">{{ p }}</p>
            </template>
          </el-table-column>
        </el-table>
      </el-tab-pane>
      <el-tab-pane>
        <template #label>
          <m-icon name="lock" />
          <span class="m-margin-l-5">{{ $t('mod.admin.permission_info') }}</span>
        </template>
        <el-table :data="permissions" border style="width: 100%" height="100%">
          <el-table-column :label="$t('mod.admin.controller')" prop="controller"> </el-table-column>
          <el-table-column :label="$t('mkh.operate')" prop="action"> </el-table-column>
          <el-table-column :label="$t('mod.admin.http_method')" prop="httpMethodName"> </el-table-column>
          <el-table-column :label="$t('mod.admin.permission_code')" prop="code"> </el-table-column>
          <el-table-column :label="$t('mod.admin.permission_mode')" prop="mode">
            <template #default="{ row }">
              <el-tag v-if="row.mode === 0" type="warning">{{ $t('mod.admin.permission_mode_0') }}</el-tag>
              <el-tag v-else-if="row.mode === 1">{{ $t('mod.admin.permission_mode_1') }}</el-tag>
              <el-tag v-if="row.mode === 2" type="success">{{ $t('mod.admin.permission_mode_2') }}</el-tag>
            </template>
          </el-table-column>
        </el-table>
      </el-tab-pane>
    </el-tabs>
  </m-dialog>
</template>
<script>
import { computed, ref, watchEffect } from 'vue'
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
      console.log(props.mod.pages)
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
