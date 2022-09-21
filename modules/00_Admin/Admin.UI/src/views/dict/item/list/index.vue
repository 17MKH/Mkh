<template>
  <m-list ref="listRef" class="m-border-none" :header="false" :cols="cols" :query-model="model" :query-method="api.query">
    <template #querybar>
      <el-form-item :label="$t('mkh.name')" prop="name">
        <el-input v-model="model.name" clearable />
      </el-form-item>
      <el-form-item :label="$t('mkh.code')" prop="code">
        <el-input v-model="model.code" clearable />
      </el-form-item>
    </template>
    <template #buttons>
      <m-button-add :code="buttons.itemAdd.code" @click="add" />
    </template>
    <template #expand="{ row }">
      <el-descriptions :column="4" size="small">
        <el-descriptions-item :label="$t('mod.admin.extend_data')" :span="4">{{ row.extend }}</el-descriptions-item>
        <el-descriptions-item :label="$t('mkh.creator')">{{ row.creator }}</el-descriptions-item>
        <el-descriptions-item :label="$t('mkh.created_time')">{{ row.createdTime }}</el-descriptions-item>
        <el-descriptions-item :label="$t('mkh.modifier')">{{ row.modifier }}</el-descriptions-item>
        <el-descriptions-item :label="$t('mkh.modified_time')">{{ row.modifiedTime }}</el-descriptions-item>
      </el-descriptions>
    </template>
    <template #col-icon="{ row }">
      <m-icon v-if="row.icon" :name="row.icon" />
    </template>
    <template #operation="{ row }">
      <m-button-edit :code="buttons.itemEdit.code" @click="edit(row)" @success="handleChange"></m-button-edit>
      <m-button-delete :code="buttons.itemRemove.code" :action="api.remove" :data="row.id" @success="handleChange"></m-button-delete>
    </template>
  </m-list>
  <action v-model="actionProps.visible" :id="id" :parent-id="parentId" :mode="actionProps.mode" @success="handleChange" />
</template>
<script setup lang="ts">
  import { computed, reactive, toRef, watch } from 'vue'
  import { useList } from 'mkh-ui'
  import useStore from '@/store'
  import Action from '../action/index.vue'
  import page from '@/views/dict/index/page'
  import api from '@/api/dictItem'

  const store = useStore()

  const buttons = page.buttons

  const props = defineProps({
    parentId: {
      type: Number,
      default: 0,
    },
  })
  const emit = defineEmits(['change'])

  const parentId = toRef(props, 'parentId')

  const groupCode = computed(() => store.dict.groupCode)
  const dictCode = computed(() => store.dict.dictCode)

  const model = reactive({ groupCode, dictCode, parentId, name: '', code: '', value: '' })
  const cols = [
    { prop: 'id', label: 'mkh.id', width: '55', show: false },
    { prop: 'name', label: 'mkh.name' },
    { prop: 'value', label: 'mkh.value' },
    { prop: 'icon', label: 'mkh.icon' },
    { prop: 'level', label: 'mkh.level' },
    { prop: 'sort', label: 'mkh.sort' },
  ]
  const {
    listRef,
    id,
    actionProps,
    methods: { add, edit, refresh },
  } = useList()

  watch([parentId, groupCode, dictCode], () => {
    console.log(parentId.value)

    console.log(model.parentId)

    refresh()
  })

  const handleChange = () => {
    refresh()
    emit('change')
  }
</script>
