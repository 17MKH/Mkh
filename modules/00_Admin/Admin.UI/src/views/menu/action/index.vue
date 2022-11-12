<template>
  <m-form-dialog ref="formRef" :model="model" :rules="rules" v-bind="form.props" v-on="form.on">
    <el-row>
      <el-col :span="24">
        <el-form-item :label="t('mod.admin.parent_menu')">
          <el-input :model-value="parent.locales[locale]" disabled />
        </el-form-item>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="12">
        <el-form-item :label="t('mod.admin.menu_group')">
          <el-input :model-value="group.name" disabled />
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item :label="t('mod.admin.menu_type')" prop="type">
          <el-select v-model="model.type" :disabled="mode === 'edit'">
            <el-option :label="t('mod.admin.node')" :value="0"></el-option>
            <el-option :label="t('mod.admin.route')" :value="1"></el-option>
            <el-option :label="t('mod.admin.link')" :value="2"></el-option>
            <el-option :label="t('mod.admin.script')" :value="3"></el-option>
          </el-select>
        </el-form-item>
      </el-col>
    </el-row>
    <el-row v-if="model.type === 1">
      <el-col :span="12">
        <el-form-item :label="t('mod.admin.menu_module')" prop="module">
          <m-admin-module-select v-model="model.module" @change="handleModuleSelectChange" />
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item :label="t('mod.admin.page_route')" prop="routeName">
          <el-select v-model="model.routeName" @change="handleRouteChange">
            <el-option v-for="page in state.pages" :key="page.name" :value="page.name" :label="`${t(`routes.${page.name}`)}(${page.name})`"></el-option>
          </el-select>
        </el-form-item>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="24">
        <el-form-item :label="t('mod.admin.menu_name')" prop="name">
          <el-table :data="localesTable" border style="width: 100%" size="small">
            <el-table-column :label="t('mod.admin.language')" prop="label" align="center" width="180"> </el-table-column>
            <el-table-column :label="t('mkh.name')" align="center">
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
          <el-form-item :label="t('mod.admin.link_url')" prop="url">
            <el-input v-model="model.url" clearable />
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item :label="t('mod.admin.open_target')" prop="openTarget">
            <m-admin-enum-select v-model="model.openTarget" module="admin" name="MenuOpenTarget" />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row v-if="model.openTarget === 2">
        <el-col :span="12">
          <el-form-item :label="t('mod.admin.dialog_width')" prop="dialogWidth">
            <el-input v-model="model.dialogWidth"></el-input>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item :label="t('mod.admin.dialog_height')" prop="dialogHeight">
            <el-input v-model="model.dialogHeight"></el-input>
          </el-form-item>
        </el-col>
      </el-row>
    </template>
    <template v-if="model.type === 3">
      <el-row>
        <el-col :span="24">
          <el-form-item :label="t('mod.admin.custom_script')" prop="customJs">
            <el-input v-model="model.customJs" type="textarea" :rows="5" />
          </el-form-item>
        </el-col>
      </el-row>
    </template>
    <el-row>
      <el-col :span="12">
        <el-form-item :label="t('mkh.is_show')" prop="show">
          <el-switch v-model="model.show"></el-switch>
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item :label="t('mkh.serial_number')" prop="sort">
          <el-input-number v-model="model.sort" controls-position="right"></el-input-number>
        </el-form-item>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="12">
        <el-form-item :label="t('mod.admin.left_icon')" prop="icon">
          <m-icon-picker v-model="model.icon" />
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item :label="t('mod.admin.icon_color')" prop="iconColor">
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
        <el-form-item :label="t('mod.admin.route_query')" prop="routeQuery">
          <el-input v-model="model.routeQuery" :rows="5" type="textarea" />
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item :label="t('mod.admin.route_params')" prop="routeParams">
          <el-input v-model="model.routeParams" :rows="5" type="textarea" />
        </el-form-item>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="24">
        <el-form-item :label="t('mkh.remarks')" prop="remarks">
          <el-input v-model="model.remarks" type="textarea" :rows="5" />
        </el-form-item>
      </el-col>
    </el-row>
    <template #footer>
      <m-button type="success" icon="save" @click="save">{{ t('mkh.save') }}</m-button>
      <m-button type="success" icon="save" @click="saveAndClose">{{ t('mod.admin.save_and_close') }}</m-button>
      <m-button type="info" icon="reset" @click="formRef.reset">{{ t('mkh.reset') }}</m-button>
    </template>
  </m-form-dialog>
