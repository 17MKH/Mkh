<template>
  <m-form-dialog :model="model" :rules="rules" v-bind="bind" v-on="on" @success="handleSuccess">
    <el-alert v-if="isEdit" class="m-margin-b-20" title="不允许修改用户名和密码" type="warning"> </el-alert>
    <el-row>
      <el-col :span="12">
        <el-form-item label="用户名：" prop="username">
          <el-input ref="nameRef" v-model="model.username" :disabled="isEdit" />
        </el-form-item>
        <el-form-item label="姓名：" prop="name">
          <el-input v-model="model.name" />
        </el-form-item>
        <el-form-item label="手机号：" prop="phone">
          <el-input v-model="model.phone" />
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item label="密码：" prop="password">
          <el-input v-model="model.password" :placeholder="`默认密码：${defaultPassword}`" :disabled="isEdit" />
        </el-form-item>
        <el-form-item label="角色：" prop="roleId">
          <m-select v-model="model.roleId" :action="$mkh.api.admin.role.select" checked-first />
        </el-form-item>
        <el-form-item label="邮箱：" prop="email">
          <el-input v-model="model.email" />
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
    const { store } = mkh
    const api = mkh.api.admin.account
    const model = reactive({ username: '', password: '', name: '', phone: '', email: '', remarks: '' })
    const rules = {
      username: [{ required: true, message: '请输入用户名' }],
      roleId: [{ required: true, message: '请选择角色' }],
      phone: [{ pattern: regex.phone, message: '请输入正确的手机号' }],
      email: [{ type: 'email', message: '请输入正确的邮箱地址' }],
    }

    const isEdit = computed(() => props.mode === 'edit')

    const nameRef = ref(null)
    const { bind, on } = useSave({ title: '账户', props, api, model, emit })
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
