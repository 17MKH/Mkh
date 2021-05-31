<template>
  <mu-container>
    <h1>{{ $t("mod.admin.title") }}</h1>
    <mu-button type="primary" text="角色管理" icon="plus" to="/admin/role" />
    <mu-button type="warning" text="子路由" to="/admin/account/add" />
    <mu-button type="danger" text="编辑子路由" to="/admin/account/edit" />
    <router-view></router-view>
  </mu-container>
</template>
