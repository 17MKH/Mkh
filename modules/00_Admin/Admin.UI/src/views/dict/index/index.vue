<template>
  <m-container>
    <m-flex-row>
      <m-flex-fixed width="300px" class="m-margin-r-10">
        <m-list-box ref="listBoxRef" title="分组列表" icon="group" :action="$mkh.api.admin.dictGroup.query" @change="handleGroupChange">
          <template #toolbar>
            <m-button :code="buttons.groupAdd.code" icon="plus" @click="add"></m-button>
          </template>
          <template #label="{ item }">
            <m-icon :name="item.icon || 'dict'" style="font-size: 20px; margin-right: 5px"></m-icon>
            <span>{{ item.name }}</span>
          </template>
          <template #action="{ item }">
            <m-button-edit text="" :code="buttons.groupEdit.code" @click.stop="edit(item)" @success="refresh"></m-button-edit>
            <m-button-delete text="" :code="buttons.groupRemove.code" :action="$mkh.api.admin.dictGroup.remove" :data="item.id" @success="refresh"></m-button-delete>
          </template>
        </m-list-box>
      </m-flex-fixed>
      <m-flex-auto>
        <m-flex-col class="m-fill-h">
          <m-flex-fixed>
            <m-box title="分组信息" icon="preview" show-collapse>
              <el-descriptions :column="2" border>
                <el-descriptions-item label="名称">{{ current.name }}</el-descriptions-item>
                <el-descriptions-item label="唯一编码">{{ current.code }}</el-descriptions-item>
                <el-descriptions-item label="备注" :span="2">{{ current.remarks }}</el-descriptions-item>
              </el-descriptions>
            </m-box>
          </m-flex-fixed>
          <m-flex-auto class="m-margin-t-10">
            <list :group-code="current.code" />
          </m-flex-auto>
        </m-flex-col>
      </m-flex-auto>
    </m-flex-row>
    <group-save :id="selection.id" v-model="saveVisible" :mode="mode" @success="refresh" />
  </m-container>
</template>
<script>
import { ref } from 'vue'
import { useList } from 'mkh-ui'
import page from './page'
import List from '../list/index.vue'
import GroupSave from '../group-save/index.vue'
export default {
  components: { List, GroupSave },
  setup() {
    const current = ref({})
    const listBoxRef = ref()
    const { selection, mode, saveVisible, add, edit } = useList()

    const handleGroupChange = (val, group) => {
      current.value = group
    }

    const refresh = () => {
      listBoxRef.value.refresh()
    }

    return { buttons: page.buttons, current, listBoxRef, selection, mode, saveVisible, add, edit, refresh, handleGroupChange }
  },
}
</script>