</template>
<script setup lang="ts">
  import { computed, reactive, ref } from 'vue'
  import { ActionMode, useAction } from 'mkh-ui'
  import { useI18n } from '@/locales'
  import api from '@/api/menu'

  const { t, locale, messages } = useI18n()

  const props = defineProps<{ id?: string; mode: ActionMode; group: any; parent: any }>()
  const emit = defineEmits()

  const localesTable = [
    { lang: 'zh-cn', label: '中文简体' },
    { lang: 'en', label: 'English' },
  ]

  const model: any = ref({
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

  const IsJsonString = (rule, value, callback) => {
    if (!value) {
      callback()
    } else {
      try {
        JSON.parse(value)
        callback()
      } catch {
        callback(new Error(t('mod.admin.input_standard_json')))
      }
    }
  }

  const rules = computed(() => {
    const baseRules = {
      name: [
        {
          validator: (rule: any, value: any, callback: any) => {
            if (model.value.locales['zh-cn'] === '') {
              callback(new Error(t('mod.admin.input_menu_name')))
            } else {
              callback()
            }
          },
        },
      ],
    }

    switch (model.value.type) {
      case 0:
        return baseRules
      case 1:
        return {
          ...baseRules,
          module: [{ required: true, message: t('mod.admin.select_module') }],
          routeName: [{ required: true, message: t('mod.admin.select_page_route') }],
          routeQuery: [{ validator: IsJsonString, trigger: 'blur' }],
        }
      case 3:
        return { ...baseRules, customJs: [{ required: true, message: t('mod.admin.input_custom_script') }] }
      default:
        return { ...baseRules, url: [{ required: true, message: t('mod.admin.input_link_url') }], openTarget: [{ required: true, message: t('mod.admin.select_open_target') }] }
    }
  })

  const state: any = reactive({ pages: [], currPage: { permissions: [], buttons: [] } })

  const formRef = ref()
  const { form } = useAction({ props, emit, api, model })

  form.props.width = '900px'
  form.props.labelWidth = '150px'
  form.props.btnOk = false
  form.props.btnReset = false
  form.props.closeOnSuccess = false
  form.props.beforeSubmit = () => {
    //提交前设置分组和父级id
    model.value.groupId = props.group.id
    model.value.parentId = props.parent.id

    //路由菜单需要设置权限信息和按钮信息
    if (model.value.type === 1) {
      const { permissions, buttons } = state.currPage
      model.value.permissions = permissions

      if (buttons) {
        model.value.buttons = Object.values(buttons).map((m: any) => {
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
      state.pages = mod.data.pages.filter((m) => !m.noMenu)
      handleRouteChange(model.value.routeName)
    } else {
      state.pages = []
      model.value.routeName = ''
    }
  }

  const handleRouteChange = (routeName) => {
    let page: any = state.pages.find((m: any) => m.name === routeName)
    if (page) {
      model.value.name = page.title
      model.value.icon = page.icon
      state.currPage = page

      for (let key in model.value.locales) {
        model.value.locales[key] = messages.value[key].routes[page.name]
      }
    }
  }

  const save = () => {
    form.props.closeOnSuccess = false
    formRef.value.submit()
  }
  const saveAndClose = () => {
    form.props.closeOnSuccess = true
    formRef.value.submit()
  }
</script>
