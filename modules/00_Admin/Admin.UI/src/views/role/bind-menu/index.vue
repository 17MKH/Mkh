<template>
  <m-container>
    <m-box page :title="$t('mod.admin.menu_authorization')" icon="link" :loading="loading" show-fullscreen>
      <el-alert :title="$t('mod.admin.menu_authorization_alert')" type="warning" class="m-margin-b-20"> </el-alert>
      <el-tree ref="treeRef" class="m-admin-bind-menu" :data="treeData" node-key="id" show-checkbox default-expand-all>
        <template #default="{ node, data }">
          <m-icon class="m-admin-bind-menu_icon" :name="data.item.icon || 'folder-o'" :style="{ color: data.item.iconColor }" />
          <span class="m-admin-bind-menu_label">{{ data.item.locales[$i18n.locale] || node.label }}</span>
          <div class="m-admin-bind-menu_buttons">
            <template v-if="data.id === 0">
              <el-checkbox v-model="checkedAllButton" :label="$t('mod.admin.select_all')" @change="handleCheckedAllButton"></el-checkbox>
            </template>
            <template v-else>
              <el-checkbox v-for="b in data.buttons" :key="b.code" v-model="b.checked" :label="$t(b.text)" :disabled="!node.checked" @change="handleCheckedButton"></el-checkbox>
            </template>
          </div>
        </template>
      </el-tree>
      <template #footer>
        <m-button type="success" icon="save" @click="submit">{{ $t('mkh.save') }}</m-button>
      </template>
    </m-box>
  </m-container>
</template>
<script>
import { getCurrentInstance, nextTick, ref, toRefs, watch } from 'vue'
import { useMessage } from 'mkh-ui'
export default {
  props: {
    role: {
      type: Object,
      required: true,
    },
  },
  setup(props) {
    const { store } = mkh
    const cit = getCurrentInstance().proxy

    const message = useMessage()
    const { menu: menuApi, role: roleApi } = mkh.api.admin

    const { role } = toRefs(props)
    const treeRef = ref()
    const treeData = ref([])
    const checkedAllButton = ref(false) //选中所有按钮
    const allButtons = ref([]) //保存所有的按钮对象引用，方便全选时切换
    const loading = ref(false)

    //解析所有页面信息
    let pages = []
    mkh.modules.forEach(m => {
      pages = pages.concat(m.pages)
    })

    //刷新菜单树
    const refreshTree = () => {
      menuApi.getTree({ groupId: role.value.menuGroupId }).then(data => {
        allButtons.value = []

        var groupName = role.value.menuGroupName
        treeData.value = [
          {
            id: 0,
            label: groupName,
            children: data.map(n => {
              resolvePage(n)
              return n
            }),
            path: [],
            item: {
              id: 0,
              icon: 'menu',
              type: 0,
              locales: {
                'zh-cn': groupName,
                en: groupName,
              },
            },
          },
        ]

        refreshBindMenus()
      })
    }

    //刷新绑定数据
    const refreshBindMenus = () => {
      roleApi.queryBindMenus({ id: role.value.id }).then(data => {
        nextTick(() => {
          data.menus.forEach(m => {
            if (m.menuType !== 0) {
              treeRef.value.setChecked(m.menuId, true)

              if (m.menuType === 1 && m.buttons && m.buttons.length > 0) {
                m.buttons.forEach(b => {
                  const button = allButtons.value.find(x => x.menuId === m.menuId && x.code === b)
                  if (button) {
                    button.checked = true
                  }
                })
              }
            }
          })
          handleCheckedButton()
        })
      })
    }

    //解析页面和按钮
    const resolvePage = node => {
      let type = node.item.type
      if (type === 1) {
        const page = pages.find(p => p.name === node.item.routeName)
        if (page) {
          node.page = page
          if (page.buttons) {
            node.buttons = Object.values(page.buttons).map(m => {
              let btn = { ...m }
              btn.menuId = node.item.id
              btn.checked = false
              return btn
            })
            allButtons.value.push(...node.buttons)
          } else {
            node.buttons = []
          }
        }
      } else if (type === 0) {
        node.children.forEach(m => {
          resolvePage(m)
        })
      }
    }

    const handleCheckedAllButton = val => {
      allButtons.value.forEach(b => {
        b.checked = val
      })
    }

    const handleCheckedButton = () => {
      checkedAllButton.value = allButtons.value.every(m => m.checked)
    }

    const submit = () => {
      const model = {
        roleId: role.value.id,
        menus: [],
      }
      treeRef.value.getCheckedNodes().forEach(n => {
        if (n.id !== 0) {
          const menu = {
            menuId: n.id,
            menuType: n.item.type,
            buttons: [],
            permissions: [],
          }

          //路由节点
          if (n.item.type === 1) {
            const buttons = n.buttons.filter(b => b.checked)
            menu.buttons = buttons.map(b => b.code)

            //保存页面关联权限
            if (n.page.permissions) menu.permissions.push(...n.page.permissions)

            //保存按钮关联权限
            buttons.forEach(b => {
              if (b.permissions) menu.permissions.push(...b.permissions)
            })
          }

          model.menus.push(menu)
        }
      })

      //只有节点才会半选中，所以不会有按钮
      treeRef.value.getHalfCheckedNodes().forEach(n => {
        if (n.id !== 0) {
          model.menus.push({
            menuId: n.id,
            menuType: n.item.type,
          })
        }
      })

      loading.value = true
      roleApi.updateBindMenus(model).then(() => {
        //如果编辑的是当前登录人关联的角色，则刷新
        if (role.value.id === store.state.app.profile.roleId) {
          store.dispatch('app/profile/init', null, { root: true }).then(() => {
            message.success(mkh.$t('mkh.save_success_msg'))
            loading.value = false
          })
        } else {
          loading.value = false
        }
      })
    }

    watch(role, () => {
      refreshTree()
    })

    return {
      treeRef,
      treeData,
      checkedAllButton,
      loading,
      handleCheckedAllButton,
      handleCheckedButton,
      submit,
    }
  },
}
</script>
<style lang="scss">
.m-admin-bind-menu {
  .el-tree-node__content {
    flex-wrap: wrap;
    height: auto;
  }

  &_node {
    position: relative;
  }

  &_buttons {
    position: absolute;
    left: 400px;
    .el-checkbox {
      margin-right: 5px;
    }
  }
}
</style>
