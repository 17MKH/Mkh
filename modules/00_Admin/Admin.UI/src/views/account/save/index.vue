<template>
  <m-form-dialog :model="model" :rules="rules" v-bind="bind" v-on="on" @success="handleSuccess">
    <el-alert v-if="isEdit" class="m-margin-b-20" :title="$t('mod.admin.not_allow_edit_username')" type="warning"> </el-alert>
    <el-row>
      <el-col :span="12">
        <el-form-item :label="$t('mkh.login.username')" prop="username">
          <el-input ref="nameRef" v-model="model.username" :disabled="isEdit" />
        </el-form-item>
        <el-form-item :label="$t('mod.admin.name')" prop="name">
          <el-input v-model="model.name" />
        </el-form-item>
        <el-form-item :label="$t('mkh.phone')" prop="phone">
          <el-input v-model="model.phone" />
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item :label="$t('mkh.login.password')" prop="password">
          <el-input v-model="model.password" :placeholder="`${$t('mod.admin.default_password')}：${defaultPassword}`" :disabled="isEdit" />
        </el-form-item>
        <el-form-item :label="$t('mkh.role')" prop="roleId">
          <m-select v-model="model.roleId" :action="$mkh.api.admin.role.select" checked-first />
        </el-form-item>
        <el-form-item :label="$t('mkh.email')" prop="email">
          <el-input v-model="model.email" />
        </el-form-item>
      </el-col>
    </el-row>
  </m-form-dialog>
</template>
<script>
import { computed, reactive, ref } from 'vue'
import { regex, useSave, withSaveProps } from 'mkh-ui'

export default {
  props: {
    ...withSaveProps,
  },
  emits: ['success'],
  setup(props, { emit }) {
    const { store, $t } = mkh
    const api = mkh.api.admin.account
    const model = reactive({ username: '', password: '', name: '', phone: '', email: '' })
    const rules = computed(() => {
      return {
        username: [{ required: true, message: $t('mod.admin.input_username') }],
        roleId: [{ required: true, message: $t('mod.admin.select_role') }],
        phone: [{ pattern: regex.phone, message: $t('mod.admin.input_phone') }],
        email: [{ type: 'email', message: $t('mod.admin.input_email') }],
      }
    })

    const nameRef = ref(null)
    const { isEdit, bind, on } = useSave({ props, api, model, emit })
    bind.autoFocusRef = nameRef
    bind.width = '700px'

    const defaultPassword = ref('')
    api.getDefaultPassword().then(data => {
      defaultPassword.value = data
    })

    const handleSuccess = () => {
      //如果编辑的是当前登录人的信息，则执行刷新操作
      if (props.mode === 'edit' && props.id === store.state.app.profile.accountId) {
        store.dispatch('app/profile/init', null, { root: true }).then(() => {
          emit('success')
        })
      } else {
        emit('success')
      }
    }

    return {
      model,
      rules,
      isEdit,
      bind,
      on,
      nameRef,
      defaultPassword,
      handleSuccess,
    }
  },
}
</script>
