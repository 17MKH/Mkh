<template>
  <m-form-dialog :model="model" :rules="rules" v-bind="bind" v-on="on">
    <el-row>
      <el-col :span="24">
        <el-form-item :label="$t('mod.admin.parent_menu')">
          <el-input :model-value="parent.locales[$i18n.locale]" disabled />
        </el-form-item>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="12">
        <el-form-item :label="$t('mod.admin.menu_group')">
          <el-input :model-value="group.name" disabled />
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item :label="$t('mod.admin.menu_type')" prop="type">
          <el-select v-model="model.type" :disabled="mode === 'edit'">
            <el-option :label="$t('mod.admin.node')" :value="0"></el-option>
            <el-option :label="$t('mod.admin.route')" :value="1"></el-option>
            <el-option :label="$t('mod.admin.link')" :value="2"></el-option>
            <el-option :label="$t('mod.admin.script')" :value="3"></el-option>
          </el-select>
        </el-form-item>
      </el-col>
    </el-row>
    <el-row v-if="model.type === 1">
      <el-col :span="12">
        <el-form-item :label="$t('mod.admin.menu_module')" prop="module">
          <m-admin-module-select v-model="model.module" @change="handleModuleSelectChange" />
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item :label="$t('mod.admin.page_route')" prop="routeName">
          <el-select v-model="model.routeName" @change="handleRouteChange">
            <el-option v-for="page in state.pages" :key="page.name" :value="page.name" :label="`${$t(`mkh.routes.${page.name}`)}(${page.name})`"></el-option>
          </el-select>
        </el-form-item>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="24">
        <el-form-item :label="$t('mod.admin.menu_name')" prop="name">
          <el-table :data="localesTable" border style="width: 100%" size="small">
            <el-table-column :label="$t('mod.admin.language')" prop="lang" align="center" width="180"> </el-table-column>
            <el-table-column :label="$t('mkh.name')" align="center">
              <template #default="{ row }">
                <el-input v-model="model.locales[row.lang]"> </el-input>
              </template>
            </el-table-column>
          </el-table>
        </el-form-item>
      </el-col>
    </el-row>
    <template v-if="model.type === 2">
      <el-row>
        <el-col :span="12">
          <el-form-item :label="$t('mod.admin.link_url')" prop="url">
            <el-input v-model="model.url" clearable />
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item :label="$t('mod.admin.open_target')" prop="openTarget">
            <m-admin-enum-select v-model="model.openTarget" module="admin" name="MenuOpenTarget" />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row v-if="model.openTarget === 2">
        <el-col :span="12">
          <el-form-item :label="$t('mod.admin.dialog_width')" prop="dialogWidth">
            <el-input v-model="model.dialogWidth"></el-input>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item :label="$t('mod.admin.dialog_height')" prop="dialogHeight">
            <el-input v-model="model.dialogHeight"></el-input>
          </el-form-item>
        </el-col>
      </el-row>
    </template>
    <template v-if="model.type === 3">
      <el-row>
        <el-col :span="24">
          <el-form-item :label="$t('mod.admin.custom_script')" prop="customJs">
            <el-input v-model="model.customJs" type="textarea" :rows="5" />
          </el-form-item>
        </el-col>
      </el-row>
    </template>
    <el-row>
      <el-col :span="12">
        <el-form-item :label="$t('mkh.is_show')" prop="show">
          <el-switch v-model="model.show"></el-switch>
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item :label="$t('mkh.serial_number')" prop="sort">
          <el-input-number v-model="model.sort" controls-position="right"></el-input-number>
        </el-form-item>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="12">
        <el-form-item :label="$t('mod.admin.left_icon')" prop="icon">
          <m-icon-picker v-model="model.icon" />
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item :label="$t('mod.admin.icon_color')" prop="iconColor">
          <m-flex-row>
            <m-flex-auto>
              <el-input v-model="model.iconColor"> </el-input>
            </m-flex-auto>
            <m-flex-fixed class="m-padding-l-3">
              <el-color-picker v-model="model.iconColor" show-alpha></el-color-picker>
            </m-flex-fixed>
          </m-flex-row>
        </el-form-item>
      </el-col>
    </el-row>
    <el-row v-if="model.type === 1">
      <el-col :span="12">
        <el-form-item :label="$t('mod.admin.route_query')" prop="routeQuery">
          <el-input v-model="model.routeQuery" :rows="5" type="textarea" />
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item :label="$t('mod.admin.route_params')" prop="routeParams">
          <el-input v-model="model.routeParams" :rows="5" type="textarea" />
        </el-form-item>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="24">
        <el-form-item :label="$t('mkh.remarks')" prop="remarks">
          <el-input v-model="model.remarks" type="textarea" :rows="5" />
        </el-form-item>
      </el-col>
    </el-row>
  </m-form-dialog>
