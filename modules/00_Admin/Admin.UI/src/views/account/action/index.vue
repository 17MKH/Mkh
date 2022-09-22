<template>
  <m-form-dialog :model="model" :rules="rules" v-bind="form.props" v-on="form.on">
    <el-row>
      <el-col :span="12">
        <el-form-item :label="t('mkh.login.username')" prop="username">
          <el-input ref="nameRef" v-model="model.username" :disabled="isEdit" />
        </el-form-item>
        <el-form-item :label="t('mod.admin.name')" prop="name">
          <el-input v-model="model.name" />
        </el-form-item>
        <el-form-item :label="t('mkh.phone')" prop="phone">
          <el-input v-model="model.phone" />
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item v-if="isAdd" :label="t('mkh.login.password')" prop="password">
          <el-input v-model="model.password" :placeholder="`${t('mod.admin.default_password')}：${defaultPassword}`" />
        </el-form-item>
        <el-form-item :label="t('mkh.role')" prop="roleId">
          <m-admin-role-select v-model="model.roleId" checked-first />
        </el-form-item>
        <el-form-item :label="t('mkh.email')" prop="email">
          <el-input v-model="model.email" />
        </el-form-item>
      </el-col>
    </el-row>
  </m-form-dialog>
</template>
<script setup lang="ts">
  import { computed, reactive, ref } from 'vue'
  import { ActionMode, regex, useAction, useProfileStore } from 'mkh-ui'
  import { useI18n } from '@/locales'
  import api from '@/api/account'

  const { t } = useI18n()

  const profileStore = useProfileStore()
  const props = defineProps<{ id: string | undefined; mode: ActionMode }>()
  const emit = defineEmits(['success'])

  const model = reactive({ id: '', username: '', password: '', name: '', phone: '', email: '', roleId: '' })
  const rules = computed(() => {
    return {
      username: [{ required: true, message: t('mod.admin.input_username') }],
      roleId: [{ required: true, message: t('mod.admin.select_role') }],
      phone: [{ pattern: regex.phone, message: t('mod.admin.input_phone') }],
      email: [{ type: 'email', message: t('mod.admin.input_email') }],
    }
  })

  const nameRef = ref(null)
  const { form, isAdd, isEdit } = useAction({ props, emit, api, model })
  form.props.autoFocusRef = nameRef

  const defaultPassword = ref('')
  api.getDefaultPassword().then((data) => {
    defaultPassword.value = data
  })

  form.on.success = (data) => {
    //如果编辑的是当前登录人的信息，则执行刷新操作
    if (isEdit.value && props.id === profileStore.accountId) {
      profileStore.init().then(() => {
        emit('success', data)
      })
    } else {
      emit('success', data)
    }
  }
</script>
