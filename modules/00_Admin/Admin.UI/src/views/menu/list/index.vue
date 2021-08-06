<template>
  <m-list ref="listRef" title="菜单列表" icon="list" :cols="cols" :query-model="model" :query-method="query" :query-on-created="false">
    <template #querybar>
      <el-form-item label="名称：" prop="name">
        <el-input v-model="model.name" clearable />
      </el-form-item>
    </template>
    <template #buttons>
      <m-button-add :code="buttons.add.code" :disabled="parent.type !== 0" @click="add"></m-button-add>
    </template>
    <template #expand="{ row }">
      <el-descriptions :column="4">
        <template v-if="row.type === 1">
          <el-descriptions-item label="路由名称：">{{ row.routeName }}</el-descriptions-item>
          <el-descriptions-item label="路由参数(params)：">{{ row.routeParams }}</el-descriptions-item>
          <el-descriptions-item label="路由参数(query)：">{{ row.routeQuery }}</el-descriptions-item>
        </template>
        <template v-else-if="row.type === 2">
          <el-descriptions-item label="链接地址：" :span="2">{{ row.url }}</el-descriptions-item>
          <el-descriptions-item label="打开方式：">{{ row.openTargetName }}</el-descriptions-item>
          <template v-if="row.openTarget === 2">
            <el-descriptions-item label="对话框宽度：">{{ row.dialogWidth }}</el-descriptions-item>
            <el-descriptions-item label="对话框高度：">{{ row.dialogHeight }}</el-descriptions-item>
          </template>
        </template>
        <template v-else-if="row.type === 3">
          <el-descriptions-item label="自定义脚本：" :span="3">{{ row.customJs }}</el-descriptions-item>
        </template>
        <el-descriptions-item label="备注：" :span="4">{{ row.remarks }}</el-descriptions-item>
        <el-descriptions-item label="创建人：">{{ row.creator }}</el-descriptions-item>
        <el-descriptions-item label="创建时间：">{{ row.createdTime }}</el-descriptions-item>
        <el-descriptions-item label="修改人：">{{ row.modifier }}</el-descriptions-item>
        <el-descriptions-item label="修改时间：">{{ row.modifiedTime }}</el-descriptions-item>
      </el-descriptions>
    </template>
    <template #col-typeName="{ row }">
      <el-tag v-if="row.type === 0" type="primary">节点</el-tag>
      <el-tag v-else-if="row.type === 1" type="success">路由</el-tag>
      <el-tag v-else-if="row.type === 2" type="warning">链接</el-tag>
      <el-tag v-else type="warning">脚本</el-tag>
    </template>
    <template #col-icon="{ row }">
      <m-icon v-if="row.icon" :name="row.icon" :style="{ color: row.iconColor }" />
    </template>
    <template #col-show="{ row }">
      <span>{{ row.show ? '是' : '否' }}</span>
    </template>
    <template #operation="{ row }">
      <m-button-edit :code="buttons.edit.code" @click.stop="edit(row)" @success="handleChange"></m-button-edit>
      <m-button-delete :code="buttons.remove.code" :action="remove" :data="row.id" @success="handleChange"></m-button-delete>
    </template>
  </m-list>
  <save :id="selection.id" v-model="saveVisible" :group="group" :parent="parent" :mode="mode" @success="handleChange" />
</template>
<script>
import { useList } from 'mkh-ui'
import { computed, reactive, watch } from 'vue'
import Save from '../save/index.vue'
import page from '../index/page'

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
    const { buttons } = page
    const { query, remove } = mkh.api.admin.menu
    const groupId = computed(() => props.group.id)
    const parentId = computed(() => props.parent.id)

    const model = reactive({ groupId, parentId, name: '' })
    const cols = [
      { prop: 'id', label: '编号', width: '55', show: false },
      { prop: 'name', label: '名称' },
      { prop: 'typeName', label: '类型' },
      { prop: 'icon', label: '图标' },
      { prop: 'level', label: '层级' },
      { prop: 'show', label: '是否显示' },
      { prop: 'sort', label: '排序' },
    ]

    const list = useList()

    watch([groupId, parentId], () => {
      list.refresh()
    })

    const handleChange = () => {
      list.refresh()
      emit('change')
    }

    return {
      buttons,
      model,
      cols,
      query,
      remove,
      ...list,
      handleChange,
    }
  },
}
</script>
