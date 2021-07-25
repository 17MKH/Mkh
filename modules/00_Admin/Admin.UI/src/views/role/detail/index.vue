<template>
  <el-descriptions :column="2" border>
    <el-descriptions-item label="名称">{{ role.name }}</el-descriptions-item>
    <el-descriptions-item label="唯一编码">{{ role.code }}</el-descriptions-item>
    <el-descriptions-item label="绑定菜单组">{{ role.menuGroupName }}</el-descriptions-item>
    <el-descriptions-item label="是否锁定">
      <el-tag v-if="role.locked" type="danger" effect="dark" size="small">锁定</el-tag>
      <el-tag v-else type="info" effect="dark" size="small">未锁定</el-tag>
    </el-descriptions-item>
    <el-descriptions-item label="创建时间">{{ role.createdTime }}</el-descriptions-item>
    <el-descriptions-item label="创建人">{{ role.creator }}</el-descriptions-item>
    <el-descriptions-item label="备注" :span="2">{{ role.remarks }}</el-descriptions-item>
  </el-descriptions>
</template>
<script>
export default {
  props: {
    role: {
      type: Object,
      required: true,
    },
  },
}
</script>
