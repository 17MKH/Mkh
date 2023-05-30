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
<script setup lang="ts">
  import { computed, nextTick, reactive, ref, watch } from 'vue'
  import useStore from '@/store'
  import api from '@/api/dict'

  const emit = defineEmits(['change'])

  const currentKey = ref(0)
  const treeData = ref([])
  const treeRef = ref()

  const store = useStore()
  const groupCode = computed(() => store.dict.groupCode)
  const dictCode = computed(() => store.dict.dictCode)

  const model = reactive({ groupCode, dictCode })

  const refresh = () => {
    api.tree(model).then((data: any) => {
      treeData.value = data
      nextTick(() => {
        treeRef.value.setCurrentKey(currentKey.value)
      })
    })
  }

  watch([groupCode, dictCode], refresh)

  refresh()

  const handleCurrentChange = (node) => {
    currentKey.value = node.data.id
    emit('change', node.data.id)
  }

  defineExpose({ refresh })
</script>
