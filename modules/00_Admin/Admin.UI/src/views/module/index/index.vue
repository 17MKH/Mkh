<template>
  <m-container class="m-admin-module">
    <m-box page icon="component" show-fullscreen>
      <template #title>
        <span>{{ $t(`mkh.routes.${page.name}`) }}</span>
        <span class="m-margin-l-10 m-font-12"
          >{{ $t('mod.admin.module_total_prefix') }}<span class="m-margin-lr-5 m-text-primary m-font-14">{{ modules.length }}</span
          >{{ $t('mod.admin.module_total_suffix') }}</span
        >
      </template>
      <div v-for="mod in modules" :key="mod.code" class="item" @click="openDetail(mod)">
        <div class="item_wrapper">
          <div class="item_bar" :style="{ backgroundColor: mod.color }"></div>
          <div class="item_icon">
            <div :style="{ backgroundColor: mod.color }">
              <m-icon v-if="!mod.icon" name="app"></m-icon>
              <img v-else-if="mod.icon.startsWith('http://') || mod.icon.startsWith('https://')" :src="mod.icon" />
              <m-icon v-else :name="mod.icon"></m-icon>
            </div>
          </div>
          <div class="item_info">
            <div class="item_title">
              <span>{{ mod.id }}_{{ mod.label }}</span>
            </div>
            <div>{{ $t('mkh.code') }}：{{ mod.code }}</div>
            <div>{{ $t('mod.admin.version') }}：{{ mod.version }}</div>
            <div>{{ $t('mod.admin.description') }}：{{ mod.description }}</div>
          </div>
        </div>
      </div>
    </m-box>
    <detail v-model="showDetail" :mod="curr" />
  </m-container>
</template>
<script>
import { ref } from 'vue'
import Detail from '../detail/index.vue'
import page from './page.json'
export default {
  components: { Detail },
  setup() {
    const { modules } = mkh
    const curr = ref({})
    const showDetail = ref(false)
    const colors = ['#409EFF', '#67C23A', '#E6A23C', '#F56C6C', '#6d3cf7', '#0079de']
    modules.forEach(m => {
      m.color = colors[parseInt(Math.random() * colors.length)]
    })

    const openDetail = mod => {
      curr.value = mod
      showDetail.value = true
    }

    return {
      page,
      modules,
      curr,
      showDetail,
      openDetail,
    }
  },
}
</script>
<style lang="scss">
.m-admin-module {
  .item {
    display: inline-block;
    margin-bottom: 10px;
    width: 25%;
    height: 100px;
    cursor: pointer;

    &_wrapper {
      position: relative;
      display: flex;
      flex-direction: row;
      align-items: stretch;
      margin: 0 5px 10px;
      height: 100%;
      border: 1px solid #ebeef5;
      overflow: hidden;
    }

    &_bar {
      position: absolute;
      left: 50%;
      top: 0;
      height: 2px;
      width: 0;
      margin-left: 0;
      transition: all 0.3s ease-in-out;
      z-index: 9999;
    }

    &_icon {
      padding: 19px 15px;
      flex-shrink: 0;

      > div {
        height: 60px;
        width: 60px;
        text-align: center;
        border-radius: 30px;
      }

      img {
        width: 60px;
      }

      .m-icon {
        margin: 10px;
        font-size: 35px;
        color: #fff;
      }
    }

    &_info {
      flex-grow: 1;
      font-size: 12px;
      div {
        margin-bottom: 5px;
        color: #606266;
      }
    }

    &_title {
      margin-top: 10px;
      font-size: 14px;
      font-weight: bold;
      color: #333;
    }

    &:hover {
      .item_wrapper {
        z-index: 999;
        box-shadow: 0 2px 12px 0 rgba(64, 158, 255, 0.5);
      }

      .item_bar {
        width: 100% !important;
        margin-left: -50%;
      }

      .item_icon {
        transform: rotateZ(360deg);
      }
    }
  }
}
</style>
