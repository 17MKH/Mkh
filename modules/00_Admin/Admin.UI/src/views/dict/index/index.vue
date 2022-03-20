<template>
  <m-container>
    <m-flex-row>
      <m-flex-fixed width="300px" class="m-margin-r-10">
        <m-list-box ref="listBoxRef" :title="$t(`mkh.routes.${page.name}`)" :icon="page.icon" :action="$mkh.api.admin.dictGroup.query" @change="handleGroupChange">
          <template #toolbar>
            <m-button :code="page.buttons.groupAdd.code" icon="plus" @click="add"></m-button>
          </template>
          <template #label="{ item }">
            <m-icon :name="item.icon || 'dict'" style="font-size: 20px; margin-right: 5px"></m-icon>
            <span>{{ item.name }}</span>
          </template>
          <template #action="{ item }">
            <m-button-edit text="" :code="page.buttons.groupEdit.code" @click.stop="edit(item)" @success="refresh"></m-button-edit>
            <m-button-delete text="" :code="page.buttons.groupRemove.code" :action="$mkh.api.admin.dictGroup.remove" :data="item.id" @success="refresh"></m-button-delete>
          </template>
        </m-list-box>
      </m-flex-fixed>
      <m-flex-auto>
        <m-flex-col class="m-fill-h">
          <m-flex-fixed>
            <m-box :title="$t('mod.admin.group_info')" icon="preview" show-collapse>
              <el-descriptions :column="2" border>
                <el-descriptions-item :label="$t('mkh.name')">{{ current.name }}</el-descriptions-item>
                <el-descriptions-item :label="$t('mkh.code')">{{ current.code }}</el-descriptions-item>
                <el-descriptions-item :label="$t('mkh.remarks')" :span="2">{{ current.remarks }}</el-descriptions-item>
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
import page from './page.json'
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

    return { page, current, listBoxRef, selection, mode, saveVisible, add, edit, refresh, handleGroupChange }
  },
}
</script>