</template>
<script>
import { computed, getCurrentInstance, reactive } from 'vue'
import { useSave, withSaveProps } from 'mkh-ui'

export default {
  props: {
    ...withSaveProps,
    group: {
      type: Object,
      required: true,
    },
    parent: {
      type: Object,
      required: true,
    },
  },
  emits: ['success'],
  setup(props, { emit }) {
    const { $t } = mkh
    const cit = getCurrentInstance().proxy
    const api = mkh.api.admin.menu
    const localesTable = [{ lang: 'zh-cn' }, { lang: 'en' }]

    const model = reactive({
      groupId: 0,
      parentId: 0,
      type: 1,
      icon: '',
      iconColor: '',
      module: '',
      routeName: '',
      routeParams: '',
      routeQuery: '',
      url: '',
      openTarget: 0,
      dialogWidth: '800px',
      dialogHeight: '600px',
      customJs: '',
      show: true,
      sort: 0,
      remarks: '',
      permissions: [],
      buttons: [],
      locales: { 'zh-cn': '', en: '' },
    })

    const baseRules = computed(() => {
      return { name: [{ required: true, message: $t('mod.admin.input_menu_name') }] }
    })

    const IsJsonString = (rule, value, callback) => {
      if (!value) {
        callback()
      } else {
        try {
          JSON.parse(value)
          callback()
        } catch {
          callback(new Error($t('mod.admin.input_standard_json')))
        }
      }
    }

    const rules = computed(() => {
      switch (model.type) {
        case 0:
          return baseRules
        case 1:
          return {
            ...baseRules,
            module: [{ required: true, message: $t('mod.admin.select_module') }],
            routeName: [{ required: true, message: $t('mod.admin.select_page_route') }],
            routeQuery: [{ validator: IsJsonString, trigger: 'blur' }],
          }
        case 3:
          return { ...baseRules, customJs: [{ required: true, message: $t('mod.admin.input_custom_script') }] }
        default:
          return { ...baseRules, url: [{ required: true, message: $t('mod.admin.input_link_url') }], openTarget: [{ required: true, message: $t('mod.admin.select_open_target') }] }
      }
    })

    const state = reactive({ pages: [], currPage: null })
    const { bind, on } = useSave({ props, api, model, emit })

    bind.width = '900px'
    bind.labelWidth = '150px'
    bind.closeOnSuccess = false
    bind.beforeSubmit = () => {
      //提交前设置分组和父级id
      model.groupId = props.group.id
      model.parentId = props.parent.id

      //路由菜单需要设置权限信息和按钮信息
      if (model.type === 1) {
        const { permissions, buttons } = state.currPage
        model.permissions = permissions

        if (buttons) {
          model.buttons = Object.values(buttons).map(m => {
            return {
              name: m.text,
              code: m.code,
              icon: m.icon,
              permissions: m.permissions,
            }
          })
        }
      }
    }

    const handleModuleSelectChange = (code, mod) => {
      if (mod) {
        state.pages = mod.data.pages.filter(m => !m.noMenu)
        handleRouteChange(model.routeName)
      } else {
        state.pages = []
        model.routeName = ''
      }
    }

    const handleRouteChange = routeName => {
      let page = state.pages.find(m => m.name === routeName)
      if (page) {
        model.name = page.title
        model.icon = page.icon
        state.currPage = page

        for (let key in model.locales) {
          model.locales[key] = cit.$i18n.messages[key].mkh.routes[page.name]
        }
      }
    }

    return {
      localesTable,
      model,
      rules,
      bind,
      on,
      state,
      handleModuleSelectChange,
      handleRouteChange,
    }
  },
}
</script>
