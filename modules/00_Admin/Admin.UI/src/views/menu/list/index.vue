<template>
  <m-list ref="listRef" :title="$t(`mkh.routes.${page.name}`)" :icon="page.icon" :cols="cols" :query-model="model" :query-method="query" :query-on-created="false">
    <template #querybar>
      <el-form-item :label="$t('mkh.name')" prop="name">
        <el-input v-model="model.name" clearable />
      </el-form-item>
    </template>
    <template #buttons>
      <m-button-add :code="page.buttons.add.code" :disabled="parent.type !== 0" @click="handleAdd"></m-button-add>
    </template>
    <template #expand="{ row }">
      <el-descriptions :column="4">
        <template v-if="row.type === 1">
          <el-descriptions-item :label="$t('mod.admin.route_name')">{{ row.routeName }}</el-descriptions-item>
          <el-descriptions-item :label="$t('mod.admin.route_params')">{{ row.routeParams }}</el-descriptions-item>
          <el-descriptions-item :label="$t('mod.admin.route_query')">{{ row.routeQuery }}</el-descriptions-item>
        </template>
        <template v-else-if="row.type === 2">
          <el-descriptions-item :label="$t('mod.admin.link_url')" :span="2">{{ row.url }}</el-descriptions-item>
          <el-descriptions-item :label="$t('mod.admin.open_target')">{{ row.openTargetName }}</el-descriptions-item>
          <template v-if="row.openTarget === 2">
            <el-descriptions-item :label="$t('mod.admin.dialog_width')">{{ row.dialogWidth }}</el-descriptions-item>
            <el-descriptions-item :label="$t('mod.admin.dialog_height')">{{ row.dialogHeight }}</el-descriptions-item>
          </template>
        </template>
        <template v-else-if="row.type === 3">
          <el-descriptions-item :label="$t('mod.admin.custom_script')" :span="3">{{ row.customJs }}</el-descriptions-item>
        </template>
        <el-descriptions-item :label="$t('mkh.creator')">{{ row.creator }}</el-descriptions-item>
        <el-descriptions-item :label="$t('mkh.created_time')">{{ row.createdTime }}</el-descriptions-item>
        <el-descriptions-item :label="$t('mkh.modifier')">{{ row.modifier }}</el-descriptions-item>
        <el-descriptions-item :label="$t('mkh.modified_time')">{{ row.modifiedTime }}</el-descriptions-item>
        <el-descriptions-item :label="$t('mkh.remarks')" :span="4">{{ row.remarks }}</el-descriptions-item>
      </el-descriptions>
    </template>
    <template #col-name="{ row }">
      {{ JSON.parse(row.localesConfig)[$i18n.locale] }}
    </template>
    <template #col-typeName="{ row }">
      <el-tag v-if="row.type === 0">{{ $t('mod.admin.node') }}</el-tag>
      <el-tag v-else-if="row.type === 1" type="success">{{ $t('mod.admin.route') }}</el-tag>
      <el-tag v-else-if="row.type === 2" type="warning">{{ $t('mod.admin.link') }}</el-tag>
      <el-tag v-else type="warning">{{ $t('mod.admin.script') }}</el-tag>
    </template>
    <template #col-icon="{ row }">
      <m-icon v-if="row.icon" :name="row.icon" :style="{ color: row.iconColor }" />
    </template>
    <template #col-show="{ row }">
      <span>{{ $t(row.show ? 'mkh.yes' : 'mkh.no') }}</span>
    </template>
    <template #operation="{ row }">
      <m-button-edit :code="page.buttons.edit.code" @click.stop="edit(row)" @success="handleChange"></m-button-edit>
      <m-button-delete :code="page.buttons.remove.code" :action="remove" :data="row.id" @success="handleChange"></m-button-delete>
    </template>
  </m-list>
  <save :id="selection.id" v-model="saveVisible" :group="group" :parent="parent" :mode="mode" @success="handleChange" />
</template>
<script>
import { useList } from 'mkh-ui'
import { computed, reactive, watch } from 'vue'
import Save from '../save/index.vue'
import page from '../index/page.json'

export default {
  components: { Save },
  props: {
    group: {
      type: Object,
      required: true,
    },
    parent: {
      type: Object,
      required: true,
    },
  },
  emits: ['change'],
  setup(props, { emit }) {
    const { query, remove } = mkh.api.admin.menu
    const groupId = computed(() => props.group.id)
    const parentId = computed(() => props.parent.id)

    const model = reactive({ groupId, parentId, name: '' })
    const cols = [
      { prop: 'id', label: 'mkh.id', width: '55', show: false },
      { prop: 'name', label: 'mkh.name' },
      { prop: 'typeName', label: 'mkh.type' },
      { prop: 'icon', label: 'mkh.icon' },
      { prop: 'level', label: 'mkh.level' },
      { prop: 'show', label: 'mkh.show' },
      { prop: 'sort', label: 'mkh.sort' },
    ]

    const list = useList()

    watch([groupId, parentId], () => {
      list.refresh()
    })

    const handleChange = () => {
      emit('change')
    }

    const handleAdd = () => {
      console.log(list.mode)
      list.add()
    }

    return {
      ...list,
      page,
      model,
      cols,
      query,
      remove,
      handleChange,
      handleAdd,
    }
  },
}
</script>
