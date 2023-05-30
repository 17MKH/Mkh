<template>
  <m-container class="m-admin-menu">
    <m-flex-row class="m-fill-h">
      <m-flex-fixed width="300px" class="m-margin-r-10">
        <m-box page :title="t('mod.admin.menu_preview')" icon="menu" no-scrollbar>
          <m-flex-col>
            <m-flex-fixed class="m-text-center m-padding-b-10">
              <m-flex-row>
                <m-flex-auto>
                  <m-select ref="groupSelectRef" v-model="group.id" v-model:label="group.name" :action="menuGroupApi.select" checked-first></m-select>
                </m-flex-auto>
                <m-flex-fixed>
                  <m-button type="primary" :code="buttons.group.code" class="m-margin-l-5" @click="showGroup = true">{{ t('mod.admin.manage_group') }}</m-button>
                </m-flex-fixed>
              </m-flex-row>
            </m-flex-fixed>
            <m-flex-auto>
              <m-container>
                <m-scrollbar>
                  <el-tree
                    ref="treeRef"
                    :data="treeData"
                    :current-node-key="0"
                    node-key="id"
                    draggable
                    highlight-current
                    default-expand-all
                    :expand-on-click-node="false"
                    :allow-drop="handleTreeAllowDrop"
                    :allow-drag="handleTreeAllowDrag"
                    @current-change="handleTreeChange"
                    @node-drop="handleTreeNodeDrop"
                  >
                    <template #default="{ node, data }">
                      <span>
                        <m-icon :name="data.item.icon || 'folder-o'" :style="{ color: data.item.iconColor }" class="m-margin-r-5" />
                        <span>{{ data.item.locales[$i18n.locale] || node.label }}</span>
                      </span>
                    </template>
                  </el-tree>
                </m-scrollbar>
              </m-container>
            </m-flex-auto>
          </m-flex-col>
        </m-box>
      </m-flex-fixed>
      <m-flex-auto>
        <list ref="listRef" :group="group" :parent="parent" @change="refreshTree" />
      </m-flex-auto>
    </m-flex-row>
    <group v-model="showGroup" @change="handleGroupChange" />
  </m-container>
</template>
<script setup lang="ts">
  import type { MenuTreeNode } from '@/types/menu'
  import { nextTick, reactive, Ref, ref, watch } from 'vue'
  import { useI18n } from '@/locales'
  import List from '../list/index.vue'
  import Group from '../group/index/index.vue'
  import page from './page'
  import api from '@/api/menu'
  import menuGroupApi from '@/api/menuGroup'

  const { t } = useI18n()
  const { buttons } = page

  const group = reactive({ id: 0, name: '' })
  const parent = reactive({
    id: 0,
    type: 0,
    locales: null,
  })

  const treeData: Ref<MenuTreeNode[]> = ref([])
  const treeRef = ref()
  const listRef = ref()
  const groupSelectRef = ref()
  const showGroup = ref(false)
  let isInit = false

  //刷新树
  const refreshTree = () => {
    api.getTree({ groupId: group.id }).then((data) => {
      treeData.value = [
        {
          id: 0,
          label: group.name,
          children: data as MenuTreeNode[],
          path: [],
          item: {
            id: 0,
            icon: 'menu',
            type: 0,
            locales: {
              'zh-cn': group.name,
              en: group.name,
            },
          },
        },
      ]
      nextTick(() => {
        treeRef.value.setCurrentKey(parent.id)
        if (!isInit) {
          const root = treeData.value[0]
          handleTreeChange(root)
          isInit = true
        }
      })
    })
  }

  const handleTreeChange = (data) => {
    parent.id = data.id
    parent.locales = data.item.locales
    parent.type = data.item.type
  }

  const handleTreeAllowDrag = (draggingNode) => {
    return draggingNode.data.id > 0
  }

  const handleTreeAllowDrop = (draggingNode, dropNode, type) => {
    if (dropNode.data.id === 0) {
      return false
    }

    if (type === 'inner' && dropNode.data.item.type !== 0) {
      return false
    }

    return true
  }

  const handleTreeNodeDrop = (draggingNode, dropNode, type) => {
    let root = treeRef.value.getNode(0)
    if (draggingNode.level === dropNode.level) {
      root = dropNode.parent
    }
    const menus = []

    resolveSort(root, menus)
    console.log(menus)
    api.updateSort(menus).then(() => {
      listRef.value.refresh()
    })
  }

  const resolveSort = (node, menus) => {
    node.childNodes.forEach((n, i) => {
      menus.push({
        id: n.key,
        sort: i + 1,
        parentId: node.key,
      })

      resolveSort(n, menus)
    })
  }

  const handleGroupChange = () => {
    groupSelectRef.value.refresh()
  }

  watch(group, () => {
    refreshTree()
  })
</script>
<style lang="scss">
  .m-admin-menu {
    &_group_select {
      width: 100%;
    }
  }
</style>
