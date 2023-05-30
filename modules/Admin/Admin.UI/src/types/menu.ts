/**
 * 菜单树节点
 */
export interface MenuTreeNode {
  id: number
  label: string
  children: MenuTreeNode[]
  path: string[]
  item: {
    id: number
    icon: string
    type: number
    locales: {
      'zh-cn': string
      en: string
    }
  }
}
