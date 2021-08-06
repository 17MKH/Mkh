<template>
  <m-button :icon="isFullscreen ? 'full-screen-exit' : 'full-screen'" @click="handleClick" />
</template>
<script>
import { ref } from 'vue'
import { dom } from 'mkh-ui'
export default {
  setup() {
    const isFullscreen = ref(false)

    const handleClick = () => {
      isFullscreen.value = !isFullscreen.value

      let el = document.querySelector('.m-admin-dict-extend')
      if (isFullscreen.value) {
        dom.addClass(el, 'fullscreen')
      } else {
        dom.removeClass(el, 'fullscreen')
      }
    }

    return {
      isFullscreen,
      handleClick,
    }
  },
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
