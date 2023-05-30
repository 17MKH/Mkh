import { ModuleDefinition } from 'mkh-ui'

declare module 'mkh-ui' {
  export interface ModuleDefinition {
    /** 主题色 */
    color: string
  }

  export interface Profile {
    /** 角色编号 */
    roleId: string
  }
}
