<template>
  <m-form-dialog v-bind="bind" v-on="on">
    <el-row>
      <el-col :span="24">
        <el-form-item label="父级菜单：">
          <el-input :model-value="parent.path.join('/')" disabled />
        </el-form-item>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="12">
        <el-form-item label="菜单分组：">
          <el-input :model-value="group.name" disabled />
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item label="菜单类型：" prop="type">
          <el-select v-model="model.type" :disabled="mode === 'edit'">
            <el-option label="节点" :value="0"></el-option>
            <el-option label="路由" :value="1"></el-option>
            <el-option label="链接" :value="2"></el-option>
            <el-option label="脚本" :value="3"></el-option>
          </el-select>
        </el-form-item>
      </el-col>
    </el-row>
    <el-row v-if="model.type === 1">
      <el-col :span="12">
        <el-form-item label="所属模块：" prop="module">
          <m-admin-module-select v-model="model.module" @change="handleModuleSelectChange" />
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item label="页面路由：" prop="routeName">
          <el-select v-model="model.routeName" @change="handleRouteChange">
            <el-option v-for="page in state.pages" :key="page.name" :value="page.name" :label="`${page.title}(${page.name})`"></el-option>
          </el-select>
        </el-form-item>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="12">
        <el-form-item label="菜单名称：" prop="name">
          <el-input v-model="model.name" clearable />
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item label="是否显示：" prop="show">
          <el-switch v-model="model.show"></el-switch>
        </el-form-item>
      </el-col>
    </el-row>
    <template v-if="model.type === 2">
      <el-row>
        <el-col :span="12">
          <el-form-item label="链接地址：" prop="url">
            <el-input v-model="model.url" clearable />
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="打开方式：" prop="openTarget">
            <m-admin-enum-select v-model="model.openTarget" module="admin" name="MenuOpenTarget" />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row v-if="model.openTarget === 2">
        <el-col :span="12">
          <el-form-item label="对话框宽度：" prop="dialogWidth">
            <el-input v-model="model.dialogWidth"></el-input>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="对话框高度：" prop="dialogHeight">
            <el-input v-model="model.dialogHeight"></el-input>
          </el-form-item>
        </el-col>
      </el-row>
    </template>
    <template v-if="model.type === 3">
      <el-row>
        <el-col :span="24">
          <el-form-item label="自定义脚本：" prop="customJs">
            <el-input v-model="model.customJs" type="textarea" :rows="5" />
          </el-form-item>
        </el-col>
      </el-row>
    </template>
    <el-row>
      <el-col :span="12">
        <el-form-item label="左侧图标：" prop="icon">
          <m-icon-picker v-model="model.icon" />
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item label="图标颜色：" prop="iconColor">
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
        <el-form-item label="路由参数：" prop="routeQuery">
          <el-input v-model="model.routeQuery" :rows="5" type="textarea" placeholder="Vue路由的query形式，需使用标准JSON格式" />
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item label="路由参数：" prop="routeParams">
          <el-input v-model="model.routeParams" :rows="5" type="textarea" placeholder="Vue路由的params形式，需使用标准JSON格式" />
        </el-form-item>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="24">
        <el-form-item label="备注：" prop="remarks">
          <el-input v-model="model.remarks" type="textarea" :rows="5" />
        </el-form-item>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="12">
        <el-form-item label="序号：" prop="sort">
          <el-input-number v-model="model.sort" controls-position="right"></el-input-number>
        </el-form-item>
      </el-col>
    </el-row>
  </m-form-dialog>
</template>
<script>
import { computed, reactive, ref } from 'vue'
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
  setup(props, { emit }) {
    const api = mkh.api.admin.menu

    const model = reactive({
      groupId: 0,
      parentId: 0,
      type: 1,
      name: '',
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
    })
    const baseRules = { name: [{ required: true, message: '请输入菜单名称' }] }

    const IsJsonString = (rule, value, callback) => {
      if (!value) {
        callback()
      } else {
        try {
          JSON.parse(value)
          callback()
        } catch {
          callback(new Error('请输入标准格式的JSON'))
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
            module: [{ required: true, message: '请选择模块' }],
            routeName: [{ required: true, message: '请选择页面路由' }],
            routeQuery: [{ validator: IsJsonString, trigger: 'blur' }],
          }
        case 3:
          return { ...baseRules, customJs: [{ required: true, message: '请输入自定义脚本' }] }
        default:
          return { ...baseRules, url: [{ required: true, message: '请输入链接地址' }], openTarget: [{ required: true, message: '请选择链接打开方式' }] }
      }
    })

    const state = reactive({ pages: [], currPage: null })

    const { bind, on } = useSave({ title: '菜单', props, api, model, rules, emit })
    bind.width = '900px'
    bind.labelWidth = '130px'
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
        state.pages = mod.data.pages
        handleRouteChange(model.routeName)
      } else {
        state.pages = []
        model.routeName = ''
      }
    }

    const handleRouteChange = routeNmae => {
      let page = state.pages.find(m => m.name === routeNmae)
      model.name = page.title
      model.icon = page.icon
      state.currPage = page
    }

    return {
      model,
      bind,
      on,
      state,
      handleModuleSelectChange,
      handleRouteChange,
    }
  },
}
</script>
