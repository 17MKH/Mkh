<template>
  <m-button :icon="isFullscreen ? 'full-screen-exit' : 'full-screen'" @click="handleClick" />
</template>
<script setup lang="ts">
  import { ref } from 'vue'

  const isFullscreen = ref(false)

  const handleClick = () => {
    isFullscreen.value = !isFullscreen.value

    let el = document.querySelector('.m-admin-dict-extend')
    if (el) {
      if (isFullscreen.value) {
        el.classList.add('fullscreen')
      } else {
        el.classList.remove('fullscreen')
      }
    }
  }
</script>
<style lang="scss">
  .m-admin-dict-extend {
    &.fullscreen {
      position: fixed;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      z-index: 9999;
      .el-textarea,
      .el-textarea__inner {
        height: 100% !important;
      }
    }
  }
</style>
