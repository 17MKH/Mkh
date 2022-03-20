<template>
  <m-dialog :title="$t('mod.admin.dict_item_manage')" icon="dict" width="80%" height="80%" no-scrollbar no-padding :close-on-click-modal="false">
    <m-split v-model="split">
      <template #fixed>
        <tree ref="treeRef" @change="handleTreeChange" />
      </template>
      <template #auto>
        <list :parent-id="parentId" @change="handleListChange" />
      </template>
    </m-split>
  </m-dialog>
</template>
<script>
import { ref } from 'vue'
import Tree from '../tree/index.vue'
import List from '../list/index.vue'
export default {
  components: { Tree, List },
  setup() {
    const split = ref('250px')
    const parentId = ref(0)
    const treeRef = ref()

    const handleTreeChange = id => {
      parentId.value = id
    }

    const handleListChange = () => {
      treeRef.value.refresh()
    }

    return {
      split,
      parentId,
      treeRef,
      handleTreeChange,
      handleListChange,
    }
  },
}
</script>
