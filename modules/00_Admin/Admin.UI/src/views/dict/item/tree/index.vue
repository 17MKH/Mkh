<template>
  <div class="m-padding-10">
    <el-tree ref="treeRef" :data="treeData" :current-node-key="currentKey" node-key="id" highlight-current default-expand-all :expand-on-click-node="false" @current-change="handleCurrentChange">
      <template #default="{ node, data }">
        <span>
          <m-icon :name="data.item.icon || 'folder-o'" class="m-margin-r-5" />
          <span>{{ node.label }}</span>
        </span>
      </template>
    </el-tree>
  </div>
</template>
<script>
import { computed, nextTick, reactive, ref, watch } from 'vue'
export default {
  emits: ['change'],
  setup(props, { emit }) {
    const { store } = mkh

    const currentKey = ref(0)
    const treeData = ref([])
    const treeRef = ref()

    const adminStore = store.state.mod.admin
    const groupCode = computed(() => adminStore.dict.groupCode)
    const dictCode = computed(() => adminStore.dict.dictCode)

    const model = reactive({ groupCode, dictCode })

    const refresh = () => {
      mkh.api.admin.dict.tree(model).then(data => {
        treeData.value = data
        nextTick(() => {
          treeRef.value.setCurrentKey(currentKey.value)
        })
      })
    }

    watch([groupCode, dictCode], refresh)

    refresh()

    const handleCurrentChange = ({ id }) => {
      currentKey.value = id
      emit('change', id)
    }

    return { currentKey, treeData, treeRef, refresh, handleCurrentChange }
  },
}
</script>
